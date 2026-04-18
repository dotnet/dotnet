


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
// Time-date stamp:            0xfa07ee3b
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00000e00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00002d86
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
// Checksum:                       0x0000df24
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00002d33 [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x00000594] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00001800 [0x00002470] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00002c00 [0x00000054] address [size] of Debug Directory:           
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
//              0x00000d8c Virtual Size
//              0x00002000 Virtual Address
//              0x00000e00 Size of Raw Data
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
//              0x00000594 Virtual Size
//              0x00004000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00001000 Pointer to Raw Data
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
//              0x00001600 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00002000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000d88
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00002d75 Import Name Table
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
// 0x00002080 [0x00000b00] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00002b80 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x0000034c Size
//              '#~' Name
//    Stream 2:
//              0x000003b8 Offset
//              0x00000444 Size
//              '#Strings' Name
//    Stream 3:
//              0x000007fc Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00000800 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00000810 Offset
//              0x000002f0 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000000901a01747 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 15472
// PE header size       : 512 (496 used)    ( 3.31%)
// PE additional info   : 11011             (71.17%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.47%)
// CLR meta-data size  : 2816               (18.20%)
// CLR additional info : 128                ( 0.83%)
// CLR method headers  : 11                 ( 0.07%)
// Managed code         : 35                ( 0.23%)
// Data                 : 2048              (13.24%)
// Unaccounted          : -1161             (-7.50%)

// Num.of PE sections   : 3
//   .text    - 3584
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 2816
//   Module        -    1 (10 bytes)
//   TypeDef       -    4 (56 bytes)      0 interfaces, 0 explicit layout
//   TypeRef       -   23 (138 bytes)
//   MethodDef     -   11 (154 bytes)     0 abstract, 0 native, 11 bodies
//   MemberRef     -   18 (108 bytes)
//   ParamDef      -    8 (48 bytes)
//   CustomAttribute-   24 (144 bytes)
//   InterfaceImpl -    1 (4 bytes)
//   PropertyMap   -    1 (4 bytes)
//   Property      -    1 (6 bytes)
//   MethodSemantic-    2 (12 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    3 (60 bytes)
//   Strings       -  1090 bytes
//   Blobs         -   752 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   188 bytes

// CLR additional info : 128

// CLR method headers : 11
//   Num.of method bodies  - 11
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 11

