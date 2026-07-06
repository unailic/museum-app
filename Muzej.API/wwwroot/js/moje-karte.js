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

async function ucitajKarte() {
    const container = document.getElementById("karte-lista");
    try {
        const karte = await apiFetch("/Ulaznice/moje");

        if (karte.length === 0) {
            container.innerHTML = "<p>Nemate kupljenih karata.</p>";
            return;
        }

        container.innerHTML = karte.map(k => `
            <div class="card">
                <h3>${k.nazivIzlozbe}</h3>
                <div class="card-info">
                    <p><strong>Datum posete:</strong> ${new Date(k.datumPosete).toLocaleDateString("sr-RS")}</p>
                    ${k.datumKupovine ? `<p><strong>Datum kupovine:</strong> ${new Date(k.datumKupovine).toLocaleDateString("sr-RS")}</p>` : ""}
                    ${k.cenaPlacena ? `<p><strong>Plaćeno:</strong> ${k.cenaPlacena} din</p>` : ""}
                </div>
                <span class="status ${statusKlasa(k.status)}">${k.status}</span>
                ${k.status === "Kupljena" ? `
                    <div class="card-actions">
                        <button class="danger" onclick="otkaziKartu(${k.id})">Otkaži kartu</button>
                    </div>
                ` : ""}
            </div>
        `).join("");
    } catch (err) {
        container.innerHTML = `<p class="error">${err.message}</p>`;
    }
}

async function otkaziKartu(id) {
    if (!confirm("Da li ste sigurni da želite da otkažete ovu kartu?")) {
        return;
    }

    try {
        await apiFetch(`/Ulaznice/${id}/otkazi`, { method: "PUT" });
        ucitajKarte();
    } catch (err) {
        alert(err.message);
    }
}

requireLogin();
podesiNavigaciju();
ucitajKarte();