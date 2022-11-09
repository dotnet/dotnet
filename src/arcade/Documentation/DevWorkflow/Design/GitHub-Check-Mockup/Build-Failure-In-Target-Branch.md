# Build Result Analysis

## Shared test failures with Target Branch

⚠️ There are build failures in this build similar to failures in pipelines in master

Issue tracking this failure [dotnet/runtime#1234](https://github.com/dotnet/runtime/issues/1234)

### runtime [[link to build]](https://dev.azure.com/dnceng/public/_build/results?buildId=983865&view=results)
* Build Linux arm Debug AllSubsets_Mono / Build Product
  * This step first failed in master on 2020-02-08, in build [20200208.13](https://dev.azure.com/dnceng/public/_build/results?buildId=983865&view=logs&j=a39b4a58-e33b-54f5-bd2c-8b6ea43701a1&t=673eb198-64b2-54fc-cc71-ec41376307e1&l=1625)
  * This step has failed 8 out of 24 runs in master, most recently on 2020-02-09 in build [20200209.11](https://dev.azure.com/dnceng/public/_build/results?buildId=983865&view=logs&j=a39b4a58-e33b-54f5-bd2c-8b6ea43701a1&t=673eb198-64b2-54fc-cc71-ec41376307e1&l=1625)
  * Errors in this PR:
    * [[Log]](https://dev.azure.com/dnceng/public/_build/results?buildId=983865&view=logs&j=a39b4a58-e33b-54f5-bd2c-8b6ea43701a1&t=673eb198-64b2-54fc-cc71-ec41376307e1&l=1625) src/mono/mono.proj(377,5): error MSB3073: The command "TARGET_BUILD_ARCH=arm PKG_CONFIG_PATH=/crossrootfs/arm/usr/lib/arm-linux-gnueabihf/pkgconfig cmake --build . --target install --config Debug --parallel 4" exited with code 2.
* Build Android x86 Debug AllSubsets_Mono / Build Product
  * This step first failed in master on 2020-02-08, in build [20200208.13](https://dev.azure.com/dnceng/public/_build/results?buildId=983865&view=logs&j=a39b4a58-e33b-54f5-bd2c-8b6ea43701a1&t=673eb198-64b2-54fc-cc71-ec41376307e1&l=1625)
  * This step has failed 9 out of 15 runs in master, most recently on 2020-02-09 in build [20200209.11](https://dev.azure.com/dnceng/public/_build/results?buildId=983865&view=logs&j=a39b4a58-e33b-54f5-bd2c-8b6ea43701a1&t=673eb198-64b2-54fc-cc71-ec41376307e1&l=1625)
  * Errors in this PR:
    * [[Log]](https://dev.azure.com/dnceng/public/_build/results?buildId=983865&view=logs&j=758ff361-657e-52c7-2383-2c9b9ea87442&t=5bdb2093-eed3-5c87-c252-b435d4bddc4c&l=3038) src/mono/mono.proj(496,5): error MSB3073: The command "cmake --build . --target install --config Debug --parallel 4" exited with code 2.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CDevWorkflow%5CDesign%5CGitHub-Check-Mockup%5CBuild-Failure-In-Target-Branch.md)](https://helix.dot.net/f/p/5?p=Documentation%5CDevWorkflow%5CDesign%5CGitHub-Check-Mockup%5CBuild-Failure-In-Target-Branch.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CDevWorkflow%5CDesign%5CGitHub-Check-Mockup%5CBuild-Failure-In-Target-Branch.md)</sub>
<!-- End Generated Content-->
