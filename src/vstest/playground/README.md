# Playground

This Plaground directory contains projects to aid interactive debugging of test platform. TestPlatform is normally built
as a set of distinct pieces and then assembled in the artifacts folder. This forces rebuilding using build.cmd to try out 
changes. The TestPlatform.Playground project builds a simpler version of TestPlatform to avoid always rebuilding via
build.cmd, offering a tighther development loop.

The project references TranslationLayer, vstest.console, TestHostProvider, testhost and MSTest1 projects, to make sure
we build all the dependencies of that are used to run tests via VSTestConsoleWrapper. It then copies the components from
their original build locations, to $(TargetDir)\vstest.console directory, and it's subfolders to create an executable
copy of TestPlatform that is similar to what we ship.

The copying might trigger only on re-build, if you see outdated dependencies, Rebuild this project instead of just Build.
 
Use this as playground for your debugging of end-to-end scenarios, it will automatically attach vstest.console and teshost
sub-processes. It won't stop at entry-point automatically, don't forget to set your breakpoints, or remove VSTEST_DEBUG_NOBP
from the environment variables of this project.