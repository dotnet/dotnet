


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
// Time-date stamp:            0xb949f53d
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00003a00
// Size of init.data:              0x00000c00
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x000079e2
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
// Size of image:                  0x0000c000
// Size of headers:                0x00000400
// Checksum:                       0x0000b712
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00007990 [0x0000004f] address [size] of Import Directory:          
// 0x00008000 [0x00000618] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00004a00 [0x00002470] address [size] of Security Directory:        
// 0x0000a000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00007974 [0x0000001c] address [size] of Debug Directory:           
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
//              0x000039e8 Virtual Size
//              0x00004000 Virtual Address
//              0x00003a00 Size of Raw Data
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
//              0x00000618 Virtual Size
//              0x00008000 Virtual Address
//              0x00000800 Size of Raw Data
//              0x00004000 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x0000a000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x00004800 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00007000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x000009e4
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00004000 Import Address Table
//              0x000079d2 Import Name Table
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
// 0x00004054 [0x000038a0] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x000078f4 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00001ad4 Size
//              '#~' Name
//    Stream 2:
//              0x00001b40 Offset
//              0x000010e0 Size
//              '#Strings' Name
//    Stream 3:
//              0x00002c20 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00002c24 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00002c34 Offset
//              0x00000c6c Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000016890ba01757 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 28272
// PE header size       : 1024 (536 used)    ( 3.62%)
// PE additional info   : 11087             (39.22%)
// Num.of PE sections   : 4
// CLR header size     : 72                 ( 0.25%)
// CLR meta-data size  : 14496              (51.27%)
// CLR additional info : 128                ( 0.45%)
// CLR method headers  : 146                ( 0.52%)
// Managed code         : 292               ( 1.03%)
// Data                 : 3072              (10.87%)
// Unaccounted          : -2045             (-7.23%)

// Num.of PE sections   : 4
//   .mvid    - 512
//   .text    - 14848
//   .rsrc    - 2048
//   .reloc   - 512

// CLR meta-data size  : 14496
//   Module        -    1 (10 bytes)
//   TypeDef       -   52 (728 bytes)     7 interfaces, 0 explicit layout
//   TypeRef       -   51 (306 bytes)
//   MethodDef     -  157 (2198 bytes)    11 abstract, 0 native, 146 bodies
//   FieldDef      -   61 (366 bytes)     2 constant
//   MemberRef     -   25 (150 bytes)
//   ParamDef      -  136 (816 bytes)
//   MethodImpl    -   14 (84 bytes)
//   CustomAttribute-  208 (1248 bytes)
//   InterfaceImpl -   24 (96 bytes)
//   PropertyMap   -   13 (52 bytes)
//   Property      -   30 (180 bytes)
//   MethodSemantic-   39 (234 bytes)
//   TypeSpec      -    4 (8 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    6 (120 bytes)
//   ExportedType  -    2 (28 bytes)
//   NestedClass   -   13 (52 bytes)
//   GenericParam  -    6 (48 bytes)
//   GenericParamConstraint-    4 (16 bytes)
//   Strings       -  4318 bytes
//   Blobs         -  3180 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   216 bytes

// CLR additional info : 128

// CLR method headers : 146
//   Num.of method bodies  - 146
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 146

