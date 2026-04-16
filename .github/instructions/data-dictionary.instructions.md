---
description: "Use when creating or modifying persistent data models, entities, schemas, or storage contracts in RecipeIQ. Requires a data dictionary with field name, type, and description, including nested or embedded JSON/subdocument fields."
name: RecipeIQ Data Dictionary Standards
applyTo:
  - "src/**/*.cs"
  - ".docs/**/*.md"
  - "**/*.json"
---
# Data Dictionary Standards

Apply these rules whenever persistent data structures are introduced or changed.

## Required Data Dictionary Coverage
- All persistent data must be documented in a data dictionary.
- Each field must include:
  - Name
  - Type
  - Description
- Documentation must cover entities, value objects, and storage-facing contracts that persist data.

## Nested And Embedded Structures
- If a model contains subdocuments, embedded JSON, or nested objects, document every nested field.
- Represent nested fields using a consistent path format (for example `Profile.Address.City` or `metadata.preferences.notifications.email`).
- Include type and description for every nested field, not only top-level objects.

## Consistency Requirements
- Keep dictionary entries aligned with code and schema names.
- Update the data dictionary in the same change as model/schema updates.
- Use precise business descriptions, not placeholder text.

## Quality Gate
- Do not consider persistent model changes complete if data dictionary entries are missing.
- Do not consider changes complete when nested/embedded fields are undocumented.
- Do not merge schema/model updates with stale or mismatched dictionary definitions.
