#!/usr/bin/env bash

# This works around an issue where .editorconfigs in nuget packages are not respected if the NUGET_PACKAGES
# environment variable does not end with a slash.  We copy the logic for setting the NUGET_PACKAGES variable from the eng/common/tools.sh
# script as we cannot modify the script itself (its arcade managed).
# This workaround is only required for non-windows builds as the powershell versions of the arcade scripts already ensure a trailing slash is present.
# Tracking https://github.com/dotnet/roslyn/issues/72657
ci=false
while [[ $# > 0 ]]; do
  opt="$(echo "${1/#--/-}" | tr "[:upper:]" "[:lower:]")"
  case "$opt" in
    -ci)
      ci=true
      break
      ;;
    *)
      shift
      ;;
  esac
done
if [[ "$ci" == true ]]; then
  if [[ -z ${NUGET_PACKAGES:-} ]]; then
    if [[ "$ci" == true ]]; then
      export NUGET_PACKAGES="$HOME/.nuget/packages/"
    else
      export NUGET_PACKAGES="$repo_root/.packages/"
      export RESTORENOCACHE=true
    fi
  fi
fi