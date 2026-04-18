


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
// Time-date stamp:            0xf0c00a55
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00001a00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000398e
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
// Checksum:                       0x000088db
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00003939 [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x000004c4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002400 [0x00002470] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000383c [0x00000054] address [size] of Debug Directory:           
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
//              0x00001994 Virtual Size
//              0x00002000 Virtual Address
//              0x00001a00 Size of Raw Data
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
//              0x000004c4 Virtual Size
//              0x00004000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00001c00 Pointer to Raw Data
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
//              0x00003000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000990
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000397b Import Name Table
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
// 0x00002114 [0x000016a8] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x000037bc [0x00000080] address [size] of Strong Name Signature:     
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
//              0x000009ec Size
//              '#~' Name
//    Stream 2:
//              0x00000a58 Offset
//              0x0000071c Size
//              '#Strings' Name
//    Stream 3:
//              0x00001174 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00001178 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001188 Offset
//              0x00000520 Size
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

// File size            : 18544
// PE header size       : 512 (496 used)    ( 2.76%)
// PE additional info   : 10803             (58.26%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.39%)
// CLR meta-data size  : 5800               (31.28%)
// CLR additional info : 128                ( 0.69%)
// CLR method headers  : 51                 ( 0.28%)
// Managed code         : 142               ( 0.77%)
// Data                 : 2048              (11.04%)
// Unaccounted          : -1012             (-5.46%)

// Num.of PE sections   : 3
//   .text    - 6656
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 5800
//   Module        -    1 (10 bytes)
//   TypeDef       -   11 (154 bytes)     1 interfaces, 0 explicit layout
//   TypeRef       -   38 (228 bytes)
//   MethodDef     -   52 (728 bytes)     1 abstract, 0 native, 51 bodies
//   FieldDef      -    9 (54 bytes)      0 constant
//   MemberRef     -   20 (120 bytes)
//   ParamDef      -   73 (438 bytes)
//   Constant      -    9 (54 bytes)
//   CustomAttribute-   49 (294 bytes)
//   InterfaceImpl -    2 (8 bytes)
//   PropertyMap   -    5 (20 bytes)
//   Property      -   10 (60 bytes)
//   MethodSemantic-   13 (78 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    4 (80 bytes)
//   GenericParam  -    8 (64 bytes)
//   GenericParamConstraint-    8 (32 bytes)
//   Strings       -  1817 bytes
//   Blobs         -  1312 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   207 bytes

// CLR additional info : 128

// CLR method headers : 51
//   Num.of method bodies  - 51
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 51

// Managed code : 142
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class ActivityTrackingOptions        (public) (auto) (ansi) (sealed) 
// Class FilterLoggingBuilderExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Interface ILoggingBuilder                (public) (abstract) (auto) (ansi) 
// Class LoggerFactory                  (public) (auto) (ansi) 
// Class LoggerFactoryOptions           (public) (auto) (ansi) 
// Class LoggerFilterOptions            (public) (auto) (ansi) 
// Class LoggerFilterRule               (public) (auto) (ansi) 
// Class LoggingBuilderExtensions       (public) (abstract) (auto) (ansi) (sealed) 
// Class ProviderAliasAttribute         (public) (auto) (ansi) 
// Class LoggingServiceCollectionExtensions (public) (abstract) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ netstandard
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 2:0:0:0
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
.assembly extern /*23000004*/ Microsoft.Extensions.Options
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly /*20000001*/ noplatform Microsoft.Extensions.Logging
{
  .custom instance void [netstandard/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                    string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000002:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000003:0A000002*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000002*/::.ctor(int32) /* 0A000002 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000004:0A000003*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000003*/::.ctor() /* 0A000003 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                             63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000005:0A000004*/ instance void [netstandard/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*/::.ctor(valuetype [netstandard/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*//DebuggingModes/*01000005*/) /* 0A000004 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000006:0A000005*/ instance void [netstandard/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000006*/::.ctor(string) /* 0A000005 */ = ( 01 00 19 2E 4E 45 54 53 74 61 6E 64 61 72 64 2C   // ....NETStandard,
                                                                                                                                                                        56 65 72 73 69 6F 6E 3D 76 32 2E 30 01 00 54 0E   // Version=v2.0..T.
                                                                                                                                                                        14 46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61   // .FrameworkDispla
                                                                                                                                                                        79 4E 61 6D 65 00 )                               // yName.
  .custom /*0C000007:0A000006*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000007*/::.ctor() /* 0A000006 */ = ( 01 00 00 00 ) 
  .custom /*0C000008:0A000007*/ instance void [netstandard/*23000001*/]System.CLSCompliantAttribute/*01000008*/::.ctor(bool) /* 0A000007 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000009:0A000008*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 1C 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ...Microsoft.Ext
                                                                                                                                                                     65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 00   // ensions.Logging.
                                                                                                                                                                     00 ) 
  .custom /*0C00000A:0A000009*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                      string) /* 0A000009 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                 41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000B:0A000009*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                      string) /* 0A000009 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                 72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000009*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                      string) /* 0A000009 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                 72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000009*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                      string) /* 0A000009 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                 72 75 65 00 00 )                                  // rue..
  .custom /*0C00000E:0A00000A*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000F:0A00000B*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                  43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                  20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                  2E 00 00 )                                        // ...
  .custom /*0C000010:0A00000C*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 1C 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ...Microsoft.Ext
                                                                                                                                                                    65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 00   // ensions.Logging.
                                                                                                                                                                    00 ) 
  .custom /*0C000011:0A00000D*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 36 2E 30 2E 32 32 32 2E 36 34 30 36 00   // ...6.0.222.6406.
                                                                                                                                                                    00 ) 
  .custom /*0C000012:0A00000E*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 36 2E 30 2E 32 2B 38 33 39 63 64 66 62   // ...6.0.2+839cdfb
                                                                                                                                                                             30 65 63 63 61 35 65 30 62 65 33 64 62 63 63 64   // 0ecca5e0be3dbccd
                                                                                                                                                                             39 32 36 65 37 36 35 31 65 66 35 30 66 64 66 31   // 926e7651ef50fdf1
                                                                                                                                                                             30 00 00 )                                        // 0..
  .custom /*0C000013:0A00000F*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                4E 45 54 00 00 )                                  // NET..
  .custom /*0C000014:0A000010*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 1C 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ...Microsoft.Ext
                                                                                                                                                              65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 00   // ensions.Logging.
                                                                                                                                                              00 ) 
  .custom /*0C000015:0A000009*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
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
.module Microsoft.Extensions.Logging.dll
// MVID: {cbac5055-c9c2-4b7e-a5c6-87b4394278d4}
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi sealed Microsoft.Extensions.Logging.ActivityTrackingOptions
       extends [netstandard/*23000001*/]System.Enum/*01000013*/
{
  .custom /*0C000017:0A000011*/ instance void [netstandard/*23000001*/]System.FlagsAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .field /*04000001*/ public specialname rtspecialname int32 value__
  .field /*04000002*/ public static literal valuetype Microsoft.Extensions.Logging.ActivityTrackingOptions/*02000002*/ None = int32(0x00000000)
  .field /*04000003*/ public static literal valuetype Microsoft.Extensions.Logging.ActivityTrackingOptions/*02000002*/ SpanId = int32(0x00000001)
  .field /*04000004*/ public static literal valuetype Microsoft.Extensions.Logging.ActivityTrackingOptions/*02000002*/ TraceId = int32(0x00000002)
  .field /*04000005*/ public static literal valuetype Microsoft.Extensions.Logging.ActivityTrackingOptions/*02000002*/ ParentId = int32(0x00000004)
  .field /*04000006*/ public static literal valuetype Microsoft.Extensions.Logging.ActivityTrackingOptions/*02000002*/ TraceState = int32(0x00000008)
  .field /*04000007*/ public static literal valuetype Microsoft.Extensions.Logging.ActivityTrackingOptions/*02000002*/ TraceFlags = int32(0x00000010)
  .field /*04000008*/ public static literal valuetype Microsoft.Extensions.Logging.ActivityTrackingOptions/*02000002*/ Tags = int32(0x00000020)
  .field /*04000009*/ public static literal valuetype Microsoft.Extensions.Logging.ActivityTrackingOptions/*02000002*/ Baggage = int32(0x00000040)
} // end of class Microsoft.Extensions.Logging.ActivityTrackingOptions

.class /*02000003*/ public abstract auto ansi sealed beforefieldinit Microsoft.Extensions.Logging.FilterLoggingBuilderExtensions
       extends [netstandard/*23000001*/]System.Object/*01000014*/
{
  .custom /*0C000019:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig static 
          class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ 
          AddFilter(class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ builder,
                    class [netstandard/*23000001*/]System.Func`2/*01000015*/<valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/,bool> levelFilter) cil managed
  // SIG: 00 02 12 10 12 10 15 12 55 02 11 59 02
  {
    .custom /*0C000001:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FilterLoggingBuilderExtensions::AddFilter

  .method /*06000002*/ public hidebysig static 
          class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ 
          AddFilter(class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ builder,
                    class [netstandard/*23000001*/]System.Func`3/*01000017*/<string,valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/,bool> categoryLevelFilter) cil managed
  // SIG: 00 02 12 10 12 10 15 12 5D 03 0E 11 59 02
  {
    .custom /*0C000016:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2053
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FilterLoggingBuilderExtensions::AddFilter

  .method /*06000003*/ public hidebysig static 
          class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ 
          AddFilter(class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ builder,
                    class [netstandard/*23000001*/]System.Func`4/*01000018*/<string,string,valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/,bool> 'filter') cil managed
  // SIG: 00 02 12 10 12 10 15 12 61 04 0E 0E 11 59 02
  {
    .custom /*0C000018:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2056
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FilterLoggingBuilderExtensions::AddFilter

  .method /*06000004*/ public hidebysig static 
          class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ 
          AddFilter(class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ builder,
                    string category,
                    valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/ level) cil managed
  // SIG: 00 03 12 10 12 10 0E 11 59
  {
    .custom /*0C00001A:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2059
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FilterLoggingBuilderExtensions::AddFilter

  .method /*06000005*/ public hidebysig static 
          class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ 
          AddFilter(class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ builder,
                    string category,
                    class [netstandard/*23000001*/]System.Func`2/*01000015*/<valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/,bool> levelFilter) cil managed
  // SIG: 00 03 12 10 12 10 0E 15 12 55 02 11 59 02
  {
    .custom /*0C00001B:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x205c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FilterLoggingBuilderExtensions::AddFilter

  .method /*06000006*/ public hidebysig static 
          class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/ 
          AddFilter(class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/ builder,
                    class [netstandard/*23000001*/]System.Func`2/*01000015*/<valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/,bool> levelFilter) cil managed
  // SIG: 00 02 12 1C 12 1C 15 12 55 02 11 59 02
  {
    .custom /*0C00001C:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x205f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FilterLoggingBuilderExtensions::AddFilter

  .method /*06000007*/ public hidebysig static 
          class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/ 
          AddFilter(class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/ builder,
                    class [netstandard/*23000001*/]System.Func`3/*01000017*/<string,valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/,bool> categoryLevelFilter) cil managed
  // SIG: 00 02 12 1C 12 1C 15 12 5D 03 0E 11 59 02
  {
    .custom /*0C00001D:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2062
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FilterLoggingBuilderExtensions::AddFilter

  .method /*06000008*/ public hidebysig static 
          class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/ 
          AddFilter(class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/ builder,
                    class [netstandard/*23000001*/]System.Func`4/*01000018*/<string,string,valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/,bool> 'filter') cil managed
  // SIG: 00 02 12 1C 12 1C 15 12 61 04 0E 0E 11 59 02
  {
    .custom /*0C00001E:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2065
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FilterLoggingBuilderExtensions::AddFilter

  .method /*06000009*/ public hidebysig static 
          class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/ 
          AddFilter(class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/ builder,
                    string category,
                    valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/ level) cil managed
  // SIG: 00 03 12 1C 12 1C 0E 11 59
  {
    .custom /*0C00001F:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2068
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FilterLoggingBuilderExtensions::AddFilter

  .method /*0600000A*/ public hidebysig static 
          class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/ 
          AddFilter(class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/ builder,
                    string category,
                    class [netstandard/*23000001*/]System.Func`2/*01000015*/<valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/,bool> levelFilter) cil managed
  // SIG: 00 03 12 1C 12 1C 0E 15 12 55 02 11 59 02
  {
    .custom /*0C000021:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x206b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FilterLoggingBuilderExtensions::AddFilter

  .method /*0600000B*/ public hidebysig static 
          class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ 
          AddFilter<([Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILoggerProvider/*01000019*/) T>(class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ builder,
                                                                                                                                         class [netstandard/*23000001*/]System.Func`2/*01000015*/<valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/,bool> levelFilter) cil managed
  // SIG: 10 01 02 12 10 12 10 15 12 55 02 11 59 02
  {
    .custom /*0C000023:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x206e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FilterLoggingBuilderExtensions::AddFilter

  .method /*0600000C*/ public hidebysig static 
          class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ 
          AddFilter<([Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILoggerProvider/*01000019*/) T>(class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ builder,
                                                                                                                                         class [netstandard/*23000001*/]System.Func`3/*01000017*/<string,valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/,bool> categoryLevelFilter) cil managed
  // SIG: 10 01 02 12 10 12 10 15 12 5D 03 0E 11 59 02
  {
    .custom /*0C000025:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2071
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FilterLoggingBuilderExtensions::AddFilter

  .method /*0600000D*/ public hidebysig static 
          class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ 
          AddFilter<([Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILoggerProvider/*01000019*/) T>(class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ builder,
                                                                                                                                         string category,
                                                                                                                                         valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/ level) cil managed
  // SIG: 10 01 03 12 10 12 10 0E 11 59
  {
    .custom /*0C000026:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2074
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FilterLoggingBuilderExtensions::AddFilter

  .method /*0600000E*/ public hidebysig static 
          class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ 
          AddFilter<([Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILoggerProvider/*01000019*/) T>(class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ builder,
                                                                                                                                         string category,
                                                                                                                                         class [netstandard/*23000001*/]System.Func`2/*01000015*/<valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/,bool> levelFilter) cil managed
  // SIG: 10 01 03 12 10 12 10 0E 15 12 55 02 11 59 02
  {
    .custom /*0C000027:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2077
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FilterLoggingBuilderExtensions::AddFilter

  .method /*0600000F*/ public hidebysig static 
          class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/ 
          AddFilter<([Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILoggerProvider/*01000019*/) T>(class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/ builder,
                                                                                                                                         class [netstandard/*23000001*/]System.Func`2/*01000015*/<valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/,bool> levelFilter) cil managed
  // SIG: 10 01 02 12 1C 12 1C 15 12 55 02 11 59 02
  {
    .custom /*0C000028:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x207a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FilterLoggingBuilderExtensions::AddFilter

  .method /*06000010*/ public hidebysig static 
          class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/ 
          AddFilter<([Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILoggerProvider/*01000019*/) T>(class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/ builder,
                                                                                                                                         class [netstandard/*23000001*/]System.Func`3/*01000017*/<string,valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/,bool> categoryLevelFilter) cil managed
  // SIG: 10 01 02 12 1C 12 1C 15 12 5D 03 0E 11 59 02
  {
    .custom /*0C000029:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x207d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FilterLoggingBuilderExtensions::AddFilter

  .method /*06000011*/ public hidebysig static 
          class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/ 
          AddFilter<([Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILoggerProvider/*01000019*/) T>(class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/ builder,
                                                                                                                                         string category,
                                                                                                                                         valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/ level) cil managed
  // SIG: 10 01 03 12 1C 12 1C 0E 11 59
  {
    .custom /*0C00002A:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2080
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FilterLoggingBuilderExtensions::AddFilter

  .method /*06000012*/ public hidebysig static 
          class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/ 
          AddFilter<([Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILoggerProvider/*01000019*/) T>(class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/ builder,
                                                                                                                                         string category,
                                                                                                                                         class [netstandard/*23000001*/]System.Func`2/*01000015*/<valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/,bool> levelFilter) cil managed
  // SIG: 10 01 03 12 1C 12 1C 0E 15 12 55 02 11 59 02
  {
    .custom /*0C00002B:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2083
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FilterLoggingBuilderExtensions::AddFilter

} // end of class Microsoft.Extensions.Logging.FilterLoggingBuilderExtensions

.class /*02000004*/ interface public abstract auto ansi Microsoft.Extensions.Logging.ILoggingBuilder
{
  .method /*06000013*/ public hidebysig newslot specialname abstract virtual 
          instance class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*0100001A*/ 
          get_Services() cil managed
  // SIG: 20 00 12 69
  {
    // Method begins at RVA 0x0
  } // end of method ILoggingBuilder::get_Services

  .property /*17000001*/ instance callconv(8) class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*0100001A*/
          Services()
  {
    .get instance class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*0100001A*/ Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/::get_Services() /* 06000013 */
  } // end of property ILoggingBuilder::Services
} // end of class Microsoft.Extensions.Logging.ILoggingBuilder

.class /*02000005*/ public auto ansi beforefieldinit Microsoft.Extensions.Logging.LoggerFactory
       extends [netstandard/*23000001*/]System.Object/*01000014*/
       implements [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILoggerFactory/*0100001B*/,
                  [netstandard/*23000001*/]System.IDisposable/*0100001C*/
{
  .method /*06000014*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2086
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [netstandard/*23000001*/]System.Object/*01000014*/::.ctor() /* 0A000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LoggerFactory::.ctor

  .method /*06000015*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [netstandard/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILoggerProvider/*01000019*/> providers) cil managed
  // SIG: 20 01 01 15 12 75 01 12 65
  {
    // Method begins at RVA 0x208e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [netstandard/*23000001*/]System.Object/*01000014*/::.ctor() /* 0A000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LoggerFactory::.ctor

  .method /*06000016*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [netstandard/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILoggerProvider/*01000019*/> providers,
                               class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/ filterOptions) cil managed
  // SIG: 20 02 01 15 12 75 01 12 65 12 1C
  {
    // Method begins at RVA 0x2096
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [netstandard/*23000001*/]System.Object/*01000014*/::.ctor() /* 0A000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LoggerFactory::.ctor

  .method /*06000017*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [netstandard/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILoggerProvider/*01000019*/> providers,
                               class [Microsoft.Extensions.Options/*23000004*/]Microsoft.Extensions.Options.IOptionsMonitor`1/*0100001E*/<class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/> filterOption) cil managed
  // SIG: 20 02 01 15 12 75 01 12 65 15 12 79 01 12 1C
  {
    // Method begins at RVA 0x209e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [netstandard/*23000001*/]System.Object/*01000014*/::.ctor() /* 0A000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LoggerFactory::.ctor

  .method /*06000018*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [netstandard/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILoggerProvider/*01000019*/> providers,
                               class [Microsoft.Extensions.Options/*23000004*/]Microsoft.Extensions.Options.IOptionsMonitor`1/*0100001E*/<class Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/> filterOption,
                               [opt] class [Microsoft.Extensions.Options/*23000004*/]Microsoft.Extensions.Options.IOptions`1/*0100001F*/<class Microsoft.Extensions.Logging.LoggerFactoryOptions/*02000006*/> options) cil managed
  // SIG: 20 03 01 15 12 75 01 12 65 15 12 79 01 12 1C 15 12 7D 01 12 18
  {
    .param [3]/*08000034*/  = nullref
    // Method begins at RVA 0x20a6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [netstandard/*23000001*/]System.Object/*01000014*/::.ctor() /* 0A000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LoggerFactory::.ctor

  .method /*06000019*/ public hidebysig newslot virtual final 
          instance void  AddProvider(class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILoggerProvider/*01000019*/ provider) cil managed
  // SIG: 20 01 01 12 65
  {
    // Method begins at RVA 0x20ae
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method LoggerFactory::AddProvider

  .method /*0600001A*/ family hidebysig newslot virtual 
          instance bool  CheckDisposed() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggerFactory::CheckDisposed

  .method /*0600001B*/ public hidebysig static 
          class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILoggerFactory/*0100001B*/ 
          Create(class [netstandard/*23000001*/]System.Action`1/*01000020*/<class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/> configure) cil managed
  // SIG: 00 01 12 6D 15 12 80 81 01 12 10
  {
    // Method begins at RVA 0x20b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggerFactory::Create

  .method /*0600001C*/ public hidebysig newslot virtual final 
          instance class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILogger/*01000021*/ 
          CreateLogger(string categoryName) cil managed
  // SIG: 20 01 12 80 85 0E
  {
    // Method begins at RVA 0x20b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggerFactory::CreateLogger

  .method /*0600001D*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20b9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method LoggerFactory::Dispose

} // end of class Microsoft.Extensions.Logging.LoggerFactory

.class /*02000006*/ public auto ansi beforefieldinit Microsoft.Extensions.Logging.LoggerFactoryOptions
       extends [netstandard/*23000001*/]System.Object/*01000014*/
{
  .method /*0600001E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20bb
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [netstandard/*23000001*/]System.Object/*01000014*/::.ctor() /* 0A000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LoggerFactoryOptions::.ctor

  .method /*0600001F*/ public hidebysig specialname 
          instance valuetype Microsoft.Extensions.Logging.ActivityTrackingOptions/*02000002*/ 
          get_ActivityTrackingOptions() cil managed
  // SIG: 20 00 11 08
  {
    // Method begins at RVA 0x20c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggerFactoryOptions::get_ActivityTrackingOptions

  .method /*06000020*/ public hidebysig specialname 
          instance void  set_ActivityTrackingOptions(valuetype Microsoft.Extensions.Logging.ActivityTrackingOptions/*02000002*/ 'value') cil managed
  // SIG: 20 01 01 11 08
  {
    // Method begins at RVA 0x20c6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method LoggerFactoryOptions::set_ActivityTrackingOptions

  .property /*17000002*/ instance callconv(8) valuetype Microsoft.Extensions.Logging.ActivityTrackingOptions/*02000002*/
          ActivityTrackingOptions()
  {
    .get instance valuetype Microsoft.Extensions.Logging.ActivityTrackingOptions/*02000002*/ Microsoft.Extensions.Logging.LoggerFactoryOptions/*02000006*/::get_ActivityTrackingOptions() /* 0600001F */
    .set instance void Microsoft.Extensions.Logging.LoggerFactoryOptions/*02000006*/::set_ActivityTrackingOptions(valuetype Microsoft.Extensions.Logging.ActivityTrackingOptions/*02000002*/) /* 06000020 */
  } // end of property LoggerFactoryOptions::ActivityTrackingOptions
} // end of class Microsoft.Extensions.Logging.LoggerFactoryOptions

.class /*02000007*/ public auto ansi beforefieldinit Microsoft.Extensions.Logging.LoggerFilterOptions
       extends [netstandard/*23000001*/]System.Object/*01000014*/
{
  .method /*06000021*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20c8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [netstandard/*23000001*/]System.Object/*01000014*/::.ctor() /* 0A000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LoggerFilterOptions::.ctor

  .method /*06000022*/ public hidebysig specialname 
          instance bool  get_CaptureScopes() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggerFilterOptions::get_CaptureScopes

  .method /*06000023*/ public hidebysig specialname 
          instance void  set_CaptureScopes(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20d3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method LoggerFilterOptions::set_CaptureScopes

  .method /*06000024*/ public hidebysig specialname 
          instance valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/ 
          get_MinLevel() cil managed
  // SIG: 20 00 11 59
  {
    // Method begins at RVA 0x20d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggerFilterOptions::get_MinLevel

  .method /*06000025*/ public hidebysig specialname 
          instance void  set_MinLevel(valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/ 'value') cil managed
  // SIG: 20 01 01 11 59
  {
    // Method begins at RVA 0x20d8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method LoggerFilterOptions::set_MinLevel

  .method /*06000026*/ public hidebysig specialname 
          instance class [netstandard/*23000001*/]System.Collections.Generic.IList`1/*01000022*/<class Microsoft.Extensions.Logging.LoggerFilterRule/*02000008*/> 
          get_Rules() cil managed
  // SIG: 20 00 15 12 80 89 01 12 20
  {
    // Method begins at RVA 0x20da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggerFilterOptions::get_Rules

  .property /*17000003*/ instance callconv(8) bool
          CaptureScopes()
  {
    .get instance bool Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/::get_CaptureScopes() /* 06000022 */
    .set instance void Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/::set_CaptureScopes(bool) /* 06000023 */
  } // end of property LoggerFilterOptions::CaptureScopes
  .property /*17000004*/ instance callconv(8) valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/
          MinLevel()
  {
    .get instance valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/ Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/::get_MinLevel() /* 06000024 */
    .set instance void Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/::set_MinLevel(valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/) /* 06000025 */
  } // end of property LoggerFilterOptions::MinLevel
  .property /*17000005*/ instance callconv(8) class [netstandard/*23000001*/]System.Collections.Generic.IList`1/*01000022*/<class Microsoft.Extensions.Logging.LoggerFilterRule/*02000008*/>
          Rules()
  {
    .get instance class [netstandard/*23000001*/]System.Collections.Generic.IList`1/*01000022*/<class Microsoft.Extensions.Logging.LoggerFilterRule/*02000008*/> Microsoft.Extensions.Logging.LoggerFilterOptions/*02000007*/::get_Rules() /* 06000026 */
  } // end of property LoggerFilterOptions::Rules
} // end of class Microsoft.Extensions.Logging.LoggerFilterOptions

.class /*02000008*/ public auto ansi beforefieldinit Microsoft.Extensions.Logging.LoggerFilterRule
       extends [netstandard/*23000001*/]System.Object/*01000014*/
{
  .method /*06000027*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string providerName,
                               string categoryName,
                               valuetype [netstandard/*23000001*/]System.Nullable`1/*01000023*/<valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/> logLevel,
                               class [netstandard/*23000001*/]System.Func`4/*01000018*/<string,string,valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/,bool> 'filter') cil managed
  // SIG: 20 04 01 0E 0E 15 11 80 8D 01 11 59 15 12 61 04 0E 0E 11 59 02
  {
    // Method begins at RVA 0x20dd
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [netstandard/*23000001*/]System.Object/*01000014*/::.ctor() /* 0A000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LoggerFilterRule::.ctor

  .method /*06000028*/ public hidebysig specialname 
          instance string  get_CategoryName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggerFilterRule::get_CategoryName

  .method /*06000029*/ public hidebysig specialname 
          instance class [netstandard/*23000001*/]System.Func`4/*01000018*/<string,string,valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/,bool> 
          get_Filter() cil managed
  // SIG: 20 00 15 12 61 04 0E 0E 11 59 02
  {
    // Method begins at RVA 0x20e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggerFilterRule::get_Filter

  .method /*0600002A*/ public hidebysig specialname 
          instance valuetype [netstandard/*23000001*/]System.Nullable`1/*01000023*/<valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/> 
          get_LogLevel() cil managed
  // SIG: 20 00 15 11 80 8D 01 11 59
  {
    // Method begins at RVA 0x20eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggerFilterRule::get_LogLevel

  .method /*0600002B*/ public hidebysig specialname 
          instance string  get_ProviderName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggerFilterRule::get_ProviderName

  .method /*0600002C*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggerFilterRule::ToString

  .property /*17000006*/ instance callconv(8) string
          CategoryName()
  {
    .get instance string Microsoft.Extensions.Logging.LoggerFilterRule/*02000008*/::get_CategoryName() /* 06000028 */
  } // end of property LoggerFilterRule::CategoryName
  .property /*17000007*/ instance callconv(8) class [netstandard/*23000001*/]System.Func`4/*01000018*/<string,string,valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/,bool>
          Filter()
  {
    .get instance class [netstandard/*23000001*/]System.Func`4/*01000018*/<string,string,valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/,bool> Microsoft.Extensions.Logging.LoggerFilterRule/*02000008*/::get_Filter() /* 06000029 */
  } // end of property LoggerFilterRule::Filter
  .property /*17000008*/ instance callconv(8) valuetype [netstandard/*23000001*/]System.Nullable`1/*01000023*/<valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/>
          LogLevel()
  {
    .get instance valuetype [netstandard/*23000001*/]System.Nullable`1/*01000023*/<valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/> Microsoft.Extensions.Logging.LoggerFilterRule/*02000008*/::get_LogLevel() /* 0600002A */
  } // end of property LoggerFilterRule::LogLevel
  .property /*17000009*/ instance callconv(8) string
          ProviderName()
  {
    .get instance string Microsoft.Extensions.Logging.LoggerFilterRule/*02000008*/::get_ProviderName() /* 0600002B */
  } // end of property LoggerFilterRule::ProviderName
} // end of class Microsoft.Extensions.Logging.LoggerFilterRule

.class /*02000009*/ public abstract auto ansi sealed beforefieldinit Microsoft.Extensions.Logging.LoggingBuilderExtensions
       extends [netstandard/*23000001*/]System.Object/*01000014*/
{
  .custom /*0C000020:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*0600002D*/ public hidebysig static 
          class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ 
          AddProvider(class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ builder,
                      class [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.ILoggerProvider/*01000019*/ provider) cil managed
  // SIG: 00 02 12 10 12 10 12 65
  {
    .custom /*0C00002C:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggingBuilderExtensions::AddProvider

  .method /*0600002E*/ public hidebysig static 
          class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ 
          ClearProviders(class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ builder) cil managed
  // SIG: 00 01 12 10 12 10
  {
    .custom /*0C00002D:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggingBuilderExtensions::ClearProviders

  .method /*0600002F*/ public hidebysig static 
          class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ 
          Configure(class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ builder,
                    class [netstandard/*23000001*/]System.Action`1/*01000020*/<class Microsoft.Extensions.Logging.LoggerFactoryOptions/*02000006*/> action) cil managed
  // SIG: 00 02 12 10 12 10 15 12 80 81 01 12 18
  {
    .custom /*0C00002E:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggingBuilderExtensions::Configure

  .method /*06000030*/ public hidebysig static 
          class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ 
          SetMinimumLevel(class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/ builder,
                          valuetype [Microsoft.Extensions.Logging.Abstractions/*23000002*/]Microsoft.Extensions.Logging.LogLevel/*01000016*/ level) cil managed
  // SIG: 00 02 12 10 12 10 11 59
  {
    .custom /*0C00002F:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggingBuilderExtensions::SetMinimumLevel

} // end of class Microsoft.Extensions.Logging.LoggingBuilderExtensions

.class /*0200000A*/ public auto ansi beforefieldinit Microsoft.Extensions.Logging.ProviderAliasAttribute
       extends [netstandard/*23000001*/]System.Attribute/*01000026*/
{
  .custom /*0C000022:0A000012*/ instance void [netstandard/*23000001*/]System.AttributeUsageAttribute/*01000025*/::.ctor(valuetype [netstandard/*23000001*/]System.AttributeTargets/*01000024*/) /* 0A000012 */ = ( 01 00 04 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                    4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                    72 69 74 65 64 00 )                               // rited.
  .method /*06000031*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string alias) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2100
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [netstandard/*23000001*/]System.Attribute/*01000026*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ProviderAliasAttribute::.ctor

  .method /*06000032*/ public hidebysig specialname 
          instance string  get_Alias() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2108
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ProviderAliasAttribute::get_Alias

  .property /*1700000A*/ instance callconv(8) string
          Alias()
  {
    .get instance string Microsoft.Extensions.Logging.ProviderAliasAttribute/*0200000A*/::get_Alias() /* 06000032 */
  } // end of property ProviderAliasAttribute::Alias
} // end of class Microsoft.Extensions.Logging.ProviderAliasAttribute

.class /*0200000B*/ public abstract auto ansi sealed beforefieldinit Microsoft.Extensions.DependencyInjection.LoggingServiceCollectionExtensions
       extends [netstandard/*23000001*/]System.Object/*01000014*/
{
  .custom /*0C000024:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000033*/ public hidebysig static 
          class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*0100001A*/ 
          AddLogging(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*0100001A*/ services) cil managed
  // SIG: 00 01 12 69 12 69
  {
    .custom /*0C000030:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x210b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggingServiceCollectionExtensions::AddLogging

  .method /*06000034*/ public hidebysig static 
          class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*0100001A*/ 
          AddLogging(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*0100001A*/ services,
                     class [netstandard/*23000001*/]System.Action`1/*01000020*/<class Microsoft.Extensions.Logging.ILoggingBuilder/*02000004*/> configure) cil managed
  // SIG: 00 02 12 69 12 69 15 12 80 81 01 12 10
  {
    .custom /*0C000031:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x210e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LoggingServiceCollectionExtensions::AddLogging

} // end of class Microsoft.Extensions.DependencyInjection.LoggingServiceCollectionExtensions


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
