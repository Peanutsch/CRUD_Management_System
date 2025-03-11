// Event listener to trigger when the DOM is fully loaded
document.addEventListener("DOMContentLoaded", function ()
{
    // Call the function to add the JWT token to the request header and fetch user data
    addAuthHeaderToRequest('/api/users'); // Haal gebruikers op via de nieuwe API
});

// Function to add JWT token to the header of the request
function addAuthHeaderToRequest(url)
{
    // Get the JWT token stored in sessionStorage
    const token = sessionStorage.getItem('jwtToken');

    // Check if the token exists
    if (token)
    {
        // Perform a GET request to the API with the token in the Authorization header
        fetch(url, {
            method: 'GET',  // GET method to fetch data
            headers: {
                'Authorization': `Bearer ${token}`, // Include the JWT token in the Authorization header
            }
        })
            .then(response =>
            {
                // Check if the response is okay (status code 200-299)
                if (!response.ok)
                {
                    throw new Error('Network response was not ok');
                }
                return response.json(); // Parse the response as JSON
            })
            .then(data =>
            {
                // Log the received data to the console
                console.log("Data ontvangen:", data);
                // Call function to display the users data on the page
                displayUsers(data);
            })
            .catch(error =>
            {
                // Log an error message if the request fails
                console.error("Request mislukt:", error);
            });
    } else
    {
        // If no JWT token is found in sessionStorage, log a warning
        console.warn("Geen JWT-token gevonden in localStorage.");
    }
}

// Function to display the list of users in the table
function displayUsers(users)
{
    // Get the table body element where user rows will be inserted
    const tableBody = document.getElementById("userTableBody");

    // Clear the current content of the table body (if any)
    tableBody.innerHTML = "";

    // Loop through each user in the received data
    users.forEach(user =>
    {
        // Create a new row for each user
        const row = document.createElement("tr");

        // Set the inner HTML of the row with the user's data
        row.innerHTML = `
            <td class="table-cell">${user.Name}</td>               <!-- Display Name -->
            <td class="table-cell">${user.Surname}</td>            <!-- Display Surname -->
            <td class="table-cell">${user.Alias}</td>              <!-- Display Alias -->
            <td class="table-cell">${user.Address}</td>            <!-- Display Address -->
            <td class="table-cell">${user.ZIP}</td>                <!-- Display ZIP Code -->
            <td class="table-cell">${user.City}</td>               <!-- Display City -->
            <td class="table-cell">${user.Email}</td>              <!-- Display Email -->
            <td class="table-cell">${user.Phonenumber}</td>        <!-- Display Phone Number -->
            <td class="table-cell">${user.Online}</td>             <!-- Display Online Status -->
            <td class="table-cell">${user.Sick}</td>               <!-- Display Sick Status -->
            <td>
                <!-- Link to the Edit User page with the user's Alias as a query parameter -->
                <a href="/EditUserDetails/Index?alias=${user.Alias}" class="btn btn-info btn-sm">Edit</a>
            </td>
            <td>
                <!-- Button to delete the user, with the Alias as a data attribute -->
                <button class="btn btn-info btn-sm delete-btn" type="button" data-alias="${user.Alias}">Delete</button>
            </td>
        `;

        // Append the newly created row to the table body
        tableBody.appendChild(row);
    });
}
