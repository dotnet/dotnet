# Core-Eng Repository Migration

It is discouraged to have repositories without code. Our documentation and issues are fragmented between multiple places, and it's hard to orient in it. To make things cleaner, it makes sense to migrate all valid items from core-eng repository and archive this repository.


## Stakeholders

* .NET Core Engineering Services team (contact: @dnceng)
* .NET Core Engineering Partners (contact:@dncpartners)


## Components to change

This work consists of four parts:
* Move repository files (documentation only)
* Move Wiki
* Code changes
* Issues / Epics
* Guidance for security issues

### Move repository files (documentation only)
This change includes:
    * [DevDocumentation](https://github.com/dotnet/core-eng/tree/main/DevDocumentation/DevWorkflow/Design) - contains new documentation for DevWorkflow. The whole folder should be moved.
    * [Docs](https://github.com/dotnet/core-eng/tree/main/docs) - some items seems to be obsolete, but there are recent (<=1 year) updates.
    * [Documentation](https://github.com/dotnet/core-eng/tree/main/Documentation) - the whole folder should be moved.

For each document we need to find the best location. Internal documentation shouldn't go to the Arcade repository which is available for public. List of target locations:
    * [arcade documentation](https://github.com/dotnet/arcade/tree/main/Documentation).
    * [arcade-services documentation](https://github.com/dotnet/arcade-services/tree/main/docs).
    * [helix-service documentation](https://dev.azure.com/dnceng/internal/_git/dotnet-helix-service?path=/docs)
    * [helix-machines documentation](https://dev.azure.com/dnceng/internal/_git/dotnet-helix-machines).
    * [AzDO wiki](https://dev.azure.com/dnceng/internal/_wiki/wikis/internal.wiki/1/Home)

Note: While moving to a new location, we need to update links in the existing code. The rest of the [repository core-eng](https://github.com/dotnet/core-eng) seems to be safe to be deleted.

### Move Wiki

The whole wiki should be migrated. There are two options for new placement:
* [AzDO dnceng wiki](https://dev.azure.com/dnceng/internal/_wiki/wikis/internal.wiki/1/Home) - visible internally only
* [Arcade wiki](https://github.com/dotnet/arcade/wiki) - available to everyone

If we split content between public and internal location, fragmentation of our documentation will increase. This is the reason why we are planning to move the whole wiki into AzDO.

### Code changes

This is to change configuration to point against a different repository and to update ZenHub logic to a new representation of epic.

Affected components:
* Alerting under dotnet-arcade-services\src\DotNet.Status.Web
* RolloutScorer under dotnet-arcade-services\src\RolloutScorer
* Maestro under dotnet-arcade-services\src\Maestro
* Async triage tool under dotnet-helix-service\src\async-triage-cli

### Issues / Epics

There are many issues created long time ago which probably aren't valid anymore. We should be automatically migrating issues that are newer than a year. Epics could be older and should be automatically migrated if they contain at least one issue that is newer than a year.

We should flag issues with the Security or EOC labels as not to be automatically migrated. We don't want to announce security issues to the public before they are fixed.

Note: No issues will be closed automatically. The core-eng repository will be archived so if required, any issue can be moved manually in the future.

### Guidance for security issues

All issues with the Security or EOC labels should not to be automatically migrated. Security issues must not be announced to the public before they are fixed. We need to come up with new guidance for security issues. One option would be to track them as AzDO work items.


## Migration approach

No item will be deleted as part of this change. Once all valid items are copied into a new location, the repository core-eng will be archived.

## Rollout and Deployment

* Affected components which points against core-eng repo has to be updated and deployed. See the list above.

## Communication of changes

FR has to be notified about:
* new location of documentation
* new location of alerts

Partners have to be notified about:
* new location of documentation

## Monitoring

All affected components have to be verified after deployment. No additional monitoring is required.

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Ccore-eng-repository-migration-15084.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Ccore-eng-repository-migration-15084.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Ccore-eng-repository-migration-15084.md)</sub>
<!-- End Generated Content-->
