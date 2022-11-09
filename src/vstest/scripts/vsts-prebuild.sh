#!/usr/bin/env bash
# Copyright (c) Microsoft. All rights reserved.

set -o nounset  # Fail on uninitialized variables.
set -e          # Fail on non-zero exit code.

# Parameter
TP_BUILD_SUFFIX="dev"
BRANCH=
IS_RTM=false

while [ $# -gt 0 ]; do
    lowerI="$(echo ${1:-} | awk '{print tolower($0)}')"
    case $lowerI in
        -build)
            TP_BUILD_SUFFIX=$2
            shift
            ;;
        -branch)
            BRANCH=$2
            shift
            ;;
        -rtm)
            IS_RTM=$2
            shift
            ;;
        -relpreview)
            IS_PREVIEW_REL_BRANCH=$2
            shift
            ;;
        *)
            break
            ;;
   esac
   shift
done

TP_ROOT_DIR=$(cd "$(dirname "$0")/.."; pwd -P)
TP_BUILD_PREFIX=$(grep TPVersionPrefix $TP_ROOT_DIR/scripts/build/TestPlatform.Settings.targets | head -1 | cut -d'>' -f2 | cut -d'<' -f1 || echo $TP_BUILD_PREFIX)
PACKAGE_VERSION="$TP_BUILD_PREFIX-$TP_BUILD_SUFFIX"

# Script
if [ $IS_RTM == true ]; then
    PACKAGE_VERSION="$TP_BUILD_PREFIX"
    TP_BUILD_SUFFIX=
else
    if [ ! -z "$BRANCH" ] && [[ $BRANCH =~ ^refs\/heads\/rel\/.*$ ]] && [ $IS_PREVIEW_REL_BRANCH == false ]; then
        TP_BUILD_SUFFIX="${TP_BUILD_SUFFIX/preview/release}"
    fi

    PACKAGE_VERSION="$TP_BUILD_PREFIX-$TP_BUILD_SUFFIX"
fi

echo "##vso[task.setvariable variable=BuildVersionPrefix;]$TP_BUILD_PREFIX"
echo "##vso[task.setvariable variable=BuildVersionSuffix;]$TP_BUILD_SUFFIX"
echo "##vso[task.setvariable variable=PackageVersion;]$PACKAGE_VERSION"
