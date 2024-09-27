# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@1230437](https://github.com/dotnet/arcade/tree/1230437de1ab7b3e15fe7cdfe7ffce2f65449959)*
- `src/aspire`  
*[dotnet/aspire@137e8dc](https://github.com/dotnet/aspire/tree/137e8dcae0a7b22c05f48c4e7a5d36fe3f00a8d7)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@afe2857](https://github.com/dotnet/aspnetcore/tree/afe2857bffef08213d6f3c7688647c60d5d5e7bd)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@0953a17](https://github.com/google/googletest/tree/0953a17a4281fc26831da647ad3fcd5e21e6473b)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@526b22d](https://github.com/dotnet/cecil/tree/526b22d829bc9b420dff6ef70877a67053b66e0f)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@7871ee3](https://github.com/dotnet/deployment-tools/tree/7871ee378dce87b64d930d4f33dca9c888f4034d)*
- `src/diagnostics`  
*[dotnet/diagnostics@513150c](https://github.com/dotnet/diagnostics/tree/513150c2f25077b1fcb194407e53c433c975f39b)*
- `src/efcore`  
*[dotnet/efcore@c7035b3](https://github.com/dotnet/efcore/tree/c7035b3ccc485a63bd03a64da4d9f0ec18f27948)*
- `src/emsdk`  
*[dotnet/emsdk@8e660ff](https://github.com/dotnet/emsdk/tree/8e660ff41e91879977e3a9d837e068bd72234c26)*
- `src/fsharp`  
*[dotnet/fsharp@3044166](https://github.com/dotnet/fsharp/tree/3044166cd923167204853d1d9f975bc26864f86f)*
- `src/msbuild`  
*[dotnet/msbuild@fea15fb](https://github.com/dotnet/msbuild/tree/fea15fbd1fdb509ee69db79420c6cc10044b6b09)*
- `src/nuget-client`  
*[nuget/nuget.client@1e0115a](https://github.com/nuget/nuget.client/tree/1e0115a608073be2ad615201be30b93dd628300d)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@3e0da42](https://github.com/dotnet/razor/tree/3e0da42f82553178744daf61795b51326084251f)*
- `src/roslyn`  
*[dotnet/roslyn@5ef52ae](https://github.com/dotnet/roslyn/tree/5ef52ae33a88c3ae0d3a037054cb66ea7eaaf902)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@a7c74cf](https://github.com/dotnet/roslyn-analyzers/tree/a7c74cf887abe4a38240bc4ead0b221d9d42434f)*
- `src/runtime`  
*[dotnet/runtime@2c4266c](https://github.com/dotnet/runtime/tree/2c4266c134aa02718179818098a11e825c9d1362)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@76edac9](https://github.com/dotnet/scenario-tests/tree/76edac963b2c6296018c7bf1e04a03c8488076d3)*
- `src/sdk`  
*[dotnet/sdk@a0cf435](https://github.com/dotnet/sdk/tree/a0cf43551db20710715bb9fb5ca19c83d9f5f395)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@4e60131](https://github.com/dotnet/source-build-externals/tree/4e60131607fd144eb86fe4487f1a37da940ca990)*
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
*[dotnet/source-build-reference-packages@6a1e863](https://github.com/dotnet/source-build-reference-packages/tree/6a1e86367923914589bda2244eb2321b97523c33)*
- `src/sourcelink`  
*[dotnet/sourcelink@402965a](https://github.com/dotnet/sourcelink/tree/402965a6a951fd19f0055ba8174deb3d0c17501f)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@a23da1c](https://github.com/dotnet/templating/tree/a23da1c15c737b5e121650cfa5a86805e74e34fc)*
- `src/test-templates`  
*[dotnet/test-templates@743a517](https://github.com/dotnet/test-templates/tree/743a517a123a8616389eb6726ec9835a3a1a3876)*
- `src/vstest`  
*[microsoft/vstest@07acde2](https://github.com/microsoft/vstest/tree/07acde22b65497e72de145d57167b83609a7f7fb)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@ea45fe7](https://github.com/dotnet/windowsdesktop/tree/ea45fe711f3747c51211a4580f402dc07de7f5fc)*
- `src/winforms`  
*[dotnet/winforms@0749a97](https://github.com/dotnet/winforms/tree/0749a97fb592ed4a5d7ddc8d13acf9f5097f68f7)*
- `src/wpf`  
*[dotnet/wpf@95b7cb7](https://github.com/dotnet/wpf/tree/95b7cb7c47cf28eb2cf855ff06f91cbe17735af4)*
- `src/xdt`  
*[dotnet/xdt@4ddd811](https://github.com/dotnet/xdt/tree/4ddd8113a29852380b7b929117bfe67f401ac320)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
