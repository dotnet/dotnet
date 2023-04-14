
//  Microsoft (R) .NET IL Disassembler.  Version 5.0.0


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
// Time-date stamp:            0xb059f015
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00001400
// Size of init.data:              0x00000c00
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00005362
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
// Checksum:                       0x000073aa
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00005310 [0x0000004f] address [size] of Import Directory:          
// 0x00006000 [0x00000670] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002400 [0x00002880] address [size] of Security Directory:        
// 0x00008000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x000052f4 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00001368 Virtual Size
//              0x00004000 Virtual Address
//              0x00001400 Size of Raw Data
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
//              0x00000670 Virtual Size
//              0x00006000 Virtual Address
//              0x00000800 Size of Raw Data
//              0x00001a00 Pointer to Raw Data
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
//              0x00002200 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000364
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00004000 Import Address Table
//              0x00005352 Import Name Table
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
// 0x00004054 [0x00001220] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00005274 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000674 Size
//              '#~' Name
//    Stream 2:
//              0x000006e0 Offset
//              0x000005dc Size
//              '#Strings' Name
//    Stream 3:
//              0x00000cbc Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00000cc0 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00000cd0 Offset
//              0x00000550 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000040909a01757 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 19584
// PE header size       : 1024 (536 used)    ( 5.23%)
// PE additional info   : 12215             (62.37%)
// Num.of PE sections   : 4
// CLR header size     : 72                 ( 0.37%)
// CLR meta-data size  : 4640               (23.69%)
// CLR additional info : 128                ( 0.65%)
// CLR method headers  : 21                 ( 0.11%)
// Managed code         : 42                ( 0.21%)
// Data                 : 3072              (15.69%)
// Unaccounted          : -1630             (-8.32%)

// Num.of PE sections   : 4
//   .mvid    - 512
//   .text    - 5120
//   .rsrc    - 2048
//   .reloc   - 512

// CLR meta-data size  : 4640
//   Module        -    1 (10 bytes)
//   TypeDef       -   11 (154 bytes)     3 interfaces, 0 explicit layout
//   TypeRef       -   27 (162 bytes)
//   MethodDef     -   26 (364 bytes)     5 abstract, 0 native, 21 bodies
//   FieldDef      -    3 (18 bytes)      0 constant
//   MemberRef     -   21 (126 bytes)
//   ParamDef      -   29 (174 bytes)
//   CustomAttribute-   62 (372 bytes)
//   InterfaceImpl -    3 (12 bytes)
//   PropertyMap   -    3 (12 bytes)
//   Property      -    8 (48 bytes)
//   MethodSemantic-    8 (48 bytes)
//   TypeSpec      -    1 (2 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    1 (20 bytes)
//   GenericParam  -    2 (16 bytes)
//   Strings       -  1500 bytes
//   Blobs         -  1360 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   200 bytes

// CLR additional info : 128

// CLR method headers : 21
//   Num.of method bodies  - 21
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 21

