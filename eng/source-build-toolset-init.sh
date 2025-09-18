#!/usr/bin/env bash

### This script provides shared functionality for initializing the source build toolset.
### It handles custom SDK setup, MSBuild SDK resolver initialization, and toolset preparation.

set -euo pipefail

function source_only_toolset_init() {
    local scriptroot="$1"
    local custom_sdk_dir="$2"
    local custom_packages_dir="$3"
    local binary_log="$4"
    local test="${5:-false}"
    local properties=("${@:6}")  # All remaining arguments are properties

    echo "Initializing source-only toolset..."

    # Set up custom SDK if specified
    if [ -n "$custom_sdk_dir" ]; then
        if [ ! -d "$custom_sdk_dir" ]; then
            echo "ERROR: Custom SDK directory '$custom_sdk_dir' does not exist"
            exit 1
        fi
        if [ ! -x "$custom_sdk_dir/dotnet" ]; then
            echo "ERROR: Custom SDK '$custom_sdk_dir/dotnet' does not exist or is not executable"
            exit 1
        fi
        export SDK_VERSION=$("$custom_sdk_dir/dotnet" --version)
        export CLI_ROOT="$custom_sdk_dir"
        echo "Using custom bootstrap SDK from '$CLI_ROOT', version '$SDK_VERSION'"
    else
        sdkLine=$(grep -m 1 'dotnet' "$scriptroot/global.json")
        sdkPattern="\"dotnet\" *: *\"(.*)\""
        if [[ $sdkLine =~ $sdkPattern ]]; then
            export SDK_VERSION=${BASH_REMATCH[1]}
            export CLI_ROOT="$scriptroot/.dotnet"
        fi
    fi

    # Set _InitializeDotNetCli & DOTNET_INSTALL_DIR so that eng/common/tools.sh doesn't attempt to restore the SDK.
    export _InitializeDotNetCli="$CLI_ROOT"
    export DOTNET_INSTALL_DIR="$CLI_ROOT"

    # Don't use the global nuget cache folder when building source-only which
    # restores prebuilt packages that should never get into the global nuget cache.
    export NUGET_PACKAGES="$scriptroot/.packages/"

    if [[ "$test" == true ]]; then
      # Use a custom package cache for tests to make prebuilt detection work.
      export NUGET_PACKAGES="${NUGET_PACKAGES}tests/"
    fi

    echo "NuGet packages cache: '${NUGET_PACKAGES}'"

    # Find the Arcade SDK version and set env vars for the msbuild sdk resolver
    local packageVersionsPath=''
    local packagesDir="$scriptroot/prereqs/packages/"
    local packagesArchiveDir="${packagesDir}archive/"
    local packagesPreviouslySourceBuiltDir="${packagesDir}previously-source-built/"
    local tempDir=""

    if [[ "$custom_packages_dir" != "" && -f "$custom_packages_dir/PackageVersions.props" ]]; then
        packageVersionsPath="$custom_packages_dir/PackageVersions.props"
    elif [ -d "$packagesArchiveDir" ]; then
        sourceBuiltArchive=$(find "$packagesArchiveDir" -maxdepth 1 -name 'Private.SourceBuilt.Artifacts*.tar.gz')
        if [ -f "${packagesPreviouslySourceBuiltDir}PackageVersions.props" ]; then
            packageVersionsPath=${packagesPreviouslySourceBuiltDir}PackageVersions.props
        elif [ -f "$sourceBuiltArchive" ]; then
            tar -xzf "$sourceBuiltArchive" -C /tmp PackageVersions.props
            packageVersionsPath=/tmp/PackageVersions.props
        fi
    fi

    if [ ! -f "$packageVersionsPath" ]; then
        echo "Cannot find PackagesVersions.props.  Debugging info:"
        echo "  Attempted custom PVP path: $custom_packages_dir/PackageVersions.props"
        echo "  Attempted previously-source-built path: ${packagesPreviouslySourceBuiltDir}PackageVersions.props"
        echo "  Attempted archive path: $packagesArchiveDir"
        exit 1
    fi

    # Extract toolset packages

    # Ensure that by default, the bootstrap version of the toolset SDK is used. Source-build infra
    # projects use bootstrap toolset SDKs, and would fail to find it in the build. The repo
    # projects overwrite this so that they use the source-built toolset SDK instad.

    # 1. Microsoft.DotNet.Arcade.Sdk
    arcadeSdkLine=$(grep -m 1 'MicrosoftDotNetArcadeSdkVersion' "$packageVersionsPath")
    arcadeSdkPattern="<MicrosoftDotNetArcadeSdkVersion>(.*)</MicrosoftDotNetArcadeSdkVersion>"
    if [[ $arcadeSdkLine =~ $arcadeSdkPattern ]]; then
        export ARCADE_BOOTSTRAP_VERSION=${BASH_REMATCH[1]}

        export SOURCE_BUILT_SDK_ID_ARCADE=Microsoft.DotNet.Arcade.Sdk
        export SOURCE_BUILT_SDK_VERSION_ARCADE=$ARCADE_BOOTSTRAP_VERSION
        export SOURCE_BUILT_SDK_DIR_ARCADE=${NUGET_PACKAGES}BootstrapPackages/microsoft.dotnet.arcade.sdk/$ARCADE_BOOTSTRAP_VERSION
    fi

    # 2. Microsoft.Build.NoTargets
    notargetsSdkLine=$(grep -m 1 'Microsoft.Build.NoTargets' "$scriptroot/global.json")
    notargetsSdkPattern="\"Microsoft\.Build\.NoTargets\" *: *\"(.*)\""
    if [[ $notargetsSdkLine =~ $notargetsSdkPattern ]]; then
        export NOTARGETS_BOOTSTRAP_VERSION=${BASH_REMATCH[1]}

        export SOURCE_BUILT_SDK_ID_NOTARGETS=Microsoft.Build.NoTargets
        export SOURCE_BUILT_SDK_VERSION_NOTARGETS=$NOTARGETS_BOOTSTRAP_VERSION
        export SOURCE_BUILT_SDK_DIR_NOTARGETS=${NUGET_PACKAGES}BootstrapPackages/microsoft.build.notargets/$NOTARGETS_BOOTSTRAP_VERSION
    fi

    # 3. Microsoft.Build.Traversal
    traversalSdkLine=$(grep -m 1 'Microsoft.Build.Traversal' "$scriptroot/global.json")
    traversalSdkPattern="\"Microsoft\.Build\.Traversal\" *: *\"(.*)\""
    if [[ $traversalSdkLine =~ $traversalSdkPattern ]]; then
        export TRAVERSAL_BOOTSTRAP_VERSION=${BASH_REMATCH[1]}

        export SOURCE_BUILT_SDK_ID_TRAVERSAL=Microsoft.Build.Traversal
        export SOURCE_BUILT_SDK_VERSION_TRAVERSAL=$TRAVERSAL_BOOTSTRAP_VERSION
        export SOURCE_BUILT_SDK_DIR_TRAVERSAL=${NUGET_PACKAGES}BootstrapPackages/microsoft.build.traversal/$TRAVERSAL_BOOTSTRAP_VERSION
    fi

    echo "Found bootstrap versions: SDK $SDK_VERSION, Arcade $ARCADE_BOOTSTRAP_VERSION, NoTargets $NOTARGETS_BOOTSTRAP_VERSION and Traversal $TRAVERSAL_BOOTSTRAP_VERSION"

    # toolset prep steps
    source "$scriptroot/eng/common/tools.sh"
    InitializeBuildTool

    initSourceOnlyBinaryLog=""
    if [[ "$binary_log" == true ]]; then
      initSourceOnlyBinaryLog="/bl:\"$log_dir/init-source-only.binlog\""
    fi

    "$_InitializeBuildTool" build-server shutdown --msbuild
    MSBuild-Core "$scriptroot/eng/init-source-only.proj" $initSourceOnlyBinaryLog "${properties[@]}"
    # kill off the MSBuild server so that on future invocations we pick up our custom SDK Resolver
    "$_InitializeBuildTool" build-server shutdown --msbuild

    local bootstrapArcadeDir=$(cat "$scriptroot/artifacts/toolset/bootstrap-sdks.txt" | grep "microsoft.dotnet.arcade.sdk")
    local arcadeBuildStepsDir="$bootstrapArcadeDir/tools/"

    # Point MSBuild to the custom SDK resolvers folder, so it will pick up our custom SDK Resolver
    export MSBUILDADDITIONALSDKRESOLVERSFOLDER="$scriptroot/artifacts/toolset/VSSdkResolvers/"

    # Set _InitializeToolset so that eng/common/tools.sh doesn't attempt to restore the arcade toolset again.
    _InitializeToolset="${arcadeBuildStepsDir}/Build.proj"

    echo "Source-only toolset initialization complete"
}
