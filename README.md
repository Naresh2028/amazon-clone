# Amazon Clone

An Amazon-style e-commerce platform built as a full-stack portfolio project: a **customer storefront** (React), an **admin portal** (Angular) and a **.NET 10 Web API**.

---

## Tech stack

| Layer | Technology |
|---|---|
| Customer portal | React, TypeScript, Vite, React Router |
| Admin portal | Angular 16, RxJS, Bootstrap |
| API | ASP.NET Core (.NET 10), Web API controllers |
| Data | EF Core, PostgreSQL |
| Caching | Redis (distributed cache, cache-aside pattern) |
| Validation | FluentValidation |
| Logging | Serilog (console and rolling file, correlation id per request) |
| API docs | OpenAPI with Scalar |
| Tooling | Docker Compose, Git |

---

## Features

**Done**
- [x] Category module: full CRUD API, Angular admin screens (list, create, update, delete)
- [x] Product module: full CRUD API with category relationship, price and stock
- [x] Global exception handling middleware returning RFC 7807 problem details
- [x] Structured logging with Serilog and a correlation id on every request
- [x] Redis caching for category reads, with invalidation on writes and graceful fallback to the database
- [x] React customer portal screens with mock data: product list and search, product details, cart, checkout form with validation

---

## Architecture

The backend follows **Clean Architecture**: dependencies point inward.

```
                 ┌──────────────────────────────┐
                 │  AmazonClone.Api             │  controllers, middleware, DI setup
                 └──────┬───────────────┬───────┘
                        │               │
                        ▼               ▼
        ┌───────────────────────┐   ┌──────────────────────────────┐
        │ AmazonClone.Application│◄──│ AmazonClone.Infrastructure   │
        │ DTOs, validators,      │   │ EF Core, repositories,       │
        │ services, interfaces   │   │ Redis cache                  │
        └───────────┬───────────┘   └──────────────────────────────┘
                    ▼
        ┌───────────────────────┐
        │ AmazonClone.Domain    │  entities, base entity, domain exceptions
        └───────────────────────┘
```

**Project layout**

```
amazon-clone/
├── backend/
│   ├── AmazonClone.Domain/          # Entities, BaseEntity, domain exceptions
│   ├── AmazonClone.Application/     # DTOs, validators, service and repository interfaces, services
│   ├── AmazonClone.Infrastructure/  # EF Core (PostgreSQL), repositories, External/Caching (Redis)
│   └── AmazonClone.Api/             # Controllers, middleware, dependency injection
├── frontend/
│   ├── admin-portal/                # Angular 16 admin app
│   └── customer-portal/             # React storefront
└── docker-compose.yml               # Local infrastructure (Redis)
```

**Request flow:** `Controller → Service → (Redis cache) → Repository → PostgreSQL`

---

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+ (LTS)](https://nodejs.org/) and the Angular CLI (`npm install -g @angular/cli@16`)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- PostgreSQL 17 (a local install, or a Docker container)

### 1. Start the infrastructure

```bash
docker compose up -d
```

This starts Redis on port `6379`. Make sure PostgreSQL is running on `5432` and that a database named `amazonclone` exists.

### 2. Configure secrets

Secrets are stored with **.NET User Secrets**, never in the repository. From the repository root:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=amazonclone;Username=postgres;Password=***" --project backend/AmazonClone.Api
dotnet user-secrets set "ConnectionStrings:Redis" "localhost:6379,abortConnect=false,connectTimeout=3000,syncTimeout=3000" --project backend/AmazonClone.Api
```

| Key | Purpose |
|---|---|
| `ConnectionStrings:DefaultConnection` | PostgreSQL connection |
| `ConnectionStrings:Redis` | Redis connection (comma-separated options) |
| `Cors:AllowedOrigins` | Frontend origins allowed by CORS (`http://localhost:5173`, `http://localhost:4200`) |

### 3. Create the database schema

```bash
dotnet ef database update --project backend/AmazonClone.Infrastructure --startup-project backend/AmazonClone.Api
```

### 4. Run the API

```bash
dotnet run --project backend/AmazonClone.Api
```

In Development, the interactive API reference is served at `/scalar/v1` on the URL printed in the console.

### 5. Run the frontends

```bash
# Admin portal (Angular) → http://localhost:4200
cd frontend/admin-portal
npm install
npm start

# Customer portal (React) → http://localhost:5173
cd frontend/customer-portal
npm install
npm run dev
```

---

## Author

**Naresh N**, full stack .NET and Angular developer

[GitHub](https://github.com/Naresh2028) · [LinkedIn](https://www.linkedin.com/in/naresh-fullstack)
