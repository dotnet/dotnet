


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
// Time-date stamp:            0x8ab2a1de
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00001c00
// Size of init.data:              0x00000a00
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00005b0a
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
// Size of image:                  0x0000a000
// Size of headers:                0x00000400
// Checksum:                       0x00006b6f
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00005ab8 [0x0000004f] address [size] of Import Directory:          
// 0x00006000 [0x00000568] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002a00 [0x00002468] address [size] of Security Directory:        
// 0x00008000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00005a9c [0x0000001c] address [size] of Debug Directory:           
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
//              0x00001b10 Virtual Size
//              0x00004000 Virtual Address
//              0x00001c00 Size of Raw Data
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
//              0x00000568 Virtual Size
//              0x00006000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00002200 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x00008000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x00002800 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00005000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000b0c
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00004000 Import Address Table
//              0x00005afa Import Name Table
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
// 0x00004054 [0x000019c8] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00005a1c [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000998 Size
//              '#~' Name
//    Stream 2:
//              0x00000a04 Offset
//              0x00000910 Size
//              '#Strings' Name
//    Stream 3:
//              0x00001314 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00001318 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001328 Offset
//              0x000006a0 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000020903a01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 20072
// PE header size       : 1024 (536 used)    ( 5.10%)
// PE additional info   : 10903             (54.32%)
// Num.of PE sections   : 4
// CLR header size     : 72                 ( 0.36%)
// CLR meta-data size  : 6600               (32.88%)
// CLR additional info : 128                ( 0.64%)
// CLR method headers  : 47                 ( 0.23%)
// Managed code         : 94                ( 0.47%)
// Data                 : 2560              (12.75%)
// Unaccounted          : -1356             (-6.76%)

// Num.of PE sections   : 4
//   .mvid    - 512
//   .text    - 7168
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 6600
//   Module        -    1 (10 bytes)
//   TypeDef       -   17 (238 bytes)     1 interfaces, 0 explicit layout
//   TypeRef       -   35 (210 bytes)
//   MethodDef     -   50 (700 bytes)     3 abstract, 0 native, 47 bodies
//   FieldDef      -   10 (60 bytes)      2 constant
//   MemberRef     -   22 (132 bytes)
//   ParamDef      -   62 (372 bytes)
//   MethodImpl    -    3 (18 bytes)
//   Constant      -    1 (6 bytes)
//   CustomAttribute-   63 (378 bytes)
//   InterfaceImpl -    7 (28 bytes)
//   PropertyMap   -    1 (4 bytes)
//   Property      -    5 (30 bytes)
//   MethodSemantic-    6 (36 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    5 (100 bytes)
//   NestedClass   -    4 (16 bytes)
//   Strings       -  2318 bytes
//   Blobs         -  1696 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   206 bytes

// CLR additional info : 128

// CLR method headers : 47
//   Num.of method bodies  - 47
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 47

