#!/usr/bin/env bash
set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd -P)"
original_global_hash="$(sha256sum "$repo_root/global.json" | cut -d' ' -f1)"
original_details_hash="$(sha256sum "$repo_root/eng/Version.Details.xml" | cut -d' ' -f1)"
original_editorconfig_hash="$(sha256sum "$repo_root/.editorconfig" | cut -d' ' -f1)"
[[ "$(bash "$repo_root/eng/BuildSourceBuildIntermediate.sh" --print-version 20260925.14)" == \
   "17.8.1-hotfix-26475-14" ]]
[[ "$(bash "$repo_root/eng/BuildSourceBuildIntermediate.sh" --print-version 20260930.26)" == \
   "17.8.1-hotfix-26480-26" ]]
[[ "$(bash "$repo_root/eng/BuildSourceBuildIntermediate.sh" --print-version 20260930.1)" == \
   "17.8.1-hotfix-26480-01" ]]
if bash "$repo_root/eng/BuildSourceBuildIntermediate.sh" --print-version 20261340.1 >/dev/null 2>&1; then
  echo "The source build accepted an invalid official build date." >&2
  exit 1
fi
if bash "$repo_root/eng/BuildSourceBuildIntermediate.sh" --print-version 20260229.1 >/dev/null 2>&1; then
  echo "The source build accepted an invalid leap day." >&2
  exit 1
fi
mkdir -p "$repo_root/artifacts/tmp"
fixture="$(mktemp -d "$repo_root/artifacts/tmp/legacy-inner-XXXXXX")"
if [[ "$fixture" != "$repo_root/artifacts/tmp/legacy-inner-"* ]]; then
  echo "Unexpected source-build test directory." >&2
  exit 1
fi
trap 'rm -rf -- "$fixture"' EXIT

inner="$fixture/artifacts/sourcebuild-intermediate/artifacts/sb/self/src"
mkdir -p "$inner/eng/common" "$fixture/.dotnet/sdk/11.0.100-preview.6.26359.118/Sdks"
printf '%s\n' '{"sdk":{"version":"8.0.100","rollForward":"disable","allowPrerelease":false},"msbuild-sdks":{"Microsoft.DotNet.Arcade.Sdk":"8.0.0-beta.26168.3"}}' \
  > "$fixture/compat-global-before.json"
cp "$fixture/compat-global-before.json" "$fixture/artifacts/sourcebuild-intermediate/global.json"
cp "$repo_root/global.json" "$inner/global.json"
cp "$repo_root/eng/Version.Details.xml" "$inner/eng/Version.Details.xml"
cp "$repo_root/.editorconfig" "$inner/.editorconfig"
cp "$repo_root/eng/BuildSourceBuildIntermediate.sh" "$inner/eng/BuildSourceBuildIntermediate.sh"
cat > "$fixture/.dotnet/dotnet" <<'EOF'
#!/usr/bin/env bash
if [[ "${1:-}" == "--version" ]]; then
  echo 8.0.100
  exit 0
fi
printf '%s\n' "$@" > "$PILOT_ARGS"
printf '%s\n' "$MSBuildSDKsPath" > "$PILOT_MSBUILD_SDKS"
EOF
chmod +x "$fixture/.dotnet/dotnet"
cat > "$inner/eng/common/dotnet-install.sh" <<'EOF'
#!/usr/bin/env bash
printf '%s\n' "$@" > "$PILOT_INSTALL_ARGS"
mkdir -p "$BUILD_SOURCESDIRECTORY/.dotnet/sdk/8.0.100/Sdks"
EOF

BUILD_SOURCESDIRECTORY="$fixture" \
BUILD_REPOSITORY_ID=ef257391-4d76-4240-8649-3f21ac49c392 \
BUILD_BUILDNUMBER=20260930.99 \
PILOT_ARGS="$fixture/args.txt" \
PILOT_INSTALL_ARGS="$fixture/install-args.txt" \
PILOT_MSBUILD_SDKS="$fixture/sdk-path.txt" \
MSBuildSDKsPath="$fixture/.dotnet/sdk/8.0.100/Sdks" \
  bash "$inner/eng/BuildSourceBuildIntermediate.sh" --inner-legacy \
    "$fixture/.dotnet/dotnet" \
    "$fixture/tool-packages/microsoft.dotnet.arcade.sdk/8.0.0-beta.26168.3/tools" \
    17.8.1-hotfix-26480-99

