# Coding Standards For Future Work

Last reviewed: 2026-06-26

This file is the working standard for future code changes in this repository. It combines current official guidance with the conventions already present in this codebase.

Sources:

- Microsoft C# coding conventions: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions
- Microsoft C# identifier naming rules: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names
- Angular style guide: https://angular.dev/style-guide
- TypeScript project coding guidelines: https://github.com/microsoft/TypeScript/wiki/Coding-guidelines

## General Rules

- Prefer consistency with the surrounding file over mechanical rewrites.
- Keep changes scoped to the requested behavior.
- Do not mix unrelated formatting, refactoring, dependency updates, and feature work.
- Favor clear names and boring code over cleverness.
- Add comments only for non-obvious decisions, constraints, or security-sensitive behavior.
- Add or update tests when changing behavior, contracts, authentication, routing, persistence, or shared helpers.

## C# And .NET

### Language And Style

- Use modern C# features when they improve clarity and are supported by the target framework and project language version.
- Prefer async/await for I/O-bound work.
- Avoid catching broad `Exception` unless the code can handle it meaningfully or is translating/logging at a boundary.
- Use C# keyword aliases for built-in types: `string`, `int`, `bool`, not `System.String`, `System.Int32`, `System.Boolean`.
- Use `var` only when the type is obvious from the right side, such as `new SomeType()`, casts, literals, or LINQ projections.
- Prefer object initializers and target-typed `new()` when the target type is clear.
- Keep LINQ readable; do not force LINQ when a simple loop is clearer.
- Use string interpolation for short string composition.
- Use `StringBuilder` for repeated string appends in loops.
- Use file-scoped namespaces for new C# files unless the surrounding project/file pattern strongly favors block-scoped namespaces.
- Keep `using` directives outside namespaces.

### Naming

- Use `PascalCase` for namespaces, types, records, public members, properties, methods, and constants.
- Use `camelCase` for method parameters and local variables.
- Prefix interfaces with `I`.
- Prefix private instance fields with `_`.
- Prefix private static fields with `s_` when adding new static fields.
- Avoid abbreviations unless they are common in the domain.
- Prefer full, descriptive names over short names. Single-letter names are only acceptable for tiny loop counters or very local generic examples.
- Record primary-constructor parameters should use `PascalCase` when they become public properties.
- Class and struct primary-constructor parameters should use `camelCase`.

### Backend Architecture

- Keep API host wiring in `Identity/PT.Identity.API`.
- Keep application use cases in `Identity/PT.Identity.Application`.
- Keep domain models and abstractions in `Identity/PT.Identity.Domain`.
- Keep EF Core, Identity, logging, and concrete persistence in `Identity/PT.Identity.Infrastructure`.
- Keep shared command/query/result abstractions in `Shared/PT.Common`.
- Prefer DI extension methods for service setup when the setup is shared or non-trivial.
- Do not bypass application/domain abstractions from API code unless the endpoint is intentionally framework-provided, such as ASP.NET Core Identity API endpoints.

### Security

- Do not commit secrets, passwords, signing keys, production connection strings, or long-lived tokens.
- Prefer environment variables, user secrets, or a secret store for sensitive values.
- For browser auth, prefer HttpOnly secure cookies plus CSRF protection over bearer tokens in browser storage.
- If UI and API have different origins, use explicit CORS origins. Never use wildcard origins with credentials.
- Unsafe API methods (`POST`, `PUT`, `PATCH`, `DELETE`) must be protected against CSRF when cookie auth is used.
- Authentication, authorization, cookie, CORS, and CSRF changes must update `docs/risk-register.md`.

### Tests

- For backend auth and DI changes, add startup/integration coverage when practical.
- For EF Core changes, test mappings or repository behavior when the change can affect persistence.
- For command handlers, test success and failure paths using the shared `OperationResult` shape.

## TypeScript And Angular

### Repository Compatibility

- This app currently uses Angular 10 and TypeScript 3.9.
- Follow current Angular style where compatible, but do not introduce APIs unavailable to Angular 10.
- In this codebase, keep constructor injection unless the project is upgraded to an Angular version where `inject()` is established and already used.
- Keep TSLint-era formatting in existing files until the project migrates to ESLint.

### Naming And Files

- Use hyphenated file names: `user-profile.component.ts`, `profile-setup.component.ts`.
- Component TypeScript, template, style, and spec files should share the same base name.
- Test files should end in `.spec.ts`.
- Organize files by feature area, not by generic type buckets.
- Keep one primary concept per file.
- Use `PascalCase` for classes, interfaces, enums, and Angular component classes.
- Use `camelCase` for functions, properties, method parameters, and local variables.
- Do not prefix TypeScript interfaces with `I`.
- Use whole words in names when practical.

### Angular Components

- Keep components focused on presentation and UI orchestration.
- Move reusable business logic, data mapping, and API work into services or pure helpers.
- Keep lifecycle hooks small; call well-named private/protected methods for longer work.
- Implement lifecycle interfaces such as `OnInit` and `OnDestroy`.
- Put Angular-specific members near the top of the class: injected dependencies, inputs, outputs, queries, then local state, then methods.
- Prefer `readonly` for properties that should not be reassigned.
- For modern Angular code, prefer `protected` for members only used by templates. For Angular 10 files, apply this only if template type checking and local conventions support it cleanly.
- Avoid complex expressions in templates; move complex logic to component methods or derived state.
- Prefer `[class.foo]` and `[style.foo]` bindings over `ngClass` and `ngStyle` for simple cases.
- Name event handlers for what they do, such as `saveProfile()`, not for the browser event, such as `handleClick()`.

### TypeScript Style

- Use single quotes and semicolons in this repo, matching `client-ui/tslint.json`.
- Use 2-space indentation in Angular files, matching the existing Angular CLI style in this repo.
- Always use braces for loops and conditionals.
- Prefer arrow functions for callbacks.
- Avoid `for..in`; use `Object.keys`, `Object.entries`, array methods, or explicit loops as appropriate.
- Prefer `const` by default, `let` when reassignment is needed, and avoid `var`.
- Avoid `any`; use explicit interfaces/types for API payloads and responses.
- Prefer `unknown` over `any` when the shape is not known yet.
- Avoid exporting types/functions unless they are used outside the file.
- Do not add values to the global namespace.
- Treat arrays and objects as immutable unless mutation is clearly local and simpler.

### RxJS And HTTP

- Services should return `Observable<T>` rather than subscribing internally, except for deliberate app initialization or fire-and-forget side effects.
- Components should unsubscribe from long-lived subscriptions or use operators/patterns that complete automatically.
- Keep HTTP options centralized in shared helpers where possible.
- For cross-origin cookie auth, all API calls that require auth must use `withCredentials: true`.
- Do not manually attach bearer tokens unless the auth architecture changes and the risk register is updated.
- Handle API errors through the shared `BaseService.handleError` pattern unless a feature needs richer error mapping.

### Frontend Security

- Do not store bearer access tokens or refresh tokens in `localStorage` or `sessionStorage`.
- It is acceptable to store non-secret UI state, such as a display email or profile id, when needed for navigation.
- CSRF request tokens may be kept in `sessionStorage` if they are not bearer credentials and are paired with server-side antiforgery validation.
- Any change to auth, cookies, CORS, CSRF, or token handling must update `docs/risk-register.md`.

### Tests

- New components and services should have focused tests beyond "should create" when behavior is added.
- For guards, test allowed and redirected paths.
- For auth and HTTP services, use `HttpClientTestingModule`.
- For interceptors, test that expected headers/options are applied and that safe HTTP methods are not modified unnecessarily.