// Managed code : 42
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Interface IStringLocalizer               (public) (abstract) (auto) (ansi) 
// Interface IStringLocalizerFactory        (public) (abstract) (auto) (ansi) 
// Interface IStringLocalizer`1             (public) (abstract) (auto) (ansi) 
// Class LocalizedString                (public) (auto) (ansi) 
// Class StringLocalizerExtensions      (public) (abstract) (auto) (ansi) (sealed) 
// Class StringLocalizer`1              (public) (auto) (ansi) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly /*20000001*/ Microsoft.Extensions.Localization.Abstractions
{
  .custom /*0C000002:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000003:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000002*/::.ctor(int32) /* 0A000002 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000004:0A000003*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000003*/::.ctor() /* 0A000003 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000005:0A000004*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*//DebuggingModes/*01000005*/) /* 0A000004 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000006:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000007:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 37 2E 30 01 00 54 0E 14   // ersion=v7.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 37 2E 30 )          // Name..NET 7.0
  .custom /*0C000008:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0A 43 6F 6D 6D 69 74 48 61 73 68 28 62 62   // ...CommitHash(bb
                                                                                                                                                                    30 31 62 62 66 34 34 33 33 65 32 37 32 38 39 62   // 01bbf4433e27289b
                                                                                                                                                                    39 39 30 30 31 62 37 64 65 36 61 35 38 32 38 37   // 99001b7de6a58287
                                                                                                                                                                    39 64 31 38 33 35 00 00 )                         // 9d1835..
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0F 53 6F 75 72 63 65 43 6F 6D 6D 69 74 55   // ...SourceCommitU
                                                                                                                                                                    72 6C 52 68 74 74 70 73 3A 2F 2F 67 69 74 68 75   // rlRhttps://githu
                                                                                                                                                                    62 2E 63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70   // b.com/dotnet/asp
                                                                                                                                                                    6E 65 74 63 6F 72 65 2F 74 72 65 65 2F 62 62 30   // netcore/tree/bb0
                                                                                                                                                                    31 62 62 66 34 34 33 33 65 32 37 32 38 39 62 39   // 1bbf4433e27289b9
                                                                                                                                                                    39 30 30 31 62 37 64 65 36 61 35 38 32 38 37 39   // 9001b7de6a582879
                                                                                                                                                                    64 31 38 33 35 00 00 )                            // d1835..
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyConfigurationAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 07 52 65 6C 65 61 73 65 00 00 )             // ...Release..
  .custom /*0C00000E:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000F:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 80 B3 41 62 73 74 72 61 63 74 69 6F 6E 73   // ....Abstractions
                                                                                                                                                                       20 6F 66 20 61 70 70 6C 69 63 61 74 69 6F 6E 20   //  of application 
                                                                                                                                                                       6C 6F 63 61 6C 69 7A 61 74 69 6F 6E 20 73 65 72   // localization ser
                                                                                                                                                                       76 69 63 65 73 2E 0D 0A 43 6F 6D 6D 6F 6E 6C 79   // vices...Commonly
                                                                                                                                                                       20 75 73 65 64 20 74 79 70 65 73 3A 0D 0A 4D 69   //  used types:..Mi
                                                                                                                                                                       63 72 6F 73 6F 66 74 2E 45 78 74 65 6E 73 69 6F   // crosoft.Extensio
                                                                                                                                                                       6E 73 2E 4C 6F 63 61 6C 69 7A 61 74 69 6F 6E 2E   // ns.Localization.
                                                                                                                                                                       49 53 74 72 69 6E 67 4C 6F 63 61 6C 69 7A 65 72   // IStringLocalizer
                                                                                                                                                                       0D 0A 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74 65   // ..Microsoft.Exte
                                                                                                                                                                       6E 73 69 6F 6E 73 2E 4C 6F 63 61 6C 69 7A 61 74   // nsions.Localizat
                                                                                                                                                                       69 6F 6E 2E 49 53 74 72 69 6E 67 4C 6F 63 61 6C   // ion.IStringLocal
                                                                                                                                                                       69 7A 65 72 3C 54 3E 00 00 )                      // izer<T>..
  .custom /*0C000010:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 37 2E 30 2E 32 32 2E 35 31 38 31 39 00   // ...7.0.22.51819.
                                                                                                                                                                       00 ) 
  .custom /*0C000011:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 37 2E 30 2E 30 2B 62 62 30 31 62 62 66   // ...7.0.0+bb01bbf
                                                                                                                                                                                34 34 33 33 65 32 37 32 38 39 62 39 39 30 30 31   // 4433e27289b99001
                                                                                                                                                                                62 37 64 65 36 61 35 38 32 38 37 39 64 31 38 33   // b7de6a582879d183
                                                                                                                                                                                35 00 00 )                                        // 5..
  .custom /*0C000012:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 19 4D 69 63 72 6F 73 6F 66 74 20 2E 4E 45   // ...Microsoft .NE
                                                                                                                                                                   54 20 45 78 74 65 6E 73 69 6F 6E 73 00 00 )       // T Extensions..
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 2E 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ...Microsoft.Ext
                                                                                                                                                                 65 6E 73 69 6F 6E 73 2E 4C 6F 63 61 6C 69 7A 61   // ensions.Localiza
                                                                                                                                                                 74 69 6F 6E 2E 41 62 73 74 72 61 63 74 69 6F 6E   // tion.Abstraction
                                                                                                                                                                 73 00 00 )                                        // s..
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
  .ver 7:0:0:0
}
.module Microsoft.Extensions.Localization.Abstractions.dll
// MVID: {81c9ac67-d618-49d5-96a9-fc8cba5eb08c}
.custom /*0C000001:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY
// Image base: 0x00007F4A6C7B0000


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
  .custom /*0C00001B:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C00001C:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001D:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
  .custom /*0C000020:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000021:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000022:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
  .custom /*0C000023:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000024:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000025:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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

.class /*02000006*/ interface public abstract auto ansi Microsoft.Extensions.Localization.IStringLocalizer
{
  .custom /*0C000026:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000027:0A000013*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000016*/::.ctor(string) /* 0A000013 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .method /*06000006*/ public hidebysig newslot specialname abstract virtual 
          instance class Microsoft.Extensions.Localization.LocalizedString/*02000009*/ 
          get_Item(string name) cil managed
  // SIG: 20 01 12 24 0E
  {
    // Method begins at RVA 0x0
  } // end of method IStringLocalizer::get_Item

  .method /*06000007*/ public hidebysig newslot specialname abstract virtual 
          instance class Microsoft.Extensions.Localization.LocalizedString/*02000009*/ 
          get_Item(string name,
                   object[] arguments) cil managed
  // SIG: 20 02 12 24 0E 1D 1C
  {
    .param [2]/*08000003*/ 
    .custom /*0C00001E:0A000014*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000017*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IStringLocalizer::get_Item

  .method /*06000008*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000019*/<class Microsoft.Extensions.Localization.LocalizedString/*02000009*/> 
          GetAllStrings(bool includeParentCultures) cil managed
  // SIG: 20 01 15 12 65 01 12 24 02
  {
    // Method begins at RVA 0x0
  } // end of method IStringLocalizer::GetAllStrings

  .property /*17000001*/ instance callconv(8) class Microsoft.Extensions.Localization.LocalizedString/*02000009*/
          Item(string)
  {
    .get instance class Microsoft.Extensions.Localization.LocalizedString/*02000009*/ Microsoft.Extensions.Localization.IStringLocalizer/*02000006*/::get_Item(string) /* 06000006 */
  } // end of property IStringLocalizer::Item
  .property /*17000002*/ instance callconv(8) class Microsoft.Extensions.Localization.LocalizedString/*02000009*/
          Item(string,
               object[])
  {
    .get instance class Microsoft.Extensions.Localization.LocalizedString/*02000009*/ Microsoft.Extensions.Localization.IStringLocalizer/*02000006*/::get_Item(string,
                                                                                                                                                               object[]) /* 06000007 */
  } // end of property IStringLocalizer::Item
} // end of class Microsoft.Extensions.Localization.IStringLocalizer

.class /*02000007*/ interface public abstract auto ansi Microsoft.Extensions.Localization.IStringLocalizerFactory
{
  .custom /*0C000029:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000009*/ public hidebysig newslot abstract virtual 
          instance class Microsoft.Extensions.Localization.IStringLocalizer/*02000006*/ 
          Create(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ resourceSource) cil managed
  // SIG: 20 01 12 18 12 69
  {
    // Method begins at RVA 0x0
  } // end of method IStringLocalizerFactory::Create

  .method /*0600000A*/ public hidebysig newslot abstract virtual 
          instance class Microsoft.Extensions.Localization.IStringLocalizer/*02000006*/ 
          Create(string baseName,
                 string location) cil managed
  // SIG: 20 02 12 18 0E 0E
  {
    // Method begins at RVA 0x0
  } // end of method IStringLocalizerFactory::Create

} // end of class Microsoft.Extensions.Localization.IStringLocalizerFactory

.class /*02000008*/ interface public abstract auto ansi Microsoft.Extensions.Localization.IStringLocalizer`1<+ T>
       implements Microsoft.Extensions.Localization.IStringLocalizer/*02000006*/
{
  .param type T /*2A000001*/ 
    .custom /*0C000016:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
} // end of class Microsoft.Extensions.Localization.IStringLocalizer`1

.class /*02000009*/ public auto ansi beforefieldinit Microsoft.Extensions.Localization.LocalizedString
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
{
  .custom /*0C00002A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600000B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               string 'value') cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalizedString::.ctor

  .method /*0600000C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               string 'value',
                               bool resourceNotFound) cil managed
  // SIG: 20 03 01 0E 0E 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalizedString::.ctor

  .method /*0600000D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               string 'value',
                               bool resourceNotFound,
                               string searchedLocation) cil managed
  // SIG: 20 04 01 0E 0E 02 0E
  {
    .param [4]/*08000010*/ 
    .custom /*0C000034:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalizedString::.ctor

  .method /*0600000E*/ public hidebysig specialname static 
          string  op_Implicit(class Microsoft.Extensions.Localization.LocalizedString/*02000009*/ localizedString) cil managed
  // SIG: 00 01 0E 12 24
  {
    .param [0]/*08000011*/ 
    .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000037:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 0F 6C 6F 63 61 6C 69 7A 65 64 53 74 72 69   // ...localizedStri
                                                                                                                                                                                    6E 67 00 00 )                                     // ng..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalizedString::op_Implicit

  .method /*0600000F*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000032:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalizedString::get_Name

  .method /*06000010*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000033:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalizedString::get_Value

  .method /*06000011*/ public hidebysig specialname 
          instance bool  get_ResourceNotFound() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000035:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalizedString::get_ResourceNotFound

  .method /*06000012*/ public hidebysig specialname 
          instance string  get_SearchedLocation() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000038:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000039:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalizedString::get_SearchedLocation

  .method /*06000013*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalizedString::ToString

  .property /*17000003*/ instance callconv(8) string
          Name()
  {
    .get instance string Microsoft.Extensions.Localization.LocalizedString/*02000009*/::get_Name() /* 0600000F */
  } // end of property LocalizedString::Name
  .property /*17000004*/ instance callconv(8) string
          Value()
  {
    .get instance string Microsoft.Extensions.Localization.LocalizedString/*02000009*/::get_Value() /* 06000010 */
  } // end of property LocalizedString::Value
  .property /*17000005*/ instance callconv(8) bool
          ResourceNotFound()
  {
    .get instance bool Microsoft.Extensions.Localization.LocalizedString/*02000009*/::get_ResourceNotFound() /* 06000011 */
  } // end of property LocalizedString::ResourceNotFound
  .property /*17000006*/ instance callconv(8) string
          SearchedLocation()
  {
    .custom /*0C000028:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string Microsoft.Extensions.Localization.LocalizedString/*02000009*/::get_SearchedLocation() /* 06000012 */
  } // end of property LocalizedString::SearchedLocation
} // end of class Microsoft.Extensions.Localization.LocalizedString

.class /*0200000A*/ public abstract auto ansi sealed beforefieldinit Microsoft.Extensions.Localization.StringLocalizerExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
{
  .custom /*0C00002C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00002E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000014*/ public hidebysig static 
          class Microsoft.Extensions.Localization.LocalizedString/*02000009*/ 
          GetString(class Microsoft.Extensions.Localization.IStringLocalizer/*02000006*/ stringLocalizer,
                    string name) cil managed
  // SIG: 00 02 12 24 12 18 0E
  {
    .custom /*0C00003A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringLocalizerExtensions::GetString

  .method /*06000015*/ public hidebysig static 
          class Microsoft.Extensions.Localization.LocalizedString/*02000009*/ 
          GetString(class Microsoft.Extensions.Localization.IStringLocalizer/*02000006*/ stringLocalizer,
                    string name,
                    object[] arguments) cil managed
  // SIG: 00 03 12 24 12 18 0E 1D 1C
  {
    .custom /*0C00003B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [3]/*08000017*/ 
    .custom /*0C00003D:0A000014*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000017*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringLocalizerExtensions::GetString

  .method /*06000016*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000019*/<class Microsoft.Extensions.Localization.LocalizedString/*02000009*/> 
          GetAllStrings(class Microsoft.Extensions.Localization.IStringLocalizer/*02000006*/ stringLocalizer) cil managed
  // SIG: 00 01 15 12 65 01 12 24 12 18
  {
    .custom /*0C00003C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringLocalizerExtensions::GetAllStrings

} // end of class Microsoft.Extensions.Localization.StringLocalizerExtensions

.class /*0200000B*/ public auto ansi beforefieldinit Microsoft.Extensions.Localization.StringLocalizer`1<TResourceSource>
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
       implements class Microsoft.Extensions.Localization.IStringLocalizer`1/*02000008*/<!TResourceSource>/*1B000001*/,
                  Microsoft.Extensions.Localization.IStringLocalizer/*02000006*/
{
  .custom /*0C00002F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000030:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000031:0A000013*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000016*/::.ctor(string) /* 0A000013 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .interfaceimpl type class Microsoft.Extensions.Localization.IStringLocalizer`1/*02000008*/<!TResourceSource>/*1B000001*/
  .custom /*0C000019:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type Microsoft.Extensions.Localization.IStringLocalizer/*02000006*/
  .custom /*0C00001F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .param type TResourceSource /*2A000002*/ 
    .custom /*0C00001A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*06000017*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.Extensions.Localization.IStringLocalizerFactory/*02000007*/ factory) cil managed
  // SIG: 20 01 01 12 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringLocalizer`1::.ctor

  .method /*06000018*/ public hidebysig newslot specialname virtual 
          instance class Microsoft.Extensions.Localization.LocalizedString/*02000009*/ 
          get_Item(string name) cil managed
  // SIG: 20 01 12 24 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringLocalizer`1::get_Item

  .method /*06000019*/ public hidebysig newslot specialname virtual 
          instance class Microsoft.Extensions.Localization.LocalizedString/*02000009*/ 
          get_Item(string name,
                   object[] arguments) cil managed
  // SIG: 20 02 12 24 0E 1D 1C
  {
    .param [2]/*0800001C*/ 
    .custom /*0C00003E:0A000014*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000017*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringLocalizer`1::get_Item

  .method /*0600001A*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000019*/<class Microsoft.Extensions.Localization.LocalizedString/*02000009*/> 
          GetAllStrings(bool includeParentCultures) cil managed
  // SIG: 20 01 15 12 65 01 12 24 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringLocalizer`1::GetAllStrings

  .property /*17000007*/ instance callconv(8) class Microsoft.Extensions.Localization.LocalizedString/*02000009*/
          Item(string)
  {
    .get instance class Microsoft.Extensions.Localization.LocalizedString/*02000009*/ Microsoft.Extensions.Localization.StringLocalizer`1/*0200000B*/::get_Item(string) /* 06000018 */
  } // end of property StringLocalizer`1::Item
  .property /*17000008*/ instance callconv(8) class Microsoft.Extensions.Localization.LocalizedString/*02000009*/
          Item(string,
               object[])
  {
    .get instance class Microsoft.Extensions.Localization.LocalizedString/*02000009*/ Microsoft.Extensions.Localization.StringLocalizer`1/*0200000B*/::get_Item(string,
                                                                                                                                                                object[]) /* 06000019 */
  } // end of property StringLocalizer`1::Item
} // end of class Microsoft.Extensions.Localization.StringLocalizer`1


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
