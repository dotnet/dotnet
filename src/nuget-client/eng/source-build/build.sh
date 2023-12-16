#!/usr/bin/env bash

echo "git config --global protocol.file.allow always"
git config --global protocol.file.allow always

source="${BASH_SOURCE[0]}"
scriptroot="$( cd -P "$( dirname "$source" )" && pwd )"

# resolve $SOURCE until the file is no longer a symlink
while [[ -h $source ]]; do
  scriptroot="$( cd -P "$( dirname "$source" )" && pwd )"
  source="$(readlink "$source")"

  # if $source was a relative symlink, we need to resolve it relative to the path where the
  # symlink file was located
  [[ $source != /* ]] && source="$scriptroot/$source"
done


while [[ $# > 0 ]]; do
    lowerI="$(echo $1 | awk '{print tolower($0)}')"
    case $lowerI in
        --configuration|-c)
            configuration=$2
            shift
            ;;
        -*)
            # just eat this so we don't try to pass it along to MSBuild
            export DOTNET_CORESDK_NOPRETTYPRINT=1
            ;;
        *)
            args="$args $1"
            ;;
    esac
    shift
done

function ReadGlobalVersion {
  local key=$1
  local global_json_file="$scriptroot/global.json"

  if command -v jq &> /dev/null; then
    _ReadGlobalVersion="$(jq -r ".[] | select(has(\"$key\")) | .\"$key\"" "$global_json_file")"
  elif [[ "$(cat "$global_json_file")" =~ \"$key\"[[:space:]\:]*\"([^\"]+) ]]; then
    _ReadGlobalVersion=${BASH_REMATCH[1]}
  fi

  if [[ -z "$_ReadGlobalVersion" ]]; then
    Write-PipelineTelemetryError -category 'Build' "Error: Cannot find \"$key\" in $global_json_file"
    ExitWithExitCode 1
  fi
}

function GetNuGetPackageCachePath {
  if [[ -z ${NUGET_PACKAGES:-} ]]; then
    if [[ "$use_global_nuget_cache" == true ]]; then
      export NUGET_PACKAGES="$HOME/.nuget/packages"
    else
      export NUGET_PACKAGES="$repo_root/.packages"
      export RESTORENOCACHE=true
    fi
  fi
}

if [[ "$DOTNET" == "" && "$DOTNET_PATH" != "" ]]; then
  export DOTNET="$DOTNET_PATH/dotnet"
else
  ReadGlobalVersion dotnet
  export SDK_VERSION=$_ReadGlobalVersion

  mkdir -p "$scriptroot/../../cli"
  curl -o "$scriptroot/../../cli/dotnet-install.sh" -L https://dot.net/v1/dotnet-install.sh

  if (( $? )); then
    echo "Could not download 'dotnet-install.sh' script. Please check your network and try again!"
    exit 1
  fi
  chmod +x "$scriptroot/../../cli/dotnet-install.sh"

  "$scriptroot/../../cli/dotnet-install.sh" -v $SDK_VERSION -i "$scriptroot/../../cli"
  export DOTNET=${DOTNET:-$scriptroot/../../cli/dotnet}
fi

ReadGlobalVersion Microsoft.DotNet.Arcade.Sdk
export ARCADE_VERSION=$_ReadGlobalVersion

if [ -z "$DotNetBuildFromSourceFlavor" ] || [ "$DotNetBuildFromSourceFlavor" != "Product" ]; then
  export NUGET_PACKAGES=$scriptroot/../../artifacts/sb/package-cache/
fi

"$DOTNET" msbuild "$scriptroot/source-build.proj" /p:Configuration=$configuration /p:DotNetBuildFromSource=true /p:ArcadeBuildFromSource=true "/p:RepoRoot=$scriptroot/../../" "/bl:$scriptroot/../../artifacts/sb/log/source-build.binlog" $args
