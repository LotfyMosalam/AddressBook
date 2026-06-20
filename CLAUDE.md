# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build the solution
dotnet build

# Run the API (HTTP: localhost:5279, HTTPS: localhost:7278, Swagger: /swagger)
dotnet run --project src/AddressBook.API

# Add an EF Core migration
dotnet ef migrations add <MigrationName> --project src/AddressBook.Infrastructure --startup-project src/AddressBook.API

# Apply migrations / update database
dotnet ef database update --project src/AddressBook.Infrastructure --startup-project src/AddressBook.API
```

There are no test projects in this solution.

## Architecture

Clean Architecture (strict layering — outer layers reference inner, never the reverse):

```
AddressBook.Domain          ← no external dependencies
AddressBook.Application     ← references Domain
AddressBook.Infrastructure  ← references Application + Domain
AddressBook.API             ← references all three
```

**Request flow:**

```
Controller → MediatR → LoggingBehavior → ValidationBehavior → Handler → Repository → DbContext
```

All controllers (except `AuthController`) require `[Authorize]` JWT bearer tokens. Errors propagate as custom exceptions (`NotFoundException`, `ValidationException`, `ConflictException`, `ForbiddenAccessException`) and are caught by `ExceptionHandlingMiddleware`, which returns ProblemDetails JSON.

## Key Patterns

**CQRS with MediatR:** Commands and queries live under `src/AddressBook.Application/`, grouped by feature (e.g., `AddressEntries/Commands/CreateEntry/`). Each feature folder contains the command/query record, its handler, and its FluentValidation validator.

**Repository pattern:** `IAddressEntryRepository`, `IJobRepository`, `IDepartmentRepository`, `IUserRepository` are defined in Application and implemented in Infrastructure. `IApplicationDbContext` abstracts the EF Core `DbContext`.

**Domain entities:** Sealed classes with private setters and static `Create()` factory methods. `BaseEntity` provides `Guid Id`, `CreatedAt`, `UpdatedAt` (auto-set in `SaveChangesAsync`). `AuditableEntity` adds `CreatedBy`/`UpdatedBy`.

**Dependency injection:** Each layer exposes an extension method (`AddApplication()`, `AddInfrastructure()`) called from `Program.cs`.

**File uploads:** Stored at `wwwroot/uploads/photos/{year}/{month}/` and served as static files. `FileUploadService` enforces the limits in `FileUploadSettings` (max 5 MB, `.jpg/.jpeg/.png/.webp`).

**Excel export:** `ExportEntriesQuery` produces `.xlsx` via ClosedXML through `IExcelExportService`.

## Package Version Constraints

This solution targets **.NET 9.0**. NuGet will sometimes resolve `Microsoft.*` packages to net10-only versions — always pin:

```bash
# Always specify --version 9.x for Microsoft.* packages
dotnet add package Microsoft.EntityFrameworkCore --version 9.x
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 9.x
```

Other pinned versions that must not change:
- `MediatR 14.1.0` — do **not** add an explicit `Microsoft.Extensions.DependencyInjection.Abstractions` package; MediatR pulls in the right version.
- `AutoMapper 16.1.1` — registration uses `cfg.AddMaps(assembly)`, **not** the older `AddAutoMapper(assembly)` overload.
- `FluentValidation 12.1.1` + `FluentValidation.DependencyInjectionExtensions 12.1.1` (in Application); `FluentValidation.AspNetCore 11.3.1` (in API only).

## Database

SQL Server (local), connection string in `appsettings.json`:
```
Server=.;Database=AddressBookDb;Trusted_Connection=True;TrustServerCertificate=True;
```

Seed data (departments, jobs) is applied automatically on startup via `ApplicationDbSeeder`. The `Migrations/` folder is currently empty — add the initial migration before running `database update` for the first time.
