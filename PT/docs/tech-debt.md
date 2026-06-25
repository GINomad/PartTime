# Tech Debt Register

Last reviewed: 2026-06-26

This file tracks code that does not meet `docs/coding-standards.md`. It is intentionally not a demand to fix everything immediately. Use it to choose focused cleanup work when touching related areas.

Status values:

- Open: Debt exists.
- In Progress: Cleanup started but incomplete.
- Resolved: Debt removed.
- Accepted: Team decided to keep it.

## TD-001: C# Files Still Use Block-Scoped Namespaces

- Status: Open
- Priority: Low
- Standard: New C# files should prefer file-scoped namespaces unless local patterns strongly require block-scoped namespaces.
- Evidence:
  - `Identity/PT.Identity.API/Program.cs`
  - `Identity/PT.Identity.API/Extensions/ServiceCollectionExtensions.cs`
  - `Identity/PT.Identity.Application/Commands/Login/LoginUserCommandHandler.cs`
  - `Shared/PT.Common/Results/OperationResult.cs`
- Impact: More nesting and more churn when adding new code that follows current .NET style.
- Suggested remediation: Convert C# files gradually when already editing a file for behavior. Avoid a repo-wide namespace-only churn commit unless the team wants one.

## TD-002: Empty Or Placeholder Backend Types

- Status: Open
- Priority: Medium
- Standard: Keep one primary concept per file and avoid unused abstractions or placeholder code.
- Evidence:
  - `Identity/PT.Identity.Infrastructure/Settings/AppSettings.cs` is empty and has unused `System.*` usings.
  - `Identity/PT.Identity.Application/Abstractions/ITokenProvider.cs` remains after moving active auth to ASP.NET Core Identity cookie endpoints.
  - `Identity/PT.Identity.Application/Abstractions/DataReaders/IUserDR.cs` and `LoginUserCommandHandler.cs` appear unused by active API endpoints.
- Impact: Future contributors may implement against obsolete extension points or assume dormant auth flow is still active.
- Suggested remediation: Decide whether custom command-based login is still planned. If not, remove obsolete abstractions and handlers. If yes, document how they fit with Identity cookie auth and wire/test them.

## TD-003: Backend DI And Endpoint Wiring Is Concentrated In `Program.cs`

- Status: Open
- Priority: Medium
- Standard: Prefer DI extension methods for non-trivial setup and keep startup code readable.
- Evidence:
  - `Identity/PT.Identity.API/Program.cs` contains inline antiforgery validation middleware.
  - `Identity/PT.Identity.API/Program.cs` maps `/api/antiforgery/token` and `/api/account/logout` inline.
- Impact: Auth/security pipeline behavior is harder to test and easier to accidentally reorder.
- Suggested remediation: Extract auth endpoint mapping and antiforgery middleware setup into focused extension methods, with tests once backend test infrastructure exists.

## TD-004: Committed SQL Credentials Remain In App Settings

- Status: Open
- Priority: High
- Standard: Do not commit secrets, passwords, production connection strings, or long-lived tokens.
- Evidence:
  - `Identity/PT.Identity.API/appsettings.json` contains `User ID=ptuser;Password=ptuser1`.
- Impact: Even local credentials in source normalize unsafe secret handling and complicate environment setup.
- Suggested remediation: Move SQL credentials to user secrets, environment variables, or a secret store. Keep only a non-secret connection string template in source.

## TD-005: Frontend Services Still Use `any` For Auth And Error Payloads

- Status: Open
- Priority: Medium
- Standard: Avoid `any`; use explicit interfaces/types for API payloads and responses.
- Evidence:
  - `client-ui/src/app/core/authentication/auth.service.ts` uses `Observable<any>` for login/register/logout.
  - `client-ui/src/app/core/authentication/auth.service.ts` accepts `userRegistration: any`.
  - `client-ui/src/app/shared/base.service.ts` accepts `error: any`.
- Impact: API contract drift is harder to catch at compile time, especially around auth and error handling.
- Suggested remediation: Add explicit request/response interfaces for Identity login/register/logout and a typed error shape for `BaseService.handleError`.

