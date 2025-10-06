#!/usr/bin/env bash

echo "git config --global protocol.file.allow always"
git config --global protocol.file.allow always

source="${BASH_SOURCE[0]}"
scriptroot="$( cd -P "$( dirname "$source" )" && pwd )"

args=()

configuration='Release'
verbosity='minimal'
source_build=false
product_build=false
from_vmr=false
ci=false
warn_as_error=true
node_reuse=true
binary_log=false

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
    --verbosity|-v)
      verbosity=$2
      shift
      ;;
    --binarylog|-bl)
      binary_log=true
      ;;
    --ci)
      ci=true
      ;;
    --configuration|-c)
      configuration=$2
      shift
      ;;
    --source-build|--sourcebuild|-sb)
      source_build=true
      product_build=true
      ;;
    --product-build|--productbuild|-pb)
      product_build=true
      ;;
    --from-vmr|--fromvmr)
      from_vmr=true
      ;;
    --warnaserror)
      warn_as_error=$2
      shift
      ;;
    --nodereuse)
      node_reuse=$2
      shift
      ;;
    *)
      args+=("$1")
      ;;
    esac

    shift
done

function ReadGlobalVersion {
  local key=$1
  local global_json_file="$repo_root/global.json"

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

# Environment variables
export ARCADE_VERSION=$_ReadGlobalVersion
export NUGET_PACKAGES=${repo_root}artifacts/.packages/

# MSBuild arguments
dotnetArguments=()
dotnetArguments+=("$scriptroot/dotnet-build.proj")
dotnetArguments+=("/p:RepoRoot=$repo_root")
dotnetArguments+=("/p:Configuration=$configuration")
dotnetArguments+=("/p:DotNetBuild=$product_build")
dotnetArguments+=("/p:DotNetBuildSourceOnly=$source_build")
dotnetArguments+=("/p:DotNetBuildFromVMR=$from_vmr")

local bl=""
if [[ "$binary_log" == true ]]; then
  bl="/bl:\"${repo_root}artifacts/log/${configuration}/Build.binlog\""
fi

if [[ "$ci" == true ]]; then
  node_reuse=false
fi

local warnaserror_switch=""
if [[ $warn_as_error == true ]]; then
  warnaserror_switch="/warnaserror"
fi

"$DOTNET" msbuild /m /nologo /clp:Summary /v:$verbosity /nr:$node_reuse $warnaserror_switch /p:TreatWarningsAsErrors=$warn_as_error /p:ContinuousIntegrationBuild=$ci $bl ${dotnetArguments[@]+"${dotnetArguments[@]}"} ${args[@]+"${args[@]}"}
