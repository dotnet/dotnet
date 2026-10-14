import argparse
import hashlib
import pathlib
import re
import subprocess


def main():
    parser = argparse.ArgumentParser(description="Verify and extract a prebuilt CI SDK.")
    parser.add_argument("--input", required=True, type=pathlib.Path)
    parser.add_argument("--filename", required=True)
    parser.add_argument("--sha512", required=True)
    parser.add_argument("--output", required=True, type=pathlib.Path)
    args = parser.parse_args()
    if pathlib.Path(args.filename).name != args.filename:
        parser.error("--filename must be a basename")
    if not re.fullmatch(r"[0-9a-fA-F]{128}", args.sha512):
        parser.error("--sha512 must contain 128 hexadecimal digits")
    matches = sorted(path for path in args.input.rglob(args.filename) if path.is_file())
    if not matches:
        parser.error(f"No SDK archive named {args.filename} found under {args.input}")
    verified = []
    for archive in matches:
        digest = hashlib.sha512()
        with archive.open("rb") as stream:
            for chunk in iter(lambda: stream.read(1024 * 1024), b""):
                digest.update(chunk)
        actual = digest.hexdigest()
        print(f"SDK candidate: {archive}\nSHA512: {actual}", flush=True)
        if actual == args.sha512.lower():
            verified.append(archive)
        else:
            print("Rejected: SHA512 does not match the requested SDK.", flush=True)
    if not verified:
        raise RuntimeError(f"No SDK archive matches expected SHA512 {args.sha512.lower()}")
    archive = verified[0]
    print(f"Found {len(verified)} checksum-matching SDK archive(s); extracting {archive}", flush=True)
    args.output.mkdir(parents=True, exist_ok=False)
    subprocess.run(["tar", "-xzf", str(archive), "-C", str(args.output)], check=True)
    if not (args.output / "dotnet").is_file():
        raise RuntimeError("Extracted archive does not contain dotnet")
    print(f"Verified SDK SHA512: {args.sha512.lower()}", flush=True)


if __name__ == "__main__":
    main()
