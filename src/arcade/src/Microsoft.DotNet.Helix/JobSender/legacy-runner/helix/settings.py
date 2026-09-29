# Licensed to the .NET Foundation under one or more agreements.
# The .NET Foundation licenses this file to you under the MIT license.

"""Environment-backed settings used by the legacy script runner."""

import os


class WorkItemSettings:
    def __init__(self):
        self.correlation_id = os.environ.get("HELIX_CORRELATION_ID")
        self.correlation_payload_dir = os.environ.get("HELIX_CORRELATION_PAYLOAD")
        self.event_uri = os.environ.get("HELIX_EVENT_URI")
        self.log_root = os.environ.get("HELIX_LOG_ROOT")
        self.output_uri = os.environ.get("HELIX_RESULTS_CONTAINER_URI")
        self.output_read_token = os.environ.get("HELIX_RESULTS_CONTAINER_RSAS")
        self.output_write_token = os.environ.get("HELIX_RESULTS_CONTAINER_WSAS")
        self.workitem_friendly_name = os.environ.get("HELIX_WORKITEM_FRIENDLYNAME")
        self.workitem_id = os.environ.get("HELIX_WORKITEM_ID")
        self.workitem_payload_dir = os.environ.get("HELIX_WORKITEM_PAYLOAD")
        self.workitem_working_dir = os.environ.get("HELIX_WORKITEM_ROOT")

    def set(self, name, value):
        setattr(self, name, value)


def settings_from_env():
    return WorkItemSettings()
