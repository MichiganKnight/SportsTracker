const connection = new signalR.HubConnectionBuilder()
    .withUrl('https://localhost:7096/scoreboardHub')
    .withAutomaticReconnect()
    .build();

connection.on("ScoreboardUpdated", async league => {
    console.log("Refreshing", league);
    
    const response = await fetch(`/Dashboard/LeagueSection?league=${league}`);
    
    if (!response.ok) {
        return;
    }
    
    const html = await response.text();    
    
    const parser = new DOMParser();    
    const documentFragment = parser.parseFromString(html, "text/html");    
    const replacement = documentFragment.body.firstElementChild;
    
    const current = document.getElementById(`league-${league.toLowerCase()}`);
    
    if (current && replacement) {
        replacement.classList.add("league-updated");
        
        current.replaceWith(replacement);
        
        console.log(`${league} Section Updated`);
    } else {
        console.warn(`${league} Section Not Found`);
    }
});

async function start() {
    try {
        await connection.start();
        
        console.log("SignalR Connected");
    } catch (err) {
        console.error(err);
        
        setTimeout(start, 5000);
    }
}

connection.onreconnecting(() => {
    console.log("Reconnecting...");
});

connection.onreconnected(() => {
    console.log("Reconnected");
});

start();