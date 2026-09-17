#!/usr/bin/env python3
"""Bounded Linux correctness experiment, not a build/performance benchmark (Python 3.6+)."""

import argparse
import datetime
import hashlib
import importlib.util
import json
import os
from pathlib import Path
import platform
import re
import shutil
import signal
import socket
import subprocess
import sys
import time
import zipfile


HERE = Path(__file__).resolve().parent
REPO = HERE.parent.parent
SDK_VERSION = "11.0.100-rc.1.26420.103"
MSBUILD_VERSION = "18.11.0-1.26420.103+1d599674e"
PRODUCT_COMMIT = "1d599674e31ad86ca9e6e1dad7055b531e4d58e4"
TIMEOUT = 120
PAYLOAD = (
    "ProbeTasks.cs", "SnapshotLogger.cs", "PacketObserverTask.cs", "ProbeTasks.csproj", "Common.props",
    "Root.proj", "WrapperA.proj", "WrapperB.proj", "P.proj", "ChildA.proj",
    "ChildB.proj", "Directory.Build.props", "Directory.Build.targets",
    "global.json", "NuGet.Config",
)


def write_json(path, value):
    path.write_text(json.dumps(value, indent=2) + "\n", encoding="utf-8")


def sha256(path):
    digest = hashlib.sha256()
    with path.open("rb") as stream:
        for block in iter(lambda: stream.read(1024 * 1024), b""):
            digest.update(block)
    return digest.hexdigest()


def utc():
    return datetime.datetime.now(datetime.timezone.utc).isoformat()


def within(path, parent):
    return path == parent or parent in path.parents


class Clock:
    def __init__(self, output):
        self.output = output
        self.start = time.monotonic()

    def event(self, stage, **details):
        record = dict(details, stage=stage, utc=utc(),
                      supervisor_seconds=round(time.monotonic() - self.start, 3))
        with (self.output / "stage-clock.jsonl").open("a", encoding="utf-8") as log:
            log.write(json.dumps(record) + "\n")
        print(json.dumps(record), flush=True)


def load_helpers():
    spec = importlib.util.spec_from_file_location(
        "vmr_perf_experiment", str(HERE.parent / "vmr-perf-experiment.py"))
    module = importlib.util.module_from_spec(spec)
    previous = sys.dont_write_bytecode
    try:
        sys.dont_write_bytecode = True
        spec.loader.exec_module(module)
    finally:
        sys.dont_write_bytecode = previous
    return module


def private_environment(sdk, output):
    # Do not pass CI tokens/credentials to diagnostic MSBuild loggers. No environment dump.
    env = {key: os.environ[key] for key in
           ("PATH", "LANG", "LANGUAGE", "LC_ALL", "LC_CTYPE", "TZ") if key in os.environ}
    for name in ("cli-home", "packages", "process-files"):
        (output / name).mkdir(exist_ok=True)
    env.update({
        "HOME": str(output / "cli-home"),
        "DOTNET_ROOT": str(sdk), "DOTNET_HOST_PATH": str(sdk / "dotnet"),
        "DOTNET_CLI_HOME": str(output / "cli-home"),
        "DOTNET_CLI_TELEMETRY_OPTOUT": "1", "DOTNET_SKIP_FIRST_TIME_EXPERIENCE": "1",
        "DOTNET_NOLOGO": "1", "DOTNET_MULTILEVEL_LOOKUP": "0",
        "MSBUILDDISABLENODEREUSE": "1", "NUGET_PACKAGES": str(output / "packages"),
        "TMPDIR": str(output / "process-files"), "TMP": str(output / "process-files"),
        "TEMP": str(output / "process-files"),
    })
    return env


