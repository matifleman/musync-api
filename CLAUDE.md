# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project overview

Musync API is a .NET 9 REST API (backend for the Musync mobile app) providing user auth/identity, posts, likes, follows, and instrument/genre catalog data. It follows a Clean Architecture / layered structure with CQRS via MediatR.

## Common commands

```bash
# Restore & build (from repo root)
dotnet restore
dotnet build

# Run the API locally (listens on 0.0.0.0:5000 in Development, Swagger UI at the root path "/")
dotnet run --project Musync.Api

# EF Core migrations (run from repo root; DbContext lives in Musync.Persistance)
dotnet ef migrations add <Name> --project Musync.Persistance --startup-project Musync.Api
dotnet ef database update --project Musync.Persistance --startup-project Musync.Api

# Run with Docker Compose (from repo root, mounts local Musync.Api/Musync.db into the container)
docker-compose up -d
docker-compose down
```

There are no test projects in this solution currently.

The solution file is `Musync.slnx` (new .slnx format), organized into `/src/API`, `/src/Core` (Application + Domain), and `/src/Infrastructure` (Persistance) folders.

## Architecture

Four projects, referencing inward (API → Application → Domain; Persistance → Application + Domain):

- **Musync.Domain** — POCO entities only (`ApplicationUser` extends `IdentityUser<int>`, `Post`, `PostLike`, `Instrument`, `Genre`, `Band`). All entities except `ApplicationUser`/identity types derive from `BaseEntity` (`Common/BaseEntity.cs`), which supplies `Id`, `CreatedAt`/`CreatedById`, `UpdatedAt`/`UpdatedById`. No business logic lives here beyond simple entity methods (e.g. `ApplicationUser.IsFollowing`).

- **Musync.Application** — Business logic, organized as CQRS slices under `Features/<Area>/[Commands|Queries]/<UseCase>/`, each with a `*Command`/`*Query` (IRequest), a `*Handler` (IRequestHandler), and often a FluentValidation `*Validator`. **Note:** validators are not wired into a MediatR pipeline behavior — handlers manually instantiate and call their validator at the top of `Handle()` and throw `BadRequestException` on failure (see `CreatePostCommandHandler`). Follow this same manual-validation pattern when adding new commands, don't assume a pipeline behavior will run it.
  - `Contracts/` defines interfaces implemented in outer layers: `Persistance/` (repository interfaces consumed by Musync.Persistance), `Identity/` (auth/token interfaces), `Services/`.
  - `Services/AuthService.cs` and `Services/CurrentUserService.cs` implement auth/current-user logic directly (not via MediatR).
  - `MappingProfiles/` holds AutoMapper profiles, auto-registered from this assembly.
  - `ApplicationServiceRegistration.cs` wires up AutoMapper, MediatR, JWT bearer authentication (reads `JwtSettings` from configuration), and ASP.NET Identity options (relaxed password rules: no digit/uppercase/lowercase/special-char requirements, min length 6).
  - Note: some feature file names don't match their namespace/class name (e.g. `Features/Follow/FollowCommand.cs` actually defines `FollowUserCommand` in namespace `...Features.Follow.Commands.FollowUser`) — grep by namespace/class, not just filename, when locating a handler.

- **Musync.Persistance** — EF Core (SQLite) implementation. `DatabaseContext/MusyncDbContext.cs` extends `IdentityDbContext`. `Configurations/` holds `IEntityTypeConfiguration<T>` classes (Fluent API config, applied via assembly scan). `Repositories/` implements the `Contracts.Persistance` interfaces; `GenericRepository<T>` (constrained to `BaseEntity`) provides basic CRUD, and specific repositories (e.g. `PostRepository`, `PostLikeRepository`) add query methods beyond the generic set. `PersistanceServiceRegistration.cs` wires the DbContext (SQLite, connection string `MusyncDatabaseConnectionString`), ASP.NET Identity (`ApplicationUser` + `IdentityRole<int>`), and all repositories.

- **Musync.Api** — ASP.NET Core Web API host. Controllers are thin: they resolve `IMediator` and dispatch Commands/Queries, or call `IAuthService` directly for `AuthController`. Custom error handling replaces the default ASP.NET problem-details pipeline:
  - `Middleware/ExceptionMiddleware.cs` catches all unhandled exceptions and dispatches to the highest-priority matching `IExceptionHandler` (`Contracts/Exceptions/IExceptionHandler.cs`, `Priority`: lower runs first).
  - `ExceptionHandlers/` has `BadRequestExceptionHandler` (for `BadRequestException`, includes FluentValidation errors), `NotFoundExceptionHandler` (for `NotFoundException`), and `DefaultExceptionHandler` (catch-all, priority `int.MaxValue`, returns 500). To add a new mapped exception type, add a new `IExceptionHandler` implementation and register it in `Program.cs` — it's picked up automatically by the middleware.
  - All handlers return a `CustomProblemDetails` (`Models/CustomValidationProblemDetails.cs`), logged as JSON via `ExceptionMiddleware` before being written to the response.
  - `Program.cs` also configures: dev-mode Kestrel bound to all interfaces on port 5000, Swagger with JWT bearer auth support and a custom-injected "Auto-Login" JS button (`wwwroot/swagger-ui/swagger-auth.js`) + dark theme CSS for local testing, permissive CORS (any origin/header/method), and static file serving (used for uploaded post images under `wwwroot/images` and avatars under `wwwroot/profile-pictures`).

### Auth

JWT bearer auth (`Microsoft.AspNetCore.Authentication.JwtBearer`), issued/validated using `JwtSettings` (`Key`/`Issuer`/`Audience`/`DurationInMinutes`) bound from configuration. Controllers/actions needing auth use `[Authorize]` (mostly per-action, not per-controller, except `FollowController`). `ICurrentUserService`/`CurrentUserService` resolves the current `ApplicationUser` from the HTTP context (used e.g. in `UserController.GetMyUser`). Token issuance logic lives in `Providers/TokenProvider.cs`; refresh-token flow is exposed via `AuthController.Refresh`.

### Adding a new feature slice

Mirror the existing pattern: create `Features/<Area>/[Commands|Queries]/<UseCase>/` with the request class implementing `IRequest<TResponse>`, a handler implementing `IRequestHandler<TRequest, TResponse>` injecting repositories/`UserManager<ApplicationUser>`/`IMapper` as needed, and (if input validation is needed) a `FluentValidation` validator manually invoked at the start of `Handle()`, throwing `BadRequestException`/`NotFoundException` from `Musync.Application.Exceptions` on failure. Add a corresponding thin action in the relevant controller under `Musync.Api/Controllers/` that sends the request via injected `IMediator`.


## Related frontend
- Repo: `../musync` (Expo + React Native)
- OpenAPI spec: confirm exact endpoint by running `dotnet run --project Musync.Api` and checking `/swagger/v1/swagger.json` (adjust if it differs)
- Any contract change (new endpoint, modified DTO) must be reflected by regenerating the frontend's types — see `musync`'s CLAUDE.md for the command
