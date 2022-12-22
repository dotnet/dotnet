# Test Retry and Quarantine Configuration
In order to drive behavior in test execution, we need a way to specify that behavior at a test level.
It is also necessary that this information be machine readable/modifyable, in order for all the desired
tooling to be written.

As such, the current plan is for a file to be locate at "eng/test-configuration.json" that contains this information.

## Sample format
```json
{
  "version" : 2,
  "defaultOnFailure": "fail",
  "localRerunCount" : 2,
  "remoteRerunCount" : 1,
  "retryOnRules": [
    {"testName": {"regex": "^System\\.Networking\\..*"}},
    {"testAssembly": {"wildcard": "System.SomethingElse.*" }},
    {"failureMessage": "network disconnected" },
  ],
  "failOnRules": [
  ],
  "quarantineRules": [
  ]
}
```

## Description
### version
Schema version for compatibility

### defaultOnFailure
- default: "fail"

One of "fail" or "rerun"
<dl>
<dt>fail</dt><dd>If a test fails, the default behavior if no rules match is to fail the test immediate</dd>
<dt>rerun</dt><dd>If a test fails, the default behavior is no rules match is to rerun the test according to the localRerun/remoteRerun counts</dd>
</dl>

## localRerunCount
- default: 1

This number indicates the number of times a test that needs to be "rerun" should be rerun on the local computer immediately.
This is the fastest rerun option, because the payloads don't need to be redownloaded, so it always the first attempted re-execution method.

In the example, with a value of "2", that means that the test will need to fail 3 times before being marks as failed (1 intial failure, and 2 rerun failures).

## remoteRerunCount
- default: 0
- required version: 2

If all "localRerun" values have been attempted, and the test is still failing, this number being non-zero means that additional
rerun attempts will be queued on a different machine. This requires redownloading all the payloads a second time, so is significantly slower
than local reruns, and should only be used when suspected machine state corruption is the cause of test failures.

## rules
The three "rules" entries are lists of rules that will be used to match test to determin desired behavior. In the case of multiple rule matches:
- if a quarantine rule matches, the test is quarantined
- if the default behavior is "rerun" and a "fail" rule matches, the test is failed
- if the default bheavior is "fail" and a "rerun" rule matches, the test is rerun
- default behavior is used

A "rule" consists of a property, and then a rule object

### Properties
<dl>
<dt>testName</dt><dd>The name of the test, including namespace, class name, and method name</dd>
<dt>testAssembly</dt><dd>The name of the assembly containing the test</dd>
<dt>failureMessage</dt><dd>The failure message logged by the test</dd>
<dt>callstack (multiline)</dt><dd>The callstack reported by the test execution</dd>
</dl>

### Rule object
For all rules, if a property is designated "multiline", then the rule must match a line, otherwise the entire value is used.

All comparisons are case-insensitive
#### Raw string (e.g. "rule string")
True if the property value exactly matches the string
#### {"contains": "value"}
True if the property contains (case-insensitive) the value string
#### {"wildcard": "value with * wildcard"}
The same as a raw string, but "*" can match any number of characters, and "?" can match one character
#### {"regex": "value with .* regex"}
true if the propety matches the regular expression


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CDevWorkflow%5CDesign%5CDev-Design-Test-Configuration-Json.md)](https://helix.dot.net/f/p/5?p=Documentation%5CDevWorkflow%5CDesign%5CDev-Design-Test-Configuration-Json.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CDevWorkflow%5CDesign%5CDev-Design-Test-Configuration-Json.md)</sub>
<!-- End Generated Content-->
