function capitalizeFirstLetter(fieldId)
{
    let inputField = document.getElementById(fieldId);
    inputField.value = inputField.value.charAt(0).toUpperCase() + inputField.value.slice(1);
}

function capitalizeZIPLetters(fieldId)
{
    let inputField = document.getElementById(fieldId);
    inputField.value = inputField.value.toUpperCase();
}

function capitalizeSurname(fieldId)
{
    let inputField = document.getElementById(fieldId);
    let words = inputField.value.split(" ");  // Split the input into words

    let tussenvoegsels = [
        "'t", "t'", "'l", "de", "in", "te", "op", "du", "la", "le",
        "van", "den", "der", "het", "ten", "ter", "aan", "von", "zu", "zum",
        "de", "de l", "de la", "d'", "l'"
    ];

    let formattedSurname = words.map(word =>
    {
        let lowerWord = word.toLowerCase();  // Convert to lowercase for comparison

        // If the word is a tussenvoegsel, make it lowercase
        if (!tussenvoegsels.includes(lowerWord))
        {
            return word.charAt(0).toUpperCase() + word.slice(1).toLowerCase();
        } else
        {
            return lowerWord;  // Keep the word lowercase
        }
    }).join(" ");

    inputField.value = formattedSurname;  // Update the field with the formatted surname
}


document.getElementById("Name").addEventListener("input", function ()
{
    capitalizeFirstLetter("Name");
});
document.getElementById("Surname").addEventListener("input", function ()
{
    capitalizeSurname("Surname");
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
