const manegerProd = () => {
    window.location.href = "products.html";
}
const uri = '/User';
let users = [];
let token;

const start = () => {
    tokenL = localStorage.getItem('token');
    if (!tokenL) {
        throw new Error("unotorize");
    }
    const typeUser = getUserTypeFromToken(tokenL);
    if (typeUser != "Admin")
        throw new Error("unotorize");
    token = tokenL;
    printUsers();
}
const printUsers = async () => {
    await getItems(token).then((data) => {
        const tBody = document.getElementById('users');
        tBody.innerHTML = '';
        const button = document.createElement('button');

        data.forEach(item => {
            let editButton = button.cloneNode(false);
            editButton.innerText = 'עריכה';
            editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

            let deleteButton = button.cloneNode(false);
            deleteButton.innerText = 'מחיקה';
            deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

            let tr = tBody.insertRow();

            let td1 = tr.insertCell(0);
            let textNodeId = document.createTextNode(item.id);
            td1.appendChild(textNodeId);

            let td2 = tr.insertCell(1);
            let textNode = document.createTextNode(item.name);
            td2.appendChild(textNode);

            let td5 = tr.insertCell(2);
            let textNodeSub = document.createTextNode(item.password);
            td5.appendChild(textNodeSub);

            let td6 = tr.insertCell(3);
            let textNodePass = document.createTextNode(item.setenceIds.toString());
            td6.appendChild(textNodePass);

            let td3 = tr.insertCell(4);
            td3.appendChild(editButton);

            let td4 = tr.insertCell(5);
            td4.appendChild(deleteButton);
        })
        users = data;
    }
    )
};
// פונקציה להחזרת פריטים מהממשק
function getItems(token) {
    return fetch(uri, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Authorization': `Bearer ${token}`  // שליחת ה-token ב-headers
        }
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to fetch items');
            }
            return response.json();
        })
        .then(data => {
            return data;
        })
        .catch(error => console.error('Unable to get items.', error));
}
function addItem() {
    const addidTextbox = document.getElementById('add-id');
    const addnameTextbox = document.getElementById('add-name');
    const addpassTextbox = document.getElementById('add-password');

    const item = {
        id: addidTextbox.value.trim(),
        name: addnameTextbox.value.trim(),
        password: addpassTextbox.value.trim(),
        setenceIds: []
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            printUsers();
            addpassTextbox.value = '';
            addnameTextbox.value = '';
            addidTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}
function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${token}`
        },
    })
        .then(() => printUsers(token))
        .catch(error => console.error('Unable to delete item.', error));
}
function displayEditForm(id) {
    const item = users.find(item => item.id === id);

    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-password').value = item.password;
    document.getElementById('edit-setenceIds').value = item.setenceIds.toString();
    document.getElementById('editForm').style.display = 'block';
}
function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}
function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        name: document.getElementById('edit-name').value.trim(),
        password: document.getElementById('edit-password').value.trim(),
        setenceIds: document.getElementById('edit-setenceIds').value.split(',').map(Number)
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(item)
    })
        .then(() => printUsers(token))
        .catch(error => console.error('Unable to update item.', error));

    closeInput();
}


// פונקציה לחילוץ סוג המשתמש מה-token (JWT)
function getUserTypeFromToken(token) {
    const payload = token.split('.')[1];
    const decodedPayload = JSON.parse(atob(payload)); // פענוח ה-payload
    return decodedPayload.type; // החזרת סוג המשתמש (מנהל או רגיל)
}

start();
