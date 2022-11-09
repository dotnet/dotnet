# Frequently Asked Questions

## Migrating from other systems

**Where is .NET CI (Jenkins ci.dot.net, ci2.dot.net, ci3.dot.net) going?**

The current .NET CI instances will remain operational for about another year,
then will be decommissioned.  This decommission is lined up with the [support
lifecycle of .NET Core 1.x](https://www.microsoft.com/net/support/policy),
thus the expected decommission date is June 27, 2019.

**I used machine X in .NET CI, does such a machine exist in Azure DevOps?**

In the move from .NET CI, the existing Jenkins static images will not be
ported.  They are largely opaque and difficult to patch.  Instead, we will
move towards using [OS
onboarding](https://dev.azure.com/dnceng/internal/_git/dotnet-helix-machines?path=%2FREADME.md&version=GBmaster)
to provide a way to declaratively create new machine images with specific
software and hook them up to dynamically scalable Helix queues. Unfortunately,
while the OS onboarding is ready to use, the automated link between Helix and
Azure DevOps is not. In the meantime Azure DevOps provides agents in the
following ways.

- Static machines with signing capability
- Manually scaling Helix queues.
- Azure DevOps Hosted VM pools.
- Azure DevOps Hosted Mac pools.

In addition, we are pushing to bootstrap in native tools (e.g. cmake) rather
than bake them into the images.  This means that setting up a developer
machine becomes a simpler process.  Documentation on the process of
bootstrapping can be found [here](./NativeToolBootstrapping.md).

See [here](AzureDevOps/AzureDevOpsOnboarding.md#agent-queues) for more information. For
additional questions contact 'dnceng'. Either @dotnet/dnceng on GitHub, or
dnceng@microsoft.com.  Also, you can post to the [Arcade Teams channel](https://teams.microsoft.com/l/channel/19%3acf9dc0ac9753432dbac4023239a9965f%40thread.skype/Arcade?groupId=147df318-61de-4f04-8f7b-ecd328c256bb&tenantId=72f988bf-86f1-41af-91ab-2d7cd011db47).

**My repo is currently on repo-toolset, how can I move into Arcade?**

https://github.com/dotnet/arcade/blob/master/Documentation/RepoToolset/MigrationToArcade.md

**Can I run .NET CI and Azure DevOps CI in parallel while I work out the
  kinks?**

Sure.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CFAQ.md)](https://helix.dot.net/f/p/5?p=Documentation%5CFAQ.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CFAQ.md)</sub>
<!-- End Generated Content-->
