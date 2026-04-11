# Shared Conventions

All agents operating in the RecipeIQ software factory follow these conventions.

**Authoritative reference**: [.NET C# Coding Conventions — Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)

---

## Project Structure

```
src/MarqSpec.RecipeIQ.Api/
  Controllers/        # One controller per marketplace participant
  Program.cs          # Composition root — DI registrations here

src/MarqSpec.RecipeIQ.Core/
  Models/             # Domain entities (plain C# classes, no framework deps)
  Services/           # Domain services + interfaces
```

- **Namespaces**: `MarqSpec.RecipeIQ.Api.*` for presentation, `MarqSpec.RecipeIQ.Core.*` for domain
- One type per file; filename matches type name
- `RecipeIQ.Core` must not reference ASP.NET or any infrastructure library

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
|-----------|-----------|---------|
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

## API Design

- RESTful resource-oriented endpoints
- Controllers named after domain participants: Recipes, Creators, Orders, Retailers, Platform
- Return `IActionResult` / `ActionResult<T>` from controller actions
- Standard HTTP status codes: 200, 201, 400, 404, 422

---

## Testing

- Framework: xUnit
- No mocking of domain services — test against `InMemoryStore` directly
- Test class naming: `{SubjectUnderTest}Tests`
- One test file per service; tests live in `tests/MarqSpec.RecipeIQ.Tests/`
- Test method names as sentences: `PlaceOrder_WithValidRecipe_ReturnsConfirmedOrder`

---

## Diagrams

- All diagrams authored in **Mermaid** format inside `.md` files
- Architecture diagrams live in `.docs/`
- Agent working diagrams live in `.org/<agent>/context/`
- No image files — diagrams are always source-controlled as text

---

## Git — Gitflow Workflow

Reference: [.docs/branching-strategy.md](../../.docs/branching-strategy.md) | [Atlassian Gitflow](https://www.atlassian.com/git/tutorials/comparing-workflows/gitflow-workflow)

| Branch | Branches from | Merges into | Purpose |
|--------|--------------|-------------|---------|
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

- Each agent writes working context to `.org/<agent>/context/`
- Context files are `.md` with Mermaid diagrams where helpful
- Context is not ephemeral — it persists across sessions as working memory
