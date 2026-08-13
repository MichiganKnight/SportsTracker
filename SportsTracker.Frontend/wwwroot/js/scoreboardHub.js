const connection = new signalR.HubConnectionBuilder()
    .withUrl(window.sportsTracker.scoreboardHub)
    .withAutomaticReconnect()
    .build();

connection.on("ScoreboardUpdated", async payload => {
    await refreshLeague(payload);
    await refreshGameDetails(payload);
    await refreshPlayByPlay(payload);
    await refreshBoxScore(payload);
});

async function refreshLeague(payload) {
    const league = payload.league;
    const leagueKey = league.toLowerCase();

    const dashboardId = `league-${leagueKey}`;
    const leaguePageId = `league-games-${leagueKey}`;

    const dashboardSection = document.getElementById(dashboardId);
    const leaguePageSection = document.getElementById(leaguePageId);

    if (dashboardSection) {
        await refreshDashboardLeague(league, dashboardId);
    }

    if (leaguePageSection) {
        await refreshLeaguePage(league, leaguePageId);
    }
}

async function refreshGameDetails(payload) {
    const container = document.querySelector("[data-game-details]");

    if (!container) {
        return;
    }

    const pageLeague = container.dataset.league;
    const gameId = container.dataset.gameId;

    if (!pageLeague || !gameId) {
        return;
    }

    if (pageLeague.toLowerCase() !== payload.league.toLowerCase()) {
        return;
    }

    const response = await fetch(`/game/content/${encodeURIComponent(pageLeague)}/${encodeURIComponent(gameId)}?t=${Date.now()}`, {
        cache: "no-store"
    });

    if (!response.ok) {
        console.error(`Unable to Refresh Game: ${gameId}`, response.status);

        return;
    }

    const html = await response.text();

    container.innerHTML = html;

    console.log(`Refreshed Game: ${gameId}`);
}

async function refreshPlayByPlay(payload) {
    const current = document.querySelector("[data-playbyplay-page]");

    if (!current) {
        return;
    }

    const league = current.dataset.league;
    const gameId = current.dataset.gameId;

    if (!league || !gameId) {
        return;
    }

    if (league.toLowerCase() !== payload.league.toLowerCase()) {
        return;
    }

    const activeFilter = document.querySelector("[data-play-filter].active")?.dataset.playFilter ?? "all";

    const response = await fetch(`/game/playbyplay/content/${encodeURIComponent(league)}/${encodeURIComponent(gameId)}?t=${Date.now()}`, {
        cache: "no-store"
    });
    
    if (!response.ok) {
        console.error(`Unable to Refresh Play-By-Play: ${gameId}`);
        
        return;
    }
    
    const html = await response.text();
    
    current.innerHTML = html;
    
    initializePlayByPlayFilters();    
    restorePlayFilter(activeFilter);
}

async function refreshBoxScore(payload) {
    const current = document.querySelector("[data-game-boxscore-page]");
    
    if (!current) {
        return;
    }
    
    const league = current.dataset.league;
    const gameId = current.dataset.gameId;
    
    if (!league || !gameId) {
        return;
    }
    
    if (league.toLowerCase() !== payload.league.toLowerCase()) {
        return;
    }
    
    const response = await fetch(`/game/boxscore/content/${encodeURIComponent(league)}/${encodeURIComponent(gameId)}?t=${Date.now()}`, {
        cache: "no-store"
    });
    
    if (!response.ok) {
        console.error(`Unable to Refresh Box-Score: ${gameId}`);
        
        return;
    }
    
    const html = await response.text();
    
    current.innerHTML = html;
    
    initializeBoxScoreSorting();
}

async function refreshDashboardLeague(league, id) {
    const current = document.getElementById(id);

    if (!current) {
        return;
    }

    const oldScores = captureScores(current);

    const response = await fetch(`/Dashboard/LeagueSection?league=${league}&t=${Date.now()}`, {
        cache: "no-store"
    });

    if (!response.ok) {
        return;
    }

    const html = await response.text();

    const doc = new DOMParser().parseFromString(html, "text/html");
    const replacement = doc.getElementById(id);

    if (!replacement) {
        return;
    }

    const changedGames = findScoreChanges(oldScores, replacement);

    current.replaceWith(replacement);

    animateScoreChanges(changedGames);
}

async function refreshLeaguePage(league, id) {
    const current = document.getElementById(id);

    if (!current) {
        return;
    }

    const oldScores = captureScores(current);

    const response = await fetch(`/League/GameSections?league=${league}&t=${Date.now()}`, {
        cache: "no-store"
    });

    if (!response.ok) {
        return;
    }

    const html = await response.text();

    const doc = new DOMParser().parseFromString(html, "text/html");
    const replacement = doc.getElementById(id);

    if (!replacement) {
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

function animateScoreChanges(changedGames) {
    changedGames.forEach(change => {
        const card = document.querySelector(
            `[data-game-id="${change.gameId}"]`
        );

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

connection.onreconnected(async () => {
    console.log("Reconnected");

    const gameDetails = document.querySelector("[data-game-details]");

    if (gameDetails) {
        await refreshGameDetails({
            league: gameDetails.dataset.league,
        });
    }
});

start();