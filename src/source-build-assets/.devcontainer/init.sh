#!/usr/bin/env bash

set -eux

source="${BASH_SOURCE[0]}"
script_root="$( cd -P "$( dirname "$source" )" && pwd )"

workspace_dir=$(realpath "$script_root/../../")
tmp_dir=$(realpath "$workspace_dir/tmp")
repo_dir=$(realpath "$workspace_dir/source-build-reference-packages")

$repo_dir/.devcontainer/init-toolset.sh $repo_dir

mkdir -p "$tmp_dir"
