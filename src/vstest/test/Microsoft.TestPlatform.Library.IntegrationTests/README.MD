Integration tests that run inside of the test process, some work might be delegated to child process,
but we are testing a component that is loaded into the process, e.g. VSTestConsoleWrapper. For this reason we need to
multi target the test project to ensure that the component works correctly in all supported TFMs.
