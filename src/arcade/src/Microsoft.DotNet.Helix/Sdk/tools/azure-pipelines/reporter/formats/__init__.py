from typing import List
from .result_format import ResultFormat
from .xunit import XUnitFormat
from .junit import JUnitFormat
from .trx import TRXFormat


all_formats = [
    XUnitFormat(),
    JUnitFormat(),
    TRXFormat()
]  # type: List[ResultFormat]
