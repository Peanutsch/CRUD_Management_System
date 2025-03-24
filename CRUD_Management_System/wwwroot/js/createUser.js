function createUser()
{
    const newUser = {
        Name: document.getElementById("Name").value.trim(),
        Surname: document.getElementById("Surname").value.trim(),
        Email: document.getElementById("Email").value.trim(),
        Alias: document.getElementById("Alias").value.trim(),
        Address: document.getElementById("Address").value.trim(),
        ZIP: document.getElementById("ZIP").value.trim(),
        City: document.getElementById("City").value.trim(),
        PhoneNumber: document.getElementById("Phonenumber").value.trim()
    };

    fetch('/CreateUser/CreateUser', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${getCookie('AuthToken')}`
        },
        body: JSON.stringify(newUser),
        credentials: "include"
    })
        .then(response => response.json())
        .then(data =>
        {
            if (data.success)
            {
                alert("User created successfully!");
                window.location.href = "/DashboardAdmin/Index";
            }
            else
            {
                alert(`Error: ${data.message}`);
                console.error("Validation Errors:", data.errors);
            }
        })
        .catch(error => console.error("Error:", error));
}

function getCookie(name)
{
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);

    if (parts.length === 2)
    {
        return decodeURIComponent(parts.pop().split(';').shift());
    }

    console.warn("[createUser.js] getCookie > No cookie found");
    return null;
}

// Zorg ervoor dat deze functie ook bereikbaar is vanuit andere bestanden
window.createUser = createUser;