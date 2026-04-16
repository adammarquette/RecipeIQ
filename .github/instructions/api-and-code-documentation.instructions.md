---
description: "Use when creating or modifying C# backend code, ASP.NET endpoints, or gRPC contracts/services in RecipeIQ. Enforces XML documentation, OpenAPI/Swagger endpoint documentation, and clear gRPC docs."
name: RecipeIQ API And Code Documentation Standards
applyTo:
  - "src/**/*.cs"
  - "tests/**/*.cs"
  - "**/*.proto"
---
# Documentation Standards

Apply these rules for all backend and API changes.

## XML Documentation In C#
- All classes must have XML documentation comments.
- All public members must have XML documentation comments, including:
  - public methods
  - public properties
  - public fields
  - public events
  - public constructors
- Public methods must document parameters and return values using `<param>` and `<returns>` where applicable.
- If a public member can throw expected domain or validation exceptions, include `<exception>` documentation.

## ASP.NET Endpoint Documentation (OpenAPI/Swagger)
- Every REST or other ASP.NET endpoint must include OpenAPI/Swagger documentation.
- Endpoint docs must clearly describe:
  - purpose/summary
  - request contract
  - response contract per status code
  - validation or error responses (for example 400/404/422)
- Keep endpoint documentation aligned with actual behavior and response types.

## gRPC Documentation
- gRPC contracts and operations must be clearly documented for readers.
- For `.proto` files, add concise comments for:
  - each service
  - each RPC
  - important request/response message fields
- For C# gRPC service implementations, keep XML docs on public classes and public members.
- Keep `.proto` comments and implementation behavior aligned.

## Quality Gate For Documentation
- Do not consider a change complete if new or changed public API surface lacks required docs.
- When updating endpoint behavior, update OpenAPI/Swagger documentation in the same change.
- When updating gRPC behavior or contracts, update corresponding docs in the same change.
