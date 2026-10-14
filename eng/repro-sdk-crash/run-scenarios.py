import argparse
import json
import os
import pathlib
import re
import shutil
import xml.etree.ElementTree as ET

from reproduce import execute


EXCLUSIONS = [
    "Category=MultiTFM",
    "Category=RequiresNet12RuntimeTargetingPack",
    "Category=RequiresNonTargetRidPackages",
    "Category=RequiresPortableAssets",
    "SkipIfBuild=SourceOnlyUnofficialBuild",
    "SkipIfBuild=SourceOnly",
]


def main():
    parser = argparse.ArgumentParser(description="Run the original scenario-test sources against a CI SDK.")
    parser.add_argument("--sdk", required=True, type=pathlib.Path)
    parser.add_argument("--source", required=True, type=pathlib.Path)
    parser.add_argument("--output", required=True, type=pathlib.Path)
    parser.add_argument("--source-commit", required=True)
    parser.add_argument("--repeats", type=int, default=3)
    parser.add_argument("--timeout", type=int, default=1200)
    args = parser.parse_args()
    if min(args.repeats, args.timeout) < 1:
        parser.error("Repeats and timeout must be positive")
    sdk = args.sdk.resolve(strict=True)
    source = args.source.resolve(strict=True)
    sdks = list((sdk / "sdk").glob("*"))
    runtimes = list((sdk / "shared/Microsoft.NETCore.App").glob("*"))
    if len(sdks) != 1 or len(runtimes) != 1:
        parser.error("Expected exactly one SDK and one runtime")
    config = source / "NuGet.config"
    if not config.is_file():
        parser.error(f"Missing original NuGet.config: {config}")
    output = args.output.resolve()
    output.mkdir(parents=True, exist_ok=False)
    for directory in ("dumps", "build", "bin"):
        (output / directory).mkdir()
    project = output / "build/ScenarioRepro.csproj"
    shutil.copyfile(pathlib.Path(__file__).with_name("ScenarioRepro.csproj"), project)
    environment = dict(os.environ)
    for key in ("DOTNET_DbgEnableMiniDump", "DOTNET_DbgMiniDumpType", "DOTNET_DbgMiniDumpName",
                "DOTNET_CreateDumpDiagnostics", "DOTNET_EnableCrashReport"):
        environment.pop(key, None)
    environment.update({
        "DOTNET_ROOT": str(sdk),
        "DOTNET_ROLL_FORWARD": "Major",
        "DOTNET_CLI_TELEMETRY_OPTOUT": "1",
        "DOTNET_SKIP_FIRST_TIME_EXPERIENCE": "1",
        "RestoreConfigFile": str(config),
        "RuntimeFrameworkVersion": runtimes[0].name,
        "COMPlus_DbgEnableMiniDump": "1",
        "COMPlus_DbgMiniDumpType": "4",
        "COMPlus_DbgMiniDumpName": str(output / "dumps/core.%e.%p.%t.dmp"),
        "COMPlus_CreateDumpDiagnostics": "1",
        "COMPlus_EnableCrashReport": "1",
    })
    metadata = {
        "source_commit": args.source_commit,
        "source": str(source),
        "sdk": sdks[0].name,
        "runtime": runtimes[0].name,
        "harness_target_framework": "net11.0",
        "xunit_version": "2.9.3",
        "exclusions": EXCLUSIONS,
        "repeats": args.repeats,
        "uid": os.getuid(),
        "home": environment.get("HOME"),
        "nuget_packages": environment.get("NUGET_PACKAGES"),
    }
    (output / "configuration.json").write_text(json.dumps(metadata, indent=2) + "\n")
    build = [
        str(sdk / "dotnet"), "build", str(project), "-c", "Release",
        "-o", str(output / "bin"),
        f"-p:ScenarioSourceRoot={source}",
        f"-p:SdkDirectory={sdks[0]}",
        f"-bl:{output / 'harness-build.{}.binlog'}",
    ]
    status = execute(build, project.parent, environment, output / "harness-build.log", args.timeout)
    print(f"Harness build exit: {status}; log: {output / 'harness-build.log'}", flush=True)
    if status != 0:
        raise RuntimeError("Scenario-test harness build failed; inspect harness-build.log and binlog")
    harness = output / "bin/Microsoft.DotNet.ScenarioTests.SdkTemplateTests.dll"
    failures = 0
    results = []
    for index in range(args.repeats):
        attempt = output / f"repeat-{index:03d}"
        projects = attempt / "projects"
        binlogs = attempt / "binlogs"
        projects.mkdir(parents=True)
        binlogs.mkdir()
        (projects / "global.json").write_text("{}\n")
        xml = attempt / "results.xml"
        command = [
            str(sdk / "dotnet"), "exec", "--fx-version", runtimes[0].name, str(harness),
            "--dotnet-root", str(sdk), "--sdk-version", sdks[0].name,
            "--test-root", str(projects), "--binlog-dir", str(binlogs),
            "--target-rid", "centos.10-x64", "--portable-rid", "linux-x64",
            "--no-cleanup", "--xml", str(xml),
        ]
        for exclusion in EXCLUSIONS:
            command.extend(["--no-traits", exclusion])
        status = execute(command, source, environment, attempt / "command.log", args.timeout)
        totals = {}
        if xml.is_file():
            assemblies = ET.parse(xml).getroot().findall("assembly")
            for name in ("total", "passed", "failed", "skipped", "errors"):
                totals[name] = sum(int(assembly.get(name, "0")) for assembly in assemblies)
        valid_results = totals.get("passed", 0) + totals.get("failed", 0) > 0
        dumps = list((output / "dumps").glob("*.dmp"))
        crash = bool(dumps) or status in (-11, -6, 139, 134) or bool(re.search(
            r"Exit code:\s*(?:139|134|-11|-6)\b", (attempt / "command.log").read_text()))
        failures += (status != 0 or not valid_results or crash
                     or totals.get("failed", 0) > 0 or totals.get("errors", 0) > 0)
        record = {"repeat": index, "exit": status, "tests": totals,
                  "crash_observed": crash, "dumps": [str(p) for p in dumps]}
        results.append(record)
        print(json.dumps(record), flush=True)
        summary = {"completed": len(results), "failures": failures,
                   "crash_observed": any(result["crash_observed"] for result in results),
                   "runs": results}
        (output / "summary.json").write_text(json.dumps(summary, indent=2) + "\n")
        if crash or status == "timeout":
            break
    return int(failures > 0)


if __name__ == "__main__":
    raise SystemExit(main())
