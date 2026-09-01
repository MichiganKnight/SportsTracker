let dashboardGameCards = [];
let dashboardRefreshTimer = null;

document.addEventListener("DOMContentLoaded", async () => {
    await refreshDashboardFeatures();
});

document.addEventListener("sportsTracker:favoritesChanged", () => {
    renderFavoritesEmptyState();
    renderFavoriteTeams();
    renderFavoriteGames();

    sportsTrackerFavorites.refreshGameCards();
});

async function refreshDashboardFeatures() {
    const dashboard = document.querySelector(".dashboard-page");

    if (!dashboard) {
        return;
    }

    dashboardGameCards = await loadDashboardGameCards();

    renderFavoritesEmptyState();
    renderFavoriteTeams();
    renderFavoriteGames();
    renderLiveGames();

    sportsTrackerFavorites.refreshGameCards(dashboard);
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

function renderFavoritesEmptyState() {
    const emptyState = document.querySelector("#dashboard-favorites-empty");
    
    if (!emptyState) {
        return;
    }
    
    const hasFavorites = sportsTrackerFavorites.getAll().length > 0;
    
    emptyState.classList.toggle("d-none", hasFavorites);
}

function renderFavoriteTeams() {
    const section = document.querySelector("#dashboard-favorite-teams");
    const container = document.querySelector("#dashboard-favorite-team-list");

    if (!section || !container) {
        return;
    }

    container.replaceChildren();

    const favorites = sportsTrackerFavorites.getAll();

    if (favorites.length === 0) {
        section.classList.add("d-none");

        return;
    }

    favorites
        .sort((a, b) => (a.displayName ?? "").localeCompare(b.displayName ?? ""))
        .forEach(team => {
            container.appendChild(createFavoriteTeamCard(team));
        });

    section.classList.remove("d-none");
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

    const favoriteKeys = new Set(
        favorites.map(favorite =>
            createFavoriteKey(favorite.league, favorite.teamId)
        )
    );

    const favoriteGames = dashboardGameCards.filter(card => {
        const league = card.dataset.league;
        const awayTeamId = card.dataset.awayTeamId;
        const homeTeamId = card.dataset.homeTeamId;

        return (
            favoriteKeys.has(createFavoriteKey(league, awayTeamId)) ||
            favoriteKeys.has(createFavoriteKey(league, homeTeamId))
        );
    });

    if (favoriteGames.length === 0) {
        section.classList.add("d-none");

        return;
    }

    favoriteGames.forEach(card => {
        container.appendChild(createDashboardGameColumn(card));
    });

    sportsTrackerFavorites.refreshGameCards(container);
    section.classList.remove("d-none");
}

function renderLiveGames() {
    const section = document.querySelector("#dashboard-live");
    const container = document.querySelector("#dashboard-live-games");

    if (!section || !container) {
        return;
    }

    container.replaceChildren();

    const liveGames = dashboardGameCards.filter(card =>
        card.dataset.isLive === "true"
    );

    if (liveGames.length === 0) {
        section.classList.add("d-none");

        return;
    }

    liveGames.forEach(card => {
        container.appendChild(createDashboardGameColumn(card));
    });

    sportsTrackerFavorites.refreshGameCards(container);

    section.classList.remove("d-none");
}

function getVisibleDashboardGameCards() {
    const leagueContainer = document.querySelector("#dashboard-leagues");

    if (!leagueContainer) {
        return [];
    }

    return Array.from(
        leagueContainer.querySelectorAll("[data-game-card]")
    );
}

function createFavoriteTeamCard(team) {
    const card = document.createElement("div");
    card.className = "dashboard-favorite-team";

    const link = document.createElement("a");

    link.className = "dashboard-favorite-team-link";
    link.href = `/team/${encodeURIComponent(team.league)}/${encodeURIComponent(team.teamId)}`;

    if (team.logo) {
        const logo = document.createElement("img");

        logo.src = team.logo;
        logo.alt = team.displayName ?? "Favorite team";
        logo.className = "dashboard-favorite-team-logo";
        link.appendChild(logo);
    }

    const info = document.createElement("div");

    info.className = "dashboard-favorite-team-info";

    const name = document.createElement("div");

    name.className = "dashboard-favorite-team-name";
    name.textContent = team.displayName ?? "Team";

    const league = document.createElement("div");

    league.className = "dashboard-favorite-team-league";
    league.textContent = [team.abbreviation, team.league].filter(Boolean).join(" · ");

    info.appendChild(name);
    info.appendChild(league);

    link.appendChild(info);
    card.appendChild(link);

    const removeButton = document.createElement("button");
    
    removeButton.type = "button";
    removeButton.className = "dashboard-favorite-team-remove";
    removeButton.title = "Remove from Favorites";
    
    removeButton.setAttribute(
        "aria-label",
        `Remove ${team.displayName ?? "team"} from Favorites`
    );
    
    removeButton.innerHTML = '<i class="bi bi-star-fill"></i>';

    removeButton.addEventListener("click", () => {
        sportsTrackerFavorites.remove(team.league, team.teamId);
    });

    card.appendChild(removeButton);

    return card;
}

function createDashboardGameColumn(card) {
    const column = document.createElement("div");
    
    column.className = "col-12 col-xl-6 col-xxl-4";

    const clone = card.cloneNode(true);
    
    column.appendChild(clone);
    
    sportsTrackerFavorites.refreshGameCards(clone);

    return column;
}

function createFavoriteKey(league, teamId) {
    return `${String(league ?? "").toLowerCase()}:${String(teamId ?? "")}`;
}

function scheduleDashboardFeaturesRefresh() {
    clearTimeout(dashboardRefreshTimer);

    dashboardRefreshTimer = setTimeout(async () => {
        await refreshDashboardFeatures();
    }, 300);
}