def prepare_tool(helpers, tools, output, clock):
    manifest = tools / "tool.json"
    archive = tools / "dotnet-stack.nupkg"
    missing = not manifest.is_file() or not archive.is_file()
    if not missing:
        prepared = json.loads(manifest.read_text(encoding="utf-8"))
        missing = not Path(prepared["assembly"]).is_file()
    if missing:
        clock.event("diagnostic-tool-missing", action="prepare pinned dotnet-stack")
        previous_timeout = socket.getdefaulttimeout()
        try:
            socket.setdefaulttimeout(30)
            helpers.setup(tools)
        finally:
            socket.setdefaulttimeout(previous_timeout)
    tool = json.loads(manifest.read_text(encoding="utf-8"))
    if (tool["version"] != helpers.STACK_VERSION or
            tool["sha256"] != helpers.STACK_SHA256 or
            sha256(archive) != helpers.STACK_SHA256):
        raise RuntimeError("Prepared diagnostic tool does not match the pinned package")
    assembly = Path(tool["assembly"]).resolve()
    if not within(assembly, tools / "stack"):
        raise RuntimeError("Diagnostic assembly is not inside the explicit tools directory")
    if not assembly.is_file():
        raise RuntimeError("Pinned tool setup did not produce its diagnostic assembly")
    member = assembly.relative_to(tools / "stack").as_posix()
    with zipfile.ZipFile(str(archive)) as package:
        expected = hashlib.sha256(package.read(member)).hexdigest()
    if sha256(assembly) != expected:
        raise RuntimeError("Extracted diagnostic assembly differs from the pinned package")
    tool.update(assembly=str(assembly), assembly_sha256=expected)
    write_json(output / "diagnostic-tool.json", tool)
    clock.event("diagnostic-tool-ready", version=tool["version"], sha256=tool["sha256"])
    return assembly


def stage_payload(output):
    harness = output / "harness"
    harness.mkdir()
    for name in PAYLOAD:
        shutil.copyfile(str(HERE / name), str(harness / name))
    sources = [HERE / name for name in PAYLOAD] + [
        HERE / "run.py", HERE.parent / "vmr-perf-experiment.py",
        REPO / "global.json", REPO / "eng" / "common" / "tools.sh",
        REPO / "eng" / "pipelines" / "pr.yml",
        REPO / "eng" / "pipelines" / "templates" / "stages" / "taskhost-reentrancy-probe.yml",
    ]
    write_json(output / "source-sha256.json",
               {str(path.relative_to(REPO)): sha256(path) for path in sources})
    return harness


def validate_sdk(sdk, harness, output, env, clock):
    root_pin = json.loads((REPO / "global.json").read_text(encoding="utf-8"))
    pin = json.loads((harness / "global.json").read_text(encoding="utf-8"))
    if (root_pin["tools"]["dotnet"] != SDK_VERSION or
            root_pin["sdk"]["version"] != SDK_VERSION or
            pin["sdk"]["version"] != SDK_VERSION or pin["sdk"]["rollForward"] != "disable"):
        raise RuntimeError("Repository/payload SDK pin changed; refusing a substitute engine")
    sdk_dir = sdk / "sdk" / SDK_VERSION
    version_file = sdk_dir / ".version"
    version_lines = version_file.read_text(encoding="utf-8").splitlines()
    write_json(output / "sdk-version-file.json", {"path": str(version_file), "lines": version_lines})
    if len(version_lines) < 3 or version_lines[:2] != [PRODUCT_COMMIT, SDK_VERSION]:
        raise RuntimeError("SDK .version does not match the required product commit and version")
    if version_lines[2] != "linux-x64":
        raise RuntimeError("Expected the Linux x64 SDK, not a Windows or different-architecture SDK")
    commands = []
    for option, name in (("--version", "sdk-version.txt"), ("--info", "sdk-info.txt")):
        command = [str(sdk / "dotnet"), option]
        commands.append(command)
        write_json(output / "sdk-commands.json", commands)
        with (output / name).open("w", encoding="utf-8") as log:
            result = subprocess.run(command, cwd=str(harness), env=env, stdout=log,
                                    stderr=subprocess.STDOUT, timeout=30)
        if result.returncode:
            raise RuntimeError("SDK identity command failed: " + option)
    actual = (output / "sdk-version.txt").read_text(encoding="utf-8").strip()
    if actual != SDK_VERSION:
        raise RuntimeError("The selected SDK is not the required exact SDK: " + actual)
    if MSBUILD_VERSION not in (output / "sdk-info.txt").read_text(encoding="utf-8"):
        raise RuntimeError("SDK --info does not report the required exact MSBuild version")
    relevant = [sdk_dir / name for name in (
        ".version", "MSBuild.dll", "Microsoft.Build.dll", "Microsoft.Build.Framework.dll",
        "Microsoft.Build.Utilities.Core.dll", "Microsoft.Build.Tasks.Core.dll")]
    write_json(output / "sdk-sha256.json",
               {str(path.relative_to(sdk)): sha256(path) for path in relevant})
    clock.event("sdk-verified", sdk=actual, msbuild=MSBUILD_VERSION, product_commit=PRODUCT_COMMIT)


