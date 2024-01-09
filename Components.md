# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@bab85ba](https://github.com/dotnet/arcade/tree/bab85ba4579526962b0dbb0aa4db625c50da8859)*
- `src/aspire`  
*[_git/dotnet-aspire@48e42f5](https://dev.azure.com/dnceng/internal/_git/dotnet-aspire/?version=GC48e42f59d64d84b404e904996a9ed61f2a17a569)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@252ad5d](https://github.com/dotnet/aspnetcore/tree/252ad5d3be61ea4aa4bfb2453e0d22f52f56ad09)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@dddb219](https://github.com/google/googletest/tree/dddb219c3eb96d7f9200f09b0a381f016e6b4562)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@02026e5](https://github.com/dotnet/cecil/tree/02026e5c1b054958851d2711fefa1b37027cab23)*
- `src/command-line-api`  
*[dotnet/command-line-api@a045dd5](https://github.com/dotnet/command-line-api/tree/a045dd54a4c44723c215d992288160eb1401bb7f)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@b4f8847](https://github.com/dotnet/deployment-tools/tree/b4f8847a36543b3274dc252534d0175de35bd16c)*
- `src/diagnostics`  
*[dotnet/diagnostics@dec7d6e](https://github.com/dotnet/diagnostics/tree/dec7d6e7a27cc6a46bad117800b7002a6f4c2f20)*
- `src/emsdk`  
*[dotnet/emsdk@5cda864](https://github.com/dotnet/emsdk/tree/5cda86493ac07dce11dcb04323d2b57eecff00b7)*
- `src/format`  
*[dotnet/format@2c2d58c](https://github.com/dotnet/format/tree/2c2d58cb25064036f853d76e7b6aff7bb7d38401)*
- `src/fsharp`  
*[dotnet/fsharp@cc74185](https://github.com/dotnet/fsharp/tree/cc741852156e5f048e4e046061fa36477f8b92fb)*
- `src/installer`  
*[dotnet/installer@bf4dd46](https://github.com/dotnet/installer/tree/bf4dd46e606c73b89ebf361f48ed62fc5f2c3f14)*
- `src/msbuild`  
*[dotnet/msbuild@7f5b7a9](https://github.com/dotnet/msbuild/tree/7f5b7a942e7efb44f59a2451b13795972539521d)*
- `src/nuget-client`  
*[nuget/nuget.client@e92be39](https://github.com/nuget/nuget.client/tree/e92be3915309e687044768de38933ac5fc4cb40c)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@df38336](https://github.com/dotnet/razor/tree/df383360c34ada8889fdf18dc36d245f2938db66)*
- `src/roslyn`  
*[dotnet/roslyn@237fb52](https://github.com/dotnet/roslyn/tree/237fb52c683601ed639f1fdeaf38ceef0c768fbc)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@8a92037](https://github.com/dotnet/roslyn-analyzers/tree/8a92037d28baf58560622cb4685ecefdc828c2c8)*
- `src/runtime`  
*[dotnet/runtime@1063fc2](https://github.com/dotnet/runtime/tree/1063fc278534a8b3e8f087fd3fe9c26dc03a9697)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@bfde902](https://github.com/dotnet/scenario-tests/tree/bfde902a10d7b672f4fc7e844198ede405dbb9c6)*
- `src/sdk`  
*[dotnet/sdk@6176125](https://github.com/dotnet/sdk/tree/61761254a0b6f1709138d39ef715c72979211938)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@2054f99](https://github.com/dotnet/source-build-externals/tree/2054f99a5685034a4ea4755b3cc2984fb2a5e4a8)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[Microsoft/ApplicationInsights-dotnet@5e2e7dd](https://github.com/Microsoft/ApplicationInsights-dotnet/tree/5e2e7ddda961ec0e16a75b1ae0a37f6a13c777f5)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@bb354ce](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/bb354ceabed19189245e075abb864f327b6c14ad)*
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
*[dotnet/source-build-reference-packages@ef691e3](https://github.com/dotnet/source-build-reference-packages/tree/ef691e3c401949dab9986a50d8288a6e489f72bb)*
- `src/sourcelink`  
*[dotnet/sourcelink@a590496](https://github.com/dotnet/sourcelink/tree/a5904960b8d365b59cb69639ea1024ab17d80c1f)*
- `src/symreader`  
*[dotnet/symreader@6b058bf](https://github.com/dotnet/symreader/tree/6b058bf0b8942460db4f0ef953e8a0e44db0474c)*
- `src/templating`  
*[dotnet/templating@51c8563](https://github.com/dotnet/templating/tree/51c8563c2b36b767013fa576c345d3f6e3734437)*
- `src/test-templates`  
*[dotnet/test-templates@416a1b0](https://github.com/dotnet/test-templates/tree/416a1b0184b99ba67edf8991adc74517ec72cd89)*
- `src/vstest`  
*[microsoft/vstest@aef3d5f](https://github.com/microsoft/vstest/tree/aef3d5fbe837aef3f7cc54bba375b744db9ed8e6)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@cd81338](https://github.com/dotnet/windowsdesktop/tree/cd813382efc14c011d79679a654d77f5743d7769)*
- `src/winforms`  
*[dotnet/winforms@a83ef80](https://github.com/dotnet/winforms/tree/a83ef804bdc6fde593302ea0628c04171b2afac9)*
- `src/wpf`  
*[dotnet/wpf@d1793bf](https://github.com/dotnet/wpf/tree/d1793bf46d221789df9bf3bcde00cba11690c868)*
- `src/xdt`  
*[dotnet/xdt@676b9dd](https://github.com/dotnet/xdt/tree/676b9ddede4b3843bb41af274343e7eebd79169a)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@194f328](https://github.com/dotnet/xliff-tasks/tree/194f32828726c3f1f63f79f3dc09b9e99c157b11)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
