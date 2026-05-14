# Sign Tool

This is a MSBuild custom task that provides batch signing and verification for
MicroBuild environments. The tool is intended to be run as a post-build step and
is able to automatically infer the files that need to be signed given a list of
container files (.nupkg, .vsix, etc) as input.

For additional information on this package, please see the [documentation
located here](../../Documentation/CorePackages/Signing.md)
