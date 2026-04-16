# Domain Glossary

Shared vocabulary for all agents. When in doubt, use these terms exactly as defined here to maintain a consistent ubiquitous language across the codebase, tests, and documentation.

| Term | Definition |
|------|-----------|
| **Home Cook** | A platform user who discovers and cooks recipes. The primary consumer participant. |
| **Creator** | A recipe author who publishes content to the platform. Earns revenue share from engagement and orders. |
| **Retailer** | A grocery or specialty food store whose inventory is available for ingredient fulfillment. |
| **Platform** | __PROJECT_NAME__ itself as a marketplace participant — orchestrates the other three sides. |
| **Recipe** | A structured set of instructions, ingredients, and metadata authored by a Creator. |
| **Discovery** | The process of surfacing relevant recipes to a Home Cook, personalized to their moment and preferences. |
| **Ingredient** | A specific item required by a Recipe, resolved against Retailer inventory during fulfillment. |
| **Fulfillment** | The end-to-end process of resolving a Recipe's ingredients from Retailer inventory and delivering them to the Home Cook. |
| **Order** | A Home Cook's request to fulfill the ingredients for a specific Recipe. |
| **Basket** | The set of Ingredients derived from a Recipe for a single Order. |
| **Subscription** | A recurring payment relationship between a Home Cook and the Platform (Free or Premium tier). |
| **Demand Signal** | Aggregate behavioral data (saves, cooks, shares) indicating which Recipes are resonating. Fed back to Creators. |
| **Revenue Share** | The Platform's distribution of Order value to Creators and Retailers according to agreed splits. |
| **Household** | The cook's living context (number of people, dietary needs) that shapes portion sizes and filtering. |
| **Dietary Filter** | A restriction or preference applied during Discovery (e.g., vegan, gluten-free, nut allergy). |
| **Platform Metrics** | Aggregate health indicators: total recipes, active cooks, orders placed, gross revenue. |
| **InMemoryStore** | The current persistence implementation — a shared in-memory collection. To be replaced with EF Core. |

