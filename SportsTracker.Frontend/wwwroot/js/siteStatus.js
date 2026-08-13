document.addEventListener("DOMContentLoaded", () => {
    initializeSiteStatus();
});

async function initializeSiteStatus() {
    await updateBackendStatus();

    updateLastRefresh();
}

async function updateBackendStatus() {
    const status = document.querySelector("[data-backend-status]");
    const dot = document.querySelector("[data-backend-status-dot]");

    if (!status) {
        return;
    }

    try {
        const response = await fetch("/health", {
            cache: "no-store"
        });
        
        const result = await response.json();        
        const healthy = response.ok && result.healthy;
        
        status.textContent = healthy ? "Online" : "Offline";
        
        if (dot) {
            dot.classList.toggle("backend-status-online", healthy);
            
            dot.classList.toggle("backend-status-offline", !healthy);
        }
    } catch {
        status.textContent = "Offline";
        
        if (dot) {
            dot.classList.remove("backend-status-online");
            
            dot.classList.add("backend-status-offline");
        }
    }
}

function updateLastRefresh(date = new Date()) {
    const element = document.querySelector("[data-last-refresh]");
    
    if (!element) {
        return;
    }
    
    element.textContent = date.toLocaleString([], {
        hour: "numeric",
        minute: "2-digit",
        second: "2-digit"
    });
}