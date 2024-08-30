# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@e3bdd9a](https://github.com/dotnet/arcade/tree/e3bdd9a0f2a65fe037ba1adb2261eea48a840fa4)*
- `src/aspire`  
*[dotnet/aspire@d304c5f](https://github.com/dotnet/aspire/tree/d304c5f6f15bcd4f34f1841b33870cfab88e6937)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@9f52b5c](https://github.com/dotnet/aspnetcore/tree/9f52b5ccf32f2171881ac6419851bac9cc0f34e4)*
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
*[dotnet/emsdk@84d6424](https://github.com/dotnet/emsdk/tree/84d642485896a97e0e443be75345c6bb1469a338)*
- `src/fsharp`  
*[dotnet/fsharp@a5feb41](https://github.com/dotnet/fsharp/tree/a5feb419073e74562fde38768898988334f379a1)*
- `src/msbuild`  
*[dotnet/msbuild@c256bcc](https://github.com/dotnet/msbuild/tree/c256bcca00827368c080b03b001cac2a2c82017f)*
- `src/nuget-client`  
*[nuget/nuget.client@3db80da](https://github.com/nuget/nuget.client/tree/3db80da582371f1387cd0b3ea029198c76cb78ff)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@1f45c70](https://github.com/dotnet/razor/tree/1f45c70f4f631c06db78bd0fdb450b9a8ab7d039)*
- `src/roslyn`  
*[dotnet/roslyn@14f5b27](https://github.com/dotnet/roslyn/tree/14f5b27c7206557c9054ba6ce59cb9c819d5b473)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@fdb9965](https://github.com/dotnet/roslyn-analyzers/tree/fdb9965ce68c1f4e1c0ff301488adf9caa958615)*
- `src/runtime`  
*[dotnet/runtime@0a5cccb](https://github.com/dotnet/runtime/tree/0a5cccb57942c0493ef680f559f69866f3b1eb7d)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@d92413b](https://github.com/dotnet/scenario-tests/tree/d92413b87d36250859d8cb51ff69a03b5f5c4cab)*
- `src/sdk`  
*[dotnet/sdk@7c0912d](https://github.com/dotnet/sdk/tree/7c0912ddf3f9c3b85ac4f77cd4d54da620a09034)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@59b98ae](https://github.com/dotnet/source-build-externals/tree/59b98ae5bd04e1eebc8ed323193c62ab9e15efe1)*
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
*[dotnet/sourcelink@9b94576](https://github.com/dotnet/sourcelink/tree/9b94576f3e56ee55c818ff611065c8acb3fdf2e2)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@133c2a9](https://github.com/dotnet/templating/tree/133c2a92258a1d4047eb077e39aa82b445dd57f5)*
- `src/test-templates`  
*[dotnet/test-templates@f0bd96c](https://github.com/dotnet/test-templates/tree/f0bd96ccdcc784f6f013fa0fef8f36381a230735)*
- `src/vstest`  
*[microsoft/vstest@54964cd](https://github.com/microsoft/vstest/tree/54964cdbcd254cbce066d3a2afa2b3908db51abd)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@b03ef02](https://github.com/dotnet/windowsdesktop/tree/b03ef02ae6a26c4d77054b95b72ddf7e25e2f5f9)*
- `src/winforms`  
*[dotnet/winforms@bcd798b](https://github.com/dotnet/winforms/tree/bcd798bb6b6a76c4c0573ce0f856db06b40fd841)*
- `src/wpf`  
*[dotnet/wpf@4e8309d](https://github.com/dotnet/wpf/tree/4e8309d8659e70818aacdb08fcb66a4677d11894)*
- `src/xdt`  
*[dotnet/xdt@0d51607](https://github.com/dotnet/xdt/tree/0d51607fb791c51a14b552ed24fe3430c252148b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
