#!/usr/bin/env bash
set -euo pipefail

if [[ "${1:-}" == "--inner-legacy" ]]; then
  echo "Entering isolated source-build inner helper."
  shift
  dotnet="$1"
  legacy="$2"
  version="$3"
  shift 3
  inner_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd -P)"
  outer_root="$(cd "${BUILD_SOURCESDIRECTORY:?}" && pwd -P)"
  if [[ "$inner_root" != "$outer_root/artifacts/sourcebuild-intermediate/artifacts/sb/self/src" ||
        "${BUILD_REPOSITORY_ID:-}" != "ef257391-4d76-4240-8649-3f21ac49c392" ||
        "$dotnet" != "$outer_root/.dotnet/dotnet" ]]; then
    echo "Legacy toolchain overrides are permitted only in the private source-build copy." >&2
    exit 1
  fi

  diagnostic="$outer_root/artifacts/log/Release/SourceBuildIntermediate/inner-toolchain-check.txt"
  mkdir -p "$(dirname "$diagnostic")"
  printf 'private-copy=verified\n' > "$diagnostic"
  global="$inner_root/global.json"
  details="$inner_root/eng/Version.Details.xml"
  if [[ ! -f "$global" || ! -f "$details" ]]; then
    echo "The private copy is missing its pinned toolchain metadata." >&2
    exit 1
  fi
  sdk_count="$(grep -Fc '11.0.100-preview.6.26359.118' "$global" || true)"
  global_arcade_count="$(grep -Fc '11.0.0-beta.26421.2' "$global" || true)"
  details_arcade_count="$(grep -Fc '8.0.0-beta.26168.3' "$details" || true)"
  details_sha_count="$(grep -Fc '60ea5b2eca5af06fc63b250f8669d2c70179b18c' "$details" || true)"
  printf 'sdk=%s global-arcade=%s details-arcade=%s details-sha=%s\n' \
    "$sdk_count" "$global_arcade_count" "$details_arcade_count" "$details_sha_count" >> "$diagnostic"
  if [[ "$sdk_count" -ne 2 || "$global_arcade_count" -ne 1 ||
        "$details_arcade_count" -ne 1 || "$details_sha_count" -ne 1 ]]; then
    echo "The copied source-build toolchain differs from its pinned input." >&2
    exit 1
  fi
  sed -i 's/11\.0\.100-preview\.6\.26359\.118/8.0.100/g' "$global"
  sed -i 's/11\.0\.0-beta\.26421\.2/8.0.0-beta.26168.3/g' "$global"
  printf 'toolchain=sdk8-and-arcade8\n' >> "$diagnostic"
  compat_global="$outer_root/artifacts/sourcebuild-intermediate/global.json"
  if [[ ! -f "$compat_global" ||
        $(grep -Fc '8.0.100' "$compat_global" || true) -ne 1 ||
        $(grep -Fc '8.0.0-beta.26168.3' "$compat_global" || true) -ne 1 ]]; then
    echo "The temporary Arcade 8 tool project has an unexpected SDK pin." >&2
    exit 1
  fi
  printf 'compat-sdk=8.0.100\n' >> "$diagnostic"
  if [[ ! -d "$outer_root/.dotnet/sdk/8.0.100" ]]; then
    printf 'sdk8-install=started\n' >> "$diagnostic"
    installer="$inner_root/eng/common/dotnet-install.sh"
    [[ -f "$installer" ]] || { echo "The checked-in .NET installer is missing." >&2; exit 1; }
    install_log="$(dirname "$diagnostic")/sdk8-install.log"
    if bash "$installer" -version 8.0.100 -dotnetpath "$outer_root/.dotnet" -runtime sdk > "$install_log" 2>&1; then
      printf 'sdk8-install=finished\n' >> "$diagnostic"
    else
      status=$?
      printf 'sdk8-install=failed\n' >> "$diagnostic"
      tail -n 20 "$install_log" >&2
      exit "$status"
    fi
  else
    printf 'sdk8-install=already-present\n' >> "$diagnostic"
  fi
  printf 'sdk-selected=%s\n' "$("$dotnet" --version)" >> "$diagnostic"
  previous_sdks="$outer_root/.dotnet/sdk/8.0.100/Sdks"
  historical_sdks="$outer_root/.dotnet/sdk/8.0.100/Sdks"
  if [[ ( -n "${MSBuildSDKsPath:-}" && "$MSBuildSDKsPath" != "$previous_sdks" ) ||
        ! -d "$historical_sdks" ]]; then
    echo "The inner MSBuild SDK resolver is not at the expected toolchain path." >&2
    exit 1
  fi
  export MSBuildSDKsPath="$historical_sdks"
  printf 'msbuild-sdks-env=%s\n' "$MSBuildSDKsPath" >> "$diagnostic"
  printf 'resolver-sdks-env=%s\n' "${DOTNET_MSBUILD_SDK_RESOLVER_SDKS_DIR:-unset}" >> "$diagnostic"
  editorconfig="$inner_root/.editorconfig"
  if [[ ! -f "$editorconfig" ||
        $(grep -Fc 'dotnet_diagnostic.IDE0005.severity = warning' "$editorconfig" || true) -ne 1 ||
        $(grep -Fc 'dotnet_diagnostic.IDE0032.severity = warning' "$editorconfig" || true) -ne 1 ||
        $(grep -Fc 'dotnet_diagnostic.IDE0055.severity = warning' "$editorconfig" || true) -ne 1 ||
        $(grep -Fc 'dotnet_diagnostic.IDE0090.severity = warning' "$editorconfig" || true) -ne 1 ]]; then
    echo "The private source-build copy has unexpected code-style settings." >&2
    exit 1
  fi
  cat >> "$editorconfig" <<'EOF'

