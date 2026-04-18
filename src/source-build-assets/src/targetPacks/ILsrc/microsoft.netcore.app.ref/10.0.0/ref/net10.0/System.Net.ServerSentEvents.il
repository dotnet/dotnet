


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
// Time-date stamp:            0xe3bf6770
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00001200
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000317a
// Base of code:                   0x00002000
// Base of data:                   0x00004000
// Image base:                     0x10000000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0004
// Minor subsystem version:        0x0000
// Size of image:                  0x00008000
// Size of headers:                0x00000200
// Checksum:                       0x00004a30
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00003128 [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x000004b8] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00001c00 [0x00002910] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000310c [0x0000001c] address [size] of Debug Directory:           
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
//              0x00001180 Virtual Size
//              0x00002000 Virtual Address
//              0x00001200 Size of Raw Data
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
//              0x000004b8 Virtual Size
//              0x00004000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00001400 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x00006000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x00001a00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00003000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x0000017c
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000316a Import Name Table
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
// 0x00002084 [0x00001008] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x0000308c [0x00000080] address [size] of Strong Name Signature:     
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
//              0x000005e8 Size
//              '#~' Name
//    Stream 2:
//              0x00000654 Offset
//              0x000005b0 Size
//              '#Strings' Name
//    Stream 3:
//              0x00000c04 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00000c08 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00000c18 Offset
//              0x000003f0 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000040901a01d57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 17680
// PE header size       : 512 (496 used)    ( 2.90%)
// PE additional info   : 11919             (67.42%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.41%)
// CLR meta-data size  : 4104               (23.21%)
// CLR additional info : 128                ( 0.72%)
// CLR method headers  : 16                 ( 0.09%)
// Managed code         : 35                ( 0.20%)
// Data                 : 2048              (11.58%)
// Unaccounted          : -1154             (-6.53%)

// Num.of PE sections   : 3
//   .text    - 4608
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 4104
//   Module        -    1 (10 bytes)
//   TypeDef       -    6 (84 bytes)      0 interfaces, 0 explicit layout
//   TypeRef       -   37 (222 bytes)
//   MethodDef     -   20 (280 bytes)     0 abstract, 0 native, 16 bodies
//   FieldDef      -    4 (24 bytes)      0 constant
//   MemberRef     -   22 (132 bytes)
//   ParamDef      -   27 (162 bytes)
//   Constant      -    5 (30 bytes)
//   CustomAttribute-   48 (288 bytes)
//   PropertyMap   -    2 (8 bytes)
//   Property      -    6 (36 bytes)
//   MethodSemantic-    8 (48 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    2 (40 bytes)
//   GenericParam  -    5 (40 bytes)
//   Strings       -  1456 bytes
//   Blobs         -  1008 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   194 bytes

// CLR additional info : 128

// CLR method headers : 16
//   Num.of method bodies  - 16
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 16

