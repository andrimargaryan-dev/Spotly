# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Spotly is an event ticketing platform for Yerevan, Armenia, built on ASP.NET Core MVC (.NET 10, `net10.0`). Events are held at venues and tickets are sold per session. Local context shows up in the domain: the `District` enum lists Yerevan's districts, and `PaymentMethod` includes Armenian providers (Idram, Telcell).

The project is early-stage. `AppDbContext` (SQL Server) is registered through `AddInfrastructure`. The `DefaultConnection` connection string is kept in User Secrets for Spotly.Web, not in appsettings, and the app throws at startup if it's missing. There's no test project yet.

## Commands

Run these from the repo root:

```bash
dotnet build Spotly.sln                                  # build all projects
dotnet run --project Spotly.Web                          # run the site (http://localhost:5014)
dotnet run --project Spotly.Web --launch-profile https   # https://localhost:7219
dotnet watch --project Spotly.Web                        # hot reload
```

### Database (EF Core)

Migrations live in Spotly.Infrastructure, but the EF tools need Spotly.Web as the startup project. Web holds the `Microsoft.EntityFrameworkCore.Design` package, the DI setup and the User Secrets. These commands need the `dotnet-ef` global tool.

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<connection string>" --project Spotly.Web
dotnet ef migrations add <Name> --project Spotly.Infrastructure --startup-project Spotly.Web --output-dir Data/Migrations
dotnet ef database update --project Spotly.Infrastructure --startup-project Spotly.Web
dotnet ef migrations remove --project Spotly.Infrastructure --startup-project Spotly.Web
```

There are no tests yet. If a test project is added, run one test with `dotnet test --filter "FullyQualifiedName~TestName"`.

## Architecture

The solution follows a layered or "clean" layout with three projects:

- **Spotly.Core**: domain entities (`Entities/`), enums (`Enums/`) and constants (`Constants/`). It references no other project. Its only package is `Microsoft.Extensions.Identity.Stores`, which `AppUser` needs. EF Core packages belong in Infrastructure, not Core.
- **Spotly.Infrastructure**: references Core. It owns the EF Core packages (`Microsoft.EntityFrameworkCore.SqlServer` and `Microsoft.AspNetCore.Identity.EntityFrameworkCore`) and holds `Data/AppDbContext`, an `IdentityDbContext<AppUser, IdentityRole<int>, int>`. It also contains:
  - `DependencyInjection.AddInfrastructure(connectionString)`, which registers the DbContext with `UseSqlServer`;
  - the entity configurations in `Data/Configurations/`;
  - the migrations in `Data/Migrations/` (currently only `InitialCreate`).
- **Spotly.Web**: the MVC app (controllers, Razor views, `wwwroot`). It references both Core and Infrastructure, and it owns the User Secrets ID and the EF Core Design package. `Program.cs` does three things:
  - calls `AddInfrastructure`;
  - registers Identity with `AddIdentity<AppUser, IdentityRole<int>>`, backed by `AppDbContext`. Emails must be unique, passwords need at least 8 characters, and symbols aren't required.
  - sets the auth cookie's paths to `/account/login` and `/account/access-denied`. There's no account controller yet, so those paths don't resolve.

Dependencies point inward: Core must never reference Infrastructure or Web.

### AppDbContext conventions

These apply to the whole model, so the entities don't show them:

- Every `decimal` is stored as `(18,2)`.
- Every enum is stored as a string with a maximum length of 32.
- Every foreign key between domain entities uses `DeleteBehavior.Restrict`, so cascades must be handled explicitly.
- `IEntityTypeConfiguration` classes in the Infrastructure assembly are picked up automatically.
- `CreatedAt` and `UpdatedAt` are set only in `SaveChangesAsync`, and only for `BaseEntity` types. Synchronous `SaveChanges` doesn't set them, and neither method sets `AppUser.CreatedAt`.

### Domain model conventions

- Every entity inherits `BaseEntity` (`int Id`, `CreatedAt`, `UpdatedAt?`) except `AppUser`. `AppUser` extends `IdentityUser<int>`, so Identity uses **int keys**, and any Identity or DbContext setup must use the int-keyed generics.
- Authorization uses global ASP.NET Identity roles. Take role names from `Spotly.Core.Constants.Roles` (Admin, Manager, Editor, Scanner, Finance) rather than writing string literals.
- Relationships use an explicit FK property plus a navigation property: `int XId` plus `X X = null!`. Collections are initialized to empty lists.
- Use the `Slug` properties (on `Event` and `Category`) for URL-friendly lookups.

### Core flow

- An `Event` belongs to one `Category` (categories form a hierarchy through `ParentId`) and one `Venue`.
- `Event` → `EventSession`: the dated occurrences of an event.
- `Event` → `TicketType`: pricing tiers defined per event. `QuantityPerSession` sets inventory per session.
- An `Order` is placed against a single **EventSession**, not an Event. It holds `OrderItem`s (TicketType × quantity at a snapshot `UnitPrice`), and each item issues one `Ticket` per unit (each ticket has a `QrToken` for check-in).
- `Order.UserId` is nullable, so guest checkout works through the `Contact*` fields. A pending order has an `ExpiresAt` hold. An order can have multiple `Payment` attempts.
