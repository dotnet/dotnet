#!/usr/bin/env bash
# Copyright (c) Microsoft. All rights reserved.
# Build script for test platform.

set -e

source "scripts/build.sh" "$@"

if [[ $? -ne 0 ]]; then
    exit 1
fi
