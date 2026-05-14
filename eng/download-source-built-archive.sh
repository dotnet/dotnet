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
  local versionPropertyOverride="${7:-}"
  local storageKey="${8:-}"

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

  archiveVersion="${versionDelimiter}${archiveVersion}${versionDelimiter}"

  # Build array of base URL entries to try in order
  # Each entry is in the format "url" or "url=storageKey"
  local baseUrls=()
  
  # Assets for the default RID are hosted in builds.dotnet.microsoft.com first
  if [[ $artifactsRid == $defaultArtifactsRid ]]; then
    baseUrls+=("https://builds.dotnet.microsoft.com/dotnet/source-build")
  fi
  
  # Always try public CI location
  baseUrls+=("https://ci.dot.net/public/source-build")
  
  # Try internal CI location if storage key is provided
  if [[ -n "$storageKey" ]]; then
    baseUrls+=("https://ci.dot.net/internal/source-build=$storageKey")
  fi

  # Determine the archive filename based on property name
  local archiveFileName
  if [[ "$propertyName" == "PrivateSourceBuiltPrebuiltsVersion" ]]; then
    archiveFileName="${prebuiltsBaseFileName}${archiveVersion}${defaultArtifactsRid}.tar.gz"
  elif [[ "$propertyName" == "PrivateSourceBuiltArtifactsVersion" ]]; then
    archiveFileName="${artifactsBaseFileName}${archiveVersion}${artifactsRid}.tar.gz"
  elif [[ "$propertyName" == "PrivateSourceBuiltSdkVersion" ]]; then
    archiveFileName="${sdkBaseFileName}${archiveVersion}${artifactsRid}.tar.gz"
  else
    echo "  ERROR: Unknown archive property name: $propertyName"
    return 1
  fi

  local archiveUrl
  local displayUrl
  local downloadedFilename=$(basename "${archiveFileName}")
  local downloadSucceeded=false

  # Try each base URL in order
  for entry in "${baseUrls[@]}"; do
    # Parse the entry: "url" or "url=storageKey"
    local baseUrl="${entry%%=*}"
    local entryStorageKey=""
    if [[ "$entry" == *"="* ]]; then
      entryStorageKey="${entry#*=}"
    fi
    
    archiveUrl="${baseUrl}/${archiveFileName}"
    displayUrl="$archiveUrl"
    
    # Append storage key as query parameter if present
    if [[ -n "$entryStorageKey" ]]; then
      local decodedKey
      decodedKey=$(echo "$entryStorageKey" | base64 -d)
      archiveUrl="${archiveUrl}?${decodedKey}"
      displayUrl="${displayUrl}?[redacted]"
    fi
    
    echo "  Downloading $label from $displayUrl..."
    if DownloadWithRetries "$archiveUrl" "$outputDir"; then
      downloadSucceeded=true
      break
    else
      local downloadResult=$?
      # Only continue to next URL if it was a 404
      if [[ $downloadResult -ne 22 ]]; then
        echo "  ERROR: Failed to download $displayUrl"
        return 1
      fi
      echo "  Not found, trying next location..."
    fi
  done

  if [[ "$downloadSucceeded" == false ]]; then
    echo "  ERROR: Failed to download from all available locations"
    return 1
  fi

  # Rename the file if a destination filename prefix is provided
  if [[ -n "$destinationFileNamePrefix" ]]; then
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
