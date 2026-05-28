# Infrastructure Ask & Tell Mode

Infrastructure changes are handled a bit differently from the product because they don't actually ship.  
The main goal is to ensure that the product can always be built and shipped.

When multiple products share the same infrastructure, the most "strict" product determines the mode according to the table below.  (Definitions right after table)

| Product "mode" | Product Infra    | Shared Infra     | Notes                                              |
| ---------------| -----------------| ---------------- |----------------------------------------------------|
| Open           | Open             | Open             | Changes are handled according to normal policy     |
| Tell           | Tell --> Prod    | Tell --> Arcade  | Product branch changes are handled by the product team, and shared infra updates reviewed by the larger working group |
| Ask            | Tell --> Tactics | Tell --> Tactics | Tactics needs to know when in ask mode.  Holding off on 'ask mode' to Arcade for now  |
| Stabilization  | Tell --> Tactics  | Ask --> Arcade  | Changes when trying to get a coherent build can be very destabilizing.  Arcade decides if it's ask or tell to tactics  |

## Definitions:
- "Branch Infra": Infrastructure changes in the product branch
- "Product Infra": Infrastructure changes in shared infra like Arcade, Helix, etc.
- "Tell Mode" for each product, teams will follow their own process
- "Tell Mode" to Arcade means including the [PR template](AskModeTellModeTemplate.md) and adding the 'tell-mode' label on your PR as an FYI that the change is being made. Review is required.
- "Ask Mode" to Arcade means including the [PR template](AskModeTellModeTemplate.md) and adding the 'servicing-consider' label on your PR. Your PR must be explicitly approved by a member of the dnceng LT prior to merge, who will replace 'servicing-consider' with 'servicing-approved' label. The approver may require additional approval from .NET Core tactics if the change is particularly impactful to the product.
- "Ask Mode" to tactics handled according the guidance managed by Lee Coward

## Things to keep in mind:
- Breaking changes are basically never ok.  See [Policy](ChangesPolicy.md) for details.
- Be aware of which arcade branch you're working in, as this determines which part of the product you're affecting. [Arcade Servicing Doc](ArcadeServicing.md)
- Shared services (like Helix and Maestro) have versioned APIs. (they don't branch)  So again, be sure you understand what you're affecting.
- When in doubt, please reach out.  (mail arcadewg@microsoft.com)

## Servicing
There are a few things to keep in mind with servicing:
- Shared infra servicing workflow and policies are [found here](ArcadeServicing.md). 
- Servicing release are basically in perpetual 'Stabilization' mode.
