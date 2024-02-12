# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@f7eb779](https://github.com/dotnet/arcade/tree/f7eb7794c703dc29a83b414b786e9a154f0ca042)*
- `src/aspire`  
*[dotnet/aspire@87d5246](https://github.com/dotnet/aspire/tree/87d5246ddfc1fb9b07fcdf7b4b42830f67427ab9)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@4deb60b](https://github.com/dotnet/aspnetcore/tree/4deb60bb65b090c50a0595452266ad73d4dcc68b)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@4872968](https://github.com/google/googletest/tree/48729681ad88a89061344ee541b4548833077e00)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@b8c2293](https://github.com/dotnet/cecil/tree/b8c2293cd1cbd9d0fe6f32d7b5befbd526b5a175)*
- `src/command-line-api`  
*[dotnet/command-line-api@46fea71](https://github.com/dotnet/command-line-api/tree/46fea71e3d98dad0d676950522004b7f295dd372)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@9abdab1](https://github.com/dotnet/deployment-tools/tree/9abdab1d923b427c26685d793e9ddc8344f3da5c)*
- `src/diagnostics`  
*[dotnet/diagnostics@79b59c5](https://github.com/dotnet/diagnostics/tree/79b59c505405b9bee1d62dfa73dfb9750b2d4376)*
- `src/emsdk`  
*[dotnet/emsdk@329f74d](https://github.com/dotnet/emsdk/tree/329f74da67c0620b98574dfa6e46e03a03f21955)*
- `src/format`  
*[dotnet/format@e07ea47](https://github.com/dotnet/format/tree/e07ea4767a72b0f81457599e92d9a89ae9cf18bd)*
- `src/fsharp`  
*[dotnet/fsharp@099e0ae](https://github.com/dotnet/fsharp/tree/099e0ae7d788bb0f53f14b5c349012c33bff3372)*
- `src/installer`  
*[dotnet/installer@aca78d6](https://github.com/dotnet/installer/tree/aca78d6450b8ec4d6dafc628288ca0c1721d27f3)*
- `src/msbuild`  
*[dotnet/msbuild@e71eb7a](https://github.com/dotnet/msbuild/tree/e71eb7a1fc535b438007286c840d7cecc139d13b)*
- `src/nuget-client`  
*[nuget/nuget.client@280d053](https://github.com/nuget/nuget.client/tree/280d053fecd4517ade3bd89e543f5c8df18a1063)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@954f475](https://github.com/dotnet/razor/tree/954f47532c3a6769b86a23c835ce3fc1ef7719e4)*
- `src/roslyn`  
*[dotnet/roslyn@2fa19b0](https://github.com/dotnet/roslyn/tree/2fa19b085cdd5bd3c69c3d66dd53aa3b65c843c8)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@68c643b](https://github.com/dotnet/roslyn-analyzers/tree/68c643b4667c6808bd21910ef32f7e2f7bd776c5)*
- `src/runtime`  
*[dotnet/runtime@a79c62d](https://github.com/dotnet/runtime/tree/a79c62ddc8089cf2879ed36eac9aa333b32bde5f)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@bfde902](https://github.com/dotnet/scenario-tests/tree/bfde902a10d7b672f4fc7e844198ede405dbb9c6)*
- `src/sdk`  
*[dotnet/sdk@b32df9a](https://github.com/dotnet/sdk/tree/b32df9afff77e8f1afbd417e7c70841867cb6cdc)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@f1ef074](https://github.com/dotnet/source-build-externals/tree/f1ef074dfcf79d2f2da6e6ff9df8696a32aa063c)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[Microsoft/ApplicationInsights-dotnet@5e2e7dd](https://github.com/Microsoft/ApplicationInsights-dotnet/tree/5e2e7ddda961ec0e16a75b1ae0a37f6a13c777f5)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@a607fa5](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/a607fa5e0005a6178cf1d2fed4fa0f8179cdb186)*
    - `src/source-build-externals/src/cssparser`  
    *[dotnet/cssparser@0d59611](https://github.com/dotnet/cssparser/tree/0d59611784841735a7778a67aa6e9d8d000c861f)*
    - `src/source-build-externals/src/docker-creds-provider-2.2.0`  
    *[mthalman/docker-creds-provider@5701f66](https://github.com/mthalman/docker-creds-provider/tree/5701f6667c1fbd805684857baaa860383bbdfed7)*
    - `src/source-build-externals/src/docker-creds-provider-2.2.1`  
    *[mthalman/docker-creds-provider@6c73fa4](https://github.com/mthalman/docker-creds-provider/tree/6c73fa4784795ae07f49305a057abf5c473d2adb)*
    - `src/source-build-externals/src/humanizer`  
    *[Humanizr/Humanizer@3ebc38d](https://github.com/Humanizr/Humanizer/tree/3ebc38de585fc641a04b0e78ed69468453b0f8a1)*
    - `src/source-build-externals/src/MSBuildLocator`  
    *[microsoft/MSBuildLocator@e0281df](https://github.com/microsoft/MSBuildLocator/tree/e0281df33274ac3c3e22acc9b07dcb4b31d57dc0)*
    - `src/source-build-externals/src/newtonsoft-json`  
    *[JamesNK/Newtonsoft.Json@0a2e291](https://github.com/JamesNK/Newtonsoft.Json/tree/0a2e291c0d9c0c7675d445703e51750363a549ef)*
    - `src/source-build-externals/src/spectre-console`  
    *[spectreconsole/spectre.console@7397169](https://github.com/spectreconsole/spectre.console/tree/7397169a2757dc3657598bdea4ac222c0f283425)*
    - `src/source-build-externals/src/xunit`  
    *[xunit/xunit@f110e5b](https://github.com/xunit/xunit/tree/f110e5bee5dfd4c08339587c9c3df9292fcb597c)*
    - `src/source-build-externals/src/xunit/src/xunit.assert/Asserts`  
    *[xunit/assert.xunit@5c8c10e](https://github.com/xunit/assert.xunit/tree/5c8c10e085eb42f39f2fe0b40c94bf56649eb0a4)*
    - `src/source-build-externals/src/xunit/tools/build`  
    *[xunit/build-tools@8e186b0](https://github.com/xunit/build-tools/tree/8e186b0f8e398796e75453f3f18952b06d29fdfd)*
    - `src/source-build-externals/src/xunit/tools/media`  
    *[xunit/media@5738b6e](https://github.com/xunit/media/tree/5738b6e86f08e0389c4392b939c20e3eca2d9822)*
- `src/source-build-reference-packages`  
*[dotnet/source-build-reference-packages@9af3d36](https://github.com/dotnet/source-build-reference-packages/tree/9af3d3655226a7218ad9e05649ed67632f359ecf)*
- `src/sourcelink`  
*[dotnet/sourcelink@482242d](https://github.com/dotnet/sourcelink/tree/482242d0f2b019db52d284be7f1da4b22a7e68a1)*
- `src/symreader`  
*[dotnet/symreader@2902dcf](https://github.com/dotnet/symreader/tree/2902dcf06494391dc65552fd0743b7d426c550fb)*
- `src/templating`  
*[dotnet/templating@511e132](https://github.com/dotnet/templating/tree/511e13222864773fd28e7a67a90a6558ea9848b5)*
- `src/test-templates`  
*[dotnet/test-templates@777cf69](https://github.com/dotnet/test-templates/tree/777cf69d17013af9153cae88ad451f5ce9934f18)*
- `src/vstest`  
*[microsoft/vstest@f21d0da](https://github.com/microsoft/vstest/tree/f21d0dae0b91fe59e4afa92166bd721ddd2f0036)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@d203ca3](https://github.com/dotnet/windowsdesktop/tree/d203ca39a4064de70d17bc60e3cf8e2b497c7cdc)*
- `src/winforms`  
*[dotnet/winforms@226a5b5](https://github.com/dotnet/winforms/tree/226a5b5d3782f40d8c84cdacfa3c49b3f7cc042e)*
- `src/wpf`  
*[dotnet/wpf@0b4cb73](https://github.com/dotnet/wpf/tree/0b4cb73076396274466fc34146f0c3a03503a69e)*
- `src/xdt`  
*[dotnet/xdt@c54253c](https://github.com/dotnet/xdt/tree/c54253c7c4413357772589c6c243b12ba4e7c595)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
