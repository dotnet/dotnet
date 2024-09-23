# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@04b9022](https://github.com/dotnet/arcade/tree/04b9022eba9c184a8036328af513c22e6949e8b6)*
- `src/aspire`  
*[dotnet/aspire@75fdcff](https://github.com/dotnet/aspire/tree/75fdcff28495bdd643f6323133a7d411df71ab70)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@c68cfd0](https://github.com/dotnet/aspnetcore/tree/c68cfd0f718c3991e3c38b6f913f17f4b8d0b169)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@0953a17](https://github.com/google/googletest/tree/0953a17a4281fc26831da647ad3fcd5e21e6473b)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@1541df9](https://github.com/dotnet/cecil/tree/1541df9c44ff8da964b2946e18655c2e37e4a198)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@7871ee3](https://github.com/dotnet/deployment-tools/tree/7871ee378dce87b64d930d4f33dca9c888f4034d)*
- `src/diagnostics`  
*[dotnet/diagnostics@513150c](https://github.com/dotnet/diagnostics/tree/513150c2f25077b1fcb194407e53c433c975f39b)*
- `src/efcore`  
*[dotnet/efcore@59d16b6](https://github.com/dotnet/efcore/tree/59d16b6188ac6328130f6b299eeac828beb4ace1)*
- `src/emsdk`  
*[dotnet/emsdk@c3d83d0](https://github.com/dotnet/emsdk/tree/c3d83d0ea894e5ee537bfc591fd48e06af8d9166)*
- `src/fsharp`  
*[dotnet/fsharp@3044166](https://github.com/dotnet/fsharp/tree/3044166cd923167204853d1d9f975bc26864f86f)*
- `src/msbuild`  
*[dotnet/msbuild@988196b](https://github.com/dotnet/msbuild/tree/988196b1c64418351f46da7d3a9ff403560a4270)*
- `src/nuget-client`  
*[nuget/nuget.client@e082545](https://github.com/nuget/nuget.client/tree/e082545cbba30ec8dc9b759f2d9beea8aabc6215)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@c2bff15](https://github.com/dotnet/razor/tree/c2bff15ca1c61c9859b88cf455944dd6f5a3ab1e)*
- `src/roslyn`  
*[dotnet/roslyn@f869d0f](https://github.com/dotnet/roslyn/tree/f869d0f8a872875a46ba9fe23c67b04eb5519c2e)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@a7c74cf](https://github.com/dotnet/roslyn-analyzers/tree/a7c74cf887abe4a38240bc4ead0b221d9d42434f)*
- `src/runtime`  
*[dotnet/runtime@7832c5d](https://github.com/dotnet/runtime/tree/7832c5d3028cf206ab7faf4a889ee8ed34abee5c)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@d92413b](https://github.com/dotnet/scenario-tests/tree/d92413b87d36250859d8cb51ff69a03b5f5c4cab)*
- `src/sdk`  
*[dotnet/sdk@3474b4f](https://github.com/dotnet/sdk/tree/3474b4f46c4d95e6d8c35c95fae37b6232ab1f3c)*
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
*[dotnet/sourcelink@1099a67](https://github.com/dotnet/sourcelink/tree/1099a677155d9d4c1a81a612dffccbcf0d94f8e9)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@a23da1c](https://github.com/dotnet/templating/tree/a23da1c15c737b5e121650cfa5a86805e74e34fc)*
- `src/test-templates`  
*[dotnet/test-templates@2b8a55f](https://github.com/dotnet/test-templates/tree/2b8a55f3d3f380ae10d2c2f4d0974c92727ccc22)*
- `src/vstest`  
*[microsoft/vstest@07acde2](https://github.com/microsoft/vstest/tree/07acde22b65497e72de145d57167b83609a7f7fb)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@f37a996](https://github.com/dotnet/windowsdesktop/tree/f37a996ae93c3aaad652c36d43a6245ad090d775)*
- `src/winforms`  
*[dotnet/winforms@f1676f5](https://github.com/dotnet/winforms/tree/f1676f5dcec5d4a84228406113fcad5fe31025af)*
- `src/wpf`  
*[dotnet/wpf@d8b93d9](https://github.com/dotnet/wpf/tree/d8b93d9e0c5ca81cfae3ff691824c3cdef74f5b3)*
- `src/xdt`  
*[dotnet/xdt@c2a9df9](https://github.com/dotnet/xdt/tree/c2a9df9c1867454039a1223cef1c090359e33646)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
