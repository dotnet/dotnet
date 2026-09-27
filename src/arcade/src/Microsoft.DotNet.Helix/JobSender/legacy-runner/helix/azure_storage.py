# Licensed to the .NET Foundation under one or more agreements.
# The .NET Foundation licenses this file to you under the MIT license.

"""Client-owned upload compatibility for legacy work-item runners."""

import os
import shutil

import helix.logs


log = helix.logs.get_logger()

_COPY_BUFFER_SIZE = 1024 * 1024


def _write_chunk(output, chunk):
    # Legacy payloads may hand us either a text or a binary stream.
    if isinstance(chunk, str):
        chunk = chunk.encode("utf-8")
    output.write(chunk)


def _destination(name):
    upload_root = os.environ.get("HELIX_WORKITEM_UPLOAD_ROOT")
    if not upload_root:
        raise ValueError("HELIX_WORKITEM_UPLOAD_ROOT is required")

    upload_root = os.path.abspath(upload_root)
    normalized_name = name.replace("\\", "/").lstrip("/")
    destination = os.path.abspath(os.path.join(upload_root, *normalized_name.split("/")))

    # commonpath raises ValueError when the paths cannot be compared at all,
    # which on Windows happens when a name carries a drive prefix and
    # os.path.join resets to that drive. Such a name is invalid here too, so
    # report it as traversal instead of leaking a different error.
    try:
        contained = os.path.commonpath([upload_root, destination]) == upload_root
    except ValueError:
        contained = False

    if not contained:
        raise ValueError("Upload name must remain under HELIX_WORKITEM_UPLOAD_ROOT")

    os.makedirs(os.path.dirname(destination), exist_ok=True)
    return destination


class UploadClient:
    """Stages files for the Helix client instead of uploading directly."""

    def upload(self, file, name, content_type="application/octet-stream"):
        destination = _destination(name)
        temporary = destination + ".tmp"

        if isinstance(file, str):
            shutil.copyfile(file, temporary)
        elif hasattr(file, "read"):
            with open(temporary, "wb") as output:
                while True:
                    chunk = file.read(_COPY_BUFFER_SIZE)
                    if not chunk:
                        break
                    _write_chunk(output, chunk)
        else:
            with open(temporary, "wb") as output:
                for chunk in file:
                    _write_chunk(output, chunk)

        os.replace(temporary, destination)
        log.info("Staged %s as %s for Helix client upload", repr(file), name)
        return destination


def get_upload_client(settings=None, credentials=None, event_client=None):
    return UploadClient()
