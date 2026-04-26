


// ----- DOS Header:
// Magic:                      0x5a4d
// Bytes on last page:         0x0090
// Pages in file:              0x0003
// Relocations:                0x0000
// Size of header (paragraphs):0x0004
// Min extra paragraphs:       0x0000
// Max extra paragraphs:       0xffff
// Initial (relative) SS:      0x0000
// Initial SP:                 0x00b8
// Checksum:                   0x0000
// Initial IP:                 0x0000
// Initial (relative) CS:      0x0000
// File addr. of reloc table:  0x0040
// Overlay number:             0x0000
// OEM identifier:             0x0000
// OEM info:                   0x0000
// File addr. of COFF header:  0x0080
// ----- COFF/PE Headers:
// Signature:                  0x00004550
// ----- COFF Header:
// Machine:                    0x014c
// Number of sections:         0x0004
// Time-date stamp:            0xc0c3eab2
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x0000d000
// Size of init.data:              0x00000a00
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00010e0a
// Base of code:                   0x00004000
// Base of data:                   0x00002000
// Image base:                     0x10000000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0004
// Minor subsystem version:        0x0000
// Size of image:                  0x00016000
// Size of headers:                0x00000400
// Checksum:                       0x000200da
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00010db8 [0x0000004f] address [size] of Import Directory:          
// 0x00012000 [0x00000530] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x0000de00 [0x00002470] address [size] of Security Directory:        
// 0x00014000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00010d9c [0x0000001c] address [size] of Debug Directory:           
// 0x00000000 [0x00000000] address [size] of Architecture Specific:     
// 0x00000000 [0x00000000] address [size] of Global Pointer:            
// 0x00000000 [0x00000000] address [size] of TLS Directory:             
// 0x00000000 [0x00000000] address [size] of Load Config Directory:     
// 0x00000000 [0x00000000] address [size] of Bound Import Directory:    
// 0x00004000 [0x00000008] address [size] of Import Address Table:      
// 0x00000000 [0x00000000] address [size] of Delay Load IAT:            
// 0x00004008 [0x00000048] address [size] of CLR Header:                


// Image sections:
//              .mvid
//              0x00000010 Virtual Size
//              0x00002000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x00000400 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

//              .text
//              0x0000ce10 Virtual Size
//              0x00004000 Virtual Address
//              0x0000d000 Size of Raw Data
//              0x00000600 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x60000020 Characteristics
//                         CNT_CODE
//                         MEM_EXECUTE
//                         MEM_READ

//              .rsrc
//              0x00000530 Virtual Size
//              0x00012000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x0000d600 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x00014000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x0000dc00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00010000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000e0c
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00004000 Import Address Table
//              0x00010dfa Import Name Table
//              0          Time Date Stamp
//              0          Index of First Forwarder Reference
//
//              0x0000  _CorDllMain

// Delay Load Import Address Table
// No data.

// Entry point code:
//FF 25 00 40 00 10 


// ----- CLR Header:
// Header size:                        0x00000048
// Major runtime version:              0x0002
// Minor runtime version:              0x0005
// 0x00004054 [0x0000ccc8] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00010d1c [0x00000080] address [size] of Strong Name Signature:     
// 0x00000000 [0x00000000] address [size] of CodeManager Table:         
// 0x00000000 [0x00000000] address [size] of VTableFixups Directory:    
// 0x00000000 [0x00000000] address [size] of Export Address Table:      
// 0x00000000 [0x00000000] address [size] of Precompile Header:         

// Metadata Header
//    Storage Signature:
//              0x424a5342 Signature
//                  0x0001 Major Version
//                  0x0001 Minor Version
//              0x00000000 Extra Data Offset
//              0x0000000c Version String Length
//              'v4.0.30319' Version String
//    Storage Header:
//                    0x00 Flags
//                  0x0005 Number of Streams
//    Stream 1:
//              0x0000006c Offset
//              0x00006638 Size
//              '#~' Name
//    Stream 2:
//              0x000066a4 Offset
//              0x00004354 Size
//              '#Strings' Name
//    Stream 3:
//              0x0000a9f8 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x0000a9fc Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x0000aa0c Offset
//              0x000022bc Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000016090ba01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 66160
// PE header size       : 1024 (536 used)    ( 1.55%)
// PE additional info   : 10855             (16.41%)
// Num.of PE sections   : 4
// CLR header size     : 72                 ( 0.11%)
// CLR meta-data size  : 52424              (79.24%)
// CLR additional info : 128                ( 0.19%)
// CLR method headers  : 666                ( 1.01%)
// Managed code         : 1332              ( 2.01%)
// Data                 : 2560              ( 3.87%)
// Unaccounted          : -2901             (-4.38%)

// Num.of PE sections   : 4
//   .mvid    - 512
//   .text    - 53248
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 52424
//   Module        -    1 (10 bytes)
//   TypeDef       -  168 (2352 bytes)    4 interfaces, 0 explicit layout
//   TypeRef       -  156 (936 bytes)
//   MethodDef     -  676 (9464 bytes)    10 abstract, 0 native, 666 bodies
//   FieldDef      -  245 (1470 bytes)    19 constant
//   MemberRef     -   47 (282 bytes)
//   ParamDef      -  546 (3276 bytes)
//   MethodImpl    -   64 (384 bytes)
//   Constant      -   27 (162 bytes)
//   CustomAttribute-  658 (3948 bytes)
//   InterfaceImpl -   81 (324 bytes)
//   PropertyMap   -   45 (180 bytes)
//   Property      -  156 (936 bytes)
//   MethodSemantic-  216 (1296 bytes)
//   TypeSpec      -   13 (26 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -   23 (460 bytes)
//   NestedClass   -   99 (396 bytes)
//   GenericParam  -   16 (128 bytes)
//   GenericParamConstraint-    2 (8 bytes)
//   Strings       - 17235 bytes
//   Blobs         -  8892 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   217 bytes

// CLR additional info : 128

// CLR method headers : 666
//   Num.of method bodies  - 666
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 666

