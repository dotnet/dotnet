# Licensed to the .NET Foundation under one or more agreements.
# The .NET Foundation licenses this file to you under the MIT license.

"""Compatibility implementation used by legacy AppCompat script runners."""

import os

import helix.azure_storage
import helix.logs
from helix.helixio import copy_tree_to, ensure_directory_exists, fix_path


log = helix.logs.get_logger()


class HelixTestExecution:
    def __init__(self, settings):
        self.settings = settings
        self.test_drop = fix_path(settings.workitem_working_dir)
        self.workitem_payload = fix_path(settings.workitem_payload_dir)
        self.test_location = os.path.join(self.test_drop, "execution")
        ensure_directory_exists(self.test_location)
        log.info(
            "Copying execution payload files from %s to %s",
            self.workitem_payload,
            self.test_location,
        )
        copy_tree_to(self.workitem_payload, self.test_location)

    def upload_file_to_storage(self, file_path, settings):
        upload_client = helix.azure_storage.get_upload_client(settings, None)
        return upload_client.upload(
            file_path,
            os.path.basename(file_path),
            "application/octet-stream",
        )
