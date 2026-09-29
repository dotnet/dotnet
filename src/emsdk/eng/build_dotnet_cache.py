#!/usr/bin/env python3

import argparse
import fnmatch
import json
import sys
from pathlib import Path


def read_exclusions(path):
  patterns = [line.strip() for line in path.read_text(encoding='utf-8').splitlines() if line.strip()]
  if not patterns:
    raise ValueError(f'Cache exclusion policy is empty: {path}')
  if any('/' in pattern or '\\' in pattern for pattern in patterns):
    raise ValueError('Cache exclusions must be filenames, not paths.')
  return patterns


def select_targets(libraries, patterns):
  selected = []
  excluded = []
  for name, library in libraries.items():
    destination = excluded if any(
      fnmatch.fnmatchcase(library.get_filename(), pattern) for pattern in patterns
    ) else selected
    destination.append(name)
  if not selected:
    raise ValueError('The cache policy excluded every system-library target.')
  return selected, excluded


def main():
  parser = argparse.ArgumentParser(description='Build the system-library cache shipped by .NET.')
  parser.add_argument('--emscripten-dir', type=Path, required=True)
  parser.add_argument('--exclusions', type=Path, required=True)
  parser.add_argument('--list', action='store_true', help='Print the selected targets without building.')
  args = parser.parse_args()
  patterns = read_exclusions(args.exclusions)
  sys.path.insert(0, str(args.emscripten_dir.resolve()))
  import embuilder

  libraries, _ = embuilder.get_system_tasks()
  additional_minimal = set(embuilder.MINIMAL_TASKS) - libraries.keys()
  if additional_minimal:
    raise ValueError(f'MINIMAL includes non-system targets that need explicit handling: {sorted(additional_minimal)}')
  selected, excluded = select_targets(libraries, patterns)
  if args.list:
    print(json.dumps({'selected': selected, 'excluded': excluded}, indent=2))
    return 0

  print(f'DOTNET cache: building {len(selected)} system targets; excluding {len(excluded)} unshipped targets.', flush=True)
  # Keep upstream initialization and build semantics without a long Windows shell command line.
  sys.argv = [str(args.emscripten_dir / 'embuilder.py'), 'build', *selected]
  return embuilder.main()


if __name__ == '__main__':
  sys.exit(main())
