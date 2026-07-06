// --- Navigacija ---
function podesiNavigaciju() {
    const ulogovan = isLoggedIn();
    const administrator = ulogovan && isAdmin();

    document.getElementById("nav-login").style.display = ulogovan ? "none" : "inline";
    document.getElementById("nav-logout").style.display = ulogovan ? "inline" : "none";
    document.getElementById("nav-moje-karte").style.display = (ulogovan && !administrator) ? "inline" : "none";

    const adminLink = document.getElementById("nav-admin");
    if (adminLink) {
        adminLink.style.display = administrator ? "inline" : "none";
    }
}

document.getElementById("nav-logout").addEventListener("click", (e) => {
    e.preventDefault();
    clearToken();
    window.location.href = "index.html";
});

function statusKlasa(status) {
    return status.toLowerCase();
}

// --- Renderovanje Izložbi ---
async function ucitajIzlozbe() {
    const container = document.getElementById("izlozbe-lista");
    try {
        const izlozbe = await apiFetch("/Izlozbe");
        if (izlozbe.length === 0) {
            container.innerHTML = "<p>Trenutno nema unetih izložbi.</p>";
            return;
        }
        container.innerHTML = izlozbe.map(i => `
            <div class="card">
                <h3>${i.naziv}</h3>
                <p class="card-opis">${i.opis}</p>
                <div class="card-info">
                    <p><strong>Period:</strong> ${new Date(i.datumPocetka).toLocaleDateString("sr-RS")} - ${new Date(i.datumZavrsetka).toLocaleDateString("sr-RS")}</p>
                    <p><strong>Cena:</strong> ${i.cena} din</p>
                    <p><strong>Slobodna mesta:</strong> ${i.brojSlobodnihKarata}</p>
                </div>
                <span class="status ${statusKlasa(i.status)}">${i.status}</span>
                <div class="card-actions">
                    <a href="izlozba.html?id=${i.id}"><button class="secondary">Detalji i kupovina</button></a>
                </div>
            </div>
        `).join("");
    } catch (err) {
        container.innerHTML = `<p class="error">${err.message}</p>`;
    }
}

// --- Renderovanje Umetničkih dela ---
async function ucitajDela() {
    const container = document.getElementById("dela-lista");
    try {
        const dela = await apiFetch("/UmetnickaDela");
        if (dela.length === 0) {
            container.innerHTML = "<p>Trenutno nema unetih dela.</p>";
            return;
        }
        container.innerHTML = dela.map(d => `
                    <div class="card card-horizontal">
                        <div class="card-text">
                            <h3>${d.naziv}</h3>
                            <p class="card-opis">${d.opis}</p>
                            <div class="card-info">
                                <p><strong>Autor:</strong> ${d.autorImePrezime}</p>
                                <p><strong>Godina:</strong> ${d.godinaNastanka}</p>
                                ${d.tehnika ? `<p><strong>Tehnika:</strong> ${d.tehnika}</p>` : ""}
                                ${d.dimenzije ? `<p><strong>Dimenzije:</strong> ${d.dimenzije}</p>` : ""}
                                ${d.materijal ? `<p><strong>Materijal:</strong> ${d.materijal}</p>` : ""}
                                ${d.visina ? `<p><strong>Visina:</strong> ${d.visina} cm</p>` : ""}
                            </div>
                        </div>
                        ${d.imgUrl ? `<img src="${d.imgUrl}" alt="${d.naziv}" class="card-img" onerror="this.style.display='none'">` : ""}
                    </div>
                `).join("");
    } catch (err) {
        container.innerHTML = `<p class="error">${err.message}</p>`;
    }
}

// --- Renderovanje Autora ---
async function ucitajAutore() {
    const container = document.getElementById("autori-lista");
    try {
        const autori = await apiFetch("/Autori");
        if (autori.length === 0) {
            container.innerHTML = "<p>Trenutno nema unetih autora.</p>";
            return;
        }
        container.innerHTML = autori.map(a => `
            <div class="card">
                <h3>${a.ime} ${a.prezime}</h3>
                <p class="card-opis">${a.biografija}</p>
                <div class="card-info">
                    <p><strong>Godina rođenja:</strong> ${a.godinaRodjenja}</p>
                    <p><strong>Broj dela u katalogu:</strong> ${a.brojDela}</p>
                </div>
            </div>
        `).join("");
    } catch (err) {
        container.innerHTML = `<p class="error">${err.message}</p>`;
    }
}

document.querySelectorAll(".tab-btn").forEach(btn => {
    btn.addEventListener("click", () => {
        document.querySelectorAll(".tab-btn").forEach(b => b.classList.remove("active"));
        document.querySelectorAll(".tab-panel").forEach(p => p.style.display = "none");

        btn.classList.add("active");
        document.getElementById("tab-" + btn.dataset.tab).style.display = "block";
    });
});

// --- Inicijalizacija ---
podesiNavigaciju();
ucitajIzlozbe();
ucitajDela();
ucitajAutore();