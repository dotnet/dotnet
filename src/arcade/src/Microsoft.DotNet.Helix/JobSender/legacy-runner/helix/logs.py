# Licensed to the .NET Foundation under one or more agreements.
# The .NET Foundation licenses this file to you under the MIT license.

"""Small helix.logs compatibility surface for legacy work-item runners.

This directory intentionally has no __init__.py. On legacy Helix machines,
Python therefore prefers the installed regular ``helix`` package. When that
package is absent, this directory becomes a namespace package and supplies the
compatibility modules.
"""

import logging
import os
import sys
import time
import uuid


_FORMAT = "%(asctime)s %(levelname)-7s %(module)s(%(lineno)d) %(funcName)s %(message)s"
_HANDLER_MARKER = "_helix_legacy_runner_compat"


class _UtcFormatter(logging.Formatter):
    """Formats timestamps as UTC, matching the legacy helix.logs formatter."""

    def formatTime(self, record, datefmt=None):
        created = time.gmtime(record.created)
        return "{}.{:03d}Z".format(
            time.strftime("%Y-%m-%dT%H:%M:%S", created), int(record.msecs)
        )


def _add_handler(handler):
    handler.setLevel(logging.INFO)
    handler.setFormatter(_UtcFormatter(_FORMAT))
    setattr(handler, _HANDLER_MARKER, True)
    logging.getLogger().addHandler(handler)


def _file_handler(path):
    # Staged logs are uploaded by the Helix client and read back as UTF-8, so
    # never fall back to the machine's locale encoding (cp1252 on many Windows
    # images), which would mangle or drop non-ASCII output.
    return logging.FileHandler(path, encoding="utf-8")


def _configure():
    root = logging.getLogger()
    root.setLevel(logging.INFO)
    if any(getattr(handler, _HANDLER_MARKER, False) for handler in root.handlers):
        return

    _add_handler(logging.StreamHandler(sys.stdout))

    upload_root = os.environ.get("HELIX_WORKITEM_UPLOAD_ROOT")
    if upload_root:
        log_directory = os.path.join(upload_root, ".helix-logs")
        os.makedirs(log_directory, exist_ok=True)
        _add_handler(_file_handler(os.path.join(log_directory, "scriptrunner.log")))


def get_logger(*args, **kwargs):
    _configure()
    return logging.getLogger(*args, **kwargs)


def set_logfile(path):
    _configure()

    directory = os.path.dirname(path)
    try:
        if directory:
            os.makedirs(directory, exist_ok=True)
        _add_handler(_file_handler(path))
    except OSError:
        # An unwritable log destination must not stop the work item from
        # running or from staging its results.
        logging.getLogger().warning("Unable to log to %s", path, exc_info=True)


def register_auto_upload(settings, credential=None):
    # Compatibility logs are created directly under HELIX_WORKITEM_UPLOAD_ROOT.
    # The Helix client uploads that directory after the command exits.
    _configure()


class SelfUploadingLogFile:
    """Compatibility context that stages its log for client-owned upload."""

    def __init__(self, settings=None, credential=None, path=None):
        if path is None:
            upload_root = os.environ.get("HELIX_WORKITEM_UPLOAD_ROOT")
            if not upload_root:
                raise ValueError("HELIX_WORKITEM_UPLOAD_ROOT is required")
            path = os.path.join(upload_root, ".helix-logs", str(uuid.uuid4()) + ".log")

        self._path = path
        self._handler = None

    def __enter__(self):
        directory = os.path.dirname(self._path)
        if directory:
            os.makedirs(directory, exist_ok=True)
        self._handler = _file_handler(self._path)
        _add_handler(self._handler)
        return self

    def __exit__(self, exc_type, exc_value, traceback):
        if exc_type is not None:
            logging.getLogger().exception(
                "Unhandled error",
                exc_info=(exc_type, exc_value, traceback),
            )

        if self._handler is not None:
            logging.getLogger().removeHandler(self._handler)
            self._handler.close()

        return False


_configure()
