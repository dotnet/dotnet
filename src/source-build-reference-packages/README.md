# .NET Source-build Reference Packages

This repository contains source and build scripts for source-buildable reference versions of historical .NET Core packages that are referenced by the [source-build](https://github.com/dotnet/source-build) repo.  These are used only when building the source-build repo.

### Continuous builds

Microsoft build outputs from this repo are published by Arcade to blob storage.
They are used by source-build when prebuilts are acceptable to avoid rebuilding
the repo. This repo doesn't change often, making a cache particularly useful.

### Build on Linux or MacOS

```
./build.sh
```

## License

This repo is licensed with [MIT](LICENSE.txt).
