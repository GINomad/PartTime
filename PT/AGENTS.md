# Project Knowledge

## Tech Stack

- Backend targets .NET 10 (`net10.0`) and uses ASP.NET Core Web API, ASP.NET Core Identity cookie authentication, EF Core SQL Server, Swagger/Swashbuckle, NLog, and MediatR abstractions.
- Frontend is Angular 10 with Angular Material/CDK, Bootstrap 4, RxJS 6, Karma/Jasmine, Protractor, and TSLint.
- The repository is split into backend solutions under `Identity/` and `Shared/`, plus the Angular client under `client-ui/`.

## Architecture

- Backend layers:
  - `Identity/PT.Identity.API`: API host, DI setup, middleware, Swagger.
  - `Identity/PT.Identity.Application`: command handlers and application abstractions.
  - `Identity/PT.Identity.Domain`: domain models and repository abstractions.
  - `Identity/PT.Identity.Infrastructure`: EF Core, ASP.NET Identity, logging, repository implementations, migrations.
  - `Identity/PT.Identity.Client`: API DTOs shared by backend application code.
  - `Shared/PT.Common`: MediatR command/query abstractions and `OperationResult`.
- Frontend modules are organized by feature (`account`, `profile`, `home`, `shell`) plus `core` and `shared`.
- Backend DI setup is commonly expressed through `ServiceCollectionExtensions`.
- Frontend authenticated API services inherit from `BaseApiService` to send cross-origin requests with credentials.
- Authentication uses ASP.NET Core Identity API endpoints under `/api/account`; Angular posts credentials to `/api/account/login`, and the backend stores the session in an HttpOnly secure cookie.
- Cross-origin unsafe API calls use antiforgery protection: Angular fetches `/api/antiforgery/token`, then sends `X-XSRF-TOKEN` through an HTTP interceptor.

## Build And Test Commands

- Backend:
  - `dotnet restore Identity\PT.Identity.sln`
  - `dotnet build Identity\PT.Identity.sln`
  - `dotnet build Shared\Shared.sln`
- Frontend, from `client-ui/`:
  - `npm start`
  - `npm run build`
  - `npm test`
  - `npm run lint`
  - `npm run e2e`

## Current Environment Note

- `global.json` requests .NET SDK `10.0.100` with `rollForward: latestFeature`.
- If only .NET 7 is installed, `dotnet build` stops before compilation with an SDK resolution error.

## Coding Conventions

- Follow `docs/coding-standards.md` for future C#/.NET and TypeScript/Angular code.
- C# projects enable nullable reference types and implicit usings.
- Application commands are records implementing shared MediatR-style command interfaces.
- Shared operation outcomes use `OperationResult` and `OperationResult<TResult>`.
- Angular code follows TSLint recommended/codelyzer rules with single quotes, semicolons, and a 140-character line limit.

## Risk Tracking

- Track known and suspected risks in `docs/risk-register.md`.
- When a risk is fixed, update its status and add evidence showing where it was removed.
- Track coding-standard debt in `docs/tech-debt.md`.
