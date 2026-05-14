#!/bin/bash

###
### This script is used as a payload of Helix jobs that execute iOS/tvOS workloads through XHarness.
### This is the entrypoint of the job that goes on to spawn the real payload in user session with
### GUI rendering capabilities.
###

app=''
forwarded_args=''

while [[ $# -gt 0 ]]; do
    opt="$(echo "$1" | tr "[:upper:]" "[:lower:]")"
    case "$opt" in
      --app)
        app="$2"
        shift
        ;;
      *)
        forwarded_args="$forwarded_args $1"
        ;;
    esac
    shift
done

set -x

# It is important we call the script via `launchctl asuser` in order to be able to spawn
# the simulator which needs to run in a user session with GUI rendering capabilities.
# The problem with launchctl is that the spawned process won't share environment variables
# so we have to set them again.
export -p > envvars
chmod +x xharness-runner.apple.sh
uid=$(id -u)
username=$(id -un)
sudo launchctl asuser "$uid" sudo -u "$username" sh ./xharness-runner.apple.sh \
    $forwarded_args                                                            \
    --app "$HELIX_WORKITEM_ROOT/$app"                                          \
    --output-directory "$HELIX_WORKITEM_UPLOAD_ROOT"                           \

exit_code=$?

# For some of the issues such as Simulators get reeaally slow and they start failing to install apps,
# the only solution is to reboot the machine.
# We usually also ask the work item to be re-tried on a different machine
# Since we run the payload script using launchctl, env vars such as PYTHON_PATH are not set there and we have to do this part here
# We signal this by creating files
if [ -f "$HELIX_WORKITEM_ROOT/.retry" ]; then
    retry_message=$(cat "$HELIX_WORKITEM_ROOT/.retry" | tr -d "'\\\\")

    if [ -z "$retry_message" ]; then
        retry_message='Infrastructural problem reported by the user, requesting retry'
    fi

    "$HELIX_PYTHONPATH" -c "from helix.workitemutil import request_infra_retry; request_infra_retry('$retry_message')"
fi

if [ -f "$HELIX_WORKITEM_ROOT/.reboot" ]; then
    reboot_message=$(cat "$HELIX_WORKITEM_ROOT/.reboot" | tr -d "'\\\\")

    if [ -z "$reboot_message" ]; then
        reboot_message='Infrastructural problem reported by the user, requesting reboot'
    fi

    "$HELIX_PYTHONPATH" -c "from helix.workitemutil import request_reboot; request_reboot('$reboot_message')"
fi

exit $exit_code
