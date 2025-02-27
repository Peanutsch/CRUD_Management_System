// Event Listener voor aan alle knoppen met de klasse 'edit-btn' bij het laden van de pagina
document.addEventListener('DOMContentLoaded', function () {
    // Laad de gebruikersgegevens bij het laden van de pagina
    refreshUserTable();

    // Event Listener voor aan alle knoppen met de klasse 'edit-btn'
    document.querySelectorAll('.edit-btn').forEach(button => {
        button.addEventListener('click', function () {
            editUser(this); // Roept de editUser functie aan en geeft de knop door
        });
    });
});


function editUser(button)
{
    const user = JSON.parse(button.getAttribute('data-user'));
    const row = button.closest('tr');
    const cells = row.querySelectorAll('td');

    // Bewaar originele waarden voor annuleren
    row.dataset.originalValues = JSON.stringify(user);

    // Vervang tekst door inputvelden, met uitzondering van alias. Totaal aantal inputvelden: 9
    cells[0].innerHTML = `<input type="text" class="input-field" value="${user.name}" />`;
    cells[1].innerHTML = `<input type="text" class="input-field" value="${user.surname}" />`;
    cells[2].innerText = user.alias; // Alias blijft onveranderd
    cells[3].innerHTML = `<input type="text" class="input-field" value="${user.address}" />`;
    cells[4].innerHTML = `<input type="text" class="input-field" value="${user.zip}" />`;
    cells[5].innerHTML = `<input type="text" class="input-field" value="${user.city}" />`;
    cells[6].innerHTML = `<input type="text" class="input-field" value="${user.email}" />`;
    cells[7].innerHTML = `<input type="text" class="input-field" value="${user.phonenumber}" />`;
    cells[8].innerHTML = `<input type="checkbox" ${user.online ? 'checked' : ''} />`;
    cells[9].innerHTML = `<input type="checkbox" ${user.sick ? 'checked' : ''} />`;

    // Vervang de "Edit" knop door "Save" en "Cancel"
    cells[10].innerHTML = `
        <button onclick="saveUser(this, '${user.alias}')" class="btn btn-success btn-sm">Save</button>
        <button onclick="cancelEdit(this)" class="btn btn-secondary btn-sm">Cancel</button>
        <button onclick="deleteUser(this, '${user.alias}')" class="btn btn-secondary btn-sm">Delete</button>
    `;
}

function saveUser(button, alias)
{
    if (!confirm(`Weet je zeker dat je deze gegevens voor ${alias} wil opslaan?`)) {
        return; // Stop als de gebruiker annuleren kiest
    }

    const row = button.closest('tr');
    const inputs = row.querySelectorAll('input'); // Verzamel alle invoervelden (input) exclusief alias

    let userData = { Alias: alias }; // Voeg alias direct toe, omdat deze niet in inputs zit
    const keys = ["Name", "Surname", "Address", "ZIP", "City", "Email", "Phonenumber", "Online", "Sick"];

    for (let i = 0; i < inputs.length; i++)
    {
        userData[keys[i]] = inputs[i].type === "checkbox" ? inputs[i].checked : inputs[i].value;
        console.log(`${keys[i]}: ${userData[keys[i]]}`);
    }

    // Controleer of alle velden correct zijn ingelezen
    console.log("Verwerkt userData:", userData);

    // AJAX-aanroep om de gegevens op te slaan
    fetch(`/User/UpdateUser`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(userData),
    })
        .then(response => response.json())
        .then(updatedUser =>
        {
            updateRow(row, updatedUser); // Werk de rij bij met de nieuwe data
        })
        .catch(error => console.error('Fetch error:', error));
}

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
        .then(result => {
            if (result.success) {
                row.remove(); // Verwijder de rij uit de tabel
                refreshUserTable(); // Ververs de tabel na verwijdering
            } else {
                alert("Fout bij verwijderen van gebruiker: " + result.message);
            }
        })
        .catch(error => console.error('Fout bij verwijderen:', error));
}

function cancelEdit(button)
{
    const row = button.closest('tr');
    const originalUser = JSON.parse(row.dataset.originalValues);

    // Herstel de originele waarden
    updateRow(row, originalUser);
}

function updateRow(row, user)
{
    const cells = row.querySelectorAll('td');

    // Zet de cellen terug naar tekst
    cells[0].innerText = user.name;
    cells[1].innerText = user.surname;
    cells[2].innerText = user.alias; // Alias blijft onveranderd
    cells[3].innerText = user.address;
    cells[4].innerText = user.zip;
    cells[5].innerText = user.city;
    cells[6].innerText = user.email;
    cells[7].innerText = user.phonenumber;
    cells[8].innerText = user.online ? "Yes" : "No";
    cells[9].innerText = user.sick ? "Yes" : "No";

    // Zet de originele "Edit" knop terug
    cells[10].innerHTML = `
        <button onclick="editUser(this)" data-user='${JSON.stringify(user)}' class="btn btn-info btn-sm">Edit</button>
    `;
}

function refreshUserTable() {
    fetch('/User/GetAllUsers') // Aangenomen dat je een endpoint hebt dat alle gebruikers retourneert
        .then(response => response.json())
        .then(users => {
            const tableBody = document.querySelector('#userTable tbody'); // Zorg ervoor dat je de juiste tabelselectoren gebruikt
            tableBody.innerHTML = ''; // Maak de tabel leeg

            users.forEach(user => {
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
                        <button onclick="editUser(this)" data-user='${JSON.stringify(user)}' class="btn btn-info btn-sm">Bewerk</button>
                        <button onclick="deleteUser(this, '${user.alias}')" class="btn btn-danger btn-sm">Verwijder</button>
                    </td>
                `;
                tableBody.appendChild(row);
            });
        })
        .catch(error => console.error('Fout bij het ophalen van gebruikers:', error));
}