// Managed code : 1332
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class TypeNameHelper                 (abstract) (auto) (ansi) (sealed) 
// Class ExceptionDetails               (auto) (ansi) 
// Class ExceptionDetailsProvider       (auto) (ansi) 
// Class LoggerExtensions               (abstract) (auto) (ansi) (sealed) 
// Class MethodDisplayInfo              (auto) (ansi) 
// Class ParameterDisplayInfo           (auto) (ansi) 
// Class StackFrameInfo                 (auto) (ansi) 
// Class StackFrameSourceCodeInfo       (auto) (ansi) 
// Class StackTraceHelper               (auto) (ansi) 
// Class AttributeValue                 (auto) (ansi) 
// Class BaseView                       (abstract) (auto) (ansi) 
// Class HelperResult                   (auto) (ansi) 
// Class GenericHostWebHostBuilderExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class WebHostBuilderOptions          (public) (auto) (ansi) 
// Class ManifestStaticWebAssetFileProvider (auto) (ansi) (sealed) 
// Class DefaultHttpContextFactory      (public) (auto) (ansi) 
// Class GenericWebHostApplicationLifetime (auto) (ansi) 
// Class GenericWebHostBuilder          (auto) (ansi) 
// Class GenericWebHostService          (auto) (ansi) (sealed) 
// Class GenericWebHostServiceOptions   (auto) (ansi) 
// Class HostingStartupWebHostBuilder   (auto) (ansi) 
// Interface ISupportsUseDefaultServiceProvider (abstract) (auto) (ansi) 
// Class ActivityFeature                (auto) (ansi) (sealed) 
// Class ApplicationLifetime            (auto) (ansi) 
// Class ConfigureBuilder               (auto) (ansi) 
// Class ConfigureContainerBuilder      (auto) (ansi) 
// Class ConfigureServicesBuilder       (auto) (ansi) 
// Class ErrorPageBuilder               (abstract) (auto) (ansi) (sealed) 
// Class HostedServiceExecutor          (auto) (ansi) 
// Class HostingApplication             (auto) (ansi) 
// Class HostingApplicationDiagnostics  (auto) (ansi) 
// Class HostingEnvironment             (auto) (ansi) 
// Class HostingEnvironmentExtensions   (abstract) (auto) (ansi) (sealed) 
// Class HostingEventSource             (auto) (ansi) (sealed) 
// Class HostingLoggerExtensions        (abstract) (auto) (ansi) (sealed) 
// Class HostingRequestFinishedLog      (auto) (ansi) 
// Class HostingRequestStartingLog      (auto) (ansi) 
// Class LoggerEventIds                 (abstract) (auto) (ansi) (sealed) 
// Class MethodInfoExtensions           (abstract) (auto) (ansi) (sealed) 
// Class ServiceCollectionExtensions    (abstract) (auto) (ansi) (sealed) 
// Class StartupLoader                  (auto) (ansi) 
// Class StartupMethods                 (auto) (ansi) 
// Class WebHost                        (auto) (ansi) (sealed) 
// Class WebHostLifetime                (auto) (ansi) 
// Class WebHostOptions                 (auto) (ansi) 
// Class WebHostUtilities               (auto) (ansi) 
// Class ConventionBasedStartup         (auto) (ansi) 
// Class DelegateStartup                (public) (auto) (ansi) 
// Class StartupBase                    (public) (abstract) (auto) (ansi) 
// Class StartupBase`1                  (public) (abstract) (auto) (ansi) 
// Class WebHostBuilder                 (public) (auto) (ansi) 
// Class WebHostBuilderExtensions       (public) (abstract) (auto) (ansi) (sealed) 
// Class WebHostExtensions              (public) (abstract) (auto) (ansi) (sealed) 
// Class Resources                      (abstract) (auto) (ansi) (sealed) 
// Class ErrorPage                      (auto) (ansi) 
// Class ErrorPageModel                 (auto) (ansi) 
// Class ErrorPageModelBuilder          (abstract) (auto) (ansi) (sealed) 
// Class StaticWebAssetsLoader          (public) (auto) (ansi) 
// Class ServerAddressesFeature         (public) (auto) (ansi) 
// Class StartupLinkerOptions           (abstract) (auto) (ansi) (sealed) 
// Interface ISupportsConfigureWebHost      (public) (abstract) (auto) (ansi) 
// Interface ISupportsStartup               (public) (abstract) (auto) (ansi) 
// Class ApplicationBuilderFactory      (public) (auto) (ansi) 
// Interface IApplicationBuilderFactory     (public) (abstract) (auto) (ansi) 
// Class DisplayNameOptions             (sequential) (ansi) (sealed) (nested private) 
// Class <GetDetails>d__4               (auto) (ansi) (sealed) (nested private) 
// Class <ReadLines>d__9                (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <ExecuteAsync>d__31            (auto) (ansi) (sealed) (nested private) 
// Class <ExecuteAsync>d__32            (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class StaticWebAssetsDirectoryContents (auto) (ansi) (sealed) (nested private) 
// Class StaticWebAssetsDirectoryInfo   (auto) (ansi) (sealed) (nested private) 
// Class StaticWebAssetsFileInfo        (auto) (ansi) (sealed) (nested private) 
// Class FileNameComparer               (auto) (ansi) (sealed) (nested private) 
// Class StaticWebAssetManifest         (auto) (ansi) (sealed) (nested assembly) 
// Class StaticWebAssetNode             (auto) (ansi) (sealed) (nested assembly) 
// Class StaticWebAssetMatch            (auto) (ansi) (sealed) (nested assembly) 
// Class StaticWebAssetPattern          (auto) (ansi) (sealed) (nested assembly) 
// Class OSBasedCaseConverter           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass7_0           (auto) (ansi) (sealed) (nested private) 
// Class HostServiceProvider            (auto) (ansi) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass10_0          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass11_0          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass12_0          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass13_0          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass14_0`1        (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass15_0          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass17_0          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass18_0          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass18_1          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass18_2          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass6_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass6_1           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass9_0           (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class <StartAsync>d__37              (auto) (ansi) (sealed) (nested private) 
// Class <StopAsync>d__38               (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass6_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass4_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass10_0          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass8_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass8_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass9_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass0_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass3_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass4_0           (auto) (ansi) (sealed) (nested private) 
// Class <ExecuteAsync>d__5             (auto) (ansi) (sealed) (nested private) 
// Class Context                        (auto) (ansi) (nested assembly) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <GetEnumerator>d__12           (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <GetEnumerator>d__11           (auto) (ansi) (sealed) (nested private) 
// Class ConfigureServicesDelegateBuilder (abstract) (auto) (ansi) (nested private) 
// Class ConfigureServicesDelegateBuilder`1 (auto) (ansi) (nested private) 
// Class <>c__DisplayClass3_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass8_0           (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <DisposeAsync>d__34            (auto) (ansi) (sealed) (nested private) 
// Class <DisposeServiceProviderAsync>d__35 (auto) (ansi) (sealed) (nested private) 
// Class <StartAsync>d__27              (auto) (ansi) (sealed) (nested private) 
// Class <StopAsync>d__32               (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass10_0          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass14_0          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass14_1          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass14_2          (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass0_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass1_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass1_1           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass2_0`1         (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass2_1`1         (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass3_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass5_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass6_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass7_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass8_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass9_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass9_1           (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <RunAsync>d__4                 (auto) (ansi) (sealed) (nested private) 
// Class <RunAsync>d__5                 (auto) (ansi) (sealed) (nested private) 
// Class <StopAsync>d__0                (auto) (ansi) (sealed) (nested private) 
// Class <WaitForShutdownAsync>d__2     (auto) (ansi) (sealed) (nested private) 
// Class <WaitForTokenShutdownAsync>d__6 (auto) (ansi) (sealed) (nested private) 
// Class <ExecuteAsync>d__0             (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class HostingLogScope                (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass13_0          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass14_0          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass15_0          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass15_1          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass16_0          (auto) (ansi) (sealed) (nested private) 
// Class <GetEnumerator>d__9            (auto) (ansi) (sealed) (nested private) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ Microsoft.Extensions.FileProviders.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ Microsoft.Extensions.Logging.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ System.Diagnostics.StackTrace
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000005*/ Microsoft.AspNetCore.Http.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000006*/ System.Text.Encodings.Web
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 6:0:0:0
}
.assembly extern /*23000007*/ Microsoft.Extensions.Hosting.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000008*/ Microsoft.AspNetCore.Hosting.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000009*/ Microsoft.Extensions.DependencyInjection.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*2300000A*/ Microsoft.Extensions.Primitives
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*2300000B*/ System.Collections
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*2300000C*/ System.Text.Json
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 6:0:0:0
}
.assembly extern /*2300000D*/ System.ComponentModel
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*2300000E*/ Microsoft.Extensions.Features
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*2300000F*/ Microsoft.AspNetCore.Http
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000010*/ Microsoft.Extensions.Configuration.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000011*/ Microsoft.Extensions.DependencyInjection
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000012*/ System.Diagnostics.DiagnosticSource
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 6:0:0:0
}
.assembly extern /*23000013*/ Microsoft.Extensions.Options
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000014*/ Microsoft.AspNetCore.Hosting.Server.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000015*/ System.Diagnostics.Tracing
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000016*/ System.Threading
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000017*/ Microsoft.Extensions.Logging
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly /*20000001*/ Microsoft.AspNetCore.Hosting
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000004:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000005:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000002*/::.ctor(int32) /* 0A000002 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000006:0A000003*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000003*/::.ctor() /* 0A000003 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000007:0A000004*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*//DebuggingModes/*01000005*/) /* 0A000004 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000008:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 81 6E 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ...nMicrosoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 48 6F 73 74 69 6E 67   // pNetCore.Hosting
                                                                                                                                                                                    2E 54 65 73 74 73 2C 20 50 75 62 6C 69 63 4B 65   // .Tests, PublicKe
                                                                                                                                                                                    79 3D 30 30 32 34 30 30 30 30 30 34 38 30 30 30   // y=00240000048000
                                                                                                                                                                                    30 30 39 34 30 30 30 30 30 30 30 36 30 32 30 30   // 0094000000060200
                                                                                                                                                                                    30 30 30 30 32 34 30 30 30 30 35 32 35 33 34 31   // 0000240000525341
                                                                                                                                                                                    33 31 30 30 30 34 30 30 30 30 30 31 30 30 30 31   // 3100040000010001
                                                                                                                                                                                    30 30 66 33 33 61 32 39 30 34 34 66 61 39 64 37   // 00f33a29044fa9d7
                                                                                                                                                                                    34 30 63 39 62 33 32 31 33 61 39 33 65 35 37 63   // 40c9b3213a93e57c
                                                                                                                                                                                    38 34 62 34 37 32 63 38 34 65 30 62 38 61 30 65   // 84b472c84e0b8a0e
                                                                                                                                                                                    31 61 65 34 38 65 36 37 61 39 66 38 66 36 64 65   // 1ae48e67a9f8f6de
                                                                                                                                                                                    39 64 35 66 37 66 33 64 35 32 61 63 32 33 65 34   // 9d5f7f3d52ac23e4
                                                                                                                                                                                    38 61 63 35 31 38 30 31 66 31 64 63 39 35 30 61   // 8ac51801f1dc950a
                                                                                                                                                                                    62 65 39 30 31 64 61 33 34 64 32 61 39 65 33 62   // be901da34d2a9e3b
                                                                                                                                                                                    61 61 64 62 31 34 31 61 31 37 63 37 37 65 66 33   // aadb141a17c77ef3
                                                                                                                                                                                    63 35 36 35 64 64 35 65 65 35 30 35 34 62 39 31   // c565dd5ee5054b91
                                                                                                                                                                                    63 66 36 33 62 62 33 63 36 61 62 38 33 66 37 32   // cf63bb3c6ab83f72
                                                                                                                                                                                    61 62 33 61 61 66 65 39 33 64 30 66 63 33 63 32   // ab3aafe93d0fc3c2
                                                                                                                                                                                    33 34 38 62 37 36 34 66 61 66 62 30 62 31 63 30   // 348b764fafb0b1c0
                                                                                                                                                                                    37 33 33 64 65 35 31 34 35 39 61 65 61 62 34 36   // 733de51459aeab46
                                                                                                                                                                                    35 38 30 33 38 34 62 66 39 64 37 34 63 34 65 32   // 580384bf9d74c4e2
                                                                                                                                                                                    38 31 36 34 62 37 63 64 65 32 34 37 66 38 39 31   // 8164b7cde247f891
                                                                                                                                                                                    62 61 30 37 38 39 31 63 39 64 38 37 32 61 64 32   // ba07891c9d872ad2
                                                                                                                                                                                    62 62 00 00 )                                     // bb..
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C00000B:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 81 66 49 64 65 6E 74 69 74 79 2E 44 65 66   // ...fIdentity.Def
                                                                                                                                                                                    61 75 6C 74 55 49 2E 57 65 62 53 69 74 65 2C 20   // aultUI.WebSite, 
                                                                                                                                                                                    50 75 62 6C 69 63 4B 65 79 3D 30 30 32 34 30 30   // PublicKey=002400
                                                                                                                                                                                    30 30 30 34 38 30 30 30 30 30 39 34 30 30 30 30   // 0004800000940000
                                                                                                                                                                                    30 30 30 36 30 32 30 30 30 30 30 30 32 34 30 30   // 0006020000002400
                                                                                                                                                                                    30 30 35 32 35 33 34 31 33 31 30 30 30 34 30 30   // 0052534131000400
                                                                                                                                                                                    30 30 30 31 30 30 30 31 30 30 66 33 33 61 32 39   // 0001000100f33a29
                                                                                                                                                                                    30 34 34 66 61 39 64 37 34 30 63 39 62 33 32 31   // 044fa9d740c9b321
                                                                                                                                                                                    33 61 39 33 65 35 37 63 38 34 62 34 37 32 63 38   // 3a93e57c84b472c8
                                                                                                                                                                                    34 65 30 62 38 61 30 65 31 61 65 34 38 65 36 37   // 4e0b8a0e1ae48e67
                                                                                                                                                                                    61 39 66 38 66 36 64 65 39 64 35 66 37 66 33 64   // a9f8f6de9d5f7f3d
                                                                                                                                                                                    35 32 61 63 32 33 65 34 38 61 63 35 31 38 30 31   // 52ac23e48ac51801
                                                                                                                                                                                    66 31 64 63 39 35 30 61 62 65 39 30 31 64 61 33   // f1dc950abe901da3
                                                                                                                                                                                    34 64 32 61 39 65 33 62 61 61 64 62 31 34 31 61   // 4d2a9e3baadb141a
                                                                                                                                                                                    31 37 63 37 37 65 66 33 63 35 36 35 64 64 35 65   // 17c77ef3c565dd5e
                                                                                                                                                                                    65 35 30 35 34 62 39 31 63 66 36 33 62 62 33 63   // e5054b91cf63bb3c
                                                                                                                                                                                    36 61 62 38 33 66 37 32 61 62 33 61 61 66 65 39   // 6ab83f72ab3aafe9
                                                                                                                                                                                    33 64 30 66 63 33 63 32 33 34 38 62 37 36 34 66   // 3d0fc3c2348b764f
                                                                                                                                                                                    61 66 62 30 62 31 63 30 37 33 33 64 65 35 31 34   // afb0b1c0733de514
                                                                                                                                                                                    35 39 61 65 61 62 34 36 35 38 30 33 38 34 62 66   // 59aeab46580384bf
                                                                                                                                                                                    39 64 37 34 63 34 65 32 38 31 36 34 62 37 63 64   // 9d74c4e28164b7cd
                                                                                                                                                                                    65 32 34 37 66 38 39 31 62 61 30 37 38 39 31 63   // e247f891ba07891c
                                                                                                                                                                                    39 64 38 37 32 61 64 32 62 62 00 00 )             // 9d872ad2bb..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0A 43 6F 6D 6D 69 74 48 61 73 68 28 31 64   // ...CommitHash(1d
                                                                                                                                                                    63 66 37 61 63 66 61 63 66 30 66 65 31 35 34 61   // cf7acfacf0fe154a
                                                                                                                                                                    64 63 63 32 33 32 37 30 63 62 30 64 61 31 31 66   // dcc23270cb0da11f
                                                                                                                                                                    66 34 34 61 63 65 00 00 )                         // f44ace..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0F 53 6F 75 72 63 65 43 6F 6D 6D 69 74 55   // ...SourceCommitU
                                                                                                                                                                    72 6C 52 68 74 74 70 73 3A 2F 2F 67 69 74 68 75   // rlRhttps://githu
                                                                                                                                                                    62 2E 63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70   // b.com/dotnet/asp
                                                                                                                                                                    6E 65 74 63 6F 72 65 2F 74 72 65 65 2F 31 64 63   // netcore/tree/1dc
                                                                                                                                                                    66 37 61 63 66 61 63 66 30 66 65 31 35 34 61 64   // f7acfacf0fe154ad
                                                                                                                                                                    63 63 32 33 32 37 30 63 62 30 64 61 31 31 66 66   // cc23270cb0da11ff
                                                                                                                                                                    34 34 61 63 65 00 00 )                            // 44ace..
  .custom /*0C00000E:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000F:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000010:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyConfigurationAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 07 52 65 6C 65 61 73 65 00 00 )             // ...Release..
  .custom /*0C000011:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000012:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 4B 41 53 50 2E 4E 45 54 20 43 6F 72 65 20   // ..KASP.NET Core 
                                                                                                                                                                       68 6F 73 74 69 6E 67 20 69 6E 66 72 61 73 74 72   // hosting infrastr
                                                                                                                                                                       75 63 74 75 72 65 20 61 6E 64 20 73 74 61 72 74   // ucture and start
                                                                                                                                                                       75 70 20 6C 6F 67 69 63 20 66 6F 72 20 77 65 62   // up logic for web
                                                                                                                                                                       20 61 70 70 6C 69 63 61 74 69 6F 6E 73 2E 00 00 ) //  applications...
  .custom /*0C000013:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 36 2E 30 2E 32 32 32 2E 36 34 31 32 00   // ...6.0.222.6412.
                                                                                                                                                                       00 ) 
  .custom /*0C000014:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 36 2E 30 2E 32 2B 31 64 63 66 37 61 63   // ...6.0.2+1dcf7ac
                                                                                                                                                                                66 61 63 66 30 66 65 31 35 34 61 64 63 63 32 33   // facf0fe154adcc23
                                                                                                                                                                                32 37 30 63 62 30 64 61 31 31 66 66 34 34 61 63   // 270cb0da11ff44ac
                                                                                                                                                                                65 00 00 )                                        // e..
  .custom /*0C000015:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 16 4D 69 63 72 6F 73 6F 66 74 20 41 53 50   // ...Microsoft ASP
                                                                                                                                                                   2E 4E 45 54 20 43 6F 72 65 00 00 )                // .NET Core..
  .custom /*0C000016:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 1C 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ...Microsoft.Asp
                                                                                                                                                                 4E 65 74 43 6F 72 65 2E 48 6F 73 74 69 6E 67 00   // NetCore.Hosting.
                                                                                                                                                                 00 ) 
  .custom /*0C000017:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    24 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // $https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70 6E 65   // com/dotnet/aspne
                                                                                                                                                                    74 63 6F 72 65 00 00 )                            // tcore..
  .custom /*0C000018:0A000011*/ instance void [System.Runtime/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*01000012*/::.ctor(string) /* 0A000011 */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
  .publickey = (00 24 00 00 04 80 00 00 94 00 00 00 06 02 00 00   // .$..............
                00 24 00 00 52 53 41 31 00 04 00 00 01 00 01 00   // .$..RSA1........
                F3 3A 29 04 4F A9 D7 40 C9 B3 21 3A 93 E5 7C 84   // .:).O..@..!:..|.
                B4 72 C8 4E 0B 8A 0E 1A E4 8E 67 A9 F8 F6 DE 9D   // .r.N......g.....
                5F 7F 3D 52 AC 23 E4 8A C5 18 01 F1 DC 95 0A BE   // _.=R.#..........
                90 1D A3 4D 2A 9E 3B AA DB 14 1A 17 C7 7E F3 C5   // ...M*.;......~..
                65 DD 5E E5 05 4B 91 CF 63 BB 3C 6A B8 3F 72 AB   // e.^..K..c.<j.?r.
                3A AF E9 3D 0F C3 C2 34 8B 76 4F AF B0 B1 C0 73   // :..=...4.vO....s
                3D E5 14 59 AE AB 46 58 03 84 BF 9D 74 C4 E2 81   // =..Y..FX....t...
                64 B7 CD E2 47 F8 91 BA 07 89 1C 9D 87 2A D2 BB ) // d...G........*..
  .hash algorithm 0x00008004
  .ver 6:0:0:0
}
.module Microsoft.AspNetCore.Hosting.dll
// MVID: {bd504f95-2bb5-46f0-ad10-d032d4cb8aeb}
.custom /*0C000003:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 01 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C00001A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C00001B:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C00001E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000020:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000001*/ public initonly uint8[] NullableFlags
  .method /*06000002*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8 A_1) cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NullableAttribute::.ctor

  .method /*06000003*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] A_1) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C000023:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000024:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000025:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000002*/ public initonly uint8 Flag
  .method /*06000004*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8 A_1) cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C000027:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000028:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000029:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000003*/ public initonly bool IncludesInternals
  .method /*06000005*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool A_1) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ private abstract auto ansi sealed beforefieldinit Microsoft.Extensions.Internal.TypeNameHelper
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .class /*02000046*/ sequential ansi sealed nested private beforefieldinit DisplayNameOptions
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001B*/
  {
    .custom /*0C000102:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001A*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*04000017*/ private initonly bool '<FullName>k__BackingField'
    .custom /*0C000074:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000018*/ private initonly bool '<IncludeGenericParameters>k__BackingField'
    .custom /*0C00007B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000019*/ private initonly bool '<IncludeGenericParameterNames>k__BackingField'
    .custom /*0C000080:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400001A*/ private initonly char '<NestedTypeDelimiter>k__BackingField'
    .custom /*0C000085:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .method /*06000171*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(bool fullName,
                                 bool includeGenericParameterNames,
                                 bool includeGenericParameters,
                                 char nestedTypeDelimiter) cil managed
    // SIG: 20 04 01 02 02 02 03
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method DisplayNameOptions::.ctor

    .method /*06000172*/ public hidebysig specialname 
            instance bool  get_FullName() cil managed
    // SIG: 20 00 02
    {
      .custom /*0C000227:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method DisplayNameOptions::get_FullName

    .method /*06000173*/ public hidebysig specialname 
            instance bool  get_IncludeGenericParameters() cil managed
    // SIG: 20 00 02
    {
      .custom /*0C000228:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method DisplayNameOptions::get_IncludeGenericParameters

    .method /*06000174*/ public hidebysig specialname 
            instance bool  get_IncludeGenericParameterNames() cil managed
    // SIG: 20 00 02
    {
      .custom /*0C000229:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method DisplayNameOptions::get_IncludeGenericParameterNames

    .method /*06000175*/ public hidebysig specialname 
            instance char  get_NestedTypeDelimiter() cil managed
    // SIG: 20 00 03
    {
      .custom /*0C00022A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method DisplayNameOptions::get_NestedTypeDelimiter

    .property /*1700006B*/ instance callconv(8) bool
            FullName()
    {
      .get instance bool Microsoft.Extensions.Internal.TypeNameHelper/*02000006*//DisplayNameOptions/*02000046*/::get_FullName() /* 06000172 */
    } // end of property DisplayNameOptions::FullName
    .property /*1700006C*/ instance callconv(8) bool
            IncludeGenericParameters()
    {
      .get instance bool Microsoft.Extensions.Internal.TypeNameHelper/*02000006*//DisplayNameOptions/*02000046*/::get_IncludeGenericParameters() /* 06000173 */
    } // end of property DisplayNameOptions::IncludeGenericParameters
    .property /*1700006D*/ instance callconv(8) bool
            IncludeGenericParameterNames()
    {
      .get instance bool Microsoft.Extensions.Internal.TypeNameHelper/*02000006*//DisplayNameOptions/*02000046*/::get_IncludeGenericParameterNames() /* 06000174 */
    } // end of property DisplayNameOptions::IncludeGenericParameterNames
    .property /*1700006E*/ instance callconv(8) char
            NestedTypeDelimiter()
    {
      .get instance char Microsoft.Extensions.Internal.TypeNameHelper/*02000006*//DisplayNameOptions/*02000046*/::get_NestedTypeDelimiter() /* 06000175 */
    } // end of property DisplayNameOptions::NestedTypeDelimiter
  } // end of class DisplayNameOptions

  .method /*06000006*/ public hidebysig static 
          string  GetTypeDisplayName(object item,
                                     [opt] bool fullName) cil managed
  // SIG: 00 02 0E 1C 02
  {
    .custom /*0C00002C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000001*/ 
    .custom /*0C000001:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000018*/::.ctor(string) /* 0A000014 */ = ( 01 00 04 69 74 65 6D 00 00 )                      // ...item..
    .param [2]/*08000003*/  = bool(true)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeNameHelper::GetTypeDisplayName

  .method /*06000007*/ public hidebysig static 
          string  GetTypeDisplayName(class [System.Runtime/*23000001*/]System.Type/*01000019*/ 'type',
                                     [opt] bool fullName,
                                     [opt] bool includeGenericParameterNames,
                                     [opt] bool includeGenericParameters,
                                     [opt] char nestedTypeDelimiter) cil managed
  // SIG: 00 05 0E 12 65 02 02 02 03
  {
    .custom /*0C00002F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000005*/  = bool(true)
    .param [3]/*08000006*/  = bool(false)
    .param [4]/*08000007*/  = bool(true)
    .param [5]/*08000008*/  = char(0x002B)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeNameHelper::GetTypeDisplayName

} // end of class Microsoft.Extensions.Internal.TypeNameHelper

.class /*02000007*/ private auto ansi beforefieldinit Microsoft.Extensions.StackTrace.Sources.ExceptionDetails
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000031:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000032:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000008*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Exception/*0100001C*/ 'error',
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/> stackFrames) cil managed
  // SIG: 20 02 01 12 71 15 12 75 01 12 34
  {
    .custom /*0C000034:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExceptionDetails::.ctor

  .method /*06000009*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string errorMessage,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/> stackFrames) cil managed
  // SIG: 20 02 01 0E 15 12 75 01 12 34
  {
    .custom /*0C00003B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExceptionDetails::.ctor

  .method /*0600000A*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Exception/*0100001C*/ 
          get_Error() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C00003E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExceptionDetails::get_Error

  .method /*0600000B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/> 
          get_StackFrames() cil managed
  // SIG: 20 00 15 12 75 01 12 34
  {
    .custom /*0C000042:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000043:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExceptionDetails::get_StackFrames

  .method /*0600000C*/ public hidebysig specialname 
          instance string  get_ErrorMessage() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000047:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExceptionDetails::get_ErrorMessage

  .method /*0600000D*/ public hidebysig specialname 
          instance void  set_ErrorMessage(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00004A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExceptionDetails::set_ErrorMessage

  .property /*17000001*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Exception/*0100001C*/
          Error()
  {
    .get instance class [System.Runtime/*23000001*/]System.Exception/*0100001C*/ Microsoft.Extensions.StackTrace.Sources.ExceptionDetails/*02000007*/::get_Error() /* 0600000A */
  } // end of property ExceptionDetails::Error
  .property /*17000002*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/>
          StackFrames()
  {
    .custom /*0C00001D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/> Microsoft.Extensions.StackTrace.Sources.ExceptionDetails/*02000007*/::get_StackFrames() /* 0600000B */
  } // end of property ExceptionDetails::StackFrames
  .property /*17000003*/ instance callconv(8) string
          ErrorMessage()
  {
    .get instance string Microsoft.Extensions.StackTrace.Sources.ExceptionDetails/*02000007*/::get_ErrorMessage() /* 0600000C */
    .set instance void Microsoft.Extensions.StackTrace.Sources.ExceptionDetails/*02000007*/::set_ErrorMessage(string) /* 0600000D */
  } // end of property ExceptionDetails::ErrorMessage
} // end of class Microsoft.Extensions.StackTrace.Sources.ExceptionDetails

.class /*02000008*/ private auto ansi beforefieldinit Microsoft.Extensions.StackTrace.Sources.ExceptionDetailsProvider
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000035:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000047*/ auto ansi sealed nested private beforefieldinit '<GetDetails>d__4'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.Extensions.StackTrace.Sources.ExceptionDetails/*02000007*/>/*1B000002*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000024*/,
                    class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<class Microsoft.Extensions.StackTrace.Sources.ExceptionDetails/*02000007*/>/*1B000001*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000021*/
  {
    .custom /*0C000104:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400001B*/ public class [System.Runtime/*23000001*/]System.Exception/*0100001C*/ '<>3__exception'
    .field /*0400001C*/ public class Microsoft.Extensions.StackTrace.Sources.ExceptionDetailsProvider/*02000008*/ '<>4__this'
    .method /*06000176*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(int32 '<>1__state') cil managed
    // SIG: 20 01 01 08
    {
      .custom /*0C00022B:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetDetails>d__4'::.ctor

    .method /*06000177*/ private hidebysig newslot virtual final 
            instance void  System.IDisposable.Dispose() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C00022C:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.IDisposable/*01000021*/::Dispose /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetDetails>d__4'::System.IDisposable.Dispose

    .method /*06000178*/ private hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/::MoveNext /*01000022::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetDetails>d__4'::MoveNext

    .method /*06000179*/ private hidebysig newslot specialname virtual final 
            instance class Microsoft.Extensions.StackTrace.Sources.ExceptionDetails/*02000007*/ 
            'System.Collections.Generic.IEnumerator<Microsoft.Extensions.StackTrace.Sources.ExceptionDetails>.get_Current'() cil managed
    // SIG: 20 00 12 1C
    {
      .custom /*0C00022D:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override  method instance !0 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<class Microsoft.Extensions.StackTrace.Sources.ExceptionDetails/*02000007*/>/*1B000001*/::get_Current() /* 0A000019 */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetDetails>d__4'::'System.Collections.Generic.IEnumerator<Microsoft.Extensions.StackTrace.Sources.ExceptionDetails>.get_Current'

    .method /*0600017A*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C00022E:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/::Reset /*01000022::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetDetails>d__4'::System.Collections.IEnumerator.Reset

    .method /*0600017B*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .custom /*0C00022F:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/::get_Current /*01000022::0A00001B*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetDetails>d__4'::System.Collections.IEnumerator.get_Current

    .method /*0600017C*/ private hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<class Microsoft.Extensions.StackTrace.Sources.ExceptionDetails/*02000007*/> 
            'System.Collections.Generic.IEnumerable<Microsoft.Extensions.StackTrace.Sources.ExceptionDetails>.GetEnumerator'() cil managed
    // SIG: 20 00 15 12 80 8D 01 12 1C
    {
      .custom /*0C000230:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.Extensions.StackTrace.Sources.ExceptionDetails/*02000007*/>/*1B000002*/::GetEnumerator() /* 0A00001C */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetDetails>d__4'::'System.Collections.Generic.IEnumerable<Microsoft.Extensions.StackTrace.Sources.ExceptionDetails>.GetEnumerator'

    .method /*0600017D*/ private hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/ 
            System.Collections.IEnumerable.GetEnumerator() cil managed
    // SIG: 20 00 12 80 89
    {
      .custom /*0C000231:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000024*/::GetEnumerator /*01000024::0A00001D*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetDetails>d__4'::System.Collections.IEnumerable.GetEnumerator

    .property /*1700006F*/ instance callconv(8) class Microsoft.Extensions.StackTrace.Sources.ExceptionDetails/*02000007*/
            'System.Collections.Generic.IEnumerator<Microsoft.Extensions.StackTrace.Sources.ExceptionDetails>.Current'()
    {
      .get instance class Microsoft.Extensions.StackTrace.Sources.ExceptionDetails/*02000007*/ Microsoft.Extensions.StackTrace.Sources.ExceptionDetailsProvider/*02000008*//'<GetDetails>d__4'/*02000047*/::'System.Collections.Generic.IEnumerator<Microsoft.Extensions.StackTrace.Sources.ExceptionDetails>.get_Current'() /* 06000179 */
    } // end of property '<GetDetails>d__4'::'System.Collections.Generic.IEnumerator<Microsoft.Extensions.StackTrace.Sources.ExceptionDetails>.Current'
    .property /*17000070*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object Microsoft.Extensions.StackTrace.Sources.ExceptionDetailsProvider/*02000008*//'<GetDetails>d__4'/*02000047*/::System.Collections.IEnumerator.get_Current() /* 0600017B */
    } // end of property '<GetDetails>d__4'::System.Collections.IEnumerator.Current
  } // end of class '<GetDetails>d__4'

  .class /*02000048*/ auto ansi sealed nested private beforefieldinit '<ReadLines>d__9'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string>/*1B000004*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000024*/,
                    class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<string>/*1B000003*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000021*/
  {
    .custom /*0C000106:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400001D*/ public class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileInfo/*01000026*/ '<>3__fileInfo'
    .method /*0600017E*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(int32 '<>1__state') cil managed
    // SIG: 20 01 01 08
    {
      .custom /*0C000232:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadLines>d__9'::.ctor

    .method /*0600017F*/ private hidebysig newslot virtual final 
            instance void  System.IDisposable.Dispose() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C000233:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.IDisposable/*01000021*/::Dispose /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadLines>d__9'::System.IDisposable.Dispose

    .method /*06000180*/ private hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/::MoveNext /*01000022::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadLines>d__9'::MoveNext

    .method /*06000181*/ private hidebysig newslot specialname virtual final 
            instance string  'System.Collections.Generic.IEnumerator<System.String>.get_Current'() cil managed
    // SIG: 20 00 0E
    {
      .custom /*0C000236:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override  method instance !0 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<string>/*1B000003*/::get_Current() /* 0A00001F */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadLines>d__9'::'System.Collections.Generic.IEnumerator<System.String>.get_Current'

    .method /*06000182*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C000238:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/::Reset /*01000022::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadLines>d__9'::System.Collections.IEnumerator.Reset

    .method /*06000183*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .custom /*0C00023A:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/::get_Current /*01000022::0A00001B*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadLines>d__9'::System.Collections.IEnumerator.get_Current

    .method /*06000184*/ private hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<string> 
            'System.Collections.Generic.IEnumerable<System.String>.GetEnumerator'() cil managed
    // SIG: 20 00 15 12 80 8D 01 0E
    {
      .custom /*0C00023B:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string>/*1B000004*/::GetEnumerator() /* 0A000020 */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadLines>d__9'::'System.Collections.Generic.IEnumerable<System.String>.GetEnumerator'

    .method /*06000185*/ private hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/ 
            System.Collections.IEnumerable.GetEnumerator() cil managed
    // SIG: 20 00 12 80 89
    {
      .custom /*0C00023C:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000024*/::GetEnumerator /*01000024::0A00001D*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadLines>d__9'::System.Collections.IEnumerable.GetEnumerator

    .property /*17000071*/ instance callconv(8) string
            'System.Collections.Generic.IEnumerator<System.String>.Current'()
    {
      .get instance string Microsoft.Extensions.StackTrace.Sources.ExceptionDetailsProvider/*02000008*//'<ReadLines>d__9'/*02000048*/::'System.Collections.Generic.IEnumerator<System.String>.get_Current'() /* 06000181 */
    } // end of property '<ReadLines>d__9'::'System.Collections.Generic.IEnumerator<System.String>.Current'
    .property /*17000072*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object Microsoft.Extensions.StackTrace.Sources.ExceptionDetailsProvider/*02000008*//'<ReadLines>d__9'/*02000048*/::System.Collections.IEnumerator.get_Current() /* 06000183 */
    } // end of property '<ReadLines>d__9'::System.Collections.IEnumerator.Current
  } // end of class '<ReadLines>d__9'

  .method /*0600000E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/ fileProvider,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ logger,
                               int32 sourceCodeLineCount) cil managed
  // SIG: 20 03 01 12 79 12 7D 08
  {
    .param [2]/*0800000F*/ 
    .custom /*0C000051:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExceptionDetailsProvider::.ctor

  .method /*0600000F*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.Extensions.StackTrace.Sources.ExceptionDetails/*02000007*/> 
          GetDetails(class [System.Runtime/*23000001*/]System.Exception/*0100001C*/ exception) cil managed
  // SIG: 20 01 15 12 75 01 12 1C 12 71
  {
    .custom /*0C00004E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IteratorStateMachineAttribute/*01000020*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000019*/) /* 0A000016 */ = ( 01 00 51 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..QMicrosoft.Ext
                                                                                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 53 74 61 63 6B 54 72 61   // ensions.StackTra
                                                                                                                                                                                                                                           63 65 2E 53 6F 75 72 63 65 73 2E 45 78 63 65 70   // ce.Sources.Excep
                                                                                                                                                                                                                                           74 69 6F 6E 44 65 74 61 69 6C 73 50 72 6F 76 69   // tionDetailsProvi
                                                                                                                                                                                                                                           64 65 72 2B 3C 47 65 74 44 65 74 61 69 6C 73 3E   // der+<GetDetails>
                                                                                                                                                                                                                                           64 5F 5F 34 00 00 )                               // d__4..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExceptionDetailsProvider::GetDetails

  .method /*06000010*/ assembly hidebysig 
          instance class Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/ 
          GetStackFrameSourceCodeInfo(string 'method',
                                      string filePath,
                                      int32 lineNumber) cil managed
  // SIG: 20 03 12 34 0E 0E 08
  {
    .custom /*0C000054:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000012*/ 
    .custom /*0C000066:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExceptionDetailsProvider::GetStackFrameSourceCodeInfo

  .method /*06000011*/ assembly hidebysig 
          instance void  ReadFrameContent(class Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/ frame,
                                          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string> allLines,
                                          int32 errorStartLineNumberInFile,
                                          int32 errorEndLineNumberInFile) cil managed
  // SIG: 20 04 01 12 34 15 12 75 01 0E 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExceptionDetailsProvider::ReadFrameContent

} // end of class Microsoft.Extensions.StackTrace.Sources.ExceptionDetailsProvider

.class /*02000009*/ private abstract auto ansi sealed beforefieldinit Microsoft.Extensions.StackTrace.Sources.LoggerExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00003C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000012*/ public hidebysig static 
          void  FailedToReadStackTraceInfo(class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ logger,
                                           class [System.Runtime/*23000001*/]System.Exception/*0100001C*/ exception) cil managed
  // SIG: 00 02 01 12 7D 12 71
  {
    .custom /*0C00005F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000060:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000061:0A000021*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*01000028*/::.ctor(int32,
                                                                                                                                                                              valuetype [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LogLevel/*01000027*/,
                                                                                                                                                                              string) /* 0A000021 */ = ( 01 00 00 00 00 00 01 00 00 00 35 46 61 69 6C 65   // ..........5Faile
                                                                                                                                                                                                         64 20 74 6F 20 72 65 61 64 20 73 74 61 63 6B 20   // d to read stack 
                                                                                                                                                                                                         74 72 61 63 65 20 69 6E 66 6F 72 6D 61 74 69 6F   // trace informatio
                                                                                                                                                                                                         6E 20 66 6F 72 20 65 78 63 65 70 74 69 6F 6E 2E   // n for exception.
                                                                                                                                                                                                         01 00 54 0E 09 45 76 65 6E 74 4E 61 6D 65 1A 46   // ..T..EventName.F
                                                                                                                                                                                                         61 69 6C 65 64 54 6F 52 65 61 64 53 74 61 63 6B   // ailedToReadStack
                                                                                                                                                                                                         54 72 61 63 65 49 6E 66 6F )                      // TraceInfo
    .custom /*0C000062:0A000022*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*01000029*/::.ctor(string,
                                                                                                                                              string) /* 0A000022 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                         65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                         47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                         2E 36 34 30 36 00 00 )                            // .6406..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggerExtensions::FailedToReadStackTraceInfo

} // end of class Microsoft.Extensions.StackTrace.Sources.LoggerExtensions

.class /*0200000A*/ private auto ansi beforefieldinit Microsoft.Extensions.StackTrace.Sources.MethodDisplayInfo
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00003F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000040:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000049*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000108:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400001E*/ public static initonly class Microsoft.Extensions.StackTrace.Sources.MethodDisplayInfo/*0200000A*//'<>c'/*02000049*/ '<>9'
    .field /*0400001F*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class Microsoft.Extensions.StackTrace.Sources.ParameterDisplayInfo/*0200000B*/,string> '<>9__16_0'
    .method /*06000186*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000187*/ assembly hidebysig 
            instance string  '<ToString>b__16_0'(class Microsoft.Extensions.StackTrace.Sources.ParameterDisplayInfo/*0200000B*/ p) cil managed
    // SIG: 20 01 0E 12 2C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<ToString>b__16_0'

  } // end of class '<>c'

  .method /*06000013*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string declaringTypeName,
                               string name,
                               string genericArguments,
                               string subMethod,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.Extensions.StackTrace.Sources.ParameterDisplayInfo/*0200000B*/> parameters) cil managed
  // SIG: 20 05 01 0E 0E 0E 0E 15 12 75 01 12 2C
  {
    .param [2]/*0800001D*/ 
    .custom /*0C000091:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [5]/*08000020*/ 
    .custom /*0C00009D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodDisplayInfo::.ctor

  .method /*06000014*/ public hidebysig specialname 
          instance string  get_DeclaringTypeName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000069:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodDisplayInfo::get_DeclaringTypeName

  .method /*06000015*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00006D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00006E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodDisplayInfo::get_Name

  .method /*06000016*/ public hidebysig specialname 
          instance string  get_GenericArguments() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000072:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodDisplayInfo::get_GenericArguments

  .method /*06000017*/ public hidebysig specialname 
          instance string  get_SubMethod() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000073:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodDisplayInfo::get_SubMethod

  .method /*06000018*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.Extensions.StackTrace.Sources.ParameterDisplayInfo/*0200000B*/> 
          get_Parameters() cil managed
  // SIG: 20 00 15 12 75 01 12 2C
  {
    .custom /*0C000079:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00007A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodDisplayInfo::get_Parameters

  .method /*06000019*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00007F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodDisplayInfo::ToString

  .property /*17000004*/ instance callconv(8) string
          DeclaringTypeName()
  {
    .get instance string Microsoft.Extensions.StackTrace.Sources.MethodDisplayInfo/*0200000A*/::get_DeclaringTypeName() /* 06000014 */
  } // end of property MethodDisplayInfo::DeclaringTypeName
  .property /*17000005*/ instance callconv(8) string
          Name()
  {
    .custom /*0C00002B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string Microsoft.Extensions.StackTrace.Sources.MethodDisplayInfo/*0200000A*/::get_Name() /* 06000015 */
  } // end of property MethodDisplayInfo::Name
  .property /*17000006*/ instance callconv(8) string
          GenericArguments()
  {
    .get instance string Microsoft.Extensions.StackTrace.Sources.MethodDisplayInfo/*0200000A*/::get_GenericArguments() /* 06000016 */
  } // end of property MethodDisplayInfo::GenericArguments
  .property /*17000007*/ instance callconv(8) string
          SubMethod()
  {
    .get instance string Microsoft.Extensions.StackTrace.Sources.MethodDisplayInfo/*0200000A*/::get_SubMethod() /* 06000017 */
  } // end of property MethodDisplayInfo::SubMethod
  .property /*17000008*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.Extensions.StackTrace.Sources.ParameterDisplayInfo/*0200000B*/>
          Parameters()
  {
    .custom /*0C000038:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.Extensions.StackTrace.Sources.ParameterDisplayInfo/*0200000B*/> Microsoft.Extensions.StackTrace.Sources.MethodDisplayInfo/*0200000A*/::get_Parameters() /* 06000018 */
  } // end of property MethodDisplayInfo::Parameters
} // end of class Microsoft.Extensions.StackTrace.Sources.MethodDisplayInfo

.class /*0200000B*/ private auto ansi beforefieldinit Microsoft.Extensions.StackTrace.Sources.ParameterDisplayInfo
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000044:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000045:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600001A*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000084:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterDisplayInfo::get_Name

  .method /*0600001B*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000089:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterDisplayInfo::set_Name

  .method /*0600001C*/ public hidebysig specialname 
          instance string  get_Type() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00008C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterDisplayInfo::get_Type

  .method /*0600001D*/ public hidebysig specialname 
          instance void  set_Type(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000090:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterDisplayInfo::set_Type

  .method /*0600001E*/ public hidebysig specialname 
          instance string  get_Prefix() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000093:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterDisplayInfo::get_Prefix

  .method /*0600001F*/ public hidebysig specialname 
          instance void  set_Prefix(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000097:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterDisplayInfo::set_Prefix

  .method /*06000020*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00009A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterDisplayInfo::ToString

  .method /*06000021*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterDisplayInfo::.ctor

  .property /*17000009*/ instance callconv(8) string
          Name()
  {
    .get instance string Microsoft.Extensions.StackTrace.Sources.ParameterDisplayInfo/*0200000B*/::get_Name() /* 0600001A */
    .set instance void Microsoft.Extensions.StackTrace.Sources.ParameterDisplayInfo/*0200000B*/::set_Name(string) /* 0600001B */
  } // end of property ParameterDisplayInfo::Name
  .property /*1700000A*/ instance callconv(8) string
          Type()
  {
    .get instance string Microsoft.Extensions.StackTrace.Sources.ParameterDisplayInfo/*0200000B*/::get_Type() /* 0600001C */
    .set instance void Microsoft.Extensions.StackTrace.Sources.ParameterDisplayInfo/*0200000B*/::set_Type(string) /* 0600001D */
  } // end of property ParameterDisplayInfo::Type
  .property /*1700000B*/ instance callconv(8) string
          Prefix()
  {
    .get instance string Microsoft.Extensions.StackTrace.Sources.ParameterDisplayInfo/*0200000B*/::get_Prefix() /* 0600001E */
    .set instance void Microsoft.Extensions.StackTrace.Sources.ParameterDisplayInfo/*0200000B*/::set_Prefix(string) /* 0600001F */
  } // end of property ParameterDisplayInfo::Prefix
} // end of class Microsoft.Extensions.StackTrace.Sources.ParameterDisplayInfo

.class /*0200000C*/ private auto ansi beforefieldinit Microsoft.Extensions.StackTrace.Sources.StackFrameInfo
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000048:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000049:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000022*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 lineNumber,
                               string filePath,
                               class [System.Diagnostics.StackTrace/*23000004*/]System.Diagnostics.StackFrame/*0100002B*/ stackFrame,
                               class Microsoft.Extensions.StackTrace.Sources.MethodDisplayInfo/*0200000A*/ methodDisplayInfo) cil managed
  // SIG: 20 04 01 08 0E 12 80 AD 12 28
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameInfo::.ctor

  .method /*06000023*/ public hidebysig specialname 
          instance int32  get_LineNumber() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0000A3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameInfo::get_LineNumber

  .method /*06000024*/ public hidebysig specialname 
          instance string  get_FilePath() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000A6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameInfo::get_FilePath

  .method /*06000025*/ public hidebysig specialname 
          instance class [System.Diagnostics.StackTrace/*23000004*/]System.Diagnostics.StackFrame/*0100002B*/ 
          get_StackFrame() cil managed
  // SIG: 20 00 12 80 AD
  {
    .custom /*0C0000A9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameInfo::get_StackFrame

  .method /*06000026*/ public hidebysig specialname 
          instance class Microsoft.Extensions.StackTrace.Sources.MethodDisplayInfo/*0200000A*/ 
          get_MethodDisplayInfo() cil managed
  // SIG: 20 00 12 28
  {
    .custom /*0C0000AD:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameInfo::get_MethodDisplayInfo

  .property /*1700000C*/ instance callconv(8) int32
          LineNumber()
  {
    .get instance int32 Microsoft.Extensions.StackTrace.Sources.StackFrameInfo/*0200000C*/::get_LineNumber() /* 06000023 */
  } // end of property StackFrameInfo::LineNumber
  .property /*1700000D*/ instance callconv(8) string
          FilePath()
  {
    .get instance string Microsoft.Extensions.StackTrace.Sources.StackFrameInfo/*0200000C*/::get_FilePath() /* 06000024 */
  } // end of property StackFrameInfo::FilePath
  .property /*1700000E*/ instance callconv(8) class [System.Diagnostics.StackTrace/*23000004*/]System.Diagnostics.StackFrame/*0100002B*/
          StackFrame()
  {
    .get instance class [System.Diagnostics.StackTrace/*23000004*/]System.Diagnostics.StackFrame/*0100002B*/ Microsoft.Extensions.StackTrace.Sources.StackFrameInfo/*0200000C*/::get_StackFrame() /* 06000025 */
  } // end of property StackFrameInfo::StackFrame
  .property /*1700000F*/ instance callconv(8) class Microsoft.Extensions.StackTrace.Sources.MethodDisplayInfo/*0200000A*/
          MethodDisplayInfo()
  {
    .get instance class Microsoft.Extensions.StackTrace.Sources.MethodDisplayInfo/*0200000A*/ Microsoft.Extensions.StackTrace.Sources.StackFrameInfo/*0200000C*/::get_MethodDisplayInfo() /* 06000026 */
  } // end of property StackFrameInfo::MethodDisplayInfo
} // end of class Microsoft.Extensions.StackTrace.Sources.StackFrameInfo

.class /*0200000D*/ private auto ansi beforefieldinit Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00004B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000027*/ public hidebysig specialname 
          instance string  get_Function() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000B1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000B2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameSourceCodeInfo::get_Function

  .method /*06000028*/ public hidebysig specialname 
          instance void  set_Function(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000B5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000B6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameSourceCodeInfo::set_Function

  .method /*06000029*/ public hidebysig specialname 
          instance string  get_File() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000BA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000BB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameSourceCodeInfo::get_File

  .method /*0600002A*/ public hidebysig specialname 
          instance void  set_File(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000BD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000BE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameSourceCodeInfo::set_File

  .method /*0600002B*/ public hidebysig specialname 
          instance int32  get_Line() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0000C2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameSourceCodeInfo::get_Line

  .method /*0600002C*/ public hidebysig specialname 
          instance void  set_Line(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C0000C3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameSourceCodeInfo::set_Line

  .method /*0600002D*/ public hidebysig specialname 
          instance int32  get_PreContextLine() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0000C5:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameSourceCodeInfo::get_PreContextLine

  .method /*0600002E*/ public hidebysig specialname 
          instance void  set_PreContextLine(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C0000C7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameSourceCodeInfo::set_PreContextLine

  .method /*0600002F*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string> 
          get_PreContextCode() cil managed
  // SIG: 20 00 15 12 75 01 0E
  {
    .custom /*0C0000CA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameSourceCodeInfo::get_PreContextCode

  .method /*06000030*/ public hidebysig specialname 
          instance void  set_PreContextCode(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string> 'value') cil managed
  // SIG: 20 01 01 15 12 75 01 0E
  {
    .custom /*0C0000CD:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameSourceCodeInfo::set_PreContextCode

  .method /*06000031*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string> 
          get_ContextCode() cil managed
  // SIG: 20 00 15 12 75 01 0E
  {
    .custom /*0C0000D0:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameSourceCodeInfo::get_ContextCode

  .method /*06000032*/ public hidebysig specialname 
          instance void  set_ContextCode(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string> 'value') cil managed
  // SIG: 20 01 01 15 12 75 01 0E
  {
    .custom /*0C0000D2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameSourceCodeInfo::set_ContextCode

  .method /*06000033*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string> 
          get_PostContextCode() cil managed
  // SIG: 20 00 15 12 75 01 0E
  {
    .custom /*0C0000D5:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameSourceCodeInfo::get_PostContextCode

  .method /*06000034*/ public hidebysig specialname 
          instance void  set_PostContextCode(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string> 'value') cil managed
  // SIG: 20 01 01 15 12 75 01 0E
  {
    .custom /*0C0000D6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameSourceCodeInfo::set_PostContextCode

  .method /*06000035*/ public hidebysig specialname 
          instance string  get_ErrorDetails() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000D9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000DA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameSourceCodeInfo::get_ErrorDetails

  .method /*06000036*/ public hidebysig specialname 
          instance void  set_ErrorDetails(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000DE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000DF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameSourceCodeInfo::set_ErrorDetails

  .method /*06000037*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameSourceCodeInfo::.ctor

  .property /*17000010*/ instance callconv(8) string
          Function()
  {
    .custom /*0C000058:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/::get_Function() /* 06000027 */
    .set instance void Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/::set_Function(string) /* 06000028 */
  } // end of property StackFrameSourceCodeInfo::Function
  .property /*17000011*/ instance callconv(8) string
          File()
  {
    .custom /*0C00005E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/::get_File() /* 06000029 */
    .set instance void Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/::set_File(string) /* 0600002A */
  } // end of property StackFrameSourceCodeInfo::File
  .property /*17000012*/ instance callconv(8) int32
          Line()
  {
    .get instance int32 Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/::get_Line() /* 0600002B */
    .set instance void Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/::set_Line(int32) /* 0600002C */
  } // end of property StackFrameSourceCodeInfo::Line
  .property /*17000013*/ instance callconv(8) int32
          PreContextLine()
  {
    .get instance int32 Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/::get_PreContextLine() /* 0600002D */
    .set instance void Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/::set_PreContextLine(int32) /* 0600002E */
  } // end of property StackFrameSourceCodeInfo::PreContextLine
  .property /*17000014*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string>
          PreContextCode()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string> Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/::get_PreContextCode() /* 0600002F */
    .set instance void Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/::set_PreContextCode(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string>) /* 06000030 */
  } // end of property StackFrameSourceCodeInfo::PreContextCode
  .property /*17000015*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string>
          ContextCode()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string> Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/::get_ContextCode() /* 06000031 */
    .set instance void Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/::set_ContextCode(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string>) /* 06000032 */
  } // end of property StackFrameSourceCodeInfo::ContextCode
  .property /*17000016*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string>
          PostContextCode()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string> Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/::get_PostContextCode() /* 06000033 */
    .set instance void Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/::set_PostContextCode(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string>) /* 06000034 */
  } // end of property StackFrameSourceCodeInfo::PostContextCode
  .property /*17000017*/ instance callconv(8) string
          ErrorDetails()
  {
    .custom /*0C000078:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/::get_ErrorDetails() /* 06000035 */
    .set instance void Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo/*0200000D*/::set_ErrorDetails(string) /* 06000036 */
  } // end of property StackFrameSourceCodeInfo::ErrorDetails
} // end of class Microsoft.Extensions.StackTrace.Sources.StackFrameSourceCodeInfo

.class /*0200000E*/ private auto ansi beforefieldinit Microsoft.Extensions.StackTrace.Sources.StackTraceHelper
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .class /*0200004A*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00010B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000020*/ public static initonly class Microsoft.Extensions.StackTrace.Sources.StackTraceHelper/*0200000E*//'<>c'/*0200004A*/ '<>9'
    .field /*04000021*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.Runtime/*23000001*/]System.Type/*01000019*/,string> '<>9__1_0'
    .field /*04000022*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*0100002F*/,class Microsoft.Extensions.StackTrace.Sources.ParameterDisplayInfo/*0200000B*/> '<>9__1_1'
    .method /*06000188*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000189*/ assembly hidebysig 
            instance string  '<GetMethodDisplayString>b__1_0'(class [System.Runtime/*23000001*/]System.Type/*01000019*/ arg) cil managed
    // SIG: 20 01 0E 12 65
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<GetMethodDisplayString>b__1_0'

    .method /*0600018A*/ assembly hidebysig 
            instance class Microsoft.Extensions.StackTrace.Sources.ParameterDisplayInfo/*0200000B*/ 
            '<GetMethodDisplayString>b__1_1'(class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*0100002F*/ parameter) cil managed
    // SIG: 20 01 12 2C 12 80 BD
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<GetMethodDisplayString>b__1_1'

  } // end of class '<>c'

  .method /*06000038*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100002C*/<class Microsoft.Extensions.StackTrace.Sources.StackFrameInfo/*0200000C*/> 
          GetFrames(class [System.Runtime/*23000001*/]System.Exception/*0100001C*/ exception,
                    [out] class [System.Runtime/*23000001*/]System.AggregateException/*0100002D*/& 'error') cil managed
  // SIG: 00 02 15 12 80 B1 01 12 30 12 71 10 12 80 B5
  {
    .custom /*0C0000E5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000031*/ 
    .custom /*0C0000D1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackTraceHelper::GetFrames

  .method /*06000039*/ assembly hidebysig static 
          class Microsoft.Extensions.StackTrace.Sources.MethodDisplayInfo/*0200000A*/ 
          GetMethodDisplayString(class [System.Runtime/*23000001*/]System.Reflection.MethodBase/*0100002E*/ 'method') cil managed
  // SIG: 00 01 12 28 12 80 B9
  {
    .custom /*0C0000E8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackTraceHelper::GetMethodDisplayString

  .method /*0600003A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackTraceHelper::.ctor

} // end of class Microsoft.Extensions.StackTrace.Sources.StackTraceHelper

.class /*0200000F*/ private auto ansi beforefieldinit Microsoft.Extensions.RazorViews.AttributeValue
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00004F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000050:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600003B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string prefix,
                               object 'value',
                               bool 'literal') cil managed
  // SIG: 20 03 01 0E 1C 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AttributeValue::.ctor

  .method /*0600003C*/ public hidebysig specialname 
          instance string  get_Prefix() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000EF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AttributeValue::get_Prefix

  .method /*0600003D*/ public hidebysig specialname 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C0000F2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AttributeValue::get_Value

  .method /*0600003E*/ public hidebysig specialname 
          instance bool  get_Literal() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000F6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AttributeValue::get_Literal

  .method /*0600003F*/ public hidebysig static 
          class Microsoft.Extensions.RazorViews.AttributeValue/*0200000F*/ 
          FromTuple(class [System.Runtime/*23000001*/]System.Tuple`3/*01000030*/<string,object,bool> 'value') cil managed
  // SIG: 00 01 12 3C 15 12 80 C1 03 0E 1C 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AttributeValue::FromTuple

  .method /*06000040*/ public hidebysig static 
          class Microsoft.Extensions.RazorViews.AttributeValue/*0200000F*/ 
          FromTuple(class [System.Runtime/*23000001*/]System.Tuple`3/*01000030*/<string,string,bool> 'value') cil managed
  // SIG: 00 01 12 3C 15 12 80 C1 03 0E 0E 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AttributeValue::FromTuple

  .method /*06000041*/ public hidebysig specialname static 
          class Microsoft.Extensions.RazorViews.AttributeValue/*0200000F*/ 
          op_Implicit(class [System.Runtime/*23000001*/]System.Tuple`3/*01000030*/<string,object,bool> 'value') cil managed
  // SIG: 00 01 12 3C 15 12 80 C1 03 0E 1C 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AttributeValue::op_Implicit

  .property /*17000018*/ instance callconv(8) string
          Prefix()
  {
    .get instance string Microsoft.Extensions.RazorViews.AttributeValue/*0200000F*/::get_Prefix() /* 0600003C */
  } // end of property AttributeValue::Prefix
  .property /*17000019*/ instance callconv(8) object
          Value()
  {
    .get instance object Microsoft.Extensions.RazorViews.AttributeValue/*0200000F*/::get_Value() /* 0600003D */
  } // end of property AttributeValue::Value
  .property /*1700001A*/ instance callconv(8) bool
          Literal()
  {
    .get instance bool Microsoft.Extensions.RazorViews.AttributeValue/*0200000F*/::get_Literal() /* 0600003E */
  } // end of property AttributeValue::Literal
} // end of class Microsoft.Extensions.RazorViews.AttributeValue

.class /*02000010*/ private abstract auto ansi beforefieldinit Microsoft.Extensions.RazorViews.BaseView
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000055:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000056:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200004B*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00010D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000023*/ public static initonly class Microsoft.Extensions.RazorViews.BaseView/*02000010*//'<>c'/*0200004B*/ '<>9'
    .field /*04000024*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<string,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string>> '<>9__53_0'
    .method /*0600018B*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*0600018C*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string> 
            '<HtmlEncodeAndReplaceLineBreaks>b__53_0'(string s) cil managed
    // SIG: 20 01 15 12 75 01 0E 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<HtmlEncodeAndReplaceLineBreaks>b__53_0'

  } // end of class '<>c'

  .class /*0200004C*/ auto ansi sealed nested private beforefieldinit '<ExecuteAsync>d__31'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001B*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C000110:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000025*/ public int32 '<>1__state'
    .field /*04000026*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100003D*/ '<>t__builder'
    .field /*04000027*/ public class Microsoft.Extensions.RazorViews.BaseView/*02000010*/ '<>4__this'
    .field /*04000028*/ public class [System.Runtime/*23000001*/]System.IO.Stream/*0100003A*/ 'stream'
    .field /*04000029*/ private class [System.Runtime/*23000001*/]System.IO.MemoryStream/*0100003E*/ '<buffer>5__2'
    .field /*0400002A*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100003F*/ '<>u__1'
    .field /*0400002B*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000040*/ '<>u__2'
    .method /*0600018D*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000025*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__31'::MoveNext

    .method /*0600018E*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C00023D:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000026*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__31'::SetStateMachine

  } // end of class '<ExecuteAsync>d__31'

  .class /*0200004D*/ auto ansi sealed nested private beforefieldinit '<ExecuteAsync>d__32'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001B*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C000113:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400002C*/ public int32 '<>1__state'
    .field /*0400002D*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100003D*/ '<>t__builder'
    .field /*0400002E*/ public class Microsoft.Extensions.RazorViews.BaseView/*02000010*/ '<>4__this'
    .field /*0400002F*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000031*/ context
    .field /*04000030*/ private class [System.Runtime/*23000001*/]System.IO.MemoryStream/*0100003E*/ '<buffer>5__2'
    .field /*04000031*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100003F*/ '<>u__1'
    .field /*04000032*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000040*/ '<>u__2'
    .method /*0600018F*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000025*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__32'::MoveNext

    .method /*06000190*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C00023E:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000026*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__32'::SetStateMachine

  } // end of class '<ExecuteAsync>d__32'

  .method /*06000042*/ family hidebysig specialname 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000031*/ 
          get_Context() cil managed
  // SIG: 20 00 12 80 C5
  {
    .custom /*0C0000FA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::get_Context

  .method /*06000043*/ family hidebysig specialname 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpRequest/*01000032*/ 
          get_Request() cil managed
  // SIG: 20 00 12 80 C9
  {
    .custom /*0C0000FC:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::get_Request

  .method /*06000044*/ family hidebysig specialname 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpResponse/*01000033*/ 
          get_Response() cil managed
  // SIG: 20 00 12 80 CD
  {
    .custom /*0C0000FE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::get_Response

  .method /*06000045*/ family hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.TextWriter/*01000034*/ 
          get_Output() cil managed
  // SIG: 20 00 12 80 D1
  {
    .custom /*0C0000FF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::get_Output

  .method /*06000046*/ family hidebysig specialname 
          instance class [System.Text.Encodings.Web/*23000006*/]System.Text.Encodings.Web.HtmlEncoder/*01000035*/ 
          get_HtmlEncoder() cil managed
  // SIG: 20 00 12 80 D5
  {
    .custom /*0C000101:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::get_HtmlEncoder

  .method /*06000047*/ family hidebysig specialname 
          instance void  set_HtmlEncoder(class [System.Text.Encodings.Web/*23000006*/]System.Text.Encodings.Web.HtmlEncoder/*01000035*/ 'value') cil managed
  // SIG: 20 01 01 12 80 D5
  {
    .custom /*0C000103:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::set_HtmlEncoder

  .method /*06000048*/ family hidebysig specialname 
          instance class [System.Text.Encodings.Web/*23000006*/]System.Text.Encodings.Web.UrlEncoder/*01000036*/ 
          get_UrlEncoder() cil managed
  // SIG: 20 00 12 80 D9
  {
    .custom /*0C000105:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::get_UrlEncoder

  .method /*06000049*/ family hidebysig specialname 
          instance void  set_UrlEncoder(class [System.Text.Encodings.Web/*23000006*/]System.Text.Encodings.Web.UrlEncoder/*01000036*/ 'value') cil managed
  // SIG: 20 01 01 12 80 D9
  {
    .custom /*0C000107:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::set_UrlEncoder

  .method /*0600004A*/ family hidebysig specialname 
          instance class [System.Text.Encodings.Web/*23000006*/]System.Text.Encodings.Web.JavaScriptEncoder/*01000037*/ 
          get_JavaScriptEncoder() cil managed
  // SIG: 20 00 12 80 DD
  {
    .custom /*0C00010A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::get_JavaScriptEncoder

  .method /*0600004B*/ family hidebysig specialname 
          instance void  set_JavaScriptEncoder(class [System.Text.Encodings.Web/*23000006*/]System.Text.Encodings.Web.JavaScriptEncoder/*01000037*/ 'value') cil managed
  // SIG: 20 01 01 12 80 DD
  {
    .custom /*0C00010C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::set_JavaScriptEncoder

  .method /*0600004C*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000039*/ 
          ExecuteAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*0100003A*/ 'stream') cil managed
  // SIG: 20 01 12 80 E5 12 80 E9
  {
    .custom /*0C00010F:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000038*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000019*/) /* 0A000023 */ = ( 01 00 3C 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..<Microsoft.Ext
                                                                                                                                                                                                                                        65 6E 73 69 6F 6E 73 2E 52 61 7A 6F 72 56 69 65   // ensions.RazorVie
                                                                                                                                                                                                                                        77 73 2E 42 61 73 65 56 69 65 77 2B 3C 45 78 65   // ws.BaseView+<Exe
                                                                                                                                                                                                                                        63 75 74 65 41 73 79 6E 63 3E 64 5F 5F 33 31 00   // cuteAsync>d__31.
                                                                                                                                                                                                                                        00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::ExecuteAsync

  .method /*0600004D*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000039*/ 
          ExecuteAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000031*/ context) cil managed
  // SIG: 20 01 12 80 E5 12 80 C5
  {
    .custom /*0C000112:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000038*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000019*/) /* 0A000023 */ = ( 01 00 3C 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..<Microsoft.Ext
                                                                                                                                                                                                                                        65 6E 73 69 6F 6E 73 2E 52 61 7A 6F 72 56 69 65   // ensions.RazorVie
                                                                                                                                                                                                                                        77 73 2E 42 61 73 65 56 69 65 77 2B 3C 45 78 65   // ws.BaseView+<Exe
                                                                                                                                                                                                                                        63 75 74 65 41 73 79 6E 63 3E 64 5F 5F 33 32 00   // cuteAsync>d__32.
                                                                                                                                                                                                                                        00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::ExecuteAsync

  .method /*0600004E*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000039*/ 
          ExecuteAsync() cil managed
  // SIG: 20 00 12 80 E5
  {
    // Method begins at RVA 0x0
  } // end of method BaseView::ExecuteAsync

  .method /*0600004F*/ family hidebysig newslot virtual 
          instance void  PushWriter(class [System.Runtime/*23000001*/]System.IO.TextWriter/*01000034*/ writer) cil managed
  // SIG: 20 01 01 12 80 D1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::PushWriter

  .method /*06000050*/ family hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IO.TextWriter/*01000034*/ 
          PopWriter() cil managed
  // SIG: 20 00 12 80 D1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::PopWriter

  .method /*06000051*/ family hidebysig instance void 
          WriteLiteral(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::WriteLiteral

  .method /*06000052*/ family hidebysig instance void 
          WriteLiteral(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000116:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::WriteLiteral

  .method /*06000053*/ family hidebysig instance void 
          WriteAttributeValue(string thingy,
                              int32 startPostion,
                              object 'value',
                              int32 endValue,
                              int32 dealyo,
                              bool yesno) cil managed
  // SIG: 20 06 01 0E 08 1C 08 08 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::WriteAttributeValue

  .method /*06000054*/ family hidebysig instance void 
          BeginWriteAttribute(string name,
                              string beginning,
                              int32 startPosition,
                              string ending,
                              int32 endPosition,
                              int32 thingy) cil managed
  // SIG: 20 06 01 0E 0E 08 0E 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::BeginWriteAttribute

  .method /*06000055*/ family hidebysig instance void 
          EndWriteAttribute() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::EndWriteAttribute

  .method /*06000056*/ family hidebysig instance void 
          WriteAttribute(string name,
                         string leader,
                         string trailer,
                         class Microsoft.Extensions.RazorViews.AttributeValue/*0200000F*/[] values) cil managed
  // SIG: 20 04 01 0E 0E 0E 1D 12 3C
  {
    .param [4]/*08000050*/ 
    .custom /*0C000115:0A000024*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100003B*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::WriteAttribute

  .method /*06000057*/ family hidebysig instance void 
          Write(class Microsoft.Extensions.RazorViews.HelperResult/*02000011*/ result) cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::Write

  .method /*06000058*/ family hidebysig instance void 
          Write(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::Write

  .method /*06000059*/ family hidebysig instance void 
          Write(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00011D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::Write

  .method /*0600005A*/ family hidebysig instance string 
          HtmlEncodeAndReplaceLineBreaks(string input) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::HtmlEncodeAndReplaceLineBreaks

  .method /*0600005B*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseView::.ctor

  .property /*1700001B*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000031*/
          Context()
  {
    .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000031*/ Microsoft.Extensions.RazorViews.BaseView/*02000010*/::get_Context() /* 06000042 */
  } // end of property BaseView::Context
  .property /*1700001C*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpRequest/*01000032*/
          Request()
  {
    .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpRequest/*01000032*/ Microsoft.Extensions.RazorViews.BaseView/*02000010*/::get_Request() /* 06000043 */
  } // end of property BaseView::Request
  .property /*1700001D*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpResponse/*01000033*/
          Response()
  {
    .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpResponse/*01000033*/ Microsoft.Extensions.RazorViews.BaseView/*02000010*/::get_Response() /* 06000044 */
  } // end of property BaseView::Response
  .property /*1700001E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.TextWriter/*01000034*/
          Output()
  {
    .get instance class [System.Runtime/*23000001*/]System.IO.TextWriter/*01000034*/ Microsoft.Extensions.RazorViews.BaseView/*02000010*/::get_Output() /* 06000045 */
  } // end of property BaseView::Output
  .property /*1700001F*/ instance callconv(8) class [System.Text.Encodings.Web/*23000006*/]System.Text.Encodings.Web.HtmlEncoder/*01000035*/
          HtmlEncoder()
  {
    .get instance class [System.Text.Encodings.Web/*23000006*/]System.Text.Encodings.Web.HtmlEncoder/*01000035*/ Microsoft.Extensions.RazorViews.BaseView/*02000010*/::get_HtmlEncoder() /* 06000046 */
    .set instance void Microsoft.Extensions.RazorViews.BaseView/*02000010*/::set_HtmlEncoder(class [System.Text.Encodings.Web/*23000006*/]System.Text.Encodings.Web.HtmlEncoder/*01000035*/) /* 06000047 */
  } // end of property BaseView::HtmlEncoder
  .property /*17000020*/ instance callconv(8) class [System.Text.Encodings.Web/*23000006*/]System.Text.Encodings.Web.UrlEncoder/*01000036*/
          UrlEncoder()
  {
    .get instance class [System.Text.Encodings.Web/*23000006*/]System.Text.Encodings.Web.UrlEncoder/*01000036*/ Microsoft.Extensions.RazorViews.BaseView/*02000010*/::get_UrlEncoder() /* 06000048 */
    .set instance void Microsoft.Extensions.RazorViews.BaseView/*02000010*/::set_UrlEncoder(class [System.Text.Encodings.Web/*23000006*/]System.Text.Encodings.Web.UrlEncoder/*01000036*/) /* 06000049 */
  } // end of property BaseView::UrlEncoder
  .property /*17000021*/ instance callconv(8) class [System.Text.Encodings.Web/*23000006*/]System.Text.Encodings.Web.JavaScriptEncoder/*01000037*/
          JavaScriptEncoder()
  {
    .get instance class [System.Text.Encodings.Web/*23000006*/]System.Text.Encodings.Web.JavaScriptEncoder/*01000037*/ Microsoft.Extensions.RazorViews.BaseView/*02000010*/::get_JavaScriptEncoder() /* 0600004A */
    .set instance void Microsoft.Extensions.RazorViews.BaseView/*02000010*/::set_JavaScriptEncoder(class [System.Text.Encodings.Web/*23000006*/]System.Text.Encodings.Web.JavaScriptEncoder/*01000037*/) /* 0600004B */
  } // end of property BaseView::JavaScriptEncoder
} // end of class Microsoft.Extensions.RazorViews.BaseView

.class /*02000011*/ private auto ansi beforefieldinit Microsoft.Extensions.RazorViews.HelperResult
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00005B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600005C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [System.Runtime/*23000001*/]System.IO.TextWriter/*01000034*/> action) cil managed
  // SIG: 20 01 01 15 12 81 05 01 12 80 D1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HelperResult::.ctor

  .method /*0600005D*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [System.Runtime/*23000001*/]System.IO.TextWriter/*01000034*/> 
          get_WriteAction() cil managed
  // SIG: 20 00 15 12 81 05 01 12 80 D1
  {
    .custom /*0C000121:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HelperResult::get_WriteAction

  .method /*0600005E*/ public hidebysig instance void 
          WriteTo(class [System.Runtime/*23000001*/]System.IO.TextWriter/*01000034*/ writer) cil managed
  // SIG: 20 01 01 12 80 D1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HelperResult::WriteTo

  .property /*17000022*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [System.Runtime/*23000001*/]System.IO.TextWriter/*01000034*/>
          WriteAction()
  {
    .get instance class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [System.Runtime/*23000001*/]System.IO.TextWriter/*01000034*/> Microsoft.Extensions.RazorViews.HelperResult/*02000011*/::get_WriteAction() /* 0600005D */
  } // end of property HelperResult::WriteAction
} // end of class Microsoft.Extensions.RazorViews.HelperResult

.class /*02000012*/ public abstract auto ansi sealed beforefieldinit Microsoft.Extensions.Hosting.GenericHostWebHostBuilderExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000063:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000064:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000065:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*0200004E*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000114:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000033*/ public static initonly class Microsoft.Extensions.Hosting.GenericHostWebHostBuilderExtensions/*02000012*//'<>c'/*0200004E*/ '<>9'
    .field /*04000034*/ public static class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class Microsoft.Extensions.Hosting.WebHostBuilderOptions/*02000013*/> '<>9__0_0'
    .field /*04000035*/ public static class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.HostBuilderContext/*01000045*/,class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/> '<>9__1_0'
    .method /*06000191*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000192*/ assembly hidebysig 
            instance void  '<ConfigureWebHost>b__0_0'(class Microsoft.Extensions.Hosting.WebHostBuilderOptions/*02000013*/ _) cil managed
    // SIG: 20 01 01 12 4C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<ConfigureWebHost>b__0_0'

    .method /*06000193*/ assembly hidebysig 
            instance void  '<ConfigureWebHost>b__1_0'(class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.HostBuilderContext/*01000045*/ context,
                                                      class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
    // SIG: 20 02 01 12 81 15 12 81 19
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<ConfigureWebHost>b__1_0'

  } // end of class '<>c'

  .method /*0600005F*/ public hidebysig static 
          class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000042*/ 
          ConfigureWebHost(class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000042*/ builder,
                           class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/> configure) cil managed
  // SIG: 00 02 12 81 09 12 81 09 15 12 81 05 01 12 81 0D
  {
    .custom /*0C000124:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericHostWebHostBuilderExtensions::ConfigureWebHost

  .method /*06000060*/ public hidebysig static 
          class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000042*/ 
          ConfigureWebHost(class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000042*/ builder,
                           class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/> configure,
                           class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class Microsoft.Extensions.Hosting.WebHostBuilderOptions/*02000013*/> configureWebHostBuilder) cil managed
  // SIG: 00 03 12 81 09 12 81 09 15 12 81 05 01 12 81 0D 15 12 81 05 01 12 4C
  {
    .custom /*0C000126:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericHostWebHostBuilderExtensions::ConfigureWebHost

} // end of class Microsoft.Extensions.Hosting.GenericHostWebHostBuilderExtensions

.class /*02000013*/ public auto ansi beforefieldinit Microsoft.Extensions.Hosting.WebHostBuilderOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*06000061*/ public hidebysig specialname 
          instance bool  get_SuppressEnvironmentConfiguration() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000128:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilderOptions::get_SuppressEnvironmentConfiguration

  .method /*06000062*/ public hidebysig specialname 
          instance void  set_SuppressEnvironmentConfiguration(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C00012A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilderOptions::set_SuppressEnvironmentConfiguration

  .method /*06000063*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilderOptions::.ctor

  .property /*17000023*/ instance callconv(8) bool
          SuppressEnvironmentConfiguration()
  {
    .get instance bool Microsoft.Extensions.Hosting.WebHostBuilderOptions/*02000013*/::get_SuppressEnvironmentConfiguration() /* 06000061 */
    .set instance void Microsoft.Extensions.Hosting.WebHostBuilderOptions/*02000013*/::set_SuppressEnvironmentConfiguration(bool) /* 06000062 */
  } // end of property WebHostBuilderOptions::SuppressEnvironmentConfiguration
} // end of class Microsoft.Extensions.Hosting.WebHostBuilderOptions

.class /*02000014*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/
{
  .custom /*0C00006A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00006B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/
  .custom /*0C000002:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200004F*/ auto ansi sealed nested private beforefieldinit StaticWebAssetsDirectoryContents
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
         implements [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IDirectoryContents/*01000047*/,
                    class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileInfo/*01000026*/>/*1B000005*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000024*/
  {
    .method /*06000194*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileInfo/*01000026*/> files) cil managed
    // SIG: 20 01 01 15 12 75 01 12 80 99
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetsDirectoryContents::.ctor

    .method /*06000195*/ public hidebysig newslot specialname virtual final 
            instance bool  get_Exists() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetsDirectoryContents::get_Exists

    .method /*06000196*/ public hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileInfo/*01000026*/> 
            GetEnumerator() cil managed
    // SIG: 20 00 15 12 80 8D 01 12 80 99
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetsDirectoryContents::GetEnumerator

    .method /*06000197*/ private hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/ 
            System.Collections.IEnumerable.GetEnumerator() cil managed
    // SIG: 20 00 12 80 89
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000024*/::GetEnumerator /*01000024::0A00001D*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetsDirectoryContents::System.Collections.IEnumerable.GetEnumerator

    .property /*17000073*/ instance callconv(8) bool
            Exists()
    {
      .get instance bool Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetsDirectoryContents/*0200004F*/::get_Exists() /* 06000195 */
    } // end of property StaticWebAssetsDirectoryContents::Exists
  } // end of class StaticWebAssetsDirectoryContents

  .class /*02000050*/ auto ansi sealed nested private beforefieldinit StaticWebAssetsDirectoryInfo
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
         implements [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileInfo/*01000026*/
  {
    .method /*06000198*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(string name) cil managed
    // SIG: 20 01 01 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetsDirectoryInfo::.ctor

    .method /*06000199*/ public hidebysig newslot specialname virtual final 
            instance bool  get_Exists() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetsDirectoryInfo::get_Exists

    .method /*0600019A*/ public hidebysig newslot specialname virtual final 
            instance int64  get_Length() cil managed
    // SIG: 20 00 0A
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetsDirectoryInfo::get_Length

    .method /*0600019B*/ public hidebysig newslot specialname virtual final 
            instance string  get_PhysicalPath() cil managed
    // SIG: 20 00 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetsDirectoryInfo::get_PhysicalPath

    .method /*0600019C*/ public hidebysig newslot specialname virtual final 
            instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000049*/ 
            get_LastModified() cil managed
    // SIG: 20 00 11 81 25
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetsDirectoryInfo::get_LastModified

    .method /*0600019D*/ public hidebysig newslot specialname virtual final 
            instance bool  get_IsDirectory() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetsDirectoryInfo::get_IsDirectory

    .method /*0600019E*/ public hidebysig newslot specialname virtual final 
            instance string  get_Name() cil managed
    // SIG: 20 00 0E
    {
      .custom /*0C00023F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetsDirectoryInfo::get_Name

    .method /*0600019F*/ public hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100003A*/ 
            CreateReadStream() cil managed
    // SIG: 20 00 12 80 E9
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetsDirectoryInfo::CreateReadStream

    .property /*17000074*/ instance callconv(8) bool
            Exists()
    {
      .get instance bool Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetsDirectoryInfo/*02000050*/::get_Exists() /* 06000199 */
    } // end of property StaticWebAssetsDirectoryInfo::Exists
    .property /*17000075*/ instance callconv(8) int64
            Length()
    {
      .get instance int64 Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetsDirectoryInfo/*02000050*/::get_Length() /* 0600019A */
    } // end of property StaticWebAssetsDirectoryInfo::Length
    .property /*17000076*/ instance callconv(8) string
            PhysicalPath()
    {
      .get instance string Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetsDirectoryInfo/*02000050*/::get_PhysicalPath() /* 0600019B */
    } // end of property StaticWebAssetsDirectoryInfo::PhysicalPath
    .property /*17000077*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000049*/
            LastModified()
    {
      .get instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000049*/ Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetsDirectoryInfo/*02000050*/::get_LastModified() /* 0600019C */
    } // end of property StaticWebAssetsDirectoryInfo::LastModified
    .property /*17000078*/ instance callconv(8) bool
            IsDirectory()
    {
      .get instance bool Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetsDirectoryInfo/*02000050*/::get_IsDirectory() /* 0600019D */
    } // end of property StaticWebAssetsDirectoryInfo::IsDirectory
    .property /*17000079*/ instance callconv(8) string
            Name()
    {
      .get instance string Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetsDirectoryInfo/*02000050*/::get_Name() /* 0600019E */
    } // end of property StaticWebAssetsDirectoryInfo::Name
  } // end of class StaticWebAssetsDirectoryInfo

  .class /*02000051*/ auto ansi sealed nested private beforefieldinit StaticWebAssetsFileInfo
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
         implements [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileInfo/*01000026*/
  {
    .method /*060001A0*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(string name,
                                 class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileInfo/*01000026*/ source) cil managed
    // SIG: 20 02 01 0E 12 80 99
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetsFileInfo::.ctor

    .method /*060001A1*/ public hidebysig newslot specialname virtual final 
            instance bool  get_Exists() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetsFileInfo::get_Exists

    .method /*060001A2*/ public hidebysig newslot specialname virtual final 
            instance int64  get_Length() cil managed
    // SIG: 20 00 0A
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetsFileInfo::get_Length

    .method /*060001A3*/ public hidebysig newslot specialname virtual final 
            instance string  get_PhysicalPath() cil managed
    // SIG: 20 00 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetsFileInfo::get_PhysicalPath

    .method /*060001A4*/ public hidebysig newslot specialname virtual final 
            instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000049*/ 
            get_LastModified() cil managed
    // SIG: 20 00 11 81 25
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetsFileInfo::get_LastModified

    .method /*060001A5*/ public hidebysig newslot specialname virtual final 
            instance bool  get_IsDirectory() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetsFileInfo::get_IsDirectory

    .method /*060001A6*/ public hidebysig newslot specialname virtual final 
            instance string  get_Name() cil managed
    // SIG: 20 00 0E
    {
      .custom /*0C000240:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetsFileInfo::get_Name

    .method /*060001A7*/ public hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100003A*/ 
            CreateReadStream() cil managed
    // SIG: 20 00 12 80 E9
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetsFileInfo::CreateReadStream

    .property /*1700007A*/ instance callconv(8) bool
            Exists()
    {
      .get instance bool Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetsFileInfo/*02000051*/::get_Exists() /* 060001A1 */
    } // end of property StaticWebAssetsFileInfo::Exists
    .property /*1700007B*/ instance callconv(8) int64
            Length()
    {
      .get instance int64 Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetsFileInfo/*02000051*/::get_Length() /* 060001A2 */
    } // end of property StaticWebAssetsFileInfo::Length
    .property /*1700007C*/ instance callconv(8) string
            PhysicalPath()
    {
      .get instance string Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetsFileInfo/*02000051*/::get_PhysicalPath() /* 060001A3 */
    } // end of property StaticWebAssetsFileInfo::PhysicalPath
    .property /*1700007D*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000049*/
            LastModified()
    {
      .get instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000049*/ Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetsFileInfo/*02000051*/::get_LastModified() /* 060001A4 */
    } // end of property StaticWebAssetsFileInfo::LastModified
    .property /*1700007E*/ instance callconv(8) bool
            IsDirectory()
    {
      .get instance bool Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetsFileInfo/*02000051*/::get_IsDirectory() /* 060001A5 */
    } // end of property StaticWebAssetsFileInfo::IsDirectory
    .property /*1700007F*/ instance callconv(8) string
            Name()
    {
      .get instance string Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetsFileInfo/*02000051*/::get_Name() /* 060001A6 */
    } // end of property StaticWebAssetsFileInfo::Name
  } // end of class StaticWebAssetsFileInfo

  .class /*02000052*/ auto ansi sealed nested private beforefieldinit FileNameComparer
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*0100004A*/<class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileInfo/*01000026*/>/*1B000006*/
  {
    .method /*060001A8*/ public hidebysig newslot virtual final 
            instance bool  Equals(class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileInfo/*01000026*/ x,
                                  class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileInfo/*01000026*/ y) cil managed
    // SIG: 20 02 02 12 80 99 12 80 99
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method FileNameComparer::Equals

    .method /*060001A9*/ public hidebysig newslot virtual final 
            instance int32  GetHashCode(class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileInfo/*01000026*/ obj) cil managed
    // SIG: 20 01 08 12 80 99
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method FileNameComparer::GetHashCode

    .method /*060001AA*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method FileNameComparer::.ctor

  } // end of class FileNameComparer

  .class /*02000053*/ auto ansi sealed nested assembly beforefieldinit StaticWebAssetManifest
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000117:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*04000036*/ static assembly initonly class [System.Runtime/*23000001*/]System.StringComparer/*0100004B*/ PathComparer
    .method /*060001AB*/ public hidebysig specialname 
            instance string[]  get_ContentRoots() cil managed
    // SIG: 20 00 1D 0E
    {
      .custom /*0C000241:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetManifest::get_ContentRoots

    .method /*060001AC*/ public hidebysig specialname 
            instance void  set_ContentRoots(string[] 'value') cil managed
    // SIG: 20 01 01 1D 0E
    {
      .custom /*0C000242:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetManifest::set_ContentRoots

    .method /*060001AD*/ public hidebysig specialname 
            instance class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/ 
            get_Root() cil managed
    // SIG: 20 00 12 81 50
    {
      .custom /*0C000243:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetManifest::get_Root

    .method /*060001AE*/ public hidebysig specialname 
            instance void  set_Root(class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/ 'value') cil managed
    // SIG: 20 01 01 12 81 50
    {
      .custom /*0C000244:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetManifest::set_Root

    .method /*060001AF*/ assembly hidebysig static 
            class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetManifest/*02000053*/ 
            Parse(class [System.Runtime/*23000001*/]System.IO.Stream/*0100003A*/ manifest) cil managed
    // SIG: 00 01 12 81 4C 12 80 E9
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetManifest::Parse

    .method /*060001B0*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetManifest::.ctor

    .property /*17000080*/ instance callconv(8) string[]
            ContentRoots()
    {
      .get instance string[] Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetManifest/*02000053*/::get_ContentRoots() /* 060001AB */
      .set instance void Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetManifest/*02000053*/::set_ContentRoots(string[]) /* 060001AC */
    } // end of property StaticWebAssetManifest::ContentRoots
    .property /*17000081*/ instance callconv(8) class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/
            Root()
    {
      .get instance class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/ Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetManifest/*02000053*/::get_Root() /* 060001AD */
      .set instance void Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetManifest/*02000053*/::set_Root(class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/) /* 060001AE */
    } // end of property StaticWebAssetManifest::Root
  } // end of class StaticWebAssetManifest

  .class /*02000054*/ auto ansi sealed nested assembly beforefieldinit StaticWebAssetNode
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000118:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000119:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .method /*060001B1*/ public hidebysig specialname 
            instance class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetMatch/*02000055*/ 
            get_Match() cil managed
    // SIG: 20 00 12 81 54
    {
      .custom /*0C000245:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetNode::get_Match

    .method /*060001B2*/ public hidebysig specialname 
            instance void  set_Match(class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetMatch/*02000055*/ 'value') cil managed
    // SIG: 20 01 01 12 81 54
    {
      .custom /*0C000246:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetNode::set_Match

    .method /*060001B3*/ public hidebysig specialname 
            instance class [System.Collections/*2300000B*/]System.Collections.Generic.Dictionary`2/*0100004C*/<string,class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/> 
            get_Children() cil managed
    // SIG: 20 00 15 12 81 31 02 0E 12 81 50
    {
      .custom /*0C000247:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      .param [0]/*0800017F*/ 
      .custom /*0C000234:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetNode::get_Children

    .method /*060001B4*/ public hidebysig specialname 
            instance void  set_Children(class [System.Collections/*2300000B*/]System.Collections.Generic.Dictionary`2/*0100004C*/<string,class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/> 'value') cil managed
    // SIG: 20 01 01 15 12 81 31 02 0E 12 81 50
    {
      .custom /*0C000248:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      .param [1]/*08000180*/ 
      .custom /*0C000235:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetNode::set_Children

    .method /*060001B5*/ public hidebysig specialname 
            instance class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetPattern/*02000056*/[] 
            get_Patterns() cil managed
    // SIG: 20 00 1D 12 81 58
    {
      .custom /*0C000249:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      .param [0]/*08000181*/ 
      .custom /*0C000237:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetNode::get_Patterns

    .method /*060001B6*/ public hidebysig specialname 
            instance void  set_Patterns(class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetPattern/*02000056*/[] 'value') cil managed
    // SIG: 20 01 01 1D 12 81 58
    {
      .custom /*0C00024A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      .param [1]/*08000182*/ 
      .custom /*0C000239:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetNode::set_Patterns

    .method /*060001B7*/ assembly hidebysig 
            instance bool  HasChildren() cil managed
    // SIG: 20 00 02
    {
      .custom /*0C00024B:0A000027*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.MemberNotNullWhenAttribute/*0100004D*/::.ctor(bool,
                                                                                                                                                            string) /* 0A000027 */ = ( 01 00 01 08 43 68 69 6C 64 72 65 6E 00 00 )       // ....Children..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetNode::HasChildren

    .method /*060001B8*/ assembly hidebysig 
            instance bool  HasPatterns() cil managed
    // SIG: 20 00 02
    {
      .custom /*0C00024C:0A000027*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.MemberNotNullWhenAttribute/*0100004D*/::.ctor(bool,
                                                                                                                                                            string) /* 0A000027 */ = ( 01 00 01 08 50 61 74 74 65 72 6E 73 00 00 )       // ....Patterns..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetNode::HasPatterns

    .method /*060001B9*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetNode::.ctor

    .property /*17000082*/ instance callconv(8) class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetMatch/*02000055*/
            Match()
    {
      .custom /*0C00014F:0A000028*/ instance void [System.Text.Json/*2300000C*/]System.Text.Json.Serialization.JsonPropertyNameAttribute/*0100004E*/::.ctor(string) /* 0A000028 */ = ( 01 00 05 41 73 73 65 74 00 00 )                   // ...Asset..
      .get instance class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetMatch/*02000055*/ Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/::get_Match() /* 060001B1 */
      .set instance void Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/::set_Match(class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetMatch/*02000055*/) /* 060001B2 */
    } // end of property StaticWebAssetNode::Match
    .property /*17000083*/ instance callconv(8) class [System.Collections/*2300000B*/]System.Collections.Generic.Dictionary`2/*0100004C*/<string,class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/>
            Children()
    {
      .custom /*0C000153:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
      .custom /*0C000154:0A000029*/ instance void [System.Text.Json/*2300000C*/]System.Text.Json.Serialization.JsonConverterAttribute/*0100004F*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000019*/) /* 0A000029 */ = ( 01 00 5C 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..\Microsoft.Asp
                                                                                                                                                                                                                                       4E 65 74 43 6F 72 65 2E 53 74 61 74 69 63 57 65   // NetCore.StaticWe
                                                                                                                                                                                                                                       62 41 73 73 65 74 73 2E 4D 61 6E 69 66 65 73 74   // bAssets.Manifest
                                                                                                                                                                                                                                       53 74 61 74 69 63 57 65 62 41 73 73 65 74 46 69   // StaticWebAssetFi
                                                                                                                                                                                                                                       6C 65 50 72 6F 76 69 64 65 72 2B 4F 53 42 61 73   // leProvider+OSBas
                                                                                                                                                                                                                                       65 64 43 61 73 65 43 6F 6E 76 65 72 74 65 72 00   // edCaseConverter.
                                                                                                                                                                                                                                       00 ) 
      .get instance class [System.Collections/*2300000B*/]System.Collections.Generic.Dictionary`2/*0100004C*/<string,class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/> Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/::get_Children() /* 060001B3 */
      .set instance void Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/::set_Children(class [System.Collections/*2300000B*/]System.Collections.Generic.Dictionary`2/*0100004C*/<string,class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/>) /* 060001B4 */
    } // end of property StaticWebAssetNode::Children
    .property /*17000084*/ instance callconv(8) class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetPattern/*02000056*/[]
            Patterns()
    {
      .custom /*0C000158:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
      .get instance class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetPattern/*02000056*/[] Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/::get_Patterns() /* 060001B5 */
      .set instance void Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/::set_Patterns(class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetPattern/*02000056*/[]) /* 060001B6 */
    } // end of property StaticWebAssetNode::Patterns
  } // end of class StaticWebAssetNode

  .class /*02000055*/ auto ansi sealed nested assembly beforefieldinit StaticWebAssetMatch
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00011A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .method /*060001BA*/ public hidebysig specialname 
            instance int32  get_ContentRoot() cil managed
    // SIG: 20 00 08
    {
      .custom /*0C00024D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetMatch::get_ContentRoot

    .method /*060001BB*/ public hidebysig specialname 
            instance void  set_ContentRoot(int32 'value') cil managed
    // SIG: 20 01 01 08
    {
      .custom /*0C00024E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetMatch::set_ContentRoot

    .method /*060001BC*/ public hidebysig specialname 
            instance string  get_Path() cil managed
    // SIG: 20 00 0E
    {
      .custom /*0C00024F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetMatch::get_Path

    .method /*060001BD*/ public hidebysig specialname 
            instance void  set_Path(string 'value') cil managed
    // SIG: 20 01 01 0E
    {
      .custom /*0C000250:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetMatch::set_Path

    .method /*060001BE*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetMatch::.ctor

    .property /*17000085*/ instance callconv(8) int32
            ContentRoot()
    {
      .custom /*0C00015B:0A000028*/ instance void [System.Text.Json/*2300000C*/]System.Text.Json.Serialization.JsonPropertyNameAttribute/*0100004E*/::.ctor(string) /* 0A000028 */ = ( 01 00 10 43 6F 6E 74 65 6E 74 52 6F 6F 74 49 6E   // ...ContentRootIn
                                                                                                                                                                                       64 65 78 00 00 )                                  // dex..
      .get instance int32 Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetMatch/*02000055*/::get_ContentRoot() /* 060001BA */
      .set instance void Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetMatch/*02000055*/::set_ContentRoot(int32) /* 060001BB */
    } // end of property StaticWebAssetMatch::ContentRoot
    .property /*17000086*/ instance callconv(8) string
            Path()
    {
      .custom /*0C00015E:0A000028*/ instance void [System.Text.Json/*2300000C*/]System.Text.Json.Serialization.JsonPropertyNameAttribute/*0100004E*/::.ctor(string) /* 0A000028 */ = ( 01 00 07 53 75 62 50 61 74 68 00 00 )             // ...SubPath..
      .get instance string Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetMatch/*02000055*/::get_Path() /* 060001BC */
      .set instance void Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetMatch/*02000055*/::set_Path(string) /* 060001BD */
    } // end of property StaticWebAssetMatch::Path
  } // end of class StaticWebAssetMatch

  .class /*02000056*/ auto ansi sealed nested assembly beforefieldinit StaticWebAssetPattern
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00011B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .method /*060001BF*/ public hidebysig specialname 
            instance int32  get_ContentRoot() cil managed
    // SIG: 20 00 08
    {
      .custom /*0C000251:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetPattern::get_ContentRoot

    .method /*060001C0*/ public hidebysig specialname 
            instance void  set_ContentRoot(int32 'value') cil managed
    // SIG: 20 01 01 08
    {
      .custom /*0C000252:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetPattern::set_ContentRoot

    .method /*060001C1*/ public hidebysig specialname 
            instance int32  get_Depth() cil managed
    // SIG: 20 00 08
    {
      .custom /*0C000253:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetPattern::get_Depth

    .method /*060001C2*/ public hidebysig specialname 
            instance void  set_Depth(int32 'value') cil managed
    // SIG: 20 01 01 08
    {
      .custom /*0C000254:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetPattern::set_Depth

    .method /*060001C3*/ public hidebysig specialname 
            instance string  get_Pattern() cil managed
    // SIG: 20 00 0E
    {
      .custom /*0C000255:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetPattern::get_Pattern

    .method /*060001C4*/ public hidebysig specialname 
            instance void  set_Pattern(string 'value') cil managed
    // SIG: 20 01 01 0E
    {
      .custom /*0C000256:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetPattern::set_Pattern

    .method /*060001C5*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StaticWebAssetPattern::.ctor

    .property /*17000087*/ instance callconv(8) int32
            ContentRoot()
    {
      .custom /*0C000161:0A000028*/ instance void [System.Text.Json/*2300000C*/]System.Text.Json.Serialization.JsonPropertyNameAttribute/*0100004E*/::.ctor(string) /* 0A000028 */ = ( 01 00 10 43 6F 6E 74 65 6E 74 52 6F 6F 74 49 6E   // ...ContentRootIn
                                                                                                                                                                                       64 65 78 00 00 )                                  // dex..
      .get instance int32 Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetPattern/*02000056*/::get_ContentRoot() /* 060001BF */
      .set instance void Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetPattern/*02000056*/::set_ContentRoot(int32) /* 060001C0 */
    } // end of property StaticWebAssetPattern::ContentRoot
    .property /*17000088*/ instance callconv(8) int32
            Depth()
    {
      .get instance int32 Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetPattern/*02000056*/::get_Depth() /* 060001C1 */
      .set instance void Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetPattern/*02000056*/::set_Depth(int32) /* 060001C2 */
    } // end of property StaticWebAssetPattern::Depth
    .property /*17000089*/ instance callconv(8) string
            Pattern()
    {
      .get instance string Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetPattern/*02000056*/::get_Pattern() /* 060001C3 */
      .set instance void Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetPattern/*02000056*/::set_Pattern(string) /* 060001C4 */
    } // end of property StaticWebAssetPattern::Pattern
  } // end of class StaticWebAssetPattern

  .class /*02000057*/ auto ansi sealed nested private beforefieldinit OSBasedCaseConverter
         extends class [System.Text.Json/*2300000C*/]System.Text.Json.Serialization.JsonConverter`1/*01000050*/<class [System.Collections/*2300000B*/]System.Collections.Generic.Dictionary`2/*0100004C*/<string,class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/>>/*1B000007*/
  {
    .method /*060001C6*/ public hidebysig virtual 
            instance class [System.Collections/*2300000B*/]System.Collections.Generic.Dictionary`2/*0100004C*/<string,class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/> 
            Read(valuetype [System.Text.Json/*2300000C*/]System.Text.Json.Utf8JsonReader/*01000051*/& reader,
                 class [System.Runtime/*23000001*/]System.Type/*01000019*/ typeToConvert,
                 class [System.Text.Json/*2300000C*/]System.Text.Json.JsonSerializerOptions/*01000052*/ options) cil managed
    // SIG: 20 03 15 12 81 31 02 0E 12 81 50 10 11 81 45 12 65 12 81 49
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method OSBasedCaseConverter::Read

    .method /*060001C7*/ public hidebysig virtual 
            instance void  Write(class [System.Text.Json/*2300000C*/]System.Text.Json.Utf8JsonWriter/*01000053*/ writer,
                                 class [System.Collections/*2300000B*/]System.Collections.Generic.Dictionary`2/*0100004C*/<string,class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/> 'value',
                                 class [System.Text.Json/*2300000C*/]System.Text.Json.JsonSerializerOptions/*01000052*/ options) cil managed
    // SIG: 20 03 01 12 81 4D 15 12 81 31 02 0E 12 81 50 12 81 49
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method OSBasedCaseConverter::Write

    .method /*060001C8*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method OSBasedCaseConverter::.ctor

    .method /*060001C9*/ assembly hidebysig static 
            void  '<Read>g__MergeChildren|0_0'(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000054*/<string,class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/> newChildren,
                                               class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000054*/<string,class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetNode/*02000054*/> existing) cil managed
    // SIG: 00 02 01 15 12 81 51 02 0E 12 81 50 15 12 81 51 02 0E 12 81 50
    {
      .custom /*0C000257:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method OSBasedCaseConverter::'<Read>g__MergeChildren|0_0'

  } // end of class OSBasedCaseConverter

  .class /*02000058*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass7_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00011C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000037*/ public class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetMatch/*02000055*/ match
    .method /*060001CA*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass7_0'::.ctor

    .method /*060001CB*/ assembly hidebysig 
            instance bool  '<GetDirectoryContents>b__2'(class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileInfo/*01000026*/ f) cil managed
    // SIG: 20 01 02 12 80 99
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass7_0'::'<GetDirectoryContents>b__2'

  } // end of class '<>c__DisplayClass7_0'

  .method /*06000064*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*//StaticWebAssetManifest/*02000053*/ manifest,
                               class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<string,class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/> fileProviderFactory) cil managed
  // SIG: 20 02 01 12 81 4C 15 12 80 A9 02 0E 12 79
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ManifestStaticWebAssetFileProvider::.ctor

  .method /*06000065*/ assembly hidebysig specialname 
          instance class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/[] 
          get_FileProviders() cil managed
  // SIG: 20 00 1D 12 79
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ManifestStaticWebAssetFileProvider::get_FileProviders

  .method /*06000066*/ public hidebysig newslot virtual final 
          instance class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IDirectoryContents/*01000047*/ 
          GetDirectoryContents(string subpath) cil managed
  // SIG: 20 01 12 81 1D 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ManifestStaticWebAssetFileProvider::GetDirectoryContents

  .method /*06000067*/ public hidebysig newslot virtual final 
          instance class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileInfo/*01000026*/ 
          GetFileInfo(string subpath) cil managed
  // SIG: 20 01 12 80 99 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ManifestStaticWebAssetFileProvider::GetFileInfo

  .method /*06000068*/ public hidebysig newslot virtual final 
          instance class [Microsoft.Extensions.Primitives/*2300000A*/]Microsoft.Extensions.Primitives.IChangeToken/*01000048*/ 
          Watch(string 'filter') cil managed
  // SIG: 20 01 12 81 21 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ManifestStaticWebAssetFileProvider::Watch

  .property /*17000024*/ instance callconv(8) class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/[]
          FileProviders()
  {
    .get instance class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/[] Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider/*02000014*/::get_FileProviders() /* 06000065 */
  } // end of property ManifestStaticWebAssetFileProvider::FileProviders
} // end of class Microsoft.AspNetCore.StaticWebAssets.ManifestStaticWebAssetFileProvider

.class /*02000015*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Http.DefaultHttpContextFactory
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.IHttpContextFactory/*01000055*/
{
  .custom /*0C00006F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000070:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.IHttpContextFactory/*01000055*/
  .custom /*0C00001C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000069*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ serviceProvider) cil managed
  // SIG: 20 01 01 12 81 59
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHttpContextFactory::.ctor

  .method /*0600006A*/ assembly hidebysig specialname 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.IHttpContextAccessor/*01000057*/ 
          get_HttpContextAccessor() cil managed
  // SIG: 20 00 12 81 5D
  {
    .custom /*0C000133:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHttpContextFactory::get_HttpContextAccessor

  .method /*0600006B*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000031*/ 
          Create(class [Microsoft.Extensions.Features/*2300000E*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000058*/ featureCollection) cil managed
  // SIG: 20 01 12 80 C5 12 81 61
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHttpContextFactory::Create

  .method /*0600006C*/ assembly hidebysig 
          instance void  Initialize(class [Microsoft.AspNetCore.Http/*2300000F*/]Microsoft.AspNetCore.Http.DefaultHttpContext/*01000059*/ httpContext,
                                    class [Microsoft.Extensions.Features/*2300000E*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000058*/ featureCollection) cil managed aggressiveinlining
  // SIG: 20 02 01 12 81 65 12 81 61
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHttpContextFactory::Initialize

  .method /*0600006D*/ public hidebysig newslot virtual final 
          instance void  Dispose(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000031*/ httpContext) cil managed
  // SIG: 20 01 01 12 80 C5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHttpContextFactory::Dispose

  .method /*0600006E*/ assembly hidebysig 
          instance void  Dispose(class [Microsoft.AspNetCore.Http/*2300000F*/]Microsoft.AspNetCore.Http.DefaultHttpContext/*01000059*/ httpContext) cil managed
  // SIG: 20 01 01 12 81 65
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHttpContextFactory::Dispose

  .property /*17000025*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.IHttpContextAccessor/*01000057*/
          HttpContextAccessor()
  {
    .custom /*0C0000AC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.IHttpContextAccessor/*01000057*/ Microsoft.AspNetCore.Http.DefaultHttpContextFactory/*02000015*/::get_HttpContextAccessor() /* 0600006A */
  } // end of property DefaultHttpContextFactory::HttpContextAccessor
} // end of class Microsoft.AspNetCore.Http.DefaultHttpContextFactory

.class /*02000016*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.GenericWebHostApplicationLifetime
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IApplicationLifetime/*0100005A*/
{
  .method /*0600006F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostApplicationLifetime/*0100005B*/ applicationLifetime) cil managed
  // SIG: 20 01 01 12 81 6D
  {
    .custom /*0C000139:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostApplicationLifetime::.ctor

  .method /*06000070*/ public hidebysig newslot specialname virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ 
          get_ApplicationStarted() cil managed
  // SIG: 20 00 11 81 71
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostApplicationLifetime::get_ApplicationStarted

  .method /*06000071*/ public hidebysig newslot specialname virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ 
          get_ApplicationStopping() cil managed
  // SIG: 20 00 11 81 71
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostApplicationLifetime::get_ApplicationStopping

  .method /*06000072*/ public hidebysig newslot specialname virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ 
          get_ApplicationStopped() cil managed
  // SIG: 20 00 11 81 71
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostApplicationLifetime::get_ApplicationStopped

  .method /*06000073*/ public hidebysig newslot virtual final 
          instance void  StopApplication() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostApplicationLifetime::StopApplication

  .property /*17000026*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/
          ApplicationStarted()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ Microsoft.AspNetCore.Hosting.GenericWebHostApplicationLifetime/*02000016*/::get_ApplicationStarted() /* 06000070 */
  } // end of property GenericWebHostApplicationLifetime::ApplicationStarted
  .property /*17000027*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/
          ApplicationStopping()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ Microsoft.AspNetCore.Hosting.GenericWebHostApplicationLifetime/*02000016*/::get_ApplicationStopping() /* 06000071 */
  } // end of property GenericWebHostApplicationLifetime::ApplicationStopping
  .property /*17000028*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/
          ApplicationStopped()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ Microsoft.AspNetCore.Hosting.GenericWebHostApplicationLifetime/*02000016*/::get_ApplicationStopped() /* 06000072 */
  } // end of property GenericWebHostApplicationLifetime::ApplicationStopped
} // end of class Microsoft.AspNetCore.Hosting.GenericWebHostApplicationLifetime

.class /*02000017*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.GenericWebHostBuilder
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/,
                  Microsoft.AspNetCore.Hosting.Infrastructure.ISupportsStartup/*02000043*/,
                  Microsoft.AspNetCore.Hosting.ISupportsUseDefaultServiceProvider/*0200001B*/
{
  .custom /*0C000075:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000076:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/
  .custom /*0C000026:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Hosting.Infrastructure.ISupportsStartup/*02000043*/
  .custom /*0C00002A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Hosting.ISupportsUseDefaultServiceProvider/*0200001B*/
  .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000059*/ auto ansi nested private beforefieldinit HostServiceProvider
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
         implements [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/
  {
    .method /*060001CC*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/ context) cil managed
    // SIG: 20 01 01 12 81 79
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method HostServiceProvider::.ctor

    .method /*060001CD*/ public hidebysig newslot virtual final 
            instance object  GetService(class [System.Runtime/*23000001*/]System.Type/*01000019*/ serviceType) cil managed
    // SIG: 20 01 1C 12 65
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method HostServiceProvider::GetService

  } // end of class HostServiceProvider

  .class /*0200005A*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00011E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000038*/ public static initonly class Microsoft.AspNetCore.Hosting.GenericWebHostBuilder/*02000017*//'<>c'/*0200005A*/ '<>9'
    .field /*04000039*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/,class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DiagnosticListener/*01000064*/> '<>9__6_4'
    .field /*0400003A*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/,class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DiagnosticSource/*01000065*/> '<>9__6_5'
    .field /*0400003B*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/,class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.ActivitySource/*01000066*/> '<>9__6_6'
    .method /*060001CE*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*060001CF*/ assembly hidebysig 
            instance class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DiagnosticListener/*01000064*/ 
            '<.ctor>b__6_4'(class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ sp) cil managed
    // SIG: 20 01 12 81 91 12 81 59
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.ctor>b__6_4'

    .method /*060001D0*/ assembly hidebysig 
            instance class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DiagnosticSource/*01000065*/ 
            '<.ctor>b__6_5'(class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ sp) cil managed
    // SIG: 20 01 12 81 95 12 81 59
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.ctor>b__6_5'

    .method /*060001D1*/ assembly hidebysig 
            instance class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.ActivitySource/*01000066*/ 
            '<.ctor>b__6_6'(class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ sp) cil managed
    // SIG: 20 01 12 81 99 12 81 59
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.ctor>b__6_6'

  } // end of class '<>c'

  .class /*0200005B*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass10_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00011F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400003C*/ public class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/> configureServices
    .method /*060001D2*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass10_0'::.ctor

    .method /*060001D3*/ assembly hidebysig 
            instance void  '<ConfigureServices>b__0'(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/ context,
                                                     class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
    // SIG: 20 02 01 12 81 79 12 81 19
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass10_0'::'<ConfigureServices>b__0'

  } // end of class '<>c__DisplayClass10_0'

  .class /*0200005C*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass11_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000120:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400003D*/ public class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/> configureServices
    .method /*060001D4*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass11_0'::.ctor

    .method /*060001D5*/ assembly hidebysig 
            instance void  '<ConfigureServices>b__0'(class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.HostBuilderContext/*01000045*/ context,
                                                     class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ builder) cil managed
    // SIG: 20 02 01 12 81 15 12 81 19
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass11_0'::'<ConfigureServices>b__0'

  } // end of class '<>c__DisplayClass11_0'

  .class /*0200005D*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass12_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000122:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400003E*/ public class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.Extensions.DependencyInjection/*23000011*/]Microsoft.Extensions.DependencyInjection.ServiceProviderOptions/*01000060*/> configure
    .method /*060001D6*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass12_0'::.ctor

    .method /*060001D7*/ assembly hidebysig 
            instance class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceProviderFactory`1/*01000067*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/> 
            '<UseDefaultServiceProvider>b__0'(class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.HostBuilderContext/*01000045*/ context) cil managed
    // SIG: 20 01 15 12 81 9D 01 12 81 19 12 81 15
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass12_0'::'<UseDefaultServiceProvider>b__0'

  } // end of class '<>c__DisplayClass12_0'

  .class /*0200005E*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass13_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000123:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400003F*/ public class Microsoft.AspNetCore.Hosting.GenericWebHostBuilder/*02000017*/ '<>4__this'
    .field /*04000040*/ public class [System.Runtime/*23000001*/]System.Type/*01000019*/ startupType
    .method /*060001D8*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass13_0'::.ctor

    .method /*060001D9*/ assembly hidebysig 
            instance void  '<UseStartup>b__0'(class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.HostBuilderContext/*01000045*/ context,
                                              class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
    // SIG: 20 02 01 12 81 15 12 81 19
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass13_0'::'<UseStartup>b__0'

  } // end of class '<>c__DisplayClass13_0'

  .class /*0200005F*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass14_0`1'<TStartup>
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000125:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000041*/ public class Microsoft.AspNetCore.Hosting.GenericWebHostBuilder/*02000017*/ '<>4__this'
    .field /*04000042*/ public class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,!TStartup> startupFactory
    .method /*060001DA*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass14_0`1'::.ctor

    .method /*060001DB*/ assembly hidebysig 
            instance void  '<UseStartup>b__0'(class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.HostBuilderContext/*01000045*/ context,
                                              class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
    // SIG: 20 02 01 12 81 15 12 81 19
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass14_0`1'::'<UseStartup>b__0'

  } // end of class '<>c__DisplayClass14_0`1'

  .class /*02000060*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass15_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000127:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000043*/ public class [System.Runtime/*23000001*/]System.Runtime.ExceptionServices.ExceptionDispatchInfo/*01000068*/ startupError
    .field /*04000044*/ public object 'instance'
    .field /*04000045*/ public class Microsoft.AspNetCore.Hosting.ConfigureBuilder/*0200001E*/ configureBuilder
    .field /*04000046*/ public class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> '<>9__1'
    .method /*060001DC*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass15_0'::.ctor

    .method /*060001DD*/ assembly hidebysig 
            instance void  '<UseStartup>b__0'(class Microsoft.AspNetCore.Hosting.GenericWebHostServiceOptions/*02000019*/ options) cil managed
    // SIG: 20 01 01 12 64
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass15_0'::'<UseStartup>b__0'

    .method /*060001DE*/ assembly hidebysig 
            instance void  '<UseStartup>b__1'(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/ app) cil managed
    // SIG: 20 01 01 12 81 8D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass15_0'::'<UseStartup>b__1'

  } // end of class '<>c__DisplayClass15_0'

  .class /*02000061*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass17_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000129:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000047*/ public class Microsoft.AspNetCore.Hosting.GenericWebHostBuilder/*02000017*/ '<>4__this'
    .field /*04000048*/ public class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> configure
    .field /*04000049*/ public class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> '<>9__2'
    .field /*0400004A*/ public class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class Microsoft.AspNetCore.Hosting.GenericWebHostServiceOptions/*02000019*/> '<>9__1'
    .method /*060001DF*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass17_0'::.ctor

    .method /*060001E0*/ assembly hidebysig 
            instance void  '<Configure>b__0'(class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.HostBuilderContext/*01000045*/ context,
                                             class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
    // SIG: 20 02 01 12 81 15 12 81 19
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass17_0'::'<Configure>b__0'

    .method /*060001E1*/ assembly hidebysig 
            instance void  '<Configure>b__1'(class Microsoft.AspNetCore.Hosting.GenericWebHostServiceOptions/*02000019*/ options) cil managed
    // SIG: 20 01 01 12 64
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass17_0'::'<Configure>b__1'

    .method /*060001E2*/ assembly hidebysig 
            instance void  '<Configure>b__2'(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/ app) cil managed
    // SIG: 20 01 01 12 81 8D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass17_0'::'<Configure>b__2'

  } // end of class '<>c__DisplayClass17_0'

  .class /*02000062*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass18_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00012B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400004B*/ public class Microsoft.AspNetCore.Hosting.GenericWebHostBuilder/*02000017*/ '<>4__this'
    .field /*0400004C*/ public class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> configure
    .method /*060001E3*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass18_0'::.ctor

    .method /*060001E4*/ assembly hidebysig 
            instance void  '<Configure>b__0'(class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.HostBuilderContext/*01000045*/ context,
                                             class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
    // SIG: 20 02 01 12 81 15 12 81 19
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass18_0'::'<Configure>b__0'

  } // end of class '<>c__DisplayClass18_0'

  .class /*02000063*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass18_1'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00012C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400004D*/ public class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.HostBuilderContext/*01000045*/ context
    .field /*0400004E*/ public class Microsoft.AspNetCore.Hosting.GenericWebHostBuilder/*02000017*//'<>c__DisplayClass18_0'/*02000062*/ 'CS$<>8__locals1'
    .method /*060001E5*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass18_1'::.ctor

    .method /*060001E6*/ assembly hidebysig 
            instance void  '<Configure>b__1'(class Microsoft.AspNetCore.Hosting.GenericWebHostServiceOptions/*02000019*/ options) cil managed
    // SIG: 20 01 01 12 64
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass18_1'::'<Configure>b__1'

  } // end of class '<>c__DisplayClass18_1'

  .class /*02000064*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass18_2'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00012D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400004F*/ public class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/ webhostBuilderContext
    .field /*04000050*/ public class Microsoft.AspNetCore.Hosting.GenericWebHostBuilder/*02000017*//'<>c__DisplayClass18_1'/*02000063*/ 'CS$<>8__locals2'
    .method /*060001E7*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass18_2'::.ctor

    .method /*060001E8*/ assembly hidebysig 
            instance void  '<Configure>b__2'(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/ app) cil managed
    // SIG: 20 01 01 12 81 8D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass18_2'::'<Configure>b__2'

  } // end of class '<>c__DisplayClass18_2'

  .class /*02000065*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass6_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00012E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000051*/ public class Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/ webHostOptions
    .field /*04000052*/ public class Microsoft.AspNetCore.Hosting.GenericWebHostBuilder/*02000017*/ '<>4__this'
    .method /*060001E9*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass6_0'::.ctor

    .method /*060001EA*/ assembly hidebysig 
            instance void  '<.ctor>b__3'(class Microsoft.AspNetCore.Hosting.GenericWebHostServiceOptions/*02000019*/ options) cil managed
    // SIG: 20 01 01 12 64
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass6_0'::'<.ctor>b__3'

  } // end of class '<>c__DisplayClass6_0'

  .class /*02000066*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass6_1'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00012F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000053*/ public class [System.Runtime/*23000001*/]System.Runtime.ExceptionServices.ExceptionDispatchInfo/*01000068*/ capture
    .field /*04000054*/ public class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> '<>9__8'
    .method /*060001EB*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass6_1'::.ctor

    .method /*060001EC*/ assembly hidebysig 
            instance void  '<.ctor>b__7'(class Microsoft.AspNetCore.Hosting.GenericWebHostServiceOptions/*02000019*/ options) cil managed
    // SIG: 20 01 01 12 64
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass6_1'::'<.ctor>b__7'

    .method /*060001ED*/ assembly hidebysig 
            instance void  '<.ctor>b__8'(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/ app) cil managed
    // SIG: 20 01 01 12 81 8D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass6_1'::'<.ctor>b__8'

  } // end of class '<>c__DisplayClass6_1'

  .class /*02000067*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass9_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000130:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000055*/ public class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*0100005F*/> configureDelegate
    .method /*060001EE*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass9_0'::.ctor

    .method /*060001EF*/ assembly hidebysig 
            instance void  '<ConfigureAppConfiguration>b__0'(class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.HostBuilderContext/*01000045*/ context,
                                                             class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*0100005F*/ builder) cil managed
    // SIG: 20 02 01 12 81 15 12 81 7D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass9_0'::'<ConfigureAppConfiguration>b__0'

  } // end of class '<>c__DisplayClass9_0'

  .method /*06000074*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000042*/ builder,
                               class Microsoft.Extensions.Hosting.WebHostBuilderOptions/*02000013*/ options) cil managed
  // SIG: 20 02 01 12 81 09 12 4C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostBuilder::.ctor

  .method /*06000075*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHost/*0100005D*/ 
          Build() cil managed
  // SIG: 20 00 12 81 75
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostBuilder::Build

  .method /*06000076*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          ConfigureAppConfiguration(class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*0100005F*/> configureDelegate) cil managed
  // SIG: 20 01 12 81 0D 15 12 81 11 02 12 81 79 12 81 7D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostBuilder::ConfigureAppConfiguration

  .method /*06000077*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          ConfigureServices(class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/> configureServices) cil managed
  // SIG: 20 01 12 81 0D 15 12 81 05 01 12 81 19
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostBuilder::ConfigureServices

  .method /*06000078*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          ConfigureServices(class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/> configureServices) cil managed
  // SIG: 20 01 12 81 0D 15 12 81 11 02 12 81 79 12 81 19
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostBuilder::ConfigureServices

  .method /*06000079*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          UseDefaultServiceProvider(class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.Extensions.DependencyInjection/*23000011*/]Microsoft.Extensions.DependencyInjection.ServiceProviderOptions/*01000060*/> configure) cil managed
  // SIG: 20 01 12 81 0D 15 12 81 11 02 12 81 79 12 81 81
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostBuilder::UseDefaultServiceProvider

  .method /*0600007A*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          UseStartup(class [System.Runtime/*23000001*/]System.Type/*01000019*/ startupType) cil managed
  // SIG: 20 01 12 81 0D 12 65
  {
    .param [1]/*0800006F*/ 
    .custom /*0C00013B:0A00002A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000062*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000061*/) /* 0A00002A */ = ( 01 00 0B 00 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostBuilder::UseStartup

  .method /*0600007B*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          UseStartup<TStartup>(class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,!!TStartup> startupFactory) cil managed
  // SIG: 30 01 01 12 81 0D 15 12 80 A9 02 12 81 79 1E 00
  {
    .param type TStartup /*2A000003*/ 
      .custom /*0C000021:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .custom /*0C000022:0A00002A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000062*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000061*/) /* 0A00002A */ = ( 01 00 08 00 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostBuilder::UseStartup

  .method /*0600007C*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          Configure(class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> configure) cil managed
  // SIG: 20 01 12 81 0D 15 12 81 05 01 12 81 8D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostBuilder::Configure

  .method /*0600007D*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          Configure(class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> configure) cil managed
  // SIG: 20 01 12 81 0D 15 12 81 11 02 12 81 79 12 81 8D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostBuilder::Configure

  .method /*0600007E*/ public hidebysig newslot virtual final 
          instance string  GetSetting(string key) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostBuilder::GetSetting

  .method /*0600007F*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          UseSetting(string key,
                     string 'value') cil managed
  // SIG: 20 02 12 81 0D 0E 0E
  {
    .param [2]/*08000075*/ 
    .custom /*0C000142:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostBuilder::UseSetting

} // end of class Microsoft.AspNetCore.Hosting.GenericWebHostBuilder

.class /*02000018*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Hosting.GenericWebHostService
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostedService/*01000069*/
{
  .custom /*0C00007C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00007D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostedService/*01000069*/
  .custom /*0C000033:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000068*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .method /*060001F0*/ public hidebysig static 
            void  ListeningOnAddress(class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ logger,
                                     string address) cil managed
    // SIG: 00 02 01 12 7D 0E
    {
      .custom /*0C000258:0A000021*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*01000028*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LogLevel/*01000027*/,
                                                                                                                                                                                string) /* 0A000021 */ = ( 01 00 0E 00 00 00 02 00 00 00 1B 4E 6F 77 20 6C   // ...........Now l
                                                                                                                                                                                                           69 73 74 65 6E 69 6E 67 20 6F 6E 3A 20 7B 61 64   // istening on: {ad
                                                                                                                                                                                                           64 72 65 73 73 7D 01 00 54 0E 09 45 76 65 6E 74   // dress}..T..Event
                                                                                                                                                                                                           4E 61 6D 65 12 4C 69 73 74 65 6E 69 6E 67 4F 6E   // Name.ListeningOn
                                                                                                                                                                                                           41 64 64 72 65 73 73 )                            // Address
      .custom /*0C000259:0A000022*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*01000029*/::.ctor(string,
                                                                                                                                                string) /* 0A000022 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ListeningOnAddress

    .method /*060001F1*/ public hidebysig static 
            void  StartupAssemblyLoaded(class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ logger,
                                        string assemblyName) cil managed
    // SIG: 00 02 01 12 7D 0E
    {
      .custom /*0C00025A:0A000021*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*01000028*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LogLevel/*01000027*/,
                                                                                                                                                                                string) /* 0A000021 */ = ( 01 00 0D 00 00 00 01 00 00 00 2E 4C 6F 61 64 65   // ...........Loade
                                                                                                                                                                                                           64 20 68 6F 73 74 69 6E 67 20 73 74 61 72 74 75   // d hosting startu
                                                                                                                                                                                                           70 20 61 73 73 65 6D 62 6C 79 20 7B 61 73 73 65   // p assembly {asse
                                                                                                                                                                                                           6D 62 6C 79 4E 61 6D 65 7D 02 00 54 0E 09 45 76   // mblyName}..T..Ev
                                                                                                                                                                                                           65 6E 74 4E 61 6D 65 1C 48 6F 73 74 69 6E 67 53   // entName.HostingS
                                                                                                                                                                                                           74 61 72 74 75 70 41 73 73 65 6D 62 6C 79 4C 6F   // tartupAssemblyLo
                                                                                                                                                                                                           61 64 65 64 54 02 10 53 6B 69 70 45 6E 61 62 6C   // adedT..SkipEnabl
                                                                                                                                                                                                           65 64 43 68 65 63 6B 01 )                         // edCheck.
      .custom /*0C00025B:0A000022*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*01000029*/::.ctor(string,
                                                                                                                                                string) /* 0A000022 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::StartupAssemblyLoaded

  } // end of class Log

  .class /*02000069*/ auto ansi sealed nested private beforefieldinit '<StartAsync>d__37'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001B*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C000131:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000056*/ public int32 '<>1__state'
    .field /*04000057*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100003D*/ '<>t__builder'
    .field /*04000058*/ public class Microsoft.AspNetCore.Hosting.GenericWebHostService/*02000018*/ '<>4__this'
    .field /*04000059*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ cancellationToken
    .field /*0400005A*/ private class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000071*/<string> '<addresses>5__2'
    .field /*0400005B*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100003F*/ '<>u__1'
    .method /*060001F2*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000025*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<StartAsync>d__37'::MoveNext

    .method /*060001F3*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C00025C:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000026*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<StartAsync>d__37'::SetStateMachine

  } // end of class '<StartAsync>d__37'

  .class /*0200006A*/ auto ansi sealed nested private beforefieldinit '<StopAsync>d__38'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001B*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C000134:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400005C*/ public int32 '<>1__state'
    .field /*0400005D*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100003D*/ '<>t__builder'
    .field /*0400005E*/ public class Microsoft.AspNetCore.Hosting.GenericWebHostService/*02000018*/ '<>4__this'
    .field /*0400005F*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ cancellationToken
    .field /*04000060*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100003F*/ '<>u__1'
    .method /*060001F4*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000025*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<StopAsync>d__38'::MoveNext

    .method /*060001F5*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C00025D:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000026*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<StopAsync>d__38'::SetStateMachine

  } // end of class '<StopAsync>d__38'

  .method /*06000080*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Options/*23000013*/]Microsoft.Extensions.Options.IOptions`1/*0100006A*/<class Microsoft.AspNetCore.Hosting.GenericWebHostServiceOptions/*02000019*/> options,
                               class [Microsoft.AspNetCore.Hosting.Server.Abstractions/*23000014*/]Microsoft.AspNetCore.Hosting.Server.IServer/*0100006B*/ server,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILoggerFactory/*0100006C*/ loggerFactory,
                               class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DiagnosticListener/*01000064*/ diagnosticListener,
                               class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.ActivitySource/*01000066*/ activitySource,
                               class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DistributedContextPropagator/*0100006D*/ propagator,
                               class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.IHttpContextFactory/*01000055*/ httpContextFactory,
                               class Microsoft.AspNetCore.Hosting.Builder.IApplicationBuilderFactory/*02000045*/ applicationBuilderFactory,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IStartupFilter/*0100006E*/> startupFilters,
                               class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfiguration/*0100006F*/ configuration,
                               class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostEnvironment/*01000070*/ hostingEnvironment) cil managed
  // SIG: 20 0B 01 15 12 81 A9 01 12 64 12 81 AD 12 81 B1 12 81 91 12 81 99 12 81 B5 12 81 55 12 81 14 15
  //      12 75 01 12 81 B9 12 81 BD 12 81 C1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostService::.ctor

  .method /*06000081*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Hosting.GenericWebHostServiceOptions/*02000019*/ 
          get_Options() cil managed
  // SIG: 20 00 12 64
  {
    .custom /*0C00014C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostService::get_Options

  .method /*06000082*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Hosting.Server.Abstractions/*23000014*/]Microsoft.AspNetCore.Hosting.Server.IServer/*0100006B*/ 
          get_Server() cil managed
  // SIG: 20 00 12 81 AD
  {
    .custom /*0C00014E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostService::get_Server

  .method /*06000083*/ public hidebysig specialname 
          instance class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ 
          get_Logger() cil managed
  // SIG: 20 00 12 7D
  {
    .custom /*0C000150:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostService::get_Logger

  .method /*06000084*/ public hidebysig specialname 
          instance class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ 
          get_LifetimeLogger() cil managed
  // SIG: 20 00 12 7D
  {
    .custom /*0C000155:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostService::get_LifetimeLogger

  .method /*06000085*/ public hidebysig specialname 
          instance class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DiagnosticListener/*01000064*/ 
          get_DiagnosticListener() cil managed
  // SIG: 20 00 12 81 91
  {
    .custom /*0C000159:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostService::get_DiagnosticListener

  .method /*06000086*/ public hidebysig specialname 
          instance class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.ActivitySource/*01000066*/ 
          get_ActivitySource() cil managed
  // SIG: 20 00 12 81 99
  {
    .custom /*0C00015C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostService::get_ActivitySource

  .method /*06000087*/ public hidebysig specialname 
          instance class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DistributedContextPropagator/*0100006D*/ 
          get_Propagator() cil managed
  // SIG: 20 00 12 81 B5
  {
    .custom /*0C00015F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostService::get_Propagator

  .method /*06000088*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.IHttpContextFactory/*01000055*/ 
          get_HttpContextFactory() cil managed
  // SIG: 20 00 12 81 55
  {
    .custom /*0C000162:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostService::get_HttpContextFactory

  .method /*06000089*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Hosting.Builder.IApplicationBuilderFactory/*02000045*/ 
          get_ApplicationBuilderFactory() cil managed
  // SIG: 20 00 12 81 14
  {
    .custom /*0C000164:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostService::get_ApplicationBuilderFactory

  .method /*0600008A*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IStartupFilter/*0100006E*/> 
          get_StartupFilters() cil managed
  // SIG: 20 00 15 12 75 01 12 81 B9
  {
    .custom /*0C000166:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostService::get_StartupFilters

  .method /*0600008B*/ public hidebysig specialname 
          instance class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfiguration/*0100006F*/ 
          get_Configuration() cil managed
  // SIG: 20 00 12 81 BD
  {
    .custom /*0C000168:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostService::get_Configuration

  .method /*0600008C*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostEnvironment/*01000070*/ 
          get_HostingEnvironment() cil managed
  // SIG: 20 00 12 81 C1
  {
    .custom /*0C00016A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostService::get_HostingEnvironment

  .method /*0600008D*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000039*/ 
          StartAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ cancellationToken) cil managed
  // SIG: 20 01 12 80 E5 11 81 71
  {
    .custom /*0C00016C:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000038*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000019*/) /* 0A000023 */ = ( 01 00 44 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..DMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 6F 73 74 69 6E 67 2E   // NetCore.Hosting.
                                                                                                                                                                                                                                        47 65 6E 65 72 69 63 57 65 62 48 6F 73 74 53 65   // GenericWebHostSe
                                                                                                                                                                                                                                        72 76 69 63 65 2B 3C 53 74 61 72 74 41 73 79 6E   // rvice+<StartAsyn
                                                                                                                                                                                                                                        63 3E 64 5F 5F 33 37 00 00 )                      // c>d__37..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostService::StartAsync

  .method /*0600008E*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000039*/ 
          StopAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ cancellationToken) cil managed
  // SIG: 20 01 12 80 E5 11 81 71
  {
    .custom /*0C00016F:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000038*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000019*/) /* 0A000023 */ = ( 01 00 43 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..CMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 6F 73 74 69 6E 67 2E   // NetCore.Hosting.
                                                                                                                                                                                                                                        47 65 6E 65 72 69 63 57 65 62 48 6F 73 74 53 65   // GenericWebHostSe
                                                                                                                                                                                                                                        72 76 69 63 65 2B 3C 53 74 6F 70 41 73 79 6E 63   // rvice+<StopAsync
                                                                                                                                                                                                                                        3E 64 5F 5F 33 38 00 00 )                         // >d__38..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostService::StopAsync

  .property /*17000029*/ instance callconv(8) class Microsoft.AspNetCore.Hosting.GenericWebHostServiceOptions/*02000019*/
          Options()
  {
    .get instance class Microsoft.AspNetCore.Hosting.GenericWebHostServiceOptions/*02000019*/ Microsoft.AspNetCore.Hosting.GenericWebHostService/*02000018*/::get_Options() /* 06000081 */
  } // end of property GenericWebHostService::Options
  .property /*1700002A*/ instance callconv(8) class [Microsoft.AspNetCore.Hosting.Server.Abstractions/*23000014*/]Microsoft.AspNetCore.Hosting.Server.IServer/*0100006B*/
          Server()
  {
    .get instance class [Microsoft.AspNetCore.Hosting.Server.Abstractions/*23000014*/]Microsoft.AspNetCore.Hosting.Server.IServer/*0100006B*/ Microsoft.AspNetCore.Hosting.GenericWebHostService/*02000018*/::get_Server() /* 06000082 */
  } // end of property GenericWebHostService::Server
  .property /*1700002B*/ instance callconv(8) class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/
          Logger()
  {
    .get instance class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ Microsoft.AspNetCore.Hosting.GenericWebHostService/*02000018*/::get_Logger() /* 06000083 */
  } // end of property GenericWebHostService::Logger
  .property /*1700002C*/ instance callconv(8) class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/
          LifetimeLogger()
  {
    .get instance class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ Microsoft.AspNetCore.Hosting.GenericWebHostService/*02000018*/::get_LifetimeLogger() /* 06000084 */
  } // end of property GenericWebHostService::LifetimeLogger
  .property /*1700002D*/ instance callconv(8) class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DiagnosticListener/*01000064*/
          DiagnosticListener()
  {
    .get instance class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DiagnosticListener/*01000064*/ Microsoft.AspNetCore.Hosting.GenericWebHostService/*02000018*/::get_DiagnosticListener() /* 06000085 */
  } // end of property GenericWebHostService::DiagnosticListener
  .property /*1700002E*/ instance callconv(8) class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.ActivitySource/*01000066*/
          ActivitySource()
  {
    .get instance class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.ActivitySource/*01000066*/ Microsoft.AspNetCore.Hosting.GenericWebHostService/*02000018*/::get_ActivitySource() /* 06000086 */
  } // end of property GenericWebHostService::ActivitySource
  .property /*1700002F*/ instance callconv(8) class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DistributedContextPropagator/*0100006D*/
          Propagator()
  {
    .get instance class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DistributedContextPropagator/*0100006D*/ Microsoft.AspNetCore.Hosting.GenericWebHostService/*02000018*/::get_Propagator() /* 06000087 */
  } // end of property GenericWebHostService::Propagator
  .property /*17000030*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.IHttpContextFactory/*01000055*/
          HttpContextFactory()
  {
    .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.IHttpContextFactory/*01000055*/ Microsoft.AspNetCore.Hosting.GenericWebHostService/*02000018*/::get_HttpContextFactory() /* 06000088 */
  } // end of property GenericWebHostService::HttpContextFactory
  .property /*17000031*/ instance callconv(8) class Microsoft.AspNetCore.Hosting.Builder.IApplicationBuilderFactory/*02000045*/
          ApplicationBuilderFactory()
  {
    .get instance class Microsoft.AspNetCore.Hosting.Builder.IApplicationBuilderFactory/*02000045*/ Microsoft.AspNetCore.Hosting.GenericWebHostService/*02000018*/::get_ApplicationBuilderFactory() /* 06000089 */
  } // end of property GenericWebHostService::ApplicationBuilderFactory
  .property /*17000032*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IStartupFilter/*0100006E*/>
          StartupFilters()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IStartupFilter/*0100006E*/> Microsoft.AspNetCore.Hosting.GenericWebHostService/*02000018*/::get_StartupFilters() /* 0600008A */
  } // end of property GenericWebHostService::StartupFilters
  .property /*17000033*/ instance callconv(8) class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfiguration/*0100006F*/
          Configuration()
  {
    .get instance class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfiguration/*0100006F*/ Microsoft.AspNetCore.Hosting.GenericWebHostService/*02000018*/::get_Configuration() /* 0600008B */
  } // end of property GenericWebHostService::Configuration
  .property /*17000034*/ instance callconv(8) class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostEnvironment/*01000070*/
          HostingEnvironment()
  {
    .get instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostEnvironment/*01000070*/ Microsoft.AspNetCore.Hosting.GenericWebHostService/*02000018*/::get_HostingEnvironment() /* 0600008C */
  } // end of property GenericWebHostService::HostingEnvironment
} // end of class Microsoft.AspNetCore.Hosting.GenericWebHostService

.class /*02000019*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.GenericWebHostServiceOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000081:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000082:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600008F*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> 
          get_ConfigureApplication() cil managed
  // SIG: 20 00 15 12 81 05 01 12 81 8D
  {
    .custom /*0C000171:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*08000083*/ 
    .custom /*0C000152:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostServiceOptions::get_ConfigureApplication

  .method /*06000090*/ public hidebysig specialname 
          instance void  set_ConfigureApplication(class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> 'value') cil managed
  // SIG: 20 01 01 15 12 81 05 01 12 81 8D
  {
    .custom /*0C000173:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*08000084*/ 
    .custom /*0C000157:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostServiceOptions::set_ConfigureApplication

  .method /*06000091*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/ 
          get_WebHostOptions() cil managed
  // SIG: 20 00 12 80 C8
  {
    .custom /*0C000175:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostServiceOptions::get_WebHostOptions

  .method /*06000092*/ public hidebysig specialname 
          instance void  set_WebHostOptions(class Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/ 'value') cil managed
  // SIG: 20 01 01 12 80 C8
  {
    .custom /*0C000177:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostServiceOptions::set_WebHostOptions

  .method /*06000093*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.AggregateException/*0100002D*/ 
          get_HostingStartupExceptions() cil managed
  // SIG: 20 00 12 80 B5
  {
    .custom /*0C000179:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00017A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostServiceOptions::get_HostingStartupExceptions

  .method /*06000094*/ public hidebysig specialname 
          instance void  set_HostingStartupExceptions(class [System.Runtime/*23000001*/]System.AggregateException/*0100002D*/ 'value') cil managed
  // SIG: 20 01 01 12 80 B5
  {
    .custom /*0C00017C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00017D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostServiceOptions::set_HostingStartupExceptions

  .method /*06000095*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericWebHostServiceOptions::.ctor

  .property /*17000035*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/>
          ConfigureApplication()
  {
    .custom /*0C0000DD:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> Microsoft.AspNetCore.Hosting.GenericWebHostServiceOptions/*02000019*/::get_ConfigureApplication() /* 0600008F */
    .set instance void Microsoft.AspNetCore.Hosting.GenericWebHostServiceOptions/*02000019*/::set_ConfigureApplication(class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/>) /* 06000090 */
  } // end of property GenericWebHostServiceOptions::ConfigureApplication
  .property /*17000036*/ instance callconv(8) class Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/
          WebHostOptions()
  {
    .get instance class Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/ Microsoft.AspNetCore.Hosting.GenericWebHostServiceOptions/*02000019*/::get_WebHostOptions() /* 06000091 */
    .set instance void Microsoft.AspNetCore.Hosting.GenericWebHostServiceOptions/*02000019*/::set_WebHostOptions(class Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/) /* 06000092 */
  } // end of property GenericWebHostServiceOptions::WebHostOptions
  .property /*17000037*/ instance callconv(8) class [System.Runtime/*23000001*/]System.AggregateException/*0100002D*/
          HostingStartupExceptions()
  {
    .custom /*0C0000E4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.AggregateException/*0100002D*/ Microsoft.AspNetCore.Hosting.GenericWebHostServiceOptions/*02000019*/::get_HostingStartupExceptions() /* 06000093 */
    .set instance void Microsoft.AspNetCore.Hosting.GenericWebHostServiceOptions/*02000019*/::set_HostingStartupExceptions(class [System.Runtime/*23000001*/]System.AggregateException/*0100002D*/) /* 06000094 */
  } // end of property GenericWebHostServiceOptions::HostingStartupExceptions
} // end of class Microsoft.AspNetCore.Hosting.GenericWebHostServiceOptions

.class /*0200001A*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.HostingStartupWebHostBuilder
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/,
                  Microsoft.AspNetCore.Hosting.Infrastructure.ISupportsStartup/*02000043*/,
                  Microsoft.AspNetCore.Hosting.ISupportsUseDefaultServiceProvider/*0200001B*/
{
  .custom /*0C000086:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000087:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/
  .custom /*0C000037:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Hosting.Infrastructure.ISupportsStartup/*02000043*/
  .custom /*0C00003D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Hosting.ISupportsUseDefaultServiceProvider/*0200001B*/
  .custom /*0C000041:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200006B*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass6_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000135:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000061*/ public class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/> configureServices
    .method /*060001F6*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass6_0'::.ctor

    .method /*060001F7*/ assembly hidebysig 
            instance void  '<ConfigureServices>b__0'(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/ context,
                                                     class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
    // SIG: 20 02 01 12 81 79 12 81 19
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass6_0'::'<ConfigureServices>b__0'

  } // end of class '<>c__DisplayClass6_0'

  .method /*06000096*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Hosting.GenericWebHostBuilder/*02000017*/ builder) cil managed
  // SIG: 20 01 01 12 5C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingStartupWebHostBuilder::.ctor

  .method /*06000097*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHost/*0100005D*/ 
          Build() cil managed
  // SIG: 20 00 12 81 75
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingStartupWebHostBuilder::Build

  .method /*06000098*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          ConfigureAppConfiguration(class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*0100005F*/> configureDelegate) cil managed
  // SIG: 20 01 12 81 0D 15 12 81 11 02 12 81 79 12 81 7D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingStartupWebHostBuilder::ConfigureAppConfiguration

  .method /*06000099*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          ConfigureServices(class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/> configureServices) cil managed
  // SIG: 20 01 12 81 0D 15 12 81 05 01 12 81 19
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingStartupWebHostBuilder::ConfigureServices

  .method /*0600009A*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          ConfigureServices(class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/> configureServices) cil managed
  // SIG: 20 01 12 81 0D 15 12 81 11 02 12 81 79 12 81 19
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingStartupWebHostBuilder::ConfigureServices

  .method /*0600009B*/ public hidebysig newslot virtual final 
          instance string  GetSetting(string key) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingStartupWebHostBuilder::GetSetting

  .method /*0600009C*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          UseSetting(string key,
                     string 'value') cil managed
  // SIG: 20 02 12 81 0D 0E 0E
  {
    .param [2]/*0800008D*/ 
    .custom /*0C00016E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingStartupWebHostBuilder::UseSetting

  .method /*0600009D*/ public hidebysig instance void 
          ConfigureServices(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/ context,
                            class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
  // SIG: 20 02 01 12 81 79 12 81 19
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingStartupWebHostBuilder::ConfigureServices

  .method /*0600009E*/ public hidebysig instance void 
          ConfigureAppConfiguration(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/ context,
                                    class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*0100005F*/ builder) cil managed
  // SIG: 20 02 01 12 81 79 12 81 7D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingStartupWebHostBuilder::ConfigureAppConfiguration

  .method /*0600009F*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          UseDefaultServiceProvider(class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.Extensions.DependencyInjection/*23000011*/]Microsoft.Extensions.DependencyInjection.ServiceProviderOptions/*01000060*/> configure) cil managed
  // SIG: 20 01 12 81 0D 15 12 81 11 02 12 81 79 12 81 81
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingStartupWebHostBuilder::UseDefaultServiceProvider

  .method /*060000A0*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          Configure(class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> configure) cil managed
  // SIG: 20 01 12 81 0D 15 12 81 05 01 12 81 8D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingStartupWebHostBuilder::Configure

  .method /*060000A1*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          Configure(class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> configure) cil managed
  // SIG: 20 01 12 81 0D 15 12 81 11 02 12 81 79 12 81 8D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingStartupWebHostBuilder::Configure

  .method /*060000A2*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          UseStartup(class [System.Runtime/*23000001*/]System.Type/*01000019*/ startupType) cil managed
  // SIG: 20 01 12 81 0D 12 65
  {
    .param [1]/*08000095*/ 
    .custom /*0C000180:0A00002A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000062*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000061*/) /* 0A00002A */ = ( 01 00 0B 00 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingStartupWebHostBuilder::UseStartup

  .method /*060000A3*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          UseStartup<TStartup>(class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,!!TStartup> startupFactory) cil managed
  // SIG: 30 01 01 12 81 0D 15 12 80 A9 02 12 81 79 1E 00
  {
    .param type TStartup /*2A000008*/ 
      .custom /*0C000039:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .custom /*0C00003A:0A00002A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000062*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000061*/) /* 0A00002A */ = ( 01 00 08 00 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingStartupWebHostBuilder::UseStartup

} // end of class Microsoft.AspNetCore.Hosting.HostingStartupWebHostBuilder

.class /*0200001B*/ interface private abstract auto ansi Microsoft.AspNetCore.Hosting.ISupportsUseDefaultServiceProvider
{
  .custom /*0C00008A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*060000A4*/ public hidebysig newslot abstract virtual 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          UseDefaultServiceProvider(class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.Extensions.DependencyInjection/*23000011*/]Microsoft.Extensions.DependencyInjection.ServiceProviderOptions/*01000060*/> configure) cil managed
  // SIG: 20 01 12 81 0D 15 12 81 11 02 12 81 79 12 81 81
  {
    // Method begins at RVA 0x0
  } // end of method ISupportsUseDefaultServiceProvider::UseDefaultServiceProvider

} // end of class Microsoft.AspNetCore.Hosting.ISupportsUseDefaultServiceProvider

.class /*0200001C*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Hosting.ActivityFeature
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [Microsoft.AspNetCore.Http/*2300000F*/]Microsoft.AspNetCore.Http.Features.IHttpActivityFeature/*01000072*/
{
  .custom /*0C00008D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00008E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Http/*2300000F*/]Microsoft.AspNetCore.Http.Features.IHttpActivityFeature/*01000072*/
  .custom /*0C000046:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000A5*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.Activity/*01000073*/ activity) cil managed
  // SIG: 20 01 01 12 81 CD
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityFeature::.ctor

  .method /*060000A6*/ public hidebysig newslot specialname virtual final 
          instance class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.Activity/*01000073*/ 
          get_Activity() cil managed
  // SIG: 20 00 12 81 CD
  {
    .custom /*0C000195:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityFeature::get_Activity

  .method /*060000A7*/ public hidebysig newslot specialname virtual final 
          instance void  set_Activity(class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.Activity/*01000073*/ 'value') cil managed
  // SIG: 20 01 01 12 81 CD
  {
    .custom /*0C000197:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityFeature::set_Activity

  .property /*17000038*/ instance callconv(8) class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.Activity/*01000073*/
          Activity()
  {
    .get instance class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.Activity/*01000073*/ Microsoft.AspNetCore.Hosting.ActivityFeature/*0200001C*/::get_Activity() /* 060000A6 */
    .set instance void Microsoft.AspNetCore.Hosting.ActivityFeature/*0200001C*/::set_Activity(class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.Activity/*01000073*/) /* 060000A7 */
  } // end of property ActivityFeature::Activity
} // end of class Microsoft.AspNetCore.Hosting.ActivityFeature

.class /*0200001D*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.ApplicationLifetime
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IApplicationLifetime/*0100005A*/,
                  [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IApplicationLifetime/*01000074*/,
                  [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostApplicationLifetime/*0100005B*/
{
  .method /*060000A8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger`1/*01000075*/<class Microsoft.AspNetCore.Hosting.ApplicationLifetime/*0200001D*/> logger) cil managed
  // SIG: 20 01 01 15 12 81 D5 01 12 74
  {
    .custom /*0C000199:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ApplicationLifetime::.ctor

  .method /*060000A9*/ public hidebysig newslot specialname virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ 
          get_ApplicationStarted() cil managed
  // SIG: 20 00 11 81 71
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ApplicationLifetime::get_ApplicationStarted

  .method /*060000AA*/ public hidebysig newslot specialname virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ 
          get_ApplicationStopping() cil managed
  // SIG: 20 00 11 81 71
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ApplicationLifetime::get_ApplicationStopping

  .method /*060000AB*/ public hidebysig newslot specialname virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ 
          get_ApplicationStopped() cil managed
  // SIG: 20 00 11 81 71
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ApplicationLifetime::get_ApplicationStopped

  .method /*060000AC*/ public hidebysig newslot virtual final 
          instance void  StopApplication() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ApplicationLifetime::StopApplication

  .method /*060000AD*/ public hidebysig instance void 
          NotifyStarted() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ApplicationLifetime::NotifyStarted

  .method /*060000AE*/ public hidebysig instance void 
          NotifyStopped() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ApplicationLifetime::NotifyStopped

  .property /*17000039*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/
          ApplicationStarted()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ Microsoft.AspNetCore.Hosting.ApplicationLifetime/*0200001D*/::get_ApplicationStarted() /* 060000A9 */
  } // end of property ApplicationLifetime::ApplicationStarted
  .property /*1700003A*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/
          ApplicationStopping()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ Microsoft.AspNetCore.Hosting.ApplicationLifetime/*0200001D*/::get_ApplicationStopping() /* 060000AA */
  } // end of property ApplicationLifetime::ApplicationStopping
  .property /*1700003B*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/
          ApplicationStopped()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ Microsoft.AspNetCore.Hosting.ApplicationLifetime/*0200001D*/::get_ApplicationStopped() /* 060000AB */
  } // end of property ApplicationLifetime::ApplicationStopped
} // end of class Microsoft.AspNetCore.Hosting.ApplicationLifetime

.class /*0200001E*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.ConfigureBuilder
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000094:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000095:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200006C*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass4_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000136:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000062*/ public class Microsoft.AspNetCore.Hosting.ConfigureBuilder/*0200001E*/ '<>4__this'
    .field /*04000063*/ public object 'instance'
    .method /*060001F8*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass4_0'::.ctor

    .method /*060001F9*/ assembly hidebysig 
            instance void  '<Build>b__0'(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/ builder) cil managed
    // SIG: 20 01 01 12 81 8D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass4_0'::'<Build>b__0'

  } // end of class '<>c__DisplayClass4_0'

  .method /*060000AF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000076*/ configure) cil managed
  // SIG: 20 01 01 12 81 D9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConfigureBuilder::.ctor

  .method /*060000B0*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000076*/ 
          get_MethodInfo() cil managed
  // SIG: 20 00 12 81 D9
  {
    .custom /*0C00019B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConfigureBuilder::get_MethodInfo

  .method /*060000B1*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> 
          Build(object 'instance') cil managed
  // SIG: 20 01 15 12 81 05 01 12 81 8D 1C
  {
    .param [1]/*0800009C*/ 
    .custom /*0C000188:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConfigureBuilder::Build

  .property /*1700003C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000076*/
          MethodInfo()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000076*/ Microsoft.AspNetCore.Hosting.ConfigureBuilder/*0200001E*/::get_MethodInfo() /* 060000B0 */
  } // end of property ConfigureBuilder::MethodInfo
} // end of class Microsoft.AspNetCore.Hosting.ConfigureBuilder

.class /*0200001F*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.ConfigureContainerBuilder
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000098:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000099:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200006D*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000137:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000064*/ public static initonly class Microsoft.AspNetCore.Hosting.ConfigureContainerBuilder/*0200001F*//'<>c'/*0200006D*/ '<>9'
    .field /*04000065*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<object>,class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<object>> '<>9__0_0'
    .method /*060001FA*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*060001FB*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<object> 
            '<.ctor>b__0_0'(class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<object> f) cil managed
    // SIG: 20 01 15 12 81 05 01 1C 15 12 81 05 01 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.ctor>b__0_0'

  } // end of class '<>c'

  .class /*0200006E*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass10_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000138:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000066*/ public class Microsoft.AspNetCore.Hosting.ConfigureContainerBuilder/*0200001F*/ '<>4__this'
    .field /*04000067*/ public object 'instance'
    .method /*060001FC*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass10_0'::.ctor

    .method /*060001FD*/ assembly hidebysig 
            instance void  '<Invoke>g__StartupConfigureContainer|0'(object containerBuilder) cil managed
    // SIG: 20 01 01 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass10_0'::'<Invoke>g__StartupConfigureContainer|0'

  } // end of class '<>c__DisplayClass10_0'

  .class /*0200006F*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass8_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00013A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000068*/ public class Microsoft.AspNetCore.Hosting.ConfigureContainerBuilder/*0200001F*/ '<>4__this'
    .field /*04000069*/ public object 'instance'
    .method /*060001FE*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass8_0'::.ctor

    .method /*060001FF*/ assembly hidebysig 
            instance void  '<Build>b__0'(object container) cil managed
    // SIG: 20 01 01 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass8_0'::'<Build>b__0'

  } // end of class '<>c__DisplayClass8_0'

  .method /*060000B2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000076*/ configureContainerMethod) cil managed
  // SIG: 20 01 01 12 81 D9
  {
    .custom /*0C00019C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConfigureContainerBuilder::.ctor

  .method /*060000B3*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000076*/ 
          get_MethodInfo() cil managed
  // SIG: 20 00 12 81 D9
  {
    .custom /*0C00019D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00019E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConfigureContainerBuilder::get_MethodInfo

  .method /*060000B4*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<object>,class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<object>> 
          get_ConfigureContainerFilters() cil managed
  // SIG: 20 00 15 12 80 A9 02 15 12 81 05 01 1C 15 12 81 05 01 1C
  {
    .custom /*0C00019F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConfigureContainerBuilder::get_ConfigureContainerFilters

  .method /*060000B5*/ public hidebysig specialname 
          instance void  set_ConfigureContainerFilters(class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<object>,class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<object>> 'value') cil managed
  // SIG: 20 01 01 15 12 80 A9 02 15 12 81 05 01 1C 15 12 81 05 01 1C
  {
    .custom /*0C0001A0:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConfigureContainerBuilder::set_ConfigureContainerFilters

  .method /*060000B6*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<object> 
          Build(object 'instance') cil managed
  // SIG: 20 01 15 12 81 05 01 1C 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConfigureContainerBuilder::Build

  .method /*060000B7*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Type/*01000019*/ 
          GetContainerType() cil managed
  // SIG: 20 00 12 65
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConfigureContainerBuilder::GetContainerType

  .property /*1700003D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000076*/
          MethodInfo()
  {
    .custom /*0C0000F5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000076*/ Microsoft.AspNetCore.Hosting.ConfigureContainerBuilder/*0200001F*/::get_MethodInfo() /* 060000B3 */
  } // end of property ConfigureContainerBuilder::MethodInfo
  .property /*1700003E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<object>,class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<object>>
          ConfigureContainerFilters()
  {
    .get instance class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<object>,class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<object>> Microsoft.AspNetCore.Hosting.ConfigureContainerBuilder/*0200001F*/::get_ConfigureContainerFilters() /* 060000B4 */
    .set instance void Microsoft.AspNetCore.Hosting.ConfigureContainerBuilder/*0200001F*/::set_ConfigureContainerFilters(class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<object>,class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<object>>) /* 060000B5 */
  } // end of property ConfigureContainerBuilder::ConfigureContainerFilters
} // end of class Microsoft.AspNetCore.Hosting.ConfigureContainerBuilder

.class /*02000020*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.ConfigureServicesBuilder
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00009B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00009C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000070*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00013C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400006A*/ public static initonly class Microsoft.AspNetCore.Hosting.ConfigureServicesBuilder/*02000020*//'<>c'/*02000070*/ '<>9'
    .field /*0400006B*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/>,class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/>> '<>9__0_0'
    .field /*0400006C*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*0100002F*/,bool> '<>9__10_0'
    .method /*06000200*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000201*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/> 
            '<.ctor>b__0_0'(class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/> f) cil managed
    // SIG: 20 01 15 12 80 A9 02 12 81 19 12 81 59 15 12 80 A9 02 12 81 19 12 81 59
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.ctor>b__0_0'

    .method /*06000202*/ assembly hidebysig 
            instance bool  '<InvokeCore>b__10_0'(class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*0100002F*/ p) cil managed
    // SIG: 20 01 02 12 80 BD
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<InvokeCore>b__10_0'

  } // end of class '<>c'

  .class /*02000071*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass8_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00013D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400006D*/ public class Microsoft.AspNetCore.Hosting.ConfigureServicesBuilder/*02000020*/ '<>4__this'
    .field /*0400006E*/ public object 'instance'
    .method /*06000203*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass8_0'::.ctor

    .method /*06000204*/ assembly hidebysig 
            instance class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ 
            '<Build>b__0'(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
    // SIG: 20 01 12 81 59 12 81 19
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass8_0'::'<Build>b__0'

  } // end of class '<>c__DisplayClass8_0'

  .class /*02000072*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass9_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00013E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400006F*/ public class Microsoft.AspNetCore.Hosting.ConfigureServicesBuilder/*02000020*/ '<>4__this'
    .field /*04000070*/ public object 'instance'
    .method /*06000205*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass9_0'::.ctor

    .method /*06000206*/ assembly hidebysig 
            instance class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ 
            '<Invoke>g__Startup|0'(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ serviceCollection) cil managed
    // SIG: 20 01 12 81 59 12 81 19
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass9_0'::'<Invoke>g__Startup|0'

  } // end of class '<>c__DisplayClass9_0'

  .method /*060000B8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000076*/ configureServices) cil managed
  // SIG: 20 01 01 12 81 D9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConfigureServicesBuilder::.ctor

  .method /*060000B9*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000076*/ 
          get_MethodInfo() cil managed
  // SIG: 20 00 12 81 D9
  {
    .custom /*0C0001A2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConfigureServicesBuilder::get_MethodInfo

  .method /*060000BA*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/>,class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/>> 
          get_StartupServiceFilters() cil managed
  // SIG: 20 00 15 12 80 A9 02 15 12 80 A9 02 12 81 19 12 81 59 15 12 80 A9 02 12 81 19 12 81 59
  {
    .custom /*0C0001A3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*080000A1*/ 
    .custom /*0C00018E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 07 00 00 00 01 01 01 02 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConfigureServicesBuilder::get_StartupServiceFilters

  .method /*060000BB*/ public hidebysig specialname 
          instance void  set_StartupServiceFilters(class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/>,class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/>> 'value') cil managed
  // SIG: 20 01 01 15 12 80 A9 02 15 12 80 A9 02 12 81 19 12 81 59 15 12 80 A9 02 12 81 19 12 81 59
  {
    .custom /*0C0001A4:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*080000A2*/ 
    .custom /*0C000190:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 07 00 00 00 01 01 01 02 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConfigureServicesBuilder::set_StartupServiceFilters

  .method /*060000BC*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/> 
          Build(object 'instance') cil managed
  // SIG: 20 01 15 12 80 A9 02 12 81 19 12 81 59 1C
  {
    .custom /*0C0001A5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [0]/*080000A3*/ 
    .custom /*0C000192:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConfigureServicesBuilder::Build

  .property /*1700003F*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000076*/
          MethodInfo()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000076*/ Microsoft.AspNetCore.Hosting.ConfigureServicesBuilder/*02000020*/::get_MethodInfo() /* 060000B9 */
  } // end of property ConfigureServicesBuilder::MethodInfo
  .property /*17000040*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/>,class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/>>
          StartupServiceFilters()
  {
    .custom /*0C0000F9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 07 00 00 00 01 01 01 02 01 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/>,class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/>> Microsoft.AspNetCore.Hosting.ConfigureServicesBuilder/*02000020*/::get_StartupServiceFilters() /* 060000BA */
    .set instance void Microsoft.AspNetCore.Hosting.ConfigureServicesBuilder/*02000020*/::set_StartupServiceFilters(class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/>,class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/>>) /* 060000BB */
  } // end of property ConfigureServicesBuilder::StartupServiceFilters
} // end of class Microsoft.AspNetCore.Hosting.ConfigureServicesBuilder

