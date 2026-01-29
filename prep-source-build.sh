#!/usr/bin/env bash

### Usage: $0
###
###   Prepares the environment for a source build by downloading Private.SourceBuilt.Artifacts.*.tar.gz,
###   installing the version of dotnet referenced in global.json,
###   and detecting binaries and removing any non-SB allowed binaries.
###
### Options:
###   --configuration <value>     Build configuration: 'Debug' or 'Release' (short: -c)
###                               Default is Release
###   --no-artifacts              Exclude the download of the previously source-built artifacts archive
###   --no-bootstrap              Don't replace portable packages in the download source-built artifacts
###   --no-prebuilts              Exclude the download of the prebuilts archive
###   --no-sdk                    Exclude the download of the .NET SDK
###   --artifacts-rid             The RID of the previously source-built artifacts archive to download
###                               Default is centos.9-x64
###   --bootstrap-rid <value>     The (portable) RID for the bootstrap artifacts to restore. For example, linux-arm64, linux-musl-x64
###                               Default is the current portable RID.
###   --runtime-source-feed       URL of a remote server or a local directory, from which SDKs and
###                               runtimes can be downloaded
###   --runtime-source-feed-key   Key for accessing the above server, if necessary
###
### Binary-Tooling options:
###   --no-binary-removal         Don't remove non-SB allowed binaries
###   --with-sdk                  Use the SDK in the specified directory
###                               Default is the .NET SDK
###   --with-packages             Specified directory to use as the source feed for packages
###                               Default is the previously source-built artifacts archive.

set -euo pipefail
IFS=$'\n\t'

source="${BASH_SOURCE[0]}"
REPO_ROOT="$( cd -P "$( dirname "$0" )" && pwd )"

# Load common helper functions
source "$REPO_ROOT/eng/download-source-built-archive.sh"
source "$REPO_ROOT/eng/source-build-toolset-init.sh"

function print_help () {
    sed -n '/^### /,/^$/p' "$source" | cut -b 5-
}

packagesDir="$REPO_ROOT/prereqs/packages"

# Common settings
configuration='Release'

# SB prep default arguments
defaultArtifactsRid='centos.10-x64'

# Binary Tooling default arguments
defaultPsbDir="$packagesDir/previously-source-built"

# SB prep arguments
buildBootstrap=true
downloadArtifacts=true
downloadPrebuilts=true
removeBinaries=true
installDotnet=true
artifactsRid=$defaultArtifactsRid
bootstrap_rid=''
runtime_source_feed='' # IBM requested these to support s390x scenarios
runtime_source_feed_key='' # IBM requested these to support s390x scenarios

# Binary Tooling arguments
customSdkDir=''
psbDir=$defaultPsbDir

artifactsBaseFileName="Private.SourceBuilt.Artifacts"
artifactsTarballPattern="$artifactsBaseFileName.*.tar.gz"

prebuiltsBaseFileName="Private.SourceBuilt.Prebuilts"
prebuiltsTarballPattern="$prebuiltsBaseFileName.*.tar.gz"

positional_args=()
while :; do
  if [ $# -le 0 ]; then
    break
  fi
  lowerI="$(echo "$1" | awk '{print tolower($0)}')"
  case $lowerI in
    "-?"|-h|--help)
      print_help
      exit 0
      ;;
    --configuration|-c)
      configuration=$2
      shift
      ;;
    --no-bootstrap)
      buildBootstrap=false
      ;;
    --no-artifacts)
      downloadArtifacts=false
      ;;
    --no-prebuilts)
      downloadPrebuilts=false
      ;;
    --no-sdk)
      installDotnet=false
      ;;
    --artifacts-rid)
      artifactsRid=$2
      shift
      ;;
    --bootstrap-rid)
      bootstrap_rid=$2
      shift
      ;;
    --runtime-source-feed)
      runtime_source_feed=$2
      shift
      ;;
    --runtime-source-feed-key)
      runtime_source_feed_key=$2
      shift
      ;;
    --no-binary-removal)
      removeBinaries=false
      ;;
    --with-sdk)
      customSdkDir="$(cd -P "$2" && pwd)"
      shift
      ;;
    --with-packages)
      psbDir="$(cd -P "$2" && pwd)"
      if [ ! -d "$psbDir" ]; then
          echo "Custom previously built packages directory '$psbDir' does not exist"
          exit 1
      fi
      shift
      ;;
    *)
      positional_args+=("$1")
      ;;
  esac

  shift
