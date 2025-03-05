const uri = '/User/Login';
let token;
const formLoginUser = document.getElementById('loginUser');
const formLoginManeger = document.getElementById('loginManeger');


const start = () => {
    token = localStorage.getItem('token');
    if (token && isTokenValid(token)) {
        printSetences(token);
    } else {
        token = getTokenFromUrl();
        if (token) {
            const newT = authenticateWithServer(token);
            localStorage.setItem('token', token);
            printSetences(newT);
        } else {
            login();
        }
    }
}

function isTokenValid(token) {
    // פיצול הטוקן לשלושה חלקים
    const parts = token.split('.');
    if (parts.length !== 3) {
        return false; // הטוקן לא תקין
    }

    // דקוד את ה-payload
    const payload = JSON.parse(atob(parts[1]));

    // קבלת תאריך התפוגה
    const exp = payload.exp; // exp הוא תאריך התפוגה ב-epoch time

    // השוואת תאריך התפוגה עם התאריך הנוכחי
    const currentTime = Math.floor(Date.now() / 1000); // זמן נוכחי ב-epoch time

    return exp > currentTime; // מחזיר true אם הטוקן עדיין בתוקף
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
    window.location.href = "products.html"
}
const loginUser = (e) => {
    e.preventDefault();
    var requestOptions = {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            "id": e.target['id'].value,
            "name": "string",
            "password": e.target['password'].value,
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
                throw new Error()
            }
        })
        .then(data => {
            localStorage.setItem('token', data);
            printSetences(data);
        }).catch(e => login())
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
                throw new Error();
            }
        })
        .then(data => {
            localStorage.setItem('token', data);
            printSetences();
        }).catch(e => login())
}




///////////////////////////
// הגדר את ה-client ID שלך
const CLIENT_ID = '839656836741-7d1vpaoe5r6spfptvld0guudbpjr7prt.apps.googleusercontent.com';

// הגדר את ה-redirect URI שלך
const REDIRECT_URI = 'https://localhost:5001';

// הגדר את ההרשאות שאתה זקוק להן
const SCOPES = 'https://www.googleapis.com/auth/userinfo.profile';

// פונקציה להתחברות
function handleAuthClick() {
    const authUrl = `https://accounts.google.com/o/oauth2/v2/auth?client_id=${CLIENT_ID}&redirect_uri=${REDIRECT_URI}&response_type=token&scope=${SCOPES}`;
    window.location.href = authUrl;
}

// פונקציה לקבלת הטוקן מה-URL
function getTokenFromUrl() {
    const hash = window.location.hash;
    if (hash) {
        const params = new URLSearchParams(hash.substring(1));
        return params.get('access_token');
    }
    return null;
}

// פונקציה לקבלת מידע על המשתמש
function getUserInfo(token) {
    fetch('https://www.googleapis.com/oauth2/v3/userinfo', {
        headers: {
            Authorization: `Bearer ${token}`
        }
    })
        .then(response => response.json())
        .then(data => {
            document.getElementById('user-info').innerText = `שלום, ${data.name}`;
            document.getElementById('user-info').style.display = 'block';
        })
        .catch(error => console.error('Error fetching user info:', error));
}
async function authenticateWithServer(googleToken) {
    try {
        const response = await fetch('/api/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ idToken: googleToken }),
        });

        if (!response.ok) {
            throw new Error('Failed to authenticate');
        }

        const data = await response.json();
        return data; // החזרת הטוקן החדש מהשרת
    } catch (error) {
        console.error('Error during authentication:', error);
        throw error; // אפשר לזרוק שגיאה או לטפל בה בהתאם לצורך
    }
}

document.getElementById('login-button').addEventListener('click', handleAuthClick);
/////////////////////////////////////////////////////
start();