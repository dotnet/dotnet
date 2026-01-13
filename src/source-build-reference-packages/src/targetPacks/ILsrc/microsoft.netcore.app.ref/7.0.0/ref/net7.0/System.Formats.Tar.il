


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
// Time-date stamp:            0xe364e93b
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00002000
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00003f3a
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
// Checksum:                       0x00011e2f
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00003ee5 [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x00000474] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002a00 [0x00002878] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00003e04 [0x00000054] address [size] of Debug Directory:           
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
//              0x00001f40 Virtual Size
//              0x00002000 Virtual Address
//              0x00002000 Size of Raw Data
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
//              0x00000474 Virtual Size
//              0x00004000 Virtual Address
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
//              0x00006000 Virtual Address
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
//              0x00003000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000f3c
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00003f27 Import Name Table
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
// 0x000021b8 [0x00001bcc] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00003d84 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000d50 Size
//              '#~' Name
//    Stream 2:
//              0x00000dbc Offset
//              0x0000095c Size
//              '#Strings' Name
//    Stream 3:
//              0x00001718 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x0000171c Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x0000172c Offset
//              0x000004a0 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000000901a01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 21112
// PE header size       : 512 (496 used)    ( 2.43%)
// PE additional info   : 11755             (55.68%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.34%)
// CLR meta-data size  : 7116               (33.71%)
// CLR additional info : 128                ( 0.61%)
// CLR method headers  : 76                 ( 0.36%)
// Managed code         : 284               ( 1.35%)
// Data                 : 2048              ( 9.70%)
// Unaccounted          : -879              (-4.16%)

// Num.of PE sections   : 3
//   .text    - 8192
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 7116
//   Module        -    1 (10 bytes)
//   TypeDef       -   18 (252 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   35 (210 bytes)
//   MethodDef     -   76 (1064 bytes)    0 abstract, 0 native, 76 bodies
//   FieldDef      -   29 (174 bytes)     0 constant
//   MemberRef     -   19 (114 bytes)
//   ParamDef      -   82 (492 bytes)
//   Constant      -   37 (222 bytes)
//   CustomAttribute-   64 (384 bytes)
//   InterfaceImpl -    4 (16 bytes)
//   PropertyMap   -    6 (24 bytes)
//   Property      -   20 (120 bytes)
//   MethodSemantic-   33 (198 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    1 (20 bytes)
//   Strings       -  2394 bytes
//   Blobs         -  1184 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   196 bytes

// CLR additional info : 128

// CLR method headers : 76
//   Num.of method bodies  - 76
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 76

