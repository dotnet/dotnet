import argparse
import datetime
import hashlib
import json
import os
import pathlib
import re
import shlex
import shutil
import xml.etree.ElementTree as ET
import zipfile

from reproduce import execute


EXCLUSIONS = [
    "Category=MultiTFM",
    "Category=RequiresNet12RuntimeTargetingPack",
    "Category=RequiresNonTargetRidPackages",
    "Category=RequiresPortableAssets",
    "SkipIfBuild=SourceOnlyUnofficialBuild",
    "SkipIfBuild=SourceOnly",
]


def extract_harness(package, expected_sha512, source_commit, destination):
    digest = hashlib.sha512()
    with package.open("rb") as stream:
        for chunk in iter(lambda: stream.read(1024 * 1024), b""):
            digest.update(chunk)
    actual = digest.hexdigest()
    if actual != expected_sha512.lower():
        raise ValueError(f"Scenario harness SHA512 mismatch: expected {expected_sha512}, got {actual}")
    with zipfile.ZipFile(package) as archive:
        manifest = ET.fromstring(archive.read("Microsoft.DotNet.ScenarioTests.SdkTemplateTests.nuspec"))
        namespace = {"n": "http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd"}
        repository = manifest.find("n:metadata/n:repository", namespace)
        if repository is None or repository.get("commit") != source_commit:
            raise ValueError("Scenario harness package was not built from the requested source commit")
        destination.mkdir(parents=True, exist_ok=False)
        prefix = "tools/net11.0/any/"
        for name in archive.namelist():
            if not name.startswith(prefix) or name.endswith("/"):
                continue
            relative = pathlib.PurePosixPath(name).relative_to(prefix)
            if relative.is_absolute() or ".." in relative.parts:
                raise ValueError(f"Invalid harness package path: {name}")
            target = destination.joinpath(*relative.parts)
            target.parent.mkdir(parents=True, exist_ok=True)
            target.write_bytes(archive.read(name))
    harness = destination / "Microsoft.DotNet.ScenarioTests.SdkTemplateTests.dll"
    if not harness.is_file():
        raise RuntimeError("Verified package did not contain the original scenario-test assembly")
    print(f"Verified original scenario harness SHA512: {actual}", flush=True)
    return harness


