# Architect — ADR Status

Current decisions. Authoritative record lives in [Architecture](../../../.docs/architecture.md).

| ADR | Decision | Status | Detail File | Implementation Evidence |
| --- | -------- | ------ | ----------- | ----------------------- |
| ADR-001 | InMemoryStore for initial persistence | Superseded | [adr-001-inmemory-store-bootstrap.md](adr-001-inmemory-store-bootstrap.md) | Present in runtime wiring |
| ADR-002 | Service interfaces (I*Service) for all domain services | Accepted | [adr-002-service-interfaces.md](adr-002-service-interfaces.md) | Implemented |
| ADR-003 | One controller per marketplace participant | Accepted | [adr-003-controller-per-participant.md](adr-003-controller-per-participant.md) | Implemented |
| ADR-004 | Introduce `MarqSpec.RecipeIQ.Data` for EF Core persistence | Accepted (Partially Implemented) | [adr-004-ef-core-data-layer.md](adr-004-ef-core-data-layer.md) | Data project present, migration in progress |
