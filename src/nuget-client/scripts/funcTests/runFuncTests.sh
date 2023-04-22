#!/usr/bin/env bash

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

echo "=============== Build and run unit tests started at `date -u +"%Y-%m-%dT%H:%M:%S"` ================="
echo "dotnet msbuild build/build.proj /restore:false /target:CoreUnitTests /property:Configuration=Release /property:ReleaseLabel=beta /bl:$LOG_DIRECTORY/binlog/03.CoreUnitTests.binlog"
dotnet msbuild build/build.proj /restore:false /target:CoreUnitTests /property:Configuration=Release /property:ReleaseLabel=beta /bl:$LOG_DIRECTORY/binlog/03.CoreUnitTests.binlog
UNIT_TEST_RESULT=$?
echo "=============== Build and run unit tests finished at `date -u +"%Y-%m-%dT%H:%M:%S"`================="
echo ""

echo "============ Build and run functional tests started at `date -u +"%Y-%m-%dT%H:%M:%S"` =============="
echo "dotnet msbuild build/build.proj /restore:false /target:CoreFuncTests /property:Configuration=Release /property:ReleaseLabel=beta /bl:$LOG_DIRECTORY/binlog/04.CoreFuncTests.binlog"
dotnet msbuild build/build.proj /restore:false /target:CoreFuncTests /property:Configuration=Release /property:ReleaseLabel=beta /bl:$LOG_DIRECTORY/binlog/04.CoreFuncTests.binlog
FUNC_TEST_RESULT=$?
echo "============== Build and run functional tests finished at `date -u +"%Y-%m-%dT%H:%M:%S"`============"
echo ""

MONO_TEST_RESULT=-1
if [ "$CI" == "true" ]; then
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
			    #MONO_TEST_RESULT=$?
			    ;;
		    Darwin)
                echo "==================== Run mono tests started at `date -u +"%Y-%m-%dT%H:%M:%S"` ======================"
			    echo "mono $VsTestConsole $TestDir/NuGet.CommandLine.Test.dll --TestCaseFilter:Platform!=Windows&Platform!=Linux --logger:console;verbosity=$VsTestVerbosity --logger:"trx" --ResultsDirectory:$TestResultsDir"
			    mono $VsTestConsole "$TestDir/NuGet.CommandLine.Test.dll" --TestCaseFilter:"Platform!=Windows&Platform!=Linux" --logger:"console;verbosity=$VsTestVerbosity" --logger:"trx" --ResultsDirectory:"$TestResultsDir"
			    MONO_TEST_RESULT=$?
                echo "================== mono tests finished started at `date -u +"%Y-%m-%dT%H:%M:%S"` ==================="
                echo ""
			    ;;
		    *) ;;
    esac
fi

popd

EXITCODE=0

echo "Test Results:"
if [ $UNIT_TEST_RESULT -eq 0 ]; then
    echo "  Unit tests:       Passed"
else
    EXITCODE=$UNIT_TEST_RESULT
    echo "  Unit tests:       Failed"
fi

if [ $FUNC_TEST_RESULT -eq 0 ]; then
    echo "  Functional tests: Passed"
else
    EXITCODE=$FUNC_TEST_RESULT
    echo "  Functional tests: Failed"
fi

if [ $MONO_TEST_RESULT -eq -1 ]; then
    echo "  Mono tests:       Not Run"
elif [ $MONO_TEST_RESULT -eq 0 ]; then
    echo "  Mono tests:       Passed"
else
    EXITCODE=$MONO_TEST_RESULT
    echo "  Mono tests:       Failed"
fi

exit $EXITCODE
