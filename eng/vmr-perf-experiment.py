#!/usr/bin/env python3
"""Private pipeline experiment: measure builds and preserve stacks before timeout."""

import argparse
import datetime
import hashlib
import json
import os
from pathlib import Path
import platform
import re
import shutil
import signal
import subprocess
import sys
import time
import urllib.request
import zipfile


STACK_VERSION = "10.0.745401"
STACK_SHA256 = "f24a1b60aaf35b5c18bbd819f29206d35d152a81bdf62633e4510471ea579e5c"


def write_json(path, value):
    path.write_text(json.dumps(value, indent=2), encoding="utf-8")


def setup(tools):
    processes = process_table()
    print(f"Process diagnostics available: {len(processes)} processes", flush=True)
    tools.mkdir(parents=True, exist_ok=True)
    url = ("https://api.nuget.org/v3-flatcontainer/dotnet-stack/"
           f"{STACK_VERSION}/dotnet-stack.{STACK_VERSION}.nupkg")
    archive = tools / "dotnet-stack.nupkg"
    urllib.request.urlretrieve(url, archive)
    digest = hashlib.sha256(archive.read_bytes()).hexdigest()
    if digest != STACK_SHA256:
        raise RuntimeError(f"Diagnostic package hash mismatch: {digest}")
    with zipfile.ZipFile(archive) as package:
        package.extractall(tools / "stack")
    assemblies = list((tools / "stack").glob("tools/*/any/dotnet-stack.dll"))
    if len(assemblies) != 1:
        raise RuntimeError(f"Expected one diagnostic entry point, found {assemblies}")
    write_json(tools / "tool.json", {
        "url": url, "version": STACK_VERSION,
        "sha256": digest,
        "assembly": str(assemblies[0]),
    })
    print(f"Prepared dotnet-stack {STACK_VERSION}", flush=True)


def process_table():
    if sys.platform.startswith("linux"):
        processes = []
        for directory in Path("/proc").iterdir():
            if not directory.name.isdigit():
                continue
            try:
                raw = (directory / "stat").read_text()
                name_end = raw.rfind(")")
                fields = raw[name_end + 2:].split()
                command = os.fsdecode((directory / "cmdline").read_bytes().replace(b"\0", b" ")).strip()
                processes.append({
                    "pid": int(directory.name), "ppid": int(fields[1]),
                    "name": raw[raw.find("(") + 1:name_end],
                    "identity": fields[19], "cpu": int(fields[11]) + int(fields[12]),
                    "rss": fields[21], "state": fields[0], "command": command,
                })
            except (FileNotFoundError, ProcessLookupError):
                continue
            except PermissionError as error:
                print(f"Cannot inspect process {directory.name}: {error}", flush=True)
        return processes
    if os.name == "nt":
        command = (
            "Get-CimInstance Win32_Process | Select-Object ProcessId,ParentProcessId,"
            "Name,CommandLine,@{n='CreationDate';e={$_.CreationDate.ToUniversalTime().ToString('o')}},"
            "KernelModeTime,UserModeTime,WorkingSetSize "
            "| ConvertTo-Json -Compress"
        )
        result = subprocess.run(
            ["powershell.exe", "-NoProfile", "-Command", command],
            stdout=subprocess.PIPE, stderr=subprocess.PIPE,
            universal_newlines=True, check=True, timeout=30)
        return [{
            "pid": p["ProcessId"], "ppid": p["ParentProcessId"],
            "name": p["Name"], "command": p["CommandLine"] or "",
            "identity": p["CreationDate"],
            "cpu": int(p["KernelModeTime"]) + int(p["UserModeTime"]),
            "rss": p["WorkingSetSize"],
        } for p in json.loads(result.stdout)]
    result = subprocess.run(
        ["ps", "-axo", "pid=,ppid=,lstart=,time=,rss=,stat=,command="],
        stdout=subprocess.PIPE, stderr=subprocess.PIPE,
        universal_newlines=True, check=True, timeout=30)
    processes = []
    for line in result.stdout.splitlines():
        fields = line.split(None, 10)
        if len(fields) == 11:
            processes.append({
                "pid": int(fields[0]), "ppid": int(fields[1]),
                "identity": " ".join(fields[2:7]), "cpu": fields[7],
                "rss": fields[8], "state": fields[9], "command": fields[10],
                "name": Path(fields[10].split()[0]).name,
            })
    return processes


