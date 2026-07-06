# Muzej

Muzej is a web application for museum management, developed as a project for the Advanced .NET Technologies course. The application allows visitors to browse a catalog of artworks, authors, and exhibitions, purchase tickets for exhibitions, and manage their own ticket history. Administrators can manage the full catalog and view all sold tickets.

> **Note:** This project was developed as a university assignment at the Faculty of Organizational Sciences, University of Belgrade, Serbia. As a result, class names, method names, and variable names throughout the codebase are written in Serbian.

## Project Description

Muzej is built as a full-stack application with an ASP.NET Core Web API backend, SQL Server database, and a plain HTML/CSS/JavaScript frontend served statically from the same API project.

Posetilac (Visitor) can:
- register an account
- log in
- browse Autori (Authors), UmetnickaDela (Artworks), and Izlozbe (Exhibitions)
- view details of a single exhibition, including the artworks on display
- purchase one or more Ulaznice (Tickets) for an exhibition
- view their own ticket history
- cancel a purchased ticket (returning it to the pool of available tickets)

Administrator can:
- add, edit, and delete Autori
- add, edit, and delete UmetnickaDela (Slika/Skulptura — Painting/Sculpture)
- add, edit, and delete Izlozbe
- add or remove artworks from an exhibition (StavkaIzlozbe)
- view all sold tickets across all visitors

An IzlozbaStatusService background service automatically updates the status of each exhibition (Najavljena/Aktivna/Zavrsena — Announced/Active/Finished) based on the current date, independent of any HTTP request.

## Technologies

**Backend**
- C#
- ASP.NET Core Web API (.NET 10)
- Entity Framework Core (Code First)
- SQL Server (LocalDB)
- ASP.NET Core Identity
- JWT Bearer Authentication
- MediatR (CQRS)
- FluentValidation
- Scalar (API documentation)

**Frontend**
- Plain HTML5, CSS3, JavaScript (no framework)
- Fetch API for communication with the backend
- JWT stored in browser localStorage

## Architecture

The project follows a layered (Clean Architecture-inspired) structure:

Muzej
├── Muzej.Domain
├── Muzej.Infrastructure
├── Muzej.Application
├── Muzej.API
│   └── wwwroot (frontend)
└── Muzej.sln

### Muzej.Domain

The core domain layer. Contains entities and enums with no dependency on any external framework or infrastructure.

Main entities:
- `UmetnickoDelo` (abstract) → `Slika`, `Skulptura`
- `Autor`
- `Izlozba`
- `StavkaIzlozbe` (associative entity for the many-to-many relationship between Izlozba and UmetnickoDelo)
- `Ulaznica`

Enums:
- `TipUmetnickogDela` (Slika, Skulptura)
- `TipPosetioca` (Redovan, Student, Penzioner)
- `StatusIzlozbe` (Najavljena, Aktivna, Zavrsena)
- `StatusUlaznice` (Slobodna, Kupljena, Iskoriscena, Otkazana)

Also contains the Repository/Unit of Work interfaces (`IRepository<T>`, `IAutorRepository`, `IIzlozbaRepository`, `IUlaznicaRepository`, `IUmetnickoDeloRepository`, `IUnitOfWork`), since these describe what the application expects from persistence without depending on how it is actually implemented.

### Muzej.Infrastructure

The infrastructure layer, containing the concrete implementation of data access and identity.

- `MuzejContext` — inherits `IdentityDbContext<Korisnik>`, meaning the application uses ASP.NET Identity for authentication and role management. Configures TPH (Table Per Hierarchy) inheritance for `UmetnickoDelo`, all entity relationships, and cascade/restrict delete behavior.
- `Korisnik` — the Identity user class (inherits `IdentityUser`), extended with `Ime`, `Prezime`, `TipPosetioca`, and `Zvanje`.
- `Repository<T>` and specific repositories (`AutorRepository`, `IzlozbaRepository`, `UlaznicaRepository`, `UmetnickoDeloRepository`) — concrete EF Core implementations, including `.Include()`-based queries for loading related data.
- `UnitOfWork` — aggregates all repositories and exposes `SaveChanges()`, using lazy initialization for each repository.

### Muzej.Application

The application layer, implementing CQRS through MediatR. Organized in "feature folders" — each operation (Command or Query) has its own subfolder containing the request, its handler, and (where applicable) its FluentValidation validator.

Structure per entity area (Autori, UmetnickaDela, Izlozbe, Ulaznice):

EntityArea
├── Commands
│   └── OperationName
│       ├── OperationNameCommand.cs
│       ├── OperationNameCommandHandler.cs
│       └── OperationNameCommandValidator.cs
├── Queries
│   └── QueryName
│       ├── QueryNameQuery.cs
│       └── QueryNameQueryHandler.cs
└── Dtos
└── EntityDto.cs

