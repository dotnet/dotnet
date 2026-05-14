# 0019 Disable Appdomain while Running Tests

## Summary
The RFC outlines why

1. Test adapters should honour disable app domain setting

2. Running of tests with disable appdomain is important for resiliency 

## Motivation
In past we have seen customer hitting issue with AppDomain.Unload. There are two main issues with AppDomain.Unload call

1. Hang in AppDomain.Unload (Tracking issue https://github.com/Microsoft/testfx/issues/225)

2. AppDomain.Unload call can crash the process even if you have an exception handler in code (check next section for details)


## Details of one such crash during Appdomain.Unload

Below is one of the analysis done for one of the crash dump while calling AppDomain.Unload

1.	An HWND (call it X) has a WndProc that is implemented in managed code.
2.	The app domain that owns the WndProc code shuts down.
3.	The OS delivers a message to window X.
4.	The CLR tries to run the WndProc, but throws AppDomainUnloadedException when it sees that the WndProc is in a dead app domain.
5.	The AppDomainUnloadedException propagates back into the OS window message dispatcher.
6.	The OS window message dispatcher immediately reports the exception as unhandled (ignoring all exception handlers that might exist "further back" on the stack), which generally has the same effect as a failfast and tears down the process.


## Proposed changes

Proposed guidelines are for customers and test adapters who wants to avoid these issues.

**Adapters**
1. Test Adapter should honour ```<DisableAppDomain>``` setting inside RunConfiguration node of runsettings. Check ./docs/configure.md for information on this setting. This will ensure that adapters dont create AppDomain at all to run tests

**Test platform**

1. Change in test platform to merge the app.config for a test assembly when ```<DisableAppDomain>``` is set. This is to ensure test's app.config is honoured while running tests

2. Make sure when ```<DisableAppDomain>``` is set, each test source have isolation. This is done by spawning testhost process for each test source.
