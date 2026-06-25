# Project Knowledge Discovery

This file captures durable knowledge discovered from the current repository layout and source code.

## 1. Backend Tech Stack

- Confidence: High
- Evidence: `Identity/PT.Identity.API/PT.Identity.API.csproj`, `Identity/PT.Identity.Infrastructure/PT.Identity.Infrastructure.csproj`, `Shared/PT.Common/PT.Common.csproj`
- Summary: Backend targets .NET 10 and uses ASP.NET Core Web API, ASP.NET Core Identity cookie authentication, EF Core SQL Server, Swagger/Swashbuckle, NLog, and MediatR.

## 2. Frontend Tech Stack

- Confidence: High
- Evidence: `client-ui/package.json`, `client-ui/angular.json`
- Summary: Frontend is Angular 10 with Angular Material/CDK, Bootstrap 4, RxJS 6, Karma/Jasmine, Protractor, and TSLint.

## 3. Repository Architecture

- Confidence: High
- Evidence: `Identity/PT.Identity.sln`, `Shared/Shared.sln`, `client-ui/angular.json`
- Summary: Backend is split across Identity API/Application/Domain/Infrastructure/Client projects and a shared common project. The Angular app lives separately under `client-ui`.

## 4. Backend Layering

- Confidence: High
- Evidence: `Identity/PT.Identity.Application/Commands/Register/RegisterUserCommandHandler.cs`, `Identity/PT.Identity.Domain/Abstractions/Repositories/IUserRepository.cs`, `Identity/PT.Identity.Infrastructure/Database/Users/Repositories/UserRepository.cs`
- Summary: Application command handlers depend on abstractions; infrastructure implements persistence using ASP.NET Core Identity and EF Core.

## 5. Frontend Architecture

- Confidence: High
- Evidence: `client-ui/src/app/app.module.ts`, `client-ui/src/app/app-routing.module.ts`, feature folders under `client-ui/src/app`
- Summary: Angular modules are organized by feature. Routes use a shell wrapper for child routes, and guards protect profile/authenticated pages.

## 6. Authentication Flow

- Confidence: Medium
- Evidence: `client-ui/src/app/core/authentication/auth.service.ts`, `client-ui/src/app/core/authentication/auth.guard.ts`, `Identity/PT.Identity.API/Extensions/ServiceCollectionExtensions.cs`
- Summary: The SPA posts credentials to the ASP.NET Core Identity API endpoints exposed under `/api/account`. The API stores the session in an HttpOnly secure cookie. Angular sends cross-origin API calls with credentials and an `X-XSRF-TOKEN` antiforgery header for unsafe methods.

## 7. Registration Flow

- Confidence: Medium
- Evidence: `client-ui/src/app/account/register/register.component.ts`, `client-ui/src/app/core/authentication/auth.service.ts`, `Identity/PT.Identity.Application/Commands/Register/RegisterUserCommandHandler.cs`, `Identity/PT.Identity.Infrastructure/Database/Users/Repositories/UserRepository.cs`
- Summary: The frontend posts registration data to `/api/account/register`. ASP.NET Core Identity API endpoints handle the primary registration path. Existing application command code still maps DTOs to domain users and infrastructure calls `UserManager.CreateAsync` for any custom registration flow.

## 8. Database Access Patterns

- Confidence: High
- Evidence: `Identity/PT.Identity.Infrastructure/Database/PtIdentityDbContext.cs`, `Identity/PT.Identity.Infrastructure/Extensions/ServiceCollectionExtensions.cs`, `Identity/PT.Identity.Infrastructure/Migrations`
- Summary: Identity persistence uses `IdentityDbContext<User>` with SQL Server. Migrations are in `PT.Identity.Infrastructure`.

## 9. API Clients And Integrations

- Confidence: High
- Evidence: `client-ui/src/app/shared/config.service.ts`, `client-ui/src/app/core/profile/profile.service.ts`, `client-ui/src/app/shared/base-api.service.ts`
- Summary: Angular calls the auth API at `https://localhost:7271` and the client/profile API at `https://localhost:44362/api`. Authenticated calls use cookies via `withCredentials: true`; unsafe methods also include an antiforgery header.

## 10. Build And Test Commands

- Confidence: High
- Evidence: `client-ui/package.json`, `client-ui/README.md`, `global.json`
- Summary: Backend builds use `dotnet restore/build` on the `Identity` and `Shared` solutions. Frontend uses npm scripts for serve, build, test, lint, and e2e. .NET SDK 10 is required.

## 11. Coding Conventions

- Confidence: Medium
- Evidence: `client-ui/tslint.json`, `.csproj` files, `Shared/PT.Common/Commands/ICommand.cs`
- Summary: TypeScript uses TSLint/codelyzer rules. C# enables nullable and implicit usings, uses MediatR-style commands/queries, and returns `OperationResult` objects.

## 12. Repeated Patterns

- Confidence: High
- Evidence: backend `ServiceCollectionExtensions`, frontend `BaseApiService`, frontend `Shell.childRoutes`
- Summary: Backend service setup is extension-method based. Frontend services centralize credentialed HTTP options and error handling. Feature routes are commonly wrapped through the shell.

## 13. Documentation Targets

- Confidence: High
- Evidence: No prior `AGENTS.md`, `docs`, `adr`, `playbooks`, or `memory` directories were present before this documentation pass.
- Summary: General development guidance belongs in `AGENTS.md`; detailed project knowledge belongs in `docs/project-knowledge.md`; risk status belongs in `docs/risk-register.md`; future architectural decisions can be added under `docs/adr/` if the project adopts ADRs.
