import sys
import os


def get_env(name):
    if name not in os.environ:
        sys.exit(name + " env var not set")
    return os.environ[name]


def batch(iterable, n=1):
    current_batch = []
    for item in iterable:
        if item is not None : current_batch.append(item)
        if len(current_batch) >= n:
            yield current_batch
            current_batch = []
    if current_batch:
        yield current_batch
