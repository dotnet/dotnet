# Build Result Analysis

## Builds Failed
There are build failures in this build for pipelines that are not currently failing in master

### runtime [[link to build]](https://dev.azure.com/dnceng/public/_build/results?buildId=983865&view=results)
* Build Linux arm Debug AllSubsets_Mono / Build Product [[Report repository issue](https://helix.dot.net/r/i/1?&r=dotnet%2Fruntime)] [[Report infrastructure issue](https://helix.dot.net/r/i/2?&r=dotnet%2Fruntime)]
  * :small_blue_diamond: This step is not failing in the [most recent master build](http://bing.com)
  * ❌ [[Log]](https://dev.azure.com/dnceng/public/_build/results?buildId=983865&view=logs&j=a39b4a58-e33b-54f5-bd2c-8b6ea43701a1&t=673eb198-64b2-54fc-cc71-ec41376307e1&l=1625) src/mono/mono.proj(377,5): error MSB3073: The command "TARGET_BUILD_ARCH=arm PKG_CONFIG_PATH=/crossrootfs/arm/usr/lib/arm-linux-gnueabihf/pkgconfig cmake --build . --target install --config Debug --parallel 4" exited with code 2.
* Build Android x86 Debug AllSubsets_Mono / Build Product [[Report repository issue](https://helix.dot.net/r/i/1?&r=dotnet%2Fruntime)] [[Report infrastructure issue](https://helix.dot.net/r/i/2?&r=dotnet%2Fruntime)]
  * :small_blue_diamond: This step is not failing in the [most recent master build](http://bing.com)
  * ❌  [[Log]](https://dev.azure.com/dnceng/public/_build/results?buildId=983865&view=logs&j=758ff361-657e-52c7-2383-2c9b9ea87442&t=5bdb2093-eed3-5c87-c252-b435d4bddc4c&l=3038) src/mono/mono.proj(496,5): error MSB3073: The command "cmake --build . --target install --config Debug --parallel 4" exited with code 2.
* Build MacCatalyst x64 Debug AllSubsets_Mono / Build Product [[Report repository issue](https://helix.dot.net/r/i/1?&r=dotnet%2Fruntime)] [[Report infrastructure issue](https://helix.dot.net/r/i/2?&r=dotnet%2Fruntime)]
  * :small_blue_diamond: This step is not failing in the [most recent master build](http://bing.com)
  * ❌  [[Log]](https://dev.azure.com/dnceng/public/_build/results?buildId=983865&view=logs&j=758ff361-657e-52c7-2383-2c9b9ea87442&t=5bdb2093-eed3-5c87-c252-b435d4bddc4c&l=3038) src/mono/mono.proj(496,5): error MSB3073: The command "cmake --build . --target install --config Debug --parallel 3" exited with code 2. 
* 27 other jobs are failing


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CDevWorkflow%5CDesign%5CGitHub-Check-Mockup%5CUnique-Build-Failure.md)](https://helix.dot.net/f/p/5?p=Documentation%5CDevWorkflow%5CDesign%5CGitHub-Check-Mockup%5CUnique-Build-Failure.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CDevWorkflow%5CDesign%5CGitHub-Check-Mockup%5CUnique-Build-Failure.md)</sub>
<!-- End Generated Content-->
