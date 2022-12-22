#!/usr/bin/env bash
# Copyright (c) Microsoft. All rights reserved.
# Test script for test platform.

set -e

source "scripts/test.sh" "$@"

if [[ $? -ne 0 ]]; then
    exit 1
fi
