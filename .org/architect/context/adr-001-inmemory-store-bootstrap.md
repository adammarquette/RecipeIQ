# ADR-001: InMemoryStore Bootstrap

- Status: Superseded
- Date: 2025-01-01
- Superseded by: ADR-004

## Context

RecipeIQ needed a fast bootstrap persistence layer to unblock service and API scaffolding.

## Decision

Use a shared in-memory store for initial persistence during foundation phase.

## Consequences

- Fast initial delivery and simple test setup.
- Not suitable for multi-instance or durable production workloads.

## Implementation Evidence

- In-memory registration still exists in `src/MarqSpec.RecipeIQ.Api/Program.cs`.
- In-memory usage exists in service implementations under `src/MarqSpec.RecipeIQ.Core/Services/`.
