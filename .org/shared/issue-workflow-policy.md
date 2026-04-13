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

## PM Label Transitions

PM applies label transitions based on agent comments:

- `Starting:` => set `status:in-progress`
- `Blocked:` => set `status:blocked`
- `Unblocked:` => set `status:in-progress`
- `Done:` => set `status:review` or next `status:ready` for routed agent

## Parallel Stage Rule

After Architect posts `Done:`, PM assigns both `agent:backend` and `agent:qa` and sets `status:ready`.

## Required Links In Every Feature Issue

- PRD path under `.org/research/context/`
- ADR path under `.org/architect/context/` once available
- Related PR link(s) once implementation starts
