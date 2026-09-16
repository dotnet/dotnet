"""Compile without MSBuild and probe initial named-mutex ownership on a supplied SDK."""

import argparse
import hashlib
import json
import os
import subprocess
import sys
from pathlib import Path


def single(paths, description):
    paths = list(paths)
    if len(paths) != 1:
        raise RuntimeError("Expected one {}: {}".format(description, paths))
    return paths[0]


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--sdk", type=Path, required=True)
    parser.add_argument("--output", type=Path, required=True)
    parser.add_argument("--seconds", type=int, default=90)
    args = parser.parse_args()
    if not 1 <= args.seconds <= 300:
        parser.error("--seconds must be between 1 and 300")
    sdk = args.sdk.resolve()
    output = args.output.resolve()
    output.mkdir(parents=True, exist_ok=True)
    host = sdk / ("dotnet.exe" if os.name == "nt" else "dotnet")
    compiler = single(sdk.glob("sdk/*/Roslyn/bincore/csc.dll"), "compiler")
    runtime = single((sdk / "shared" / "Microsoft.NETCore.App").iterdir(), "runtime")
    refs = single(sdk.glob("packs/Microsoft.NETCore.App.Ref/*/ref/net*"), "reference directory")
    source = Path(__file__).with_name("Program.cs").resolve()
    assembly = output / "MutexOwnershipProbe.dll"
    response = output / "compile.rsp"
    response.write_text("\n".join([
        "/nologo", "/target:exe", "/optimize+", "/nullable:enable", "/warnaserror+",
        '/out:"{}"'.format(assembly),
        *('/reference:"{}"'.format(p) for p in sorted(refs.glob("*.dll"))),
        '"{}"'.format(source),
    ]), encoding="utf-8")
    env = dict(os.environ, DOTNET_ROOT=str(sdk), DOTNET_MULTILEVEL_LOOKUP="0")
    provenance = {
        "sdk": str(sdk), "compiler": str(compiler), "runtime": runtime.name,
        "sdkBuildId": os.environ.get("MUTEX_SDK_BUILD_ID"),
        "sourceVersion": os.environ.get("BUILD_SOURCEVERSION"),
        "probeSourceSha256": hashlib.sha256(source.read_bytes()).hexdigest(),
        "coreLibSha256": hashlib.sha256((runtime / "System.Private.CoreLib.dll").read_bytes()).hexdigest(),
    }
    (output / "provenance.json").write_text(json.dumps(provenance, indent=2), encoding="utf-8")
    with (output / "compile.log").open("w", encoding="utf-8") as log:
        subprocess.run([str(host), "exec", str(compiler), "/noconfig", "@" + str(response)],
                       cwd=str(output), env=env, stdout=log, stderr=subprocess.STDOUT,
                       timeout=120, check=True)
    (output / "MutexOwnershipProbe.runtimeconfig.json").write_text(json.dumps({
        "runtimeOptions": {"framework": {"name": "Microsoft.NETCore.App", "version": runtime.name},
                           "rollForward": "Disable"}
    }), encoding="utf-8")
    results = []
    for initially_owned in (True, False):
        label = "initially-owned" if initially_owned else "explicit-wait"
        with (output / (label + ".log")).open("w", encoding="utf-8") as log:
            result = subprocess.run(
                [str(host), str(assembly), str(args.seconds), str(initially_owned)],
                cwd=str(output), env=env, stdout=log, stderr=subprocess.STDOUT,
                timeout=args.seconds + 100)
        text = (output / (label + ".log")).read_text(encoding="utf-8")
        print("{} (exit {}):\n{}".format(label, result.returncode, text), flush=True)
        results.append({"mode": label, "exitCode": result.returncode})
    (output / "results.json").write_text(json.dumps(results, indent=2), encoding="utf-8")
    return 0 if all(r["exitCode"] == 0 for r in results) else 1


if __name__ == "__main__":
    sys.exit(main())
