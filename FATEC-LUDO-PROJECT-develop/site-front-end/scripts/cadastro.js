document.getElementById('my-form').addEventListener('submit', function(event) {
    event.preventDefault();
    const form = event.target;
    var name = form.elements['name'].value;
    var password = form.elements['password'].value;
    var email = form.elements['email'].value; // Campo adicional para o e-mail


    if(name.length && password.length && email.length){
        console.log("senha: " + password);
        console.log("nome: " + name);
        console.log("email: " + email);

        const object = {
            "username": name,
            "email": email, 
            "password": password
        };

        const requestOptions = {
            method: 'POST',
            headers: { 
                'Content-Type': 'application/json' 
            },
            body: JSON.stringify(object)
        };

   
        fetch('http://localhost:5005/user/subscribe', requestOptions) 
            .then(response => {
                
                if (response.status === 400) {
                    // Resposta 400 Bad Request
                    throw new Error('Dados inválidos');
                }
                else if (response.status === 409) {
                    var elemento = document.getElementById('conflict-message');
                    elemento.style.display = 'block';
                    throw new Error('Conflito: Usuário/Email já existente');
                }
            })
            .then(data => {
                console.log('Usuário cadastrado:', data);
                window.location.href = "login.html"; 
            })
            .catch(error => console.error('Erro:', error));
    } else {
        console.error('Por favor, preencha todos os campos.');
    }
});
