#!/bin/bash

vmr_dir="${1:-/workspaces/dotnet}"
. "$vmr_dir/eng/common/tools.sh"

InitializeDotNetCli true
