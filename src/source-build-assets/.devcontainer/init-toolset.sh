#!/bin/bash

repo_dir="${1:-/workspaces/source-build-assets}"
. "$repo_dir/eng/common/tools.sh"

InitializeDotNetCli true
