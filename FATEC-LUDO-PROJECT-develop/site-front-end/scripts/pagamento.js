document.getElementById('payment-form').addEventListener('submit', function(event) {
    event.preventDefault();
    const form = event.target;
    
    var paymentMethod = form.elements['payment-method'].value;
    var cardNumber = form.elements['card-number'].value;
    var cardName = form.elements['card-name'].value;
    var expiration = form.elements['expiration'].value;
    var cvv = form.elements['cvv'].value;
    var value = form.elements['value'].value;

    // Verifica se todos os campos estão preenchidos
    if(cardNumber.length && cardName.length && expiration.length && cvv.length && value.length) {
        console.log("Método de pagamento: " + paymentMethod);
        console.log("Número do cartão: " + cardNumber);
        console.log("Nome impresso no cartão: " + cardName);
        console.log("Vencimento: " + expiration);
        console.log("CVV: " + cvv);
        console.log("Valor: R$" + value);

        const paymentData = {
            "paymentMethod": paymentMethod,
            "cardNumber": cardNumber,
            "cardName": cardName,
            "expiration": expiration,
            "cvv": cvv,
            "value": value
        };

        const requestOptions = {
            method: 'POST',
            headers: { 
                'Content-Type': 'application/json' 
            },
            body: JSON.stringify(paymentData)
        };

        // Enviar os dados para o servidor
        fetch('http://localhost:5005/payment', requestOptions)
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    throw new Error('Erro na requisição');
                }
            })
            .then(data => {
                console.log('Resposta do servidor:', data);
                alert("Pagamento realizado com sucesso!");
            })
            .catch(error => console.error('Erro:', error));
    } else {
        alert("Por favor, preencha todos os campos.");
    }

    function openModal() {
        document.getElementById("confirmation-modal").style.display = "block";
    }

    function closeModal() {
        document.getElementById("confirmation-modal").style.display = "none";
    }

    function confirmPayment() {
        alert("Pagamento confirmado com sucesso!");
        closeModal();
    }

});
