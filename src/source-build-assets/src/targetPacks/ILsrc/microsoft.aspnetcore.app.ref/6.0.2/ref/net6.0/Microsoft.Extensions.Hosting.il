


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
// Time-date stamp:            0xc1210aba
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00002400
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00004356
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
// Checksum:                       0x00013434
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00004301 [0x0000004f] address [size] of Import Directory:          
// 0x00006000 [0x000004c4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002e00 [0x00002468] address [size] of Security Directory:        
// 0x00008000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00004204 [0x00000054] address [size] of Debug Directory:           
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
//              0x0000235c Virtual Size
//              0x00002000 Virtual Address
//              0x00002400 Size of Raw Data
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
//              0x00006000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00002600 Pointer to Raw Data
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
//              0x00002c00 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000358
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00004343 Import Name Table
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
// 0x00002170 [0x00002014] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00004184 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000cd8 Size
//              '#~' Name
//    Stream 2:
//              0x00000d44 Offset
//              0x00000d00 Size
//              '#Strings' Name
//    Stream 3:
//              0x00001a44 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00001a48 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001a58 Offset
//              0x000005bc Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000040901a01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 21096
// PE header size       : 512 (496 used)    ( 2.43%)
// PE additional info   : 10795             (51.17%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.34%)
// CLR meta-data size  : 8212               (38.93%)
// CLR additional info : 128                ( 0.61%)
// CLR method headers  : 66                 ( 0.31%)
// Managed code         : 220               ( 1.04%)
// Data                 : 2048              ( 9.71%)
// Unaccounted          : -957              (-4.54%)

// Num.of PE sections   : 3
//   .text    - 9216
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 8212
//   Module        -    1 (10 bytes)
//   TypeDef       -   19 (266 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   50 (300 bytes)
//   MethodDef     -   66 (924 bytes)     0 abstract, 0 native, 66 bodies
//   FieldDef      -   22 (132 bytes)     0 constant
//   MemberRef     -   21 (126 bytes)
//   ParamDef      -   66 (396 bytes)
//   Constant      -   20 (120 bytes)
//   CustomAttribute-   71 (426 bytes)
//   InterfaceImpl -    7 (28 bytes)
//   PropertyMap   -    7 (28 bytes)
//   Property      -   13 (78 bytes)
//   MethodSemantic-   20 (120 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    9 (180 bytes)
//   GenericParam  -    5 (40 bytes)
//   Strings       -  3327 bytes
//   Blobs         -  1468 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   201 bytes

// CLR additional info : 128

// CLR method headers : 66
//   Num.of method bodies  - 66
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 66

