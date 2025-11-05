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

# Generic function to download an archive based on property name
function DownloadArchive {
  local label="$1"
  local propertyName="$2"
  local isRequired="$3"
  local artifactsRid="$4"
  local outputDir="$5"
  local destinationFileNamePrefix="${6:-}"
  local versionPropertyOverride="${7:-}"
  local useCILocation="${8:-false}"

  local notFoundMessage="No $label found to download..."

  local archiveVersion
  local versionsPath
  local versionProperty="${versionPropertyOverride:-$propertyName}"
  if [[ "$versionProperty" == "MicrosoftNETSdkPackageVersion" ]]; then
    versionsPath="$PACKAGE_VERSION_DETAILS_PATH"
  else
    versionsPath="$PACKAGE_VERSIONS_PATH"
  fi
  archiveVersion=$(GetXmlPropertyValue "$versionProperty" "$versionsPath")
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

  local versionDelimiter="."
  if [[ "$propertyName" == "PrivateSourceBuiltSdkVersion" ]]; then
    versionDelimiter="-"
  fi

  # Assets for the default RID are hosted in builds.dotnet.microsoft.com
  if [[ $artifactsRid == $defaultArtifactsRid ]]; then
    useCILocation=false
  fi

  archiveVersion="${versionDelimiter}${archiveVersion}${versionDelimiter}"

  baseUrl="https://builds.dotnet.microsoft.com/source-built-artifacts/assets"
  if [[ "$useCILocation" == true ]]; then
    baseUrl="https://ci.dot.net/public/source-build"
  fi

  if [[ "$propertyName" == "PrivateSourceBuiltPrebuiltsVersion" ]]; then
    archiveUrl="${baseUrl}/${prebuiltsBaseFileName}${archiveVersion}${defaultArtifactsRid}.tar.gz"
  elif [[ "$propertyName" == "PrivateSourceBuiltArtifactsVersion" ]]; then
    archiveUrl="${baseUrl}/${artifactsBaseFileName}${archiveVersion}${artifactsRid}.tar.gz"
  elif [[ "$propertyName" == "PrivateSourceBuiltSdkVersion" ]]; then
    archiveUrl="${baseUrl}/${sdkBaseFileName}${archiveVersion}${artifactsRid}.tar.gz"
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
    # Extract the suffix from the downloaded filename
    local baseFilenameForSuffix="$artifactsBaseFileName"
    if [[ "$propertyName" == *Prebuilts* ]]; then
      baseFilenameForSuffix="$prebuiltsBaseFileName"
    elif [[ "$propertyName" == *Sdk* ]]; then
      baseFilenameForSuffix="$sdkBaseFileName"
    fi
    local suffix="${downloadedFilename#$baseFilenameForSuffix}"
    local newFilename="$destinationFileNamePrefix$suffix"
    mv "$outputDir/$downloadedFilename" "$outputDir/$newFilename"
    echo "  Renamed $downloadedFilename to $newFilename"
  fi

  return 0
}
