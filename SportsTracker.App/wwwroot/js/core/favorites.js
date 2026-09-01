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
    
    function isFavorite(league, teamId) {
        return getAll().some(favorite => favorite.league === league && favorite.teamId === teamId);
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
        const favorites = getAll().filter(favorite => !(favorite.league === league && favorite.teamId === teamId));
        
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
    
    return {
        getAll,
        add,
        remove,
        toggle,
        isFavorite
    }
})();