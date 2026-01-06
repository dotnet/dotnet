#!/bin/bash

# Common helper functions for downloading archives based on versions in XML files

# Get the repository root directory
REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

PACKAGE_VERSIONS_PATH="$REPO_ROOT/eng/Versions.props"
PACKAGE_VERSION_DETAILS_PATH="$REPO_ROOT/eng/Version.Details.props"

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
        local exitCode=$?
        case $exitCode in
          22)
            # HTTP error (including 404) - don't retry
            return 22
            ;;
          *)
            # For all other errors (including partial transfers), sleep and retry
            sleep 3
            ;;
        esac
      fi
    done
    return 1
  )
}

# Generic function to download an archive based on property name
function DownloadArchive {
  local label="$1"
  local propertyName="$2"
  local isRequired="$3"
  local artifactsRid="$4"
  local outputDir="$5"
  local destinationFileNamePrefix="${6:-}"
  local archiveBaseFileNameOverride="${7:-}"

  local notFoundMessage="No $label found to download..."
  local sdkVersionProperty="MicrosoftNETSdkPackageVersion"

  local archiveVersion
  local versionsPath
  if [[ "$propertyName" == "$sdkVersionProperty" ]]; then
    versionsPath="$PACKAGE_VERSION_DETAILS_PATH"
  else
    versionsPath="$PACKAGE_VERSIONS_PATH"
  fi
  archiveVersion=$(GetXmlPropertyValue "$propertyName" "$versionsPath")
  if [[ -z "$archiveVersion" ]]; then
    if [ "$isRequired" == true ]; then
      echo "  ERROR: $notFoundMessage"
      return 1
    else
      echo "  $notFoundMessage"
      return 0
    fi
  fi

  local archiveUrl
  local artifactsBaseFileName="Private.SourceBuilt.Artifacts"
  local prebuiltsBaseFileName="Private.SourceBuilt.Prebuilts"
  local sdkBaseFileName="dotnet-sdk"
  local defaultArtifactsRid='centos.10-x64'

  # Use override base filename if provided
  if [[ -n "$archiveBaseFileNameOverride" ]]; then
    artifactsBaseFileName="$archiveBaseFileNameOverride"
    prebuiltsBaseFileName="$archiveBaseFileNameOverride"
    sdkBaseFileName="$archiveBaseFileNameOverride"
  fi

  local versionDelimiter="."
  if [[ "$propertyName" == "PrivateSourceBuiltSdkVersion" ]] || [[ "$archiveBaseFileNameOverride" == *sdk* ]]; then
    versionDelimiter="-"
  fi

  archiveVersion="${versionDelimiter}${archiveVersion}${versionDelimiter}"

  if [[ "$propertyName" == "$sdkVersionProperty" ]]; then
    archiveUrl="https://ci.dot.net/public/source-build/${artifactsBaseFileName}${archiveVersion}${artifactsRid}.tar.gz"
  elif [[ "$propertyName" == "PrivateSourceBuiltPrebuiltsVersion" ]]; then
    archiveUrl="https://builds.dotnet.microsoft.com/dotnet/source-build/${prebuiltsBaseFileName}${archiveVersion}${defaultArtifactsRid}.tar.gz"
  elif [[ "$propertyName" == "PrivateSourceBuiltArtifactsVersion" ]]; then
    archiveUrl="https://builds.dotnet.microsoft.com/dotnet/source-build/${artifactsBaseFileName}${archiveVersion}${artifactsRid}.tar.gz"
  elif [[ "$propertyName" == "PrivateSourceBuiltSdkVersion" ]]; then
    archiveUrl="https://builds.dotnet.microsoft.com/dotnet/source-build/${sdkBaseFileName}${archiveVersion}${artifactsRid}.tar.gz"
  else
    echo "  ERROR: Unknown archive property name: $propertyName"
    return 1
  fi

  echo "  Downloading $label from $archiveUrl..."
  if ! DownloadWithRetries "$archiveUrl" "$outputDir"; then
    echo "  ERROR: Failed to download $archiveUrl"
    return 1
  fi

  # Rename the file if a destination filename prefix is provided
  if [[ -n "$destinationFileNamePrefix" ]]; then
    local downloadedFilename
    downloadedFilename=$(basename "$archiveUrl")
    # Extract the suffix from the downloaded filename using the appropriate base filename
    local baseFilenameForSuffix="$artifactsBaseFileName"
    if [[ "$propertyName" == *Prebuilts* ]]; then
      baseFilenameForSuffix="$prebuiltsBaseFileName"
    fi
    local suffix="${downloadedFilename#$baseFilenameForSuffix}"
    local newFilename="$destinationFileNamePrefix$suffix"
    mv "$outputDir/$downloadedFilename" "$outputDir/$newFilename"
    echo "  Renamed $downloadedFilename to $newFilename"
  fi

  return 0
}
