let dashboardGameCards = [];

document.addEventListener("DOMContentLoaded", async () => {
   await initializeDashboard(); 
});

document.addEventListener("sportsTracker:favoritesChanged", () => {
    renderFavoriteGames();
});

async function initializeDashboard() {
    const dashboard = document.querySelector(".dashboard-page");
    
    if (!dashboard) {
        return;
    }
    
    dashboardGameCards = await loadDashboardGameCards();
    
    renderFavoriteGames();
    renderLiveGames();
}

async function loadDashboardGameCards() {
    try {
        const response = await fetch(`/Dashboard/AllGames?t=${Date.now()}`);
        
        if (!response.ok) {
            console.error("Unable to Load Complete Dashboard Scoreboard");

            return getVisibleDashboardGameCards();
        }
        
        const html = await response.text();
        
        const parser = new DOMParser();
        
        const documentResult = parser.parseFromString(html, "text/html");
        
        return Array.from(documentResult.querySelectorAll("[data-game-card]"));
    } catch (error) {
        console.error("Unable to Load Complete Dashboard Scoreboard", error);

        return getVisibleDashboardGameCards();
    }
}

function renderFavoriteGames() {
    const section = document.querySelector("#dashboard-favorites");
    const container = document.querySelector("#dashboard-favorite-games");
    
    if (!section || !container) {
        return;
    }
    
    container.replaceChildren();
    
    const favorites = sportsTrackerFavorites.getAll();
    
    if (favorites.length === 0) {
        section.classList.add("d-none");
        
        return;
    }
    
    const favoriteKeys = new Set(favorites.map(favorite => createFavoriteKey(favorite.league, favorite.teamId)));
    
    const favoriteGames = dashboardGameCards.filter(card => {
        const league = card.dataset.league;
        const awayTeamId = card.dataset.awayTeamId;
        const homeTeamId = card.dataset.homeTeamId;
        
        return (favoriteKeys.has(createFavoriteKey(league, awayTeamId)) || favoriteKeys.has(createFavoriteKey(league, homeTeamId)));
    });
    
    if (favoriteGames.length === 0) {
        section.classList.add("d-none");
        
        return;
    }
    
    favoriteGames.forEach(card => {
        container.appendChild(createDashboardGameColumn(card));
    });
    
    section.classList.remove("d-none");
}

function renderLiveGames() {
    const section = document.querySelector("#dashboard-live");
    const container = document.querySelector("#dashboard-live-games");
    
    if (!section || !container) {
        return;
    }
    
    container.replaceChildren();
    
    const liveGames = dashboardGameCards.filter(card => card.dataset.isLive === "true");
    
    if (liveGames.length === 0) {
        section.classList.add("d-none");
        
        return;
    }
    
    liveGames.forEach(card => {
        container.appendChild(createDashboardGameColumn(card));
    });
    
    section.classList.remove("d-none");
}

function getVisibleDashboardGameCards() {
    const leagueContainer = document.querySelector("#dashboard-leagues");
    
    if (!leagueContainer) {
        return [];
    }
    
    return Array.from(leagueContainer.querySelectorAll("[data-game-card]"));
}

function createDashboardGameColumn(card) {
    const column = document.createElement("div");
    
    column.className = "col-12 col-xl-6 col-xxl-4";
    
    column.appendChild(card.cloneNode(true));
    
    return column;
}

function createFavoriteKey(league, teamId) {
    return `${String(league ?? "").toLowerCase()}:${String(teamId ?? "")}`;
}