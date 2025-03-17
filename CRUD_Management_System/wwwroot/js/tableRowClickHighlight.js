document.addEventListener("DOMContentLoaded", function ()
{
    const tableBody = document.getElementById("userTableBody");

    // Exit if the table body does not exist
    if (!tableBody) return;

    // Add a click event listener to the table body (Event Delegation)
    tableBody.addEventListener("click", function (event)
    {
        const row = event.target.closest("tr"); // Find the nearest <tr> element
        if (!row) return; // Exit if no row was clicked

        // Remove highlight from all rows
        document.querySelectorAll("tbody tr").forEach(r => r.classList.remove("highlight-row"));

        // Add highlight to the clicked row
        row.classList.add("highlight-row");
    });

    // Remove highlight when clicking outside the table
    document.addEventListener("click", function (event)
    {
        if (!tableBody.contains(event.target))
        {
            document.querySelectorAll("tbody tr").forEach(row => row.classList.remove("highlight-row"));
        }
    });
});
