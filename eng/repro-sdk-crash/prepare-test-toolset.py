import argparse
import pathlib
import re
import xml.etree.ElementTree as ET
import zipfile


def prepare(packages_directory, source):
    packages = [
        path for path in packages_directory.rglob("*.nupkg")
        if path.name.lower().startswith("microsoft.dotnet.arcade.sdk.")
        and not path.name.lower().endswith(".symbols.nupkg")
    ]
    if len(packages) != 1:
        raise ValueError(f"Expected one prebuilt Arcade SDK package, found {len(packages)}")
    with zipfile.ZipFile(packages[0]) as package:
        specifications = [name for name in package.namelist() if name.endswith(".nuspec")]
        if len(specifications) != 1:
            raise ValueError("Expected one Arcade package specification")
        version = ET.fromstring(package.read(specifications[0])).findtext("{*}metadata/{*}version")
        if not version or not re.fullmatch(r"[A-Za-z0-9.+-]+", version):
            raise ValueError(f"Invalid Arcade package version: {version}")
        package.getinfo("toolset/Build.proj")
        destination = source / "artifacts/toolset" / version
        for member in package.infolist():
            if not member.filename.startswith("toolset/") or member.is_dir():
                continue
            relative = pathlib.PurePosixPath(member.filename).relative_to("toolset")
            if relative.is_absolute() or ".." in relative.parts:
                raise ValueError(f"Unsafe Arcade toolset path: {member.filename}")
            target = destination / relative
            target.parent.mkdir(parents=True, exist_ok=True)
            target.write_bytes(package.read(member))
    print(f"Prepared the original Arcade {version} toolset from {packages[0]}", flush=True)


def main():
    parser = argparse.ArgumentParser(description="Prepare the scenario build scripts from prebuilt Arcade.")
    parser.add_argument("--packages", required=True, type=pathlib.Path)
    parser.add_argument("--source", required=True, type=pathlib.Path)
    args = parser.parse_args()
    prepare(args.packages, args.source)


if __name__ == "__main__":
    main()
