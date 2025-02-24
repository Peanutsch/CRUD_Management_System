function filterTable()
{                                    
    const input = document.getElementById("searchInput");                   // Verkrijg het zoekveld en de waarde die de gebruiker heeft ingevoerd
    const filter = input.value.toLowerCase();                               // Zet de invoer om naar kleine letters voor case-insensitieve vergelijking

    const table = document.getElementById("userTableBody");                 // Verkrijg de table body waar we de rijen willen doorzoeken
    const tr = table.getElementsByTagName("tr");                            // Verkrijg alle rijen in de tabel

    // 
    for (let i = 0; i < tr.length; i++)                                     // Loop door elke rij in de tabel
    {                                                                       // let i = variable(var) i in for-block(block - scope)
        let row = tr[i];                                                    // Huidige rij
        let cells = row.getElementsByTagName("td");                         // Verkrijg de cellen in de huidige rij
        let found = false;                                                  // Vlag om bij te houden of een overeenkomst is gevonden

        for (let j = 0; j < cells.length; j++)                              // Loop door elke cel in de huidige rij
        {
            if (cells[j])                                                    // Controleer of de cel bestaat
            { 
                const txtValue = cells[j].textContent || cells[j].innerText; // Verkrijg de tekstinhoud van de cel
                if (txtValue.toLowerCase().indexOf(filter) > -1)             // Controleer of de tekstinhoud de zoekterm bevat
                {
                    found = true;                                            // Een overeenkomst is gevonden
                    break;                                                   // Stop met zoeken in deze rij
                }
            }
        }

        if (found)                                                           // Toont of verbergt de rij op basis van de overeenkomst
        {
            row.style.display = "";                                          // Toont de rij
        }
        else
        {
            row.style.display = "none";                                      // Verbergt de rij
        }
    }
}