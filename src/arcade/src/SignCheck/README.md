SignCheck
=========

`SignCheck` is a tool that scans files, archives, and packages to ensure their contents have valid signatures.

### Usage

The `SignCheck` tooling is divided into two components, a task and a CLI tool.

#### Signing Task

Arcade defaults to using the signing task via script invocation for signing validation. This is the preferred method for signature validation.

- **Supported Frameworks**: .NET Core and .NET Framework
- **Invocation**:
  - On Linux/macOS: `./eng/common/sdk-task.sh --task SigningValidation`
  - On Windows: `./eng/common/sdk-task.ps1 -task SigningValidation`
- **Task Options**:
  - **Input Files**: `/p:PackageBasePath` (required)  
    A list of files to scan. Wildcards (* and ?) are supported.  
  - **Exclusions File**: `/p:SignCheckExclusionsFile` 
    Path to a file containing a list of files to ignore when verification fails.  
  - **Enable JAR Signature Verification**: `/p:EnableJarSigningCheck` (default: false)  
    Enable JAR signature verification.  
  - **Verify Strong Name**: `/p:EnableStrongNameCheck` (default: false)  
    Enable strong name checks for managed code files.  
  - **Log File**: `/p:SignCheckLog`
    Output results to the specified log file.  
  - **Error Log File**: `/p:SignCheckErrorLog`
    Log errors to a separate file.  
  - **Results XML File**: `/p:SignCheckResultsXmlFile`
    Output signing results to the specified XML log file.  

#### Signing CLI Tool

The CLI tool is maintained for legacy purposes and is only recommended for repositories that already use it. Refrane from using this; new repositories should use the Signing Task instead.

- **Supported Frameworks**: .NET Framework only
- **Invocation**:
  - `Microsoft.DotNet.SignCheck.exe`
- **CLI Options**:
```
Microsoft.DotNet.SignCheck.exe [options]

Options:

  -e, --error-log-file              Log errors to a separate file. If the file already exists it will be overwritten.

  -f, --file-status                 Report the status of a specific set of files. Default is 'UnsignedFiles'.

  -g, --generate-exclusions-file    Name of the exclusions file to generate. If the file already exists it will be overwritten.

  -i, --input-files                 A list of files to scan. Wildcards (* and ?) are supported.

  -j, --verify-jar                  Enable JAR signature verification. By default, .jar files are not verified.

  -l, --log-file                    Output results to the specified log file. If the file already exists it will be overwritten.

  --results-xml-file                Output signing results to the specified XML log file. If the file already exists it will be overwritten.

  -m, --verify-xml                  Enable XML signature verification. By default, .xml files are not verified.

  -p, --skip-timestamp              Ignore timestamp checks for AuthentiCode signatures.

  -r, --recursive                   Traverse subdirectories or container files such as .zip, .nupkg, .cab, and .msi.

  -s, --verify-strongname           Enable strong name checks for managed code files.

  -t, --traverse-subfolders         Traverse subfolders to find files matching wildcard patterns.

  -v, --verbosity                   Set the verbosity level: Minimum, Normal, Detailed, Diagnostic.

  -x, --exclusions-file             Path to a file containing a list of files to ignore when verification fails.

  --help                            Display this help screen.

  --version                         Display version information.
```

### Supported Files

#### Detected via File Extensions

| File Extension | Platforms                  | .NET Product         |
|----------------|----------------------------|----------------------|
| .a             | macOS                      | .NET Core            |
| .app           | macOS                      | .NET Core            |
| .cab           | Windows                    | .NET Framework       |
| .deb           | Linux                      | .NET Core            |
| .dll           | Windows, macOS, Linux      | .NET Framework, Core |
| .dylib         | macOS                      | .NET Core            |
| .exe           | Windows, macOS, Linux      | .NET Framework, Core |
| .gz            | macOS, Linux               | .NET Core            |
| .jar           | Windows                    | .NET Framework       |
| .js            | Windows, macOS, Linux      | .NET Framework, Core |
| .lzma          | Windows, macOS, Linux      | .NET Framework, Core |
| .macho         | macOS                      | .NET Core            |
| .msi           | Windows                    | .NET Framework       |
| .msp           | Windows                    | .NET Framework       |
| .msu           | Windows                    | .NET Framework       |
| .nupkg         | Windows, macOS, Linux      | .NET Framework, Core |
| .pkg           | macOS                      | .NET Core            |
| .ps1           | Windows                    | .NET Framework       |
| .ps1xml        | Windows                    | .NET Framework       |
| .psd1          | Windows                    | .NET Framework       |
| .psm1          | Windows                    | .NET Framework       |
| .rpm           | Linux                      | .NET Core            |
| .so            | macOS                      | .NET Core            |
| .tar           | macOS, Linux               | .NET Core            |
| .tgz           | macOS, Linux               | .NET Core            |
| .vsix          | Windows                    | .NET Framework       |
| .xml           | Windows, macOS, Linux      | .NET Framework, Core |
| .zip           | Windows, macOS, Linux      | .NET Framework, Core |

#### Detected via File Headers

| File Type                  | Platforms                  | .NET Product         |
|----------------------------|----------------------------|----------------------|
| Cab Files                  | Windows                    | .NET Framework       |
| EXE Files                  | Windows                    | .NET Framework       |
| Jar Files                  | Windows                    | .NET Framework       |
| Mach-O Files               | macOS                      | .NET Core            |
| NuGet Packages             | Windows, macOS, Linux      | .NET Framework, Core |
| PE Files                   | Windows                    | .NET Framework       |
| VSIX Files                 | Windows                    | .NET Framework       |
| Zip Files                  | Windows, macOS, Linux      | .NET Framework, Core |
