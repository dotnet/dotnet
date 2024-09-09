## Description

## PR Checklist

If you are upgrading the version of a repo submodule, please follow this
checklist:

- [ ] Provide a link to an issue in the consuming repo describing the need for
the upgrade. Both this PR and the PR doing the upgrade in the consuming repo
should link to that issue.
- [ ] Have you done your due diligence to ensure the upgrade can be completed
in the consuming repo in a timely manner? If consuming the dependency flow of
this update takes a long time or needs to be backed out, it may require the
reversion of the upgrade in this PR. That's something we want to avoid.
- [ ] When consuming the dependency flow from this repo for the purposes of a
version upgrade, consider using a separate PR or at least changing the title of
the dependency flow PR to accurately reflect the purpose of the change. Seeing
a PR named "Upgrade IdentityModel to 8.0.1", for example, provides better
clarity than "Update dependencies from dotnet/source-build-externals".
