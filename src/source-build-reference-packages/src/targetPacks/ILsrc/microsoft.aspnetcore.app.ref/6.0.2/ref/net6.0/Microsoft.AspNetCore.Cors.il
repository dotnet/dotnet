


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
// Time-date stamp:            0xa8c90208
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00003600
// Size of init.data:              0x00000c00
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x000075ce
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
// Checksum:                       0x00016695
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000757c [0x0000004f] address [size] of Import Directory:          
// 0x00008000 [0x00000614] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00004600 [0x00002470] address [size] of Security Directory:        
// 0x0000a000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00007560 [0x0000001c] address [size] of Debug Directory:           
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
//              0x000035d4 Virtual Size
//              0x00004000 Virtual Address
//              0x00003600 Size of Raw Data
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
//              0x00000614 Virtual Size
//              0x00008000 Virtual Address
//              0x00000800 Size of Raw Data
//              0x00003c00 Pointer to Raw Data
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
//              0x00004400 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x000005d0
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00004000 Import Address Table
//              0x000075be Import Name Table
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
// 0x00004054 [0x0000348c] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x000074e0 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00001540 Size
//              '#~' Name
//    Stream 2:
//              0x000015ac Offset
//              0x00001114 Size
//              '#Strings' Name
//    Stream 3:
//              0x000026c0 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x000026c4 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x000026d4 Offset
//              0x00000db8 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000160903a01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 27248
// PE header size       : 1024 (536 used)    ( 3.76%)
// PE additional info   : 11083             (40.67%)
// Num.of PE sections   : 4
// CLR header size     : 72                 ( 0.26%)
// CLR meta-data size  : 13452              (49.37%)
// CLR additional info : 128                ( 0.47%)
// CLR method headers  : 120                ( 0.44%)
// Managed code         : 240               ( 0.88%)
// Data                 : 3072              (11.27%)
// Unaccounted          : -1943             (-7.13%)

// Num.of PE sections   : 4
//   .mvid    - 512
//   .text    - 13824
//   .rsrc    - 2048
//   .reloc   - 512

// CLR meta-data size  : 13452
//   Module        -    1 (10 bytes)
//   TypeDef       -   33 (462 bytes)     5 interfaces, 0 explicit layout
//   TypeRef       -   58 (348 bytes)
//   MethodDef     -  126 (1764 bytes)    6 abstract, 0 native, 120 bodies
//   FieldDef      -   26 (156 bytes)     12 constant
//   MemberRef     -   26 (156 bytes)
//   ParamDef      -  111 (666 bytes)
//   MethodImpl    -    2 (12 bytes)
//   Constant      -    1 (6 bytes)
//   CustomAttribute-  173 (1038 bytes)
//   InterfaceImpl -   12 (48 bytes)
//   PropertyMap   -    8 (32 bytes)
//   Property      -   31 (186 bytes)
//   MethodSemantic-   44 (264 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    5 (100 bytes)
//   NestedClass   -    5 (20 bytes)
//   GenericParam  -    4 (32 bytes)
//   GenericParamConstraint-    4 (16 bytes)
//   Strings       -  4370 bytes
//   Blobs         -  3512 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   212 bytes

// CLR additional info : 128

// CLR method headers : 120
//   Num.of method bodies  - 120
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 120

