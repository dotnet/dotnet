# 0011 Test Session Timeout

## Summary
Allow user to specify timeout which will terminate a test session when it exceeds a given timeout.

## Motivation
The test platform should have the ability for users to specify a test run level timeout which allows users to terminate a test session when it exceeds a given timeout. This ensures that resources are well consumed and test sessions are constrained to a set time.

## Detailed Design

User would specify timeout using runsetting or through commandline.

### User can specify timeout using runsettings as follows:
```xml
<RunSettings>
  <RunConfiguration>
     <!-- Specify timeout in milliseconds. A valid value should be greater than 0 -->
     <TestSessionTimeout>10000</TestSessionTimeout>
  </RunConfiguration>
</RunSettings>
```

### User can specify timeout using commandline as follows:
```
vstest.console <testContainersList> -- RunConfiguration.TestSessionTimeout=10000
```

### Overview of changes
Testplatform will cancel the current test run if it has exceeded given `TestSessionTimeout`. It will also make sure that testhost is getting test cancel request. Finally it will conclude the run with whatever test run till that point.

### Exit Code of vstest.console
It will be a failed test and exit code will be 1

### Console output
```
Starting test execution, please wait...
Passed   TestNameSpace.UnitTestClass.TestMethod1
Passed   TestNameSpace.UnitTestClass.TestMethod2
Passed   TestNameSpace.UnitTestClass.TestMethod3
Passed   TestNameSpace.UnitTestClass.TestMethod4
Passed   TestNameSpace.UnitTestClass.TestMethod5
Canceling test run: test run timeout of 10000 milliseconds exceeded.
Passed   TestNameSpace.UnitTestClass.TestMethod6
Passed   TestNameSpace.UnitTestClass.TestMethod6

Total tests: 7. Passed: 7. Failed: 0. Skipped: 0.
Test Run Canceled.
Test execution time: 18.2893 Seconds
```


