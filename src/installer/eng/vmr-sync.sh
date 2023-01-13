#!/bin/bash

### This script is used for synchronizing the dotnet/dotnet repository locally
### It is used during CI to ingest new code based on dotnet/installer
### I can also help for reproducing potential failures during installer's PRs,
### namely during errors during the 'Synchronize dotnet/dotnet' build step from
### the 'VMR Source-Build'.
### The following scenario is assumed:
### - There is a PR in dotnet/installer
### - The PR is failing on the 'VMR Source-Build' job in the 'Synchronize dotnet/dotnet' step
###
### There are a few possible reasons but usually this happens because there is a mismatch between
### source patches that are applied on top of the files that are being synchronized from repositories
### into the 'dotnet/dotnet' repo and new changes in the repositories.
### This manifests as the following error:
###     fail: Failed to synchronize repo installer
###           Failed to apply the patch for src/aspnetcore
###           Exit code: 1
###           Std err:
###           error: patch failed: src/aspnetcore/eng/SourceBuild.props:55
###           error: src/aspnetcore/eng/SourceBuild.props: patch does not apply
###
### where 'src/aspnetcore/eng/SourceBuild.props' would be the file that is being patched and new
### changes to it are conflicting with the patch that is being applied.
###
### The whole process can be reproduced locally easily by running this script.
### The patches are located in a folder in the 'dotnet/installer' repository.
### At the moment of writing, the location is 'src/SourceBuild/patches' but to get
### the up-to-date location, please see the 'patchesPath' property in
### https://github.com/dotnet/dotnet/blob/main/src/source-mappings.json
###
### You will need to compare what is in the patch and what is in the file and fix the patch.
###
### The tooling that synchronizes the VMR will need to clone the various repositories.
### It clones them into a temporary folder and re-uses them on future runs so it is advised
### you dedicate a folder to this to speed up your re-runs.
###
### This script will synchronize the 'dotnet/dotnet' repo locally and let you inspect the changes.
###
### USAGE:
###     ./vmr-sync.sh --tmp-dir "$HOME/repos/tmp"
### Options:
###   -t, --tmp, --tmp-dir PATH
###       Required. Path to the temporary folder where repositories will be cloned
###   -v, --vmr, --vmr-dir PATH
###       Optional. Path to the dotnet/dotnet repository. When null, gets cloned to the temporary folder
###   -b, --branch, --vmr-branch BRANCH_NAME
###       Optional. Branch of the 'dotnet/dotnet' repo to synchronize to
###       This should match the target branch of the PR, defaults to 'main'
###   --target-ref GIT_REF
###       Optional. Git ref to synchronize to. This can be a specific commit, branch, tag..
###       Defaults to the revision of the parent installer repo
###   --debug
###       Optional. Turns on the most verbose logging for the VMR tooling

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

COLOR_RED=$(tput setaf 1 2>/dev/null || true)
COLOR_CYAN=$(tput setaf 6 2>/dev/null || true)
COLOR_CLEAR=$(tput sgr0 2>/dev/null || true)
COLOR_RESET=uniquesearchablestring
FAILURE_PREFIX='> '

function fail () {
  echo "${COLOR_RED}$FAILURE_PREFIX${1//${COLOR_RESET}/${COLOR_RED}}${COLOR_CLEAR}" >&2
}

function highlight () {
  echo "${COLOR_CYAN}$FAILURE_PREFIX${1//${COLOR_RESET}/${COLOR_CYAN}}${COLOR_CLEAR}"
}

installer_dir=$(realpath "$scriptroot/../")
tmp_dir=''
vmr_dir=''
vmr_branch='main'
target_ref=''
verbosity=verbose

while [[ $# -gt 0 ]]; do
  opt="$(echo "$1" | tr "[:upper:]" "[:lower:]")"
  case "$opt" in
    -t|--tmp|--tmp-dir)
      tmp_dir=$2
      shift
      ;;
    -v|--vmr|--vmr-dir)
      vmr_dir=$2
      shift
      ;;
    -b|--branch|--vmr-branch)
      vmr_branch=$2
      shift
      ;;
    -d|--debug)
      verbosity=debug
      ;;
    --target-ref)
      target_ref=$2
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

if [[ ! -d "$installer_dir" ]]; then
  fail "Directory '$installer_dir' does not exist. Please specify the path to the dotnet/installer repo"
  exit 1
fi

if [[ -z "$tmp_dir" ]]; then
  fail "Missing --tmp-dir argument. Please specify the path to the temporary folder where the repositories will be cloned"
  exit 1
fi

if [[ -z "$vmr_dir" ]]; then
  vmr_dir="$tmp_dir/dotnet"
fi

if [[ ! -d "$tmp_dir" ]]; then
  mkdir -p "$tmp_dir"
fi

if [[ "$verbosity" == "debug" ]]; then
  set -x
fi

if [[ ! -d "$vmr_dir" ]]; then
  highlight "Cloning 'dotnet/dotnet' into $vmr_dir.."
  git clone https://github.com/dotnet/dotnet "$vmr_dir"
else
  if ! git -C "$vmr_dir" diff --quiet; then
    fail "There are changes in the working tree of $vmr_dir. Please commit or stash your changes"
    exit 1
  fi

  highlight "Preparing $vmr_dir"
  git -C "$vmr_dir" checkout "$vmr_branch"
  git -C "$vmr_dir" pull
fi

set -e

# Prepare darc
highlight 'Installing .NET, preparing the tooling..'
source "$scriptroot/common/tools.sh"
InitializeDotNetCli true
dotnet="$scriptroot/../.dotnet/dotnet"
"$dotnet" tool restore

# Run the sync
if [[ -z "$target_ref" ]]; then
  target_ref=$(git -C "$installer_dir" rev-parse HEAD)
fi

highlight "Starting the synchronization to '$target_ref'.."
set +e

if "$dotnet" darc vmr update --vmr "$vmr_dir" --tmp "$tmp_dir" --$verbosity --recursive --additional-remotes "installer:$installer_dir" "installer:$target_ref"; then
  highlight "Synchronization succeeded"
else
  fail "Synchronization of dotnet/dotnet to '$target_ref' failed!"
  fail "'$vmr_dir' is left in its last state (re-run of this script will reset it)."
  fail "Please inspect the logs which contain path to the failing patch file (use --debug to get all the details)."
  fail "Once you make changes to the conflicting VMR patch, commit it locally and re-run this script."
  exit 1
fi
