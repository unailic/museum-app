# Muzej

> **Note (English):** This README is written in Serbian, as this project was developed as a university assignment at the Faculty of Organizational Sciences, University of Belgrade, Serbia. Class names, method names, and variable names throughout the codebase are also written in Serbian. Feel free to use a translation tool if needed.

Muzej je veb aplikacija za upravljanje muzejom, razvijena kao projekat iz predmeta Napredne .NET tehnologije. Aplikacija omogućava posetiocima da pregledaju katalog umetničkih dela, autora i izložbi, kupuju ulaznice za izložbe, i prate sopstvenu istoriju kupljenih karata. Administratori mogu da upravljaju celokupnim katalogom i pregledaju sve prodate ulaznice.

## Opis projekta

Muzej je izgrađen kao full-stack aplikacija sa ASP.NET Core Web API backend-om, SQL Server bazom podataka, i jednostavnim HTML/CSS/JavaScript frontend-om koji pokreće sam API server.

Posetilac može da:
- registruje nalog
- se prijavi
- pregleda Autore, Umetnička dela i Izložbe
- pregleda detalje pojedinačne izložbe, uključujući dela koja su na njoj izložena
- kupi jednu ili više ulaznica za izložbu
- pregleda sopstvenu istoriju kupljenih karata
- otkaže kupljenu kartu (karta se time vraća u fond dostupnih karata)

Administrator može da:
- dodaje, menja i briše autore
- dodaje, menja i briše umetnička dela (slike/skulpture)
- dodaje, menja i briše izložbe
- dodaje ili uklanja umetnička dela sa izložbe
- pregleda sve prodate ulaznice svih posetilaca

Pozadinski servis (Background Service) automatski ažurira status svake izložbe (Najavljena/Aktivna/Zavrsena) na osnovu trenutnog datuma, nezavisno od bilo kog HTTP zahteva.

## Tehnologije

**Backend**
- C#
- ASP.NET Core Web API (.NET 10)
- Entity Framework Core (Code First)
- SQL Server (LocalDB)
- ASP.NET Core Identity
- JWT Bearer autentifikacija
- MediatR (CQRS)
- FluentValidation
- Scalar (API dokumentacija)

**Frontend**
- Čist HTML5, CSS3, JavaScript (bez frameworka)
- Fetch API za komunikaciju sa backend-om
- JWT token se čuva u localStorage browsera

## Arhitektura

Projekat prati slojevitu (Clean Architecture inspirisanu) strukturu:

```
Muzej
├── Muzej.Domain
├── Muzej.Infrastructure
├── Muzej.Application
├── Muzej.API
│   └── wwwroot (frontend)
└── Muzej.sln
```

### Muzej.Domain

Osnovni domenski sloj. Sadrži entitete i enume, bez zavisnosti od bilo kog spoljnog frameworka ili infrastrukture.

Glavni entiteti:
- `UmetnickoDelo` (apstraktna) → `Slika`, `Skulptura`
- `Autor`
- `Izlozba`
- `StavkaIzlozbe` (asocijativni entitet za N:M vezu između Izlozbe i UmetnickogDela)
- `Ulaznica`

Enumi:
- `TipUmetnickogDela` (Slika, Skulptura)
- `TipPosetioca` (Redovan, Student, Penzioner)
- `StatusIzlozbe` (Najavljena, Aktivna, Zavrsena)
- `StatusUlaznice` (Slobodna, Kupljena, Iskoriscena, Otkazana)

Takođe sadrži Repository/Unit of Work interfejse (`IRepository<T>`, `IAutorRepository`, `IIzlozbaRepository`, `IUlaznicaRepository`, `IUmetnickoDeloRepository`, `IUnitOfWork`), pošto oni opisuju šta aplikacija očekuje od skladištenja podataka, bez zavisnosti od toga kako je to stvarno implementirano.

### Muzej.Infrastructure

Infrastrukturni sloj, sadrži konkretnu implementaciju pristupa podacima i identiteta korisnika.

