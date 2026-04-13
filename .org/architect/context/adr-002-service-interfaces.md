# ADR-002: Service Interfaces for Domain Services

- Status: Accepted
- Date: 2025-01-05

## Context

Parallel implementation and testing require stable contracts independent from concrete classes.

## Decision

Define and maintain `I*Service` interfaces for core domain services.

## Consequences

- Better dependency inversion and DI clarity.
- Enables Backend and QA to work in parallel with stable contracts.

## Implementation Evidence

- Interface contracts exist in `src/MarqSpec.RecipeIQ.Core/Services/`.
- Implementations are wired via DI in `src/MarqSpec.RecipeIQ.Api/Program.cs`.
