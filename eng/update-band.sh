#!/bin/bash
set -euo pipefail

usage() {
  echo "Usage: $0 --remote <name> --1xx-branch <branch>"
  echo ""
  echo "  --remote <name>         Git remote to pull from (e.g., upstream)"
  echo "  --1xx-branch <branch>   1xx branch name to merge in (e.g., main)"
  echo "  --continue-merge        Continue after manually fixing merge conflicts"
  echo "  --help, -help           Show this help message and exit"
  echo ""
  echo "  Arguments may also be passed with a single dash."
}

remote=''
branch_1xx=''
continue_merge='false'

attempt_merge() {
  if { git diff --check | grep -q .; } || [ -n "$(git ls-files -u)" ]; then
    echo "There are unresolved conflicts. Please resolve them before continuing."
    exit 1
  else
    echo "Continuing with merge..."
    merge_msg=$(head -n 1 .git/MERGE_MSG)
    git commit -m "$merge_msg"
  fi
}

# Deleted repos to exclude from merge
deleted_repos=(
  "aspire" "aspnetcore" "cecil" "command-line-api" "deployment-tools"
  "diagnostics" "efcore" "emsdk" "runtime" "source-build-externals"
  "sourcelink" "symreader" "windowsdesktop" "winforms" "wpf" "xdt"
)

while [[ $# > 0 ]]; do
  opt="$(echo "${1/#--/-}" | tr "[:upper:]" "[:lower:]")"
  case "$opt" in
    -help|-h)
      usage
      exit 0
      ;;
    -remote)
      remote=$2
      shift
      ;;
    -1xx-branch)
      branch_1xx=$2
      shift
      ;;
    -continue-merge)
      continue_merge=true
      ;;
    *)
      echo "Error: Unknown argument."
      exit 1
      ;;
  esac

  shift
done

if [[ "$continue_merge" == 'true' ]]; then
  attempt_merge
elif [[ -z "$remote" ]]; then
  echo "Error: --remote is required."
  usage
  exit 1
elif [[ -z "$branch_1xx" ]]; then
  echo "Error: --1xx-branch is required."
  usage
  exit 1
fi

if ! git diff --quiet || ! git diff --cached --quiet; then
  echo "You have uncommitted changes. Please commit or stash them before continuing."
  exit 1
fi

# Attempt merge
if [ "$continue_merge" != "true" ] && ! git merge --no-commit --no-ff "$remote/$branch_1xx" >/dev/null 2>&1; then
  echo "Cleaning excluded paths..."

  for repo in "${deleted_repos[@]}"; do
    if [ -e "src/$repo" ]; then
      git reset HEAD -- "src/$repo" 2>/dev/null || true
      git rm -rf --cached "src/$repo" 2>/dev/null || true
      rm -rf "src/$repo" 2>/dev/null || true
    fi
  done

  attempt_merge
fi

echo "Completed merge"
