#!/usr/bin/env bash
# Updates SourceRevisionId and FileVersionRevision in an external package .proj file.
#
# After adding or updating a git submodule for an external component, run this script
# to automatically set the correct SourceRevisionId (from the submodule commit) and
# FileVersionRevision (from the Microsoft-shipped NuGet package) in the component's
# .proj file.
#
# Usage: ./update-external-metadata.sh <component-name>
# Example: ./update-external-metadata.sh azure-activedirectory-identitymodel-extensions-for-dotnet

set -euo pipefail

component_name="${1:?Usage: $0 <component-name>}"

repo_root="$(cd -P "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

dotnet_tool="$repo_root/.dotnet/dotnet"
if [ ! -x "$dotnet_tool" ]; then
    dotnet_tool="dotnet"
fi

project_path="$repo_root/eng/tools/UpdateExternalMetadata/UpdateExternalMetadata.csproj"

"$dotnet_tool" run --project "$project_path" -- "$component_name"
