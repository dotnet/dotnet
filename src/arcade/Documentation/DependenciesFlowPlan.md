# Dependencies Flow Plan
The components needed in order to flow dependencies through different repositories are: Darc, Maestro++, and the Build Asset Registry (using Darc)
For more information take a look at [version querying and updating](./VersionQueryingAndUpdating.md).

This document outlines the main stages to implement the work needed to accomplish the dependencies flow.
 First, we'll focus on [prototype](#Prototype) and once validated, we'll start adding functionalities to each component.

In order for the dependency flow plan to be done, tier 1 product repositories need to:
 - Use Darc for dependency management.
 - Subscribe to Maestro++.
 - Builds for both CI and official builds happen in Azure DevOps.

# Prototype
The goal of the prototype is to validate the interaction proposed between Darc and Maestro++, as well as the boundaries of each.
We will showcase it by the end of S136.

## Conditions:
- The code for both Darc and Maestro++ live in Arcade.
- Maestro++ uses Darc to update Arcade dependencies.

## Scenario:
1. A file gets updated in Arcade, that needs to go to other branches in Arcade.
2. Maestro++ schedule trigger happens.
3. Maestro++ asks Darc if there are any updates required for Arcade.
4. Maestro++ calls Darc asking to update make an update.
5. Darc creates a PR in Arcade.
6. Dev merges the PR.

## Assumptions:
- The trigger of Maestro++ for the Arcade repo will be scheduled.
- Maestro++ has a the subscriptions hardcoded.
- PR created will have one of the developers as owner.
- The PR created by Darc will need to be merged by a developer.
- PRs won't be monitored by Maestro++.
- We'll use a mock for the Build Asset Registry.

# Adding functionalities - V1
Once we have gathered feedback from the prototype, we can start adding functionalities to create Dependencies Flow V1. Targeting 3 sprints (S139 - June 8th) of work that will include:

## Build Asset Registry
### Design the model of the Build Asset Registry (S137 - June 29th)
The Build Asset Registry is going to be used by:
- Builds: As part of the publish steps, once the assets produced during the build have been published, the build needs to tell the Build Asset Registry about what was produced and where it is located at.
- Darc: Query and updates the Build Asset Registry.
- Mission Control: Shows the information of the dependencies to users.

### Implement API to access the Build Asset Registry (S138 - July 20th)
For V1, the minimum APIs needed are:
- Add
- Get

## Darc
### Interaction with the Build Asset Registry (S138 - July 20th)
Darc uses the GET API call from the Build Asset Registry

### Add the ability to set the owner of the PR
By using GitHub tokens, be able to set the owner of the PR.

### Push to more than one repo
Define if we need this and how it should work.

## Maestro++
### Implement Channel subscription mechanism to Arcade (S137 - June 29th)
This involves:
- Evaluate if the current model (manage the subscriptions in a repository with a JSON file) is sufficient.
- Have the subscriptions in the Arcade repository.

### Implement the ability to set policies. (S138 - S139 - July 20th)
For triggers, PR and auto-merge management.

**auto-merge**
The minimum set of policies we need to enable this functionality are:
- Set the group of checks/validations that are needed in order to determine the PR is green and ready to merge. Note that each repo will have a different request on what goes into a group of checks, for example:
    - Green CI on GitHub/Azure DevOps PR
    - No newer version updates have been merged
    - No non-maestro bot commits have been added to the PR
    - Would committing the PR results in an invalid dependency graph (e.g. aspnet's core-setup dependency is no longer matching CLI's core-setup dependency)
- Turn on/off the ability to auto-merge a PR that has as owner Maestro++ (in the form of a Bot, for example). => at this point, this is not enabled and it is by default on the Dev.

**Note** that policies are for each repository.

## Onboard 1 repository to use Darc and Maestro++ for their official builds. (S139 - August 10th)
It involves:
- Define which repository.
- Add the repository to the Maestro++ subscriptions mechanism.
- Set Maestro++ policies for the repository.
- Builds send information to the Build Asset Registry using the API to report on the assets produced, the location, and other Build information. 

# Adding functionalities - V2
Targeting 2 sprints (S141 - Sept 21st) of work that will include:

## Local work for Darc
Make sure a dev can use Darc locally to update the dependencies of the repo, and other repos too. 

## Finish implementing Build Asset Registry APIs

## On board other repositories to use Darc and Maestro++ for their official builds.
It involves:
- Add the repositories to the Maestro++ subscriptions mechanism.
- Set Maestro++ policies for each repository.
- Builds send information to the Build Asset Registry using the API to report on the assets produced, the location, and other Build information.

# Adding functionalities - V3...
## Mirror builds using Maestro++
Currently, Maestro is in charge of this builds. Part of the work here is to define how those mirrors are going to happen in this new world and eventually stop relying on Maestro to build .NET CORE 3.0.

## Speculative version flow
More information [here]( https://github.com/dotnet/arcade/blob/master/Documentation/Maestro.md#speculative-version-flow).

## Speculative product build
More information [here](https://github.com/dotnet/arcade/blob/master/Documentation/Maestro.md#speculative-product-builds).


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CDependenciesFlowPlan.md)](https://helix.dot.net/f/p/5?p=Documentation%5CDependenciesFlowPlan.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CDependenciesFlowPlan.md)</sub>
<!-- End Generated Content-->
