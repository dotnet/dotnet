


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
// Time-date stamp:            0xc78a92ad
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
// Addr. of entry point:           0x00003bb6
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
// Checksum:                       0x000082e1
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00003b61 [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x00000504] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002600 [0x00002470] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00003a5c [0x00000054] address [size] of Debug Directory:           
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
//              0x00001bbc Virtual Size
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
//              0x00000504 Virtual Size
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
//              Entry 1: Type 0x3 Offset 0x00000bb8
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00003ba3 Import Name Table
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
// 0x000020f4 [0x000018e8] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x000039dc [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000948 Size
//              '#~' Name
//    Stream 2:
//              0x000009b4 Offset
//              0x00000848 Size
//              '#Strings' Name
//    Stream 3:
//              0x000011fc Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00001200 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001210 Offset
//              0x000006d8 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000140901a01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 19056
// PE header size       : 512 (496 used)    ( 2.69%)
// PE additional info   : 10867             (57.03%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.38%)
// CLR meta-data size  : 6376               (33.46%)
// CLR additional info : 128                ( 0.67%)
// CLR method headers  : 47                 ( 0.25%)
// Managed code         : 114               ( 0.60%)
// Data                 : 2048              (10.75%)
// Unaccounted          : -1108             (-5.81%)

// Num.of PE sections   : 3
//   .text    - 7168
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 6376
//   Module        -    1 (10 bytes)
//   TypeDef       -   11 (154 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   40 (240 bytes)
//   MethodDef     -   48 (672 bytes)     1 abstract, 0 native, 47 bodies
//   FieldDef      -   10 (60 bytes)      0 constant
//   MemberRef     -   23 (138 bytes)
//   ParamDef      -   37 (222 bytes)
//   Constant      -    8 (48 bytes)
//   CustomAttribute-   47 (282 bytes)
//   InterfaceImpl -    3 (12 bytes)
//   PropertyMap   -    5 (20 bytes)
//   Property      -   14 (84 bytes)
//   MethodSemantic-   27 (162 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    5 (100 bytes)
//   GenericParam  -    5 (40 bytes)
//   GenericParamConstraint-    4 (16 bytes)
//   Strings       -  2118 bytes
//   Blobs         -  1752 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   204 bytes

// CLR additional info : 128

// CLR method headers : 47
//   Num.of method bodies  - 47
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 47

