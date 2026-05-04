


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
// Number of sections:         0x0003
// Time-date stamp:            0xfaf3b4d5
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00002a00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x000048ca
// Base of code:                   0x00002000
// Base of data:                   0x00006000
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
// Size of headers:                0x00000200
// Checksum:                       0x00014dda
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00004878 [0x0000004f] address [size] of Import Directory:          
// 0x00006000 [0x00000484] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00003400 [0x00002910] address [size] of Security Directory:        
// 0x00008000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000485c [0x0000001c] address [size] of Debug Directory:           
// 0x00000000 [0x00000000] address [size] of Architecture Specific:     
// 0x00000000 [0x00000000] address [size] of Global Pointer:            
// 0x00000000 [0x00000000] address [size] of TLS Directory:             
// 0x00000000 [0x00000000] address [size] of Load Config Directory:     
// 0x00000000 [0x00000000] address [size] of Bound Import Directory:    
// 0x00002000 [0x00000008] address [size] of Import Address Table:      
// 0x00000000 [0x00000000] address [size] of Delay Load IAT:            
// 0x00002008 [0x00000048] address [size] of CLR Header:                


// Image sections:
//              .text
//              0x000028d0 Virtual Size
//              0x00002000 Virtual Address
//              0x00002a00 Size of Raw Data
//              0x00000200 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x60000020 Characteristics
//                         CNT_CODE
//                         MEM_EXECUTE
//                         MEM_READ

//              .rsrc
//              0x00000484 Virtual Size
//              0x00006000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00002c00 Pointer to Raw Data
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
//              0x00003200 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00004000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x000008cc
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x000048ba Import Name Table
//              0          Time Date Stamp
//              0          Index of First Forwarder Reference
//
//              0x0000  _CorDllMain

// Delay Load Import Address Table
// No data.

// Entry point code:
//FF 25 00 20 00 10 


// ----- CLR Header:
// Header size:                        0x00000048
// Major runtime version:              0x0002
// Minor runtime version:              0x0005
// 0x00002120 [0x000026bc] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x000047dc [0x00000080] address [size] of Strong Name Signature:     
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
//              0x0000184c Size
//              '#~' Name
//    Stream 2:
//              0x000018b8 Offset
//              0x000006c8 Size
//              '#Strings' Name
//    Stream 3:
//              0x00001f80 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00001f84 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001f94 Offset
//              0x00000728 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000040901a01d47 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 23824
// PE header size       : 512 (496 used)    ( 2.15%)
// PE additional info   : 11867             (49.81%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.30%)
// CLR meta-data size  : 9916               (41.62%)
// CLR additional info : 128                ( 0.54%)
// CLR method headers  : 68                 ( 0.29%)
// Managed code         : 140               ( 0.59%)
// Data                 : 2048              ( 8.60%)
// Unaccounted          : -927              (-3.89%)

// Num.of PE sections   : 3
//   .text    - 10752
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 9916
//   Module        -    1 (10 bytes)
//   TypeDef       -    4 (56 bytes)      0 interfaces, 0 explicit layout
//   TypeRef       -   41 (246 bytes)
//   MethodDef     -   68 (952 bytes)     0 abstract, 0 native, 68 bodies
//   MemberRef     -   25 (150 bytes)
//   ParamDef      -  297 (1782 bytes)
//   Constant      -   63 (378 bytes)
//   CustomAttribute-  347 (2082 bytes)
//   PropertyMap   -    1 (4 bytes)
//   Property      -    2 (12 bytes)
//   MethodSemantic-    2 (12 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    4 (80 bytes)
//   GenericParam  -   44 (352 bytes)
//   Strings       -  1733 bytes
//   Blobs         -  1832 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   193 bytes

// CLR additional info : 128

// CLR method headers : 68
//   Num.of method bodies  - 68
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 68

