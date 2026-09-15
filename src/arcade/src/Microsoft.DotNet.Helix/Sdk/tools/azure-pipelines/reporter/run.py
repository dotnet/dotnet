import os
import re
import sys
import time
import traceback
import logging
import shutil
from queue import Queue
from threading import Thread, Lock
from typing import Tuple, Optional
 
from helpers import get_env
from test_results_reader import read_results

# Bundled, dependency-free shim. Always importable. Provides the JSON writer
# plus stand-ins for TestResult / TestResultAttachment / AzureDevOpsReportingParameters
# used when helix-scripts is not installed (e.g. AOT Helix client).
import _helix_compat

# Try to use the real helix-scripts types so the legacy pickle (which keys on
# fully-qualified class names) stays byte-identical with previous releases.
# Fall back to the shim when helix-scripts is not present on the machine.
try:
    from helix.public import (
        DefaultTestReporter,
        AzureDevOpsReportingParameters,
        PackingTestReporter,
    )
    _HELIX_SCRIPTS_AVAILABLE = True
except ImportError:
    DefaultTestReporter = None
    PackingTestReporter = None
    AzureDevOpsReportingParameters = _helix_compat.AzureDevOpsReportingParameters
    _HELIX_SCRIPTS_AVAILABLE = False

def process_args() -> Tuple[str, str, str, Optional[str]]:
    if len(sys.argv) < 4 or len(sys.argv) > 5:
        print("Usage:")
        print("run.py <collection URI> <team project> <test run ID>")
        print("run.py <collection URI> <team project> <test run ID> <access token>")
        sys.exit("Expected 3 or 4 arguments")

    # argv[0] is the script name
    collection_uri = sys.argv[1]
    team_project = sys.argv[2]
    test_run_id = sys.argv[3]
    if len(sys.argv) == 5:
        access_token = sys.argv[4] # type: Optional[str]
    else:
        access_token = None

    return collection_uri, team_project, test_run_id, access_token


def main():
    logging.basicConfig(
        format='%(asctime)s: %(levelname)s: %(thread)d: %(module)s(%(lineno)d): %(funcName)s: %(message)s',
        level=logging.INFO,
        handlers=[
            logging.StreamHandler()
        ]
    )
    log = logging.getLogger(__name__)

    collection_uri, team_project, test_run_id, access_token = process_args()

    log.info("Beginning reading of test results.")

    # In case the user puts the results in HELIX_WORKITEM_UPLOAD_ROOT for upload, check there too.
    all_results = read_results([
        os.getcwd(),
        get_env("HELIX_WORKITEM_UPLOAD_ROOT"),
    ])

    azdo_parameters = AzureDevOpsReportingParameters(
        collection_uri,
        team_project,
        test_run_id,
        access_token,
    )

    # 1) Legacy: when helix-scripts is installed, write the pickle file at
    #    {HELIX_WORKITEM_ROOT}/__test_report.json that the Python Helix client
    #    (helix.executor) consumes. Behavior here is byte-identical to the
    #    pre-change reporter so existing consumers see no difference.
    if _HELIX_SCRIPTS_AVAILABLE:
        try:
            reporter = DefaultTestReporter(azdo_parameters)
            reporter.report_results(all_results)
        except Exception:
            log.exception("Legacy pickle reporter failed; continuing to write JSON results")
    else:
        log.warning(
            "helix-scripts not available; skipping legacy pickle reporter. "
            "Consumers must read the JSON results file at '%s'.",
            _helix_compat.json_results_path(),
        )

    # 2) New: always write a portable JSON results file alongside. This is
    #    language-neutral and lets non-Python Helix clients (e.g. the AOT
    #    client) consume results without installing helix-scripts.
    try:
        _helix_compat.JsonReporter(azdo_parameters, log=log).report_results(all_results)
    except Exception:
        log.exception("Failed to write JSON results file")
        # Don't fail the work item solely because of the new path; the legacy
        # pickle file (if it was written above) is still the primary contract.
        if not _HELIX_SCRIPTS_AVAILABLE:
            raise

if __name__ == '__main__':
    main()


