const connection = new signalR.HubConnectionBuilder()
    .withUrl('https://localhost:7096/scoreboardHub')
    .withAutomaticReconnect()
    .build();

connection.on("ScoreboardUpdated", message => {
    console.log(`${message.league} Updated at ${message.updatedUtc}`);
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