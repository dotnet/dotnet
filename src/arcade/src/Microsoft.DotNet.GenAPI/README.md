Microsoft.DotNet.GenAPI
===============================

Contains GenAPITask - MSBuild task that allows to synthesize C# source tree out of a dll.
The project is build on top of Microsoft.CCI and Microsoft.CCI.Extensions.

Currently, new Roslyn-based GenAPIv2 is in development in folders:
* Shared - common library for Tool and Tasks.
* Tasks - MSBuild tasks for GenAPIv2.
* Tool - CLI tool used to generate reference assemblies out of executable and produce output in console or to file.

We'll remove the original CCI-based implementation after GenAPIv2 is ready.