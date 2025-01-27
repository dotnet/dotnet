#!/usr/bin/env bash
# Copyright (c) Microsoft. All rights reserved.
# Test script for Test Platform.

set -o nounset  # Fail on uninitialized variables.
set -e          # Fail on non-zero exit code.

# ANSI color codes
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[0;33m'
NOCOLOR='\033[0m'

#
# Parse options
#
CONFIGURATION="Debug"
TARGET_RUNTIME="ubuntu.18.04-x64"
FAIL_FAST=false
VERBOSE=false

while [ $# -gt 0 ]
do
    lowerI="$(echo ${1:-} | awk '{print tolower($0)}')"
    case $lowerI in
        -h | --help)
            usage
            exit
            ;;
        -c)
            CONFIGURATION=$2
            shift
            shift
            ;;
        -r)
            TARGET_RUNTIME=$2
            shift
            shift
            ;;
        -p)
            PROJECT_NAME_PATTERNS=$2
            shift
            shift
            ;;
        -verbose)
            VERBOSE=$2
            shift
            shift
            ;;
        *)
            echo Unknown parameter $key
            shift
            ;;
   esac
done

#
# Variables
#
PROJECT_NAME_PATTERNS=**$PROJECT_NAME_PATTERNS*bin*$CONFIGURATION*net9.0*$PROJECT_NAME_PATTERNS*Tests*dll
TP_ROOT_DIR=$(cd "$(dirname "$0")"; pwd -P)
TP_TOOLS_DIR="$TP_ROOT_DIR/tools"
TP_PACKAGES_DIR="$TP_ROOT_DIR/packages"
TP_OUT_DIR="$TP_ROOT_DIR/artifacts"

#
# Dotnet configuration
#
# Disable first run since we want to control all package sources
export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
# Dotnet build doesnt support --packages yet. See https://github.com/dotnet/cli/issues/2712
export NUGET_PACKAGES=$TP_PACKAGES_DIR
DOTNET_CLI_VERSION="latest"

#
# Build configuration
#
TPB_Solution="TestPlatform.sln"
TPB_TargetFrameworkCore="net8.0"
TPB_Configuration=$CONFIGURATION
TPB_TargetRuntime=$TARGET_RUNTIME
TPB_Verbose=$VERBOSE

#
# Logging
#
log()
{
    printf "${GREEN}... $@${NOCOLOR}\n"
}

verbose()
{
    if [ ${TPB_Verbose-false} ]
    then
        printf "${YELLOW}... $@${NOCOLOR}\n" >&2
    fi
}

error()
{
    printf "${RED}... $@${NOCOLOR}\n" >&2
}

function usage()
{
    log " Usage: ./test.sh [Options]"
    log ""
    log " -c <CONFIGURATION>                Build the specified Configuration (Debug or Release, default: Debug)"
    log " -r <TARGET_RUNTIME>               Build for the specified runtime moniker (ubuntu.16.04-x64)"
    log " -p <PROJECT_NAME_PATTERNS>        Pattern to test specific projects"
    log " -verbose <VERBOSE>                Enable verbose logging (true, false)"
}

#
# Test steps
#
function invoke_test()
{
    local dotnet=$(_get_dotnet_path)
    local vstest=$TP_OUT_DIR/$TPB_Configuration/$TPB_TargetFrameworkCore/vstest.console.dll

    find ./test -ipath $PROJECT_NAME_PATTERNS | xargs $dotnet $vstest --parallel --testcasefilter:"TestCategory!=Windows&TestCategory!=Windows-Review" --logger:"trx"
}

#
# Privates
#
_get_dotnet_path()
{
    echo "$TP_TOOLS_DIR/dotnet-linux/dotnet"
}

invoke_test
