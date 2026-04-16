# ADR-003: One Controller Per Marketplace Participant

- Status: Accepted
- Date: 2025-01-10

## Context

__PROJECT_NAME__ is a four-sided marketplace and needs API boundaries aligned to participant concepts.

## Decision

Organize API surface as one controller per participant concept.

## Consequences

- API structure aligns with domain language.
- Clear ownership boundaries for endpoint evolution.

## Implementation Evidence

- Controllers exist for recipes, creators, orders, retailers, and platform under `src/__ROOT_NAMESPACE__.Api/Controllers/`.

