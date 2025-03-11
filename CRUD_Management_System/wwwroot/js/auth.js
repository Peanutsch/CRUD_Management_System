document.addEventListener("DOMContentLoaded", function ()
{

    // Functie om de JWT-token op te slaan na een succesvolle login
    function handleLogin(response)
    {
        if (response.token)
        {
            sessionStorage.setItem('jwtToken', response.token); // Token opslaan
        } else
        {
            console.warn("Geen token ontvangen bij login.");
        }
    }

    // Login functie aanroepen
    function login()
    {
        const alias = document.getElementById("alias").value;
        const password = document.getElementById("password").value;

        fetch('/Login/Login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ AliasId: alias, Password: password })
        })
            .then(response =>
            {
                if (!response.ok)
                {
                    throw new Error("Login request failed");
                }
                return response.json();
            })
            .then(responseData =>
            {
                if (responseData.token)
                {
                    console.log("Token Received");

                    handleLogin(responseData);
                    window.location.href = '/DashboardAdmin/Index'; // Login succes: Go to /DashboardAdmin/Index
                } else
                {
                    alert("Invalid login credentials");
                }
            })
            .catch(error =>
            {
                console.error("Login failed", error);
                alert("Login failed");
            });
    }

    // Event listener toevoegen (voorkom dubbele listeners)
    const loginButton = document.querySelector("button[type='submit']");
    if (loginButton)
    {
        loginButton.addEventListener("click", function (event)
        {
            event.preventDefault();
            login();
        });
    } else
    {
        console.error("Login-knop niet gevonden.");
    }
});