Also contains:
- `Common/Behaviors/ValidationBehavior.cs` — a MediatR pipeline behavior that automatically runs FluentValidation on every Command/Query before it reaches its handler, throwing a `ValidationException` on failure.

One exception to this structure exists: `GetSveUlazniceQueryHandler`, which combines data from both the Domain (Ulaznice) and Identity (Korisnik) — since Muzej.Application must not depend on Muzej.Infrastructure, this specific handler lives in `Muzej.API/Handlers` instead.

### Muzej.API

The backend entry point. Contains controllers, the JWT token service, middleware, the background service, and the static frontend.

Controllers:
- `AuthController` — registration and login, issuing JWT tokens
- `AutoriController` — CRUD for Autor (Create/Update/Delete restricted to Administrator)
- `UmetnickaDelaController` — CRUD for UmetnickoDelo (Create/Update/Delete restricted to Administrator)
- `IzlozbeController` — CRUD for Izlozba, plus adding/removing artworks from an exhibition and a GetById endpoint with full exhibition details
- `UlanziceController` — ticket purchasing, viewing own tickets, cancelling a ticket, and (Administrator only) viewing all sold tickets

Other:
- `Service/JwtTokenService.cs` — generates JWT tokens with claims for user id, email, name, and role
- `Middleware/GlobalExceptionMiddleware.cs` — catches `ValidationException` and `InvalidOperationException` globally, converting them into consistent JSON 400 responses instead of raw 500 errors
- `BackgroundServices/IzlozbaStatusService.cs` — a `BackgroundService` that periodically updates exhibition statuses based on the current date, independent of any HTTP request

### wwwroot (Frontend)

A plain HTML/CSS/JavaScript frontend, served statically by the API itself (no separate frontend server, no CORS configuration needed).

Pages:
- `index.html` — login and registration (single form, toggled via JavaScript)
- `katalog.html` — public browsing of Izlozbe, UmetnickaDela, and Autori (tabbed view)
- `izlozba.html` — details of a single exhibition and ticket purchase form
- `moje-karte.html` — a visitor's own ticket history, with cancellation
- `admin.html` — full CRUD interface for Administrator, plus exhibition/artwork linking and a view of all sold tickets

The JWT token is decoded client-side (`decodeToken` in `js/api.js`) purely to adjust which navigation links and pages are shown — this is a UX convenience only. The actual security boundary is enforced server-side through `[Authorize(Roles = "Administrator")]` on the relevant endpoints.

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server LocalDB (comes bundled with Visual Studio)
- Visual Studio 2022/2026 (or any IDE with .NET support)

### Setup

1. Clone the repository
2. Copy `Muzej.API/appsettings.Example.json` to `Muzej.API/appsettings.json` and fill in a JWT signing key (minimum 32 characters)
3. Open `Muzej.sln` in Visual Studio
4. Set `Muzej.API` as the startup project
5. Open the Package Manager Console, set the default project to `Muzej.Infrastructure`, and run: Update-Database. This creates the `MuzejDb` database on LocalDB with all required tables.
6. Run the application (F5). On first run, the "Posetilac" and "Administrator" roles are seeded automatically, along with a default Administrator account:
   - Email: `admin@muzej.com`
   - Password: `Admin123!`
7. The frontend is served automatically at the application's root URL (e.g. `https://localhost:7013/`). API documentation (Scalar) is available at `/scalar/v1`.

### Running Tests

No automated test suite is currently included in this project.
## Security

- Passwords are never stored in plain text — ASP.NET Identity stores a salted hash.
- JWT is used for authentication; the token is sent as a Bearer token in the Authorization header on every protected request.
- `[Authorize]` is used on endpoints that require any authenticated user (e.g. purchasing or viewing own tickets).
- `[Authorize(Roles = "Administrator")]` is used on endpoints that only an Administrator may access (Create/Update/Delete on the catalog, viewing all sold tickets).
- The visitor purchasing a ticket is always resolved from the authenticated user's token (`ClaimTypes.NameIdentifier`), never from client-supplied input — preventing a user from purchasing tickets on another user's behalf.

## Business Rules Worth Noting

- When an Izlozba is created, a number of Ulaznica records equal to its Kapacitet are automatically generated with status Slobodna.
- Purchasing tickets is all-or-nothing: if there are not enough Slobodna tickets available, the entire purchase is rejected.
- The final CenaPlacena on each ticket is calculated once, at purchase time, based on the visitor's TipPosetioca (discount), and is not affected by later changes to the exhibition's base price.
- Cancelling a ticket returns it to Slobodna status (rather than a permanent Otkazana state), making it available for purchase by another visitor, provided the exhibition date has not already passed.
- Deleting an Autor or UmetnickoDelo that still has related records is blocked with a clear error message, rather than silently cascading.
- Deleting an Izlozba with existing Kupljena tickets is blocked.

## Author

Una Ilić
