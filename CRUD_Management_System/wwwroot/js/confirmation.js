// SweetAlert <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

document.addEventListener("DOMContentLoaded", function () {
    var saveEditButton = document.getElementById("saveEditButton");
    if (saveEditButton) {
        saveEditButton.addEventListener("click", function (event) {
            Swal.fire({
                title: "Bevestiging",
                text: "Wilt u de wijzigingen opslaan?",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Opslaan",
                cancelButtonText: "Annuleren"
            }).then((result) => {
                if (result.isConfirmed) {
                    document.querySelector("form").submit();
                }
            });
        });
    }

    var saveCreateUserButton = document.getElementById("saveCreateUserButton");
    if (saveCreateUserButton) {
        saveCreateUserButton.addEventListener("click", function (event) {
            Swal.fire({
                title: "Bevestiging",
                text: "Wilt u de nieuwe gebruiker opslaan?",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Opslaan",
                cancelButtonText: "Annuleren"
            }).then((result) => {
                if (result.isConfirmed) {
                    document.querySelector(".user-form").submit(); // LET OP: correcte class-selector
                }
            });
        });
    }
});
