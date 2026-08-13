document.addEventListener("DOMContentLoaded", function () {
    initializePlayByPlayFilters();
});

function initializePlayByPlayFilters() {
    const buttons = document.querySelectorAll("[data-play-filter]");
    
    if (buttons.length === 0) {
        return;
    }
    
    buttons.forEach(button => {
        button.addEventListener("click", () => {
            const filter = button.dataset.playFilter ?? "all";
            
            setActivePlayFilter(buttons, button);
            filterPlays(filter);
        });
    });
}

function setActivePlayFilter(buttons, activeButton) {
    buttons.forEach(button => {
        const active = button === activeButton;
        
        button.classList.toggle("btn-primary", active);        
        button.classList.toggle("btn-outline-secondary", !active);
        
        button.classList.toggle("active", active);
    });
}

function filterPlays(filter) {
    const plays = document.querySelectorAll("[data-game-play]");
    
    const normalizedFilter = filter.toLowerCase();
    
    plays.forEach(play => {
        const categories = (play.dataset.playCategory ?? "other").toLowerCase().split(/\s+/).filter(Boolean);
        
        const visible = normalizedFilter === "all" || categories.includes(normalizedFilter);
        
        play.classList.toggle("play-hidden", !visible);
    });
    
    updateVisiblePeriods();
}

function updateVisiblePeriods() {
    const periods = document.querySelectorAll("[data-play-period]");
    
    periods.forEach(period => {
        const visiblePlay = period.querySelector("[data-game-play]:not(.play-hidden)");
        
        period.classList.toggle("play-period-hidden", !visiblePlay);
    });
}