# How to re-bootstrap the toolset used to build the VMR

.NET utilizes itself to build. Therefore, in order to build .NET from source,
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
is often a long and time consuming process for them to re-bootstrap. It is
likely to cause significant delays in the release/availability of .NET within
the distros that are source built.

## Why is re-bootstrap necessary?

Re-bootstrapping is necessary when .NET takes a dependency on new functionality
added within the bootstrap toolset. For example suppose a new compiler feature
is added. In order for a repo to take a dependency on the new feature, a
re-bootstrap would be necessary. The implication of this, and the restrictions
of when re-bootstrapping is allowed, means that repos should, in general, wait
to take a dependency on a new toolset feature until after that feature has been
released.

## Steps to re-bootstrap

### Automated

> [!IMPORTANT]  
> The re-bootstrap pipeline uploads the artifacts to the official blob storage,
> so do not use this pipeline for testing of any kind. To test stage 2 failures,
> please refer to [this
> documentation](bootstrapping-guidelines.md#building-on-a-supported-platform-using-rid-known-to-net).

You can re-bootstrap the VMR using [this
pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=1371). The
pipeline will upload the artifacts & open the corresponding re-bootstrap PR.

### Manual

In case the automated re-bootstrapping pipeline is unavailable, you can manually
re-bootstrap the VMR:

1. Update previous source-build artifacts
    1. Find a
    [dotnet-source-build](https://dev.azure.com/dnceng/internal/_build?definitionId=1219)
    run with the desired changes.
        1. If a rebootstrap is needed quickly and it is not feasibly to wait for
           a
           [dotnet-source-build](https://dev.azure.com/dnceng/internal/_build?definitionId=1219)
           run, you can also use the artifacts from a
           [dotnet-source-build-lite](https://dev.azure.com/dnceng/internal/_build?definitionId=1299)
           run.
    1. Retrieve the built SDKs and private source-built artifacts archives, from
       the following legs:
        1. Alpine\<nnn\>_Online_MsftSdk_x64
        1. CentOSStream\<n\>_Online_MsftSdk_x64
    1. Upload the SDKs to the [source build sdk blob
       storage](https://dotnetcli.blob.core.windows.net/source-built-artifacts/sdks/)
    1. Upload the private source-built artifacts archives to the [source build
       assets blob
       storage](https://dotnetcli.blob.core.windows.net/source-built-artifacts/assets/)
1. Update .NET SDK
    1. Find the
    [dotnet-sdk-official-ci](https://dev.azure.com/dnceng/internal/_build?definitionId=140)
    build that best matches the dotnet-source-build. The following is the
    suggested order of precedence for finding the best match.
        1. A build from the same commit.
            1. From the
            [dotnet-source-build](https://dev.azure.com/dnceng/internal/_build?definitionId=1219),
            look at the build's installer tag.
            1. From a VMR commit, you can find the corresponding installer
            commit by looking at the
            [source-manifest.json](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json).
        1. The next passing build after the same commit.
        1. In the odd case where the are no passing builds after the commit, you
        can try using an earlier passing build.
    1. Retrieve the built SDK version from the build.
    1. Update the dotnet version in the
       [global.json](https://github.com/dotnet/dotnet/blob/main/global.json).
1. Update arcade
    1. Lookup the arcade commit and version. From a VMR commit, you can find the
    corresponding arcade commit/version by looking at the
    [source-manifest.json](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json).
    1. Update the arcade SDK version in the
       [global.json](https://github.com/dotnet/dotnet/blob/main/global.json).
    1. Update the arcade dependency commit and version in the
       [Version.Details.xml](https://github.com/dotnet/dotnet/blob/main/eng/Version.Details.xml).
1. Update private source-built SDK and artifacts versions
    1. Update `PrivateSourceBuiltSdkVersion` and
       `PrivateSourceBuiltArtifactsVersion` in the
       [Versions.props](https://github.com/dotnet/dotnet/blob/main/eng/Versions.props).

[Tracking issue for automating this
process.](https://github.com/dotnet/source-build/issues/4246)
