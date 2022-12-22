import xml.etree.ElementTree
from .result_format import ResultFormat
from helix.public import TestResult, TestResultAttachment


class JUnitFormat(ResultFormat):

    def __init__(self):
        super(JUnitFormat, self).__init__()
        pass

    @property
    def name(self):
        return 'junit'

    @property
    def acceptable_file_suffixes(self):
        yield 'junit-results.xml'
        yield 'junitresults.xml'

    def read_results(self, path):
        for (_, element) in xml.etree.ElementTree.iterparse(path, events=['end']):
            if element.tag == 'testcase':
                test_name = element.get("name")
                classname = element.get("classname")
                name = classname + "." + test_name
                type_name = classname
                method = test_name
                duration = float(element.get("time"))
                result = "Pass"
                exception_type = None
                failure_message = None
                stack_trace = None
                skip_reason = None
                attachments = []


                failure_element = element.find("failure")
                if failure_element is None:
                    failure_element = element.find("error")

                if failure_element is not None:
                    result = "Fail"
                    exception_type = failure_element.get("type")
                    failure_message = failure_element.get("message")
                    stack_trace = failure_element.text

                    stdout_element = element.find("system-out")
                    if stdout_element is not None:
                        attachments.append(TestResultAttachment(
                            name=u"Console_Output.log",
                            text=stdout_element.text,
                        ))

                    stderr_element = element.find("system-err")
                    if stderr_element is not None:
                        attachments.append(TestResultAttachment(
                            name=u"Error_Output.log",
                            text=stderr_element.text,
                        ))

                skipped_element = element.find("skipped")
                if skipped_element is not None:
                    result = "Skip"
                    skip_reason = skipped_element.text or u""


                res = TestResult(name, u'junit', type_name, method, duration, result, exception_type, failure_message, stack_trace,
                                 skip_reason, attachments)
                yield res
                # remove the element's content so we don't keep it around too long.
                element.clear()