// Managed code : 292
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class LoggingExtensions              (abstract) (auto) (ansi) (sealed) 
// Class AuthorizationServiceCollectionExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class AllowAnonymousAttribute        (public) (auto) (ansi) 
// Class AuthorizationFailure           (public) (auto) (ansi) 
// Class AuthorizationFailureReason     (public) (auto) (ansi) 
// Class AuthorizationHandler`1         (public) (abstract) (auto) (ansi) 
// Class AuthorizationHandler`2         (public) (abstract) (auto) (ansi) 
// Class AuthorizationHandlerContext    (public) (auto) (ansi) 
// Class AuthorizationOptions           (public) (auto) (ansi) 
// Class AuthorizationPolicy            (public) (auto) (ansi) 
// Class AuthorizationPolicyBuilder     (public) (auto) (ansi) 
// Class AuthorizationResult            (public) (auto) (ansi) 
// Class AuthorizationServiceExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class AuthorizeAttribute             (public) (auto) (ansi) 
// Class DefaultAuthorizationEvaluator  (public) (auto) (ansi) 
// Class DefaultAuthorizationHandlerContextFactory (public) (auto) (ansi) 
// Class DefaultAuthorizationHandlerProvider (public) (auto) (ansi) 
// Class DefaultAuthorizationPolicyProvider (public) (auto) (ansi) 
// Class DefaultAuthorizationService    (public) (auto) (ansi) 
// Interface IAuthorizationEvaluator        (public) (abstract) (auto) (ansi) 
// Interface IAuthorizationHandler          (public) (abstract) (auto) (ansi) 
// Interface IAuthorizationHandlerContextFactory (public) (abstract) (auto) (ansi) 
// Interface IAuthorizationHandlerProvider  (public) (abstract) (auto) (ansi) 
// Interface IAuthorizationPolicyProvider   (public) (abstract) (auto) (ansi) 
// Interface IAuthorizationRequirement      (public) (abstract) (auto) (ansi) 
// Interface IAuthorizationService          (public) (abstract) (auto) (ansi) 
// Class Resources                      (abstract) (auto) (ansi) (sealed) 
// Class AssertionRequirement           (public) (auto) (ansi) 
// Class ClaimsAuthorizationRequirement (public) (auto) (ansi) 
// Class DenyAnonymousAuthorizationRequirement (public) (auto) (ansi) 
// Class NameAuthorizationRequirement   (public) (auto) (ansi) 
// Class OperationAuthorizationRequirement (public) (auto) (ansi) 
// Class PassThroughAuthorizationHandler (public) (auto) (ansi) 
// Class RolesAuthorizationRequirement  (public) (auto) (ansi) 
// Class <HandleAsync>d__0              (auto) (ansi) (sealed) (nested private) 
// Class <HandleAsync>d__0              (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <CombineAsync>d__9             (auto) (ansi) (sealed) (nested private) 
// Class <AuthorizeAsync>d__7           (auto) (ansi) (sealed) (nested private) 
// Class <AuthorizeAsync>d__8           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass3_0           (auto) (ansi) (sealed) (nested private) 
// Class <HandleAsync>d__5              (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass7_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass4_0           (auto) (ansi) (sealed) (nested private) 
// Class <HandleAsync>d__0              (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass4_0           (auto) (ansi) (sealed) (nested private) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ Microsoft.Extensions.Logging.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ Microsoft.Extensions.DependencyInjection.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ Microsoft.AspNetCore.Metadata
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000005*/ System.Security.Claims
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000006*/ Microsoft.Extensions.Options
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly /*20000001*/ Microsoft.AspNetCore.Authorization
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000002:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000003:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000002*/::.ctor(int32) /* 0A000002 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000004:0A000003*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000003*/::.ctor() /* 0A000003 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000005:0A000004*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*//DebuggingModes/*01000005*/) /* 0A000004 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000006:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000007:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C000008:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0A 43 6F 6D 6D 69 74 48 61 73 68 28 31 64   // ...CommitHash(1d
                                                                                                                                                                    63 66 37 61 63 66 61 63 66 30 66 65 31 35 34 61   // cf7acfacf0fe154a
                                                                                                                                                                    64 63 63 32 33 32 37 30 63 62 30 64 61 31 31 66   // dcc23270cb0da11f
                                                                                                                                                                    66 34 34 61 63 65 00 00 )                         // f44ace..
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0F 53 6F 75 72 63 65 43 6F 6D 6D 69 74 55   // ...SourceCommitU
                                                                                                                                                                    72 6C 52 68 74 74 70 73 3A 2F 2F 67 69 74 68 75   // rlRhttps://githu
                                                                                                                                                                    62 2E 63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70   // b.com/dotnet/asp
                                                                                                                                                                    6E 65 74 63 6F 72 65 2F 74 72 65 65 2F 31 64 63   // netcore/tree/1dc
                                                                                                                                                                    66 37 61 63 66 61 63 66 30 66 65 31 35 34 61 64   // f7acfacf0fe154ad
                                                                                                                                                                    63 63 32 33 32 37 30 63 62 30 64 61 31 31 66 66   // cc23270cb0da11ff
                                                                                                                                                                    34 34 61 63 65 00 00 )                            // 44ace..
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyConfigurationAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 07 52 65 6C 65 61 73 65 00 00 )             // ...Release..
  .custom /*0C00000E:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000F:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 80 AC 41 53 50 2E 4E 45 54 20 43 6F 72 65   // ....ASP.NET Core
                                                                                                                                                                       20 61 75 74 68 6F 72 69 7A 61 74 69 6F 6E 20 63   //  authorization c
                                                                                                                                                                       6C 61 73 73 65 73 2E 0D 0A 43 6F 6D 6D 6F 6E 6C   // lasses...Commonl
                                                                                                                                                                       79 20 75 73 65 64 20 74 79 70 65 73 3A 0D 0A 4D   // y used types:..M
                                                                                                                                                                       69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43   // icrosoft.AspNetC
                                                                                                                                                                       6F 72 65 2E 41 75 74 68 6F 72 69 7A 61 74 69 6F   // ore.Authorizatio
                                                                                                                                                                       6E 2E 41 6C 6C 6F 77 41 6E 6F 6E 79 6D 6F 75 73   // n.AllowAnonymous
                                                                                                                                                                       41 74 74 72 69 62 75 74 65 0D 0A 4D 69 63 72 6F   // Attribute..Micro
                                                                                                                                                                       73 6F 66 74 2E 41 73 70 4E 65 74 43 6F 72 65 2E   // soft.AspNetCore.
                                                                                                                                                                       41 75 74 68 6F 72 69 7A 61 74 69 6F 6E 2E 41 75   // Authorization.Au
                                                                                                                                                                       74 68 6F 72 69 7A 65 41 74 74 72 69 62 75 74 65   // thorizeAttribute
                                                                                                                                                                       00 00 ) 
  .custom /*0C000010:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 36 2E 30 2E 32 32 32 2E 36 34 31 32 00   // ...6.0.222.6412.
                                                                                                                                                                       00 ) 
  .custom /*0C000011:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 36 2E 30 2E 32 2B 31 64 63 66 37 61 63   // ...6.0.2+1dcf7ac
                                                                                                                                                                                66 61 63 66 30 66 65 31 35 34 61 64 63 63 32 33   // facf0fe154adcc23
                                                                                                                                                                                32 37 30 63 62 30 64 61 31 31 66 66 34 34 61 63   // 270cb0da11ff44ac
                                                                                                                                                                                65 00 00 )                                        // e..
  .custom /*0C000012:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 16 4D 69 63 72 6F 73 6F 66 74 20 41 53 50   // ...Microsoft ASP
                                                                                                                                                                   2E 4E 45 54 20 43 6F 72 65 00 00 )                // .NET Core..
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 22 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // .."Microsoft.Asp
                                                                                                                                                                 4E 65 74 43 6F 72 65 2E 41 75 74 68 6F 72 69 7A   // NetCore.Authoriz
                                                                                                                                                                 61 74 69 6F 6E 00 00 )                            // ation..
  .custom /*0C000014:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    24 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // $https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70 6E 65   // com/dotnet/aspne
                                                                                                                                                                    74 63 6F 72 65 00 00 )                            // tcore..
  .custom /*0C000015:0A000010*/ instance void [System.Runtime/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
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
.class extern /*27000001*/ forwarder Microsoft.AspNetCore.Authorization.IAllowAnonymous
{
  .assembly extern Microsoft.AspNetCore.Metadata /*23000004*/ 
}
.class extern /*27000002*/ forwarder Microsoft.AspNetCore.Authorization.IAuthorizeData
{
  .assembly extern Microsoft.AspNetCore.Metadata /*23000004*/ 
}
.module Microsoft.AspNetCore.Authorization.dll
// MVID: {6adb9ab6-7f51-4b69-9a76-6a370cae3c40}
.custom /*0C000001:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000017:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000018:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C00001A:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C00001B:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001C:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C00001F:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000020:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000021:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000024:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000025:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000026:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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

.class /*02000006*/ private abstract auto ansi sealed beforefieldinit Microsoft.Extensions.Logging.LoggingExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C00002A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000006*/ public hidebysig static 
          void  UserAuthorizationSucceeded(class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger/*0100001A*/ logger) cil managed
  // SIG: 00 01 01 12 69
  {
    .custom /*0C000027:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000028:0A000013*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*01000018*/::.ctor(int32,
                                                                                                                                                                              valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000017*/,
                                                                                                                                                                              string) /* 0A000013 */ = ( 01 00 01 00 00 00 01 00 00 00 1D 41 75 74 68 6F   // ...........Autho
                                                                                                                                                                                                         72 69 7A 61 74 69 6F 6E 20 77 61 73 20 73 75 63   // rization was suc
                                                                                                                                                                                                         63 65 73 73 66 75 6C 2E 01 00 54 0E 09 45 76 65   // cessful...T..Eve
                                                                                                                                                                                                         6E 74 4E 61 6D 65 1A 55 73 65 72 41 75 74 68 6F   // ntName.UserAutho
                                                                                                                                                                                                         72 69 7A 61 74 69 6F 6E 53 75 63 63 65 65 64 65   // rizationSucceede
                                                                                                                                                                                                         64 )                                              // d
    .custom /*0C000029:0A000014*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*01000019*/::.ctor(string,
                                                                                                                                              string) /* 0A000014 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                         65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                         47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                         2E 36 34 30 36 00 00 )                            // .6406..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggingExtensions::UserAuthorizationSucceeded

  .method /*06000007*/ public hidebysig static 
          void  UserAuthorizationFailed(class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger/*0100001A*/ logger,
                                        class Microsoft.AspNetCore.Authorization.AuthorizationFailure/*02000009*/ failure) cil managed
  // SIG: 00 02 01 12 69 12 24
  {
    .custom /*0C00002C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggingExtensions::UserAuthorizationFailed

} // end of class Microsoft.Extensions.Logging.LoggingExtensions

.class /*02000007*/ public abstract auto ansi sealed beforefieldinit Microsoft.Extensions.DependencyInjection.AuthorizationServiceCollectionExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C00002D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00002F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000008*/ public hidebysig static 
          class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*0100001B*/ 
          AddAuthorizationCore(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*0100001B*/ services) cil managed
  // SIG: 00 01 12 6D 12 6D
  {
    .custom /*0C000030:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationServiceCollectionExtensions::AddAuthorizationCore

  .method /*06000009*/ public hidebysig static 
          class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*0100001B*/ 
          AddAuthorizationCore(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*0100001B*/ services,
                               class [System.Runtime/*23000001*/]System.Action`1/*0100001C*/<class Microsoft.AspNetCore.Authorization.AuthorizationOptions/*0200000E*/> configure) cil managed
  // SIG: 00 02 12 6D 12 6D 15 12 71 01 12 38
  {
    .custom /*0C000034:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationServiceCollectionExtensions::AddAuthorizationCore

} // end of class Microsoft.Extensions.DependencyInjection.AuthorizationServiceCollectionExtensions

.class /*02000008*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
       implements [Microsoft.AspNetCore.Metadata/*23000004*/]Microsoft.AspNetCore.Authorization.IAllowAnonymous/*0100001D*/
{
  .custom /*0C000031:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 44 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..D.....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 01 )                               // rited.
  .method /*0600000A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AllowAnonymousAttribute::.ctor

} // end of class Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute

.class /*02000009*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.AuthorizationFailure
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000035:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600000B*/ public hidebysig specialname 
          instance bool  get_FailCalled() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00003B:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationFailure::get_FailCalled

  .method /*0600000C*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/> 
          get_FailedRequirements() cil managed
  // SIG: 20 00 15 12 79 01 12 78
  {
    .custom /*0C00003D:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationFailure::get_FailedRequirements

  .method /*0600000D*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.AuthorizationFailureReason/*0200000A*/> 
          get_FailureReasons() cil managed
  // SIG: 20 00 15 12 79 01 12 28
  {
    .custom /*0C00003F:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationFailure::get_FailureReasons

  .method /*0600000E*/ public hidebysig static 
          class Microsoft.AspNetCore.Authorization.AuthorizationFailure/*02000009*/ 
          ExplicitFail() cil managed
  // SIG: 00 00 12 24
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationFailure::ExplicitFail

  .method /*0600000F*/ public hidebysig static 
          class Microsoft.AspNetCore.Authorization.AuthorizationFailure/*02000009*/ 
          Failed(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.AuthorizationFailureReason/*0200000A*/> reasons) cil managed
  // SIG: 00 01 12 24 15 12 79 01 12 28
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationFailure::Failed

  .method /*06000010*/ public hidebysig static 
          class Microsoft.AspNetCore.Authorization.AuthorizationFailure/*02000009*/ 
          Failed(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/> failed) cil managed
  // SIG: 00 01 12 24 15 12 79 01 12 78
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationFailure::Failed

  .property /*17000001*/ instance callconv(8) bool
          FailCalled()
  {
    .get instance bool Microsoft.AspNetCore.Authorization.AuthorizationFailure/*02000009*/::get_FailCalled() /* 0600000B */
  } // end of property AuthorizationFailure::FailCalled
  .property /*17000002*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/>
          FailedRequirements()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/> Microsoft.AspNetCore.Authorization.AuthorizationFailure/*02000009*/::get_FailedRequirements() /* 0600000C */
  } // end of property AuthorizationFailure::FailedRequirements
  .property /*17000003*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.AuthorizationFailureReason/*0200000A*/>
          FailureReasons()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.AuthorizationFailureReason/*0200000A*/> Microsoft.AspNetCore.Authorization.AuthorizationFailure/*02000009*/::get_FailureReasons() /* 0600000D */
  } // end of property AuthorizationFailure::FailureReasons
} // end of class Microsoft.AspNetCore.Authorization.AuthorizationFailure

.class /*0200000A*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.AuthorizationFailureReason
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000038:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000039:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000011*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Authorization.IAuthorizationHandler/*0200001A*/ 'handler',
                               string message) cil managed
  // SIG: 20 02 01 12 68 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationFailureReason::.ctor

  .method /*06000012*/ public hidebysig specialname 
          instance string  get_Message() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00004F:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationFailureReason::get_Message

  .method /*06000013*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Authorization.IAuthorizationHandler/*0200001A*/ 
          get_Handler() cil managed
  // SIG: 20 00 12 68
  {
    .custom /*0C000053:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationFailureReason::get_Handler

  .property /*17000004*/ instance callconv(8) string
          Message()
  {
    .get instance string Microsoft.AspNetCore.Authorization.AuthorizationFailureReason/*0200000A*/::get_Message() /* 06000012 */
  } // end of property AuthorizationFailureReason::Message
  .property /*17000005*/ instance callconv(8) class Microsoft.AspNetCore.Authorization.IAuthorizationHandler/*0200001A*/
          Handler()
  {
    .get instance class Microsoft.AspNetCore.Authorization.IAuthorizationHandler/*0200001A*/ Microsoft.AspNetCore.Authorization.AuthorizationFailureReason/*0200000A*/::get_Handler() /* 06000013 */
  } // end of property AuthorizationFailureReason::Handler
} // end of class Microsoft.AspNetCore.Authorization.AuthorizationFailureReason

.class /*0200000B*/ public abstract auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.AuthorizationHandler`1<(Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/) TRequirement>
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements Microsoft.AspNetCore.Authorization.IAuthorizationHandler/*0200001A*/
{
  .param constraint   TRequirement/*2A000001*/ , Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/
    .custom /*0C000016:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  .class /*02000028*/ auto ansi sealed nested private beforefieldinit '<HandleAsync>d__0'<(Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/) TRequirement>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000022*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/
  {
    .custom /*0C000082:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .param constraint   TRequirement/*2A000004*/ , Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/
      .custom /*0C00001E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .field /*04000004*/ public int32 '<>1__state'
    .field /*04000005*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000024*/ '<>t__builder'
    .field /*04000006*/ public class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ context
    .field /*04000007*/ public class Microsoft.AspNetCore.Authorization.AuthorizationHandler`1/*0200000B*/<!TRequirement> '<>4__this'
    .field /*04000008*/ private class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<!TRequirement> '<>7__wrap1'
    .field /*04000009*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000026*/ '<>u__1'
    .method /*0600008A*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/::MoveNext /*01000023::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<HandleAsync>d__0'::MoveNext

    .method /*0600008B*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 8D
    {
      .custom /*0C0000CA:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000027*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/::SetStateMachine /*01000023::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<HandleAsync>d__0'::SetStateMachine

  } // end of class '<HandleAsync>d__0'

  .method /*06000014*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000021*/ 
          HandleAsync(class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ context) cil managed
  // SIG: 20 01 12 80 85 12 34
  {
    .custom /*0C000057:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000058:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000020*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100001F*/) /* 0A000015 */ = ( 01 00 4B 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..KMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 41 75 74 68 6F 72 69 7A   // NetCore.Authoriz
                                                                                                                                                                                                                                        61 74 69 6F 6E 2E 41 75 74 68 6F 72 69 7A 61 74   // ation.Authorizat
                                                                                                                                                                                                                                        69 6F 6E 48 61 6E 64 6C 65 72 60 31 2B 3C 48 61   // ionHandler`1+<Ha
                                                                                                                                                                                                                                        6E 64 6C 65 41 73 79 6E 63 3E 64 5F 5F 30 00 00 ) // ndleAsync>d__0..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationHandler`1::HandleAsync

  .method /*06000015*/ family hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000021*/ 
          HandleRequirementAsync(class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ context,
                                 !TRequirement requirement) cil managed
  // SIG: 20 02 12 80 85 12 34 13 00
  {
    .custom /*0C00005A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method AuthorizationHandler`1::HandleRequirementAsync

  .method /*06000016*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationHandler`1::.ctor

} // end of class Microsoft.AspNetCore.Authorization.AuthorizationHandler`1

.class /*0200000C*/ public abstract auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.AuthorizationHandler`2<(Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/) TRequirement,TResource>
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements Microsoft.AspNetCore.Authorization.IAuthorizationHandler/*0200001A*/
{
  .param constraint   TRequirement/*2A000002*/ , Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/
    .custom /*0C000019:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  .param type TResource /*2A000003*/ 
    .custom /*0C00001D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .class /*02000029*/ auto ansi sealed nested private beforefieldinit '<HandleAsync>d__0'<(Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/) TRequirement,TResource>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000022*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/
  {
    .custom /*0C000085:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .param constraint   TRequirement/*2A000005*/ , Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/
      .custom /*0C000023:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param type TResource /*2A000006*/ 
      .custom /*0C00002B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .field /*0400000A*/ public int32 '<>1__state'
    .field /*0400000B*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000024*/ '<>t__builder'
    .field /*0400000C*/ public class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ context
    .field /*0400000D*/ public class Microsoft.AspNetCore.Authorization.AuthorizationHandler`2/*0200000C*/<!TRequirement,!TResource> '<>4__this'
    .field /*0400000E*/ private class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<!TRequirement> '<>7__wrap1'
    .field /*0400000F*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000026*/ '<>u__1'
    .method /*0600008C*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/::MoveNext /*01000023::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<HandleAsync>d__0'::MoveNext

    .method /*0600008D*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 8D
    {
      .custom /*0C0000CB:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000027*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/::SetStateMachine /*01000023::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<HandleAsync>d__0'::SetStateMachine

  } // end of class '<HandleAsync>d__0'

  .method /*06000017*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000021*/ 
          HandleAsync(class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ context) cil managed
  // SIG: 20 01 12 80 85 12 34
  {
    .custom /*0C00005C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00005D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000020*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100001F*/) /* 0A000015 */ = ( 01 00 4B 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..KMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 41 75 74 68 6F 72 69 7A   // NetCore.Authoriz
                                                                                                                                                                                                                                        61 74 69 6F 6E 2E 41 75 74 68 6F 72 69 7A 61 74   // ation.Authorizat
                                                                                                                                                                                                                                        69 6F 6E 48 61 6E 64 6C 65 72 60 32 2B 3C 48 61   // ionHandler`2+<Ha
                                                                                                                                                                                                                                        6E 64 6C 65 41 73 79 6E 63 3E 64 5F 5F 30 00 00 ) // ndleAsync>d__0..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationHandler`2::HandleAsync

  .method /*06000018*/ family hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000021*/ 
          HandleRequirementAsync(class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ context,
                                 !TRequirement requirement,
                                 !TResource resource) cil managed
  // SIG: 20 03 12 80 85 12 34 13 00 13 01
  {
    .custom /*0C000060:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method AuthorizationHandler`2::HandleRequirementAsync

  .method /*06000019*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationHandler`2::.ctor

} // end of class Microsoft.AspNetCore.Authorization.AuthorizationHandler`2

.class /*0200000D*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000040:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000041:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600001A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/> requirements,
                               class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000028*/ user,
                               object resource) cil managed
  // SIG: 20 03 01 15 12 79 01 12 78 12 80 A1 1C
  {
    .param [3]/*08000014*/ 
    .custom /*0C000059:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationHandlerContext::.ctor

  .method /*0600001B*/ public hidebysig newslot specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/> 
          get_Requirements() cil managed
  // SIG: 20 00 15 12 79 01 12 78
  {
    .custom /*0C000065:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationHandlerContext::get_Requirements

  .method /*0600001C*/ public hidebysig newslot specialname virtual 
          instance class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000028*/ 
          get_User() cil managed
  // SIG: 20 00 12 80 A1
  {
    .custom /*0C000068:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationHandlerContext::get_User

  .method /*0600001D*/ public hidebysig newslot specialname virtual 
          instance object  get_Resource() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C00006A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00006B:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationHandlerContext::get_Resource

  .method /*0600001E*/ public hidebysig newslot specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/> 
          get_PendingRequirements() cil managed
  // SIG: 20 00 15 12 79 01 12 78
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationHandlerContext::get_PendingRequirements

  .method /*0600001F*/ public hidebysig newslot specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.AuthorizationFailureReason/*0200000A*/> 
          get_FailureReasons() cil managed
  // SIG: 20 00 15 12 79 01 12 28
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationHandlerContext::get_FailureReasons

  .method /*06000020*/ public hidebysig newslot specialname virtual 
          instance bool  get_HasFailed() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationHandlerContext::get_HasFailed

  .method /*06000021*/ public hidebysig newslot specialname virtual 
          instance bool  get_HasSucceeded() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationHandlerContext::get_HasSucceeded

  .method /*06000022*/ public hidebysig newslot virtual 
          instance void  Fail() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationHandlerContext::Fail

  .method /*06000023*/ public hidebysig newslot virtual 
          instance void  Fail(class Microsoft.AspNetCore.Authorization.AuthorizationFailureReason/*0200000A*/ reason) cil managed
  // SIG: 20 01 01 12 28
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationHandlerContext::Fail

  .method /*06000024*/ public hidebysig newslot virtual 
          instance void  Succeed(class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/ requirement) cil managed
  // SIG: 20 01 01 12 78
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationHandlerContext::Succeed

  .property /*17000006*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/>
          Requirements()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/> Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/::get_Requirements() /* 0600001B */
  } // end of property AuthorizationHandlerContext::Requirements
  .property /*17000007*/ instance callconv(8) class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000028*/
          User()
  {
    .get instance class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000028*/ Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/::get_User() /* 0600001C */
  } // end of property AuthorizationHandlerContext::User
  .property /*17000008*/ instance callconv(8) object
          Resource()
  {
    .custom /*0C000033:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance object Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/::get_Resource() /* 0600001D */
  } // end of property AuthorizationHandlerContext::Resource
  .property /*17000009*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/>
          PendingRequirements()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/> Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/::get_PendingRequirements() /* 0600001E */
  } // end of property AuthorizationHandlerContext::PendingRequirements
  .property /*1700000A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.AuthorizationFailureReason/*0200000A*/>
          FailureReasons()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.AuthorizationFailureReason/*0200000A*/> Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/::get_FailureReasons() /* 0600001F */
  } // end of property AuthorizationHandlerContext::FailureReasons
  .property /*1700000B*/ instance callconv(8) bool
          HasFailed()
  {
    .get instance bool Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/::get_HasFailed() /* 06000020 */
  } // end of property AuthorizationHandlerContext::HasFailed
  .property /*1700000C*/ instance callconv(8) bool
          HasSucceeded()
  {
    .get instance bool Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/::get_HasSucceeded() /* 06000021 */
  } // end of property AuthorizationHandlerContext::HasSucceeded
} // end of class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext

.class /*0200000E*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.AuthorizationOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000043:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000044:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000025*/ public hidebysig specialname 
          instance bool  get_InvokeHandlersAfterFailure() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000079:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationOptions::get_InvokeHandlersAfterFailure

  .method /*06000026*/ public hidebysig specialname 
          instance void  set_InvokeHandlersAfterFailure(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C00007C:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationOptions::set_InvokeHandlersAfterFailure

  .method /*06000027*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/ 
          get_DefaultPolicy() cil managed
  // SIG: 20 00 12 3C
  {
    .custom /*0C00007D:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationOptions::get_DefaultPolicy

  .method /*06000028*/ public hidebysig specialname 
          instance void  set_DefaultPolicy(class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/ 'value') cil managed
  // SIG: 20 01 01 12 3C
  {
    .custom /*0C000081:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationOptions::set_DefaultPolicy

  .method /*06000029*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/ 
          get_FallbackPolicy() cil managed
  // SIG: 20 00 12 3C
  {
    .custom /*0C000083:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000084:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationOptions::get_FallbackPolicy

  .method /*0600002A*/ public hidebysig specialname 
          instance void  set_FallbackPolicy(class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/ 'value') cil managed
  // SIG: 20 01 01 12 3C
  {
    .custom /*0C000086:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000087:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationOptions::set_FallbackPolicy

  .method /*0600002B*/ public hidebysig instance void 
          AddPolicy(string name,
                    class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/ policy) cil managed
  // SIG: 20 02 01 0E 12 3C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationOptions::AddPolicy

  .method /*0600002C*/ public hidebysig instance void 
          AddPolicy(string name,
                    class [System.Runtime/*23000001*/]System.Action`1/*0100001C*/<class Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder/*02000010*/> configurePolicy) cil managed
  // SIG: 20 02 01 0E 15 12 71 01 12 40
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationOptions::AddPolicy

  .method /*0600002D*/ public hidebysig instance class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/ 
          GetPolicy(string name) cil managed
  // SIG: 20 01 12 3C 0E
  {
    .param [0]/*0800001E*/ 
    .custom /*0C00006D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationOptions::GetPolicy

  .method /*0600002E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationOptions::.ctor

  .property /*1700000D*/ instance callconv(8) bool
          InvokeHandlersAfterFailure()
  {
    .get instance bool Microsoft.AspNetCore.Authorization.AuthorizationOptions/*0200000E*/::get_InvokeHandlersAfterFailure() /* 06000025 */
    .set instance void Microsoft.AspNetCore.Authorization.AuthorizationOptions/*0200000E*/::set_InvokeHandlersAfterFailure(bool) /* 06000026 */
  } // end of property AuthorizationOptions::InvokeHandlersAfterFailure
  .property /*1700000E*/ instance callconv(8) class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/
          DefaultPolicy()
  {
    .get instance class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/ Microsoft.AspNetCore.Authorization.AuthorizationOptions/*0200000E*/::get_DefaultPolicy() /* 06000027 */
    .set instance void Microsoft.AspNetCore.Authorization.AuthorizationOptions/*0200000E*/::set_DefaultPolicy(class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/) /* 06000028 */
  } // end of property AuthorizationOptions::DefaultPolicy
  .property /*1700000F*/ instance callconv(8) class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/
          FallbackPolicy()
  {
    .custom /*0C000049:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/ Microsoft.AspNetCore.Authorization.AuthorizationOptions/*0200000E*/::get_FallbackPolicy() /* 06000029 */
    .set instance void Microsoft.AspNetCore.Authorization.AuthorizationOptions/*0200000E*/::set_FallbackPolicy(class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/) /* 0600002A */
  } // end of property AuthorizationOptions::FallbackPolicy
} // end of class Microsoft.AspNetCore.Authorization.AuthorizationOptions

.class /*0200000F*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.AuthorizationPolicy
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000046:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000047:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200002A*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C000088:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*04000010*/ public static initonly class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*//'<>c'/*0200002A*/ '<>9'
    .field /*04000011*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100002D*/<string,bool> '<>9__9_0'
    .field /*04000012*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100002D*/<string,string> '<>9__9_1'
    .method /*0600008E*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

  } // end of class '<>c'

  .class /*0200002B*/ auto ansi sealed nested private beforefieldinit '<CombineAsync>d__9'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000022*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/
  {
    .custom /*0C000089:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*04000013*/ public int32 '<>1__state'
    .field /*04000014*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100002E*/<class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/> '<>t__builder'
    .field /*04000015*/ public class Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider/*0200001D*/ policyProvider
    .field /*04000016*/ public class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class [Microsoft.AspNetCore.Metadata/*23000004*/]Microsoft.AspNetCore.Authorization.IAuthorizeData/*0100002C*/> authorizeData
    .field /*04000017*/ private class Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder/*02000010*/ '<policyBuilder>5__2'
    .field /*04000018*/ private class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<class [Microsoft.AspNetCore.Metadata/*23000004*/]Microsoft.AspNetCore.Authorization.IAuthorizeData/*0100002C*/> '<>7__wrap2'
    .field /*04000019*/ private class [Microsoft.AspNetCore.Metadata/*23000004*/]Microsoft.AspNetCore.Authorization.IAuthorizeData/*0100002C*/ '<authorizeDatum>5__4'
    .field /*0400001A*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*0100002F*/<class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/> '<>u__1'
    .field /*0400001B*/ private class Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder/*02000010*/ '<>7__wrap4'
    .method /*0600008F*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/::MoveNext /*01000023::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<CombineAsync>d__9'::MoveNext

    .method /*06000090*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 8D
    {
      .custom /*0C0000CC:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000027*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/::SetStateMachine /*01000023::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<CombineAsync>d__9'::SetStateMachine

  } // end of class '<CombineAsync>d__9'

  .method /*0600002F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/> requirements,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string> authenticationSchemes) cil managed
  // SIG: 20 02 01 15 12 79 01 12 78 15 12 79 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicy::.ctor

  .method /*06000030*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*01000029*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/> 
          get_Requirements() cil managed
  // SIG: 20 00 15 12 80 A5 01 12 78
  {
    .custom /*0C000091:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicy::get_Requirements

  .method /*06000031*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*01000029*/<string> 
          get_AuthenticationSchemes() cil managed
  // SIG: 20 00 15 12 80 A5 01 0E
  {
    .custom /*0C000093:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicy::get_AuthenticationSchemes

  .method /*06000032*/ public hidebysig static 
          class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/ 
          Combine(class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/[] policies) cil managed
  // SIG: 00 01 12 3C 1D 12 3C
  {
    .param [1]/*08000022*/ 
    .custom /*0C000073:0A000019*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicy::Combine

  .method /*06000033*/ public hidebysig static 
          class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/ 
          Combine(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/> policies) cil managed
  // SIG: 00 01 12 3C 15 12 79 01 12 3C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicy::Combine

  .method /*06000034*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/> 
          CombineAsync(class Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider/*0200001D*/ policyProvider,
                       class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class [Microsoft.AspNetCore.Metadata/*23000004*/]Microsoft.AspNetCore.Authorization.IAuthorizeData/*0100002C*/> authorizeData) cil managed
  // SIG: 00 02 15 12 80 AD 01 12 3C 12 74 15 12 79 01 12 80 B1
  {
    .custom /*0C000098:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000020*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100001F*/) /* 0A000015 */ = ( 01 00 49 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..IMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 41 75 74 68 6F 72 69 7A   // NetCore.Authoriz
                                                                                                                                                                                                                                        61 74 69 6F 6E 2E 41 75 74 68 6F 72 69 7A 61 74   // ation.Authorizat
                                                                                                                                                                                                                                        69 6F 6E 50 6F 6C 69 63 79 2B 3C 43 6F 6D 62 69   // ionPolicy+<Combi
                                                                                                                                                                                                                                        6E 65 41 73 79 6E 63 3E 64 5F 5F 39 00 00 )       // neAsync>d__9..
    .param [0]/*08000024*/ 
    .custom /*0C000078:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicy::CombineAsync

  .property /*17000010*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*01000029*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/>
          Requirements()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*01000029*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/> Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/::get_Requirements() /* 06000030 */
  } // end of property AuthorizationPolicy::Requirements
  .property /*17000011*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*01000029*/<string>
          AuthenticationSchemes()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*01000029*/<string> Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/::get_AuthenticationSchemes() /* 06000031 */
  } // end of property AuthorizationPolicy::AuthenticationSchemes
} // end of class Microsoft.AspNetCore.Authorization.AuthorizationPolicy

.class /*02000010*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C00004A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000035*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string[] authenticationSchemes) cil managed
  // SIG: 20 01 01 1D 0E
  {
    .param [1]/*08000027*/ 
    .custom /*0C000080:0A000019*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicyBuilder::.ctor

  .method /*06000036*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/ policy) cil managed
  // SIG: 20 01 01 12 3C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicyBuilder::.ctor

  .method /*06000037*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000030*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/> 
          get_Requirements() cil managed
  // SIG: 20 00 15 12 80 C1 01 12 78
  {
    .custom /*0C00009A:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicyBuilder::get_Requirements

  .method /*06000038*/ public hidebysig specialname 
          instance void  set_Requirements(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000030*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/> 'value') cil managed
  // SIG: 20 01 01 15 12 80 C1 01 12 78
  {
    .custom /*0C00009B:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicyBuilder::set_Requirements

  .method /*06000039*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000030*/<string> 
          get_AuthenticationSchemes() cil managed
  // SIG: 20 00 15 12 80 C1 01 0E
  {
    .custom /*0C00009C:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicyBuilder::get_AuthenticationSchemes

  .method /*0600003A*/ public hidebysig specialname 
          instance void  set_AuthenticationSchemes(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000030*/<string> 'value') cil managed
  // SIG: 20 01 01 15 12 80 C1 01 0E
  {
    .custom /*0C00009D:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicyBuilder::set_AuthenticationSchemes

  .method /*0600003B*/ public hidebysig instance class Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder/*02000010*/ 
          AddAuthenticationSchemes(string[] schemes) cil managed
  // SIG: 20 01 12 40 1D 0E
  {
    .param [1]/*0800002B*/ 
    .custom /*0C00008A:0A000019*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicyBuilder::AddAuthenticationSchemes

  .method /*0600003C*/ public hidebysig instance class Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder/*02000010*/ 
          AddRequirements(class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/[] requirements) cil managed
  // SIG: 20 01 12 40 1D 12 78
  {
    .param [1]/*0800002C*/ 
    .custom /*0C00008C:0A000019*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicyBuilder::AddRequirements

  .method /*0600003D*/ public hidebysig instance class Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder/*02000010*/ 
          Combine(class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/ policy) cil managed
  // SIG: 20 01 12 40 12 3C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicyBuilder::Combine

  .method /*0600003E*/ public hidebysig instance class Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder/*02000010*/ 
          RequireClaim(string claimType,
                       string[] allowedValues) cil managed
  // SIG: 20 02 12 40 0E 1D 0E
  {
    .param [2]/*0800002F*/ 
    .custom /*0C000090:0A000019*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicyBuilder::RequireClaim

  .method /*0600003F*/ public hidebysig instance class Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder/*02000010*/ 
          RequireClaim(string claimType,
                       class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string> allowedValues) cil managed
  // SIG: 20 02 12 40 0E 15 12 79 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicyBuilder::RequireClaim

  .method /*06000040*/ public hidebysig instance class Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder/*02000010*/ 
          RequireClaim(string claimType) cil managed
  // SIG: 20 01 12 40 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicyBuilder::RequireClaim

  .method /*06000041*/ public hidebysig instance class Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder/*02000010*/ 
          RequireRole(string[] roles) cil managed
  // SIG: 20 01 12 40 1D 0E
  {
    .param [1]/*08000033*/ 
    .custom /*0C000097:0A000019*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicyBuilder::RequireRole

  .method /*06000042*/ public hidebysig instance class Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder/*02000010*/ 
          RequireRole(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string> roles) cil managed
  // SIG: 20 01 12 40 15 12 79 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicyBuilder::RequireRole

  .method /*06000043*/ public hidebysig instance class Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder/*02000010*/ 
          RequireUserName(string userName) cil managed
  // SIG: 20 01 12 40 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicyBuilder::RequireUserName

  .method /*06000044*/ public hidebysig instance class Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder/*02000010*/ 
          RequireAuthenticatedUser() cil managed
  // SIG: 20 00 12 40
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicyBuilder::RequireAuthenticatedUser

  .method /*06000045*/ public hidebysig instance class Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder/*02000010*/ 
          RequireAssertion(class [System.Runtime/*23000001*/]System.Func`2/*0100002D*/<class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/,bool> 'handler') cil managed
  // SIG: 20 01 12 40 15 12 80 B5 02 12 34 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicyBuilder::RequireAssertion

  .method /*06000046*/ public hidebysig instance class Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder/*02000010*/ 
          RequireAssertion(class [System.Runtime/*23000001*/]System.Func`2/*0100002D*/<class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<bool>> 'handler') cil managed
  // SIG: 20 01 12 40 15 12 80 B5 02 12 34 15 12 80 AD 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicyBuilder::RequireAssertion

  .method /*06000047*/ public hidebysig instance class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/ 
          Build() cil managed
  // SIG: 20 00 12 3C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationPolicyBuilder::Build

  .property /*17000012*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000030*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/>
          Requirements()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000030*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/> Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder/*02000010*/::get_Requirements() /* 06000037 */
    .set instance void Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder/*02000010*/::set_Requirements(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000030*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/>) /* 06000038 */
  } // end of property AuthorizationPolicyBuilder::Requirements
  .property /*17000013*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000030*/<string>
          AuthenticationSchemes()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000030*/<string> Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder/*02000010*/::get_AuthenticationSchemes() /* 06000039 */
    .set instance void Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder/*02000010*/::set_AuthenticationSchemes(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000030*/<string>) /* 0600003A */
  } // end of property AuthorizationPolicyBuilder::AuthenticationSchemes
} // end of class Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder

.class /*02000011*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.AuthorizationResult
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C00004C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000048*/ public hidebysig specialname 
          instance bool  get_Succeeded() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000A0:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationResult::get_Succeeded

  .method /*06000049*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Authorization.AuthorizationFailure/*02000009*/ 
          get_Failure() cil managed
  // SIG: 20 00 12 24
  {
    .custom /*0C0000A1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000A2:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationResult::get_Failure

  .method /*0600004A*/ public hidebysig static 
          class Microsoft.AspNetCore.Authorization.AuthorizationResult/*02000011*/ 
          Success() cil managed
  // SIG: 00 00 12 44
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationResult::Success

  .method /*0600004B*/ public hidebysig static 
          class Microsoft.AspNetCore.Authorization.AuthorizationResult/*02000011*/ 
          Failed(class Microsoft.AspNetCore.Authorization.AuthorizationFailure/*02000009*/ failure) cil managed
  // SIG: 00 01 12 44 12 24
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationResult::Failed

  .method /*0600004C*/ public hidebysig static 
          class Microsoft.AspNetCore.Authorization.AuthorizationResult/*02000011*/ 
          Failed() cil managed
  // SIG: 00 00 12 44
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationResult::Failed

  .property /*17000014*/ instance callconv(8) bool
          Succeeded()
  {
    .get instance bool Microsoft.AspNetCore.Authorization.AuthorizationResult/*02000011*/::get_Succeeded() /* 06000048 */
  } // end of property AuthorizationResult::Succeeded
  .property /*17000015*/ instance callconv(8) class Microsoft.AspNetCore.Authorization.AuthorizationFailure/*02000009*/
          Failure()
  {
    .custom /*0C00005B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class Microsoft.AspNetCore.Authorization.AuthorizationFailure/*02000009*/ Microsoft.AspNetCore.Authorization.AuthorizationResult/*02000011*/::get_Failure() /* 06000049 */
  } // end of property AuthorizationResult::Failure
} // end of class Microsoft.AspNetCore.Authorization.AuthorizationResult

.class /*02000012*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Authorization.AuthorizationServiceExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000050:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000051:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000052:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*0600004D*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<class Microsoft.AspNetCore.Authorization.AuthorizationResult/*02000011*/> 
          AuthorizeAsync(class Microsoft.AspNetCore.Authorization.IAuthorizationService/*0200001F*/ service,
                         class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000028*/ user,
                         object resource,
                         class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/ requirement) cil managed
  // SIG: 00 04 15 12 80 AD 01 12 44 12 7C 12 80 A1 1C 12 78
  {
    .custom /*0C0000A3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [3]/*0800003B*/ 
    .custom /*0C00009E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationServiceExtensions::AuthorizeAsync

  .method /*0600004E*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<class Microsoft.AspNetCore.Authorization.AuthorizationResult/*02000011*/> 
          AuthorizeAsync(class Microsoft.AspNetCore.Authorization.IAuthorizationService/*0200001F*/ service,
                         class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000028*/ user,
                         object resource,
                         class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/ policy) cil managed
  // SIG: 00 04 15 12 80 AD 01 12 44 12 7C 12 80 A1 1C 12 3C
  {
    .custom /*0C0000A4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [3]/*0800003F*/ 
    .custom /*0C00009F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationServiceExtensions::AuthorizeAsync

  .method /*0600004F*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<class Microsoft.AspNetCore.Authorization.AuthorizationResult/*02000011*/> 
          AuthorizeAsync(class Microsoft.AspNetCore.Authorization.IAuthorizationService/*0200001F*/ service,
                         class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000028*/ user,
                         class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/ policy) cil managed
  // SIG: 00 03 15 12 80 AD 01 12 44 12 7C 12 80 A1 12 3C
  {
    .custom /*0C0000A6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationServiceExtensions::AuthorizeAsync

  .method /*06000050*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<class Microsoft.AspNetCore.Authorization.AuthorizationResult/*02000011*/> 
          AuthorizeAsync(class Microsoft.AspNetCore.Authorization.IAuthorizationService/*0200001F*/ service,
                         class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000028*/ user,
                         string policyName) cil managed
  // SIG: 00 03 15 12 80 AD 01 12 44 12 7C 12 80 A1 0E
  {
    .custom /*0C0000A7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizationServiceExtensions::AuthorizeAsync

} // end of class Microsoft.AspNetCore.Authorization.AuthorizationServiceExtensions

.class /*02000013*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.AuthorizeAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
       implements [Microsoft.AspNetCore.Metadata/*23000004*/]Microsoft.AspNetCore.Authorization.IAuthorizeData/*0100002C*/
{
  .custom /*0C000054:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000055:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000056:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 44 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..D.....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 01 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 01 )                               // rited.
  .interfaceimpl type [Microsoft.AspNetCore.Metadata/*23000004*/]Microsoft.AspNetCore.Authorization.IAuthorizeData/*0100002C*/
  .custom /*0C000022:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000051*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizeAttribute::.ctor

  .method /*06000052*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string policy) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000A8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizeAttribute::.ctor

  .method /*06000053*/ public hidebysig newslot specialname virtual final 
          instance string  get_Policy() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000AA:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizeAttribute::get_Policy

  .method /*06000054*/ public hidebysig newslot specialname virtual final 
          instance void  set_Policy(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000AC:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizeAttribute::set_Policy

  .method /*06000055*/ public hidebysig newslot specialname virtual final 
          instance string  get_Roles() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000AD:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizeAttribute::get_Roles

  .method /*06000056*/ public hidebysig newslot specialname virtual final 
          instance void  set_Roles(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000AE:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizeAttribute::set_Roles

  .method /*06000057*/ public hidebysig newslot specialname virtual final 
          instance string  get_AuthenticationSchemes() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000AF:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizeAttribute::get_AuthenticationSchemes

  .method /*06000058*/ public hidebysig newslot specialname virtual final 
          instance void  set_AuthenticationSchemes(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000B0:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthorizeAttribute::set_AuthenticationSchemes

  .property /*17000016*/ instance callconv(8) string
          Policy()
  {
    .get instance string Microsoft.AspNetCore.Authorization.AuthorizeAttribute/*02000013*/::get_Policy() /* 06000053 */
    .set instance void Microsoft.AspNetCore.Authorization.AuthorizeAttribute/*02000013*/::set_Policy(string) /* 06000054 */
  } // end of property AuthorizeAttribute::Policy
  .property /*17000017*/ instance callconv(8) string
          Roles()
  {
    .get instance string Microsoft.AspNetCore.Authorization.AuthorizeAttribute/*02000013*/::get_Roles() /* 06000055 */
    .set instance void Microsoft.AspNetCore.Authorization.AuthorizeAttribute/*02000013*/::set_Roles(string) /* 06000056 */
  } // end of property AuthorizeAttribute::Roles
  .property /*17000018*/ instance callconv(8) string
          AuthenticationSchemes()
  {
    .get instance string Microsoft.AspNetCore.Authorization.AuthorizeAttribute/*02000013*/::get_AuthenticationSchemes() /* 06000057 */
    .set instance void Microsoft.AspNetCore.Authorization.AuthorizeAttribute/*02000013*/::set_AuthenticationSchemes(string) /* 06000058 */
  } // end of property AuthorizeAttribute::AuthenticationSchemes
} // end of class Microsoft.AspNetCore.Authorization.AuthorizeAttribute

.class /*02000014*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.DefaultAuthorizationEvaluator
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements Microsoft.AspNetCore.Authorization.IAuthorizationEvaluator/*02000019*/
{
  .method /*06000059*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.Authorization.AuthorizationResult/*02000011*/ 
          Evaluate(class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ context) cil managed
  // SIG: 20 01 12 44 12 34
  {
    .custom /*0C0000B1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAuthorizationEvaluator::Evaluate

  .method /*0600005A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAuthorizationEvaluator::.ctor

} // end of class Microsoft.AspNetCore.Authorization.DefaultAuthorizationEvaluator

.class /*02000015*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.DefaultAuthorizationHandlerContextFactory
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements Microsoft.AspNetCore.Authorization.IAuthorizationHandlerContextFactory/*0200001B*/
{
  .method /*0600005B*/ public hidebysig newslot virtual 
          instance class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ 
          CreateContext(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/> requirements,
                        class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000028*/ user,
                        object resource) cil managed
  // SIG: 20 03 12 34 15 12 79 01 12 78 12 80 A1 1C
  {
    .custom /*0C0000B2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [3]/*0800004E*/ 
    .custom /*0C0000A5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAuthorizationHandlerContextFactory::CreateContext

  .method /*0600005C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAuthorizationHandlerContextFactory::.ctor

} // end of class Microsoft.AspNetCore.Authorization.DefaultAuthorizationHandlerContextFactory

.class /*02000016*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.DefaultAuthorizationHandlerProvider
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements Microsoft.AspNetCore.Authorization.IAuthorizationHandlerProvider/*0200001C*/
{
  .method /*0600005D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationHandler/*0200001A*/> handlers) cil managed
  // SIG: 20 01 01 15 12 79 01 12 68
  {
    .custom /*0C0000B4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAuthorizationHandlerProvider::.ctor

  .method /*0600005E*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationHandler/*0200001A*/>> 
          GetHandlersAsync(class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ context) cil managed
  // SIG: 20 01 15 12 80 AD 01 15 12 79 01 12 68 12 34
  {
    .custom /*0C0000B5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAuthorizationHandlerProvider::GetHandlersAsync

} // end of class Microsoft.AspNetCore.Authorization.DefaultAuthorizationHandlerProvider

.class /*02000017*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.DefaultAuthorizationPolicyProvider
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider/*0200001D*/
{
  .custom /*0C00005E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider/*0200001D*/
  .custom /*0C000032:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600005F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Options/*23000006*/]Microsoft.Extensions.Options.IOptions`1/*01000031*/<class Microsoft.AspNetCore.Authorization.AuthorizationOptions/*0200000E*/> options) cil managed
  // SIG: 20 01 01 15 12 80 C5 01 12 38
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAuthorizationPolicyProvider::.ctor

  .method /*06000060*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/> 
          GetDefaultPolicyAsync() cil managed
  // SIG: 20 00 15 12 80 AD 01 12 3C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAuthorizationPolicyProvider::GetDefaultPolicyAsync

  .method /*06000061*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/> 
          GetFallbackPolicyAsync() cil managed
  // SIG: 20 00 15 12 80 AD 01 12 3C
  {
    .param [0]/*08000052*/ 
    .custom /*0C0000A9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAuthorizationPolicyProvider::GetFallbackPolicyAsync

  .method /*06000062*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/> 
          GetPolicyAsync(string policyName) cil managed
  // SIG: 20 01 15 12 80 AD 01 12 3C 0E
  {
    .param [0]/*08000053*/ 
    .custom /*0C0000AB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAuthorizationPolicyProvider::GetPolicyAsync

} // end of class Microsoft.AspNetCore.Authorization.DefaultAuthorizationPolicyProvider

.class /*02000018*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.DefaultAuthorizationService
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements Microsoft.AspNetCore.Authorization.IAuthorizationService/*0200001F*/
{
  .custom /*0C000061:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000062:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Authorization.IAuthorizationService/*0200001F*/
  .custom /*0C000037:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200002C*/ auto ansi sealed nested private beforefieldinit '<AuthorizeAsync>d__7'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000022*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/
  {
    .custom /*0C00008B:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*0400001C*/ public int32 '<>1__state'
    .field /*0400001D*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100002E*/<class Microsoft.AspNetCore.Authorization.AuthorizationResult/*02000011*/> '<>t__builder'
    .field /*0400001E*/ public class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/> requirements
    .field /*0400001F*/ public class Microsoft.AspNetCore.Authorization.DefaultAuthorizationService/*02000018*/ '<>4__this'
    .field /*04000020*/ public class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000028*/ user
    .field /*04000021*/ public object resource
    .field /*04000022*/ private class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ '<authContext>5__2'
    .field /*04000023*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*0100002F*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationHandler/*0200001A*/>> '<>u__1'
    .field /*04000024*/ private class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<class Microsoft.AspNetCore.Authorization.IAuthorizationHandler/*0200001A*/> '<>7__wrap2'
    .field /*04000025*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000026*/ '<>u__2'
    .method /*06000091*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/::MoveNext /*01000023::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<AuthorizeAsync>d__7'::MoveNext

    .method /*06000092*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 8D
    {
      .custom /*0C0000CD:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000027*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/::SetStateMachine /*01000023::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<AuthorizeAsync>d__7'::SetStateMachine

  } // end of class '<AuthorizeAsync>d__7'

  .class /*0200002D*/ auto ansi sealed nested private beforefieldinit '<AuthorizeAsync>d__8'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000022*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/
  {
    .custom /*0C00008D:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*04000026*/ public int32 '<>1__state'
    .field /*04000027*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100002E*/<class Microsoft.AspNetCore.Authorization.AuthorizationResult/*02000011*/> '<>t__builder'
    .field /*04000028*/ public string policyName
    .field /*04000029*/ public class Microsoft.AspNetCore.Authorization.DefaultAuthorizationService/*02000018*/ '<>4__this'
    .field /*0400002A*/ public class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000028*/ user
    .field /*0400002B*/ public object resource
    .field /*0400002C*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*0100002F*/<class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/> '<>u__1'
    .field /*0400002D*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*0100002F*/<class Microsoft.AspNetCore.Authorization.AuthorizationResult/*02000011*/> '<>u__2'
    .method /*06000093*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/::MoveNext /*01000023::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<AuthorizeAsync>d__8'::MoveNext

    .method /*06000094*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 8D
    {
      .custom /*0C0000CE:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000027*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/::SetStateMachine /*01000023::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<AuthorizeAsync>d__8'::SetStateMachine

  } // end of class '<AuthorizeAsync>d__8'

  .method /*06000063*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider/*0200001D*/ policyProvider,
                               class Microsoft.AspNetCore.Authorization.IAuthorizationHandlerProvider/*0200001C*/ handlers,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger`1/*01000032*/<class Microsoft.AspNetCore.Authorization.DefaultAuthorizationService/*02000018*/> logger,
                               class Microsoft.AspNetCore.Authorization.IAuthorizationHandlerContextFactory/*0200001B*/ contextFactory,
                               class Microsoft.AspNetCore.Authorization.IAuthorizationEvaluator/*02000019*/ evaluator,
                               class [Microsoft.Extensions.Options/*23000006*/]Microsoft.Extensions.Options.IOptions`1/*01000031*/<class Microsoft.AspNetCore.Authorization.AuthorizationOptions/*0200000E*/> options) cil managed
  // SIG: 20 06 01 12 74 12 70 15 12 80 C9 01 12 60 12 6C 12 64 15 12 80 C5 01 12 38
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAuthorizationService::.ctor

  .method /*06000064*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<class Microsoft.AspNetCore.Authorization.AuthorizationResult/*02000011*/> 
          AuthorizeAsync(class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000028*/ user,
                         object resource,
                         class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/> requirements) cil managed
  // SIG: 20 03 15 12 80 AD 01 12 44 12 80 A1 1C 15 12 79 01 12 78
  {
    .custom /*0C0000B7:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000020*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100001F*/) /* 0A000015 */ = ( 01 00 53 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..SMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 41 75 74 68 6F 72 69 7A   // NetCore.Authoriz
                                                                                                                                                                                                                                        61 74 69 6F 6E 2E 44 65 66 61 75 6C 74 41 75 74   // ation.DefaultAut
                                                                                                                                                                                                                                        68 6F 72 69 7A 61 74 69 6F 6E 53 65 72 76 69 63   // horizationServic
                                                                                                                                                                                                                                        65 2B 3C 41 75 74 68 6F 72 69 7A 65 41 73 79 6E   // e+<AuthorizeAsyn
                                                                                                                                                                                                                                        63 3E 64 5F 5F 37 00 00 )                         // c>d__7..
    .param [2]/*0800005C*/ 
    .custom /*0C0000B3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAuthorizationService::AuthorizeAsync

  .method /*06000065*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<class Microsoft.AspNetCore.Authorization.AuthorizationResult/*02000011*/> 
          AuthorizeAsync(class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000028*/ user,
                         object resource,
                         string policyName) cil managed
  // SIG: 20 03 15 12 80 AD 01 12 44 12 80 A1 1C 0E
  {
    .custom /*0C0000B8:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000020*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100001F*/) /* 0A000015 */ = ( 01 00 53 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..SMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 41 75 74 68 6F 72 69 7A   // NetCore.Authoriz
                                                                                                                                                                                                                                        61 74 69 6F 6E 2E 44 65 66 61 75 6C 74 41 75 74   // ation.DefaultAut
                                                                                                                                                                                                                                        68 6F 72 69 7A 61 74 69 6F 6E 53 65 72 76 69 63   // horizationServic
                                                                                                                                                                                                                                        65 2B 3C 41 75 74 68 6F 72 69 7A 65 41 73 79 6E   // e+<AuthorizeAsyn
                                                                                                                                                                                                                                        63 3E 64 5F 5F 38 00 00 )                         // c>d__8..
    .param [2]/*0800005F*/ 
    .custom /*0C0000B6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAuthorizationService::AuthorizeAsync

} // end of class Microsoft.AspNetCore.Authorization.DefaultAuthorizationService

.class /*02000019*/ interface public abstract auto ansi Microsoft.AspNetCore.Authorization.IAuthorizationEvaluator
{
  .custom /*0C000063:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000066*/ public hidebysig newslot abstract virtual 
          instance class Microsoft.AspNetCore.Authorization.AuthorizationResult/*02000011*/ 
          Evaluate(class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ context) cil managed
  // SIG: 20 01 12 44 12 34
  {
    // Method begins at RVA 0x0
  } // end of method IAuthorizationEvaluator::Evaluate

} // end of class Microsoft.AspNetCore.Authorization.IAuthorizationEvaluator

.class /*0200001A*/ interface public abstract auto ansi Microsoft.AspNetCore.Authorization.IAuthorizationHandler
{
  .custom /*0C000064:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000067*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000021*/ 
          HandleAsync(class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ context) cil managed
  // SIG: 20 01 12 80 85 12 34
  {
    // Method begins at RVA 0x0
  } // end of method IAuthorizationHandler::HandleAsync

} // end of class Microsoft.AspNetCore.Authorization.IAuthorizationHandler

.class /*0200001B*/ interface public abstract auto ansi Microsoft.AspNetCore.Authorization.IAuthorizationHandlerContextFactory
{
  .custom /*0C000066:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000068*/ public hidebysig newslot abstract virtual 
          instance class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ 
          CreateContext(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/> requirements,
                        class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000028*/ user,
                        object resource) cil managed
  // SIG: 20 03 12 34 15 12 79 01 12 78 12 80 A1 1C
  {
    .param [3]/*08000065*/ 
    .custom /*0C0000B9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IAuthorizationHandlerContextFactory::CreateContext

} // end of class Microsoft.AspNetCore.Authorization.IAuthorizationHandlerContextFactory

.class /*0200001C*/ interface public abstract auto ansi Microsoft.AspNetCore.Authorization.IAuthorizationHandlerProvider
{
  .custom /*0C000069:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000069*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationHandler/*0200001A*/>> 
          GetHandlersAsync(class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ context) cil managed
  // SIG: 20 01 15 12 80 AD 01 15 12 79 01 12 68 12 34
  {
    // Method begins at RVA 0x0
  } // end of method IAuthorizationHandlerProvider::GetHandlersAsync

} // end of class Microsoft.AspNetCore.Authorization.IAuthorizationHandlerProvider

.class /*0200001D*/ interface public abstract auto ansi Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider
{
  .custom /*0C00006C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600006A*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/> 
          GetPolicyAsync(string policyName) cil managed
  // SIG: 20 01 15 12 80 AD 01 12 3C 0E
  {
    .param [0]/*08000067*/ 
    .custom /*0C0000BA:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IAuthorizationPolicyProvider::GetPolicyAsync

  .method /*0600006B*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/> 
          GetDefaultPolicyAsync() cil managed
  // SIG: 20 00 15 12 80 AD 01 12 3C
  {
    // Method begins at RVA 0x0
  } // end of method IAuthorizationPolicyProvider::GetDefaultPolicyAsync

  .method /*0600006C*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<class Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0200000F*/> 
          GetFallbackPolicyAsync() cil managed
  // SIG: 20 00 15 12 80 AD 01 12 3C
  {
    .param [0]/*08000069*/ 
    .custom /*0C0000BB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IAuthorizationPolicyProvider::GetFallbackPolicyAsync

} // end of class Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider

.class /*0200001E*/ interface public abstract auto ansi Microsoft.AspNetCore.Authorization.IAuthorizationRequirement
{
} // end of class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement

.class /*0200001F*/ interface public abstract auto ansi Microsoft.AspNetCore.Authorization.IAuthorizationService
{
  .custom /*0C00006E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600006D*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<class Microsoft.AspNetCore.Authorization.AuthorizationResult/*02000011*/> 
          AuthorizeAsync(class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000028*/ user,
                         object resource,
                         class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<class Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/> requirements) cil managed
  // SIG: 20 03 15 12 80 AD 01 12 44 12 80 A1 1C 15 12 79 01 12 78
  {
    .param [2]/*0800006B*/ 
    .custom /*0C0000BC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IAuthorizationService::AuthorizeAsync

  .method /*0600006E*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<class Microsoft.AspNetCore.Authorization.AuthorizationResult/*02000011*/> 
          AuthorizeAsync(class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000028*/ user,
                         object resource,
                         string policyName) cil managed
  // SIG: 20 03 15 12 80 AD 01 12 44 12 80 A1 1C 0E
  {
    .param [2]/*0800006E*/ 
    .custom /*0C0000BD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IAuthorizationService::AuthorizeAsync

} // end of class Microsoft.AspNetCore.Authorization.IAuthorizationService

.class /*02000020*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Authorization.Resources
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
} // end of class Microsoft.AspNetCore.Authorization.Resources

.class /*02000021*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.Infrastructure.AssertionRequirement
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements Microsoft.AspNetCore.Authorization.IAuthorizationHandler/*0200001A*/,
                  Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/
{
  .custom /*0C00006F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000070:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Authorization.IAuthorizationHandler/*0200001A*/
  .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/
  .custom /*0C00003C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200002E*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass3_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C00008E:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*0400002E*/ public class [System.Runtime/*23000001*/]System.Func`2/*0100002D*/<class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/,bool> 'handler'
    .method /*06000095*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass3_0'::.ctor

  } // end of class '<>c__DisplayClass3_0'

  .class /*0200002F*/ auto ansi sealed nested private beforefieldinit '<HandleAsync>d__5'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000022*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/
  {
    .custom /*0C00008F:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*0400002F*/ public int32 '<>1__state'
    .field /*04000030*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000024*/ '<>t__builder'
    .field /*04000031*/ public class Microsoft.AspNetCore.Authorization.Infrastructure.AssertionRequirement/*02000021*/ '<>4__this'
    .field /*04000032*/ public class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ context
    .field /*04000033*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*0100002F*/<bool> '<>u__1'
    .method /*06000096*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/::MoveNext /*01000023::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<HandleAsync>d__5'::MoveNext

    .method /*06000097*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 8D
    {
      .custom /*0C0000CF:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000027*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/::SetStateMachine /*01000023::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<HandleAsync>d__5'::SetStateMachine

  } // end of class '<HandleAsync>d__5'

  .method /*0600006F*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Func`2/*0100002D*/<class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<bool>> 
          get_Handler() cil managed
  // SIG: 20 00 15 12 80 B5 02 12 34 15 12 80 AD 01 02
  {
    .custom /*0C0000BE:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssertionRequirement::get_Handler

  .method /*06000070*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Func`2/*0100002D*/<class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/,bool> 'handler') cil managed
  // SIG: 20 01 01 15 12 80 B5 02 12 34 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssertionRequirement::.ctor

  .method /*06000071*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Func`2/*0100002D*/<class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<bool>> 'handler') cil managed
  // SIG: 20 01 01 15 12 80 B5 02 12 34 15 12 80 AD 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssertionRequirement::.ctor

  .method /*06000072*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000021*/ 
          HandleAsync(class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ context) cil managed
  // SIG: 20 01 12 80 85 12 34
  {
    .custom /*0C0000BF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000020*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100001F*/) /* 0A000015 */ = ( 01 00 58 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..XMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 41 75 74 68 6F 72 69 7A   // NetCore.Authoriz
                                                                                                                                                                                                                                        61 74 69 6F 6E 2E 49 6E 66 72 61 73 74 72 75 63   // ation.Infrastruc
                                                                                                                                                                                                                                        74 75 72 65 2E 41 73 73 65 72 74 69 6F 6E 52 65   // ture.AssertionRe
                                                                                                                                                                                                                                        71 75 69 72 65 6D 65 6E 74 2B 3C 48 61 6E 64 6C   // quirement+<Handl
                                                                                                                                                                                                                                        65 41 73 79 6E 63 3E 64 5F 5F 35 00 00 )          // eAsync>d__5..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssertionRequirement::HandleAsync

  .method /*06000073*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssertionRequirement::ToString

  .property /*17000019*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Func`2/*0100002D*/<class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<bool>>
          Handler()
  {
    .get instance class [System.Runtime/*23000001*/]System.Func`2/*0100002D*/<class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002B*/<bool>> Microsoft.AspNetCore.Authorization.Infrastructure.AssertionRequirement/*02000021*/::get_Handler() /* 0600006F */
  } // end of property AssertionRequirement::Handler
} // end of class Microsoft.AspNetCore.Authorization.Infrastructure.AssertionRequirement

.class /*02000022*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.Infrastructure.ClaimsAuthorizationRequirement
       extends class Microsoft.AspNetCore.Authorization.AuthorizationHandler`1/*0200000B*/<class Microsoft.AspNetCore.Authorization.Infrastructure.ClaimsAuthorizationRequirement/*02000022*/>/*1B000001*/
       implements Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/
{
  .custom /*0C000071:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000072:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/
  .custom /*0C00003E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000030*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass7_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C000092:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*04000034*/ public class Microsoft.AspNetCore.Authorization.Infrastructure.ClaimsAuthorizationRequirement/*02000022*/ requirement
    .method /*06000098*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass7_0'::.ctor

  } // end of class '<>c__DisplayClass7_0'

  .method /*06000074*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string claimType,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string> allowedValues) cil managed
  // SIG: 20 02 01 0E 15 12 79 01 0E
  {
    .param [2]/*08000074*/ 
    .custom /*0C0000C0:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClaimsAuthorizationRequirement::.ctor

  .method /*06000075*/ public hidebysig specialname 
          instance string  get_ClaimType() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000C1:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClaimsAuthorizationRequirement::get_ClaimType

  .method /*06000076*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string> 
          get_AllowedValues() cil managed
  // SIG: 20 00 15 12 79 01 0E
  {
    .custom /*0C0000C3:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .param [0]/*08000075*/ 
    .custom /*0C0000C2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClaimsAuthorizationRequirement::get_AllowedValues

  .method /*06000077*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000021*/ 
          HandleRequirementAsync(class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ context,
                                 class Microsoft.AspNetCore.Authorization.Infrastructure.ClaimsAuthorizationRequirement/*02000022*/ requirement) cil managed
  // SIG: 20 02 12 80 85 12 34 12 80 88
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClaimsAuthorizationRequirement::HandleRequirementAsync

  .method /*06000078*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClaimsAuthorizationRequirement::ToString

  .property /*1700001A*/ instance callconv(8) string
          ClaimType()
  {
    .get instance string Microsoft.AspNetCore.Authorization.Infrastructure.ClaimsAuthorizationRequirement/*02000022*/::get_ClaimType() /* 06000075 */
  } // end of property ClaimsAuthorizationRequirement::ClaimType
  .property /*1700001B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string>
          AllowedValues()
  {
    .custom /*0C000067:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string> Microsoft.AspNetCore.Authorization.Infrastructure.ClaimsAuthorizationRequirement/*02000022*/::get_AllowedValues() /* 06000076 */
  } // end of property ClaimsAuthorizationRequirement::AllowedValues
} // end of class Microsoft.AspNetCore.Authorization.Infrastructure.ClaimsAuthorizationRequirement

.class /*02000023*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.Infrastructure.DenyAnonymousAuthorizationRequirement
       extends class Microsoft.AspNetCore.Authorization.AuthorizationHandler`1/*0200000B*/<class Microsoft.AspNetCore.Authorization.Infrastructure.DenyAnonymousAuthorizationRequirement/*02000023*/>/*1B000002*/
       implements Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/
{
  .custom /*0C000074:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000075:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/
  .custom /*0C000042:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000031*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C000094:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*04000035*/ public static initonly class Microsoft.AspNetCore.Authorization.Infrastructure.DenyAnonymousAuthorizationRequirement/*02000023*//'<>c'/*02000031*/ '<>9'
    .field /*04000036*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100002D*/<class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsIdentity/*01000033*/,bool> '<>9__0_0'
    .method /*06000099*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

  } // end of class '<>c'

  .method /*06000079*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000021*/ 
          HandleRequirementAsync(class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ context,
                                 class Microsoft.AspNetCore.Authorization.Infrastructure.DenyAnonymousAuthorizationRequirement/*02000023*/ requirement) cil managed
  // SIG: 20 02 12 80 85 12 34 12 80 8C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DenyAnonymousAuthorizationRequirement::HandleRequirementAsync

  .method /*0600007A*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DenyAnonymousAuthorizationRequirement::ToString

  .method /*0600007B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DenyAnonymousAuthorizationRequirement::.ctor

} // end of class Microsoft.AspNetCore.Authorization.Infrastructure.DenyAnonymousAuthorizationRequirement

.class /*02000024*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.Infrastructure.NameAuthorizationRequirement
       extends class Microsoft.AspNetCore.Authorization.AuthorizationHandler`1/*0200000B*/<class Microsoft.AspNetCore.Authorization.Infrastructure.NameAuthorizationRequirement/*02000024*/>/*1B000003*/
       implements Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/
{
  .custom /*0C000076:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000077:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/
  .custom /*0C000045:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000032*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass4_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C000095:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*04000037*/ public class Microsoft.AspNetCore.Authorization.Infrastructure.NameAuthorizationRequirement/*02000024*/ requirement
    .method /*0600009A*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass4_0'::.ctor

  } // end of class '<>c__DisplayClass4_0'

  .method /*0600007C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string requiredName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NameAuthorizationRequirement::.ctor

  .method /*0600007D*/ public hidebysig specialname 
          instance string  get_RequiredName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000C4:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NameAuthorizationRequirement::get_RequiredName

  .method /*0600007E*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000021*/ 
          HandleRequirementAsync(class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ context,
                                 class Microsoft.AspNetCore.Authorization.Infrastructure.NameAuthorizationRequirement/*02000024*/ requirement) cil managed
  // SIG: 20 02 12 80 85 12 34 12 80 90
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NameAuthorizationRequirement::HandleRequirementAsync

  .method /*0600007F*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NameAuthorizationRequirement::ToString

  .property /*1700001C*/ instance callconv(8) string
          RequiredName()
  {
    .get instance string Microsoft.AspNetCore.Authorization.Infrastructure.NameAuthorizationRequirement/*02000024*/::get_RequiredName() /* 0600007D */
  } // end of property NameAuthorizationRequirement::RequiredName
} // end of class Microsoft.AspNetCore.Authorization.Infrastructure.NameAuthorizationRequirement

.class /*02000025*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.Infrastructure.OperationAuthorizationRequirement
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/
{
  .custom /*0C00007A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00007B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/
  .custom /*0C000048:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000080*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000C5:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OperationAuthorizationRequirement::get_Name

  .method /*06000081*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000C6:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OperationAuthorizationRequirement::set_Name

  .method /*06000082*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OperationAuthorizationRequirement::ToString

  .method /*06000083*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OperationAuthorizationRequirement::.ctor

  .property /*1700001D*/ instance callconv(8) string
          Name()
  {
    .get instance string Microsoft.AspNetCore.Authorization.Infrastructure.OperationAuthorizationRequirement/*02000025*/::get_Name() /* 06000080 */
    .set instance void Microsoft.AspNetCore.Authorization.Infrastructure.OperationAuthorizationRequirement/*02000025*/::set_Name(string) /* 06000081 */
  } // end of property OperationAuthorizationRequirement::Name
} // end of class Microsoft.AspNetCore.Authorization.Infrastructure.OperationAuthorizationRequirement

.class /*02000026*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.Infrastructure.PassThroughAuthorizationHandler
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements Microsoft.AspNetCore.Authorization.IAuthorizationHandler/*0200001A*/
{
  .class /*02000033*/ auto ansi sealed nested private beforefieldinit '<HandleAsync>d__0'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000022*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/
  {
    .custom /*0C000096:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*04000038*/ public int32 '<>1__state'
    .field /*04000039*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000024*/ '<>t__builder'
    .field /*0400003A*/ public class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ context
    .field /*0400003B*/ private class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000025*/<class Microsoft.AspNetCore.Authorization.IAuthorizationHandler/*0200001A*/> '<>7__wrap1'
    .field /*0400003C*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000026*/ '<>u__1'
    .method /*0600009B*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/::MoveNext /*01000023::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<HandleAsync>d__0'::MoveNext

    .method /*0600009C*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 8D
    {
      .custom /*0C0000D0:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000027*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000023*/::SetStateMachine /*01000023::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<HandleAsync>d__0'::SetStateMachine

  } // end of class '<HandleAsync>d__0'

  .method /*06000084*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000021*/ 
          HandleAsync(class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ context) cil managed
  // SIG: 20 01 12 80 85 12 34
  {
    .custom /*0C0000C7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000C8:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000020*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100001F*/) /* 0A000015 */ = ( 01 00 63 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..cMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 41 75 74 68 6F 72 69 7A   // NetCore.Authoriz
                                                                                                                                                                                                                                        61 74 69 6F 6E 2E 49 6E 66 72 61 73 74 72 75 63   // ation.Infrastruc
                                                                                                                                                                                                                                        74 75 72 65 2E 50 61 73 73 54 68 72 6F 75 67 68   // ture.PassThrough
                                                                                                                                                                                                                                        41 75 74 68 6F 72 69 7A 61 74 69 6F 6E 48 61 6E   // AuthorizationHan
                                                                                                                                                                                                                                        64 6C 65 72 2B 3C 48 61 6E 64 6C 65 41 73 79 6E   // dler+<HandleAsyn
                                                                                                                                                                                                                                        63 3E 64 5F 5F 30 00 00 )                         // c>d__0..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PassThroughAuthorizationHandler::HandleAsync

  .method /*06000085*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PassThroughAuthorizationHandler::.ctor

} // end of class Microsoft.AspNetCore.Authorization.Infrastructure.PassThroughAuthorizationHandler

.class /*02000027*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Authorization.Infrastructure.RolesAuthorizationRequirement
       extends class Microsoft.AspNetCore.Authorization.AuthorizationHandler`1/*0200000B*/<class Microsoft.AspNetCore.Authorization.Infrastructure.RolesAuthorizationRequirement/*02000027*/>/*1B000004*/
       implements Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/
{
  .custom /*0C00007E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00007F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Authorization.IAuthorizationRequirement/*0200001E*/
  .custom /*0C00004E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000034*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass4_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C000099:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*0400003D*/ public class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ context
    .method /*0600009D*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass4_0'::.ctor

  } // end of class '<>c__DisplayClass4_0'

  .method /*06000086*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string> allowedRoles) cil managed
  // SIG: 20 01 01 15 12 79 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RolesAuthorizationRequirement::.ctor

  .method /*06000087*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string> 
          get_AllowedRoles() cil managed
  // SIG: 20 00 15 12 79 01 0E
  {
    .custom /*0C0000C9:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RolesAuthorizationRequirement::get_AllowedRoles

  .method /*06000088*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000021*/ 
          HandleRequirementAsync(class Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext/*0200000D*/ context,
                                 class Microsoft.AspNetCore.Authorization.Infrastructure.RolesAuthorizationRequirement/*02000027*/ requirement) cil managed
  // SIG: 20 02 12 80 85 12 34 12 80 9C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RolesAuthorizationRequirement::HandleRequirementAsync

  .method /*06000089*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RolesAuthorizationRequirement::ToString

  .property /*1700001E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string>
          AllowedRoles()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001E*/<string> Microsoft.AspNetCore.Authorization.Infrastructure.RolesAuthorizationRequirement/*02000027*/::get_AllowedRoles() /* 06000087 */
  } // end of property RolesAuthorizationRequirement::AllowedRoles
} // end of class Microsoft.AspNetCore.Authorization.Infrastructure.RolesAuthorizationRequirement


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
