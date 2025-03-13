document.addEventListener("DOMContentLoaded", function ()
{

    // Functie om de JWT-token op te slaan in een cookie na een succesvolle login
    function handleLogin(response)
    {
        if (response.token)
        {
            // Token opslaan in een HttpOnly cookie. Cookie blijft 24 uur lang geldig (86400000 ms)
            document.cookie = "AuthToken=" + response.token +
                              "; Secure; HttpOnly; SameSite=Strict; expires=" +
                              new Date(Date.now() + 86400000).toUTCString() +
                              "; path=/";

        }
        else
        {
            console.warn("[auth.js]\n handleLogin > No token received at login...");
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
            body: JSON.stringify({ AliasId: alias, Password: password }),
            credentials: "include" // Zorg ervoor dat cookies meegestuurd worden bij verzoeken
        })
            .then(response =>
            {
                if (!response.ok)
                {
                    throw new Error("[auth.js]\n login() Response > Login request failed");
                }
                return response.json();
            })
            .then(responseData =>
            {
                if (responseData.token)
                {
                    handleLogin(responseData);                      // Token wordt nu opgeslagen in een cookie
                    window.location.href = '/DashboardAdmin/Index'; // Login succes: ga naar /DashboardAdmin/Index
                } else
                {
                    alert("[auth.js]\n ResponseData > Invalid login credentials");
                }
            })
            .catch(error =>
            {
                console.error("\n[auth.js]\n login()", error);
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
        console.error("Login-knop niet gevonden...");
    }
});
