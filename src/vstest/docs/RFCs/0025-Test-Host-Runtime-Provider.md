Test Host Runtime Extensibility

## Summary
Allow the test platform to discover/launch test host provided by 3rd party. This would be done via a custom host provider, which understands the deployment and launching of test host for a specified runtime.

## Motivation
As the platforms are evolving, the test platform cannot provide the ability to deploy test on every platform. The test platform should be able to run tests on any platform, provided there is a corresponding test host that understands the platform and can provide environment for the tests to discover/run. This provides an extensibility point where the test platform discovers available tests host and based on the test run criteria delegates the responsibility to execute test to appropriate Test Host provider.

## Detailed Design

This design will be detailed through the following sections:

	1. RunTime Provider Contract
	2. Discovering available TestHost (Acquisition)
	3. Choosing appropriate TestHost

### RunTime Provider Contract
A Test RunTime provider will implement [ITestRunTimeProvider](./src/Microsoft.TestPlatform.ObjectModel/Host/ITestRunTimeProvider.cs#L18)

ITestRunTimeProvider provides set of API's needed for any Test RunTime provider to be able to deploy, & launch the Test RunTime.

Interface [ITestHostLauncher](./src/Microsoft.TestPlatform.ObjectModel/Client/Interfaces/ITestHostLauncher.cs) provides an Extension point for IDE's to start TestHost process themselves. For example, in case of debugging/profiling test, the VS IDE would need to launch the TestHost itself.
The interfaces would be part of Object Model.

### Discovering TestHost

Default Test Host required by the test platform are packaged along with it in a directory that always gets probed. This directory called "Extensions" resides next to vstest.console.exe. An assembly with name ending with *RuntimeProvider.dll* placed in this directory is a candidate Test Runtime provider.

### Choosing the right TestHost

The test platform enumerates over the host providers, providing them with current RunConfiguration. The host providers then return if they are can Launch a TestHost based on RunConfiguration. This is achieved via following implementation

```
  ITestRunTimeProvider.CanExecuteCurrentRunConfiguration():
```