done

source "$REPO_ROOT/eng/common/tools.sh"

# Default properties
properties=( "/p:Configuration=$configuration" )
properties+=( "/p:DotNetBuildSourceOnly=true" )

function BootstrapArtifacts {
  DOTNET_SDK_PATH="$REPO_ROOT/.dotnet"
  local tarballDir="$1"
  local tarball=$(find "$tarballDir" -maxdepth 1 -name "$artifactsTarballPattern" | head -n 1)

  echo "  Building bootstrap from $tarballDir"

  # Extract PackageVersions.props from the found tarball
  if [ -f "$tarball" ]; then
    echo "  Extracting PackageVersions.props from $tarball"
    workingDir="$REPO_ROOT/artifacts/prep-bootstrap"
    mkdir -p "$workingDir"
    tar -xzf "$tarball" -C "$workingDir" PackageVersions.props || true
  else
    echo "  ERROR: Tarball $tarball not found in $tarballDir"
    exit 1
  fi

  # Copy bootstrap project to working dir
  cp "$REPO_ROOT/eng/bootstrap/buildBootstrapPreviouslySB.csproj" "$workingDir"

  # Copy NuGet.config from the sdk repo to have the right feeds
  cp "$REPO_ROOT/src/sdk/NuGet.config" "$workingDir"

  local bootstrapProperties=( "${properties[@]}" )
  bootstrapProperties+=( "/p:ArchiveDir=$packagesArchiveDir" )
  if [[ -n "$bootstrap_rid" ]]; then
    bootstrapProperties+=( "/p:PortableTargetRid=$bootstrap_rid" )
  fi

  # Run restore on project to initiate download of bootstrap packages
  "$DOTNET_SDK_PATH/dotnet" restore "$workingDir/buildBootstrapPreviouslySB.csproj" /bl:$log_dir/prep-bootstrap.binlog /fileLoggerParameters:LogFile=$log_dir/prep-bootstrap.log "${bootstrapProperties[@]}"

  # Remove working directory
  rm -rf "$workingDir"
}

# Attempting to bootstrap without an SDK will fail. So either the --no-sdk flag must be passed,
# a pre-existing .dotnet SDK directory must exist, or a custom SDK must be provided via --with-sdk.
if [ "$buildBootstrap" == true ] && [ "$installDotnet" == false ] && [ ! -d "$REPO_ROOT/.dotnet" ] && [ -z "$customSdkDir" ]; then
  echo "  ERROR: --no-sdk requires --no-bootstrap, a pre-existing .dotnet SDK directory, or --with-sdk.  Exiting..."
  exit 1
fi

# Check to make sure curl exists to download the archive files
if ! command -v curl &> /dev/null
then
  echo "  ERROR: curl not found.  Exiting..."
  exit 1
fi

# Check if Private.SourceBuilt artifacts archive exists
downloadPsbArtifacts=$downloadArtifacts
packagesArchiveDir="$packagesDir/archive/"
if [ "$downloadArtifacts" == true ] && [ -f ${packagesArchiveDir}${artifactsTarballPattern} ]; then
  echo "  $artifactsTarballPattern exists in $packagesArchiveDir...it will not be downloaded"
  downloadPsbArtifacts=false
fi

