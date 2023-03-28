# Microsoft.DiaSymReader

Provides managed definitions for COM interfaces exposed by DiaSymReader APIs ([ISymUnmanagedReader](https://msdn.microsoft.com/en-us/library/ms232131.aspx), [ISymUnmanagedBinder](https://msdn.microsoft.com/en-us/library/ms232451.aspx), etc.) for reading Windows and [Portable PDBs](https://github.com/dotnet/core/blob/main/Documentation/diagnostics/portable_pdb.md).

The implementation of these interfaces for Windows PDBs is provided by [Microsoft.DiaSymReader.Native](https://www.nuget.org/packages/Microsoft.DiaSymReader.Native) package. The implementation for Portable PDBs is provided by [Microsoft.DiaSymReader.PortablePdb](https://www.nuget.org/packages/Microsoft.DiaSymReader.PortablePdb) package. 

It is recommended that new applications and libraries read Portable PDBs directly using APIs provided by [System.Reflection.Metadata](https://www.nuget.org/packages/System.Reflection.Metadata) package. These APIs are much more efficient than DiaSymReader APIs. Microsoft.DiaSymReader.PortablePdb bridge is recommended for existings apps that already use DiaSymReader APIs and need to be able to read Portable PDBs without significant changes to their source.

Pre-release builds are available from Azure DevOps public feed: `https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json` ([browse](https://dev.azure.com/dnceng/public/_packaging?_a=feed&feed=dotnet-tools)).

[![Build Status](https://dnceng.visualstudio.com/public/_apis/build/status/dotnet/symreader/SymReader%20PR?branchName=main)](https://dnceng.visualstudio.com/public/_build/latest?definitionId=291&branchName=main)
