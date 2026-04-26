


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
// Time-date stamp:            0x97b37768
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00001c00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00003b06
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
// Checksum:                       0x00005a5c
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00003ab4 [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x00000488] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002600 [0x00002910] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00003a98 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00001b0c Virtual Size
//              0x00002000 Virtual Address
//              0x00001c00 Size of Raw Data
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
//              0x00000488 Virtual Size
//              0x00004000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00001e00 Pointer to Raw Data
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
//              0x00002400 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000b08
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00003af6 Import Name Table
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
// 0x00002120 [0x000018f8] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00003a18 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000908 Size
//              '#~' Name
//    Stream 2:
//              0x00000974 Offset
//              0x0000096c Size
//              '#Strings' Name
//    Stream 3:
//              0x000012e0 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x000012e4 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x000012f4 Offset
//              0x00000604 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000002090bb45f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 20240
// PE header size       : 512 (496 used)    ( 2.53%)
// PE additional info   : 11871             (58.65%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.36%)
// CLR meta-data size  : 6392               (31.58%)
// CLR additional info : 128                ( 0.63%)
// CLR method headers  : 74                 ( 0.37%)
// Managed code         : 129               ( 0.64%)
// Data                 : 2048              (10.12%)
// Unaccounted          : -986              (-4.87%)

// Num.of PE sections   : 3
//   .text    - 7168
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 6392
//   Module        -    1 (10 bytes)
//   TypeDef       -   10 (140 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   45 (270 bytes)
//   MethodDef     -   45 (630 bytes)     0 abstract, 0 native, 45 bodies
//   FieldDef      -    2 (12 bytes)      0 constant
//   MemberRef     -   32 (192 bytes)
//   ParamDef      -   40 (240 bytes)
//   MethodImpl    -    1 (6 bytes)
//   Constant      -    1 (6 bytes)
//   CustomAttribute-   75 (450 bytes)
//   InterfaceImpl -    1 (4 bytes)
//   PropertyMap   -    4 (16 bytes)
//   Property      -    9 (54 bytes)
//   MethodSemantic-   15 (90 bytes)
//   Security      -    1 (6 bytes)
//   TypeSpec      -    3 (6 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    1 (20 bytes)
//   NestedClass   -    1 (4 bytes)
//   EventMap      -    1 (4 bytes)
//   Event         -    3 (18 bytes)
//   Strings       -  2412 bytes
//   Blobs         -  1540 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   220 bytes

// CLR additional info : 128

// CLR method headers : 74
//   Num.of method bodies  - 45
//   Num.of fat headers    - 1
//   Num.of tiny headers   - 44
//   Num.of fat sections   - 0
//   Num.of small sections - 1

