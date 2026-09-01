const sportsTrackerFavorites = (() => {
    const storageKey = "sportsTracker.favoriteTeams";

    function getAll() {
        const stored = localStorage.getItem(storageKey);

        if (!stored) {
            return [];
        }

        try {
            const parsed = JSON.parse(stored);

            return Array.isArray(parsed) ? parsed : [];
        } catch {
            return [];
        }
    }

    function save(favorites) {
        localStorage.setItem(storageKey, JSON.stringify(favorites));

        document.dispatchEvent(new CustomEvent("sportsTracker:favoritesChanged", {
            detail: {
                favorites
            }
        }));
    }

    function createKey(league, teamId) {
        return `${String(league ?? "").toLowerCase()}:${String(teamId ?? "")}`;
    }

    function isFavorite(league, teamId) {
        const key = createKey(league, teamId);

        return getAll().some(favorite =>
            createKey(favorite.league, favorite.teamId) === key
        );
    }

    function add(team) {
        const favorites = getAll();

        if (isFavorite(team.league, team.teamId)) {
            return;
        }

        favorites.push(team);
        save(favorites);
    }

    function remove(league, teamId) {
        const key = createKey(league, teamId);

        const favorites = getAll().filter(favorite =>
            createKey(favorite.league, favorite.teamId) !== key
        );

        save(favorites);
    }

    function toggle(team) {
        if (isFavorite(team.league, team.teamId)) {
            remove(team.league, team.teamId);
            return false;
        }

        add(team);
        return true;
    }

    function refreshGameCards(root = document) {
        const gameCards = [
            ...(root.matches?.("[data-game-card]") ? [root] : []),
            ...root.querySelectorAll("[data-game-card]")
        ];

        gameCards.forEach(card => {
            const badge = card.querySelector("[data-favorite-game-badge]");

            if (!badge) {
                return;
            }

            const league = card.dataset.league;
            const awayTeamId = card.dataset.awayTeamId;
            const homeTeamId = card.dataset.homeTeamId;

            const hasFavoriteTeam = isFavorite(league, awayTeamId) || isFavorite(league, homeTeamId);

            badge.classList.toggle("d-none", !hasFavoriteTeam);
        });
    }

    return {
        getAll,
        add,
        remove,
        toggle,
        isFavorite,
        refreshGameCards
    };
})();

document.addEventListener("DOMContentLoaded", () => {
    sportsTrackerFavorites.refreshGameCards();
});

document.addEventListener("sportsTracker:favoritesChanged", () => {
    sportsTrackerFavorites.refreshGameCards();
});
