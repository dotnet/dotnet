# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@3c393bb](https://github.com/dotnet/arcade/tree/3c393bbd85ae16ddddba20d0b75035b0c6f1a52d)*
- `src/aspire`  
*[dotnet/aspire@5fa9337](https://github.com/dotnet/aspire/tree/5fa9337a84a52e9bd185d04d156eccbdcf592f74)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@1e7a7af](https://github.com/dotnet/aspnetcore/tree/1e7a7af6d2417242b244d2a0f4f23fcce8e88d2f)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@6dae7eb](https://github.com/google/googletest/tree/6dae7eb4a5c3a169f3e298392bff4680224aa94a)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@9511905](https://github.com/aspnet/MessagePack-CSharp/tree/95119056ee8f4da1714b055a4f16893afaa73af7)*
- `src/cecil`  
*[dotnet/cecil@9c94433](https://github.com/dotnet/cecil/tree/9c9443396f8deacceb8edb169890e52aac25f311)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@7871ee3](https://github.com/dotnet/deployment-tools/tree/7871ee378dce87b64d930d4f33dca9c888f4034d)*
- `src/diagnostics`  
*[dotnet/diagnostics@513150c](https://github.com/dotnet/diagnostics/tree/513150c2f25077b1fcb194407e53c433c975f39b)*
- `src/efcore`  
*[dotnet/efcore@dfc53da](https://github.com/dotnet/efcore/tree/dfc53da799c9dd97d87649b1d19242e56e5576cc)*
- `src/emsdk`  
*[dotnet/emsdk@8be5676](https://github.com/dotnet/emsdk/tree/8be5676af1ccf568b258133788a24aedd1a80994)*
- `src/fsharp`  
*[dotnet/fsharp@f07a914](https://github.com/dotnet/fsharp/tree/f07a91420bec3f657153e16c9f047cf151c1179f)*
- `src/msbuild`  
*[dotnet/msbuild@43a2496](https://github.com/dotnet/msbuild/tree/43a24969a23bd2dd76cd26be26210e2afcd0595e)*
- `src/nuget-client`  
*[nuget/nuget.client@aa7eb99](https://github.com/nuget/nuget.client/tree/aa7eb9987d28e7169cfabfa484f2fdd22d2b91d2)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@8bf9ad1](https://github.com/dotnet/razor/tree/8bf9ad1ce4cfc0d77916f8db993e2d7f29b22665)*
- `src/roslyn`  
*[dotnet/roslyn@21192bf](https://github.com/dotnet/roslyn/tree/21192bfc323cbdd5a1f6e5dadca56ef0558c8adf)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@3d61c57](https://github.com/dotnet/roslyn-analyzers/tree/3d61c57c73c3dd5f1f407ef9cd3414d94bf0eaf2)*
- `src/runtime`  
*[_git/dotnet-runtime@9d5a6a9](https://dev.azure.com/dnceng/internal/_git/dotnet-runtime/?version=GC9d5a6a9aa463d6d10b0b0ba6d5982cc82f363dc3)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@1009e3b](https://github.com/dotnet/scenario-tests/tree/1009e3b6d23e049de56b91de82fe975fe84444f8)*
- `src/sdk`  
*[dotnet/sdk@7370d24](https://github.com/dotnet/sdk/tree/7370d2412bdb704c73df2e98e981b2dc53eeb244)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@4df883d](https://github.com/dotnet/source-build-externals/tree/4df883d781a4290873b3b968afc0ff0df7132507)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[microsoft/ApplicationInsights-dotnet@43825e0](https://github.com/microsoft/ApplicationInsights-dotnet/tree/43825e06a22cdfb702fc199a7ba99a7d541d48c6)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@e67b25b](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/e67b25be77532af9ba405670b34b4d263d505fde)*
    - `src/source-build-externals/src/cssparser`  
    *[dotnet/cssparser@0d59611](https://github.com/dotnet/cssparser/tree/0d59611784841735a7778a67aa6e9d8d000c861f)*
    - `src/source-build-externals/src/docker-creds-provider`  
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
*[dotnet/source-build-reference-packages@a2ad736](https://github.com/dotnet/source-build-reference-packages/tree/a2ad736b86ce484df4d2dfc967bd29cd9d1b5de9)*
- `src/sourcelink`  
*[dotnet/sourcelink@7c7d764](https://github.com/dotnet/sourcelink/tree/7c7d76475d055bb47dc9cadbb47065cd3625b515)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@b699ff1](https://github.com/dotnet/templating/tree/b699ff195f37e5039d378be3be64ccf7512df50a)*
- `src/test-templates`  
*[dotnet/test-templates@282a211](https://github.com/dotnet/test-templates/tree/282a2113ba98fd9c9dc18021d5c79d446ef7d45d)*
- `src/vstest`  
*[microsoft/vstest@bc91613](https://github.com/microsoft/vstest/tree/bc9161306b23641b0364b8f93d546da4d48da1eb)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@5307f2a](https://github.com/dotnet/windowsdesktop/tree/5307f2abc4387e29964c6f46cb1f63cfdc218602)*
- `src/winforms`  
*[dotnet/winforms@5f03f3d](https://github.com/dotnet/winforms/tree/5f03f3d8a99d8094fd0067e2497c4ea9b440e324)*
- `src/wpf`  
*[dotnet/wpf@375aed2](https://github.com/dotnet/wpf/tree/375aed28c289639ec572af58067a31c3d7742ef9)*
- `src/xdt`  
*[dotnet/xdt@1a54480](https://github.com/dotnet/xdt/tree/1a54480f52703fb45fac2a6b955247d33758383e)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