def main():
    parser = argparse.ArgumentParser(description="Replay the original prebuilt scenario harness through MSBuild.")
    parser.add_argument("--sdk", required=True, type=pathlib.Path)
    parser.add_argument("--source", required=True, type=pathlib.Path)
    parser.add_argument("--output", required=True, type=pathlib.Path)
    parser.add_argument("--source-commit", required=True)
    parser.add_argument("--harness-package", required=True, type=pathlib.Path)
    parser.add_argument("--harness-sha512", required=True)
    parser.add_argument("--repeats", type=int, default=3)
    parser.add_argument("--timeout", type=int, default=1200)
    args = parser.parse_args()
    if min(args.repeats, args.timeout) < 1:
        parser.error("Repeats and timeout must be positive")
    if not re.fullmatch(r"[0-9a-fA-F]{128}", args.harness_sha512):
        parser.error("--harness-sha512 must contain 128 hexadecimal digits")
    sdk = args.sdk.resolve(strict=True)
    source = args.source.resolve(strict=True)
    package = args.harness_package.resolve(strict=True)
    sdks = list((sdk / "sdk").glob("*"))
    runtimes = list((sdk / "shared/Microsoft.NETCore.App").glob("*"))
    if len(sdks) != 1 or len(runtimes) != 1:
        parser.error("Expected exactly one SDK and one runtime")
    config = source / "NuGet.config"
    if not config.is_file():
        parser.error(f"Missing original NuGet.config: {config}")
    project_directory = source / "src/Microsoft.DotNet.ScenarioTests.SdkTemplateTests"
    if not project_directory.is_dir():
        parser.error(f"Missing original scenario project directory: {project_directory}")
    repo = source.parents[1]
    output = args.output.resolve()
    output.mkdir(parents=True, exist_ok=False)
    (output / "dumps").mkdir()
    shutil.copyfile(package, output / package.name)
    harness = extract_harness(
        package, args.harness_sha512, args.source_commit,
        source / "artifacts/bin/Microsoft.DotNet.ScenarioTests.SdkTemplateTests/Release/net11.0")
    launcher = pathlib.Path(__file__).with_name("ReplayScenarios.proj")
    msbuild = sdks[0] / "MSBuild.dll"
    if not msbuild.is_file():
        raise RuntimeError(f"Missing MSBuild from the prebuilt SDK: {msbuild}")
    environment = dict(os.environ)
    for key in ("DOTNET_DbgEnableMiniDump", "DOTNET_DbgMiniDumpType", "DOTNET_DbgMiniDumpName",
                "DOTNET_CreateDumpDiagnostics", "DOTNET_EnableCrashReport"):
        environment.pop(key, None)
    environment.update({
        "DOTNET_ROOT": str(sdk),
        "DOTNET_ROLL_FORWARD": "Major",
        "DOTNET_CLI_TELEMETRY_OPTOUT": "1",
        "DOTNET_SKIP_FIRST_TIME_EXPERIENCE": "1",
        "NUGET_PACKAGES": str(source / "artifacts/.packages"),
        "NuGetAudit": "false",
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
        "harness": str(harness),
        "harness_package_sha512": args.harness_sha512.lower(),
        "launcher": "MSBuild Exec",
        "msbuild": str(msbuild),
        "working_directory": str(project_directory),
        "xunit_version": "2.9.3",
        "exclusions": EXCLUSIONS,
        "repeats": args.repeats,
        "uid": os.getuid(),
        "home": environment.get("HOME"),
        "nuget_packages": environment.get("NUGET_PACKAGES"),
    }
    (output / "configuration.json").write_text(json.dumps(metadata, indent=2) + "\n")
    failures = 0
    results = []
    for index in range(args.repeats):
        attempt = output / f"repeat-{index:03d}"
        attempt.mkdir()
        projects = source / "artifacts/obj/generatedtests"
        binlogs = repo / "artifacts/log/Release/scenario-tests"
        projects.mkdir(parents=True)
        binlogs.mkdir(parents=True)
        (projects / "global.json").write_text("{}\n")
        xml = repo / "artifacts/TestResults/Release/scenario-tests" / (
            datetime.datetime.now().strftime("%Y-%m-%d_%H_%M_%S") + f"-{index:03d}.xml")
        xml.parent.mkdir(parents=True, exist_ok=True)
        arguments = [
            "--dotnet-root", str(sdk),
            "--test-root", str(projects), "--binlog-dir", str(binlogs),
            "--sdk-version", sdks[0].name, "--xml", str(xml),
            "--target-rid", "centos.10-x64", "--portable-rid", "linux-x64",
            "--no-cleanup",
        ]
        for exclusion in EXCLUSIONS:
            arguments.extend(["--no-traits", exclusion])
        command = [
            str(sdk / "dotnet"), "exec", "--fx-version", runtimes[0].name, str(msbuild),
            str(launcher), "-t:Test", "-m", "-nr:true", "-v:minimal", "-nologo",
            f"-bl:{attempt / 'launcher.{}.binlog'}",
            f"-p:TestDotNetRoot={sdk}",
            f"-p:TestFxVersion={runtimes[0].name}",
            f"-p:TestAssembly={harness}",
            f"-p:ScenarioProjectDirectory={project_directory}",
            f"-p:TestRestoreConfigFile={config}",
            f"-p:TestArgs={shlex.join(arguments)}",
        ]
        (attempt / "launch-command.json").write_text(json.dumps(command, indent=2) + "\n")
        print(f"Starting original scenario suite repetition {index} through MSBuild Exec", flush=True)
        status = execute(command, project_directory, environment, attempt / "command.log", args.timeout)
        if xml.is_file():
            shutil.copyfile(xml, attempt / "results.xml")
        shutil.move(str(projects), attempt / "projects")
        shutil.move(str(binlogs), attempt / "binlogs")
        totals = {}
        if xml.is_file():
            assemblies = ET.parse(xml).getroot().findall("assembly")
            for name in ("total", "passed", "failed", "skipped", "errors"):
                totals[name] = sum(int(assembly.get(name, "0")) for assembly in assemblies)
        valid_results = totals.get("passed", 0) + totals.get("failed", 0) > 0
        dumps = list((output / "dumps").glob("*.dmp"))
        crash = bool(dumps) or status in (-11, -6, 139, 134) or bool(re.search(
            r"(?:Exit code:\s*|exited with code\s+)(?:139|134|-11|-6)\b",
            (attempt / "command.log").read_text()))
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
