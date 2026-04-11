# RecipeIQ — Branching Strategy

RecipeIQ follows the **Gitflow workflow** as defined by Atlassian:
https://www.atlassian.com/git/tutorials/comparing-workflows/gitflow-workflow

This model supports parallel agent development across isolated feature branches while keeping `main` stable for production releases.

---

## Branch Model

```mermaid
gitGraph
    commit id: "initial"
    branch develop
    checkout develop
    commit id: "dev baseline"

    branch feature/recipe-search
    checkout feature/recipe-search
    commit id: "add search filters"
    commit id: "add ranking logic"
    checkout develop
    merge feature/recipe-search id: "PR: recipe search"

    branch feature/ef-core-persistence
    checkout feature/ef-core-persistence
    commit id: "add DbContext"
    commit id: "replace InMemoryStore"
    checkout develop
    merge feature/ef-core-persistence id: "PR: ef core"

    branch release/1.0.0
    checkout release/1.0.0
    commit id: "bump version"
    commit id: "release notes"
    checkout main
    merge release/1.0.0 id: "v1.0.0" tag: "v1.0.0"
    checkout develop
    merge release/1.0.0 id: "back-merge release"

    branch hotfix/order-null-ref
    checkout hotfix/order-null-ref
    commit id: "fix null ref"
    checkout main
    merge hotfix/order-null-ref id: "hotfix" tag: "v1.0.1"
    checkout develop
    merge hotfix/order-null-ref id: "back-merge hotfix"
```

---

## Branch Types

### `main`
- **Contains**: production-ready, released code only
- **Protected**: no direct commits; only merges from `release/*` and `hotfix/*`
- **Tagged**: every merge to `main` is tagged with a version (`v1.0.0`)
- **Requires**: human approval before merge

### `develop`
- **Contains**: latest integrated development work
- **The integration branch**: all feature branches merge here via PR
- **Source of truth** for agents — always branch feature work from here
- **Requires**: Claude Code Review pass before merge

### `feature/<name>`
- **Branches from**: `develop`
- **Merges into**: `develop` via PR
- **Naming**: `feature/<short-kebab-description>` (e.g., `feature/ef-core-persistence`)
- **Lifetime**: deleted after PR merges
- **Parallel-safe**: multiple agents can work on separate feature branches simultaneously

### `release/<version>`
- **Branches from**: `develop` when scope for a release is complete
- **Merges into**: `main` AND `develop`
- **Naming**: `release/<semver>` (e.g., `release/1.0.0`)
- **Purpose**: final stabilization, version bump, release notes — no new features
- **Requires**: human approval to merge to `main`

### `hotfix/<name>`
- **Branches from**: `main` (the specific release tag)
- **Merges into**: `main` AND `develop`
- **Naming**: `hotfix/<short-description>` (e.g., `hotfix/order-null-ref`)
- **Purpose**: urgent production fixes only — bypass the develop cycle
- **Requires**: human approval to merge to `main`

---

## Parallel Agent Workflow

Each agent works on its own feature branch. Branches are isolated, so agents don't block each other.

```mermaid
flowchart TD
    Dev[develop]

    Dev --> F1[feature/ef-core-persistence\nBackend Engineer]
    Dev --> F2[feature/auth-middleware\nBackend Engineer]
    Dev --> F3[feature/retailer-service-tests\nQA Engineer]
    Dev --> F4[feature/build-pipeline\nPlatform Engineer]

    F1 -->|PR + review| Dev
    F2 -->|PR + review| Dev
    F3 -->|PR + review| Dev
    F4 -->|PR + review| Dev

    Dev -->|release branch| Rel[release/1.0.0]
    Rel -->|human approval| Main[main]
    Rel -->|back-merge| Dev
```

**Rules for agents working in parallel:**
- Branch from the latest `develop` at the start of work
- Keep feature branches short-lived — small, focused scope
- Rebase on `develop` if the branch diverges significantly before opening a PR
- Never merge directly — always via PR with Claude Code Review

---

## PR Rules

| PR Target | Branch Source | Required Checks | Approver |
|-----------|--------------|----------------|----------|
| `develop` | `feature/*` | Claude Code Review | Automated (CI pass) |
| `main` | `release/*` | Claude Code Review + build | Human required |
| `main` | `hotfix/*` | Claude Code Review + build | Human required |
| `develop` | `hotfix/*` | Claude Code Review | Automated (CI pass) |

---

## Release Cycle

```mermaid
sequenceDiagram
    participant Agent as Agent(s)
    participant Dev as develop
    participant Rel as release/x.y.z
    participant Main as main
    participant Human as Human Reviewer

    Agent->>Dev: Feature PRs (automated review)
    Note over Dev: Integration & testing
    Dev->>Rel: Branch when scope is complete
    Note over Rel: Version bump, release notes
    Rel->>Human: PR to main — human approval required
    Human-->>Main: Approve & merge
    Main->>Main: Tag vX.Y.Z
    Rel->>Dev: Back-merge release branch
```

---

## Version Tagging

- Tags follow semantic versioning: `vMAJOR.MINOR.PATCH`
- Every merge to `main` must be tagged
- Tag message should summarize the release scope
