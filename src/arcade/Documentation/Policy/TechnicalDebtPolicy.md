# Driving technical debt principles:
1. We must not add new technical debt.  (unless there’s an explicit business decision to do so)
1. We must reduce our current debt – reasonably over time.

It should be said that it’s easy to miss some of this stuff, so we should feel free to hold each other accountable in code reviews.  In fact, getting more active participation in code reviews across the board would be fantastic.

## Stop increasing technical debt:
We need do more than just say “don’t add debt”.  We don’t always know what this means, we have business pressures which make this difficult at times, and we don’t necessarily know where to start.  Actually doing this requires a combination of concrete changes in policy, and continued cultural changes.  (yes, they’re intertwined…)  But the good news is that we’re ready as a team to tackle this together.

### Policy:  (automation of checks wherever we can will be important for sustainability)
-	The following need confirmation from another senior dev.  (see ‘ getting confirmation’ below for details)
    - New dependencies (for example, this leads to inconsistencies within the project – making for a mess)
    - New end points or services
    - Duplication of code
    - New language or language version
    - Use of a prelease service/library
-	Appropriate test coverage
    - Make it more obvious the extent of test coverage.  For example, show code coverage percentage as part of the PR results.
    - An initial bar of at least 50% code coverage –80% better.  Exceptions can be given as appropriate.  (either up or down)  The point is to get the right coverage, not just get a number….  (For example, see here for our current code coverage)
-	Documentation should exist  (to say the obvious, we still need better clarity on how we document in general – but that’s outside the scope of this email)
    - It needs to be usable by the intended customer
    - It needs to be discoverable
    - Appropriate existing documentation updated  (remember this is debt as well)
-	Features need a design “one pager” and confirmation from another senior dev
    - Bug fixes and/or small items don’t need this
    - For now, let’s put design into the epic itself so we can always find it

### Cultural Changes
-	Brown bag at least once a year to recount war stories.  The goal is to keep the “why” top of mind.
-	Retrospectives where/when appropriate.
-	General management support (including active participation) for encouraging/enforcing this new policy on our PRs.  It’ll hurt some at times…
-	Encourage conversations about how we keep our quality up, debt low, and general improve our technical offerings.
-	Discuss in our monthly team meeting from time to time

### Getting Confirmation for a change/proposal
-	Email ‘dnceng’ for broad awareness
-	Get at least one other senior dev to ACK
-	It should be noted that we’ll make mistakes.  We can only make decisions based on what we knew at the time.  The expectation is less about building zero debt, but rather changing our culture – e.g. the way we think about these things.

## Reduce existing technical debt:
It would be unreasonable to do nothing other than pay down our technical debt as we have a business to run as well.  However, we can focus on a specific area and make a big impact.

### Policy:
-	There should always be a business priority on the board that actively focuses on reducing technical debt.  (right this minute we have two, Helix, and Validation)
-	We should limit debt reduction business priorities to one at a time.  This is to help stay away from “peanut butter” (spreading effort over a wide area) and make significant progress reducing debt in one area.  It’s sorta like choosing a credit card balance with the highest interest to pay down first.
-	There may be times where the business dictates that we don’t have a debt reduction business priority on the board.  However, this should be the exception and requires M2 approval as it would go against one of our core principles.  For a review, remember to check out our guiding principles.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CPolicy%5CTechnicalDebtPolicy.md)](https://helix.dot.net/f/p/5?p=Documentation%5CPolicy%5CTechnicalDebtPolicy.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CPolicy%5CTechnicalDebtPolicy.md)</sub>
<!-- End Generated Content-->