class OwnedTree:
    """Wrap shared procfs helpers without their unconditional, potentially reused root seed."""

    def __init__(self, helpers, process):
        self.helpers = helpers
        self.process = process
        self.known = {}
        self.history = {}
        table = helpers.process_table()
        root = next((p for p in table if p["pid"] == process.pid), None)
        if root is None:
            raise RuntimeError("Launched root missing from procfs before its first wait")
        self.known[root["pid"]] = root["identity"]
        self.root_identity = root["identity"]
        self.remember(self.select(table))

    def select(self, table):
        root = self.process.pid
        if not any(p["pid"] == root and p["identity"] == self.root_identity for p in table):
            root = -1
        return self.helpers.descendants(table, root, self.known)

    def remember(self, selected):
        for process in selected:
            self.known[process["pid"]] = process["identity"]
            self.history[(process["pid"], process["identity"])] = process
        return selected

    def refresh(self):
        return self.remember(self.select(self.helpers.process_table()))

    def snapshot(self, directory, label):
        table = self.refresh()
        write_json(directory / ("processes-" + label + ".json"),
                   {"utc": utc(), "processes": table})
        return table

    def signal(self, process, signum, actions):
        current = next((p for p in self.helpers.process_table() if p["pid"] == process["pid"]), None)
        if current is None or current["identity"] != process["identity"] or current["state"] == "Z":
            return
        try:
            os.kill(current["pid"], signum)
            actions.append({"pid": current["pid"], "identity": current["identity"], "signal": signum})
        except ProcessLookupError:
            pass
        except OSError as error:
            actions.append({"pid": current["pid"], "identity": current["identity"],
                            "signal": signum, "error": str(error)})

    def cleanup(self, directory):
        actions = []
        previous_handlers = {sig: signal.signal(sig, signal.SIG_IGN)
                             for sig in (signal.SIGINT, signal.SIGTERM)}
        try:
            # No process-group or name-based signals: include only identity-checked owned PIDs.
            for signum, grace in ((signal.SIGINT, 3), (signal.SIGTERM, 2), (signal.SIGKILL, 3)):
                deadline = time.monotonic() + grace
                signaled = set()
                while time.monotonic() < deadline:
                    table = [p for p in self.refresh() if p["state"] != "Z"]
                    for process in table:
                        identity = (process["pid"], process["identity"])
                        if identity not in signaled:
                            self.signal(process, signum, actions)
                            signaled.add(identity)
                    self.process.poll()
                    if not any(p["state"] != "Z" for p in self.refresh()):
                        break
                    time.sleep(0.1)
                if not any(p["state"] != "Z" for p in self.refresh()):
                    break
            remaining = [p for p in self.snapshot(directory, "after-cleanup") if p["state"] != "Z"]
            self.process.poll()
            write_json(directory / "cleanup.json", {"signals": actions, "remaining": remaining})
            return not remaining
        finally:
            write_json(directory / "process-history.json", list(self.history.values()))
            for sig, previous in previous_handlers.items():
                signal.signal(sig, previous)


def capture_stacks(helpers, tree, directory, label, sdk, assembly, env, deadline, clock):
    table = tree.snapshot(directory, label)
    start = time.monotonic()
    clock.event("stack-capture-start", run=directory.name, label=label)
    stack_env = dict(env, DOTNET_ROLL_FORWARD="Major", DOTNET_ROLL_FORWARD_TO_PRERELEASE="1")
    for process in table:
        remaining = deadline - time.monotonic()
        if remaining <= 0:
            break
        # All selected processes are owned, but only managed MSBuild hosts are attach targets.
        if not re.search(r"dotnet|msbuild", process["name"], re.I) or process["state"] == "Z":
            continue
        if not any(p["pid"] == process["pid"] and p["identity"] == process["identity"]
                   for p in tree.refresh()):
            continue
        command = [str(sdk / "dotnet"), "--roll-forward", "Major", str(assembly),
                   "report", "--process-id", str(process["pid"])]
        stem = "stack-{}-{}".format(label, process["pid"])
        write_json(directory / (stem + ".command.json"), command)
        helpers.diagnostic_command(command, directory / (stem + ".txt"),
                                   timeout=min(6, remaining), env=stack_env)
    elapsed = time.monotonic() - start
    clock.event("stack-capture-end", run=directory.name, label=label,
                diagnostic_seconds=round(elapsed, 3))
    return elapsed


