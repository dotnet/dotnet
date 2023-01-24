#!/usr/bin/env bash

usage() {
  echo "$0 [options]"
  echo "Creates a git clone of <source> into <dest> directory, optionally saving WIP changes."
  echo ""
  echo "Required:"
  echo "  --source <dir>  (Required) The source Git directory."
  echo "  --dest <dir>    (Required) The destination Git directory. Created if doesn't exist."
  echo ""
  echo "Optional:"
  echo "  --clean     If dest directory already exists, delete it."
  echo "  --copy-wip  Transfer most types of uncommitted change into the"
  echo "              destination directory. Useful for dev workflows."
  echo "  -h --help   Print help and exit."
}

# An alternative to "git clone" would be to use a "git worktree". Potential benefits:
# * This has more integration with the user's normal repo. If they make ad-hoc changes in the
#   worktree, it is easy to cherry-pick onto the developer's "real" branch.
# * A worktree uses a '.git' file rather than a full '.git' directory, which might have storage
#   implications. (However, a local clone's '.git' directory uses hard links to save space/time,
#   so it's not certain that this affects performance at all.)
# 
# Downside of worktrees:
# * Some configuration is set up in '.git/worktrees' which may be difficult to coordinate
#   properly. In particular, if the user is already using worktrees for their own purposes, we
#   would have to be careful that running source-build in one worktree doesn't interfere with
#   source-build in the other worktree.

set -euo pipefail

while [[ $# > 0 ]]; do
  opt="$(echo "$1" | tr "[:upper:]" "[:lower:]")"
  case "$opt" in
    -h|--help)
      usage
      exit 0
      ;;
    --source)
      sourceDir="$2"
      shift
      ;;
    --dest)
      destDir="$2"
      shift
      ;;
    --clean)
      clean=1
      ;;
    --copy-wip)
      copyWip=1
      ;;
    *)
      echo "Unrecognized option: $1"
      echo ""
      usage
      exit 1
      ;;
  esac
  shift
done

[ ! -d "${sourceDir:-}" ] && echo "--source not a directory: $sourceDir" && exit 1

[ ! "${destDir:-}" ] && echo "--dest not specified" && exit 1

echo "Cloning repository at: $sourceDir -> $destDir ..."

if [ -e "$destDir" ]; then
  echo "Destination already exists!"
  if [ -f "$destDir" ]; then
    echo "Existing destination is a regular file, not a directory. This is unusual: aborting."
    exit 1
  elif [ "${clean:-}" ]; then
    echo "Clean is enabled: removing $destDir and continuing..."
    rm -rf "$destDir"
  else
    echo "Clean is not enabled: aborting."
    exit 1
  fi
fi

(
  # This script uses "--git-dir" later on to detect a shallow submodule. "--git-dir" may give us a
  # relative path. The version of Git we use doesn't have "--absolute-git-dir". To keep things
  # simple, change to the repo's directory and work from there.
  cd "$sourceDir"

  if [ ! -e "$destDir" ]; then
    mkdir -p "$destDir"

    if [ "${copyWip:-}" ]; then
      # Copy over changes that haven't been committed, for dev inner loop.
      # This gets changes (whether staged or not) but misses untracked files.
      stashCommit=$(cd "$sourceDir"; git stash create)

      if [ "$stashCommit" ]; then
        echo "WIP changes detected: created temporary stash $stashCommit to transfer to inner repository..."
      else
        echo "No WIP changes detected..."
      fi
    fi

    echo "Creating empty clone at: $destDir"

    shallowFile="$(git rev-parse --git-dir)/shallow"

    if [ -f "$shallowFile" ]; then
      echo "Source repository is shallow..."
      if [ "${stashCommit:-}" ]; then
        echo "WIP stash is not supported in a shallow repository: aborting."
        exit 1
      fi

      # If source repo is shallow, old versions of Git refuse to clone it to another directory. First,
      # remove the 'shallow' file to trick Git into allowing the clone.
      shallowContent=$(cat "$shallowFile")
      rm "$shallowFile"

      # Then, run the clone:
      # * 'depth=1' avoids encountering the leaf commit in the shallow repo that points to a parent
      #   that doesn't exist. (The commit marked "grafted".) Git would fail here, otherwise.
      # * '--no-local' allows a shallow clone from a git dir on the same filesystem. This means the
      #   clone will not use hard links and takes up more space. However, since we're doing a shallow
      #   clone anyway, the difference is probably not significant. (This has not been measured.)
      # * '-c protocol.file.allow=alwyas allows cloning from the local filesystem even on newer versions
      #   of git which have disabled this for security reasons.
      git clone -c protocol.file.allow=always --depth=1 --no-local --no-checkout "$sourceDir" "$destDir"

      # Put the 'shallow' file back so operations on the outer Git repo continue to work normally.
      printf "%s" "$shallowContent" > "$shallowFile"
    else
      git clone -c protocol.file.allow=always --no-checkout "$sourceDir" "$destDir"
    fi

    (
      cd "$destDir"
      echo "Checking out sources..."
      # If no changes were stashed, stashCommit is empty string, and this is a simple checkout.
      git checkout ${stashCommit:-}
    )

    echo "Clone complete: $sourceDir -> $destDir"
  fi
)
