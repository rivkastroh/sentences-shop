const uri = '/User/Login';
let token;
const formLoginUser = document.getElementById('loginUser');
const formLoginManeger = document.getElementById('loginManeger');


const start = () => {
    token = localStorage.getItem('token');
    if (token) {
        printSetences(token);
    } else {
        login();
    }
}

//מציגה כפתורים לבחירת סוג כניסה ומסדרת את פונקצית 
// onclick עבור כל סוג לוגין
const login = () => {
    const IManeger = document.getElementById('IManeger');
    const IUser = document.getElementById('IUser');
    IManeger.style.display = 'inline-block';
    IUser.style.display = 'inline-block';

    IUser.onclick = () => {
        IManeger.style.display = 'none';
        IUser.style.display = 'none';
        formLoginUser.style.display = 'inline-block';
        formLoginUser.onsubmit = (e) => {
            e.preventDefault();
            loginUser(e);
        }
    }
    IManeger.onclick = () => {
        IManeger.style.display = 'none';
        IUser.style.display = 'none';
        formLoginManeger.style.display = 'inline-block';
        formLoginManeger.onsubmit = (e) => {
            e.preventDefault();
            loginManeger(e);
        }
    }
}
const printSetences = (token) => {
    alert("ברוך הבא!")
    const typeUser = getUserTypeFromToken(token);
    if (typeUser == "User") {

    }
    else if (typeUser == "Admin") {

    }
    else {
        alert("not fount type user");
        login();
    }
}
const loginUser = (e) => {
    e.preventDefault();
    var requestOptions = {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            "id": e.target['id'].value,
            "name": "string",
            "pasword": e.target['password'].value,
            "setenceIds": [
                0
            ]
        }),
        redirect: "follow",
    };
    fetch(uri, requestOptions)
        .then(response => {
            formLoginUser.style.display = 'none';
            if (response.status === 200)
                return response.text();
            else {
                alert("Unauthorized");
                login();
            }
        })
        .then(data => {
            alert(data);
            localStorage.setItem('token', data);
            printSetences(data);
        })
}
const loginManeger = (e) => {
    e.preventDefault();
    var requestOptions = {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ password: e.target['password'].value }),
        redirect: "follow",
    };
    fetch('/Maneger/Login', requestOptions)
        .then(response => {
            formLoginManeger.style.display = 'none';
            if (response.status === 200)
                return response.text();
            else {
                alert("Unauthorized");
                login();
            }
        })
        .then(data => {
            localStorage.setItem('token', data.token);
            printSetences();
        })
}

// פונקציה לחילוץ סוג המשתמש מה-token (JWT)
function getUserTypeFromToken(token) {
    const payload = token.split('.')[1];
    const decodedPayload = JSON.parse(atob(payload)); // פענוח ה-payload
    alert(decodedPayload.type);
    return decodedPayload.type; // החזרת סוג המשתמש (מנהל או רגיל)
}


start();









// const uri = '/pizza';
// let pizzas = [];

// function getItems() {
//     fetch(uri)
//         .then(response => response.json())
//         .then(data => _displayItems(data))
//         .catch(error => console.error('Unable to get items.', error));
// }

// function addItem() {
//     const addNameTextbox = document.getElementById('add-name');

//     const item = {
//         isGlutenFree: false,
//         name: addNameTextbox.value.trim()
//     };

//     fetch(uri, {
//             method: 'POST',
//             headers: {
//                 'Accept': 'application/json',
//                 'Content-Type': 'application/json'
//             },
//             body: JSON.stringify(item)
//         })
//         .then(response => response.json())
//         .then(() => {
//             getItems();
//             addNameTextbox.value = '';
//         })
//         .catch(error => console.error('Unable to add item.', error));
// }

// function deleteItem(id) {
//     fetch(`${uri}/${id}`, {
//             method: 'DELETE'
//         })
//         .then(() => getItems())
//         .catch(error => console.error('Unable to delete item.', error));
// }

// function displayEditForm(id) {
//     const item = pizzas.find(item => item.id === id);

//     document.getElementById('edit-name').value = item.name;
//     document.getElementById('edit-id').value = item.id;
//     document.getElementById('edit-isGlutenFree').checked = item.isGlutenFree;
//     document.getElementById('editForm').style.display = 'block';
// }

// function updateItem() {
//     const itemId = document.getElementById('edit-id').value;
//     const item = {
//         id: parseInt(itemId, 10),
//         isGlutenFree: document.getElementById('edit-isGlutenFree').checked,
//         name: document.getElementById('edit-name').value.trim()
//     };

//     fetch(`${uri}/${itemId}`, {
//             method: 'PUT',
//             headers: {
//                 'Accept': 'application/json',
//                 'Content-Type': 'application/json'
//             },
//             body: JSON.stringify(item)
//         })
//         .then(() => getItems())
//         .catch(error => console.error('Unable to update item.', error));

//     closeInput();

//     return false;
// }

// function closeInput() {
//     document.getElementById('editForm').style.display = 'none';
// }

// function _displayCount(itemCount) {
//     const name = (itemCount === 1) ? 'pizza' : 'pizza kinds';

//     document.getElementById('counter').innerText = `${itemCount} ${name}`;
// }

// function _displayItems(data) {
//     const tBody = document.getElementById('pizzas');
//     tBody.innerHTML = '';

//     _displayCount(data.length);

//     const button = document.createElement('button');

//     data.forEach(item => {
//         let isGlutenFreeCheckbox = document.createElement('input');
//         isGlutenFreeCheckbox.type = 'checkbox';
//         isGlutenFreeCheckbox.disabled = true;
//         isGlutenFreeCheckbox.checked = item.isGlutenFree;

//         let editButton = button.cloneNode(false);
//         editButton.innerText = 'Edit';
//         editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

//         let deleteButton = button.cloneNode(false);
//         deleteButton.innerText = 'Delete';
//         deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

//         let tr = tBody.insertRow();

//         let td1 = tr.insertCell(0);
//         td1.appendChild(isGlutenFreeCheckbox);

//         let td2 = tr.insertCell(1);
//         let textNode = document.createTextNode(item.name);
//         td2.appendChild(textNode);

//         let td3 = tr.insertCell(2);
//         td3.appendChild(editButton);

//         let td4 = tr.insertCell(3);
//         td4.appendChild(deleteButton);
//     });

//     computers = data;
// }