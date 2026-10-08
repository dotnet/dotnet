#!/usr/bin/env bash
set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd -P)"
original_global_hash="$(sha256sum "$repo_root/global.json" | cut -d' ' -f1)"
original_details_hash="$(sha256sum "$repo_root/eng/Version.Details.xml" | cut -d' ' -f1)"
mkdir -p "$repo_root/artifacts/tmp"
fixture="$(mktemp -d "$repo_root/artifacts/tmp/legacy-inner-XXXXXX")"
if [[ "$fixture" != "$repo_root/artifacts/tmp/legacy-inner-"* ]]; then
  echo "Unexpected source-build test directory." >&2
  exit 1
fi
trap 'rm -rf -- "$fixture"' EXIT

inner="$fixture/artifacts/sourcebuild-intermediate/artifacts/sb/src"
mkdir -p "$inner/eng/common" "$fixture/.dotnet/sdk/11.0.100-preview.6.26359.118/Sdks"
printf '%s\n' '{"sdk":{"version":"11.0.100-preview.6.26359.118"},"msbuild-sdks":{"Microsoft.DotNet.Arcade.Sdk":"9.0.0-beta.25164.2"}}' \
  > "$fixture/compat-global-before.json"
cp "$fixture/compat-global-before.json" "$fixture/artifacts/sourcebuild-intermediate/global.json"
cp "$repo_root/global.json" "$inner/global.json"
cp "$repo_root/eng/Version.Details.xml" "$inner/eng/Version.Details.xml"
cp "$repo_root/eng/BuildSourceBuildIntermediate.sh" "$inner/eng/BuildSourceBuildIntermediate.sh"
cat > "$fixture/.dotnet/dotnet" <<'EOF'
#!/usr/bin/env bash
if [[ "${1:-}" == "--version" ]]; then
  echo 9.0.104
  exit 0
fi
printf '%s\n' "$@" > "$PILOT_ARGS"
printf '%s\n' "$MSBuildSDKsPath" > "$PILOT_MSBUILD_SDKS"
EOF
chmod +x "$fixture/.dotnet/dotnet"
cat > "$inner/eng/common/dotnet-install.sh" <<'EOF'
#!/usr/bin/env bash
printf '%s\n' "$@" > "$PILOT_INSTALL_ARGS"
mkdir -p "$BUILD_SOURCESDIRECTORY/.dotnet/sdk/9.0.104/Sdks"
EOF

BUILD_SOURCESDIRECTORY="$fixture" \
BUILD_REPOSITORY_ID=ef257391-4d76-4240-8649-3f21ac49c392 \
BUILD_BUILDNUMBER=20260930.99 \
PILOT_ARGS="$fixture/args.txt" \
PILOT_INSTALL_ARGS="$fixture/install-args.txt" \
PILOT_MSBUILD_SDKS="$fixture/sdk-path.txt" \
MSBuildSDKsPath="$fixture/.dotnet/sdk/11.0.100-preview.6.26359.118/Sdks" \
  bash "$inner/eng/BuildSourceBuildIntermediate.sh" --inner-legacy \
    "$fixture/.dotnet/dotnet" \
    "$fixture/tool-packages/microsoft.dotnet.arcade.sdk/9.0.0-beta.25164.2/tools" \
    17.12.1-hotfix-26480-99

[[ $(grep -Fc '9.0.104' "$inner/global.json") -eq 2 ]]
[[ $(grep -Fc '9.0.0-beta.25164.2' "$inner/global.json") -eq 1 ]]
[[ $(grep -Fc '9.0.104' "$fixture/artifacts/sourcebuild-intermediate/global.json") -eq 1 ]]
cmp <(sed -e 's/11\.0\.100-preview\.6\.26359\.118/9.0.104/g' \
          -e 's/11\.0\.0-beta\.26421\.2/9.0.0-beta.25164.2/g' "$repo_root/global.json") \
  "$inner/global.json"
cmp <(sed -e 's/9\.0\.0-beta\.24408\.2/9.0.0-beta.25164.2/g' \
          -e 's/60ae233c3d77f11c5fdb53e570b64d503b13ba59/5ba9ca776c1d0bb72b2791591e54cf51fc52dfee/g' \
          "$repo_root/eng/Version.Details.xml") "$inner/eng/Version.Details.xml"
