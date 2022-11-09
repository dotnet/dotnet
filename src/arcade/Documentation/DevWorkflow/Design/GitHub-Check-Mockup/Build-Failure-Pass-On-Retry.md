# Build Result Analysis

## Build Rerun because of non-deterministic failure
:small_blue_diamond: Due to ongoing networking issues, Azure Pipeline Key Vault access fails intermittently. *This text is from a "known retry issue" stored somewhere, currently no plans to retry builds without known issues*

Issue tracking this failure [dotnet/runtime#1234](https://github.com/dotnet/runtime/issues/1234)

### (Attempt #1) runtime [[link to build]](https://dev.azure.com/dnceng/public/_build/results?buildId=983865&view=results)
* Build Linux arm Debug AllSubsets_Mono / Build Product
  * [[Log]](https://dev.azure.com/dnceng/public/_build/results?buildId=983865&view=logs&j=a39b4a58-e33b-54f5-bd2c-8b6ea43701a1&t=673eb198-64b2-54fc-cc71-ec41376307e1&l=1625) src/mono/mono.proj(377,5): error MSB3073: The command "TARGET_BUILD_ARCH=arm PKG_CONFIG_PATH=/crossrootfs/arm/usr/lib/arm-linux-gnueabihf/pkgconfig cmake --build . --target install --config Debug --parallel 4" exited with code 2.
  * [[Log]](https://dev.azure.com/dnceng/public/_build/results?buildId=983865&view=logs&j=a39b4a58-e33b-54f5-bd2c-8b6ea43701a1&t=673eb198-64b2-54fc-cc71-ec41376307e1&l=1625) command completed with exit code 1
* Build Android x86 Debug AllSubsets_Mono / Build Product
  * [[Log]](https://dev.azure.com/dnceng/public/_build/results?buildId=983865&view=logs&j=758ff361-657e-52c7-2383-2c9b9ea87442&t=5bdb2093-eed3-5c87-c252-b435d4bddc4c&l=3038) src/mono/mono.proj(496,5): error MSB3073: The command "cmake --build . --target install --config Debug --parallel 4" exited with code 2
  * [[Log]](https://dev.azure.com/dnceng/public/_build/results?buildId=983865&view=logs&j=a39b4a58-e33b-54f5-bd2c-8b6ea43701a1&t=673eb198-64b2-54fc-cc71-ec41376307e1&l=1625) command completed with exit code 1


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CDevWorkflow%5CDesign%5CGitHub-Check-Mockup%5CBuild-Failure-Pass-On-Retry.md)](https://helix.dot.net/f/p/5?p=Documentation%5CDevWorkflow%5CDesign%5CGitHub-Check-Mockup%5CBuild-Failure-Pass-On-Retry.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CDevWorkflow%5CDesign%5CGitHub-Check-Mockup%5CBuild-Failure-Pass-On-Retry.md)</sub>
<!-- End Generated Content-->
