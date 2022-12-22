#!/bin/bash

###
### This script is used as a payload of Helix jobs that execute iOS/tvOS workloads through XHarness.
### It is executed via `launchctl asuser` in order to be able to spawn the simulator which needs to
### run in a user session with GUI rendering capabilities.
###

app=''
target=''
timeout=''
launch_timeout=''
command_timeout=20
xcode_version=''
app_arguments=''
expected_exit_code=0
includes_test_runner=false
reset_simulator=false

# Ignore shellcheck lint warning about unused variables (they can be used in the sourced script)
# shellcheck disable=SC2034
while [[ $# -gt 0 ]]; do
    opt="$(echo "$1" | tr "[:upper:]" "[:lower:]")"
    case "$opt" in
      --app)
        app="$2"
        shift
        ;;
      --target)
        target="$2"
        shift
        ;;
      --timeout)
        timeout="$2"
        shift
        ;;
      --command-timeout)
        command_timeout="$2"
        shift
        ;;
      --launch-timeout)
        launch_timeout="$2"
        shift
        ;;
      --xcode-version)
        xcode_version="$2"
        shift
        ;;
      --app-arguments)
        app_arguments="$2"
        shift
        ;;
      --expected-exit-code)
        expected_exit_code="$2"
        shift
        ;;
      --includes-test-runner)
        includes_test_runner=true
        ;;
      --reset-simulator)
        reset_simulator=true
        ;;
    esac
    shift
done

function die ()
{
    echo "$1" 1>&2
    exit 1
}

function sign ()
{
    echo "Signing $1"

    provisioning_profile="$1/embedded.mobileprovision"
    if [ ! -f "$provisioning_profile" ]; then
        echo "No embedded provisioning profile found at $provisioning_profile! Failed to sign the app!"
        exit 21
    fi

    # Unlock the keychain with certs
    keychain_name='signing-certs.keychain-db'
    keychain_password=$(cat ~/.config/keychain)

    security list-keychains | grep "$keychain_name"
    result=$?
    if [ $result != 0 ]; then
        echo "Keychain '$keychain_name' was not found"
        exit 22
    fi

    security find-identity -vp codesigning "$keychain_name" | grep " 0 valid identities found"
    result=$?
    if [ $result == 0 ]; then
        echo "No valid signing identities found in the keychain"
        exit 23
    fi

    security unlock-keychain -p "$keychain_password" "$keychain_name"

    # Generate entitlements file
    security cms -D -i "$provisioning_profile" > provision.plist
    /usr/libexec/PlistBuddy -x -c 'Print :Entitlements' provision.plist > entitlements.plist

    # Sign the app
    /usr/bin/codesign -v --force --sign "Apple Development" --keychain "$keychain_name" --entitlements entitlements.plist "$1"
}

if [ -z "$app" ]; then
    die "App bundle path wasn't provided";
fi

if [ -z "$target" ]; then
    die "No target was provided";
fi

if [ -z "$xcode_version" ]; then
    xcode_path="$(dirname "$(dirname "$(xcode-select -p)")")"
else
    xcode_path="/Applications/Xcode_${xcode_version}.app"

    if [ ! -d "$xcode_path" ]; then
      xcode_path="/Applications/Xcode${xcode_version/./}.app"
    fi
fi

if [ ! -d "$xcode_path" ]; then
    echo "WARNING - Xcode not found at $xcode_path"
fi

# First we need to revive env variables since they were erased by launchctl
# This file already has the expressions in the `export name=value` format
# shellcheck disable=SC1091
. ./envvars

output_directory=$HELIX_WORKITEM_UPLOAD_ROOT

# Signing
if [ "$target" == 'ios-device' ] || [ "$target" == 'tvos-device' ]; then
    if [ -d "$app" ]; then
        sign "$app"
    else
        echo 'Device target detected but app not found, skipping signing..'
    fi
elif [[ "$target" =~ "simulator" ]]; then
    # Start the simulator if it is not running already
    export SIMULATOR_APP="$xcode_path/Contents/Developer/Applications/Simulator.app"
    open -a "$SIMULATOR_APP"
fi

# The xharness alias
function xharness() {
    dotnet exec "$XHARNESS_CLI_PATH" "$@"
}

function report_infrastructure_failure() {
    echo "Infrastructural problem reported by the user, requesting retry+reboot: $1"

    echo "$1" > "$HELIX_WORKITEM_ROOT/.retry"

    if [[ "$2" -ne "--no-reboot" ]]; then
        echo "$1" > "$HELIX_WORKITEM_ROOT/.reboot"
    fi
}

# Used to grep sys logs later in case of crashes
start_time="$(date '+%Y-%m-%d %H:%M:%S')"

# Act out the actual commands (and time constrain them to create buffer for the end of this script)
# shellcheck disable=SC1091
source command.sh & PID=$! ; (sleep "$command_timeout" && kill -s 0 $PID > /dev/null 2>&1 && echo "ERROR: WORKLOAD TIMED OUT - Killing user command.." && kill $PID 2> /dev/null & ) ; wait $PID
exit_code=$?

# In case of issues, include the syslog (last 2 MB from the time this work item has been running)
if [ $exit_code -ne 0 ]; then
    sudo log show --style syslog --start "$start_time" --end "$(date '+%Y-%m-%d %H:%M:%S')" | tail -c 2097152 > "$output_directory/macos.system.log"
fi

echo "Removing empty log files:"
find "$output_directory" -name "*.log" -maxdepth 1 -size 0 -print -delete

# Rename test result XML so that AzDO reporter recognizes it
test_results=$(ls "$output_directory"/xunit-*.xml)
if [ -f "$test_results" ]; then
    echo "Found test results in $test_results. Renaming to testResults.xml to prepare for Helix upload"

    # Prepare test results for Helix to pick up
    mv "$test_results" "$output_directory/testResults.xml"
fi

exit $exit_code
