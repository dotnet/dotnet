SignCheck
=========

This console tool scans files, archives, and packages to ensure their contents have Authenticode signatures.

### Usage

```
Microsoft.DotNet.SignCheck.exe [options]

Options:

  -e, --error-log-file              Log errors to a separate file. If the file already exists it will be overwritten.

  -f, --file-status                 Report the status of a speficic set of files. Any combination of the following values are allowed. 
                                    Values are separated by a ','.
                                    
                                    'UnsignedFiles', 'SignedFiles', 'SkippedFiles', 'ExcludedFiles', 'AllFiles'. Default is 'UnsignedFiles'

  -g, --generate-exclusions-file    Name of the exclusions file to generate. The entries in the file are generated using reported 
                                    unsigned files. If the file already exists it will be overwritten.

  -i, --input-files                 A list of files to scan. Wildcards (* and ?) are supported. You can specify groups of files, 
                                    e.g. C:\Dir1\Dir*\File?.EXE or a URL (http or https).

  -j, --verify-jar                  Enable JAR signature verification. By default, .jar files are no verified.

  -l, --log-file                    Output results to the specified log file. If the file already exists it will be overwritten.

  -m, --verify-xml                  Enable XML signature verification. By default, .xml files are not verified.

  -p, --skip-timestamp              Ignore timestamp checks for AuthentiCode signatures.

  -r, --recursive                   Traverse subdirectories or container files such as .zip, .nupkg, .cab, and .msi

  -s, --verify-strongname           Enable strongname checks for managed code files (.exe and .dll)

  -t, --traverse-subfolders         Traverse subfolders to find files matching wildcard patterns used by the --input-files option.

  -v, --verbosity                   Set the verbosity level: Minimum, Normal, Detailed, Diagnostic.

  -x, --exclusions-file             Path to a file containing a list of files to ignore when verification fails. Exclusions are not 
                                    reported as errors.

  --help                            Display this help screen.

  --version                         Display version information.
```