// Managed code : 220
//   Ave method size - 3
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class OSPlatformAttribute            (abstract) (auto) (ansi) 
// Class TargetPlatformAttribute        (auto) (ansi) (sealed) 
// Class SupportedOSPlatformAttribute   (auto) (ansi) (sealed) 
// Class UnsupportedOSPlatformAttribute (auto) (ansi) (sealed) 
// Class SupportedOSPlatformGuardAttribute (auto) (ansi) (sealed) 
// Class UnsupportedOSPlatformGuardAttribute (auto) (ansi) (sealed) 
// Class DynamicallyAccessedMembersAttribute (auto) (ansi) (sealed) 
// Class DynamicallyAccessedMemberTypes (auto) (ansi) (sealed) 
// Class BackgroundServiceExceptionBehavior (public) (auto) (ansi) (sealed) 
// Class ConsoleLifetimeOptions         (public) (auto) (ansi) 
// Class Host                           (public) (abstract) (auto) (ansi) (sealed) 
// Class HostBuilder                    (public) (auto) (ansi) 
// Class HostingHostBuilderExtensions   (public) (abstract) (auto) (ansi) (sealed) 
// Class HostOptions                    (public) (auto) (ansi) 
// Class ApplicationLifetime            (public) (auto) (ansi) 
// Class ConsoleLifetime                (public) (auto) (ansi) 
// Class HostingEnvironment             (public) (auto) (ansi) 
// Class OptionsBuilderExtensions       (public) (abstract) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ netstandard
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 2:0:0:0
}
.assembly extern /*23000002*/ Microsoft.Extensions.Hosting.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ Microsoft.Extensions.Configuration.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ Microsoft.Extensions.DependencyInjection.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000005*/ Microsoft.Extensions.Logging
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000006*/ Microsoft.Extensions.DependencyInjection
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000007*/ Microsoft.Extensions.Logging.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000008*/ Microsoft.Extensions.Options
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000009*/ Microsoft.Extensions.FileProviders.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly /*20000001*/ noplatform Microsoft.Extensions.Hosting
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
                                                                                                                                                                     65 6E 73 69 6F 6E 73 2E 48 6F 73 74 69 6E 67 00   // ensions.Hosting.
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
                                                                                                                                                                    65 6E 73 69 6F 6E 73 2E 48 6F 73 74 69 6E 67 00   // ensions.Hosting.
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
                                                                                                                                                              65 6E 73 69 6F 6E 73 2E 48 6F 73 74 69 6E 67 00   // ensions.Hosting.
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
.module Microsoft.Extensions.Hosting.dll
// MVID: {c4d2e5b7-68a9-4035-b7fb-69523c861325}
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private abstract auto ansi beforefieldinit System.Runtime.Versioning.OSPlatformAttribute
       extends [netstandard/*23000001*/]System.Attribute/*01000012*/
{
  .field /*04000001*/ private initonly string '<PlatformName>k__BackingField'
  .custom /*0C000001:0A000011*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ famandassem hidebysig specialname rtspecialname 
          instance void  .ctor(string platformName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2050
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [netstandard/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000014 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      string System.Runtime.Versioning.OSPlatformAttribute/*02000002*/::'<PlatformName>k__BackingField' /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method OSPlatformAttribute::.ctor

  .method /*06000002*/ public hidebysig specialname 
          instance string  get_PlatformName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000016:0A000011*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x205f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000001       */ ldfld      string System.Runtime.Versioning.OSPlatformAttribute/*02000002*/::'<PlatformName>k__BackingField' /* 04000001 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method OSPlatformAttribute::get_PlatformName

  .property /*17000001*/ instance callconv(8) string
          PlatformName()
  {
    .get instance string System.Runtime.Versioning.OSPlatformAttribute/*02000002*/::get_PlatformName() /* 06000002 */
  } // end of property OSPlatformAttribute::PlatformName
} // end of class System.Runtime.Versioning.OSPlatformAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.Versioning.TargetPlatformAttribute
       extends System.Runtime.Versioning.OSPlatformAttribute/*02000002*/
{
  .custom /*0C000018:0A000012*/ instance void [netstandard/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [netstandard/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 01 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                    4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                    72 69 74 65 64 00 )                               // rited.
  .method /*06000003*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string platformName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2067
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (06)000001       */ call       instance void System.Runtime.Versioning.OSPlatformAttribute/*02000002*/::.ctor(string) /* 06000001 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TargetPlatformAttribute::.ctor

} // end of class System.Runtime.Versioning.TargetPlatformAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.Versioning.SupportedOSPlatformAttribute
       extends System.Runtime.Versioning.OSPlatformAttribute/*02000002*/
{
  .custom /*0C000019:0A000012*/ instance void [netstandard/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [netstandard/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 FF 07 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                    4D 75 6C 74 69 70 6C 65 01 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                    72 69 74 65 64 00 )                               // rited.
  .method /*06000004*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string platformName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2070
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (06)000001       */ call       instance void System.Runtime.Versioning.OSPlatformAttribute/*02000002*/::.ctor(string) /* 06000001 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method SupportedOSPlatformAttribute::.ctor

} // end of class System.Runtime.Versioning.SupportedOSPlatformAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.Versioning.UnsupportedOSPlatformAttribute
       extends System.Runtime.Versioning.OSPlatformAttribute/*02000002*/
{
  .custom /*0C00001A:0A000012*/ instance void [netstandard/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [netstandard/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 FF 07 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                    4D 75 6C 74 69 70 6C 65 01 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                    72 69 74 65 64 00 )                               // rited.
  .method /*06000005*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string platformName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2079
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (06)000001       */ call       instance void System.Runtime.Versioning.OSPlatformAttribute/*02000002*/::.ctor(string) /* 06000001 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method UnsupportedOSPlatformAttribute::.ctor

} // end of class System.Runtime.Versioning.UnsupportedOSPlatformAttribute

.class /*02000006*/ private auto ansi sealed beforefieldinit System.Runtime.Versioning.SupportedOSPlatformGuardAttribute
       extends System.Runtime.Versioning.OSPlatformAttribute/*02000002*/
{
  .custom /*0C00001C:0A000012*/ instance void [netstandard/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [netstandard/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 C0 01 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                    4D 75 6C 74 69 70 6C 65 01 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                    72 69 74 65 64 00 )                               // rited.
  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string platformName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2082
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (06)000001       */ call       instance void System.Runtime.Versioning.OSPlatformAttribute/*02000002*/::.ctor(string) /* 06000001 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method SupportedOSPlatformGuardAttribute::.ctor

} // end of class System.Runtime.Versioning.SupportedOSPlatformGuardAttribute

.class /*02000007*/ private auto ansi sealed beforefieldinit System.Runtime.Versioning.UnsupportedOSPlatformGuardAttribute
       extends System.Runtime.Versioning.OSPlatformAttribute/*02000002*/
{
  .custom /*0C00001D:0A000012*/ instance void [netstandard/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [netstandard/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 C0 01 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                    4D 75 6C 74 69 70 6C 65 01 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                    72 69 74 65 64 00 )                               // rited.
  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string platformName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x208b
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (06)000001       */ call       instance void System.Runtime.Versioning.OSPlatformAttribute/*02000002*/::.ctor(string) /* 06000001 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method UnsupportedOSPlatformGuardAttribute::.ctor

} // end of class System.Runtime.Versioning.UnsupportedOSPlatformGuardAttribute

.class /*02000008*/ private auto ansi sealed beforefieldinit System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute
       extends [netstandard/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00001E:0A000012*/ instance void [netstandard/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [netstandard/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 CC 6D 00 00 01 00 54 02 09 49 6E 68 65 72   // ...m....T..Inher
                                                                                                                                                                                                                    69 74 65 64 00 )                                  // ited.
  .field /*04000002*/ private initonly valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ '<MemberTypes>k__BackingField'
  .custom /*0C000017:0A000011*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .method /*06000008*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ memberTypes) cil managed
  // SIG: 20 01 01 11 24
  {
    // Method begins at RVA 0x2094
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [netstandard/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000014 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*02000008*/::'<MemberTypes>k__BackingField' /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method DynamicallyAccessedMembersAttribute::.ctor

  .method /*06000009*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ 
          get_MemberTypes() cil managed
  // SIG: 20 00 11 24
  {
    .custom /*0C00001F:0A000011*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20a3
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000002       */ ldfld      valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*02000008*/::'<MemberTypes>k__BackingField' /* 04000002 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DynamicallyAccessedMembersAttribute::get_MemberTypes

  .property /*17000002*/ instance callconv(8) valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/
          MemberTypes()
  {
    .get instance valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*02000008*/::get_MemberTypes() /* 06000009 */
  } // end of property DynamicallyAccessedMembersAttribute::MemberTypes
} // end of class System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute

.class /*02000009*/ private auto ansi sealed System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes
       extends [netstandard/*23000001*/]System.Enum/*01000017*/
{
  .custom /*0C000020:0A000013*/ instance void [netstandard/*23000001*/]System.FlagsAttribute/*01000016*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*04000003*/ public specialname rtspecialname int32 value__
  .field /*04000004*/ public static literal valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ None = int32(0x00000000)
  .field /*04000005*/ public static literal valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ PublicParameterlessConstructor = int32(0x00000001)
  .field /*04000006*/ public static literal valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ PublicConstructors = int32(0x00000003)
  .field /*04000007*/ public static literal valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ NonPublicConstructors = int32(0x00000004)
  .field /*04000008*/ public static literal valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ PublicMethods = int32(0x00000008)
  .field /*04000009*/ public static literal valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ NonPublicMethods = int32(0x00000010)
  .field /*0400000A*/ public static literal valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ PublicFields = int32(0x00000020)
  .field /*0400000B*/ public static literal valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ NonPublicFields = int32(0x00000040)
  .field /*0400000C*/ public static literal valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ PublicNestedTypes = int32(0x00000080)
  .field /*0400000D*/ public static literal valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ NonPublicNestedTypes = int32(0x00000100)
  .field /*0400000E*/ public static literal valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ PublicProperties = int32(0x00000200)
  .field /*0400000F*/ public static literal valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ NonPublicProperties = int32(0x00000400)
  .field /*04000010*/ public static literal valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ PublicEvents = int32(0x00000800)
  .field /*04000011*/ public static literal valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ NonPublicEvents = int32(0x00001000)
  .field /*04000012*/ public static literal valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ Interfaces = int32(0x00002000)
  .field /*04000013*/ public static literal valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/ All = int32(0xFFFFFFFF)
} // end of class System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes

.class /*0200000A*/ public auto ansi sealed Microsoft.Extensions.Hosting.BackgroundServiceExceptionBehavior
       extends [netstandard/*23000001*/]System.Enum/*01000017*/
{
  .field /*04000014*/ public specialname rtspecialname int32 value__
  .field /*04000015*/ public static literal valuetype Microsoft.Extensions.Hosting.BackgroundServiceExceptionBehavior/*0200000A*/ StopHost = int32(0x00000000)
  .field /*04000016*/ public static literal valuetype Microsoft.Extensions.Hosting.BackgroundServiceExceptionBehavior/*0200000A*/ Ignore = int32(0x00000001)
} // end of class Microsoft.Extensions.Hosting.BackgroundServiceExceptionBehavior

.class /*0200000B*/ public auto ansi beforefieldinit Microsoft.Extensions.Hosting.ConsoleLifetimeOptions
       extends [netstandard/*23000001*/]System.Object/*01000018*/
{
  .method /*0600000A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ab
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [netstandard/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ConsoleLifetimeOptions::.ctor

  .method /*0600000B*/ public hidebysig specialname 
          instance bool  get_SuppressStatusMessages() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLifetimeOptions::get_SuppressStatusMessages

  .method /*0600000C*/ public hidebysig specialname 
          instance void  set_SuppressStatusMessages(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20b6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConsoleLifetimeOptions::set_SuppressStatusMessages

  .property /*17000003*/ instance callconv(8) bool
          SuppressStatusMessages()
  {
    .get instance bool Microsoft.Extensions.Hosting.ConsoleLifetimeOptions/*0200000B*/::get_SuppressStatusMessages() /* 0600000B */
    .set instance void Microsoft.Extensions.Hosting.ConsoleLifetimeOptions/*0200000B*/::set_SuppressStatusMessages(bool) /* 0600000C */
  } // end of property ConsoleLifetimeOptions::SuppressStatusMessages
} // end of class Microsoft.Extensions.Hosting.ConsoleLifetimeOptions

.class /*0200000C*/ public abstract auto ansi sealed beforefieldinit Microsoft.Extensions.Hosting.Host
       extends [netstandard/*23000001*/]System.Object/*01000018*/
{
  .method /*0600000D*/ public hidebysig static 
          class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          CreateDefaultBuilder() cil managed
  // SIG: 00 00 12 65
  {
    // Method begins at RVA 0x20b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Host::CreateDefaultBuilder

  .method /*0600000E*/ public hidebysig static 
          class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          CreateDefaultBuilder(string[] args) cil managed
  // SIG: 00 01 12 65 1D 0E
  {
    // Method begins at RVA 0x20bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Host::CreateDefaultBuilder

} // end of class Microsoft.Extensions.Hosting.Host

.class /*0200000D*/ public auto ansi beforefieldinit Microsoft.Extensions.Hosting.HostBuilder
       extends [netstandard/*23000001*/]System.Object/*01000018*/
       implements [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/
{
  .method /*0600000F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20be
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [netstandard/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HostBuilder::.ctor

  .method /*06000010*/ public hidebysig newslot specialname virtual final 
          instance class [netstandard/*23000001*/]System.Collections.Generic.IDictionary`2/*0100001A*/<object,object> 
          get_Properties() cil managed
  // SIG: 20 00 15 12 69 02 1C 1C
  {
    // Method begins at RVA 0x20c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostBuilder::get_Properties

  .method /*06000011*/ public hidebysig newslot virtual final 
          instance class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHost/*0100001B*/ 
          Build() cil managed
  // SIG: 20 00 12 6D
  {
    // Method begins at RVA 0x20c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostBuilder::Build

  .method /*06000012*/ public hidebysig newslot virtual final 
          instance class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          ConfigureAppConfiguration(class [netstandard/*23000001*/]System.Action`2/*0100001C*/<class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.HostBuilderContext/*0100001D*/,class [Microsoft.Extensions.Configuration.Abstractions/*23000003*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*0100001E*/> configureDelegate) cil managed
  // SIG: 20 01 12 65 15 12 71 02 12 75 12 79
  {
    // Method begins at RVA 0x20cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostBuilder::ConfigureAppConfiguration

  .method /*06000013*/ public hidebysig newslot virtual final 
          instance class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          ConfigureContainer<TContainerBuilder>(class [netstandard/*23000001*/]System.Action`2/*0100001C*/<class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.HostBuilderContext/*0100001D*/,!!TContainerBuilder> configureDelegate) cil managed
  // SIG: 30 01 01 12 65 15 12 71 02 12 75 1E 00
  {
    // Method begins at RVA 0x20cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostBuilder::ConfigureContainer

  .method /*06000014*/ public hidebysig newslot virtual final 
          instance class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          ConfigureHostConfiguration(class [netstandard/*23000001*/]System.Action`1/*0100001F*/<class [Microsoft.Extensions.Configuration.Abstractions/*23000003*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*0100001E*/> configureDelegate) cil managed
  // SIG: 20 01 12 65 15 12 7D 01 12 79
  {
    // Method begins at RVA 0x20d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostBuilder::ConfigureHostConfiguration

  .method /*06000015*/ public hidebysig newslot virtual final 
          instance class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          ConfigureServices(class [netstandard/*23000001*/]System.Action`2/*0100001C*/<class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.HostBuilderContext/*0100001D*/,class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000004*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000020*/> configureDelegate) cil managed
  // SIG: 20 01 12 65 15 12 71 02 12 75 12 80 81
  {
    // Method begins at RVA 0x20d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostBuilder::ConfigureServices

  .method /*06000016*/ public hidebysig newslot virtual final 
          instance class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          UseServiceProviderFactory<TContainerBuilder>(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000004*/]Microsoft.Extensions.DependencyInjection.IServiceProviderFactory`1/*01000021*/<!!TContainerBuilder> factory) cil managed
  // SIG: 30 01 01 12 65 15 12 80 85 01 1E 00
  {
    // Method begins at RVA 0x20d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostBuilder::UseServiceProviderFactory

  .method /*06000017*/ public hidebysig newslot virtual final 
          instance class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          UseServiceProviderFactory<TContainerBuilder>(class [netstandard/*23000001*/]System.Func`2/*01000022*/<class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.HostBuilderContext/*0100001D*/,class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000004*/]Microsoft.Extensions.DependencyInjection.IServiceProviderFactory`1/*01000021*/<!!TContainerBuilder>> factory) cil managed
  // SIG: 30 01 01 12 65 15 12 80 89 02 12 75 15 12 80 85 01 1E 00
  {
    // Method begins at RVA 0x20db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostBuilder::UseServiceProviderFactory

  .property /*17000004*/ instance callconv(8) class [netstandard/*23000001*/]System.Collections.Generic.IDictionary`2/*0100001A*/<object,object>
          Properties()
  {
    .get instance class [netstandard/*23000001*/]System.Collections.Generic.IDictionary`2/*0100001A*/<object,object> Microsoft.Extensions.Hosting.HostBuilder/*0200000D*/::get_Properties() /* 06000010 */
  } // end of property HostBuilder::Properties
} // end of class Microsoft.Extensions.Hosting.HostBuilder

.class /*0200000E*/ public abstract auto ansi sealed beforefieldinit Microsoft.Extensions.Hosting.HostingHostBuilderExtensions
       extends [netstandard/*23000001*/]System.Object/*01000018*/
{
  .custom /*0C000021:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000018*/ public hidebysig static 
          class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          ConfigureAppConfiguration(class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ hostBuilder,
                                    class [netstandard/*23000001*/]System.Action`1/*0100001F*/<class [Microsoft.Extensions.Configuration.Abstractions/*23000003*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*0100001E*/> configureDelegate) cil managed
  // SIG: 00 02 12 65 12 65 15 12 7D 01 12 79
  {
    .custom /*0C000027:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingHostBuilderExtensions::ConfigureAppConfiguration

  .method /*06000019*/ public hidebysig static 
          class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          ConfigureContainer<TContainerBuilder>(class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ hostBuilder,
                                                class [netstandard/*23000001*/]System.Action`1/*0100001F*/<!!TContainerBuilder> configureDelegate) cil managed
  // SIG: 10 01 02 12 65 12 65 15 12 7D 01 1E 00
  {
    .custom /*0C000028:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingHostBuilderExtensions::ConfigureContainer

  .method /*0600001A*/ public hidebysig static 
          class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          ConfigureDefaults(class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ builder,
                            string[] args) cil managed
  // SIG: 00 02 12 65 12 65 1D 0E
  {
    .custom /*0C000029:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingHostBuilderExtensions::ConfigureDefaults

  .method /*0600001B*/ public hidebysig static 
          class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          ConfigureHostOptions(class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ hostBuilder,
                               class [netstandard/*23000001*/]System.Action`2/*0100001C*/<class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.HostBuilderContext/*0100001D*/,class Microsoft.Extensions.Hosting.HostOptions/*0200000F*/> configureOptions) cil managed
  // SIG: 00 02 12 65 12 65 15 12 71 02 12 75 12 3C
  {
    .custom /*0C00002A:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingHostBuilderExtensions::ConfigureHostOptions

  .method /*0600001C*/ public hidebysig static 
          class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          ConfigureHostOptions(class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ hostBuilder,
                               class [netstandard/*23000001*/]System.Action`1/*0100001F*/<class Microsoft.Extensions.Hosting.HostOptions/*0200000F*/> configureOptions) cil managed
  // SIG: 00 02 12 65 12 65 15 12 7D 01 12 3C
  {
    .custom /*0C00002B:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingHostBuilderExtensions::ConfigureHostOptions

  .method /*0600001D*/ public hidebysig static 
          class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          ConfigureLogging(class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ hostBuilder,
                           class [netstandard/*23000001*/]System.Action`2/*0100001C*/<class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.HostBuilderContext/*0100001D*/,class [Microsoft.Extensions.Logging/*23000005*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000023*/> configureLogging) cil managed
  // SIG: 00 02 12 65 12 65 15 12 71 02 12 75 12 80 8D
  {
    .custom /*0C00002C:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingHostBuilderExtensions::ConfigureLogging

  .method /*0600001E*/ public hidebysig static 
          class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          ConfigureLogging(class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ hostBuilder,
                           class [netstandard/*23000001*/]System.Action`1/*0100001F*/<class [Microsoft.Extensions.Logging/*23000005*/]Microsoft.Extensions.Logging.ILoggingBuilder/*01000023*/> configureLogging) cil managed
  // SIG: 00 02 12 65 12 65 15 12 7D 01 12 80 8D
  {
    .custom /*0C00002D:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingHostBuilderExtensions::ConfigureLogging

  .method /*0600001F*/ public hidebysig static 
          class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          ConfigureServices(class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ hostBuilder,
                            class [netstandard/*23000001*/]System.Action`1/*0100001F*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000004*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000020*/> configureDelegate) cil managed
  // SIG: 00 02 12 65 12 65 15 12 7D 01 12 80 81
  {
    .custom /*0C00002E:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingHostBuilderExtensions::ConfigureServices

  .method /*06000020*/ public hidebysig static 
          class [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000024*/ 
          RunConsoleAsync(class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ hostBuilder,
                          class [netstandard/*23000001*/]System.Action`1/*0100001F*/<class Microsoft.Extensions.Hosting.ConsoleLifetimeOptions/*0200000B*/> configureOptions,
                          [opt] valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*01000025*/ cancellationToken) cil managed
  // SIG: 00 03 12 80 91 12 65 15 12 7D 01 12 2C 11 80 95
  {
    .custom /*0C00002F:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000030:06000005*/ instance void System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*02000005*/::.ctor(string) /* 06000005 */ = ( 01 00 07 61 6E 64 72 6F 69 64 00 00 )             // ...android..
    .custom /*0C000031:06000005*/ instance void System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*02000005*/::.ctor(string) /* 06000005 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .custom /*0C000032:06000005*/ instance void System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*02000005*/::.ctor(string) /* 06000005 */ = ( 01 00 03 69 6F 73 00 00 )                         // ...ios..
    .custom /*0C000033:06000005*/ instance void System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*02000005*/::.ctor(string) /* 06000005 */ = ( 01 00 04 74 76 6F 73 00 00 )                      // ...tvos..
    .param [3]/*08000022*/  = nullref
    // Method begins at RVA 0x20f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingHostBuilderExtensions::RunConsoleAsync

  .method /*06000021*/ public hidebysig static 
          class [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000024*/ 
          RunConsoleAsync(class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ hostBuilder,
                          [opt] valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*01000025*/ cancellationToken) cil managed
  // SIG: 00 02 12 80 91 12 65 11 80 95
  {
    .custom /*0C000034:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000035:06000005*/ instance void System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*02000005*/::.ctor(string) /* 06000005 */ = ( 01 00 07 61 6E 64 72 6F 69 64 00 00 )             // ...android..
    .custom /*0C000036:06000005*/ instance void System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*02000005*/::.ctor(string) /* 06000005 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .custom /*0C000037:06000005*/ instance void System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*02000005*/::.ctor(string) /* 06000005 */ = ( 01 00 03 69 6F 73 00 00 )                         // ...ios..
    .custom /*0C000038:06000005*/ instance void System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*02000005*/::.ctor(string) /* 06000005 */ = ( 01 00 04 74 76 6F 73 00 00 )                      // ...tvos..
    .param [2]/*08000024*/  = nullref
    // Method begins at RVA 0x20f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingHostBuilderExtensions::RunConsoleAsync

  .method /*06000022*/ public hidebysig static 
          class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          UseConsoleLifetime(class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ hostBuilder) cil managed
  // SIG: 00 01 12 65 12 65
  {
    .custom /*0C000039:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00003A:06000005*/ instance void System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*02000005*/::.ctor(string) /* 06000005 */ = ( 01 00 07 61 6E 64 72 6F 69 64 00 00 )             // ...android..
    .custom /*0C00003B:06000005*/ instance void System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*02000005*/::.ctor(string) /* 06000005 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .custom /*0C00003C:06000005*/ instance void System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*02000005*/::.ctor(string) /* 06000005 */ = ( 01 00 03 69 6F 73 00 00 )                         // ...ios..
    .custom /*0C00003D:06000005*/ instance void System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*02000005*/::.ctor(string) /* 06000005 */ = ( 01 00 04 74 76 6F 73 00 00 )                      // ...tvos..
    // Method begins at RVA 0x20fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingHostBuilderExtensions::UseConsoleLifetime

  .method /*06000023*/ public hidebysig static 
          class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          UseConsoleLifetime(class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ hostBuilder,
                             class [netstandard/*23000001*/]System.Action`1/*0100001F*/<class Microsoft.Extensions.Hosting.ConsoleLifetimeOptions/*0200000B*/> configureOptions) cil managed
  // SIG: 00 02 12 65 12 65 15 12 7D 01 12 2C
  {
    .custom /*0C00003E:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00003F:06000005*/ instance void System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*02000005*/::.ctor(string) /* 06000005 */ = ( 01 00 07 61 6E 64 72 6F 69 64 00 00 )             // ...android..
    .custom /*0C000040:06000005*/ instance void System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*02000005*/::.ctor(string) /* 06000005 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .custom /*0C000041:06000005*/ instance void System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*02000005*/::.ctor(string) /* 06000005 */ = ( 01 00 03 69 6F 73 00 00 )                         // ...ios..
    .custom /*0C000042:06000005*/ instance void System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*02000005*/::.ctor(string) /* 06000005 */ = ( 01 00 04 74 76 6F 73 00 00 )                      // ...tvos..
    // Method begins at RVA 0x20ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingHostBuilderExtensions::UseConsoleLifetime

  .method /*06000024*/ public hidebysig static 
          class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          UseContentRoot(class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ hostBuilder,
                         string contentRoot) cil managed
  // SIG: 00 02 12 65 12 65 0E
  {
    .custom /*0C000043:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2102
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingHostBuilderExtensions::UseContentRoot

  .method /*06000025*/ public hidebysig static 
          class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          UseDefaultServiceProvider(class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ hostBuilder,
                                    class [netstandard/*23000001*/]System.Action`1/*0100001F*/<class [Microsoft.Extensions.DependencyInjection/*23000006*/]Microsoft.Extensions.DependencyInjection.ServiceProviderOptions/*01000026*/> configure) cil managed
  // SIG: 00 02 12 65 12 65 15 12 7D 01 12 80 99
  {
    .custom /*0C000044:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2105
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingHostBuilderExtensions::UseDefaultServiceProvider

  .method /*06000026*/ public hidebysig static 
          class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          UseDefaultServiceProvider(class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ hostBuilder,
                                    class [netstandard/*23000001*/]System.Action`2/*0100001C*/<class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.HostBuilderContext/*0100001D*/,class [Microsoft.Extensions.DependencyInjection/*23000006*/]Microsoft.Extensions.DependencyInjection.ServiceProviderOptions/*01000026*/> configure) cil managed
  // SIG: 00 02 12 65 12 65 15 12 71 02 12 75 12 80 99
  {
    .custom /*0C000045:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2108
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingHostBuilderExtensions::UseDefaultServiceProvider

  .method /*06000027*/ public hidebysig static 
          class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ 
          UseEnvironment(class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostBuilder/*01000019*/ hostBuilder,
                         string environment) cil managed
  // SIG: 00 02 12 65 12 65 0E
  {
    .custom /*0C000046:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x210b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingHostBuilderExtensions::UseEnvironment

} // end of class Microsoft.Extensions.Hosting.HostingHostBuilderExtensions

.class /*0200000F*/ public auto ansi beforefieldinit Microsoft.Extensions.Hosting.HostOptions
       extends [netstandard/*23000001*/]System.Object/*01000018*/
{
  .method /*06000028*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x210e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [netstandard/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HostOptions::.ctor

  .method /*06000029*/ public hidebysig specialname 
          instance valuetype [netstandard/*23000001*/]System.TimeSpan/*01000027*/ 
          get_ShutdownTimeout() cil managed
  // SIG: 20 00 11 80 9D
  {
    // Method begins at RVA 0x2116
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostOptions::get_ShutdownTimeout

  .method /*0600002A*/ public hidebysig specialname 
          instance void  set_ShutdownTimeout(valuetype [netstandard/*23000001*/]System.TimeSpan/*01000027*/ 'value') cil managed
  // SIG: 20 01 01 11 80 9D
  {
    // Method begins at RVA 0x2119
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HostOptions::set_ShutdownTimeout

  .method /*0600002B*/ public hidebysig specialname 
          instance valuetype Microsoft.Extensions.Hosting.BackgroundServiceExceptionBehavior/*0200000A*/ 
          get_BackgroundServiceExceptionBehavior() cil managed
  // SIG: 20 00 11 28
  {
    // Method begins at RVA 0x211b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostOptions::get_BackgroundServiceExceptionBehavior

  .method /*0600002C*/ public hidebysig specialname 
          instance void  set_BackgroundServiceExceptionBehavior(valuetype Microsoft.Extensions.Hosting.BackgroundServiceExceptionBehavior/*0200000A*/ 'value') cil managed
  // SIG: 20 01 01 11 28
  {
    // Method begins at RVA 0x211e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HostOptions::set_BackgroundServiceExceptionBehavior

  .property /*17000005*/ instance callconv(8) valuetype [netstandard/*23000001*/]System.TimeSpan/*01000027*/
          ShutdownTimeout()
  {
    .get instance valuetype [netstandard/*23000001*/]System.TimeSpan/*01000027*/ Microsoft.Extensions.Hosting.HostOptions/*0200000F*/::get_ShutdownTimeout() /* 06000029 */
    .set instance void Microsoft.Extensions.Hosting.HostOptions/*0200000F*/::set_ShutdownTimeout(valuetype [netstandard/*23000001*/]System.TimeSpan/*01000027*/) /* 0600002A */
  } // end of property HostOptions::ShutdownTimeout
  .property /*17000006*/ instance callconv(8) valuetype Microsoft.Extensions.Hosting.BackgroundServiceExceptionBehavior/*0200000A*/
          BackgroundServiceExceptionBehavior()
  {
    .get instance valuetype Microsoft.Extensions.Hosting.BackgroundServiceExceptionBehavior/*0200000A*/ Microsoft.Extensions.Hosting.HostOptions/*0200000F*/::get_BackgroundServiceExceptionBehavior() /* 0600002B */
    .set instance void Microsoft.Extensions.Hosting.HostOptions/*0200000F*/::set_BackgroundServiceExceptionBehavior(valuetype Microsoft.Extensions.Hosting.BackgroundServiceExceptionBehavior/*0200000A*/) /* 0600002C */
  } // end of property HostOptions::BackgroundServiceExceptionBehavior
} // end of class Microsoft.Extensions.Hosting.HostOptions

.class /*02000010*/ public auto ansi beforefieldinit Microsoft.Extensions.Hosting.Internal.ApplicationLifetime
       extends [netstandard/*23000001*/]System.Object/*01000018*/
       implements [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IApplicationLifetime/*01000028*/,
                  [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostApplicationLifetime/*01000029*/
{
  .method /*0600002D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Logging.Abstractions/*23000007*/]Microsoft.Extensions.Logging.ILogger`1/*0100002A*/<class Microsoft.Extensions.Hosting.Internal.ApplicationLifetime/*02000010*/> logger) cil managed
  // SIG: 20 01 01 15 12 80 A9 01 12 40
  {
    // Method begins at RVA 0x2120
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [netstandard/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ApplicationLifetime::.ctor

  .method /*0600002E*/ public hidebysig newslot specialname virtual final 
          instance valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*01000025*/ 
          get_ApplicationStarted() cil managed
  // SIG: 20 00 11 80 95
  {
    // Method begins at RVA 0x2128
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ApplicationLifetime::get_ApplicationStarted

  .method /*0600002F*/ public hidebysig newslot specialname virtual final 
          instance valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*01000025*/ 
          get_ApplicationStopped() cil managed
  // SIG: 20 00 11 80 95
  {
    // Method begins at RVA 0x212b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ApplicationLifetime::get_ApplicationStopped

  .method /*06000030*/ public hidebysig newslot specialname virtual final 
          instance valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*01000025*/ 
          get_ApplicationStopping() cil managed
  // SIG: 20 00 11 80 95
  {
    // Method begins at RVA 0x212e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ApplicationLifetime::get_ApplicationStopping

  .method /*06000031*/ public hidebysig instance void 
          NotifyStarted() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2131
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ApplicationLifetime::NotifyStarted

  .method /*06000032*/ public hidebysig instance void 
          NotifyStopped() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2133
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ApplicationLifetime::NotifyStopped

  .method /*06000033*/ public hidebysig newslot virtual final 
          instance void  StopApplication() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2135
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ApplicationLifetime::StopApplication

  .property /*17000007*/ instance callconv(8) valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*01000025*/
          ApplicationStarted()
  {
    .get instance valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*01000025*/ Microsoft.Extensions.Hosting.Internal.ApplicationLifetime/*02000010*/::get_ApplicationStarted() /* 0600002E */
  } // end of property ApplicationLifetime::ApplicationStarted
  .property /*17000008*/ instance callconv(8) valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*01000025*/
          ApplicationStopped()
  {
    .get instance valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*01000025*/ Microsoft.Extensions.Hosting.Internal.ApplicationLifetime/*02000010*/::get_ApplicationStopped() /* 0600002F */
  } // end of property ApplicationLifetime::ApplicationStopped
  .property /*17000009*/ instance callconv(8) valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*01000025*/
          ApplicationStopping()
  {
    .get instance valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*01000025*/ Microsoft.Extensions.Hosting.Internal.ApplicationLifetime/*02000010*/::get_ApplicationStopping() /* 06000030 */
  } // end of property ApplicationLifetime::ApplicationStopping
} // end of class Microsoft.Extensions.Hosting.Internal.ApplicationLifetime

.class /*02000011*/ public auto ansi beforefieldinit Microsoft.Extensions.Hosting.Internal.ConsoleLifetime
       extends [netstandard/*23000001*/]System.Object/*01000018*/
       implements [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostLifetime/*0100002B*/,
                  [netstandard/*23000001*/]System.IDisposable/*0100002C*/
{
  .custom /*0C000022:06000005*/ instance void System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*02000005*/::.ctor(string) /* 06000005 */ = ( 01 00 07 61 6E 64 72 6F 69 64 00 00 )             // ...android..
  .custom /*0C000023:06000005*/ instance void System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*02000005*/::.ctor(string) /* 06000005 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .custom /*0C000024:06000005*/ instance void System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*02000005*/::.ctor(string) /* 06000005 */ = ( 01 00 03 69 6F 73 00 00 )                         // ...ios..
  .custom /*0C000025:06000005*/ instance void System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*02000005*/::.ctor(string) /* 06000005 */ = ( 01 00 04 74 76 6F 73 00 00 )                      // ...tvos..
  .method /*06000034*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.IOptions`1/*0100002D*/<class Microsoft.Extensions.Hosting.ConsoleLifetimeOptions/*0200000B*/> options,
                               class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostEnvironment/*0100002E*/ environment,
                               class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostApplicationLifetime/*01000029*/ applicationLifetime,
                               class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.IOptions`1/*0100002D*/<class Microsoft.Extensions.Hosting.HostOptions/*0200000F*/> hostOptions) cil managed
  // SIG: 20 04 01 15 12 80 B5 01 12 2C 12 80 B9 12 80 A5 15 12 80 B5 01 12 3C
  {
    // Method begins at RVA 0x2137
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [netstandard/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ConsoleLifetime::.ctor

  .method /*06000035*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.IOptions`1/*0100002D*/<class Microsoft.Extensions.Hosting.ConsoleLifetimeOptions/*0200000B*/> options,
                               class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostEnvironment/*0100002E*/ environment,
                               class [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostApplicationLifetime/*01000029*/ applicationLifetime,
                               class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.IOptions`1/*0100002D*/<class Microsoft.Extensions.Hosting.HostOptions/*0200000F*/> hostOptions,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000007*/]Microsoft.Extensions.Logging.ILoggerFactory/*0100002F*/ loggerFactory) cil managed
  // SIG: 20 05 01 15 12 80 B5 01 12 2C 12 80 B9 12 80 A5 15 12 80 B5 01 12 3C 12 80 BD
  {
    // Method begins at RVA 0x213f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [netstandard/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ConsoleLifetime::.ctor

  .method /*06000036*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2147
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConsoleLifetime::Dispose

  .method /*06000037*/ public hidebysig newslot virtual final 
          instance class [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000024*/ 
          StopAsync(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*01000025*/ cancellationToken) cil managed
  // SIG: 20 01 12 80 91 11 80 95
  {
    // Method begins at RVA 0x2149
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLifetime::StopAsync

  .method /*06000038*/ public hidebysig newslot virtual final 
          instance class [netstandard/*23000001*/]System.Threading.Tasks.Task/*01000024*/ 
          WaitForStartAsync(valuetype [netstandard/*23000001*/]System.Threading.CancellationToken/*01000025*/ cancellationToken) cil managed
  // SIG: 20 01 12 80 91 11 80 95
  {
    // Method begins at RVA 0x214c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConsoleLifetime::WaitForStartAsync

} // end of class Microsoft.Extensions.Hosting.Internal.ConsoleLifetime

.class /*02000012*/ public auto ansi beforefieldinit Microsoft.Extensions.Hosting.Internal.HostingEnvironment
       extends [netstandard/*23000001*/]System.Object/*01000018*/
       implements [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostEnvironment/*0100002E*/,
                  [Microsoft.Extensions.Hosting.Abstractions/*23000002*/]Microsoft.Extensions.Hosting.IHostingEnvironment/*01000030*/
{
  .method /*06000039*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x214f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [netstandard/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HostingEnvironment::.ctor

  .method /*0600003A*/ public hidebysig newslot specialname virtual final 
          instance string  get_ApplicationName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2157
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEnvironment::get_ApplicationName

  .method /*0600003B*/ public hidebysig newslot specialname virtual final 
          instance void  set_ApplicationName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x215a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HostingEnvironment::set_ApplicationName

  .method /*0600003C*/ public hidebysig newslot specialname virtual final 
          instance class [Microsoft.Extensions.FileProviders.Abstractions/*23000009*/]Microsoft.Extensions.FileProviders.IFileProvider/*01000031*/ 
          get_ContentRootFileProvider() cil managed
  // SIG: 20 00 12 80 C5
  {
    // Method begins at RVA 0x215c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEnvironment::get_ContentRootFileProvider

  .method /*0600003D*/ public hidebysig newslot specialname virtual final 
          instance void  set_ContentRootFileProvider(class [Microsoft.Extensions.FileProviders.Abstractions/*23000009*/]Microsoft.Extensions.FileProviders.IFileProvider/*01000031*/ 'value') cil managed
  // SIG: 20 01 01 12 80 C5
  {
    // Method begins at RVA 0x215f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HostingEnvironment::set_ContentRootFileProvider

  .method /*0600003E*/ public hidebysig newslot specialname virtual final 
          instance string  get_ContentRootPath() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2161
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEnvironment::get_ContentRootPath

  .method /*0600003F*/ public hidebysig newslot specialname virtual final 
          instance void  set_ContentRootPath(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2164
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HostingEnvironment::set_ContentRootPath

  .method /*06000040*/ public hidebysig newslot specialname virtual final 
          instance string  get_EnvironmentName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2166
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HostingEnvironment::get_EnvironmentName

  .method /*06000041*/ public hidebysig newslot specialname virtual final 
          instance void  set_EnvironmentName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2169
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HostingEnvironment::set_EnvironmentName

  .property /*1700000A*/ instance callconv(8) string
          ApplicationName()
  {
    .get instance string Microsoft.Extensions.Hosting.Internal.HostingEnvironment/*02000012*/::get_ApplicationName() /* 0600003A */
    .set instance void Microsoft.Extensions.Hosting.Internal.HostingEnvironment/*02000012*/::set_ApplicationName(string) /* 0600003B */
  } // end of property HostingEnvironment::ApplicationName
  .property /*1700000B*/ instance callconv(8) class [Microsoft.Extensions.FileProviders.Abstractions/*23000009*/]Microsoft.Extensions.FileProviders.IFileProvider/*01000031*/
          ContentRootFileProvider()
  {
    .get instance class [Microsoft.Extensions.FileProviders.Abstractions/*23000009*/]Microsoft.Extensions.FileProviders.IFileProvider/*01000031*/ Microsoft.Extensions.Hosting.Internal.HostingEnvironment/*02000012*/::get_ContentRootFileProvider() /* 0600003C */
    .set instance void Microsoft.Extensions.Hosting.Internal.HostingEnvironment/*02000012*/::set_ContentRootFileProvider(class [Microsoft.Extensions.FileProviders.Abstractions/*23000009*/]Microsoft.Extensions.FileProviders.IFileProvider/*01000031*/) /* 0600003D */
  } // end of property HostingEnvironment::ContentRootFileProvider
  .property /*1700000C*/ instance callconv(8) string
          ContentRootPath()
  {
    .get instance string Microsoft.Extensions.Hosting.Internal.HostingEnvironment/*02000012*/::get_ContentRootPath() /* 0600003E */
    .set instance void Microsoft.Extensions.Hosting.Internal.HostingEnvironment/*02000012*/::set_ContentRootPath(string) /* 0600003F */
  } // end of property HostingEnvironment::ContentRootPath
  .property /*1700000D*/ instance callconv(8) string
          EnvironmentName()
  {
    .get instance string Microsoft.Extensions.Hosting.Internal.HostingEnvironment/*02000012*/::get_EnvironmentName() /* 06000040 */
    .set instance void Microsoft.Extensions.Hosting.Internal.HostingEnvironment/*02000012*/::set_EnvironmentName(string) /* 06000041 */
  } // end of property HostingEnvironment::EnvironmentName
} // end of class Microsoft.Extensions.Hosting.Internal.HostingEnvironment

.class /*02000013*/ public abstract auto ansi sealed beforefieldinit Microsoft.Extensions.DependencyInjection.OptionsBuilderExtensions
       extends [netstandard/*23000001*/]System.Object/*01000018*/
{
  .custom /*0C000026:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000042*/ public hidebysig static 
          class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000032*/<!!TOptions> 
          ValidateOnStart<class TOptions>(class [Microsoft.Extensions.Options/*23000008*/]Microsoft.Extensions.Options.OptionsBuilder`1/*01000032*/<!!TOptions> optionsBuilder) cil managed
  // SIG: 10 01 01 15 12 80 C9 01 1E 00 15 12 80 C9 01 1E 00
  {
    .custom /*0C000047:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOptions /*2A000005*/ 
      .custom /*0C00001B:06000008*/ instance void System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*02000008*/::.ctor(valuetype System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*02000009*/) /* 06000008 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x216b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OptionsBuilderExtensions::ValidateOnStart

} // end of class Microsoft.Extensions.DependencyInjection.OptionsBuilderExtensions


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
