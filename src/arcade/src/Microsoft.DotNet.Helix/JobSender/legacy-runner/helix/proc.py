# Licensed to the .NET Foundation under one or more agreements.
# The .NET Foundation licenses this file to you under the MIT license.

"""Dependency-free process execution required by the legacy script runner."""

import os
import subprocess

import helix.logs


def run_and_log_output(args, cwd=None, env=None):
    # The child inherits the work item's environment, matching legacy
    # helix.proc. That environment includes HELIX_RESULTS_CONTAINER_* , so it
    # is tempting to strip those here, but doing so would not contain them and
    # would break compatibility:
    #
    #   * The Helix client exports them to the work item command itself, so the
    #     runner that calls this function already holds them. The runner and the
    #     command it launches are both work-item-supplied code at the same trust
    #     level, so removing them here crosses no privilege boundary.
    #   * Legacy payloads are allowed to use them to upload directly, and
    #     dropping them would silently lose those results.
    #
    # Narrowing what work items receive is a Helix client decision, applied to
    # the whole work item at once, not something this compatibility shim can do
    # correctly on its own.
    process_environment = os.environ.copy()
    if env is not None:
        process_environment.update(env)

    log = helix.logs.get_logger()
    log.info("Running: %s", " ".join(args))
    if cwd is not None:
        log.info("CWD: %s", cwd)

    process = subprocess.Popen(
        args,
        cwd=cwd,
        env=process_environment,
        stderr=subprocess.STDOUT,
        stdout=subprocess.PIPE,
        stdin=subprocess.PIPE,
        shell=False,
    )
    process.stdin.close()

    with process.stdout:
        for line in iter(process.stdout.readline, b""):
            log.info("Output: %s", line.rstrip(b"\n\r").decode("utf-8", errors="replace"))

    return_code = process.wait()
    log.info("Exit Code: %s", return_code)
    return return_code