// Managed code : 114
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class ConsoleLoggerExtensions        (public) (abstract) (auto) (ansi) (sealed) 
// Class ConsoleFormatter               (public) (abstract) (auto) (ansi) 
// Class ConsoleFormatterNames          (public) (abstract) (auto) (ansi) (sealed) 
// Class ConsoleFormatterOptions        (public) (auto) (ansi) 
// Class ConsoleLoggerFormat            (public) (auto) (ansi) (sealed) 
// Class ConsoleLoggerOptions           (public) (auto) (ansi) 
// Class ConsoleLoggerProvider          (public) (auto) (ansi) 
// Class JsonConsoleFormatterOptions    (public) (auto) (ansi) 
// Class LoggerColorBehavior            (public) (auto) (ansi) (sealed) 
// Class SimpleConsoleFormatterOptions  (public) (auto) (ansi) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ Microsoft.Extensions.Logging
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ Microsoft.Extensions.Logging.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ Microsoft.Extensions.Options
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000005*/ System.Text.Json
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 6:0:0:0
}
.assembly /*20000001*/ noplatform Microsoft.Extensions.Logging.Console
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

  .custom /*0C000006:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000006*/::.ctor(string) /* 0A000005 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C000007:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000007*/::.ctor() /* 0A000006 */ = ( 01 00 00 00 ) 
  .custom /*0C000008:0A000007*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000008*/::.ctor(bool) /* 0A000007 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000009:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 24 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..$Microsoft.Ext
                                                                                                                                                                        65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                        43 6F 6E 73 6F 6C 65 00 00 )                      // Console..
  .custom /*0C00000A:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000B:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000E:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000F:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000010:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 24 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..$Microsoft.Ext
                                                                                                                                                                       65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                       43 6F 6E 73 6F 6C 65 00 00 )                      // Console..
  .custom /*0C000011:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 36 2E 30 2E 32 32 32 2E 36 34 30 36 00   // ...6.0.222.6406.
                                                                                                                                                                       00 ) 
  .custom /*0C000012:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 36 2E 30 2E 32 2B 38 33 39 63 64 66 62   // ...6.0.2+839cdfb
                                                                                                                                                                                30 65 63 63 61 35 65 30 62 65 33 64 62 63 63 64   // 0ecca5e0be3dbccd
                                                                                                                                                                                39 32 36 65 37 36 35 31 65 66 35 30 66 64 66 31   // 926e7651ef50fdf1
                                                                                                                                                                                30 00 00 )                                        // 0..
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000014:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 24 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..$Microsoft.Ext
                                                                                                                                                                 65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                 43 6F 6E 73 6F 6C 65 00 00 )                      // Console..
  .custom /*0C000015:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    21 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // !https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 72 75 6E 74 69   // com/dotnet/runti
                                                                                                                                                                    6D 65 00 00 )                                     // me..
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
.module Microsoft.Extensions.Logging.Console.dll
// MVID: {c89a987d-e9de-4bb9-901e-a2ae082002bc}
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public abstract auto ansi sealed beforefieldinit Microsoft.Extensions.Logging.ConsoleLoggerExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .custom /*0C000018:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000019:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000012*/::.ctor(string) /* 0A000011 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*06000001*/ public hidebysig static 
          class [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000014*/ 
          AddConsole(class [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000014*/ builder) cil managed
  // SIG: 00 01 12 51 12 51
  {
    .custom /*0C000001:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLoggerExtensions::AddConsole

  .method /*06000002*/ public hidebysig static 
          class [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000014*/ 
          AddConsole(class [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000014*/ builder,
                     class [System.Runtime/*23000001*/]System.Action`1/*01000015*/<class Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions/*02000007*/> configure) cil managed
  // SIG: 00 02 12 51 12 51 15 12 55 01 12 1C
  {
    .custom /*0C000017:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2053
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLoggerExtensions::AddConsole

  .method /*06000003*/ public hidebysig static 
          class [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000014*/ 
          AddConsoleFormatter<(Microsoft.Extensions.Logging.Console.ConsoleFormatter/*02000003*/) TFormatter,(Microsoft.Extensions.Logging.Console.ConsoleFormatterOptions/*02000005*/) TOptions>(class [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000014*/ builder) cil managed
  // SIG: 10 02 01 12 51 12 51
  {
    .custom /*0C00001B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00001C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000016*/::.ctor(string) /* 0A000012 */ = ( 01 00 65 54 4F 70 74 69 6F 6E 73 27 73 20 64 65   // ..eTOptions's de
                                                                                                                                                                                            70 65 6E 64 65 6E 74 20 74 79 70 65 73 20 6D 61   // pendent types ma
                                                                                                                                                                                            79 20 68 61 76 65 20 74 68 65 69 72 20 6D 65 6D   // y have their mem
                                                                                                                                                                                            62 65 72 73 20 74 72 69 6D 6D 65 64 2E 20 45 6E   // bers trimmed. En
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 72 65 71 75 69 72 65   // sure all require
                                                                                                                                                                                            64 20 6D 65 6D 62 65 72 73 20 61 72 65 20 70 72   // d members are pr
                                                                                                                                                                                            65 73 65 72 76 65 64 2E 00 00 )                   // eserved...
    .param type TFormatter /*2A000001*/ 
      .custom /*0C000016:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000017*/) /* 0A000013 */ = ( 01 00 03 00 00 00 00 00 ) 
    .param type TOptions /*2A000002*/ 
      .custom /*0C00001A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000017*/) /* 0A000013 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x2056
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLoggerExtensions::AddConsoleFormatter

  .method /*06000004*/ public hidebysig static 
          class [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000014*/ 
          AddConsoleFormatter<(Microsoft.Extensions.Logging.Console.ConsoleFormatter/*02000003*/) TFormatter,(Microsoft.Extensions.Logging.Console.ConsoleFormatterOptions/*02000005*/) TOptions>(class [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000014*/ builder,
                                                                                                                                                                                                  class [System.Runtime/*23000001*/]System.Action`1/*01000015*/<!!TOptions> configure) cil managed
  // SIG: 10 02 02 12 51 12 51 15 12 55 01 1E 01
  {
    .custom /*0C00001E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00001F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000016*/::.ctor(string) /* 0A000012 */ = ( 01 00 65 54 4F 70 74 69 6F 6E 73 27 73 20 64 65   // ..eTOptions's de
                                                                                                                                                                                            70 65 6E 64 65 6E 74 20 74 79 70 65 73 20 6D 61   // pendent types ma
                                                                                                                                                                                            79 20 68 61 76 65 20 74 68 65 69 72 20 6D 65 6D   // y have their mem
                                                                                                                                                                                            62 65 72 73 20 74 72 69 6D 6D 65 64 2E 20 45 6E   // bers trimmed. En
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 72 65 71 75 69 72 65   // sure all require
                                                                                                                                                                                            64 20 6D 65 6D 62 65 72 73 20 61 72 65 20 70 72   // d members are pr
                                                                                                                                                                                            65 73 65 72 76 65 64 2E 00 00 )                   // eserved...
    .param type TFormatter /*2A000003*/ 
      .custom /*0C00001D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000017*/) /* 0A000013 */ = ( 01 00 03 00 00 00 00 00 ) 
    .param type TOptions /*2A000004*/ 
      .custom /*0C000020:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000017*/) /* 0A000013 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x2059
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLoggerExtensions::AddConsoleFormatter

  .method /*06000005*/ public hidebysig static 
          class [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000014*/ 
          AddJsonConsole(class [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000014*/ builder) cil managed
  // SIG: 00 01 12 51 12 51
  {
    .custom /*0C000021:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x205c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLoggerExtensions::AddJsonConsole

  .method /*06000006*/ public hidebysig static 
          class [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000014*/ 
          AddJsonConsole(class [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000014*/ builder,
                         class [System.Runtime/*23000001*/]System.Action`1/*01000015*/<class Microsoft.Extensions.Logging.Console.JsonConsoleFormatterOptions/*02000009*/> configure) cil managed
  // SIG: 00 02 12 51 12 51 15 12 55 01 12 24
  {
    .custom /*0C000023:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x205f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLoggerExtensions::AddJsonConsole

  .method /*06000007*/ public hidebysig static 
          class [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000014*/ 
          AddSimpleConsole(class [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000014*/ builder) cil managed
  // SIG: 00 01 12 51 12 51
  {
    .custom /*0C000026:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2062
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLoggerExtensions::AddSimpleConsole

  .method /*06000008*/ public hidebysig static 
          class [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000014*/ 
          AddSimpleConsole(class [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000014*/ builder,
                           class [System.Runtime/*23000001*/]System.Action`1/*01000015*/<class Microsoft.Extensions.Logging.Console.SimpleConsoleFormatterOptions/*0200000B*/> configure) cil managed
  // SIG: 00 02 12 51 12 51 15 12 55 01 12 2C
  {
    .custom /*0C000027:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2065
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLoggerExtensions::AddSimpleConsole

  .method /*06000009*/ public hidebysig static 
          class [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000014*/ 
          AddSystemdConsole(class [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000014*/ builder) cil managed
  // SIG: 00 01 12 51 12 51
  {
    .custom /*0C00002B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2068
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLoggerExtensions::AddSystemdConsole

  .method /*0600000A*/ public hidebysig static 
          class [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000014*/ 
          AddSystemdConsole(class [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000014*/ builder,
                            class [System.Runtime/*23000001*/]System.Action`1/*01000015*/<class Microsoft.Extensions.Logging.Console.ConsoleFormatterOptions/*02000005*/> configure) cil managed
  // SIG: 00 02 12 51 12 51 15 12 55 01 12 14
  {
    .custom /*0C00002C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x206b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLoggerExtensions::AddSystemdConsole

} // end of class Microsoft.Extensions.Logging.ConsoleLoggerExtensions

.class /*02000003*/ public abstract auto ansi beforefieldinit Microsoft.Extensions.Logging.Console.ConsoleFormatter
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .method /*0600000B*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x206e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ConsoleFormatter::.ctor

  .method /*0600000C*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2076
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleFormatter::get_Name

  .method /*0600000D*/ public hidebysig newslot abstract virtual 
          instance void  Write<TState>([in] valuetype [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.Abstractions.LogEntry`1/*0100001B*/<!!TState>& modreq([System.Runtime/*23000001*/]System.Runtime.InteropServices.InAttribute/*0100001A*/) logEntry,
                                       class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.IExternalScopeProvider/*0100001C*/ scopeProvider,
                                       class [System.Runtime/*23000001*/]System.IO.TextWriter/*0100001D*/ textWriter) cil managed
  // SIG: 30 01 03 01 1F 69 10 15 11 6D 01 1E 00 12 71 12 75
  {
    .param [1]/*08000011*/ 
    .custom /*0C00002F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ConsoleFormatter::Write

  .property /*17000001*/ instance callconv(8) string
          Name()
  {
    .get instance string Microsoft.Extensions.Logging.Console.ConsoleFormatter/*02000003*/::get_Name() /* 0600000C */
  } // end of property ConsoleFormatter::Name
} // end of class Microsoft.Extensions.Logging.Console.ConsoleFormatter

.class /*02000004*/ public abstract auto ansi sealed beforefieldinit Microsoft.Extensions.Logging.Console.ConsoleFormatterNames
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .field /*04000001*/ public static literal string Json = "json"
  .field /*04000002*/ public static literal string Simple = "simple"
  .field /*04000003*/ public static literal string Systemd = "systemd"
} // end of class Microsoft.Extensions.Logging.Console.ConsoleFormatterNames

.class /*02000005*/ public auto ansi beforefieldinit Microsoft.Extensions.Logging.Console.ConsoleFormatterOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .method /*0600000E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2079
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ConsoleFormatterOptions::.ctor

  .method /*0600000F*/ public hidebysig specialname 
          instance bool  get_IncludeScopes() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2081
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleFormatterOptions::get_IncludeScopes

  .method /*06000010*/ public hidebysig specialname 
          instance void  set_IncludeScopes(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2084
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConsoleFormatterOptions::set_IncludeScopes

  .method /*06000011*/ public hidebysig specialname 
          instance string  get_TimestampFormat() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2086
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleFormatterOptions::get_TimestampFormat

  .method /*06000012*/ public hidebysig specialname 
          instance void  set_TimestampFormat(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2089
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConsoleFormatterOptions::set_TimestampFormat

  .method /*06000013*/ public hidebysig specialname 
          instance bool  get_UseUtcTimestamp() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x208b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleFormatterOptions::get_UseUtcTimestamp

  .method /*06000014*/ public hidebysig specialname 
          instance void  set_UseUtcTimestamp(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x208e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConsoleFormatterOptions::set_UseUtcTimestamp

  .property /*17000002*/ instance callconv(8) bool
          IncludeScopes()
  {
    .get instance bool Microsoft.Extensions.Logging.Console.ConsoleFormatterOptions/*02000005*/::get_IncludeScopes() /* 0600000F */
    .set instance void Microsoft.Extensions.Logging.Console.ConsoleFormatterOptions/*02000005*/::set_IncludeScopes(bool) /* 06000010 */
  } // end of property ConsoleFormatterOptions::IncludeScopes
  .property /*17000003*/ instance callconv(8) string
          TimestampFormat()
  {
    .get instance string Microsoft.Extensions.Logging.Console.ConsoleFormatterOptions/*02000005*/::get_TimestampFormat() /* 06000011 */
    .set instance void Microsoft.Extensions.Logging.Console.ConsoleFormatterOptions/*02000005*/::set_TimestampFormat(string) /* 06000012 */
  } // end of property ConsoleFormatterOptions::TimestampFormat
  .property /*17000004*/ instance callconv(8) bool
          UseUtcTimestamp()
  {
    .get instance bool Microsoft.Extensions.Logging.Console.ConsoleFormatterOptions/*02000005*/::get_UseUtcTimestamp() /* 06000013 */
    .set instance void Microsoft.Extensions.Logging.Console.ConsoleFormatterOptions/*02000005*/::set_UseUtcTimestamp(bool) /* 06000014 */
  } // end of property ConsoleFormatterOptions::UseUtcTimestamp
} // end of class Microsoft.Extensions.Logging.Console.ConsoleFormatterOptions

.class /*02000006*/ public auto ansi sealed Microsoft.Extensions.Logging.Console.ConsoleLoggerFormat
       extends [System.Runtime/*23000001*/]System.Enum/*0100001F*/
{
  .custom /*0C000024:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 28 43 6F 6E 73 6F 6C 65 4C 6F 67 67 65 72   // ..(ConsoleLogger
                                                                                                                                                 46 6F 72 6D 61 74 20 68 61 73 20 62 65 65 6E 20   // Format has been 
                                                                                                                                                 64 65 70 72 65 63 61 74 65 64 2E 00 00 )          // deprecated...
  .field /*04000004*/ public specialname rtspecialname int32 value__
  .field /*04000005*/ public static literal valuetype Microsoft.Extensions.Logging.Console.ConsoleLoggerFormat/*02000006*/ Default = int32(0x00000000)
  .field /*04000006*/ public static literal valuetype Microsoft.Extensions.Logging.Console.ConsoleLoggerFormat/*02000006*/ Systemd = int32(0x00000001)
} // end of class Microsoft.Extensions.Logging.Console.ConsoleLoggerFormat

.class /*02000007*/ public auto ansi beforefieldinit Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
{
  .method /*06000015*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2090
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ConsoleLoggerOptions::.ctor

  .method /*06000016*/ public hidebysig specialname 
          instance bool  get_DisableColors() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2098
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLoggerOptions::get_DisableColors

  .method /*06000017*/ public hidebysig specialname 
          instance void  set_DisableColors(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x209b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConsoleLoggerOptions::set_DisableColors

  .method /*06000018*/ public hidebysig specialname 
          instance valuetype Microsoft.Extensions.Logging.Console.ConsoleLoggerFormat/*02000006*/ 
          get_Format() cil managed
  // SIG: 20 00 11 18
  {
    // Method begins at RVA 0x209d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLoggerOptions::get_Format

  .method /*06000019*/ public hidebysig specialname 
          instance void  set_Format(valuetype Microsoft.Extensions.Logging.Console.ConsoleLoggerFormat/*02000006*/ 'value') cil managed
  // SIG: 20 01 01 11 18
  {
    // Method begins at RVA 0x20a0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConsoleLoggerOptions::set_Format

  .method /*0600001A*/ public hidebysig specialname 
          instance string  get_FormatterName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLoggerOptions::get_FormatterName

  .method /*0600001B*/ public hidebysig specialname 
          instance void  set_FormatterName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20a5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConsoleLoggerOptions::set_FormatterName

  .method /*0600001C*/ public hidebysig specialname 
          instance bool  get_IncludeScopes() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLoggerOptions::get_IncludeScopes

  .method /*0600001D*/ public hidebysig specialname 
          instance void  set_IncludeScopes(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20aa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConsoleLoggerOptions::set_IncludeScopes

  .method /*0600001E*/ public hidebysig specialname 
          instance valuetype [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LogLevel/*01000020*/ 
          get_LogToStandardErrorThreshold() cil managed
  // SIG: 20 00 11 80 81
  {
    // Method begins at RVA 0x20ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLoggerOptions::get_LogToStandardErrorThreshold

  .method /*0600001F*/ public hidebysig specialname 
          instance void  set_LogToStandardErrorThreshold(valuetype [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LogLevel/*01000020*/ 'value') cil managed
  // SIG: 20 01 01 11 80 81
  {
    // Method begins at RVA 0x20af
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConsoleLoggerOptions::set_LogToStandardErrorThreshold

  .method /*06000020*/ public hidebysig specialname 
          instance string  get_TimestampFormat() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLoggerOptions::get_TimestampFormat

  .method /*06000021*/ public hidebysig specialname 
          instance void  set_TimestampFormat(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20b4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConsoleLoggerOptions::set_TimestampFormat

  .method /*06000022*/ public hidebysig specialname 
          instance bool  get_UseUtcTimestamp() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLoggerOptions::get_UseUtcTimestamp

  .method /*06000023*/ public hidebysig specialname 
          instance void  set_UseUtcTimestamp(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20b9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConsoleLoggerOptions::set_UseUtcTimestamp

  .property /*17000005*/ instance callconv(8) bool
          DisableColors()
  {
    .custom /*0C000022:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 70 43 6F 6E 73 6F 6C 65 4C 6F 67 67 65 72   // ..pConsoleLogger
                                                                                                                                                   4F 70 74 69 6F 6E 73 2E 44 69 73 61 62 6C 65 43   // Options.DisableC
                                                                                                                                                   6F 6C 6F 72 73 20 68 61 73 20 62 65 65 6E 20 64   // olors has been d
                                                                                                                                                   65 70 72 65 63 61 74 65 64 2E 20 55 73 65 20 53   // eprecated. Use S
                                                                                                                                                   69 6D 70 6C 65 43 6F 6E 73 6F 6C 65 46 6F 72 6D   // impleConsoleForm
                                                                                                                                                   61 74 74 65 72 4F 70 74 69 6F 6E 73 2E 43 6F 6C   // atterOptions.Col
                                                                                                                                                   6F 72 42 65 68 61 76 69 6F 72 20 69 6E 73 74 65   // orBehavior inste
                                                                                                                                                   61 64 2E 00 00 )                                  // ad...
    .get instance bool Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions/*02000007*/::get_DisableColors() /* 06000016 */
    .set instance void Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions/*02000007*/::set_DisableColors(bool) /* 06000017 */
  } // end of property ConsoleLoggerOptions::DisableColors
  .property /*17000006*/ instance callconv(8) valuetype Microsoft.Extensions.Logging.Console.ConsoleLoggerFormat/*02000006*/
          Format()
  {
    .custom /*0C000025:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 60 43 6F 6E 73 6F 6C 65 4C 6F 67 67 65 72   // ..`ConsoleLogger
                                                                                                                                                   4F 70 74 69 6F 6E 73 2E 46 6F 72 6D 61 74 20 68   // Options.Format h
                                                                                                                                                   61 73 20 62 65 65 6E 20 64 65 70 72 65 63 61 74   // as been deprecat
                                                                                                                                                   65 64 2E 20 55 73 65 20 43 6F 6E 73 6F 6C 65 4C   // ed. Use ConsoleL
                                                                                                                                                   6F 67 67 65 72 4F 70 74 69 6F 6E 73 2E 46 6F 72   // oggerOptions.For
                                                                                                                                                   6D 61 74 74 65 72 4E 61 6D 65 20 69 6E 73 74 65   // matterName inste
                                                                                                                                                   61 64 2E 00 00 )                                  // ad...
    .get instance valuetype Microsoft.Extensions.Logging.Console.ConsoleLoggerFormat/*02000006*/ Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions/*02000007*/::get_Format() /* 06000018 */
    .set instance void Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions/*02000007*/::set_Format(valuetype Microsoft.Extensions.Logging.Console.ConsoleLoggerFormat/*02000006*/) /* 06000019 */
  } // end of property ConsoleLoggerOptions::Format
  .property /*17000007*/ instance callconv(8) string
          FormatterName()
  {
    .get instance string Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions/*02000007*/::get_FormatterName() /* 0600001A */
    .set instance void Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions/*02000007*/::set_FormatterName(string) /* 0600001B */
  } // end of property ConsoleLoggerOptions::FormatterName
  .property /*17000008*/ instance callconv(8) bool
          IncludeScopes()
  {
    .custom /*0C00002A:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 6A 43 6F 6E 73 6F 6C 65 4C 6F 67 67 65 72   // ..jConsoleLogger
                                                                                                                                                   4F 70 74 69 6F 6E 73 2E 49 6E 63 6C 75 64 65 53   // Options.IncludeS
                                                                                                                                                   63 6F 70 65 73 20 68 61 73 20 62 65 65 6E 20 64   // copes has been d
                                                                                                                                                   65 70 72 65 63 61 74 65 64 2E 20 55 73 65 20 43   // eprecated. Use C
                                                                                                                                                   6F 6E 73 6F 6C 65 46 6F 72 6D 61 74 74 65 72 4F   // onsoleFormatterO
                                                                                                                                                   70 74 69 6F 6E 73 2E 49 6E 63 6C 75 64 65 53 63   // ptions.IncludeSc
                                                                                                                                                   6F 70 65 73 20 69 6E 73 74 65 61 64 2E 00 00 )    // opes instead...
    .get instance bool Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions/*02000007*/::get_IncludeScopes() /* 0600001C */
    .set instance void Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions/*02000007*/::set_IncludeScopes(bool) /* 0600001D */
  } // end of property ConsoleLoggerOptions::IncludeScopes
  .property /*17000009*/ instance callconv(8) valuetype [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LogLevel/*01000020*/
          LogToStandardErrorThreshold()
  {
    .get instance valuetype [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LogLevel/*01000020*/ Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions/*02000007*/::get_LogToStandardErrorThreshold() /* 0600001E */
    .set instance void Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions/*02000007*/::set_LogToStandardErrorThreshold(valuetype [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.LogLevel/*01000020*/) /* 0600001F */
  } // end of property ConsoleLoggerOptions::LogToStandardErrorThreshold
  .property /*1700000A*/ instance callconv(8) string
          TimestampFormat()
  {
    .custom /*0C00002D:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 6E 43 6F 6E 73 6F 6C 65 4C 6F 67 67 65 72   // ..nConsoleLogger
                                                                                                                                                   4F 70 74 69 6F 6E 73 2E 54 69 6D 65 73 74 61 6D   // Options.Timestam
                                                                                                                                                   70 46 6F 72 6D 61 74 20 68 61 73 20 62 65 65 6E   // pFormat has been
                                                                                                                                                   20 64 65 70 72 65 63 61 74 65 64 2E 20 55 73 65   //  deprecated. Use
                                                                                                                                                   20 43 6F 6E 73 6F 6C 65 46 6F 72 6D 61 74 74 65   //  ConsoleFormatte
                                                                                                                                                   72 4F 70 74 69 6F 6E 73 2E 54 69 6D 65 73 74 61   // rOptions.Timesta
                                                                                                                                                   6D 70 46 6F 72 6D 61 74 20 69 6E 73 74 65 61 64   // mpFormat instead
                                                                                                                                                   2E 00 00 )                                        // ...
    .get instance string Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions/*02000007*/::get_TimestampFormat() /* 06000020 */
    .set instance void Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions/*02000007*/::set_TimestampFormat(string) /* 06000021 */
  } // end of property ConsoleLoggerOptions::TimestampFormat
  .property /*1700000B*/ instance callconv(8) bool
          UseUtcTimestamp()
  {
    .custom /*0C00002E:0A000015*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 6E 43 6F 6E 73 6F 6C 65 4C 6F 67 67 65 72   // ..nConsoleLogger
                                                                                                                                                   4F 70 74 69 6F 6E 73 2E 55 73 65 55 74 63 54 69   // Options.UseUtcTi
                                                                                                                                                   6D 65 73 74 61 6D 70 20 68 61 73 20 62 65 65 6E   // mestamp has been
                                                                                                                                                   20 64 65 70 72 65 63 61 74 65 64 2E 20 55 73 65   //  deprecated. Use
                                                                                                                                                   20 43 6F 6E 73 6F 6C 65 46 6F 72 6D 61 74 74 65   //  ConsoleFormatte
                                                                                                                                                   72 4F 70 74 69 6F 6E 73 2E 55 73 65 55 74 63 54   // rOptions.UseUtcT
                                                                                                                                                   69 6D 65 73 74 61 6D 70 20 69 6E 73 74 65 61 64   // imestamp instead
                                                                                                                                                   2E 00 00 )                                        // ...
    .get instance bool Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions/*02000007*/::get_UseUtcTimestamp() /* 06000022 */
    .set instance void Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions/*02000007*/::set_UseUtcTimestamp(bool) /* 06000023 */
  } // end of property ConsoleLoggerOptions::UseUtcTimestamp
} // end of class Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions

.class /*02000008*/ public auto ansi beforefieldinit Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider
       extends [System.Runtime/*23000001*/]System.Object/*01000013*/
       implements [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILoggerProvider/*01000022*/,
                  [System.Runtime/*23000001*/]System.IDisposable/*01000023*/,
                  [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ISupportExternalScope/*01000024*/
{
  .custom /*0C000028:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000012*/::.ctor(string) /* 0A000011 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .custom /*0C000029:0A000016*/ instance void [Microsoft.Extensions.Logging/*23000002*/]Microsoft.Extensions.Logging.ProviderAliasAttribute/*01000021*/::.ctor(string) /* 0A000016 */ = ( 01 00 07 43 6F 6E 73 6F 6C 65 00 00 )             // ...Console..
  .method /*06000024*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Options/*23000004*/]Microsoft.Extensions.Options.IOptionsMonitor`1/*01000025*/<class Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions/*02000007*/> options) cil managed
  // SIG: 20 01 01 15 12 80 95 01 12 1C
  {
    // Method begins at RVA 0x20bb
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ConsoleLoggerProvider::.ctor

  .method /*06000025*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Options/*23000004*/]Microsoft.Extensions.Options.IOptionsMonitor`1/*01000025*/<class Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions/*02000007*/> options,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000026*/<class Microsoft.Extensions.Logging.Console.ConsoleFormatter/*02000003*/> formatters) cil managed
  // SIG: 20 02 01 15 12 80 95 01 12 1C 15 12 80 99 01 12 0C
  {
    // Method begins at RVA 0x20c3
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000013*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ConsoleLoggerProvider::.ctor

  .method /*06000026*/ public hidebysig newslot virtual final 
          instance class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.ILogger/*01000027*/ 
          CreateLogger(string name) cil managed
  // SIG: 20 01 12 80 9D 0E
  {
    // Method begins at RVA 0x20cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLoggerProvider::CreateLogger

  .method /*06000027*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ce
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConsoleLoggerProvider::Dispose

  .method /*06000028*/ public hidebysig newslot virtual final 
          instance void  SetScopeProvider(class [Microsoft.Extensions.Logging.Abstractions/*23000003*/]Microsoft.Extensions.Logging.IExternalScopeProvider/*0100001C*/ scopeProvider) cil managed
  // SIG: 20 01 01 12 71
  {
    // Method begins at RVA 0x20d0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConsoleLoggerProvider::SetScopeProvider

} // end of class Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider

.class /*02000009*/ public auto ansi beforefieldinit Microsoft.Extensions.Logging.Console.JsonConsoleFormatterOptions
       extends Microsoft.Extensions.Logging.Console.ConsoleFormatterOptions/*02000005*/
{
  .method /*06000029*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20d2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00000E       */ call       instance void Microsoft.Extensions.Logging.Console.ConsoleFormatterOptions/*02000005*/::.ctor() /* 0600000E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method JsonConsoleFormatterOptions::.ctor

  .method /*0600002A*/ public hidebysig specialname 
          instance valuetype [System.Text.Json/*23000005*/]System.Text.Json.JsonWriterOptions/*01000028*/ 
          get_JsonWriterOptions() cil managed
  // SIG: 20 00 11 80 A1
  {
    // Method begins at RVA 0x20da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonConsoleFormatterOptions::get_JsonWriterOptions

  .method /*0600002B*/ public hidebysig specialname 
          instance void  set_JsonWriterOptions(valuetype [System.Text.Json/*23000005*/]System.Text.Json.JsonWriterOptions/*01000028*/ 'value') cil managed
  // SIG: 20 01 01 11 80 A1
  {
    // Method begins at RVA 0x20dd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method JsonConsoleFormatterOptions::set_JsonWriterOptions

  .property /*1700000C*/ instance callconv(8) valuetype [System.Text.Json/*23000005*/]System.Text.Json.JsonWriterOptions/*01000028*/
          JsonWriterOptions()
  {
    .get instance valuetype [System.Text.Json/*23000005*/]System.Text.Json.JsonWriterOptions/*01000028*/ Microsoft.Extensions.Logging.Console.JsonConsoleFormatterOptions/*02000009*/::get_JsonWriterOptions() /* 0600002A */
    .set instance void Microsoft.Extensions.Logging.Console.JsonConsoleFormatterOptions/*02000009*/::set_JsonWriterOptions(valuetype [System.Text.Json/*23000005*/]System.Text.Json.JsonWriterOptions/*01000028*/) /* 0600002B */
  } // end of property JsonConsoleFormatterOptions::JsonWriterOptions
} // end of class Microsoft.Extensions.Logging.Console.JsonConsoleFormatterOptions

.class /*0200000A*/ public auto ansi sealed Microsoft.Extensions.Logging.Console.LoggerColorBehavior
       extends [System.Runtime/*23000001*/]System.Enum/*0100001F*/
{
  .field /*04000007*/ public specialname rtspecialname int32 value__
  .field /*04000008*/ public static literal valuetype Microsoft.Extensions.Logging.Console.LoggerColorBehavior/*0200000A*/ Default = int32(0x00000000)
  .field /*04000009*/ public static literal valuetype Microsoft.Extensions.Logging.Console.LoggerColorBehavior/*0200000A*/ Enabled = int32(0x00000001)
  .field /*0400000A*/ public static literal valuetype Microsoft.Extensions.Logging.Console.LoggerColorBehavior/*0200000A*/ Disabled = int32(0x00000002)
} // end of class Microsoft.Extensions.Logging.Console.LoggerColorBehavior

.class /*0200000B*/ public auto ansi beforefieldinit Microsoft.Extensions.Logging.Console.SimpleConsoleFormatterOptions
       extends Microsoft.Extensions.Logging.Console.ConsoleFormatterOptions/*02000005*/
{
  .method /*0600002C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20df
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00000E       */ call       instance void Microsoft.Extensions.Logging.Console.ConsoleFormatterOptions/*02000005*/::.ctor() /* 0600000E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SimpleConsoleFormatterOptions::.ctor

  .method /*0600002D*/ public hidebysig specialname 
          instance valuetype Microsoft.Extensions.Logging.Console.LoggerColorBehavior/*0200000A*/ 
          get_ColorBehavior() cil managed
  // SIG: 20 00 11 28
  {
    // Method begins at RVA 0x20e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SimpleConsoleFormatterOptions::get_ColorBehavior

  .method /*0600002E*/ public hidebysig specialname 
          instance void  set_ColorBehavior(valuetype Microsoft.Extensions.Logging.Console.LoggerColorBehavior/*0200000A*/ 'value') cil managed
  // SIG: 20 01 01 11 28
  {
    // Method begins at RVA 0x20ea
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SimpleConsoleFormatterOptions::set_ColorBehavior

  .method /*0600002F*/ public hidebysig specialname 
          instance bool  get_SingleLine() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SimpleConsoleFormatterOptions::get_SingleLine

  .method /*06000030*/ public hidebysig specialname 
          instance void  set_SingleLine(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20ef
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SimpleConsoleFormatterOptions::set_SingleLine

  .property /*1700000D*/ instance callconv(8) valuetype Microsoft.Extensions.Logging.Console.LoggerColorBehavior/*0200000A*/
          ColorBehavior()
  {
    .get instance valuetype Microsoft.Extensions.Logging.Console.LoggerColorBehavior/*0200000A*/ Microsoft.Extensions.Logging.Console.SimpleConsoleFormatterOptions/*0200000B*/::get_ColorBehavior() /* 0600002D */
    .set instance void Microsoft.Extensions.Logging.Console.SimpleConsoleFormatterOptions/*0200000B*/::set_ColorBehavior(valuetype Microsoft.Extensions.Logging.Console.LoggerColorBehavior/*0200000A*/) /* 0600002E */
  } // end of property SimpleConsoleFormatterOptions::ColorBehavior
  .property /*1700000E*/ instance callconv(8) bool
          SingleLine()
  {
    .get instance bool Microsoft.Extensions.Logging.Console.SimpleConsoleFormatterOptions/*0200000B*/::get_SingleLine() /* 0600002F */
    .set instance void Microsoft.Extensions.Logging.Console.SimpleConsoleFormatterOptions/*0200000B*/::set_SingleLine(bool) /* 06000030 */
  } // end of property SimpleConsoleFormatterOptions::SingleLine
} // end of class Microsoft.Extensions.Logging.Console.SimpleConsoleFormatterOptions


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