def evidence(directory):
    traces = {}
    errors = []
    pattern = re.compile(r"^(\S+) role=(\S+) pid=(\d+) tid=(\d+) stage=(.*)$")
    for path in sorted(directory.glob("*.trace")):
        events = []
        for line in path.read_text(encoding="utf-8").splitlines():
            match = pattern.match(line)
            if match is None:
                errors.append("Malformed trace: " + path.name)
                continue
            timestamp, role, pid, tid, stage = match.groups()
            events.append({"utc": timestamp, "role": role, "pid": int(pid), "tid": int(tid), "stage": stage})
        roles = {e["role"] for e in events}
        if len(roles) != 1:
            errors.append("Missing/mixed trace role: " + path.name)
            continue
        role = events[0]["role"]
        if role in traces:
            errors.append("Duplicate trace role: " + role)
        traces[role] = events
    log_path = directory / "build.log"
    log = log_path.read_text(encoding="utf-8", errors="replace") if log_path.is_file() else ""
    messages = {
        "root_completed": "PROBE ROOT COMPLETED",
        "parent_a_completed": "PROBE P TARGET A PARENT COMPLETED",
        "parent_b_completed": "PROBE P TARGET B PARENT COMPLETED",
        "b_child_request_completed": "PROBE B CHILD A REQUEST COMPLETED",
    }
    observed = {key: any(line.strip().startswith(message) for line in log.splitlines())
                for key, message in messages.items()}
    gates = {}
    for name in ("A-child-ready", "B-child-ready"):
        path = directory / name
        gates[name] = path.read_text(encoding="utf-8") if path.is_file() else None
    snapshot = directory / "snapshot.binlog"
    status = directory / "snapshot.binlog.status.txt"
    finalized = (snapshot.is_file() and snapshot.stat().st_size > 64 and status.is_file()
                 and "Actual event prefix only" in status.read_text(encoding="utf-8"))
    packet_observations = []
    packet_misrouting_proved = False
    for path in sorted(directory.glob("packet-ownership-*.json")):
        observation = json.loads(path.read_text(encoding="utf-8"))
        packet_observations.append(observation)
        if observation.get("error"):
            errors.append("Packet observer failed: " + observation["error"])
            continue
        if observation.get("readOnly") is not True:
            errors.append("Packet observer did not identify a read-only snapshot")
            continue
        for snapshot in observation.get("snapshots", []):
            handlers = snapshot["handlers"]
            if len(handlers) != 2 or handlers[0]["role"] != "B" or handlers[1]["role"] != "A":
                continue
            b, a = handlers
            a_message = any(p["type"] == "LogMessagePacket" and
                            "PROBE role=A " in (p.get("probeMessage") or "") and
                            "CHILD_RETURN success=True" in p["probeMessage"] for p in b["packets"])
            completion = any(p["type"] == "TaskHostTaskComplete" and p["taskResult"] == "Success"
                             for p in b["packets"])
            if (b["stackIndex"] == 0 and a["stackIndex"] == 1 and a_message and completion
                    and not a["packets"] and a["node"] == b["node"] and a["task"] != b["task"]):
                packet_misrouting_proved = True
    return dict(observed, traces=traces, trace_errors=errors, gates=gates,
                packet_observations=packet_observations,
                packet_misrouting_proved=packet_misrouting_proved,
                snapshot_finalized=finalized,
                gate_timeout="GATE_TIMEOUT" in log or any(
                    e["stage"] == "GATE_TIMEOUT" for events in traces.values() for e in events))


