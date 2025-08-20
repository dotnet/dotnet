#!/usr/bin/env bash

# Setup SDK overrides for offline source builds
# This function extracts and configures SDK packages from bootstrap archives
# to enable offline builds that don't rely on online NuGet feeds.

function SetupOfflineSdks {
  local scriptroot="$1"
  local packagesArchiveDir="$2"
  local packagesPreviouslySourceBuiltDir="$3"
  local packageVersionsPath=""
  local NUGET_PACKAGES="${NUGET_PACKAGES:-$HOME/.nuget/packages/}"

  # Ensure NUGET_PACKAGES ends with a slash
  if [[ "${NUGET_PACKAGES}" != */ ]]; then
    NUGET_PACKAGES="${NUGET_PACKAGES}/"
  fi

  echo "  Setting up offline SDK overrides..."

  # Try to find PackageVersions.props in order of preference
  if [[ -n "${CUSTOM_PACKAGES_DIR:-}" ]] && [[ -f "$CUSTOM_PACKAGES_DIR/PackageVersions.props" ]]; then
    packageVersionsPath="$CUSTOM_PACKAGES_DIR/PackageVersions.props"
  elif [[ -f "${packagesPreviouslySourceBuiltDir}/PackageVersions.props" ]]; then
    packageVersionsPath="${packagesPreviouslySourceBuiltDir}/PackageVersions.props"
  else
    # Try to extract from source-built archive
    sourceBuiltArchive=$(find "$packagesArchiveDir" -maxdepth 1 -name "Private.SourceBuilt.Artifacts.*.tar.gz" | head -n 1)
    if [[ -f "$sourceBuiltArchive" ]]; then
      echo "  Extracting PackageVersions.props from $sourceBuiltArchive"
      tmpDir=$(mktemp -d)
      tar -xzf "$sourceBuiltArchive" -C "$tmpDir" PackageVersions.props 2>/dev/null || true
      if [[ -f "$tmpDir/PackageVersions.props" ]]; then
        packageVersionsPath="$tmpDir/PackageVersions.props"
      fi
    fi
  fi

  if [[ ! -f "$packageVersionsPath" ]]; then
    echo "  WARNING: Cannot find PackageVersions.props. SDK overrides may not work correctly."
    echo "  Attempted paths:"
    echo "    Custom packages: ${CUSTOM_PACKAGES_DIR:-}/PackageVersions.props"
    echo "    Previously source-built: ${packagesPreviouslySourceBuiltDir}/PackageVersions.props"
    echo "    Archive dir: $packagesArchiveDir"
    echo "  Will try to use global.json for SDK versions..."
    packageVersionsPath=""  # Clear this so we skip the Arcade SDK setup
  fi

  echo "  Using PackageVersions.props from: $packageVersionsPath"

  # 1. Microsoft.DotNet.Arcade.Sdk
  if [[ -n "$packageVersionsPath" ]]; then
    arcadeSdkLine=$(grep -m 1 'MicrosoftDotNetArcadeSdkVersion' "$packageVersionsPath" 2>/dev/null || true)
    arcadeSdkPattern="<MicrosoftDotNetArcadeSdkVersion>(.*)</MicrosoftDotNetArcadeSdkVersion>"
    if [[ $arcadeSdkLine =~ $arcadeSdkPattern ]]; then
      export ARCADE_BOOTSTRAP_VERSION=${BASH_REMATCH[1]}
      export SOURCE_BUILT_SDK_ID_ARCADE=Microsoft.DotNet.Arcade.Sdk
      export SOURCE_BUILT_SDK_VERSION_ARCADE=$ARCADE_BOOTSTRAP_VERSION
      export SOURCE_BUILT_SDK_DIR_ARCADE=${NUGET_PACKAGES}BootstrapPackages/microsoft.dotnet.arcade.sdk/$ARCADE_BOOTSTRAP_VERSION
      echo "  Set up Arcade SDK override: $ARCADE_BOOTSTRAP_VERSION"
    fi
  fi

  # 2. Microsoft.Build.NoTargets
  notargetsSdkLine=$(grep -m 1 'Microsoft.Build.NoTargets' "$scriptroot/global.json" 2>/dev/null || true)
  notargetsSdkPattern="\"Microsoft\.Build\.NoTargets\" *: *\"(.*)\""
  if [[ $notargetsSdkLine =~ $notargetsSdkPattern ]]; then
    export NOTARGETS_BOOTSTRAP_VERSION=${BASH_REMATCH[1]}
    export SOURCE_BUILT_SDK_ID_NOTARGETS=Microsoft.Build.NoTargets
    export SOURCE_BUILT_SDK_VERSION_NOTARGETS=$NOTARGETS_BOOTSTRAP_VERSION
    export SOURCE_BUILT_SDK_DIR_NOTARGETS=${NUGET_PACKAGES}BootstrapPackages/microsoft.build.notargets/$NOTARGETS_BOOTSTRAP_VERSION
    echo "  Set up NoTargets SDK override: $NOTARGETS_BOOTSTRAP_VERSION"
  fi

  # 3. Microsoft.Build.Traversal
  traversalSdkLine=$(grep -m 1 'Microsoft.Build.Traversal' "$scriptroot/global.json" 2>/dev/null || true)
  traversalSdkPattern="\"Microsoft\.Build\.Traversal\" *: *\"(.*)\""
  if [[ $traversalSdkLine =~ $traversalSdkPattern ]]; then
    export TRAVERSAL_BOOTSTRAP_VERSION=${BASH_REMATCH[1]}
    export SOURCE_BUILT_SDK_ID_TRAVERSAL=Microsoft.Build.Traversal
    export SOURCE_BUILT_SDK_VERSION_TRAVERSAL=$TRAVERSAL_BOOTSTRAP_VERSION
    export SOURCE_BUILT_SDK_DIR_TRAVERSAL=${NUGET_PACKAGES}BootstrapPackages/microsoft.build.traversal/$TRAVERSAL_BOOTSTRAP_VERSION
    echo "  Set up Traversal SDK override: $TRAVERSAL_BOOTSTRAP_VERSION"
  fi

  # Clean up temporary directory if created
  if [[ -n "${tmpDir:-}" && -d "$tmpDir" ]]; then
    rm -rf "$tmpDir"
  fi
}