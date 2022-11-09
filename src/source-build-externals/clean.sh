#!/usr/bin/env bash
set -e

git submodule foreach --recursive git clean -xdff
git submodule foreach --recursive git reset --hard

if [ "$*" == "-a" ]
then
    echo "Removing all untracked files in the working tree"
    git clean -xdff
    exit $?
fi
