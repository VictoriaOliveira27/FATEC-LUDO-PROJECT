document.getElementById('my-form').addEventListener('submit', function(event) {
    event.preventDefault();
    const form = event.target;
    var name = form.elements['email'].value;
    var password = form.elements['password'].value;

    //se todos os campos tiverem preenchidos, proceder no login
    if(name.length && password.length){
        console.log("senha:"+password);
        console.log("nome:"+name);
        object = {
            "username": name,
            "password": password
        }

        const requestOptions = {
            method: 'POST',
            headers: { 
            'Content-Type': 'application/json' 
            },
            body: JSON.stringify(object)
        };

        //post
        fetch('http://localhost:5005/user/login', requestOptions)
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
                console.log(data)
                setCookie('loginToken', JSON.stringify(data), 1); // Armazena por 1 dia
                console.log('Resposta armazenada no cookie:', data);
                window.location.href = "index.html";
            })
            .catch(error => console.error('Erro:', error));
    }

    function setCookie(name, value, days) {
        const date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        const expires = "expires=" + date.toUTCString();
        document.cookie = name + "=" + value + ";" + expires + ";path=/";
    }
    
});
