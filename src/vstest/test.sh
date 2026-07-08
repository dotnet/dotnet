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
if [[ -z "${DOTNET_ROOT:-}" && -d "$scriptroot/.dotnet" ]]; then
  export DOTNET_ROOT="$scriptroot/.dotnet"
fi
# Disable MSBuild terminal logger so test output isn't buffered. TestCaptureOutput=false
# means there are no log files to reference, so the terminal logger would show
# misleading "See log file" messages with paths to files that don't exist.
export MSBUILDTERMINALLOGGER=${MSBUILDTERMINALLOGGER:-off}
"$scriptroot/eng/common/build.sh" --test "$@"
