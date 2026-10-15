import argparse
import json
import pathlib


def summarize(directory):
    records = []
    dumps = set(directory.glob("*.dmp"))
    dumps.update(
        pathlib.Path(str(report).removesuffix(".crashreport.json"))
        for report in directory.glob("*.dmp.crashreport.json")
    )
    for dump in sorted(dumps):
        report = pathlib.Path(str(dump) + ".crashreport.json")
        record = {"dump": dump.name if dump.is_file() else None}
        if report.is_file():
            record["report"] = report.name
            payload = json.loads(report.read_text())["payload"]
            threads = [
                thread for thread in payload["threads"]
                if str(thread.get("crashed", "false")).lower() == "true"
            ]
            record.update({
                "classification": "crash" if threads else "diagnostic",
                "process": payload.get("process_name"),
                "crashed_threads": threads,
            })
        else:
            record["classification"] = "unclassified"
        records.append(record)
    return records


def main():
    parser = argparse.ArgumentParser(description="Distinguish crash dumps from diagnostic snapshots.")
    parser.add_argument("directory", type=pathlib.Path)
    args = parser.parse_args()
    if not args.directory.is_dir():
        parser.error(f"Dump directory does not exist: {args.directory}")
    records = summarize(args.directory)
    (args.directory / "summary.json").write_text(json.dumps(records, indent=2) + "\n")
    for record in records:
        print(json.dumps(record), flush=True)
        if record["classification"] == "unclassified":
            print(
                f"##vso[task.logissue type=warning]Dump has no crash report; "
                f"its cause is unclassified: {record['dump']}",
                flush=True,
            )
    return int(any(record["classification"] == "crash" for record in records))


if __name__ == "__main__":
    raise SystemExit(main())
