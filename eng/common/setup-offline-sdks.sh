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

  # Get SDK versions from global.json first since they're always needed
  local notargetsSdkLine traversalSdkLine
  notargetsSdkLine=$(grep -m 1 'Microsoft.Build.NoTargets' "$scriptroot/global.json" 2>/dev/null || true)
  notargetsSdkPattern="\"Microsoft\.Build\.NoTargets\" *: *\"(.*)\""
  if [[ $notargetsSdkLine =~ $notargetsSdkPattern ]]; then
    export NOTARGETS_BOOTSTRAP_VERSION=${BASH_REMATCH[1]}
    export SOURCE_BUILT_SDK_ID_NOTARGETS=Microsoft.Build.NoTargets
    export SOURCE_BUILT_SDK_VERSION_NOTARGETS=$NOTARGETS_BOOTSTRAP_VERSION
    export SOURCE_BUILT_SDK_DIR_NOTARGETS=${NUGET_PACKAGES}BootstrapPackages/microsoft.build.notargets/$NOTARGETS_BOOTSTRAP_VERSION
    echo "  Set up NoTargets SDK override: $NOTARGETS_BOOTSTRAP_VERSION"
  fi

  traversalSdkLine=$(grep -m 1 'Microsoft.Build.Traversal' "$scriptroot/global.json" 2>/dev/null || true)
  traversalSdkPattern="\"Microsoft\.Build\.Traversal\" *: *\"(.*)\""
  if [[ $traversalSdkLine =~ $traversalSdkPattern ]]; then
    export TRAVERSAL_BOOTSTRAP_VERSION=${BASH_REMATCH[1]}
    export SOURCE_BUILT_SDK_ID_TRAVERSAL=Microsoft.Build.Traversal
    export SOURCE_BUILT_SDK_VERSION_TRAVERSAL=$TRAVERSAL_BOOTSTRAP_VERSION
    export SOURCE_BUILT_SDK_DIR_TRAVERSAL=${NUGET_PACKAGES}BootstrapPackages/microsoft.build.traversal/$TRAVERSAL_BOOTSTRAP_VERSION
    echo "  Set up Traversal SDK override: $TRAVERSAL_BOOTSTRAP_VERSION"
  fi

  # 1. Microsoft.DotNet.Arcade.Sdk - only if we have PackageVersions.props
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

  # Extract bootstrap packages if they don't exist
  ExtractBootstrapPackages "$packagesPreviouslySourceBuiltDir"

  # Clean up temporary directory if created
  if [[ -n "${tmpDir:-}" && -d "$tmpDir" ]]; then
    rm -rf "$tmpDir"
  fi
}

function ExtractBootstrapPackages {
  local packagesDir="$1"
  local NUGET_PACKAGES="${NUGET_PACKAGES:-$HOME/.nuget/packages/}"
  
  # Ensure NUGET_PACKAGES ends with a slash
  if [[ "${NUGET_PACKAGES}" != */ ]]; then
    NUGET_PACKAGES="${NUGET_PACKAGES}/"
  fi
  
  echo "  Setting up bootstrap SDK packages for offline resolution"
  
  # Create bootstrap packages directory
  local bootstrapDir="${NUGET_PACKAGES}BootstrapPackages"
  mkdir -p "$bootstrapDir"
  
  # Extract SDK packages if available and not already extracted
  local packages=("microsoft.build.notargets" "microsoft.build.traversal" "microsoft.dotnet.arcade.sdk")
  local packageNames=("Microsoft.Build.NoTargets" "Microsoft.Build.Traversal" "Microsoft.DotNet.Arcade.Sdk")
  local packageVersions=("$NOTARGETS_BOOTSTRAP_VERSION" "$TRAVERSAL_BOOTSTRAP_VERSION" "${ARCADE_BOOTSTRAP_VERSION:-}")
  
  for i in "${!packages[@]}"; do
    local package="${packages[$i]}"
    local packageName="${packageNames[$i]}"
    local version="${packageVersions[$i]}"
    
    if [[ -n "$version" ]]; then
      local targetDir="$bootstrapDir/$package/$version"
      local nupkgFile="$packagesDir/$packageName.$version.nupkg"
      
      if [[ -f "$nupkgFile" ]] && [[ ! -d "$targetDir" ]]; then
        echo "    Extracting $packageName $version"
        mkdir -p "$targetDir"
        (cd "$targetDir" && unzip -q "$nupkgFile" 2>/dev/null || true)
      elif [[ -d "$targetDir" ]]; then
        echo "    $packageName $version already extracted"
      elif [[ ! -f "$nupkgFile" ]]; then
        echo "    WARNING: $nupkgFile not found - SDK resolution may fail in offline scenarios"
        echo "      Expected: $nupkgFile"
        echo "      Make sure PSB artifacts are downloaded or use --with-packages"
      fi
    fi
  done
  
  # Create a simple SDK resolver manifest if the UnifiedBuild resolver is available
  local sdkResolversDir="${DOTNET_ROOT:-$HOME/.dotnet}/sdk/*/SdkResolvers"
  if ls $sdkResolversDir >/dev/null 2>&1; then
    local resolverDir=$(ls -d $sdkResolversDir | head -1)/Microsoft.DotNet.UnifiedBuild.MSBuildSdkResolver
    if [[ ! -d "$resolverDir" ]]; then
      echo "    Note: UnifiedBuild SDK resolver not installed - some offline scenarios may not work"
      echo "          This is expected when running prep without full source-build setup"
    fi
  fi
}