# Sign Tool

This is a MSBuild custom task that provides batch signing and simple verification for MicroBuild environments. The tool is intended to be run as a post-build step and is able to automatically infer the files that need to be signed given a list of container files (.nupkg, .vsix, etc) as input. The high level features of this tool are:

- **Performance:** It operates as a post-build step and uses the minimum number of requests possible. This can have a dramatic performance improvement over the typical implementation which signs as a post-compile step. For example, in Roslyn it took build + sign times down from 1-2 hours to 8 minutes.
- **Automated:** It is able to recursively open container files (like .nupkg, .vsix) and create a list of nested files that need to be signed. Therefore, you don't need to manually specify which files need to be signed.
- **Explicit Signing Info:**
  - The tool supports receiving an *explicit list of files* that need to be signed or ignored from the signing process.
  - The tool support specifying which certificate should be used for each file and is able to identify files based on public key token, target framework, extension and file name.
  - The tool will report an error if a [signable file](https://microsoft.sharepoint.com/teams/codesigninfo/Wiki/Signable%20Files.aspx) is discovered for which no signing information was found. That means that all signable files will need to be either explicitly ignored or have signing information attributed in one of the many ways explained below.
- **Containers:** It can handle the nesting issues that come with container files: both nested PE and nested containers. The tool will correctly sign the inner most content first, repack the containing package with the signed content and then sign the outermost container. Arbitrary levels of nesting are supported.
- **Post signing checks:** It takes extra steps to ensure that **PE files** are properly signed after the signing process completes. This tool parses the signed binary file to make sure the binary was modified and marked as signed.
- **Dual certificates:** The tool can receive a list of certificate descriptors which contain a property that can flag a certificate as dual. Dual certificates can be used to sign already signed files.

## Arguments

| Name                   | Type     | Description                                                  |
| ---------------------- | -------- | ------------------------------------------------------------ |
| DryRun                 | bool     | When true the list of files to be signed will be created but won't be sent to the signing server. Default is false. |
| TestSign               | bool     | When true the binaries will be test signed. The default is to real sign. |
| DoStrongNameCheck      | bool     | When true binaries will be checked for valid strong name signature. The default is false. |
| **ItemsToSign**        | Array    | This is a list of *full path* to files that need to be signed. Container files will be expanded to look for nested files that need to be signed. |
| **ItemsToSkipStrongNameCheck** | Array  | This is a list of *file names* that should be skipped when performing strong-name check. |
| StrongNameSignInfo     | Array    | Should store the default certificate name and strong name to be used for a given Public Key Token. See details below. |
| FileSignInfo           | Array    | Used to override the default certificate information for specific files and target frameworks combinations. If not specified default information is used or error occurs. See details below. |
| FileExtensionSignInfo  | Array    | This is a mapping between extension (in the format ".ext") to default sign information for those kind of files. Overriding of the default sign info is done using the other parameters. |
| CertificatesSignInfo   | Array    | List of certificate names that can be flagged using the `DualSigningAllowed` attribute as dual certificates. |
| **MicroBuildCorePath** | Dir Path | Path to MicroBuild.Core package directory.                   |
| DotNetPath             | Exe path | Path to the dotnet executable used to run the signing process on MicroBuild. |
| SNBinaryPath           | Exe path | Path to the sn.exe binary used to strong-name sign / validate signature of managed files. |
| **TempDir**            | Dir path | Used to store temporary files during the process of calling MicroBuild signing. |
| LogDir                 | Dir path | MSBuild binary log information from the signing rounds will be stored in this directory. |

**Note:** 

​	Items in bold are required: `ItemsToSign`, `MicroBuildCorePath` and `TempDir`.

​	`DotNetPath`, `SNBinaryPath` and `LogDir` are only required if `DryRun == false`.


# Arguments Metadata

**StrongNameSignInfo** - Optional parameter

This field **requires** the following metadata: `PublicKeyToken`, `CertificateName` and the `Include` field is assumed to hold the `Strong Name`. This information will be used as the default certificate and strong name information for all **PE files** that match the `PublicKeyToken`.

**FileExtensionSignInfo** - Optional parameter

This field requires two metadata attributes: `CertificateName` and `Include` which should be a file extension in the format `.ext`. This field is used to configure a default certificate for all files that have an specific extension.

Note:  Technically this is optional, but it's best practice to at least provide the default value for this property when calling signtool (ie. `FileExtensionSigninfo=@(FileExtensionSignInfo)`.  If this property is entirely ommitted, signtool will ignore all files.

**CertificatesSignInfo** - Optional parameter

This field requires the following metadata: `DualSigningAllowed` (boolean) and `Include` which is assumed to hold a valid certificate name. Use this list to explicitly configure the tool to allow the use of the specified certificate as a dual certificate - i.e., be able to use it to sign already signed files.

**FileSignInfo** - Optional parameter

This field accepts the following metadata: `PublicKeyToken` (*optional*), `CertificateName`, `TargetFramework` (*optional*) and the `Include` field is assumed to hold a file name (*including extension; not a full path*). The `CertificateName` attribute accepts the value "*None*" to flag a file that should not be signed.

All files that match the provided combination will use the Signing information provided. This applies to files discovered during recursive signing.

**ItemsToSign** - Required parameter

This field only attribute should be  `Include`. This field holds a list of full path to files that need to be considered during the signing process. Path to containers and regular files are accepted. Containers will be opened and their content will be processed recursively.

# Signing Info Precedence

The signing information (SI) for a given file is looked up in the order shown below. Later conditions override previous ones.

1. A default SI is looked up on `FileExtensionSignInfo` **based on the extension of the file**.
2. If the file is a **managed PE file**, SI based on the file *Public Key Token* (and only on it) is looked up in the `StrongNameSignInfo` parameter.
3. If the file is a PE file (not necessarily managed), SI based on a combination of **file name, public key token and target framework** is looked up in `FileSignInfo`. *If there is no match, then:*
4. If the file is a PE file (not necessarily managed), SI based on a combination of **file name and public key token** is looked up in `FileSignInfo`. *If there is no match, then:*
5. SI based **only on file name** is looked up in `FileSignInfo`.

Note that the logic starts looking for SI on a broad scope and then looks for specific information for the file. Also, the last three conditions are mutually exclusive.

At the end, if the file is signable but no signing information was determined for the file an error message will be logged and execution is expected to fail.

#### The Signing.props file

The Arcade SDK include a set of predefined configurations for the SignTool in the [Sign.proj](../../src/Microsoft.DotNet.Arcade.Sdk/tools/Sign.proj) file. However, you can override/remove/update any of these configurations by including a file named `Signing.props` in the `\eng\` folder of your repository. See examples on the next section.

#### Signing 3rd Party Binaries

Any 3rd party assembly which is distributed on public Microsoft feeds is 
supposed to be signed with the "3PartySHA2" certificate - a dual certificate. 
Arcade itself use the SignTool and as such the Arcade SDK is configured to
dual sign 3rd party libraries that it uses. In case you need to sign 
3rd party files take a look at [how Arcade does it](../../eng/Signing.props).

## Usage Examples

#### 1. Use the SDK predefined configuration

The Arcade SDK will [include](../../src/Microsoft.DotNet.Arcade.Sdk/tools/Sign.proj) all NuGet packages from the `$(ArtifactsPackagesDir)` and all VSIX packages from the`$(VisualStudioSetupOutputPath)` folder (these properties are set [here](../../src/Microsoft.DotNet.Arcade.Sdk/tools/RepoLayout.props)) in the list of containers to be looked up for - `ItemsToSign`. Note that only projects marked with `<IsPackable>true</IsPackable>` will be packed and copied to these folders. 

The [default configuration](../../src/Microsoft.DotNet.Arcade.Sdk/tools/Sign.proj) of Arcade SDK + SignTool also assigns default certificates to many signable file types and to all files that have the `31bf3856ad364e35` Public Key Token. Therefore, if all files that your repo need to sign are covered under these conditions you won't need to do any specific setup for the tool.

#### 2. Use a different certificate for an specific Public Key Token

If your repo has signable files that have a different Public Key Token than the one preconfigured in the SDK (i.e., `31bf3856ad364e35`) you might add an entry to `StrongNameSignInfo` to specify the certificate name that should be used for those files. To do that, place an entry like the one show below in your `eng\Signing.props` file.

```xml
<ItemGroup>
	<StrongNameSignInfo Include="(MSBuildThisFileDirectory)..\StrongName1.snk" PublicKeyToken="4321abcda1b2c3d4" CertificateName="DifferentCertName" />
</ItemGroup>
```

If that is your only custom configuration all files with that Public Key Token will be signed with the `DifferentCertName` certificate and `StrongName1` strong name.

#### 3. Configure signing information for an specific file

It is possible to override the default signing information or explicitly skip signing for specific files. For that you need to use the `FileSignInfo` property. For instance, in the snippet below the certificate `MyCustomCert` will be used for `My.Library.dll` when it targets `.NETStandard,Version=v2.0` and has Public Key Token `31bf3856ad364e35`:

```xml
<ItemGroup> 
    <FileSignInfo Include="My.Library.dll" TargetFramework=".NETStandard,Version=v2.0" PublicKeyToken="31bf3856ad364e35" CertificateName="MyCustomCert" />
</ItemGroup>
```
In this snippet the library `Other.Library.dll` with Public Key Token `31bf3856ad364e35` won't be signed, independent of its target framework:

```xml
<ItemGroup>
  <FileSignInfo Include="Other.Library.dll" PublicKeyToken="31bf3856ad364e35" CertificateName="None" />
</ItemGroup>
```
#### 4. How to remove all preconfigured signing information?

To remove *all* preconfigured signing information put the following snippet in your `eng\Signing.props` file:

```xml
<ItemGroup>
    <!-- Remove all predefined dual certificate information -->
	<CertificatesSignInfo Remove="@(CertificatesSignInfo)" />
    
    <!-- Remove all automatically included packages -->
	<ItemsToSign Remove="@(ItemsToSign)" />
    
    <!-- Remove default signing for `31bf3856ad364e35` PKT -->
	<StrongNameSignInfo Remove="@(StrongNameSignInfo)" />
    
    <!-- Remove default signing for signable extensions -->
	<FileExtensionSignInfo Remove="@(FileExtensionSignInfo)" />
</ItemGroup>
```

#### 5. How to explicitly list the certificates for each file to be signed?

Assuming that you *don't want* to use any of the preconfigured information in the SDK and want to specify by yourself the list of files to be signed and the certificate to be used for each file, take a look at the example below:

```xml
<ItemGroup>
    <!-- Remove all preconfigured signing info as shown above in (4). -->
	
	<!-- My custom list of files to be signed -->
	<ItemsToSign Include="c:\build\file1.dll" />
	<ItemsToSign Include="c:\build\file2.ps1" />
	<ItemsToSign Include="c:\build\file3.js" />
	<ItemsToSign Include="c:\build\file4.nupkg" />
		
	<!-- configure the certificate for each file -->
	<FileSignInfo Include="file1.dll" CertificateName="DLLCert" />
	<FileSignInfo Include="file2.ps1" CertificateName="PS1Cert" />
	<FileSignInfo Include="file3.js" CertificateName="JSCert" />
	<FileSignInfo Include="file4.nupkg" CertificateName="NuGet" />
	
	<!-- Assuming this file is present in file4.nupkg -->
	<!-- 
		 If we hadn't specified this information here the signing process 
		 would fail because no signing information would be found for a 
		 signable file (.dll) 
	-->
	<FileSignInfo Include="nested.dll" CertificateName="DeepCert" />
</ItemGroup>
```

#### 6. How to sign different files that have same name?

The tool assumes that you will be able to differentiate the files using name, public key token or target framework. If you have two files with the same name your only option (currently) is to differentiate them using public key token or target framework as below:

```xml
<ItemGroup>
    <!-- Note that these are full paths -->
	<ItemsToSign Include="c:\build\pack1\lib.dll" />
	<ItemsToSign Include="c:\build\pack2\lib.dll" />
	<ItemsToSign Include="c:\build\pack3\lib.dll" />
	
    <!-- Note that here only file names + extension are used -->
	<FileSignInfo Include="lib.dll" 
                  CertificateName="File1Cert" 
                  PublicKeyToken="a1b2c3d4e5f6g7h8" 
                  TargetFramework=".NETStandard,Version=v2.0" />
	
    <FileSignInfo Include="lib.dll" 
                  CertificateName="File2Cert" 
                  PublicKeyToken="abcdefghi12345678" 
                  TargetFramework=".NETCore,Version=v2.0" />
	
    <FileSignInfo Include="lib.dll" 
                  CertificateName="File3Cert" 
                  PublicKeyToken="abcdefghi12345678" 
                  TargetFramework=".NETStandard,Version=v2.0" />
</ItemGroup>
```
#### 7. How should I call the SignToolTask?

Click [here](../../src/Microsoft.DotNet.Arcade.Sdk/tools/Sign.proj) to see how the Arcade SDK calls the `SignToolTask`. Here's a snippet:

```xml
...
<Microsoft.DotNet.SignTool.SignToolTask
    DryRun="$(DryRun)"
    TestSign="$(TestSign)"
    CertificatesSignInfo="$(CertificatesSignInfo)"
    ItemsToSign="@(ItemsToSign)"
    StrongNameSignInfo="@(StrongNameSignInfo)"
    FileSignInfo="@(FileSignInfo)"
    FileExtensionSignInfo="@(FileExtensionSignInfo)"
    TempDir="$(ArtifactsTmpDir)"
    LogDir="$(ArtifactsLogDir)"
    MSBuildPath="$(DesktopMSBuildPath)"
    MicroBuildCorePath="$(NuGetPackageRoot)microbuild.core\$(MicroBuildCoreVersion)"/>
...
```

#### 8. How can I use the .NET specific certificate for executable files?

By default, `Microsoft400` is the cert used by default for most executable files (e.g .dll, .js, .exe). To use the .NET specific cert (`MicrosoftDotNet500`),
use one of the following approaches:

1. Update the existing `FileExtensionSignInfo` and `StrongNameSignInfo` metadata in [Sign.props](../../src/Microsoft.DotNet.Arcade.Sdk/tools/Sign.props)
to use `MicrosoftDotNet500`. This approach must be used if some files still need to be signed with `Microsoft400`.
   ```
   <ItemGroup>
     <FileExtensionSignInfo Update=".js;.ps1;.psd1;.psm1;.psc1;.py;.dll;.exe" CertificateName="MicrosoftDotNet500" />
     <StrongNameSignInfo Update="MsSharedLib72" PublicKeyToken="31bf3856ad364e35" CertificateName="MicrosoftDotNet500" />
     <StrongNameSignInfo Update="SilverlightCert121" PublicKeyToken="7cec85d7bea7798e" CertificateName="MicrosoftDotNet500" />
     <StrongNameSignInfo Update="StrongName" PublicKeyToken="b77a5c561934e089" CertificateName="MicrosoftDotNet500" />
     <StrongNameSignInfo Update="StrongName" PublicKeyToken="b03f5f7f11d50a3a" CertificateName="MicrosoftDotNet500" />
     <StrongNameSignInfo Update="$(MSBuildThisFileDirectory)snk\Open.snk" PublicKeyToken="cc7b13ffcd2ddd51" CertificateName="MicrosoftDotNet500" />
   </ItemGroup>
   ```
2. Specify the property `UseDotNetCertificate` with value `true` in your `eng/Signing.props` file. This **replaces** all existing use of `Microsoft400` with `MicrosoftDotNet500`.
   ```
   <PropertyGroup>
     <UseDotNetCertificate>f</UseDotNetCertificate>
   </PropertyGroup>
   ```

#### 9. Certificate Subjects

The authenticode certificates in use by .NET's core binaries have specific subjects that have meaning to various tools. These subjects are **not** expected to change. However, if they do, or if we introduce a new certificate for signing a specific .NET binary (e.g. a new DAC), .NET tactics and the Defender team should be contacted with this information. We want to ensure that the certificates are explicitly trusted by Defender.

Examples of subjects that should be treated with special care:

```
CN=.NET, O=Microsoft Corporation, L=Redmond, S=Washington, C=US
CN=.NET, O=Microsoft Corporat, L=Redmond, S=Washington, C=US
CN=.NET DAC, O=Microsoft Corporation, L=Redmond, S=Washington, C=US
```

## Logs & MicroBuild configuration files

The log messages from the SignToolTask itself will be included in the log (+.binlog) of the original build process. The binary log of executing the MicroBuild signing plugin will be stored in files named `SigningX.binlog` in the `LogDir` folder. The project files used to call the MicroBuild plugin will be stored in files named `RoundX.proj` in the `TempDir` folder. In both cases the `X` in the name refers to a signing round.

## Valid Argument Values

**Certificate Name:** name of the Authenticode certificate to use for signing. Valid values include `Microsoft402`, `WindowsPhone623`, `MicrosoftSHA1Win8WinBlue` and `VsixSHA2`.  

**Strong Name:** name of the key to use when strong naming the binary. This can be `null` for values which do not require strong name signing such as VSIX files. 

**Target Framework:** valid values include (but are not limited to): `.NETStandard,Version=v2.0`,  `.NETFramework,Version=v4.6.1`, `.NET Core,Version=v2.0`, etc.

**Public Key Token:** valid values are 16 characters in length comprising values between `[0-9]` and `[a-Z]`.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CCorePackages%5CSigning.md)](https://helix.dot.net/f/p/5?p=Documentation%5CCorePackages%5CSigning.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CCorePackages%5CSigning.md)</sub>
<!-- End Generated Content-->
