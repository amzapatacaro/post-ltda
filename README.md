# Post Ltda. API

HTTP microservice for **Customer** and **Post** management (Post Ltda. technical interview scenario).

## Stack

| Area          | Technology                                                                                                   |
| ------------- | ------------------------------------------------------------------------------------------------------------ |
| Runtime / API | **ASP.NET Core 2.1** (MVC), **C# 7.x**                                                                       |
| Data          | **Entity Framework Core 2.1**, **SQL Server**                                                                |
| Layers        | **API** → **Business** → **DataAccess**                                                                      |
| API docs      | **Swashbuckle** (Swagger UI)                                                                                 |
| Logging       | **Serilog** (configurable via `appsettings`)                                                                 |
| Tests         | **xUnit**, **Moq** — `Tests/PostLtda.Tests` project (**net8.0** for the test runner; the API remains on 2.1) |

---

## Prerequisites

1. **[.NET SDK](https://dotnet.microsoft.com/download)** compatible with `netcoreapp2.1` (e.g. SDK 2.1 or a newer SDK that can build 2.1 projects).
2. **SQL Server** on your **local machine** with the **`JujuTest`** database (or the name you use in the connection string) **already created and populated**. This repository does **not** ship **Docker Compose** or automated container-based database setup.
3. If you need schema/data, use the interview **`.bak`** or run **`Seeds/JujuTests.Script.sql`** manually against your SQL Server instance.

---

## Configuration

1. Clone the repository and go to the solution root.
2. Edit the connection string in **`API/appsettings.Development.json`** → **`ConnectionStrings:Development`** so it points at your local SQL Server and database.
3. (Optional) Adjust **`ASPNETCORE_ENVIRONMENT`** if you do not use `Development`.

**DbContext**, **repositories**, and **services** are registered from **Business** via `AddPostLtdaPersistence` (see `PostLtdaServiceCollectionExtensions.cs`).

---

## Build

From the repository root:

```bash
dotnet restore ProjectAPI.sln
dotnet build ProjectAPI.sln -c Debug
```

To run only unit tests (requires **.NET 8** SDK installed for the test project):

```bash
dotnet test Tests/PostLtda.Tests/PostLtda.Tests.csproj -c Debug
```

**GitHub Actions:** `.github/workflows/ci.yml` runs on pushes to **`main`** and on pull requests. It restores packages, builds the solution, and runs unit tests only.

---

## Run the API

```bash
cd API
dotnet run
```

Listening URLs depend on the active launch profile and **`API/Properties/launchSettings.json`**; **`dotnet run`** prints them in the console. Swagger is usually at **`/swagger`**.

To override URLs for a run:

```bash
dotnet run --urls "http://localhost:5000"
```

---

## Scenario checklist

Based on **`Instructions.md`**:

- [x] **Customer — update:** `PUT` with JSON body and consistent update in the service layer.
- [x] **Customer — create:** required name and no duplicates (trim, case-insensitive comparison); conflict → **409 Conflict**.
- [x] **Post — create:** customer must exist; **Body** if length > 20 → 97-character prefix + `...`; **Type** 1 / 2 / 3 → **Farándula** / **Política** / **Futbol**; any other **Type** keeps the category sent.
- [x] **Customer — delete:** removes all **Post** rows with the same `CustomerId`, then the **Customer** (transaction).
- [x] **Post — batch:** `POST /Post/List` with multiple posts; same rules as single create; **all-or-nothing** transaction.

---

## Implemented improvements (refactor and quality)

Architecture and maintenance changes **in addition** to the above:

- **HTTP layer:** **`IActionResult`** responses with **400 / 404 / 409** and JSON body `{ message }` where applicable (duplicate customer, validation, customer not found, etc.).
- **Application contracts:** **`ICustomerService`** and **`IPostService`** expose only **DTOs** (`CustomerDto`, `PostDto`); the API does not depend on **`DataAccess`** types.
- **Mapping:** **`Business/Mapping/MappingExtensions.cs`** — extension methods for **entity ↔ DTO** (`Customer`/`Post` ↔ `CustomerDto`/`PostDto`). **AutoMapper is not used**; API and Business have no AutoMapper package references.
- **Dependency injection:** EF and service registration centralized in **`AddPostLtdaPersistence`** (**Business** project), API **`Startup`** with no direct references to `DataAccess`.
- **Batch posts:** **`POST /Post/List`** endpoint with `CreatePostsRequest` body and **all-or-nothing** transaction in the service.
- **Unit tests:** **`Tests/PostLtda.Tests`** project (helpers, controller tests with mocks).

---

## Solution structure

| Project                  | Role                                                  |
| ------------------------ | ----------------------------------------------------- |
| **API**                  | Web host, controllers, request models (`API/Models`). |
| **Business**             | Services, DTOs, mapping, DI extensions.               |
| **DataAccess**           | `DbContext`, entities, `BaseModel` / `IBaseModel`.    |
| **Tests/PostLtda.Tests** | Unit tests.                                           |
