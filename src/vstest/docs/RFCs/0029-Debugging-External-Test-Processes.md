# 0029 Debugging External Test Processes

# Summary
Introduce APIs to improve debugging support in Visual Studio for tests that run in an external process other than `testhost*.exe`.

# Motivation
Some test frameworks (examples include [TAEF](https://docs.microsoft.com/en-us/windows-hardware/drivers/taef/) and [Python](https://docs.microsoft.com/en-us/visualstudio/python/unit-testing-python-in-visual-studio)) need to execute tests in a external process other than `testhost*.exe`. When it comes to debugging such tests in Visual Studio today, there are a couple of problems. 

1. A test adapter can request to launch a child process with debugger attached by calling  [`IFrameworkHandle.LaunchProcessWithDebuggerAttached()`](./src/Microsoft.TestPlatform.ObjectModel/Adapter/Interfaces/IFrameworkHandle.cs#L29) within adapter's implementation of [`ITestExecutor.RunTests()`](./src/Microsoft.TestPlatform.ObjectModel/Adapter/Interfaces/ITestExecutor.cs#L23) (after checking that [`IRunContext.IsBeingDebugged`](./src/Microsoft.TestPlatform.ObjectModel/Adapter/Interfaces/IRunContext.cs#L32) is `true`). However, there is no supported way for a test adapter to request that debugger should be attached to an **already running process**.

2. Even though a test adapter can launch a child process with debugger attached as described above, today the debugger is always attached to the `testhost*.exe` process as well. This means that **Visual Studio ends up debugging two processes** instead of just the single process where tests are running.

Debugging of Python tests is supported in VS today. However, the Python adapter works around the above limitations by talking to the Python extension inside VS whenever `ITestExecutor.RunTests()` is invoked (and `IRunContext.IsBeingDebugged` is `true`). The Python extension in VS then attaches VS debugger to the Python test process independently and also detaches the `testhost*.exe` process to achieve the desired behavior.

While this works for Python, not all test frameworks that need to support debugging of tests running in external processes would want their users to also install a VS extension. For this reason, debugging of TAEF tests is currently not supported in VS.

# Proposed Changes
1. Introduce a new `IFrameworkHandle2` interface that inherits [`IFrameworkHandle`](./src/Microsoft.TestPlatform.ObjectModel/Adapter/Interfaces/IFrameworkHandle.cs#L12) and adds the following `AttachDebuggerToProcess()` API that an adapter can invoke from within [`ITestExecutor.RunTests()`](./src/Microsoft.TestPlatform.ObjectModel/Adapter/Interfaces/ITestExecutor.cs#L23) to attach debugger to an already running process. 

```
namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter
{
    /// <summary>
    /// Handle to the framework which is passed to the test executors.
    /// </summary>
    public interface IFrameworkHandle2 : IFrameworkHandle
    {
        /// <summary>
        /// Attach debugger to an already running process.
        /// </summary>
        /// <param name="pid">Process ID of the process to which the debugger should be attached.</param>
        /// <returns><see langword="true"/> if the debugger was successfully attached to the requested process, <see langword="false"/> otherwise.</returns>
        bool AttachDebuggerToProcess(int pid);
    }
}
```

```
// Adapter's implementation of ITestExecutor.RunTests()
void ITestExecutor.RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
{
    ...

    if (runContext.IsBeingDebugged && frameworkHandle is IFrameworkHandle2 frameworkHandle2)
    {
        frameworkHandle2.AttachDebuggerToProcess(testProcessId);
    }

    ...
}
```

2. Introduce a new `ITestExecutor2` interface that inherits [`ITestExecutor`](./src/Microsoft.TestPlatform.ObjectModel/Adapter/Interfaces/ITestExecutor.cs#L15) and adds the following `ShouldAttachToTestHost()` API. Newer adapters can choose to implement `ITestExecutor2` instead of `ITestExecutor`. If implemented, `ITestExecutor.ShouldAttachToTestHost()` will be invoked before `ITestExecutor.RunTests()` is invoked for any test execution. `ITestExecutor2.ShouldAttachToTestHost()` will also be supplied the same set of inputs as `ITestExecutor.RunTests()`. This would allow adapters to control whether or not the debugger should be attached to `testhost*.exe` for the subsequent invocation of `ITestExecutor.RunTests()`.

```
namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter
{
    /// <summary>
    /// Defines the test executor which provides capability to run tests.  
    /// 
    /// A class that implements this interface will be available for use if its containing 
    //  assembly is either placed in the Extensions folder or is marked as a 'UnitTestExtension' type 
    //  in the vsix package.
    /// </summary>
    public interface ITestExecutor2 : ITestExecutor
    {
        /// <summary>
        /// Indicates whether or not the default test host process should be attached to.
        /// </summary>
        /// <param name="sources">Path to test container files to look for tests in.</param>
        /// <param name="runContext">Context to use when executing the tests.</param>
        /// <returns><see langword="true"/> if the default test host process should be attached to, <see langword="false"/> otherwise.</returns>
        bool ShouldAttachToTestHost(IEnumerable<string> sources, IRunContext runContext);

        /// <summary>
        /// Indicates whether or not the default test host process should be attached to.
        /// </summary>
        /// <param name="tests">Tests to be run.</param>
        /// <param name="runContext">Context to use when executing the tests.</param>
        /// <returns><see langword="true"/> if the default test host process should be attached to, <see langword="false"/> otherwise.</returns>
        bool ShouldAttachToTestHost(IEnumerable<TestCase> tests, IRunContext runContext);
    }
}
```

3. Introduce a new `ITestHostLauncher2` interface that inherits [`ITestHostLauncher`](./src/Microsoft.TestPlatform.ObjectModel/Client/Interfaces/ITestHostLauncher.cs#L11) and adds the following `AttachToProcess()` API. Visual Studio's Test Explorer will supply an implementation of this interface via [`IVsTestConsoleWrapper.RunTestsWithCustomTestHost()`](./src/Microsoft.TestPlatform.VsTestConsole.TranslationLayer/Interfaces/IVsTestConsoleWrapper.cs#L120) and `ITestHostLauncher2.AttachToProcess()` will be called when  `IFrameworkHandle2.AttachDebuggerToProcess()` is called within an adapter.

```
namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces
{
    /// <summary>
    /// Interface defining contract for custom test host implementations.
    /// </summary>
    public interface ITestHostLauncher2 : ITestHostLauncher
    {
        /// <summary>
        /// Attach to already running custom test host process.
        /// </summary>
        /// <param name="pid">Process ID of the process to attach.</param>
        /// <returns><see langword="true"/> if the attach was successful, <see langword="false"/> otherwise.</returns>
        bool AttachToProcess(int pid);

        /// <summary>
        /// Attach to already running custom test host process.
        /// </summary>
        /// <param name="pid">Process ID of the process to attach.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><see langword="true"/> if the attach was successful, <see langword="false"/> otherwise.</returns>
        bool AttachToProcess(int pid, CancellationToken cancellationToken);
    }
}

```
