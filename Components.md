# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@e96d0e9](https://github.com/dotnet/arcade/tree/e96d0e9f197ef2be216415e19ec927adfaefb82a)*
- `src/aspire`  
*[dotnet/aspire@66a1dd7](https://github.com/dotnet/aspire/tree/66a1dd77e4077592a587c1429c8814d1057dc474)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@a19c70e](https://github.com/dotnet/aspnetcore/tree/a19c70eba3f89c7cbe9447f029b4c63322f47846)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@dddb219](https://github.com/google/googletest/tree/dddb219c3eb96d7f9200f09b0a381f016e6b4562)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@02026e5](https://github.com/dotnet/cecil/tree/02026e5c1b054958851d2711fefa1b37027cab23)*
- `src/command-line-api`  
*[dotnet/command-line-api@27035e8](https://github.com/dotnet/command-line-api/tree/27035e88527f555a3806ae7d63af7501b41ea5d5)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@b4f8847](https://github.com/dotnet/deployment-tools/tree/b4f8847a36543b3274dc252534d0175de35bd16c)*
- `src/diagnostics`  
*[dotnet/diagnostics@9b4cfdf](https://github.com/dotnet/diagnostics/tree/9b4cfdfde85a1bcab1e87e78be8db99785ba3e1f)*
- `src/emsdk`  
*[dotnet/emsdk@5cda864](https://github.com/dotnet/emsdk/tree/5cda86493ac07dce11dcb04323d2b57eecff00b7)*
- `src/format`  
*[dotnet/format@eda92c2](https://github.com/dotnet/format/tree/eda92c290b08a2be0fcb8e79ac07ebc94dffb2a3)*
- `src/fsharp`  
*[dotnet/fsharp@b65db15](https://github.com/dotnet/fsharp/tree/b65db15f3262c439f81e6d02c0b8a6892a737e60)*
- `src/installer`  
*[dotnet/installer@8683de0](https://github.com/dotnet/installer/tree/8683de064555fb69cb31769fd3b7938cfdc27ff4)*
- `src/msbuild`  
*[dotnet/msbuild@1725b24](https://github.com/dotnet/msbuild/tree/1725b247e8737804076c8ff3b09fcee02ecdf51e)*
- `src/nuget-client`  
*[nuget/nuget.client@e92be39](https://github.com/nuget/nuget.client/tree/e92be3915309e687044768de38933ac5fc4cb40c)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@569c9c5](https://github.com/dotnet/razor/tree/569c9c50f393066c853bc796f3b5c720b35e55aa)*
- `src/roslyn`  
*[dotnet/roslyn@237fb52](https://github.com/dotnet/roslyn/tree/237fb52c683601ed639f1fdeaf38ceef0c768fbc)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@8d8ac7d](https://github.com/dotnet/roslyn-analyzers/tree/8d8ac7de5f76d08f457465027b67f805a4153899)*
- `src/runtime`  
*[dotnet/runtime@fd37310](https://github.com/dotnet/runtime/tree/fd373103d7f27a0aa88b8ffba51d26c0d2b418bc)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@bfde902](https://github.com/dotnet/scenario-tests/tree/bfde902a10d7b672f4fc7e844198ede405dbb9c6)*
- `src/sdk`  
*[dotnet/sdk@ed82846](https://github.com/dotnet/sdk/tree/ed82846a16642e421e76925b4d9787ccda5a1500)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@6513a2c](https://github.com/dotnet/source-build-externals/tree/6513a2c0cd7be2706742181af63d717a90cec5be)*
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
*[dotnet/source-build-reference-packages@f2c3fed](https://github.com/dotnet/source-build-reference-packages/tree/f2c3fed62861b918dfe300f01b497551813a56df)*
- `src/sourcelink`  
*[dotnet/sourcelink@493e89e](https://github.com/dotnet/sourcelink/tree/493e89e2251674d974480b9523added944d45ed2)*
- `src/symreader`  
*[dotnet/symreader@1194d0b](https://github.com/dotnet/symreader/tree/1194d0bd9b2a0257706a2635531948ceaa02e729)*
- `src/templating`  
*[dotnet/templating@b88627c](https://github.com/dotnet/templating/tree/b88627cbe240db4bb211e7f6d5997f8d229e8a20)*
- `src/test-templates`  
*[dotnet/test-templates@7d2f271](https://github.com/dotnet/test-templates/tree/7d2f2719628e6744f3172a2d48e0d1f600b360c0)*
- `src/vstest`  
*[microsoft/vstest@1772349](https://github.com/microsoft/vstest/tree/17723493fc8befbb889db2ff17b1ac98ba7b7c48)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@179eaa8](https://github.com/dotnet/windowsdesktop/tree/179eaa8020ebdab03cbbf1625252b455906159c1)*
- `src/winforms`  
*[dotnet/winforms@a83ef80](https://github.com/dotnet/winforms/tree/a83ef804bdc6fde593302ea0628c04171b2afac9)*
- `src/wpf`  
*[dotnet/wpf@d1793bf](https://github.com/dotnet/wpf/tree/d1793bf46d221789df9bf3bcde00cba11690c868)*
- `src/xdt`  
*[dotnet/xdt@a10f0a8](https://github.com/dotnet/xdt/tree/a10f0a85b91b1e2e18cbd2ea2537eae9c5a64ea9)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