// Managed code : 94
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class BitHelpers                     (abstract) (auto) (ansi) (sealed) 
// Class DataProtectionAdvancedExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class DataProtectionProvider         (public) (abstract) (auto) (ansi) (sealed) 
// Interface ITimeLimitedDataProtector      (public) (abstract) (auto) (ansi) 
// Class TimeLimitedDataProtector       (auto) (ansi) (sealed) 
// Class EncodingUtil                   (abstract) (auto) (ansi) (sealed) 
// Class ExceptionExtensions            (abstract) (auto) (ansi) (sealed) 
// Class Resources                      (abstract) (auto) (ansi) (sealed) 
// Class TimeLimitedWrappingProtector   (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass0_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass3_0           (auto) (ansi) (sealed) (nested private) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ Microsoft.AspNetCore.DataProtection.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ Microsoft.AspNetCore.DataProtection
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ System.Security.Cryptography.X509Certificates
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000005*/ System.Text.Encoding.Extensions
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly /*20000001*/ Microsoft.AspNetCore.DataProtection.Extensions
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
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 81 80 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ....Microsoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 44 61 74 61 50 72 6F   // pNetCore.DataPro
                                                                                                                                                                                    74 65 63 74 69 6F 6E 2E 45 78 74 65 6E 73 69 6F   // tection.Extensio
                                                                                                                                                                                    6E 73 2E 54 65 73 74 73 2C 20 50 75 62 6C 69 63   // ns.Tests, Public
                                                                                                                                                                                    4B 65 79 3D 30 30 32 34 30 30 30 30 30 34 38 30   // Key=002400000480
                                                                                                                                                                                    30 30 30 30 39 34 30 30 30 30 30 30 30 36 30 32   // 0000940000000602
                                                                                                                                                                                    30 30 30 30 30 30 32 34 30 30 30 30 35 32 35 33   // 0000002400005253
                                                                                                                                                                                    34 31 33 31 30 30 30 34 30 30 30 30 30 31 30 30   // 4131000400000100
                                                                                                                                                                                    30 31 30 30 66 33 33 61 32 39 30 34 34 66 61 39   // 0100f33a29044fa9
                                                                                                                                                                                    64 37 34 30 63 39 62 33 32 31 33 61 39 33 65 35   // d740c9b3213a93e5
                                                                                                                                                                                    37 63 38 34 62 34 37 32 63 38 34 65 30 62 38 61   // 7c84b472c84e0b8a
                                                                                                                                                                                    30 65 31 61 65 34 38 65 36 37 61 39 66 38 66 36   // 0e1ae48e67a9f8f6
                                                                                                                                                                                    64 65 39 64 35 66 37 66 33 64 35 32 61 63 32 33   // de9d5f7f3d52ac23
                                                                                                                                                                                    65 34 38 61 63 35 31 38 30 31 66 31 64 63 39 35   // e48ac51801f1dc95
                                                                                                                                                                                    30 61 62 65 39 30 31 64 61 33 34 64 32 61 39 65   // 0abe901da34d2a9e
                                                                                                                                                                                    33 62 61 61 64 62 31 34 31 61 31 37 63 37 37 65   // 3baadb141a17c77e
                                                                                                                                                                                    66 33 63 35 36 35 64 64 35 65 65 35 30 35 34 62   // f3c565dd5ee5054b
                                                                                                                                                                                    39 31 63 66 36 33 62 62 33 63 36 61 62 38 33 66   // 91cf63bb3c6ab83f
                                                                                                                                                                                    37 32 61 62 33 61 61 66 65 39 33 64 30 66 63 33   // 72ab3aafe93d0fc3
                                                                                                                                                                                    63 32 33 34 38 62 37 36 34 66 61 66 62 30 62 31   // c2348b764fafb0b1
                                                                                                                                                                                    63 30 37 33 33 64 65 35 31 34 35 39 61 65 61 62   // c0733de51459aeab
                                                                                                                                                                                    34 36 35 38 30 33 38 34 62 66 39 64 37 34 63 34   // 46580384bf9d74c4
                                                                                                                                                                                    65 32 38 31 36 34 62 37 63 64 65 32 34 37 66 38   // e28164b7cde247f8
                                                                                                                                                                                    39 31 62 61 30 37 38 39 31 63 39 64 38 37 32 61   // 91ba07891c9d872a
                                                                                                                                                                                    64 32 62 62 00 00 )                               // d2bb..
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
  .custom /*0C000010:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 31 41 64 64 69 74 69 6F 6E 61 6C 20 41 50   // ..1Additional AP
                                                                                                                                                                       49 73 20 66 6F 72 20 41 53 50 2E 4E 45 54 20 43   // Is for ASP.NET C
                                                                                                                                                                       6F 72 65 20 64 61 74 61 20 70 72 6F 74 65 63 74   // ore data protect
                                                                                                                                                                       69 6F 6E 2E 00 00 )                               // ion...
  .custom /*0C000011:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 36 2E 30 2E 32 32 32 2E 36 34 31 32 00   // ...6.0.222.6412.
                                                                                                                                                                       00 ) 
  .custom /*0C000012:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 36 2E 30 2E 32 2B 31 64 63 66 37 61 63   // ...6.0.2+1dcf7ac
                                                                                                                                                                                66 61 63 66 30 66 65 31 35 34 61 64 63 63 32 33   // facf0fe154adcc23
                                                                                                                                                                                32 37 30 63 62 30 64 61 31 31 66 66 34 34 61 63   // 270cb0da11ff44ac
                                                                                                                                                                                65 00 00 )                                        // e..
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 16 4D 69 63 72 6F 73 6F 66 74 20 41 53 50   // ...Microsoft ASP
                                                                                                                                                                   2E 4E 45 54 20 43 6F 72 65 00 00 )                // .NET Core..
  .custom /*0C000014:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 2E 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ...Microsoft.Asp
                                                                                                                                                                 4E 65 74 43 6F 72 65 2E 44 61 74 61 50 72 6F 74   // NetCore.DataProt
                                                                                                                                                                 65 63 74 69 6F 6E 2E 45 78 74 65 6E 73 69 6F 6E   // ection.Extension
                                                                                                                                                                 73 00 00 )                                        // s..
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
.module Microsoft.AspNetCore.DataProtection.Extensions.dll
// MVID: {adb4700c-4054-418e-bb86-1af3c5d80185}
.custom /*0C000002:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 01 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C000017:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C00001A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C00001B:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001C:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
  .custom /*0C00001F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000020:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000021:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
  .custom /*0C000023:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000024:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000025:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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

.class /*02000006*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.DataProtection.BitHelpers
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000027:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000028:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000006*/ public hidebysig static 
          uint64  ReadUInt64(uint8[] buffer,
                             int32 offset) cil managed
  // SIG: 00 02 0B 1D 05 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BitHelpers::ReadUInt64

  .method /*06000007*/ public hidebysig static 
          void  WriteUInt64(uint8[] buffer,
                            int32 offset,
                            uint64 'value') cil managed
  // SIG: 00 03 01 1D 05 08 0B
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BitHelpers::WriteUInt64

} // end of class Microsoft.AspNetCore.DataProtection.BitHelpers

