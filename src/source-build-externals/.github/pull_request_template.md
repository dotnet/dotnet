## Description

<Insert description of your changes>

## PR Checklist

If you are upgrading the version of a repo submodule, please follow this
checklist:

- [ ] Provide a link to an issue in the consuming repo describing the need for
the upgrade. Both this PR and the PR doing the upgrade in the consuming repo
should link to that issue.
- [ ] Are you targeting the correct branch? For example, if you need to
reference this version in 9.0, you should target the 9.0 branch.
- [ ] Have you done your due diligence to ensure the upgrade can be completed
in the consuming repo in a timely manner? If consuming the dependency flow of
this update takes a long time or needs to be backed out, it may require the
reversion of the upgrade in this PR. That's something we want to avoid.

## After Merge

- [ ] Once the package gets published via darc, you may need to manually trigger
the darc subscription on the configuration from the consuming repo. Subscriptions
for release branches, for example, are typically disabled.
- [ ] When consuming the dependency flow from this repo for the purposes of a
version upgrade, consider using a separate PR or at least changing the title of
the dependency flow PR to accurately reflect the purpose of the change. Seeing
a PR named "Upgrade IdentityModel to 8.0.1", for example, provides better
clarity than "Update dependencies from dotnet/source-build-externals".
- [ ] If this PR targeted a release branch, port the changes to higher release
and main branches.