// Managed code : 35
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class SseFormatter                   (public) (abstract) (auto) (ansi) (sealed) 
// Class SseItemParser`1                (public) (auto) (ansi) (sealed) 
// Class SseItem`1                      (public) (sequential) (ansi) (sealed) 
// Class SseParser                      (public) (abstract) (auto) (ansi) (sealed) 
// Class SseParser`1                    (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly extern /*23000002*/ System.Memory
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 10:0:0:0
}
.assembly /*20000001*/ noplatform System.Net.ServerSentEvents
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000004:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000001*/::.ctor(int32) /* 0A000001 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000005:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000002*/::.ctor() /* 0A000002 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000006:0A000003*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*//DebuggingModes/*01000004*/) /* 0A000003 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000007:0A000004*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 19 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 31 30 2E 30 01 00 54 0E   // ersion=v10.0..T.
                                                                                                                                                                           14 46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61   // .FrameworkDispla
                                                                                                                                                                           79 4E 61 6D 65 09 2E 4E 45 54 20 31 30 2E 30 )    // yName..NET 10.0
  .custom /*0C000008:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 1B 53 79 73 74 65 6D 2E 4E 65 74 2E 53 65   // ...System.Net.Se
                                                                                                                                                                        72 76 65 72 53 65 6E 74 45 76 65 6E 74 73 00 00 ) // rverSentEvents..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000D:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000E:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0F 49 73 41 6F 74 43 6F 6D 70 61 74 69 62   // ...IsAotCompatib
                                                                                                                                                                    6C 65 04 54 72 75 65 00 00 )                      // le.True..
  .custom /*0C00000F:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000010:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000011:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 1B 53 79 73 74 65 6D 2E 4E 65 74 2E 53 65   // ...System.Net.Se
                                                                                                                                                                       72 76 65 72 53 65 6E 74 45 76 65 6E 74 73 00 00 ) // rverSentEvents..
  .custom /*0C000012:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0D 31 30 2E 30 2E 32 35 2E 35 32 34 31 31   // ...10.0.25.52411
                                                                                                                                                                       00 00 ) 
  .custom /*0C000013:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2F 31 30 2E 30 2E 30 2B 62 30 66 33 34 64   // ../10.0.0+b0f34d
                                                                                                                                                                                35 31 66 63 63 63 36 39 66 64 33 33 34 32 35 33   // 51fccc69fd334253
                                                                                                                                                                                39 32 34 61 62 64 38 64 36 38 35 33 66 61 64 37   // 924abd8d6853fad7
                                                                                                                                                                                61 61 00 00 )                                     // aa..
  .custom /*0C000014:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000015:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 1B 53 79 73 74 65 6D 2E 4E 65 74 2E 53 65   // ...System.Net.Se
                                                                                                                                                                 72 76 65 72 53 65 6E 74 45 76 65 6E 74 73 00 00 ) // rverSentEvents..
  .custom /*0C000016:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    20 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   //  https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 64 6F 74 6E 65   // com/dotnet/dotne
                                                                                                                                                                    74 00 00 )                                        // t..
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
  .ver 10:0:0:0
}
.module System.Net.ServerSentEvents.dll
// MVID: {c3981311-4868-4da9-9dd4-9d1c36f0f5e5}
.custom /*0C000002:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000011*/::.ctor(int32) /* 0A000010 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000003:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000012*/::.ctor(bool) /* 0A000011 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public abstract auto ansi sealed beforefieldinit System.Net.ServerSentEvents.SseFormatter
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000018:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000019:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000001*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000016*/ 
          WriteAsync(class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*01000017*/<valuetype System.Net.ServerSentEvents.SseItem`1/*02000004*/<string>> source,
                     class [System.Runtime/*23000001*/]System.IO.Stream/*01000018*/ destination,
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000019*/ cancellationToken) cil managed
  // SIG: 00 03 12 59 15 12 5D 01 15 11 10 01 0E 12 61 11 65
  {
    .param [1]/*08000001*/ 
    .custom /*0C000001:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 03 00 00 00 01 00 01 00 00 ) 
    .param [3]/*08000003*/  = nullref
    // Method begins at RVA 0x2050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SseFormatter::WriteAsync

  .method /*06000002*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000016*/ 
          WriteAsync<T>(class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*01000017*/<valuetype System.Net.ServerSentEvents.SseItem`1/*02000004*/<!!T>> source,
                        class [System.Runtime/*23000001*/]System.IO.Stream/*01000018*/ destination,
                        class [System.Runtime/*23000001*/]System.Action`2/*0100001A*/<valuetype System.Net.ServerSentEvents.SseItem`1/*02000004*/<!!T>,class [System.Memory/*23000002*/]System.Buffers.IBufferWriter`1/*0100001B*/<uint8>> itemFormatter,
                        [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000019*/ cancellationToken) cil managed
  // SIG: 10 01 04 12 59 15 12 5D 01 15 11 10 01 1E 00 12 61 15 12 69 02 15 11 10 01 1E 00 15 12 6D 01 05
  //      11 65
  {
    .param type T /*2A000001*/ 
      .custom /*0C000017:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000004*/ 
    .custom /*0C000021:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 03 00 00 00 01 00 01 00 00 ) 
    .param [3]/*08000006*/ 
    .custom /*0C000028:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 04 00 00 00 01 00 01 01 00 00 ) 
    .param [4]/*08000007*/  = nullref
    // Method begins at RVA 0x2053
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SseFormatter::WriteAsync

} // end of class System.Net.ServerSentEvents.SseFormatter

.class /*02000003*/ public auto ansi sealed System.Net.ServerSentEvents.SseItemParser`1<+ T>
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*0100001C*/
{
  .param type T /*2A000002*/ 
    .custom /*0C00001B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .method /*06000003*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method SseItemParser`1::.ctor

  .method /*06000004*/ public hidebysig newslot virtual 
          instance !T  Invoke(string eventType,
                              valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001D*/<uint8> data) runtime managed
  // SIG: 20 02 13 00 0E 15 11 75 01 05
  {
    .custom /*0C00001D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800000B*/ 
    .custom /*0C00002B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  } // end of method SseItemParser`1::Invoke

  .method /*06000005*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001E*/ 
          BeginInvoke(string eventType,
                      valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001D*/<uint8> data,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 79 0E 15 11 75 01 05 12 7D 1C
  {
    .param [1]/*0800000C*/ 
    .custom /*0C00002C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method SseItemParser`1::BeginInvoke

  .method /*06000006*/ public hidebysig newslot virtual 
          instance !T  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001E*/ result) runtime managed
  // SIG: 20 01 13 00 12 79
  {
    .param [0]/*08000010*/ 
    .custom /*0C00002D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method SseItemParser`1::EndInvoke

} // end of class System.Net.ServerSentEvents.SseItemParser`1

.class /*02000004*/ public sequential ansi sealed beforefieldinit System.Net.ServerSentEvents.SseItem`1<T>
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
{
  .custom /*0C00001E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000020:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000020*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .param type T /*2A000003*/ 
    .custom /*0C00001C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .field /*04000001*/ private initonly !T _Data_k__BackingField
  .field /*04000002*/ private initonly object _dummy
  .field /*04000003*/ private initonly int32 _dummyPrimitive
  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T data,
                               [opt] string eventType) cil managed
  // SIG: 20 02 01 13 00 0E
  {
    .param [2]/*08000013*/  = nullref
    .custom /*0C00002E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2056
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SseItem`1::.ctor

  .method /*06000008*/ public hidebysig specialname 
          instance !T  get_Data() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x2059
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SseItem`1::get_Data

  .method /*06000009*/ public hidebysig specialname 
          instance string  get_EventId() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000029:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x205c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SseItem`1::get_EventId

  .method /*0600000A*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000022*/) 
          set_EventId(string 'value') cil managed
  // SIG: 20 01 1F 80 89 01 0E
  {
    .custom /*0C00002A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x205f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SseItem`1::set_EventId

  .method /*0600000B*/ public hidebysig specialname 
          instance string  get_EventType() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2061
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SseItem`1::get_EventType

  .method /*0600000C*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000024*/> 
          get_ReconnectionInterval() cil managed
  // SIG: 20 00 15 11 80 8D 01 11 80 91
  {
    // Method begins at RVA 0x2064
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SseItem`1::get_ReconnectionInterval

  .method /*0600000D*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000022*/) 
          set_ReconnectionInterval(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000024*/> 'value') cil managed
  // SIG: 20 01 1F 80 89 01 15 11 80 8D 01 11 80 91
  {
    // Method begins at RVA 0x2067
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SseItem`1::set_ReconnectionInterval

  .property /*17000001*/ instance callconv(8) !T
          Data()
  {
    .get instance !T System.Net.ServerSentEvents.SseItem`1/*02000004*/::get_Data() /* 06000008 */
  } // end of property SseItem`1::Data
  .property /*17000002*/ instance callconv(8) string
          EventId()
  {
    .custom /*0C00001A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.ServerSentEvents.SseItem`1/*02000004*/::get_EventId() /* 06000009 */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000022*/) System.Net.ServerSentEvents.SseItem`1/*02000004*/::set_EventId(string) /* 0600000A */
  } // end of property SseItem`1::EventId
  .property /*17000003*/ instance callconv(8) string
          EventType()
  {
    .get instance string System.Net.ServerSentEvents.SseItem`1/*02000004*/::get_EventType() /* 0600000B */
  } // end of property SseItem`1::EventType
  .property /*17000004*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000024*/>
          ReconnectionInterval()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000024*/> System.Net.ServerSentEvents.SseItem`1/*02000004*/::get_ReconnectionInterval() /* 0600000C */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000022*/) System.Net.ServerSentEvents.SseItem`1/*02000004*/::set_ReconnectionInterval(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000024*/>) /* 0600000D */
  } // end of property SseItem`1::ReconnectionInterval
} // end of class System.Net.ServerSentEvents.SseItem`1

.class /*02000005*/ public abstract auto ansi sealed beforefieldinit System.Net.ServerSentEvents.SseParser
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000023:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000024:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .field /*04000004*/ public static literal string EventTypeDefault = "message"
  .method /*0600000E*/ public hidebysig static 
          class System.Net.ServerSentEvents.SseParser`1/*02000006*/<string> 
          Create(class [System.Runtime/*23000001*/]System.IO.Stream/*01000018*/ sseStream) cil managed
  // SIG: 00 01 15 12 18 01 0E 12 61
  {
    // Method begins at RVA 0x2069
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SseParser::Create

  .method /*0600000F*/ public hidebysig static 
          class System.Net.ServerSentEvents.SseParser`1/*02000006*/<!!T> 
          Create<T>(class [System.Runtime/*23000001*/]System.IO.Stream/*01000018*/ sseStream,
                    class System.Net.ServerSentEvents.SseItemParser`1/*02000003*/<!!T> itemParser) cil managed
  // SIG: 10 01 02 15 12 18 01 1E 00 12 61 15 12 0C 01 1E 00
  {
    .param type T /*2A000005*/ 
      .custom /*0C000025:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x206c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SseParser::Create

} // end of class System.Net.ServerSentEvents.SseParser

.class /*02000006*/ public auto ansi sealed beforefieldinit System.Net.ServerSentEvents.SseParser`1<T>
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000026:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000027:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .param type T /*2A000004*/ 
    .custom /*0C000022:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .method /*06000010*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x206f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SseParser`1::.ctor

  .method /*06000011*/ public hidebysig specialname 
          instance string  get_LastEventId() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2077
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SseParser`1::get_LastEventId

  .method /*06000012*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000024*/ 
          get_ReconnectionInterval() cil managed
  // SIG: 20 00 11 80 91
  {
    // Method begins at RVA 0x207a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SseParser`1::get_ReconnectionInterval

  .method /*06000013*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000025*/<valuetype System.Net.ServerSentEvents.SseItem`1/*02000004*/<!T>> 
          Enumerate() cil managed
  // SIG: 20 00 15 12 80 95 01 15 11 10 01 13 00
  {
    .param [0]/*08000019*/ 
    .custom /*0C00002F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 03 00 00 00 01 00 01 00 00 ) 
    // Method begins at RVA 0x207d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SseParser`1::Enumerate

  .method /*06000014*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*01000017*/<valuetype System.Net.ServerSentEvents.SseItem`1/*02000004*/<!T>> 
          EnumerateAsync([opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000019*/ cancellationToken) cil managed
  // SIG: 20 01 15 12 5D 01 15 11 10 01 13 00 11 65
  {
    .param [0]/*0800001A*/ 
    .custom /*0C000030:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 03 00 00 00 01 00 01 00 00 ) 
    .param [1]/*0800001B*/  = nullref
    // Method begins at RVA 0x2080
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SseParser`1::EnumerateAsync

  .property /*17000005*/ instance callconv(8) string
          LastEventId()
  {
    .get instance string System.Net.ServerSentEvents.SseParser`1/*02000006*/::get_LastEventId() /* 06000011 */
  } // end of property SseParser`1::LastEventId
  .property /*17000006*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000024*/
          ReconnectionInterval()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000024*/ System.Net.ServerSentEvents.SseParser`1/*02000006*/::get_ReconnectionInterval() /* 06000012 */
  } // end of property SseParser`1::ReconnectionInterval
} // end of class System.Net.ServerSentEvents.SseParser`1


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
