# Licensed to the .NET Foundation under one or more agreements.
# The .NET Foundation licenses this file to you under the MIT license.

"""File helpers required by the legacy script runner."""

import os
import shutil


def fix_path(path):
    if path.startswith("\\\\?\\"):
        return path[4:]
    return path


def ensure_directory_exists(path):
    if not os.path.exists(path):
        os.makedirs(path)


def copy_tree_to(source, target_path, overwrite=True):
    source = fix_path(source)
    target_path = fix_path(target_path)
    for directory, _, file_names in os.walk(str(source)):
        for file_name in file_names:
            source_path = os.path.join(directory, file_name)
            relative_path = os.path.relpath(source_path, source)
            destination = os.path.join(target_path, relative_path)
            ensure_directory_exists(os.path.dirname(destination))
            if overwrite or not os.path.exists(destination):
                shutil.copy2(source_path, destination)
