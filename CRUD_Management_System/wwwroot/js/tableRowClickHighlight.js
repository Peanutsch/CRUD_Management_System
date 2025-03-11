document.addEventListener("DOMContentLoaded", function ()
{
    // Get all rows in the table
    var tableRows = document.querySelectorAll("tbody tr");

    // Add a click event listener to each row
    tableRows.forEach(function (row)
    {
        row.addEventListener("click", function (event)
        {
            // Remove the highlight class from all rows
            tableRows.forEach(function (r)
            {
                r.classList.remove("highlight-row");  // Removes the highlight class
            });

            // Add the highlight class to the clicked row
            row.classList.add("highlight-row");  // Adds the highlight class to the clicked row

            // Prevent the click event from propagating to the document
            event.stopPropagation();
        });
    });

    // Add a click event listener to the document to reset the background color when clicked outside the table
    document.addEventListener("click", function ()
    {
        tableRows.forEach(function (row)
        {
            row.classList.remove("highlight-row");  // Reset background color
        });
    });
});