[*.cs]
dotnet_diagnostic.IDE0005.severity = none
dotnet_diagnostic.IDE0032.severity = none
dotnet_diagnostic.IDE0055.severity = none
dotnet_diagnostic.IDE0090.severity = none
EOF
  printf 'inner-style-only=IDE0005,IDE0032,IDE0055,IDE0090\n' >> "$diagnostic"
  printf 'inner-build=starting\n' >> "$diagnostic"
  exec "$dotnet" msbuild "$legacy/Build.proj" -nologo -nr:false -t:Execute \
    -p:Restore=true -p:Build=true -p:Pack=true -p:Publish=false \
    -p:Configuration=Release \
    -p:ArcadeBuildFromSource=true -p:DotNetBuildFromSource=true \
    "-p:OfficialBuildId=${BUILD_BUILDNUMBER:?}" \
    "-p:Version=$version" \
    -p:MicrosoftDotNetSourceBuildTasksVersion=8.0.0-beta.26168.3 \
    -p:ArcadeSdkVersion=8.0.0-beta.26168.3 "$@"
fi

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$repo_root"

source_build_version() {
  local build_number="$1"
  local versions="$repo_root/eng/Versions.props"
  if [[ ! "$build_number" =~ ^20[0-9]{6}\.[0-9]+$ ||
        $(grep -Fc '<VersionPrefix>17.8.1</VersionPrefix>' "$versions" || true) -ne 1 ||
        $(grep -Fc '<PreReleaseVersionLabel>hotfix</PreReleaseVersionLabel>' "$versions" || true) -ne 1 ||
        $(grep -Fc '<SemanticVersioningV1>true</SemanticVersioningV1>' "$versions" || true) -ne 1 ]]; then
    echo "The servicing build number or version settings do not match the pinned Arcade version formula." >&2
    return 1
  fi
  local date_part="${build_number%%.*}"
  if [[ "$(date -u -d "$date_part" +%Y%m%d 2>/dev/null)" != "$date_part" ]]; then
    echo "The servicing build number contains an invalid date." >&2
    return 1
  fi
  local year="${build_number:2:2}" month="${build_number:4:2}" day="${build_number:6:2}"
  local run="${build_number#*.}"
  # Arcade's SemVerV1 date stamp is YY * 1000 + MM * 50 + DD.
  local date_stamp=$((10#$year * 1000 + 10#$month * 50 + 10#$day))
  printf '17.8.1-hotfix-%d-%02d\n' "$date_stamp" "$((10#$run))"
}

if [[ "${1:-}" == "--print-version" ]]; then
  [[ $# -eq 2 ]] || { echo "Expected exactly one build number." >&2; exit 1; }
  source_build_version "$2"
  exit 0
fi

if [[ "${BUILD_REASON:-}" != "PullRequest" && "${BUILD_SOURCEBRANCH:-}" != "refs/heads/internal/rel/17.8" ]]; then
  echo "Refusing source build on ${BUILD_SOURCEBRANCH:-unknown}." >&2
  exit 1
fi

if [[ "${SYSTEM_TEAMPROJECTID:-}" != "7ea9116e-9fac-403d-b258-b31fcf1bb293" ||
      "${BUILD_REPOSITORY_ID:-}" != "ef257391-4d76-4240-8649-3f21ac49c392" ]]; then
  echo "Refusing source build outside the private VSTest producer." >&2
  exit 1
fi

if [[ ! "${BUILD_SOURCEVERSION:-}" =~ ^[0-9a-f]{40}$ ]]; then
  echo "Missing exact official build number or source commit." >&2
  exit 1
fi

version="$(source_build_version "${BUILD_BUILDNUMBER:-}")"

dotnet="$repo_root/.dotnet/dotnet"
compat="$repo_root/artifacts/sourcebuild-intermediate"
legacy_cache="$compat/tool-packages"
mkdir -p "$compat"
if [[ ! -d "$repo_root/.dotnet/sdk/8.0.100" ]]; then
  installer="$repo_root/eng/common/dotnet-install.sh"
  [[ -f "$installer" ]] || { echo "The checked-in .NET installer is missing." >&2; exit 1; }
  bash "$installer" -version 8.0.100 -dotnetpath "$repo_root/.dotnet" -runtime sdk
fi
cat > "$compat/global.json" <<EOF
{"sdk":{"version":"8.0.100","rollForward":"disable","allowPrerelease":false},"msbuild-sdks":{"Microsoft.DotNet.Arcade.Sdk":"8.0.0-beta.26168.3"}}
EOF
cd "$compat"
if [[ "$("$dotnet" --version)" != "8.0.100" ]]; then
  echo "The source build did not select its pinned SDK 8." >&2
  exit 1
fi

NUGET_PACKAGES="$legacy_cache" "$dotnet" restore "$repo_root/eng/SourceBuildIntermediateTools.csproj" \
  -p:ImportDirectoryBuildProps=false -p:ImportDirectoryBuildTargets=false -p:ImportDirectoryPackagesProps=false \
  --packages "$legacy_cache" --source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-eng/nuget/v3/index.json \
  --source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-public/nuget/v3/index.json

legacy="$legacy_cache/microsoft.dotnet.arcade.sdk/8.0.0-beta.26168.3/tools"
for file in "$legacy/SourceBuild/AfterSourceBuild.proj" \
            "$legacy/Publish.proj" \
            "$legacy_cache/microsoft.dotnet.sourcebuild.tasks/8.0.0-beta.26168.3/build/Microsoft.DotNet.SourceBuild.Tasks.props" \
            "$legacy_cache/microsoft.dotnet.build.tasks.feed/8.0.0-beta.26168.3/build/Microsoft.DotNet.Build.Tasks.Feed.targets"; do
  [[ -f "$file" ]] || { echo "Legacy intermediate tool missing: $file" >&2; exit 1; }
done

shared=(
  "-p:RepoRoot=$repo_root/"
  "-p:ArcadeSdkVersion=8.0.0-beta.26168.3"
  "-p:NuGetPackageRoot=$legacy_cache/"
  "-p:Configuration=Release"
  "-p:ArcadeBuildFromSource=true"
  "-p:DotNetBuildFromSource=true"
  "-p:SourceBuildManagedOnly=true"
  "-p:GitHubRepositoryName=vstest"
  "-p:OfficialBuildId=$BUILD_BUILDNUMBER"
  "-p:Version=$version"
  "-p:RepositoryCommit=$BUILD_SOURCEVERSION"
  "-p:RepositoryUrl=https://dev.azure.com/dnceng/internal/_git/microsoft-vstest"
  "-p:RepositoryType=git"
  "-p:PackageProjectUrl=https://github.com/microsoft/vstest"
  "-p:PackageDescription=Source-built VSTest packages for downstream SDK builds"
  "-p:ArtifactsDir=$compat/artifacts/"
  "-p:ArtifactsObjDir=$compat/artifacts/obj/"
  "-p:SourceBuildOutputDir=$compat/artifacts/sb/"
)
inner_command="bash ./eng/BuildSourceBuildIntermediate.sh --inner-legacy $dotnet $legacy $version"
if ! NUGET_PACKAGES="$legacy_cache" "$dotnet" msbuild "$legacy/Build.proj" -nologo -nr:false -t:Execute \
  -p:Restore=true -p:Build=true -p:Pack=true -p:Publish=false \
  "-p:BaseInnerSourceBuildCommand=$inner_command" \
  "-p:MicrosoftDotNetSourceBuildTasksVersion=8.0.0-beta.26168.3" "${shared[@]}"; then
  report_dir="$compat/artifacts/sb/self/prebuilt-report"
  report_logs="$repo_root/artifacts/log/Release/SourceBuildIntermediate"
  mkdir -p "$report_logs"
  for report in annotated-usage.xml prebuilt-usage.xml baseline-comparison.xml generated-new-baseline.xml; do
    if [[ -s "$report_dir/$report" ]]; then
      cp "$report_dir/$report" "$report_logs/$report"
    fi
  done
  if [[ -s "$report_logs/inner-toolchain-check.txt" ]]; then
    cat "$report_logs/inner-toolchain-check.txt"
  else
    echo "The isolated inner helper did not reach its private-copy check."
  fi
  upstream="$compat/artifacts/sb/self/src/artifacts/obj/source-built-upstream-cache"
  if [[ -d "$upstream" ]]; then
    printf 'Source-built upstream cache has %s packages.\n' \
      "$(find "$upstream" -maxdepth 1 -type f -name '*.nupkg' | wc -l)"
    for name in Microsoft.Build.Framework Microsoft.NET.StringTools Microsoft.DotNet.Arcade.Sdk Newtonsoft.Json; do
      found=("$upstream/$name".*.nupkg)
      for package in "${found[@]}"; do
        if [[ -f "$package" ]]; then
          printf 'Source-built upstream: %s\n' "${package##*/}"
        fi
      done
    done
  else
    echo "The source-built upstream package cache is absent."
  fi
  exit 1
fi

shopt -s nullglob
inner_shipping="$compat/artifacts/sb/self/src/artifacts/packages/Release/Shipping"
inner_build=("$inner_shipping"/Microsoft.TestPlatform.Build.*.nupkg)
inner_cli=("$inner_shipping"/Microsoft.TestPlatform.CLI.*.nupkg)
inner_products=()
for package in "${inner_build[@]}" "${inner_cli[@]}"; do
  [[ "$package" == *.symbols.nupkg ]] || inner_products+=("$package")
done
if [[ ${#inner_products[@]} -ne 2 ||
      "${inner_products[0]##*/}" != "Microsoft.TestPlatform.Build.$version.nupkg" ||
      "${inner_products[1]##*/}" != "Microsoft.TestPlatform.CLI.$version.nupkg" ]]; then
  echo "The isolated source-built product packages do not match the signed build's version." >&2
  exit 1
fi

intermediate="$compat/artifacts/packages/Release/NonShipping/Microsoft.SourceBuild.Intermediate.vstest.$version.nupkg"
[[ -s "$intermediate" ]] || { echo "The source-built intermediate package is missing or empty." >&2; exit 1; }
for report in annotated-usage.xml prebuilt-usage.xml baseline-comparison.xml generated-new-baseline.xml; do
  [[ -s "$compat/artifacts/sb/self/prebuilt-report/$report" ]] || { echo "Missing real prebuilt report: $report" >&2; exit 1; }
done

if [[ "${BUILD_REASON:-}" == "PullRequest" ]]; then
  echo "Validated the intermediate package locally; PR builds never publish it."
  exit 0
fi

NUGET_PACKAGES="$legacy_cache" "$dotnet" msbuild "$legacy/Publish.proj" -nologo -nr:false -t:Publish \
  "-p:MicrosoftDotNetBuildTasksFeedVersion=8.0.0-beta.26168.3" \
  "-p:DotNetPublishUsingPipelines=true" "-p:PublishToSymbolServer=false" \
  "-p:AutoGenerateSymbolPackages=false" "-p:AllowEmptySignPostBuildList=true" \
  "-p:AssetManifestFilePath=$compat/artifacts/log/Release/AssetManifest/SourceBuild_Managed.xml" \
  "${shared[@]}"
