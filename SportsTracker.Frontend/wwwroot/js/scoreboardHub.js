const connection = new signalR.HubConnectionBuilder()
    .withUrl(window.sportsTracker.scoreboardHub)
    .withAutomaticReconnect()
    .build();

connection.on("ScoreboardUpdated", async payload => {
    await handleScoreboardUpdated(payload)
});

async function handleScoreboardUpdated(payload) {
    await Promise.all([
        refreshDashboardLeague(payload),
        refreshLeaguePage(payload),
        refreshGameDetails(payload),
        refreshBoxScore(payload),
        refreshPlayByPlay(payload)
    ]);
}

async function refreshDashboardLeague(payload) {
    const league = payload.league;
    const leagueKey = league.toLowerCase();
    const selector = `#league-${leagueKey}`;
    const current = document.querySelector(selector);

    if (!current) {
        return;
    }

    const oldScores = captureScores(current);

    await refreshPartial({
        selector: selector,
        url: `/Dashboard/LeagueSection?league=${encodeURIComponent(league)}&t=${Date.now()}`,
        afterReplace: replacement => {
            const changedGames = findScoreChanges(oldScores, replacement);

            animateScoreChanges(changedGames);
        }
    });
}

async function refreshLeaguePage(payload) {
    const league = payload.league;
    const leagueKey = league.toLowerCase();
    const selector = `#league-games-${leagueKey}`;
    const current = document.querySelector(selector);

    if (!current) {
        return;
    }

    const oldScores = captureScores(current);

    await partialRefresh({
        selector: selector,
        url: `/league/GameSections?league=${encodeURIComponent(league)}&t=${Date.now()}`,
        afterReplace: replacement => {
            const changedGames = findScoreChanges(oldScores, replacement);

            animateScoreChanges(changedGames);
        }
    });
}

async function refreshGameDetails(payload) {
    const page = document.querySelector("[data-game-summary-page]");

    if (!page) {
        return;
    }

    const league = page.dataset.league;
    const gameId = page.dataset.gameId;

    if (!isMatchingLeague(league, payload.league)) {
        return;
    }

    await partialRefresh({
        selector: "[data-game-summary-page]",
        url: `/game/content/${encodeURIComponent(league)}/${encodeURIComponent(gameId)}?t=${Date.now()}`,
    });
}

async function refreshBoxScore(payload) {
    const page = document.querySelector("[data-game-boxscore-page]");

    if (!page) {
        return;
    }

    const league = page.dataset.league;
    const gameId = page.dataset.gameId;

    if (!isMatchingLeague(league, payload.league)) {
        return;
    }

    await partialRefresh({
        selector: "[data-game-boxscore-page]",
        url: `/game/boxscore/content/${encodeURIComponent(league)}/${encodeURIComponent(gameId)}?t=${Date.now()}`,
        afterReplace: () => {
            initializeBoxScoreSorting();
        }
    })
}

async function refreshPlayByPlay(payload) {
    const page = document.querySelector("[data-game-playbyplay-page]");

    if (!page) {
        return;
    }

    const league = page.dataset.league;
    const gameId = page.dataset.gameId;

    if (!isMatchingLeague(league, payload.league)) {
        return;
    }

    await partialRefresh({
        selector: "[data-game-playbyplay-page]",
        url: `/game/playbyplay/content/${encodeURIComponent(league)}/${encodeURIComponent(gameId)}?t=${Date.now()}`,
        beforeReplace: () => ({
            filter: document.querySelector("[data-play-filter].active")?.dataset.playFilter ?? "all",
            followLatest: isNearLatestPlay(),
            playIds: getPlayIds()
        }),
        afterReplace: (replacement, state) => {
            initializePlayByPlayFilters();
            
            restorePlayFilter(state.filter);
            
            highlightNewPlays(state.playIds);
            
            if (state.followLatest) {
                scrollToLatestPlay();
            }
        }
    });
}

function isMatchingLeague(pageLeague, updatedLeague) {
    if (!pageLeague || !updatedLeague) {
        return false;
    }
    
    return pageLeague.toLowerCase() === updatedLeague.toLowerCase();
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

    const gameDetails = document.querySelector("[data-game-summary-page]");

    if (gameDetails) {
        await refreshGameDetails({
            league: gameDetails.dataset.league,
        });
    }
});

start();