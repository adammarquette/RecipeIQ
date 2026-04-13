# Issue Workflow Policy

This file is the canonical policy for GitHub Issue routing and labels.

## Ownership

- PM is the only role that edits issue labels.
- PM is the only role that transitions ownership between agents.
- Agents never edit `agent:*` or `status:*` labels.

## Agent Comment Contract

Agents communicate state through comments only:

- `Starting: <approach summary>`
- `Blocked: <blocker summary>`
- `Unblocked: <resolution summary>`
- `Done: <deliverable summary with links>`

## Human Review Gates

`status:awaiting-human-review` is used at two mandatory pause points:

1. **Scope gate** — PM sets on every new issue. No agent assigned until a human comments `Approved: proceed`.
2. **UX design gate** — PM sets after UX Designer posts `Done:`. Architect is not assigned until a human approves the designs.

In both cases: human approves with `Approved: proceed`; rejects with feedback and PM resets to `status:awaiting-human-review`.

## PM Label Transitions

PM applies label transitions based on human approvals and agent comments:

| Trigger | PM action |
| ------- | --------- |
| Issue opened | set `status:awaiting-human-review`; no agent label yet |
| Human `Approved: proceed` (scope gate) | `type:frontend` → set `agent:ux` + `status:ready`; otherwise set `agent:architect` + `status:ready` |
| Agent `Starting:` | set `status:in-progress` |
| Agent `Blocked:` | set `status:blocked` |
| Agent `Unblocked:` | set `status:in-progress` |
| UX `Done:` | set `status:awaiting-human-review` (UX design gate); remove `agent:ux` |
| Human `Approved: proceed` (UX design gate) | set `agent:architect` + `status:ready` |
| Agent `Done:` (non-UX) | set `status:review`; route to next agent with `status:ready` |

## Parallel Stage Rule

After Architect posts `Done:`, PM assigns both `agent:backend` and `agent:qa` and sets `status:ready`.

## Required Links In Every Feature Issue

- PRD path under `.org/research/context/`
- ADR path under `.org/architect/context/` once available
- Related PR link(s) once implementation starts
