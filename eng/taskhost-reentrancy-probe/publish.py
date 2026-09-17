#!/usr/bin/env python3
"""Publish only explicit diagnostic evidence, never private CLI/package/process-file directories."""

import argparse
import json
from pathlib import Path
import shutil

from run import PAYLOAD, sha256, within


ROOT_FILES = (
    "bootstrap.log", "stage-clock.jsonl", "result.json", "diagnostic-tool.json",
    "sdk-version-file.json", "sdk-commands.json", "sdk-version.txt", "sdk-info.txt",
    "sdk-sha256.json", "source-sha256.json", "probe-assembly-sha256.json",
)


def publish(source, destination):
    source, destination = source.resolve(), destination.resolve()
    if within(source, destination) or within(destination, source):
        raise ValueError("Private work and publication directories must be separate")
    destination.mkdir(parents=True, exist_ok=True)
    if any(destination.iterdir()):
        raise ValueError("Publication directory must be empty")
    files = [source / name for name in ROOT_FILES]
    files += [path for path in source.glob("*.binlog") if path.stem.isdigit()]
    files += [source / "harness" / name for name in PAYLOAD]
    for name in ("compile", "normal", "mt-1", "mt-2"):
        directory = source / name
        if directory.is_symlink():
            raise ValueError("Unexpected diagnostic-directory symlink: " + name)
        files += list(directory.rglob("*"))
    copied = []
    for path in files:
        if path.is_symlink():
            raise ValueError("Unexpected diagnostic-file symlink: " + str(path))
        if not path.is_file():
            continue
        relative = path.relative_to(source)
        target = destination / relative
        target.parent.mkdir(parents=True, exist_ok=True)
        shutil.copyfile(str(path), str(target))
        copied.append({"path": relative.as_posix(), "bytes": target.stat().st_size,
                       "sha256": sha256(target)})
    manifest = {
        "private_source_exists": source.is_dir(),
        "excluded": ["cli-home", "packages", "process-files", "harness/bin", "harness/obj"],
        "files": copied,
    }
    (destination / "publication-manifest.json").write_text(
        json.dumps(manifest, indent=2) + "\n", encoding="utf-8")
    print(json.dumps({"published_files": len(copied), "private_source_exists": source.is_dir()}))


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--source", required=True, type=Path)
    parser.add_argument("--destination", required=True, type=Path)
    arguments = parser.parse_args()
    publish(arguments.source, arguments.destination)
