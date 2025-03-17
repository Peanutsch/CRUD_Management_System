// Schakel de "Opslaan" knop in wanneer alle velden zijn ingevuld
document.querySelector('form').addEventListener('input', function ()
{
    const isFormValid = this.checkValidity();  // Gebruik ingebouwde validatie van HTML
    document.getElementById('saveCreateUserButton').disabled = !isFormValid;
});
