# Licensed to the .NET Foundation under one or more agreements.
# The .NET Foundation licenses this file to you under the MIT license.

"""Command-line adapter used by the legacy script runner."""

import getopt
import os
import sys
import uuid

import helix.logs
import helix.settings


log = helix.logs.get_logger()


def command_main(main_method, opt_long, args=None):
    if args is None:
        args = sys.argv[1:]

    optlist, args = getopt.gnu_getopt(
        args,
        "",
        opt_long + ["help", "config=", "setting="],
    )
    optdict = dict(optlist)

    if "--help" in optdict:
        import inspect
        log.info(inspect.getdoc(main_method))
        return None

    if "--config" in optdict:
        raise ValueError("--config is not supported by the compatibility runner")

    settings = helix.settings.settings_from_env()
    if settings.log_root:
        helix.logs.set_logfile(os.path.join(settings.log_root, uuid.uuid4().hex + ".log"))
        helix.logs.register_auto_upload(settings)

    for value in [value for name, value in optlist if name == "--setting"]:
        setting_name, setting_value = str(value).split("=", 1)
        settings.set(setting_name, setting_value)

    return main_method(settings, optlist, args)