def classify(result):
    observed = result["evidence"]
    traces = observed["traces"]
    reasons = list(observed["trace_errors"])
    expected_roles = {"A", "B", "A-child", "B-child", "B-before-nested"}

    def stages(role):
        return [e["stage"] for e in traces.get(role, [])]

    def event(role, prefix):
        return next((e for e in traces.get(role, []) if e["stage"].startswith(prefix)), None)

    if set(traces) != expected_roles:
        reasons.append("Missing or unexpected task roles")
    if observed["gate_timeout"]:
        reasons.append("A gate timed out")
    for role in ("A-child", "B-child", "B-before-nested"):
        if not {"GATE_OPEN", "REACQUIRED", "EXECUTE_RETURN success=True"} <= set(stages(role)):
            reasons.append("Gate did not successfully finish: " + role)
    for role, name in (("A-child", "A-child-ready"), ("B-child", "B-child-ready")):
        start = event(role, "START")
        if start is None or observed["gates"][name] != "{} {}".format(role, start["pid"]):
            reasons.append("Gate is not a matching real child of RunDir: " + name)
    if not observed["snapshot_finalized"]:
        reasons.append("Real-event snapshot was not finalized")
    if not result["cleanup_succeeded"] or result.get("error"):
        reasons.append("Supervisor or cleanup failure")
    returned = all("EXECUTE_RETURN success=True" in stages(role) and
                   "CHILD_RETURN success=True" in stages(role) for role in ("A", "B"))
    completed = all(observed[key] for key in (
        "root_completed", "parent_a_completed", "parent_b_completed", "b_child_request_completed"))
    if not reasons and not result["timed_out"] and result["observed_exit_code"] == 0 and returned and completed:
        return {"classification": "control-passed" if result["mode"] == "normal" else "non-reproduction",
                "reasons": ["Natural exit 0; root, both parents and both nested calls completed"]}
    if result["mode"] == "normal":
        return {"classification": "control-failed", "reasons": reasons or ["Control did not naturally complete"]}
    a_call, b_call = event("A", "CALL_CHILD"), event("B", "CALL_CHILD")
    a_return = event("A", "EXECUTE_RETURN success=True")
    ordered = (a_call is not None and b_call is not None and a_return is not None
               and a_call["utc"] < b_call["utc"] < a_return["utc"])
    same_host = a_call is not None and b_call is not None and a_call["pid"] == b_call["pid"]
    host = next((p for p in result["observed_processes"]
                 if same_host and p["pid"] == a_call["pid"]
                 and re.search(r"[/-]nodemode:2(?:\s|$)", p["command"], re.I)), None)
    blocked = ("CHILD_RETURN success=True" in stages("A") and
               not any(s.startswith(("CHILD_RETURN", "EXECUTE_RETURN")) for s in stages("B")))
    if (not reasons and result["timed_out"] and ordered and same_host and host is not None
            and blocked and not any(observed[key] for key in (
                "root_completed", "parent_a_completed", "parent_b_completed", "b_child_request_completed"))):
        return {"classification": "observed-reproduction",
                "reasons": ["120-second bound; same owned TaskHost; A returned after B entered; "
                            "B and both parents still outstanding; all gates completed"],
                "taskhost_pid": host["pid"], "taskhost_identity": host["identity"],
                "packet_misrouting_proved": observed.get("packet_misrouting_proved", False),
                "scope": "Minimal nested-callback graph; original VMR instance attribution is separate"}
    return {"classification": "inconclusive", "reasons": reasons or [
        "Run neither completed normally nor reached all required same-host stall markers"]}


def next_binlog(output):
    numbers = [int(path.stem) for path in output.rglob("*.binlog") if path.stem.isdigit()]
    return output / ("{}.binlog".format(max(numbers, default=0) + 1))


def build_command(sdk, harness, directory, mode, binlog):
    if mode == "compile":
        command = [str(sdk / "dotnet"), "build", str(harness / "ProbeTasks.csproj"),
                   "--nologo", "/nr:false", "/p:NuGetAudit=false", "/p:UseSharedCompilation=false"]
    else:
        command = [str(sdk / "dotnet"), "msbuild", str(harness / "Root.proj"), "/nologo",
                   "/m:2", "/nr:false", "/p:RunDir=" + str(directory)]
        if mode == "mt":
            command.append("-mt")
        assembly = harness / "bin" / "Debug" / "net11.0" / "ProbeTasks.dll"
        command.append("/logger:NestedReentrancyProbe.SnapshotLogger,{};{}".format(
            assembly, directory / "snapshot.binlog"))
    return command + ["/bl:" + str(binlog), "/v:diag",
                      "/flp:logfile={};verbosity=diagnostic".format(directory / "build.log")]


