# Shared Conventions

All agents operating in the RecipeIQ software factory follow these conventions.

**Authoritative reference**: [.NET C# Coding Conventions — Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)

**Target framework**: .NET 10 — all projects must target `net10.0`

---

## Project Structure

```text
src/MarqSpec.RecipeIQ.Api/
  Controllers/        # One controller per marketplace participant
  Program.cs          # Composition root — DI registrations here

src/MarqSpec.RecipeIQ.Core/
  Models/             # Domain entities (plain C# classes, no framework deps)
  Services/           # Domain services + interfaces

src/MarqSpec.RecipeIQ.Data/
  Entities/           # EF Core entity models (mapped to DB schema)
  Migrations/         # EF Core migrations (auto-generated; do not edit manually)
  RecipeIQDbContext.cs  # Single DbContext for the solution
```

- **Namespaces**: `MarqSpec.RecipeIQ.Api.*` for presentation, `MarqSpec.RecipeIQ.Core.*` for domain, `MarqSpec.RecipeIQ.Data.*` for data access
- One type per file; filename matches type name
- `MarqSpec.RecipeIQ.Core` must not reference ASP.NET or any infrastructure library
- `MarqSpec.RecipeIQ.Data` owns all EF Core concerns; no entity models or `DbContext` types may live outside this project
- `MarqSpec.RecipeIQ.Core` may reference `MarqSpec.RecipeIQ.Data` but not `MarqSpec.RecipeIQ.Api`

---

## Layout

- Four spaces for indentation — no tab characters
- One statement per line; one declaration per line
- **Allman brace style**: open and closing brace each on their own line, aligned with current indentation
- Limit lines to 65 characters where practical; break long statements across lines
- Line breaks before binary operators when wrapping
- At least one blank line between method and property definitions
- Use parentheses to make compound conditions explicit: `if ((x > 0) && (y > 0))`

---

## Naming

| Construct | Convention | Example |
| --------- | ---------- | ------- |
| Types, methods, properties, events | PascalCase | `RecipeDiscoveryService` |
| Local variables, parameters | camelCase | `recipeId` |
| Interfaces | `I` prefix + PascalCase | `IRecipeDiscoveryService` |
| Primary ctor params on `record` | PascalCase | `record Person(string FirstName, ...)` |
| Primary ctor params on `class`/`struct` | camelCase | `class Foo(string label)` |
| Static members | Referenced via class name | `InMemoryStore.Instance` |

---

## Language Features

### Types and variables

- Use language keywords, not runtime types: `string` not `String`, `int` not `Int32`
- Use `int` over unsigned types unless the domain specifically requires unsigned
- Use `var` only when the type is obvious from the right-hand side (e.g., `new`, explicit cast, literal)
- Do not use `var` when the type comes from a method return — spell it out
- Use implicit typing for `for` loop variables; use explicit typing for `foreach` loop variables
- Avoid `var` in place of `dynamic`; use `dynamic` only when run-time type inference is the goal

### Strings

- Use string interpolation for short concatenations: `$"{firstName} {lastName}"`
- Use `StringBuilder` for string building inside loops over large data
- Prefer raw string literals over escape sequences or verbatim strings

### Collections and initialization

- Use collection expressions to initialize collections: `string[] vowels = ["a", "e", "i", "o", "u"];`
- Use object initializers to simplify object creation
- Use `required` properties instead of constructors to force initialization where appropriate

### Delegates

- Use `Func<>` and `Action<>` instead of defining custom delegate types
- Use concise delegate instantiation syntax; avoid `new Del(Method)` when `Method` alone suffices
- Use lambda expressions for event handlers that don't need to be removed

### Exceptions

- Use `try-catch` for exception handling; catch specific exception types — never catch bare `Exception` without a filter
- Use `using` (braceless form preferred) instead of `try-finally` when the only `finally` code is `Dispose()`
- Only catch exceptions that can be properly handled

### Operators and conditionals

- Use `&&` and `||` (short-circuit) instead of `&` and `|` for boolean comparisons
- Use the `new()` target-typed form or `var` for object instantiation — avoid repeating the full type on both sides

### Async

- Use `async`/`await` for all I/O-bound operations
- Be aware of deadlocks; use `ConfigureAwait` where appropriate

### Namespaces and usings

- Use **file-scoped namespace declarations**: `namespace MarqSpec.RecipeIQ.Core.Services;`
- Place `using` directives **outside** the namespace declaration

### LINQ

- Use LINQ for collection manipulation to improve readability
- Use meaningful query variable names (e.g., `matchingRecipes`, not `q`)
- Use implicit typing for query and range variables
- Align query clauses under the `from` clause
- Place `where` clauses before other clauses
- Use Pascal case for anonymous type properties; rename ambiguous properties

### Static members

- Always qualify static members with the class name, never a derived class name

---

## Comments

- Use `//` for single-line comments; avoid `/* */` for explanation
- Place comments on their own line above the code they describe — not at end of line
- Begin comment text with a capital letter; end with a period
- One space between `//` and comment text
- Use XML doc comments (`///`) for all public members, types, and interfaces

---

## Git — Gitflow Workflow

Reference: [.docs/branching-strategy.md](../../.docs/branching-strategy.md) | [Atlassian Gitflow](https://www.atlassian.com/git/tutorials/comparing-workflows/gitflow-workflow)

| Branch | Branches from | Merges into | Purpose |
| ------ | ------------- | ----------- | ------- |
| `main` | `release/*`, `hotfix/*` | — | Production releases only; tagged |
| `develop` | `main` (once) | `main` via release | Integration branch; all features land here |
| `feature/<name>` | `develop` | `develop` | One feature or task per branch |
| `release/<version>` | `develop` | `main` + `develop` | Release stabilization; version bump |
| `hotfix/<name>` | `main` | `main` + `develop` | Urgent production fixes only |

- **Always branch from `develop`** for new feature work: `git checkout -b feature/<name> origin/develop`
- Feature branch naming: `feature/<short-kebab-description>`
- PRs to `develop` require Claude Code Review pass
- PRs to `main` require human approval — never merge without it
- Never commit directly to `main` or `develop`
- Commit messages: imperative mood, concise, no period at end
- No force-pushing to `main` or `develop`
- Delete feature branches after merge

---

## Agent Context Files

- Each agent writes working notes and reference documents to `.org/<agent>/context/`
- Context files are `.md` with Mermaid diagrams where helpful
- Context is not ephemeral — it persists across sessions as working memory
- **Never embed volatile working state in a CLAUDE.md persona file** — put it in `context/` instead
- Context files are reference material (PRDs, ADRs, design notes) — they are **not** the coordination mechanism between agents

---

## GitHub Issue Workflow

GitHub Issues are the **single source of truth** for all inter-agent coordination. Status, progress, blockers, decisions, and handoffs are all tracked as issue comments and labels. Context files in `.org/<agent>/context/` are reference documents linked from issues — not triggers for work.

### Tooling

All agents use the `gh` CLI for issue interaction:

```bash
gh issue list --label "agent:architect,status:ready"
gh issue view <number>
gh issue comment <number> --body "..."
gh issue edit <number> --add-label "status:in-progress" --remove-label "status:ready" # PM only
```

Label ownership and transition authority are defined in `.org/shared/issue-workflow-policy.md`.

### Workflow sequence

```text
1. Product Owner — writes PRD to .org/research/context/; notifies PM via issue comment or new issue
2. PM            — opens GitHub Issue, links PRD, adds acceptance criteria and assumptions,
                   sets status:awaiting-human-review
3. Human review  — reviews issue scope, acceptance criteria, and assumptions; approves by commenting
                   "Approved: proceed" or rejects with feedback
4. PM            — on approval, sets status:ready + agent:architect
5. Architect     — reads issue + linked PRD; writes ADR to .org/architect/context/;
                   comments on issue with ADR link and interface contract summary;
                   PM re-assigns: status:ready + agent:backend + agent:qa
6. Backend ↔ QA  — run in parallel; each reads issue + prior comments + linked reference docs
7. Backend       — comments on issue when implementation is complete; opens PR linked to issue
8. QA            — comments on issue when tests are complete; opens PR linked to issue
9. PM            — verifies acceptance criteria checked off; closes issue on final merge
```

### Human review

`status:awaiting-human-review` is a mandatory pause state. PM sets it when opening any new issue. No agent is assigned and no work begins until a human approves.

**To approve**: comment `Approved: proceed` on the issue. PM will route to the first agent.
**To reject or redirect**: comment with feedback. PM will update the issue body and reset to `status:awaiting-human-review`.

This is the primary human guardrail — it ensures a human has reviewed scope and acceptance criteria before any agent begins work.

### Agent comment protocol

Every agent must follow this pattern when working an issue:

| Event | Action |
| ----- | ------ |
| Starting work | Comment: `Starting: [brief description of approach]`; PM applies label transitions |
| Blocked | Comment: `Blocked: [description]`; PM applies label transitions |
| Unblocked | Comment: `Unblocked: [description]`; PM applies label transitions |
| Work complete | Comment: `Done: [summary of output, links to PRs or reference files]`; PM applies label transitions |

### Reads before starting

| Agent | Must read before starting work |
| ----- | ------------------------------ |
| Architect | Issue body + linked PRD in `.org/research/context/` |
| Backend | Issue body + all prior comments; linked ADR in `.org/architect/context/` |
| QA | Issue body + all prior comments; linked ADR in `.org/architect/context/` |
| Platform | Issue body + all prior comments; `.docs/architecture.md` for deployment target |

### Reference file naming (context folders)

Context files are reference documents, not handoff triggers. Name them consistently so they are easy to link from issues:

| Document type | File name pattern | Example |
| ------------- | ----------------- | ------- |
| PRD / feature brief | `prd-<feature>.md` | `prd-cook-profile.md` |
| ADR | `adr-<nnn>-<topic>.md` | `adr-005-auth-strategy.md` |
| Pipeline / infra notes | `infra-<topic>.md` | `infra-azure-deploy.md` |
