# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@ed14da5](https://github.com/dotnet/arcade/tree/ed14da5934ffb536cff8f41f8b5719334524cbed)*
- `src/aspire`  
*[dotnet/aspire@0514ea9](https://github.com/dotnet/aspire/tree/0514ea9e12ece4dd764824ce925ae0eae6fcbd86)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@94ad1d4](https://github.com/dotnet/aspnetcore/tree/94ad1d4e3edfe952f4621c47b4445d463b2e7042)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@fa6de7f](https://github.com/google/googletest/tree/fa6de7f4382f5c8fb8b9e32eea28a2eb44966c32)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/Node-Externals`  
    *[dotnet/Node-Externals@fb911de](https://github.com/dotnet/Node-Externals/tree/fb911deddbaf7367146718374a403d393571f18a)*
- `src/cecil`  
*[dotnet/cecil@7a4a59f](https://github.com/dotnet/cecil/tree/7a4a59f9f66baf6711a6ce2de01d3b2c62ed72d8)*
- `src/command-line-api`  
*[dotnet/command-line-api@963d34b](https://github.com/dotnet/command-line-api/tree/963d34b1fb712c673bfb198133d7e988182c9ef4)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@6b80783](https://github.com/dotnet/deployment-tools/tree/6b80783f6743ee9f18940eb6acb7135e5c111d4b)*
- `src/diagnostics`  
*[dotnet/diagnostics@a9efc2e](https://github.com/dotnet/diagnostics/tree/a9efc2e9a04c86be5f66995522f63679ced519c7)*
- `src/efcore`  
*[dotnet/efcore@ab14ca2](https://github.com/dotnet/efcore/tree/ab14ca2e5e25ba3ee4c175b3ea09833f9912e124)*
- `src/emsdk`  
*[dotnet/emsdk@53c4a10](https://github.com/dotnet/emsdk/tree/53c4a109e2abc0112996e2bc4a7f830f572f4efd)*
- `src/fsharp`  
*[dotnet/fsharp@7d176e9](https://github.com/dotnet/fsharp/tree/7d176e9b00021c50338c69d93ba8d326c65db1f1)*
- `src/msbuild`  
*[dotnet/msbuild@66e0371](https://github.com/dotnet/msbuild/tree/66e0371a64e08160e63000fc2ced8cb8bbc6739e)*
- `src/nuget-client`  
*[nuget/nuget.client@f702fc3](https://github.com/nuget/nuget.client/tree/f702fc3dc44e17a0666dea01e9aa18c5c1175b27)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@e7eed09](https://github.com/dotnet/razor/tree/e7eed098a10a7fd20edf7cad9db9db9af87ce8b9)*
- `src/roslyn`  
*[dotnet/roslyn@dd1d653](https://github.com/dotnet/roslyn/tree/dd1d6535773f71c1f921589cf59d1d62f6e8c27f)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@8dcccce](https://github.com/dotnet/roslyn-analyzers/tree/8dccccec1ce3bd2fb532ec77d7e092ab9d684db7)*
- `src/runtime`  
*[dotnet/runtime@5474ab5](https://github.com/dotnet/runtime/tree/5474ab5c50fc639e780e0322cd500b2e43ee9b0d)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@259edc6](https://github.com/dotnet/scenario-tests/tree/259edc6efe049ed49f9e37890be702a886ba5ed8)*
- `src/sdk`  
*[dotnet/sdk@2ac543c](https://github.com/dotnet/sdk/tree/2ac543c7eb3d73ba7ddff90f2aa76d4f9d492e6e)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@59204e5](https://github.com/dotnet/source-build-externals/tree/59204e5b14e6e197b3c942f992f6e3ec9196e50b)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[microsoft/ApplicationInsights-dotnet@43825e0](https://github.com/microsoft/ApplicationInsights-dotnet/tree/43825e06a22cdfb702fc199a7ba99a7d541d48c6)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@c1c24e2](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/c1c24e29d5eeac2a2cd53fe0b5656924bdb69e3d)*
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
*[dotnet/source-build-reference-packages@4642f9b](https://github.com/dotnet/source-build-reference-packages/tree/4642f9b75b588565642dd00dabcfedfe67ca106f)*
- `src/sourcelink`  
*[dotnet/sourcelink@14a0a42](https://github.com/dotnet/sourcelink/tree/14a0a42ffb29b53fb9939f14da5a4be8c6c07e0b)*
- `src/symreader`  
*[dotnet/symreader@409af43](https://github.com/dotnet/symreader/tree/409af431ee684f9e07d34bbd4e51b9933345c1e1)*
- `src/templating`  
*[dotnet/templating@d588634](https://github.com/dotnet/templating/tree/d588634504d1db224ece2c502daee24fabd21212)*
- `src/test-templates`  
*[dotnet/test-templates@1a39b68](https://github.com/dotnet/test-templates/tree/1a39b68e67a00df1be070569c3bcce5f38e8c36a)*
- `src/vstest`  
*[microsoft/vstest@2588f02](https://github.com/microsoft/vstest/tree/2588f022c1c4a12e159e0ed07f5a5ea3f3c9eaa8)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@5fa1ab1](https://github.com/dotnet/windowsdesktop/tree/5fa1ab1d2e21c956d5bf5c2f97cfc8d3da71bef8)*
- `src/winforms`  
*[dotnet/winforms@0aa3a4d](https://github.com/dotnet/winforms/tree/0aa3a4dfea1dad493ecbfe15cd513604468b3916)*
- `src/wpf`  
*[dotnet/wpf@504fd82](https://github.com/dotnet/wpf/tree/504fd82cd5acb1844141f58fc65eb1445e810ce7)*
- `src/xdt`  
*[dotnet/xdt@d62d4e0](https://github.com/dotnet/xdt/tree/d62d4e036e024404581283f02321453a331821c2)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
