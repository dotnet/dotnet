## How to create a Known Issue: step by step
This is a summarized version of our documentation, you can always look at [Complete known issues documentation](https://github.com/dotnet/arcade/blob/main/Documentation/Projects/Build%20Analysis/KnownIssues.md#filling-out-known-issues-json-blob) or at [Runtime known issues documentation](https://github.com/dotnet/runtime/blob/main/docs/workflow/ci/failure-analysis.md) for more details.

1. Select the line of the error message that you want to match. Choose the line that best represents the failure.
1. Decide if you need to use “ErrorMessage” or “ErrorPattern”
    - "ErrorMessage" works as `contains` (single line, case-insensitive)
    - "ErrorPattern" works as `regex` (single line, case-insensitive, no backtracking)
1. Write the error message: <sup>  [Additional documentation on how to write an error message](https://github.com/dotnet/arcade/blob/main/Documentation/Projects/Build%20Analysis/KnownIssues.md#filling-out-known-issues-json-blob)</sup>
    - If you use “ErrorMessage” select the part of the line that you want to match, use [string-functions](https://string-functions.com/countsubstrings.aspx) to test it.
    - If you use "ErrorPattern" write the regex that matches your error message. Use [regex101](https://regex101.com/) (choose `.NET (C#)` flavor with `Single line`, `Insensitive`, `No backtracking` flags enabled) to test it.
    - To escape special characters in json, use: [freeformatter](https://www.freeformatter.com/json-escape.html).
1. Set BuildRetry to `true` if you would like to enable automatic build retries for any build that matches the error.
1. Set ExcludeConsoleLog to `true` if you would like to exclude the Helix console logs from the search space. 

Tip: Look at [.NET Core Engineering Services: Known Build Errors Project](https://github.com/orgs/dotnet/projects/111/views/2) to see other known issues and use them as examples.

## Examples
Expand/Collapse the examples to navigate between them more easily.

<details open>
  <summary><b>Example 1: ErrorMessage</b></summary>
  
**Error**
```
CMake Error at /usr/share/cmake-3.21/Modules/FindPackageHandleStandardArgs.cmake:230 (message):
    Could NOT find ZLIB (missing: ZLIB_LIBRARY ZLIB_INCLUDE_DIR)
Call Stack (most recent call first):
    /usr/share/cmake-3.21/Modules/FindPackageHandleStandardArgs.cmake:594 (_FPHSA_FAILURE_MESSAGE)
    /usr/share/cmake-3.21/Modules/FindZLIB.cmake:120 (FIND_PACKAGE_HANDLE_STANDARD_ARGS)
    /__w/1/s/src/native/libs/System.IO.Compression.Native/extra_libs.cmake:12 (find_package)
    CMakeLists.txt:532 (append_extra_compression_libs)

```

**Known issue json:**

    ```json
    {
    "ErrorMessage": "Could NOT find ZLIB (missing: ZLIB_LIBRARY ZLIB_INCLUDE_DIR)",
    "BuildRetry": false,
    "ExcludeConsoleLog": false
    }
    ```

**Explanation:**
We selected the error line and copied and pasted it on error message, as this will work as a case-insensitive contains 
</details>

<details open>
  <summary><b>Example 2: ErrorPatttern</b></summary>


### 
**Error**
```
Restored /__w/1/s/src/coreclr/tools/SuperFileCheck/SuperFileCheck.csproj (in 5.22 sec).
  Restored /__w/1/s/src/coreclr/tools/dotnet-pgo/dotnet-pgo.csproj (in 8.42 sec).
  Restored /__w/1/s/src/coreclr/tools/aot/crossgen2/crossgen2.csproj (in 5.66 sec).
CSC : error CS8034: Unable to load Analyzer assembly /__w/1/s/.packages/microsoft.codeanalysis.analyzers/3.3.3/analyzers/dotnet/cs/Microsoft.CodeAnalysis.Analyzers.dll : Not a valid assembly: /__w/1/s/.packages/microsoft.codeanalysis.analyzers/3.3.3/analyzers/dotnet/cs/Microsoft.CodeAnalysis.Analyzers.dll [/__w/1/s/src/libraries/System.Private.CoreLib/gen/System.Private.CoreLib.Generators.csproj]
##[error]CSC(0,0): error CS8034: (NETCORE_ENGINEERING_TELEMETRY=Build) Unable to load Analyzer assembly /__w/1/s/.packages/microsoft.codeanalysis.analyzers/3.3.3/analyzers/dotnet/cs/Microsoft.CodeAnalysis.Analyzers.dll : Not a valid assembly: /__w/1/s/.packages/microsoft.codeanalysis.analyzers/3.3.3/analyzers/dotnet/cs/Microsoft.CodeAnalysis.Analyzers.dll
CSC : error CS8032: An instance of analyzer Microsoft.CodeAnalysis.CSharp.Analyzers.CSharpImmutableObjectMethodAnalyzer cannot be created from /__w/1/s/.packages/microsoft.codeanalysis.analyzers/3.3.3/analyzers/dotnet/cs/Microsoft.CodeAnalysis.CSharp.Analyzers.dll : Exception has been thrown by the target of an invocation.. [/__w/1/s/src/libraries/System.Private.CoreLib/gen/System.Private.CoreLib.Generators.csproj]
##[error]CSC(0,0): error CS8032: (NETCORE_ENGINEERING_TELEMETRY=Build) An instance of analyzer Microsoft.CodeAnalysis.CSharp.Analyzers.CSharpImmutableObjectMethodAnalyzer cannot be created from /__w/1/s/.packages/microsoft.codeanalysis.analyzers/3.3.3/analyzers/dotnet/cs/Microsoft.CodeAnalysis.CSharp.Analyzers.dll : Exception has been thrown by the target of an invocation..
CSC : error CS8032: An instance of analyzer Microsoft.CodeAnalysis.CSharp.Analyzers.CSharpUpgradeMSBuildWorkspaceAnalyzer cannot be created from /__w/1/s/.packages/microsoft.codeanalysis.analyzers/3.3.3/analyzers/dotnet/cs/Microsoft.CodeAnalysis.CSharp.Analyzers.dll : Could not load file or assembly 'Microsoft.CodeAnalysis.Analyzers, Version=3.3.5.2003, Culture=neutral, PublicKeyToken=31bf3856ad364e35'. The system cannot find the file specified.. [/__w/1/s/src/libraries/System.Private.CoreLib/gen/System.Private.CoreLib.Generators.csproj]
##[error]CSC(0,0): error CS8032: (NETCORE_ENGINEERING_TELEMETRY=Build) An instance of analyzer Microsoft.CodeAnalysis.CSharp.Analyzers.CSharpUpgradeMSBuildWorkspaceAnalyzer cannot be created from /__w/1/s/.packages/microsoft.codeanalysis.analyzers/3.3.3/analyzers/dotnet/cs/Microsoft.CodeAnalysis.CSharp.Analyzers.dll : Could not load file or assembly 'Microsoft.CodeAnalysis.Analyzers, Version=3.3.5.2003, Culture=neutral, PublicKeyToken=31bf3856ad364e35'. The system cannot find the file specified..
```

**Known issue json:**
(Please notice that there are some characters escaped)

```json
{
  "ErrorPattern": "An instance of analyzer Microsoft\\.CodeAnalysis\\.CSharp\\.Analyzers\\..* cannot be created from",
  "BuildRetry": false,
  "ExcludeConsoleLog": false
}
```

**Explanation:**
 The main error is `an instance of the analyzer cannot be created...` but the instance of the analyzer can change (Ex .CSharpImmutableObjectMethodAnalyzer or CSharpUpgradeMSBuildWorkspaceAnalyzer). Therefore, we need to use a regex. Keep in mind that the regex matches one line and does not backtrack. 
</details>

<details open>
  <summary><b>Example 3: BuildRetry</b></summary>

**Error:**

```
A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond. (pkgs.dev.azure.com:443)

A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond.

D:\a\1\s\src\Directory.Build.props(3,3): warning : Failed to retrieve information about 'Microsoft.DotNet.Arcade.Sdk' from remote source 'https://pkgs.dev.azure.com/dnceng/9ee6d478-d288-47f7-aacc-f6e6d082ae6d/_packaging/1a5f89f6-d8da-4080-b15f-242650c914a8/nuget/v3/flat2/microsoft.dotnet.arcade.sdk/index.json'.
  ```

**Known issue json:**

```json
{
   "ErrorMessage" : "Failed to retrieve information about",
   "BuildRetry": true
}
```

**Explanation:** This is a flaky failure that is related to a connection issue. It’s highly likely that the problem will be resolved by just retrying the build, so we set BuildRetry to true.

</details>