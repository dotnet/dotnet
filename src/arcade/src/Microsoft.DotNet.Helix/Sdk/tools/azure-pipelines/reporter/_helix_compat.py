# Licensed to the .NET Foundation under one or more agreements.
# The .NET Foundation licenses this file to you under the MIT license.

"""Minimal stand-ins for the helix.public types used by the reporter.

The reporter has historically depended on the `helix-scripts` Python package
being installed on the test machine (typically via the helix-prep venv at
`/etc/helix-prep/venv`). Newer Helix client implementations (notably the AOT
client) do not ship that package by default, so the import fails and the
reporter cannot run.

This module provides API-compatible substitutes for the small surface the
reporter actually consumes:

    - TestResult           (data container)
    - TestResultAttachment (data container)
    - AzureDevOpsReportingParameters (data container)
    - JsonReporter         (writes a portable JSON results file)

The constructors mirror the signatures used by the format parsers and by
run.py so they can be swapped in transparently when the real classes are
not importable.

The JSON file written by JsonReporter is the long-term wire format: it is
language-neutral (no pickle / no class identity coupling) and can be
consumed by either the legacy Python Helix client or a native (e.g. C#)
client without spawning Python.
"""

import json
import logging
import os
from typing import Iterable, List, Optional


# Schema version embedded in every emitted JSON file. Bump when the on-disk
# layout changes in a backwards-incompatible way; consumers should refuse
# files with an unknown major version.
SCHEMA_VERSION = 1

# Filename written next to (not replacing) the legacy `__test_report.json`
# pickle file. The pickle file's name is historical and misleading; this
# new file is the real JSON.
JSON_RESULTS_FILENAME = "__test_report_v2.json"


def _results_dir():
    """Return the directory the JSON results file is written into.

    Mirrors helix.test_reporting.packing_test_reporter._file_name() closely
    enough that a Helix client looking in the same place will find both
    files. Falls back to cwd if HELIX_WORKITEM_ROOT is unset (which would
    only happen outside a real Helix work item).
    """
    return os.environ.get("HELIX_WORKITEM_ROOT") or os.getcwd()


def json_results_path():
    """Absolute path to the JSON results file for the current work item."""
    return os.path.join(_results_dir(), JSON_RESULTS_FILENAME)


class TestResultAttachment(object):
    """API-compatible stand-in for helix.public.TestResultAttachment."""

    __test__ = False  # pytest: do not collect

    def __init__(self, name, text):
        self._name = name
        self._text = text

    @property
    def name(self):
        return self._name

    @property
    def text(self):
        return self._text

    def to_dict(self):
        return {"name": self._name, "text": self._text}


class TestResult(object):
    """API-compatible stand-in for helix.public.TestResult."""

    __test__ = False  # pytest: do not collect

    def __init__(self,
                 name,
                 kind,
                 type_name,
                 method,
                 duration,
                 result,
                 exception_type,
                 failure_message,
                 stack_trace,
                 skip_reason,
                 attachments):
        self._name = name
        self._kind = kind
        self._type = type_name
        self._method = method
        self._duration_seconds = duration
        self._result = result
        self._exception_type = exception_type
        self._failure_message = failure_message
        self._stack_trace = stack_trace
        self._skip_reason = skip_reason
        self._attachments = attachments
        self.ignored = False

    @property
    def name(self):
        return self._name

    @property
    def kind(self):
        return self._kind

    @property
    def type(self):
        return self._type

    @property
    def method(self):
        return self._method

    @property
    def duration_seconds(self):
        return self._duration_seconds

    @property
    def result(self):
        return self._result

    @property
    def exception_type(self):
        return self._exception_type

    @property
    def failure_message(self):
        return self._failure_message

    @property
    def stack_trace(self):
        return self._stack_trace

    @property
    def skip_reason(self):
        return self._skip_reason

    @property
    def attachments(self):
        return self._attachments

    def to_dict(self):
        return {
            "name": self._name,
            "kind": self._kind,
            "type": self._type,
            "method": self._method,
            "duration_seconds": self._duration_seconds,
            "result": self._result,
            "exception_type": self._exception_type,
            "failure_message": self._failure_message,
            "stack_trace": self._stack_trace,
            "skip_reason": self._skip_reason,
            "ignored": self.ignored,
            "attachments": [_attachment_to_dict(a) for a in (self._attachments or [])],
        }


