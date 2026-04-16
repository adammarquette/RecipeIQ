---
description: "Use when creating or modifying C# backend services, controllers, or domain workflows in __PROJECT_NAME__. Enforces Microsoft.Extensions.Logging usage, environment-appropriate log levels, sensitive-data exclusions, and username masking rules."
name: __PROJECT_NAME__ Logging And Sensitive Data Standards
applyTo:
  - "src/**/*.cs"
---
# Logging Standards

Apply these rules to all public method implementations in backend C# code.

## Logging Framework
- Use Microsoft.Extensions.Logging via dependency injection.
- Use ILogger<TCategoryName> in classes that emit logs.
- Do not use Console.WriteLine or other ad hoc logging for application diagnostics.

## Public Method Logging
- Public methods must include meaningful logging for entry, significant decisions, and failures.
- Use structured logging with named properties.
- Prefer message templates over string interpolation.

## Log Level Guidance
- Use LogTrace for low-level diagnostics and high-volume detail.
- Use LogDebug for development-time diagnostic context.
- Use LogInformation for expected business flow milestones.
- Use LogWarning for recoverable issues and degraded behavior.
- Use LogError for failures of a specific operation.
- Use LogCritical for unrecoverable failures requiring urgent intervention.
- Choose levels based on environment and signal-to-noise expectations.

## Sensitive Data Protection
- Never log secrets, tokens, credentials, payment data, or other sensitive data.
- Never log raw personally identifiable information when masking or omission is possible.
- Log identifiers only when required for diagnostics and use minimal exposure.

## Username Masking Rule
- Any username included in logs must be masked after the first three characters.
- Example transformation:
  - abcdefgh becomes abc*****
  - john.doe becomes joh*****
- For usernames with length less than or equal to three characters, mask the entire value.

## Quality Gate
- Do not consider a change complete if new or changed public methods lack required logging.
- Do not approve a change if logs include sensitive data or unmasked usernames.