.class /*02000021*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Hosting.ErrorPageBuilder
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .class /*02000073*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass0_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00013F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000071*/ public class Microsoft.AspNetCore.Hosting.Views.ErrorPage/*0200003C*/ errorPage
    .method /*06000207*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass0_0'::.ctor

    .method /*06000208*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000039*/ 
            '<BuildErrorPageApplication>b__0'(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000031*/ context) cil managed
    // SIG: 20 01 12 80 E5 12 80 C5
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass0_0'::'<BuildErrorPageApplication>b__0'

  } // end of class '<>c__DisplayClass0_0'

  .method /*060000BD*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.RequestDelegate/*01000077*/ 
          BuildErrorPageApplication(class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/ contentRootFileProvider,
                                    class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ logger,
                                    bool showDetailedErrors,
                                    class [System.Runtime/*23000001*/]System.Exception/*0100001C*/ exception) cil managed
  // SIG: 00 04 12 81 DD 12 79 12 7D 02 12 71
  {
    .custom /*0C0001A6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorPageBuilder::BuildErrorPageApplication

} // end of class Microsoft.AspNetCore.Hosting.ErrorPageBuilder

.class /*02000022*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.HostedServiceExecutor
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000A0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000074*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass3_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000140:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000072*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ token
    .method /*06000209*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass3_0'::.ctor

    .method /*0600020A*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000039*/ 
            '<StartAsync>b__0'(class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostedService/*01000069*/ service) cil managed
    // SIG: 20 01 12 80 E5 12 81 A5
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass3_0'::'<StartAsync>b__0'

  } // end of class '<>c__DisplayClass3_0'

  .class /*02000075*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass4_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000141:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000073*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ token
    .method /*0600020B*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass4_0'::.ctor

    .method /*0600020C*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000039*/ 
            '<StopAsync>b__0'(class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostedService/*01000069*/ service) cil managed
    // SIG: 20 01 12 80 E5 12 81 A5
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass4_0'::'<StopAsync>b__0'

  } // end of class '<>c__DisplayClass4_0'

  .class /*02000076*/ auto ansi sealed nested private beforefieldinit '<ExecuteAsync>d__5'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001B*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C000143:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000074*/ public int32 '<>1__state'
    .field /*04000075*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100003D*/ '<>t__builder'
    .field /*04000076*/ public class Microsoft.AspNetCore.Hosting.HostedServiceExecutor/*02000022*/ '<>4__this'
    .field /*04000077*/ public class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostedService/*01000069*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000039*/> callback
    .field /*04000078*/ public bool throwOnFirstFailure
    .field /*04000079*/ private class [System.Collections/*2300000B*/]System.Collections.Generic.List`1/*01000078*/<class [System.Runtime/*23000001*/]System.Exception/*0100001C*/> '<exceptions>5__2'
    .field /*0400007A*/ private class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostedService/*01000069*/> '<>7__wrap2'
    .field /*0400007B*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100003F*/ '<>u__1'
    .method /*0600020D*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000025*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__5'::MoveNext

    .method /*0600020E*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C00025E:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000026*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__5'::SetStateMachine

  } // end of class '<ExecuteAsync>d__5'

  .method /*060000BE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger`1/*01000075*/<class Microsoft.AspNetCore.Hosting.HostedServiceExecutor/*02000022*/> logger,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostedService/*01000069*/> services) cil managed
  // SIG: 20 02 01 15 12 81 D5 01 12 80 88 15 12 75 01 12 81 A5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostedServiceExecutor::.ctor

  .method /*060000BF*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000039*/ 
          StartAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ token) cil managed
  // SIG: 20 01 12 80 E5 11 81 71
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostedServiceExecutor::StartAsync

  .method /*060000C0*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000039*/ 
          StopAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ token) cil managed
  // SIG: 20 01 12 80 E5 11 81 71
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostedServiceExecutor::StopAsync

} // end of class Microsoft.AspNetCore.Hosting.HostedServiceExecutor

