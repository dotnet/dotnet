# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@438a8e2](https://github.com/dotnet/arcade/tree/438a8e2488313fb3aa1b24a741a85c2669ee7e0d)*
- `src/aspire`  
*[dotnet/aspire@87d5246](https://github.com/dotnet/aspire/tree/87d5246ddfc1fb9b07fcdf7b4b42830f67427ab9)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@70abdab](https://github.com/dotnet/aspnetcore/tree/70abdab0a5969fd7746479c1fae4697bce2dbc92)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@6a59382](https://github.com/google/googletest/tree/6a5938233b6519ba99ddb7c7314d45d3fa877969)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@b8c2293](https://github.com/dotnet/cecil/tree/b8c2293cd1cbd9d0fe6f32d7b5befbd526b5a175)*
- `src/command-line-api`  
*[dotnet/command-line-api@ecd2ce5](https://github.com/dotnet/command-line-api/tree/ecd2ce5eafbba3008a7d4f5d04b025d30928c812)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@643a0cb](https://github.com/dotnet/deployment-tools/tree/643a0cb2966b097763158082a138189e22205d3b)*
- `src/diagnostics`  
*[dotnet/diagnostics@b0ee5b9](https://github.com/dotnet/diagnostics/tree/b0ee5b9a01e571161bf772aa659440a986bbe532)*
- `src/emsdk`  
*[dotnet/emsdk@687be2a](https://github.com/dotnet/emsdk/tree/687be2a32a302aaade82380c0eaafa5af85fb4da)*
- `src/format`  
*[dotnet/format@32d58a6](https://github.com/dotnet/format/tree/32d58a6d830fc36999912707947f23b57966f103)*
- `src/fsharp`  
*[dotnet/fsharp@7c217c4](https://github.com/dotnet/fsharp/tree/7c217c487c6e2b7d824f3d40666b3cbad412cad4)*
- `src/installer`  
*[dotnet/installer@d693228](https://github.com/dotnet/installer/tree/d693228ff8022dbb5a7886d7cd45f09cde57c568)*
- `src/msbuild`  
*[dotnet/msbuild@0d8d09e](https://github.com/dotnet/msbuild/tree/0d8d09e5c582526daeb4af0b52956c3290e424d1)*
- `src/nuget-client`  
*[nuget/nuget.client@d55931a](https://github.com/nuget/nuget.client/tree/d55931a69dcda3dcb87ba46a09fe268e0febc223)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@5942153](https://github.com/dotnet/razor/tree/59421532418d132935fd48a4d363bd8cecd4e34a)*
- `src/roslyn`  
*[dotnet/roslyn@3cd939f](https://github.com/dotnet/roslyn/tree/3cd939f76803da435c20b082a5cfcc844386fcfb)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@9b918ec](https://github.com/dotnet/roslyn-analyzers/tree/9b918ec314040fb7f547f0458347440105e099e3)*
- `src/runtime`  
*[dotnet/runtime@d40c654](https://github.com/dotnet/runtime/tree/d40c654c274fe4f4afe66328f0599130f3eb2ea6)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@bfde902](https://github.com/dotnet/scenario-tests/tree/bfde902a10d7b672f4fc7e844198ede405dbb9c6)*
- `src/sdk`  
*[dotnet/sdk@aea4e42](https://github.com/dotnet/sdk/tree/aea4e42aff076545d4709fd6071dda62fe0ae332)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@9f553c8](https://github.com/dotnet/source-build-externals/tree/9f553c88e8a6787c560ab3e7adec226311de7e2c)*
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
*[dotnet/source-build-reference-packages@69b60d2](https://github.com/dotnet/source-build-reference-packages/tree/69b60d2af1775f374c91b3e52da02de6b7de1943)*
- `src/sourcelink`  
*[dotnet/sourcelink@7b1a779](https://github.com/dotnet/sourcelink/tree/7b1a779fc3533784ee533515f5c4421d6f2baea6)*
- `src/symreader`  
*[dotnet/symreader@2902dcf](https://github.com/dotnet/symreader/tree/2902dcf06494391dc65552fd0743b7d426c550fb)*
- `src/templating`  
*[dotnet/templating@521a674](https://github.com/dotnet/templating/tree/521a6740eb526eecb03b93683784ab8afb788216)*
- `src/test-templates`  
*[dotnet/test-templates@4486ff2](https://github.com/dotnet/test-templates/tree/4486ff28949aa10726517ddf7ecabedc2a7e1ceb)*
- `src/vstest`  
*[microsoft/vstest@a09e17e](https://github.com/microsoft/vstest/tree/a09e17e9efd7f85abab5a83d627d15aef5b6a954)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@4feebc7](https://github.com/dotnet/windowsdesktop/tree/4feebc7df48f42a8cb8f2c0ad620ffce61e86a8e)*
- `src/winforms`  
*[dotnet/winforms@5207c65](https://github.com/dotnet/winforms/tree/5207c6554bb20236bd91c6083c3e1ee3c76c9402)*
- `src/wpf`  
*[dotnet/wpf@3db410e](https://github.com/dotnet/wpf/tree/3db410e6645fd9a13e9b1c13f36ab32087b5b970)*
- `src/xdt`  
*[dotnet/xdt@d71290d](https://github.com/dotnet/xdt/tree/d71290db981c297b17054b64b2bc7c707a547545)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
