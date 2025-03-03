// Functie om de alias te genereren via AJAX
function generateAlias()
{
    var name = $('#Name').val();
    var surname = $('#Surname').val();

    if (name && surname)
    {
        $.ajax({
            url: '@Url.Action("GenerateAlias", "CreateUser")',
            type: 'POST',
            data: { name: name, surname: surname },
            success: function (data)
            {
                // Vul het alias veld in met de gegenereerde alias
                $('input[asp-for="Alias"]').val(data);
            },
            error: function ()
            {
                console.error('Er is een fout opgetreden bij het genereren van de alias.');
            }
        });
    }
}