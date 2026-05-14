# Test Platform Migration Known issues

Here are the current known issues you may face when running tests, along with available workarounds.

## Change in Thread.CurrentPrincipal value

- **Issue:** Tests that depend on `Thread.CurrentPrincipal` may fail. This is due to change in inter process communication in Test Platform.
- **Workaround:** Use an alternative like `System.Security.Principal.WindowsIdentity.GetCurrent()`

## Change in test execution processes name

- **Issue:** Tests that depend on the name of the currnet running process may fail.
- **Workaround:** Tests run in one of following process ```vstest.console.exe```, ```testhost.exe```, ```testhost.x86.exe``` or ```dotnet.exe``` based on run configuration (```/Platform``` and ```/Framework```). If the tests depend on the process name, then update the tests accordingly.