def run_build(helpers, sdk, harness, output, assembly, env, mode, name, clock):
    directory = output / name
    directory.mkdir()
    binlog = next_binlog(output)
    command = build_command(sdk, harness, directory, mode, binlog)
    write_json(directory / "command.json", {"argv": command, "cwd": str(harness)})
    result = {"mode": mode, "name": name, "binlog": str(binlog), "timeout_seconds": TIMEOUT,
              "timed_out": False, "observed_exit_code": None, "cleanup_succeeded": False,
              "diagnostic_seconds": 0, "observed_processes": [], "cli_succeeded": False,
              "classification": "inconclusive"}
    process = None
    tree = None
    start = time.monotonic()
    clock.event("build-start", run=name, binlog=str(binlog))
    try:
        with (directory / "stdout.txt").open("w", encoding="utf-8") as stdout, \
                (directory / "stderr.txt").open("w", encoding="utf-8") as stderr:
            process = subprocess.Popen(command, cwd=str(harness), env=env, stdout=stdout,
                                       stderr=stderr, start_new_session=True)
            result["pid"] = process.pid
            (directory / "pid.txt").write_text(str(process.pid) + "\n", encoding="utf-8")
            tree = OwnedTree(helpers, process)
            tree.snapshot(directory, "start")
            rounds = [(20, "pending"), (96, "before-timeout")]
            while True:
                tree.refresh()
                code = process.poll()
                elapsed = time.monotonic() - start
                if code is not None:
                    result["observed_exit_code"] = code
                    break
                if elapsed >= TIMEOUT:
                    result["timed_out"] = True
                    break
                if rounds and elapsed >= rounds[0][0]:
                    _, label = rounds.pop(0)
                    result["diagnostic_seconds"] += capture_stacks(
                        helpers, tree, directory, label, sdk, assembly, env,
                        min(start + TIMEOUT - 2, time.monotonic() + 18), clock)
                time.sleep(0.1)
            result["build_observation_seconds"] = round(time.monotonic() - start, 3)
            result["observed_processes"] = tree.snapshot(directory, "observation-end")
            result["cli_succeeded"] = not result["timed_out"] and result["observed_exit_code"] == 0
            # Freeze the classification input before cancellation can introduce new events/errors.
            if mode != "compile":
                result["evidence"] = evidence(directory)
                write_json(directory / "observation-evidence.json", result["evidence"])
            clock.event("build-observation-end", run=name, timed_out=result["timed_out"],
                        observed_exit_code=result["observed_exit_code"],
                        observation_seconds=result["build_observation_seconds"])
    except BaseException as error:
        result["error"] = "{}: {}".format(type(error).__name__, error)
        raise
    finally:
        try:
            if process is not None:
                # Popen has not reaped the root if the initial procfs read threw.
                if tree is None:
                    tree = OwnedTree(helpers, process)
                result["cleanup_succeeded"] = tree.cleanup(directory)
                result["post_cleanup_exit_code"] = process.poll()
        finally:
            result["total_supervision_seconds"] = round(time.monotonic() - start, 3)
            result["default_binlog_may_be_unfinalized"] = result["timed_out"]
            result["timing_note"] = "Correctness observation includes diagnostic overhead; not benchmark time"
            write_json(directory / "result.json", result)
    if mode != "compile":
        result.update(classify(result))
    else:
        result["classification"] = ("compile-passed" if result["cli_succeeded"] and
                                    result["cleanup_succeeded"] else "compile-failed")
    write_json(directory / "result.json", result)
    clock.event("run-classified", run=name, classification=result["classification"],
                cli_succeeded=result["cli_succeeded"])
    return result


