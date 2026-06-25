# Risk Register

Use this file to review whether known risks are still present. When a risk is removed, change `Status` and add evidence.

Status values:

- Open: Risk is still present or not yet investigated.
- Mitigated: Risk still exists but a control reduces impact.
- Removed: Risk is no longer present.
- Accepted: Team intentionally accepts the risk.

## RISK-001: Hardcoded Secrets And Local Configuration

- Status: Open
- Severity: High
- Confidence: High
- Evidence:
  - `Identity/PT.Identity.API/appsettings.json` contains SQL credentials.
  - The previously committed JWT secret was removed from `Identity/PT.Identity.API/appsettings.json`.
  - `client-ui/src/app/shared/config.service.ts` hardcodes auth and client API URLs.
- Why it matters: Secrets and environment-specific URLs in source code increase leakage risk and make deployments brittle. This is reduced but not removed because SQL credentials and local API URLs remain in source.
- Review/removal criteria: Secrets come from user secrets, environment variables, or a secure secret store; frontend API/OIDC settings are environment-specific config; no production secret values are committed.
- Suggested owner: Backend/frontend platform owner.
- Last reviewed: 2026-06-25

## RISK-002: Backend API Surface Appears Incomplete

- Status: Mitigated
- Severity: High
- Confidence: High
- Evidence:
  - `Identity/PT.Identity.API/Program.cs` calls `app.MapControllers()`.
  - `Identity/PT.Identity.API/Program.cs` now maps ASP.NET Core Identity endpoints at `/api/account`.
  - No custom `*Controller.cs` files were found under `Identity/`.
  - `Identity/PT.Identity.Application/Commands/*` contains command handlers that do not appear exposed by controllers in this repo.
- Why it matters: Registration/login are now reachable through built-in Identity endpoints, but any intended custom command-handler API surface still appears incomplete.
- Review/removal criteria: Controllers or minimal API endpoints exist for intended identity operations, and route tests or integration tests verify them.
- Suggested owner: Backend owner.
- Last reviewed: 2026-06-25

## RISK-003: Application Handler Dependencies Are Not Fully Wired

- Status: Open
- Severity: High
- Confidence: High
- Evidence:
  - `LoginUserCommandHandler` depends on `IUserDR` and `ITokenProvider`.
  - No concrete implementations of `IUserDR` or `ITokenProvider` were found.
  - No MediatR registration was found in API startup.
  - `IUserRepository` implementation exists, but repository registration was not found in DI setup.
- Why it matters: Handlers may fail runtime activation even if the projects compile.
- Review/removal criteria: DI registration exists for MediatR, repositories, data readers, and token provider; app startup/integration tests exercise handler resolution.
- Suggested owner: Backend owner.
- Last reviewed: 2026-06-25

## RISK-004: Legacy SPA OIDC Implicit Flow

- Status: Removed
- Severity: Medium
- Confidence: Medium
- Evidence:
  - `client-ui/src/app/core/authentication/auth.service.ts` no longer imports `oidc-client` or calls `signinRedirect`.
  - `client-ui/package.json` no longer references `oidc-client`.
  - The API maps built-in Identity endpoints at `/api/account`.
- Why it matters: The legacy implicit redirect flow has been replaced with ASP.NET Core Identity API cookie login.
- Review/removal criteria: SPA auth uses HttpOnly secure cookies, Authorization Code with PKCE, or another documented accepted alternative; token storage/renewal behavior is reviewed.
- Suggested owner: Frontend/security owner.
- Last reviewed: 2026-06-25

## RISK-005: Frontend And Backend Identity Contracts May Not Match

- Status: Mitigated
- Severity: Medium
- Confidence: Medium
- Evidence:
  - Frontend `UserRegistration` and `UserLogin` models now use `email` and `password`.
  - `AuthService` posts to `/api/account/register` and `/api/account/login` with `email` and `password`, matching ASP.NET Core Identity API endpoints.
  - Backend `UserRegistrationDto` has `UserName`, `Password`, and `ConfirmPassword`.
  - `UserMapper` still expects `dto.UserName` for any future custom command-handler registration path.
- Why it matters: The active built-in Identity endpoint contract is aligned, but the dormant custom DTO/command path still differs from frontend models.
- Review/removal criteria: Shared API contract is documented and tested; frontend payload fields match backend DTOs; registration has integration coverage.
- Suggested owner: Backend/frontend owner.
- Last reviewed: 2026-06-25

## RISK-006: Database User Creation Does Not Set UserName Explicitly

- Status: Removed
- Severity: Medium
- Confidence: High
- Evidence:
  - `Identity/PT.Identity.Infrastructure/Database/Users/Repositories/UserRepository.cs` now sets both `Email = user.UserName` and `UserName = user.UserName`.
  - Identity is configured with `RequireUniqueEmail = true`.
- Why it matters: ASP.NET Identity commonly requires `UserName`; this mapping issue is now fixed for the custom repository path.
- Review/removal criteria: User creation sets both intended `Email` and `UserName`, or a documented Identity configuration confirms this is safe; registration tests cover it.
- Suggested owner: Backend owner.
- Last reviewed: 2026-06-25

