function generateAlias()
{
    var name = $('#Name').val();
    var surname = $('#Surname').val();

    // Controleer of zowel voornaam als achternaam ingevuld zijn
    if (name && surname)
    {
        console.log('js name: ' + name);    // Console log voor naam
        console.log('js surname: ' + surname); // Console log voor achternaam
        console.log(generateAliasUrl); // Log de juiste URL

        $.ajax({
            url: generateAliasUrl, // Gebruik de server-side gegenereerde URL
            type: 'POST',
            data: { name: name, surname: surname },

            success: function (data)
            {
                console.log('Generated alias: ' + data); // Log de gegenereerde alias
                $('#Alias').val(data); // Vul het alias veld in
            },
            error: function ()
            {
                console.error('Er is een fout opgetreden bij het genereren van de alias.');
            }
        });
    }
}