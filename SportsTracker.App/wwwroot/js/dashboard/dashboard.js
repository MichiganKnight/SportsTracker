let dashboardGameCards = [];
let dashboardRefreshTimer = null;

document.addEventListener("DOMContentLoaded", async () => {
    await refreshDashboardFeatures();
});

document.addEventListener("sportsTracker:favoritesChanged", async () => {
    await refreshDashboardFeatures();
});

async function refreshDashboardFeatures() {
    const dashboard = document.querySelector(".dashboard-page");

    if (!dashboard) {
        return;
    }

    dashboardGameCards = await loadDashboardGameCards();

    renderFavoritesEmptyState();
    renderFavoriteTeams();
    renderFavoriteAthletes();
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

    const hasFavoriteTeams = sportsTrackerFavorites.teams.getAll().length > 0;
    const hasFavoriteAthletes = sportsTrackerFavorites.athletes.getAll().length > 0;

    emptyState.classList.toggle("d-none", hasFavoriteTeams || hasFavoriteAthletes);
}

function renderFavoriteAthletes() {
    const section = document.querySelector("#dashboard-favorite-athletes");
    const container = document.querySelector("#dashboard-favorite-athlete-list");

    if (!section || !container) {
        return;
    }

    container.replaceChildren();

    const favorites = sportsTrackerFavorites.athletes.getAll();

    if (favorites.length === 0) {
        section.classList.add("d-none");

        return;
    }

    favorites.sort((a, b) => (a.displayName ?? "").localeCompare(b.displayName ?? "")).forEach(athlete => {
        container.appendChild(createFavoriteAthleteCard(athlete));
    });

    section.classList.remove("d-none");
}

function renderFavoriteTeams() {
    const section = document.querySelector("#dashboard-favorite-teams");
    const container = document.querySelector("#dashboard-favorite-team-list");

    if (!section || !container) {
        return;
    }

    container.replaceChildren();

    const favorites = sportsTrackerFavorites.teams.getAll();

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

    const favorites = sportsTrackerFavorites.teams.getAll();

    if (favorites.length === 0) {
        section.classList.add("d-none");

        return;
    }

    const favoriteGames = dashboardGameCards.filter(card => {
        const league = card.dataset.league;
        const awayTeamId = card.dataset.awayTeamId;
        const homeTeamId = card.dataset.homeTeamId;

        return (sportsTrackerFavorites.teams.isFavorite(league, awayTeamId) || sportsTrackerFavorites.teams.isFavorite(league, homeTeamId));
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

function createFavoriteAthleteCard(athlete) {
    const card = document.createElement("div");
    card.className = "dashboard-favorite-athlete";

    const link = document.createElement("a");

    link.className = "dashboard-favorite-athlete-link";
    link.href = `/athlete/${encodeURIComponent(athlete.league)}/${encodeURIComponent(athlete.athleteId)}`;

    const imageWrapper = document.createElement("div");
    imageWrapper.className = "dashboard-favorite-athlete-image-wrap";

    if (athlete.headshot) {
        const image = document.createElement("img");

        image.src = athlete.headshot;
        image.alt = athlete.displayName ?? "Favorite Player";
        image.className = "dashboard-favorite-athlete-image";
        imageWrapper.appendChild(image);
    } else {
        const placeholder = document.createElement("div");

        placeholder.className = "dashboard-favorite-athlete-placeholder";
        placeholder.innerHTML = '<i class="bi bi-person-fill"></i>';

        imageWrapper.appendChild(placeholder);
    }

    const info = document.createElement("div");

    info.className = "dashboard-favorite-athlete-info";

    const name = document.createElement("div");

    name.className = "dashboard-favorite-athlete-name";
    name.textContent = athlete.displayName ?? "Player";

    const details = document.createElement("div");

    details.className = "dashboard-favorite-athlete-details";

    details.textContent = getFavoriteAthleteDetails(athlete);

    info.appendChild(name);

    if (details.textContent) {
        info.appendChild(details);
    }

    link.appendChild(imageWrapper);
    link.appendChild(info);

    card.appendChild(link);

    const removeButton = document.createElement("button");
    removeButton.type = "button";
    removeButton.className = "dashboard-favorite-athlete-remove";
    removeButton.title = "Remove from Favorites";

    removeButton.setAttribute("aria-label", `Remove ${athlete.displayName ?? "Player"} from Favorites`);

    removeButton.innerHTML = '<i class="bi bi-star-fill"></i>';

    removeButton.addEventListener("click", () => {
        sportsTrackerFavorites.athletes.remove(athlete.league, athlete.athleteId);
    });

    card.appendChild(removeButton);

    return card;
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
        sportsTrackerFavorites.teams.remove(team.league, team.teamId);
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

function getFavoriteAthleteDetails(athlete) {
    const details = [];

    if (athlete.position) {
        details.push(athlete.position);
    }

    if (athlete.teamName) {
        details.push(athlete.teamName);
    } else if (athlete.citizenship) {
        details.push(athlete.citizenship);
    }

    if (athlete.league) {
        details.push(athlete.league);
    }

    return details.join(" · ");
}

function scheduleDashboardFeaturesRefresh() {
    clearTimeout(dashboardRefreshTimer);

    dashboardRefreshTimer = setTimeout(async () => {
        await refreshDashboardFeatures();
    }, 300);
}