## RISK-007: Frontend Auth State Can Be Null At Call Sites

- Status: Mitigated
- Severity: Medium
- Confidence: High
- Evidence:
  - `AuthService` no longer exposes `authorizationHeaderValue`.
  - `AuthService.profile` now has a nullable return type.
  - `ProfileCardComponent` now handles a missing profile before calling `getProfile`.
- Why it matters: Protected API calls are safer, but route guards and services still need focused tests around unauthenticated and expired-token states.
- Review/removal criteria: Auth service exposes nullable-safe token/profile access, guards wait for auth initialization, and tests cover unauthenticated calls.
- Suggested owner: Frontend owner.
- Last reviewed: 2026-06-25

## RISK-008: Angular Test Specs Are Mostly Generated Smoke Tests

- Status: Open
- Severity: Low
- Confidence: Medium
- Evidence:
  - Many `*.spec.ts` files only check component/service creation.
  - No backend test projects were found.
- Why it matters: Build success may not catch routing, auth, API-contract, or backend handler regressions.
- Review/removal criteria: Add focused tests for auth flow, route guards, API services, registration handler, repository behavior, and startup DI.
- Suggested owner: QA/application owners.
- Last reviewed: 2026-06-25

## RISK-009: Protractor And TSLint Tooling Are Deprecated

- Status: Open
- Severity: Low
- Confidence: High
- Evidence:
  - `client-ui/package.json` uses Protractor and TSLint-era Angular 10 tooling.
  - `client-ui/e2e/protractor.conf.js` configures Protractor e2e tests.
  - `client-ui/tslint.json` configures TSLint/codelyzer.
- Why it matters: Deprecated tooling can block dependency upgrades and CI modernization.
- Review/removal criteria: E2E tests migrate to a maintained runner, linting migrates to ESLint, and package upgrade path is documented.
- Suggested owner: Frontend owner.
- Last reviewed: 2026-06-25

## RISK-010: NLog File Target Writes Outside App Folder

- Status: Open
- Severity: Low
- Confidence: Medium
- Evidence:
  - `Identity/PT.Identity.API/nlog.config` writes to `${aspnet-appbasepath}\..\..\log\identity-service{shortdate}.log`.
- Why it matters: Runtime environments may not have permission to create or write this relative log path, and logs may end up outside expected deployment directories.
- Review/removal criteria: Logging path is environment-configurable and verified in local/dev/prod runtime environments.
- Suggested owner: Backend/platform owner.
- Last reviewed: 2026-06-25

## RISK-011: .NET 10 SDK Requirement Can Break Local Builds

- Status: Open
- Severity: Medium
- Confidence: High
- Evidence:
  - `global.json` requests SDK `10.0.100`.
  - Current local check previously showed only SDK `7.0.400` installed.
- Why it matters: Developers without .NET 10 SDK cannot restore or build backend projects.
- Review/removal criteria: Onboarding docs list SDK requirement; CI image and local environments install compatible .NET 10 SDK.
- Suggested owner: Platform/dev owner.
- Last reviewed: 2026-06-25

## RISK-012: Browser Local Storage Token Exposure

- Status: Removed
- Severity: Medium
- Confidence: High
- Evidence:
  - `client-ui/src/app/core/authentication/auth.service.ts` no longer stores Identity API bearer tokens in `localStorage`.
  - `client-ui/src/app/shared/base-api.service.ts` uses `withCredentials: true` instead of an `Authorization` bearer header.
  - `Identity/PT.Identity.API/Extensions/ServiceCollectionExtensions.cs` configures application cookies as HttpOnly, Secure, and `SameSite=None`.
  - `Identity/PT.Identity.API/Program.cs` exposes `/api/antiforgery/token`, validates antiforgery tokens for unsafe `/api` requests, and maps `/api/account/logout` to clear the cookie.
- Why it matters: Bearer token theft from browser local storage is no longer the active auth-storage model.
- Review/removal criteria: Confirm no bearer access or refresh tokens are persisted in browser storage.
- Suggested owner: Frontend/security owner.
- Last reviewed: 2026-06-25

## RISK-013: Cookie Auth Requires Cross-Origin And CSRF Configuration Discipline

- Status: Mitigated
- Severity: Medium
- Confidence: High
- Evidence:
  - `Identity/PT.Identity.API/Extensions/ServiceCollectionExtensions.cs` uses CORS `.AllowCredentials()` with configured allowed origins.
  - `Identity/PT.Identity.API/appsettings.json` lists allowed UI origins.
  - `client-ui/src/app/core/authentication/xsrf.interceptor.ts` sends `X-XSRF-TOKEN` on unsafe requests.
  - `client-ui/src/app/core/authentication/auth.service.ts` fetches `/api/antiforgery/token` before login, register, and logout.
- Why it matters: Cross-origin cookie auth breaks if allowed origins, `SameSite=None`, `Secure`, `withCredentials`, and antiforgery token handling drift out of sync.
- Review/removal criteria: Add integration tests for cross-origin login, authenticated API calls, logout, and CSRF rejection when the header is missing.
- Suggested owner: Backend/frontend security owner.
- Last reviewed: 2026-06-25
