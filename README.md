# AddressBook API

![.NET](https://img.shields.io/badge/.NET-9.0-blueviolet)
![EF Core](https://img.shields.io/badge/EF_Core-9.0-blue)
![SQL Server](https://img.shields.io/badge/SQL_Server-LocalDB-red)
![License](https://img.shields.io/badge/license-MIT-green)

A professional **Address Book REST API** built with **.NET 9** and Clean Architecture. Supports contact management with photo uploads, Excel export, JWT authentication, and full-text search with filtering and pagination.

---

## Features

- **CRUD** for address entries, jobs, and departments
- **JWT authentication** — register and login with BCrypt-hashed passwords
- **Pagination, sorting, and filtering** across all address fields and date ranges
- **Photo upload** — date-organized storage under `wwwroot/uploads/photos/{year}/{month}/`
- **Excel export** — download all entries as `.xlsx` (ClosedXML)
- **MediatR pipeline** — LoggingBehavior + ValidationBehavior on every request
- **Auto-migrations and seeding** — database is created and seeded with lookup data on first run

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | ASP.NET Core 9 Web API |
| ORM | Entity Framework Core 9 (Code First, SQL Server) |
| CQRS | MediatR 14.1 |
| Mapping | AutoMapper 16.1 |
| Validation | FluentValidation 12.1 |
| Auth | JWT Bearer (Microsoft.AspNetCore.Authentication.JwtBearer 9.x) |
| Password hashing | BCrypt.Net-Next 4.2 |
| Excel | ClosedXML 0.105 |
| API docs | Swashbuckle (Swagger UI) |

---

## Architecture

Clean Architecture with strict layering — outer layers depend on inner, never the reverse:

```
AddressBook.Domain          ← Entities, interfaces. No external packages.
AddressBook.Application     ← CQRS handlers, DTOs, validators. References Domain.
AddressBook.Infrastructure  ← EF Core, repositories, services. References Application + Domain.
AddressBook.API             ← Controllers, middleware. References all three.
```

**Request pipeline:**

```
HTTP Request
  → Controller
  → MediatR
  → LoggingBehavior   (logs elapsed time, warns on slow requests)
  → ValidationBehavior (FluentValidation, throws on failure)
  → Handler
  → Repository
  → DbContext (SQL Server)
```

Errors surface as typed exceptions (`NotFoundException`, `ValidationException`, `ConflictException`) caught by `ExceptionHandlingMiddleware`, which returns RFC 7807 ProblemDetails JSON.

---

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9)
- SQL Server (local instance — `Server=.`)
- EF Core CLI tools:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

---

## Getting Started

**1. Clone the repository**

```bash
git clone https://github.com/your-username/AddressBook.Backend.git
cd AddressBook.Backend
```

**2. Configure the connection string**

Open `src/AddressBook.API/appsettings.json` and verify:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=AddressBookDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Update `Server=.` if your SQL Server instance has a different name.

**3. Run the API**

```bash
dotnet run --project src/AddressBook.API
```

On first run the app will:
- Apply all EF Core migrations (creates `AddressBookDb` automatically)
- Seed 10 jobs and 8 departments

**4. Open Swagger**

Navigate to `https://localhost:7278/swagger` (or `http://localhost:5279/swagger`).

Click **Authorize** and paste your JWT token after registering or logging in.

---

## API Endpoints

> Full request/response documentation with field types and validation rules is in [API_REFERENCE.md](API_REFERENCE.md).

### Auth — no token required

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/auth/register` | Register a new user (role: 0=User, 1=Admin) |
| `POST` | `/api/auth/login` | Login and receive a JWT token |

### Jobs — `Authorization: Bearer <token>`

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/jobs` | List all jobs |
| `GET` | `/api/jobs/{id}` | Get job by ID |
| `POST` | `/api/jobs` | Create a job |
| `PUT` | `/api/jobs/{id}` | Update a job |
| `DELETE` | `/api/jobs/{id}` | Delete a job |

### Departments — `Authorization: Bearer <token>`

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/departments` | List all departments |
| `GET` | `/api/departments/{id}` | Get department by ID |
| `POST` | `/api/departments` | Create a department |
| `PUT` | `/api/departments/{id}` | Update a department |
| `DELETE` | `/api/departments/{id}` | Delete a department |

### Addresses — `Authorization: Bearer <token>`

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/addresses` | Paginated list (sort + page) |
| `GET` | `/api/addresses/search` | Filter by name, email, mobile, job, department, date range |
| `GET` | `/api/addresses/export` | Download all entries as Excel (.xlsx) |
| `GET` | `/api/addresses/{id}` | Get full entry details |
| `POST` | `/api/addresses` | Create an address entry |
| `PUT` | `/api/addresses/{id}` | Update an address entry |
| `DELETE` | `/api/addresses/{id}` | Delete an address entry |
| `POST` | `/api/addresses/{id}/photo` | Upload / replace profile photo (multipart/form-data) |

---

## Project Structure

```
src/
├── AddressBook.Domain/
│   ├── Entities/          # AddressEntry, Job, Department, User (factory methods)
│   ├── Enums/             # UserRole
│   └── Common/            # BaseEntity, AuditableEntity, IRepository<T>
│
├── AddressBook.Application/
│   ├── AddressEntries/    # CQRS commands, queries, DTOs, validators
│   ├── Auth/              # Register, Login commands + AuthResponseDto
│   ├── Lookup/            # Jobs and Departments commands + queries
│   └── Common/            # Behaviors, exceptions, interfaces, PaginatedResult<T>
│
├── AddressBook.Infrastructure/
│   ├── Persistence/       # ApplicationDbContext, EF configurations, repositories, seeder
│   ├── Services/          # JwtService, PasswordHasher, FileUploadService, ExcelExportService
│   ├── Identity/          # CurrentUserService
│   └── Settings/          # JwtSettings, FileUploadSettings
│
└── AddressBook.API/
    ├── Controllers/        # AuthController, AddressController, JobController, DepartmentController
    ├── Middleware/         # ExceptionHandlingMiddleware
    ├── Extensions/         # ServiceExtensions (JWT, CORS, Swagger)
    └── Properties/         # launchSettings.json (opens Swagger on start)
```


---

## Configuration

All settings live in `src/AddressBook.API/appsettings.json`:

| Key | Description | Default |
|-----|-------------|---------|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string | `Server=.;Database=AddressBookDb;...` |
| `JwtSettings:SecretKey` | Signing key — must be ≥ 32 characters | — |
| `JwtSettings:Issuer` | JWT issuer | `AddressBookAPI` |
| `JwtSettings:Audience` | JWT audience | `AddressBookClient` |
| `JwtSettings:ExpiryDays` | Token lifetime in days | `7` |
| `FileUploadSettings:StorageFolderName` | Subfolder under `wwwroot/uploads/` | `photos` |
| `FileUploadSettings:MaxFileSizeBytes` | Maximum upload size | `5242880` (5 MB) |
| `FileUploadSettings:AllowedExtensions` | Accepted image types | `.jpg .jpeg .png .webp` |

---

## Common Commands

```bash
# Build
dotnet build

# Run
dotnet run --project src/AddressBook.API

# Add a migration
dotnet ef migrations add <MigrationName> \
  --project src/AddressBook.Infrastructure \
  --startup-project src/AddressBook.API

# Apply migrations manually
dotnet ef database update \
  --project src/AddressBook.Infrastructure \
  --startup-project src/AddressBook.API
```
