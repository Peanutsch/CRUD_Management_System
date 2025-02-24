function filterTable()
{
    // Verkrijg het zoekveld en de waarde die de gebruiker heeft ingevoerd
    const input = document.getElementById("searchInput");
    const filter = input.value.toLowerCase(); // Zet de invoer om naar kleine letters voor case-insensitieve vergelijking

    // Verkrijg de table body waar we de rijen willen doorzoeken
    const table = document.getElementById("userTableBody");
    const tr = table.getElementsByTagName("tr"); // Verkrijg alle rijen in de tabel

    // Loop door elke rij in de tabel
    for (let i = 0; i < tr.length; i++)
    {
        let row = tr[i]; // Huidige rij
        let cells = row.getElementsByTagName("td"); // Verkrijg de cellen in de huidige rij
        let found = false; // Vlag om bij te houden of een overeenkomst is gevonden

        // Loop door elke cel in de huidige rij
        for (let j = 0; j < cells.length; j++)
        {
            if (cells[j])
            { // Controleer of de cel bestaat
                const txtValue = cells[j].textContent || cells[j].innerText; // Verkrijg de tekstinhoud van de cel
                // Controleer of de tekstinhoud de zoekterm bevat
                if (txtValue.toLowerCase().indexOf(filter) > -1)
                {
                    found = true; // Een overeenkomst is gevonden
                    break; // Stop met zoeken in deze rij
                }
            }
        }

        // Toont of verbergt de rij op basis van de overeenkomst
        if (found)
        {
            row.style.display = ""; // Toont de rij
        } else
        {
            row.style.display = "none"; // Verbergt de rij
        }
    }
}