class AzureDevOpsReportingParameters(object):
    """API-compatible stand-in for helix.public.AzureDevOpsReportingParameters."""

    def __init__(self, collection_uri, team_project, test_run_id, access_token):
        self.collection_uri = collection_uri
        self.team_project = team_project
        self.test_run_id = test_run_id
        self.access_token = access_token

    def to_dict(self, include_token=True):
        d = {
            "collection_uri": self.collection_uri,
            "team_project": self.team_project,
            "test_run_id": self.test_run_id,
        }
        if include_token:
            d["access_token"] = self.access_token
        return d


def _attachment_to_dict(a):
    """Serialize either our TestResultAttachment or the helix-scripts one."""
    if hasattr(a, "to_dict"):
        return a.to_dict()
    return {"name": getattr(a, "name", None), "text": getattr(a, "text", None)}


def _result_to_dict(r):
    """Serialize either our TestResult or the helix-scripts one."""
    if hasattr(r, "to_dict"):
        return r.to_dict()
    return {
        "name": getattr(r, "name", None),
        "kind": getattr(r, "kind", None),
        "type": getattr(r, "type", None),
        "method": getattr(r, "method", None),
        "duration_seconds": getattr(r, "duration_seconds", None),
        "result": getattr(r, "result", None),
        "exception_type": getattr(r, "exception_type", None),
        "failure_message": getattr(r, "failure_message", None),
        "stack_trace": getattr(r, "stack_trace", None),
        "skip_reason": getattr(r, "skip_reason", None),
        "ignored": getattr(r, "ignored", False),
        "attachments": [_attachment_to_dict(a) for a in (getattr(r, "attachments", None) or [])],
    }


class JsonReporter(object):
    """Writes a portable, schema-versioned JSON file with the test results.

    Always runs alongside the legacy pickle-based PackingTestReporter (when
    available) so existing consumers continue to work unchanged. The file
    layout is:

        {
          "schema_version": 1,
          "azdo": { "collection_uri", "team_project", "test_run_id", "access_token" },
          "results": [ { TestResult fields }, ... ]
        }
    """

    __test__ = False

    def __init__(self, azdo_parameters, log=None):
        self._azdo = azdo_parameters
        self._log = log or logging.getLogger(__name__)

    def report_results(self, results):
        results = [r for r in (results or []) if r is not None]
        path = json_results_path()
        payload = {
            "schema_version": SCHEMA_VERSION,
            "azdo": _azdo_to_dict(self._azdo),
            "results": [_result_to_dict(r) for r in results],
        }
        try:
            os.makedirs(os.path.dirname(path), exist_ok=True)
        except OSError:
            # Directory already exists or cannot be created; let open() raise.
            pass
        self._log.info("Writing %d test results to '%s' (JSON v%d)",
                       len(results), path, SCHEMA_VERSION)
        with open(path, "w", encoding="utf-8") as f:
            json.dump(payload, f, ensure_ascii=False)
        try:
            size = os.path.getsize(path)
            self._log.info("Wrote %d bytes to '%s'", size, path)
        except OSError:
            pass


def _azdo_to_dict(p):
    if hasattr(p, "to_dict"):
        return p.to_dict()
    return {
        "collection_uri": getattr(p, "collection_uri", None),
        "team_project": getattr(p, "team_project", None),
        "test_run_id": getattr(p, "test_run_id", None),
        "access_token": getattr(p, "access_token", None),
    }
