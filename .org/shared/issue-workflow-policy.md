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

## Human Review Gate

Every new issue opens in `status:awaiting-human-review`. No agent is assigned and no work starts until a human comments `Approved: proceed`. PM then routes to the first agent and sets `status:ready`.

If a human rejects or redirects, PM updates the issue body with the feedback and resets to `status:awaiting-human-review`.

## PM Label Transitions

PM applies label transitions based on human approvals and agent comments:

| Trigger | PM action |
| ------- | --------- |
| Issue opened | set `status:awaiting-human-review`; no agent label yet |
| Human `Approved: proceed` | set `status:ready` + `agent:architect` |
| Agent `Starting:` | set `status:in-progress` |
| Agent `Blocked:` | set `status:blocked` |
| Agent `Unblocked:` | set `status:in-progress` |
| Agent `Done:` | set `status:review`; route to next agent with `status:ready` |

## Parallel Stage Rule

After Architect posts `Done:`, PM assigns both `agent:backend` and `agent:qa` and sets `status:ready`.

## Required Links In Every Feature Issue

- PRD path under `.org/research/context/`
- ADR path under `.org/architect/context/` once available
- Related PR link(s) once implementation starts
