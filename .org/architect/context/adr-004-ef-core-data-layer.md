# ADR-004: EF Core Data Layer Introduction

- Status: Accepted (Partially Implemented)
- Date: 2025-02-01

## Context

RecipeIQ requires durable persistence and clear infrastructure boundaries beyond in-memory state.

## Decision

Introduce `MarqSpec.RecipeIQ.Data` with `RecipeIQDbContext` and EF Core packages.

## Consequences

- Infrastructure concerns are isolated from API project.
- Persistence migration can proceed incrementally by replacing InMemoryStore dependencies.

## Implementation Evidence

- Data project exists: `src/MarqSpec.RecipeIQ.Data/`.
- DbContext exists: `src/MarqSpec.RecipeIQ.Data/RecipeIQDbContext.cs`.
- InMemoryStore still wired in API/Core, so migration is not complete.
