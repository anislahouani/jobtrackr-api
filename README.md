# JobTrackr API

[![JobTrackr API CI](https://github.com/anislahouani/jobtrackr-api/actions/workflows/ci.yml/badge.svg?branch=master)](https://github.com/anislahouani/jobtrackr-api/actions/workflows/ci.yml)

JobTrackr API is the backend service powering the JobTrackr application.

It provides authentication, authorization, job application management, persistence, and secure communication with the Angular frontend.

---

## Live API

https://jobtrackr-api-gwkx.onrender.com

Frontend:

https://jobtrackr-web-navy.vercel.app

---

## Features

- User registration
- User login
- JWT authentication
- Protected endpoints
- BCrypt password hashing
- Create, read, update and delete job applications
- PostgreSQL persistence
- Entity Framework Core migrations
- CORS configuration for local and production environments
- Docker-based deployment

---

## Tech Stack

- ASP.NET Core 10
- C#
- Entity Framework Core
- PostgreSQL
- Npgsql
- JWT
- BCrypt
- Docker
- Render
- Neon PostgreSQL

### Testing & CI

- xUnit v3
- Entity Framework Core InMemory
- GitHub Actions

---

## Architecture

```text
Angular Frontend
      │
      │ HTTPS / JSON
      ▼
ASP.NET Core REST API
      │
      │ Entity Framework Core
      ▼
PostgreSQL
```

---

## Project Structure

```text
Configurations/
Controllers/
Data/
DTOs/
Enums/
Extensions/
Interfaces/
Mappings/
Middleware/
Migrations/
Models/
Repositories/
Services/
tests/
  └── JobTrackr.Api.Tests/
```

---

## Authentication

The API uses JSON Web Tokens for authentication.

After a successful login or registration, the API returns a JWT that must be included in protected requests using the `Authorization` header:

```text
Authorization: Bearer <token>
```

Passwords are never stored in plain text. They are hashed using BCrypt before being saved to the database.

---

## Main Endpoints

### Authentication

```text
POST /api/Auth/register
POST /api/Auth/login
```

### Job Applications

```text
GET    /api/job-applications
GET    /api/job-applications/{id}
POST   /api/job-applications
PUT    /api/job-applications/{id}
DELETE /api/job-applications/{id}
```

Job application endpoints require authentication.

---

## Running Locally

Clone the repository:

```bash
git clone https://github.com/anislahouani/jobtrackr-api.git
cd jobtrackr-api
```

Restore dependencies:

```bash
dotnet restore JobTrackr.Api.csproj
dotnet restore tests/JobTrackr.Api.Tests/JobTrackr.Api.Tests.csproj
```

Configure the PostgreSQL connection string and JWT settings.

A typical development configuration requires:

```text
ConnectionStrings:DefaultConnection
Jwt:Key
Jwt:Issuer
Jwt:Audience
```

Apply database migrations:

```bash
dotnet ef database update
```

Run the API:

```bash
dotnet run
```

The API is available locally at:

```text
http://localhost:5159
```

---

## Tests

The test suite currently covers the core `JobApplicationService` business rules, including:

- Application creation and input trimming
- Per-user data isolation
- Newest-first ordering
- Read access protection across users
- Authorized and unauthorized updates
- Authorized and unauthorized deletion

Run the suite with:

```bash
dotnet run --project tests/JobTrackr.Api.Tests/JobTrackr.Api.Tests.csproj
```

---

## Docker

Build the image:

```bash
docker build -t jobtrackr-api .
```

Run the container:

```bash
docker run -p 8080:8080 jobtrackr-api
```

---

## Deployment

The production API is deployed on Render using Docker.

The PostgreSQL database is hosted on Neon.

Production secrets are provided through environment variables and are not stored in the repository.

---

## CI

GitHub Actions automatically restores dependencies, builds the API and test project in Release mode, and executes the backend test suite on pushes and pull requests to `master`.

---

## Related Repository

Frontend:

https://github.com/anislahouani/jobtrackr-web

---

## Roadmap

Planned improvements include:

- Integration tests
- Global exception handling improvements
- Structured logging
- Pagination
- Improved API documentation

---

## Author

**Anis Lahouani**

GitHub:

https://github.com/anislahouani

---

## License

This project is licensed under the MIT License.
