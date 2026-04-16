---
description: "Use when creating or modifying ASP.NET Web API code in RecipeIQ. Enforces FluentValidation for request validation and DI-based validator registration."
name: RecipeIQ FluentValidation And DI Standards
applyTo:
  - "src/MarqSpec.RecipeIQ.Api/**/*.cs"
  - "src/MarqSpec.RecipeIQ.Core/**/*.cs"
---
# FluentValidation Standards

Apply these rules for Web API request validation.

## Validation Framework
- Use FluentValidation for Web API validation logic.
- Prefer validator classes over inline validation in controllers or services.
- Define one validator per request model or command where validation rules are required.
- Downstream methods may use inline guard validation to ensure sanity of provided parameter values.
- Inline guard checks in downstream methods do not replace FluentValidation for API request validation.

## Dependency Injection
- Register validators through dependency injection in Program.cs.
- Use assembly scanning registration where practical to keep validator wiring maintainable.
- Do not instantiate validators manually in controllers or services.

## API Behavior
- Ensure validation failures are surfaced through consistent API responses.
- Use appropriate HTTP status codes for all REST API error responses, including but not limited to validation errors.
- At minimum, map common error categories to appropriate codes (for example 400/404/409/422/500) based on actual failure semantics.
- Keep error response payloads consistent across endpoints for client predictability.
- Keep validation responsibilities separate from business logic.
- Keep rules deterministic and environment-safe.

## REST Error Status Code Mapping
- Use this mapping unless a documented endpoint-specific exception exists.
- `400 Bad Request`: malformed request syntax, invalid query/path/header format, or missing required request components.
- `401 Unauthorized`: missing or invalid authentication credentials.
- `403 Forbidden`: authenticated caller lacks required permission.
- `404 Not Found`: addressed resource does not exist.
- `409 Conflict`: request conflicts with current resource state (for example duplicate creation, version conflict).
- `422 Unprocessable Entity`: request is syntactically valid but fails business or validation rules.
- `429 Too Many Requests`: throttling or rate-limit policy violation.
- `500 Internal Server Error`: unexpected server-side failure.
- `503 Service Unavailable`: temporary dependency outage or maintenance condition.
- Do not return `200` for failed operations.

## Quality Gate
- Do not consider API changes complete when new request validation bypasses FluentValidation.
- Do not consider changes complete when validators are not registered via DI.
- Do not consider changes complete when error paths return incorrect or inconsistent HTTP status codes.
