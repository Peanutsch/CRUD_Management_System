function capitalizeFirstLetter(fieldId)
{
    let inputField = document.getElementById(fieldId);
    inputField.value = inputField.value.charAt(0).toUpperCase() + inputField.value.slice(1);
}

function capitalizeZIPLetters(fieldId)
{
    let inputField = document.getElementById(fieldId)
    inputField.value = inputField.value.toUpperCase();
}

function capitalizeSurname(fieldId)
{
    let inputField = document.getElementById(fieldId);
    let words = inputField.value.toLowerCase().split(" ");

    let tussenvoegsels = [
                            "van", "de", "den", "der", "het", "in", "te", "ten", "op", "aan",
                            "de la", "de l", "de", "du", "la", "le", "ter", "t'", "von", "zu",
                            "van der", "van den", "van de", "d'", "'t"
                         ];

    let formattedSurname = words.map((word, index, arr) =>
    {
        // Eerste of laatste woord krijgt een hoofdletter, tussenvoegsels blijven klein
        if (index === 0 || index === arr.length - 1 || !tussenvoegsels.includes(word))
        {
            return word.charAt(0).toUpperCase() + word.slice(1);
        }
        return word;
    }).join(" ");

    inputField.value = formattedSurname;
}

document.getElementById("Name").addEventListener("input", function ()
{
    capitalizeFirstLetter("Name");
});
document.getElementById("Address").addEventListener("input", function ()
{
    capitalizeFirstLetter("Address");
});
document.getElementById("City").addEventListener("input", function ()
{
    capitalizeFirstLetter("City");
});
document.getElementById("ZIP").addEventListener("input", function ()
{
    capitalizeZIPLetters("ZIP");
});
document.getElementById("Surname").addEventListener("input", function ()
{
    capitalizeSurname("Surname");
});    

