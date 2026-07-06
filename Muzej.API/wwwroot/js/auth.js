document.getElementById("idi-na-registraciju").addEventListener("click", (e) => {
    e.preventDefault();
    document.getElementById("login-panel").style.display = "none";
    document.getElementById("register-panel").style.display = "block";
});

document.getElementById("idi-na-prijavu").addEventListener("click", (e) => {
    e.preventDefault();
    document.getElementById("register-panel").style.display = "none";
    document.getElementById("login-panel").style.display = "block";
});

document.getElementById("login-form").addEventListener("submit", async (e) => {
    e.preventDefault();
    const messageEl = document.getElementById("login-message");
    messageEl.textContent = "";
    messageEl.className = "";

    const email = document.getElementById("login-email").value;
    const password = document.getElementById("login-password").value;

    try {
        const result = await apiFetch("/Auth/login", {
            method: "POST",
            body: JSON.stringify({ email, password })
        });

        setToken(result.token);
        window.location.href = "katalog.html";
    } catch (err) {
        messageEl.textContent = err.message;
        messageEl.className = "error";
    }
});

document.getElementById("register-form").addEventListener("submit", async (e) => {
    e.preventDefault();
    const messageEl = document.getElementById("register-message");
    messageEl.textContent = "";
    messageEl.className = "";

    const ime = document.getElementById("reg-ime").value;
    const prezime = document.getElementById("reg-prezime").value;
    const email = document.getElementById("reg-email").value;
    const password = document.getElementById("reg-password").value;
    const tipPosetioca = parseInt(document.getElementById("reg-tip").value);

    try {
        const result = await apiFetch("/Auth/register", {
            method: "POST",
            body: JSON.stringify({ ime, prezime, email, password, tipPosetioca })
        });

        setToken(result.token);
        window.location.href = "katalog.html";
    } catch (err) {
        messageEl.textContent = err.message;
        messageEl.className = "error";
    }
});

if (isLoggedIn()) {
    window.location.href = "katalog.html";
}