// Managed code : 35
//   Ave method size - 3
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EnvironmentVariablesExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class EnvironmentVariablesConfigurationProvider (public) (auto) (ansi) 
// Class EnvironmentVariablesConfigurationSource (public) (auto) (ansi) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ netstandard
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 2:0:0:0
}
.assembly extern /*23000002*/ Microsoft.Extensions.Configuration.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ Microsoft.Extensions.Configuration
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly /*20000001*/ noplatform Microsoft.Extensions.Configuration.EnvironmentVariables
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
  .custom /*0C000009:0A000008*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 37 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..7Microsoft.Ext
                                                                                                                                                                     65 6E 73 69 6F 6E 73 2E 43 6F 6E 66 69 67 75 72   // ensions.Configur
                                                                                                                                                                     61 74 69 6F 6E 2E 45 6E 76 69 72 6F 6E 6D 65 6E   // ation.Environmen
                                                                                                                                                                     74 56 61 72 69 61 62 6C 65 73 00 00 )             // tVariables..
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
  .custom /*0C000010:0A00000C*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 37 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..7Microsoft.Ext
                                                                                                                                                                    65 6E 73 69 6F 6E 73 2E 43 6F 6E 66 69 67 75 72   // ensions.Configur
                                                                                                                                                                    61 74 69 6F 6E 2E 45 6E 76 69 72 6F 6E 6D 65 6E   // ation.Environmen
                                                                                                                                                                    74 56 61 72 69 61 62 6C 65 73 00 00 )             // tVariables..
  .custom /*0C000011:0A00000D*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 36 2E 30 2E 32 32 32 2E 36 34 30 36 00   // ...6.0.222.6406.
                                                                                                                                                                    00 ) 
  .custom /*0C000012:0A00000E*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 36 2E 30 2E 32 2B 38 33 39 63 64 66 62   // ...6.0.2+839cdfb
                                                                                                                                                                             30 65 63 63 61 35 65 30 62 65 33 64 62 63 63 64   // 0ecca5e0be3dbccd
                                                                                                                                                                             39 32 36 65 37 36 35 31 65 66 35 30 66 64 66 31   // 926e7651ef50fdf1
                                                                                                                                                                             30 00 00 )                                        // 0..
  .custom /*0C000013:0A00000F*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                4E 45 54 00 00 )                                  // NET..
  .custom /*0C000014:0A000010*/ instance void [netstandard/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 37 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..7Microsoft.Ext
                                                                                                                                                              65 6E 73 69 6F 6E 73 2E 43 6F 6E 66 69 67 75 72   // ensions.Configur
                                                                                                                                                              61 74 69 6F 6E 2E 45 6E 76 69 72 6F 6E 6D 65 6E   // ation.Environmen
                                                                                                                                                              74 56 61 72 69 61 62 6C 65 73 00 00 )             // tVariables..
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
.module Microsoft.Extensions.Configuration.EnvironmentVariables.dll
// MVID: {8cd4d490-3af6-4383-bca4-2cc8f23bd599}
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public abstract auto ansi sealed beforefieldinit Microsoft.Extensions.Configuration.EnvironmentVariablesExtensions
       extends [netstandard/*23000001*/]System.Object/*01000012*/
{
  .custom /*0C000017:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig static 
          class [Microsoft.Extensions.Configuration.Abstractions/*23000002*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*01000013*/ 
          AddEnvironmentVariables(class [Microsoft.Extensions.Configuration.Abstractions/*23000002*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*01000013*/ configurationBuilder) cil managed
  // SIG: 00 01 12 4D 12 4D
  {
    .custom /*0C000001:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnvironmentVariablesExtensions::AddEnvironmentVariables

  .method /*06000002*/ public hidebysig static 
          class [Microsoft.Extensions.Configuration.Abstractions/*23000002*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*01000013*/ 
          AddEnvironmentVariables(class [Microsoft.Extensions.Configuration.Abstractions/*23000002*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*01000013*/ builder,
                                  class [netstandard/*23000001*/]System.Action`1/*01000014*/<class Microsoft.Extensions.Configuration.EnvironmentVariables.EnvironmentVariablesConfigurationSource/*02000004*/> configureSource) cil managed
  // SIG: 00 02 12 4D 12 4D 15 12 51 01 12 10
  {
    .custom /*0C000016:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2053
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnvironmentVariablesExtensions::AddEnvironmentVariables

  .method /*06000003*/ public hidebysig static 
          class [Microsoft.Extensions.Configuration.Abstractions/*23000002*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*01000013*/ 
          AddEnvironmentVariables(class [Microsoft.Extensions.Configuration.Abstractions/*23000002*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*01000013*/ configurationBuilder,
                                  string prefix) cil managed
  // SIG: 00 02 12 4D 12 4D 0E
  {
    .custom /*0C000018:0A000001*/ instance void [netstandard/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2056
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnvironmentVariablesExtensions::AddEnvironmentVariables

} // end of class Microsoft.Extensions.Configuration.EnvironmentVariablesExtensions

.class /*02000003*/ public auto ansi beforefieldinit Microsoft.Extensions.Configuration.EnvironmentVariables.EnvironmentVariablesConfigurationProvider
       extends [Microsoft.Extensions.Configuration/*23000003*/]Microsoft.Extensions.Configuration.ConfigurationProvider/*01000015*/
{
  .method /*06000004*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2059
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000011       */ call       instance void [Microsoft.Extensions.Configuration/*23000003*/]Microsoft.Extensions.Configuration.ConfigurationProvider/*01000015*/::.ctor() /* 0A000011 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EnvironmentVariablesConfigurationProvider::.ctor

  .method /*06000005*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string prefix) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2061
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000011       */ call       instance void [Microsoft.Extensions.Configuration/*23000003*/]Microsoft.Extensions.Configuration.ConfigurationProvider/*01000015*/::.ctor() /* 0A000011 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EnvironmentVariablesConfigurationProvider::.ctor

  .method /*06000006*/ public hidebysig virtual 
          instance void  Load() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2069
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EnvironmentVariablesConfigurationProvider::Load

  .method /*06000007*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x206b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnvironmentVariablesConfigurationProvider::ToString

} // end of class Microsoft.Extensions.Configuration.EnvironmentVariables.EnvironmentVariablesConfigurationProvider

.class /*02000004*/ public auto ansi beforefieldinit Microsoft.Extensions.Configuration.EnvironmentVariables.EnvironmentVariablesConfigurationSource
       extends [netstandard/*23000001*/]System.Object/*01000012*/
       implements [Microsoft.Extensions.Configuration.Abstractions/*23000002*/]Microsoft.Extensions.Configuration.IConfigurationSource/*01000016*/
{
  .method /*06000008*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x206e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000012       */ call       instance void [netstandard/*23000001*/]System.Object/*01000012*/::.ctor() /* 0A000012 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EnvironmentVariablesConfigurationSource::.ctor

  .method /*06000009*/ public hidebysig specialname 
          instance string  get_Prefix() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2076
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnvironmentVariablesConfigurationSource::get_Prefix

  .method /*0600000A*/ public hidebysig specialname 
          instance void  set_Prefix(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2079
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EnvironmentVariablesConfigurationSource::set_Prefix

  .method /*0600000B*/ public hidebysig newslot virtual final 
          instance class [Microsoft.Extensions.Configuration.Abstractions/*23000002*/]Microsoft.Extensions.Configuration.IConfigurationProvider/*01000017*/ 
          Build(class [Microsoft.Extensions.Configuration.Abstractions/*23000002*/]Microsoft.Extensions.Configuration.IConfigurationBuilder/*01000013*/ builder) cil managed
  // SIG: 20 01 12 5D 12 4D
  {
    // Method begins at RVA 0x207b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnvironmentVariablesConfigurationSource::Build

  .property /*17000001*/ instance callconv(8) string
          Prefix()
  {
    .get instance string Microsoft.Extensions.Configuration.EnvironmentVariables.EnvironmentVariablesConfigurationSource/*02000004*/::get_Prefix() /* 06000009 */
    .set instance void Microsoft.Extensions.Configuration.EnvironmentVariables.EnvironmentVariablesConfigurationSource/*02000004*/::set_Prefix(string) /* 0600000A */
  } // end of property EnvironmentVariablesConfigurationSource::Prefix
} // end of class Microsoft.Extensions.Configuration.EnvironmentVariables.EnvironmentVariablesConfigurationSource


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
