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

- **Invocation**:
  - `dnx Microsoft.DotNet.SignCheck`
- **CLI Options**:
```
Microsoft.DotNet.SignCheck [options]

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

| File Extension | Platforms                  |
|----------------|----------------------------|
| .a             | macOS                      |
| .app           | macOS                      |
| .cab           | Windows                    |
| .deb           | Linux                      |
| .dll           | Windows, macOS, Linux      |
| .dylib         | macOS                      |
| .exe           | Windows, macOS, Linux      |
| .gz            | macOS, Linux               |
| .jar           | Windows                    |
| .js            | Windows, macOS, Linux      |
| .lzma          | Windows, macOS, Linux      |
| .macho         | macOS                      |
| .msi           | Windows                    |
| .msp           | Windows                    |
| .msu           | Windows                    |
| .nupkg         | Windows, macOS, Linux      |
| .pkg           | macOS                      |
| .ps1           | Windows                    |
| .ps1xml        | Windows                    |
| .psd1          | Windows                    |
| .psm1          | Windows                    |
| .rpm           | Linux                      |
| .so            | macOS                      |
| .tar           | macOS, Linux               |
| .tgz           | macOS, Linux               |
| .vsix          | Windows                    |
| .xml           | Windows, macOS, Linux      |
| .zip           | Windows, macOS, Linux      |

#### Detected via File Headers

| File Type                  | Platforms                  |
|----------------------------|----------------------------|
| Cab Files                  | Windows                    |
| EXE Files                  | Windows                    |
| Jar Files                  | Windows                    |
| Mach-O Files               | macOS                      |
| NuGet Packages             | Windows, macOS, Linux      |
| PE Files                   | Windows                    |
| VSIX Files                 | Windows                    |
| Zip Files                  | Windows, macOS, Linux      |
