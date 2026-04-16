# ADR-001: InMemoryStore Bootstrap

- Status: Superseded
- Date: 2025-01-01
- Superseded by: ADR-004

## Context

__PROJECT_NAME__ needed a fast bootstrap persistence layer to unblock service and API scaffolding.

## Decision

Use a shared in-memory store for initial persistence during foundation phase.

## Consequences

- Fast initial delivery and simple test setup.
- Not suitable for multi-instance or durable production workloads.

## Implementation Evidence

- In-memory registration still exists in `src/__ROOT_NAMESPACE__.Api/Program.cs`.
- In-memory usage exists in service implementations under `src/__ROOT_NAMESPACE__.Core/Services/`.

