// Event Listener voor aan alle knoppen met de klasse 'delete-btn' bij het laden van de pagina
document.addEventListener('DOMContentLoaded', function ()
{
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

function deleteUser(button, alias) {
    if (!confirm(`Weet je zeker dat je gebruiker ${alias} wilt verwijderen?`)) {
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
                alert(result.message); // Optioneel: toon een succesbericht
                window.location.reload(); // Ververs de pagina na verwijdering
            }
            else {
                alert("Fout bij verwijderen van gebruiker: " + result.message);
            }
        })
        .catch(error => console.error('Fout bij verwijderen:', error));
}
