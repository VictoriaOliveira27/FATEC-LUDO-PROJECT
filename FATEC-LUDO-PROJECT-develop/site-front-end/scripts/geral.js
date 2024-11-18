// scripts/goBack.js
function goBack() {
    window.history.back();
}

//checar se o usuario ta logado
function getCookie(name) {
    let cookieArr = document.cookie.split(";");
    for(let i = 0; i < cookieArr.length; i++) {
        let cookiePair = cookieArr[i].split("=");
        if(name === cookiePair[0].trim()) {
            return decodeURIComponent(cookiePair[1]);
        }
    }
    return null;
}

function isCookieExpired(cookieName) {
    let cookieValue = getCookie(cookieName);
    if (cookieValue) {
        let cookieParts = cookieValue.split("|");
        let expirationDate = new Date(cookieParts[1]);
        return expirationDate < new Date();
    }
    return true;
}

if (getCookie("loginToken") && !isCookieExpired("loginToken")) {
    console.log("usuario ta logado");
} else {
    console.log("usuario nao ta logado, rediricionando a pagina de login...");
    window.location.href = "login.html";
}
//checar se o usuario ta logado
