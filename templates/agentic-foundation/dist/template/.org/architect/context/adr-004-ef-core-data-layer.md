# ADR-004: EF Core Data Layer Introduction

- Status: Accepted (Partially Implemented)
- Date: 2025-02-01

## Context

__PROJECT_NAME__ requires durable persistence and clear infrastructure boundaries beyond in-memory state.

## Decision

Introduce `__ROOT_NAMESPACE__.Data` with `__PROJECT_NAME__DbContext` and EF Core packages.

## Consequences

- Infrastructure concerns are isolated from API project.
- Persistence migration can proceed incrementally by replacing InMemoryStore dependencies.

## Implementation Evidence

- Data project exists: `src/__ROOT_NAMESPACE__.Data/`.
- DbContext exists: `src/__ROOT_NAMESPACE__.Data/__PROJECT_NAME__DbContext.cs`.
- InMemoryStore still wired in API/Core, so migration is not complete.

