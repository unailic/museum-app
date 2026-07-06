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

if (!isLoggedIn() || !isAdmin()) {
    window.location.href = "katalog.html";
}

document.querySelectorAll(".tab-btn").forEach(btn => {
    btn.addEventListener("click", () => {
        document.querySelectorAll(".tab-btn").forEach(b => b.classList.remove("active"));
        document.querySelectorAll(".tab-panel").forEach(p => p.style.display = "none");
        btn.classList.add("active");
        document.getElementById("tab-" + btn.dataset.tab).style.display = "block";

        if (btn.dataset.tab === "admin-karte") {
            ucitajKarteAdmin();
        }
    });
});

// ==================== AUTORI ====================
let autoriKes = [];

async function ucitajAutoreAdmin() {
    const container = document.getElementById("autori-admin-lista");
    try {
        const autori = await apiFetch("/Autori");
        autoriKes = autori;
        container.innerHTML = autori.map(a => `
            <div class="card">
                <h3>${a.ime} ${a.prezime}</h3>
                <p class="card-opis">${a.biografija}</p>
                <div class="card-info">
                    <p><strong>Godina rođenja:</strong> ${a.godinaRodjenja}</p>
                    <p><strong>Broj dela:</strong> ${a.brojDela}</p>
                </div>
                <div class="card-actions">
                    <button class="secondary" onclick="izmeniAutora(${a.id})">Izmeni</button>
                    <button class="danger" onclick="obrisiAutora(${a.id})">Obriši</button>
                </div>
            </div>
        `).join("");
        popuniSelektorAutora(autori);
    } catch (err) {
        container.innerHTML = `<p class="error">${err.message}</p>`;
    }
}

function popuniSelektorAutora(autori) {
    const select = document.getElementById("delo-autor");
    select.innerHTML = autori.map(a => `<option value="${a.id}">${a.ime} ${a.prezime}</option>`).join("");
}

function izmeniAutora(id) {
    const autor = autoriKes.find(a => a.id === id);
    if (!autor) return;
    document.getElementById("autor-id").value = autor.id;
    document.getElementById("autor-ime").value = autor.ime;
    document.getElementById("autor-prezime").value = autor.prezime;
    document.getElementById("autor-biografija").value = autor.biografija;
    document.getElementById("autor-godina").value = autor.godinaRodjenja;
    document.getElementById("autor-form-naslov").textContent = "Izmena autora";
    document.getElementById("autor-submit-btn").textContent = "Sačuvaj izmene";
    document.getElementById("autor-cancel-btn").style.display = "inline-block";
    window.scrollTo(0, 0);
}

document.getElementById("autor-cancel-btn").addEventListener("click", () => resetujAutorForm());

function resetujAutorForm() {
    document.getElementById("autor-form").reset();
    document.getElementById("autor-id").value = "";
    document.getElementById("autor-form-naslov").textContent = "Dodaj novog autora";
    document.getElementById("autor-submit-btn").textContent = "Dodaj autora";
    document.getElementById("autor-cancel-btn").style.display = "none";
}

async function obrisiAutora(id) {
    if (!confirm("Da li ste sigurni da želite da obrišete ovog autora?")) return;
    try {
        await apiFetch(`/Autori/${id}`, { method: "DELETE" });
        ucitajAutoreAdmin();
    } catch (err) { alert(err.message); }
}

document.getElementById("autor-form").addEventListener("submit", async (e) => {
    e.preventDefault();
    const btn = document.getElementById("autor-submit-btn");
    const messageEl = document.getElementById("autor-message");

    btn.disabled = true; // SPREČAVA DUPLIRANJE
    messageEl.textContent = "Obrada...";

    const id = document.getElementById("autor-id").value;
    const body = {
        ime: document.getElementById("autor-ime").value,
        prezime: document.getElementById("autor-prezime").value,
        biografija: document.getElementById("autor-biografija").value,
        godinaRodjenja: parseInt(document.getElementById("autor-godina").value)
    };

    try {
        if (id) {
            body.id = parseInt(id);
            await apiFetch(`/Autori/${id}`, { method: "PUT", body: JSON.stringify(body) });
            messageEl.textContent = "Autor je uspešno izmenjen.";
        } else {
            await apiFetch("/Autori", { method: "POST", body: JSON.stringify(body) });
            messageEl.textContent = "Autor je uspešno dodat.";
        }
        messageEl.className = "success";
        resetujAutorForm();
        ucitajAutoreAdmin();
    } catch (err) {
        messageEl.textContent = err.message;
        messageEl.className = "error";
    } finally {
        btn.disabled = false;
    }
});

// ==================== UMETNIČKA DELA ====================
let delaKes = [];

