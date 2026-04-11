# Shared Conventions

All agents operating in the RecipeIQ software factory follow these conventions.

## Code Style

- **Language**: C# (latest stable, LTS preferred)
- **Naming**: PascalCase for types/methods, camelCase for locals/parameters, `I` prefix for interfaces
- **File layout**: One type per file, filename matches type name
- **Namespaces**: `RecipeIQ.Api.*` for presentation, `RecipeIQ.Core.*` for domain

## Project Structure

```
src/RecipeIQ.Api/
  Controllers/        # One controller per marketplace participant
  Program.cs          # Composition root — DI registrations here

src/RecipeIQ.Core/
  Models/             # Domain entities (plain C# classes, no framework deps)
  Services/           # Domain services + interfaces
```

## API Design

- RESTful resource-oriented endpoints
- Controllers named after domain participants: Recipes, Creators, Orders, Retailers, Platform
- Return `IActionResult` / `ActionResult<T>` from controller actions
- Use standard HTTP status codes (200, 201, 400, 404, 422)

## Testing

- Framework: xUnit
- No mocking of domain services — test against `InMemoryStore` directly
- Test class naming: `{SubjectUnderTest}Tests`
- One test file per service
- Tests live in `tests/RecipeIQ.Tests/`

## Diagrams

- All diagrams authored in **Mermaid** format inside `.md` files
- Architecture diagrams live in `.docs/`
- Agent working diagrams live in `.org/<agent>/context/`
- No image files — diagrams are always source-controlled as text

## Git

- Branch: `feature/<short-description>` off `main`
- PRs require Claude Code Review workflow pass before merge
- Commit messages: imperative mood, concise, no period at end
- No force-pushing to `main`

## Agent Context Files

- Each agent writes working context to `.org/<agent>/context/`
- Context files are `.md` with Mermaid diagrams where helpful
- Context is not ephemeral — it persists across sessions as working memory