[[ $(grep -Fc '8.0.100' "$inner/global.json") -eq 2 ]]
[[ $(grep -Fc '8.0.0-beta.26168.3' "$inner/global.json") -eq 1 ]]
[[ $(grep -Fc '8.0.100' "$fixture/artifacts/sourcebuild-intermediate/global.json") -eq 1 ]]
cmp "$fixture/compat-global-before.json" "$fixture/artifacts/sourcebuild-intermediate/global.json"
cmp <(sed -e 's/11\.0\.100-preview\.6\.26359\.118/8.0.100/g' \
          -e 's/11\.0\.0-beta\.26421\.2/8.0.0-beta.26168.3/g' "$repo_root/global.json") \
  "$inner/global.json"
cmp "$repo_root/eng/Version.Details.xml" "$inner/eng/Version.Details.xml"
head -c "$(wc -c < "$repo_root/.editorconfig")" "$inner/.editorconfig" | cmp - "$repo_root/.editorconfig"
for diagnostic in IDE0005 IDE0032 IDE0055 IDE0090; do
  [[ $(grep -Fxc "dotnet_diagnostic.$diagnostic.severity = none" "$inner/.editorconfig") -eq 1 ]]
done
grep -Fxq -- '-p:Publish=false' "$fixture/args.txt"
grep -Fxq -- '-p:ArcadeBuildFromSource=true' "$fixture/args.txt"
grep -Fxq -- '-p:DotNetBuildFromSource=true' "$fixture/args.txt"
grep -Fxq -- '-p:Version=17.8.1-hotfix-26480-99' "$fixture/args.txt"
grep -Fxq -- '-p:Configuration=Release' "$fixture/args.txt"
grep -Fxq -- '-p:ArcadeSdkVersion=8.0.0-beta.26168.3' "$fixture/args.txt"
grep -Fxq -- 'private-copy=verified' "$fixture/artifacts/log/Release/SourceBuildIntermediate/inner-toolchain-check.txt"
grep -Fxq -- 'sdk8-install=started' "$fixture/artifacts/log/Release/SourceBuildIntermediate/inner-toolchain-check.txt"
grep -Fxq -- 'sdk8-install=finished' "$fixture/artifacts/log/Release/SourceBuildIntermediate/inner-toolchain-check.txt"
grep -Fxq -- 'compat-sdk=8.0.100' "$fixture/artifacts/log/Release/SourceBuildIntermediate/inner-toolchain-check.txt"
grep -Fxq -- 'sdk-selected=8.0.100' "$fixture/artifacts/log/Release/SourceBuildIntermediate/inner-toolchain-check.txt"
grep -Fxq -- "$fixture/.dotnet/sdk/8.0.100/Sdks" "$fixture/sdk-path.txt"
grep -Fxq -- 'inner-build=starting' "$fixture/artifacts/log/Release/SourceBuildIntermediate/inner-toolchain-check.txt"
grep -Fxq -- 'inner-style-only=IDE0005,IDE0032,IDE0055,IDE0090' "$fixture/artifacts/log/Release/SourceBuildIntermediate/inner-toolchain-check.txt"
grep -Fxq -- '-version' "$fixture/install-args.txt"
grep -Fxq -- '8.0.100' "$fixture/install-args.txt"
grep -Fxq -- '-dotnetpath' "$fixture/install-args.txt"
grep -Fxq -- "$fixture/.dotnet" "$fixture/install-args.txt"
grep -Fxq -- '-runtime' "$fixture/install-args.txt"
grep -Fxq -- 'sdk' "$fixture/install-args.txt"
if grep -Fq -- '-p:NuGetPackageRoot=' "$fixture/args.txt"; then
  echo "The inner source build must resolve from its own package cache." >&2
  exit 1
fi
[[ "$(sha256sum "$repo_root/global.json" | cut -d' ' -f1)" == "$original_global_hash" ]]
[[ "$(sha256sum "$repo_root/eng/Version.Details.xml" | cut -d' ' -f1)" == "$original_details_hash" ]]
[[ "$(sha256sum "$repo_root/.editorconfig" | cut -d' ' -f1)" == "$original_editorconfig_hash" ]]

mkdir -p "$fixture/eng"
cp "$repo_root/eng/Versions.props" "$fixture/eng/Versions.props"
cp "$repo_root/eng/BuildSourceBuildIntermediate.sh" "$fixture/eng/BuildSourceBuildIntermediate.sh"
legacy="$fixture/artifacts/sourcebuild-intermediate/tool-packages"
mkdir -p "$legacy/microsoft.dotnet.arcade.sdk/8.0.0-beta.26168.3/tools/SourceBuild" \
         "$legacy/microsoft.dotnet.sourcebuild.tasks/8.0.0-beta.26168.3/build" \
         "$legacy/microsoft.dotnet.build.tasks.feed/8.0.0-beta.26168.3/build"