def descendants(table, root, known):
    selected = {root}
    selected.update(p["pid"] for p in table if known.get(p["pid"]) == p["identity"])
    while True:
        added = {p["pid"] for p in table if p["ppid"] in selected}
        if added <= selected:
            break
        selected |= added
    return [p for p in table if p["pid"] in selected]


def snapshot(output, root, known, label):
    table = descendants(process_table(), root, known)
    known.update((p["pid"], p["identity"]) for p in table)
    write_json(output / f"processes-{label}.json", {
        "utc": datetime.datetime.now(datetime.timezone.utc).isoformat(),
        "processes": table,
    })
    return table


def diagnostic_command(command, destination, timeout=30, env=None):
    with destination.open("w", encoding="utf-8") as log:
        try:
            result = subprocess.run(command, stdout=log, stderr=subprocess.STDOUT,
                                    timeout=timeout, env=env)
            log.write(f"\nDiagnostic exit code: {result.returncode}\n")
        except subprocess.TimeoutExpired:
            log.write(f"\nDiagnostic timed out after {timeout} seconds.\n")
        except OSError as error:
            log.write(f"\nDiagnostic could not start: {error}\n")


def collect_stacks(args, table, round_number, deadline):
    tool = json.loads((args.tools / "tool.json").read_text(encoding="utf-8"))
    hosts = list(args.sources.glob(".dotnet/dotnet*"))
    if args.sdk:
        hosts = list(args.sdk.glob("dotnet*")) + hosts
    host = next((p for p in hosts if p.name in ("dotnet", "dotnet.exe")), None)
    if host is None:
        (args.output / f"stack-error-{round_number}.txt").write_text(
            "No bootstrapped dotnet host found.\n", encoding="utf-8")
    env = dict(os.environ, DOTNET_ROLL_FORWARD="Major", DOTNET_ROLL_FORWARD_TO_PRERELEASE="1")
    for process in table:
        if time.monotonic() >= deadline:
            (args.output / "diagnostic-budget-exhausted.txt").write_text(
                "Stopped stack collection to leave time for log publication.\n", encoding="utf-8")
            break
        if not re.search(r"dotnet|msbuild|taskhost|vbc|csc|crossgen", process["name"], re.I):
            continue
        pid = process["pid"]
        # Read only the diagnostic socket location, never persist the process environment.
        target_env = env.copy()
        prefix = []
        if sys.platform.startswith("linux"):
            environ = Path(f"/proc/{pid}/environ")
            try:
                values = environ.read_bytes().split(b"\0")
                for value in values:
                    if value.startswith(b"TMPDIR="):
                        target_env["TMPDIR"] = os.fsdecode(value.split(b"=", 1)[1])
            except (FileNotFoundError, PermissionError) as error:
                (args.output / f"socket-{pid}-{round_number}.txt").write_text(
                    str(error), encoding="utf-8")
            if Path(f"/proc/{pid}").exists() and Path(f"/proc/{pid}").stat().st_uid != os.getuid():
                prefix = ["sudo", "-n", "env", "DOTNET_ROLL_FORWARD=Major",
                          "DOTNET_ROLL_FORWARD_TO_PRERELEASE=1",
                          f"TMPDIR={target_env.get('TMPDIR', '/tmp')}"]
        if host is not None:
            diagnostic_command(
                prefix + [str(host), tool["assembly"], "report", "--process-id", str(pid)],
                args.output / f"stacks-{pid}-{round_number}.txt",
                timeout=min(30, max(1, deadline - time.monotonic())), env=target_env)
        remaining = deadline - time.monotonic()
        if remaining <= 0:
            break
        if os.name == "nt":
            debugger = Path(os.environ.get("ProgramFiles(x86)", r"C:\Program Files (x86)"))
            debugger /= r"Windows Kits\10\Debuggers\x64\cdb.exe"
            if debugger.exists():
                diagnostic_command(
                    [str(debugger), "-pv", "-p", str(pid), "-c",
                     ".loadby sos clr;~*e !clrstack;~* kb;q"],
                    args.output / f"native-stacks-{pid}-{round_number}.txt", timeout=min(30, remaining))
            else:
                (args.output / f"framework-stacks-{pid}-{round_number}.txt").write_text(
                    f"Framework debugger not installed: {debugger}\n", encoding="utf-8")
        elif sys.platform == "darwin":
            diagnostic_command(
                ["sample", str(pid), "2", "-file", str(args.output / f"native-{pid}-{round_number}.txt")],
                args.output / f"sample-{pid}-{round_number}.txt", timeout=min(10, remaining))
        elif sys.platform.startswith("linux"):
            debugger = shutil.which("gdb")
            if debugger:
                diagnostic_command(
                    ["sudo", "-n", debugger, "-nx", "-batch", "-p", str(pid),
                     "-ex", "set pagination off", "-ex", "thread apply all bt",
                     "-ex", "detach"],
                    args.output / f"native-stacks-{pid}-{round_number}.txt",
                    timeout=min(30, remaining))
            else:
                (args.output / f"native-stacks-{pid}-{round_number}.txt").write_text(
                    "gdb not installed; managed stacks and kernel wait channels collected.\n", encoding="utf-8")
            for task in Path(f"/proc/{pid}/task").glob("*"):
                if time.monotonic() >= deadline:
                    break
                try:
                    text = (task / "wchan").read_text()
                    (args.output / f"wait-{pid}-{task.name}-{round_number}.txt").write_text(text)
                except (FileNotFoundError, PermissionError) as error:
                    print(f"Cannot read wait channel {task}: {error}", flush=True)


