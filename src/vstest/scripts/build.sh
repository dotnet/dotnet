#!/usr/bin/env/ bash
# Copyright (c) Microsoft. All rights reserved.
# Build script for Test Platform.

set -o nounset  # Fail on uninitialized variables.
set -e          # Fail on non-zero exit code.

# ANSI color codes
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[0;33m'
NOCOLOR='\033[0m'

#
# Parse options
#
CONFIGURATION="Debug"
TARGET_RUNTIME="ubuntu.18.04-x64"
VERSION="" # Will set this later by reading TestPlatform.Settings.targets file.
VERSION_SUFFIX="dev"
FAIL_FAST=false
DISABLE_LOCALIZED_BUILD=false
CI_BUILD=false
VERBOSE=false
PROJECT_NAME_PATTERNS=

#
# Source build repo api
# See https://github.com/dotnet/source-build/blob/dev/release/2.0/Documentation/RepoApi.md
#
DOTNET_BUILD_FROM_SOURCE=0
DOTNET_CORE_SDK_DIR=
DOTNET_BUILD_TOOLS_DIR=

while [ $# -gt 0 ]; do
    lowerI="$(echo ${1:-} | awk '{print tolower($0)}')"
    case $lowerI in
        -h | --help)
            usage
            exit
            ;;
        -c)
            CONFIGURATION=$2
            shift
            ;;
        -r)
            TARGET_RUNTIME=$2
            shift
            ;;
        -v)
            VERSION=$2
            shift
            ;;
        -vs)
            VERSION_SUFFIX=$2
            shift
            ;;
        -noloc)
            DISABLE_LOCALIZED_BUILD=$2
            shift
            ;;
        -ci)
            CI_BUILD=$2
            shift
            ;;
        -p)
            PROJECT_NAME_PATTERNS=$2
            shift
            ;;
        -dotnetbuildfromsource)
            DOTNET_BUILD_FROM_SOURCE=1
            ;;
        -dotnetcoresdkdir)
            DOTNET_CORE_SDK_DIR=$2
            shift
            ;;
        -dotnetbuildtoolsdir)
            DOTNET_BUILD_TOOLS_DIR=$2
            shift
            ;;
        -verbose)
            VERBOSE=true
            ;;
        *)
            break
            ;;
   esac
   shift
done

#
# Variables
#
TP_ROOT_DIR=$(cd "$(dirname "$0")"; pwd -P)
TP_TOOLS_DIR="$TP_ROOT_DIR/tools"
TP_DOTNET_DIR="${DOTNET_CORE_SDK_DIR:-${TP_TOOLS_DIR}/dotnet-linux}"
TP_PACKAGES_DIR="${NUGET_PACKAGES:-${TP_ROOT_DIR}/packages}"
TP_OUT_DIR="$TP_ROOT_DIR/artifacts"
TP_PACKAGE_PROJ_DIR="$TP_ROOT_DIR/src/package/package"
TP_PACKAGE_NUSPEC_DIR="$TP_ROOT_DIR/src/package/nuspec"
TP_SRC_DIR="$TP_ROOT_DIR/src"
TP_USE_REPO_API=$DOTNET_BUILD_FROM_SOURCE

global_json_file="$TP_ROOT_DIR/global.json"


# Set VERSION from scripts/build/TestPlatform.Settings.targets
VERSION=$(test -z $VERSION && grep TPVersionPrefix $TP_ROOT_DIR/scripts/build/TestPlatform.Settings.targets | head -1 | cut -d'>' -f2 | cut -d'<' -f1 || echo $VERSION)

