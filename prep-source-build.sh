#!/usr/bin/env bash

### Usage: $0
###
###   Prepares the environment for a source build by downloading Private.SourceBuilt.Artifacts.*.tar.gz,
###   installing the version of dotnet referenced in global.json,
###   and detecting binaries and removing any non-SB allowed binaries.
###
### Options:
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

function print_help () {
    sed -n '/^### /,/^$/p' "$source" | cut -b 5-
}

packagesDir="$REPO_ROOT/prereqs/packages"

# SB prep default arguments
defaultArtifactsRid='centos.10-x64'

# Binary Tooling default arguments
defaultDotnetSdk="$REPO_ROOT/.dotnet"
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
dotnetSdk=$defaultDotnetSdk
psbDir=$defaultPsbDir

artifactsBaseFileName="Private.SourceBuilt.Artifacts"
artifactsTarballPattern="$artifactsBaseFileName.*.tar.gz"

sharedComponentsBaseFileName="Private.SourceBuilt.SharedComponents"
sharedComponentsTarballPattern="$sharedComponentsBaseFileName.*.tar.gz"

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
      dotnetSdk=$2
      shift
      ;;
    --with-packages)
      psbDir=$2
      shift
      ;;
    *)
      positional_args+=("$1")
      ;;
  esac

  shift
done

# Attempting to bootstrap without an SDK will fail. So either the --no-sdk flag must be passed
# or a pre-existing .dotnet SDK directory must exist.
if [ "$buildBootstrap" == true ] && [ "$installDotnet" == false ] && [ ! -d "$REPO_ROOT/.dotnet" ]; then
  echo "  ERROR: --no-sdk requires --no-bootstrap or a pre-existing .dotnet SDK directory.  Exiting..."
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

# Check if shared components archive exists
downloadSharedComponentsArtifacts=$downloadArtifacts
if [ "$downloadArtifacts" == true ] && [ -f ${packagesArchiveDir}${sharedComponentsTarballPattern} ]; then
  echo "  $sharedComponentsTarballPattern exists in $packagesArchiveDir...it will not be downloaded"
  downloadSharedComponentsArtifacts=false
fi

# Check if Private.SourceBuilt prebuilts archive exists
if [ "$downloadPrebuilts" == true ] && [ -f ${packagesArchiveDir}${prebuiltsTarballPattern} ]; then
  echo "  $prebuiltsTarballPattern exists in $packagesArchiveDir...it will not be downloaded"
  downloadPrebuilts=false
fi

# Check if dotnet is installed
if [ "$installDotnet" == true ] && [ -d "$REPO_ROOT/.dotnet" ]; then
  echo "  ./.dotnet SDK directory exists...it will not be installed"
  installDotnet=false;
fi

# Helper to extract a property value from an XML file
function GetXmlPropertyValue {
  local propName="$1"
  local filePath="$2"
  local value=""
  local line pattern
  line=$(grep -m 1 "<$propName>" "$filePath" || :)
  pattern="<$propName>(.*)</$propName>"
  if [[ $line =~ $pattern ]]; then
    value="${BASH_REMATCH[1]}"
  fi
  echo "$value"
}

# Helper to download a file with retries
function DownloadWithRetries {
  local url="$1"
  local targetDir="$2"
  (
    cd "$targetDir" &&
    for i in {1..5}; do
      if curl -fL --retry 5 -O "$url"; then
        return 0
      else
        case $? in
          18)
            sleep 3
            ;;
          *)
            return 1
            ;;
        esac
      fi
    done
    return 1
  )
}

