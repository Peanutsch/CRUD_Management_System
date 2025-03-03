// Event Listener voor aan alle knoppen met de klasse 'delete-btn' bij het laden van de pagina
document.addEventListener('DOMContentLoaded', function ()
{
    // Laad de gebruikersgegevens bij het laden van de pagina
    refreshUserTable();

    // Event Listener voor alle "Delete" knoppen
    document.querySelectorAll('.delete-btn').forEach(button =>
    {
        button.addEventListener('click', function ()
        {
            const alias = this.getAttribute('data-alias');
            deleteUser(this, alias);
        });
    });
});

function deleteUser(button, alias)
{
    if (!confirm(`Weet je zeker dat je gebruiker ${alias} wilt verwijderen?`))
    {
        return; // Stop als de gebruiker annuleren kiest
    }

    const row = button.closest('tr');

    fetch(`/User/DeleteUser`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify({ Alias: alias })
    })
        .then(response => response.json())
        .then(result =>
        {
            if (result.success)
            {
                row.remove(); // Verwijder de rij uit de tabel
                refreshUserTable(); // Ververs de tabel na verwijdering
            } else
            {
                alert("Fout bij verwijderen van gebruiker: " + result.message);
            }
        })
        .catch(error => console.error('Fout bij verwijderen:', error));
}

function refreshUserTable()
{
    fetch('/User/GetAllUsers') // Aangenomen dat je een endpoint hebt dat alle gebruikers retourneert
        .then(response => response.json())
        .then(users =>
        {
            const tableBody = document.querySelector('#userTable tbody'); // Zorg ervoor dat je de juiste tabelselectoren gebruikt
            tableBody.innerHTML = ''; // Maak de tabel leeg

            users.forEach(user =>
            {
                const row = document.createElement('tr');
                row.innerHTML = `
                    <td>${user.name}</td>
                    <td>${user.surname}</td>
                    <td>${user.alias}</td>
                    <td>${user.address}</td>
                    <td>${user.zip}</td>
                    <td>${user.city}</td>
                    <td>${user.email}</td>
                    <td>${user.phonenumber}</td>
                    <td>${user.online ? 'Ja' : 'Nee'}</td>
                    <td>${user.sick ? 'Ja' : 'Nee'}</td>
                    <td>
                        <a href="/EditUserDetails/Index?alias=${user.alias}" class="btn btn-info btn-sm">Bewerk</a>
                    </td>
                    <td>
                        <button onclick="deleteUser(this, '${user.alias}')" class="btn btn-danger btn-sm">Verwijder</button>
                    </td>
                `;
                tableBody.appendChild(row);
            });
        })
        .catch(error => console.error('Fout bij het ophalen van gebruikers:', error));
}
