#!/usr/bin/env bash

### This script exports the sources of VMR (dotnet/dotnet) as an archive that can be used
### to build the .NET SDK.
### It expects you clone the dotnet/dotnet repo locally and check out the desired revision.
###
### USAGE:
###   ./pack-sources.sh -o dotnet.tar.gz
### Options:
###   -o, --output PATH
###       Optional. Path where the archive is created.
###       Defaults to artifacts/packages/dotnet-[SHA].tar.gz

source="${BASH_SOURCE[0]}"

# resolve $source until the file is no longer a symlink
while [[ -h "$source" ]]; do
  scriptroot="$( cd -P "$( dirname "$source" )" && pwd )"
  source="$(readlink "$source")"
  # if $source was a relative symlink, we need to resolve it relative to the path where the
  # symlink file was located
  [[ $source != /* ]] && source="$scriptroot/$source"
done
scriptroot="$( cd -P "$( dirname "$source" )" && pwd )"

function print_help () {
    sed -n '/^### /,/^$/p' "$source" | cut -b 5-
}

GIT_ROOT=$(realpath "$scriptroot/../")

output=''

while [[ $# -gt 0 ]]; do
  opt="$(echo "$1" | tr "[:upper:]" "[:lower:]")"
  case "$opt" in
    -o|--output)
      output=$2
      shift
      ;;
    -h|--help)
      print_help
      exit 0
      ;;
    *)
      fail "Invalid argument: $1"
      usage
      exit 1
      ;;
  esac

  shift
done

revision=$(git -C "$GIT_ROOT" rev-parse HEAD)
filename="dotnet-$revision"

if [[ -z "$output" ]]; then
  output="$GIT_ROOT/artifacts/packages/$filename.tar.gz"
fi

set -e

echo "Packing sources of $revision to $output.."
mkdir -p "$(dirname "$output")"
rm -f "$output"

# We need to had `.git/HEAD` and `.git/config` to the archive as the build expects those
git_config=$'[remote "origin"]\nurl="http://github.com/dotnet/dotnet"'

git -C "$GIT_ROOT" archive                               \
  -o "$output"                                           \
  --add-virtual-file "$filename/.git/HEAD:$revision"     \
  --add-virtual-file "$filename/.git/config:$git_config" \
  --prefix "$filename/"                                  \
  "$revision" "$GIT_ROOT"

echo 'Done.'
