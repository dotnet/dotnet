# Test Platform Roadmap

This repo is the modern, OSS, cross-plat testing engine that has been powering testing on .NET Core via the "test" verb in dotnet test, and Live Unit Testing scenario (LUT) in Visual Studio. Internally we call this repo "TPV2" (Test Platform V2)

We aim to continuously deliver improvements that will ship with Visual Studio and with the .NET Tools SDK. These improvements are directly informed by your feedback filed as [issues](https://github.com/Microsoft/vstest/issues). If you do not see your issue addressed already, we will get to it soon! If you would like to help out, let us know!

Over the past several quarters, we have made many enhancements - from introducing support for Mono, to refactoring the platform to make it ready to support device testing, to performance improvements, to enabling robust C++ support, to improved documentation, and more. For a complete list see here: [Release Notes](./releases.md)

## Roadmap

We typically plan for a quarter, and establish a set of themes we want to work towards. Here are the themes we will work on this quarter.

### Reach: Enable leveraging your vstest experience across all supported platforms

Over the course of the next phase of execution we will make TPV2 the "default" for all scenarios across Visual Studio and Visual Studio Team Services (VSTS) – i.e. extending it to .NET Framework, UWP, and the VSTest task in VSTS. We will ship a standalone package that can be potentially used in other CI systems even. This is a big switch. We will strive to maintain backwards compat, and publish migration guides for the few features that require to be migrated, and help you in the migration.

### Performance: At scale

Performance has been an area where we have received feedback, and made strong progress as well. It will continue to remain a focus. We will look to make improvements across the pipeline from the Test Explorer to the framework adapters, to enhance the overall end to end performance.

### UWP, Win10 IoT Core Support

UWP is the application platform for Windows 10, to reach all Windows 10 devices – PC, tablet, phone, Xbox, HoloLens, Surface Hub and more. The vstest engine is architected so that it can be extended to support new application platforms. Such extensions will come from teams who understand their platforms the best, and integrated with vstest. To drive home this point, vstest will be extended to support testing UWP applications. In particular we will light up support for Win10 IoT Core.

### Code Coverage for .NET Core

This has been a clear ask from the community, and we are working towards enabling this support. The code coverage infrastructure consumes information from PDB files. Specifically with regard to .NET Core, it now needs to understand the new portable PDB format. We are working cross-team to introduce this support in order to light up code coverage support for .NET Core.

## Summary

These are examples of the work we will be focusing on this quarter. We will provide details through individual issues. Follow along, and let us know what you think. We look forward to working with you!
