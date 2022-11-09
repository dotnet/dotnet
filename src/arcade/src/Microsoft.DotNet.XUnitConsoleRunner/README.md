# Custom Version of the Xunit Console Runner

## Origin/Attribution
This is a fork of the code in https://github.com/xunit/xunit for building the `Microsoft.DotNet.XUnitConsoleRunner` NuGet package.
The original authors of this code are Brad Wilson and Oren Novotny.  See `../../THIRD-PARTY-NOTICES.TXT` for
the license for this code.

## Significant Changes
* Added Response File parsing to `src/CommandLine.cs`
* Fixed a race condition in `src/common/AssemblyResolution/DependencyContextAssemblyCache.cs` that manifests themself as
```
System.IO.FileLoadException: Could not load file or assembly 'Exceptions.Finalization.XUnitWrapper, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'.
An operation is not legal in the current state. (Exception from HRESULT: 0x80131509 (COR_E_INVALIDOPERATION))
```