// Managed code : 129
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class AssemblyDependencyResolver     (public) (auto) (ansi) (sealed) 
// Class AssemblyLoadContext            (public) (auto) (ansi) 
// Class CreateNewOnMetadataUpdateAttribute (public) (auto) (ansi) (sealed) 
// Class MetadataUpdateDeletedAttribute (public) (auto) (ansi) (sealed) 
// Class MetadataUpdateOriginalTypeAttribute (public) (auto) (ansi) 
// Class AssemblyExtensions             (public) (abstract) (auto) (ansi) (sealed) 
// Class MetadataUpdater                (public) (abstract) (auto) (ansi) (sealed) 
// Class MetadataUpdateHandlerAttribute (public) (auto) (ansi) (sealed) 
// Class ContextualReflectionScope      (sequential) (ansi) (sealed) (nested public) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly /*20000001*/ noplatform System.Runtime.Loader
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000005:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000006:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000002*/::.ctor(int32) /* 0A000002 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000007:0A000003*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000003*/::.ctor() /* 0A000003 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000008:0A000004*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*//DebuggingModes/*01000005*/) /* 0A000004 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000009:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000006*/::.ctor(string) /* 0A000005 */ = ( 01 00 19 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 31 30 2E 30 01 00 54 0E   // ersion=v10.0..T.
                                                                                                                                                                           14 46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61   // .FrameworkDispla
                                                                                                                                                                           79 4E 61 6D 65 09 2E 4E 45 54 20 31 30 2E 30 )    // yName..NET 10.0
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000007*/::.ctor() /* 0A000006 */ = ( 01 00 00 00 ) 
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 15 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D   // ...System.Runtim
                                                                                                                                                                        65 2E 4C 6F 61 64 65 72 00 00 )                   // e.Loader..
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000F:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C000010:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0F 49 73 41 6F 74 43 6F 6D 70 61 74 69 62   // ...IsAotCompatib
                                                                                                                                                                    6C 65 04 54 72 75 65 00 00 )                      // le.True..
  .custom /*0C000011:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000012:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000013:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 15 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D   // ...System.Runtim
                                                                                                                                                                       65 2E 4C 6F 61 64 65 72 00 00 )                   // e.Loader..
  .custom /*0C000014:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0D 31 30 2E 30 2E 32 35 2E 35 32 34 31 31   // ...10.0.25.52411
                                                                                                                                                                       00 00 ) 
  .custom /*0C000015:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2F 31 30 2E 30 2E 30 2B 62 30 66 33 34 64   // ../10.0.0+b0f34d
                                                                                                                                                                                35 31 66 63 63 63 36 39 66 64 33 33 34 32 35 33   // 51fccc69fd334253
                                                                                                                                                                                39 32 34 61 62 64 38 64 36 38 35 33 66 61 64 37   // 924abd8d6853fad7
                                                                                                                                                                                61 61 00 00 )                                     // aa..
  .custom /*0C000016:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000017:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 15 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D   // ...System.Runtim
                                                                                                                                                                 65 2E 4C 6F 61 64 65 72 00 00 )                   // e.Loader..
  .custom /*0C000018:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    20 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   //  https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 64 6F 74 6E 65   // com/dotnet/dotne
                                                                                                                                                                    74 00 00 )                                        // t..
  .permissionset reqmin
             = {[System.Runtime/*23000001*/]System.Security.Permissions.SecurityPermissionAttribute/*01000013*/ = {property bool 'SkipVerification' = bool(true)}}
  .publickey = (00 24 00 00 04 80 00 00 94 00 00 00 06 02 00 00   // .$..............
                00 24 00 00 52 53 41 31 00 04 00 00 01 00 01 00   // .$..RSA1........
                07 D1 FA 57 C4 AE D9 F0 A3 2E 84 AA 0F AE FD 0D   // ...W............
                E9 E8 FD 6A EC 8F 87 FB 03 76 6C 83 4C 99 92 1E   // ...j.....vl.L...
                B2 3B E7 9A D9 D5 DC C1 DD 9A D2 36 13 21 02 90   // .;.........6.!..
                0B 72 3C F9 80 95 7F C4 E1 77 10 8F C6 07 77 4F   // .r<......w....wO
                29 E8 32 0E 92 EA 05 EC E4 E8 21 C0 A5 EF E8 F1   // ).2.......!.....
                64 5C 4C 0C 93 C1 AB 99 28 5D 62 2C AA 65 2C 1D   // d\L.....(]b,.e,.
                FA D6 3D 74 5D 6F 2D E5 F1 7E 5E AF 0F C4 96 3D   // ..=t]o-..~^....=
                26 1C 8A 12 43 65 18 20 6D C0 93 34 4D 5A D2 93 ) // &...Ce. m..4MZ..
  .hash algorithm 0x00008004
  .ver 10:0:0:0
}
.module System.Runtime.Loader.dll
// MVID: {c48feb7d-a360-4781-b865-7cc56654f03c}
.custom /*0C000001:0A000012*/ instance void [System.Runtime/*23000001*/]System.Security.UnverifiableCodeAttribute/*01000014*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
.custom /*0C000002:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000015*/::.ctor(int32) /* 0A000013 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000003:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000016*/::.ctor(bool) /* 0A000014 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi sealed beforefieldinit System.Runtime.Loader.AssemblyDependencyResolver
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000019:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00001B:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000019*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 61 6E 64 72 6F 69 64 00 00 )             // ...android..
  .custom /*0C00001C:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000019*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .custom /*0C00001D:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000019*/::.ctor(string) /* 0A000017 */ = ( 01 00 03 69 6F 73 00 00 )                         // ...ios..
  .custom /*0C00001E:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000019*/::.ctor(string) /* 0A000017 */ = ( 01 00 04 74 76 6F 73 00 00 )                      // ...tvos..
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string componentAssemblyPath) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AssemblyDependencyResolver::.ctor

  .method /*06000002*/ public hidebysig instance string 
          ResolveAssemblyToPath(class [System.Runtime/*23000001*/]System.Reflection.AssemblyName/*0100001B*/ assemblyName) cil managed
  // SIG: 20 01 0E 12 6D
  {
    .param [0]/*08000002*/ 
    .custom /*0C00001F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2058
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyDependencyResolver::ResolveAssemblyToPath

  .method /*06000003*/ public hidebysig instance string 
          ResolveUnmanagedDllToPath(string unmanagedDllName) cil managed
  // SIG: 20 01 0E 0E
  {
    .param [0]/*08000004*/ 
    .custom /*0C000026:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x205b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyDependencyResolver::ResolveUnmanagedDllToPath

} // end of class System.Runtime.Loader.AssemblyDependencyResolver

.class /*02000003*/ public auto ansi beforefieldinit System.Runtime.Loader.AssemblyLoadContext
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000021:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000022:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
  .class /*0200000A*/ sequential ansi sealed nested public beforefieldinit ContextualReflectionScope
         extends [System.Runtime/*23000001*/]System.ValueType/*01000024*/
         implements [System.Runtime/*23000001*/]System.IDisposable/*01000025*/
  {
    .custom /*0C000035:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 00 00 00 ) 
    .custom /*0C000036:0A00001B*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000023*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000022*/) /* 0A00001B */ = ( 01 00 01 00 00 00 00 00 ) 
    .field /*04000001*/ private object _dummy
    .field /*04000002*/ private int32 _dummyPrimitive
    .method /*0600002D*/ public hidebysig newslot virtual final 
            instance void  Dispose() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x211b
      // Code size       1 (0x1)
      .maxstack  8
      IL_0000:  /* 2A   |                  */ ret
    } // end of method ContextualReflectionScope::Dispose

  } // end of class ContextualReflectionScope

  .method /*06000004*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x205e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AssemblyLoadContext::.ctor

  .method /*06000005*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(bool isCollectible) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2066
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AssemblyLoadContext::.ctor

  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               [opt] bool isCollectible) cil managed
  // SIG: 20 02 01 0E 02
  {
    .custom /*0C000028:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000008*/  = bool(false)
    // Method begins at RVA 0x206e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AssemblyLoadContext::.ctor

  .method /*06000007*/ public hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class System.Runtime.Loader.AssemblyLoadContext/*02000003*/> 
          get_All() cil managed
  // SIG: 00 00 15 12 7D 01 12 0C
  {
    // Method begins at RVA 0x2076
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyLoadContext::get_All

  .method /*06000008*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/> 
          get_Assemblies() cil managed
  // SIG: 20 00 15 12 7D 01 12 75
  {
    // Method begins at RVA 0x2079
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyLoadContext::get_Assemblies

  .method /*06000009*/ public hidebysig specialname static 
          class System.Runtime.Loader.AssemblyLoadContext/*02000003*/ 
          get_CurrentContextualReflectionContext() cil managed
  // SIG: 00 00 12 0C
  {
    .custom /*0C00002F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x207c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyLoadContext::get_CurrentContextualReflectionContext

  .method /*0600000A*/ public hidebysig specialname static 
          class System.Runtime.Loader.AssemblyLoadContext/*02000003*/ 
          get_Default() cil managed
  // SIG: 00 00 12 0C
  {
    // Method begins at RVA 0x207f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyLoadContext::get_Default

  .method /*0600000B*/ public hidebysig specialname 
          instance bool  get_IsCollectible() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2082
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyLoadContext::get_IsCollectible

  .method /*0600000C*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000039:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2085
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyLoadContext::get_Name

  .method /*0600000D*/ public hidebysig specialname 
          instance void  add_Resolving(class [System.Runtime/*23000001*/]System.Func`3/*0100001C*/<class System.Runtime.Loader.AssemblyLoadContext/*02000003*/,class [System.Runtime/*23000001*/]System.Reflection.AssemblyName/*0100001B*/,class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/> 'value') cil managed
  // SIG: 20 01 01 15 12 71 03 12 0C 12 6D 12 75
  {
    .param [1]/*08000009*/ 
    .custom /*0C000033:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 04 00 00 00 02 01 01 02 00 00 ) 
    // Method begins at RVA 0x2088
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AssemblyLoadContext::add_Resolving

  .method /*0600000E*/ public hidebysig specialname 
          instance void  remove_Resolving(class [System.Runtime/*23000001*/]System.Func`3/*0100001C*/<class System.Runtime.Loader.AssemblyLoadContext/*02000003*/,class [System.Runtime/*23000001*/]System.Reflection.AssemblyName/*0100001B*/,class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/> 'value') cil managed
  // SIG: 20 01 01 15 12 71 03 12 0C 12 6D 12 75
  {
    .param [1]/*0800000A*/ 
    .custom /*0C000037:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 04 00 00 00 02 01 01 02 00 00 ) 
    // Method begins at RVA 0x208a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AssemblyLoadContext::remove_Resolving

  .method /*0600000F*/ public hidebysig specialname 
          instance void  add_ResolvingUnmanagedDll(class [System.Runtime/*23000001*/]System.Func`3/*0100001C*/<class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/,string,native int> 'value') cil managed
  // SIG: 20 01 01 15 12 71 03 12 75 0E 18
  {
    .param [1]/*0800000B*/ 
    .custom /*0C000038:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x208c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AssemblyLoadContext::add_ResolvingUnmanagedDll

  .method /*06000010*/ public hidebysig specialname 
          instance void  remove_ResolvingUnmanagedDll(class [System.Runtime/*23000001*/]System.Func`3/*0100001C*/<class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/,string,native int> 'value') cil managed
  // SIG: 20 01 01 15 12 71 03 12 75 0E 18
  {
    .param [1]/*0800000C*/ 
    .custom /*0C00003A:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x208e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AssemblyLoadContext::remove_ResolvingUnmanagedDll

  .method /*06000011*/ public hidebysig specialname 
          instance void  add_Unloading(class [System.Runtime/*23000001*/]System.Action`1/*0100001E*/<class System.Runtime.Loader.AssemblyLoadContext/*02000003*/> 'value') cil managed
  // SIG: 20 01 01 15 12 79 01 12 0C
  {
    .param [1]/*0800000D*/ 
    .custom /*0C00003B:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2090
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AssemblyLoadContext::add_Unloading

  .method /*06000012*/ public hidebysig specialname 
          instance void  remove_Unloading(class [System.Runtime/*23000001*/]System.Action`1/*0100001E*/<class System.Runtime.Loader.AssemblyLoadContext/*02000003*/> 'value') cil managed
  // SIG: 20 01 01 15 12 79 01 12 0C
  {
    .param [1]/*0800000E*/ 
    .custom /*0C00003C:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2092
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AssemblyLoadContext::remove_Unloading

  .method /*06000013*/ public hidebysig instance valuetype System.Runtime.Loader.AssemblyLoadContext/*02000003*//ContextualReflectionScope/*0200000A*/ 
          EnterContextualReflection() cil managed
  // SIG: 20 00 11 28
  {
    // Method begins at RVA 0x2094
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyLoadContext::EnterContextualReflection

  .method /*06000014*/ public hidebysig static 
          valuetype System.Runtime.Loader.AssemblyLoadContext/*02000003*//ContextualReflectionScope/*0200000A*/ 
          EnterContextualReflection(class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/ activating) cil managed
  // SIG: 00 01 11 28 12 75
  {
    .custom /*0C00003F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2097
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyLoadContext::EnterContextualReflection

  .method /*06000015*/ family hidebysig virtual 
          instance void  Finalize() cil managed
  // SIG: 20 00 01
  {
    .override [System.Runtime/*23000001*/]System.Object/*0100001A*/::Finalize /*0100001A::0A000018*/ 
    // Method begins at RVA 0x209c
    // Code size       10 (0xa)
    .maxstack  1
    .try
    {
      IL_0000:  /* DE   | 07               */ leave.s    IL_0009

    }  // end .try
    finally
    {
      IL_0002:  /* 02   |                  */ ldarg.0
      IL_0003:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001A*/::Finalize() /* 0A000018 */
      IL_0008:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 00 00 02 02 00 07 00 00 00 00
    IL_0009:  /* 2A   |                  */ ret
  } // end of method AssemblyLoadContext::Finalize

  .method /*06000016*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.AssemblyName/*0100001B*/ 
          GetAssemblyName(string assemblyPath) cil managed
  // SIG: 00 01 12 6D 0E
  {
    // Method begins at RVA 0x20c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyLoadContext::GetAssemblyName

  .method /*06000017*/ public hidebysig static 
          class System.Runtime.Loader.AssemblyLoadContext/*02000003*/ 
          GetLoadContext(class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/ 'assembly') cil managed
  // SIG: 00 01 12 0C 12 75
  {
    .param [0]/*08000011*/ 
    .custom /*0C00003D:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyLoadContext::GetLoadContext

  .method /*06000018*/ family hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/ 
          Load(class [System.Runtime/*23000001*/]System.Reflection.AssemblyName/*0100001B*/ assemblyName) cil managed
  // SIG: 20 01 12 75 12 6D
  {
    .param [0]/*08000013*/ 
    .custom /*0C00003E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyLoadContext::Load

  .method /*06000019*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/ 
          LoadFromAssemblyName(class [System.Runtime/*23000001*/]System.Reflection.AssemblyName/*0100001B*/ assemblyName) cil managed
  // SIG: 20 01 12 75 12 6D
  {
    // Method begins at RVA 0x20cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyLoadContext::LoadFromAssemblyName

  .method /*0600001A*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/ 
          LoadFromAssemblyPath(string assemblyPath) cil managed
  // SIG: 20 01 12 75 0E
  {
    .custom /*0C000041:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A00001A */ = ( 01 00 41 54 79 70 65 73 20 61 6E 64 20 6D 65 6D   // ..ATypes and mem
                                                                                                                                                                                            62 65 72 73 20 74 68 65 20 6C 6F 61 64 65 64 20   // bers the loaded 
                                                                                                                                                                                            61 73 73 65 6D 62 6C 79 20 64 65 70 65 6E 64 73   // assembly depends
                                                                                                                                                                                            20 6F 6E 20 6D 69 67 68 74 20 62 65 20 72 65 6D   //  on might be rem
                                                                                                                                                                                            6F 76 65 64 00 00 )                               // oved..
    // Method begins at RVA 0x20d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyLoadContext::LoadFromAssemblyPath

  .method /*0600001B*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/ 
          LoadFromNativeImagePath(string nativeImagePath,
                                  string assemblyPath) cil managed
  // SIG: 20 02 12 75 0E 0E
  {
    .custom /*0C000042:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A00001A */ = ( 01 00 41 54 79 70 65 73 20 61 6E 64 20 6D 65 6D   // ..ATypes and mem
                                                                                                                                                                                            62 65 72 73 20 74 68 65 20 6C 6F 61 64 65 64 20   // bers the loaded 
                                                                                                                                                                                            61 73 73 65 6D 62 6C 79 20 64 65 70 65 6E 64 73   // assembly depends
                                                                                                                                                                                            20 6F 6E 20 6D 69 67 68 74 20 62 65 20 72 65 6D   //  on might be rem
                                                                                                                                                                                            6F 76 65 64 00 00 )                               // oved..
    .param [2]/*08000018*/ 
    .custom /*0C000040:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyLoadContext::LoadFromNativeImagePath

  .method /*0600001C*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/ 
          LoadFromStream(class [System.Runtime/*23000001*/]System.IO.Stream/*01000021*/ 'assembly') cil managed
  // SIG: 20 01 12 75 12 80 85
  {
    .custom /*0C000044:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A00001A */ = ( 01 00 41 54 79 70 65 73 20 61 6E 64 20 6D 65 6D   // ..ATypes and mem
                                                                                                                                                                                            62 65 72 73 20 74 68 65 20 6C 6F 61 64 65 64 20   // bers the loaded 
                                                                                                                                                                                            61 73 73 65 6D 62 6C 79 20 64 65 70 65 6E 64 73   // assembly depends
                                                                                                                                                                                            20 6F 6E 20 6D 69 67 68 74 20 62 65 20 72 65 6D   //  on might be rem
                                                                                                                                                                                            6F 76 65 64 00 00 )                               // oved..
    // Method begins at RVA 0x20d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyLoadContext::LoadFromStream

  .method /*0600001D*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/ 
          LoadFromStream(class [System.Runtime/*23000001*/]System.IO.Stream/*01000021*/ 'assembly',
                         class [System.Runtime/*23000001*/]System.IO.Stream/*01000021*/ assemblySymbols) cil managed
  // SIG: 20 02 12 75 12 80 85 12 80 85
  {
    .custom /*0C000045:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A00001A */ = ( 01 00 41 54 79 70 65 73 20 61 6E 64 20 6D 65 6D   // ..ATypes and mem
                                                                                                                                                                                            62 65 72 73 20 74 68 65 20 6C 6F 61 64 65 64 20   // bers the loaded 
                                                                                                                                                                                            61 73 73 65 6D 62 6C 79 20 64 65 70 65 6E 64 73   // assembly depends
                                                                                                                                                                                            20 6F 6E 20 6D 69 67 68 74 20 62 65 20 72 65 6D   //  on might be rem
                                                                                                                                                                                            6F 76 65 64 00 00 )                               // oved..
    .param [2]/*0800001B*/ 
    .custom /*0C000043:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyLoadContext::LoadFromStream

  .method /*0600001E*/ family hidebysig newslot virtual 
          instance native int  LoadUnmanagedDll(string unmanagedDllName) cil managed
  // SIG: 20 01 18 0E
  {
    // Method begins at RVA 0x20dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyLoadContext::LoadUnmanagedDll

  .method /*0600001F*/ family hidebysig instance native int 
          LoadUnmanagedDllFromPath(string unmanagedDllPath) cil managed
  // SIG: 20 01 18 0E
  {
    // Method begins at RVA 0x20df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyLoadContext::LoadUnmanagedDllFromPath

  .method /*06000020*/ public hidebysig instance void 
          SetProfileOptimizationRoot(string directoryPath) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20e2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AssemblyLoadContext::SetProfileOptimizationRoot

  .method /*06000021*/ public hidebysig instance void 
          StartProfileOptimization(string profile) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000046:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AssemblyLoadContext::StartProfileOptimization

  .method /*06000022*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyLoadContext::ToString

  .method /*06000023*/ public hidebysig instance void 
          Unload() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20e9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AssemblyLoadContext::Unload

  .event /*14000001*/ class [System.Runtime/*23000001*/]System.Func`3/*0100001C*/<class System.Runtime.Loader.AssemblyLoadContext/*02000003*/,class [System.Runtime/*23000001*/]System.Reflection.AssemblyName/*0100001B*/,class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/>/*1B000001*/ Resolving
  {
    .custom /*0C000004:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 04 00 00 00 02 01 01 02 00 00 ) 
    .addon instance void System.Runtime.Loader.AssemblyLoadContext/*02000003*/::add_Resolving(class [System.Runtime/*23000001*/]System.Func`3/*0100001C*/<class System.Runtime.Loader.AssemblyLoadContext/*02000003*/,class [System.Runtime/*23000001*/]System.Reflection.AssemblyName/*0100001B*/,class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/>) /* 0600000D */
    .removeon instance void System.Runtime.Loader.AssemblyLoadContext/*02000003*/::remove_Resolving(class [System.Runtime/*23000001*/]System.Func`3/*0100001C*/<class System.Runtime.Loader.AssemblyLoadContext/*02000003*/,class [System.Runtime/*23000001*/]System.Reflection.AssemblyName/*0100001B*/,class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/>) /* 0600000E */
  } // end of event AssemblyLoadContext::Resolving
  .event /*14000002*/ class [System.Runtime/*23000001*/]System.Func`3/*0100001C*/<class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/,string,native int>/*1B000002*/ ResolvingUnmanagedDll
  {
    .custom /*0C000020:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .addon instance void System.Runtime.Loader.AssemblyLoadContext/*02000003*/::add_ResolvingUnmanagedDll(class [System.Runtime/*23000001*/]System.Func`3/*0100001C*/<class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/,string,native int>) /* 0600000F */
    .removeon instance void System.Runtime.Loader.AssemblyLoadContext/*02000003*/::remove_ResolvingUnmanagedDll(class [System.Runtime/*23000001*/]System.Func`3/*0100001C*/<class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/,string,native int>) /* 06000010 */
  } // end of event AssemblyLoadContext::ResolvingUnmanagedDll
  .event /*14000003*/ class [System.Runtime/*23000001*/]System.Action`1/*0100001E*/<class System.Runtime.Loader.AssemblyLoadContext/*02000003*/>/*1B000003*/ Unloading
  {
    .custom /*0C000024:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8[]) /* 0A000019 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .addon instance void System.Runtime.Loader.AssemblyLoadContext/*02000003*/::add_Unloading(class [System.Runtime/*23000001*/]System.Action`1/*0100001E*/<class System.Runtime.Loader.AssemblyLoadContext/*02000003*/>) /* 06000011 */
    .removeon instance void System.Runtime.Loader.AssemblyLoadContext/*02000003*/::remove_Unloading(class [System.Runtime/*23000001*/]System.Action`1/*0100001E*/<class System.Runtime.Loader.AssemblyLoadContext/*02000003*/>) /* 06000012 */
  } // end of event AssemblyLoadContext::Unloading
  .property /*17000001*/ callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class System.Runtime.Loader.AssemblyLoadContext/*02000003*/>
          All()
  {
    .get class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class System.Runtime.Loader.AssemblyLoadContext/*02000003*/> System.Runtime.Loader.AssemblyLoadContext/*02000003*/::get_All() /* 06000007 */
  } // end of property AssemblyLoadContext::All
  .property /*17000002*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/>
          Assemblies()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/> System.Runtime.Loader.AssemblyLoadContext/*02000003*/::get_Assemblies() /* 06000008 */
  } // end of property AssemblyLoadContext::Assemblies
  .property /*17000003*/ callconv(8) class System.Runtime.Loader.AssemblyLoadContext/*02000003*/
          CurrentContextualReflectionContext()
  {
    .custom /*0C000023:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .get class System.Runtime.Loader.AssemblyLoadContext/*02000003*/ System.Runtime.Loader.AssemblyLoadContext/*02000003*/::get_CurrentContextualReflectionContext() /* 06000009 */
  } // end of property AssemblyLoadContext::CurrentContextualReflectionContext
  .property /*17000004*/ callconv(8) class System.Runtime.Loader.AssemblyLoadContext/*02000003*/
          Default()
  {
    .get class System.Runtime.Loader.AssemblyLoadContext/*02000003*/ System.Runtime.Loader.AssemblyLoadContext/*02000003*/::get_Default() /* 0600000A */
  } // end of property AssemblyLoadContext::Default
  .property /*17000005*/ instance callconv(8) bool
          IsCollectible()
  {
    .get instance bool System.Runtime.Loader.AssemblyLoadContext/*02000003*/::get_IsCollectible() /* 0600000B */
  } // end of property AssemblyLoadContext::IsCollectible
  .property /*17000006*/ instance callconv(8) string
          Name()
  {
    .custom /*0C00002C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Runtime.Loader.AssemblyLoadContext/*02000003*/::get_Name() /* 0600000C */
  } // end of property AssemblyLoadContext::Name
} // end of class System.Runtime.Loader.AssemblyLoadContext

.class /*02000004*/ public auto ansi sealed beforefieldinit System.Runtime.CompilerServices.CreateNewOnMetadataUpdateAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000028*/
{
  .custom /*0C000025:0A00001C*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000027*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000026*/) /* 0A00001C */ = ( 01 00 0C 00 00 00 01 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 )                      // Multiple.
  .method /*06000024*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20eb
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000028*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CreateNewOnMetadataUpdateAttribute::.ctor

} // end of class System.Runtime.CompilerServices.CreateNewOnMetadataUpdateAttribute

.class /*02000005*/ public auto ansi sealed beforefieldinit System.Runtime.CompilerServices.MetadataUpdateDeletedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000028*/
{
  .custom /*0C000027:0A00001C*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000027*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000026*/) /* 0A00001C */ = ( 01 00 FF 7F 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .method /*06000025*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20f3
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000028*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MetadataUpdateDeletedAttribute::.ctor

} // end of class System.Runtime.CompilerServices.MetadataUpdateDeletedAttribute

.class /*02000006*/ public auto ansi beforefieldinit System.Runtime.CompilerServices.MetadataUpdateOriginalTypeAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000028*/
{
  .custom /*0C000029:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00002B:0A00001C*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000027*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000026*/) /* 0A00001C */ = ( 01 00 0C 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .method /*06000026*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*01000029*/ originalType) cil managed
  // SIG: 20 01 01 12 80 A5
  {
    // Method begins at RVA 0x20fb
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000028*/::.ctor() /* 0A000020 */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method MetadataUpdateOriginalTypeAttribute::.ctor

  .method /*06000027*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000029*/ 
          get_OriginalType() cil managed
  // SIG: 20 00 12 80 A5
  {
    // Method begins at RVA 0x2104
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MetadataUpdateOriginalTypeAttribute::get_OriginalType

  .property /*17000007*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000029*/
          OriginalType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000029*/ System.Runtime.CompilerServices.MetadataUpdateOriginalTypeAttribute/*02000006*/::get_OriginalType() /* 06000027 */
  } // end of property MetadataUpdateOriginalTypeAttribute::OriginalType
} // end of class System.Runtime.CompilerServices.MetadataUpdateOriginalTypeAttribute

.class /*02000007*/ public abstract auto ansi sealed beforefieldinit System.Reflection.Metadata.AssemblyExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C00002D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000028*/ public hidebysig static 
          bool  TryGetRawMetadata(class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/ 'assembly',
                                  [out] uint8*& 'blob',
                                  [out] int32& length) cil managed
  // SIG: 00 03 02 12 75 10 0F 05 10 08
  {
    .custom /*0C000049:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00004A:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000021*/ 
    .custom /*0C000047:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2107
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyExtensions::TryGetRawMetadata

} // end of class System.Reflection.Metadata.AssemblyExtensions

.class /*02000008*/ public abstract auto ansi sealed beforefieldinit System.Reflection.Metadata.MetadataUpdater
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .method /*06000029*/ public hidebysig static 
          void  ApplyUpdate(class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001D*/ 'assembly',
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002A*/<uint8> metadataDelta,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002A*/<uint8> ilDelta,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002A*/<uint8> pdbDelta) cil managed
  // SIG: 00 04 01 12 75 15 11 80 A9 01 05 15 11 80 A9 01 05 15 11 80 A9 01 05
  {
    .param [1]/*08000024*/ 
    .custom /*0C000048:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x210a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MetadataUpdater::ApplyUpdate

  .method /*0600002A*/ public hidebysig specialname static 
          bool  get_IsSupported() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x210d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MetadataUpdater::get_IsSupported

  .property /*17000008*/ callconv(8) bool
          IsSupported()
  {
    .custom /*0C00002E:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.FeatureSwitchDefinitionAttribute/*0100002B*/::.ctor(string) /* 0A00001D */ = ( 01 00 36 53 79 73 74 65 6D 2E 52 65 66 6C 65 63   // ..6System.Reflec
                                                                                                                                                                                           74 69 6F 6E 2E 4D 65 74 61 64 61 74 61 2E 4D 65   // tion.Metadata.Me
                                                                                                                                                                                           74 61 64 61 74 61 55 70 64 61 74 65 72 2E 49 73   // tadataUpdater.Is
                                                                                                                                                                                           53 75 70 70 6F 72 74 65 64 00 00 )                // Supported..
    .get bool System.Reflection.Metadata.MetadataUpdater/*02000008*/::get_IsSupported() /* 0600002A */
  } // end of property MetadataUpdater::IsSupported
} // end of class System.Reflection.Metadata.MetadataUpdater

.class /*02000009*/ public auto ansi sealed beforefieldinit System.Reflection.Metadata.MetadataUpdateHandlerAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000028*/
{
  .custom /*0C000030:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000017*/::.ctor(uint8) /* 0A000015 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000031:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000018*/::.ctor(uint8) /* 0A000016 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000032:0A00001C*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000027*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000026*/) /* 0A00001C */ = ( 01 00 01 00 00 00 01 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 01 )                      // Multiple.
  .method /*0600002B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*01000029*/ handlerType) cil managed
  // SIG: 20 01 01 12 80 A5
  {
    .param [1]/*08000028*/ 
    .custom /*0C00004B:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100002D*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100002C*/) /* 0A00001E */ = ( 01 00 18 00 00 00 00 00 ) 
    // Method begins at RVA 0x2110
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000028*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MetadataUpdateHandlerAttribute::.ctor

  .method /*0600002C*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000029*/ 
          get_HandlerType() cil managed
  // SIG: 20 00 12 80 A5
  {
    // Method begins at RVA 0x2118
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MetadataUpdateHandlerAttribute::get_HandlerType

  .property /*17000009*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000029*/
          HandlerType()
  {
    .custom /*0C000034:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100002D*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100002C*/) /* 0A00001E */ = ( 01 00 18 00 00 00 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000029*/ System.Reflection.Metadata.MetadataUpdateHandlerAttribute/*02000009*/::get_HandlerType() /* 0600002C */
  } // end of property MetadataUpdateHandlerAttribute::HandlerType
} // end of class System.Reflection.Metadata.MetadataUpdateHandlerAttribute


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
