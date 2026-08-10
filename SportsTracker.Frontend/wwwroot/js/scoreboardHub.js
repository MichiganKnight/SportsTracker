const connection = new signalR.HubConnectionBuilder()
    .withUrl(window.sportsTracker.scoreboardHub)
    .withAutomaticReconnect()
    .build();

connection.on("ScoreboardUpdated", refreshLeague);

async function refreshLeague(payload) {
    const league = payload.league;
    const id = `league-${league.toLowerCase()}`;

    const current = document.getElementById(id);
    if (!current) {
        console.warn(`League Section Not Found: ${id}`)
        return;       
    }
    
    const oldScores = captureScores(current);
    
    const response = await fetch(`/Dashboard/LeagueSection?league=${league}&t=${Date.now()}`, {
        cache: "no-store"
    });
    
    if (!response.ok) {    
        console.error(`Failed to Refresh League:`, response.status);
        return;
    }
    
    const html = await response.text();
    
    const parser = new DOMParser();
    const doc = parser.parseFromString(html, "text/html");    
    
    const replacement = doc.getElementById(id);
    
    if (!replacement) {
        console.error(`Replacement League Section Not Found: ${id}`);
        return;
    }
    
    const changedGames = findScoreChanges(oldScores, replacement);
    
    current.replaceWith(replacement);
    
    animateScoreChanges(changedGames);
}

function captureScores(leagueSection) {
    const scores = new Map();
    
    const games = leagueSection.querySelectorAll("[data-game-id]");
    
    games.forEach(game => {
        const gameId = game.dataset.gameId;        
        const scoreElements = game.querySelectorAll("[data-team-score]");
        
        const gameScores = Array.from(scoreElements).map(element => Number.parseInt(element.textContent.trim(), 10) || 0);
        
        scores.set(gameId, gameScores);
    });
    
    return scores;
}

function findScoreChanges(oldScores, replacementSection) {
    const changedGames = [];    
    const games = replacementSection.querySelectorAll("[data-game-id]");
    
    games.forEach(game => {
        const gameId = game.dataset.gameId;        
        const oldScore = oldScores.get(gameId);
        
        if (!oldScore) {
            return;
        }
        
        const newScore = Array.from(game.querySelectorAll("[data-team-score]")).map(element => Number.parseInt(element.textContent.trim(), 10) || 0);
        
        if (oldScore.length !== newScore.length) {
            return;
        }
        
        const changedTeams = [];
        
        newScore.forEach((score, index) => {
            const previousScore = oldScore[index];
            
            if (score > previousScore) {
                changedTeams.push({
                    index: index,
                    difference: score - previousScore
                });
            }
        });
        
        if (changedTeams.length > 0) {
            changedGames.push({
                gameId: gameId,
                teams: changedTeams
            });
        }       
    });
    
    return changedGames;
}

function animateScoreChanges(gameIds) {
    gameIds.forEach(gameId => {
        const card = document.querySelector(`[data-game-id="${gameId}"]`);
        
        if (!card) {
            return;
        }
        
        card.classList.remove("score-changed");
        
        void card.offsetWidth;
        
        card.classList.add("score-changed");       
        
        setTimeout(() => {
            card.classList.remove("score-changed");
        }, 1200);
    });
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