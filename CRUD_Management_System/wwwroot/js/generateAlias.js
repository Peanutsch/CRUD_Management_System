function generateAlias()
{
    var name = $('#Name').val().trim(); // Trim om spaties te verwijderen
    var surname = $('#Surname').val().trim();

    // Als een van beide velden leeg is, alias veld leegmaken en stoppen
    if (!name || !surname)
    {
        $('#Alias').val(''); // Leegmaken
        return; // Stop de functie
    }

    console.log('js name: ' + name);
    console.log('js surname: ' + surname);
    console.log(generateAliasUrl); // Log de juiste URL

    $.ajax({
        url: generateAliasUrl,
        type: 'POST',
        data: { name: name, surname: surname },
        success: function (data)
        {
            console.log('Generated alias: ' + data);
            $('#Alias').val(data);
        },
        error: function ()
        {
            console.error('Er is een fout opgetreden bij het genereren van de alias.');
        }
    });
}
