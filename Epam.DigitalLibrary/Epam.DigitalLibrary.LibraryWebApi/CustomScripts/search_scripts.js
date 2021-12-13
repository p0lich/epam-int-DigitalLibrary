async function GetNotes() {
    const response = await fetch("api/Search/GetNotes", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    if (response.ok == true) {
        const notes = await response.json();
        let rows = document.querySelector("tbody");
        notes.forEach(note => {
            rows.append(row(note));
        });
    }
}