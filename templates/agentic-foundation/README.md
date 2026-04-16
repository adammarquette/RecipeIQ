# Agentic Foundation Template

This folder defines a reusable template for the governance/agent assets in this repository.

## What Gets Templated

- `.github/agents`
- `.github/instructions`
- `.github/workflows`
- `.github/scripts`
- `.org`
- `.docs`
- `.github/copilot-instructions.md`
- `CLAUDE.md`

## Placeholders

The export step replaces repository-specific identifiers with placeholders:

- `__PROJECT_NAME__`
- `__ROOT_NAMESPACE__`
- `__GITHUB_OWNER__`
- `__REPOSITORY_NAME__`
- `__GITHUB_REPO_SLUG__`

## Usage

1. Export from this repo into a portable template snapshot:

```powershell
pwsh ./scripts/Export-AgenticTemplate.ps1
```

2. Apply the generated template to another repo:

```powershell
pwsh ./scripts/Apply-AgenticTemplate.ps1 \
  -TargetRoot "C:/path/to/other-repo" \
  -ProjectName "OtherProject" \
  -RootNamespace "Contoso.OtherProject" \
  -GitHubOwner "contoso" \
  -RepositoryName "other-project"
```

3. Commit generated files in the target repository.
