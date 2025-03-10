document.addEventListener("DOMContentLoaded", function ()
{
    addAuthHeaderToRequest('/DashboardAdmin/Index'); // JWT-token toevoegen na paginalaad
});

// Functie om de JWT-token toe te voegen aan de header van een aanvraag
function addAuthHeaderToRequest(url)
{
    const token = sessionStorage.getItem('jwtToken');

    if (token)
    {
        fetch(url, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Accept': 'application/json'
            }
        })
            .then(response =>
            {
                if (!response.ok)
                {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data =>
            {
                console.log("Data ontvangen:", data);
            })
            .catch(error =>
            {
                console.error("Request mislukt:", error);
            });
    } else
    {
        console.warn("Geen JWT-token gevonden in sessionStorage.");
    }
}
