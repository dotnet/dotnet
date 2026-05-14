# 0014 - Packaging
## Motivation
As mentioned in the roadmap, vstest is looking to broaden its reach to all scenarios across Visual Studio and Visual Studio Team Services (VSTS) and Team Foundation Server (TFS) – i.e. extending it to .NET Framework, the VSTest task in VSTS and in TFS. We aspire to ship a standalone package that can be potentially used in other CI systems even.
Appropriate packaging is essential for such broad reach.
The following document describes how the Visual Studio Test Platform will be packaged.

## Packaging
Conceptually, the capabilities fall into these two categories:
1. a subset of the capabilities that allows you to run tests and that is cross-plat, and that we have ported to Windows/Mac/Linux.
2. the full set of capabilities that have always shipped in-box with Visual Studio, and that are mostly Windows only (for e.g. some of the adapters  are supported only on Windows, and shipped in-box with Visual Studio).

Accordingly, we will create the following NuGet packages:
### Microsoft.TestPlatform.Portable
Contents:
- Runner: vstest.console.exe, vstest.console.dll)
- In-box adapters: None
- In-box data collectors: Blame datacollector (and any other data collectors which are supported crossplat)
- In-box loggers: trx
- Supports legacy test execution via TMI: No

### Microsoft.TestPlatform
Contents:
 - Microsoft.TestPlatform.Portable +
 - In-box adapters: MSTest V1, Ordered Test, Generic Test, Web test adapters
 - In-box data collectors: CodeCoverage, Fakes, TIA, video, SysInfo
 - Supports legacy test execution via TMI: Yes
 
## Versioning
The NuGet packages will continue to follow VS versioning. For e.g. 15.5.0-preview-2017MMDD-XY (XY are # of build of that day). 