def stop_build(process, known, output):
    table = descendants(process_table(), process.pid, known)
    # Target only this build's observed process identities, not names or unrelated jobs.
    if os.name == "nt":
        if process.poll() is None:
            try:
                process.send_signal(signal.CTRL_BREAK_EVENT)
            except OSError as error:
                (output / "interrupt-error.txt").write_text(str(error), encoding="utf-8")
    else:
        for p in reversed(table):
            try:
                os.kill(p["pid"], signal.SIGINT)
            except ProcessLookupError:
                pass
            except PermissionError:
                subprocess.run(["sudo", "-n", "kill", "-INT", str(p["pid"])], check=True)
    time.sleep(20)
    for p in reversed(table):
        live = {p["pid"]: p for p in process_table()}
        if p["pid"] not in live or live[p["pid"]]["identity"] != p["identity"]:
            continue
        if os.name == "nt":
            diagnostic_command(
                ["powershell.exe", "-NoProfile", "-Command",
                 f"$p = Get-Process -Id {p['pid']} -ErrorAction Stop; "
                 "$handle = $p.Handle; "
                 f"$expected = [datetime]::Parse('{p['identity']}').ToUniversalTime().Ticks; "
                 "if ([Math]::Abs($p.StartTime.ToUniversalTime().Ticks - $expected) -gt 10) "
                 "{ throw 'Process identity changed; refusing to kill' }; $p.Kill()"],
                output / f"stop-{p['pid']}.txt")
        else:
            try:
                os.kill(p["pid"], signal.SIGKILL)
            except ProcessLookupError:
                pass
            except PermissionError:
                subprocess.run(["sudo", "-n", "kill", "-KILL", str(p["pid"])], check=True)


