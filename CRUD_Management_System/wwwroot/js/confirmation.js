document.addEventListener("DOMContentLoaded", function ()
{
    // SweetAlert for Save Edit Button
    var saveEditButton = document.getElementById("saveEditButton");
    if (saveEditButton)
    {
        saveEditButton.addEventListener("click", function (event)
        {
            var alias = saveEditButton.getAttribute("data-alias"); // Get user alias from data-alias attribute

            Swal.fire({
                title: "Confirm",
                html: `Please confirm to save changes for <strong>[${alias.toUpperCase()}]</strong>`,  // Make alias bold using <strong>
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Save",
                cancelButtonText: "Cancel"
            }).then((result) =>
            {
                if (result.isConfirmed)
                {
                    Swal.fire({
                        toast: true,
                        position: "top-end",
                        icon: "success",
                        title: "Succes!",
                        html: `Changes for <strong>[${alias.toUpperCase()}]</strong> are saved!`,
                        showConfirmButton: false,
                        timer: 1500
                    }).then(() =>
                    {
                        document.querySelector("form").submit();
                    });
                }
            });
        });
    }

    // SweetAlert for Save Create User Button
    var saveCreateUserButton = document.getElementById("saveCreateUserButton");
    if (saveCreateUserButton)
    {
        saveCreateUserButton.addEventListener("click", function (event)
        {
            var alias = saveCreateUserButton.getAttribute("data-alias"); // Get user alias from data-alias attribute

            Swal.fire({
                title: "Confirm",
                html: `Please confirm to save new account <strong>[${alias.toUpperCase()}]</strong>`,  // Make alias bold using <strong>
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Save",
                cancelButtonText: "Cancel"
            }).then((result) =>
            {
                if (result.isConfirmed)
                {
                    Swal.fire({
                        toast: true,
                        position: "top-end",
                        icon: "success",
                        title: "Succes!",
                        html: `New user account <strong>[${alias.toUpperCase()}]</strong> is saved!`,
                        showConfirmButton: false,
                        timer: 1500
                    }).then(() =>
                    {
                        createUser();
                    });
                }
            });
        });
    }

    // SweetAlert for Delete User Button(s) in the Table
    // Selecteer de <tbody> waarin gebruikers dynamisch worden geladen
    var tableBody = document.getElementById("userTableBody");

    if (!tableBody) return; // Stop als het element niet bestaat

    // Gebruik event delegation om clicks op delete-buttons af te vangen
    tableBody.addEventListener("click", function (event)
    {
        var button = event.target.closest(".delete-btn");
        if (!button) return; // Stop als er niet op een delete-button is geklikt

        var alias = button.getAttribute("data-alias");

        Swal.fire({
            title: "Confirm",
            html: `Are you sure you want to delete account <strong>[${alias.toUpperCase()}]</strong>?`,
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Delete",
            cancelButtonText: "Cancel"
        }).then((result) =>
        {
            if (result.isConfirmed)
            {
                Swal.fire({
                    toast: true,
                    position: "top-end",
                    icon: "success",
                    title: "Succes!",
                    html: `User account <strong>[${alias.toUpperCase()}]</strong> is deleted!`,
                    showConfirmButton: false,
                    timer: 1500
                }).then(() =>
                {
                    deleteUser(button, alias);
                });
            }
        });
    });

    // SweetAlert and handler for LogOut
    var logoutButton = document.getElementById("LogOutButton");
    if (logoutButton)
    {
        logoutButton.addEventListener("click", function (event)
        {
            event.preventDefault();

            Swal.fire({
                title: "Confirm Exit",
                html: `Are you sure you want to log out?`,
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Yes",
                cancelButtonText: "No"
            }).then((result) =>
            {
                if (result.isConfirmed)
                {
                    fetch('/Login/Logout', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                        }
                    })
                        .then(response =>
                        {
                            if (response.ok)
                            {
                                Swal.fire({
                                    toast: true,
                                    position: "top-end",
                                    icon: "success",
                                    title: "Succes!",
                                    html: `Logout succes!`,
                                    showConfirmButton: false,
                                    timer: 1500
                                }).then(() =>
                                {
                                    window.location.href = "/Login/Index";  // Redirect to login
                                });
                            } else
                            {
                                throw new Error("Logout failed");
                            }
                        })
                        .catch(error => console.error('Logout error:', error));
                }
            });
        });
    }
});