// Managed code : 240
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class CorsServiceCollectionExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class CorsEndpointConventionBuilderExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class CorsMiddlewareExtensions       (public) (abstract) (auto) (ansi) (sealed) 
// Class CORSLoggerExtensions           (abstract) (auto) (ansi) (sealed) 
// Class CorsPolicyMetadata             (public) (auto) (ansi) 
// Class DisableCorsAttribute           (public) (auto) (ansi) 
// Class EnableCorsAttribute            (public) (auto) (ansi) 
// Class Resources                      (abstract) (auto) (ansi) (sealed) 
// Class CorsConstants                  (public) (abstract) (auto) (ansi) (sealed) 
// Class CorsMiddleware                 (public) (auto) (ansi) 
// Class CorsOptions                    (public) (auto) (ansi) 
// Class CorsPolicy                     (public) (auto) (ansi) 
// Class CorsPolicyBuilder              (public) (auto) (ansi) 
// Class CorsPolicyExtensions           (abstract) (auto) (ansi) (sealed) 
// Class CorsResult                     (public) (auto) (ansi) 
// Class CorsService                    (public) (auto) (ansi) 
// Class DefaultCorsPolicyProvider      (public) (auto) (ansi) 
// Interface ICorsPolicyMetadata            (public) (abstract) (auto) (ansi) 
// Interface ICorsPolicyProvider            (public) (abstract) (auto) (ansi) 
// Interface ICorsService                   (public) (abstract) (auto) (ansi) 
// Interface IDisableCorsAttribute          (public) (abstract) (auto) (ansi) 
// Interface IEnableCorsAttribute           (public) (abstract) (auto) (ansi) 
// Class UriHelpers                     (abstract) (auto) (ansi) (sealed) 
// Class <>c__DisplayClass0_0`1         (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass1_0`1         (auto) (ansi) (sealed) (nested private) 
// Class <<Invoke>g__InvokeCoreAwaited|15_0>d (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass1_0           (auto) (ansi) (sealed) (nested private) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ Microsoft.Extensions.DependencyInjection.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ Microsoft.AspNetCore.Http.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ Microsoft.Extensions.Logging.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000005*/ Microsoft.Extensions.Options
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly /*20000001*/ Microsoft.AspNetCore.Cors
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000003:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000004:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000002*/::.ctor(int32) /* 0A000002 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000005:0A000003*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000003*/::.ctor() /* 0A000003 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000006:0A000004*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*//DebuggingModes/*01000005*/) /* 0A000004 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000007:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 81 69 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ...iMicrosoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 43 6F 72 73 2E 54 65   // pNetCore.Cors.Te
                                                                                                                                                                                    73 74 2C 50 75 62 6C 69 63 4B 65 79 3D 30 30 32   // st,PublicKey=002
                                                                                                                                                                                    34 30 30 30 30 30 34 38 30 30 30 30 30 39 34 30   // 4000004800000940
                                                                                                                                                                                    30 30 30 30 30 30 36 30 32 30 30 30 30 30 30 32   // 0000006020000002
                                                                                                                                                                                    34 30 30 30 30 35 32 35 33 34 31 33 31 30 30 30   // 4000052534131000
                                                                                                                                                                                    34 30 30 30 30 30 31 30 30 30 31 30 30 66 33 33   // 4000001000100f33
                                                                                                                                                                                    61 32 39 30 34 34 66 61 39 64 37 34 30 63 39 62   // a29044fa9d740c9b
                                                                                                                                                                                    33 32 31 33 61 39 33 65 35 37 63 38 34 62 34 37   // 3213a93e57c84b47
                                                                                                                                                                                    32 63 38 34 65 30 62 38 61 30 65 31 61 65 34 38   // 2c84e0b8a0e1ae48
                                                                                                                                                                                    65 36 37 61 39 66 38 66 36 64 65 39 64 35 66 37   // e67a9f8f6de9d5f7
                                                                                                                                                                                    66 33 64 35 32 61 63 32 33 65 34 38 61 63 35 31   // f3d52ac23e48ac51
                                                                                                                                                                                    38 30 31 66 31 64 63 39 35 30 61 62 65 39 30 31   // 801f1dc950abe901
                                                                                                                                                                                    64 61 33 34 64 32 61 39 65 33 62 61 61 64 62 31   // da34d2a9e3baadb1
                                                                                                                                                                                    34 31 61 31 37 63 37 37 65 66 33 63 35 36 35 64   // 41a17c77ef3c565d
                                                                                                                                                                                    64 35 65 65 35 30 35 34 62 39 31 63 66 36 33 62   // d5ee5054b91cf63b
                                                                                                                                                                                    62 33 63 36 61 62 38 33 66 37 32 61 62 33 61 61   // b3c6ab83f72ab3aa
                                                                                                                                                                                    66 65 39 33 64 30 66 63 33 63 32 33 34 38 62 37   // fe93d0fc3c2348b7
                                                                                                                                                                                    36 34 66 61 66 62 30 62 31 63 30 37 33 33 64 65   // 64fafb0b1c0733de
                                                                                                                                                                                    35 31 34 35 39 61 65 61 62 34 36 35 38 30 33 38   // 51459aeab4658038
                                                                                                                                                                                    34 62 66 39 64 37 34 63 34 65 32 38 31 36 34 62   // 4bf9d74c4e28164b
                                                                                                                                                                                    37 63 64 65 32 34 37 66 38 39 31 62 61 30 37 38   // 7cde247f891ba078
                                                                                                                                                                                    39 31 63 39 64 38 37 32 61 64 32 62 62 00 00 )    // 91c9d872ad2bb..
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C00000A:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0A 43 6F 6D 6D 69 74 48 61 73 68 28 31 64   // ...CommitHash(1d
                                                                                                                                                                    63 66 37 61 63 66 61 63 66 30 66 65 31 35 34 61   // cf7acfacf0fe154a
                                                                                                                                                                    64 63 63 32 33 32 37 30 63 62 30 64 61 31 31 66   // dcc23270cb0da11f
                                                                                                                                                                    66 34 34 61 63 65 00 00 )                         // f44ace..
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0F 53 6F 75 72 63 65 43 6F 6D 6D 69 74 55   // ...SourceCommitU
                                                                                                                                                                    72 6C 52 68 74 74 70 73 3A 2F 2F 67 69 74 68 75   // rlRhttps://githu
                                                                                                                                                                    62 2E 63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70   // b.com/dotnet/asp
                                                                                                                                                                    6E 65 74 63 6F 72 65 2F 74 72 65 65 2F 31 64 63   // netcore/tree/1dc
                                                                                                                                                                    66 37 61 63 66 61 63 66 30 66 65 31 35 34 61 64   // f7acfacf0fe154ad
                                                                                                                                                                    63 63 32 33 32 37 30 63 62 30 64 61 31 31 66 66   // cc23270cb0da11ff
                                                                                                                                                                    34 34 61 63 65 00 00 )                            // 44ace..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000E:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyConfigurationAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 07 52 65 6C 65 61 73 65 00 00 )             // ...Release..
  .custom /*0C00000F:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000010:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 80 C9 43 4F 52 53 20 6D 69 64 64 6C 65 77   // ....CORS middlew
                                                                                                                                                                       61 72 65 20 61 6E 64 20 70 6F 6C 69 63 79 20 66   // are and policy f
                                                                                                                                                                       6F 72 20 41 53 50 2E 4E 45 54 20 43 6F 72 65 20   // or ASP.NET Core 
                                                                                                                                                                       74 6F 20 65 6E 61 62 6C 65 20 63 72 6F 73 73 2D   // to enable cross-
                                                                                                                                                                       6F 72 69 67 69 6E 20 72 65 73 6F 75 72 63 65 20   // origin resource 
                                                                                                                                                                       73 68 61 72 69 6E 67 2E 0D 0A 43 6F 6D 6D 6F 6E   // sharing...Common
                                                                                                                                                                       6C 79 20 75 73 65 64 20 74 79 70 65 73 3A 0D 0A   // ly used types:..
                                                                                                                                                                       4D 69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74   // Microsoft.AspNet
                                                                                                                                                                       43 6F 72 65 2E 43 6F 72 73 2E 44 69 73 61 62 6C   // Core.Cors.Disabl
                                                                                                                                                                       65 43 6F 72 73 41 74 74 72 69 62 75 74 65 0D 0A   // eCorsAttribute..
                                                                                                                                                                       4D 69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74   // Microsoft.AspNet
                                                                                                                                                                       43 6F 72 65 2E 43 6F 72 73 2E 45 6E 61 62 6C 65   // Core.Cors.Enable
                                                                                                                                                                       43 6F 72 73 41 74 74 72 69 62 75 74 65 00 00 )    // CorsAttribute..
  .custom /*0C000011:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 36 2E 30 2E 32 32 32 2E 36 34 31 32 00   // ...6.0.222.6412.
                                                                                                                                                                       00 ) 
  .custom /*0C000012:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 36 2E 30 2E 32 2B 31 64 63 66 37 61 63   // ...6.0.2+1dcf7ac
                                                                                                                                                                                66 61 63 66 30 66 65 31 35 34 61 64 63 63 32 33   // facf0fe154adcc23
                                                                                                                                                                                32 37 30 63 62 30 64 61 31 31 66 66 34 34 61 63   // 270cb0da11ff44ac
                                                                                                                                                                                65 00 00 )                                        // e..
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 16 4D 69 63 72 6F 73 6F 66 74 20 41 53 50   // ...Microsoft ASP
                                                                                                                                                                   2E 4E 45 54 20 43 6F 72 65 00 00 )                // .NET Core..
  .custom /*0C000014:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 19 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ...Microsoft.Asp
                                                                                                                                                                 4E 65 74 43 6F 72 65 2E 43 6F 72 73 00 00 )       // NetCore.Cors..
  .custom /*0C000015:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    24 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // $https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70 6E 65   // com/dotnet/aspne
                                                                                                                                                                    74 63 6F 72 65 00 00 )                            // tcore..
  .custom /*0C000016:0A000011*/ instance void [System.Runtime/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*01000012*/::.ctor(string) /* 0A000011 */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
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
.module Microsoft.AspNetCore.Cors.dll
// MVID: {a4638be6-53fc-45a9-ac68-d5ca5cb0c93e}
.custom /*0C000001:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 01 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C000018:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000019:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
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
  .custom /*0C00001B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C00001C:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001D:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
  .custom /*0C00001E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000020:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
  .custom /*0C000021:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000022:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000023:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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

.class /*02000006*/ public abstract auto ansi sealed beforefieldinit Microsoft.Extensions.DependencyInjection.CorsServiceCollectionExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000026:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000027:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000028:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000006*/ public hidebysig static 
          class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000002*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000018*/ 
          AddCors(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000002*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000018*/ services) cil managed
  // SIG: 00 01 12 61 12 61
  {
    .custom /*0C000025:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsServiceCollectionExtensions::AddCors

  .method /*06000007*/ public hidebysig static 
          class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000002*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000018*/ 
          AddCors(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000002*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000018*/ services,
                  class [System.Runtime/*23000001*/]System.Action`1/*01000019*/<class Microsoft.AspNetCore.Cors.Infrastructure.CorsOptions/*02000010*/> setupAction) cil managed
  // SIG: 00 02 12 61 12 61 15 12 65 01 12 40
  {
    .custom /*0C00002A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsServiceCollectionExtensions::AddCors

} // end of class Microsoft.Extensions.DependencyInjection.CorsServiceCollectionExtensions

.class /*02000007*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Builder.CorsEndpointConventionBuilderExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00002B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00002D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*0200001D*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass0_0`1'<([Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Builder.IEndpointConventionBuilder/*0100001A*/) TBuilder>
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00007C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400000F*/ public string policyName
    .method /*06000075*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass0_0`1'::.ctor

    .method /*06000076*/ assembly hidebysig 
            instance void  '<RequireCors>b__0'(class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Builder.EndpointBuilder/*0100001B*/ endpointBuilder) cil managed
    // SIG: 20 01 01 12 6D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass0_0`1'::'<RequireCors>b__0'

  } // end of class '<>c__DisplayClass0_0`1'

  .class /*0200001E*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass1_0`1'<([Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Builder.IEndpointConventionBuilder/*0100001A*/) TBuilder>
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00007E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000010*/ public class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/ policy
    .method /*06000077*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass1_0`1'::.ctor

    .method /*06000078*/ assembly hidebysig 
            instance void  '<RequireCors>b__0'(class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Builder.EndpointBuilder/*0100001B*/ endpointBuilder) cil managed
    // SIG: 20 01 01 12 6D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass1_0`1'::'<RequireCors>b__0'

  } // end of class '<>c__DisplayClass1_0`1'

  .method /*06000008*/ public hidebysig static 
          !!TBuilder  RequireCors<([Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Builder.IEndpointConventionBuilder/*0100001A*/) TBuilder>(!!TBuilder builder,
                                                                                                                                                                      string policyName) cil managed
  // SIG: 10 01 02 1E 00 1E 00 0E
  {
    .custom /*0C00002F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TBuilder /*2A000001*/ 
      .custom /*0C000017:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsEndpointConventionBuilderExtensions::RequireCors

  .method /*06000009*/ public hidebysig static 
          !!TBuilder  RequireCors<([Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Builder.IEndpointConventionBuilder/*0100001A*/) TBuilder>(!!TBuilder builder,
                                                                                                                                                                      class [System.Runtime/*23000001*/]System.Action`1/*01000019*/<class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder/*02000012*/> configurePolicy) cil managed
  // SIG: 10 01 02 1E 00 1E 00 15 12 65 01 12 48
  {
    .custom /*0C000035:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TBuilder /*2A000002*/ 
      .custom /*0C00001A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsEndpointConventionBuilderExtensions::RequireCors

} // end of class Microsoft.AspNetCore.Builder.CorsEndpointConventionBuilderExtensions

.class /*02000008*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Builder.CorsMiddlewareExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000030:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000031:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000032:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*0600000A*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*0100001C*/ 
          UseCors(class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*0100001C*/ app) cil managed
  // SIG: 00 01 12 71 12 71
  {
    .custom /*0C00003A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsMiddlewareExtensions::UseCors

  .method /*0600000B*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*0100001C*/ 
          UseCors(class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*0100001C*/ app,
                  string policyName) cil managed
  // SIG: 00 02 12 71 12 71 0E
  {
    .custom /*0C00003B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsMiddlewareExtensions::UseCors

  .method /*0600000C*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*0100001C*/ 
          UseCors(class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*0100001C*/ app,
                  class [System.Runtime/*23000001*/]System.Action`1/*01000019*/<class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder/*02000012*/> configurePolicy) cil managed
  // SIG: 00 02 12 71 12 71 15 12 65 01 12 48
  {
    .custom /*0C00003E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsMiddlewareExtensions::UseCors

} // end of class Microsoft.AspNetCore.Builder.CorsMiddlewareExtensions

.class /*02000009*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Cors.CORSLoggerExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000036:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000037:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000038:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*0600000D*/ public hidebysig static 
          void  IsPreflightRequest(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000020*/ logger) cil managed
  // SIG: 00 01 01 12 80 81
  {
    .custom /*0C000042:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000043:0A000014*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100001E*/::.ctor(int32,
                                                                                                                                                                              valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*0100001D*/,
                                                                                                                                                                              string) /* 0A000014 */ = ( 01 00 01 00 00 00 01 00 00 00 23 54 68 65 20 72   // ..........#The r
                                                                                                                                                                                                         65 71 75 65 73 74 20 69 73 20 61 20 70 72 65 66   // equest is a pref
                                                                                                                                                                                                         6C 69 67 68 74 20 72 65 71 75 65 73 74 2E 01 00   // light request...
                                                                                                                                                                                                         54 0E 09 45 76 65 6E 74 4E 61 6D 65 12 49 73 50   // T..EventName.IsP
                                                                                                                                                                                                         72 65 66 6C 69 67 68 74 52 65 71 75 65 73 74 )    // reflightRequest
    .custom /*0C000044:0A000015*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100001F*/::.ctor(string,
                                                                                                                                              string) /* 0A000015 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                         65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                         47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                         2E 36 34 30 36 00 00 )                            // .6406..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CORSLoggerExtensions::IsPreflightRequest

  .method /*0600000E*/ public hidebysig static 
          void  RequestHasOriginHeader(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000020*/ logger,
                                       string origin) cil managed
  // SIG: 00 02 01 12 80 81 0E
  {
    .custom /*0C000045:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000046:0A000014*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100001E*/::.ctor(int32,
                                                                                                                                                                              valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*0100001D*/,
                                                                                                                                                                              string) /* 0A000014 */ = ( 01 00 02 00 00 00 01 00 00 00 2D 54 68 65 20 72   // ..........-The r
                                                                                                                                                                                                         65 71 75 65 73 74 20 68 61 73 20 61 6E 20 6F 72   // equest has an or
                                                                                                                                                                                                         69 67 69 6E 20 68 65 61 64 65 72 3A 20 27 7B 6F   // igin header: '{o
                                                                                                                                                                                                         72 69 67 69 6E 7D 27 2E 01 00 54 0E 09 45 76 65   // rigin}'...T..Eve
                                                                                                                                                                                                         6E 74 4E 61 6D 65 16 52 65 71 75 65 73 74 48 61   // ntName.RequestHa
                                                                                                                                                                                                         73 4F 72 69 67 69 6E 48 65 61 64 65 72 )          // sOriginHeader
    .custom /*0C000047:0A000015*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100001F*/::.ctor(string,
                                                                                                                                              string) /* 0A000015 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                         65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                         47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                         2E 36 34 30 36 00 00 )                            // .6406..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CORSLoggerExtensions::RequestHasOriginHeader

  .method /*0600000F*/ public hidebysig static 
          void  RequestDoesNotHaveOriginHeader(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000020*/ logger) cil managed
  // SIG: 00 01 01 12 80 81
  {
    .custom /*0C00004A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00004B:0A000014*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100001E*/::.ctor(int32,
                                                                                                                                                                              valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*0100001D*/,
                                                                                                                                                                              string) /* 0A000014 */ = ( 01 00 03 00 00 00 01 00 00 00 2B 54 68 65 20 72   // ..........+The r
                                                                                                                                                                                                         65 71 75 65 73 74 20 64 6F 65 73 20 6E 6F 74 20   // equest does not 
                                                                                                                                                                                                         68 61 76 65 20 61 6E 20 6F 72 69 67 69 6E 20 68   // have an origin h
                                                                                                                                                                                                         65 61 64 65 72 2E 01 00 54 0E 09 45 76 65 6E 74   // eader...T..Event
                                                                                                                                                                                                         4E 61 6D 65 1E 52 65 71 75 65 73 74 44 6F 65 73   // Name.RequestDoes
                                                                                                                                                                                                         4E 6F 74 48 61 76 65 4F 72 69 67 69 6E 48 65 61   // NotHaveOriginHea
                                                                                                                                                                                                         64 65 72 )                                        // der
    .custom /*0C00004C:0A000015*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100001F*/::.ctor(string,
                                                                                                                                              string) /* 0A000015 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                         65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                         47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                         2E 36 34 30 36 00 00 )                            // .6406..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CORSLoggerExtensions::RequestDoesNotHaveOriginHeader

  .method /*06000010*/ public hidebysig static 
          void  PolicySuccess(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000020*/ logger) cil managed
  // SIG: 00 01 01 12 80 81
  {
    .custom /*0C00004F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000050:0A000014*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100001E*/::.ctor(int32,
                                                                                                                                                                              valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*0100001D*/,
                                                                                                                                                                              string) /* 0A000014 */ = ( 01 00 04 00 00 00 02 00 00 00 21 43 4F 52 53 20   // ..........!CORS 
                                                                                                                                                                                                         70 6F 6C 69 63 79 20 65 78 65 63 75 74 69 6F 6E   // policy execution
                                                                                                                                                                                                         20 73 75 63 63 65 73 73 66 75 6C 2E 01 00 54 0E   //  successful...T.
                                                                                                                                                                                                         09 45 76 65 6E 74 4E 61 6D 65 0D 50 6F 6C 69 63   // .EventName.Polic
                                                                                                                                                                                                         79 53 75 63 63 65 73 73 )                         // ySuccess
    .custom /*0C000051:0A000015*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100001F*/::.ctor(string,
                                                                                                                                              string) /* 0A000015 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                         65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                         47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                         2E 36 34 30 36 00 00 )                            // .6406..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CORSLoggerExtensions::PolicySuccess

  .method /*06000011*/ public hidebysig static 
          void  PolicyFailure(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000020*/ logger) cil managed
  // SIG: 00 01 01 12 80 81
  {
    .custom /*0C000054:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000055:0A000014*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100001E*/::.ctor(int32,
                                                                                                                                                                              valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*0100001D*/,
                                                                                                                                                                              string) /* 0A000014 */ = ( 01 00 05 00 00 00 02 00 00 00 1D 43 4F 52 53 20   // ...........CORS 
                                                                                                                                                                                                         70 6F 6C 69 63 79 20 65 78 65 63 75 74 69 6F 6E   // policy execution
                                                                                                                                                                                                         20 66 61 69 6C 65 64 2E 01 00 54 0E 09 45 76 65   //  failed...T..Eve
                                                                                                                                                                                                         6E 74 4E 61 6D 65 0D 50 6F 6C 69 63 79 46 61 69   // ntName.PolicyFai
                                                                                                                                                                                                         6C 75 72 65 )                                     // lure
    .custom /*0C000056:0A000015*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100001F*/::.ctor(string,
                                                                                                                                              string) /* 0A000015 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                         65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                         47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                         2E 36 34 30 36 00 00 )                            // .6406..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CORSLoggerExtensions::PolicyFailure

  .method /*06000012*/ public hidebysig static 
          void  OriginNotAllowed(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000020*/ logger,
                                 string origin) cil managed
  // SIG: 00 02 01 12 80 81 0E
  {
    .custom /*0C000059:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00005A:0A000014*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100001E*/::.ctor(int32,
                                                                                                                                                                              valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*0100001D*/,
                                                                                                                                                                              string) /* 0A000014 */ = ( 01 00 06 00 00 00 02 00 00 00 48 52 65 71 75 65   // ..........HReque
                                                                                                                                                                                                         73 74 20 6F 72 69 67 69 6E 20 7B 6F 72 69 67 69   // st origin {origi
                                                                                                                                                                                                         6E 7D 20 64 6F 65 73 20 6E 6F 74 20 68 61 76 65   // n} does not have
                                                                                                                                                                                                         20 70 65 72 6D 69 73 73 69 6F 6E 20 74 6F 20 61   //  permission to a
                                                                                                                                                                                                         63 63 65 73 73 20 74 68 65 20 72 65 73 6F 75 72   // ccess the resour
                                                                                                                                                                                                         63 65 2E 01 00 54 0E 09 45 76 65 6E 74 4E 61 6D   // ce...T..EventNam
                                                                                                                                                                                                         65 10 4F 72 69 67 69 6E 4E 6F 74 41 6C 6C 6F 77   // e.OriginNotAllow
                                                                                                                                                                                                         65 64 )                                           // ed
    .custom /*0C00005B:0A000015*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100001F*/::.ctor(string,
                                                                                                                                              string) /* 0A000015 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                         65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                         47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                         2E 36 34 30 36 00 00 )                            // .6406..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CORSLoggerExtensions::OriginNotAllowed

  .method /*06000013*/ public hidebysig static 
          void  AccessControlMethodNotAllowed(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000020*/ logger,
                                              string accessControlRequestMethod) cil managed
  // SIG: 00 02 01 12 80 81 0E
  {
    .custom /*0C00005E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00005F:0A000014*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100001E*/::.ctor(int32,
                                                                                                                                                                              valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*0100001D*/,
                                                                                                                                                                              string) /* 0A000014 */ = ( 01 00 07 00 00 00 02 00 00 00 47 52 65 71 75 65   // ..........GReque
                                                                                                                                                                                                         73 74 20 6D 65 74 68 6F 64 20 7B 61 63 63 65 73   // st method {acces
                                                                                                                                                                                                         73 43 6F 6E 74 72 6F 6C 52 65 71 75 65 73 74 4D   // sControlRequestM
                                                                                                                                                                                                         65 74 68 6F 64 7D 20 6E 6F 74 20 61 6C 6C 6F 77   // ethod} not allow
                                                                                                                                                                                                         65 64 20 69 6E 20 43 4F 52 53 20 70 6F 6C 69 63   // ed in CORS polic
                                                                                                                                                                                                         79 2E 01 00 54 0E 09 45 76 65 6E 74 4E 61 6D 65   // y...T..EventName
                                                                                                                                                                                                         1D 41 63 63 65 73 73 43 6F 6E 74 72 6F 6C 4D 65   // .AccessControlMe
                                                                                                                                                                                                         74 68 6F 64 4E 6F 74 41 6C 6C 6F 77 65 64 )       // thodNotAllowed
    .custom /*0C000060:0A000015*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100001F*/::.ctor(string,
                                                                                                                                              string) /* 0A000015 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                         65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                         47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                         2E 36 34 30 36 00 00 )                            // .6406..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CORSLoggerExtensions::AccessControlMethodNotAllowed

  .method /*06000014*/ public hidebysig static 
          void  RequestHeaderNotAllowed(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000020*/ logger,
                                        string requestHeader) cil managed
  // SIG: 00 02 01 12 80 81 0E
  {
    .custom /*0C000062:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000063:0A000014*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100001E*/::.ctor(int32,
                                                                                                                                                                              valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*0100001D*/,
                                                                                                                                                                              string) /* 0A000014 */ = ( 01 00 08 00 00 00 02 00 00 00 3C 52 65 71 75 65   // ..........<Reque
                                                                                                                                                                                                         73 74 20 68 65 61 64 65 72 20 27 7B 72 65 71 75   // st header '{requ
                                                                                                                                                                                                         65 73 74 48 65 61 64 65 72 7D 27 20 6E 6F 74 20   // estHeader}' not 
                                                                                                                                                                                                         61 6C 6C 6F 77 65 64 20 69 6E 20 43 4F 52 53 20   // allowed in CORS 
                                                                                                                                                                                                         70 6F 6C 69 63 79 2E 01 00 54 0E 09 45 76 65 6E   // policy...T..Even
                                                                                                                                                                                                         74 4E 61 6D 65 17 52 65 71 75 65 73 74 48 65 61   // tName.RequestHea
                                                                                                                                                                                                         64 65 72 4E 6F 74 41 6C 6C 6F 77 65 64 )          // derNotAllowed
    .custom /*0C000064:0A000015*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100001F*/::.ctor(string,
                                                                                                                                              string) /* 0A000015 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                         65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                         47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                         2E 36 34 30 36 00 00 )                            // .6406..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CORSLoggerExtensions::RequestHeaderNotAllowed

  .method /*06000015*/ public hidebysig static 
          void  FailedToSetCorsHeaders(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000020*/ logger,
                                       class [System.Runtime/*23000001*/]System.Exception/*01000021*/ exception) cil managed
  // SIG: 00 02 01 12 80 81 12 80 85
  {
    .custom /*0C000067:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000068:0A000014*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100001E*/::.ctor(int32,
                                                                                                                                                                              valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*0100001D*/,
                                                                                                                                                                              string) /* 0A000014 */ = ( 01 00 09 00 00 00 03 00 00 00 26 46 61 69 6C 65   // ..........&Faile
                                                                                                                                                                                                         64 20 74 6F 20 61 70 70 6C 79 20 43 4F 52 53 20   // d to apply CORS 
                                                                                                                                                                                                         52 65 73 70 6F 6E 73 65 20 68 65 61 64 65 72 73   // Response headers
                                                                                                                                                                                                         2E 01 00 54 0E 09 45 76 65 6E 74 4E 61 6D 65 16   // ...T..EventName.
                                                                                                                                                                                                         46 61 69 6C 65 64 54 6F 53 65 74 43 6F 72 73 48   // FailedToSetCorsH
                                                                                                                                                                                                         65 61 64 65 72 73 )                               // eaders
    .custom /*0C000069:0A000015*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100001F*/::.ctor(string,
                                                                                                                                              string) /* 0A000015 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                         65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                         47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                         2E 36 34 30 36 00 00 )                            // .6406..
    .param [2]/*0800001A*/ 
    .custom /*0C000079:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CORSLoggerExtensions::FailedToSetCorsHeaders

  .method /*06000016*/ public hidebysig static 
          void  NoCorsPolicyFound(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000020*/ logger) cil managed
  // SIG: 00 01 01 12 80 81
  {
    .custom /*0C00006C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00006D:0A000014*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100001E*/::.ctor(int32,
                                                                                                                                                                              valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*0100001D*/,
                                                                                                                                                                              string) /* 0A000014 */ = ( 01 00 0A 00 00 00 02 00 00 00 2F 4E 6F 20 43 4F   // ........../No CO
                                                                                                                                                                                                         52 53 20 70 6F 6C 69 63 79 20 66 6F 75 6E 64 20   // RS policy found 
                                                                                                                                                                                                         66 6F 72 20 74 68 65 20 73 70 65 63 69 66 69 65   // for the specifie
                                                                                                                                                                                                         64 20 72 65 71 75 65 73 74 2E 01 00 54 0E 09 45   // d request...T..E
                                                                                                                                                                                                         76 65 6E 74 4E 61 6D 65 11 4E 6F 43 6F 72 73 50   // ventName.NoCorsP
                                                                                                                                                                                                         6F 6C 69 63 79 46 6F 75 6E 64 )                   // olicyFound
    .custom /*0C00006E:0A000015*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100001F*/::.ctor(string,
                                                                                                                                              string) /* 0A000015 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                         65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                         47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                         2E 36 34 30 36 00 00 )                            // .6406..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CORSLoggerExtensions::NoCorsPolicyFound

  .method /*06000017*/ public hidebysig static 
          void  IsNotPreflightRequest(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000020*/ logger) cil managed
  // SIG: 00 01 01 12 80 81
  {
    .custom /*0C000070:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000071:0A000014*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100001E*/::.ctor(int32,
                                                                                                                                                                              valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*0100001D*/,
                                                                                                                                                                              string) /* 0A000014 */ = ( 01 00 0C 00 00 00 01 00 00 00 80 A2 54 68 69 73   // ............This
                                                                                                                                                                                                         20 72 65 71 75 65 73 74 20 75 73 65 73 20 74 68   //  request uses th
                                                                                                                                                                                                         65 20 48 54 54 50 20 4F 50 54 49 4F 4E 53 20 6D   // e HTTP OPTIONS m
                                                                                                                                                                                                         65 74 68 6F 64 20 62 75 74 20 64 6F 65 73 20 6E   // ethod but does n
                                                                                                                                                                                                         6F 74 20 68 61 76 65 20 61 6E 20 41 63 63 65 73   // ot have an Acces
                                                                                                                                                                                                         73 2D 43 6F 6E 74 72 6F 6C 2D 52 65 71 75 65 73   // s-Control-Reques
                                                                                                                                                                                                         74 2D 4D 65 74 68 6F 64 20 68 65 61 64 65 72 2E   // t-Method header.
                                                                                                                                                                                                         20 54 68 69 73 20 72 65 71 75 65 73 74 20 77 69   //  This request wi
                                                                                                                                                                                                         6C 6C 20 6E 6F 74 20 62 65 20 74 72 65 61 74 65   // ll not be treate
                                                                                                                                                                                                         64 20 61 73 20 61 20 43 4F 52 53 20 70 72 65 66   // d as a CORS pref
                                                                                                                                                                                                         6C 69 67 68 74 20 72 65 71 75 65 73 74 2E 01 00   // light request...
                                                                                                                                                                                                         54 0E 09 45 76 65 6E 74 4E 61 6D 65 15 49 73 4E   // T..EventName.IsN
                                                                                                                                                                                                         6F 74 50 72 65 66 6C 69 67 68 74 52 65 71 75 65   // otPreflightReque
                                                                                                                                                                                                         73 74 )                                           // st
    .custom /*0C000072:0A000015*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100001F*/::.ctor(string,
                                                                                                                                              string) /* 0A000015 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                         65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                         47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                         2E 36 34 30 36 00 00 )                            // .6406..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CORSLoggerExtensions::IsNotPreflightRequest

} // end of class Microsoft.AspNetCore.Cors.CORSLoggerExtensions

.class /*0200000A*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Cors.CorsPolicyMetadata
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.Cors.Infrastructure.ICorsPolicyMetadata/*02000017*/,
                  [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Cors.Infrastructure.ICorsMetadata/*01000022*/
{
  .method /*06000018*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/ policy) cil managed
  // SIG: 20 01 01 12 44
  {
    .custom /*0C000074:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicyMetadata::.ctor

  .method /*06000019*/ public hidebysig newslot specialname virtual final 
          instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/ 
          get_Policy() cil managed
  // SIG: 20 00 12 44
  {
    .custom /*0C000076:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000077:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicyMetadata::get_Policy

  .property /*17000001*/ instance callconv(8) class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/
          Policy()
  {
    .custom /*0C000002:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/ Microsoft.AspNetCore.Cors.CorsPolicyMetadata/*0200000A*/::get_Policy() /* 06000019 */
  } // end of property CorsPolicyMetadata::Policy
} // end of class Microsoft.AspNetCore.Cors.CorsPolicyMetadata

.class /*0200000B*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Cors.DisableCorsAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
       implements Microsoft.AspNetCore.Cors.Infrastructure.IDisableCorsAttribute/*0200001A*/,
                  [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Cors.Infrastructure.ICorsMetadata/*01000022*/
{
  .custom /*0C00003C:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 44 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..D.....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .method /*0600001A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DisableCorsAttribute::.ctor

} // end of class Microsoft.AspNetCore.Cors.DisableCorsAttribute

.class /*0200000C*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Cors.EnableCorsAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
       implements Microsoft.AspNetCore.Cors.Infrastructure.IEnableCorsAttribute/*0200001B*/,
                  [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Cors.Infrastructure.ICorsMetadata/*01000022*/
{
  .custom /*0C00003F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000040:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000041:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 44 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..D.....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 01 )                               // rited.
  .interfaceimpl type Microsoft.AspNetCore.Cors.Infrastructure.IEnableCorsAttribute/*0200001B*/
  .custom /*0C000024:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Cors.Infrastructure.ICorsMetadata/*01000022*/
  .custom /*0C000029:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600001B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnableCorsAttribute::.ctor

  .method /*0600001C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string policyName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnableCorsAttribute::.ctor

  .method /*0600001D*/ public hidebysig newslot specialname virtual final 
          instance string  get_PolicyName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00007B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnableCorsAttribute::get_PolicyName

  .method /*0600001E*/ public hidebysig newslot specialname virtual final 
          instance void  set_PolicyName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00007D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnableCorsAttribute::set_PolicyName

  .property /*17000002*/ instance callconv(8) string
          PolicyName()
  {
    .get instance string Microsoft.AspNetCore.Cors.EnableCorsAttribute/*0200000C*/::get_PolicyName() /* 0600001D */
    .set instance void Microsoft.AspNetCore.Cors.EnableCorsAttribute/*0200000C*/::set_PolicyName(string) /* 0600001E */
  } // end of property EnableCorsAttribute::PolicyName
} // end of class Microsoft.AspNetCore.Cors.EnableCorsAttribute

.class /*0200000D*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Cors.Resources
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*0600001F*/ assembly hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Resources.ResourceManager/*01000023*/ 
          get_ResourceManager() cil managed
  // SIG: 00 00 12 80 8D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_ResourceManager

  .method /*06000020*/ assembly hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000024*/ 
          get_Culture() cil managed
  // SIG: 00 00 12 80 91
  {
    .custom /*0C000080:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_Culture

  .method /*06000021*/ assembly hidebysig specialname static 
          void  set_Culture(class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000024*/ 'value') cil managed
  // SIG: 00 01 01 12 80 91
  {
    .custom /*0C000082:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::set_Culture

  .method /*06000022*/ assembly hidebysig static 
          string  GetResourceString(string resourceKey,
                                    [opt] string defaultValue) cil managed aggressiveinlining
  // SIG: 00 02 0E 0E 0E
  {
    .param [2]/*08000022*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::GetResourceString

  .method /*06000023*/ assembly hidebysig specialname static 
          string  get_InsecureConfiguration() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_InsecureConfiguration

  .method /*06000024*/ assembly hidebysig specialname static 
          string  get_PolicyNotFound() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_PolicyNotFound

  .method /*06000025*/ assembly hidebysig static 
          string  FormatPolicyNotFound(object p0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::FormatPolicyNotFound

  .method /*06000026*/ assembly hidebysig specialname static 
          string  get_PreflightMaxAgeOutOfRange() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_PreflightMaxAgeOutOfRange

  .property /*17000003*/ callconv(8) class [System.Runtime/*23000001*/]System.Resources.ResourceManager/*01000023*/
          ResourceManager()
  {
    .get class [System.Runtime/*23000001*/]System.Resources.ResourceManager/*01000023*/ Microsoft.AspNetCore.Cors.Resources/*0200000D*/::get_ResourceManager() /* 0600001F */
  } // end of property Resources::ResourceManager
  .property /*17000004*/ callconv(8) class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000024*/
          Culture()
  {
    .get class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000024*/ Microsoft.AspNetCore.Cors.Resources/*0200000D*/::get_Culture() /* 06000020 */
    .set void Microsoft.AspNetCore.Cors.Resources/*0200000D*/::set_Culture(class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000024*/) /* 06000021 */
  } // end of property Resources::Culture
  .property /*17000005*/ callconv(8) string
          InsecureConfiguration()
  {
    .get string Microsoft.AspNetCore.Cors.Resources/*0200000D*/::get_InsecureConfiguration() /* 06000023 */
  } // end of property Resources::InsecureConfiguration
  .property /*17000006*/ callconv(8) string
          PolicyNotFound()
  {
    .get string Microsoft.AspNetCore.Cors.Resources/*0200000D*/::get_PolicyNotFound() /* 06000024 */
  } // end of property Resources::PolicyNotFound
  .property /*17000007*/ callconv(8) string
          PreflightMaxAgeOutOfRange()
  {
    .get string Microsoft.AspNetCore.Cors.Resources/*0200000D*/::get_PreflightMaxAgeOutOfRange() /* 06000026 */
  } // end of property Resources::PreflightMaxAgeOutOfRange
} // end of class Microsoft.AspNetCore.Cors.Resources

.class /*0200000E*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Cors.Infrastructure.CorsConstants
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000048:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000049:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000004*/ public static initonly string PreflightHttpMethod
  .field /*04000005*/ public static initonly string Origin
  .field /*04000006*/ public static initonly string AnyOrigin
  .field /*04000007*/ public static initonly string AccessControlRequestMethod
  .field /*04000008*/ public static initonly string AccessControlRequestHeaders
  .field /*04000009*/ public static initonly string AccessControlAllowOrigin
  .field /*0400000A*/ public static initonly string AccessControlAllowHeaders
  .field /*0400000B*/ public static initonly string AccessControlExposeHeaders
  .field /*0400000C*/ public static initonly string AccessControlAllowMethods
  .field /*0400000D*/ public static initonly string AccessControlAllowCredentials
  .field /*0400000E*/ public static initonly string AccessControlMaxAge
} // end of class Microsoft.AspNetCore.Cors.Infrastructure.CorsConstants

.class /*0200000F*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Cors.Infrastructure.CorsMiddleware
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00004D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200001F*/ auto ansi sealed nested private beforefieldinit '<<Invoke>g__InvokeCoreAwaited|15_0>d'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/
  {
    .custom /*0C00007F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000011*/ public int32 '<>1__state'
    .field /*04000012*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002B*/ '<>t__builder'
    .field /*04000013*/ public class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002C*/<class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/> policyTask
    .field /*04000014*/ public class Microsoft.AspNetCore.Cors.Infrastructure.CorsMiddleware/*0200000F*/ '<>4__this'
    .field /*04000015*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Http.HttpContext/*01000028*/ context
    .field /*04000016*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*0100002D*/<class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/> '<>u__1'
    .field /*04000017*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100002E*/ '<>u__2'
    .method /*06000079*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::MoveNext /*0100002A::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<<Invoke>g__InvokeCoreAwaited|15_0>d'::MoveNext

    .method /*0600007A*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 A9
    {
      .custom /*0C0000AD:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002F*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::SetStateMachine /*0100002A::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<<Invoke>g__InvokeCoreAwaited|15_0>d'::SetStateMachine

  } // end of class '<<Invoke>g__InvokeCoreAwaited|15_0>d'

  .method /*06000027*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Http.RequestDelegate/*01000025*/ next,
                               class Microsoft.AspNetCore.Cors.Infrastructure.ICorsService/*02000019*/ corsService,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILoggerFactory/*01000026*/ loggerFactory) cil managed
  // SIG: 20 03 01 12 80 95 12 64 12 80 99
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsMiddleware::.ctor

  .method /*06000028*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Http.RequestDelegate/*01000025*/ next,
                               class Microsoft.AspNetCore.Cors.Infrastructure.ICorsService/*02000019*/ corsService,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILoggerFactory/*01000026*/ loggerFactory,
                               string policyName) cil managed
  // SIG: 20 04 01 12 80 95 12 64 12 80 99 0E
  {
    .param [4]/*0800002A*/ 
    .custom /*0C000084:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsMiddleware::.ctor

  .method /*06000029*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Http.RequestDelegate/*01000025*/ next,
                               class Microsoft.AspNetCore.Cors.Infrastructure.ICorsService/*02000019*/ corsService,
                               class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/ policy,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILoggerFactory/*01000026*/ loggerFactory) cil managed
  // SIG: 20 04 01 12 80 95 12 64 12 44 12 80 99
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsMiddleware::.ctor

  .method /*0600002A*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/ 
          Invoke(class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Http.HttpContext/*01000028*/ context,
                 class Microsoft.AspNetCore.Cors.Infrastructure.ICorsPolicyProvider/*02000018*/ corsPolicyProvider) cil managed
  // SIG: 20 02 12 80 9D 12 80 A1 12 60
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsMiddleware::Invoke

} // end of class Microsoft.AspNetCore.Cors.Infrastructure.CorsMiddleware

.class /*02000010*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Cors.Infrastructure.CorsOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000052:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000053:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600002B*/ assembly hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000031*/<string,valuetype [System.Runtime/*23000001*/]System.ValueTuple`2/*01000032*/<class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002C*/<class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/>>> 
          get_PolicyMap() cil managed
  // SIG: 20 00 15 12 80 C5 02 0E 15 11 80 C9 02 12 44 15 12 80 B1 01 12 44
  {
    .custom /*0C000085:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*08000031*/ 
    .custom /*0C000086:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TupleElementNamesAttribute/*01000030*/::.ctor(string[]) /* 0A000019 */ = ( 01 00 02 00 00 00 06 70 6F 6C 69 63 79 0A 70 6F   // .......policy.po
                                                                                                                                                                                       6C 69 63 79 54 61 73 6B 00 00 )                   // licyTask..
    .custom /*0C000087:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 06 00 00 00 01 01 00 01 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsOptions::get_PolicyMap

  .method /*0600002C*/ public hidebysig specialname 
          instance string  get_DefaultPolicyName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsOptions::get_DefaultPolicyName

  .method /*0600002D*/ public hidebysig specialname 
          instance void  set_DefaultPolicyName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsOptions::set_DefaultPolicyName

  .method /*0600002E*/ public hidebysig instance void 
          AddDefaultPolicy(class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/ policy) cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsOptions::AddDefaultPolicy

  .method /*0600002F*/ public hidebysig instance void 
          AddDefaultPolicy(class [System.Runtime/*23000001*/]System.Action`1/*01000019*/<class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder/*02000012*/> configurePolicy) cil managed
  // SIG: 20 01 01 15 12 65 01 12 48
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsOptions::AddDefaultPolicy

  .method /*06000030*/ public hidebysig instance void 
          AddPolicy(string name,
                    class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/ policy) cil managed
  // SIG: 20 02 01 0E 12 44
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsOptions::AddPolicy

  .method /*06000031*/ public hidebysig instance void 
          AddPolicy(string name,
                    class [System.Runtime/*23000001*/]System.Action`1/*01000019*/<class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder/*02000012*/> configurePolicy) cil managed
  // SIG: 20 02 01 0E 15 12 65 01 12 48
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsOptions::AddPolicy

  .method /*06000032*/ public hidebysig instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/ 
          GetPolicy(string name) cil managed
  // SIG: 20 01 12 44 0E
  {
    .param [0]/*08000039*/ 
    .custom /*0C000089:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsOptions::GetPolicy

  .method /*06000033*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsOptions::.ctor

  .property /*17000008*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000031*/<string,valuetype [System.Runtime/*23000001*/]System.ValueTuple`2/*01000032*/<class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002C*/<class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/>>>
          PolicyMap()
  {
    .custom /*0C000033:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TupleElementNamesAttribute/*01000030*/::.ctor(string[]) /* 0A000019 */ = ( 01 00 02 00 00 00 06 70 6F 6C 69 63 79 0A 70 6F   // .......policy.po
                                                                                                                                                                                       6C 69 63 79 54 61 73 6B 00 00 )                   // licyTask..
    .custom /*0C000034:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 06 00 00 00 01 01 00 01 01 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000031*/<string,valuetype [System.Runtime/*23000001*/]System.ValueTuple`2/*01000032*/<class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002C*/<class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/>>> Microsoft.AspNetCore.Cors.Infrastructure.CorsOptions/*02000010*/::get_PolicyMap() /* 0600002B */
  } // end of property CorsOptions::PolicyMap
  .property /*17000009*/ instance callconv(8) string
          DefaultPolicyName()
  {
    .get instance string Microsoft.AspNetCore.Cors.Infrastructure.CorsOptions/*02000010*/::get_DefaultPolicyName() /* 0600002C */
    .set instance void Microsoft.AspNetCore.Cors.Infrastructure.CorsOptions/*02000010*/::set_DefaultPolicyName(string) /* 0600002D */
  } // end of property CorsOptions::DefaultPolicyName
} // end of class Microsoft.AspNetCore.Cors.Infrastructure.CorsOptions

.class /*02000011*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000057:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000058:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000034*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicy::.ctor

  .method /*06000035*/ public hidebysig specialname 
          instance bool  get_AllowAnyHeader() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicy::get_AllowAnyHeader

  .method /*06000036*/ public hidebysig specialname 
          instance bool  get_AllowAnyMethod() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicy::get_AllowAnyMethod

  .method /*06000037*/ public hidebysig specialname 
          instance bool  get_AllowAnyOrigin() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicy::get_AllowAnyOrigin

  .method /*06000038*/ assembly hidebysig specialname 
          instance bool  get_IsDefaultIsOriginAllowed() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000088:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicy::get_IsDefaultIsOriginAllowed

  .method /*06000039*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Func`2/*01000033*/<string,bool> 
          get_IsOriginAllowed() cil managed
  // SIG: 20 00 15 12 80 CD 02 0E 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicy::get_IsOriginAllowed

  .method /*0600003A*/ public hidebysig specialname 
          instance void  set_IsOriginAllowed(class [System.Runtime/*23000001*/]System.Func`2/*01000033*/<string,bool> 'value') cil managed
  // SIG: 20 01 01 15 12 80 CD 02 0E 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicy::set_IsOriginAllowed

  .method /*0600003B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string> 
          get_ExposedHeaders() cil managed
  // SIG: 20 00 15 12 80 D1 01 0E
  {
    .custom /*0C00008A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicy::get_ExposedHeaders

  .method /*0600003C*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string> 
          get_Headers() cil managed
  // SIG: 20 00 15 12 80 D1 01 0E
  {
    .custom /*0C00008B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicy::get_Headers

  .method /*0600003D*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string> 
          get_Methods() cil managed
  // SIG: 20 00 15 12 80 D1 01 0E
  {
    .custom /*0C00008C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicy::get_Methods

  .method /*0600003E*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string> 
          get_Origins() cil managed
  // SIG: 20 00 15 12 80 D1 01 0E
  {
    .custom /*0C00008D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicy::get_Origins

  .method /*0600003F*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000035*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000036*/> 
          get_PreflightMaxAge() cil managed
  // SIG: 20 00 15 11 80 D5 01 11 80 D9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicy::get_PreflightMaxAge

  .method /*06000040*/ public hidebysig specialname 
          instance void  set_PreflightMaxAge(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000035*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000036*/> 'value') cil managed
  // SIG: 20 01 01 15 11 80 D5 01 11 80 D9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicy::set_PreflightMaxAge

  .method /*06000041*/ public hidebysig specialname 
          instance bool  get_SupportsCredentials() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000090:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicy::get_SupportsCredentials

  .method /*06000042*/ public hidebysig specialname 
          instance void  set_SupportsCredentials(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000091:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicy::set_SupportsCredentials

  .method /*06000043*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicy::ToString

  .property /*1700000A*/ instance callconv(8) bool
          AllowAnyHeader()
  {
    .get instance bool Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/::get_AllowAnyHeader() /* 06000035 */
  } // end of property CorsPolicy::AllowAnyHeader
  .property /*1700000B*/ instance callconv(8) bool
          AllowAnyMethod()
  {
    .get instance bool Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/::get_AllowAnyMethod() /* 06000036 */
  } // end of property CorsPolicy::AllowAnyMethod
  .property /*1700000C*/ instance callconv(8) bool
          AllowAnyOrigin()
  {
    .get instance bool Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/::get_AllowAnyOrigin() /* 06000037 */
  } // end of property CorsPolicy::AllowAnyOrigin
  .property /*1700000D*/ instance callconv(8) bool
          IsDefaultIsOriginAllowed()
  {
    .get instance bool Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/::get_IsDefaultIsOriginAllowed() /* 06000038 */
  } // end of property CorsPolicy::IsDefaultIsOriginAllowed
  .property /*1700000E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Func`2/*01000033*/<string,bool>
          IsOriginAllowed()
  {
    .get instance class [System.Runtime/*23000001*/]System.Func`2/*01000033*/<string,bool> Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/::get_IsOriginAllowed() /* 06000039 */
    .set instance void Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/::set_IsOriginAllowed(class [System.Runtime/*23000001*/]System.Func`2/*01000033*/<string,bool>) /* 0600003A */
  } // end of property CorsPolicy::IsOriginAllowed
  .property /*1700000F*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string>
          ExposedHeaders()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string> Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/::get_ExposedHeaders() /* 0600003B */
  } // end of property CorsPolicy::ExposedHeaders
  .property /*17000010*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string>
          Headers()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string> Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/::get_Headers() /* 0600003C */
  } // end of property CorsPolicy::Headers
  .property /*17000011*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string>
          Methods()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string> Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/::get_Methods() /* 0600003D */
  } // end of property CorsPolicy::Methods
  .property /*17000012*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string>
          Origins()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string> Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/::get_Origins() /* 0600003E */
  } // end of property CorsPolicy::Origins
  .property /*17000013*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000035*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000036*/>
          PreflightMaxAge()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000035*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000036*/> Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/::get_PreflightMaxAge() /* 0600003F */
    .set instance void Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/::set_PreflightMaxAge(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000035*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000036*/>) /* 06000040 */
  } // end of property CorsPolicy::PreflightMaxAge
  .property /*17000014*/ instance callconv(8) bool
          SupportsCredentials()
  {
    .get instance bool Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/::get_SupportsCredentials() /* 06000041 */
    .set instance void Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/::set_SupportsCredentials(bool) /* 06000042 */
  } // end of property CorsPolicy::SupportsCredentials
} // end of class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy

.class /*02000012*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00005C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000044*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string[] origins) cil managed
  // SIG: 20 01 01 1D 0E
  {
    .param [1]/*0800003E*/ 
    .custom /*0C00008E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000037*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicyBuilder::.ctor

  .method /*06000045*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/ policy) cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicyBuilder::.ctor

  .method /*06000046*/ public hidebysig instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder/*02000012*/ 
          WithOrigins(string[] origins) cil managed
  // SIG: 20 01 12 48 1D 0E
  {
    .param [1]/*08000040*/ 
    .custom /*0C00008F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000037*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicyBuilder::WithOrigins

  .method /*06000047*/ assembly hidebysig static 
          string  GetNormalizedOrigin(string origin) cil managed
  // SIG: 00 01 0E 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicyBuilder::GetNormalizedOrigin

  .method /*06000048*/ public hidebysig instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder/*02000012*/ 
          WithHeaders(string[] headers) cil managed
  // SIG: 20 01 12 48 1D 0E
  {
    .param [1]/*08000042*/ 
    .custom /*0C000092:0A00001A*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000037*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicyBuilder::WithHeaders

  .method /*06000049*/ public hidebysig instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder/*02000012*/ 
          WithExposedHeaders(string[] exposedHeaders) cil managed
  // SIG: 20 01 12 48 1D 0E
  {
    .param [1]/*08000043*/ 
    .custom /*0C000093:0A00001A*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000037*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicyBuilder::WithExposedHeaders

  .method /*0600004A*/ public hidebysig instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder/*02000012*/ 
          WithMethods(string[] methods) cil managed
  // SIG: 20 01 12 48 1D 0E
  {
    .param [1]/*08000044*/ 
    .custom /*0C000094:0A00001A*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000037*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicyBuilder::WithMethods

  .method /*0600004B*/ public hidebysig instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder/*02000012*/ 
          AllowCredentials() cil managed
  // SIG: 20 00 12 48
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicyBuilder::AllowCredentials

  .method /*0600004C*/ public hidebysig instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder/*02000012*/ 
          DisallowCredentials() cil managed
  // SIG: 20 00 12 48
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicyBuilder::DisallowCredentials

  .method /*0600004D*/ public hidebysig instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder/*02000012*/ 
          AllowAnyOrigin() cil managed
  // SIG: 20 00 12 48
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicyBuilder::AllowAnyOrigin

  .method /*0600004E*/ public hidebysig instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder/*02000012*/ 
          AllowAnyMethod() cil managed
  // SIG: 20 00 12 48
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicyBuilder::AllowAnyMethod

  .method /*0600004F*/ public hidebysig instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder/*02000012*/ 
          AllowAnyHeader() cil managed
  // SIG: 20 00 12 48
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicyBuilder::AllowAnyHeader

  .method /*06000050*/ public hidebysig instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder/*02000012*/ 
          SetPreflightMaxAge(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000036*/ preflightMaxAge) cil managed
  // SIG: 20 01 12 48 11 80 D9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicyBuilder::SetPreflightMaxAge

  .method /*06000051*/ public hidebysig instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder/*02000012*/ 
          SetIsOriginAllowed(class [System.Runtime/*23000001*/]System.Func`2/*01000033*/<string,bool> isOriginAllowed) cil managed
  // SIG: 20 01 12 48 15 12 80 CD 02 0E 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicyBuilder::SetIsOriginAllowed

  .method /*06000052*/ public hidebysig instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder/*02000012*/ 
          SetIsOriginAllowedToAllowWildcardSubdomains() cil managed
  // SIG: 20 00 12 48
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicyBuilder::SetIsOriginAllowedToAllowWildcardSubdomains

  .method /*06000053*/ public hidebysig instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/ 
          Build() cil managed
  // SIG: 20 00 12 44
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicyBuilder::Build

} // end of class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder

.class /*02000013*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000061:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*02000020*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000081:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000018*/ public static initonly class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyExtensions/*02000013*//'<>c'/*02000020*/ '<>9'
    .field /*04000019*/ public static class [System.Runtime/*23000001*/]System.Func`2/*01000033*/<string,bool> '<>9__1_0'
    .method /*0600007B*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*0600007C*/ assembly hidebysig 
            instance bool  '<IsOriginAnAllowedSubdomain>b__1_0'(string o) cil managed
    // SIG: 20 01 02 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<IsOriginAnAllowedSubdomain>b__1_0'

  } // end of class '<>c'

  .class /*02000021*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass1_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000083:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400001A*/ public class [System.Runtime/*23000001*/]System.Uri/*01000038*/ originUri
    .method /*0600007D*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass1_0'::.ctor

    .method /*0600007E*/ assembly hidebysig 
            instance bool  '<IsOriginAnAllowedSubdomain>b__1'(class [System.Runtime/*23000001*/]System.Uri/*01000038*/ domain) cil managed
    // SIG: 20 01 02 12 80 E1
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass1_0'::'<IsOriginAnAllowedSubdomain>b__1'

  } // end of class '<>c__DisplayClass1_0'

  .method /*06000054*/ public hidebysig static 
          bool  IsOriginAnAllowedSubdomain(class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/ policy,
                                           string origin) cil managed
  // SIG: 00 02 02 12 44 0E
  {
    .custom /*0C000095:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000096:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsPolicyExtensions::IsOriginAnAllowedSubdomain

} // end of class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyExtensions

.class /*02000014*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Cors.Infrastructure.CorsResult
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000065:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000066:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000055*/ public hidebysig specialname 
          instance bool  get_IsPreflightRequest() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000097:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsResult::get_IsPreflightRequest

  .method /*06000056*/ public hidebysig specialname 
          instance void  set_IsPreflightRequest(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000098:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsResult::set_IsPreflightRequest

  .method /*06000057*/ public hidebysig specialname 
          instance string  get_AllowedOrigin() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000099:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00009A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsResult::get_AllowedOrigin

  .method /*06000058*/ public hidebysig specialname 
          instance void  set_AllowedOrigin(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00009B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00009C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsResult::set_AllowedOrigin

  .method /*06000059*/ public hidebysig specialname 
          instance bool  get_IsOriginAllowed() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00009D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsResult::get_IsOriginAllowed

  .method /*0600005A*/ public hidebysig specialname 
          instance void  set_IsOriginAllowed(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C00009E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsResult::set_IsOriginAllowed

  .method /*0600005B*/ public hidebysig specialname 
          instance bool  get_SupportsCredentials() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00009F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsResult::get_SupportsCredentials

  .method /*0600005C*/ public hidebysig specialname 
          instance void  set_SupportsCredentials(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000A0:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsResult::set_SupportsCredentials

  .method /*0600005D*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string> 
          get_AllowedMethods() cil managed
  // SIG: 20 00 15 12 80 D1 01 0E
  {
    .custom /*0C0000A1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsResult::get_AllowedMethods

  .method /*0600005E*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string> 
          get_AllowedHeaders() cil managed
  // SIG: 20 00 15 12 80 D1 01 0E
  {
    .custom /*0C0000A2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsResult::get_AllowedHeaders

  .method /*0600005F*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string> 
          get_AllowedExposedHeaders() cil managed
  // SIG: 20 00 15 12 80 D1 01 0E
  {
    .custom /*0C0000A4:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsResult::get_AllowedExposedHeaders

  .method /*06000060*/ public hidebysig specialname 
          instance bool  get_VaryByOrigin() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000A5:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsResult::get_VaryByOrigin

  .method /*06000061*/ public hidebysig specialname 
          instance void  set_VaryByOrigin(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000A7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsResult::set_VaryByOrigin

  .method /*06000062*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000035*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000036*/> 
          get_PreflightMaxAge() cil managed
  // SIG: 20 00 15 11 80 D5 01 11 80 D9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsResult::get_PreflightMaxAge

  .method /*06000063*/ public hidebysig specialname 
          instance void  set_PreflightMaxAge(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000035*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000036*/> 'value') cil managed
  // SIG: 20 01 01 15 11 80 D5 01 11 80 D9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsResult::set_PreflightMaxAge

  .method /*06000064*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsResult::ToString

  .method /*06000065*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsResult::.ctor

  .property /*17000015*/ instance callconv(8) bool
          IsPreflightRequest()
  {
    .get instance bool Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/::get_IsPreflightRequest() /* 06000055 */
    .set instance void Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/::set_IsPreflightRequest(bool) /* 06000056 */
  } // end of property CorsResult::IsPreflightRequest
  .property /*17000016*/ instance callconv(8) string
          AllowedOrigin()
  {
    .custom /*0C00006F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/::get_AllowedOrigin() /* 06000057 */
    .set instance void Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/::set_AllowedOrigin(string) /* 06000058 */
  } // end of property CorsResult::AllowedOrigin
  .property /*17000017*/ instance callconv(8) bool
          IsOriginAllowed()
  {
    .get instance bool Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/::get_IsOriginAllowed() /* 06000059 */
    .set instance void Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/::set_IsOriginAllowed(bool) /* 0600005A */
  } // end of property CorsResult::IsOriginAllowed
  .property /*17000018*/ instance callconv(8) bool
          SupportsCredentials()
  {
    .get instance bool Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/::get_SupportsCredentials() /* 0600005B */
    .set instance void Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/::set_SupportsCredentials(bool) /* 0600005C */
  } // end of property CorsResult::SupportsCredentials
  .property /*17000019*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string>
          AllowedMethods()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string> Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/::get_AllowedMethods() /* 0600005D */
  } // end of property CorsResult::AllowedMethods
  .property /*1700001A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string>
          AllowedHeaders()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string> Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/::get_AllowedHeaders() /* 0600005E */
  } // end of property CorsResult::AllowedHeaders
  .property /*1700001B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string>
          AllowedExposedHeaders()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000034*/<string> Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/::get_AllowedExposedHeaders() /* 0600005F */
  } // end of property CorsResult::AllowedExposedHeaders
  .property /*1700001C*/ instance callconv(8) bool
          VaryByOrigin()
  {
    .get instance bool Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/::get_VaryByOrigin() /* 06000060 */
    .set instance void Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/::set_VaryByOrigin(bool) /* 06000061 */
  } // end of property CorsResult::VaryByOrigin
  .property /*1700001D*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000035*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000036*/>
          PreflightMaxAge()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000035*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000036*/> Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/::get_PreflightMaxAge() /* 06000062 */
    .set instance void Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/::set_PreflightMaxAge(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000035*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000036*/>) /* 06000063 */
  } // end of property CorsResult::PreflightMaxAge
} // end of class Microsoft.AspNetCore.Cors.Infrastructure.CorsResult

.class /*02000015*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Cors.Infrastructure.CorsService
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.Cors.Infrastructure.ICorsService/*02000019*/
{
  .custom /*0C00006A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00006B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Cors.Infrastructure.ICorsService/*02000019*/
  .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000066*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Options/*23000005*/]Microsoft.Extensions.Options.IOptions`1/*01000039*/<class Microsoft.AspNetCore.Cors.Infrastructure.CorsOptions/*02000010*/> options,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILoggerFactory/*01000026*/ loggerFactory) cil managed
  // SIG: 20 02 01 15 12 80 E5 01 12 40 12 80 99
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsService::.ctor

  .method /*06000067*/ public hidebysig instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/ 
          EvaluatePolicy(class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Http.HttpContext/*01000028*/ context,
                         string policyName) cil managed
  // SIG: 20 02 12 50 12 80 A1 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsService::EvaluatePolicy

  .method /*06000068*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/ 
          EvaluatePolicy(class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Http.HttpContext/*01000028*/ context,
                         class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/ policy) cil managed
  // SIG: 20 02 12 50 12 80 A1 12 44
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsService::EvaluatePolicy

  .method /*06000069*/ public hidebysig newslot virtual 
          instance void  EvaluateRequest(class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Http.HttpContext/*01000028*/ context,
                                         class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/ policy,
                                         class Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/ result) cil managed
  // SIG: 20 03 01 12 80 A1 12 44 12 50
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsService::EvaluateRequest

  .method /*0600006A*/ public hidebysig newslot virtual 
          instance void  EvaluatePreflightRequest(class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Http.HttpContext/*01000028*/ context,
                                                  class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/ policy,
                                                  class Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/ result) cil managed
  // SIG: 20 03 01 12 80 A1 12 44 12 50
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsService::EvaluatePreflightRequest

  .method /*0600006B*/ public hidebysig newslot virtual 
          instance void  ApplyResult(class Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/ result,
                                     class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Http.HttpResponse/*0100003A*/ response) cil managed
  // SIG: 20 02 01 12 50 12 80 E9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorsService::ApplyResult

} // end of class Microsoft.AspNetCore.Cors.Infrastructure.CorsService

.class /*02000016*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Cors.Infrastructure.DefaultCorsPolicyProvider
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.Cors.Infrastructure.ICorsPolicyProvider/*02000018*/
{
  .method /*0600006C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Options/*23000005*/]Microsoft.Extensions.Options.IOptions`1/*01000039*/<class Microsoft.AspNetCore.Cors.Infrastructure.CorsOptions/*02000010*/> options) cil managed
  // SIG: 20 01 01 15 12 80 E5 01 12 40
  {
    .custom /*0C0000AA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultCorsPolicyProvider::.ctor

  .method /*0600006D*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002C*/<class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/> 
          GetPolicyAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Http.HttpContext/*01000028*/ context,
                         string policyName) cil managed
  // SIG: 20 02 15 12 80 B1 01 12 44 12 80 A1 0E
  {
    .custom /*0C0000AB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [0]/*0800005E*/ 
    .custom /*0C0000A3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000060*/ 
    .custom /*0C0000A6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultCorsPolicyProvider::GetPolicyAsync

} // end of class Microsoft.AspNetCore.Cors.Infrastructure.DefaultCorsPolicyProvider

.class /*02000017*/ interface public abstract auto ansi Microsoft.AspNetCore.Cors.Infrastructure.ICorsPolicyMetadata
       implements [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Cors.Infrastructure.ICorsMetadata/*01000022*/
{
  .custom /*0C000073:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Cors.Infrastructure.ICorsMetadata/*01000022*/
  .custom /*0C000039:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600006E*/ public hidebysig newslot specialname abstract virtual 
          instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/ 
          get_Policy() cil managed
  // SIG: 20 00 12 44
  {
    // Method begins at RVA 0x0
  } // end of method ICorsPolicyMetadata::get_Policy

  .property /*1700001E*/ instance callconv(8) class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/
          Policy()
  {
    .get instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/ Microsoft.AspNetCore.Cors.Infrastructure.ICorsPolicyMetadata/*02000017*/::get_Policy() /* 0600006E */
  } // end of property ICorsPolicyMetadata::Policy
} // end of class Microsoft.AspNetCore.Cors.Infrastructure.ICorsPolicyMetadata

.class /*02000018*/ interface public abstract auto ansi Microsoft.AspNetCore.Cors.Infrastructure.ICorsPolicyProvider
{
  .custom /*0C000075:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600006F*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002C*/<class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/> 
          GetPolicyAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Http.HttpContext/*01000028*/ context,
                         string policyName) cil managed
  // SIG: 20 02 15 12 80 B1 01 12 44 12 80 A1 0E
  {
    .param [0]/*08000061*/ 
    .custom /*0C0000A8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000063*/ 
    .custom /*0C0000A9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ICorsPolicyProvider::GetPolicyAsync

} // end of class Microsoft.AspNetCore.Cors.Infrastructure.ICorsPolicyProvider

.class /*02000019*/ interface public abstract auto ansi Microsoft.AspNetCore.Cors.Infrastructure.ICorsService
{
  .custom /*0C000078:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000070*/ public hidebysig newslot abstract virtual 
          instance class Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/ 
          EvaluatePolicy(class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Http.HttpContext/*01000028*/ context,
                         class Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy/*02000011*/ policy) cil managed
  // SIG: 20 02 12 50 12 80 A1 12 44
  {
    // Method begins at RVA 0x0
  } // end of method ICorsService::EvaluatePolicy

  .method /*06000071*/ public hidebysig newslot abstract virtual 
          instance void  ApplyResult(class Microsoft.AspNetCore.Cors.Infrastructure.CorsResult/*02000014*/ result,
                                     class [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Http.HttpResponse/*0100003A*/ response) cil managed
  // SIG: 20 02 01 12 50 12 80 E9
  {
    // Method begins at RVA 0x0
  } // end of method ICorsService::ApplyResult

} // end of class Microsoft.AspNetCore.Cors.Infrastructure.ICorsService

.class /*0200001A*/ interface public abstract auto ansi Microsoft.AspNetCore.Cors.Infrastructure.IDisableCorsAttribute
       implements [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Cors.Infrastructure.ICorsMetadata/*01000022*/
{
} // end of class Microsoft.AspNetCore.Cors.Infrastructure.IDisableCorsAttribute

.class /*0200001B*/ interface public abstract auto ansi Microsoft.AspNetCore.Cors.Infrastructure.IEnableCorsAttribute
       implements [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Cors.Infrastructure.ICorsMetadata/*01000022*/
{
  .custom /*0C00007A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Http.Abstractions/*23000003*/]Microsoft.AspNetCore.Cors.Infrastructure.ICorsMetadata/*01000022*/
  .custom /*0C00003D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000072*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_PolicyName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method IEnableCorsAttribute::get_PolicyName

  .method /*06000073*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_PolicyName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method IEnableCorsAttribute::set_PolicyName

  .property /*1700001F*/ instance callconv(8) string
          PolicyName()
  {
    .get instance string Microsoft.AspNetCore.Cors.Infrastructure.IEnableCorsAttribute/*0200001B*/::get_PolicyName() /* 06000072 */
    .set instance void Microsoft.AspNetCore.Cors.Infrastructure.IEnableCorsAttribute/*0200001B*/::set_PolicyName(string) /* 06000073 */
  } // end of property IEnableCorsAttribute::PolicyName
} // end of class Microsoft.AspNetCore.Cors.Infrastructure.IEnableCorsAttribute

.class /*0200001C*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Cors.Infrastructure.UriHelpers
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*06000074*/ public hidebysig static 
          bool  IsSubdomainOf(class [System.Runtime/*23000001*/]System.Uri/*01000038*/ subdomain,
                              class [System.Runtime/*23000001*/]System.Uri/*01000038*/ domain) cil managed
  // SIG: 00 02 02 12 80 E1 12 80 E1
  {
    .custom /*0C0000AC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UriHelpers::IsSubdomainOf

} // end of class Microsoft.AspNetCore.Cors.Infrastructure.UriHelpers


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
