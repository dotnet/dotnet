#!/bin/bash

### This script is used as a payload of Helix jobs that execute Android workloads through XHarness on Linux systems.
###
### The purpose of this script is to time-constrain user commands (command.ps1) so that we have time at the end of the
### work item to process XHarness telemetry.

echo "XHarness Helix Job Wrapper calling '$@'"

set -x

app=''
command_timeout=20
timeout=''
package_name=''
expected_exit_code=0
device_output_path=''
instrumentation=''
output_directory=$HELIX_WORKITEM_UPLOAD_ROOT

while [[ $# -gt 0 ]]; do
    opt="$(echo "$1" | tr "[:upper:]" "[:lower:]")"
    case "$opt" in
      --app)
        app="$2"
        shift
        ;;
      --command_timeout)
        command_timeout="$2"
        shift
        ;;
      --timeout)
        timeout="$2"
        shift
        ;;
      --package_name)
        package_name="$2"
        shift
        ;;
      --expected_exit_code)
        expected_exit_code="$2"
        shift
        ;;
      --device_output_path)
        device_output_path="$2"
        shift
        ;;
      --instrumentation)
        instrumentation="$2"
        shift
        ;;
    esac
    shift
done

if [ -z "$app" ]; then
    die "App bundle path wasn't provided";
fi

if [ -z "$timeout" ]; then
    die "No timeout was provided";
fi

if [ -z "$output_directory" ]; then
    die "No output directory provided";
fi

# The xharness alias
function xharness() {
    dotnet exec "$XHARNESS_CLI_PATH" "$@"
}

function report_infrastructure_failure() {
    echo "Infrastructural problem reported by the user, requesting retry+reboot: $1"

    echo "$1" > "$HELIX_WORKITEM_ROOT/.retry"
    echo "$1" > "$HELIX_WORKITEM_ROOT/.reboot"
}

# Act out the actual commands (and time constrain them to create buffer for the end of this script)
source command.sh & PID=$! ; (sleep "$command_timeout" && kill -s 0 $PID > /dev/null 2>&1 && echo "ERROR: WORKLOAD TIMED OUT - Killing user command.." && kill $PID 2> /dev/null & ) ; wait $PID

exit $?
