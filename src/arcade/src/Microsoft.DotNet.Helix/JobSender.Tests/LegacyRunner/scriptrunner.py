#!/usr/bin/env py

# Licensed to the .NET Foundation under one or more agreements.
# The .NET Foundation licenses this file to you under the MIT license.
# See the LICENSE file in the project root for more information.

import os.path
import re

import helix.logs
import helix.proc
import helix.saferequests

from helix.cmdline import command_main
from helix.helixio import fix_path
from helix_test_execution import HelixTestExecution
from io import open

log = helix.logs.get_logger()


def main(args=None):
    def _main(settings, optlist, args):
        optdict = dict(optlist)
        log.info("BuildTools Helix Script Runner v0.1 starting")
        if '--args' in optdict:
            script_arguments = optdict['--args']
            log.info("Script Arguments:"+script_arguments)

        script_to_execute = optdict['--script']
        unpack_dir = fix_path(settings.workitem_payload_dir)
        execution_args = [os.path.join(unpack_dir, script_to_execute)] + args

        test_executor = HelixTestExecution(settings)

        return_code = helix.proc.run_and_log_output(
            execution_args,
            cwd=unpack_dir,
            env=None
        )

        results_location = os.path.join(settings.correlation_payload_dir, 'TestResults.zip')
        if not os.path.exists(results_location):
            for root, dirs, files in os.walk(settings.workitem_working_dir):
                for file_name in files:
                    if file_name == 'TestResults.zip':
                        results_location = os.path.join(root, file_name)

        test_executor.upload_file_to_storage(results_location, settings)

        return return_code

    return command_main(_main, ['script=', 'args='], args)


if __name__ == '__main__':
    import sys
    sys.exit(main())
