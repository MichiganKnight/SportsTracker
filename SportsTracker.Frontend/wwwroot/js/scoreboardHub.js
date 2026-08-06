const connection = new signalR.HubConnectionBuilder()
    .withUrl(window.sportsTracker.scoreboardHub)
    .withAutomaticReconnect()
    .build();

connection.on("ScoreboardUpdated", refreshLeague);

async function refreshLeague(payload) {
    const league = payload.league;
    const id = `league-${league.toLowerCase()}`;
    
    const response = await fetch(`/Dashboard/LeagueSection?league=${league}&t=${Date.now()}`, {
        cache: "no-store"
    });
    
    if (!response.ok) {        
        return;
    }
    
    const html = await response.text();
    
    const parser = new DOMParser();
    const doc = parser.parseFromString(html, "text/html");
    
    const current = document.getElementById(id);
    const replacement = doc.getElementById(id);
    
    if (!current || !replacement) {
        return;
    }
    
    current.replaceWith(replacement);
}

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