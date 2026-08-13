async function partialRefresh({selector, url, replacementSelector = selector, beforeReplace = null, afterReplace = null}) {
    const current = document.querySelector(selector);
    
    if (!current) {
        return false;
    }
    
    const state = beforeReplace ? beforeReplace(current) : null;
    
    const response = await fetch(url, {
        cache: "no-store"
    });
    
    if (!response.ok) {
        console.error(`Unable to Refresh ${selector}: ${response.status}`);
        
        return false;
    }
    
    const html = await response.text();
    
    const documentFragment = new DOMParser().parseFromString(html, "text/html");
    
    const replacement = documentFragment.querySelector(replacementSelector);
    
    current.replaceWith(replacement);
    
    if (afterReplace) {
        afterReplace(replacement, state);
    }
    
    return true;
}