// Managed code : 284
//   Ave method size - 3
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class RefSafetyRulesAttribute        (auto) (ansi) (sealed) 
// Class GnuTarEntry                    (public) (auto) (ansi) (sealed) 
// Class PaxGlobalExtendedAttributesTarEntry (public) (auto) (ansi) (sealed) 
// Class PaxTarEntry                    (public) (auto) (ansi) (sealed) 
// Class PosixTarEntry                  (public) (abstract) (auto) (ansi) 
// Class TarEntry                       (public) (abstract) (auto) (ansi) 
// Class TarEntryFormat                 (public) (auto) (ansi) (sealed) 
// Class TarEntryType                   (public) (auto) (ansi) (sealed) 
// Class TarFile                        (public) (abstract) (auto) (ansi) (sealed) 
// Class TarReader                      (public) (auto) (ansi) (sealed) 
// Class TarWriter                      (public) (auto) (ansi) (sealed) 
// Class UstarTarEntry                  (public) (auto) (ansi) (sealed) 
// Class V7TarEntry                     (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly /*20000001*/ noplatform System.Formats.Tar
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000003:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000001*/::.ctor(int32) /* 0A000001 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000004:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000002*/::.ctor() /* 0A000002 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000005:0A000003*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*//DebuggingModes/*01000004*/) /* 0A000003 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000006:0A000004*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 37 2E 30 01 00 54 0E 14   // ersion=v7.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 37 2E 30 )          // Name..NET 7.0
  .custom /*0C000007:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 12 53 79 73 74 65 6D 2E 46 6F 72 6D 61 74   // ...System.Format
                                                                                                                                                                        73 2E 54 61 72 00 00 )                            // s.Tar..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000E:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000F:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 12 53 79 73 74 65 6D 2E 46 6F 72 6D 61 74   // ...System.Format
                                                                                                                                                                       73 2E 54 61 72 00 00 )                            // s.Tar..
  .custom /*0C000010:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 37 2E 30 2E 32 32 2E 35 31 38 30 35 00   // ...7.0.22.51805.
                                                                                                                                                                       00 ) 
  .custom /*0C000011:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 37 2E 30 2E 30 2B 64 30 39 39 66 30 37   // ...7.0.0+d099f07
                                                                                                                                                                                35 65 34 35 64 32 61 61 36 30 30 37 61 32 32 62   // 5e45d2aa6007a22b
                                                                                                                                                                                37 31 62 34 35 61 30 38 37 35 38 35 35 39 66 38   // 71b45a08758559f8
                                                                                                                                                                                30 00 00 )                                        // 0..
  .custom /*0C000012:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 12 53 79 73 74 65 6D 2E 46 6F 72 6D 61 74   // ...System.Format
                                                                                                                                                                 73 2E 54 61 72 00 00 )                            // s.Tar..
  .custom /*0C000014:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
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
  .ver 7:0:0:0
}
.module System.Formats.Tar.dll
// MVID: {1fc51935-717f-480e-8b9b-a203bd6e67c6}
.custom /*0C000001:06000006*/ instance void System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::.ctor(int32) /* 06000006 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000002:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000015:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000016:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000012       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000012 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000017:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000018:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000019:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000001*/ public initonly uint8[] NullableFlags
  .method /*06000002*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8 A_1) cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x2058
    // Code size       23 (0x17)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000012       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000012 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)000023       */ newarr     [System.Runtime/*23000001*/]System.Byte/*01000023*/
    IL_000d:  /* 25   |                  */ dup
    IL_000e:  /* 16   |                  */ ldc.i4.0
    IL_000f:  /* 03   |                  */ ldarg.1
    IL_0010:  /* 9C   |                  */ stelem.i1
    IL_0011:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_0016:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

  .method /*06000003*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] A_1) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x2070
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000012       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000012 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00001B:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00001C:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001D:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000002*/ public initonly uint8 Flag
  .method /*06000004*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8 A_1) cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x207f
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000012       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000012 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00001F:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000020:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000021:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000003*/ public initonly bool IncludesInternals
  .method /*06000005*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool A_1) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x208e
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000012       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000012 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.RefSafetyRulesAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000022:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000023:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000024:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000004*/ public initonly int32 Version
  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 A_1) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x209d
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000012       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000012 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000004       */ stfld      int32 System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::Version /* 04000004 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method RefSafetyRulesAttribute::.ctor

} // end of class System.Runtime.CompilerServices.RefSafetyRulesAttribute

.class /*02000007*/ public auto ansi sealed beforefieldinit System.Formats.Tar.GnuTarEntry
       extends System.Formats.Tar.PosixTarEntry/*0200000A*/
{
  .custom /*0C000026:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000027:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Formats.Tar.TarEntry/*0200000B*/ other) cil managed
  // SIG: 20 01 01 12 2C
  {
    // Method begins at RVA 0x20ac
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000013       */ call       instance void System.Formats.Tar.PosixTarEntry/*0200000A*/::.ctor() /* 06000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GnuTarEntry::.ctor

  .method /*06000008*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Formats.Tar.TarEntryType/*0200000D*/ entryType,
                               string entryName) cil managed
  // SIG: 20 02 01 11 34 0E
  {
    // Method begins at RVA 0x20b4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000013       */ call       instance void System.Formats.Tar.PosixTarEntry/*0200000A*/::.ctor() /* 06000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GnuTarEntry::.ctor

  .method /*06000009*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/ 
          get_AccessTime() cil managed
  // SIG: 20 00 11 55
  {
    // Method begins at RVA 0x20bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GnuTarEntry::get_AccessTime

  .method /*0600000A*/ public hidebysig specialname 
          instance void  set_AccessTime(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/ 'value') cil managed
  // SIG: 20 01 01 11 55
  {
    // Method begins at RVA 0x20bf
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GnuTarEntry::set_AccessTime

  .method /*0600000B*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/ 
          get_ChangeTime() cil managed
  // SIG: 20 00 11 55
  {
    // Method begins at RVA 0x20c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GnuTarEntry::get_ChangeTime

  .method /*0600000C*/ public hidebysig specialname 
          instance void  set_ChangeTime(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/ 'value') cil managed
  // SIG: 20 01 01 11 55
  {
    // Method begins at RVA 0x20c4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GnuTarEntry::set_ChangeTime

  .property /*17000001*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/
          AccessTime()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/ System.Formats.Tar.GnuTarEntry/*02000007*/::get_AccessTime() /* 06000009 */
    .set instance void System.Formats.Tar.GnuTarEntry/*02000007*/::set_AccessTime(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/) /* 0600000A */
  } // end of property GnuTarEntry::AccessTime
  .property /*17000002*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/
          ChangeTime()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/ System.Formats.Tar.GnuTarEntry/*02000007*/::get_ChangeTime() /* 0600000B */
    .set instance void System.Formats.Tar.GnuTarEntry/*02000007*/::set_ChangeTime(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/) /* 0600000C */
  } // end of property GnuTarEntry::ChangeTime
} // end of class System.Formats.Tar.GnuTarEntry

