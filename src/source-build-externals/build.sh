#!/usr/bin/env bash

source="${BASH_SOURCE[0]}"

# resolve $SOURCE until the file is no longer a symlink
while [[ -h $source ]]; do
  scriptroot="$( cd -P "$( dirname "$source" )" && pwd )"
  source="$(readlink "$source")"

  # if $source was a relative symlink, we need to resolve it relative to the path where the
  # symlink file was located
  [[ $source != /* ]] && source="$scriptroot/$source"
done

scriptroot="$( cd -P "$( dirname "$source" )" && pwd )"

sdkLine=$(grep -m 1 'dotnet' "$scriptroot/global.json")
sdkPattern="\"dotnet\" *: *\"(.*)\""
if [[ $sdkLine =~ $sdkPattern ]]; then
  export SDK_VERSION=${BASH_REMATCH[1]}
fi

"$scriptroot/eng/common/build.sh" --build --restore "$@"