function DownloadArchive {
  local label="$1"
  local propertyName="$2"
  local isRequired="$3"
  local artifactsRid="$4"
  local outputDir="$5"
  local destinationFilenamePrefix="${6:-}"

  local packageVersionsPath="$REPO_ROOT/eng/Versions.props"
  local notFoundMessage="No $label found to download..."

  local archiveVersion
  archiveVersion=$(GetXmlPropertyValue "$propertyName" "$packageVersionsPath")
  if [[ -z "$archiveVersion" ]]; then
    if [ "$isRequired" == true ]; then
      echo "  ERROR: $notFoundMessage"
      exit 1
    else
      echo "  $notFoundMessage"
      return
    fi
  fi

  local archiveUrl
  if [[ "$propertyName" == "MicrosoftNETSdkVersion" ]]; then
    archiveUrl="https://ci.dot.net/public/source-build/$artifactsBaseFileName.$archiveVersion.$artifactsRid.tar.gz"
  elif [[ "$propertyName" == *Prebuilts* ]]; then
    archiveUrl="https://builds.dotnet.microsoft.com/source-built-artifacts/assets/$prebuiltsBaseFileName.$archiveVersion.$defaultArtifactsRid.tar.gz"
  elif [[ "$propertyName" == *Artifacts* ]]; then
    archiveUrl="https://builds.dotnet.microsoft.com/source-built-artifacts/assets/$artifactsBaseFileName.$archiveVersion.$artifactsRid.tar.gz"
  else
    echo "  ERROR: Unknown archive property name: $propertyName"
    exit 1
  fi

  echo "  Downloading $label from $archiveUrl..."
  if ! DownloadWithRetries "$archiveUrl" "$outputDir"; then
    echo "  ERROR: Failed to download $archiveUrl"
    exit 1
  fi

  # Rename the file if a destination filename prefix is provided
  if [[ -n "$destinationFilenamePrefix" ]]; then
    local downloadedFilename
    downloadedFilename=$(basename "$archiveUrl")
    # Extract the suffix from the downloaded filename
    local suffix="${downloadedFilename#$artifactsBaseFileName}"
    local newFilename="$destinationFilenamePrefix$suffix"
    mv "$outputDir/$downloadedFilename" "$outputDir/$newFilename"
    echo "  Renamed $downloadedFilename to $newFilename"
  fi
}

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

  properties=( "/p:ArchiveDir=$packagesArchiveDir" )
  if [[ -n "$bootstrap_rid" ]]; then
    properties+=( "/p:PortableTargetRid=$bootstrap_rid" )
  fi

  # Run restore on project to initiate download of bootstrap packages
  "$DOTNET_SDK_PATH/dotnet" restore "$workingDir/buildBootstrapPreviouslySB.csproj" /bl:artifacts/log/prep-bootstrap.binlog /fileLoggerParameters:LogFile=artifacts/log/prep-bootstrap.log "${properties[@]}"

  # Remove working directory
  rm -rf "$workingDir"
}

# Check for the version of dotnet to install
if [ "$installDotnet" == true ]; then
  echo "  Installing dotnet..."
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

if [ "$downloadSharedComponentsArtifacts" == true ]; then
  source $REPO_ROOT/eng/common/native/init-os-and-arch.sh
  source $REPO_ROOT/eng/common/native/init-distro-rid.sh
  initDistroRidGlobal "$os" "$arch" ""

  DownloadArchive "shared component artifacts" "MicrosoftNETSdkVersion" false "$__DistroRid" "$packagesArchiveDir" "$sharedComponentsBaseFileName"
fi

if [ "$downloadPrebuilts" == true ]; then

  DownloadArchive "prebuilts" "PrivateSourceBuiltPrebuiltsVersion" false "$artifactsRid" "$packagesArchiveDir"
fi

if [ "$removeBinaries" == true ]; then

  originalPackagesDir=$packagesDir
  # Create working directory for extracking packages
  workingDir=$(mktemp -d)

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
  fi

  "$dotnetSdk/dotnet" build \
    "$REPO_ROOT/eng/init-detect-binaries.proj" \
    "/p:BinariesMode=Clean" \
    "/p:AllowedBinariesFile=$REPO_ROOT/eng/allowed-sb-binaries.txt" \
    "/p:BinariesPackagesDir=$psbDir" \
    "/bl:artifacts/log/prep-remove-binaries.binlog" \
    "/fileLoggerParameters:LogFile=artifacts/log/prep-remove-binaries.log" \
    "${positional_args[@]}"

  rm -rf "$workingDir"

  psbDir="$originalPackagesDir"
  unset originalPackagesDir
fi
