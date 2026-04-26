


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
// Time-date stamp:            0xd5e1572d
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00001c00
// Size of init.data:              0x00000c00
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00005a82
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
// Checksum:                       0x0000d7b6
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00005a30 [0x0000004f] address [size] of Import Directory:          
// 0x00006000 [0x00000678] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002c00 [0x00002470] address [size] of Security Directory:        
// 0x00008000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00005a14 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00001a88 Virtual Size
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
//              0x00000678 Virtual Size
//              0x00006000 Virtual Address
//              0x00000800 Size of Raw Data
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
//              0x00002a00 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000a84
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00004000 Import Address Table
//              0x00005a72 Import Name Table
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
// 0x00004054 [0x00001940] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00005994 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000aa0 Size
//              '#~' Name
//    Stream 2:
//              0x00000b0c Offset
//              0x000007dc Size
//              '#Strings' Name
//    Stream 3:
//              0x000012e8 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x000012ec Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x000012fc Offset
//              0x00000644 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000020901a01d57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 20592
// PE header size       : 1024 (536 used)    ( 4.97%)
// PE additional info   : 11183             (54.31%)
// Num.of PE sections   : 4
// CLR header size     : 72                 ( 0.35%)
// CLR meta-data size  : 6464               (31.39%)
// CLR additional info : 128                ( 0.62%)
// CLR method headers  : 43                 ( 0.21%)
// Managed code         : 86                ( 0.42%)
// Data                 : 3072              (14.92%)
// Unaccounted          : -1480             (-7.19%)

// Num.of PE sections   : 4
//   .mvid    - 512
//   .text    - 7168
//   .rsrc    - 2048
//   .reloc   - 512

// CLR meta-data size  : 6464
//   Module        -    1 (10 bytes)
//   TypeDef       -   14 (196 bytes)     2 interfaces, 0 explicit layout
//   TypeRef       -   35 (210 bytes)
//   MethodDef     -   45 (630 bytes)     2 abstract, 0 native, 43 bodies
//   FieldDef      -   20 (120 bytes)     2 constant
//   MemberRef     -   18 (108 bytes)
//   ParamDef      -   55 (330 bytes)
//   Constant      -   16 (96 bytes)
//   CustomAttribute-   98 (588 bytes)
//   PropertyMap   -    5 (20 bytes)
//   Property      -   19 (114 bytes)
//   MethodSemantic-   24 (144 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    2 (40 bytes)
//   NestedClass   -    1 (4 bytes)
//   Strings       -  2011 bytes
//   Blobs         -  1604 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   197 bytes

// CLR additional info : 128

// CLR method headers : 43
//   Num.of method bodies  - 43
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 43

