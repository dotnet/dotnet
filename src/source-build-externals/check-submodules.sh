#!/usr/bin/env bash
set -euo pipefail
IFS=$'\n\t'

SCRIPT_ROOT="$(cd -P "$( dirname "$0" )" && pwd)"
SUBMODULES="$SCRIPT_ROOT/.gitmodules"
CLEAN_ALL_SENTINEL="$SCRIPT_ROOT/.cleansourcebuildsubmodules"
PROMPT_TIMEOUT=30

# has the user answered "yes to all" to init'ing submodules?
answered_all=0

# ask for confirmation and initialize a submodule if approved.
init_submodule () {
  module=$1
  done=0
  while [ $done == 0 ]; do
    noreply=0
    echo "warning: submodules $module does not appear to be initialized."
    read -p "Should I initialize it for you? [Y]es / [n]o / [a]ll / [q]uit] " -n 1 -r -t $PROMPT_TIMEOUT || noreply=1
    echo
    if [[ $noreply == 1 || $REPLY =~ ^[Aa]$ ]]; then
      answered_all=1
      done=1
      git submodule update --init --recursive "$module"
    elif [[ $REPLY =~ ^[Qq]$ ]]; then
      exit 0
    elif [[ $REPLY =~ ^[Nn]$ ]]; then
      done=1
    elif [[ $REPLY == "" || $REPLY == " " || $REPLY =~ ^[Yy]$ ]]; then
      done=1
      git submodule update --init --recursive "$module"
    else
      echo "didn't understand that ($REPLY)"
    fi
  done
}

# update a submodule to an expected commit.  We give different messages
# for being ahead vs being behind or diverged from the expected commit,
# so this function is parameterized.
fix_submodule () {
  path="$1"
  expected=$2
  actual=$3
  msg="$4"
  prompt="$5"
  done=0
  while [ $done == 0 ]; do
    noreply=0
    echo -e "$msg"
    read -p "$prompt " -n 1 -r -t $PROMPT_TIMEOUT || noreply=1
    echo
    if [[ $noreply == 1 || $REPLY == "" || $REPLY == " " || $REPLY =~ ^[Nn]$ ]]; then
      done=1
    elif [[ $REPLY =~ ^[Qq]$ ]]; then
      exit 1
    elif [[ $REPLY =~ ^[Yy]$ ]]; then
      # check if we have this commit locally and can skip the fetch
      git cat-file -e $expected^{commit} 2>/dev/null || exitCode=$?
      if [ $exitCode != 0 ]; then
        git fetch
      fi
      exitCode=0
      # double-check, we should have the commit now unless something
      # weird is going on.
      git cat-file -e $expected^{commit} 2>/dev/null || exitCode=$?
      if [ $exitCode != 0 ]; then
        echo "error: commit $expected was not found in $path"
        echo "The remote may have changed in source-build; run 'git submodule sync' and retry."
        echo "Are you using a custom remote for this submodule?  You may need to pick up changes from upstream."
        echo "Canceling remainder of checks."
        exit 1
      fi
      git checkout $expected
      done=1
    else
      echo "didn't understand that ($REPLY)"
    fi
  done
}

# clean a submodule if the user approves.
clean_submodule() {
  path="$1"
  msg="$2"
  prompt="$3"
  done=0
  while [ $done == 0 ]; do
    noreply=0
    echo -e "$msg"
    read -p "$prompt " -n 1 -r -t $PROMPT_TIMEOUT || noreply=1
    echo
    if [[ $noreply == 1 || $REPLY == "" || $REPLY == " " || $REPLY =~ ^[Nn]$ ]]; then
      done=1
    elif [[ $REPLY =~ ^[Aa]$ ]]; then
      git clean -fxd
      git reset --hard HEAD
      touch "$CLEAN_ALL_SENTINEL"
      done=1
    elif [[ $REPLY =~ ^[Qq]$ ]]; then
      exit 1
    elif [[ $REPLY =~ ^[Yy]$ ]]; then
      git clean -fxd
      git reset --hard HEAD
      done=1
    else
      echo "didn't understand that ($REPLY)"
    fi
  done
}

# We use the same script for checking the super-repo and the submodules.
# Having the first argument be "in-submodule" triggers this submodule behavior.
if [ ${1:-default} == "in-submodule" ]; then
  path="$2"
  expected_sha=$3
  subcommit=`git rev-parse HEAD`
  if [ "$subcommit" != "$expected_sha" ]; then
    exitCode=0
    # merge-base fails if the commit is missing, so check for that first.
    git cat-file -e $expected_sha^{commit} 2>/dev/null || exitCode=$?
    if [ $exitCode != 0 ]; then
      mergeBase="missing commit"
    else
      mergeBase=$(git merge-base HEAD $expected_sha)
    fi
    if [ "$mergeBase" != "$expected_sha" ]; then
      fix_submodule $path $expected_sha $subcommit "warning: submodule $path, currently at $subcommit, has diverged from checked-in version $expected_sha\nif you are changing a submodule branch or moving a submodule backwards, this is expected.\nShould I checkout $path to the expected commit $expected_sha?" "[N]o / [y]es / [q]uit"
    else
      fix_submodule $path $expected_sha $subcommit "warning: submodule $path, currently at $subcommit, is ahead of checked-in version $expected_sha\nif you are updating a submodule, this is expected.\nShould I checkout $path to the expected commit $expected_sha?" "[N]o / [y]es / [q]uit"
    fi
  fi
  # check for staged changes and unstaged modifications
  git diff-index --quiet HEAD -- || exit_code=$?
  # check for untracked new files
  untracked="$(git ls-files --others --exclude-standard)"
  if [[ ${exit_code:-0} != 0 || ! -z "$untracked" ]]; then
    if [ -e "$CLEAN_ALL_SENTINEL" ]; then
      git clean -fxd
      git reset --hard HEAD
    else
      clean_submodule $path "warning: submodule $path has uncommitted changes\nShould I clean and reset $path (this will lose ALL uncommitted changes)?" "[N]o / [y]es / [a]ll / [q]uit"
    fi
  fi
# Main branch for super-repo behavior
else
  # submodule foreach doesn't work until submodules are init'd, read the modules manually
  for module in `git config --file "$SUBMODULES" --get-regexp path | awk '{ print $2 }'`
  do
    if [ ! -e "$SCRIPT_ROOT/$module/.git" ]; then
      if [ $answered_all == 1 ]; then
        git submodule update --init --recursive "$module"
      else
        init_submodule "$module"
      fi
    fi
  done
  # kick off the submodule behavior for each repo
  rm -f "$CLEAN_ALL_SENTINEL"
  git submodule foreach --quiet --recursive "$SCRIPT_ROOT/check-submodules.sh in-submodule \"\$path\" \"\$sha1\""
  rm -f "$CLEAN_ALL_SENTINEL"
fi

