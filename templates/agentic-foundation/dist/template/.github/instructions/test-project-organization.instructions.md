---
description: "Use when creating or modifying tests in __PROJECT_NAME__. Enforces a single development unit test project, namespace-aligned folder organization, and separation of development unit tests from QA integration tests."
name: __PROJECT_NAME__ Test Project Organization Standards
applyTo:
  - "src/**/*.cs"
  - "src/**/*.csproj"
  - "tests/**/*.cs"
  - "tests/**/*.csproj"
---
# Test Project Organization Standards

Apply these rules for all test-related changes.

## Development Unit Test Project Scope
- Development unit test projects must be colocated with other solution projects (for example under `src/`), not in a separate top-level test folder.
- Add new development unit test files to the existing development unit test project unless a project rename is explicitly required.
- Do not create multiple development unit test projects for normal feature work.

## Unit Test Framework And Dependency Policy
- Development unit tests must use xUnit, FakeItEasy, and FluentAssertions.
- Unit tests must not rely on external dependencies or shared external infrastructure.
- Keep unit tests deterministic and fast-running.
- QA integration tests are responsible for validating interactions with external dependencies in a separate integration test project.

## Namespace And Folder Alignment
- Test folder structure must reflect the namespaces of the production classes under test.
- Place each test class in the folder that corresponds to the class-under-test namespace path.
- Keep test namespaces aligned with their folder path for predictable discovery and maintenance.

## Unit Test Naming Conventions
- Use one unit test file per class under test.
- Name the test class as `<ClassUnderTest>Tests`.
- Name each unit test method as `<MethodUnderTest>_<Condition>_<ExpectedBehavior>`.
- Keep test method names descriptive and behavior-focused.
- Example class naming:
  - `Arithmetic` class -> `ArithmeticTests`.
- Example method naming:
  - `Divide_ShouldThrowInvalidArgument_WhenProvidedDivisorIsZero`.

## Separation Of Test Types
- Development unit tests and integration tests are different and must remain separated.
- QA integration tests must use a separate project from the development unit test project.
- Do not place integration tests into the development unit test project.

## Quality Gate
- Do not consider test changes complete when folder and namespace structure are misaligned.
- Do not consider test changes complete when integration tests are added to the development unit test project.
- Do not consider test changes complete when test class and method names do not follow the required naming format.
- Do not consider test changes complete when the required unit test libraries are not used.
- Do not consider test changes complete when unit tests depend on external systems.

