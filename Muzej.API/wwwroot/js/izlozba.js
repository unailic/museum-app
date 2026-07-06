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

function getIzlozbaId() {
    const params = new URLSearchParams(window.location.search);
    return params.get("id");
}

function statusKlasa(status) {
    return status.toLowerCase();
}

async function ucitajIzlozbu() {
    const container = document.getElementById("izlozba-detalji");
    const id = getIzlozbaId();

    if (!id) {
        container.innerHTML = `<p class="error">Nije naveden ID izložbe.</p>`;
        return;
    }

    try {
        const izlozba = await apiFetch(`/Izlozbe/${id}`);

        const delaHtml = izlozba.naziviDela.length > 0
            ? `<ul>${izlozba.naziviDela.map(n => `<li>${n}</li>`).join("")}</ul>`
            : `<p>Trenutno nema dela dodeljenih ovoj izložbi.</p>`;

        container.innerHTML = `
            <div class="card">
                <h2>${izlozba.naziv}</h2>
                <p class="card-opis">${izlozba.opis}</p>
                <div class="card-info">
                    <p><strong>Period:</strong> ${new Date(izlozba.datumPocetka).toLocaleDateString("sr-RS")} - ${new Date(izlozba.datumZavrsetka).toLocaleDateString("sr-RS")}</p>
                    <p><strong>Cena:</strong> ${izlozba.cena} din</p>
                    <p><strong>Slobodna mesta:</strong> ${izlozba.brojSlobodnihKarata}</p>
                </div>
                <span class="status ${statusKlasa(izlozba.status)}">${izlozba.status}</span>
                <h3 style="margin-top: 1.25rem;">Dela na izložbi</h3>
                ${delaHtml}
            </div>
        `;

        if (isLoggedIn() && !isAdmin()) {
            document.getElementById("kupovina-sekcija").style.display = "block";
        } else if (!isLoggedIn()) {
            document.getElementById("prijava-poruka").style.display = "block";
        }
    } catch (err) {
        container.innerHTML = `<p class="error">${err.message}</p>`;
    }
}

document.getElementById("kupovina-form").addEventListener("submit", async (e) => {
    e.preventDefault();
    const messageEl = document.getElementById("kupovina-message");
    messageEl.textContent = "";
    messageEl.className = "";

    const brojKarata = parseInt(document.getElementById("broj-karata").value);
    const tipPosetioca = parseInt(document.getElementById("tip-posetioca").value);
    const izlozbaId = parseInt(getIzlozbaId());

    try {
        const ids = await apiFetch("/Ulaznice", {
            method: "POST",
            body: JSON.stringify({ brojKarata, tipPosetioca, izlozbaId })
        });

        messageEl.textContent = `Uspešno ste kupili ${ids.length} kart(u/e). Pogledajte "Moje karte" za detalje.`;
        messageEl.className = "success";
        ucitajIzlozbu();
    } catch (err) {
        messageEl.textContent = err.message;
        messageEl.className = "error";
    }
});

podesiNavigaciju();
ucitajIzlozbu();