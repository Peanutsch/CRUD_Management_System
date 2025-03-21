function generateAlias() {
    var name = $('#Name').val().trim();
    var surname = $('#Surname').val().trim();

    if (!name || !surname) {
        $('#Alias').val('');
        return;
    }

    $.ajax({
        url: "/CreateUser/GenerateAlias",
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ name: name, surname: surname }),
        success: function (data) {
            console.log('Generated alias: ' + data);
            $('#Alias').val(data);
            $('#saveCreateUserButton').attr('data-alias', data);
        },
        error: function (xhr, status, error) {
            console.error('Error generating alias:', xhr.responseText);
        }
    });
}
