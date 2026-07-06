const API_BASE = "/api";

function getToken() {
    return localStorage.getItem("token");
}

function setToken(token) {
    localStorage.setItem("token", token);
}

function clearToken() {
    localStorage.removeItem("token");
}

function isLoggedIn() {
    return !!getToken();
}

async function apiFetch(url, options = {}) {
    const headers = options.headers || {};
    headers["Content-Type"] = "application/json";

    const token = getToken();
    if (token) headers["Authorization"] = "Bearer " + token;

    const response = await fetch(API_BASE + url, { ...options, headers });

    // LOGOVANJE DA VIDIMO ŠTA SE DEŠAVA
    console.log(`API poziv: ${url}, Status: ${response.status}`);

    if (response.status === 401) {
        clearToken();
        window.location.href = "index.html";
        return null;
    }

    // Provera uspeha
    if (response.ok) {
        // Ako je 204 ili 201, odmah vraćamo null
        if (response.status === 204 || response.status === 205 || response.status === 201) {
            return null;
        }

        const text = await response.text();
        return text ? JSON.parse(text) : null;
    } else {
        // AKO NIJE OK, POKUŠAJ DA UHVATIŠ ŠTA JE SERVER VRATIO
        const text = await response.text();
        console.error("Greška sa servera:", text); // OVO ĆE TI PISATI U KONZOLI

        let errorBody;
        try { errorBody = JSON.parse(text); } catch (e) { errorBody = { poruka: text || "Nepoznata greška" }; }

        throw new Error(errorBody?.poruka || errorBody?.greske?.[0]?.poruka || "Greška pri komunikaciji sa serverom.");
    }
}
function requireLogin() {
    if (!isLoggedIn()) {
        window.location.href = "index.html";
    }
}

function decodeToken(token) {
    try {
        const payload = token.split(".")[1];
        const json = atob(payload.replace(/-/g, "+").replace(/_/g, "/"));
        return JSON.parse(json);
    } catch {
        return null;
    }
}

function getUserRole() {
    const token = getToken();
    if (!token) return null;
    const data = decodeToken(token);
    return data ? data["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] : null;
}

function getUserId() {
    const token = getToken();
    if (!token) return null;
    const data = decodeToken(token);
    return data ? data["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"] : null;
}

function isAdmin() {
    return getUserRole() === "Administrator";
}