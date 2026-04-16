---
description: "Use when creating or modifying C# backend configuration in __PROJECT_NAME__. Enforces the Options pattern, startup validation, and secure handling of sensitive values through environment-based configuration sources."
name: __PROJECT_NAME__ Configuration And Options Standards
applyTo:
  - "src/**/*.cs"
  - "src/**/appsettings*.json"
---
# Configuration Standards

Apply these rules for all backend configuration changes.

## Options Pattern Required
- Use the Options pattern for configurable application data.
- Define a strongly typed options class per configuration section.
- Bind options in Program.cs using section-based configuration binding.
- Do not inject IConfiguration outside Program.cs unless there is a documented exception.

## Options Registration And Validation
- Register options with the dependency injection container in Program.cs.
- Validate options at startup using ValidateDataAnnotations and ValidateOnStart.
- Treat missing or invalid required configuration as a startup failure.

## Selecting Options Interfaces
- Use IOptions<T> for singleton consumers.
- Use IOptionsSnapshot<T> for scoped or request-specific consumers.
- Use IOptionsMonitor<T> when runtime change tracking is explicitly required.

## Sensitive Configuration Values
- Sensitive values must be loaded from environment-based configuration sources, not hardcoded in code.
- Do not commit secrets to appsettings files or source code.
- Support standard environment variable mapping for nested sections (double underscore convention).
- Keep non-sensitive defaults in appsettings files and override sensitive values per environment.

## Quality Gate
- Do not consider configuration work complete if new configurable behavior bypasses Options pattern.
- Do not consider work complete if sensitive values are stored in source-controlled configuration files.
- Keep options classes, binding, and validation changes in the same pull request when introducing new settings.

