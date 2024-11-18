document.getElementById('login-form').addEventListener('submit', function(event) {
    event.preventDefault();

    const form = event.target;
    const name = form.elements['name'].value;
    const password = form.elements['password'].value;
    const email = form.elements['email'].value;

    if (name.length && password.length && email.length) {
        console.log("Senha: " + password);
        console.log("Nome: " + name);
        console.log("Email: " + email);

        const userData = {
            "username": name,
            "email": email,
            "password": password
        };

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(userData)
        };

        fetch('http://localhost:5005/user/subscribe', requestOptions)
            .then(response => {
                if (response.status === 400) {
                    throw new Error('Dados inválidos');
                } else {
                    return response.json();
                }
            })
            .then(data => {
                console.log('Usuário cadastrado:', data);
                window.location.href = "menu.html"; // Redireciona para a página de login
            })
            .catch(error => console.error('Erro:', error));
    } else {
        console.error('Por favor, preencha todos os campos.');
    }
});
