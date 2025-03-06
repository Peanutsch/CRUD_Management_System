document.addEventListener("DOMContentLoaded", function ()
{
    const aliasInput = document.getElementById("alias");
    const passwordInput = document.getElementById("password");
    const loginButton = document.querySelector("button[type='submit']");

    function toggleButtonState()
    {
        if (aliasInput.value.trim() !== "" && passwordInput.value.trim() !== "")
        {
            loginButton.removeAttribute("disabled");
        } else
        {
            loginButton.setAttribute("disabled", "disabled");
        }
    }

    aliasInput.addEventListener("input", toggleButtonState);
    passwordInput.addEventListener("input", toggleButtonState);

    // Zet de knop standaard uit bij het laden van de pagina
    toggleButtonState();
});