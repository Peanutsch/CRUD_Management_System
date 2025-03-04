function generateAlias()
{
    var name = $('#Name').val().trim(); // Trim spaces to remove leading/trailing spaces
    var surname = $('#Surname').val().trim(); // Trim spaces for the surname

    // If either field is empty, clear the alias field and stop the function
    if (!name || !surname)
    {
        $('#Alias').val(''); // Clear the alias input
        return; // Stop the function execution
    }

    $.ajax({
        url: generateAliasUrl, // URL to send the request to
        type: 'POST', // HTTP method to use
        data: { name: name, surname: surname }, // Send name and surname as data
        success: function (data)
        {
            console.log('Generated alias: ' + data); // Log the generated alias

            // Set the alias in the input field (readonly input field)
            document.getElementById('Alias').value = data;

            // Set the alias in the data-alias attribute of the save button
            document.getElementById('saveCreateUserButton').setAttribute('data-alias', data);
        },
        error: function ()
        {
            console.error('An error occurred while generating the alias.'); // Log error if AJAX fails
        }
    });
}