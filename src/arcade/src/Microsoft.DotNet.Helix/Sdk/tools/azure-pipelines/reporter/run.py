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

from helix.public import DefaultTestReporter, AzureDevOpsReportingParameters, PackingTestReporter

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

    reporter = DefaultTestReporter(
        AzureDevOpsReportingParameters(
            collection_uri,
            team_project,
            test_run_id,
            access_token
        )
    )

    reporter.report_results(all_results)

if __name__ == '__main__':
    main()