## TD-006: Frontend Code Still Uses `var`, Mutable Reassignment, And Loose Formatting

- Status: Open
- Priority: Medium
- Standard: Prefer `const` by default, `let` when reassignment is needed, avoid `var`, use semicolons, and keep formatting consistent.
- Evidence:
  - `client-ui/src/app/shared/base.service.ts` uses `var applicationError`, `var modelStateErrors`, and `for (var key in ...)`.
  - `client-ui/src/app/core/profile/profile.guard.ts` uses `var hasClientId`.
  - `client-ui/src/app/profile/profile-setup/profile-setup.component.ts` uses `var profile`, missing semicolons, and long inline object construction.
- Impact: Lint drift and avoidable bugs, especially in `for..in` error parsing and form mapping.
- Suggested remediation: Replace `var` with `const`/`let`, avoid `for..in`, split long mapping expressions into named helpers, and run lint after frontend dependencies are restored.

## TD-007: Console Logging In Application Code

- Status: Open
- Priority: Low
- Standard: Do not leave debug logging in application code unless it is intentional and routed through the app logging strategy.
- Evidence:
  - `client-ui/src/app/profile/profile-setup/profile-setup.component.ts` logs profile data with `console.log(profile)`.
  - `client-ui/src/app/home/index/index.component.ts` logs API call results with `console.log(result)`.
- Impact: Noisy browser console and possible exposure of user/profile data during development or demos.
- Suggested remediation: Remove debug logs or replace with intentional user-facing state/logging.

## TD-008: Components And Guards Subscribe Without Clear Completion Strategy

- Status: Open
- Priority: Medium
- Standard: Components should unsubscribe from long-lived subscriptions or use patterns that complete automatically.
- Evidence:
  - `client-ui/src/app/core/profile/profile.guard.ts` subscribes inside `canActivate` without unsubscribing.
  - `client-ui/src/app/shell/header/header.component.ts` subscribes and manually unsubscribes.
  - `client-ui/src/app/shell/shell/shell.component.ts` subscribes twice and manually unsubscribes.
  - `client-ui/src/app/shell/profile-card/profile-card.component.ts` subscribes to `getProfile` directly.
- Impact: Guard logic can leak or behave unpredictably; component subscription patterns are inconsistent.
- Suggested remediation: For guards, read synchronous state directly or return an observable with `take(1)`. For components, prefer `async` pipe or `takeUntil`/cleanup helpers for long-lived streams.

## TD-009: Components Mix UI Work With Data Mapping And Side Effects

- Status: Open
- Priority: Medium
- Standard: Keep components focused on presentation and UI orchestration; move reusable mapping and business logic to services/helpers.
- Evidence:
  - `client-ui/src/app/profile/profile-setup/profile-setup.component.ts` builds API payloads inline from form controls and manually concatenates phone prefix.
  - `client-ui/src/app/account/register/register.component.ts` handles loading, error state, DTO submission, and success state directly.
- Impact: Harder to test form-to-API mapping and validation behavior independently.
- Suggested remediation: Extract typed form models and mapping helpers. Keep components responsible for calling helpers and handling navigation/state.

## TD-010: Angular Template API Visibility Is Not Deliberate

- Status: Open
- Priority: Low
- Standard: Prefer `protected` for members only used by templates where compatible; use `readonly` for properties that should not be reassigned.
- Evidence:
  - `client-ui/src/app/account/login/login.component.ts` exposes `error`, `hide`, and `userLogin` as public mutable members.
  - `client-ui/src/app/account/register/register.component.ts` exposes UI state as public mutable members.
  - `client-ui/src/app/profile/profile-setup/profile-setup.component.ts` exposes `phonePrefix` and `profileForm` as public mutable members.
- Impact: Component public APIs are broader than intended.
- Suggested remediation: During Angular modernization, mark template-only members `protected` and stable injected/config members `readonly` where Angular 10 tooling allows it cleanly.

## TD-011: Tests Are Mostly Generated Smoke Tests

