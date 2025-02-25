// Event Listener voor aan alle knoppen met de klasse 'edit-btn' bij het laden van de pagina
document.addEventListener('DOMContentLoaded', function ()
{
    document.querySelectorAll('.edit-btn').forEach(button =>
    {
        button.addEventListener('click', function ()
        {
            console.log('Event Edit Button');
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
    cells[0].innerHTML = `<input type="text" value="${user.name}" />`;
    cells[1].innerHTML = `<input type="text" value="${user.surname}" />`;
    cells[2].innerText = user.alias; // Alias blijft onveranderd
    cells[3].innerHTML = `<input type="text" value="${user.address}" />`;
    cells[4].innerHTML = `<input type="text" value="${user.zip}" />`;
    cells[5].innerHTML = `<input type="text" value="${user.city}" />`;
    cells[6].innerHTML = `<input type="text" value="${user.email}" />`;
    cells[7].innerHTML = `<input type="text" value="${user.phonenumber}" />`;
    cells[8].innerHTML = `<input type="checkbox" ${user.online ? 'checked' : ''} />`;
    cells[9].innerHTML = `<input type="checkbox" ${user.sick ? 'checked' : ''} />`;

    // Vervang de "Edit" knop door "Save" en "Cancel"
    cells[10].innerHTML = `
        <button onclick="saveUser(this, '${user.alias}')" class="btn btn-success btn-sm">Save</button>
        <button onclick="cancelEdit(this)" class="btn btn-secondary btn-sm">Cancel</button>
    `;
}

function saveUser(button, alias)
{
    const row = button.closest('tr');
    const inputs = row.querySelectorAll('input'); // Verzamel alle invoervelden exclusief alias

    console.log(`inputs= ${JSON.stringify(inputs)}`);
    console.log(`Aantal invoervelden: ${inputs.length}`);

    // Controleer of er genoeg inputs zijn
    if (inputs.length < 9)
    {
        console.error("Niet genoeg invoervelden gevonden!");
        return; // Stop de functie als er niet genoeg inputs zijn
    }

    const userData = {
        Name: inputs[0].value,
        Surname: inputs[1].value,
        Alias: alias, // Alias blijft onveranderd
        Address: inputs[2].value,
        ZIP: inputs[3].value,
        City: inputs[4].value,
        Email: inputs[5].value,
        Phonenumber: inputs[6].value,
        Online: inputs[7] ? inputs[7].checked : false, // Controleer of input bestaat
        Sick: inputs[8] ? inputs[8].checked : false // Controleer of input bestaat
    };

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
        <button onclick="editUser(this)" data-user='${JSON.stringify(user)}' class="btn btn-info btn-sm">Edit Details</button>
    `;
}