touch "$legacy/microsoft.dotnet.arcade.sdk/8.0.0-beta.26168.3/tools/SourceBuild/AfterSourceBuild.proj" \
      "$legacy/microsoft.dotnet.arcade.sdk/8.0.0-beta.26168.3/tools/Publish.proj" \
      "$legacy/microsoft.dotnet.sourcebuild.tasks/8.0.0-beta.26168.3/build/Microsoft.DotNet.SourceBuild.Tasks.props" \
      "$legacy/microsoft.dotnet.build.tasks.feed/8.0.0-beta.26168.3/build/Microsoft.DotNet.Build.Tasks.Feed.targets"
if BUILD_REASON=PullRequest \
    BUILD_REPOSITORY_ID=ef257391-4d76-4240-8649-3f21ac49c392 \
    SYSTEM_TEAMPROJECTID=7ea9116e-9fac-403d-b258-b31fcf1bb293 \
    BUILD_BUILDNUMBER=20260930.99 \
    BUILD_SOURCEVERSION=0123456789abcdef0123456789abcdef01234567 \
    PILOT_ARGS="$fixture/outer-args.txt" PILOT_MSBUILD_SDKS="$fixture/outer-sdk-path.txt" \
    bash "$fixture/eng/BuildSourceBuildIntermediate.sh" --ci --configuration Release \
      > "$fixture/outer-build.log" 2>&1; then
  echo "A source build without real product packages must fail." >&2
  exit 1
fi
grep -Fq 'The isolated source-built product packages do not match' "$fixture/outer-build.log"
grep -Fxq -- '-p:DotNetBuildFromSource=true' "$fixture/outer-args.txt"
grep -Fxq -- '-p:Publish=false' "$fixture/outer-args.txt"
grep -Fxq -- '-p:Version=17.8.1-hotfix-26480-99' "$fixture/outer-args.txt"

rmdir "$fixture/.dotnet/sdk/8.0.100/Sdks" "$fixture/.dotnet/sdk/8.0.100"
cp "$repo_root/global.json" "$inner/global.json"
cp "$repo_root/eng/Version.Details.xml" "$inner/eng/Version.Details.xml"
cp "$repo_root/.editorconfig" "$inner/.editorconfig"
cp "$fixture/compat-global-before.json" "$fixture/artifacts/sourcebuild-intermediate/global.json"
cat > "$inner/eng/common/dotnet-install.sh" <<'EOF'
#!/usr/bin/env bash
echo "Simulated SDK installation failure" >&2
exit 23
EOF
if BUILD_SOURCESDIRECTORY="$fixture" \
    BUILD_REPOSITORY_ID=ef257391-4d76-4240-8649-3f21ac49c392 \
    BUILD_BUILDNUMBER=20260930.99 \
    bash "$inner/eng/BuildSourceBuildIntermediate.sh" --inner-legacy \
      "$fixture/.dotnet/dotnet" \
      "$fixture/tool-packages/microsoft.dotnet.arcade.sdk/8.0.0-beta.26168.3/tools" \
      17.8.1-hotfix-26480-99 > "$fixture/install-failure.log" 2>&1; then
  echo "The source build ignored the SDK installation failure." >&2
  exit 1
else
  [[ "$?" -eq 23 ]]
fi
grep -Fq 'Simulated SDK installation failure' "$fixture/install-failure.log"
grep -Fxq 'sdk8-install=failed' "$fixture/artifacts/log/Release/SourceBuildIntermediate/inner-toolchain-check.txt"
cmp "$repo_root/.editorconfig" "$inner/.editorconfig"

cp "$repo_root/global.json" "$inner/global.json"
cp "$repo_root/eng/Version.Details.xml" "$inner/eng/Version.Details.xml"
cp "$repo_root/.editorconfig" "$inner/.editorconfig"
if BUILD_SOURCESDIRECTORY="$fixture" BUILD_REPOSITORY_ID=wrong-repository \
    BUILD_BUILDNUMBER=20260930.99 \
    bash "$inner/eng/BuildSourceBuildIntermediate.sh" --inner-legacy \
      "$fixture/.dotnet/dotnet" \
      "$fixture/tool-packages/microsoft.dotnet.arcade.sdk/8.0.0-beta.26168.3/tools" \
      17.8.1-hotfix-26480-99 >/dev/null 2>&1; then
  echo "The inner-only guard accepted a different repository." >&2
  exit 1
fi
cmp "$repo_root/global.json" "$inner/global.json"
cmp "$repo_root/eng/Version.Details.xml" "$inner/eng/Version.Details.xml"
cmp "$repo_root/.editorconfig" "$inner/.editorconfig"
printf '%s\n' "Legacy source-build helper kept toolchain edits in the private inner copy."
