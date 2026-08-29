from abc import ABCMeta, abstractmethod, abstractproperty
try:
    from helix.public import TestResult
except ImportError:
    from _helix_compat import TestResult
from typing import Iterable


class ResultFormat:
    __metaclass__ = ABCMeta

    def __init__(self):
        pass

    @abstractproperty
    def name(self) -> str:
        pass

    @abstractproperty
    def acceptable_file_suffixes(self) -> Iterable[str]:
        pass

    @abstractmethod
    def read_results(self, path) -> Iterable[TestResult]:
        pass
