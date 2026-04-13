# RecipeIQ — System Architecture

## Overview

RecipeIQ is structured as a layered ASP.NET Core application implementing a four-sided marketplace. The current implementation uses an in-memory store and is designed for incremental growth toward a persistent, event-driven architecture.

## Component Architecture

```mermaid
graph LR
    subgraph Client["Clients"]
        Web[Web / Mobile]
        Admin[Admin Tools]
    end

    subgraph Api["MarqSpec.RecipeIQ.Api (Presentation)"]
        direction TB
        RecipesCtrl[RecipesController]
        CreatorsCtrl[CreatorsController]
        OrdersCtrl[OrdersController]
        RetailersCtrl[RetailersController]
        PlatformCtrl[PlatformController]
    end

    subgraph Core["MarqSpec.RecipeIQ.Core (Domain)"]
        direction TB
        subgraph Services["Services"]
            RDS[IRecipeDiscoveryService]
            CS[ICreatorService]
            FS[IFulfillmentService]
            RS[IRetailerService]
            PS[IPlatformService]
        end
        subgraph Models["Domain Models"]
            Recipe
            Creator
            HomeCook
            Retailer
            Order
            Basket
            Ingredient
            Subscription
            PlatformMetrics
        end
    end

    subgraph Data["MarqSpec.RecipeIQ.Data (Infrastructure)"]
        direction TB
        DbCtx[RecipeIQDbContext]
        Entities["Entities (EF Core models)"]
        Migrations["Migrations"]
    end

    Web & Admin --> Api
    RecipesCtrl --> RDS
    CreatorsCtrl --> CS
    OrdersCtrl --> FS
    RetailersCtrl --> RS
    PlatformCtrl --> PS
    Services --> Models
    Services --> DbCtx
    DbCtx --> Entities
```

## Domain Model

```mermaid
erDiagram
    HomeCook {
        Guid Id
        string Name
        string[] DietaryRestrictions
        string[] Preferences
        decimal Budget
    }
    Recipe {
        Guid Id
        string Title
        string CreatorId
        string[] Tags
        decimal EstimatedCost
        int PrepTimeMinutes
    }
    Creator {
        Guid Id
        string Name
        string[] Specialties
        int FollowerCount
    }
    Retailer {
        Guid Id
        string Name
        string Region
    }
    Ingredient {
        Guid Id
        string Name
        decimal Price
        string RetailerId
    }
    Order {
        Guid Id
        string HomeCookId
        string RecipeId
        OrderStatus Status
        decimal TotalCost
    }
    Basket {
        Guid Id
        string OrderId
        string RetailerId
    }
    Subscription {
        Guid Id
        string HomeCookId
        SubscriptionTier Tier
        DateTimeOffset RenewsAt
    }
    PlatformMetrics {
        int TotalRecipes
        int ActiveCooks
        int TotalOrders
        decimal GrossRevenue
    }

    HomeCook ||--o{ Order : places
    HomeCook ||--o| Subscription : holds
    Recipe ||--o{ Ingredient : contains
    Recipe }o--|| Creator : "authored by"
    Order ||--|| Basket : "produces"
    Order }o--|| Recipe : "for"
    Basket }o--o{ Ingredient : "contains"
    Basket }o--|| Retailer : "fulfilled by"
    Ingredient }o--|| Retailer : "stocked by"
```

## Request Flow

```mermaid
sequenceDiagram
    actor Cook as Home Cook
    participant API as MarqSpec.RecipeIQ.Api
    participant RDS as RecipeDiscoveryService
    participant FS as FulfillmentService
    participant Store as RecipeIQDbContext

    Cook->>API: GET /recipes?budget=25&tags=quick
    API->>RDS: DiscoverAsync(filters)
    RDS->>Store: Query recipes
    Store-->>RDS: Matching recipes
    RDS-->>API: Ranked recipe list
    API-->>Cook: 200 OK — recipes

    Cook->>API: POST /orders {recipeId}
    API->>FS: PlaceOrderAsync(order)
    FS->>Store: Resolve ingredients + retailers
    FS->>Store: Persist order
    Store-->>FS: Order confirmed
    FS-->>API: Order result
    API-->>Cook: 201 Created — order
```

## Deployment Architecture (Target)

```mermaid
graph TB
    subgraph Azure["Azure (Target)"]
        APIM[API Management]
        App[App Service / Container Apps]
        DB[(Azure SQL / Cosmos DB)]
        SB[Service Bus]
        KV[Key Vault]
    end

    subgraph CI["GitHub Actions"]
        Build[Build & Test]
        Review[Claude Code Review]
        Deploy[Deploy]
    end

    Client --> APIM --> App
    App --> DB
    App --> SB
    App --> KV
    CI --> App
```

## Architecture Decision Records

| # | Decision | Status | Notes |
|---|----------|--------|-------|
| ADR-001 | Use InMemoryStore for initial persistence | Superseded | Replaced by EF Core / `RecipeIQDbContext` in `MarqSpec.RecipeIQ.Data` |
| ADR-002 | Service interfaces (I*Service) for all domain services | Accepted | Enables DI and future test isolation |
| ADR-003 | One controller per marketplace participant | Accepted | Mirrors four-sided marketplace structure |
| ADR-004 | Introduce `MarqSpec.RecipeIQ.Data` for EF Core persistence | Accepted | Separates infrastructure from domain; Core remains framework-agnostic |