document.getElementById("delo-tip").addEventListener("change", (e) => {
    const jeSlika = e.target.value === "0";
    document.getElementById("delo-slika-polja").style.display = jeSlika ? "block" : "none";
    document.getElementById("delo-skulptura-polja").style.display = jeSlika ? "none" : "block";
});

async function ucitajDelaAdmin() {
    const container = document.getElementById("dela-admin-lista");
    try {
        const dela = await apiFetch("/UmetnickaDela");
        delaKes = dela;
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
                    <div class="card-actions">
                        <button class="secondary" onclick="izmeniDelo(${d.id})">Izmeni</button>
                        <button class="danger" onclick="obrisiDelo(${d.id})">Obriši</button>
                    </div>
                </div>
                ${d.imgUrl ? `<img src="${d.imgUrl}" alt="${d.naziv}" class="card-img" onerror="this.style.display='none'">` : ""}
            </div>
        `).join("");
    } catch (err) { container.innerHTML = `<p class="error">${err.message}</p>`; }
}

function izmeniDelo(id) {
    const delo = delaKes.find(d => d.id === id);
    if (!delo) return;
    document.getElementById("delo-id").value = delo.id;
    document.getElementById("delo-naziv").value = delo.naziv;
    document.getElementById("delo-godina").value = delo.godinaNastanka;
    document.getElementById("delo-opis").value = delo.opis;
    document.getElementById("delo-img").value = delo.imgUrl || "";
    const autorSelect = document.getElementById("delo-autor");
    const autor = autoriKes.find(a => `${a.ime} ${a.prezime}` === delo.autorImePrezime);
    if (autor) autorSelect.value = autor.id;
    const jeSlika = delo.tip === "Slika";
    document.getElementById("delo-tip").value = jeSlika ? "0" : "1";
    document.getElementById("delo-slika-polja").style.display = jeSlika ? "block" : "none";
    document.getElementById("delo-skulptura-polja").style.display = jeSlika ? "none" : "block";
    document.getElementById("delo-tehnika").value = delo.tehnika || "";
    document.getElementById("delo-dimenzije").value = delo.dimenzije || "";
    document.getElementById("delo-materijal").value = delo.materijal || "";
    document.getElementById("delo-visina").value = delo.visina || "";
    document.getElementById("delo-tip").disabled = true;
    document.getElementById("delo-form-naslov").textContent = "Izmena umetničkog dela";
    document.getElementById("delo-submit-btn").textContent = "Sačuvaj izmene";
    document.getElementById("delo-cancel-btn").style.display = "inline-block";
    window.scrollTo(0, 0);
}

document.getElementById("delo-cancel-btn").addEventListener("click", () => resetujDeloForm());

function resetujDeloForm() {
    document.getElementById("delo-form").reset();
    document.getElementById("delo-id").value = "";
    document.getElementById("delo-tip").disabled = false;
    document.getElementById("delo-slika-polja").style.display = "block";
    document.getElementById("delo-skulptura-polja").style.display = "none";
    document.getElementById("delo-form-naslov").textContent = "Dodaj novo delo";
    document.getElementById("delo-submit-btn").textContent = "Dodaj delo";
    document.getElementById("delo-cancel-btn").style.display = "none";
}

async function obrisiDelo(id) {
    if (!confirm("Da li ste sigurni da želite da obrišete ovo delo?")) return;
    try {
        await apiFetch(`/UmetnickaDela/${id}`, { method: "DELETE" });
        ucitajDelaAdmin();
    } catch (err) { alert(err.message); }
}

document.getElementById("delo-form").addEventListener("submit", async (e) => {
    e.preventDefault();
    const btn = document.getElementById("delo-submit-btn");
    const messageEl = document.getElementById("delo-message");

    btn.disabled = true; // SPREČAVA DUPLIRANJE
    const id = document.getElementById("delo-id").value;
    const jeSlika = document.getElementById("delo-tip").value === "0";
    const body = {
        naziv: document.getElementById("delo-naziv").value,
        godinaNastanka: parseInt(document.getElementById("delo-godina").value),
        opis: document.getElementById("delo-opis").value,
        imgUrl: document.getElementById("delo-img").value,
        autorId: parseInt(document.getElementById("delo-autor").value),
        tehnika: jeSlika ? document.getElementById("delo-tehnika").value : null,
        dimenzije: jeSlika ? document.getElementById("delo-dimenzije").value : null,
        materijal: !jeSlika ? document.getElementById("delo-materijal").value : null,
        visina: !jeSlika ? parseFloat(document.getElementById("delo-visina").value) || null : null
    };

    try {
        if (id) {
            body.id = parseInt(id);
            await apiFetch(`/UmetnickaDela/${id}`, { method: "PUT", body: JSON.stringify(body) });
            messageEl.textContent = "Delo je uspešno izmenjeno.";
        } else {
            body.tipDela = jeSlika ? 0 : 1;
            await apiFetch("/UmetnickaDela", { method: "POST", body: JSON.stringify(body) });
            messageEl.textContent = "Delo je uspešno dodato.";
        }
        messageEl.className = "success";
        resetujDeloForm();
        ucitajDelaAdmin();
    } catch (err) {
        messageEl.textContent = err.message;
        messageEl.className = "error";
    } finally {
        btn.disabled = false;
    }
});

// ==================== IZLOŽBE ====================
let izlozbeKes = [];

async function ucitajIzlozbeAdmin() {
    const container = document.getElementById("izlozbe-admin-lista");
    try {
        const izlozbe = await apiFetch("/Izlozbe");
        izlozbeKes = izlozbe;
        container.innerHTML = izlozbe.map(i => `
            <div class="card">
                <h3>${i.naziv}</h3>
                <p class="card-opis">${i.opis}</p>
                <div class="card-info">
                    <p><strong>Period:</strong> ${new Date(i.datumPocetka).toLocaleDateString("sr-RS")} - ${new Date(i.datumZavrsetka).toLocaleDateString("sr-RS")}</p>
                    <p><strong>Cena:</strong> ${i.cena} din</p>
                    <p><strong>Kapacitet:</strong> ${i.kapacitet}</p>
                    <p><strong>Slobodna mesta:</strong> ${i.brojSlobodnihKarata}</p>
                </div>
                <span class="status ${i.status.toLowerCase()}">${i.status}</span>
                <div class="card-actions">
                    <button class="secondary" onclick="izmeniIzlozbu(${i.id})">Izmeni</button>
                    <button class="danger" onclick="obrisiIzlozbu(${i.id})">Obriši</button>
                </div>
                <div style="margin-top: 1rem; border-top: 1px solid var(--line); padding-top: 0.85rem;">
                    <label>Dela na izložbi</label>
                    <div id="dela-na-izlozbi-${i.id}"><p class="card-opis">Učitavanje...</p></div>
                    <label>Dodaj delo na izložbu</label>
                    <select id="dodaj-delo-select-${i.id}"></select>
                    <button type="button" onclick="dodajDeloNaIzlozbu(${i.id})">Dodaj</button>
                </div>
            </div>
        `).join("");
        izlozbe.forEach(i => {
            const select = document.getElementById(`dodaj-delo-select-${i.id}`);
            if (select) select.innerHTML = delaKes.map(d => `<option value="${d.id}">${d.naziv}</option>`).join("");
            ucitajDelaNaIzlozbi(i.id);
        });
    } catch (err) { container.innerHTML = `<p class="error">${err.message}</p>`; }
}

async function ucitajDelaNaIzlozbi(izlozbaId) {
    const container = document.getElementById(`dela-na-izlozbi-${izlozbaId}`);
    if (!container) return;
    try {
        const detalji = await apiFetch(`/Izlozbe/${izlozbaId}`);
        if (!detalji.stavke || detalji.stavke.length === 0) {
            container.innerHTML = `<p class="card-opis">Nema dodatih dela.</p>`;
            return;
        }
        container.innerHTML = detalji.stavke.map(s => `
            <div style="display:flex; justify-content:space-between; align-items:center; padding: 0.35rem 0;">
                <span>${s.nazivDela}</span>
                <button type="button" class="danger" onclick="ukloniDeloSaIzlozbe(${s.stavkaId}, ${izlozbaId})">Ukloni</button>
            </div>
        `).join("");
    } catch (err) { container.innerHTML = `<p class="error">${err.message}</p>`; }
}

async function dodajDeloNaIzlozbu(izlozbaId) {
    const select = document.getElementById(`dodaj-delo-select-${izlozbaId}`);
    try {
        await apiFetch(`/Izlozbe/${izlozbaId}/dela/${select.value}`, { method: "POST", body: JSON.stringify("") });
        ucitajDelaNaIzlozbi(izlozbaId);
        ucitajIzlozbeAdmin();
    } catch (err) { alert(err.message); }
}

async function ukloniDeloSaIzlozbe(stavkaId, izlozbaId) {
    if (!confirm("Da li ste sigurni?")) return;
    try {
        await apiFetch(`/Izlozbe/stavke/${stavkaId}`, { method: "DELETE" });
        ucitajDelaNaIzlozbi(izlozbaId);
    } catch (err) { alert(err.message); }
}

function formatujZaInput(datum) { return new Date(datum).toISOString().split("T")[0]; }

function izmeniIzlozbu(id) {
    const izlozba = izlozbeKes.find(i => i.id === id);
    if (!izlozba) return;
    document.getElementById("izlozba-id").value = izlozba.id;
    document.getElementById("izlozba-naziv").value = izlozba.naziv;
    document.getElementById("izlozba-opis").value = izlozba.opis;
    document.getElementById("izlozba-pocetak").value = formatujZaInput(izlozba.datumPocetka);
    document.getElementById("izlozba-kraj").value = formatujZaInput(izlozba.datumZavrsetka);
    document.getElementById("izlozba-cena").value = izlozba.cena;
    const kapacitetInput = document.getElementById("izlozba-kapacitet");
    kapacitetInput.value = izlozba.kapacitet;
    kapacitetInput.disabled = true;
    document.getElementById("kapacitet-label").textContent = "Kapacitet (ne može se menjati)";
    document.getElementById("izlozba-form-naslov").textContent = "Izmena izložbe";
    document.getElementById("izlozba-submit-btn").textContent = "Sačuvaj izmene";
    document.getElementById("izlozba-cancel-btn").style.display = "inline-block";
    window.scrollTo(0, 0);
}

document.getElementById("izlozba-cancel-btn").addEventListener("click", () => resetujIzlozbaForm());

function resetujIzlozbaForm() {
    document.getElementById("izlozba-form").reset();
    document.getElementById("izlozba-id").value = "";
    document.getElementById("izlozba-kapacitet").disabled = false;
    document.getElementById("kapacitet-label").textContent = "Kapacitet";
    document.getElementById("izlozba-form-naslov").textContent = "Dodaj novu izložbu";
    document.getElementById("izlozba-submit-btn").textContent = "Dodaj izložbu";
    document.getElementById("izlozba-cancel-btn").style.display = "none";
}

async function obrisiIzlozbu(id) {
    if (!confirm("Sigurno?")) return;
    try {
        await apiFetch(`/Izlozbe/${id}`, { method: "DELETE" });
        ucitajIzlozbeAdmin();
    } catch (err) { alert(err.message); }
}

document.getElementById("izlozba-form").addEventListener("submit", async (e) => {
    e.preventDefault();
    const btn = document.getElementById("izlozba-submit-btn");
    const messageEl = document.getElementById("izlozba-message");

    btn.disabled = true; // SPREČAVA DUPLIRANJE
    const id = document.getElementById("izlozba-id").value;
    const body = {
        naziv: document.getElementById("izlozba-naziv").value,
        opis: document.getElementById("izlozba-opis").value,
        datumPocetka: document.getElementById("izlozba-pocetak").value,
        datumZavrsetka: document.getElementById("izlozba-kraj").value,
        cena: parseFloat(document.getElementById("izlozba-cena").value),
        kapacitet: parseInt(document.getElementById("izlozba-kapacitet").value) || 0
    };

    try {
        if (id) {
            body.id = parseInt(id);
            await apiFetch(`/Izlozbe/${id}`, { method: "PUT", body: JSON.stringify(body) });
            messageEl.textContent = "Izložba je uspešno izmenjena.";
        } else {
            // Ostavljamo samo poziv, ne treba nam `const response` ako ga ne koristimo
            await apiFetch("/Izlozbe", {
                method: "POST",
                body: JSON.stringify(body)
            });
            messageEl.textContent = "Izložba je uspešno dodata.";
        }

        // Ovo će se desiti za obe operacije (i POST i PUT)
        messageEl.className = "success";
        resetujIzlozbaForm();
        ucitajIzlozbeAdmin();

    } catch (err) {
        messageEl.textContent = "Greška: " + err.message;
        messageEl.className = "error";
    } finally {
        btn.disabled = false;
    }
});

// ==================== PRODATE KARTE ====================
async function ucitajKarteAdmin() {
    const container = document.getElementById("karte-admin-lista");
    try {
        const karte = await apiFetch("/Ulaznice/admin/sve");
        if (karte.length === 0) {
            container.innerHTML = "<p>Trenutno nema prodatih karata.</p>";
            return;
        }
        container.innerHTML = karte.map(k => `
            <div class="card">
                <h3>${k.nazivIzlozbe}</h3>
                <div class="card-info">
                    <p><strong>Posetilac:</strong> ${k.posetilacImePrezime || "Nepoznato"} (${k.posetilacEmail || "-"})</p>
                    <p><strong>Datum posete:</strong> ${new Date(k.datumPosete).toLocaleDateString("sr-RS")}</p>
                    ${k.datumKupovine ? `<p><strong>Datum kupovine:</strong> ${new Date(k.datumKupovine).toLocaleString("sr-RS")}</p>` : ""}
                    ${k.cenaPlacena ? `<p><strong>Plaćeno:</strong> ${k.cenaPlacena} din</p>` : ""}
                </div>
                <span class="status ${k.status.toLowerCase()}">${k.status}</span>
            </div>
        `).join("");
    } catch (err) { container.innerHTML = `<p class="error">${err.message}</p>`; }
}

podesiNavigaciju();
ucitajAutoreAdmin();
ucitajDelaAdmin().then(() => ucitajIzlozbeAdmin());