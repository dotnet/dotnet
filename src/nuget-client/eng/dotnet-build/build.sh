#!/usr/bin/env bash

echo "git config --global protocol.file.allow always"
git config --global protocol.file.allow always

source="${BASH_SOURCE[0]}"
scriptroot="$( cd -P "$( dirname "$source" )" && pwd )"
configuration='Release'
source_build=false
properties=''

# resolve $SOURCE until the file is no longer a symlink
while [[ -h $source ]]; do
  scriptroot="$( cd -P "$( dirname "$source" )" && pwd )"
  source="$(readlink "$source")"

  # if $source was a relative symlink, we need to resolve it relative to the path where the
  # symlink file was located
  [[ $source != /* ]] && source="$scriptroot/$source"
done

repo_root=`cd -P "$scriptroot/../.." && pwd`
repo_root="${repo_root}/"

while [[ $# > 0 ]]; do
    lowerI="$(echo $1 | awk '{print tolower($0)}')"
    case $lowerI in
        --configuration|-c)
            configuration=$2
            shift
            ;;
        --source-build|-sb)
            source_build=true
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

if [[ "$DOTNET" == "" && "$DOTNET_PATH" != "" ]]; then
  export DOTNET="$DOTNET_PATH/dotnet"
else
  ReadGlobalVersion dotnet
  export SDK_VERSION=$_ReadGlobalVersion

  mkdir -p "${repo_root}cli"
  curl -o "${repo_root}cli/dotnet-install.sh" -L https://dot.net/v1/dotnet-install.sh

  if (( $? )); then
    echo "Could not download 'dotnet-install.sh' script. Please check your network and try again!"
    exit 1
  fi
  chmod +x "${repo_root}cli/dotnet-install.sh"

  "${repo_root}cli/dotnet-install.sh" -v $SDK_VERSION -i "${repo_root}cli"
  export DOTNET=${repo_root}cli/dotnet
fi

ReadGlobalVersion Microsoft.DotNet.Arcade.Sdk
export ARCADE_VERSION=$_ReadGlobalVersion
export NUGET_PACKAGES=${repo_root}artifacts/sb/package-cache/

if [[ "$source_build" == true ]]; then
  properties="$properties /p:DotNetBuildSourceOnly=true"
fi

properties="$properties /p:Configuration=$configuration"
properties="$properties /p:DotNetBuildRepo=true"
properties="$properties /p:RepoRoot=$repo_root"

"$DOTNET" msbuild "$scriptroot/dotnet-build.proj" "/bl:${repo_root}artifacts/sb/log/source-inner-build.binlog" $properties $args
