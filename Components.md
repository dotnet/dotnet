# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@c246c9d](https://github.com/dotnet/arcade/tree/c246c9d7bfb98646ec52a18471d075219b26dccc)*
- `src/aspire`  
*[dotnet/aspire@66a1dd7](https://github.com/dotnet/aspire/tree/66a1dd77e4077592a587c1429c8814d1057dc474)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@b89e21e](https://github.com/dotnet/aspnetcore/tree/b89e21ed72c7afd58fd372f7b8e6299847a8153a)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@7c07a86](https://github.com/google/googletest/tree/7c07a863693b0c831f80473f7c6905d7e458682c)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@81facb3](https://github.com/dotnet/cecil/tree/81facb3f6009be2cdce70df30452bb75e9a8f993)*
- `src/command-line-api`  
*[dotnet/command-line-api@ecd2ce5](https://github.com/dotnet/command-line-api/tree/ecd2ce5eafbba3008a7d4f5d04b025d30928c812)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@b4f8847](https://github.com/dotnet/deployment-tools/tree/b4f8847a36543b3274dc252534d0175de35bd16c)*
- `src/diagnostics`  
*[dotnet/diagnostics@82c9a13](https://github.com/dotnet/diagnostics/tree/82c9a134f8c13dcfe8a9c243a19ee1861bbcb8ea)*
- `src/emsdk`  
*[dotnet/emsdk@5cda864](https://github.com/dotnet/emsdk/tree/5cda86493ac07dce11dcb04323d2b57eecff00b7)*
- `src/format`  
*[dotnet/format@0c424b6](https://github.com/dotnet/format/tree/0c424b69c28b068d03c89afa3fb0ebb18dc96ea2)*
- `src/fsharp`  
*[dotnet/fsharp@8d7795d](https://github.com/dotnet/fsharp/tree/8d7795d4a68a21010577f11084ba937e51daf9a3)*
- `src/installer`  
*[dotnet/installer@9b36bbc](https://github.com/dotnet/installer/tree/9b36bbc4ab15dd844a358551278f6b8467c64641)*
- `src/msbuild`  
*[dotnet/msbuild@f9b862a](https://github.com/dotnet/msbuild/tree/f9b862a1351872f041ca93b0a044379ba4cc37ba)*
- `src/nuget-client`  
*[nuget/nuget.client@d55931a](https://github.com/nuget/nuget.client/tree/d55931a69dcda3dcb87ba46a09fe268e0febc223)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@3552977](https://github.com/dotnet/razor/tree/35529773663446c499b7cee3ca924612c6e945b3)*
- `src/roslyn`  
*[dotnet/roslyn@3cd939f](https://github.com/dotnet/roslyn/tree/3cd939f76803da435c20b082a5cfcc844386fcfb)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@23ec029](https://github.com/dotnet/roslyn-analyzers/tree/23ec029b47d68b1a80348c0dabc3bccf013c1fe6)*
- `src/runtime`  
*[dotnet/runtime@dbb335c](https://github.com/dotnet/runtime/tree/dbb335c6ba14b38400b2d8c3a5876698021ec089)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@bfde902](https://github.com/dotnet/scenario-tests/tree/bfde902a10d7b672f4fc7e844198ede405dbb9c6)*
- `src/sdk`  
*[dotnet/sdk@734bbd9](https://github.com/dotnet/sdk/tree/734bbd953941b957173151a597c08e2bb75836d0)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@6fc8c1a](https://github.com/dotnet/source-build-externals/tree/6fc8c1ac45220a4d9b4c59bf2ff187dafcb1da3f)*
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
*[dotnet/source-build-reference-packages@549aadf](https://github.com/dotnet/source-build-reference-packages/tree/549aadff1660b230bdfffa562eea3edf59dd0bb4)*
- `src/sourcelink`  
*[dotnet/sourcelink@dd7d9a8](https://github.com/dotnet/sourcelink/tree/dd7d9a8c045af2612311cc3e61afa5c859e866ad)*
- `src/symreader`  
*[dotnet/symreader@b264fbe](https://github.com/dotnet/symreader/tree/b264fbe4dfb1e665a49f052940697bd6dabd561f)*
- `src/templating`  
*[dotnet/templating@fe2fe0b](https://github.com/dotnet/templating/tree/fe2fe0b57523d022f620a907441ce0a953702564)*
- `src/test-templates`  
*[dotnet/test-templates@81349c1](https://github.com/dotnet/test-templates/tree/81349c13c2b8e8babf1cdd4e7ab350fbb1b193a4)*
- `src/vstest`  
*[microsoft/vstest@1c68c24](https://github.com/microsoft/vstest/tree/1c68c247cc3d69ad04afeb574c3d0bc352ff16e5)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@02e434c](https://github.com/dotnet/windowsdesktop/tree/02e434c9aec0eaafcfefb260598de0882e3595c3)*
- `src/winforms`  
*[dotnet/winforms@03d1fd7](https://github.com/dotnet/winforms/tree/03d1fd7ba7f16194c80b5bf39a4879ac0c7a823c)*
- `src/wpf`  
*[dotnet/wpf@793eb57](https://github.com/dotnet/wpf/tree/793eb57cc75af91afad52eb77676c63f0001e49a)*
- `src/xdt`  
*[dotnet/xdt@a10f0a8](https://github.com/dotnet/xdt/tree/a10f0a85b91b1e2e18cbd2ea2537eae9c5a64ea9)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