function ReadGlobalVersion {
  local key=$1

  if command -v jq &> /dev/null; then
    _ReadGlobalVersion="$(jq -r ".[] | select(has(\"$key\")) | .\"$key\"" "$global_json_file")"
  elif [[ "$(cat "$global_json_file")" =~ \"$key\"[[:space:]\:]*\"([^\"]+) ]]; then
    _ReadGlobalVersion=${BASH_REMATCH[1]}
  fi

  if [[ -z "$_ReadGlobalVersion" ]]; then
    Write-PipelineTelemetryError -category 'Build' "Error: Cannot find \"$key\" in $global_json_file"
    ExitWithExitCode 1
  fi
}

#
# Dotnet configuration
#
# Disable first run since we want to control all package sources
export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
# Dotnet build doesnt support --packages yet. See https://github.com/dotnet/cli/issues/2712
export NUGET_PACKAGES=$TP_PACKAGES_DIR

ReadGlobalVersion "dotnet"
DOTNET_CLI_VERSION=$_ReadGlobalVersion

#DOTNET_RUNTIME_VERSION="LATEST"

#
# Build configuration
#
TPB_Solution="TestPlatform.sln"
TPB_Build_From_Source_Solution="TestPlatform_BuildFromSource.sln"
TPB_TargetFramework="net462"
TPB_TargetFrameworkCore="netcoreapp3.1"
TPB_Configuration=$CONFIGURATION
TPB_TargetRuntime=$TARGET_RUNTIME
TPB_Version=$(test -z $VERSION_SUFFIX && echo $VERSION || echo $VERSION-$VERSION_SUFFIX)
TPB_CIBuild=$CI_BUILD
TPB_LocalizedBuild=$DISABLE_LOCALIZED_BUILD
TPB_Verbose=$VERBOSE
TPB_EXTERNALS_VERSION=$(grep TestPlatformExternalsVersion $TP_ROOT_DIR/scripts/build/TestPlatform.Dependencies.props | head -1 | cut -d'>' -f2 | cut -d'<' -f1 || echo $VERSION)
TPB_CC_EXTERNALS_VERSION=$(grep MicrosoftInternalCodeCoverageVersion $TP_ROOT_DIR/eng/Versions.props | head -1 | cut -d'>' -f2 | cut -d'<' -f1 || echo $VERSION)
TPB_NEWTONSOFT_JSON_VERSION=$(grep NewtonsoftJsonVersion $TP_ROOT_DIR/eng/Versions.props | head -1 | cut -d'>' -f2 | cut -d'<' -f1 || echo $VERSION)

TPB_BRANCH="$(git -C "." rev-parse --abbrev-ref HEAD 2>/dev/null)" || TPB_BRANCH="LOCALBRANCH" # detached HEAD
TPB_COMMIT="$(git -C "." rev-parse HEAD 2>/dev/null)"              || TPB_COMMIT="LOCALBUILD" # detached HEAD

if [[ $TP_USE_REPO_API = 1 ]]; then
    TPB_TargetFrameworkCore="net7.0"
fi

#
# Logging
#
log()
{
    printf "${GREEN}... $@${NOCOLOR}\n"
}

verbose()
{
    if [ ${TPB_Verbose-false} ]
    then
        printf "${YELLOW}... $@${NOCOLOR}\n" >&2
    fi
}

error()
{
    printf "${RED}... $@${NOCOLOR}\n" >&2
}

function usage()
{
    log " Usage: ./build.sh [Options]"
    log ""
    log " -c <CONFIGURATION>                Build the specified Configuration (Debug or Release, default: Debug)"
    log " -r <TARGET_RUNTIME>               Build for the specified runtime moniker (ubuntu.14.04-x64)"
    log " -v <VERSION>                      Version number for the package generated (15.0.0)"
    log " -vs <VERSION_SUFFIX>              Version suffix for package generated (dev)"
    log " -noloc <DISABLE_LOCALIZED_BUILD>  Disable Localized builds (true,false)"
    log " -ci <CI_BUILD>                    Declares if this is a CI_BUILD or not"
    log " -p <PROJECT_NAME_PATTERNS>        Pattern to build specific projects"
    log " -verbose <VERBOSE>                Enable verbose logging (true, false)"
}

#
# Build steps
#
function install_cli()
{
    if [[ $TP_USE_REPO_API = 0 ]]; then
        # Skip download of dotnet toolset if REPO API is enabled
        local failed=false
        local install_script="$TP_TOOLS_DIR/dotnet-install.sh"
        local remote_path="https://dot.net/v1/dotnet-install.sh"

        log "Installing dotnet cli..."
        local start=$SECONDS

        # Install the latest version of dotnet-cli
        curl --retry 10 -sSL --create-dirs -o $install_script $remote_path || failed=true
        if [ "$failed" = true ]; then
            error "Failed to download dotnet-install.sh script."
            return 1
        fi
        chmod u+x $install_script

        # Runtime versions installed usually need to be kept in sync with the ones installed in common.lib.ps1
        $install_script --runtime dotnet --install-dir "$TP_DOTNET_DIR" --no-path --architecture x64 --channel "2.1" # install the latest patch
        $install_script --runtime dotnet --install-dir "$TP_DOTNET_DIR" --no-path --architecture x64 --channel "3.1" # install the latest patch
        $install_script --runtime dotnet --install-dir "$TP_DOTNET_DIR" --no-path --architecture x64 --channel "5.0" # install the latest patch
        $install_script --runtime dotnet --install-dir "$TP_DOTNET_DIR" --no-path --architecture x64 --channel "6.0" # install the latest patch

        log "install_cli: Get the latest dotnet cli toolset..."
        $install_script --install-dir "$TP_DOTNET_DIR" --channel "7.0" --no-path --version $DOTNET_CLI_VERSION

        log " ---- dotnet x64"
        "$TP_DOTNET_DIR/dotnet" --info
    fi

    local dotnet_path=$(_get_dotnet_path)
    if [[ ! -e $dotnet_path ]]; then
        log "dotnet not found at $dotnet_path. Did the dotnet cli installation succeed?"
        return 1
    fi

    log "install_cli: Complete. Elapsed $(( SECONDS - start ))s."
    return 0
}

function restore_package()
{
    local failed=false
    local dotnet=$(_get_dotnet_path)

    log "restore_package: Start restoring packages to $TP_PACKAGES_DIR."
    local start=$SECONDS

    if [[ $TP_USE_REPO_API = 0 ]]; then
        log ".. .. Restore: Source: $TP_ROOT_DIR/src/package/external/external.csproj"
        $dotnet restore $TP_ROOT_DIR/src/package/external/external.csproj --packages $TP_PACKAGES_DIR -v:minimal -warnaserror -p:Version=$TPB_Version || failed=true
    else
        log ".. .. Restore: Source: $TP_ROOT_DIR/src/package/external/external_BuildFromSource.csproj"
        $dotnet restore $TP_ROOT_DIR/src/package/external/external.csproj /bl:restore_external.binlog --packages $TP_PACKAGES_DIR -v:minimal -warnaserror -p:Version=$TPB_Version -p:DotNetBuildFromSource=true || failed=true
    fi

    if [ "$failed" = true ]; then
        error "Failed to restore packages."
        return 2
    fi

    log "restore_package: Complete. Elapsed $(( SECONDS - start ))s."
}

function invoke_build()
{
    local failed=false
    local dotnet=$(_get_dotnet_path)

    log "invoke_build: Start build."
    local start=$SECONDS
    log ".. .. Build: Source: $TPB_Solution"

    if [ -z "$PROJECT_NAME_PATTERNS" ]
    then
        if [[ $TP_USE_REPO_API = 0 ]]; then
            $dotnet build $TPB_Solution --configuration $TPB_Configuration -v:minimal -p:Version=$TPB_Version -p:CIBuild=$TPB_CIBuild -p:LocalizedBuild=$TPB_LocalizedBuild -bl:TestPlatform.binlog || failed=true
        else
            $dotnet build $TPB_Build_From_Source_Solution --configuration $TPB_Configuration -v:minimal -p:Version=$TPB_Version -p:CIBuild=$TPB_CIBuild -p:LocalizedBuild=$TPB_LocalizedBuild -p:DotNetBuildFromSource=true -bl:TestPlatform.binlog || failed=true
       fi
    else
        find . -name "$PROJECT_NAME_PATTERNS" | xargs -L 1 $dotnet build --configuration $TPB_Configuration -v:minimal -p:Version=$TPB_Version -p:CIBuild=$TPB_CIBuild -p:LocalizedBuild=$TPB_LocalizedBuild
    fi

    log ".. .. Build: Complete."
    if [ "$failed" = true ]; then
        error "Failed to build solution."
        return 2
    fi

    log "invoke_build: Complete. Elapsed $(( SECONDS - start ))s."
}

function publish_package()
{
    local failed=false
    local dotnet=$(_get_dotnet_path)

    log "publish_package: Started."
    local start=$SECONDS

    local packageDir=$TP_OUT_DIR/$TPB_Configuration/$TPB_TargetFramework/$TPB_TargetRuntime
    local coreCLRPackageDir=$TP_OUT_DIR/$TPB_Configuration/$TPB_TargetFrameworkCore
    local frameworkPackageDirMap=( \
        $TPB_TargetFrameworkCore:$coreCLRPackageDir
    )

    if [[ $TP_USE_REPO_API = 0 ]]; then
       frameworkPackageDirMap+=( \
           $TPB_TargetFramework:$packageDir
       )
    fi

    for fxpkg in "${frameworkPackageDirMap[@]}" ;
    do
        local framework="${fxpkg%%:*}"
        local packageDir="${fxpkg##*:}"
        local projects=( \
            $TP_ROOT_DIR/src/package/package/package.csproj \
            $TP_ROOT_DIR/src/vstest.console/vstest.console.csproj \
            $TP_ROOT_DIR/src/datacollector/datacollector.csproj \
            $TP_ROOT_DIR/src/testhost/testhost.csproj \
            $TP_ROOT_DIR/src/testhost.x86/testhost.x86.csproj
        )

        log "Package: Publish projects for $framework"
        for project in "${projects[@]}" ;
        do
            if [[ $TP_USE_REPO_API = 0 ]]; then
                log ".. Package: Publish $project"
                $dotnet publish $project /bl:publish_$project.binlog --configuration $TPB_Configuration --framework $framework --output $packageDir -v:minimal -p:Version=$TPB_Version -p:CIBuild=$TPB_CIBuild -p:LocalizedBuild=$TPB_LocalizedBuild
            else
                log ".. Package: Publish $project (Source Build)"
                $dotnet publish $project /bl:publish_$project.binlog --configuration $TPB_Configuration --framework $framework --output $packageDir -v:minimal -p:Version=$TPB_Version -p:CIBuild=$TPB_CIBuild -p:LocalizedBuild=$TPB_LocalizedBuild -p:DotNetBuildFromSource=true
            fi
        done

        # Copy TestHost for desktop targets
        local testhost=$packageDir/TestHost
        mkdir -p $testhost

        if [[ $TP_USE_REPO_API = 0 ]]; then
            cp -r src/testhost/bin/$TPB_Configuration/$TPB_TargetFramework/win7-x64/* $testhost
            cp -r src/testhost.x86/bin/$TPB_Configuration/$TPB_TargetFramework/win7-x86/* $testhost
        else
            cp -r src/testhost/bin/$TPB_Configuration/net7.0/* $testhost
            cp -r src/testhost.x86/bin/$TPB_Configuration/net7.0/* $testhost
        fi

        # Copy over the logger assemblies to the Extensions folder.
        local extensionsDir="$packageDir/Extensions"

        if [[ $TP_USE_REPO_API = 1 ]]; then
            log ".. Package: mv (Source Build)"
            local current_tfn="net7.0"
        else
            log ".. Package: mv"
            local current_tfn="netstandard2.0"
        fi

        # Create an extensions directory.
        mkdir -p $extensionsDir

        #*************************************************************************************************************#
        # Note Note: If there are some dependencies for the logger assemblies, those need to be moved too.
        # Ideally we should just be publishing the loggers to the Extensions folder.
        loggers=( \
            "Microsoft.TestPlatform.Extensions.TrxLogger/bin/$TPB_Configuration/$current_tfn/Microsoft.VisualStudio.TestPlatform.Extensions.Trx.TestLogger.dll" \
            "Microsoft.TestPlatform.Extensions.TrxLogger/bin/$TPB_Configuration/$current_tfn/Microsoft.VisualStudio.TestPlatform.Extensions.Trx.TestLogger.pdb" \
            "Microsoft.TestPlatform.Extensions.HtmlLogger/bin/$TPB_Configuration/$current_tfn/Microsoft.VisualStudio.TestPlatform.Extensions.Html.TestLogger.dll" \
            "Microsoft.TestPlatform.Extensions.HtmlLogger/bin/$TPB_Configuration/$current_tfn/Microsoft.VisualStudio.TestPlatform.Extensions.Html.TestLogger.pdb"
        )

        for i in ${loggers[@]}; do
            if [[ $TP_USE_REPO_API = 1 ]]; then
                log ".. Package: ${i} (Source Build)"
            else
                log ".. Package: ${i}"
            fi
            mv "$TP_ROOT_DIR/src/${i}" $extensionsDir
        done
        # TODO: move loc files too
        #*************************************************************************************************************#

        #*************************************************************************************************************#
        #
        blame=( \
            "Microsoft.TestPlatform.Extensions.BlameDataCollector/bin/$TPB_Configuration/$current_tfn/Microsoft.TestPlatform.Extensions.BlameDataCollector.dll" \
            "Microsoft.TestPlatform.Extensions.BlameDataCollector/bin/$TPB_Configuration/$current_tfn/Microsoft.TestPlatform.Extensions.BlameDataCollector.pdb" \
            "DataCollectors/Microsoft.TestPlatform.Extensions.EventLogCollector/bin/$TPB_Configuration/$current_tfn/Microsoft.TestPlatform.Extensions.EventLogCollector.dll" \
            "DataCollectors/Microsoft.TestPlatform.Extensions.EventLogCollector/bin/$TPB_Configuration/$current_tfn/Microsoft.TestPlatform.Extensions.EventLogCollector.pdb"
        )

        for i in ${blame[@]}; do
            if [[ $TP_USE_REPO_API = 1 ]]; then
                log ".. Package: ${i} (Source Build)"
            else
                log ".. Package: ${i}"
            fi
            mv "$TP_ROOT_DIR/src/${i}" $extensionsDir
        done
        # TODO: move loc files too
        #*************************************************************************************************************#

        #*************************************************************************************************************#
        # Note Note: If there are some dependencies for the TestHostRuntimeProvider assemblies, those need to be moved too.
        runtimeproviders=( \
            "Microsoft.TestPlatform.TestHostProvider/bin/$TPB_Configuration/$current_tfn/Microsoft.TestPlatform.TestHostRuntimeProvider.dll" \
            "Microsoft.TestPlatform.TestHostProvider/bin/$TPB_Configuration/$current_tfn/Microsoft.TestPlatform.TestHostRuntimeProvider.pdb"
        )

        for i in ${runtimeproviders[@]}; do
            if [[ $TP_USE_REPO_API = 1 ]]; then
                log ".. Package: ${i} (Source Build)"
            else
                log ".. Package: ${i}"
            fi
            mv "$TP_ROOT_DIR/src/${i}" $extensionsDir
        done
        # TODO: move loc files too
        #*************************************************************************************************************#

        #*************************************************************************************************************#
        externals=( \
            # "microsoft.internal.dia/$TPB_EXTERNALS_VERSION/tools/net451"         \ # no linux support (cp to $testhost)
            # "microsoft.internal.dia.interop/$TPB_EXTERNALS_VERSION/tools/net451" \ # no linux support (cp to $testhost)
            "microsoft.codecoverage.io/$TPB_CC_EXTERNALS_VERSION/lib/netstandard2.0/"
        )

        for i in ${externals[@]}; do
            if [[ $TP_USE_REPO_API = 1 ]]; then
                log ".. Package: ${i} (Source Build)"
            else
                log ".. Package: ${i}"
            fi
            cp -r "${TP_PACKAGES_DIR}${i}"** "$packageDir"
        done
        #*************************************************************************************************************#

        newtonsoft=$TP_PACKAGES_DIR/newtonsoft.json/$TPB_NEWTONSOFT_JSON_VERSION/lib/netstandard2.0/Newtonsoft.Json.dll
        cp $newtonsoft $packageDir
    done

    # For libraries that are externally published, copy the output into artifacts. These will be signed and packaged independently.
    packageName="Microsoft.TestPlatform.Build"
    binariesDirectory="src/$packageName/bin/$TPB_Configuration/**"
    publishDirectory="$TP_OUT_DIR/$TPB_Configuration/$packageName"
    mkdir -p $publishDirectory
    cp -r $binariesDirectory $publishDirectory

    #*************************************************************************************************************#
    #
    assemblies=( \
        "Microsoft.TestPlatform.PlatformAbstractions/bin/$TPB_Configuration/$current_tfn" \
        "Microsoft.TestPlatform.ObjectModel/bin/$TPB_Configuration/$current_tfn"
    )

    for i in ${assemblies[@]}; do
        if [[ $TP_USE_REPO_API = 1 ]]; then
            log ".. Package: ${i} (Source Build)"
        else
            log ".. Package: ${i}"
        fi
        cp -r "$TP_ROOT_DIR/src/${i}" $TP_OUT_DIR/$TPB_Configuration
    done
    # TODO: move loc files too
    #*************************************************************************************************************#

    log "publish_package: Complete. Elapsed $(( SECONDS - start ))s."
}

function create_package()
{
    local failed=false
    local dotnet=$(_get_dotnet_path)

    local start=$SECONDS
    log "Create-NugetPackages: Started."
    stagingDir="$TP_OUT_DIR/$TPB_Configuration"

    if [[ $TP_USE_REPO_API = 0 ]]; then
        packageOutputDir="$TP_OUT_DIR/$TPB_Configuration/packages"
    else
        packageOutputDir="$TP_OUT_DIR/packages/$TPB_Configuration"
    fi

    mkdir -p $packageOutputDir

    nuspecFiles=("TestPlatform.TranslationLayer.nuspec" "TestPlatform.ObjectModel.nuspec" "TestPlatform.ObjectModel.nuspec" "TestPlatform.TestHost.nuspec"\
        "Microsoft.TestPlatform.nuspec" "Microsoft.TestPlatform.Portable.nuspec" "TestPlatform.CLI.nuspec" "TestPlatform.Build.nuspec" "Microsoft.NET.Test.Sdk.nuspec"\
        "Microsoft.CodeCoverage.nuspec" "Microsoft.TestPlatform.AdapterUtilities.nuspec" "TestPlatform.Extensions.TrxLogger.nuspec")
    projectFiles=("Microsoft.TestPlatform.CLI.csproj" "Microsoft.TestPlatform.Build.csproj")
    binDir="$TP_ROOT_DIR/bin/packages"

    for i in ${nuspecFiles[@]}; do
        cp $TP_PACKAGE_NUSPEC_DIR/${i} $stagingDir
    done
    for i in ${projectFiles[@]}; do
        cp $TP_PACKAGE_NUSPEC_DIR/${i} $stagingDir
    done

    # Copy and rename props file.
    cp "$TP_PACKAGE_NUSPEC_DIR/Microsoft.NET.Test.Sdk.props" $stagingDir

    # Copy over empty and third patry notice file
    cp "$TP_PACKAGE_NUSPEC_DIR/_._" $stagingDir
    cp "$TP_PACKAGE_NUSPEC_DIR/../ThirdPartyNotices.txt" $stagingDir
    cp "$TP_PACKAGE_NUSPEC_DIR/../Icon.png" $stagingDir
    cp -r "$TP_PACKAGE_NUSPEC_DIR/../licenses" $stagingDir

    for i in ${projectFiles[@]}; do
        if [[ $TP_USE_REPO_API = 0 ]]; then
            log "$dotnet pack --no-build $stagingDir/${i} -o $packageOutputDir -p:Version=$TPB_Version -p:BranchName=$TPB_BRANCH -p:CommitId=$TPB_COMMIT"

            $dotnet restore $stagingDir/${i} -p:Version=$TPB_Version -p:BranchName="$TPB_BRANCH" -p:CommitId="$TPB_COMMIT" -bl:pack_$i.binlog \
                && $dotnet pack --no-build $stagingDir/${i} -o $packageOutputDir -p:Version=$TPB_Version -p:BranchName="$TPB_BRANCH" -p:CommitId="$TPB_COMMIT" -bl:pack_$i.binlog
        else
            log "$dotnet pack --no-build $stagingDir/${i} -o $packageOutputDir -p:Version=$TPB_Version -p:BranchName=$TPB_BRANCH -p:CommitId=$TPB_COMMIT -p:DotNetBuildFromSource=true (Source Build)"

            $dotnet restore $stagingDir/${i} -p:Version=$TPB_Version -bl:restore_$i.binlog -p:DotNetBuildFromSource=true -p:BranchName="$TPB_BRANCH" -p:CommitId="$TPB_COMMIT" \
                && $dotnet pack --no-build $stagingDir/${i} -o $packageOutputDir -p:Version=$TPB_Version -bl:pack_$i.binlog -p:DotNetBuildFromSource=true -p:BranchName="$TPB_BRANCH" -p:CommitId="$TPB_COMMIT"
        fi
    done



    log "Create-NugetPackages: Elapsed $(( SECONDS - start ))s."
}

#
# Privates
#
_get_dotnet_path()
{
    echo "$TP_DOTNET_DIR/dotnet"
}

# Execute build
start=$SECONDS
log "Build started"
log "Test platform environment variables: "
(set | grep ^TP_)

log "Test platform build variables: "
(set | grep ^TPB_)

if [ -z "$PROJECT_NAME_PATTERNS" ]
then
    install_cli && restore_package && invoke_build && publish_package && create_package
else
    invoke_build
fi

if [[ $? -ne 0 ]]; then
    log "Build failed. Elapsed $(( SECONDS - start ))s."
    exit 1
fi

log "Build complete. Elapsed $(( SECONDS - start ))s."