# Check if Private.SourceBuilt prebuilts archive exists
if [ "$downloadPrebuilts" == true ] && [ -f ${packagesArchiveDir}${prebuiltsTarballPattern} ]; then
  echo "  $prebuiltsTarballPattern exists in $packagesArchiveDir...it will not be downloaded"
  downloadPrebuilts=false
fi

# Check if dotnet is installed
expectedSdkVersion=$(GetXmlPropertyValue "PrivateSourceBuiltSdkVersion" "$REPO_ROOT/eng/Versions.props")
if [ "$installDotnet" == true ] && [ -d "$REPO_ROOT/.dotnet" ]; then
  installedVersions=$("$REPO_ROOT/.dotnet/dotnet" --list-sdks | awk '{print $1}')
  if grep -qx "$expectedSdkVersion" <<< "${installedVersions[*]}"; then
    echo "  Skipping SDK installation - version $expectedSdkVersion detected"
    installDotnet=false
  fi
fi

# Check for the version of dotnet to install
if [ "$installDotnet" == true ]; then
  echo "  Installing .NET SDK $expectedSdkVersion"
  use_installed_dotnet_cli=false
  (source ./eng/common/tools.sh && InitializeDotNetCli true)
fi

# Read the eng/Versions.props to get the archives to download and download them
if [ "$downloadPsbArtifacts" == true ]; then
  DownloadArchive "previously source-built artifacts" "PrivateSourceBuiltArtifactsVersion" true "$artifactsRid" "$packagesArchiveDir"

  if [ "$buildBootstrap" == true ]; then
    BootstrapArtifacts "$packagesArchiveDir"
  fi
fi

if [ "$downloadPrebuilts" == true ]; then
  DownloadArchive "prebuilts" "PrivateSourceBuiltPrebuiltsVersion" false "$artifactsRid" "$packagesArchiveDir"
fi

if [ "$removeBinaries" == true ]; then

  originalPackagesDir=$packagesDir
  # Create working directory for extracting packages
  workingDir=$(mktemp -d)

  toolsetInitProperties=( "${properties[@]}" )
  toolsetInitProperties+=( "/p:DisableSharedComponentValidation=true" )

  # If --with-packages is not passed, unpack PSB artifacts
  if [[ $psbDir == $defaultPsbDir ]]; then
    echo "  Extracting previously source-built to $workingDir"
    sourceBuiltArchive=$(find "$packagesArchiveDir" -maxdepth 1 -name "$artifactsTarballPattern")

    if [ ! -f "$sourceBuiltArchive" ]; then
      echo "  ERROR: $artifactsTarballPattern does not exist..."\
            "Cannot remove non-SB allowed binaries. Either pass --with-packages or download the artifacts."
      exit 1
    fi

    echo "  Unpacking $artifactsTarballPattern into $workingDir"
    tar -xzf "$sourceBuiltArchive" -C "$workingDir"

    psbDir=$workingDir
  else
    echo "  Using previously source-built packages from $psbDir"
    toolsetInitProperties+=( "/p:CustomPreviouslySourceBuiltPackagesPath=$psbDir" )
  fi


  # Initialize source-only toolset for binary detection (includes custom SDK setup, MSBuild resolver, and source-built resolver)
  source_only_toolset_init "$customSdkDir" "$psbDir" "true" "" "${toolsetInitProperties[@]}"

  "$_InitializeBuildTool" build \
    "$REPO_ROOT/eng/init-detect-binaries.proj" \
    "${properties[@]}" \
    "/p:BinariesMode=Clean" \
    "/p:AllowedBinariesFile=$REPO_ROOT/eng/allowed-sb-binaries.txt" \
    "/p:BinariesPackagesDir=$psbDir" \
    "/bl:$log_dir/prep-remove-binaries.binlog" \
    "/fileLoggerParameters:LogFile=$log_dir/prep-remove-binaries.log" \
    "${positional_args[@]}"

  rm -rf "$workingDir"

  psbDir="$originalPackagesDir"
  unset originalPackagesDir
fi
