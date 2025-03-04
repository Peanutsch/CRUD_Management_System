function deleteUser(button, alias)
{
    const row = button.closest('tr'); // Get the closest row

    fetch('/User/DeleteUser', {
        method: 'DELETE', // DELETE method to match the server's API
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value // CSRF token
        },
        body: JSON.stringify({ Alias: alias }) // Send the alias in the body
    })
        .then(response => response.json())
        .then(result =>
        {
            if (result.success)
            {
                row.remove(); // Remove the row from the table
                alert(result.message); // Show success message
            } else
            {
                alert("Error deleting user: " + result.message); // Show error message
            }
        })
        .catch(error =>
        {
            console.error('Error deleting:', error);
            alert("Error deleting user.");
        });
}