import re
import xml.etree.ElementTree

from .result_format import ResultFormat
from helix.public import TestResult, TestResultAttachment

_unescape_char_map = {
    'r': '\r',
    'n': '\n',
    't': '\t',
    '0': '\0',
    'a': '\a',
    'b': '\b',
    'v': '\v',
    'f': '\f',
}

def _unescape_xunit_message(value):
    # xunit does some escaping on the error message we need to do our
    # best to turn back into something resembling the original message
    # It only uses \x**, \x**** (indistinguishably), and then the items from __unescape_char_map
    def bs(match):
        grp = match.group(0)
        sym = grp[1]
        if sym == 'x':
            return chr(int(grp[2:], 16))
        return _unescape_char_map.get(match.group(0)[1]) or sym
    return re.sub(r'\\x[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]?[0-9a-fA-F]?|\\[^x]', bs, value)

class XUnitFormat(ResultFormat):

    def __init__(self):
        super(XUnitFormat, self).__init__()
        pass

    @property
    def name(self):
        return 'xunit'

    @property
    def acceptable_file_suffixes(self):
        yield 'testResults.xml'
        yield 'test-results.xml'
        yield 'test_results.xml'

    def read_results(self, path):
        for (_, element) in xml.etree.ElementTree.iterparse(path, events=['end']):
            if element.tag == 'test':
                name = element.get("name")
                type_name = element.get("type")
                method = element.get("method")
                duration = float(element.get("time"))
                result = element.get("result")
                exception_type = None
                failure_message = None
                stack_trace = None
                skip_reason = None
                attachments = []

                failure_element = element.find("failure")
                if failure_element is not None:
                    exception_type = failure_element.get("exception-type")
                    message_element = failure_element.find("message")
                    if message_element is not None:
                        failure_message = _unescape_xunit_message(message_element.text)
                    stack_trace_element = failure_element.find("stack-trace")
                    if stack_trace_element is not None:
                        stack_trace = stack_trace_element.text

                    output_element = element.find("output")
                    if output_element is not None:
                        attachments.append(TestResultAttachment(
                            name=u"Console_Output.log",
                            text=output_element.text,
                        ))

                reason_element = element.find("reason")
                if reason_element is not None:
                    skip_reason = reason_element.text

                res = TestResult(name, u'xunit', type_name, method, duration, result, exception_type, failure_message, stack_trace,
                                 skip_reason, attachments)
                yield res
                # remove the element's content so we don't keep it around too long.
                element.clear()

