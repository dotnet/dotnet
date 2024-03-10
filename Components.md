# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@2215d18](https://github.com/dotnet/arcade/tree/2215d18d04a6565e9133c15ece7df7161fdf1e8e)*
- `src/aspire`  
*[dotnet/aspire@fa3e45e](https://github.com/dotnet/aspire/tree/fa3e45ee2a76c81e7a4876c6bef05282e35243f1)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@d9df235](https://github.com/dotnet/aspnetcore/tree/d9df235c2e6d41b65d8e860c5e209d9df8cbcae1)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@31993df](https://github.com/google/googletest/tree/31993dfa6b47e11c7a6ef67cfa8af90892b9bd1c)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/Node-Externals`  
    *[dotnet/Node-Externals@1bcb78c](https://github.com/dotnet/Node-Externals/tree/1bcb78ca694568f7993d9d385eee0687ad0f5dfe)*
- `src/cecil`  
*[dotnet/cecil@0d0bc8e](https://github.com/dotnet/cecil/tree/0d0bc8e0f47fdae9834e1eac678f364c50946133)*
- `src/command-line-api`  
*[dotnet/command-line-api@5ea97af](https://github.com/dotnet/command-line-api/tree/5ea97af07263ea3ef68a18557c8aa3f7e3200bda)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@822ff26](https://github.com/dotnet/deployment-tools/tree/822ff266c5f999ab9ceb6928df59d79285ea4a4f)*
- `src/diagnostics`  
*[dotnet/diagnostics@5eb514a](https://github.com/dotnet/diagnostics/tree/5eb514a41f900ac1aa1e9a3e12b2931dcb064069)*
- `src/emsdk`  
*[dotnet/emsdk@94f048f](https://github.com/dotnet/emsdk/tree/94f048f4d194119ec20c8878790972fc3a83dc3b)*
- `src/format`  
*[dotnet/format@87c757a](https://github.com/dotnet/format/tree/87c757a75d7619e8153b1ac114f11b5dbab4497d)*
- `src/fsharp`  
*[dotnet/fsharp@661d88a](https://github.com/dotnet/fsharp/tree/661d88ab1975665fbe8c580e7e2dab01bf264c27)*
- `src/installer`  
*[dotnet/installer@232d2df](https://github.com/dotnet/installer/tree/232d2df252560690e936f80455db99c352926671)*
- `src/msbuild`  
*[dotnet/msbuild@6f44380](https://github.com/dotnet/msbuild/tree/6f44380e4fdea6ddf5c11f48efeb25c2bf181e62)*
- `src/nuget-client`  
*[nuget/nuget.client@309f2e3](https://github.com/nuget/nuget.client/tree/309f2e302dff9e6ba8a2f48ddf28133d68ff7eae)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@c126dfc](https://github.com/dotnet/razor/tree/c126dfc208bca863b378ae32f8c5cd3f271cbff6)*
- `src/roslyn`  
*[dotnet/roslyn@c759290](https://github.com/dotnet/roslyn/tree/c7592906094bc8f16f7371d2fd59c5025d317b8a)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@7b1f39d](https://github.com/dotnet/roslyn-analyzers/tree/7b1f39d7b8bd81d003898c49d0320682b6f06c54)*
- `src/runtime`  
*[dotnet/runtime@da781b3](https://github.com/dotnet/runtime/tree/da781b3aab1bc30793812bced4a6b64d2df31a9f)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@bfde902](https://github.com/dotnet/scenario-tests/tree/bfde902a10d7b672f4fc7e844198ede405dbb9c6)*
- `src/sdk`  
*[dotnet/sdk@01dbe9e](https://github.com/dotnet/sdk/tree/01dbe9e70a94ac6add230d4fa44ee2db93281e5d)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@60a35f5](https://github.com/dotnet/source-build-externals/tree/60a35f5b8ce2839e56457164844d91fc89622c6a)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[Microsoft/ApplicationInsights-dotnet@43825e0](https://github.com/Microsoft/ApplicationInsights-dotnet/tree/43825e06a22cdfb702fc199a7ba99a7d541d48c6)*
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
*[dotnet/source-build-reference-packages@768378e](https://github.com/dotnet/source-build-reference-packages/tree/768378e775fc5ddc99d41f2c4d1c78182f326ea7)*
- `src/sourcelink`  
*[dotnet/sourcelink@f25aa97](https://github.com/dotnet/sourcelink/tree/f25aa979d3690da74b91e7162956b49c73a90948)*
- `src/symreader`  
*[dotnet/symreader@71a20ad](https://github.com/dotnet/symreader/tree/71a20ad4aaedc284ef2d9a7302f5d2ec4df7dca3)*
- `src/templating`  
*[dotnet/templating@c799af3](https://github.com/dotnet/templating/tree/c799af392be3da0ae48a4d98c7cc04f03db329d8)*
- `src/test-templates`  
*[dotnet/test-templates@b27ce8a](https://github.com/dotnet/test-templates/tree/b27ce8a9d5b1931cb91cbc20a16d15bd7231c792)*
- `src/vstest`  
*[microsoft/vstest@3161673](https://github.com/microsoft/vstest/tree/316167369cea59e0ad6ece2a39d94a3a6d49cf12)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@d6b074b](https://github.com/dotnet/windowsdesktop/tree/d6b074b0ab8aeb928bb93d258161ed16f31f85f2)*
- `src/winforms`  
*[dotnet/winforms@389c8da](https://github.com/dotnet/winforms/tree/389c8da733c43105275cfc9f9bc482b144073880)*
- `src/wpf`  
*[dotnet/wpf@a2c7547](https://github.com/dotnet/wpf/tree/a2c7547c602fe7dfbf8447a563b440d7449b6b6d)*
- `src/xdt`  
*[dotnet/xdt@a86209b](https://github.com/dotnet/xdt/tree/a86209be9faeafe337633ab1746df7bc19a83538)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