def interrupted(signum, frame):
    raise InterruptedError("Supervisor received signal {}".format(signum))


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--sdk", type=Path, required=True, help="Exact Linux SDK installation directory")
    parser.add_argument("--output", type=Path, required=True, help="Fresh artifact directory outside the checkout")
    parser.add_argument("--tools", type=Path, required=True, help="Private pinned diagnostic tool directory")
    args = parser.parse_args()
    args.sdk, args.output, args.tools = (path.resolve() for path in (args.sdk, args.output, args.tools))
    if not sys.platform.startswith("linux") or platform.machine().lower() not in ("x86_64", "amd64"):
        parser.error("This supervisor requires Linux x64; the XML/C# payload is cross-platform")
    if within(args.output, REPO) or within(args.sdk, args.output) or within(args.output, args.sdk):
        parser.error("Use an artifact output outside the checkout, separate from the SDK")
    if (within(args.tools, REPO) or within(args.tools, args.sdk) or within(args.sdk, args.tools)
            or within(args.tools, args.output / "harness")):
        parser.error("Use private tools outside the checkout, separate from the SDK and staged payload")
    args.output.mkdir(parents=True, exist_ok=True)
    # Bootstrap logs and old numeric logs are allowed; never overwrite a previous experiment.
    if any((args.output / name).exists() for name in ("harness", "result.json", "normal", "mt-1", "mt-2")):
        parser.error("Output already contains an experiment; choose a fresh output directory")
    clock = Clock(args.output)
    result = {
        "classification": "inconclusive", "experiment_exit_code": 2, "runs": [],
        "buildId": os.environ.get("BUILD_BUILDID"), "sourceVersion": os.environ.get("BUILD_SOURCEVERSION"),
        "job": os.environ.get("SYSTEM_JOBDISPLAYNAME"), "jobId": os.environ.get("SYSTEM_JOBID"),
        "jobAttempt": os.environ.get("SYSTEM_JOBATTEMPT"),
        "sdk_version": SDK_VERSION, "msbuild_version": MSBUILD_VERSION, "sdk_product_commit": PRODUCT_COMMIT,
        "scope": "Diagnostic nested-callback graph only; not performance or original VMR failure identity",
        "packet_misrouting_proved": False,
        "proof_gap": "Original full-VMR instance attribution remains separate from this minimal graph",
    }
    write_json(args.output / "result.json", result)
    handlers = {sig: signal.signal(sig, interrupted) for sig in (signal.SIGINT, signal.SIGTERM)}
    try:
        # Only this supervisor and its children: never change the agent's machine-wide limits.
        import resource
        resource.setrlimit(resource.RLIMIT_CORE, (0, 0))
        helpers = load_helpers()
        clock.event("setup-start")
        harness = stage_payload(args.output)
        env = private_environment(args.sdk, args.output)
        validate_sdk(args.sdk, harness, args.output, env, clock)
        assembly = prepare_tool(helpers, args.tools, args.output, clock)
        for mode, name in (("compile", "compile"), ("normal", "normal"), ("mt", "mt-1"), ("mt", "mt-2")):
            run = run_build(helpers, args.sdk, harness, args.output, assembly, env, mode, name, clock)
            result["runs"].append({"name": name, "classification": run["classification"],
                                   "cli_succeeded": run["cli_succeeded"], "timed_out": run["timed_out"],
                                   "observed_exit_code": run["observed_exit_code"],
                                   "packet_misrouting_proved": run.get("packet_misrouting_proved", False)})
            write_json(args.output / "result.json", result)
            if mode == "compile":
                if run["classification"] != "compile-passed":
                    raise RuntimeError("Compile failed; inspect its numeric binlog")
                task_assembly = harness / "bin" / "Debug" / "net11.0" / "ProbeTasks.dll"
                write_json(args.output / "probe-assembly-sha256.json", {str(task_assembly): sha256(task_assembly)})
            if mode == "normal" and run["classification"] != "control-passed":
                raise RuntimeError("Normal control failed; MT comparison is invalid")
            if not run["cleanup_succeeded"]:
                raise RuntimeError("Owned processes remain; refusing another run")
        outcomes = [run["classification"] for run in result["runs"] if run["name"].startswith("mt-")]
        if outcomes == ["observed-reproduction", "observed-reproduction"]:
            result.update(classification="observed-reproduction", experiment_exit_code=0,
                          packet_misrouting_proved=all(
                              run["packet_misrouting_proved"] for run in result["runs"]
                              if run["name"].startswith("mt-")))
        elif outcomes == ["non-reproduction", "non-reproduction"]:
            result.update(classification="non-reproduction", experiment_exit_code=0)
        else:
            result["reason"] = "MT observations are inconclusive or inconsistent: " + ", ".join(outcomes)
    except BaseException as error:
        result["error"] = "{}: {}".format(type(error).__name__, error)
        clock.event("experiment-error", error=result["error"])
    finally:
        write_json(args.output / "result.json", result)
        clock.event("experiment-end", classification=result["classification"],
                    experiment_exit_code=result["experiment_exit_code"],
                    note="Exit 0 means a valid experiment outcome, NOT that timed-out MSBuild passed")
        for sig, previous in handlers.items():
            signal.signal(sig, previous)
    return result["experiment_exit_code"]


if __name__ == "__main__":
    sys.exit(main())
