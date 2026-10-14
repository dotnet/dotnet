#!/usr/bin/env bash
set -euo pipefail

if [[ $# -lt 2 ]]; then
  echo "Usage: bash run-native.sh SDK_BUNDLE_DIR OUTPUT_DIR [reproduce.py options]" >&2
  echo "Example: bash run-native.sh /data/pr10100-repro /data/repro-run-1 --iterations 1000 --parallelism 8 --commands globaljson,mstest-fsharp" >&2
  exit 2
fi
bundle=$(cd "$1" && pwd)
mkdir -p "$2"
output=$(cd "$2" && pwd)
shift 2
scripts=$(cd "$(dirname "$0")" && pwd)
arch=$(docker info --format '{{.Architecture}}')
case "$arch" in
  x86_64|amd64) ;;
  *) echo "Requires a native x64 Docker server; server reports $arch." >&2; exit 1 ;;
esac
if [[ -e "$output/sdk" || -e "$output/results" ]]; then
  echo "Choose a fresh output directory; SDK/results already exist." >&2
  exit 1
fi
docker run --rm --platform linux/amd64 \
  --privileged --ulimit core=-1 --ulimit nofile=65536:65536 \
  -v "$bundle:/input:ro" -v "$scripts:/repro:ro" -v "$output:/output" \
  --entrypoint /bin/bash \
  mcr.microsoft.com/dotnet-buildtools/prereqs@sha256:b06d7a274acbc1f5e7b2ae9eefeb8e73012708d79aff5c637b0321b7984a96d9 \
  -c 'set -euo pipefail
    python3 /repro/prepare-sdk.py --input /input \
      --filename dotnet-sdk-12.0.100-ci-centos.10-x64.tar.gz \
      --sha512 5a3a58f354efc5a73a1858ce25b536852009fabd6d65ef3508ef6df011bbcd031725eb116ec11477447a682a58d6f7daa75b11b4796c10f1971c10587e348ea8 \
      --output /output/sdk
    python3 /repro/reproduce.py --sdk /output/sdk --output /output/results \
      --nuget-config /input/tested-source/NuGet.config --verify-dumps "$@"
  ' bash "$@"
