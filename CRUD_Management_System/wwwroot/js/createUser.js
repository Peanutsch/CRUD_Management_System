document.addEventListener("DOMContentLoaded", function ()
{
    document.getElementById("saveCreateUserButton").addEventListener("click", function (event)
    {
        event.preventDefault();
        createUser();
    });

    function createUser()
    {
        const newUser = {
            Name: document.getElementById("Name").value.trim(),
            Surname: document.getElementById("Surname").value.trim(),
            Email: document.getElementById("Email").value.trim(),
            Alias: document.getElementById("Alias").value.trim()
        };

        console.log(`[DEBUG CreateUser] Name: ${newUser.Name}, Surname: ${newUser.Surname}, Alias: ${newUser.Alias}`);

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
        //name = name.toLowerCase();  // Zet de naam om naar kleine letters
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);

        if (parts.length === 2)
        {
            return decodeURIComponent(parts.pop().split(';').shift());
        }

        console.warn("[createUser.js] getCookie > No cookie found");
        return null;
    }
});