- `MuzejContext` - nasleđuje `IdentityDbContext<Korisnik>`, što znači da aplikacija koristi ASP.NET Identity za autentifikaciju i upravljanje ulogama. Konfiguriše TPH (Table Per Hierarchy) nasleđivanje za `UmetnickoDelo`, sve veze između entiteta, i cascade/restrict ponašanje pri brisanju.
- `Korisnik` - Identity klasa korisnika (nasleđuje `IdentityUser`), proširena poljima `Ime`, `Prezime`, `TipPosetioca` i `Zvanje`.
- `Repository<T>` i specifični repository-ji (`AutorRepository`, `IzlozbaRepository`, `UlaznicaRepository`, `UmetnickoDeloRepository`) - konkretne EF Core implementacije, uključujući upite zasnovane na `.Include()` za učitavanje povezanih podataka.
- `UnitOfWork` - objedinjuje sve repository-je i izlaže `SaveChanges()`, koristeći lenju (lazy) inicijalizaciju za svaki repository.

### Muzej.Application

Aplikacioni sloj, implementira CQRS kroz MediatR. Organizovan kroz "feature foldere" - svaka operacija (Command ili Query) ima svoj podfolder sa zahtevom, njegovim handler-om, i (gde je primenljivo) FluentValidation validatorom.

Struktura po EntityArea:

```
EntityArea
├── Commands
│   └── NazivOperacije
│       ├── NazivOperacijeCommand.cs
│       ├── NazivOperacijeCommandHandler.cs
│       └── NazivOperacijeCommandValidator.cs
├── Queries
│   └── NazivUpita
│       ├── NazivUpitaQuery.cs
│       └── NazivUpitaQueryHandler.cs
└── Dtos
└── EntityDto.cs
```

Takođe sadrži:
- `Common/Behaviors/ValidationBehavior.cs` - MediatR pipeline behavior koji automatski pokreće FluentValidation nad svakim Command/Query pre nego što stigne do svog handler-a, bacajući `ValidationException` u slučaju greške.

Postoji jedan izuzetak od ove strukture: `GetSveUlazniceQueryHandler`, koji kombinuje podatke iz Domain sloja (Ulaznice) i Identity sloja (Korisnik). Pošto `Muzej.Application` ne sme da zavisi od `Muzej.Infrastructure`, ovaj konkretan handler se nalazi u `Muzej.API/Handlers`.

### Muzej.API

Ulazna tačka backend-a. Sadrži kontrolere, JWT token servis, middleware, pozadinski servis, i statički frontend.

Kontroleri:
- `AuthController` - registracija i prijava, izdavanje JWT tokena
- `AutoriController` - CRUD za Autora (Create/Update/Delete ograničeni na Administratora)
- `UmetnickaDelaController` - CRUD za UmetnickoDelo (Create/Update/Delete ograničeni na Administratora)
- `IzlozbeController` - CRUD za Izlozbu, plus dodavanje/uklanjanje dela sa izložbe i GetById endpoint sa punim detaljima izložbe
- `UlanziceController` - kupovina karata, pregled sopstvenih karata, otkazivanje karte, i (samo Administrator) pregled svih prodatih karata

Ostalo:
- `Service/JwtTokenService.cs` - generiše JWT tokene sa claim-ovima za ID korisnika, email, ime i ulogu
- `Middleware/GlobalExceptionMiddleware.cs` - globalno hvata `ValidationException` i `InvalidOperationException`, pretvarajući ih u dosledne JSON 400 odgovore umesto sirovih 500 grešaka
- `BackgroundServices/IzlozbaStatusService.cs` - `BackgroundService` koji periodično ažurira status izložbi na osnovu trenutnog datuma, nezavisno od bilo kog HTTP zahteva

### wwwroot (Frontend)

Čist HTML/CSS/JavaScript frontend, koji pokreće sam API server (bez posebnog frontend servera, bez potrebe za CORS konfiguracijom).

