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
    matches = list(args.input.rglob(args.filename))
    if len(matches) != 1:
        parser.error(f"Expected one SDK archive, found {len(matches)}")
    digest = hashlib.sha512()
    with matches[0].open("rb") as stream:
        for chunk in iter(lambda: stream.read(1024 * 1024), b""):
            digest.update(chunk)
    if digest.hexdigest() != args.sha512.lower():
        raise RuntimeError(f"SDK checksum mismatch: {digest.hexdigest()}")
    args.output.mkdir(parents=True, exist_ok=False)
    subprocess.run(["tar", "-xzf", str(matches[0]), "-C", str(args.output)], check=True)
    if not (args.output / "dotnet").is_file():
        raise RuntimeError("Extracted archive does not contain dotnet")
    print(f"Verified SDK SHA512: {digest.hexdigest()}", flush=True)


if __name__ == "__main__":
    main()