.class /*02000007*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.DataProtection.DataProtectionAdvancedExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000029:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00002B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*0200000E*/ auto ansi sealed nested private beforefieldinit TimeLimitedWrappingProtector
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
         implements [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtector/*0100001A*/,
                    [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtectionProvider/*0100001B*/
  {
    .field /*04000005*/ public valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000019*/ Expiration
    .method /*06000028*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class Microsoft.AspNetCore.DataProtection.ITimeLimitedDataProtector/*02000009*/ innerProtector) cil managed
    // SIG: 20 01 01 12 24
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TimeLimitedWrappingProtector::.ctor

    .method /*06000029*/ public hidebysig newslot virtual final 
            instance class [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtector/*0100001A*/ 
            CreateProtector(string purpose) cil managed
    // SIG: 20 01 12 69 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TimeLimitedWrappingProtector::CreateProtector

    .method /*0600002A*/ public hidebysig newslot virtual final 
            instance uint8[]  Protect(uint8[] plaintext) cil managed
    // SIG: 20 01 1D 05 1D 05
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TimeLimitedWrappingProtector::Protect

    .method /*0600002B*/ public hidebysig newslot virtual final 
            instance uint8[]  Unprotect(uint8[] protectedData) cil managed
    // SIG: 20 01 1D 05 1D 05
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TimeLimitedWrappingProtector::Unprotect

  } // end of class TimeLimitedWrappingProtector

  .method /*06000008*/ public hidebysig static 
          uint8[]  Protect(class Microsoft.AspNetCore.DataProtection.ITimeLimitedDataProtector/*02000009*/ protector,
                           uint8[] plaintext,
                           valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000018*/ lifetime) cil managed
  // SIG: 00 03 1D 05 12 24 1D 05 11 61
  {
    .custom /*0C00002C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectionAdvancedExtensions::Protect

  .method /*06000009*/ public hidebysig static 
          string  Protect(class Microsoft.AspNetCore.DataProtection.ITimeLimitedDataProtector/*02000009*/ protector,
                          string plaintext,
                          valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000019*/ expiration) cil managed
  // SIG: 00 03 0E 12 24 0E 11 65
  {
    .custom /*0C00002F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectionAdvancedExtensions::Protect

  .method /*0600000A*/ public hidebysig static 
          string  Protect(class Microsoft.AspNetCore.DataProtection.ITimeLimitedDataProtector/*02000009*/ protector,
                          string plaintext,
                          valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000018*/ lifetime) cil managed
  // SIG: 00 03 0E 12 24 0E 11 61
  {
    .custom /*0C000031:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectionAdvancedExtensions::Protect

  .method /*0600000B*/ public hidebysig static 
          class Microsoft.AspNetCore.DataProtection.ITimeLimitedDataProtector/*02000009*/ 
          ToTimeLimitedDataProtector(class [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtector/*0100001A*/ protector) cil managed
  // SIG: 00 01 12 24 12 69
  {
    .custom /*0C000034:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectionAdvancedExtensions::ToTimeLimitedDataProtector

  .method /*0600000C*/ public hidebysig static 
          string  Unprotect(class Microsoft.AspNetCore.DataProtection.ITimeLimitedDataProtector/*02000009*/ protector,
                            string protectedData,
                            [out] valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000019*/& expiration) cil managed
  // SIG: 00 03 0E 12 24 0E 10 11 65
  {
    .custom /*0C000035:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectionAdvancedExtensions::Unprotect

} // end of class Microsoft.AspNetCore.DataProtection.DataProtectionAdvancedExtensions

.class /*02000008*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.DataProtection.DataProtectionProvider
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00002D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200000F*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000037:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000006*/ public static initonly class Microsoft.AspNetCore.DataProtection.DataProtectionProvider/*02000008*//'<>c'/*0200000F*/ '<>9'
    .field /*04000007*/ public static class [System.Runtime/*23000001*/]System.Action`1/*0100001D*/<class [Microsoft.AspNetCore.DataProtection/*23000003*/]Microsoft.AspNetCore.DataProtection.IDataProtectionBuilder/*0100001E*/> '<>9__1_0'
    .field /*04000008*/ public static class [System.Runtime/*23000001*/]System.Action`1/*0100001D*/<class [Microsoft.AspNetCore.DataProtection/*23000003*/]Microsoft.AspNetCore.DataProtection.IDataProtectionBuilder/*0100001E*/> '<>9__4_0'
    .method /*0600002C*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*0600002D*/ assembly hidebysig 
            instance void  '<Create>b__1_0'(class [Microsoft.AspNetCore.DataProtection/*23000003*/]Microsoft.AspNetCore.DataProtection.IDataProtectionBuilder/*0100001E*/ builder) cil managed
    // SIG: 20 01 01 12 79
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<Create>b__1_0'

    .method /*0600002E*/ assembly hidebysig 
            instance void  '<Create>b__4_0'(class [Microsoft.AspNetCore.DataProtection/*23000003*/]Microsoft.AspNetCore.DataProtection.IDataProtectionBuilder/*0100001E*/ builder) cil managed
    // SIG: 20 01 01 12 79
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<Create>b__4_0'

  } // end of class '<>c'

  .class /*02000010*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass0_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000038:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000009*/ public string applicationName
    .method /*0600002F*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass0_0'::.ctor

    .method /*06000030*/ assembly hidebysig 
            instance void  '<Create>b__0'(class [Microsoft.AspNetCore.DataProtection/*23000003*/]Microsoft.AspNetCore.DataProtection.IDataProtectionBuilder/*0100001E*/ builder) cil managed
    // SIG: 20 01 01 12 79
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass0_0'::'<Create>b__0'

  } // end of class '<>c__DisplayClass0_0'

  .class /*02000011*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass3_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000039:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400000A*/ public string applicationName
    .method /*06000031*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass3_0'::.ctor

    .method /*06000032*/ assembly hidebysig 
            instance void  '<Create>b__0'(class [Microsoft.AspNetCore.DataProtection/*23000003*/]Microsoft.AspNetCore.DataProtection.IDataProtectionBuilder/*0100001E*/ builder) cil managed
    // SIG: 20 01 01 12 79
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass3_0'::'<Create>b__0'

  } // end of class '<>c__DisplayClass3_0'

  .method /*0600000D*/ public hidebysig static 
          class [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtectionProvider/*0100001B*/ 
          Create(string applicationName) cil managed
  // SIG: 00 01 12 6D 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectionProvider::Create

  .method /*0600000E*/ public hidebysig static 
          class [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtectionProvider/*0100001B*/ 
          Create(class [System.Runtime/*23000001*/]System.IO.DirectoryInfo/*0100001C*/ keyDirectory) cil managed
  // SIG: 00 01 12 6D 12 71
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectionProvider::Create

  .method /*0600000F*/ public hidebysig static 
          class [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtectionProvider/*0100001B*/ 
          Create(class [System.Runtime/*23000001*/]System.IO.DirectoryInfo/*0100001C*/ keyDirectory,
                 class [System.Runtime/*23000001*/]System.Action`1/*0100001D*/<class [Microsoft.AspNetCore.DataProtection/*23000003*/]Microsoft.AspNetCore.DataProtection.IDataProtectionBuilder/*0100001E*/> setupAction) cil managed
  // SIG: 00 02 12 6D 12 71 15 12 75 01 12 79
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectionProvider::Create

  .method /*06000010*/ public hidebysig static 
          class [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtectionProvider/*0100001B*/ 
          Create(string applicationName,
                 class [System.Security.Cryptography.X509Certificates/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100001F*/ certificate) cil managed
  // SIG: 00 02 12 6D 0E 12 7D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectionProvider::Create

  .method /*06000011*/ public hidebysig static 
          class [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtectionProvider/*0100001B*/ 
          Create(class [System.Runtime/*23000001*/]System.IO.DirectoryInfo/*0100001C*/ keyDirectory,
                 class [System.Security.Cryptography.X509Certificates/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100001F*/ certificate) cil managed
  // SIG: 00 02 12 6D 12 71 12 7D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectionProvider::Create

  .method /*06000012*/ public hidebysig static 
          class [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtectionProvider/*0100001B*/ 
          Create(class [System.Runtime/*23000001*/]System.IO.DirectoryInfo/*0100001C*/ keyDirectory,
                 class [System.Runtime/*23000001*/]System.Action`1/*0100001D*/<class [Microsoft.AspNetCore.DataProtection/*23000003*/]Microsoft.AspNetCore.DataProtection.IDataProtectionBuilder/*0100001E*/> setupAction,
                 class [System.Security.Cryptography.X509Certificates/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100001F*/ certificate) cil managed
  // SIG: 00 03 12 6D 12 71 15 12 75 01 12 79 12 7D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectionProvider::Create

  .method /*06000013*/ assembly hidebysig static 
          class [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtectionProvider/*0100001B*/ 
          CreateProvider(class [System.Runtime/*23000001*/]System.IO.DirectoryInfo/*0100001C*/ keyDirectory,
                         class [System.Runtime/*23000001*/]System.Action`1/*0100001D*/<class [Microsoft.AspNetCore.DataProtection/*23000003*/]Microsoft.AspNetCore.DataProtection.IDataProtectionBuilder/*0100001E*/> setupAction,
                         class [System.Security.Cryptography.X509Certificates/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100001F*/ certificate) cil managed
  // SIG: 00 03 12 6D 12 71 15 12 75 01 12 79 12 7D
  {
    .param [1]/*0800001E*/ 
    .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000020*/ 
    .custom /*0C00003D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataProtectionProvider::CreateProvider

} // end of class Microsoft.AspNetCore.DataProtection.DataProtectionProvider

.class /*02000009*/ interface public abstract auto ansi Microsoft.AspNetCore.DataProtection.ITimeLimitedDataProtector
       implements [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtector/*0100001A*/,
                  [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtectionProvider/*0100001B*/
{
  .custom /*0C000030:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtector/*0100001A*/
  .custom /*0C000001:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtectionProvider/*0100001B*/
  .custom /*0C000019:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000014*/ public hidebysig newslot abstract virtual 
          instance class Microsoft.AspNetCore.DataProtection.ITimeLimitedDataProtector/*02000009*/ 
          CreateProtector(string purpose) cil managed
  // SIG: 20 01 12 24 0E
  {
    // Method begins at RVA 0x0
  } // end of method ITimeLimitedDataProtector::CreateProtector

  .method /*06000015*/ public hidebysig newslot abstract virtual 
          instance uint8[]  Protect(uint8[] plaintext,
                                    valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000019*/ expiration) cil managed
  // SIG: 20 02 1D 05 1D 05 11 65
  {
    // Method begins at RVA 0x0
  } // end of method ITimeLimitedDataProtector::Protect

  .method /*06000016*/ public hidebysig newslot abstract virtual 
          instance uint8[]  Unprotect(uint8[] protectedData,
                                      [out] valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000019*/& expiration) cil managed
  // SIG: 20 02 1D 05 1D 05 10 11 65
  {
    // Method begins at RVA 0x0
  } // end of method ITimeLimitedDataProtector::Unprotect

} // end of class Microsoft.AspNetCore.DataProtection.ITimeLimitedDataProtector

.class /*0200000A*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.DataProtection.TimeLimitedDataProtector
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.DataProtection.ITimeLimitedDataProtector/*02000009*/,
                  [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtector/*0100001A*/,
                  [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtectionProvider/*0100001B*/
{
  .custom /*0C000032:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000033:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.DataProtection.ITimeLimitedDataProtector/*02000009*/
  .custom /*0C00001D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtector/*0100001A*/
  .custom /*0C000022:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtectionProvider/*0100001B*/
  .custom /*0C000026:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000017*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtector/*0100001A*/ innerProtector) cil managed
  // SIG: 20 01 01 12 69
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TimeLimitedDataProtector::.ctor

  .method /*06000018*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.DataProtection.ITimeLimitedDataProtector/*02000009*/ 
          CreateProtector(string purpose) cil managed
  // SIG: 20 01 12 24 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TimeLimitedDataProtector::CreateProtector

  .method /*06000019*/ public hidebysig newslot virtual final 
          instance uint8[]  Protect(uint8[] plaintext,
                                    valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000019*/ expiration) cil managed
  // SIG: 20 02 1D 05 1D 05 11 65
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TimeLimitedDataProtector::Protect

  .method /*0600001A*/ public hidebysig newslot virtual final 
          instance uint8[]  Unprotect(uint8[] protectedData,
                                      [out] valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000019*/& expiration) cil managed
  // SIG: 20 02 1D 05 1D 05 10 11 65
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TimeLimitedDataProtector::Unprotect

  .method /*0600001B*/ assembly hidebysig 
          instance uint8[]  UnprotectCore(uint8[] protectedData,
                                          valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000019*/ now,
                                          [out] valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000019*/& expiration) cil managed
  // SIG: 20 03 1D 05 1D 05 11 65 10 11 65
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TimeLimitedDataProtector::UnprotectCore

  .method /*0600001C*/ private hidebysig newslot virtual final 
          instance class [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtector/*0100001A*/ 
          Microsoft.AspNetCore.DataProtection.IDataProtectionProvider.CreateProtector(string purpose) cil managed
  // SIG: 20 01 12 69 0E
  {
    .override [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtectionProvider/*0100001B*/::CreateProtector /*0100001B::0A000014*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TimeLimitedDataProtector::Microsoft.AspNetCore.DataProtection.IDataProtectionProvider.CreateProtector

  .method /*0600001D*/ private hidebysig newslot virtual final 
          instance uint8[]  Microsoft.AspNetCore.DataProtection.IDataProtector.Protect(uint8[] plaintext) cil managed
  // SIG: 20 01 1D 05 1D 05
  {
    .override [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtector/*0100001A*/::Protect /*0100001A::0A000015*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TimeLimitedDataProtector::Microsoft.AspNetCore.DataProtection.IDataProtector.Protect

  .method /*0600001E*/ private hidebysig newslot virtual final 
          instance uint8[]  Microsoft.AspNetCore.DataProtection.IDataProtector.Unprotect(uint8[] protectedData) cil managed
  // SIG: 20 01 1D 05 1D 05
  {
    .override [Microsoft.AspNetCore.DataProtection.Abstractions/*23000002*/]Microsoft.AspNetCore.DataProtection.IDataProtector/*0100001A*/::Unprotect /*0100001A::0A000016*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TimeLimitedDataProtector::Microsoft.AspNetCore.DataProtection.IDataProtector.Unprotect

} // end of class Microsoft.AspNetCore.DataProtection.TimeLimitedDataProtector

.class /*0200000B*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.DataProtection.EncodingUtil
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .field /*04000004*/ public static initonly class [System.Text.Encoding.Extensions/*23000005*/]System.Text.UTF8Encoding/*01000020*/ SecureUtf8Encoding
  .custom /*0C00001E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
} // end of class Microsoft.AspNetCore.DataProtection.EncodingUtil

.class /*0200000C*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.DataProtection.ExceptionExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000036:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*0600001F*/ public hidebysig static 
          bool  RequiresHomogenization(class [System.Runtime/*23000001*/]System.Exception/*01000021*/ ex) cil managed
  // SIG: 00 01 02 12 80 85
  {
    .custom /*0C00003B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00003C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExceptionExtensions::RequiresHomogenization

} // end of class Microsoft.AspNetCore.DataProtection.ExceptionExtensions

.class /*0200000D*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.DataProtection.Extensions.Resources
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*06000020*/ assembly hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Resources.ResourceManager/*01000022*/ 
          get_ResourceManager() cil managed
  // SIG: 00 00 12 80 89
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_ResourceManager

  .method /*06000021*/ assembly hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000023*/ 
          get_Culture() cil managed
  // SIG: 00 00 12 80 8D
  {
    .custom /*0C00003E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_Culture

  .method /*06000022*/ assembly hidebysig specialname static 
          void  set_Culture(class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000023*/ 'value') cil managed
  // SIG: 00 01 01 12 80 8D
  {
    .custom /*0C00003F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::set_Culture

  .method /*06000023*/ assembly hidebysig static 
          string  GetResourceString(string resourceKey,
                                    [opt] string defaultValue) cil managed aggressiveinlining
  // SIG: 00 02 0E 0E 0E
  {
    .param [2]/*08000035*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::GetResourceString

  .method /*06000024*/ assembly hidebysig specialname static 
          string  get_CryptCommon_GenericError() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_CryptCommon_GenericError

  .method /*06000025*/ assembly hidebysig specialname static 
          string  get_TimeLimitedDataProtector_PayloadExpired() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_TimeLimitedDataProtector_PayloadExpired

  .method /*06000026*/ assembly hidebysig static 
          string  FormatTimeLimitedDataProtector_PayloadExpired(object p0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::FormatTimeLimitedDataProtector_PayloadExpired

  .method /*06000027*/ assembly hidebysig specialname static 
          string  get_TimeLimitedDataProtector_PayloadInvalid() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_TimeLimitedDataProtector_PayloadInvalid

  .property /*17000001*/ callconv(8) class [System.Runtime/*23000001*/]System.Resources.ResourceManager/*01000022*/
          ResourceManager()
  {
    .get class [System.Runtime/*23000001*/]System.Resources.ResourceManager/*01000022*/ Microsoft.AspNetCore.DataProtection.Extensions.Resources/*0200000D*/::get_ResourceManager() /* 06000020 */
  } // end of property Resources::ResourceManager
  .property /*17000002*/ callconv(8) class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000023*/
          Culture()
  {
    .get class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000023*/ Microsoft.AspNetCore.DataProtection.Extensions.Resources/*0200000D*/::get_Culture() /* 06000021 */
    .set void Microsoft.AspNetCore.DataProtection.Extensions.Resources/*0200000D*/::set_Culture(class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000023*/) /* 06000022 */
  } // end of property Resources::Culture
  .property /*17000003*/ callconv(8) string
          CryptCommon_GenericError()
  {
    .get string Microsoft.AspNetCore.DataProtection.Extensions.Resources/*0200000D*/::get_CryptCommon_GenericError() /* 06000024 */
  } // end of property Resources::CryptCommon_GenericError
  .property /*17000004*/ callconv(8) string
          TimeLimitedDataProtector_PayloadExpired()
  {
    .get string Microsoft.AspNetCore.DataProtection.Extensions.Resources/*0200000D*/::get_TimeLimitedDataProtector_PayloadExpired() /* 06000025 */
  } // end of property Resources::TimeLimitedDataProtector_PayloadExpired
  .property /*17000005*/ callconv(8) string
          TimeLimitedDataProtector_PayloadInvalid()
  {
    .get string Microsoft.AspNetCore.DataProtection.Extensions.Resources/*0200000D*/::get_TimeLimitedDataProtector_PayloadInvalid() /* 06000027 */
  } // end of property Resources::TimeLimitedDataProtector_PayloadInvalid
} // end of class Microsoft.AspNetCore.DataProtection.Extensions.Resources


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
