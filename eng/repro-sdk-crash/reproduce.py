import argparse
import concurrent.futures
import datetime
import json
import os
import pathlib
import platform
import resource
import shutil
import signal
import subprocess
import threading


COMMANDS = {
    "globaljson": ["new", "globaljson"],
    "console": ["new", "console"],
    "classlib": ["new", "classlib"],
    "mstest-fsharp": ["new", "mstest", "--language", "F#"],
}


def execute(command, directory, environment, log, timeout):
    with log.open("wb") as stream:
        process = subprocess.Popen(
            command, cwd=directory, env=environment,
            stdout=stream, stderr=subprocess.STDOUT, start_new_session=True,
        )
        try:
            return process.wait(timeout=timeout)
        except subprocess.TimeoutExpired:
            try:
                os.killpg(process.pid, signal.SIGKILL)
            except ProcessLookupError:
                pass
            process.wait()
            return "timeout"


def main():
    parser = argparse.ArgumentParser(
        description="Repeat prebuilt SDK commands with full crash dumps; no product build.")
    parser.add_argument("--sdk", required=True, type=pathlib.Path)
    parser.add_argument("--output", required=True, type=pathlib.Path)
    parser.add_argument("--nuget-config", type=pathlib.Path)
    parser.add_argument("--iterations", type=int, default=1000)
    parser.add_argument("--parallelism", type=int, default=8)
    parser.add_argument("--commands", default="globaljson")
    parser.add_argument("--locales", default="en-US,it-IT")
    parser.add_argument("--timeout", type=int, default=300)
    parser.add_argument("--dump-prefix", choices=("DOTNET", "COMPlus"), default="DOTNET")
    parser.add_argument("--verify-dumps", action="store_true",
                        help="Use prebuilt F# Interactive to FailFast before the real attempts.")
    args = parser.parse_args()
    if platform.system() != "Linux" or platform.machine() not in ("x86_64", "amd64"):
        parser.error("Requires native Linux x64; do not validate capture under ARM emulation")
    if min(args.iterations, args.parallelism, args.timeout) < 1:
        parser.error("Iterations, parallelism, and timeout must be positive")
    if args.nuget_config and not args.nuget_config.is_file():
        parser.error(f"NuGet configuration not found: {args.nuget_config}")
    modes = args.commands.split(",")
    locales = args.locales.split(",")
    if (any(mode not in COMMANDS for mode in modes)
            or len(set(modes)) != len(modes) or not all(locales)):
        parser.error(f"Commands must be drawn from {','.join(COMMANDS)}; locales must be nonempty")
    sdk = args.sdk.resolve(strict=True)
    sdk_directories = list((sdk / "sdk").glob("*"))
    runtime_directories = list((sdk / "shared/Microsoft.NETCore.App").glob("*"))
    if len(sdk_directories) != 1 or len(runtime_directories) != 1:
        parser.error("Expected exactly one SDK and one runtime in the extracted CI archive")
    output = args.output.resolve()
    output.mkdir(parents=True, exist_ok=False)
    for name in ("dumps", "attempts", "home", "packages", "capture-check"):
        (output / name).mkdir()
    debugging_sdk = output / "debugging-sdk"
    debugging_sdk.mkdir()
    shutil.copy2(sdk / "dotnet", debugging_sdk / "dotnet")
    shutil.copytree(
        runtime_directories[0],
        debugging_sdk / "shared/Microsoft.NETCore.App" / runtime_directories[0].name,
    )
    resource.setrlimit(resource.RLIMIT_NOFILE, (65536, 65536))
    environment = {
        key: value for key, value in os.environ.items()
        if not key.upper().startswith(("DOTNET_", "MSBUILD", "COMPLUS_"))
    }
    environment.update({
        "HOME": str(output / "home"),
        "PATH": str(sdk) + os.pathsep + os.environ["PATH"],
        "NUGET_PACKAGES": str(output / "packages"),
        "RuntimeFrameworkVersion": runtime_directories[0].name,
        "ImportDirectoryBuildProps": "false",
        "ImportDirectoryBuildTargets": "false",
        "ImportDirectoryPackagesProps": "false",
        "DOTNET_ROOT": str(sdk),
        "DOTNET_ROLL_FORWARD": "Major",
        "DOTNET_CLI_TELEMETRY_OPTOUT": "1",
        "DOTNET_SKIP_FIRST_TIME_EXPERIENCE": "1",
    })
    environment.update({
        f"{args.dump_prefix}_{key}": value for key, value in {
            "DbgEnableMiniDump": "1",
            "DbgMiniDumpType": "4",
            "DbgMiniDumpName": str(output / "dumps/core.%e.%p.%t.dmp"),
            "CreateDumpDiagnostics": "1",
            "EnableCrashReport": "1",
        }.items()
    })
    if args.nuget_config:
        config = output / "NuGet.config"
        shutil.copyfile(args.nuget_config, config)
        environment["RestoreConfigFile"] = str(config)
    metadata = {
        "utc": datetime.datetime.now(datetime.timezone.utc).isoformat(),
        "uname": list(platform.uname()),
        "sdk": sdk_directories[0].name,
        "runtime": runtime_directories[0].name,
        "iterations_per_command": args.iterations,
        "parallelism": args.parallelism,
        "commands": modes,
        "locales": locales,
        "dump_prefix": args.dump_prefix,
    }
    for name, path in (("cpuinfo", "/proc/cpuinfo"), ("core_pattern", "/proc/sys/kernel/core_pattern"),
                       ("os-release", "/etc/os-release")):
        metadata[name] = pathlib.Path(path).read_text()
    (output / "environment.json").write_text(json.dumps(metadata, indent=2) + "\n")
    if args.verify_dumps:
        check = output / "capture-check"
        check_environment = dict(environment)
        check_environment[f"{args.dump_prefix}_DbgMiniDumpName"] = str(check / "smoke.%p.%t.dmp")
        status = execute(
            [str(sdk / "dotnet"), str(sdk_directories[0] / "FSharp/fsi.dll"),
             "--exec", str(pathlib.Path(__file__).with_name("verify-dumps.fsx"))],
            check, check_environment, check / "command.log", args.timeout,
        )
        dumps = [path for path in check.glob("smoke.*.dmp") if path.stat().st_size > 0]
        reports = list(check.glob("smoke.*.dmp.crashreport.json"))
        if status not in (-signal.SIGABRT, 128 + signal.SIGABRT) or not dumps or not reports:
            raise RuntimeError(
                f"Dump capture check failed (exit {status}); inspect {check / 'command.log'}")
        print("Intentional FailFast produced a dump and crash report.", flush=True)
    stop = threading.Event()

    def attempt(index, mode):
        if stop.is_set():
            return None
        directory = output / "attempts" / f"{mode}-{index:06d}"
        directory.mkdir()
        culture = locales[index % len(locales)]
        env = dict(environment, DOTNET_CLI_UI_LANGUAGE=culture)
        command = [str(sdk / "dotnet"), *COMMANDS[mode]]
        if mode == "globaljson":
            command.extend(["--sdk-version", sdk_directories[0].name])
        else:
            (directory / "global.json").write_text(
                json.dumps({"sdk": {"version": sdk_directories[0].name}}))
            command.extend(["--name", "SdkCrashRepro", "--output", str(directory)])
        status = execute(command, directory, env, directory / "command.log", args.timeout)
        if status in (-signal.SIGSEGV, -signal.SIGABRT, 139, 134):
            stop.set()
        restore_ok = None
        if mode != "globaljson":
            assets_path = directory / "obj/project.assets.json"
            restore_ok = False
            if assets_path.is_file():
                assets = json.loads(assets_path.read_text())
                restore_ok = bool(assets.get("targets")) and not any(
                    entry.get("level", "").lower() == "error"
                    for entry in assets.get("logs", [])
                )
        return {"index": index, "mode": mode, "culture": culture, "exit": status,
                "restore_ok": restore_ok, "command": command, "directory": str(directory)}

    completed = failures = 0
    with (output / "results.jsonl").open("w") as records:
        with concurrent.futures.ThreadPoolExecutor(max_workers=args.parallelism) as executor:
            futures = [
                executor.submit(attempt, index, mode)
                for index in range(args.iterations) for mode in modes
            ]
            for future in concurrent.futures.as_completed(futures):
                record = future.result()
                if record is None:
                    continue
                completed += 1
                failures += record["exit"] != 0 or record["restore_ok"] is False
                line = json.dumps(record)
                records.write(line + "\n")
                records.flush()
                print(line, flush=True)
    summary = {"completed": completed, "failures": failures, "crash_observed": stop.is_set(),
               "dumps": [str(path) for path in (output / "dumps").glob("*")]}
    (output / "summary.json").write_text(json.dumps(summary, indent=2) + "\n")
    print(json.dumps(summary), flush=True)
    return int(failures > 0)


if __name__ == "__main__":
    raise SystemExit(main())
