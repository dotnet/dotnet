# How to re-bootstrap the toolset used to build the VMR

.NET utilizes itself to build. Therefore, in order to build the .NET SDK,
you first need to acquire or build a bootstrapping .NET SDK and other tooling
such as [Arcade](https://github.com/dotnet/arcade). Re-bootstrapping is the term
used to describe when the bootstrapped toolset need to be updated. This document
describes the steps to re-bootstrap the VMR.

## When is it appropriate to re-bootstrap?

As part of the release process, the toolset is updated (e.g. PRs are created via
the release automation). Outside of a release, re-bootstrapping is only
permitted during preview releases. It is not allowed during RC, GA, or servicing
releases. The reason it is not allowed during non-preview releases is because of
the negative impact it has on Linux distro maintainers who source build .NET. It
is often a long and time-consuming process for them to re-bootstrap. It is
likely to cause significant delays in the release/availability of .NET within
the distros that are source built.

## Why is re-bootstrap necessary?

Re-bootstrapping is necessary when .NET takes a dependency on new functionality
added within the bootstrap toolset. For example, suppose a new compiler feature
is added. In order for a repo to take a dependency on the new feature, a
re-bootstrap would be necessary. The implication of this, and the restrictions
of when re-bootstrapping is allowed, means that repos should, in general, wait
to take a dependency on a new toolset feature until after that feature has been
released.

## Steps to re-bootstrap

> [!IMPORTANT] 
> Eligible builds must have published artifacts to a public channel.
> For this reason, re-bootstrapping is only allowed with official builds
> from the main branch and non-internal release branches.

### Automated

You can re-bootstrap the VMR using [this
pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=1571). The
pipeline will open the corresponding re-bootstrap PRs.

### Manual

In case the automated re-bootstrapping pipeline is unavailable, you can manually
re-bootstrap the VMR:

1. Find a
    [dotnet-unified-build](https://dev.azure.com/dnceng/internal/_build?definitionId=1330)
    run with the desired changes.
    1. Retrieve the stable and non-stable SDK versions:
        1. Download the MergedManifest.xml from Artifacts -> AssetManifests
        1. In the MergedManifest.xml, locate a blob entry for an SDK tarball,
           e.g. IDs ending with `dotnet-sdk-*-linux-x64.tar.gz`.
        1. Read the version values from the blob ID to identify both variants:
           `Sdk/<non-stable-sdk-version>/dotnet-sdk-<stable-sdk-version>-linux-x64.tar.gz`
    1. Retrieve the bar ID from the `BAR ID - <id>` tag in the pipeline run
1. Update .NET SDK
    1. Update the tools.dotnet version in the
       [global.json](https://github.com/dotnet/dotnet/blob/main/global.json)
       with the stable SDK version.
1. Update private source-built SDK and artifacts versions
    1. Update `PrivateSourceBuiltSdkVersion` and
       `PrivateSourceBuiltArtifactsVersion` in the
       [Versions.props](https://github.com/dotnet/dotnet/blob/main/eng/Versions.props)
       with the non-stable SDK version.
1. Update arcade
    1. Run `darc update-dependencies --id <bar_id>` from the root of the VMR
