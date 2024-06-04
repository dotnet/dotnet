# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@1cf3eaa](https://github.com/dotnet/arcade/tree/1cf3eaa1f6ada43ab988145a3f3efddb1ffa3b10)*
- `src/aspire`  
*[dotnet/aspire@9faf59f](https://github.com/dotnet/aspire/tree/9faf59f870abdeb427c51c1380fce84d8163f2f0)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@1be2ad8](https://github.com/dotnet/aspnetcore/tree/1be2ad87ffe9cac0697eeaebc06f74a86a86a0f1)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@33af80a](https://github.com/google/googletest/tree/33af80a883ddc33d9c0fac0a5b4578301efb18de)*
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
*[dotnet/efcore@a982436](https://github.com/dotnet/efcore/tree/a982436676f5b9532bbce02003a803c2de5e13e2)*
- `src/emsdk`  
*[dotnet/emsdk@297f406](https://github.com/dotnet/emsdk/tree/297f406779e4852950496acecff0b522fa8e6441)*
- `src/fsharp`  
*[dotnet/fsharp@61cf1c1](https://github.com/dotnet/fsharp/tree/61cf1c139360aec37ccadd3e1a0701fd91fd81fe)*
- `src/msbuild`  
*[dotnet/msbuild@b963c24](https://github.com/dotnet/msbuild/tree/b963c24ef3657479f662347a4b1dbf8185042966)*
- `src/nuget-client`  
*[nuget/nuget.client@16fc5fb](https://github.com/nuget/nuget.client/tree/16fc5fb903a24fdde58baeb0e957d9a7cb3b9815)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@ce7a0bf](https://github.com/dotnet/razor/tree/ce7a0bf3fcd7e70d25e1fbacbb4c24080b68986c)*
- `src/roslyn`  
*[dotnet/roslyn@6f2f8aa](https://github.com/dotnet/roslyn/tree/6f2f8aa9b5ccf6a2ee1eae079919a13a9b618d51)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@8dcccce](https://github.com/dotnet/roslyn-analyzers/tree/8dccccec1ce3bd2fb532ec77d7e092ab9d684db7)*
- `src/runtime`  
*[dotnet/runtime@de6897b](https://github.com/dotnet/runtime/tree/de6897b3b71c1628329ab586ea3e4ecc6a5a2ab3)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@259edc6](https://github.com/dotnet/scenario-tests/tree/259edc6efe049ed49f9e37890be702a886ba5ed8)*
- `src/sdk`  
*[dotnet/sdk@1fb3b28](https://github.com/dotnet/sdk/tree/1fb3b2862a44d109ed755f96344fdd65830c15ca)*
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
*[dotnet/source-build-reference-packages@0df72d8](https://github.com/dotnet/source-build-reference-packages/tree/0df72d85186994facaefcb4eb832b8c8a8e5ae3d)*
- `src/sourcelink`  
*[dotnet/sourcelink@14a0a42](https://github.com/dotnet/sourcelink/tree/14a0a42ffb29b53fb9939f14da5a4be8c6c07e0b)*
- `src/symreader`  
*[dotnet/symreader@409af43](https://github.com/dotnet/symreader/tree/409af431ee684f9e07d34bbd4e51b9933345c1e1)*
- `src/templating`  
*[dotnet/templating@fbef813](https://github.com/dotnet/templating/tree/fbef8134d41410fea9c6899648cafe6a83eafa35)*
- `src/test-templates`  
*[dotnet/test-templates@f57e2c3](https://github.com/dotnet/test-templates/tree/f57e2c3eb78ac1033d410fde85509100b7512730)*
- `src/vstest`  
*[microsoft/vstest@2588f02](https://github.com/microsoft/vstest/tree/2588f022c1c4a12e159e0ed07f5a5ea3f3c9eaa8)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@4b985fd](https://github.com/dotnet/windowsdesktop/tree/4b985fd0f9794ca084e5d0ced977dccad4a31436)*
- `src/winforms`  
*[dotnet/winforms@5ad8b52](https://github.com/dotnet/winforms/tree/5ad8b52370086eb9edeb96e3830b5122bb8ea975)*
- `src/wpf`  
*[dotnet/wpf@c7e3f90](https://github.com/dotnet/wpf/tree/c7e3f907a6784961c0292138eaa01b7fa8cbf4f1)*
- `src/xdt`  
*[dotnet/xdt@ab3fab3](https://github.com/dotnet/xdt/tree/ab3fab3f13fe09c8eb14aafc7811bf33e6de5654)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
