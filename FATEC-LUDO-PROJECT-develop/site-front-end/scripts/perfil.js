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

function applyData(tier, played_rounds, victories, ludo_coins) {
    var tier_output = document.getElementById('tier_output');
    tier_output.textContent = "Tier: "+ tier;

    var plays_output = document.getElementById('plays_output');
    plays_output.textContent = "Partidas jogadas: "+ played_rounds;

    var victories_output = document.getElementById('victories_output');
    victories_output.textContent = "Vitórias: "+ victories;

    var ludocoins_output = document.getElementById('ludocoins_output');
    ludocoins_output.textContent = "Ludocoins: "+ludo_coins;
}

function applyForm(username, email, password) {
    var user_name_input = document.getElementById('user_name_input');
    user_name_input.value = username;

    var user_email_input = document.getElementById('user_email_input');
    user_email_input.value = email;

    var user_password_input = document.getElementById('user_password_input');
    user_password_input.value = password;
}

function applyUserImage(base64) {

}

function openForEdit() {
    var user_name_input = document.getElementById('user_name_input');
    user_name_input.disabled = false;

    var user_email_input = document.getElementById('user_email_input');
    user_email_input.disabled = false;

    var user_password_input = document.getElementById('user_password_input');
    user_password_input.disabled = false;
    user_password_input.value = "";

    var save_button = document.getElementById('save_btn');
    save_button.disabled  = false;

    var edit_button = document.getElementById('edit_btn');
    edit_button.disabled  = true;
}

function closeForSave() {
    var user_name_input = document.getElementById('user_name_input');
    user_name_input.disabled = true;

    var user_email_input = document.getElementById('user_email_input');
    user_email_input.disabled = true;

    var user_password_input = document.getElementById('user_password_input');
    user_password_input.disabled = true;

    var save_button = document.getElementById('save_btn');
    save_button.disabled  = true;

    var edit_button = document.getElementById('edit_btn');
    edit_button.disabled  = false;
}

async function main() {
    var userData = JSON.parse(getCookie("loginToken"));
    console.log(userData);
    var user_id = userData.id;

    var user_tier = 0;
    var user_played_rounds = 0;
    var user_victories = 0;
    var user_coins = 0;

    user_tier = userData.user_level;
    user_played_rounds = userData.loses + userData.wins;
    user_victories = userData.wins;
    user_coins = await getUserCoins(user_id);

    applyData(user_tier, user_played_rounds, user_victories, user_coins);

    var user_name = "";
    var user_email = "";
    var user_password = "";

    user_name = userData.username;
    user_email = userData.email;
    user_password = userData.password;

    applyForm(user_name, user_email, user_password); 
}

main();