.class /*02000023*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.HostingApplication
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements class [Microsoft.AspNetCore.Hosting.Server.Abstractions/*23000014*/]Microsoft.AspNetCore.Hosting.Server.IHttpApplication`1/*01000079*/<class Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/>/*1B000008*/
{
  .custom /*0C0000A4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [Microsoft.AspNetCore.Hosting.Server.Abstractions/*23000014*/]Microsoft.AspNetCore.Hosting.Server.IHttpApplication`1/*01000079*/<class Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/>/*1B000008*/
  .custom /*0C000052:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .class /*02000077*/ auto ansi nested assembly beforefieldinit Context
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000144:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000145:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*0400007C*/ assembly class [Microsoft.AspNetCore.Http/*2300000F*/]Microsoft.AspNetCore.Http.Features.IHttpActivityFeature/*01000072*/ HttpActivityFeature
    .method /*0600020F*/ public hidebysig specialname 
            instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000031*/ 
            get_HttpContext() cil managed
    // SIG: 20 00 12 80 C5
    {
      .custom /*0C00025F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Context::get_HttpContext

    .method /*06000210*/ public hidebysig specialname 
            instance void  set_HttpContext(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000031*/ 'value') cil managed
    // SIG: 20 01 01 12 80 C5
    {
      .custom /*0C000260:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Context::set_HttpContext

    .method /*06000211*/ public hidebysig specialname 
            instance class [System.Runtime/*23000001*/]System.IDisposable/*01000021*/ 
            get_Scope() cil managed
    // SIG: 20 00 12 80 85
    {
      .custom /*0C000261:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Context::get_Scope

    .method /*06000212*/ public hidebysig specialname 
            instance void  set_Scope(class [System.Runtime/*23000001*/]System.IDisposable/*01000021*/ 'value') cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000262:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Context::set_Scope

    .method /*06000213*/ public hidebysig specialname 
            instance class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.Activity/*01000073*/ 
            get_Activity() cil managed
    // SIG: 20 00 12 81 CD
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Context::get_Activity

    .method /*06000214*/ public hidebysig specialname 
            instance void  set_Activity(class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.Activity/*01000073*/ 'value') cil managed
    // SIG: 20 01 01 12 81 CD
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Context::set_Activity

    .method /*06000215*/ assembly hidebysig specialname 
            instance class Microsoft.AspNetCore.Hosting.HostingRequestStartingLog/*0200002A*/ 
            get_StartLog() cil managed
    // SIG: 20 00 12 80 A8
    {
      .custom /*0C000263:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Context::get_StartLog

    .method /*06000216*/ assembly hidebysig specialname 
            instance void  set_StartLog(class Microsoft.AspNetCore.Hosting.HostingRequestStartingLog/*0200002A*/ 'value') cil managed
    // SIG: 20 01 01 12 80 A8
    {
      .custom /*0C000264:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Context::set_StartLog

    .method /*06000217*/ public hidebysig specialname 
            instance int64  get_StartTimestamp() cil managed
    // SIG: 20 00 0A
    {
      .custom /*0C000265:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Context::get_StartTimestamp

    .method /*06000218*/ public hidebysig specialname 
            instance void  set_StartTimestamp(int64 'value') cil managed
    // SIG: 20 01 01 0A
    {
      .custom /*0C000266:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Context::set_StartTimestamp

    .method /*06000219*/ assembly hidebysig specialname 
            instance bool  get_HasDiagnosticListener() cil managed
    // SIG: 20 00 02
    {
      .custom /*0C000267:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Context::get_HasDiagnosticListener

    .method /*0600021A*/ assembly hidebysig specialname 
            instance void  set_HasDiagnosticListener(bool 'value') cil managed
    // SIG: 20 01 01 02
    {
      .custom /*0C000268:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Context::set_HasDiagnosticListener

    .method /*0600021B*/ public hidebysig specialname 
            instance bool  get_EventLogEnabled() cil managed
    // SIG: 20 00 02
    {
      .custom /*0C000269:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Context::get_EventLogEnabled

    .method /*0600021C*/ public hidebysig specialname 
            instance void  set_EventLogEnabled(bool 'value') cil managed
    // SIG: 20 01 01 02
    {
      .custom /*0C00026A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Context::set_EventLogEnabled

    .method /*0600021D*/ public hidebysig 
            instance void  Reset() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Context::Reset

    .method /*0600021E*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Context::.ctor

    .property /*1700008A*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000031*/
            HttpContext()
    {
      .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000031*/ Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/::get_HttpContext() /* 0600020F */
      .set instance void Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/::set_HttpContext(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000031*/) /* 06000210 */
    } // end of property Context::HttpContext
    .property /*1700008B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IDisposable/*01000021*/
            Scope()
    {
      .get instance class [System.Runtime/*23000001*/]System.IDisposable/*01000021*/ Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/::get_Scope() /* 06000211 */
      .set instance void Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/::set_Scope(class [System.Runtime/*23000001*/]System.IDisposable/*01000021*/) /* 06000212 */
    } // end of property Context::Scope
    .property /*1700008C*/ instance callconv(8) class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.Activity/*01000073*/
            Activity()
    {
      .get instance class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.Activity/*01000073*/ Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/::get_Activity() /* 06000213 */
      .set instance void Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/::set_Activity(class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.Activity/*01000073*/) /* 06000214 */
    } // end of property Context::Activity
    .property /*1700008D*/ instance callconv(8) class Microsoft.AspNetCore.Hosting.HostingRequestStartingLog/*0200002A*/
            StartLog()
    {
      .get instance class Microsoft.AspNetCore.Hosting.HostingRequestStartingLog/*0200002A*/ Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/::get_StartLog() /* 06000215 */
      .set instance void Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/::set_StartLog(class Microsoft.AspNetCore.Hosting.HostingRequestStartingLog/*0200002A*/) /* 06000216 */
    } // end of property Context::StartLog
    .property /*1700008E*/ instance callconv(8) int64
            StartTimestamp()
    {
      .get instance int64 Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/::get_StartTimestamp() /* 06000217 */
      .set instance void Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/::set_StartTimestamp(int64) /* 06000218 */
    } // end of property Context::StartTimestamp
    .property /*1700008F*/ instance callconv(8) bool
            HasDiagnosticListener()
    {
      .get instance bool Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/::get_HasDiagnosticListener() /* 06000219 */
      .set instance void Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/::set_HasDiagnosticListener(bool) /* 0600021A */
    } // end of property Context::HasDiagnosticListener
    .property /*17000090*/ instance callconv(8) bool
            EventLogEnabled()
    {
      .get instance bool Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/::get_EventLogEnabled() /* 0600021B */
      .set instance void Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/::set_EventLogEnabled(bool) /* 0600021C */
    } // end of property Context::EventLogEnabled
  } // end of class Context

  .method /*060000C1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.RequestDelegate/*01000077*/ application,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ logger,
                               class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DiagnosticListener/*01000064*/ diagnosticSource,
                               class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.ActivitySource/*01000066*/ activitySource,
                               class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DistributedContextPropagator/*0100006D*/ propagator,
                               class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.IHttpContextFactory/*01000055*/ httpContextFactory) cil managed
  // SIG: 20 06 01 12 81 DD 12 7D 12 81 91 12 81 99 12 81 B5 12 81 55
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingApplication::.ctor

  .method /*060000C2*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/ 
          CreateContext(class [Microsoft.Extensions.Features/*2300000E*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000058*/ contextFeatures) cil managed
  // SIG: 20 01 12 81 DC 12 81 61
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingApplication::CreateContext

  .method /*060000C3*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000039*/ 
          ProcessRequestAsync(class Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/ context) cil managed
  // SIG: 20 01 12 80 E5 12 81 DC
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingApplication::ProcessRequestAsync

  .method /*060000C4*/ public hidebysig newslot virtual final 
          instance void  DisposeContext(class Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/ context,
                                        class [System.Runtime/*23000001*/]System.Exception/*0100001C*/ exception) cil managed
  // SIG: 20 02 01 12 81 DC 12 71
  {
    .param [2]/*080000B6*/ 
    .custom /*0C0001A1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingApplication::DisposeContext

} // end of class Microsoft.AspNetCore.Hosting.HostingApplication

.class /*02000024*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.HostingApplicationDiagnostics
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000A7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000078*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .class /*020000A2*/ auto ansi sealed nested private beforefieldinit HostingLogScope
           extends [System.Runtime/*23000001*/]System.Object/*01000017*/
           implements class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100007B*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B000009*/,
                      class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B00000A*/,
                      [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000024*/,
                      class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*0100007D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B00000B*/
    {
      .custom /*0C00018F:0A00002B*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*0100007A*/::.ctor(string) /* 0A00002B */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
      .class /*020000A8*/ auto ansi sealed nested private beforefieldinit '<GetEnumerator>d__9'
             extends [System.Runtime/*23000001*/]System.Object/*01000017*/
             implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B00000C*/,
                        [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/,
                        [System.Runtime/*23000001*/]System.IDisposable/*01000021*/
      {
        .custom /*0C00019A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
        .field /*040000F5*/ public class Microsoft.AspNetCore.Hosting.HostingApplicationDiagnostics/*02000024*//Log/*02000078*//HostingLogScope/*020000A2*/ '<>4__this'
        .method /*0600029F*/ public hidebysig specialname rtspecialname 
                instance void  .ctor(int32 '<>1__state') cil managed
        // SIG: 20 01 01 08
        {
          .custom /*0C00028E:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
          // Method begins at RVA 0x4050
          // Code size       2 (0x2)
          .maxstack  8
          IL_0000:  /* 14   |                  */ ldnull
          IL_0001:  /* 7A   |                  */ throw
        } // end of method '<GetEnumerator>d__9'::.ctor

        .method /*060002A0*/ private hidebysig newslot virtual final 
                instance void  System.IDisposable.Dispose() cil managed
        // SIG: 20 00 01
        {
          .custom /*0C00028F:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
          .override [System.Runtime/*23000001*/]System.IDisposable/*01000021*/::Dispose /*01000021::0A000017*/ 
          // Method begins at RVA 0x4050
          // Code size       2 (0x2)
          .maxstack  8
          IL_0000:  /* 14   |                  */ ldnull
          IL_0001:  /* 7A   |                  */ throw
        } // end of method '<GetEnumerator>d__9'::System.IDisposable.Dispose

        .method /*060002A1*/ private hidebysig newslot virtual final 
                instance bool  MoveNext() cil managed
        // SIG: 20 00 02
        {
          .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/::MoveNext /*01000022::0A000018*/ 
          // Method begins at RVA 0x4050
          // Code size       2 (0x2)
          .maxstack  8
          IL_0000:  /* 14   |                  */ ldnull
          IL_0001:  /* 7A   |                  */ throw
        } // end of method '<GetEnumerator>d__9'::MoveNext

        .method /*060002A2*/ private hidebysig newslot specialname virtual final 
                instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object> 
                'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Current'() cil managed
        // SIG: 20 00 15 11 81 F1 02 0E 1C
        {
          .custom /*0C000290:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
          .override  method instance !0 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B00000C*/::get_Current() /* 0A00002C */
          // Method begins at RVA 0x4050
          // Code size       2 (0x2)
          .maxstack  8
          IL_0000:  /* 14   |                  */ ldnull
          IL_0001:  /* 7A   |                  */ throw
        } // end of method '<GetEnumerator>d__9'::'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Current'

        .method /*060002A3*/ private hidebysig newslot virtual final 
                instance void  System.Collections.IEnumerator.Reset() cil managed
        // SIG: 20 00 01
        {
          .custom /*0C000291:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
          .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/::Reset /*01000022::0A00001A*/ 
          // Method begins at RVA 0x4050
          // Code size       2 (0x2)
          .maxstack  8
          IL_0000:  /* 14   |                  */ ldnull
          IL_0001:  /* 7A   |                  */ throw
        } // end of method '<GetEnumerator>d__9'::System.Collections.IEnumerator.Reset

        .method /*060002A4*/ private hidebysig newslot specialname virtual final 
                instance object  System.Collections.IEnumerator.get_Current() cil managed
        // SIG: 20 00 1C
        {
          .custom /*0C000292:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
          .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/::get_Current /*01000022::0A00001B*/ 
          // Method begins at RVA 0x4050
          // Code size       2 (0x2)
          .maxstack  8
          IL_0000:  /* 14   |                  */ ldnull
          IL_0001:  /* 7A   |                  */ throw
        } // end of method '<GetEnumerator>d__9'::System.Collections.IEnumerator.get_Current

        .property /*1700009B*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>
                'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Current'()
        {
          .get instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object> Microsoft.AspNetCore.Hosting.HostingApplicationDiagnostics/*02000024*//Log/*02000078*//HostingLogScope/*020000A2*//'<GetEnumerator>d__9'/*020000A8*/::'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Current'() /* 060002A2 */
        } // end of property '<GetEnumerator>d__9'::'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Current'
        .property /*1700009C*/ instance callconv(8) object
                System.Collections.IEnumerator.Current()
        {
          .get instance object Microsoft.AspNetCore.Hosting.HostingApplicationDiagnostics/*02000024*//Log/*02000078*//HostingLogScope/*020000A2*//'<GetEnumerator>d__9'/*020000A8*/::System.Collections.IEnumerator.get_Current() /* 060002A4 */
        } // end of property '<GetEnumerator>d__9'::System.Collections.IEnumerator.Current
      } // end of class '<GetEnumerator>d__9'

      .method /*0600028D*/ public hidebysig newslot specialname virtual final 
              instance int32  get_Count() cil managed
      // SIG: 20 00 08
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method HostingLogScope::get_Count

      .method /*0600028E*/ public hidebysig newslot specialname virtual final 
              instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object> 
              get_Item(int32 index) cil managed
      // SIG: 20 01 15 11 81 F1 02 0E 1C 08
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method HostingLogScope::get_Item

      .method /*0600028F*/ public hidebysig specialname rtspecialname 
              instance void  .ctor(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000031*/ httpContext) cil managed
      // SIG: 20 01 01 12 80 C5
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method HostingLogScope::.ctor

      .method /*06000290*/ public hidebysig virtual 
              instance string  ToString() cil managed
      // SIG: 20 00 0E
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method HostingLogScope::ToString

      .method /*06000291*/ public hidebysig newslot virtual final 
              instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>> 
              GetEnumerator() cil managed
      // SIG: 20 00 15 12 80 8D 01 15 11 81 F1 02 0E 1C
      {
        .custom /*0C00028D:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IteratorStateMachineAttribute/*01000020*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000019*/) /* 0A000016 */ = ( 01 00 62 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..bMicrosoft.Asp
                                                                                                                                                                                                                                               4E 65 74 43 6F 72 65 2E 48 6F 73 74 69 6E 67 2E   // NetCore.Hosting.
                                                                                                                                                                                                                                               48 6F 73 74 69 6E 67 41 70 70 6C 69 63 61 74 69   // HostingApplicati
                                                                                                                                                                                                                                               6F 6E 44 69 61 67 6E 6F 73 74 69 63 73 2B 4C 6F   // onDiagnostics+Lo
                                                                                                                                                                                                                                               67 2B 48 6F 73 74 69 6E 67 4C 6F 67 53 63 6F 70   // g+HostingLogScop
                                                                                                                                                                                                                                               65 2B 3C 47 65 74 45 6E 75 6D 65 72 61 74 6F 72   // e+<GetEnumerator
                                                                                                                                                                                                                                               3E 64 5F 5F 39 00 00 )                            // >d__9..
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method HostingLogScope::GetEnumerator

      .method /*06000292*/ private hidebysig newslot virtual final 
              instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/ 
              System.Collections.IEnumerable.GetEnumerator() cil managed
      // SIG: 20 00 12 80 89
      {
        .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000024*/::GetEnumerator /*01000024::0A00001D*/ 
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method HostingLogScope::System.Collections.IEnumerable.GetEnumerator

      .property /*17000099*/ instance callconv(8) int32
              Count()
      {
        .get instance int32 Microsoft.AspNetCore.Hosting.HostingApplicationDiagnostics/*02000024*//Log/*02000078*//HostingLogScope/*020000A2*/::get_Count() /* 0600028D */
      } // end of property HostingLogScope::Count
      .property /*1700009A*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>
              Item(int32)
      {
        .get instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object> Microsoft.AspNetCore.Hosting.HostingApplicationDiagnostics/*02000024*//Log/*02000078*//HostingLogScope/*020000A2*/::get_Item(int32) /* 0600028E */
      } // end of property HostingLogScope::Item
    } // end of class HostingLogScope

    .method /*0600021F*/ public hidebysig static 
            class [System.Runtime/*23000001*/]System.IDisposable/*01000021*/ 
            RequestScope(class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ logger,
                         class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000031*/ httpContext) cil managed
    // SIG: 00 02 12 80 85 12 7D 12 80 C5
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::RequestScope

  } // end of class Log

  .class /*02000079*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000146:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400007D*/ public static initonly class Microsoft.AspNetCore.Hosting.HostingApplicationDiagnostics/*02000024*//'<>c'/*02000079*/ '<>9'
    .field /*0400007E*/ public static class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DistributedContextPropagator/*0100006D*//PropagatorGetterCallback/*0100007E*/ '<>9__21_0'
    .field /*0400007F*/ public static class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DistributedContextPropagator/*0100006D*//PropagatorGetterCallback/*0100007E*/ '<>9__21_1'
    .method /*06000220*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000221*/ assembly hidebysig 
            instance void  '<StartActivity>b__21_0'(object carrier,
                                                    string fieldName,
                                                    [out] string& fieldValue,
                                                    [out] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string>& fieldValues) cil managed
    // SIG: 20 04 01 1C 0E 10 0E 10 15 12 75 01 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<StartActivity>b__21_0'

    .method /*06000222*/ assembly hidebysig 
            instance void  '<StartActivity>b__21_1'(object carrier,
                                                    string fieldName,
                                                    [out] string& fieldValue,
                                                    [out] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string>& fieldValues) cil managed
    // SIG: 20 04 01 1C 0E 10 0E 10 15 12 75 01 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<StartActivity>b__21_1'

  } // end of class '<>c'

  .field /*04000004*/ static assembly literal string ActivityName = "Microsoft.AspNetCore.Hosting.HttpRequestIn"
  .method /*060000C5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ logger,
                               class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DiagnosticListener/*01000064*/ diagnosticListener,
                               class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.ActivitySource/*01000066*/ activitySource,
                               class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DistributedContextPropagator/*0100006D*/ propagator) cil managed
  // SIG: 20 04 01 12 7D 12 81 91 12 81 99 12 81 B5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingApplicationDiagnostics::.ctor

  .method /*060000C6*/ public hidebysig instance void 
          BeginRequest(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000031*/ httpContext,
                       class Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/ context) cil managed aggressiveinlining
  // SIG: 20 02 01 12 80 C5 12 81 DC
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingApplicationDiagnostics::BeginRequest

  .method /*060000C7*/ public hidebysig instance void 
          RequestEnd(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000031*/ httpContext,
                     class [System.Runtime/*23000001*/]System.Exception/*0100001C*/ exception,
                     class Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/ context) cil managed aggressiveinlining
  // SIG: 20 03 01 12 80 C5 12 71 12 81 DC
  {
    .param [2]/*080000BE*/ 
    .custom /*0C0001A7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingApplicationDiagnostics::RequestEnd

  .method /*060000C8*/ public hidebysig static 
          void  ContextDisposed(class Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/ context) cil managed aggressiveinlining
  // SIG: 00 01 01 12 81 DC
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingApplicationDiagnostics::ContextDisposed

} // end of class Microsoft.AspNetCore.Hosting.HostingApplicationDiagnostics

.class /*02000025*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.HostingEnvironment
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IHostingEnvironment/*0100007F*/,
                  [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostingEnvironment/*01000080*/,
                  [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostEnvironment/*01000070*/,
                  [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostEnvironment/*01000081*/
{
  .custom /*0C0000AA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000AB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IHostingEnvironment/*0100007F*/
  .custom /*0C000057:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostingEnvironment/*01000080*/
  .custom /*0C00005D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostEnvironment/*01000070*/
  .custom /*0C000067:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostEnvironment/*01000081*/
  .custom /*0C000068:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000C9*/ public hidebysig newslot specialname virtual final 
          instance string  get_EnvironmentName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001A8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEnvironment::get_EnvironmentName

  .method /*060000CA*/ public hidebysig newslot specialname virtual final 
          instance void  set_EnvironmentName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001A9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEnvironment::set_EnvironmentName

  .method /*060000CB*/ public hidebysig newslot specialname virtual final 
          instance string  get_ApplicationName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001AA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEnvironment::get_ApplicationName

  .method /*060000CC*/ public hidebysig newslot specialname virtual final 
          instance void  set_ApplicationName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001AB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEnvironment::set_ApplicationName

  .method /*060000CD*/ public hidebysig newslot specialname virtual final 
          instance string  get_WebRootPath() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001AC:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEnvironment::get_WebRootPath

  .method /*060000CE*/ public hidebysig newslot specialname virtual final 
          instance void  set_WebRootPath(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001AD:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEnvironment::set_WebRootPath

  .method /*060000CF*/ public hidebysig newslot specialname virtual final 
          instance class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/ 
          get_WebRootFileProvider() cil managed
  // SIG: 20 00 12 79
  {
    .custom /*0C0001AE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEnvironment::get_WebRootFileProvider

  .method /*060000D0*/ public hidebysig newslot specialname virtual final 
          instance void  set_WebRootFileProvider(class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/ 'value') cil managed
  // SIG: 20 01 01 12 79
  {
    .custom /*0C0001AF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEnvironment::set_WebRootFileProvider

  .method /*060000D1*/ public hidebysig newslot specialname virtual final 
          instance string  get_ContentRootPath() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001B0:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEnvironment::get_ContentRootPath

  .method /*060000D2*/ public hidebysig newslot specialname virtual final 
          instance void  set_ContentRootPath(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001B1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEnvironment::set_ContentRootPath

  .method /*060000D3*/ public hidebysig newslot specialname virtual final 
          instance class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/ 
          get_ContentRootFileProvider() cil managed
  // SIG: 20 00 12 79
  {
    .custom /*0C0001B2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEnvironment::get_ContentRootFileProvider

  .method /*060000D4*/ public hidebysig newslot specialname virtual final 
          instance void  set_ContentRootFileProvider(class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/ 'value') cil managed
  // SIG: 20 01 01 12 79
  {
    .custom /*0C0001B3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEnvironment::set_ContentRootFileProvider

  .method /*060000D5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEnvironment::.ctor

  .property /*17000041*/ instance callconv(8) string
          EnvironmentName()
  {
    .get instance string Microsoft.AspNetCore.Hosting.HostingEnvironment/*02000025*/::get_EnvironmentName() /* 060000C9 */
    .set instance void Microsoft.AspNetCore.Hosting.HostingEnvironment/*02000025*/::set_EnvironmentName(string) /* 060000CA */
  } // end of property HostingEnvironment::EnvironmentName
  .property /*17000042*/ instance callconv(8) string
          ApplicationName()
  {
    .get instance string Microsoft.AspNetCore.Hosting.HostingEnvironment/*02000025*/::get_ApplicationName() /* 060000CB */
    .set instance void Microsoft.AspNetCore.Hosting.HostingEnvironment/*02000025*/::set_ApplicationName(string) /* 060000CC */
  } // end of property HostingEnvironment::ApplicationName
  .property /*17000043*/ instance callconv(8) string
          WebRootPath()
  {
    .get instance string Microsoft.AspNetCore.Hosting.HostingEnvironment/*02000025*/::get_WebRootPath() /* 060000CD */
    .set instance void Microsoft.AspNetCore.Hosting.HostingEnvironment/*02000025*/::set_WebRootPath(string) /* 060000CE */
  } // end of property HostingEnvironment::WebRootPath
  .property /*17000044*/ instance callconv(8) class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/
          WebRootFileProvider()
  {
    .get instance class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/ Microsoft.AspNetCore.Hosting.HostingEnvironment/*02000025*/::get_WebRootFileProvider() /* 060000CF */
    .set instance void Microsoft.AspNetCore.Hosting.HostingEnvironment/*02000025*/::set_WebRootFileProvider(class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/) /* 060000D0 */
  } // end of property HostingEnvironment::WebRootFileProvider
  .property /*17000045*/ instance callconv(8) string
          ContentRootPath()
  {
    .get instance string Microsoft.AspNetCore.Hosting.HostingEnvironment/*02000025*/::get_ContentRootPath() /* 060000D1 */
    .set instance void Microsoft.AspNetCore.Hosting.HostingEnvironment/*02000025*/::set_ContentRootPath(string) /* 060000D2 */
  } // end of property HostingEnvironment::ContentRootPath
  .property /*17000046*/ instance callconv(8) class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/
          ContentRootFileProvider()
  {
    .get instance class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/ Microsoft.AspNetCore.Hosting.HostingEnvironment/*02000025*/::get_ContentRootFileProvider() /* 060000D3 */
    .set instance void Microsoft.AspNetCore.Hosting.HostingEnvironment/*02000025*/::set_ContentRootFileProvider(class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/) /* 060000D4 */
  } // end of property HostingEnvironment::ContentRootFileProvider
} // end of class Microsoft.AspNetCore.Hosting.HostingEnvironment

.class /*02000026*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Hosting.HostingEnvironmentExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000AE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000AF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000B0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*060000D6*/ assembly hidebysig static 
          void  Initialize(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IHostingEnvironment/*0100007F*/ hostingEnvironment,
                           string contentRootPath,
                           class Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/ options) cil managed
  // SIG: 00 03 01 12 81 FD 0E 12 80 C8
  {
    .custom /*0C0001B4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEnvironmentExtensions::Initialize

  .method /*060000D7*/ assembly hidebysig static 
          void  Initialize(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostEnvironment/*01000070*/ hostingEnvironment,
                           string contentRootPath,
                           class Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/ options) cil managed
  // SIG: 00 03 01 12 81 C1 0E 12 80 C8
  {
    .custom /*0C0001B5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEnvironmentExtensions::Initialize

} // end of class Microsoft.AspNetCore.Hosting.HostingEnvironmentExtensions

.class /*02000027*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Hosting.HostingEventSource
       extends [System.Diagnostics.Tracing/*23000015*/]System.Diagnostics.Tracing.EventSource/*01000082*/
{
  .custom /*0C0000B3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000B4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000005*/ public static initonly class Microsoft.AspNetCore.Hosting.HostingEventSource/*02000027*/ Log
  .method /*060000D8*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEventSource::.ctor

  .method /*060000D9*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(string eventSourceName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEventSource::.ctor

  .method /*060000DA*/ public hidebysig instance void 
          HostStart() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0001B7:0A00002D*/ instance void [System.Diagnostics.Tracing/*23000015*/]System.Diagnostics.Tracing.EventAttribute/*01000083*/::.ctor(int32) /* 0A00002D */ = ( 01 00 01 00 00 00 01 00 54 55 80 84 53 79 73 74   // ........TU..Syst
                                                                                                                                                                               65 6D 2E 44 69 61 67 6E 6F 73 74 69 63 73 2E 54   // em.Diagnostics.T
                                                                                                                                                                               72 61 63 69 6E 67 2E 45 76 65 6E 74 4C 65 76 65   // racing.EventLeve
                                                                                                                                                                               6C 2C 20 53 79 73 74 65 6D 2E 44 69 61 67 6E 6F   // l, System.Diagno
                                                                                                                                                                               73 74 69 63 73 2E 54 72 61 63 69 6E 67 2C 20 56   // stics.Tracing, V
                                                                                                                                                                               65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20   // ersion=6.0.0.0, 
                                                                                                                                                                               43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C   // Culture=neutral,
                                                                                                                                                                               20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D   //  PublicKeyToken=
                                                                                                                                                                               62 30 33 66 35 66 37 66 31 31 64 35 30 61 33 61   // b03f5f7f11d50a3a
                                                                                                                                                                               05 4C 65 76 65 6C 04 00 00 00 )                   // .Level....
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEventSource::HostStart

  .method /*060000DB*/ public hidebysig instance void 
          HostStop() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0001B8:0A00002D*/ instance void [System.Diagnostics.Tracing/*23000015*/]System.Diagnostics.Tracing.EventAttribute/*01000083*/::.ctor(int32) /* 0A00002D */ = ( 01 00 02 00 00 00 01 00 54 55 80 84 53 79 73 74   // ........TU..Syst
                                                                                                                                                                               65 6D 2E 44 69 61 67 6E 6F 73 74 69 63 73 2E 54   // em.Diagnostics.T
                                                                                                                                                                               72 61 63 69 6E 67 2E 45 76 65 6E 74 4C 65 76 65   // racing.EventLeve
                                                                                                                                                                               6C 2C 20 53 79 73 74 65 6D 2E 44 69 61 67 6E 6F   // l, System.Diagno
                                                                                                                                                                               73 74 69 63 73 2E 54 72 61 63 69 6E 67 2C 20 56   // stics.Tracing, V
                                                                                                                                                                               65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20   // ersion=6.0.0.0, 
                                                                                                                                                                               43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C   // Culture=neutral,
                                                                                                                                                                               20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D   //  PublicKeyToken=
                                                                                                                                                                               62 30 33 66 35 66 37 66 31 31 64 35 30 61 33 61   // b03f5f7f11d50a3a
                                                                                                                                                                               05 4C 65 76 65 6C 04 00 00 00 )                   // .Level....
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEventSource::HostStop

  .method /*060000DC*/ public hidebysig instance void 
          RequestStart(string 'method',
                       string path) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .custom /*0C0001B9:0A00002D*/ instance void [System.Diagnostics.Tracing/*23000015*/]System.Diagnostics.Tracing.EventAttribute/*01000083*/::.ctor(int32) /* 0A00002D */ = ( 01 00 03 00 00 00 01 00 54 55 80 84 53 79 73 74   // ........TU..Syst
                                                                                                                                                                               65 6D 2E 44 69 61 67 6E 6F 73 74 69 63 73 2E 54   // em.Diagnostics.T
                                                                                                                                                                               72 61 63 69 6E 67 2E 45 76 65 6E 74 4C 65 76 65   // racing.EventLeve
                                                                                                                                                                               6C 2C 20 53 79 73 74 65 6D 2E 44 69 61 67 6E 6F   // l, System.Diagno
                                                                                                                                                                               73 74 69 63 73 2E 54 72 61 63 69 6E 67 2C 20 56   // stics.Tracing, V
                                                                                                                                                                               65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20   // ersion=6.0.0.0, 
                                                                                                                                                                               43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C   // Culture=neutral,
                                                                                                                                                                               20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D   //  PublicKeyToken=
                                                                                                                                                                               62 30 33 66 35 66 37 66 31 31 64 35 30 61 33 61   // b03f5f7f11d50a3a
                                                                                                                                                                               05 4C 65 76 65 6C 04 00 00 00 )                   // .Level....
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEventSource::RequestStart

  .method /*060000DD*/ public hidebysig instance void 
          RequestStop() cil managed noinlining
  // SIG: 20 00 01
  {
    .custom /*0C0001BA:0A00002D*/ instance void [System.Diagnostics.Tracing/*23000015*/]System.Diagnostics.Tracing.EventAttribute/*01000083*/::.ctor(int32) /* 0A00002D */ = ( 01 00 04 00 00 00 01 00 54 55 80 84 53 79 73 74   // ........TU..Syst
                                                                                                                                                                               65 6D 2E 44 69 61 67 6E 6F 73 74 69 63 73 2E 54   // em.Diagnostics.T
                                                                                                                                                                               72 61 63 69 6E 67 2E 45 76 65 6E 74 4C 65 76 65   // racing.EventLeve
                                                                                                                                                                               6C 2C 20 53 79 73 74 65 6D 2E 44 69 61 67 6E 6F   // l, System.Diagno
                                                                                                                                                                               73 74 69 63 73 2E 54 72 61 63 69 6E 67 2C 20 56   // stics.Tracing, V
                                                                                                                                                                               65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20   // ersion=6.0.0.0, 
                                                                                                                                                                               43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C   // Culture=neutral,
                                                                                                                                                                               20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D   //  PublicKeyToken=
                                                                                                                                                                               62 30 33 66 35 66 37 66 31 31 64 35 30 61 33 61   // b03f5f7f11d50a3a
                                                                                                                                                                               05 4C 65 76 65 6C 04 00 00 00 )                   // .Level....
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEventSource::RequestStop

  .method /*060000DE*/ public hidebysig instance void 
          UnhandledException() cil managed noinlining
  // SIG: 20 00 01
  {
    .custom /*0C0001BC:0A00002D*/ instance void [System.Diagnostics.Tracing/*23000015*/]System.Diagnostics.Tracing.EventAttribute/*01000083*/::.ctor(int32) /* 0A00002D */ = ( 01 00 05 00 00 00 01 00 54 55 80 84 53 79 73 74   // ........TU..Syst
                                                                                                                                                                               65 6D 2E 44 69 61 67 6E 6F 73 74 69 63 73 2E 54   // em.Diagnostics.T
                                                                                                                                                                               72 61 63 69 6E 67 2E 45 76 65 6E 74 4C 65 76 65   // racing.EventLeve
                                                                                                                                                                               6C 2C 20 53 79 73 74 65 6D 2E 44 69 61 67 6E 6F   // l, System.Diagno
                                                                                                                                                                               73 74 69 63 73 2E 54 72 61 63 69 6E 67 2C 20 56   // stics.Tracing, V
                                                                                                                                                                               65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20   // ersion=6.0.0.0, 
                                                                                                                                                                               43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C   // Culture=neutral,
                                                                                                                                                                               20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D   //  PublicKeyToken=
                                                                                                                                                                               62 30 33 66 35 66 37 66 31 31 64 35 30 61 33 61   // b03f5f7f11d50a3a
                                                                                                                                                                               05 4C 65 76 65 6C 02 00 00 00 )                   // .Level....
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEventSource::UnhandledException

  .method /*060000DF*/ assembly hidebysig 
          instance void  RequestFailed() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEventSource::RequestFailed

  .method /*060000E0*/ family hidebysig virtual 
          instance void  OnEventCommand(class [System.Diagnostics.Tracing/*23000015*/]System.Diagnostics.Tracing.EventCommandEventArgs/*01000085*/ command) cil managed
  // SIG: 20 01 01 12 82 15
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEventSource::OnEventCommand

} // end of class Microsoft.AspNetCore.Hosting.HostingEventSource

.class /*02000028*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Hosting.HostingLoggerExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000B7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000B8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000B9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*060000E1*/ public hidebysig static 
          void  ApplicationError(class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ logger,
                                 class [System.Runtime/*23000001*/]System.Exception/*0100001C*/ exception) cil managed
  // SIG: 00 02 01 12 7D 12 71
  {
    .custom /*0C0001BE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingLoggerExtensions::ApplicationError

  .method /*060000E2*/ public hidebysig static 
          void  HostingStartupAssemblyError(class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ logger,
                                            class [System.Runtime/*23000001*/]System.Exception/*0100001C*/ exception) cil managed
  // SIG: 00 02 01 12 7D 12 71
  {
    .custom /*0C0001C0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingLoggerExtensions::HostingStartupAssemblyError

  .method /*060000E3*/ public hidebysig static 
          void  ApplicationError(class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ logger,
                                 valuetype [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.EventId/*01000086*/ eventId,
                                 string message,
                                 class [System.Runtime/*23000001*/]System.Exception/*0100001C*/ exception) cil managed
  // SIG: 00 04 01 12 7D 11 82 19 0E 12 71
  {
    .custom /*0C0001C2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingLoggerExtensions::ApplicationError

} // end of class Microsoft.AspNetCore.Hosting.HostingLoggerExtensions

.class /*02000029*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.HostingRequestFinishedLog
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100007B*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B000009*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B00000A*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000024*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*0100007D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B00000B*/
{
  .custom /*0C0000BC:0A00002B*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*0100007A*/::.ctor(string) /* 0A00002B */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100007B*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B000009*/
  .custom /*0C00006C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B00000A*/
  .custom /*0C000071:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*0100007D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B00000B*/
  .custom /*0C000077:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .class /*0200007A*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000147:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000080*/ public static initonly class Microsoft.AspNetCore.Hosting.HostingRequestFinishedLog/*02000029*//'<>c'/*0200007A*/ '<>9'
    .method /*06000223*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000224*/ assembly hidebysig 
            instance string  '<.cctor>b__14_0'(object state,
                                               class [System.Runtime/*23000001*/]System.Exception/*0100001C*/ exception) cil managed
    // SIG: 20 02 0E 1C 12 71
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.cctor>b__14_0'

  } // end of class '<>c'

  .class /*0200007B*/ auto ansi sealed nested private beforefieldinit '<GetEnumerator>d__12'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B00000C*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000021*/
  {
    .custom /*0C000148:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000081*/ public class Microsoft.AspNetCore.Hosting.HostingRequestFinishedLog/*02000029*/ '<>4__this'
    .method /*06000225*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(int32 '<>1__state') cil managed
    // SIG: 20 01 01 08
    {
      .custom /*0C00026B:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetEnumerator>d__12'::.ctor

    .method /*06000226*/ private hidebysig newslot virtual final 
            instance void  System.IDisposable.Dispose() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C00026C:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.IDisposable/*01000021*/::Dispose /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetEnumerator>d__12'::System.IDisposable.Dispose

    .method /*06000227*/ private hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/::MoveNext /*01000022::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetEnumerator>d__12'::MoveNext

    .method /*06000228*/ private hidebysig newslot specialname virtual final 
            instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object> 
            'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Current'() cil managed
    // SIG: 20 00 15 11 81 F1 02 0E 1C
    {
      .custom /*0C00026D:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override  method instance !0 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B00000C*/::get_Current() /* 0A00002C */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetEnumerator>d__12'::'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Current'

    .method /*06000229*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C00026E:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/::Reset /*01000022::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetEnumerator>d__12'::System.Collections.IEnumerator.Reset

    .method /*0600022A*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .custom /*0C00026F:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/::get_Current /*01000022::0A00001B*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetEnumerator>d__12'::System.Collections.IEnumerator.get_Current

    .property /*17000091*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>
            'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Current'()
    {
      .get instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object> Microsoft.AspNetCore.Hosting.HostingRequestFinishedLog/*02000029*//'<GetEnumerator>d__12'/*0200007B*/::'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Current'() /* 06000228 */
    } // end of property '<GetEnumerator>d__12'::'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Current'
    .property /*17000092*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object Microsoft.AspNetCore.Hosting.HostingRequestFinishedLog/*02000029*//'<GetEnumerator>d__12'/*0200007B*/::System.Collections.IEnumerator.get_Current() /* 0600022A */
    } // end of property '<GetEnumerator>d__12'::System.Collections.IEnumerator.Current
  } // end of class '<GetEnumerator>d__12'

  .field /*04000006*/ static assembly initonly class [System.Runtime/*23000001*/]System.Func`3/*01000087*/<object,class [System.Runtime/*23000001*/]System.Exception/*0100001C*/,string> Callback
  .custom /*0C00002D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 01 02 01 00 00 ) 
  .method /*060000E4*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000088*/ 
          get_Elapsed() cil managed
  // SIG: 20 00 11 82 21
  {
    .custom /*0C0001C4:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingRequestFinishedLog::get_Elapsed

  .method /*060000E5*/ public hidebysig newslot specialname virtual final 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingRequestFinishedLog::get_Count

  .method /*060000E6*/ public hidebysig newslot specialname virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object> 
          get_Item(int32 index) cil managed
  // SIG: 20 01 15 11 81 F1 02 0E 1C 08
  {
    .param [0]/*080000D9*/ 
    .custom /*0C0001B6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingRequestFinishedLog::get_Item

  .method /*060000E7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Hosting.HostingApplication/*02000023*//Context/*02000077*/ context,
                               valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000088*/ elapsed) cil managed
  // SIG: 20 02 01 12 81 DC 11 82 21
  {
    .custom /*0C0001C6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingRequestFinishedLog::.ctor

  .method /*060000E8*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001C8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingRequestFinishedLog::ToString

  .method /*060000E9*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>> 
          GetEnumerator() cil managed
  // SIG: 20 00 15 12 80 8D 01 15 11 81 F1 02 0E 1C
  {
    .custom /*0C0001C9:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IteratorStateMachineAttribute/*01000020*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000019*/) /* 0A000016 */ = ( 01 00 4B 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..KMicrosoft.Asp
                                                                                                                                                                                                                                           4E 65 74 43 6F 72 65 2E 48 6F 73 74 69 6E 67 2E   // NetCore.Hosting.
                                                                                                                                                                                                                                           48 6F 73 74 69 6E 67 52 65 71 75 65 73 74 46 69   // HostingRequestFi
                                                                                                                                                                                                                                           6E 69 73 68 65 64 4C 6F 67 2B 3C 47 65 74 45 6E   // nishedLog+<GetEn
                                                                                                                                                                                                                                           75 6D 65 72 61 74 6F 72 3E 64 5F 5F 31 32 00 00 ) // umerator>d__12..
    .param [0]/*080000DD*/ 
    .custom /*0C0001BB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingRequestFinishedLog::GetEnumerator

  .method /*060000EA*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 80 89
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000024*/::GetEnumerator /*01000024::0A00001D*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingRequestFinishedLog::System.Collections.IEnumerable.GetEnumerator

  .property /*17000047*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000088*/
          Elapsed()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000088*/ Microsoft.AspNetCore.Hosting.HostingRequestFinishedLog/*02000029*/::get_Elapsed() /* 060000E4 */
  } // end of property HostingRequestFinishedLog::Elapsed
  .property /*17000048*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 Microsoft.AspNetCore.Hosting.HostingRequestFinishedLog/*02000029*/::get_Count() /* 060000E5 */
  } // end of property HostingRequestFinishedLog::Count
  .property /*17000049*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>
          Item(int32)
  {
    .custom /*0C000109:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object> Microsoft.AspNetCore.Hosting.HostingRequestFinishedLog/*02000029*/::get_Item(int32) /* 060000E6 */
  } // end of property HostingRequestFinishedLog::Item
} // end of class Microsoft.AspNetCore.Hosting.HostingRequestFinishedLog

.class /*0200002A*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.HostingRequestStartingLog
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100007B*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B000009*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B00000A*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000024*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*0100007D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B00000B*/
{
  .custom /*0C0000BF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000C0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000C1:0A00002B*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*0100007A*/::.ctor(string) /* 0A00002B */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100007B*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B000009*/
  .custom /*0C00007E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B00000A*/
  .custom /*0C000083:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000024*/
  .custom /*0C000088:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*0100007D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B00000B*/
  .custom /*0C00008B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .class /*0200007C*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000149:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000082*/ public static initonly class Microsoft.AspNetCore.Hosting.HostingRequestStartingLog/*0200002A*//'<>c'/*0200007C*/ '<>9'
    .method /*0600022B*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*0600022C*/ assembly hidebysig 
            instance string  '<.cctor>b__16_0'(object state,
                                               class [System.Runtime/*23000001*/]System.Exception/*0100001C*/ exception) cil managed
    // SIG: 20 02 0E 1C 12 71
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.cctor>b__16_0'

  } // end of class '<>c'

  .class /*0200007D*/ auto ansi sealed nested private beforefieldinit '<GetEnumerator>d__11'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B00000C*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000021*/
  {
    .custom /*0C00014A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000083*/ public class Microsoft.AspNetCore.Hosting.HostingRequestStartingLog/*0200002A*/ '<>4__this'
    .method /*0600022D*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(int32 '<>1__state') cil managed
    // SIG: 20 01 01 08
    {
      .custom /*0C000270:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetEnumerator>d__11'::.ctor

    .method /*0600022E*/ private hidebysig newslot virtual final 
            instance void  System.IDisposable.Dispose() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C000271:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.IDisposable/*01000021*/::Dispose /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetEnumerator>d__11'::System.IDisposable.Dispose

    .method /*0600022F*/ private hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/::MoveNext /*01000022::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetEnumerator>d__11'::MoveNext

    .method /*06000230*/ private hidebysig newslot specialname virtual final 
            instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object> 
            'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Current'() cil managed
    // SIG: 20 00 15 11 81 F1 02 0E 1C
    {
      .custom /*0C000272:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override  method instance !0 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>>/*1B00000C*/::get_Current() /* 0A00002C */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetEnumerator>d__11'::'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Current'

    .method /*06000231*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C000273:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/::Reset /*01000022::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetEnumerator>d__11'::System.Collections.IEnumerator.Reset

    .method /*06000232*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .custom /*0C000274:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/::get_Current /*01000022::0A00001B*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetEnumerator>d__11'::System.Collections.IEnumerator.get_Current

    .property /*17000093*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>
            'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Current'()
    {
      .get instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object> Microsoft.AspNetCore.Hosting.HostingRequestStartingLog/*0200002A*//'<GetEnumerator>d__11'/*0200007D*/::'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Current'() /* 06000230 */
    } // end of property '<GetEnumerator>d__11'::'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Current'
    .property /*17000094*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object Microsoft.AspNetCore.Hosting.HostingRequestStartingLog/*0200002A*//'<GetEnumerator>d__11'/*0200007D*/::System.Collections.IEnumerator.get_Current() /* 06000232 */
    } // end of property '<GetEnumerator>d__11'::System.Collections.IEnumerator.Current
  } // end of class '<GetEnumerator>d__11'

  .field /*04000007*/ static assembly initonly class [System.Runtime/*23000001*/]System.Func`3/*01000087*/<object,class [System.Runtime/*23000001*/]System.Exception/*0100001C*/,string> Callback
  .custom /*0C000030:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 01 02 01 00 00 ) 
  .method /*060000EB*/ public hidebysig newslot specialname virtual final 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingRequestStartingLog::get_Count

  .method /*060000EC*/ public hidebysig newslot specialname virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object> 
          get_Item(int32 index) cil managed
  // SIG: 20 01 15 11 81 F1 02 0E 1C 08
  {
    .param [0]/*080000DE*/ 
    .custom /*0C0001BD:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingRequestStartingLog::get_Item

  .method /*060000ED*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000031*/ httpContext) cil managed
  // SIG: 20 01 01 12 80 C5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingRequestStartingLog::.ctor

  .method /*060000EE*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingRequestStartingLog::ToString

  .method /*060000EF*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>> 
          GetEnumerator() cil managed
  // SIG: 20 00 15 12 80 8D 01 15 11 81 F1 02 0E 1C
  {
    .custom /*0C0001CC:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IteratorStateMachineAttribute/*01000020*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000019*/) /* 0A000016 */ = ( 01 00 4B 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..KMicrosoft.Asp
                                                                                                                                                                                                                                           4E 65 74 43 6F 72 65 2E 48 6F 73 74 69 6E 67 2E   // NetCore.Hosting.
                                                                                                                                                                                                                                           48 6F 73 74 69 6E 67 52 65 71 75 65 73 74 53 74   // HostingRequestSt
                                                                                                                                                                                                                                           61 72 74 69 6E 67 4C 6F 67 2B 3C 47 65 74 45 6E   // artingLog+<GetEn
                                                                                                                                                                                                                                           75 6D 65 72 61 74 6F 72 3E 64 5F 5F 31 31 00 00 ) // umerator>d__11..
    .param [0]/*080000E1*/ 
    .custom /*0C0001BF:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingRequestStartingLog::GetEnumerator

  .method /*060000F0*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000022*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 80 89
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000024*/::GetEnumerator /*01000024::0A00001D*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingRequestStartingLog::System.Collections.IEnumerable.GetEnumerator

  .method /*060000F1*/ assembly hidebysig 
          instance string  ToStringWithoutPreamble() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingRequestStartingLog::ToStringWithoutPreamble

  .method /*060000F2*/ assembly hidebysig static 
          string  EscapedValueOrEmptyMarker(string potentialValue) cil managed
  // SIG: 00 01 0E 0E
  {
    .param [1]/*080000E2*/ 
    .custom /*0C0001C1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingRequestStartingLog::EscapedValueOrEmptyMarker

  .method /*060000F3*/ assembly hidebysig static 
          string  ValueOrEmptyMarker<valuetype .ctor ([System.Runtime/*23000001*/]System.IFormattable/*01000089*/, [System.Runtime/*23000001*/]System.ValueType/*0100001B*/) T>(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100008A*/<!!T> potentialValue) cil managed
  // SIG: 10 01 01 0E 15 11 82 29 01 1E 00
  {
    .custom /*0C0001CF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A00000D*/ , [System.Runtime/*23000001*/]System.IFormattable/*01000089*/
      .custom /*0C000019:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [0]/*080000E3*/ 
    .custom /*0C0001C3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingRequestStartingLog::ValueOrEmptyMarker

  .property /*1700004A*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 Microsoft.AspNetCore.Hosting.HostingRequestStartingLog/*0200002A*/::get_Count() /* 060000EB */
  } // end of property HostingRequestStartingLog::Count
  .property /*1700004B*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object>
          Item(int32)
  {
    .custom /*0C00010E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100007C*/<string,object> Microsoft.AspNetCore.Hosting.HostingRequestStartingLog/*0200002A*/::get_Item(int32) /* 060000EC */
  } // end of property HostingRequestStartingLog::Item
} // end of class Microsoft.AspNetCore.Hosting.HostingRequestStartingLog

.class /*0200002B*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Hosting.LoggerEventIds
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .field /*04000008*/ public static literal int32 RequestStarting = int32(0x00000001)
  .field /*04000009*/ public static literal int32 RequestFinished = int32(0x00000002)
  .field /*0400000A*/ public static literal int32 Starting = int32(0x00000003)
  .field /*0400000B*/ public static literal int32 Started = int32(0x00000004)
  .field /*0400000C*/ public static literal int32 Shutdown = int32(0x00000005)
  .field /*0400000D*/ public static literal int32 ApplicationStartupException = int32(0x00000006)
  .field /*0400000E*/ public static literal int32 ApplicationStoppingException = int32(0x00000007)
  .field /*0400000F*/ public static literal int32 ApplicationStoppedException = int32(0x00000008)
  .field /*04000010*/ public static literal int32 HostedServiceStartException = int32(0x00000009)
  .field /*04000011*/ public static literal int32 HostedServiceStopException = int32(0x0000000A)
  .field /*04000012*/ public static literal int32 HostingStartupAssemblyException = int32(0x0000000B)
  .field /*04000013*/ public static literal int32 ServerShutdownException = int32(0x0000000C)
  .field /*04000014*/ public static literal int32 HostingStartupAssemblyLoaded = int32(0x0000000D)
  .field /*04000015*/ public static literal int32 ServerListeningOnAddresses = int32(0x0000000E)
} // end of class Microsoft.AspNetCore.Hosting.LoggerEventIds

.class /*0200002C*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Hosting.MethodInfoExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000C4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*060000F4*/ public hidebysig static 
          object  InvokeWithoutWrappingExceptions(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000076*/ methodInfo,
                                                  object obj,
                                                  object[] parameters) cil managed
  // SIG: 00 03 1C 12 81 D9 1C 1D 1C
  {
    .custom /*0C0001D1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001D2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*080000E5*/ 
    .custom /*0C0001C5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*080000E7*/ 
    .custom /*0C0001C7:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodInfoExtensions::InvokeWithoutWrappingExceptions

} // end of class Microsoft.AspNetCore.Hosting.MethodInfoExtensions

.class /*0200002D*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Hosting.ServiceCollectionExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000C6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*060000F5*/ public hidebysig static 
          class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ 
          Clone(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ serviceCollection) cil managed
  // SIG: 00 01 12 81 19 12 81 19
  {
    .custom /*0C0001D3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001D4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServiceCollectionExtensions::Clone

} // end of class Microsoft.AspNetCore.Hosting.ServiceCollectionExtensions

.class /*0200002E*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.StartupLoader
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000C8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000C9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200007E*/ abstract auto ansi nested private beforefieldinit ConfigureServicesDelegateBuilder
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .method /*06000233*/ public hidebysig newslot abstract virtual 
            instance class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/> 
            Build() cil managed
    // SIG: 20 00 15 12 80 A9 02 12 81 19 12 81 59
    {
      // Method begins at RVA 0x0
    } // end of method ConfigureServicesDelegateBuilder::Build

    .method /*06000234*/ family hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ConfigureServicesDelegateBuilder::.ctor

  } // end of class ConfigureServicesDelegateBuilder

  .class /*0200007F*/ auto ansi nested private beforefieldinit ConfigureServicesDelegateBuilder`1<TContainerBuilder>
         extends Microsoft.AspNetCore.Hosting.StartupLoader/*0200002E*//ConfigureServicesDelegateBuilder/*0200007E*/
  {
    .class /*020000A3*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass13_0'<TContainerBuilder>
           extends [System.Runtime/*23000001*/]System.Object/*01000017*/
    {
      .custom /*0C000191:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      .field /*040000EA*/ public class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<object> action
      .field /*040000EB*/ public class Microsoft.AspNetCore.Hosting.StartupLoader/*0200002E*//ConfigureServicesDelegateBuilder`1/*0200007F*/<!TContainerBuilder> '<>4__this'
      .method /*06000293*/ public hidebysig specialname rtspecialname 
              instance void  .ctor() cil managed
      // SIG: 20 00 01
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method '<>c__DisplayClass13_0'::.ctor

      .method /*06000294*/ assembly hidebysig 
              instance void  '<Build>g__Source|1'(!TContainerBuilder containerBuilder) cil managed
      // SIG: 20 01 01 13 00
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method '<>c__DisplayClass13_0'::'<Build>g__Source|1'

      .method /*06000295*/ assembly hidebysig 
              instance void  '<Build>g__Target|2'(object containerBuilder) cil managed
      // SIG: 20 01 01 1C
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method '<>c__DisplayClass13_0'::'<Build>g__Target|2'

    } // end of class '<>c__DisplayClass13_0'

    .class /*020000A4*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass14_0'<TContainerBuilder>
           extends [System.Runtime/*23000001*/]System.Object/*01000017*/
    {
      .custom /*0C000193:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      .field /*040000EC*/ public class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/> configureServicesCallback
      .field /*040000ED*/ public class Microsoft.AspNetCore.Hosting.StartupLoader/*0200002E*//ConfigureServicesDelegateBuilder`1/*0200007F*/<!TContainerBuilder> '<>4__this'
      .field /*040000EE*/ public class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<object> configureContainerCallback
      .method /*06000296*/ public hidebysig specialname rtspecialname 
              instance void  .ctor() cil managed
      // SIG: 20 00 01
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method '<>c__DisplayClass14_0'::.ctor

      .method /*06000297*/ assembly hidebysig 
              instance class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ 
              '<ConfigureServices>g__ConfigureServicesWithContainerConfiguration|0'(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
      // SIG: 20 01 12 81 59 12 81 19
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method '<>c__DisplayClass14_0'::'<ConfigureServices>g__ConfigureServicesWithContainerConfiguration|0'

    } // end of class '<>c__DisplayClass14_0'

    .class /*020000A5*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass15_0'<TContainerBuilder>
           extends [System.Runtime/*23000001*/]System.Object/*01000017*/
    {
      .custom /*0C000194:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      .field /*040000EF*/ public class Microsoft.AspNetCore.Hosting.StartupLoader/*0200002E*//ConfigureServicesDelegateBuilder`1/*0200007F*/<!TContainerBuilder> '<>4__this'
      .field /*040000F0*/ public class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/> startup
      .method /*06000298*/ public hidebysig specialname rtspecialname 
              instance void  .ctor() cil managed
      // SIG: 20 00 01
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method '<>c__DisplayClass15_0'::.ctor

      .method /*06000299*/ assembly hidebysig 
              instance class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ 
              '<BuildStartupServicesFilterPipeline>g__RunPipeline|0'(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
      // SIG: 20 01 12 81 59 12 81 19
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method '<>c__DisplayClass15_0'::'<BuildStartupServicesFilterPipeline>g__RunPipeline|0'

    } // end of class '<>c__DisplayClass15_0'

    .class /*020000A6*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass15_1'<TContainerBuilder>
           extends [System.Runtime/*23000001*/]System.Object/*01000017*/
    {
      .custom /*0C000196:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      .field /*040000F1*/ public class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IStartupConfigureServicesFilter/*0100008C*/[] filters
      .field /*040000F2*/ public class Microsoft.AspNetCore.Hosting.StartupLoader/*0200002E*//ConfigureServicesDelegateBuilder`1/*0200007F*//'<>c__DisplayClass15_0'/*020000A5*/<!TContainerBuilder> 'CS$<>8__locals1'
      .method /*0600029A*/ public hidebysig specialname rtspecialname 
              instance void  .ctor() cil managed
      // SIG: 20 00 01
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method '<>c__DisplayClass15_1'::.ctor

      .method /*0600029B*/ assembly hidebysig 
              instance void  '<BuildStartupServicesFilterPipeline>g__InvokeStartup|1'(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ serviceCollection) cil managed
      // SIG: 20 01 01 12 81 19
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method '<>c__DisplayClass15_1'::'<BuildStartupServicesFilterPipeline>g__InvokeStartup|1'

    } // end of class '<>c__DisplayClass15_1'

    .class /*020000A7*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass16_0'<TContainerBuilder>
           extends [System.Runtime/*23000001*/]System.Object/*01000017*/
    {
      .custom /*0C000198:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      .field /*040000F3*/ public class Microsoft.AspNetCore.Hosting.StartupLoader/*0200002E*//ConfigureServicesDelegateBuilder`1/*0200007F*/<!TContainerBuilder> '<>4__this'
      .field /*040000F4*/ public class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<!TContainerBuilder> configureContainer
      .method /*0600029C*/ public hidebysig specialname rtspecialname 
              instance void  .ctor() cil managed
      // SIG: 20 00 01
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method '<>c__DisplayClass16_0'::.ctor

      .method /*0600029D*/ assembly hidebysig 
              instance void  '<BuildStartupConfigureContainerFiltersPipeline>g__RunPipeline|0'(!TContainerBuilder containerBuilder) cil managed
      // SIG: 20 01 01 13 00
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method '<>c__DisplayClass16_0'::'<BuildStartupConfigureContainerFiltersPipeline>g__RunPipeline|0'

      .method /*0600029E*/ assembly hidebysig 
              instance void  '<BuildStartupConfigureContainerFiltersPipeline>g__InvokeConfigureContainer|1'(!TContainerBuilder builder) cil managed
      // SIG: 20 01 01 13 00
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method '<>c__DisplayClass16_0'::'<BuildStartupConfigureContainerFiltersPipeline>g__InvokeConfigureContainer|1'

    } // end of class '<>c__DisplayClass16_0'

    .method /*06000235*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ hostingServiceProvider,
                                 class Microsoft.AspNetCore.Hosting.ConfigureServicesBuilder/*02000020*/ configureServicesBuilder,
                                 class Microsoft.AspNetCore.Hosting.ConfigureContainerBuilder/*0200001F*/ configureContainerBuilder,
                                 object 'instance') cil managed
    // SIG: 20 04 01 12 81 59 12 80 80 12 7C 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ConfigureServicesDelegateBuilder`1::.ctor

    .method /*06000236*/ public hidebysig specialname 
            instance class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ 
            get_HostingServiceProvider() cil managed
    // SIG: 20 00 12 81 59
    {
      .custom /*0C000275:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ConfigureServicesDelegateBuilder`1::get_HostingServiceProvider

    .method /*06000237*/ public hidebysig specialname 
            instance class Microsoft.AspNetCore.Hosting.ConfigureServicesBuilder/*02000020*/ 
            get_ConfigureServicesBuilder() cil managed
    // SIG: 20 00 12 80 80
    {
      .custom /*0C000276:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ConfigureServicesDelegateBuilder`1::get_ConfigureServicesBuilder

    .method /*06000238*/ public hidebysig specialname 
            instance class Microsoft.AspNetCore.Hosting.ConfigureContainerBuilder/*0200001F*/ 
            get_ConfigureContainerBuilder() cil managed
    // SIG: 20 00 12 7C
    {
      .custom /*0C000277:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ConfigureServicesDelegateBuilder`1::get_ConfigureContainerBuilder

    .method /*06000239*/ public hidebysig specialname 
            instance object  get_Instance() cil managed
    // SIG: 20 00 1C
    {
      .custom /*0C000278:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ConfigureServicesDelegateBuilder`1::get_Instance

    .method /*0600023A*/ public hidebysig virtual 
            instance class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/> 
            Build() cil managed
    // SIG: 20 00 15 12 80 A9 02 12 81 19 12 81 59
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ConfigureServicesDelegateBuilder`1::Build

    .property /*17000095*/ instance callconv(8) class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/
            HostingServiceProvider()
    {
      .get instance class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ Microsoft.AspNetCore.Hosting.StartupLoader/*0200002E*//ConfigureServicesDelegateBuilder`1/*0200007F*/::get_HostingServiceProvider() /* 06000236 */
    } // end of property ConfigureServicesDelegateBuilder`1::HostingServiceProvider
    .property /*17000096*/ instance callconv(8) class Microsoft.AspNetCore.Hosting.ConfigureServicesBuilder/*02000020*/
            ConfigureServicesBuilder()
    {
      .get instance class Microsoft.AspNetCore.Hosting.ConfigureServicesBuilder/*02000020*/ Microsoft.AspNetCore.Hosting.StartupLoader/*0200002E*//ConfigureServicesDelegateBuilder`1/*0200007F*/::get_ConfigureServicesBuilder() /* 06000237 */
    } // end of property ConfigureServicesDelegateBuilder`1::ConfigureServicesBuilder
    .property /*17000097*/ instance callconv(8) class Microsoft.AspNetCore.Hosting.ConfigureContainerBuilder/*0200001F*/
            ConfigureContainerBuilder()
    {
      .get instance class Microsoft.AspNetCore.Hosting.ConfigureContainerBuilder/*0200001F*/ Microsoft.AspNetCore.Hosting.StartupLoader/*0200002E*//ConfigureServicesDelegateBuilder`1/*0200007F*/::get_ConfigureContainerBuilder() /* 06000238 */
    } // end of property ConfigureServicesDelegateBuilder`1::ConfigureContainerBuilder
    .property /*17000098*/ instance callconv(8) object
            Instance()
    {
      .get instance object Microsoft.AspNetCore.Hosting.StartupLoader/*0200002E*//ConfigureServicesDelegateBuilder`1/*0200007F*/::get_Instance() /* 06000239 */
    } // end of property ConfigureServicesDelegateBuilder`1::Instance
  } // end of class ConfigureServicesDelegateBuilder`1

  .class /*02000080*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass3_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00014B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000084*/ public string startupNameWithEnv
    .field /*04000085*/ public string startupNameWithoutEnv
    .method /*0600023B*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass3_0'::.ctor

    .method /*0600023C*/ assembly hidebysig 
            instance bool  '<FindStartupType>b__0'(class [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*0100008D*/ info) cil managed
    // SIG: 20 01 02 12 82 35
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass3_0'::'<FindStartupType>b__0'

    .method /*0600023D*/ assembly hidebysig 
            instance bool  '<FindStartupType>b__1'(class [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*0100008D*/ info) cil managed
    // SIG: 20 01 02 12 82 35
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass3_0'::'<FindStartupType>b__1'

  } // end of class '<>c__DisplayClass3_0'

  .class /*02000081*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass8_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00014D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000086*/ public string methodNameWithEnv
    .field /*04000087*/ public string methodNameWithNoEnv
    .method /*0600023E*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass8_0'::.ctor

    .method /*0600023F*/ assembly hidebysig 
            instance bool  '<FindMethod>b__0'(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000076*/ 'method') cil managed
    // SIG: 20 01 02 12 81 D9
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass8_0'::'<FindMethod>b__0'

    .method /*06000240*/ assembly hidebysig 
            instance bool  '<FindMethod>b__1'(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000076*/ 'method') cil managed
    // SIG: 20 01 02 12 81 D9
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass8_0'::'<FindMethod>b__1'

  } // end of class '<>c__DisplayClass8_0'

  .method /*060000F6*/ public hidebysig static 
          class Microsoft.AspNetCore.Hosting.StartupMethods/*0200002F*/ 
          LoadMethods(class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ hostingServiceProvider,
                      class [System.Runtime/*23000001*/]System.Type/*01000019*/ startupType,
                      string environmentName,
                      [opt] object 'instance') cil managed
  // SIG: 00 04 12 80 BC 12 81 59 12 65 0E 1C
  {
    .param [2]/*080000EA*/ 
    .custom /*0C0001CA:0A00002A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000062*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000061*/) /* 0A00002A */ = ( 01 00 0B 00 00 00 00 00 ) 
    .param [4]/*080000EC*/  = nullref
    .custom /*0C0001CB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StartupLoader::LoadMethods

  .method /*060000F7*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Type/*01000019*/ 
          FindStartupType(string startupAssemblyName,
                          string environmentName) cil managed
  // SIG: 00 02 12 65 0E 0E
  {
    .custom /*0C0001D6:0A00002E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessageAttribute/*0100008B*/::.ctor(string,
                                                                                                                                                                     string) /* 0A00002E */ = ( 01 00 12 52 65 66 6C 65 63 74 69 6F 6E 41 6E 61   // ...ReflectionAna
                                                                                                                                                                                                6C 79 73 69 73 1F 49 4C 32 30 32 36 3A 52 65 71   // lysis.IL2026:Req
                                                                                                                                                                                                75 69 72 65 73 55 6E 72 65 66 65 72 65 6E 63 65   // uiresUnreference
                                                                                                                                                                                                64 43 6F 64 65 01 00 54 0E 0D 4A 75 73 74 69 66   // dCode..T..Justif
                                                                                                                                                                                                69 63 61 74 69 6F 6E 43 57 65 27 72 65 20 77 61   // icationCWe're wa
                                                                                                                                                                                                72 6E 69 6E 67 20 61 74 20 74 68 65 20 65 6E 74   // rning at the ent
                                                                                                                                                                                                72 79 20 70 6F 69 6E 74 2E 20 54 68 69 73 20 69   // ry point. This i
                                                                                                                                                                                                73 20 61 6E 20 69 6D 70 6C 65 6D 65 6E 74 61 74   // s an implementat
                                                                                                                                                                                                69 6F 6E 20 64 65 74 61 69 6C 2E )                // ion detail.
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StartupLoader::FindStartupType

  .method /*060000F8*/ assembly hidebysig static 
          class Microsoft.AspNetCore.Hosting.ConfigureBuilder/*0200001E*/ 
          FindConfigureDelegate(class [System.Runtime/*23000001*/]System.Type/*01000019*/ startupType,
                                string environmentName) cil managed
  // SIG: 00 02 12 78 12 65 0E
  {
    .param [1]/*080000EF*/ 
    .custom /*0C0001CD:0A00002A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000062*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000061*/) /* 0A00002A */ = ( 01 00 0B 00 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StartupLoader::FindConfigureDelegate

  .method /*060000F9*/ assembly hidebysig static 
          class Microsoft.AspNetCore.Hosting.ConfigureContainerBuilder/*0200001F*/ 
          FindConfigureContainerDelegate(class [System.Runtime/*23000001*/]System.Type/*01000019*/ startupType,
                                         string environmentName) cil managed
  // SIG: 00 02 12 7C 12 65 0E
  {
    .param [1]/*080000F1*/ 
    .custom /*0C0001CE:0A00002A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000062*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000061*/) /* 0A00002A */ = ( 01 00 0B 00 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StartupLoader::FindConfigureContainerDelegate

  .method /*060000FA*/ assembly hidebysig static 
          bool  HasConfigureServicesIServiceProviderDelegate(class [System.Runtime/*23000001*/]System.Type/*01000019*/ startupType,
                                                             string environmentName) cil managed
  // SIG: 00 02 02 12 65 0E
  {
    .param [1]/*080000F3*/ 
    .custom /*0C0001D0:0A00002A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000062*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000061*/) /* 0A00002A */ = ( 01 00 0B 00 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StartupLoader::HasConfigureServicesIServiceProviderDelegate

  .method /*060000FB*/ assembly hidebysig static 
          class Microsoft.AspNetCore.Hosting.ConfigureServicesBuilder/*02000020*/ 
          FindConfigureServicesDelegate(class [System.Runtime/*23000001*/]System.Type/*01000019*/ startupType,
                                        string environmentName) cil managed
  // SIG: 00 02 12 80 80 12 65 0E
  {
    .param [1]/*080000F5*/ 
    .custom /*0C0001D5:0A00002A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000062*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000061*/) /* 0A00002A */ = ( 01 00 0B 00 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StartupLoader::FindConfigureServicesDelegate

  .method /*060000FC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StartupLoader::.ctor

} // end of class Microsoft.AspNetCore.Hosting.StartupLoader

.class /*0200002F*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.StartupMethods
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000CB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000CC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000FD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'instance',
                               class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> configure,
                               class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/> configureServices) cil managed
  // SIG: 20 03 01 1C 15 12 81 05 01 12 81 8D 15 12 80 A9 02 12 81 19 12 81 59
  {
    .param [1]/*080000F7*/ 
    .custom /*0C0001D7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StartupMethods::.ctor

  .method /*060000FE*/ public hidebysig specialname 
          instance object  get_StartupInstance() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C0001D8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001D9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StartupMethods::get_StartupInstance

  .method /*060000FF*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/> 
          get_ConfigureServicesDelegate() cil managed
  // SIG: 20 00 15 12 80 A9 02 12 81 19 12 81 59
  {
    .custom /*0C0001DB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StartupMethods::get_ConfigureServicesDelegate

  .method /*06000100*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> 
          get_ConfigureDelegate() cil managed
  // SIG: 20 00 15 12 81 05 01 12 81 8D
  {
    .custom /*0C0001DC:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StartupMethods::get_ConfigureDelegate

  .property /*1700004C*/ instance callconv(8) object
          StartupInstance()
  {
    .custom /*0C000111:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance object Microsoft.AspNetCore.Hosting.StartupMethods/*0200002F*/::get_StartupInstance() /* 060000FE */
  } // end of property StartupMethods::StartupInstance
  .property /*1700004D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/>
          ConfigureServicesDelegate()
  {
    .get instance class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/,class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/> Microsoft.AspNetCore.Hosting.StartupMethods/*0200002F*/::get_ConfigureServicesDelegate() /* 060000FF */
  } // end of property StartupMethods::ConfigureServicesDelegate
  .property /*1700004E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/>
          ConfigureDelegate()
  {
    .get instance class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> Microsoft.AspNetCore.Hosting.StartupMethods/*0200002F*/::get_ConfigureDelegate() /* 06000100 */
  } // end of property StartupMethods::ConfigureDelegate
} // end of class Microsoft.AspNetCore.Hosting.StartupMethods

.class /*02000030*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Hosting.WebHost
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHost/*0100005D*/,
                  [System.Runtime/*23000001*/]System.IDisposable/*01000021*/,
                  [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100008E*/
{
  .custom /*0C0000CE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000CF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHost/*0100005D*/
  .custom /*0C00008F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000021*/
  .custom /*0C000092:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100008E*/
  .custom /*0C000096:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000082*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .method /*06000241*/ public hidebysig static 
            void  Starting(class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ logger) cil managed
    // SIG: 00 01 01 12 7D
    {
      .custom /*0C000279:0A000021*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*01000028*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LogLevel/*01000027*/,
                                                                                                                                                                                string) /* 0A000021 */ = ( 01 00 03 00 00 00 01 00 00 00 10 48 6F 73 74 69   // ...........Hosti
                                                                                                                                                                                                           6E 67 20 73 74 61 72 74 69 6E 67 01 00 54 0E 09   // ng starting..T..
                                                                                                                                                                                                           45 76 65 6E 74 4E 61 6D 65 08 53 74 61 72 74 69   // EventName.Starti
                                                                                                                                                                                                           6E 67 )                                           // ng
      .custom /*0C00027A:0A000022*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*01000029*/::.ctor(string,
                                                                                                                                                string) /* 0A000022 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::Starting

    .method /*06000242*/ public hidebysig static 
            void  Started(class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ logger) cil managed
    // SIG: 00 01 01 12 7D
    {
      .custom /*0C00027B:0A000021*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*01000028*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LogLevel/*01000027*/,
                                                                                                                                                                                string) /* 0A000021 */ = ( 01 00 04 00 00 00 01 00 00 00 0F 48 6F 73 74 69   // ...........Hosti
                                                                                                                                                                                                           6E 67 20 73 74 61 72 74 65 64 01 00 54 0E 09 45   // ng started..T..E
                                                                                                                                                                                                           76 65 6E 74 4E 61 6D 65 07 53 74 61 72 74 65 64 ) // ventName.Started
      .custom /*0C00027C:0A000022*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*01000029*/::.ctor(string,
                                                                                                                                                string) /* 0A000022 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::Started

    .method /*06000243*/ public hidebysig static 
            void  Shutdown(class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ logger) cil managed
    // SIG: 00 01 01 12 7D
    {
      .custom /*0C00027D:0A000021*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*01000028*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LogLevel/*01000027*/,
                                                                                                                                                                                string) /* 0A000021 */ = ( 01 00 05 00 00 00 01 00 00 00 10 48 6F 73 74 69   // ...........Hosti
                                                                                                                                                                                                           6E 67 20 73 68 75 74 64 6F 77 6E 01 00 54 0E 09   // ng shutdown..T..
                                                                                                                                                                                                           45 76 65 6E 74 4E 61 6D 65 07 53 74 61 72 74 65   // EventName.Starte
                                                                                                                                                                                                           64 )                                              // d
      .custom /*0C00027E:0A000022*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*01000029*/::.ctor(string,
                                                                                                                                                string) /* 0A000022 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::Shutdown

    .method /*06000244*/ public hidebysig static 
            void  ServerShutdownException(class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ logger,
                                          class [System.Runtime/*23000001*/]System.Exception/*0100001C*/ ex) cil managed
    // SIG: 00 02 01 12 7D 12 71
    {
      .custom /*0C00027F:0A000021*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*01000028*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LogLevel/*01000027*/,
                                                                                                                                                                                string) /* 0A000021 */ = ( 01 00 0C 00 00 00 01 00 00 00 19 53 65 72 76 65   // ...........Serve
                                                                                                                                                                                                           72 20 73 68 75 74 64 6F 77 6E 20 65 78 63 65 70   // r shutdown excep
                                                                                                                                                                                                           74 69 6F 6E 01 00 54 0E 09 45 76 65 6E 74 4E 61   // tion..T..EventNa
                                                                                                                                                                                                           6D 65 17 53 65 72 76 65 72 53 68 75 74 64 6F 77   // me.ServerShutdow
                                                                                                                                                                                                           6E 45 78 63 65 70 74 69 6F 6E )                   // nException
      .custom /*0C000280:0A000022*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*01000029*/::.ctor(string,
                                                                                                                                                string) /* 0A000022 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ServerShutdownException

    .method /*06000245*/ public hidebysig static 
            void  StartupAssemblyLoaded(class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ logger,
                                        string assemblyName) cil managed
    // SIG: 00 02 01 12 7D 0E
    {
      .custom /*0C000281:0A000021*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*01000028*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LogLevel/*01000027*/,
                                                                                                                                                                                string) /* 0A000021 */ = ( 01 00 0D 00 00 00 01 00 00 00 2E 4C 6F 61 64 65   // ...........Loade
                                                                                                                                                                                                           64 20 68 6F 73 74 69 6E 67 20 73 74 61 72 74 75   // d hosting startu
                                                                                                                                                                                                           70 20 61 73 73 65 6D 62 6C 79 20 7B 61 73 73 65   // p assembly {asse
                                                                                                                                                                                                           6D 62 6C 79 4E 61 6D 65 7D 02 00 54 0E 09 45 76   // mblyName}..T..Ev
                                                                                                                                                                                                           65 6E 74 4E 61 6D 65 1C 48 6F 73 74 69 6E 67 53   // entName.HostingS
                                                                                                                                                                                                           74 61 72 74 75 70 41 73 73 65 6D 62 6C 79 4C 6F   // tartupAssemblyLo
                                                                                                                                                                                                           61 64 65 64 54 02 10 53 6B 69 70 45 6E 61 62 6C   // adedT..SkipEnabl
                                                                                                                                                                                                           65 64 43 68 65 63 6B 01 )                         // edCheck.
      .custom /*0C000282:0A000022*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*01000029*/::.ctor(string,
                                                                                                                                                string) /* 0A000022 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::StartupAssemblyLoaded

  } // end of class Log

  .class /*02000083*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000151:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000088*/ public static initonly class Microsoft.AspNetCore.Hosting.WebHost/*02000030*//'<>c'/*02000083*/ '<>9'
    .field /*04000089*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/,class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostApplicationLifetime/*0100005B*/> '<>9__20_0'
    .field /*0400008A*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/,class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IApplicationLifetime/*0100005A*/> '<>9__20_1'
    .field /*0400008B*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/,class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IApplicationLifetime/*01000074*/> '<>9__20_2'
    .method /*06000246*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000247*/ assembly hidebysig 
            instance class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostApplicationLifetime/*0100005B*/ 
            '<.ctor>b__20_0'(class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ services) cil managed
    // SIG: 20 01 12 81 6D 12 81 59
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.ctor>b__20_0'

    .method /*06000248*/ assembly hidebysig 
            instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IApplicationLifetime/*0100005A*/ 
            '<.ctor>b__20_1'(class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ services) cil managed
    // SIG: 20 01 12 81 69 12 81 59
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.ctor>b__20_1'

    .method /*06000249*/ assembly hidebysig 
            instance class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IApplicationLifetime/*01000074*/ 
            '<.ctor>b__20_2'(class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ services) cil managed
    // SIG: 20 01 12 81 D1 12 81 59
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.ctor>b__20_2'

  } // end of class '<>c'

  .class /*02000084*/ auto ansi sealed nested private beforefieldinit '<DisposeAsync>d__34'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001B*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C000156:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400008C*/ public int32 '<>1__state'
    .field /*0400008D*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*01000090*/ '<>t__builder'
    .field /*0400008E*/ public class Microsoft.AspNetCore.Hosting.WebHost/*02000030*/ '<>4__this'
    .field /*0400008F*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable/*01000091*//ConfiguredTaskAwaiter/*01000092*/ '<>u__1'
    .field /*04000090*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable/*01000093*//ConfiguredValueTaskAwaiter/*01000094*/ '<>u__2'
    .method /*0600024A*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000025*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAsync>d__34'::MoveNext

    .method /*0600024B*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C000283:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000026*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAsync>d__34'::SetStateMachine

  } // end of class '<DisposeAsync>d__34'

  .class /*02000085*/ auto ansi sealed nested private beforefieldinit '<DisposeServiceProviderAsync>d__35'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001B*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C00015A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000091*/ public int32 '<>1__state'
    .field /*04000092*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*01000090*/ '<>t__builder'
    .field /*04000093*/ public class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ serviceProvider
    .field /*04000094*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable/*01000093*//ConfiguredValueTaskAwaiter/*01000094*/ '<>u__1'
    .method /*0600024C*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000025*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeServiceProviderAsync>d__35'::MoveNext

    .method /*0600024D*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C000284:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000026*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeServiceProviderAsync>d__35'::SetStateMachine

  } // end of class '<DisposeServiceProviderAsync>d__35'

  .class /*02000086*/ auto ansi sealed nested private beforefieldinit '<StartAsync>d__27'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001B*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C00015D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000095*/ public int32 '<>1__state'
    .field /*04000096*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100003D*/ '<>t__builder'
    .field /*04000097*/ public class Microsoft.AspNetCore.Hosting.WebHost/*02000030*/ '<>4__this'
    .field /*04000098*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ cancellationToken
    .field /*04000099*/ private class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.RequestDelegate/*01000077*/ '<application>5__2'
    .field /*0400009A*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable/*01000091*//ConfiguredTaskAwaiter/*01000092*/ '<>u__1'
    .method /*0600024E*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000025*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<StartAsync>d__27'::MoveNext

    .method /*0600024F*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C000285:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000026*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<StartAsync>d__27'::SetStateMachine

  } // end of class '<StartAsync>d__27'

  .class /*02000087*/ auto ansi sealed nested private beforefieldinit '<StopAsync>d__32'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001B*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C000160:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400009B*/ public int32 '<>1__state'
    .field /*0400009C*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100003D*/ '<>t__builder'
    .field /*0400009D*/ public class Microsoft.AspNetCore.Hosting.WebHost/*02000030*/ '<>4__this'
    .field /*0400009E*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ cancellationToken
    .field /*0400009F*/ private class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*01000095*/ '<timeoutCTS>5__2'
    .field /*040000A0*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable/*01000091*//ConfiguredTaskAwaiter/*01000092*/ '<>u__1'
    .method /*06000250*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000025*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<StopAsync>d__32'::MoveNext

    .method /*06000251*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C000286:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000026*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<StopAsync>d__32'::SetStateMachine

  } // end of class '<StopAsync>d__32'

  .method /*06000101*/ assembly hidebysig specialname 
          instance class Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/ 
          get_Options() cil managed
  // SIG: 20 00 12 80 C8
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHost::get_Options

  .method /*06000102*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ appServices,
                               class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ hostingServiceProvider,
                               class Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/ options,
                               class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfiguration/*0100006F*/ config,
                               class [System.Runtime/*23000001*/]System.AggregateException/*0100002D*/ hostingStartupErrors) cil managed
  // SIG: 20 05 01 12 81 19 12 81 59 12 80 C8 12 81 BD 12 80 B5
  {
    .param [5]/*080000FE*/ 
    .custom /*0C0001DA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHost::.ctor

  .method /*06000103*/ public hidebysig newslot specialname virtual final 
          instance class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ 
          get_Services() cil managed
  // SIG: 20 00 12 81 59
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHost::get_Services

  .method /*06000104*/ public hidebysig newslot specialname virtual final 
          instance class [Microsoft.Extensions.Features/*2300000E*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000058*/ 
          get_ServerFeatures() cil managed
  // SIG: 20 00 12 81 61
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHost::get_ServerFeatures

  .method /*06000105*/ public hidebysig instance void 
          Initialize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHost::Initialize

  .method /*06000106*/ public hidebysig newslot virtual final 
          instance void  Start() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHost::Start

  .method /*06000107*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000039*/ 
          StartAsync([opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ cancellationToken) cil managed
  // SIG: 20 01 12 80 E5 11 81 71
  {
    .custom /*0C0001DD:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000038*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000019*/) /* 0A000023 */ = ( 01 00 36 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..6Microsoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 6F 73 74 69 6E 67 2E   // NetCore.Hosting.
                                                                                                                                                                                                                                        57 65 62 48 6F 73 74 2B 3C 53 74 61 72 74 41 73   // WebHost+<StartAs
                                                                                                                                                                                                                                        79 6E 63 3E 64 5F 5F 32 37 00 00 )                // ync>d__27..
    .param [1]/*080000FF*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHost::StartAsync

  .method /*06000108*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000039*/ 
          StopAsync([opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ cancellationToken) cil managed
  // SIG: 20 01 12 80 E5 11 81 71
  {
    .custom /*0C0001DE:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000038*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000019*/) /* 0A000023 */ = ( 01 00 35 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..5Microsoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 6F 73 74 69 6E 67 2E   // NetCore.Hosting.
                                                                                                                                                                                                                                        57 65 62 48 6F 73 74 2B 3C 53 74 6F 70 41 73 79   // WebHost+<StopAsy
                                                                                                                                                                                                                                        6E 63 3E 64 5F 5F 33 32 00 00 )                   // nc>d__32..
    .param [1]/*08000100*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHost::StopAsync

  .method /*06000109*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHost::Dispose

  .method /*0600010A*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100008F*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 82 3D
  {
    .custom /*0C0001DF:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000038*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000019*/) /* 0A000023 */ = ( 01 00 38 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..8Microsoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 6F 73 74 69 6E 67 2E   // NetCore.Hosting.
                                                                                                                                                                                                                                        57 65 62 48 6F 73 74 2B 3C 44 69 73 70 6F 73 65   // WebHost+<Dispose
                                                                                                                                                                                                                                        41 73 79 6E 63 3E 64 5F 5F 33 34 00 00 )          // Async>d__34..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHost::DisposeAsync

  .property /*1700004F*/ instance callconv(8) class Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/
          Options()
  {
    .get instance class Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/ Microsoft.AspNetCore.Hosting.WebHost/*02000030*/::get_Options() /* 06000101 */
  } // end of property WebHost::Options
  .property /*17000050*/ instance callconv(8) class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/
          Services()
  {
    .get instance class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ Microsoft.AspNetCore.Hosting.WebHost/*02000030*/::get_Services() /* 06000103 */
  } // end of property WebHost::Services
  .property /*17000051*/ instance callconv(8) class [Microsoft.Extensions.Features/*2300000E*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000058*/
          ServerFeatures()
  {
    .get instance class [Microsoft.Extensions.Features/*2300000E*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000058*/ Microsoft.AspNetCore.Hosting.WebHost/*02000030*/::get_ServerFeatures() /* 06000104 */
  } // end of property WebHost::ServerFeatures
} // end of class Microsoft.AspNetCore.Hosting.WebHost

.class /*02000031*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.WebHostLifetime
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000021*/
{
  .method /*0600010B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*01000095*/ cts,
                               class [System.Threading/*23000016*/]System.Threading.ManualResetEventSlim/*01000096*/ resetEvent,
                               string shutdownMessage) cil managed
  // SIG: 20 03 01 12 82 55 12 82 59 0E
  {
    .custom /*0C0001E0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostLifetime::.ctor

  .method /*0600010C*/ assembly hidebysig 
          instance void  SetExitedGracefully() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostLifetime::SetExitedGracefully

  .method /*0600010D*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostLifetime::Dispose

} // end of class Microsoft.AspNetCore.Hosting.WebHostLifetime

.class /*02000032*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.WebHostOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000D3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000D4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600010E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfiguration/*0100006F*/ configuration,
                               string applicationNameFallback) cil managed
  // SIG: 20 02 01 12 81 BD 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::.ctor

  .method /*0600010F*/ public hidebysig specialname 
          instance string  get_ApplicationName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001E1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::get_ApplicationName

  .method /*06000110*/ public hidebysig specialname 
          instance void  set_ApplicationName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001E2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::set_ApplicationName

  .method /*06000111*/ public hidebysig specialname 
          instance bool  get_PreventHostingStartup() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0001E3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::get_PreventHostingStartup

  .method /*06000112*/ public hidebysig specialname 
          instance void  set_PreventHostingStartup(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0001E4:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::set_PreventHostingStartup

  .method /*06000113*/ public hidebysig specialname 
          instance bool  get_SuppressStatusMessages() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0001E5:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::get_SuppressStatusMessages

  .method /*06000114*/ public hidebysig specialname 
          instance void  set_SuppressStatusMessages(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0001E6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::set_SuppressStatusMessages

  .method /*06000115*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100007B*/<string> 
          get_HostingStartupAssemblies() cil managed
  // SIG: 20 00 15 12 81 ED 01 0E
  {
    .custom /*0C0001E7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::get_HostingStartupAssemblies

  .method /*06000116*/ public hidebysig specialname 
          instance void  set_HostingStartupAssemblies(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100007B*/<string> 'value') cil managed
  // SIG: 20 01 01 15 12 81 ED 01 0E
  {
    .custom /*0C0001E8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::set_HostingStartupAssemblies

  .method /*06000117*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100007B*/<string> 
          get_HostingStartupExcludeAssemblies() cil managed
  // SIG: 20 00 15 12 81 ED 01 0E
  {
    .custom /*0C0001E9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::get_HostingStartupExcludeAssemblies

  .method /*06000118*/ public hidebysig specialname 
          instance void  set_HostingStartupExcludeAssemblies(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100007B*/<string> 'value') cil managed
  // SIG: 20 01 01 15 12 81 ED 01 0E
  {
    .custom /*0C0001EA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::set_HostingStartupExcludeAssemblies

  .method /*06000119*/ public hidebysig specialname 
          instance bool  get_DetailedErrors() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0001EB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::get_DetailedErrors

  .method /*0600011A*/ public hidebysig specialname 
          instance void  set_DetailedErrors(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0001EC:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::set_DetailedErrors

  .method /*0600011B*/ public hidebysig specialname 
          instance bool  get_CaptureStartupErrors() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0001ED:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::get_CaptureStartupErrors

  .method /*0600011C*/ public hidebysig specialname 
          instance void  set_CaptureStartupErrors(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0001EE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::set_CaptureStartupErrors

  .method /*0600011D*/ public hidebysig specialname 
          instance string  get_Environment() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001EF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::get_Environment

  .method /*0600011E*/ public hidebysig specialname 
          instance void  set_Environment(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001F0:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::set_Environment

  .method /*0600011F*/ public hidebysig specialname 
          instance string  get_StartupAssembly() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001F1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::get_StartupAssembly

  .method /*06000120*/ public hidebysig specialname 
          instance void  set_StartupAssembly(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001F2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::set_StartupAssembly

  .method /*06000121*/ public hidebysig specialname 
          instance string  get_WebRoot() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001F3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::get_WebRoot

  .method /*06000122*/ public hidebysig specialname 
          instance void  set_WebRoot(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001F4:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::set_WebRoot

  .method /*06000123*/ public hidebysig specialname 
          instance string  get_ContentRootPath() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001F5:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::get_ContentRootPath

  .method /*06000124*/ public hidebysig specialname 
          instance void  set_ContentRootPath(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001F7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::set_ContentRootPath

  .method /*06000125*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000088*/ 
          get_ShutdownTimeout() cil managed
  // SIG: 20 00 11 82 21
  {
    .custom /*0C0001F8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::get_ShutdownTimeout

  .method /*06000126*/ public hidebysig specialname 
          instance void  set_ShutdownTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000088*/ 'value') cil managed
  // SIG: 20 01 01 11 82 21
  {
    .custom /*0C0001F9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::set_ShutdownTimeout

  .method /*06000127*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string> 
          GetFinalHostingStartupAssemblies() cil managed
  // SIG: 20 00 15 12 75 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostOptions::GetFinalHostingStartupAssemblies

  .property /*17000052*/ instance callconv(8) string
          ApplicationName()
  {
    .get instance string Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::get_ApplicationName() /* 0600010F */
    .set instance void Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::set_ApplicationName(string) /* 06000110 */
  } // end of property WebHostOptions::ApplicationName
  .property /*17000053*/ instance callconv(8) bool
          PreventHostingStartup()
  {
    .get instance bool Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::get_PreventHostingStartup() /* 06000111 */
    .set instance void Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::set_PreventHostingStartup(bool) /* 06000112 */
  } // end of property WebHostOptions::PreventHostingStartup
  .property /*17000054*/ instance callconv(8) bool
          SuppressStatusMessages()
  {
    .get instance bool Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::get_SuppressStatusMessages() /* 06000113 */
    .set instance void Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::set_SuppressStatusMessages(bool) /* 06000114 */
  } // end of property WebHostOptions::SuppressStatusMessages
  .property /*17000055*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100007B*/<string>
          HostingStartupAssemblies()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100007B*/<string> Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::get_HostingStartupAssemblies() /* 06000115 */
    .set instance void Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::set_HostingStartupAssemblies(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100007B*/<string>) /* 06000116 */
  } // end of property WebHostOptions::HostingStartupAssemblies
  .property /*17000056*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100007B*/<string>
          HostingStartupExcludeAssemblies()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100007B*/<string> Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::get_HostingStartupExcludeAssemblies() /* 06000117 */
    .set instance void Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::set_HostingStartupExcludeAssemblies(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100007B*/<string>) /* 06000118 */
  } // end of property WebHostOptions::HostingStartupExcludeAssemblies
  .property /*17000057*/ instance callconv(8) bool
          DetailedErrors()
  {
    .get instance bool Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::get_DetailedErrors() /* 06000119 */
    .set instance void Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::set_DetailedErrors(bool) /* 0600011A */
  } // end of property WebHostOptions::DetailedErrors
  .property /*17000058*/ instance callconv(8) bool
          CaptureStartupErrors()
  {
    .get instance bool Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::get_CaptureStartupErrors() /* 0600011B */
    .set instance void Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::set_CaptureStartupErrors(bool) /* 0600011C */
  } // end of property WebHostOptions::CaptureStartupErrors
  .property /*17000059*/ instance callconv(8) string
          Environment()
  {
    .get instance string Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::get_Environment() /* 0600011D */
    .set instance void Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::set_Environment(string) /* 0600011E */
  } // end of property WebHostOptions::Environment
  .property /*1700005A*/ instance callconv(8) string
          StartupAssembly()
  {
    .get instance string Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::get_StartupAssembly() /* 0600011F */
    .set instance void Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::set_StartupAssembly(string) /* 06000120 */
  } // end of property WebHostOptions::StartupAssembly
  .property /*1700005B*/ instance callconv(8) string
          WebRoot()
  {
    .get instance string Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::get_WebRoot() /* 06000121 */
    .set instance void Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::set_WebRoot(string) /* 06000122 */
  } // end of property WebHostOptions::WebRoot
  .property /*1700005C*/ instance callconv(8) string
          ContentRootPath()
  {
    .get instance string Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::get_ContentRootPath() /* 06000123 */
    .set instance void Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::set_ContentRootPath(string) /* 06000124 */
  } // end of property WebHostOptions::ContentRootPath
  .property /*1700005D*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000088*/
          ShutdownTimeout()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000088*/ Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::get_ShutdownTimeout() /* 06000125 */
    .set instance void Microsoft.AspNetCore.Hosting.WebHostOptions/*02000032*/::set_ShutdownTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000088*/) /* 06000126 */
  } // end of property WebHostOptions::ShutdownTimeout
} // end of class Microsoft.AspNetCore.Hosting.WebHostOptions

.class /*02000033*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.WebHostUtilities
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*06000128*/ public hidebysig static 
          bool  ParseBool(class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfiguration/*0100006F*/ configuration,
                          string key) cil managed
  // SIG: 00 02 02 12 81 BD 0E
  {
    .custom /*0C0001FA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostUtilities::ParseBool

  .method /*06000129*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostUtilities::.ctor

} // end of class Microsoft.AspNetCore.Hosting.WebHostUtilities

.class /*02000034*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.ConventionBasedStartup
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IStartup/*01000097*/
{
  .custom /*0C0000D7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000D8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IStartup/*01000097*/
  .custom /*0C00009E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600012A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Hosting.StartupMethods/*0200002F*/ methods) cil managed
  // SIG: 20 01 01 12 80 BC
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConventionBasedStartup::.ctor

  .method /*0600012B*/ public hidebysig newslot virtual final 
          instance void  Configure(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/ app) cil managed
  // SIG: 20 01 01 12 81 8D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConventionBasedStartup::Configure

  .method /*0600012C*/ public hidebysig newslot virtual final 
          instance class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ 
          ConfigureServices(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
  // SIG: 20 01 12 81 59 12 81 19
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConventionBasedStartup::ConfigureServices

} // end of class Microsoft.AspNetCore.Hosting.ConventionBasedStartup

.class /*02000035*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.DelegateStartup
       extends class Microsoft.AspNetCore.Hosting.StartupBase`1/*02000037*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/>/*1B00000D*/
{
  .custom /*0C0000DB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000DC:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .method /*0600012D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceProviderFactory`1/*01000067*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/> factory,
                               class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> configureApp) cil managed
  // SIG: 20 02 01 15 12 81 9D 01 12 81 19 15 12 81 05 01 12 81 8D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DelegateStartup::.ctor

  .method /*0600012E*/ public hidebysig virtual 
          instance void  Configure(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/ app) cil managed
  // SIG: 20 01 01 12 81 8D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DelegateStartup::Configure

} // end of class Microsoft.AspNetCore.Hosting.DelegateStartup

.class /*02000036*/ public abstract auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.StartupBase
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IStartup/*01000097*/
{
  .custom /*0C0000E0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000E1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IStartup/*01000097*/
  .custom /*0C00009F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600012F*/ public hidebysig newslot abstract virtual 
          instance void  Configure(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/ app) cil managed
  // SIG: 20 01 01 12 81 8D
  {
    // Method begins at RVA 0x0
  } // end of method StartupBase::Configure

  .method /*06000130*/ private hidebysig newslot virtual final 
          instance class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ 
          Microsoft.AspNetCore.Hosting.IStartup.ConfigureServices(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
  // SIG: 20 01 12 81 59 12 81 19
  {
    .override [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IStartup/*01000097*/::ConfigureServices /*01000097::0A00002F*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StartupBase::Microsoft.AspNetCore.Hosting.IStartup.ConfigureServices

  .method /*06000131*/ public hidebysig newslot virtual 
          instance void  ConfigureServices(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
  // SIG: 20 01 01 12 81 19
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StartupBase::ConfigureServices

  .method /*06000132*/ public hidebysig newslot virtual 
          instance class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ 
          CreateServiceProvider(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
  // SIG: 20 01 12 81 59 12 81 19
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StartupBase::CreateServiceProvider

  .method /*06000133*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StartupBase::.ctor

} // end of class Microsoft.AspNetCore.Hosting.StartupBase

.class /*02000037*/ public abstract auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.StartupBase`1<TBuilder>
       extends Microsoft.AspNetCore.Hosting.StartupBase/*02000036*/
{
  .custom /*0C0000E2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000E3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000134*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceProviderFactory`1/*01000067*/<!TBuilder> factory) cil managed
  // SIG: 20 01 01 15 12 81 9D 01 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StartupBase`1::.ctor

  .method /*06000135*/ public hidebysig virtual 
          instance class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ 
          CreateServiceProvider(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
  // SIG: 20 01 12 81 59 12 81 19
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StartupBase`1::CreateServiceProvider

  .method /*06000136*/ public hidebysig newslot virtual 
          instance void  ConfigureContainer(!TBuilder builder) cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StartupBase`1::ConfigureContainer

} // end of class Microsoft.AspNetCore.Hosting.StartupBase`1

.class /*02000038*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.WebHostBuilder
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/
{
  .custom /*0C0000E6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000E7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/
  .custom /*0C0000A2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000088*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000163:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000A1*/ public static initonly class Microsoft.AspNetCore.Hosting.WebHostBuilder/*02000038*//'<>c'/*02000088*/ '<>9'
    .field /*040000A2*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/,class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DiagnosticListener/*01000064*/> '<>9__14_1'
    .field /*040000A3*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/,class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DiagnosticSource/*01000065*/> '<>9__14_2'
    .field /*040000A4*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/,class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.ActivitySource/*01000066*/> '<>9__14_3'
    .method /*06000252*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000253*/ assembly hidebysig 
            instance class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DiagnosticListener/*01000064*/ 
            '<BuildCommonServices>b__14_1'(class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ sp) cil managed
    // SIG: 20 01 12 81 91 12 81 59
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<BuildCommonServices>b__14_1'

    .method /*06000254*/ assembly hidebysig 
            instance class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.DiagnosticSource/*01000065*/ 
            '<BuildCommonServices>b__14_2'(class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ sp) cil managed
    // SIG: 20 01 12 81 95 12 81 59
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<BuildCommonServices>b__14_2'

    .method /*06000255*/ assembly hidebysig 
            instance class [System.Diagnostics.DiagnosticSource/*23000012*/]System.Diagnostics.ActivitySource/*01000066*/ 
            '<BuildCommonServices>b__14_3'(class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ sp) cil managed
    // SIG: 20 01 12 81 99 12 81 59
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<BuildCommonServices>b__14_3'

  } // end of class '<>c'

  .class /*02000089*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass10_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000165:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000A5*/ public class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/> configureServices
    .method /*06000256*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass10_0'::.ctor

    .method /*06000257*/ assembly hidebysig 
            instance void  '<ConfigureServices>b__0'(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/ _,
                                                     class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
    // SIG: 20 02 01 12 81 79 12 81 19
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass10_0'::'<ConfigureServices>b__0'

  } // end of class '<>c__DisplayClass10_0'

  .class /*0200008A*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass14_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000167:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000A6*/ public class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfigurationRoot/*01000098*/ configuration
    .method /*06000258*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass14_0'::.ctor

    .method /*06000259*/ assembly hidebysig 
            instance class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfiguration/*0100006F*/ 
            '<BuildCommonServices>b__0'(class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ _) cil managed
    // SIG: 20 01 12 81 BD 12 81 59
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass14_0'::'<BuildCommonServices>b__0'

  } // end of class '<>c__DisplayClass14_0'

  .class /*0200008B*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass14_1'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000169:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000A7*/ public class [System.Runtime/*23000001*/]System.Type/*01000019*/ startupType
    .method /*0600025A*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass14_1'::.ctor

    .method /*0600025B*/ assembly hidebysig 
            instance object  '<BuildCommonServices>b__4'(class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ sp) cil managed
    // SIG: 20 01 1C 12 81 59
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass14_1'::'<BuildCommonServices>b__4'

  } // end of class '<>c__DisplayClass14_1'

  .class /*0200008C*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass14_2'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00016B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000A8*/ public class [System.Runtime/*23000001*/]System.Runtime.ExceptionServices.ExceptionDispatchInfo/*01000068*/ capture
    .method /*0600025C*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass14_2'::.ctor

    .method /*0600025D*/ assembly hidebysig 
            instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IStartup/*01000097*/ 
            '<BuildCommonServices>b__5'(class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ _) cil managed
    // SIG: 20 01 12 82 5D 12 81 59
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass14_2'::'<BuildCommonServices>b__5'

  } // end of class '<>c__DisplayClass14_2'

  .method /*06000137*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilder::.ctor

  .method /*06000138*/ public hidebysig newslot virtual final 
          instance string  GetSetting(string key) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilder::GetSetting

  .method /*06000139*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          UseSetting(string key,
                     string 'value') cil managed
  // SIG: 20 02 12 81 0D 0E 0E
  {
    .param [2]/*08000123*/ 
    .custom /*0C0001F6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilder::UseSetting

  .method /*0600013A*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          ConfigureServices(class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/> configureServices) cil managed
  // SIG: 20 01 12 81 0D 15 12 81 05 01 12 81 19
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilder::ConfigureServices

  .method /*0600013B*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          ConfigureServices(class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/> configureServices) cil managed
  // SIG: 20 01 12 81 0D 15 12 81 11 02 12 81 79 12 81 19
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilder::ConfigureServices

  .method /*0600013C*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          ConfigureAppConfiguration(class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*0100005F*/> configureDelegate) cil managed
  // SIG: 20 01 12 81 0D 15 12 81 11 02 12 81 79 12 81 7D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilder::ConfigureAppConfiguration

  .method /*0600013D*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHost/*0100005D*/ 
          Build() cil managed
  // SIG: 20 00 12 81 75
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilder::Build

  .method /*0600013E*/ assembly hidebysig static 
          class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ 
          '<Build>g__GetProviderFromFactory|13_0'(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ collection) cil managed
  // SIG: 00 01 12 81 59 12 81 19
  {
    .custom /*0C0001FC:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilder::'<Build>g__GetProviderFromFactory|13_0'

} // end of class Microsoft.AspNetCore.Hosting.WebHostBuilder

.class /*02000039*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Hosting.WebHostBuilderExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000E9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000EA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000EB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*0200008D*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00016D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000A9*/ public static initonly class Microsoft.AspNetCore.Hosting.WebHostBuilderExtensions/*02000039*//'<>c'/*0200008D*/ '<>9'
    .field /*040000AA*/ public static class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*0100005F*/> '<>9__10_0'
    .method /*0600025E*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*0600025F*/ assembly hidebysig 
            instance void  '<UseStaticWebAssets>b__10_0'(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/ context,
                                                         class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*0100005F*/ configBuilder) cil managed
    // SIG: 20 02 01 12 81 79 12 81 7D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<UseStaticWebAssets>b__10_0'

  } // end of class '<>c'

  .class /*0200008E*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass0_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000170:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000AB*/ public class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> configureApp
    .field /*040000AC*/ public class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> '<>9__2'
    .field /*040000AD*/ public class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/,class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IStartup/*01000097*/> '<>9__1'
    .method /*06000260*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass0_0'::.ctor

    .method /*06000261*/ assembly hidebysig 
            instance void  '<Configure>b__0'(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/ context,
                                             class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
    // SIG: 20 02 01 12 81 79 12 81 19
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass0_0'::'<Configure>b__0'

    .method /*06000262*/ assembly hidebysig 
            instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IStartup/*01000097*/ 
            '<Configure>b__1'(class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ sp) cil managed
    // SIG: 20 01 12 82 5D 12 81 59
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass0_0'::'<Configure>b__1'

    .method /*06000263*/ assembly hidebysig 
            instance void  '<Configure>b__2'(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/ app) cil managed
    // SIG: 20 01 01 12 81 8D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass0_0'::'<Configure>b__2'

  } // end of class '<>c__DisplayClass0_0'

  .class /*0200008F*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass1_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000172:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000AE*/ public class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> configureApp
    .method /*06000264*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass1_0'::.ctor

    .method /*06000265*/ assembly hidebysig 
            instance void  '<Configure>b__0'(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/ context,
                                             class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
    // SIG: 20 02 01 12 81 79 12 81 19
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass1_0'::'<Configure>b__0'

  } // end of class '<>c__DisplayClass1_0'

  .class /*02000090*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass1_1'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000174:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000AF*/ public class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/ context
    .field /*040000B0*/ public class Microsoft.AspNetCore.Hosting.WebHostBuilderExtensions/*02000039*//'<>c__DisplayClass1_0'/*0200008F*/ 'CS$<>8__locals1'
    .field /*040000B1*/ public class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> '<>9__2'
    .method /*06000266*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass1_1'::.ctor

    .method /*06000267*/ assembly hidebysig 
            instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IStartup/*01000097*/ 
            '<Configure>b__1'(class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ sp) cil managed
    // SIG: 20 01 12 82 5D 12 81 59
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass1_1'::'<Configure>b__1'

    .method /*06000268*/ assembly hidebysig 
            instance void  '<Configure>b__2'(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/ app) cil managed
    // SIG: 20 01 01 12 81 8D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass1_1'::'<Configure>b__2'

  } // end of class '<>c__DisplayClass1_1'

  .class /*02000091*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass2_0`1'<class TStartup>
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000176:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000B2*/ public class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,!TStartup> startupFactory
    .method /*06000269*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass2_0`1'::.ctor

    .method /*0600026A*/ assembly hidebysig 
            instance void  '<UseStartup>b__0'(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/ context,
                                              class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
    // SIG: 20 02 01 12 81 79 12 81 19
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass2_0`1'::'<UseStartup>b__0'

  } // end of class '<>c__DisplayClass2_0`1'

  .class /*02000092*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass2_1`1'<class TStartup>
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000178:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000B3*/ public class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/ context
    .field /*040000B4*/ public class Microsoft.AspNetCore.Hosting.WebHostBuilderExtensions/*02000039*//'<>c__DisplayClass2_0`1'/*02000091*/<!TStartup> 'CS$<>8__locals1'
    .method /*0600026B*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass2_1`1'::.ctor

    .method /*0600026C*/ assembly hidebysig 
            instance object  '<UseStartup>b__1'(class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ sp) cil managed
    // SIG: 20 01 1C 12 81 59
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass2_1`1'::'<UseStartup>b__1'

  } // end of class '<>c__DisplayClass2_1`1'

  .class /*02000093*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass3_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00017B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000B5*/ public class [System.Runtime/*23000001*/]System.Type/*01000019*/ startupType
    .field /*040000B6*/ public class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/,object> '<>9__1'
    .method /*0600026D*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass3_0'::.ctor

    .method /*0600026E*/ assembly hidebysig 
            instance void  '<UseStartup>b__0'(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
    // SIG: 20 01 01 12 81 19
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass3_0'::'<UseStartup>b__0'

    .method /*0600026F*/ assembly hidebysig 
            instance object  '<UseStartup>b__1'(class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ sp) cil managed
    // SIG: 20 01 1C 12 81 59
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass3_0'::'<UseStartup>b__1'

  } // end of class '<>c__DisplayClass3_0'

  .class /*02000094*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass5_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00017E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000B7*/ public class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.Extensions.DependencyInjection/*23000011*/]Microsoft.Extensions.DependencyInjection.ServiceProviderOptions/*01000060*/> configure
    .method /*06000270*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass5_0'::.ctor

    .method /*06000271*/ assembly hidebysig 
            instance void  '<UseDefaultServiceProvider>b__0'(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/ context,
                                                             class [Microsoft.Extensions.DependencyInjection/*23000011*/]Microsoft.Extensions.DependencyInjection.ServiceProviderOptions/*01000060*/ options) cil managed
    // SIG: 20 02 01 12 81 79 12 81 81
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass5_0'::'<UseDefaultServiceProvider>b__0'

  } // end of class '<>c__DisplayClass5_0'

  .class /*02000095*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass6_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00017F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000B8*/ public class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.Extensions.DependencyInjection/*23000011*/]Microsoft.Extensions.DependencyInjection.ServiceProviderOptions/*01000060*/> configure
    .method /*06000272*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass6_0'::.ctor

    .method /*06000273*/ assembly hidebysig 
            instance void  '<UseDefaultServiceProvider>b__0'(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/ context,
                                                             class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ services) cil managed
    // SIG: 20 02 01 12 81 79 12 81 19
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass6_0'::'<UseDefaultServiceProvider>b__0'

  } // end of class '<>c__DisplayClass6_0'

  .class /*02000096*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass7_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000181:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000B9*/ public class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*0100005F*/> configureDelegate
    .method /*06000274*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass7_0'::.ctor

    .method /*06000275*/ assembly hidebysig 
            instance void  '<ConfigureAppConfiguration>b__0'(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/ context,
                                                             class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*0100005F*/ builder) cil managed
    // SIG: 20 02 01 12 81 79 12 81 7D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass7_0'::'<ConfigureAppConfiguration>b__0'

  } // end of class '<>c__DisplayClass7_0'

  .class /*02000097*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass8_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000182:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000BA*/ public class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.Extensions.Logging/*23000017*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000099*/> configureLogging
    .method /*06000276*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass8_0'::.ctor

    .method /*06000277*/ assembly hidebysig 
            instance void  '<ConfigureLogging>b__0'(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ collection) cil managed
    // SIG: 20 01 01 12 81 19
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass8_0'::'<ConfigureLogging>b__0'

  } // end of class '<>c__DisplayClass8_0'

  .class /*02000098*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass9_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000183:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000BB*/ public class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.Extensions.Logging/*23000017*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000099*/> configureLogging
    .method /*06000278*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass9_0'::.ctor

    .method /*06000279*/ assembly hidebysig 
            instance void  '<ConfigureLogging>b__0'(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/ context,
                                                    class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000009*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000046*/ collection) cil managed
    // SIG: 20 02 01 12 81 79 12 81 19
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass9_0'::'<ConfigureLogging>b__0'

  } // end of class '<>c__DisplayClass9_0'

  .class /*02000099*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass9_1'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000184:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000BC*/ public class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/ context
    .field /*040000BD*/ public class Microsoft.AspNetCore.Hosting.WebHostBuilderExtensions/*02000039*//'<>c__DisplayClass9_0'/*02000098*/ 'CS$<>8__locals1'
    .method /*0600027A*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass9_1'::.ctor

    .method /*0600027B*/ assembly hidebysig 
            instance void  '<ConfigureLogging>b__1'(class [Microsoft.Extensions.Logging/*23000017*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000099*/ builder) cil managed
    // SIG: 20 01 01 12 82 65
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass9_1'::'<ConfigureLogging>b__1'

  } // end of class '<>c__DisplayClass9_1'

  .method /*0600013F*/ public hidebysig static 
          class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          Configure(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ hostBuilder,
                    class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> configureApp) cil managed
  // SIG: 00 02 12 81 0D 12 81 0D 15 12 81 05 01 12 81 8D
  {
    .custom /*0C0001FD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilderExtensions::Configure

  .method /*06000140*/ public hidebysig static 
          class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          Configure(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ hostBuilder,
                    class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> configureApp) cil managed
  // SIG: 00 02 12 81 0D 12 81 0D 15 12 81 11 02 12 81 79 12 81 8D
  {
    .custom /*0C0001FE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilderExtensions::Configure

  .method /*06000141*/ public hidebysig static 
          class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          UseStartup<class TStartup>(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ hostBuilder,
                                     class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,!!TStartup> startupFactory) cil managed
  // SIG: 10 01 02 12 81 0D 12 81 0D 15 12 80 A9 02 12 81 79 1E 00
  {
    .custom /*0C0001FF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TStartup /*2A00000E*/ 
      .custom /*0C00004D:0A00002A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000062*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000061*/) /* 0A00002A */ = ( 01 00 08 00 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilderExtensions::UseStartup

  .method /*06000142*/ public hidebysig static 
          class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          UseStartup(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ hostBuilder,
                     class [System.Runtime/*23000001*/]System.Type/*01000019*/ startupType) cil managed
  // SIG: 00 02 12 81 0D 12 81 0D 12 65
  {
    .custom /*0C000200:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*0800012F*/ 
    .custom /*0C0001FB:0A00002A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000062*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000061*/) /* 0A00002A */ = ( 01 00 0B 00 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilderExtensions::UseStartup

  .method /*06000143*/ public hidebysig static 
          class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          UseStartup<class TStartup>(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ hostBuilder) cil managed
  // SIG: 10 01 01 12 81 0D 12 81 0D
  {
    .custom /*0C000201:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TStartup /*2A00000F*/ 
      .custom /*0C000053:0A00002A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000062*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000061*/) /* 0A00002A */ = ( 01 00 0B 00 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilderExtensions::UseStartup

  .method /*06000144*/ public hidebysig static 
          class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          UseDefaultServiceProvider(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ hostBuilder,
                                    class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.Extensions.DependencyInjection/*23000011*/]Microsoft.Extensions.DependencyInjection.ServiceProviderOptions/*01000060*/> configure) cil managed
  // SIG: 00 02 12 81 0D 12 81 0D 15 12 81 05 01 12 81 81
  {
    .custom /*0C000202:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilderExtensions::UseDefaultServiceProvider

  .method /*06000145*/ public hidebysig static 
          class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          UseDefaultServiceProvider(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ hostBuilder,
                                    class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.Extensions.DependencyInjection/*23000011*/]Microsoft.Extensions.DependencyInjection.ServiceProviderOptions/*01000060*/> configure) cil managed
  // SIG: 00 02 12 81 0D 12 81 0D 15 12 81 11 02 12 81 79 12 81 81
  {
    .custom /*0C000203:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilderExtensions::UseDefaultServiceProvider

  .method /*06000146*/ public hidebysig static 
          class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          ConfigureAppConfiguration(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ hostBuilder,
                                    class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*0100005F*/> configureDelegate) cil managed
  // SIG: 00 02 12 81 0D 12 81 0D 15 12 81 05 01 12 81 7D
  {
    .custom /*0C000204:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilderExtensions::ConfigureAppConfiguration

  .method /*06000147*/ public hidebysig static 
          class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          ConfigureLogging(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ hostBuilder,
                           class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.Extensions.Logging/*23000017*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000099*/> configureLogging) cil managed
  // SIG: 00 02 12 81 0D 12 81 0D 15 12 81 05 01 12 82 65
  {
    .custom /*0C000205:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilderExtensions::ConfigureLogging

  .method /*06000148*/ public hidebysig static 
          class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          ConfigureLogging(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ hostBuilder,
                           class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.Extensions.Logging/*23000017*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000099*/> configureLogging) cil managed
  // SIG: 00 02 12 81 0D 12 81 0D 15 12 81 11 02 12 81 79 12 82 65
  {
    .custom /*0C000206:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilderExtensions::ConfigureLogging

  .method /*06000149*/ public hidebysig static 
          class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          UseStaticWebAssets(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ builder) cil managed
  // SIG: 00 01 12 81 0D 12 81 0D
  {
    .custom /*0C000207:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilderExtensions::UseStaticWebAssets

} // end of class Microsoft.AspNetCore.Hosting.WebHostBuilderExtensions

.class /*0200003A*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Hosting.WebHostExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000EC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000ED:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000EE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*0200009A*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000185:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000BE*/ public static initonly class Microsoft.AspNetCore.Hosting.WebHostExtensions/*0200003A*//'<>c'/*0200009A*/ '<>9'
    .field /*040000BF*/ public static class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<object> '<>9__6_0'
    .field /*040000C0*/ public static class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<object> '<>9__6_1'
    .method /*0600027C*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*0600027D*/ assembly hidebysig 
            instance void  '<WaitForTokenShutdownAsync>b__6_0'(object state) cil managed
    // SIG: 20 01 01 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<WaitForTokenShutdownAsync>b__6_0'

    .method /*0600027E*/ assembly hidebysig 
            instance void  '<WaitForTokenShutdownAsync>b__6_1'(object obj) cil managed
    // SIG: 20 01 01 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<WaitForTokenShutdownAsync>b__6_1'

  } // end of class '<>c'

  .class /*0200009B*/ auto ansi sealed nested private beforefieldinit '<RunAsync>d__4'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001B*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C000186:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000C1*/ public int32 '<>1__state'
    .field /*040000C2*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100003D*/ '<>t__builder'
    .field /*040000C3*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ token
    .field /*040000C4*/ public class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHost/*0100005D*/ host
    .field /*040000C5*/ private class [System.Threading/*23000016*/]System.Threading.ManualResetEventSlim/*01000096*/ '<done>5__2'
    .field /*040000C6*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100003F*/ '<>u__1'
    .field /*040000C7*/ private class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*01000095*/ '<cts>5__3'
    .field /*040000C8*/ private class Microsoft.AspNetCore.Hosting.WebHostLifetime/*02000031*/ '<lifetime>5__4'
    .method /*0600027F*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000025*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<RunAsync>d__4'::MoveNext

    .method /*06000280*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C000287:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000026*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<RunAsync>d__4'::SetStateMachine

  } // end of class '<RunAsync>d__4'

  .class /*0200009C*/ auto ansi sealed nested private beforefieldinit '<RunAsync>d__5'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001B*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C000187:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000C9*/ public int32 '<>1__state'
    .field /*040000CA*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100003D*/ '<>t__builder'
    .field /*040000CB*/ public class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHost/*0100005D*/ host
    .field /*040000CC*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ token
    .field /*040000CD*/ public string startupMessage
    .field /*040000CE*/ private object '<>7__wrap1'
    .field /*040000CF*/ private int32 '<>7__wrap2'
    .field /*040000D0*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100003F*/ '<>u__1'
    .field /*040000D1*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable/*01000093*//ConfiguredValueTaskAwaiter/*01000094*/ '<>u__2'
    .method /*06000281*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000025*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<RunAsync>d__5'::MoveNext

    .method /*06000282*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C000288:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000026*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<RunAsync>d__5'::SetStateMachine

  } // end of class '<RunAsync>d__5'

  .class /*0200009D*/ auto ansi sealed nested private beforefieldinit '<StopAsync>d__0'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001B*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C000189:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000D2*/ public int32 '<>1__state'
    .field /*040000D3*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100003D*/ '<>t__builder'
    .field /*040000D4*/ public valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000088*/ timeout
    .field /*040000D5*/ public class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHost/*0100005D*/ host
    .field /*040000D6*/ private class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*01000095*/ '<cts>5__2'
    .field /*040000D7*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100003F*/ '<>u__1'
    .method /*06000283*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000025*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<StopAsync>d__0'::MoveNext

    .method /*06000284*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C000289:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000026*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<StopAsync>d__0'::SetStateMachine

  } // end of class '<StopAsync>d__0'

  .class /*0200009E*/ auto ansi sealed nested private beforefieldinit '<WaitForShutdownAsync>d__2'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001B*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C00018A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000D8*/ public int32 '<>1__state'
    .field /*040000D9*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100003D*/ '<>t__builder'
    .field /*040000DA*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ token
    .field /*040000DB*/ public class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHost/*0100005D*/ host
    .field /*040000DC*/ private class [System.Threading/*23000016*/]System.Threading.ManualResetEventSlim/*01000096*/ '<done>5__2'
    .field /*040000DD*/ private class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*01000095*/ '<cts>5__3'
    .field /*040000DE*/ private class Microsoft.AspNetCore.Hosting.WebHostLifetime/*02000031*/ '<lifetime>5__4'
    .field /*040000DF*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100003F*/ '<>u__1'
    .method /*06000285*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000025*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WaitForShutdownAsync>d__2'::MoveNext

    .method /*06000286*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C00028A:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000026*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WaitForShutdownAsync>d__2'::SetStateMachine

  } // end of class '<WaitForShutdownAsync>d__2'

  .class /*0200009F*/ auto ansi sealed nested private beforefieldinit '<WaitForTokenShutdownAsync>d__6'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001B*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C00018B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000E0*/ public int32 '<>1__state'
    .field /*040000E1*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100003D*/ '<>t__builder'
    .field /*040000E2*/ public class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHost/*0100005D*/ host
    .field /*040000E3*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ cancellationToken
    .field /*040000E4*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100003F*/ '<>u__1'
    .method /*06000287*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000025*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WaitForTokenShutdownAsync>d__6'::MoveNext

    .method /*06000288*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C00028B:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000026*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WaitForTokenShutdownAsync>d__6'::SetStateMachine

  } // end of class '<WaitForTokenShutdownAsync>d__6'

  .method /*0600014A*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000039*/ 
          StopAsync(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHost/*0100005D*/ host,
                    valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000088*/ timeout) cil managed
  // SIG: 00 02 12 80 E5 12 81 75 11 82 21
  {
    .custom /*0C000208:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000038*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000019*/) /* 0A000023 */ = ( 01 00 3E 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..>Microsoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 6F 73 74 69 6E 67 2E   // NetCore.Hosting.
                                                                                                                                                                                                                                        57 65 62 48 6F 73 74 45 78 74 65 6E 73 69 6F 6E   // WebHostExtension
                                                                                                                                                                                                                                        73 2B 3C 53 74 6F 70 41 73 79 6E 63 3E 64 5F 5F   // s+<StopAsync>d__
                                                                                                                                                                                                                                        30 00 00 )                                        // 0..
    .custom /*0C000209:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostExtensions::StopAsync

  .method /*0600014B*/ public hidebysig static 
          void  WaitForShutdown(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHost/*0100005D*/ host) cil managed
  // SIG: 00 01 01 12 81 75
  {
    .custom /*0C00020A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostExtensions::WaitForShutdown

  .method /*0600014C*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000039*/ 
          WaitForShutdownAsync(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHost/*0100005D*/ host,
                               [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ token) cil managed
  // SIG: 00 02 12 80 E5 12 81 75 11 81 71
  {
    .custom /*0C00020B:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000038*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000019*/) /* 0A000023 */ = ( 01 00 49 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..IMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 6F 73 74 69 6E 67 2E   // NetCore.Hosting.
                                                                                                                                                                                                                                        57 65 62 48 6F 73 74 45 78 74 65 6E 73 69 6F 6E   // WebHostExtension
                                                                                                                                                                                                                                        73 2B 3C 57 61 69 74 46 6F 72 53 68 75 74 64 6F   // s+<WaitForShutdo
                                                                                                                                                                                                                                        77 6E 41 73 79 6E 63 3E 64 5F 5F 32 00 00 )       // wnAsync>d__2..
    .custom /*0C00020C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000140*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostExtensions::WaitForShutdownAsync

  .method /*0600014D*/ public hidebysig static 
          void  Run(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHost/*0100005D*/ host) cil managed
  // SIG: 00 01 01 12 81 75
  {
    .custom /*0C00020D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostExtensions::Run

  .method /*0600014E*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000039*/ 
          RunAsync(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHost/*0100005D*/ host,
                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100005C*/ token) cil managed
  // SIG: 00 02 12 80 E5 12 81 75 11 81 71
  {
    .custom /*0C00020E:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000038*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000019*/) /* 0A000023 */ = ( 01 00 3D 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..=Microsoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 6F 73 74 69 6E 67 2E   // NetCore.Hosting.
                                                                                                                                                                                                                                        57 65 62 48 6F 73 74 45 78 74 65 6E 73 69 6F 6E   // WebHostExtension
                                                                                                                                                                                                                                        73 2B 3C 52 75 6E 41 73 79 6E 63 3E 64 5F 5F 34   // s+<RunAsync>d__4
                                                                                                                                                                                                                                        00 00 ) 
    .custom /*0C00020F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000143*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostExtensions::RunAsync

} // end of class Microsoft.AspNetCore.Hosting.WebHostExtensions

.class /*0200003B*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Hosting.Resources
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*0600014F*/ assembly hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Resources.ResourceManager/*0100009A*/ 
          get_ResourceManager() cil managed
  // SIG: 00 00 12 82 69
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_ResourceManager

  .method /*06000150*/ assembly hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*0100009B*/ 
          get_Culture() cil managed
  // SIG: 00 00 12 82 6D
  {
    .custom /*0C000210:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_Culture

  .method /*06000151*/ assembly hidebysig specialname static 
          void  set_Culture(class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*0100009B*/ 'value') cil managed
  // SIG: 00 01 01 12 82 6D
  {
    .custom /*0C000211:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::set_Culture

  .method /*06000152*/ assembly hidebysig static 
          string  GetResourceString(string resourceKey,
                                    [opt] string defaultValue) cil managed aggressiveinlining
  // SIG: 00 02 0E 0E 0E
  {
    .param [2]/*08000146*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::GetResourceString

  .method /*06000153*/ assembly hidebysig specialname static 
          string  get_WebHostBuilder_SingleInstance() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_WebHostBuilder_SingleInstance

  .property /*1700005E*/ callconv(8) class [System.Runtime/*23000001*/]System.Resources.ResourceManager/*0100009A*/
          ResourceManager()
  {
    .get class [System.Runtime/*23000001*/]System.Resources.ResourceManager/*0100009A*/ Microsoft.AspNetCore.Hosting.Resources/*0200003B*/::get_ResourceManager() /* 0600014F */
  } // end of property Resources::ResourceManager
  .property /*1700005F*/ callconv(8) class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*0100009B*/
          Culture()
  {
    .get class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*0100009B*/ Microsoft.AspNetCore.Hosting.Resources/*0200003B*/::get_Culture() /* 06000150 */
    .set void Microsoft.AspNetCore.Hosting.Resources/*0200003B*/::set_Culture(class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*0100009B*/) /* 06000151 */
  } // end of property Resources::Culture
  .property /*17000060*/ callconv(8) string
          WebHostBuilder_SingleInstance()
  {
    .get string Microsoft.AspNetCore.Hosting.Resources/*0200003B*/::get_WebHostBuilder_SingleInstance() /* 06000153 */
  } // end of property Resources::WebHostBuilder_SingleInstance
} // end of class Microsoft.AspNetCore.Hosting.Resources

.class /*0200003C*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.Views.ErrorPage
       extends Microsoft.Extensions.RazorViews.BaseView/*02000010*/
{
  .custom /*0C0000F0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000F1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*020000A0*/ auto ansi sealed nested private beforefieldinit '<ExecuteAsync>d__0'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001B*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/
  {
    .custom /*0C00018C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000E5*/ public int32 '<>1__state'
    .field /*040000E6*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100003D*/ '<>t__builder'
    .field /*040000E7*/ public class Microsoft.AspNetCore.Hosting.Views.ErrorPage/*0200003C*/ '<>4__this'
    .method /*06000289*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::MoveNext /*0100003C::0A000025*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__0'::MoveNext

    .method /*0600028A*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 F1
    {
      .custom /*0C00028C:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000025*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100003C*/::SetStateMachine /*0100003C::0A000026*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__0'::SetStateMachine

  } // end of class '<ExecuteAsync>d__0'

  .method /*06000154*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000039*/ 
          ExecuteAsync() cil managed
  // SIG: 20 00 12 80 E5
  {
    .custom /*0C000213:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .custom /*0C000214:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000038*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000019*/) /* 0A000023 */ = ( 01 00 3F 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..?Microsoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 6F 73 74 69 6E 67 2E   // NetCore.Hosting.
                                                                                                                                                                                                                                        56 69 65 77 73 2E 45 72 72 6F 72 50 61 67 65 2B   // Views.ErrorPage+
                                                                                                                                                                                                                                        3C 45 78 65 63 75 74 65 41 73 79 6E 63 3E 64 5F   // <ExecuteAsync>d_
                                                                                                                                                                                                                                        5F 30 00 00 )                                     // _0..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorPage::ExecuteAsync

  .method /*06000155*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Hosting.Views.ErrorPageModel/*0200003D*/ model) cil managed
  // SIG: 20 01 01 12 80 F4
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorPage::.ctor

  .method /*06000156*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Hosting.Views.ErrorPageModel/*0200003D*/ 
          get_Model() cil managed
  // SIG: 20 00 12 80 F4
  {
    .custom /*0C000215:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorPage::get_Model

  .method /*06000157*/ public hidebysig specialname 
          instance void  set_Model(class Microsoft.AspNetCore.Hosting.Views.ErrorPageModel/*0200003D*/ 'value') cil managed
  // SIG: 20 01 01 12 80 F4
  {
    .custom /*0C000216:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorPage::set_Model

  .property /*17000061*/ instance callconv(8) class Microsoft.AspNetCore.Hosting.Views.ErrorPageModel/*0200003D*/
          Model()
  {
    .get instance class Microsoft.AspNetCore.Hosting.Views.ErrorPageModel/*0200003D*/ Microsoft.AspNetCore.Hosting.Views.ErrorPage/*0200003C*/::get_Model() /* 06000156 */
    .set instance void Microsoft.AspNetCore.Hosting.Views.ErrorPage/*0200003C*/::set_Model(class Microsoft.AspNetCore.Hosting.Views.ErrorPageModel/*0200003D*/) /* 06000157 */
  } // end of property ErrorPage::Model
} // end of class Microsoft.AspNetCore.Hosting.Views.ErrorPage

.class /*0200003D*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.Views.ErrorPageModel
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000F3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000F4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000158*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.Extensions.StackTrace.Sources.ExceptionDetails/*02000007*/> errorDetails,
                               bool showRuntimeDetails,
                               string runtimeDisplayName,
                               string runtimeArchitecture,
                               string clrVersion,
                               string currentAssemblyVesion,
                               string operatingSystemDescription) cil managed
  // SIG: 20 07 01 15 12 75 01 12 1C 02 0E 0E 0E 0E 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorPageModel::.ctor

  .method /*06000159*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.Extensions.StackTrace.Sources.ExceptionDetails/*02000007*/> 
          get_ErrorDetails() cil managed
  // SIG: 20 00 15 12 75 01 12 1C
  {
    .custom /*0C000218:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorPageModel::get_ErrorDetails

  .method /*0600015A*/ public hidebysig specialname 
          instance bool  get_ShowRuntimeDetails() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000219:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorPageModel::get_ShowRuntimeDetails

  .method /*0600015B*/ public hidebysig specialname 
          instance string  get_RuntimeDisplayName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00021A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorPageModel::get_RuntimeDisplayName

  .method /*0600015C*/ public hidebysig specialname 
          instance string  get_RuntimeArchitecture() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00021B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorPageModel::get_RuntimeArchitecture

  .method /*0600015D*/ public hidebysig specialname 
          instance string  get_ClrVersion() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00021C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorPageModel::get_ClrVersion

  .method /*0600015E*/ public hidebysig specialname 
          instance string  get_CurrentAssemblyVesion() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00021D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorPageModel::get_CurrentAssemblyVesion

  .method /*0600015F*/ public hidebysig specialname 
          instance string  get_OperatingSystemDescription() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00021E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorPageModel::get_OperatingSystemDescription

  .property /*17000062*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.Extensions.StackTrace.Sources.ExceptionDetails/*02000007*/>
          ErrorDetails()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class Microsoft.Extensions.StackTrace.Sources.ExceptionDetails/*02000007*/> Microsoft.AspNetCore.Hosting.Views.ErrorPageModel/*0200003D*/::get_ErrorDetails() /* 06000159 */
  } // end of property ErrorPageModel::ErrorDetails
  .property /*17000063*/ instance callconv(8) bool
          ShowRuntimeDetails()
  {
    .get instance bool Microsoft.AspNetCore.Hosting.Views.ErrorPageModel/*0200003D*/::get_ShowRuntimeDetails() /* 0600015A */
  } // end of property ErrorPageModel::ShowRuntimeDetails
  .property /*17000064*/ instance callconv(8) string
          RuntimeDisplayName()
  {
    .get instance string Microsoft.AspNetCore.Hosting.Views.ErrorPageModel/*0200003D*/::get_RuntimeDisplayName() /* 0600015B */
  } // end of property ErrorPageModel::RuntimeDisplayName
  .property /*17000065*/ instance callconv(8) string
          RuntimeArchitecture()
  {
    .get instance string Microsoft.AspNetCore.Hosting.Views.ErrorPageModel/*0200003D*/::get_RuntimeArchitecture() /* 0600015C */
  } // end of property ErrorPageModel::RuntimeArchitecture
  .property /*17000066*/ instance callconv(8) string
          ClrVersion()
  {
    .get instance string Microsoft.AspNetCore.Hosting.Views.ErrorPageModel/*0200003D*/::get_ClrVersion() /* 0600015D */
  } // end of property ErrorPageModel::ClrVersion
  .property /*17000067*/ instance callconv(8) string
          CurrentAssemblyVesion()
  {
    .get instance string Microsoft.AspNetCore.Hosting.Views.ErrorPageModel/*0200003D*/::get_CurrentAssemblyVesion() /* 0600015E */
  } // end of property ErrorPageModel::CurrentAssemblyVesion
  .property /*17000068*/ instance callconv(8) string
          OperatingSystemDescription()
  {
    .get instance string Microsoft.AspNetCore.Hosting.Views.ErrorPageModel/*0200003D*/::get_OperatingSystemDescription() /* 0600015F */
  } // end of property ErrorPageModel::OperatingSystemDescription
} // end of class Microsoft.AspNetCore.Hosting.Views.ErrorPageModel

.class /*0200003E*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Hosting.Views.ErrorPageModelBuilder
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*06000160*/ public hidebysig static 
          class Microsoft.AspNetCore.Hosting.Views.ErrorPageModel/*0200003D*/ 
          CreateErrorPageModel(class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/ contentRootFileProvider,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*0100001F*/ logger,
                               bool showDetailedErrors,
                               class [System.Runtime/*23000001*/]System.Exception/*0100001C*/ exception) cil managed
  // SIG: 00 04 12 80 F4 12 79 12 7D 02 12 71
  {
    .custom /*0C00021F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000151*/ 
    .custom /*0C000212:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorPageModelBuilder::CreateErrorPageModel

} // end of class Microsoft.AspNetCore.Hosting.Views.ErrorPageModelBuilder

.class /*0200003F*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.StaticWebAssets.StaticWebAssetsLoader
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000F7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000F8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*020000A1*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00018D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*040000E8*/ public static initonly class Microsoft.AspNetCore.Hosting.StaticWebAssets.StaticWebAssetsLoader/*0200003F*//'<>c'/*020000A1*/ '<>9'
    .field /*040000E9*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<string,class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/> '<>9__1_0'
    .method /*0600028B*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*0600028C*/ assembly hidebysig 
            instance class [Microsoft.Extensions.FileProviders.Abstractions/*23000002*/]Microsoft.Extensions.FileProviders.IFileProvider/*0100001E*/ 
            '<UseStaticWebAssetsCore>b__1_0'(string contentRoot) cil managed
    // SIG: 20 01 12 79 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<UseStaticWebAssetsCore>b__1_0'

  } // end of class '<>c'

  .method /*06000161*/ public hidebysig static 
          void  UseStaticWebAssets(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostEnvironment/*01000070*/ environment,
                                   class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfiguration/*0100006F*/ configuration) cil managed
  // SIG: 00 02 01 12 81 C1 12 81 BD
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StaticWebAssetsLoader::UseStaticWebAssets

  .method /*06000162*/ assembly hidebysig static 
          void  UseStaticWebAssetsCore(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostEnvironment/*01000070*/ environment,
                                       class [System.Runtime/*23000001*/]System.IO.Stream/*0100003A*/ manifest) cil managed
  // SIG: 00 02 01 12 81 C1 12 80 E9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StaticWebAssetsLoader::UseStaticWebAssetsCore

  .method /*06000163*/ assembly hidebysig static 
          class [System.Runtime/*23000001*/]System.IO.Stream/*0100003A*/ 
          ResolveManifest(class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostEnvironment/*01000070*/ environment,
                          class [Microsoft.Extensions.Configuration.Abstractions/*23000010*/]Microsoft.Extensions.Configuration.IConfiguration/*0100006F*/ configuration) cil managed
  // SIG: 00 02 12 80 E9 12 81 C1 12 81 BD
  {
    .param [0]/*08000158*/ 
    .custom /*0C000217:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StaticWebAssetsLoader::ResolveManifest

  .method /*06000164*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StaticWebAssetsLoader::.ctor

} // end of class Microsoft.AspNetCore.Hosting.StaticWebAssets.StaticWebAssetsLoader

.class /*02000040*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.Server.Features.ServerAddressesFeature
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [Microsoft.AspNetCore.Hosting.Server.Abstractions/*23000014*/]Microsoft.AspNetCore.Hosting.Server.Features.IServerAddressesFeature/*0100009C*/
{
  .method /*06000165*/ public hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000071*/<string> 
          get_Addresses() cil managed
  // SIG: 20 00 15 12 81 C5 01 0E
  {
    .custom /*0C000221:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000222:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServerAddressesFeature::get_Addresses

  .method /*06000166*/ public hidebysig newslot specialname virtual final 
          instance bool  get_PreferHostingUrls() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000223:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServerAddressesFeature::get_PreferHostingUrls

  .method /*06000167*/ public hidebysig newslot specialname virtual final 
          instance void  set_PreferHostingUrls(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000224:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServerAddressesFeature::set_PreferHostingUrls

  .method /*06000168*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServerAddressesFeature::.ctor

  .property /*17000069*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000071*/<string>
          Addresses()
  {
    .custom /*0C000132:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000071*/<string> Microsoft.AspNetCore.Hosting.Server.Features.ServerAddressesFeature/*02000040*/::get_Addresses() /* 06000165 */
  } // end of property ServerAddressesFeature::Addresses
  .property /*1700006A*/ instance callconv(8) bool
          PreferHostingUrls()
  {
    .get instance bool Microsoft.AspNetCore.Hosting.Server.Features.ServerAddressesFeature/*02000040*/::get_PreferHostingUrls() /* 06000166 */
    .set instance void Microsoft.AspNetCore.Hosting.Server.Features.ServerAddressesFeature/*02000040*/::set_PreferHostingUrls(bool) /* 06000167 */
  } // end of property ServerAddressesFeature::PreferHostingUrls
} // end of class Microsoft.AspNetCore.Hosting.Server.Features.ServerAddressesFeature

.class /*02000041*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Hosting.Internal.StartupLinkerOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .field /*04000016*/ public static literal valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000061*/ Accessibility = int32(0x0000000B)
} // end of class Microsoft.AspNetCore.Hosting.Internal.StartupLinkerOptions

.class /*02000042*/ interface public abstract auto ansi Microsoft.AspNetCore.Hosting.Infrastructure.ISupportsConfigureWebHost
{
  .custom /*0C0000FB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000169*/ public hidebysig newslot abstract virtual 
          instance class [Microsoft.Extensions.Hosting.Abstractions/*23000007*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000042*/ 
          ConfigureWebHost(class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/> configure,
                           class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class Microsoft.Extensions.Hosting.WebHostBuilderOptions/*02000013*/> configureOptions) cil managed
  // SIG: 20 02 12 81 09 15 12 81 05 01 12 81 0D 15 12 81 05 01 12 4C
  {
    // Method begins at RVA 0x0
  } // end of method ISupportsConfigureWebHost::ConfigureWebHost

} // end of class Microsoft.AspNetCore.Hosting.Infrastructure.ISupportsConfigureWebHost

.class /*02000043*/ interface public abstract auto ansi Microsoft.AspNetCore.Hosting.Infrastructure.ISupportsStartup
{
  .custom /*0C0000FD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600016A*/ public hidebysig newslot abstract virtual 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          Configure(class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> configure) cil managed
  // SIG: 20 01 12 81 0D 15 12 81 05 01 12 81 8D
  {
    // Method begins at RVA 0x0
  } // end of method ISupportsStartup::Configure

  .method /*0600016B*/ public hidebysig newslot abstract virtual 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          Configure(class [System.Runtime/*23000001*/]System.Action`2/*01000044*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/> configure) cil managed
  // SIG: 20 01 12 81 0D 15 12 81 11 02 12 81 79 12 81 8D
  {
    // Method begins at RVA 0x0
  } // end of method ISupportsStartup::Configure

  .method /*0600016C*/ public hidebysig newslot abstract virtual 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          UseStartup(class [System.Runtime/*23000001*/]System.Type/*01000019*/ startupType) cil managed
  // SIG: 20 01 12 81 0D 12 65
  {
    .param [1]/*08000160*/ 
    .custom /*0C000220:0A00002A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000062*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000061*/) /* 0A00002A */ = ( 01 00 0B 00 00 00 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ISupportsStartup::UseStartup

  .method /*0600016D*/ public hidebysig newslot abstract virtual 
          instance class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*01000043*/ 
          UseStartup<TStartup>(class [System.Runtime/*23000001*/]System.Func`2/*0100002A*/<class [Microsoft.AspNetCore.Hosting.Abstractions/*23000008*/]Microsoft.AspNetCore.Hosting.WebHostBuilderContext/*0100005E*/,!!TStartup> startupFactory) cil managed
  // SIG: 30 01 01 12 81 0D 15 12 80 A9 02 12 81 79 1E 00
  {
    .param type TStartup /*2A000010*/ 
      .custom /*0C000059:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .custom /*0C00005A:0A00002A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000062*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000061*/) /* 0A00002A */ = ( 01 00 08 00 00 00 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ISupportsStartup::UseStartup

} // end of class Microsoft.AspNetCore.Hosting.Infrastructure.ISupportsStartup

.class /*02000044*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Hosting.Builder.ApplicationBuilderFactory
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.Hosting.Builder.IApplicationBuilderFactory/*02000045*/
{
  .method /*0600016E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.ComponentModel/*2300000D*/]System.IServiceProvider/*01000056*/ serviceProvider) cil managed
  // SIG: 20 01 01 12 81 59
  {
    .custom /*0C000225:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ApplicationBuilderFactory::.ctor

  .method /*0600016F*/ public hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/ 
          CreateBuilder(class [Microsoft.Extensions.Features/*2300000E*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000058*/ serverFeatures) cil managed
  // SIG: 20 01 12 81 8D 12 81 61
  {
    .custom /*0C000226:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ApplicationBuilderFactory::CreateBuilder

} // end of class Microsoft.AspNetCore.Hosting.Builder.ApplicationBuilderFactory

.class /*02000045*/ interface public abstract auto ansi Microsoft.AspNetCore.Hosting.Builder.IApplicationBuilderFactory
{
  .custom /*0C000100:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000170*/ public hidebysig newslot abstract virtual 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000063*/ 
          CreateBuilder(class [Microsoft.Extensions.Features/*2300000E*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000058*/ serverFeatures) cil managed
  // SIG: 20 01 12 81 8D 12 81 61
  {
    // Method begins at RVA 0x0
  } // end of method IApplicationBuilderFactory::CreateBuilder

} // end of class Microsoft.AspNetCore.Hosting.Builder.IApplicationBuilderFactory


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
