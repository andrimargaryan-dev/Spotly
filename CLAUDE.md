# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Spotly is an event ticketing platform for Yerevan, Armenia, built on ASP.NET Core MVC (.NET 10, `net10.0`). Organizations publish events at venues and sell tickets per session. Local context shows up in the domain: the `District` enum lists Yerevan's districts, and `PaymentMethod` includes Armenian providers (Idram, Telcell).

The project is early-stage. The domain model exists, but there is no DbContext, no persistence, no Identity wiring and no test project yet.

## Commands

Run these from the repo root:

```bash
dotnet build Spotly.sln                                  # build all projects
dotnet run --project Spotly.Web                          # run the site (http://localhost:5014)
dotnet run --project Spotly.Web --launch-profile https   # https://localhost:7219
dotnet watch --project Spotly.Web                        # hot reload
```

There are no tests yet. If a test project is added, run one test with `dotnet test --filter "FullyQualifiedName~TestName"`.

## Architecture

The solution follows a layered or "clean" layout with three projects:

- **Spotly.Core**: domain entities (`Entities/`) and enums (`Enums/`). It references no other project. Its only package is `Microsoft.Extensions.Identity.Stores`, which `AppUser` needs.
- **Spotly.Infrastructure**: references Core. It is currently empty and is meant for EF Core persistence, the DbContext and external integrations such as payment providers.
- **Spotly.Web**: the MVC app (controllers, Razor views, `wwwroot`). It references both Core and Infrastructure. `Program.cs` is still the default template.

Dependencies point inward: Core must never reference Infrastructure or Web.

### Domain model conventions

- Most entities inherit `BaseEntity` (`int Id`, `CreatedAt`, `UpdatedAt?`). Two exceptions:
  - `AppUser` extends `IdentityUser<int>`, so Identity uses **int keys**. Any Identity or DbContext setup must use the int-keyed generics.
  - `OrganizationMember` is a plain class (a join between `AppUser` and `Organization` that carries an `OrganizationRole`).
- Relationships use an explicit FK property plus a navigation property: `int XId` plus `X X = null!`. Collections are initialized to empty lists.
- Use `Slug` properties (on `Event`, `Organization` and `Category`) for URL-friendly lookups.

### Core flow

- `Organization` → `Event`. An event belongs to one organization, one `Category` (categories form a hierarchy through `ParentId`) and one `Venue`.
- `Event` → `EventSession`: the dated occurrences of an event.
- `Event` → `TicketType`: pricing tiers defined per event. `QuantityPerSession` sets inventory per session.
- An `Order` is placed against a single **EventSession**, not an Event. It holds `OrderItem`s (TicketType × quantity at a snapshot `UnitPrice`), and each item issues one `Ticket` per unit (each ticket has a `QrToken` for check-in).
- `Order.UserId` is nullable, so guest checkout works through the `Contact*` fields. A pending order has an `ExpiresAt` hold. An order can have multiple `Payment` attempts.
