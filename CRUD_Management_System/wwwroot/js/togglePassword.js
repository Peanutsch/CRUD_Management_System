document.getElementById("togglePassword").addEventListener("click", function () {
    var passwordInput = document.getElementById("password");
    if (passwordInput.type === "password") {
        passwordInput.type = "text";
        this.innerHTML = "<i class='fa fa-eye-slash'></i>"; // Oog-icoon voor zichtbaar
        this.classList.remove("btn-outline-secondary");
        this.classList.add("btn-outline-danger"); // Verander naar rode stijl
        passwordInput.classList.add("input-pswisvisible"); // Voeg rode rand toe
    } else {
        passwordInput.type = "password";
        this.innerHTML = "<i class='fa fa-eye'></i>"; // Oog-icoon voor onzichtbaar
        this.classList.remove("btn-outline-danger");
        this.classList.add("btn-outline-secondary"); // Verander terug naar secundaire stijl
        passwordInput.classList.remove("input-pswisvisible"); // Verwijder rode rand
    }
});
