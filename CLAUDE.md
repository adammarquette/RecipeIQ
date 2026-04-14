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
│   ├── research/              # Product Owner agent persona and context
│   ├── security/              # Security engineer agent persona and context
│   └── ux/                    # UX Designer agent persona and context
├── src/
│   ├── MarqSpec.RecipeIQ.Api/     # ASP.NET Core Web API (controllers, entry point)
│   ├── MarqSpec.RecipeIQ.Core/    # Domain models and services
│   ├── MarqSpec.RecipeIQ.Data/    # EF Core DbContext, entities, migrations
│   ├── MarqSpec.RecipeIQ.Web/     # React + MUI web application (planned)
│   └── MarqSpec.RecipeIQ.Mobile/  # React Native mobile app — Android + iOS (planned)
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
| UX Designer | [.org/ux/CLAUDE.md](.org/ux/CLAUDE.md) | UI/UX design, user flow diagrams, MUI component specs, mockups for web and mobile |
| Security Engineer | [.org/security/CLAUDE.md](.org/security/CLAUDE.md) | Threat modeling, dependency/vulnerability governance, secrets/security controls |
| Project Manager | [.org/pm/CLAUDE.md](.org/pm/CLAUDE.md) | Sprint planning, GitHub Issue management, cross-agent coordination, roadmap sync |
| Product Owner | [.org/research/CLAUDE.md](.org/research/CLAUDE.md) | Product vision, backlog prioritization, user research, PRDs, acceptance criteria, final roadmap authority |

## High-Level Architecture

See [Architecture](.docs/architecture.md) for the full system diagram, component map, ADRs, and deployment target.

## Collaboration Model

If something is unclear, ask! Agents are expected to proactively seek clarification from each other via GitHub Issue comments, and to document assumptions in the issue thread for transparency. The Project Manager is the single source of truth for issue transitions and ownership — all work items must be tracked as GitHub Issues, and all progress must be communicated through issue comments.

- **Product Owner** owns the product vision and roadmap priorities, is the final authority on scope and priority decisions, and signals the PM when a PRD is ready for implementation.
- **Architect** sets technical direction via `.docs/architecture.md` and `.docs/domain-model.md`, approves technical constraints, chooses technologies, and defines public interfaces before implementation starts.
- **Backend Engineer** owns `src/` and implements features aligned to the architecture.
- **QA Engineer** owns `tests/`, works in parallel with Backend whenever prerequisites are met, and records assumptions from GitHub Issues.
- **Platform Engineer** owns `.github/workflows/` and ensures CI passes before merge.
- **UX Designer** owns UI/UX design for web (React + MUI) and mobile (React Native). Produces user flows and component specs from PRDs. Work is gated by human review before Architect begins — no frontend feature is implemented without approved designs.
- **Security Engineer** owns security policy guardrails (secrets hygiene, dependency/vulnerability controls, and threat modeling checklists) and advises Platform/Backend on remediation priorities.
- **Project Manager** is the sole transition authority for GitHub Issues — opens issues from PRDs, routes them between agents via labels and comments, and closes them when acceptance criteria are verified. Agents communicate progress exclusively via issue comments.
- All agents write their working context to their own folder under `.org/<agent>/context/`.
- All diagrams are authored in Mermaid format.
- Shared conventions and domain language live in [.org/shared/](.org/shared/).
- Label and transition ownership rules are canonical in [.org/shared/issue-workflow-policy.md](.org/shared/issue-workflow-policy.md).

## RACI (Role Clarity)

| Work Item | Product Owner | UX Designer | Architect | PM | Backend | QA | Platform | Security |
| --------- | ------------- | ----------- | --------- | -- | ------- | -- | -------- | -------- |
| Roadmap priorities and scope | A/R | I | C | C | I | I | I | I |
| UI/UX design and component specs | C | A/R | C | I | C | I | I | I |
| Technical constraints and technology choice | C | I | A/R | I | C | C | C | C |
| Public interface contracts (pre-implementation) | I | C | A/R | C | C | C | I | C |
| Feature implementation in `src/` | I | I | C | I | A/R | C | I | C |
| Test strategy and verification in `tests/` | C | I | C | I | C | A/R | C | C |
| CI/CD quality gates and release automation | I | I | C | C | I | C | A/R | C |
| Secrets and dependency security baseline | I | I | C | I | C | C | C | A/R |

Legend: `A` = Accountable, `R` = Responsible, `C` = Consulted, `I` = Informed.

## Key Conventions

- Language: C# / .NET (latest LTS)
- Web frontend: React + Material UI (MUI)
- Mobile frontend: React Native + React Native Paper — shared codebase for Android and iOS
- API style: RESTful, controller-per-domain-concept
- Tests: xUnit, no mocking of domain services — prefer integration style against real service implementations
- Diagrams: Mermaid (`.md` files in `.docs/` or agent context folders)
- Branch strategy: Gitflow — `feature/*` → `develop` → `release/*` → `master`; see [.docs/branching-strategy.md](.docs/branching-strategy.md)
- `master` is production-only; all development integrates through `develop`
- Merges to `master` require human approval; merges to `develop` require Claude Code Review pass
