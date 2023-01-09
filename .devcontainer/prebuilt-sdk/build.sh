#!/usr/bin/env bash

# GitHub Codespaces sets this and it conflicts with source-build scripts.
unset RepositoryName

source="${BASH_SOURCE[0]}"
scriptroot="$( cd -P "$( dirname "$source" )" && pwd )"

"$scriptroot"/../../prep.sh
"$scriptroot"/../../build.sh --online --clean-while-building || exit 0
