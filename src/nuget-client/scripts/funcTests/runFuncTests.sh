#!/usr/bin/env bash

UNIT_TESTS=0
FUNCTIONAL_TESTS=0
MONO_TESTS=0

while true ; do
    case "$1" in
        -u|--unit-tests) UNIT_TESTS=1 ; shift ;;
        -f|--functional-tests) FUNCTIONAL_TESTS=1 ; shift ;;
        -m|--mono-tests) MONO_TESTS=1 ; shift ;;
        --) shift ; break ;;
        *) shift ; break ;;
    esac
done

# move up to the repo root
SCRIPTDIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
DIR=$SCRIPTDIR/../..
pushd $DIR/

LOG_DIRECTORY=$BUILD_STAGINGDIRECTORY
if [ "$LOG_DIRECTORY" == "" ]; then
    LOG_DIRECTORY="$(pwd)/.test"
fi

if [ "$TestResultsDir" == "" ]; then
    TestResultsDir="$(pwd)/.test"
fi

# Run configure which installs the .NET SDK
. ./configure.sh
if [ $? -ne 0 ]; then
    echo "configure.sh failed !!"
    exit 1
fi

EXIT_CODE=0

if [ "$UNIT_TESTS" == "1" ]; then
    echo "=============== Build and run unit tests started at `date -u +"%Y-%m-%dT%H:%M:%S"` ================="
    echo "dotnet msbuild build/build.proj /restore:false /target:CoreUnitTests /property:Configuration=Release /property:ReleaseLabel=beta /bl:$LOG_DIRECTORY/binlog/03.CoreUnitTests.binlog"
    dotnet msbuild build/build.proj /restore:false /target:CoreUnitTests /property:Configuration=Release /property:ReleaseLabel=beta /bl:$LOG_DIRECTORY/binlog/03.CoreUnitTests.binlog
    EXIT_CODE=$?
    echo "=============== Build and run unit tests finished at `date -u +"%Y-%m-%dT%H:%M:%S"`================="
fi

if [ "$FUNCTIONAL_TESTS" == "1" ]; then
    echo "============ Build and run functional tests started at `date -u +"%Y-%m-%dT%H:%M:%S"` =============="
    echo "dotnet msbuild build/build.proj /restore:false /target:CoreFuncTests /property:Configuration=Release /property:ReleaseLabel=beta /bl:$LOG_DIRECTORY/binlog/04.CoreFuncTests.binlog"
    dotnet msbuild build/build.proj /restore:false /target:CoreFuncTests /property:Configuration=Release /property:ReleaseLabel=beta /bl:$LOG_DIRECTORY/binlog/04.CoreFuncTests.binlog
    EXIT_CODE=$?
    echo "============== Build and run functional tests finished at `date -u +"%Y-%m-%dT%H:%M:%S"`============"
fi

if [ "$MONO_TESTS" == "1" ]; then
    # Run mono test
    TestDir="$DIR/artifacts/NuGet.CommandLine.Test/"
    VsTestConsole="$DIR/artifacts/NuGet.CommandLine.Test/vstest/vstest.console.exe"
    TestResultsDir="$DIR/build/TestResults"
    VsTestVerbosity="minimal"

    if [ "$SYSTEM_DEBUG" == "true" ]; then
        VsTestVerbosity="detailed"
    fi

    #Clean System dll
    rm -rf "$TestDir/System.*" "$TestDir/WindowsBase.dll" "$TestDir/Microsoft.CSharp.dll" "$TestDir/Microsoft.Build.Engine.dll"

    case "$(uname -s)" in
		    Linux)
			    # We are not testing Mono on linux currently, so comment it out.
			    #echo "mono $VsTestConsole $TestDir/NuGet.CommandLine.Test.dll --TestCaseFilter:Platform!=Windows&Platform!=Darwin --logger:console;verbosity=$VsTestVerbosity --logger:"trx" --ResultsDirectory:$TestResultsDir"
			    #mono $VsTestConsole "$TestDir/NuGet.CommandLine.Test.dll" --TestCaseFilter:"Platform!=Windows&Platform!=Darwin" --logger:"console;verbosity=$VsTestVerbosity" --logger:"trx" --ResultsDirectory:"$TestResultsDir"
			    #EXIT_CODE=$?
			    ;;
		    Darwin)
                echo "==================== Run mono tests started at `date -u +"%Y-%m-%dT%H:%M:%S"` ======================"
			    echo "mono $VsTestConsole $TestDir/NuGet.CommandLine.Test.dll --TestCaseFilter:Platform!=Windows&Platform!=Linux --logger:console;verbosity=$VsTestVerbosity --logger:"trx" --ResultsDirectory:$TestResultsDir"
			    mono $VsTestConsole "$TestDir/NuGet.CommandLine.Test.dll" --TestCaseFilter:"Platform!=Windows&Platform!=Linux" --logger:"console;verbosity=$VsTestVerbosity" --logger:"trx" --ResultsDirectory:"$TestResultsDir"
			    EXIT_CODE=$?
                echo "================== mono tests finished at `date -u +"%Y-%m-%dT%H:%M:%S"` ==================="
                echo ""
			    ;;
		    *) ;;
    esac
fi

popd

if [ $EXIT_CODE -eq 0 ]; then
    echo "SUCCESS!"
else
    echo "FAILED!"
fi

exit $EXIT_CODE
