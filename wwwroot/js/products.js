const uri = '/SentenceControler';
let sentences = [];
let token;


function addItem() {
    const addcontentTextbox = document.getElementById('add-content');
    const addsubjectTextbox = document.getElementById('add-subject');

    const item = {
        id: -1,
        content: addcontentTextbox.value.trim(),
        subject: addsubjectTextbox.value.trim()
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
            printSen(token);
            addsubjectTextbox.value = '';
            addcontentTextbox.value = '';
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
        .then(() => printSen(token))
        .catch(error => console.error('Unable to delete item.', error));
}
function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}
function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        content: document.getElementById('edit-content').value.trim(),
        subject: document.getElementById('edit-subject').value.trim()
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
        .then(() => printSen(token))
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}
function displayEditForm(id) {
    const item = sentences.find(item => item.id === id);

    document.getElementById('edit-content').value = item.content;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-subject').value = item.subject;
    document.getElementById('editForm').style.display = 'block';
}
const start = () => {
    token = localStorage.getItem('token');
    if (!token) {
        throw new Error("unotorize");
    }
    const typeUser = getUserTypeFromToken(token);
    if (typeUser == "Admin")
        document.getElementById("manegerUsers").style.display = "inline";
    printSen(token);
}
const printSen = async (token) => {
    await getItems(token).then((data) => {
        const tBody = document.getElementById('sentence');
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
            let textNode = document.createTextNode(item.content);
            td2.appendChild(textNode);

            let td5 = tr.insertCell(2);
            let textNodeSub = document.createTextNode(item.subject);
            td5.appendChild(textNodeSub);

            let td3 = tr.insertCell(3);
            td3.appendChild(editButton);

            let td4 = tr.insertCell(4);
            td4.appendChild(deleteButton);
        })
        sentences = data;
    }
    )
};
// פונקציה לחילוץ סוג המשתמש מה-token (JWT)
function getUserTypeFromToken(token) {
    const payload = token.split('.')[1];
    const decodedPayload = JSON.parse(atob(payload)); // פענוח ה-payload
    return decodedPayload.type; // החזרת סוג המשתמש (מנהל או רגיל)
}
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
const manegerUsers = () => {
    window.location.href = "users.html";
}

start();