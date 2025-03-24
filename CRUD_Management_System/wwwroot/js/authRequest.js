// Wait for the DOM to fully load before executing the script
document.addEventListener("DOMContentLoaded", function ()
{
    addAuthHeaderToRequest('/api/users'); // Fetch users using the new API
});

// Function to add the JWT token to the request header and fetch user data
function addAuthHeaderToRequest(url)
{
    const token = getCookie('AuthToken'); // Retrieve the JWT token from cookies

    if (!token)
    {
        console.warn("[authRequest.js] No JWT token found in cookies...");
        return;
    }

    // Send a GET request to the API with the JWT token in the Authorization header
    fetch(url, {
        method: 'GET',
        headers: { 'Authorization': `Bearer ${token}` }
    })
        .then(response => response.ok ? response.json() : Promise.reject('Network response was not ok')) // Handle HTTP response
        .then(displayUsers) // Pass retrieved users data to displayUsers function
        .catch(error => console.error("Request failed:", error)); // Log errors if the request fails
}

// Function to retrieve a cookie by name
function getCookie(name)
{
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);

    if (parts.length === 2)
    {
        return parts.pop().split(';').shift();
    }

    console.warn("[authRequest.js] getCookie > No cookie found");
    return null;
}

// Function to display user data in a table
function displayUsers(users)
{
    const tableBody = document.getElementById("userTableBody");
    if (!tableBody) return; // Exit if table body is not found

    // Generate the table rows in one operation and insert them into the DOM
    tableBody.innerHTML = users.map(user => `
        <tr>
            <td class="table-cell">${user.name}</td>          <!-- Display Name -->
            <td class="table-cell">${user.surname}</td>       <!-- Display Surname -->
            <td class="table-cell">${user.alias}</td>         <!-- Display Alias -->
            <td class="table-cell">${user.address}</td>       <!-- Display Address -->
            <td class="table-cell">${user.zip}</td>           <!-- Display ZIP Code -->
            <td class="table-cell">${user.city}</td>          <!-- Display City -->
            <td class="table-cell">${user.email}</td>         <!-- Display Email -->
            <td class="table-cell">${user.phonenumber}</td>   <!-- Display Phone Number -->
            <td class="table-cell">${user.online}</td>        <!-- Display Online Status -->
            <td class="table-cell">${user.sick}</td>          <!-- Display Sick Status -->
            <td>
                <!-- Edit button linking to the edit user page -->
                <a href="/EditUserDetails/Index?alias=${user.alias}" class="btn btn-info btn-sm">Edit</a>
            </td>
            <td>
                <!-- Delete button with the alias as a data attribute -->
                <button class="btn btn-info btn-sm delete-btn" data-alias="${user.alias}">Delete</button>
            </td>
        </tr>
    `).join(""); // Join the array into a single HTML string and update the table body
}

/*
// Event delegation to handle delete button clicks dynamically
document.addEventListener("click", function (event)
{
    const deleteButton = event.target.closest(".delete-btn");
    if (!deleteButton) return; // Ignore clicks that are not on a delete button

    const alias = deleteButton.dataset.alias; // Retrieve the alias from the data attribute

    // Call the function to show the delete confirmation
    showDeleteConfirmation(alias, deleteButton, deleteUser);
});
*/
