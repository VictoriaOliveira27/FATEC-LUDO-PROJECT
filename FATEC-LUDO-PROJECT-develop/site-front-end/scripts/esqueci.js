document.getElementById('forgot-password-form').addEventListener('submit', function(event) {
    event.preventDefault();
    const form = event.target;
    var email = form.elements['email'].value;

    // Verifica se o campo de e-mail está preenchido
    if(email.length){
        console.log("E-mail a senha:" + email);
        const requestBody = {
            "email": email
        };

        const requestOptions = {
            method: 'POST',
            headers: { 
                'Content-Type': 'application/json' 
            },
            body: JSON.stringify(requestBody)
        };

        // Envia requisição para o endpoint de recuperação de senha
        fetch('http://localhost:5005/user/forgot-password', requestOptions)
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    throw new Error('Erro na requisição');
                }
            })
            .then(data => {
                console.log("Resposta do servidor:", data);
                alert("Se o e-mail estiver cadastrado, você receberá um link para redefinir a senha.");
            })
            .catch(error => console.error('Erro:', error));
    } else {
        alert("Por favor, insira um e-mail válido.");
    }
});