.class /*02000008*/ public auto ansi sealed beforefieldinit System.Formats.Tar.PaxGlobalExtendedAttributesTarEntry
       extends System.Formats.Tar.PosixTarEntry/*0200000A*/
{
  .custom /*0C000028:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000029:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600000D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000016*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000017*/<string,string>> globalExtendedAttributes) cil managed
  // SIG: 20 01 01 15 12 59 01 15 11 5D 02 0E 0E
  {
    .param [1]/*08000006*/ 
    .custom /*0C000025:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 01 00 00 ) 
    // Method begins at RVA 0x20c6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000013       */ call       instance void System.Formats.Tar.PosixTarEntry/*0200000A*/::.ctor() /* 06000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PaxGlobalExtendedAttributesTarEntry::.ctor

  .method /*0600000E*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000018*/<string,string> 
          get_GlobalExtendedAttributes() cil managed
  // SIG: 20 00 15 12 61 02 0E 0E
  {
    // Method begins at RVA 0x20ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PaxGlobalExtendedAttributesTarEntry::get_GlobalExtendedAttributes

  .property /*17000003*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000018*/<string,string>
          GlobalExtendedAttributes()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000018*/<string,string> System.Formats.Tar.PaxGlobalExtendedAttributesTarEntry/*02000008*/::get_GlobalExtendedAttributes() /* 0600000E */
  } // end of property PaxGlobalExtendedAttributesTarEntry::GlobalExtendedAttributes
} // end of class System.Formats.Tar.PaxGlobalExtendedAttributesTarEntry

.class /*02000009*/ public auto ansi sealed beforefieldinit System.Formats.Tar.PaxTarEntry
       extends System.Formats.Tar.PosixTarEntry/*0200000A*/
{
  .custom /*0C00002A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600000F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Formats.Tar.TarEntry/*0200000B*/ other) cil managed
  // SIG: 20 01 01 12 2C
  {
    // Method begins at RVA 0x20d1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000013       */ call       instance void System.Formats.Tar.PosixTarEntry/*0200000A*/::.ctor() /* 06000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PaxTarEntry::.ctor

  .method /*06000010*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Formats.Tar.TarEntryType/*0200000D*/ entryType,
                               string entryName) cil managed
  // SIG: 20 02 01 11 34 0E
  {
    // Method begins at RVA 0x20d9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000013       */ call       instance void System.Formats.Tar.PosixTarEntry/*0200000A*/::.ctor() /* 06000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PaxTarEntry::.ctor

  .method /*06000011*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Formats.Tar.TarEntryType/*0200000D*/ entryType,
                               string entryName,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000016*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000017*/<string,string>> extendedAttributes) cil managed
  // SIG: 20 03 01 11 34 0E 15 12 59 01 15 11 5D 02 0E 0E
  {
    .param [3]/*0800000C*/ 
    .custom /*0C000031:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 01 00 00 ) 
    // Method begins at RVA 0x20e1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000013       */ call       instance void System.Formats.Tar.PosixTarEntry/*0200000A*/::.ctor() /* 06000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PaxTarEntry::.ctor

  .method /*06000012*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000018*/<string,string> 
          get_ExtendedAttributes() cil managed
  // SIG: 20 00 15 12 61 02 0E 0E
  {
    // Method begins at RVA 0x20e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PaxTarEntry::get_ExtendedAttributes

  .property /*17000004*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000018*/<string,string>
          ExtendedAttributes()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000018*/<string,string> System.Formats.Tar.PaxTarEntry/*02000009*/::get_ExtendedAttributes() /* 06000012 */
  } // end of property PaxTarEntry::ExtendedAttributes
} // end of class System.Formats.Tar.PaxTarEntry