Stranice:
- `index.html` - prijava i registracija (jedna forma, prebacuje se preko JavaScript-a)
- `katalog.html` - javan pregled Izložbi, Umetničkih dela i Autora (podeljeno u kartice/tabove)
- `izlozba.html` - detalji pojedinačne izložbe i forma za kupovinu karata
- `moje-karte.html` - istorija sopstvenih kupljenih karata posetioca, sa otkazivanjem
- `admin.html` - kompletan CRUD interfejs za Administratora, plus povezivanje izložbi i dela, i pregled svih prodatih karata

JWT token se dekodira na strani klijenta (`decodeToken` u `js/api.js`) isključivo da bi se prilagodilo koji navigacioni linkovi i stranice se prikazuju - ovo je samo pogodnost za korisničko iskustvo. Stvarna sigurnosna granica se sprovodi na serverskoj strani kroz `[Authorize(Roles = "Administrator")]` na odgovarajućim endpoint-ima.

## Pokretanje projekta

### Preduslovi

- .NET 10 SDK
- SQL Server LocalDB (dolazi uz Visual Studio)
- Visual Studio 2022/2026 (ili bilo koji IDE sa .NET podrškom)

### Podešavanje

1. Klonirajte repozitorijum
2. Kopirajte `Muzej.API/appsettings.Example.json` u `Muzej.API/appsettings.json` i popunite JWT ključ za potpisivanje (minimum 32 karaktera)
3. Otvorite `Muzej.sln` u Visual Studio-u
4. Postavite `Muzej.API` kao startup projekat
5. Otvorite Package Manager Console, postavite default projekat na `Muzej.Infrastructure`, i pokrenite: Update-Database. Ovo kreira `MuzejDb` bazu na LocalDB-u sa svim potrebnim tabelama.
6. Pokrenite aplikaciju (F5). Pri prvom pokretanju, uloge "Posetilac" i "Administrator" se automatski kreiraju, zajedno sa podrazumevanim administratorskim nalogom:
   - Email: `admin@muzej.com`
   - Lozinka: `Admin123!`
7. Frontend se automatski učitava na početnoj putanji aplikacije (npr. `https://localhost:7013/`). API dokumentacija (Scalar) je dostupna na `/scalar/v1`.

### Pokretanje testova

Projekat trenutno ne sadrži automatizovane testove.

## Bezbednost

- Lozinke se nikad ne čuvaju kao čist tekst - ASP.NET Identity čuva heširanu (hashed) i "posoljenu" (salted) verziju.
- JWT se koristi za autentifikaciju; token se šalje kao Bearer token u Authorization header-u pri svakom zaštićenom zahtevu.
- `[Authorize]` se koristi na endpoint-ima koji zahtevaju bilo kog prijavljenog korisnika (npr. kupovina ili pregled sopstvenih karata).
- `[Authorize(Roles = "Administrator")]` se koristi na endpoint-ima kojima sme da pristupi samo Administrator (Create/Update/Delete nad katalogom, pregled svih prodatih karata).
- Posetilac koji kupuje kartu se uvek određuje na osnovu tokena autentifikovanog korisnika (`ClaimTypes.NameIdentifier`), nikad na osnovu podatka koji šalje klijent - čime se sprečava da korisnik kupuje karte u ime nekog drugog.

## Bitna poslovna pravila

- Kad se izložba kreira, automatski se generiše broj Ulaznica jednak njenom Kapacitetu, sa statusom Slobodna.
- Kupovina karata je "sve ili ništa": ako nema dovoljno slobodnih karata, cela kupovina se odbija.
- Konačna cena (CenaPlacena) svake karte se izračunava jednom, u trenutku kupovine, na osnovu tipa posetioca (popust), i ne menja se naknadnim izmenama osnovne cene izložbe.
- Otkazivanjem karte, ona se vraća u status Slobodna (umesto trajnog statusa Otkazana), čime postaje dostupna za kupovinu drugom posetiocu, pod uslovom da datum izložbe još nije prošao.
- Brisanje Autora ili UmetnickogDela koje i dalje ima povezane zapise je onemogućeno, uz jasnu poruku greške, umesto tihog kaskadnog brisanja.
- Brisanje Izlozbe sa postojećim kupljenim kartama je onemogućeno.

## Autor

Una Ilić

