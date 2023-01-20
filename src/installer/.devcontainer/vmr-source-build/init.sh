#!/usr/bin/env bash

set -ex

source="${BASH_SOURCE[0]}"
script_root="$( cd -P "$( dirname "$source" )" && pwd )"

installer_dir=$(realpath "$script_root/../..")
workspace_dir=$(realpath "$installer_dir/../")
tmp_dir=$(realpath "$workspace_dir/tmp")
vmr_dir=$(realpath "$workspace_dir/dotnet")

mkdir -p "$tmp_dir"

# Codespaces performs a shallow fetch only
git -C "$installer_dir" fetch --all --unshallow

# We will try to figure out, which branch is the current (PR) branch based off of
# We need this to figure out, which VMR branch to use
vmr_branch=$(git log --pretty=format:'%D' HEAD^ | grep 'origin/' | head -n1 | sed 's@origin/@@' | sed 's@,.*@@')

"$installer_dir/eng/vmr-sync.sh" \
    --vmr "$vmr_dir"             \
    --tmp "$tmp_dir"             \
    --branch "$vmr_branch"       \
    --debug

# Run prep.sh
unset RepositoryName
pushd "$vmr_dir"
./prep.sh
popd
