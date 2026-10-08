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
  if [[ "$inner_root" != "$outer_root/artifacts/sourcebuild-intermediate/artifacts/sb/src" ||
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
  details_arcade_count="$(grep -Fc '9.0.0-beta.24408.2' "$details" || true)"
  details_sha_count="$(grep -Fc '60ae233c3d77f11c5fdb53e570b64d503b13ba59' "$details" || true)"
  printf 'sdk=%s global-arcade=%s details-arcade=%s details-sha=%s\n' \
    "$sdk_count" "$global_arcade_count" "$details_arcade_count" "$details_sha_count" >> "$diagnostic"
  if [[ "$sdk_count" -ne 2 || "$global_arcade_count" -ne 1 ||
        "$details_arcade_count" -ne 3 || "$details_sha_count" -ne 3 ]]; then
    echo "The copied source-build toolchain differs from its pinned input." >&2
    exit 1
  fi
  sed -i 's/11\.0\.100-preview\.6\.26359\.118/9.0.104/g' "$global"
  sed -i 's/11\.0\.0-beta\.26421\.2/9.0.0-beta.25164.2/g' "$global"
  sed -i 's/9\.0\.0-beta\.24408\.2/9.0.0-beta.25164.2/g' "$details"
  sed -i 's/60ae233c3d77f11c5fdb53e570b64d503b13ba59/5ba9ca776c1d0bb72b2791591e54cf51fc52dfee/g' "$details"
  printf 'toolchain=historical-pins-applied\n' >> "$diagnostic"
  compat_global="$outer_root/artifacts/sourcebuild-intermediate/global.json"
  if [[ ! -f "$compat_global" ||
        $(grep -Fc '11.0.100-preview.6.26359.118' "$compat_global" || true) -ne 1 ||
        $(grep -Fc '9.0.0-beta.25164.2' "$compat_global" || true) -ne 1 ]]; then
    echo "The temporary Arcade 9 tool project has an unexpected SDK pin." >&2
    exit 1
  fi
  sed -i 's/11\.0\.100-preview\.6\.26359\.118/9.0.104/g' "$compat_global"
  printf 'compat-sdk=9.0.104\n' >> "$diagnostic"
  if [[ ! -d "$outer_root/.dotnet/sdk/9.0.104" ]]; then
    printf 'sdk9-install=started\n' >> "$diagnostic"
    installer="$inner_root/eng/common/dotnet-install.sh"
    [[ -f "$installer" ]] || { echo "The checked-in .NET installer is missing." >&2; exit 1; }
    install_log="$(dirname "$diagnostic")/sdk9-install.log"
    if bash "$installer" -version 9.0.104 -dotnetpath "$outer_root/.dotnet" -runtime sdk > "$install_log" 2>&1; then
      printf 'sdk9-install=finished\n' >> "$diagnostic"
    else
      status=$?
      printf 'sdk9-install=failed\n' >> "$diagnostic"
      tail -n 20 "$install_log" >&2
      exit "$status"
    fi
  else
    printf 'sdk9-install=already-present\n' >> "$diagnostic"
  fi
  printf 'sdk-selected=%s\n' "$("$dotnet" --version)" >> "$diagnostic"
  previous_sdks="$outer_root/.dotnet/sdk/11.0.100-preview.6.26359.118/Sdks"
  historical_sdks="$outer_root/.dotnet/sdk/9.0.104/Sdks"
  if [[ ( -n "${MSBuildSDKsPath:-}" && "$MSBuildSDKsPath" != "$previous_sdks" ) ||
        ! -d "$historical_sdks" ]]; then
    echo "The inner MSBuild SDK resolver is not at the expected toolchain path." >&2
    exit 1
  fi
  export MSBuildSDKsPath="$historical_sdks"
  printf 'msbuild-sdks-env=%s\n' "$MSBuildSDKsPath" >> "$diagnostic"
  printf 'resolver-sdks-env=%s\n' "${DOTNET_MSBUILD_SDK_RESOLVER_SDKS_DIR:-unset}" >> "$diagnostic"
  printf 'inner-build=starting\n' >> "$diagnostic"
  exec "$dotnet" msbuild "$legacy/Build.proj" -nologo -nr:false -t:Execute \
    -p:Restore=true -p:Build=true -p:Pack=true -p:Publish=false \
    -p:Configuration=Release \
    -p:ArcadeBuildFromSource=true -p:DotNetBuildRepo=true \
    "-p:OfficialBuildId=${BUILD_BUILDNUMBER:?}" \
    "-p:Version=$version" \
    -p:MicrosoftDotNetSourceBuildTasksVersion=9.0.0-beta.25164.2 \
    -p:ArcadeSdkVersion=9.0.0-beta.25164.2 "$@"
fi

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$repo_root"
./build.sh "$@"

if [[ "${BUILD_REASON:-}" != "PullRequest" && "${BUILD_SOURCEBRANCH:-}" != "refs/heads/internal/rel/17.12" ]]; then
  echo "No intermediate publication on ${BUILD_SOURCEBRANCH:-unknown}."
  exit 0
fi

if [[ "${BUILD_REASON:-}" != "PullRequest" &&
      ( "${SYSTEM_TEAMPROJECTID:-}" != "7ea9116e-9fac-403d-b258-b31fcf1bb293" ||
        "${BUILD_REPOSITORY_ID:-}" != "ef257391-4d76-4240-8649-3f21ac49c392" ) ]]; then
  echo "Refusing to publish outside the private VSTest producer." >&2
  exit 1
fi

if [[ ! "${BUILD_BUILDNUMBER:-}" =~ ^[0-9]{8}\.[0-9]+$ || ! "${BUILD_SOURCEVERSION:-}" =~ ^[0-9a-f]{40}$ ]]; then
  echo "Missing exact official build number or source commit." >&2
  exit 1
fi

shipping="$repo_root/artifacts/packages/Release/Shipping"
shopt -s nullglob
build=("$shipping"/Microsoft.TestPlatform.Build.*.nupkg)
cli=("$shipping"/Microsoft.TestPlatform.CLI.*.nupkg)
source_built=()
for package in "${build[@]}" "${cli[@]}"; do
  [[ "$package" == *.symbols.nupkg ]] || source_built+=("$package")
done
if [[ ${#source_built[@]} -ne 2 ]]; then
  echo "Expected exactly two source-built, non-symbol VSTest packages." >&2
  exit 1
fi
version="${source_built[0]##*/Microsoft.TestPlatform.Build.}"
version="${version%.nupkg}"
if [[ ! "$version" =~ ^17\.12\.1-hotfix-[0-9]+-[0-9]+$ ||
      "${source_built[1]##*/}" != "Microsoft.TestPlatform.CLI.$version.nupkg" ]]; then
  echo "Source-built package versions do not match the servicing build." >&2
  exit 1
fi

dotnet="$repo_root/.dotnet/dotnet"
cache="${NUGET_PACKAGES:-$repo_root/.packages}"
compat="$repo_root/artifacts/sourcebuild-intermediate"
legacy_cache="$compat/tool-packages"
mkdir -p "$compat"
sdk_version="$("$dotnet" --version)"
cat > "$compat/global.json" <<EOF
{"sdk":{"version":"$sdk_version","rollForward":"disable","allowPrerelease":true},"msbuild-sdks":{"Microsoft.DotNet.Arcade.Sdk":"9.0.0-beta.25164.2"}}
EOF

NUGET_PACKAGES="$legacy_cache" "$dotnet" restore "$repo_root/eng/SourceBuildIntermediateTools.csproj" \
  -p:ImportDirectoryBuildProps=false -p:ImportDirectoryBuildTargets=false -p:ImportDirectoryPackagesProps=false \
  --packages "$legacy_cache" --source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-eng/nuget/v3/index.json \
  --source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-public/nuget/v3/index.json

legacy="$legacy_cache/microsoft.dotnet.arcade.sdk/9.0.0-beta.25164.2/tools"
for file in "$legacy/SourceBuild/AfterSourceBuild.proj" \
            "$legacy/Publish.proj" \
            "$legacy_cache/microsoft.dotnet.sourcebuild.tasks/9.0.0-beta.25164.2/build/Microsoft.DotNet.SourceBuild.Tasks.props" \
            "$legacy_cache/microsoft.dotnet.build.tasks.feed/9.0.0-beta.25164.2/build/Microsoft.DotNet.Build.Tasks.Feed.targets"; do
  [[ -f "$file" ]] || { echo "Legacy intermediate tool missing: $file" >&2; exit 1; }
done

cd "$compat"
shared=(
  "-p:RepoRoot=$repo_root/"
  "-p:ArcadeSdkVersion=9.0.0-beta.25164.2"
  "-p:NuGetPackageRoot=$legacy_cache/"
  "-p:Configuration=Release"
  "-p:UseInnerClone=true"
  "-p:ArcadeBuildFromSource=true"
  "-p:DotNetBuildSourceOnly=true"
  "-p:DotNetBuildRepo=true"
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
  "-p:MicrosoftDotNetSourceBuildTasksVersion=9.0.0-beta.25164.2" "${shared[@]}"; then
  report_dir="$compat/artifacts/sb/prebuilt-report"
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
  upstream="$compat/artifacts/sb/src/artifacts/obj/source-built-upstream-cache"
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

inner_shipping="$compat/artifacts/sb/src/artifacts/packages/Release/Shipping"
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
  [[ -s "$compat/artifacts/sb/prebuilt-report/$report" ]] || { echo "Missing real prebuilt report: $report" >&2; exit 1; }
done

if [[ "${BUILD_REASON:-}" == "PullRequest" ]]; then
  echo "Validated the intermediate package locally; PR builds never publish it."
  exit 0
fi

NUGET_PACKAGES="$legacy_cache" "$dotnet" msbuild "$legacy/Publish.proj" -nologo -nr:false -t:Publish \
  "-p:MicrosoftDotNetBuildTasksFeedVersion=9.0.0-beta.25164.2" \
  "-p:DotNetPublishUsingPipelines=true" "-p:PublishToSymbolServer=false" \
  "-p:AutoGenerateSymbolPackages=false" "-p:AllowEmptySignPostBuildList=true" \
  "-p:AssetManifestFilePath=$compat/artifacts/log/Release/AssetManifest/SourceBuild_Managed.xml" \
  "${shared[@]}"