def run(args):
    args.output.mkdir(parents=True, exist_ok=True)
    # Fail before starting a build if this image cannot provide process diagnostics.
    process_table()
    started = time.monotonic()
    metadata = {
        "startUtc": datetime.datetime.now(datetime.timezone.utc).isoformat(),
        "sourceVersion": os.environ.get("BUILD_SOURCEVERSION"),
        "buildId": os.environ.get("BUILD_BUILDID"),
        "job": os.environ.get("AGENT_JOBNAME"),
        "mt": os.environ.get("EXPERIMENTMT"), "nr": os.environ.get("EXPERIMENTNR"),
        "cutoffSeconds": args.cutoff, "platform": platform.platform(),
        "machine": platform.machine(), "cpuCount": os.cpu_count(),
        "command": args.command,
        "diagnosticTool": json.loads((args.tools / "tool.json").read_text(encoding="utf-8")),
    }
    write_json(args.output / "measurement.json", metadata)
    creation = {"creationflags": subprocess.CREATE_NEW_PROCESS_GROUP} if os.name == "nt" else {}
    process = subprocess.Popen(args.command, cwd=args.sources, shell=os.name == "nt", **creation)
    known = {}
    cutoff = False
    next_capture = args.cutoff
    captures = 0
    try:
        while process.poll() is None:
            elapsed = time.monotonic() - started
            table = snapshot(args.output, process.pid, known, f"{int(elapsed):05}")
            if elapsed >= next_capture:
                captures += 1
                metadata["diagnosticCaptureCount"] = captures
                print("Experiment diagnostic checkpoint reached; this is NOT a declaration of a hang.", flush=True)
                deadline = time.monotonic() + 600
                collect_stacks(args, table, f"{captures}-1", deadline - 300)
                time.sleep(15)
                table = snapshot(args.output, process.pid, known, f"checkpoint-{captures}-second")
                collect_stacks(args, table, f"{captures}-2", deadline)
                write_json(args.output / "measurement.json", metadata)
                # Upload while the build is alive; a later job cancellation cannot erase this snapshot.
                archive = args.output.parent / f"checkpoint-{captures}.zip"
                with zipfile.ZipFile(str(archive), "w", zipfile.ZIP_DEFLATED) as package:
                    for path in args.output.iterdir():
                        if path.is_file():
                            package.write(str(path), path.name)
                job = os.environ.get("AGENT_JOBNAME")
                if job:
                    print(f"##vso[artifact.upload containerfolder=experiment/{job};artifactname={job}_Experiment;]{archive}", flush=True)
                if args.stop_at_checkpoint:
                    cutoff = True
                    metadata["cutoffObservedSeconds"] = elapsed
                    stop_build(process, known, args.output)
                    break
                next_capture += 3600
            try:
                process.wait(timeout=min(60, max(0.1, next_capture - (time.monotonic() - started))))
            except subprocess.TimeoutExpired:
                pass
    finally:
        if process.poll() is None:
            print("Stopping owned build processes before watchdog exit.", flush=True)
            stop_build(process, known, args.output)
        metadata.update({
            "finishUtc": datetime.datetime.now(datetime.timezone.utc).isoformat(),
            "elapsedSecondsIncludingDiagnostics": time.monotonic() - started,
            "buildElapsedSeconds": metadata.get("cutoffObservedSeconds", time.monotonic() - started),
            "cutoffReached": cutoff, "exitCode": process.poll(),
        })
        write_json(args.output / "measurement.json", metadata)
    return 124 if cutoff else process.returncode


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("action", choices=("setup", "run"))
    parser.add_argument("--tools", type=Path, required=True)
    parser.add_argument("--sources", type=Path)
    parser.add_argument("--output", type=Path)
    parser.add_argument("--sdk", type=Path)
    parser.add_argument("--cutoff", type=float, default=9000)
    parser.add_argument("--stop-at-checkpoint", action="store_true")
    args, command = parser.parse_known_args()
    args.command = command[1:] if command[:1] == ["--"] else command
    if args.action == "setup":
        setup(args.tools)
        return 0
    if not args.sources or not args.output or not args.command or args.cutoff <= 0:
        parser.error("run requires sources, output, a positive cutoff and a command after --")
    return run(args)


if __name__ == "__main__":
    sys.exit(main())
