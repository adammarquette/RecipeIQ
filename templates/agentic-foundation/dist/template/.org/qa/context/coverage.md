# QA — Test Coverage State

## Current Coverage

```mermaid
graph LR
    subgraph Tests["tests/__ROOT_NAMESPACE__.Tests/"]
        RDT[RecipeDiscoveryServiceTests]
        CT[CreatorServiceTests]
        FT[FulfillmentServiceTests]
        PT[PlatformServiceTests]
        RT[RetailerServiceTests]
    end

    subgraph Services["__ROOT_NAMESPACE__.Core Services"]
        RDS[RecipeDiscoveryService]
        CS[CreatorService]
        FS[FulfillmentService]
        PS[PlatformService]
        RS[RetailerService]
    end

    RDT --> RDS
    CT --> CS
    FT --> FS
    PT --> PS
    RT --> RS
```

## Gaps

- `RetailerService` tests exist in `tests/__ROOT_NAMESPACE__.Tests/RetailerServiceTests.cs`.
- Add edge-case scenarios for zero-inventory, region mismatch, and duplicate retailer naming.

