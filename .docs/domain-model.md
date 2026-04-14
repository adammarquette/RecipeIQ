# RecipeIQ — Domain Model

## Bounded Contexts

```mermaid
graph TB
    subgraph Discovery["Recipe Discovery Context"]
        Recipe
        Tag
        DietaryFilter
        RecipeRanking
    end

    subgraph Identity["Cook Identity Context"]
        HomeCook
        Subscription
        Household
        Preferences
    end

    subgraph CreatorCtx["Creator Context"]
        Creator
        Portfolio
        Analytics
        Earnings
    end

    subgraph Fulfillment["Fulfillment Context"]
        Order
        Basket
        Ingredient
        Retailer
    end

    subgraph Platform["Platform Context"]
        PlatformMetrics
        RevenueShare
        Moderation
    end

    Discovery -->|recipe references| Fulfillment
    Identity -->|cook places| Fulfillment
    Creator -->|publishes to| Discovery
    Fulfillment -->|revenue flows to| Platform
    Discovery -->|demand signal to| Creator
```

## Value Flows

```mermaid
flowchart LR
    HC[Home Cook]
    CR[Creator]
    RT[Retailer]
    PL[Platform]

    HC -->|"subscription fee\n+ order value"| PL
    PL -->|"revenue share\n(demand signal)"| CR
    PL -->|"order routing\n+ basket value"| RT
    RT -->|"ingredient fulfillment"| HC
    CR -->|"recipe content"| HC
    HC -->|"engagement signal"| CR
```

## Core Aggregates

### Recipe (Discovery Context)
- Root entity owned by a `Creator`
- Contains a list of `Ingredient` references (fulfilled in Fulfillment context)
- Carries tags, dietary flags, estimated cost, and prep time
- Ranked by the `RecipeDiscoveryService` based on cook preferences

### HomeCook (Identity Context)
- Root entity representing a platform user
- Holds dietary restrictions, preferences, and budget constraints
- May hold a `Subscription` (tier: Free | Premium)

### Order (Fulfillment Context)
- Root aggregate representing a cook's intent to make a recipe
- Resolves `Ingredient` availability across `Retailer` inventory
- Tracks `OrderStatus`: Pending → Confirmed → Fulfilled

### Basket (Fulfillment Context)
- Value object produced when an Order resolves its Recipe's ingredients
- Groups ingredients by Retailer for fulfillment routing
- Lifecycle is tied to its parent Order — not persisted independently

### Creator (Creator Context)
- Root entity representing a recipe author
- Accrues analytics from recipe engagement and orders placed

## Domain Language (Ubiquitous Language)

| Term | Meaning |
|------|---------|
| Discovery | The act of surfacing relevant recipes to a home cook |
| Fulfillment | Resolving and delivering the ingredients for a recipe |
| Demand Signal | Aggregate data about which recipes are being saved/cooked |
| Revenue Share | Platform distribution of order revenue to creators and retailers |
| Household | The cook's family/living context that shapes portion and dietary needs |
| Basket | The set of ingredients derived from a recipe for a single order |