.class /*0200000A*/ public abstract auto ansi beforefieldinit System.Formats.Tar.PosixTarEntry
       extends System.Formats.Tar.TarEntry/*0200000B*/
{
  .custom /*0C00002C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000013*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ec
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00001C       */ call       instance void System.Formats.Tar.TarEntry/*0200000B*/::.ctor() /* 0600001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PosixTarEntry::.ctor

  .method /*06000014*/ public hidebysig specialname 
          instance int32  get_DeviceMajor() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PosixTarEntry::get_DeviceMajor

  .method /*06000015*/ public hidebysig specialname 
          instance void  set_DeviceMajor(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20f7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PosixTarEntry::set_DeviceMajor

  .method /*06000016*/ public hidebysig specialname 
          instance int32  get_DeviceMinor() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PosixTarEntry::get_DeviceMinor

  .method /*06000017*/ public hidebysig specialname 
          instance void  set_DeviceMinor(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20fc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PosixTarEntry::set_DeviceMinor

  .method /*06000018*/ public hidebysig specialname 
          instance string  get_GroupName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PosixTarEntry::get_GroupName

  .method /*06000019*/ public hidebysig specialname 
          instance void  set_GroupName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2101
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PosixTarEntry::set_GroupName

  .method /*0600001A*/ public hidebysig specialname 
          instance string  get_UserName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2103
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PosixTarEntry::get_UserName

  .method /*0600001B*/ public hidebysig specialname 
          instance void  set_UserName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2106
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PosixTarEntry::set_UserName

  .property /*17000005*/ instance callconv(8) int32
          DeviceMajor()
  {
    .get instance int32 System.Formats.Tar.PosixTarEntry/*0200000A*/::get_DeviceMajor() /* 06000014 */
    .set instance void System.Formats.Tar.PosixTarEntry/*0200000A*/::set_DeviceMajor(int32) /* 06000015 */
  } // end of property PosixTarEntry::DeviceMajor
  .property /*17000006*/ instance callconv(8) int32
          DeviceMinor()
  {
    .get instance int32 System.Formats.Tar.PosixTarEntry/*0200000A*/::get_DeviceMinor() /* 06000016 */
    .set instance void System.Formats.Tar.PosixTarEntry/*0200000A*/::set_DeviceMinor(int32) /* 06000017 */
  } // end of property PosixTarEntry::DeviceMinor
  .property /*17000007*/ instance callconv(8) string
          GroupName()
  {
    .get instance string System.Formats.Tar.PosixTarEntry/*0200000A*/::get_GroupName() /* 06000018 */
    .set instance void System.Formats.Tar.PosixTarEntry/*0200000A*/::set_GroupName(string) /* 06000019 */
  } // end of property PosixTarEntry::GroupName
  .property /*17000008*/ instance callconv(8) string
          UserName()
  {
    .get instance string System.Formats.Tar.PosixTarEntry/*0200000A*/::get_UserName() /* 0600001A */
    .set instance void System.Formats.Tar.PosixTarEntry/*0200000A*/::set_UserName(string) /* 0600001B */
  } // end of property PosixTarEntry::UserName
} // end of class System.Formats.Tar.PosixTarEntry

.class /*0200000B*/ public abstract auto ansi beforefieldinit System.Formats.Tar.TarEntry
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C00002F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000030:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600001C*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2108
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TarEntry::.ctor

  .method /*0600001D*/ public hidebysig specialname 
          instance int32  get_Checksum() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2110
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_Checksum

  .method /*0600001E*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ 
          get_DataStream() cil managed
  // SIG: 20 00 12 69
  {
    .custom /*0C00003A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2113
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_DataStream

  .method /*0600001F*/ public hidebysig specialname 
          instance void  set_DataStream(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ 'value') cil managed
  // SIG: 20 01 01 12 69
  {
    .custom /*0C00003B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2116
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarEntry::set_DataStream

  .method /*06000020*/ public hidebysig specialname 
          instance valuetype System.Formats.Tar.TarEntryType/*0200000D*/ 
          get_EntryType() cil managed
  // SIG: 20 00 11 34
  {
    // Method begins at RVA 0x2118
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_EntryType

  .method /*06000021*/ public hidebysig specialname 
          instance valuetype System.Formats.Tar.TarEntryFormat/*0200000C*/ 
          get_Format() cil managed
  // SIG: 20 00 11 30
  {
    // Method begins at RVA 0x211b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_Format

  .method /*06000022*/ public hidebysig specialname 
          instance int32  get_Gid() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x211e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_Gid

  .method /*06000023*/ public hidebysig specialname 
          instance void  set_Gid(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2121
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarEntry::set_Gid

  .method /*06000024*/ public hidebysig specialname 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2123
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_Length

  .method /*06000025*/ public hidebysig specialname 
          instance string  get_LinkName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2126
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_LinkName

  .method /*06000026*/ public hidebysig specialname 
          instance void  set_LinkName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2129
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarEntry::set_LinkName

  .method /*06000027*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.IO.UnixFileMode/*0100001B*/ 
          get_Mode() cil managed
  // SIG: 20 00 11 6D
  {
    // Method begins at RVA 0x212b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_Mode

  .method /*06000028*/ public hidebysig specialname 
          instance void  set_Mode(valuetype [System.Runtime/*23000001*/]System.IO.UnixFileMode/*0100001B*/ 'value') cil managed
  // SIG: 20 01 01 11 6D
  {
    // Method begins at RVA 0x212e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarEntry::set_Mode

  .method /*06000029*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/ 
          get_ModificationTime() cil managed
  // SIG: 20 00 11 55
  {
    // Method begins at RVA 0x2130
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_ModificationTime

  .method /*0600002A*/ public hidebysig specialname 
          instance void  set_ModificationTime(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/ 'value') cil managed
  // SIG: 20 01 01 11 55
  {
    // Method begins at RVA 0x2133
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarEntry::set_ModificationTime

  .method /*0600002B*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2135
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_Name

  .method /*0600002C*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2138
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarEntry::set_Name

  .method /*0600002D*/ public hidebysig specialname 
          instance int32  get_Uid() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x213a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::get_Uid

  .method /*0600002E*/ public hidebysig specialname 
          instance void  set_Uid(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x213d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarEntry::set_Uid

  .method /*0600002F*/ public hidebysig instance void 
          ExtractToFile(string destinationFileName,
                        bool overwrite) cil managed
  // SIG: 20 02 01 0E 02
  {
    // Method begins at RVA 0x213f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarEntry::ExtractToFile

  .method /*06000030*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001C*/ 
          ExtractToFileAsync(string destinationFileName,
                             bool overwrite,
                             [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 20 03 12 71 0E 02 11 75
  {
    .param [3]/*0800001C*/  = nullref
    // Method begins at RVA 0x2141
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::ExtractToFileAsync

  .method /*06000031*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2144
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarEntry::ToString

  .property /*17000009*/ instance callconv(8) int32
          Checksum()
  {
    .get instance int32 System.Formats.Tar.TarEntry/*0200000B*/::get_Checksum() /* 0600001D */
  } // end of property TarEntry::Checksum
  .property /*1700000A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/
          DataStream()
  {
    .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ System.Formats.Tar.TarEntry/*0200000B*/::get_DataStream() /* 0600001E */
    .set instance void System.Formats.Tar.TarEntry/*0200000B*/::set_DataStream(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/) /* 0600001F */
  } // end of property TarEntry::DataStream
  .property /*1700000B*/ instance callconv(8) valuetype System.Formats.Tar.TarEntryType/*0200000D*/
          EntryType()
  {
    .get instance valuetype System.Formats.Tar.TarEntryType/*0200000D*/ System.Formats.Tar.TarEntry/*0200000B*/::get_EntryType() /* 06000020 */
  } // end of property TarEntry::EntryType
  .property /*1700000C*/ instance callconv(8) valuetype System.Formats.Tar.TarEntryFormat/*0200000C*/
          Format()
  {
    .get instance valuetype System.Formats.Tar.TarEntryFormat/*0200000C*/ System.Formats.Tar.TarEntry/*0200000B*/::get_Format() /* 06000021 */
  } // end of property TarEntry::Format
  .property /*1700000D*/ instance callconv(8) int32
          Gid()
  {
    .get instance int32 System.Formats.Tar.TarEntry/*0200000B*/::get_Gid() /* 06000022 */
    .set instance void System.Formats.Tar.TarEntry/*0200000B*/::set_Gid(int32) /* 06000023 */
  } // end of property TarEntry::Gid
  .property /*1700000E*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 System.Formats.Tar.TarEntry/*0200000B*/::get_Length() /* 06000024 */
  } // end of property TarEntry::Length
  .property /*1700000F*/ instance callconv(8) string
          LinkName()
  {
    .get instance string System.Formats.Tar.TarEntry/*0200000B*/::get_LinkName() /* 06000025 */
    .set instance void System.Formats.Tar.TarEntry/*0200000B*/::set_LinkName(string) /* 06000026 */
  } // end of property TarEntry::LinkName
  .property /*17000010*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.IO.UnixFileMode/*0100001B*/
          Mode()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.IO.UnixFileMode/*0100001B*/ System.Formats.Tar.TarEntry/*0200000B*/::get_Mode() /* 06000027 */
    .set instance void System.Formats.Tar.TarEntry/*0200000B*/::set_Mode(valuetype [System.Runtime/*23000001*/]System.IO.UnixFileMode/*0100001B*/) /* 06000028 */
  } // end of property TarEntry::Mode
  .property /*17000011*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/
          ModificationTime()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/ System.Formats.Tar.TarEntry/*0200000B*/::get_ModificationTime() /* 06000029 */
    .set instance void System.Formats.Tar.TarEntry/*0200000B*/::set_ModificationTime(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000015*/) /* 0600002A */
  } // end of property TarEntry::ModificationTime
  .property /*17000012*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Formats.Tar.TarEntry/*0200000B*/::get_Name() /* 0600002B */
    .set instance void System.Formats.Tar.TarEntry/*0200000B*/::set_Name(string) /* 0600002C */
  } // end of property TarEntry::Name
  .property /*17000013*/ instance callconv(8) int32
          Uid()
  {
    .get instance int32 System.Formats.Tar.TarEntry/*0200000B*/::get_Uid() /* 0600002D */
    .set instance void System.Formats.Tar.TarEntry/*0200000B*/::set_Uid(int32) /* 0600002E */
  } // end of property TarEntry::Uid
} // end of class System.Formats.Tar.TarEntry

.class /*0200000C*/ public auto ansi sealed System.Formats.Tar.TarEntryFormat
       extends [System.Runtime/*23000001*/]System.Enum/*0100001E*/
{
  .field /*04000005*/ public specialname rtspecialname int32 value__
  .field /*04000006*/ public static literal valuetype System.Formats.Tar.TarEntryFormat/*0200000C*/ Unknown = int32(0x00000000)
  .field /*04000007*/ public static literal valuetype System.Formats.Tar.TarEntryFormat/*0200000C*/ V7 = int32(0x00000001)
  .field /*04000008*/ public static literal valuetype System.Formats.Tar.TarEntryFormat/*0200000C*/ Ustar = int32(0x00000002)
  .field /*04000009*/ public static literal valuetype System.Formats.Tar.TarEntryFormat/*0200000C*/ Pax = int32(0x00000003)
  .field /*0400000A*/ public static literal valuetype System.Formats.Tar.TarEntryFormat/*0200000C*/ Gnu = int32(0x00000004)
} // end of class System.Formats.Tar.TarEntryFormat

.class /*0200000D*/ public auto ansi sealed System.Formats.Tar.TarEntryType
       extends [System.Runtime/*23000001*/]System.Enum/*0100001E*/
{
  .field /*0400000B*/ public specialname rtspecialname uint8 value__
  .field /*0400000C*/ public static literal valuetype System.Formats.Tar.TarEntryType/*0200000D*/ V7RegularFile = uint8(0x00)
  .field /*0400000D*/ public static literal valuetype System.Formats.Tar.TarEntryType/*0200000D*/ RegularFile = uint8(0x30)
  .field /*0400000E*/ public static literal valuetype System.Formats.Tar.TarEntryType/*0200000D*/ HardLink = uint8(0x31)
  .field /*0400000F*/ public static literal valuetype System.Formats.Tar.TarEntryType/*0200000D*/ SymbolicLink = uint8(0x32)
  .field /*04000010*/ public static literal valuetype System.Formats.Tar.TarEntryType/*0200000D*/ CharacterDevice = uint8(0x33)
  .field /*04000011*/ public static literal valuetype System.Formats.Tar.TarEntryType/*0200000D*/ BlockDevice = uint8(0x34)
  .field /*04000012*/ public static literal valuetype System.Formats.Tar.TarEntryType/*0200000D*/ Directory = uint8(0x35)
  .field /*04000013*/ public static literal valuetype System.Formats.Tar.TarEntryType/*0200000D*/ Fifo = uint8(0x36)
  .field /*04000014*/ public static literal valuetype System.Formats.Tar.TarEntryType/*0200000D*/ ContiguousFile = uint8(0x37)
  .field /*04000015*/ public static literal valuetype System.Formats.Tar.TarEntryType/*0200000D*/ DirectoryList = uint8(0x44)
  .field /*04000016*/ public static literal valuetype System.Formats.Tar.TarEntryType/*0200000D*/ LongLink = uint8(0x4B)
  .field /*04000017*/ public static literal valuetype System.Formats.Tar.TarEntryType/*0200000D*/ LongPath = uint8(0x4C)
  .field /*04000018*/ public static literal valuetype System.Formats.Tar.TarEntryType/*0200000D*/ MultiVolume = uint8(0x4D)
  .field /*04000019*/ public static literal valuetype System.Formats.Tar.TarEntryType/*0200000D*/ RenamedOrSymlinked = uint8(0x4E)
  .field /*0400001A*/ public static literal valuetype System.Formats.Tar.TarEntryType/*0200000D*/ SparseFile = uint8(0x53)
  .field /*0400001B*/ public static literal valuetype System.Formats.Tar.TarEntryType/*0200000D*/ TapeVolume = uint8(0x56)
  .field /*0400001C*/ public static literal valuetype System.Formats.Tar.TarEntryType/*0200000D*/ GlobalExtendedAttributes = uint8(0x67)
  .field /*0400001D*/ public static literal valuetype System.Formats.Tar.TarEntryType/*0200000D*/ ExtendedAttributes = uint8(0x78)
} // end of class System.Formats.Tar.TarEntryType

.class /*0200000E*/ public abstract auto ansi sealed beforefieldinit System.Formats.Tar.TarFile
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C000032:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000033:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000032*/ public hidebysig static 
          void  CreateFromDirectory(string sourceDirectoryName,
                                    class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ destination,
                                    bool includeBaseDirectory) cil managed
  // SIG: 00 03 01 0E 12 69 02
  {
    // Method begins at RVA 0x2147
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarFile::CreateFromDirectory

  .method /*06000033*/ public hidebysig static 
          void  CreateFromDirectory(string sourceDirectoryName,
                                    string destinationFileName,
                                    bool includeBaseDirectory) cil managed
  // SIG: 00 03 01 0E 0E 02
  {
    // Method begins at RVA 0x2149
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarFile::CreateFromDirectory

  .method /*06000034*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001C*/ 
          CreateFromDirectoryAsync(string sourceDirectoryName,
                                   class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ destination,
                                   bool includeBaseDirectory,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 00 04 12 71 0E 12 69 02 11 75
  {
    .param [4]/*08000026*/  = nullref
    // Method begins at RVA 0x214b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarFile::CreateFromDirectoryAsync

  .method /*06000035*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001C*/ 
          CreateFromDirectoryAsync(string sourceDirectoryName,
                                   string destinationFileName,
                                   bool includeBaseDirectory,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 00 04 12 71 0E 0E 02 11 75
  {
    .param [4]/*0800002A*/  = nullref
    // Method begins at RVA 0x214e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarFile::CreateFromDirectoryAsync

  .method /*06000036*/ public hidebysig static 
          void  ExtractToDirectory(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ source,
                                   string destinationDirectoryName,
                                   bool overwriteFiles) cil managed
  // SIG: 00 03 01 12 69 0E 02
  {
    // Method begins at RVA 0x2151
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarFile::ExtractToDirectory

  .method /*06000037*/ public hidebysig static 
          void  ExtractToDirectory(string sourceFileName,
                                   string destinationDirectoryName,
                                   bool overwriteFiles) cil managed
  // SIG: 00 03 01 0E 0E 02
  {
    // Method begins at RVA 0x2153
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarFile::ExtractToDirectory

  .method /*06000038*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001C*/ 
          ExtractToDirectoryAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ source,
                                  string destinationDirectoryName,
                                  bool overwriteFiles,
                                  [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 00 04 12 71 12 69 0E 02 11 75
  {
    .param [4]/*08000034*/  = nullref
    // Method begins at RVA 0x2155
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarFile::ExtractToDirectoryAsync

  .method /*06000039*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001C*/ 
          ExtractToDirectoryAsync(string sourceFileName,
                                  string destinationDirectoryName,
                                  bool overwriteFiles,
                                  [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 00 04 12 71 0E 0E 02 11 75
  {
    .param [4]/*08000038*/  = nullref
    // Method begins at RVA 0x2158
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarFile::ExtractToDirectoryAsync

} // end of class System.Formats.Tar.TarFile

.class /*0200000F*/ public auto ansi sealed beforefieldinit System.Formats.Tar.TarReader
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
       implements [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100001F*/,
                  [System.Runtime/*23000001*/]System.IDisposable/*01000020*/
{
  .method /*0600003A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ archiveStream,
                               [opt] bool leaveOpen) cil managed
  // SIG: 20 02 01 12 69 02
  {
    .custom /*0C00003C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800003A*/  = bool(false)
    // Method begins at RVA 0x215b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TarReader::.ctor

  .method /*0600003B*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2163
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarReader::Dispose

  .method /*0600003C*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000021*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 80 85
  {
    // Method begins at RVA 0x2165
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarReader::DisposeAsync

  .method /*0600003D*/ public hidebysig instance class System.Formats.Tar.TarEntry/*0200000B*/ 
          GetNextEntry([opt] bool copyData) cil managed
  // SIG: 20 01 12 2C 02
  {
    .custom /*0C00003E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800003B*/  = bool(false)
    // Method begins at RVA 0x2168
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarReader::GetNextEntry

  .method /*0600003E*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000022*/<class System.Formats.Tar.TarEntry/*0200000B*/> 
          GetNextEntryAsync([opt] bool copyData,
                            [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 89 01 12 2C 02 11 75
  {
    .param [0]/*0800003C*/ 
    .custom /*0C00003D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 02 00 00 ) 
    .param [1]/*0800003D*/  = bool(false)
    .param [2]/*0800003E*/  = nullref
    // Method begins at RVA 0x216b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarReader::GetNextEntryAsync

} // end of class System.Formats.Tar.TarReader

.class /*02000010*/ public auto ansi sealed beforefieldinit System.Formats.Tar.TarWriter
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
       implements [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100001F*/,
                  [System.Runtime/*23000001*/]System.IDisposable/*01000020*/
{
  .custom /*0C000034:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000035:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100001F*/
  .custom /*0C00001A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000020*/
  .custom /*0C00001E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600003F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ archiveStream) cil managed
  // SIG: 20 01 01 12 69
  {
    // Method begins at RVA 0x216e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TarWriter::.ctor

  .method /*06000040*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ archiveStream,
                               [opt] bool leaveOpen) cil managed
  // SIG: 20 02 01 12 69 02
  {
    .param [2]/*08000041*/  = bool(false)
    // Method begins at RVA 0x2176
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TarWriter::.ctor

  .method /*06000041*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001A*/ archiveStream,
                               [opt] valuetype System.Formats.Tar.TarEntryFormat/*0200000C*/ format,
                               [opt] bool leaveOpen) cil managed
  // SIG: 20 03 01 12 69 11 30 02
  {
    .param [2]/*08000043*/  = int32(0x00000003)
    .param [3]/*08000044*/  = bool(false)
    // Method begins at RVA 0x217e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TarWriter::.ctor

  .method /*06000042*/ public hidebysig specialname 
          instance valuetype System.Formats.Tar.TarEntryFormat/*0200000C*/ 
          get_Format() cil managed
  // SIG: 20 00 11 30
  {
    // Method begins at RVA 0x2186
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarWriter::get_Format

  .method /*06000043*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2189
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarWriter::Dispose

  .method /*06000044*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000021*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 80 85
  {
    // Method begins at RVA 0x218b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarWriter::DisposeAsync

  .method /*06000045*/ public hidebysig instance void 
          WriteEntry(class System.Formats.Tar.TarEntry/*0200000B*/ entry) cil managed
  // SIG: 20 01 01 12 2C
  {
    // Method begins at RVA 0x218e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarWriter::WriteEntry

  .method /*06000046*/ public hidebysig instance void 
          WriteEntry(string fileName,
                     string entryName) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .param [2]/*08000047*/ 
    .custom /*0C00003F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2190
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TarWriter::WriteEntry

  .method /*06000047*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001C*/ 
          WriteEntryAsync(class System.Formats.Tar.TarEntry/*0200000B*/ entry,
                          [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 20 02 12 71 12 2C 11 75
  {
    .param [2]/*08000049*/  = nullref
    // Method begins at RVA 0x2192
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarWriter::WriteEntryAsync

  .method /*06000048*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001C*/ 
          WriteEntryAsync(string fileName,
                          string entryName,
                          [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 20 03 12 71 0E 0E 11 75
  {
    .param [2]/*0800004B*/ 
    .custom /*0C000040:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800004C*/  = nullref
    // Method begins at RVA 0x2195
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TarWriter::WriteEntryAsync

  .property /*17000014*/ instance callconv(8) valuetype System.Formats.Tar.TarEntryFormat/*0200000C*/
          Format()
  {
    .get instance valuetype System.Formats.Tar.TarEntryFormat/*0200000C*/ System.Formats.Tar.TarWriter/*02000010*/::get_Format() /* 06000042 */
  } // end of property TarWriter::Format
} // end of class System.Formats.Tar.TarWriter

.class /*02000011*/ public auto ansi sealed beforefieldinit System.Formats.Tar.UstarTarEntry
       extends System.Formats.Tar.PosixTarEntry/*0200000A*/
{
  .custom /*0C000036:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000037:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000049*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Formats.Tar.TarEntry/*0200000B*/ other) cil managed
  // SIG: 20 01 01 12 2C
  {
    // Method begins at RVA 0x2198
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000013       */ call       instance void System.Formats.Tar.PosixTarEntry/*0200000A*/::.ctor() /* 06000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method UstarTarEntry::.ctor

  .method /*0600004A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Formats.Tar.TarEntryType/*0200000D*/ entryType,
                               string entryName) cil managed
  // SIG: 20 02 01 11 34 0E
  {
    // Method begins at RVA 0x21a0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000013       */ call       instance void System.Formats.Tar.PosixTarEntry/*0200000A*/::.ctor() /* 06000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method UstarTarEntry::.ctor

} // end of class System.Formats.Tar.UstarTarEntry

.class /*02000012*/ public auto ansi sealed beforefieldinit System.Formats.Tar.V7TarEntry
       extends System.Formats.Tar.TarEntry/*0200000B*/
{
  .custom /*0C000038:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000039:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600004B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Formats.Tar.TarEntry/*0200000B*/ other) cil managed
  // SIG: 20 01 01 12 2C
  {
    // Method begins at RVA 0x21a8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00001C       */ call       instance void System.Formats.Tar.TarEntry/*0200000B*/::.ctor() /* 0600001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method V7TarEntry::.ctor

  .method /*0600004C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Formats.Tar.TarEntryType/*0200000D*/ entryType,
                               string entryName) cil managed
  // SIG: 20 02 01 11 34 0E
  {
    // Method begins at RVA 0x21b0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00001C       */ call       instance void System.Formats.Tar.TarEntry/*0200000B*/::.ctor() /* 0600001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method V7TarEntry::.ctor

} // end of class System.Formats.Tar.V7TarEntry


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
