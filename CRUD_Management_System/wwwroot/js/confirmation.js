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
                html: "Please confirm to save changes for <strong>[" + alias + "]</strong>",  // Make alias bold using <strong>
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
                    document.querySelector("form").submit();
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
                html: "Please confirm to save new account <strong>[" + alias + "]</strong>",  // Make alias bold using <strong>
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
                    document.querySelector(".user-form").submit();
                }
            });
        });
    }

    // SweetAlert for Delete User Button(s) in the Table
    var deleteButtons = document.querySelectorAll(".delete-btn"); // Select all delete buttons
    deleteButtons.forEach(function (button)
    {
        button.addEventListener("click", function (event)
        {
            var alias = button.getAttribute("data-alias"); // Get user alias

            Swal.fire({
                title: "Confirm",
                html: "Are you sure you want to delete account <strong>[" + alias + "]</strong>?",  // Make alias bold using <strong>
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
                    // Perform delete action
                    deleteUser(button, alias); // Perform the delete action via fetch
                }
            });
        });
    });
});
