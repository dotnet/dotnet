#!/bin/bash

repo_dir="${1:-/workspaces/source-build-reference-packages}"
. "$repo_dir/eng/common/tools.sh"

InitializeDotNetCli true
