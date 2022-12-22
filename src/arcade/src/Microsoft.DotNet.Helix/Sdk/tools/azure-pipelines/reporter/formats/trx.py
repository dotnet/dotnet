import glob
import xml.etree.ElementTree
from .result_format import ResultFormat
from helix.public import TestResult, TestResultAttachment


class TRXFormat(ResultFormat):

    def __init__(self):
        super(TRXFormat, self).__init__()
        pass

    @property
    def name(self):
        return 'trx'

    @property
    def acceptable_file_suffixes(self):
        # Repos generating trx files produce multiple trx files, which are named based on the class names
        # Find all trx files that might exist
        yield ".trx"

    def read_results(self, path):
        test_classes = {}
        test_methods = {}
        ns = {'vstest' : 'http://microsoft.com/schemas/VisualStudio/TeamTest/2010'}

        # class names are stored separately from the results. Gather testName->className information
        # and store it away first, since it is smaller than storing the results
        for (_, element) in xml.etree.ElementTree.iterparse(path, events=['end']):
            if element.tag.endswith("UnitTest"):
                test_id = element.get("id")
                testMethod_element = element.find("vstest:TestMethod", ns)
                if testMethod_element is not None:
                    test_classes[test_id] = testMethod_element.get("className")
                    test_methods[test_id] = testMethod_element.get("name")
                element.clear()

        for (_, element) in xml.etree.ElementTree.iterparse(path, events=['end']):
            if element.tag.endswith("UnitTestResult"):
                test_name = element.get("testName")
                test_id = element.get("testId")

                # Find the class name from the dictionary we created earlier
                classname = test_classes[test_id]
                method = test_methods[test_id]

                name = classname + '.' + test_name
                type_name = classname
                duration = 0.0
                result = "Pass"
                outcome = element.get("outcome")
                exception_type = None
                failure_message = None
                stack_trace = None
                skip_reason = None
                attachments = []

                if element.get("duration") is not None:
                    hour, minute, second = element.get("duration").split(':')
                    duration = float(hour)*60*60 + float(minute)*60 + float(second)

                if outcome == "NotExecuted":
                    result = "Skip"
                    output_element = element.find("vstest:Output", ns)
                    if output_element is not None:
                        stdout_element = output_element.find("vstest:StdOut", ns)
                        if stdout_element is not None:
                            skip_reason = stdout_element.text

                elif outcome == "Failed":
                    result = "Fail"
                    output_element = element.find("vstest:Output", ns)
                    if output_element is not None:
                        error_element = output_element.find("vstest:ErrorInfo", ns)
                        if error_element is not None:
                            message_element = error_element.find("vstest:Message", ns)
                            if message_element is not None:
                                failure_message = message_element.text
                            stacktrace_element = error_element.find("vstest:StackTrace", ns)
                            if stacktrace_element is not None:
                                stack_trace = stacktrace_element.text

                        stdout_element = output_element.find("vstest:StdOut", ns)
                        if stdout_element is not None:
                            attachments.append(TestResultAttachment(
                                name=u"Console_Output.log",
                                text=stdout_element.text,
                            ))

                        stderr_element = output_element.find("vstest:StdErr", ns)
                        if stderr_element is not None:
                            attachments.append(TestResultAttachment(
                                name=u"Error_Output.log",
                                text=stderr_element.text,
                            ))
                else:
                    result = "Pass"

                res = TestResult(name, u'trx', type_name, method, duration, result, exception_type, failure_message, stack_trace,
                                 skip_reason, attachments)
                yield res

                # remove the element's content so we don't keep it around too long.
                element.clear()