// Managed code : 140
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class HttpClientJsonExtensions       (public) (abstract) (auto) (ansi) (sealed) 
// Class HttpContentJsonExtensions      (public) (abstract) (auto) (ansi) (sealed) 
// Class JsonContent                    (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly extern /*23000002*/ System.Net.Http
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly extern /*23000003*/ System.Text.Json
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 8:0:0:0
}
.assembly extern /*23000004*/ System.Net.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly /*20000001*/ noplatform System.Net.Http.Json
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000007:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000008:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000002*/::.ctor(int32) /* 0A000002 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000009:0A000003*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000003*/::.ctor() /* 0A000003 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C00000A:0A000004*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*//DebuggingModes/*01000005*/) /* 0A000004 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C00000B:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000006*/::.ctor(string) /* 0A000005 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 38 2E 30 01 00 54 0E 14   // ersion=v8.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 38 2E 30 )          // Name..NET 8.0
  .custom /*0C00000C:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000007*/::.ctor() /* 0A000006 */ = ( 01 00 00 00 ) 
  .custom /*0C00000D:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000E:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000F:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 14 53 79 73 74 65 6D 2E 4E 65 74 2E 48 74   // ...System.Net.Ht
                                                                                                                                                                        74 70 2E 4A 73 6F 6E 00 00 )                      // tp.Json..
  .custom /*0C000010:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000011:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000012:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000013:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 14 53 79 73 74 65 6D 2E 4E 65 74 2E 48 74   // ...System.Net.Ht
                                                                                                                                                                       74 70 2E 4A 73 6F 6E 00 00 )                      // tp.Json..
  .custom /*0C000014:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 38 2E 30 2E 32 33 2E 35 33 31 30 33 00   // ...8.0.23.53103.
                                                                                                                                                                       00 ) 
  .custom /*0C000015:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 38 2E 30 2E 30 2B 35 35 33 35 65 33 31   // ...8.0.0+5535e31
                                                                                                                                                                                61 37 31 32 33 34 33 61 36 33 66 35 64 37 64 37   // a712343a63f5d7d7
                                                                                                                                                                                39 36 63 64 38 37 34 65 35 36 33 65 35 61 63 31   // 96cd874e563e5ac1
                                                                                                                                                                                34 00 00 )                                        // 4..
  .custom /*0C000016:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000017:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 14 53 79 73 74 65 6D 2E 4E 65 74 2E 48 74   // ...System.Net.Ht
                                                                                                                                                                 74 70 2E 4A 73 6F 6E 00 00 )                      // tp.Json..
  .custom /*0C000018:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    21 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // !https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 72 75 6E 74 69   // com/dotnet/runti
                                                                                                                                                                    6D 65 00 00 )                                     // me..
  .publickey = (00 24 00 00 04 80 00 00 94 00 00 00 06 02 00 00   // .$..............
                00 24 00 00 52 53 41 31 00 04 00 00 01 00 01 00   // .$..RSA1........
                4B 86 C4 CB 78 54 9B 34 BA B6 1A 3B 18 00 E2 3B   // K...xT.4...;...;
                FE B5 B3 EC 39 00 74 04 15 36 A7 E3 CB D9 7F 5F   // ....9.t..6....._
                04 CF 0F 85 71 55 A8 92 8E AA 29 EB FD 11 CF BB   // ....qU....).....
                AD 3B A7 0E FE A7 BD A3 22 6C 6A 8D 37 0A 4C D3   // .;......"lj.7.L.
                03 F7 14 48 6B 6E BC 22 59 85 A6 38 47 1E 6E F5   // ...Hkn."Y..8G.n.
                71 CC 92 A4 61 3C 00 B8 FA 65 D6 1C CE E0 CB E5   // q...a<...e......
                F3 63 30 C9 A0 1F 41 83 55 9F 1B EF 24 CC 29 17   // .c0...A.U...$.).
                C6 D9 13 E3 A5 41 33 3A 1D 05 D9 BE D2 2B 38 CB ) // .....A3:.....+8.
  .hash algorithm 0x00008004
  .ver 8:0:0:0
}
.module System.Net.Http.Json.dll
// MVID: {b08af778-8c8c-4397-a338-559ffed9db13}
.custom /*0C000005:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000012*/::.ctor(int32) /* 0A000011 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000006:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000013*/::.ctor(bool) /* 0A000012 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public abstract auto ansi sealed beforefieldinit System.Net.Http.Json.HttpClientJsonExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C00001A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00001C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<object> 
          DeleteFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                              string requestUri,
                              class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                              class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options,
                              [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 00 05 15 12 65 01 1C 12 69 0E 12 71 12 75 11 79
  {
    .custom /*0C000001:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000002:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C000003:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*08000001*/ 
    .custom /*0C000004:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000003*/ 
    .custom /*0C000025:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000026:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [4]/*08000005*/ 
    .custom /*0C00002D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000006*/  = nullref
    // Method begins at RVA 0x2050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::DeleteFromJsonAsync

  .method /*06000002*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<object> 
          DeleteFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                              string requestUri,
                              class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                              class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.JsonSerializerContext/*0100001F*/ context,
                              [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 00 05 15 12 65 01 1C 12 69 0E 12 71 12 7D 11 79
  {
    .custom /*0C000019:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000007*/ 
    .custom /*0C000036:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000009*/ 
    .custom /*0C00003D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00003E:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [5]/*0800000C*/  = nullref
    // Method begins at RVA 0x2053
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::DeleteFromJsonAsync

  .method /*06000003*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<object> 
          DeleteFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                              string requestUri,
                              class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                              [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 00 04 15 12 65 01 1C 12 69 0E 12 71 11 79
  {
    .custom /*0C00001F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000020:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C000021:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*0800000D*/ 
    .custom /*0C00004D:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*0800000F*/ 
    .custom /*0C000054:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000055:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [4]/*08000011*/  = nullref
    // Method begins at RVA 0x2056
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::DeleteFromJsonAsync

  .method /*06000004*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<object> 
          DeleteFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                              class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                              class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                              class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options,
                              [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 00 05 15 12 65 01 1C 12 69 12 80 81 12 71 12 75 11 79
  {
    .custom /*0C000027:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000028:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C000029:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*08000012*/ 
    .custom /*0C000060:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000014*/ 
    .custom /*0C000066:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000016*/ 
    .custom /*0C00006F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000017*/  = nullref
    // Method begins at RVA 0x2059
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::DeleteFromJsonAsync

  .method /*06000005*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<object> 
          DeleteFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                              class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                              class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                              class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.JsonSerializerContext/*0100001F*/ context,
                              [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 00 05 15 12 65 01 1C 12 69 12 80 81 12 71 12 7D 11 79
  {
    .custom /*0C00002C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000018*/ 
    .custom /*0C000076:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*0800001A*/ 
    .custom /*0C00007E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800001D*/  = nullref
    // Method begins at RVA 0x205c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::DeleteFromJsonAsync

  .method /*06000006*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<object> 
          DeleteFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                              class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                              class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                              [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 00 04 15 12 65 01 1C 12 69 12 80 81 12 71 11 79
  {
    .custom /*0C00002F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000030:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C000031:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*0800001E*/ 
    .custom /*0C000090:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000020*/ 
    .custom /*0C000097:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000022*/  = nullref
    // Method begins at RVA 0x205f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::DeleteFromJsonAsync

  .method /*06000007*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<!!TValue> 
          DeleteFromJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                      string requestUri,
                                      class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options,
                                      [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 65 01 1E 00 12 69 0E 12 75 11 79
  {
    .custom /*0C000032:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000033:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000034:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C000035:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*08000023*/ 
    .custom /*0C0000A2:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*08000024*/ 
    .custom /*0C0000A7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000025*/ 
    .custom /*0C0000AC:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [4]/*08000027*/  = nullref
    // Method begins at RVA 0x2062
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::DeleteFromJsonAsync

  .method /*06000008*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<!!TValue> 
          DeleteFromJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                      string requestUri,
                                      class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*01000021*/<!!TValue> jsonTypeInfo,
                                      [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 65 01 1E 00 12 69 0E 15 12 80 85 01 1E 00 11 79
  {
    .custom /*0C000037:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A000002*/ 
      .custom /*0C00001E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000028*/ 
    .custom /*0C0000B6:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*0800002A*/ 
    .custom /*0C0000BC:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000BD:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [4]/*0800002C*/  = nullref
    // Method begins at RVA 0x2065
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::DeleteFromJsonAsync

  .method /*06000009*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<!!TValue> 
          DeleteFromJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                      string requestUri,
                                      [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 03 15 12 65 01 1E 00 12 69 0E 11 79
  {
    .custom /*0C000039:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00003A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00003B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C00003C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*0800002D*/ 
    .custom /*0C0000C6:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*0800002E*/ 
    .custom /*0C0000CA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800002F*/ 
    .custom /*0C0000CC:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [3]/*08000030*/  = nullref
    // Method begins at RVA 0x2068
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::DeleteFromJsonAsync

  .method /*0600000A*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<!!TValue> 
          DeleteFromJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                      class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                                      class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options,
                                      [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 65 01 1E 00 12 69 12 80 81 12 75 11 79
  {
    .custom /*0C00003F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000040:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000041:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C000042:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*08000031*/ 
    .custom /*0C0000D4:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*08000032*/ 
    .custom /*0C0000D6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000035*/  = nullref
    // Method begins at RVA 0x206b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::DeleteFromJsonAsync

  .method /*0600000B*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<!!TValue> 
          DeleteFromJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                      class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                                      class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*01000021*/<!!TValue> jsonTypeInfo,
                                      [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 65 01 1E 00 12 69 12 80 81 15 12 80 85 01 1E 00 11 79
  {
    .custom /*0C000043:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A000005*/ 
      .custom /*0C00002E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000036*/ 
    .custom /*0C0000E1:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000038*/ 
    .custom /*0C0000E7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800003A*/  = nullref
    // Method begins at RVA 0x206e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::DeleteFromJsonAsync

  .method /*0600000C*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<!!TValue> 
          DeleteFromJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                      class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                                      [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 03 15 12 65 01 1E 00 12 69 12 80 81 11 79
  {
    .custom /*0C000045:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000046:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000047:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C000048:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*0800003B*/ 
    .custom /*0C0000EB:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*0800003C*/ 
    .custom /*0C0000ED:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [3]/*0800003E*/  = nullref
    // Method begins at RVA 0x2071
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::DeleteFromJsonAsync

  .method /*0600000D*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*01000022*/<!!TValue> 
          GetFromJsonAsAsyncEnumerable<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                               string requestUri,
                                               class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options,
                                               [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 80 89 01 1E 00 12 69 0E 12 75 11 79
  {
    .custom /*0C000049:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00004A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00004B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C00004C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*0800003F*/ 
    .custom /*0C0000F5:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*08000040*/ 
    .custom /*0C0000F6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000041*/ 
    .custom /*0C0000F7:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [4]/*08000043*/  = nullref
    // Method begins at RVA 0x2074
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsAsyncEnumerable

  .method /*0600000E*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*01000022*/<!!TValue> 
          GetFromJsonAsAsyncEnumerable<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                               string requestUri,
                                               class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*01000021*/<!!TValue> jsonTypeInfo,
                                               [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 80 89 01 1E 00 12 69 0E 15 12 80 85 01 1E 00 11 79
  {
    .custom /*0C00004E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A000008*/ 
      .custom /*0C000038:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000044*/ 
    .custom /*0C0000F8:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000046*/ 
    .custom /*0C0000F9:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000FA:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [4]/*08000048*/  = nullref
    // Method begins at RVA 0x2077
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsAsyncEnumerable

  .method /*0600000F*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*01000022*/<!!TValue> 
          GetFromJsonAsAsyncEnumerable<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                               string requestUri,
                                               [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 03 15 12 80 89 01 1E 00 12 69 0E 11 79
  {
    .custom /*0C000050:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000051:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000052:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C000053:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*08000049*/ 
    .custom /*0C0000FB:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*0800004A*/ 
    .custom /*0C0000FC:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800004B*/ 
    .custom /*0C0000FD:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [3]/*0800004C*/  = nullref
    // Method begins at RVA 0x207a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsAsyncEnumerable

  .method /*06000010*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*01000022*/<!!TValue> 
          GetFromJsonAsAsyncEnumerable<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                               class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                                               class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options,
                                               [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 80 89 01 1E 00 12 69 12 80 81 12 75 11 79
  {
    .custom /*0C000056:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000057:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000058:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C000059:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*0800004D*/ 
    .custom /*0C0000FE:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*0800004E*/ 
    .custom /*0C0000FF:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000051*/  = nullref
    // Method begins at RVA 0x207d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsAsyncEnumerable

  .method /*06000011*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*01000022*/<!!TValue> 
          GetFromJsonAsAsyncEnumerable<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                               class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                                               class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*01000021*/<!!TValue> jsonTypeInfo,
                                               [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 80 89 01 1E 00 12 69 12 80 81 15 12 80 85 01 1E 00 11 79
  {
    .custom /*0C00005A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A00000B*/ 
      .custom /*0C000044:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000052*/ 
    .custom /*0C000100:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000054*/ 
    .custom /*0C000101:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000056*/  = nullref
    // Method begins at RVA 0x2080
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsAsyncEnumerable

  .method /*06000012*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*01000022*/<!!TValue> 
          GetFromJsonAsAsyncEnumerable<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                               class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                                               [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 03 15 12 80 89 01 1E 00 12 69 12 80 81 11 79
  {
    .custom /*0C00005C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00005D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00005E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C00005F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*08000057*/ 
    .custom /*0C000102:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*08000058*/ 
    .custom /*0C000103:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [3]/*0800005A*/  = nullref
    // Method begins at RVA 0x2083
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsAsyncEnumerable

  .method /*06000013*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<object> 
          GetFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                           string requestUri,
                           class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                           class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options,
                           [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 00 05 15 12 65 01 1C 12 69 0E 12 71 12 75 11 79
  {
    .custom /*0C000061:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000062:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C000063:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*0800005B*/ 
    .custom /*0C000104:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*0800005D*/ 
    .custom /*0C000105:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000106:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [4]/*0800005F*/ 
    .custom /*0C000107:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000060*/  = nullref
    // Method begins at RVA 0x2086
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*06000014*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<object> 
          GetFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                           string requestUri,
                           class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                           class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.JsonSerializerContext/*0100001F*/ context,
                           [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 00 05 15 12 65 01 1C 12 69 0E 12 71 12 7D 11 79
  {
    .custom /*0C000065:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000061*/ 
    .custom /*0C000108:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000063*/ 
    .custom /*0C000109:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00010A:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [5]/*08000066*/  = nullref
    // Method begins at RVA 0x2089
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*06000015*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<object> 
          GetFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                           string requestUri,
                           class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                           [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 00 04 15 12 65 01 1C 12 69 0E 12 71 11 79
  {
    .custom /*0C000068:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000069:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C00006A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*08000067*/ 
    .custom /*0C00010B:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000069*/ 
    .custom /*0C00010C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00010D:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [4]/*0800006B*/  = nullref
    // Method begins at RVA 0x208c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*06000016*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<object> 
          GetFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                           class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                           class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                           class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options,
                           [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 00 05 15 12 65 01 1C 12 69 12 80 81 12 71 12 75 11 79
  {
    .custom /*0C00006C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00006D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C00006E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*0800006C*/ 
    .custom /*0C00010E:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*0800006E*/ 
    .custom /*0C00010F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000070*/ 
    .custom /*0C000110:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000071*/  = nullref
    // Method begins at RVA 0x208f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*06000017*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<object> 
          GetFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                           class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                           class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                           class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.JsonSerializerContext/*0100001F*/ context,
                           [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 00 05 15 12 65 01 1C 12 69 12 80 81 12 71 12 7D 11 79
  {
    .custom /*0C000071:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000072*/ 
    .custom /*0C000111:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000074*/ 
    .custom /*0C000112:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000077*/  = nullref
    // Method begins at RVA 0x2092
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*06000018*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<object> 
          GetFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                           class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                           class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                           [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 00 04 15 12 65 01 1C 12 69 12 80 81 12 71 11 79
  {
    .custom /*0C000073:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000074:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C000075:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*08000078*/ 
    .custom /*0C000113:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*0800007A*/ 
    .custom /*0C000114:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800007C*/  = nullref
    // Method begins at RVA 0x2095
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*06000019*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<!!TValue> 
          GetFromJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                   string requestUri,
                                   class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 65 01 1E 00 12 69 0E 12 75 11 79
  {
    .custom /*0C000078:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000079:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00007A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C00007B:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*0800007D*/ 
    .custom /*0C000115:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*0800007E*/ 
    .custom /*0C000116:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800007F*/ 
    .custom /*0C000117:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [4]/*08000081*/  = nullref
    // Method begins at RVA 0x2098
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*0600001A*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<!!TValue> 
          GetFromJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                   string requestUri,
                                   class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*01000021*/<!!TValue> jsonTypeInfo,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 65 01 1E 00 12 69 0E 15 12 80 85 01 1E 00 11 79
  {
    .custom /*0C00007D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A00000E*/ 
      .custom /*0C00004F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000082*/ 
    .custom /*0C000118:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000084*/ 
    .custom /*0C000119:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00011A:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [4]/*08000086*/  = nullref
    // Method begins at RVA 0x209b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*0600001B*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<!!TValue> 
          GetFromJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                   string requestUri,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 03 15 12 65 01 1E 00 12 69 0E 11 79
  {
    .custom /*0C000080:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000081:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000082:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C000083:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*08000087*/ 
    .custom /*0C00011B:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*08000088*/ 
    .custom /*0C00011C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000089*/ 
    .custom /*0C00011D:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [3]/*0800008A*/  = nullref
    // Method begins at RVA 0x209e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*0600001C*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<!!TValue> 
          GetFromJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                   class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                                   class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 65 01 1E 00 12 69 12 80 81 12 75 11 79
  {
    .custom /*0C000085:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000086:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000087:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C000088:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*0800008B*/ 
    .custom /*0C00011E:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*0800008C*/ 
    .custom /*0C00011F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [4]/*0800008F*/  = nullref
    // Method begins at RVA 0x20a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*0600001D*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<!!TValue> 
          GetFromJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                   class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                                   class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*01000021*/<!!TValue> jsonTypeInfo,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 65 01 1E 00 12 69 12 80 81 15 12 80 85 01 1E 00 11 79
  {
    .custom /*0C00008A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A000011*/ 
      .custom /*0C00005B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000090*/ 
    .custom /*0C000120:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000092*/ 
    .custom /*0C000121:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000094*/  = nullref
    // Method begins at RVA 0x20a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*0600001E*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<!!TValue> 
          GetFromJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                   class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 03 15 12 65 01 1E 00 12 69 12 80 81 11 79
  {
    .custom /*0C00008C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00008D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00008E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C00008F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*08000095*/ 
    .custom /*0C000122:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*08000096*/ 
    .custom /*0C000123:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000098*/  = nullref
    // Method begins at RVA 0x20a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*0600001F*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000023*/> 
          PatchAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                   string requestUri,
                                   !!TValue 'value',
                                   [opt] class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 05 15 12 65 01 12 80 8D 12 69 0E 1E 00 12 75 11 79
  {
    .custom /*0C000092:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000093:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C000094:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A000013*/ 
      .custom /*0C000064:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800009A*/ 
    .custom /*0C000124:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000125:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [4]/*0800009C*/  = nullref
    .custom /*0C000126:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800009D*/  = nullref
    // Method begins at RVA 0x20aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PatchAsJsonAsync

  .method /*06000020*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000023*/> 
          PatchAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                   string requestUri,
                                   !!TValue 'value',
                                   class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*01000021*/<!!TValue> jsonTypeInfo,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 05 15 12 65 01 12 80 8D 12 69 0E 1E 00 15 12 80 85 01 1E 00 11 79
  {
    .custom /*0C000096:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A000014*/ 
      .custom /*0C000067:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800009F*/ 
    .custom /*0C000127:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000128:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [5]/*080000A2*/  = nullref
    // Method begins at RVA 0x20ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PatchAsJsonAsync

  .method /*06000021*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000023*/> 
          PatchAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                   string requestUri,
                                   !!TValue 'value',
                                   valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 65 01 12 80 8D 12 69 0E 1E 00 11 79
  {
    .custom /*0C000099:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00009A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C00009B:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A000015*/ 
      .custom /*0C00006B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000A4*/ 
    .custom /*0C000129:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00012A:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x20b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PatchAsJsonAsync

  .method /*06000022*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000023*/> 
          PatchAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                   class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                                   !!TValue 'value',
                                   [opt] class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 05 15 12 65 01 12 80 8D 12 69 12 80 81 1E 00 12 75 11 79
  {
    .custom /*0C00009D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00009E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C00009F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A000016*/ 
      .custom /*0C000070:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000A8*/ 
    .custom /*0C00012B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080000AA*/  = nullref
    .custom /*0C00012C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080000AB*/  = nullref
    // Method begins at RVA 0x20b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PatchAsJsonAsync

  .method /*06000023*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000023*/> 
          PatchAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                   class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                                   !!TValue 'value',
                                   class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*01000021*/<!!TValue> jsonTypeInfo,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 05 15 12 65 01 12 80 8D 12 69 12 80 81 1E 00 15 12 80 85 01 1E 00 11 79
  {
    .custom /*0C0000A1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A000017*/ 
      .custom /*0C000072:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000AD*/ 
    .custom /*0C00012D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080000B0*/  = nullref
    // Method begins at RVA 0x20b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PatchAsJsonAsync

  .method /*06000024*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000023*/> 
          PatchAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                   class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                                   !!TValue 'value',
                                   valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 65 01 12 80 8D 12 69 12 80 81 1E 00 11 79
  {
    .custom /*0C0000A4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000A5:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C0000A6:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A000018*/ 
      .custom /*0C000077:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000B2*/ 
    .custom /*0C00012E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PatchAsJsonAsync

  .method /*06000025*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000023*/> 
          PostAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                  string requestUri,
                                  !!TValue 'value',
                                  [opt] class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options,
                                  [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 05 15 12 65 01 12 80 8D 12 69 0E 1E 00 12 75 11 79
  {
    .custom /*0C0000A9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000AA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C0000AB:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A000019*/ 
      .custom /*0C00007C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000B6*/ 
    .custom /*0C00012F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000130:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [4]/*080000B8*/  = nullref
    .custom /*0C000131:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080000B9*/  = nullref
    // Method begins at RVA 0x20bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PostAsJsonAsync

  .method /*06000026*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000023*/> 
          PostAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                  string requestUri,
                                  !!TValue 'value',
                                  class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*01000021*/<!!TValue> jsonTypeInfo,
                                  [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 05 15 12 65 01 12 80 8D 12 69 0E 1E 00 15 12 80 85 01 1E 00 11 79
  {
    .custom /*0C0000AD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A00001A*/ 
      .custom /*0C00007F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000BB*/ 
    .custom /*0C000132:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000133:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [5]/*080000BE*/  = nullref
    // Method begins at RVA 0x20bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PostAsJsonAsync

  .method /*06000027*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000023*/> 
          PostAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                  string requestUri,
                                  !!TValue 'value',
                                  valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 65 01 12 80 8D 12 69 0E 1E 00 11 79
  {
    .custom /*0C0000AF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000B0:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C0000B1:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A00001B*/ 
      .custom /*0C000084:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000C0*/ 
    .custom /*0C000134:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000135:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x20c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PostAsJsonAsync

  .method /*06000028*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000023*/> 
          PostAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                  class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                                  !!TValue 'value',
                                  [opt] class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options,
                                  [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 05 15 12 65 01 12 80 8D 12 69 12 80 81 1E 00 12 75 11 79
  {
    .custom /*0C0000B3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000B4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C0000B5:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A00001C*/ 
      .custom /*0C000089:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000C4*/ 
    .custom /*0C000136:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080000C6*/  = nullref
    .custom /*0C000137:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080000C7*/  = nullref
    // Method begins at RVA 0x20c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PostAsJsonAsync

  .method /*06000029*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000023*/> 
          PostAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                  class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                                  !!TValue 'value',
                                  class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*01000021*/<!!TValue> jsonTypeInfo,
                                  [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 05 15 12 65 01 12 80 8D 12 69 12 80 81 1E 00 15 12 80 85 01 1E 00 11 79
  {
    .custom /*0C0000B7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A00001D*/ 
      .custom /*0C00008B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000C9*/ 
    .custom /*0C000138:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080000CC*/  = nullref
    // Method begins at RVA 0x20c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PostAsJsonAsync

  .method /*0600002A*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000023*/> 
          PostAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                  class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                                  !!TValue 'value',
                                  valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 65 01 12 80 8D 12 69 12 80 81 1E 00 11 79
  {
    .custom /*0C0000B9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000BA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C0000BB:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A00001E*/ 
      .custom /*0C000091:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000CE*/ 
    .custom /*0C000139:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PostAsJsonAsync

  .method /*0600002B*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000023*/> 
          PutAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                 string requestUri,
                                 !!TValue 'value',
                                 [opt] class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options,
                                 [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 05 15 12 65 01 12 80 8D 12 69 0E 1E 00 12 75 11 79
  {
    .custom /*0C0000BF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000C0:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C0000C1:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A00001F*/ 
      .custom /*0C000095:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000D2*/ 
    .custom /*0C00013A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00013B:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [4]/*080000D4*/  = nullref
    .custom /*0C00013C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080000D5*/  = nullref
    // Method begins at RVA 0x20ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PutAsJsonAsync

  .method /*0600002C*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000023*/> 
          PutAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                 string requestUri,
                                 !!TValue 'value',
                                 class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*01000021*/<!!TValue> jsonTypeInfo,
                                 [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 05 15 12 65 01 12 80 8D 12 69 0E 1E 00 15 12 80 85 01 1E 00 11 79
  {
    .custom /*0C0000C2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A000020*/ 
      .custom /*0C000098:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000D7*/ 
    .custom /*0C00013D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00013E:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    .param [5]/*080000DA*/  = nullref
    // Method begins at RVA 0x20d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PutAsJsonAsync

  .method /*0600002D*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000023*/> 
          PutAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                 string requestUri,
                                 !!TValue 'value',
                                 valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 65 01 12 80 8D 12 69 0E 1E 00 11 79
  {
    .custom /*0C0000C3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000C4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C0000C5:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A000021*/ 
      .custom /*0C00009C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000DC*/ 
    .custom /*0C00013F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000140:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100001B*/::.ctor(string) /* 0A000018 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x20d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PutAsJsonAsync

  .method /*0600002E*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000023*/> 
          PutAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                 class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                                 !!TValue 'value',
                                 [opt] class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options,
                                 [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 05 15 12 65 01 12 80 8D 12 69 12 80 81 1E 00 12 75 11 79
  {
    .custom /*0C0000C7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000C8:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C0000C9:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A000022*/ 
      .custom /*0C0000A0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000E0*/ 
    .custom /*0C000141:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080000E2*/  = nullref
    .custom /*0C000142:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080000E3*/  = nullref
    // Method begins at RVA 0x20d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PutAsJsonAsync

  .method /*0600002F*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000023*/> 
          PutAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                 class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                                 !!TValue 'value',
                                 class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*01000021*/<!!TValue> jsonTypeInfo,
                                 [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 05 15 12 65 01 12 80 8D 12 69 12 80 81 1E 00 15 12 80 85 01 1E 00 11 79
  {
    .custom /*0C0000CB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A000023*/ 
      .custom /*0C0000A3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000E5*/ 
    .custom /*0C000143:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080000E8*/  = nullref
    // Method begins at RVA 0x20da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PutAsJsonAsync

  .method /*06000030*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000023*/> 
          PutAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*0100001A*/ client,
                                 class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri,
                                 !!TValue 'value',
                                 valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 65 01 12 80 8D 12 69 12 80 81 1E 00 11 79
  {
    .custom /*0C0000CD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000CE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C0000CF:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A000024*/ 
      .custom /*0C0000A8:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000EA*/ 
    .custom /*0C000144:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PutAsJsonAsync

} // end of class System.Net.Http.Json.HttpClientJsonExtensions

.class /*02000003*/ public abstract auto ansi sealed beforefieldinit System.Net.Http.Json.HttpContentJsonExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000022:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000023:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000024:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000031*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*01000022*/<!!TValue> 
          ReadFromJsonAsAsyncEnumerable<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpContent/*01000024*/ content,
                                                class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options,
                                                [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 03 15 12 80 89 01 1E 00 12 80 91 12 75 11 79
  {
    .custom /*0C0000D0:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000D1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000D2:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C0000D3:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*080000ED*/ 
    .custom /*0C000145:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*080000EE*/ 
    .custom /*0C000146:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [3]/*080000F0*/  = nullref
    // Method begins at RVA 0x20e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentJsonExtensions::ReadFromJsonAsAsyncEnumerable

  .method /*06000032*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*01000022*/<!!TValue> 
          ReadFromJsonAsAsyncEnumerable<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpContent/*01000024*/ content,
                                                class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*01000021*/<!!TValue> jsonTypeInfo,
                                                [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 03 15 12 80 89 01 1E 00 12 80 91 15 12 80 85 01 1E 00 11 79
  {
    .custom /*0C0000D5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A000026*/ 
      .custom /*0C0000AE:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080000F1*/ 
    .custom /*0C000147:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [3]/*080000F4*/  = nullref
    // Method begins at RVA 0x20e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentJsonExtensions::ReadFromJsonAsAsyncEnumerable

  .method /*06000033*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*01000022*/<!!TValue> 
          ReadFromJsonAsAsyncEnumerable<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpContent/*01000024*/ content,
                                                [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 02 15 12 80 89 01 1E 00 12 80 91 11 79
  {
    .custom /*0C0000D7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000D8:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C0000D9:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A000027*/ 
      .custom /*0C0000B2:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080000F5*/ 
    .custom /*0C000148:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*080000F7*/  = nullref
    // Method begins at RVA 0x20e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentJsonExtensions::ReadFromJsonAsAsyncEnumerable

  .method /*06000034*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<object> 
          ReadFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpContent/*01000024*/ content,
                            class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                            class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options,
                            [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 00 04 15 12 65 01 1C 12 80 91 12 71 12 75 11 79
  {
    .custom /*0C0000DA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000DB:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C0000DC:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*080000F8*/ 
    .custom /*0C000149:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [3]/*080000FB*/ 
    .custom /*0C00014A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080000FC*/  = nullref
    // Method begins at RVA 0x20e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentJsonExtensions::ReadFromJsonAsync

  .method /*06000035*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<object> 
          ReadFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpContent/*01000024*/ content,
                            class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                            class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.JsonSerializerContext/*0100001F*/ context,
                            [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 00 04 15 12 65 01 1C 12 80 91 12 71 12 7D 11 79
  {
    .custom /*0C0000DD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*080000FD*/ 
    .custom /*0C00014B:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [4]/*08000101*/  = nullref
    // Method begins at RVA 0x20ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentJsonExtensions::ReadFromJsonAsync

  .method /*06000036*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<object> 
          ReadFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpContent/*01000024*/ content,
                            class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                            [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 00 03 15 12 65 01 1C 12 80 91 12 71 11 79
  {
    .custom /*0C0000DE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000DF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C0000E0:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*08000102*/ 
    .custom /*0C00014C:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [3]/*08000105*/  = nullref
    // Method begins at RVA 0x20ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentJsonExtensions::ReadFromJsonAsync

  .method /*06000037*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<!!T> 
          ReadFromJsonAsync<T>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpContent/*01000024*/ content,
                               class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options,
                               [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 03 15 12 65 01 1E 00 12 80 91 12 75 11 79
  {
    .custom /*0C0000E2:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000E3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000E4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C0000E5:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*08000106*/ 
    .custom /*0C00014D:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*08000107*/ 
    .custom /*0C00014E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000109*/  = nullref
    // Method begins at RVA 0x20f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentJsonExtensions::ReadFromJsonAsync

  .method /*06000038*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<!!T> 
          ReadFromJsonAsync<T>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpContent/*01000024*/ content,
                               class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*01000021*/<!!T> jsonTypeInfo,
                               [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 03 15 12 65 01 1E 00 12 80 91 15 12 80 85 01 1E 00 11 79
  {
    .custom /*0C0000E6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000029*/ 
      .custom /*0C0000B8:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800010A*/ 
    .custom /*0C00014F:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [3]/*0800010D*/  = nullref
    // Method begins at RVA 0x20f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentJsonExtensions::ReadFromJsonAsync

  .method /*06000039*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000019*/<!!T> 
          ReadFromJsonAsync<T>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpContent/*01000024*/ content,
                               [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 10 01 02 15 12 65 01 1E 00 12 80 91 11 79
  {
    .custom /*0C0000E8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000E9:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .custom /*0C0000EA:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type T /*2A00002A*/ 
      .custom /*0C0000BE:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800010E*/ 
    .custom /*0C000150:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000017 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000110*/  = nullref
    // Method begins at RVA 0x20f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentJsonExtensions::ReadFromJsonAsync

} // end of class System.Net.Http.Json.HttpContentJsonExtensions

.class /*02000004*/ public auto ansi sealed beforefieldinit System.Net.Http.Json.JsonContent
       extends [System.Net.Http/*23000002*/]System.Net.Http.HttpContent/*01000024*/
{
  .custom /*0C00002A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*0600003A*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20fb
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Net.Http/*23000002*/]System.Net.Http.HttpContent/*01000024*/::.ctor() /* 0A000019 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method JsonContent::.ctor

  .method /*0600003B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_ObjectType() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x2103
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonContent::get_ObjectType

  .method /*0600003C*/ public hidebysig specialname 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C0000EC:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2106
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonContent::get_Value

  .method /*0600003D*/ public hidebysig static 
          class System.Net.Http.Json.JsonContent/*02000004*/ 
          Create(object inputValue,
                 class [System.Runtime/*23000001*/]System.Type/*0100001C*/ inputType,
                 [opt] class [System.Net.Http/*23000002*/]System.Net.Http.Headers.MediaTypeHeaderValue/*01000025*/ mediaType,
                 [opt] class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options) cil managed
  // SIG: 00 04 12 10 1C 12 71 12 80 95 12 75
  {
    .custom /*0C0000EE:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000EF:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .custom /*0C0000F0:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .param [0]/*08000111*/ 
    .custom /*0C000151:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000113*/ 
    .custom /*0C000152:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000114*/  = nullref
    .param [4]/*08000115*/  = nullref
    // Method begins at RVA 0x2109
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonContent::Create

  .method /*0600003E*/ public hidebysig static 
          class System.Net.Http.Json.JsonContent/*02000004*/ 
          Create<T>(!!T inputValue,
                    [opt] class [System.Net.Http/*23000002*/]System.Net.Http.Headers.MediaTypeHeaderValue/*01000025*/ mediaType,
                    [opt] class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001D*/ options) cil managed
  // SIG: 10 01 03 12 10 1E 00 12 80 95 12 75
  {
    .custom /*0C0000F1:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000F2:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .custom /*0C0000F3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000017*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 A3 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                       7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                       69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                       20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                       68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                       61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                       64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                       6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                       20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                       20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                       6F 6E 74 65 78 74 2E 00 00 )                      // ontext...
    .param [0]/*08000116*/ 
    .custom /*0C000153:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000117*/ 
    .custom /*0C000154:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000118*/  = nullref
    .param [3]/*08000119*/  = nullref
    // Method begins at RVA 0x210c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonContent::Create

  .method /*0600003F*/ public hidebysig static 
          class System.Net.Http.Json.JsonContent/*02000004*/ 
          Create<T>(!!T inputValue,
                    class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*01000021*/<!!T> jsonTypeInfo,
                    [opt] class [System.Net.Http/*23000002*/]System.Net.Http.Headers.MediaTypeHeaderValue/*01000025*/ mediaType) cil managed
  // SIG: 10 01 03 12 10 1E 00 15 12 80 85 01 1E 00 12 80 95
  {
    .custom /*0C0000F4:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800011A*/ 
    .custom /*0C000155:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800011C*/ 
    .custom /*0C000156:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [3]/*0800011D*/  = nullref
    // Method begins at RVA 0x210f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonContent::Create

  .method /*06000040*/ public hidebysig static 
          class System.Net.Http.Json.JsonContent/*02000004*/ 
          Create(object inputValue,
                 class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo/*01000026*/ jsonTypeInfo,
                 [opt] class [System.Net.Http/*23000002*/]System.Net.Http.Headers.MediaTypeHeaderValue/*01000025*/ mediaType) cil managed
  // SIG: 00 03 12 10 1C 12 80 99 12 80 95
  {
    .param [1]/*0800011E*/ 
    .custom /*0C000157:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000120*/  = nullref
    .custom /*0C000158:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2112
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonContent::Create

  .method /*06000041*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/ 
          SerializeToStreamAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000028*/ 'stream',
                                 class [System.Net.Primitives/*23000004*/]System.Net.TransportContext/*01000029*/ context) cil managed
  // SIG: 20 02 12 80 9D 12 80 A1 12 80 A5
  {
    .param [2]/*08000122*/ 
    .custom /*0C000159:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2115
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonContent::SerializeToStreamAsync

  .method /*06000042*/ family hidebysig virtual 
          instance bool  TryComputeLength([out] int64& length) cil managed
  // SIG: 20 01 02 10 0A
  {
    // Method begins at RVA 0x2118
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonContent::TryComputeLength

  .method /*06000043*/ family hidebysig virtual 
          instance void  SerializeToStream(class [System.Runtime/*23000001*/]System.IO.Stream/*01000028*/ 'stream',
                                           class [System.Net.Primitives/*23000004*/]System.Net.TransportContext/*01000029*/ context,
                                           valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 20 03 01 12 80 A1 12 80 A5 11 79
  {
    .param [2]/*08000125*/ 
    .custom /*0C00015A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method JsonContent::SerializeToStream

  .method /*06000044*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000027*/ 
          SerializeToStreamAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000028*/ 'stream',
                                 class [System.Net.Primitives/*23000004*/]System.Net.TransportContext/*01000029*/ context,
                                 valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001E*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 9D 12 80 A1 12 80 A5 11 79
  {
    .param [2]/*08000128*/ 
    .custom /*0C00015B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonContent::SerializeToStreamAsync

  .property /*17000001*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          ObjectType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Net.Http.Json.JsonContent/*02000004*/::get_ObjectType() /* 0600003B */
  } // end of property JsonContent::ObjectType
  .property /*17000002*/ instance callconv(8) object
          Value()
  {
    .custom /*0C00001D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance object System.Net.Http.Json.JsonContent/*02000004*/::get_Value() /* 0600003C */
  } // end of property JsonContent::Value
} // end of class System.Net.Http.Json.JsonContent


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
