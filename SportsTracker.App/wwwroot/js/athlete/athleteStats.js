document.addEventListener("click", event => {
    const button = event.target.closest("[data-athlete-stats-category]");

    if (!button) {
        return;
    }

    const container = button.closest("[data-athlete-stats]");

    if (!container) {
        return;
    }

    const category = button.dataset.athleteStatsCategory;

    container
        .querySelectorAll("[data-athlete-stats-category]").forEach(item => {
        const isActive = item.dataset.athleteStatsCategory === category;

        item.classList.toggle("btn-primary", isActive);
        item.classList.toggle("btn-outline-secondary", !isActive);
    });

    container.querySelectorAll("[data-athlete-stats-category-panel]").forEach(panel => {
        const isActive = panel.dataset.athleteStatsCategoryPanel === category;

        panel.classList.toggle("d-none", !isActive);
    });
});