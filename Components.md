# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@d21db44](https://github.com/dotnet/arcade/tree/d21db44e84b9038ea7b2add139adee2303d46800)*
- `src/aspire`  
*[dotnet/aspire@d304c5f](https://github.com/dotnet/aspire/tree/d304c5f6f15bcd4f34f1841b33870cfab88e6937)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@36cdc65](https://github.com/dotnet/aspnetcore/tree/36cdc65bf29bac3d7bd0c5d38242a92608fab7eb)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@ff233bd](https://github.com/google/googletest/tree/ff233bdd4cac0a0bf6e5cd45bda3406814cb2796)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@c667bfe](https://github.com/dotnet/cecil/tree/c667bfea9cdbc5b5493e49e7ddc8dd635a217891)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@7871ee3](https://github.com/dotnet/deployment-tools/tree/7871ee378dce87b64d930d4f33dca9c888f4034d)*
- `src/diagnostics`  
*[dotnet/diagnostics@513150c](https://github.com/dotnet/diagnostics/tree/513150c2f25077b1fcb194407e53c433c975f39b)*
- `src/efcore`  
*[dotnet/efcore@d18b30f](https://github.com/dotnet/efcore/tree/d18b30f63dd3473c0d2bc61683eb997a1e8ef6ca)*
- `src/emsdk`  
*[dotnet/emsdk@c8026c0](https://github.com/dotnet/emsdk/tree/c8026c0932574ddad057e54d983ab8c2ba082b93)*
- `src/fsharp`  
*[dotnet/fsharp@20cca61](https://github.com/dotnet/fsharp/tree/20cca61a546fe378948f0550a0026ec6077c1600)*
- `src/msbuild`  
*[dotnet/msbuild@2206a05](https://github.com/dotnet/msbuild/tree/2206a054f2c82e91918809ea27d1ef5b3f7cfc4b)*
- `src/nuget-client`  
*[nuget/nuget.client@5d08fbd](https://github.com/nuget/nuget.client/tree/5d08fbd496ee2ce63b50dfe0803edbd9701e1b35)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@8bddfe9](https://github.com/dotnet/razor/tree/8bddfe9971d754f736d360f16a340e2a8e3e2c55)*
- `src/roslyn`  
*[dotnet/roslyn@c6a0795](https://github.com/dotnet/roslyn/tree/c6a0795ce110a904bf7d706a70335f7579390833)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@930872a](https://github.com/dotnet/roslyn-analyzers/tree/930872ad8817ff59fbb4454e79edf738904d173a)*
- `src/runtime`  
*[dotnet/runtime@dec716d](https://github.com/dotnet/runtime/tree/dec716d1c4651b70e4710e781e832a64be1e713b)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@d92413b](https://github.com/dotnet/scenario-tests/tree/d92413b87d36250859d8cb51ff69a03b5f5c4cab)*
- `src/sdk`  
*[dotnet/sdk@255bc25](https://github.com/dotnet/sdk/tree/255bc25d18622db910dc3cb49d01b65a27f38ac0)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@eab3dc5](https://github.com/dotnet/source-build-externals/tree/eab3dc5eabdf8bcd9bbdf917741aab335c74373d)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[microsoft/ApplicationInsights-dotnet@43825e0](https://github.com/microsoft/ApplicationInsights-dotnet/tree/43825e06a22cdfb702fc199a7ba99a7d541d48c6)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@e67b25b](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/e67b25be77532af9ba405670b34b4d263d505fde)*
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
*[dotnet/source-build-reference-packages@f22b107](https://github.com/dotnet/source-build-reference-packages/tree/f22b1078535aa38f914c2304a02303e92de0adc0)*
- `src/sourcelink`  
*[dotnet/sourcelink@a3a6c55](https://github.com/dotnet/sourcelink/tree/a3a6c5560d7593ede6e78835f53bbaa08a48a2df)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@90d52f2](https://github.com/dotnet/templating/tree/90d52f21f86cded2998c882c61dd8452ef6a9f5f)*
- `src/test-templates`  
*[dotnet/test-templates@e833d46](https://github.com/dotnet/test-templates/tree/e833d4684ffa6144968f530a0b3250c540fae026)*
- `src/vstest`  
*[microsoft/vstest@07acde2](https://github.com/microsoft/vstest/tree/07acde22b65497e72de145d57167b83609a7f7fb)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@3467c9b](https://github.com/dotnet/windowsdesktop/tree/3467c9b992c2b23e08d4cc8aed3eceb93d457350)*
- `src/winforms`  
*[dotnet/winforms@7d8d282](https://github.com/dotnet/winforms/tree/7d8d28254a994996518d78895d5d32a96ed56aaf)*
- `src/wpf`  
*[dotnet/wpf@4bbee65](https://github.com/dotnet/wpf/tree/4bbee6591a871580d0ad50ec0191220c24544915)*
- `src/xdt`  
*[dotnet/xdt@c2a9df9](https://github.com/dotnet/xdt/tree/c2a9df9c1867454039a1223cef1c090359e33646)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
