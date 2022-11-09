#!/usr/bin/env bash

./.devcontainer/source-build-scripts/createTarball.sh

cd $(realpath ..)/dotnet-source/

./prep.sh

# GitHub Codespaces automatically sets RepositoryName, which conflicts with source-build scripts.
unset RepositoryName

./build.sh --online --clean-while-building || true
