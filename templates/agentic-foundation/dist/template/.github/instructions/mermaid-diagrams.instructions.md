---
description: "Use when creating or modifying documentation diagrams in __PROJECT_NAME__ markdown files. Enforces Mermaid-only diagrams that are markdown-compatible and source-controlled as text."
name: __PROJECT_NAME__ Mermaid Diagram Standards
applyTo:
  - ".docs/**/*.md"
  - ".org/**/*.md"
  - "README.md"
---
# Mermaid Diagram Standards

Apply these rules for all documentation diagrams.

## Diagram Format
- All diagrams must be authored in Mermaid format.
- Diagrams must be embedded in markdown using fenced code blocks with the `mermaid` info string.
- Diagram definitions must remain plain text in source control.

## Required Diagram Model Coverage
- Use C4-style diagrams (Context, Container, and Component where relevant) for system architecture communication.
- Use ERD diagrams for data modeling and entity relationship documentation.
- Use UML-style diagrams (for example class, sequence, or state diagrams) when modeling behavior and structure.
- Choose the model type that best fits the documentation purpose, and include these model families across architecture documentation as needed.

## Markdown Compatibility
- Use Mermaid syntax that is compatible with markdown rendering environments used by the repository.
- Keep Mermaid blocks valid and renderable without external image assets.
- Do not replace Mermaid diagrams with image files for architecture or workflow documentation.

## Placement
- Architecture and domain diagrams belong in `.docs/` markdown files.
- Agent working diagrams belong in `.org/<agent>/context/` markdown files.

## Quality Gate
- Do not consider documentation updates complete when a required diagram is provided as a non-Mermaid image.
- Do not consider diagram updates complete when Mermaid syntax is invalid or not markdown-compatible.