grep -Fq 'Name="Microsoft.SourceBuild.Intermediate.source-build-reference-packages" Version="9.0.0-alpha.1.24352.1"' \
  "$inner/eng/Version.Details.xml"
grep -Fq '<Sha>cc732c57199f725857c201da146525e3be6bc504</Sha>' \
  "$inner/eng/Version.Details.xml"
[[ $(grep -Fc '9.0.0-beta.25164.2' "$inner/eng/Version.Details.xml") -eq 3 ]]
[[ $(grep -Fc '5ba9ca776c1d0bb72b2791591e54cf51fc52dfee' "$inner/eng/Version.Details.xml") -eq 3 ]]
grep -Fxq -- '-p:Publish=false' "$fixture/args.txt"
grep -Fxq -- '-p:ArcadeBuildFromSource=true' "$fixture/args.txt"
grep -Fxq -- '-p:DotNetBuildRepo=true' "$fixture/args.txt"
grep -Fxq -- '-p:Version=17.12.1-hotfix-26480-99' "$fixture/args.txt"
grep -Fxq -- '-p:Configuration=Release' "$fixture/args.txt"
grep -Fxq -- '-p:ArcadeSdkVersion=9.0.0-beta.25164.2' "$fixture/args.txt"
grep -Fxq -- 'private-copy=verified' "$fixture/artifacts/log/Release/SourceBuildIntermediate/inner-toolchain-check.txt"
grep -Fxq -- 'sdk9-install=started' "$fixture/artifacts/log/Release/SourceBuildIntermediate/inner-toolchain-check.txt"
grep -Fxq -- 'sdk9-install=finished' "$fixture/artifacts/log/Release/SourceBuildIntermediate/inner-toolchain-check.txt"
grep -Fxq -- 'compat-sdk=9.0.104' "$fixture/artifacts/log/Release/SourceBuildIntermediate/inner-toolchain-check.txt"
grep -Fxq -- 'sdk-selected=9.0.104' "$fixture/artifacts/log/Release/SourceBuildIntermediate/inner-toolchain-check.txt"
grep -Fxq -- "$fixture/.dotnet/sdk/9.0.104/Sdks" "$fixture/sdk-path.txt"
grep -Fxq -- 'inner-build=starting' "$fixture/artifacts/log/Release/SourceBuildIntermediate/inner-toolchain-check.txt"
grep -Fxq -- '-version' "$fixture/install-args.txt"
grep -Fxq -- '9.0.104' "$fixture/install-args.txt"
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

rmdir "$fixture/.dotnet/sdk/9.0.104/Sdks" "$fixture/.dotnet/sdk/9.0.104"
cp "$repo_root/global.json" "$inner/global.json"
cp "$repo_root/eng/Version.Details.xml" "$inner/eng/Version.Details.xml"
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
      "$fixture/tool-packages/microsoft.dotnet.arcade.sdk/9.0.0-beta.25164.2/tools" \
      17.12.1-hotfix-26480-99 > "$fixture/install-failure.log" 2>&1; then
  echo "The source build ignored the SDK installation failure." >&2
  exit 1
else
  [[ "$?" -eq 23 ]]
fi
grep -Fq 'Simulated SDK installation failure' "$fixture/install-failure.log"
grep -Fxq 'sdk9-install=failed' "$fixture/artifacts/log/Release/SourceBuildIntermediate/inner-toolchain-check.txt"

cp "$repo_root/global.json" "$inner/global.json"
cp "$repo_root/eng/Version.Details.xml" "$inner/eng/Version.Details.xml"
if BUILD_SOURCESDIRECTORY="$fixture" BUILD_REPOSITORY_ID=wrong-repository \
    BUILD_BUILDNUMBER=20260930.99 \
    bash "$inner/eng/BuildSourceBuildIntermediate.sh" --inner-legacy \
      "$fixture/.dotnet/dotnet" \
      "$fixture/tool-packages/microsoft.dotnet.arcade.sdk/9.0.0-beta.25164.2/tools" \
      17.12.1-hotfix-26480-99 >/dev/null 2>&1; then
  echo "The inner-only guard accepted a different repository." >&2
  exit 1
fi
cmp "$repo_root/global.json" "$inner/global.json"
cmp "$repo_root/eng/Version.Details.xml" "$inner/eng/Version.Details.xml"
printf '%s\n' "Legacy source-build helper kept toolchain edits in the private inner copy."
