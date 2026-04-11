#!/usr/bin/env bash
set -euo pipefail

failures=0

echo "Checking governance drift..."

# Rule 1: If architecture says ADR-001 is superseded, Program should not wire InMemoryStore.
if grep -q "ADR-001.*Superseded" .docs/architecture.md; then
  if grep -q "AddSingleton<InMemoryStore>" src/MarqSpec.RecipeIQ.Api/Program.cs; then
    echo "[DRIFT] ADR-001 is superseded in architecture, but InMemoryStore is still registered in Program.cs"
    failures=$((failures + 1))
  fi
fi

# Rule 2: QA coverage notes must not claim missing RetailerService tests when the file exists.
if grep -q "RetailerService.*test file not yet written" .org/qa/context/coverage.md; then
  if [[ -f tests/MarqSpec.RecipeIQ.Tests/RetailerServiceTests.cs ]]; then
    echo "[DRIFT] QA coverage note is stale: RetailerServiceTests.cs exists"
    failures=$((failures + 1))
  fi
fi

# Rule 3: ADR files referenced by adr-status should exist.
for adr in \
  .org/architect/context/adr-001-inmemory-store-bootstrap.md \
  .org/architect/context/adr-002-service-interfaces.md \
  .org/architect/context/adr-003-controller-per-participant.md \
  .org/architect/context/adr-004-ef-core-data-layer.md
  do
  if [[ ! -f "$adr" ]]; then
    echo "[DRIFT] Missing ADR file: $adr"
    failures=$((failures + 1))
  fi
done

if [[ "$failures" -gt 0 ]]; then
  echo "Governance drift checks failed with $failures issue(s)."
  exit 1
fi

echo "Governance drift checks passed."
