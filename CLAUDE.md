# RecipeIQ — Agentic Development Organization

RecipeIQ is a personalized home cooking platform connecting home cooks, recipe creators, retailers, and the platform itself in a four-sided marketplace. It matches people to recipes suited to their moment, household, dietary needs, and budget, with seamless ingredient fulfillment and a creator ecosystem powered by real demand signal.

## Solution Structure

```text
RecipeIQ/
├── CLAUDE.md                  # This file — org overview and agent roster
├── .docs/                     # Master architecture and planning documents
│   ├── architecture.md        # System architecture and component diagrams
│   ├── branching-strategy.md  # Gitflow branch model and PR rules
│   ├── domain-model.md        # Domain model and bounded contexts
│   └── roadmap.md             # Feature roadmap and planning
├── .org/                      # Agent personas and shared org context
│   ├── shared/                # Shared conventions and glossary (all agents reference)
│   ├── architect/             # Architect agent persona and context
│   ├── backend/               # Backend engineer agent persona and context
│   ├── qa/                    # QA engineer agent persona and context
│   ├── platform/              # Platform engineer agent persona and context
│   ├── pm/                    # Project Manager agent persona and context
│   └── research/              # Research & Requirements agent persona and context
├── src/
│   ├── MarqSpec.RecipeIQ.Api/   # ASP.NET Core Web API (controllers, entry point)
│   ├── MarqSpec.RecipeIQ.Core/  # Domain models and services
│   └── MarqSpec.RecipeIQ.Data/  # EF Core DbContext, entities, migrations
└── tests/
    └── MarqSpec.RecipeIQ.Tests/ # xUnit integration and unit tests
```

## Agent Roster

| Agent | Persona File | Responsibilities |
| ----- | ------------ | ---------------- |
| Architect | [.org/architect/CLAUDE.md](.org/architect/CLAUDE.md) | System design, ADRs, technical constraints and technology selection, interface contract ownership |
| Backend Engineer | [.org/backend/CLAUDE.md](.org/backend/CLAUDE.md) | .NET/C# API, domain services, data layer, feature implementation |
| QA Engineer | [.org/qa/CLAUDE.md](.org/qa/CLAUDE.md) | Parallel test strategy with Backend, issue-driven assumptions, quality gates, coverage |
| Platform Engineer | [.org/platform/CLAUDE.md](.org/platform/CLAUDE.md) | CI/CD, GitHub Actions, infrastructure, observability |
| Project Manager | [.org/pm/CLAUDE.md](.org/pm/CLAUDE.md) | Sprint planning, GitHub Issue management, cross-agent coordination, roadmap sync |
| Research & Requirements | [.org/research/CLAUDE.md](.org/research/CLAUDE.md) | User research, PRDs, user stories, acceptance criteria, final roadmap authority |

## High-Level Architecture

See [Architecture](.docs/architecture.md) for the full system diagram, component map, ADRs, and deployment target.

## Collaboration Model

- **Research & Requirements** owns roadmap priorities and is the final authority for roadmap decisions.
- **Architect** sets technical direction via `.docs/architecture.md` and `.docs/domain-model.md`, approves technical constraints, chooses technologies, and defines public interfaces before implementation starts.
- **Backend Engineer** owns `src/` and implements features aligned to the architecture.
- **QA Engineer** owns `tests/`, works in parallel with Backend whenever prerequisites are met, and records assumptions from GitHub Issues.
- **Platform Engineer** owns `.github/workflows/` and ensures CI passes before merge.
- **Project Manager** consumes PRDs from Research and ADRs/interface contracts from Architect to create and manage GitHub Issues; coordinates handoffs and tracks sprint state.
- All agents write their working context to their own folder under `.org/<agent>/context/`.
- All diagrams are authored in Mermaid format.
- Shared conventions and domain language live in [.org/shared/](.org/shared/).

## RACI (Role Clarity)

| Work Item | Research | Architect | PM | Backend | QA | Platform |
| --------- | -------- | --------- | -- | ------- | -- | -------- |
| Roadmap priorities and scope | A/R | C | C | I | I | I |
| Technical constraints and technology choice | C | A/R | I | C | C | C |
| Public interface contracts (pre-implementation) | I | A/R | C | C | C | I |
| Feature implementation in `src/` | I | C | I | A/R | C | I |
| Test strategy and verification in `tests/` | C | C | I | C | A/R | C |
| CI/CD quality gates and release automation | I | C | C | I | C | A/R |

Legend: `A` = Accountable, `R` = Responsible, `C` = Consulted, `I` = Informed.

## Key Conventions

- Language: C# / .NET (latest LTS)
- API style: RESTful, controller-per-domain-concept
- Tests: xUnit, no mocking of domain services — prefer integration style against real service implementations
- Diagrams: Mermaid (`.md` files in `.docs/` or agent context folders)
- Branch strategy: Gitflow — `feature/*` → `develop` → `release/*` → `main`; see [.docs/branching-strategy.md](.docs/branching-strategy.md)
- `main` is production-only; all development integrates through `develop`
- Merges to `main` require human approval; merges to `develop` require Claude Code Review pass