- Status: Open
- Priority: High
- Standard: Add focused tests when changing behavior, contracts, authentication, routing, persistence, or shared helpers.
- Evidence:
  - Many specs only assert `should create`, such as component specs under `client-ui/src/app/**`.
  - `client-ui/src/app/core/authentication/auth.service.spec.ts` does not test login/register/logout, CSRF token fetching, or state transitions.
  - `client-ui/src/app/core/authentication/xsrf.interceptor.ts` has no spec.
  - No backend test projects were found.
- Impact: Auth, CORS, CSRF, route guard, and API contract regressions can pass unnoticed.
- Suggested remediation: Add focused tests for `AuthService`, `XsrfInterceptor`, route guards, `BaseApiService`, and backend auth pipeline once test infrastructure is available.

## TD-012: Abstract Angular Service Is Injectable And Tested As A Concrete Service

- Status: Open
- Priority: Medium
- Standard: Keep one primary concept per file and test behavior through concrete services or small test doubles.
- Evidence:
  - `client-ui/src/app/shared/base-api.service.ts` is abstract but decorated with `@Injectable({ providedIn: 'root' })`.
  - `client-ui/src/app/shared/base-api.service.spec.ts` injects `BaseApiService` directly.
  - `client-ui/src/app/shared/base.service.spec.ts` likely follows the same generated pattern for an abstract base class.
- Impact: Tests do not represent how the base service is actually used and can fail under stricter Angular/TypeScript settings.
- Suggested remediation: Remove root providers from abstract base classes if not needed. Test via a concrete test subclass or through real services such as `ProfileService`.

## TD-013: Frontend Configuration Is Hardcoded In A Service

- Status: Open
- Priority: Medium
- Standard: Environment-specific values should be configurable and not scattered through code.
- Evidence:
  - `client-ui/src/app/shared/config.service.ts` hardcodes `https://localhost:7271` and `https://localhost:44362/api`.
  - `client-ui/src/environments/environment.ts` and `environment.prod.ts` do not carry API origin settings.
- Impact: Different UI/API origins are supported by auth code, but changing environments still requires code edits.
- Suggested remediation: Move API origins into Angular environment files or runtime-loaded configuration.

## TD-014: Backend Security Pipeline Needs Integration Tests

- Status: Open
- Priority: High
- Standard: Authentication, authorization, cookie, CORS, and CSRF changes must be tested and update the risk register.
- Evidence:
  - `Identity/PT.Identity.API/Program.cs` configures cookie auth, CORS, CSRF validation, token endpoint, Identity endpoints, and logout endpoint.
  - No backend test project exists.
- Impact: Middleware order and cross-origin cookie behavior can break without compiler errors.
- Suggested remediation: Add API integration tests for antiforgery token generation, CSRF rejection without header, login cookie issuance, credentialed protected request, and logout.

## TD-015: C# Nullable Suppression And Null Handling Need Review

- Status: Open
- Priority: Medium
- Standard: Favor clear null handling; avoid null-forgiving operators unless the invariant is obvious and documented.
- Evidence:
  - `Identity/PT.Identity.Application/Commands/Login/LoginUserCommandHandler.cs` uses `loginResult!.TransformTo<string>()`.
  - `Identity/PT.Identity.Application/Commands/Login/LoginUserCommandHandler.cs` uses `role.Name!`.
  - `Identity/PT.Identity.Domain/Role.cs` initializes nullable `Id` with `default!`.
- Impact: Nullable annotations are enabled, but some null assumptions are hidden from readers and tests.
- Suggested remediation: Replace suppressions with guards, non-null domain contracts, or explicit error paths.

## TD-016: Generated EF Migration Namespaces May Be Stale

- Status: Open
- Priority: Low
- Standard: Keep generated code correct, but avoid hand-editing generated artifacts unless necessary.
- Evidence:
  - Migration snapshots reference `PT.Identity.Infrastructure.Users.User` while the current user type is under `PT.Identity.Infrastructure.Database.Users`.
- Impact: Future EF migrations may produce noisy diffs or fail model comparison if namespace metadata is stale.
- Suggested remediation: After .NET 10 SDK and EF tools are available, run EF migration commands to verify whether a migration refresh is needed. Avoid manual edits unless EF tooling requires it.
