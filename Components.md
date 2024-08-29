# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@8fe02ba](https://github.com/dotnet/arcade/tree/8fe02bab989df1265eee225df2c28af6dbdccc83)*
- `src/aspire`  
*[dotnet/aspire@d304c5f](https://github.com/dotnet/aspire/tree/d304c5f6f15bcd4f34f1841b33870cfab88e6937)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@42af9fe](https://github.com/dotnet/aspnetcore/tree/42af9fe6ddd7c3f9cde04ac003bf97509881873b)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@ff233bd](https://github.com/google/googletest/tree/ff233bdd4cac0a0bf6e5cd45bda3406814cb2796)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@b9d928a](https://github.com/dotnet/cecil/tree/b9d928a9d65ed39b9257846e1b8e853cea609c00)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@7871ee3](https://github.com/dotnet/deployment-tools/tree/7871ee378dce87b64d930d4f33dca9c888f4034d)*
- `src/diagnostics`  
*[dotnet/diagnostics@513150c](https://github.com/dotnet/diagnostics/tree/513150c2f25077b1fcb194407e53c433c975f39b)*
- `src/efcore`  
*[dotnet/efcore@d94cc5a](https://github.com/dotnet/efcore/tree/d94cc5a79cb9d557485a95fba59ee3738c46aa81)*
- `src/emsdk`  
*[dotnet/emsdk@edf3e90](https://github.com/dotnet/emsdk/tree/edf3e90fa25b1fc4f7f63ceb45ef70f49c6b121a)*
- `src/fsharp`  
*[dotnet/fsharp@a5feb41](https://github.com/dotnet/fsharp/tree/a5feb419073e74562fde38768898988334f379a1)*
- `src/msbuild`  
*[dotnet/msbuild@6bc91d5](https://github.com/dotnet/msbuild/tree/6bc91d5e2d3d8a199fdbe367ed015b55daf57046)*
- `src/nuget-client`  
*[nuget/nuget.client@9cb2652](https://github.com/nuget/nuget.client/tree/9cb26528a73dce163a0288268c6139ee8519d85f)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@562be1a](https://github.com/dotnet/razor/tree/562be1a2201aa08b09970966224de44d40636d86)*
- `src/roslyn`  
*[dotnet/roslyn@a3dbd80](https://github.com/dotnet/roslyn/tree/a3dbd808a5ee9b8010d38f37726eeb596e2f3aaa)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@3211f48](https://github.com/dotnet/roslyn-analyzers/tree/3211f48253bc18560156d90dc5e710d35f7d03fa)*
- `src/runtime`  
*[dotnet/runtime@0fbd814](https://github.com/dotnet/runtime/tree/0fbd81404d1f211572387498474063bc6f407f0f)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@f1715bf](https://github.com/dotnet/scenario-tests/tree/f1715bf842371d15ecb5fc52d7dc87ab2e47df30)*
- `src/sdk`  
*[dotnet/sdk@7f4f157](https://github.com/dotnet/sdk/tree/7f4f157ba7539b8ef71adef95da745564ed700b1)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@f08c68b](https://github.com/dotnet/source-build-externals/tree/f08c68bfc6c6e065f72e56dcd4dc15d3ea1f43a4)*
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
*[dotnet/source-build-reference-packages@6c26545](https://github.com/dotnet/source-build-reference-packages/tree/6c26545ca7999a4192df6899ea9ed4eaa950d367)*
- `src/sourcelink`  
*[dotnet/sourcelink@7b0d5e6](https://github.com/dotnet/sourcelink/tree/7b0d5e684c4d646b1b71866cdc6abfbba7026619)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@1d04c91](https://github.com/dotnet/templating/tree/1d04c91bc230347a865e1dd7cc9cf1d9b2e24d4a)*
- `src/test-templates`  
*[dotnet/test-templates@9fea743](https://github.com/dotnet/test-templates/tree/9fea743e85a24984211f854d44f1f6debea8285d)*
- `src/vstest`  
*[microsoft/vstest@54964cd](https://github.com/microsoft/vstest/tree/54964cdbcd254cbce066d3a2afa2b3908db51abd)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@6c45e7d](https://github.com/dotnet/windowsdesktop/tree/6c45e7d678a9af55b5e792a3528648aee5f4c4c7)*
- `src/winforms`  
*[dotnet/winforms@3b37d24](https://github.com/dotnet/winforms/tree/3b37d24254fe96bc6ad3fd63c6adea21c04f7024)*
- `src/wpf`  
*[dotnet/wpf@a13bb63](https://github.com/dotnet/wpf/tree/a13bb634c1dcdf0bf7cda9a02c448b53366b8550)*
- `src/xdt`  
*[dotnet/xdt@0d51607](https://github.com/dotnet/xdt/tree/0d51607fb791c51a14b552ed24fe3430c252148b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
