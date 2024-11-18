async function getUserCosmetics(user_id){
    object = {
        "user_id": user_id,
        "page": 0,
        "items_per_page": 1000
    }

    const requestOptions = {
        method: 'POST',
        headers: { 
        'Content-Type': 'application/json' 
        },
        body: JSON.stringify(object)
    };

    var items_list = [];

    //post
    await fetch('http://localhost:5005/inventory/list', requestOptions)
    .then(response => {
        if (response.ok) {
            // Resposta 200 OK
            return response.json();
        } else if (response.status === 401) {
            // Resposta 401 Unauthorized
            var elemento = document.getElementById('error-message');
            elemento.style.display = 'block';
            throw new Error('Não autorizado');
        } else {
            // Outros tipos de resposta
            throw new Error('Erro na requisição');
        }
    })
    .then(data => {
        data.forEach(element => {
            items_list.push(element.id)
        });
        return items_list;
    })
    .catch(error => {
        console.error('Erro:', error);
    });

    return items_list;
}

async function getUserCoins(user_id){
    var user_coins = 0;
    //get
    await fetch('http://localhost:5005/user/user_coins?user_id='+user_id)
    .then(response => {
        if (response.ok) {
            // Resposta 200 OK
            return response.json();
        } else if (response.status === 401) {
            // Resposta 401 Unauthorized
            var elemento = document.getElementById('error-message');
            elemento.style.display = 'block';
            throw new Error('Não autorizado');
        } else {
            // Outros tipos de resposta
            throw new Error('Erro na requisição');
        }
    })
    .then(data => {
        user_coins = data;
    })
    .catch(error => {
        console.error('Erro:', error);
    });
    return user_coins;
}

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

function applyCoins(user_coins) {
    var paragrafo = document.getElementById('user-coins');
    paragrafo.textContent = user_coins + " Coins";
}


async function main() {
    var userData = JSON.parse(getCookie("loginToken"));
    console.log(userData);
    var user_id = userData.id;

    // loadCosmetics();

    // var user_coins = userData.ludo_coins;
    var user_coins = await getUserCoins(user_id);
    applyCoins(user_coins)

    var user_owned_cosmetics = await getUserCosmetics(user_id);
    // checkForOwned();

    console.log(user_owned_cosmetics);
    console.log(user_coins);
    
    
}

main();