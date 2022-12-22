import logging
import os
from helix.public import TestResult, TestResultAttachment
from typing import Iterable, List
from formats import all_formats
from helpers import get_env


log = logging.getLogger(__name__)

def __no_results_result():
    exitCode = get_env("_commandExitCode")
    work_item_name = get_env("HELIX_WORKITEM_FRIENDLYNAME")
    
    if exitCode != "0":
        # if we have a catastrophic failure, we want to create the fake test result with attached dump files and logs (if available)
        return
    else:
        result = 'Pass'
        failure_message = None

    return TestResult(
        name=u'{}.WorkItemExecution'.format(work_item_name),
        kind=u'unknown',
        type_name=u'{}'.format(work_item_name),
        method=u'WorkItemExecution',
        duration=1,
        result=u'{}'.format(result),
        exception_type=None,
        failure_message=u'{}'.format(failure_message),
        stack_trace=None,
        skip_reason=None,
        attachments=[],
    )


def read_results(dirs_to_check: List[str]) -> Iterable[TestResult]:

    found = False

    for dir in dirs_to_check:
        log.info("Searching '{}' for test results files".format(dir))
        for root, dirs, files in os.walk(dir):
            for file_name in files:
                for f in all_formats:
                    if file_name.endswith(tuple(f.acceptable_file_suffixes)):
                        file_path = os.path.join(root, file_name)
                        log.info('Found results file {} with format {}'.format(file_path, f.name))
                        found = True
                        file_results = f.read_results(file_path)
                        for result in file_results:
                            yield result

    if not found:
        log.warn('No results file found in any of the following formats: {}'.format(', '.join((f.name for f in all_formats))))
        yield __no_results_result()
