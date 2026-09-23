import tempfile
import unittest
from pathlib import Path

from build_dotnet_cache import read_exclusions, select_targets


class Library:
  def __init__(self, filename):
    self.filename = filename

  def get_filename(self):
    return self.filename


class DotNetCacheTests(unittest.TestCase):
  def test_preserves_retained_variants_and_order(self):
    libraries = {
      'builtins': Library('libclang_rt.builtins.a'),
      'worker': Library('libc-ww.a'),
      'pthread': Library('libc-mt.a'),
      'worker_debug': Library('libc-ww-debug.a'),
      'worker_api': Library('libwasm_workers-mt.a'),
      'sanitizer_runtime': Library('libclang_rt.asan.a'),
      'startup': Library('crt1.o'),
    }
    selected, excluded = select_targets(libraries, ['lib*-ww.a', 'lib*-ww-*.a'])
    self.assertEqual(selected, ['builtins', 'pthread', 'worker_api', 'sanitizer_runtime', 'startup'])
    self.assertEqual(excluded, ['worker', 'worker_debug'])

  def test_matches_archive_filename_not_target_name(self):
    selected, excluded = select_targets({
      'retained': Library('libc.a'),
      'different_target_name': Library('libc-asan.a'),
    }, ['libc-asan.a'])
    self.assertEqual(selected, ['retained'])
    self.assertEqual(excluded, ['different_target_name'])

  def test_stale_patterns_do_not_remove_other_sanitizers(self):
    selected, excluded = select_targets({'asan': Library('libclang_rt.asan.a')}, ['libasan_rt.a'])
    self.assertEqual(selected, ['asan'])
    self.assertEqual(excluded, [])

  def test_rejects_empty_selection(self):
    with self.assertRaisesRegex(ValueError, 'every'):
      select_targets({'libc': Library('libc.a')}, ['*.a'])

  def test_reads_one_policy_for_build_and_packaging(self):
    with tempfile.TemporaryDirectory() as root:
      path = Path(root) / 'exclusions.txt'
      path.write_text('libc-asan.a\n\nlib*-ww.a\n', encoding='utf-8')
      self.assertEqual(read_exclusions(path), ['libc-asan.a', 'lib*-ww.a'])

  def test_rejects_empty_or_path_based_policy(self):
    with tempfile.TemporaryDirectory() as root:
      path = Path(root) / 'exclusions.txt'
      for contents in ('', 'folder/libc.a', 'folder\\libc.a'):
        with self.subTest(contents=contents):
          path.write_text(contents, encoding='utf-8')
          with self.assertRaises(ValueError):
            read_exclusions(path)


if __name__ == '__main__':
  unittest.main()