// Managed code : 86
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class HealthCheckContext             (public) (auto) (ansi) (sealed) 
// Class HealthCheckRegistration        (public) (auto) (ansi) (sealed) 
// Class HealthCheckResult              (public) (sequential) (ansi) (sealed) 
// Class HealthReport                   (public) (auto) (ansi) (sealed) 
// Class HealthReportEntry              (public) (sequential) (ansi) (sealed) 
// Class HealthStatus                   (public) (auto) (ansi) (sealed) 
// Interface IHealthCheck                   (public) (abstract) (auto) (ansi) 
// Interface IHealthCheckPublisher          (public) (abstract) (auto) (ansi) 
// Class <>c__DisplayClass4_0           (auto) (ansi) (sealed) (nested private) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ System.ComponentModel
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly /*20000001*/ Microsoft.Extensions.Diagnostics.HealthChecks.Abstractions
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000002:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000001*/::.ctor(int32) /* 0A000001 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000003:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000002*/::.ctor() /* 0A000002 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000004:0A000003*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*//DebuggingModes/*01000004*/) /* 0A000003 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000005:0A000004*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000005*/::.ctor() /* 0A000004 */ = ( 01 00 00 00 ) 
  .custom /*0C000006:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000006*/::.ctor(string) /* 0A000005 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C000007:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0A 43 6F 6D 6D 69 74 48 61 73 68 28 31 64   // ...CommitHash(1d
                                                                                                                                                                    63 66 37 61 63 66 61 63 66 30 66 65 31 35 34 61   // cf7acfacf0fe154a
                                                                                                                                                                    64 63 63 32 33 32 37 30 63 62 30 64 61 31 31 66   // dcc23270cb0da11f
                                                                                                                                                                    66 34 34 61 63 65 00 00 )                         // f44ace..
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0F 53 6F 75 72 63 65 43 6F 6D 6D 69 74 55   // ...SourceCommitU
                                                                                                                                                                    72 6C 52 68 74 74 70 73 3A 2F 2F 67 69 74 68 75   // rlRhttps://githu
                                                                                                                                                                    62 2E 63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70   // b.com/dotnet/asp
                                                                                                                                                                    6E 65 74 63 6F 72 65 2F 74 72 65 65 2F 31 64 63   // netcore/tree/1dc
                                                                                                                                                                    66 37 61 63 66 61 63 66 30 66 65 31 35 34 61 64   // f7acfacf0fe154ad
                                                                                                                                                                    63 63 32 33 32 37 30 63 62 30 64 61 31 31 66 66   // cc23270cb0da11ff
                                                                                                                                                                    34 34 61 63 65 00 00 )                            // 44ace..
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyConfigurationAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 07 52 65 6C 65 61 73 65 00 00 )             // ...Release..
  .custom /*0C00000C:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000D:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 80 95 41 62 73 74 72 61 63 74 69 6F 6E 73   // ....Abstractions
                                                                                                                                                                       20 66 6F 72 20 64 65 66 69 6E 69 6E 67 20 68 65   //  for defining he
                                                                                                                                                                       61 6C 74 68 20 63 68 65 63 6B 73 20 69 6E 20 2E   // alth checks in .
                                                                                                                                                                       4E 45 54 20 61 70 70 6C 69 63 61 74 69 6F 6E 73   // NET applications
                                                                                                                                                                       0D 0A 0D 0A 43 6F 6D 6D 6F 6E 6C 79 20 55 73 65   // ....Commonly Use
                                                                                                                                                                       64 20 54 79 70 65 73 0D 0A 4D 69 63 72 6F 73 6F   // d Types..Microso
                                                                                                                                                                       66 74 2E 45 78 74 65 6E 73 69 6F 6E 73 2E 44 69   // ft.Extensions.Di
                                                                                                                                                                       61 67 6E 6F 73 74 69 63 73 2E 48 65 61 6C 74 68   // agnostics.Health
                                                                                                                                                                       43 68 65 63 6B 73 2E 49 48 65 61 6C 74 68 43 68   // Checks.IHealthCh
                                                                                                                                                                       65 63 6B 0D 0A 20 20 20 20 00 00 )                // eck..    ..
  .custom /*0C00000E:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 0C 36 2E 30 2E 32 32 32 2E 36 34 31 32 00   // ...6.0.222.6412.
                                                                                                                                                                       00 ) 
  .custom /*0C00000F:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 2E 36 2E 30 2E 32 2B 31 64 63 66 37 61 63   // ...6.0.2+1dcf7ac
                                                                                                                                                                                66 61 63 66 30 66 65 31 35 34 61 64 63 63 32 33   // facf0fe154adcc23
                                                                                                                                                                                32 37 30 63 62 30 64 61 31 31 66 66 34 34 61 63   // 270cb0da11ff44ac
                                                                                                                                                                                65 00 00 )                                        // e..
  .custom /*0C000010:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 16 4D 69 63 72 6F 73 6F 66 74 20 41 53 50   // ...Microsoft ASP
                                                                                                                                                                   2E 4E 45 54 20 43 6F 72 65 00 00 )                // .NET Core..
  .custom /*0C000011:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 3A 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..:Microsoft.Ext
                                                                                                                                                                 65 6E 73 69 6F 6E 73 2E 44 69 61 67 6E 6F 73 74   // ensions.Diagnost
                                                                                                                                                                 69 63 73 2E 48 65 61 6C 74 68 43 68 65 63 6B 73   // ics.HealthChecks
                                                                                                                                                                 2E 41 62 73 74 72 61 63 74 69 6F 6E 73 00 00 )    // .Abstractions..
  .custom /*0C000012:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    24 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // $https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70 6E 65   // com/dotnet/aspne
                                                                                                                                                                    74 63 6F 72 65 00 00 )                            // tcore..
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
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
.module Microsoft.Extensions.Diagnostics.HealthChecks.Abstractions.dll
// MVID: {828d786a-e87b-40c3-8841-641ed62015eb}
.custom /*0C000001:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000014:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000015:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000016:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000017:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000018:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000019:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00001A:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001B:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00001D:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00001E:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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

.class /*02000006*/ public auto ansi sealed beforefieldinit Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckContext
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000023:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000024:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000006*/ public hidebysig specialname 
          instance class Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckRegistration/*02000007*/ 
          get_Registration() cil managed
  // SIG: 20 00 12 1C
  {
    .custom /*0C000021:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckContext::get_Registration

  .method /*06000007*/ public hidebysig specialname 
          instance void  set_Registration(class Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckRegistration/*02000007*/ 'value') cil managed
  // SIG: 20 01 01 12 1C
  {
    .custom /*0C000025:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckContext::set_Registration

  .method /*06000008*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckContext::.ctor

  .property /*17000001*/ instance callconv(8) class Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckRegistration/*02000007*/
          Registration()
  {
    .get instance class Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckRegistration/*02000007*/ Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckContext/*02000006*/::get_Registration() /* 06000006 */
    .set instance void Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckContext/*02000006*/::set_Registration(class Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckRegistration/*02000007*/) /* 06000007 */
  } // end of property HealthCheckContext::Registration
} // end of class Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckContext

.class /*02000007*/ public auto ansi sealed beforefieldinit Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckRegistration
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000027:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000028:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200000E*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass4_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000015*/
  {
    .custom /*0C000039:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    .field /*04000014*/ public class Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck/*0200000C*/ 'instance'
    .method /*0600002D*/ public hidebysig specialname rtspecialname 
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

  .method /*06000009*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck/*0200000C*/ 'instance',
                               valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000016*/<valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/> failureStatus,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000017*/<string> tags) cil managed
  // SIG: 20 04 01 0E 12 30 15 11 59 01 11 2C 15 12 5D 01 0E
  {
    .param [4]/*08000005*/ 
    .custom /*0C000020:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckRegistration::.ctor

  .method /*0600000A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck/*0200000C*/ 'instance',
                               valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000016*/<valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/> failureStatus,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000017*/<string> tags,
                               valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000016*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000018*/> timeout) cil managed
  // SIG: 20 05 01 0E 12 30 15 11 59 01 11 2C 15 12 5D 01 0E 15 11 59 01 11 61
  {
    .param [4]/*08000009*/ 
    .custom /*0C00002F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckRegistration::.ctor

  .method /*0600000B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<class [System.ComponentModel/*23000002*/]System.IServiceProvider/*0100001A*/,class Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck/*0200000C*/> factory,
                               valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000016*/<valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/> failureStatus,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000017*/<string> tags) cil managed
  // SIG: 20 04 01 0E 15 12 65 02 12 69 12 30 15 11 59 01 11 2C 15 12 5D 01 0E
  {
    .param [4]/*0800000E*/ 
    .custom /*0C00003A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckRegistration::.ctor

  .method /*0600000C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<class [System.ComponentModel/*23000002*/]System.IServiceProvider/*0100001A*/,class Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck/*0200000C*/> factory,
                               valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000016*/<valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/> failureStatus,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000017*/<string> tags,
                               valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000016*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000018*/> timeout) cil managed
  // SIG: 20 05 01 0E 15 12 65 02 12 69 12 30 15 11 59 01 11 2C 15 12 5D 01 0E 15 11 59 01 11 61
  {
    .param [4]/*08000012*/ 
    .custom /*0C00003F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckRegistration::.ctor

  .method /*0600000D*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<class [System.ComponentModel/*23000002*/]System.IServiceProvider/*0100001A*/,class Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck/*0200000C*/> 
          get_Factory() cil managed
  // SIG: 20 00 15 12 65 02 12 69 12 30
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckRegistration::get_Factory

  .method /*0600000E*/ public hidebysig specialname 
          instance void  set_Factory(class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<class [System.ComponentModel/*23000002*/]System.IServiceProvider/*0100001A*/,class Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck/*0200000C*/> 'value') cil managed
  // SIG: 20 01 01 15 12 65 02 12 69 12 30
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckRegistration::set_Factory

  .method /*0600000F*/ public hidebysig specialname 
          instance valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/ 
          get_FailureStatus() cil managed
  // SIG: 20 00 11 2C
  {
    .custom /*0C00003C:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckRegistration::get_FailureStatus

  .method /*06000010*/ public hidebysig specialname 
          instance void  set_FailureStatus(valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/ 'value') cil managed
  // SIG: 20 01 01 11 2C
  {
    .custom /*0C00003E:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckRegistration::set_FailureStatus

  .method /*06000011*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000018*/ 
          get_Timeout() cil managed
  // SIG: 20 00 11 61
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckRegistration::get_Timeout

  .method /*06000012*/ public hidebysig specialname 
          instance void  set_Timeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000018*/ 'value') cil managed
  // SIG: 20 01 01 11 61
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckRegistration::set_Timeout

  .method /*06000013*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckRegistration::get_Name

  .method /*06000014*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckRegistration::set_Name

  .method /*06000015*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ISet`1/*0100001B*/<string> 
          get_Tags() cil managed
  // SIG: 20 00 15 12 6D 01 0E
  {
    .custom /*0C000041:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckRegistration::get_Tags

  .property /*17000002*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<class [System.ComponentModel/*23000002*/]System.IServiceProvider/*0100001A*/,class Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck/*0200000C*/>
          Factory()
  {
    .get instance class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<class [System.ComponentModel/*23000002*/]System.IServiceProvider/*0100001A*/,class Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck/*0200000C*/> Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckRegistration/*02000007*/::get_Factory() /* 0600000D */
    .set instance void Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckRegistration/*02000007*/::set_Factory(class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<class [System.ComponentModel/*23000002*/]System.IServiceProvider/*0100001A*/,class Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck/*0200000C*/>) /* 0600000E */
  } // end of property HealthCheckRegistration::Factory
  .property /*17000003*/ instance callconv(8) valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/
          FailureStatus()
  {
    .get instance valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/ Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckRegistration/*02000007*/::get_FailureStatus() /* 0600000F */
    .set instance void Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckRegistration/*02000007*/::set_FailureStatus(valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/) /* 06000010 */
  } // end of property HealthCheckRegistration::FailureStatus
  .property /*17000004*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000018*/
          Timeout()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000018*/ Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckRegistration/*02000007*/::get_Timeout() /* 06000011 */
    .set instance void Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckRegistration/*02000007*/::set_Timeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000018*/) /* 06000012 */
  } // end of property HealthCheckRegistration::Timeout
  .property /*17000005*/ instance callconv(8) string
          Name()
  {
    .get instance string Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckRegistration/*02000007*/::get_Name() /* 06000013 */
    .set instance void Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckRegistration/*02000007*/::set_Name(string) /* 06000014 */
  } // end of property HealthCheckRegistration::Name
  .property /*17000006*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ISet`1/*0100001B*/<string>
          Tags()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ISet`1/*0100001B*/<string> Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckRegistration/*02000007*/::get_Tags() /* 06000015 */
  } // end of property HealthCheckRegistration::Tags
} // end of class Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckRegistration

.class /*02000008*/ public sequential ansi sealed beforefieldinit Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult
       extends [System.Runtime/*23000001*/]System.ValueType/*0100001C*/
{
  .custom /*0C00002B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00002C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000004*/ private static initonly class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,object> _emptyReadOnlyDictionary
  .field /*04000005*/ private initonly class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,object> '<Data>k__BackingField'
  .custom /*0C00001C:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .field /*04000006*/ private initonly string '<Description>k__BackingField'
  .custom /*0C000022:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .field /*04000007*/ private initonly class [System.Runtime/*23000001*/]System.Exception/*0100001E*/ '<Exception>k__BackingField'
  .custom /*0C000026:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .field /*04000008*/ private initonly valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/ '<Status>k__BackingField'
  .custom /*0C00002A:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .method /*06000016*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/ status,
                               [opt] string description,
                               [opt] class [System.Runtime/*23000001*/]System.Exception/*0100001E*/ exception,
                               [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,object> data) cil managed
  // SIG: 20 04 01 11 2C 0E 12 79 15 12 75 02 0E 1C
  {
    .param [2]/*08000019*/  = nullref
    .param [3]/*0800001A*/  = nullref
    .param [4]/*0800001B*/  = nullref
    .custom /*0C00004B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckResult::.ctor

  .method /*06000017*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,object> 
          get_Data() cil managed
  // SIG: 20 00 15 12 75 02 0E 1C
  {
    .custom /*0C000042:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001F*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C000043:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000044:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckResult::get_Data

  .method /*06000018*/ public hidebysig specialname 
          instance string  get_Description() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000045:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001F*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C000046:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckResult::get_Description

  .method /*06000019*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Exception/*0100001E*/ 
          get_Exception() cil managed
  // SIG: 20 00 12 79
  {
    .custom /*0C000047:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001F*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C000048:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckResult::get_Exception

  .method /*0600001A*/ public hidebysig specialname 
          instance valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/ 
          get_Status() cil managed
  // SIG: 20 00 11 2C
  {
    .custom /*0C000049:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001F*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C00004A:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckResult::get_Status

  .method /*0600001B*/ public hidebysig static 
          valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult/*02000008*/ 
          Healthy([opt] string description,
                  [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,object> data) cil managed
  // SIG: 00 02 11 20 0E 15 12 75 02 0E 1C
  {
    .param [1]/*0800001C*/  = nullref
    .param [2]/*0800001D*/  = nullref
    .custom /*0C00004C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckResult::Healthy

  .method /*0600001C*/ public hidebysig static 
          valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult/*02000008*/ 
          Degraded([opt] string description,
                   [opt] class [System.Runtime/*23000001*/]System.Exception/*0100001E*/ exception,
                   [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,object> data) cil managed
  // SIG: 00 03 11 20 0E 12 79 15 12 75 02 0E 1C
  {
    .param [1]/*0800001E*/  = nullref
    .param [2]/*0800001F*/  = nullref
    .param [3]/*08000020*/  = nullref
    .custom /*0C00004E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckResult::Degraded

  .method /*0600001D*/ public hidebysig static 
          valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult/*02000008*/ 
          Unhealthy([opt] string description,
                    [opt] class [System.Runtime/*23000001*/]System.Exception/*0100001E*/ exception,
                    [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,object> data) cil managed
  // SIG: 00 03 11 20 0E 12 79 15 12 75 02 0E 1C
  {
    .param [1]/*08000021*/  = nullref
    .param [2]/*08000022*/  = nullref
    .param [3]/*08000023*/  = nullref
    .custom /*0C000051:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthCheckResult::Unhealthy

  .property /*17000007*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,object>
          Data()
  {
    .custom /*0C000029:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,object> Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult/*02000008*/::get_Data() /* 06000017 */
  } // end of property HealthCheckResult::Data
  .property /*17000008*/ instance callconv(8) string
          Description()
  {
    .get instance string Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult/*02000008*/::get_Description() /* 06000018 */
  } // end of property HealthCheckResult::Description
  .property /*17000009*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Exception/*0100001E*/
          Exception()
  {
    .get instance class [System.Runtime/*23000001*/]System.Exception/*0100001E*/ Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult/*02000008*/::get_Exception() /* 06000019 */
  } // end of property HealthCheckResult::Exception
  .property /*1700000A*/ instance callconv(8) valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/
          Status()
  {
    .get instance valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/ Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult/*02000008*/::get_Status() /* 0600001A */
  } // end of property HealthCheckResult::Status
} // end of class Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult

.class /*02000009*/ public auto ansi sealed beforefieldinit Microsoft.Extensions.Diagnostics.HealthChecks.HealthReport
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C00002D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600001E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthReportEntry/*0200000A*/> entries,
                               valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000018*/ totalDuration) cil managed
  // SIG: 20 02 01 15 12 75 02 0E 11 28 11 61
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthReport::.ctor

  .method /*0600001F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthReportEntry/*0200000A*/> entries,
                               valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/ status,
                               valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000018*/ totalDuration) cil managed
  // SIG: 20 03 01 15 12 75 02 0E 11 28 11 2C 11 61
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthReport::.ctor

  .method /*06000020*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthReportEntry/*0200000A*/> 
          get_Entries() cil managed
  // SIG: 20 00 15 12 75 02 0E 11 28
  {
    .custom /*0C00004D:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthReport::get_Entries

  .method /*06000021*/ public hidebysig specialname 
          instance valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/ 
          get_Status() cil managed
  // SIG: 20 00 11 2C
  {
    .custom /*0C00004F:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthReport::get_Status

  .method /*06000022*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000018*/ 
          get_TotalDuration() cil managed
  // SIG: 20 00 11 61
  {
    .custom /*0C000050:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthReport::get_TotalDuration

  .property /*1700000B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthReportEntry/*0200000A*/>
          Entries()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthReportEntry/*0200000A*/> Microsoft.Extensions.Diagnostics.HealthChecks.HealthReport/*02000009*/::get_Entries() /* 06000020 */
  } // end of property HealthReport::Entries
  .property /*1700000C*/ instance callconv(8) valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/
          Status()
  {
    .get instance valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/ Microsoft.Extensions.Diagnostics.HealthChecks.HealthReport/*02000009*/::get_Status() /* 06000021 */
  } // end of property HealthReport::Status
  .property /*1700000D*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000018*/
          TotalDuration()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000018*/ Microsoft.Extensions.Diagnostics.HealthChecks.HealthReport/*02000009*/::get_TotalDuration() /* 06000022 */
  } // end of property HealthReport::TotalDuration
} // end of class Microsoft.Extensions.Diagnostics.HealthChecks.HealthReport

.class /*0200000A*/ public sequential ansi sealed beforefieldinit Microsoft.Extensions.Diagnostics.HealthChecks.HealthReportEntry
       extends [System.Runtime/*23000001*/]System.ValueType/*0100001C*/
{
  .custom /*0C000031:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000032:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000009*/ private static initonly class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,object> _emptyReadOnlyDictionary
  .field /*0400000A*/ private initonly class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,object> '<Data>k__BackingField'
  .custom /*0C000030:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .field /*0400000B*/ private initonly string '<Description>k__BackingField'
  .custom /*0C000033:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .field /*0400000C*/ private initonly valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000018*/ '<Duration>k__BackingField'
  .custom /*0C000034:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .field /*0400000D*/ private initonly class [System.Runtime/*23000001*/]System.Exception/*0100001E*/ '<Exception>k__BackingField'
  .custom /*0C000036:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .field /*0400000E*/ private initonly valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/ '<Status>k__BackingField'
  .custom /*0C000038:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .field /*0400000F*/ private initonly class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000017*/<string> '<Tags>k__BackingField'
  .custom /*0C00003D:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .method /*06000023*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/ status,
                               string description,
                               valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000018*/ duration,
                               class [System.Runtime/*23000001*/]System.Exception/*0100001E*/ exception,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,object> data) cil managed
  // SIG: 20 05 01 11 2C 0E 11 61 12 79 15 12 75 02 0E 1C
  {
    .param [5]/*0800002D*/ 
    .custom /*0C000060:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthReportEntry::.ctor

  .method /*06000024*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/ status,
                               string description,
                               valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000018*/ duration,
                               class [System.Runtime/*23000001*/]System.Exception/*0100001E*/ exception,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,object> data,
                               [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000017*/<string> tags) cil managed
  // SIG: 20 06 01 11 2C 0E 11 61 12 79 15 12 75 02 0E 1C 15 12 5D 01 0E
  {
    .param [5]/*08000032*/ 
    .custom /*0C000061:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [6]/*08000033*/  = nullref
    .custom /*0C000062:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthReportEntry::.ctor

  .method /*06000025*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,object> 
          get_Data() cil managed
  // SIG: 20 00 15 12 75 02 0E 1C
  {
    .custom /*0C000052:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001F*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C000053:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000054:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthReportEntry::get_Data

  .method /*06000026*/ public hidebysig specialname 
          instance string  get_Description() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000055:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001F*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C000056:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthReportEntry::get_Description

  .method /*06000027*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000018*/ 
          get_Duration() cil managed
  // SIG: 20 00 11 61
  {
    .custom /*0C000057:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001F*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C000058:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthReportEntry::get_Duration

  .method /*06000028*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Exception/*0100001E*/ 
          get_Exception() cil managed
  // SIG: 20 00 12 79
  {
    .custom /*0C000059:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001F*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C00005A:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthReportEntry::get_Exception

  .method /*06000029*/ public hidebysig specialname 
          instance valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/ 
          get_Status() cil managed
  // SIG: 20 00 11 2C
  {
    .custom /*0C00005B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001F*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C00005C:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthReportEntry::get_Status

  .method /*0600002A*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000017*/<string> 
          get_Tags() cil managed
  // SIG: 20 00 15 12 5D 01 0E
  {
    .custom /*0C00005D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001F*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C00005E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00005F:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HealthReportEntry::get_Tags

  .property /*1700000E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,object>
          Data()
  {
    .custom /*0C00003B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*0100001D*/<string,object> Microsoft.Extensions.Diagnostics.HealthChecks.HealthReportEntry/*0200000A*/::get_Data() /* 06000025 */
  } // end of property HealthReportEntry::Data
  .property /*1700000F*/ instance callconv(8) string
          Description()
  {
    .get instance string Microsoft.Extensions.Diagnostics.HealthChecks.HealthReportEntry/*0200000A*/::get_Description() /* 06000026 */
  } // end of property HealthReportEntry::Description
  .property /*17000010*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000018*/
          Duration()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000018*/ Microsoft.Extensions.Diagnostics.HealthChecks.HealthReportEntry/*0200000A*/::get_Duration() /* 06000027 */
  } // end of property HealthReportEntry::Duration
  .property /*17000011*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Exception/*0100001E*/
          Exception()
  {
    .get instance class [System.Runtime/*23000001*/]System.Exception/*0100001E*/ Microsoft.Extensions.Diagnostics.HealthChecks.HealthReportEntry/*0200000A*/::get_Exception() /* 06000028 */
  } // end of property HealthReportEntry::Exception
  .property /*17000012*/ instance callconv(8) valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/
          Status()
  {
    .get instance valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/ Microsoft.Extensions.Diagnostics.HealthChecks.HealthReportEntry/*0200000A*/::get_Status() /* 06000029 */
  } // end of property HealthReportEntry::Status
  .property /*17000013*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000017*/<string>
          Tags()
  {
    .custom /*0C000040:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000017*/<string> Microsoft.Extensions.Diagnostics.HealthChecks.HealthReportEntry/*0200000A*/::get_Tags() /* 0600002A */
  } // end of property HealthReportEntry::Tags
} // end of class Microsoft.Extensions.Diagnostics.HealthChecks.HealthReportEntry

.class /*0200000B*/ public auto ansi sealed Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus
       extends [System.Runtime/*23000001*/]System.Enum/*01000020*/
{
  .field /*04000010*/ public specialname rtspecialname int32 value__
  .field /*04000011*/ public static literal valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/ Unhealthy = int32(0x00000000)
  .field /*04000012*/ public static literal valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/ Degraded = int32(0x00000001)
  .field /*04000013*/ public static literal valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus/*0200000B*/ Healthy = int32(0x00000002)
} // end of class Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus

.class /*0200000C*/ interface public abstract auto ansi Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck
{
  .custom /*0C000035:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600002B*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000021*/<valuetype Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult/*02000008*/> 
          CheckHealthAsync(class Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckContext/*02000006*/ context,
                           [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000022*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 80 85 01 11 20 12 18 11 80 89
  {
    .param [2]/*08000035*/  = nullref
    // Method begins at RVA 0x0
  } // end of method IHealthCheck::CheckHealthAsync

} // end of class Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck

.class /*0200000D*/ interface public abstract auto ansi Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheckPublisher
{
  .custom /*0C000037:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600002C*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000023*/ 
          PublishAsync(class Microsoft.Extensions.Diagnostics.HealthChecks.HealthReport/*02000009*/ report,
                       valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000022*/ cancellationToken) cil managed
  // SIG: 20 02 12 80 8D 12 24 11 80 89
  {
    // Method begins at RVA 0x0
  } // end of method IHealthCheckPublisher::PublishAsync

} // end of class Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheckPublisher


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
