


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
// Time-date stamp:            0xd878e2af
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
// Addr. of entry point:           0x00003b56
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
// Checksum:                       0x00012328
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00003b04 [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x000004e8] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002600 [0x00002930] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00003ae8 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00001b5c Virtual Size
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
//              0x000004e8 Virtual Size
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
//              Entry 1: Type 0x3 Offset 0x00000b58
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00003b46 Import Name Table
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
// 0x000020f0 [0x00001978] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00003a68 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000d1c Size
//              '#~' Name
//    Stream 2:
//              0x00000d88 Offset
//              0x000006d4 Size
//              '#Strings' Name
//    Stream 3:
//              0x0000145c Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00001460 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001470 Offset
//              0x00000508 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000008900001547 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 20272
// PE header size       : 512 (496 used)    ( 2.53%)
// PE additional info   : 11999             (59.19%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.36%)
// CLR meta-data size  : 6520               (32.16%)
// CLR additional info : 128                ( 0.63%)
// CLR method headers  : 53                 ( 0.26%)
// Managed code         : 106               ( 0.52%)
// Data                 : 2048              (10.10%)
// Unaccounted          : -1166             (-5.75%)

// Num.of PE sections   : 3
//   .text    - 7168
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 6520
//   Module        -    1 (10 bytes)
//   TypeDef       -    8 (112 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   39 (234 bytes)
//   MethodDef     -   53 (742 bytes)     0 abstract, 0 native, 53 bodies
//   MemberRef     -   24 (144 bytes)
//   ParamDef      -  116 (696 bytes)
//   CustomAttribute-  216 (1296 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    1 (20 bytes)
//   ExportedType  -    1 (14 bytes)
//   Strings       -  1745 bytes
//   Blobs         -  1288 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   177 bytes

// CLR additional info : 128

// CLR method headers : 53
//   Num.of method bodies  - 53
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 53

// Managed code : 106
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class AssemblyExtensions             (public) (abstract) (auto) (ansi) (sealed) 
// Class EventInfoExtensions            (public) (abstract) (auto) (ansi) (sealed) 
// Class MemberInfoExtensions           (public) (abstract) (auto) (ansi) (sealed) 
// Class MethodInfoExtensions           (public) (abstract) (auto) (ansi) (sealed) 
// Class ModuleExtensions               (public) (abstract) (auto) (ansi) (sealed) 
// Class PropertyInfoExtensions         (public) (abstract) (auto) (ansi) (sealed) 
// Class TypeExtensions                 (public) (abstract) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly /*20000001*/ noplatform System.Reflection.TypeExtensions
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000006:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000007:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000002*/::.ctor(int32) /* 0A000002 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000008:0A000003*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000003*/::.ctor() /* 0A000003 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000009:0A000004*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*//DebuggingModes/*01000005*/) /* 0A000004 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C00000A:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000006*/::.ctor(string) /* 0A000005 */ = ( 01 00 19 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 31 30 2E 30 01 00 54 0E   // ersion=v10.0..T.
                                                                                                                                                                           14 46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61   // .FrameworkDispla
                                                                                                                                                                           79 4E 61 6D 65 09 2E 4E 45 54 20 31 30 2E 30 )    // yName..NET 10.0
  .custom /*0C00000B:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000007*/::.ctor() /* 0A000006 */ = ( 01 00 00 00 ) 
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000E:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 20 53 79 73 74 65 6D 2E 52 65 66 6C 65 63   // .. System.Reflec
                                                                                                                                                                        74 69 6F 6E 2E 54 79 70 65 45 78 74 65 6E 73 69   // tion.TypeExtensi
                                                                                                                                                                        6F 6E 73 00 00 )                                  // ons..
  .custom /*0C00000F:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000010:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C000011:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0F 49 73 41 6F 74 43 6F 6D 70 61 74 69 62   // ...IsAotCompatib
                                                                                                                                                                    6C 65 04 54 72 75 65 00 00 )                      // le.True..
  .custom /*0C000012:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000013:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000014:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 20 53 79 73 74 65 6D 2E 52 65 66 6C 65 63   // .. System.Reflec
                                                                                                                                                                       74 69 6F 6E 2E 54 79 70 65 45 78 74 65 6E 73 69   // tion.TypeExtensi
                                                                                                                                                                       6F 6E 73 00 00 )                                  // ons..
  .custom /*0C000015:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0D 31 30 2E 30 2E 32 35 2E 35 32 34 31 31   // ...10.0.25.52411
                                                                                                                                                                       00 00 ) 
  .custom /*0C000016:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2F 31 30 2E 30 2E 30 2B 62 30 66 33 34 64   // ../10.0.0+b0f34d
                                                                                                                                                                                35 31 66 63 63 63 36 39 66 64 33 33 34 32 35 33   // 51fccc69fd334253
                                                                                                                                                                                39 32 34 61 62 64 38 64 36 38 35 33 66 61 64 37   // 924abd8d6853fad7
                                                                                                                                                                                61 61 00 00 )                                     // aa..
  .custom /*0C000017:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000018:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 20 53 79 73 74 65 6D 2E 52 65 66 6C 65 63   // .. System.Reflec
                                                                                                                                                                 74 69 6F 6E 2E 54 79 70 65 45 78 74 65 6E 73 69   // tion.TypeExtensi
                                                                                                                                                                 6F 6E 73 00 00 )                                  // ons..
  .custom /*0C000019:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    20 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   //  https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 64 6F 74 6E 65   // com/dotnet/dotne
                                                                                                                                                                    74 00 00 )                                        // t..
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
.class extern /*27000001*/ forwarder System.Reflection.BindingFlags
{
  .assembly extern System.Runtime /*23000001*/ 
}
.module System.Reflection.TypeExtensions.dll
// MVID: {74053b29-d1a1-48d3-9949-1195bf23c0ce}
.custom /*0C000004:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000012*/::.ctor(int32) /* 0A000011 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000005:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000013*/::.ctor(bool) /* 0A000012 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public abstract auto ansi sealed beforefieldinit System.Reflection.AssemblyExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
{
  .custom /*0C00001C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00001E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000001*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Type/*0100001A*/[] 
          GetExportedTypes(class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001B*/ 'assembly') cil managed
  // SIG: 00 01 1D 12 69 12 6D
  {
    .custom /*0C000001:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000002:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000019*/::.ctor(string) /* 0A000016 */ = ( 01 00 16 54 79 70 65 73 20 6D 69 67 68 74 20 62   // ...Types might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 00 00 )                // e removed..
    .custom /*0C000003:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyExtensions::GetExportedTypes

  .method /*06000002*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.Module/*0100001C*/[] 
          GetModules(class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001B*/ 'assembly') cil managed
  // SIG: 00 01 1D 12 71 12 6D
  {
    .custom /*0C00001A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00001B:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2053
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyExtensions::GetModules

  .method /*06000003*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Type/*0100001A*/[] 
          GetTypes(class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100001B*/ 'assembly') cil managed
  // SIG: 00 01 1D 12 69 12 6D
  {
    .custom /*0C000020:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000021:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000019*/::.ctor(string) /* 0A000016 */ = ( 01 00 16 54 79 70 65 73 20 6D 69 67 68 74 20 62   // ...Types might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 00 00 )                // e removed..
    .custom /*0C000022:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2056
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyExtensions::GetTypes

} // end of class System.Reflection.AssemblyExtensions

.class /*02000003*/ public abstract auto ansi sealed beforefieldinit System.Reflection.EventInfoExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
{
  .custom /*0C000023:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000024:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000025:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000026:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000004*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001D*/ 
          GetAddMethod(class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100001E*/ eventInfo) cil managed
  // SIG: 00 01 12 75 12 79
  {
    .custom /*0C000027:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000028:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*08000004*/ 
    .custom /*0C00002D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2059
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EventInfoExtensions::GetAddMethod

  .method /*06000005*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001D*/ 
          GetAddMethod(class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100001E*/ eventInfo,
                       bool nonPublic) cil managed
  // SIG: 00 02 12 75 12 79 02
  {
    .custom /*0C00002E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00002F:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*08000006*/ 
    .custom /*0C000038:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x205c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EventInfoExtensions::GetAddMethod

  .method /*06000006*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001D*/ 
          GetRaiseMethod(class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100001E*/ eventInfo) cil managed
  // SIG: 00 01 12 75 12 79
  {
    .custom /*0C000032:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000033:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*08000009*/ 
    .custom /*0C000047:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x205f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EventInfoExtensions::GetRaiseMethod

  .method /*06000007*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001D*/ 
          GetRaiseMethod(class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100001E*/ eventInfo,
                         bool nonPublic) cil managed
  // SIG: 00 02 12 75 12 79 02
  {
    .custom /*0C000039:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00003A:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*0800000B*/ 
    .custom /*0C00004C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2062
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EventInfoExtensions::GetRaiseMethod

  .method /*06000008*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001D*/ 
          GetRemoveMethod(class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100001E*/ eventInfo) cil managed
  // SIG: 00 01 12 75 12 79
  {
    .custom /*0C00003F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000040:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*0800000E*/ 
    .custom /*0C000054:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2065
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EventInfoExtensions::GetRemoveMethod

  .method /*06000009*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001D*/ 
          GetRemoveMethod(class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100001E*/ eventInfo,
                          bool nonPublic) cil managed
  // SIG: 00 02 12 75 12 79 02
  {
    .custom /*0C000045:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000046:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*08000010*/ 
    .custom /*0C000059:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2068
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EventInfoExtensions::GetRemoveMethod

} // end of class System.Reflection.EventInfoExtensions

.class /*02000004*/ public abstract auto ansi sealed beforefieldinit System.Reflection.MemberInfoExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
{
  .custom /*0C000029:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00002B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00002C:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600000A*/ public hidebysig static 
          int32  GetMetadataToken(class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*0100001F*/ member) cil managed
  // SIG: 00 01 08 12 7D
  {
    .custom /*0C000048:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000049:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x206b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberInfoExtensions::GetMetadataToken

  .method /*0600000B*/ public hidebysig static 
          bool  HasMetadataToken(class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*0100001F*/ member) cil managed
  // SIG: 00 01 02 12 7D
  {
    .custom /*0C00004A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00004B:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x206e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemberInfoExtensions::HasMetadataToken

} // end of class System.Reflection.MemberInfoExtensions

.class /*02000005*/ public abstract auto ansi sealed beforefieldinit System.Reflection.MethodInfoExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
{
  .custom /*0C000030:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000031:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600000C*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001D*/ 
          GetBaseDefinition(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001D*/ 'method') cil managed
  // SIG: 00 01 12 75 12 75
  {
    .custom /*0C00004D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00004E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00004F:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2071
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodInfoExtensions::GetBaseDefinition

} // end of class System.Reflection.MethodInfoExtensions

.class /*02000006*/ public abstract auto ansi sealed beforefieldinit System.Reflection.ModuleExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
{
  .custom /*0C000034:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000035:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000036:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000037:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600000D*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Guid/*01000020*/ 
          GetModuleVersionId(class [System.Runtime/*23000001*/]System.Reflection.Module/*0100001C*/ module) cil managed
  // SIG: 00 01 11 80 81 12 71
  {
    .custom /*0C000050:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000051:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2074
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleExtensions::GetModuleVersionId

  .method /*0600000E*/ public hidebysig static 
          bool  HasModuleVersionId(class [System.Runtime/*23000001*/]System.Reflection.Module/*0100001C*/ module) cil managed
  // SIG: 00 01 02 12 71
  {
    .custom /*0C000052:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000053:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2077
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleExtensions::HasModuleVersionId

} // end of class System.Reflection.ModuleExtensions

.class /*02000007*/ public abstract auto ansi sealed beforefieldinit System.Reflection.PropertyInfoExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
{
  .custom /*0C00003B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00003D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00003E:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600000F*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001D*/[] 
          GetAccessors(class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000021*/ 'property') cil managed
  // SIG: 00 01 1D 12 75 12 80 85
  {
    .custom /*0C000055:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000056:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x207a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyInfoExtensions::GetAccessors

  .method /*06000010*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001D*/[] 
          GetAccessors(class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000021*/ 'property',
                       bool nonPublic) cil managed
  // SIG: 00 02 1D 12 75 12 80 85 02
  {
    .custom /*0C000057:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000058:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x207d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyInfoExtensions::GetAccessors

  .method /*06000011*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001D*/ 
          GetGetMethod(class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000021*/ 'property') cil managed
  // SIG: 00 01 12 75 12 80 85
  {
    .custom /*0C00005A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00005B:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*0800001B*/ 
    .custom /*0C000070:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2080
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyInfoExtensions::GetGetMethod

  .method /*06000012*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001D*/ 
          GetGetMethod(class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000021*/ 'property',
                       bool nonPublic) cil managed
  // SIG: 00 02 12 75 12 80 85 02
  {
    .custom /*0C00005C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00005D:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*0800001D*/ 
    .custom /*0C000075:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2083
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyInfoExtensions::GetGetMethod

  .method /*06000013*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001D*/ 
          GetSetMethod(class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000021*/ 'property') cil managed
  // SIG: 00 01 12 75 12 80 85
  {
    .custom /*0C00005E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00005F:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*08000020*/ 
    .custom /*0C00007C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2086
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyInfoExtensions::GetSetMethod

  .method /*06000014*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001D*/ 
          GetSetMethod(class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000021*/ 'property',
                       bool nonPublic) cil managed
  // SIG: 00 02 12 75 12 80 85 02
  {
    .custom /*0C000060:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000061:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*08000022*/ 
    .custom /*0C000081:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2089
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyInfoExtensions::GetSetMethod

} // end of class System.Reflection.PropertyInfoExtensions

.class /*02000008*/ public abstract auto ansi sealed beforefieldinit System.Reflection.TypeExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
{
  .custom /*0C000041:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000042:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000043:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000044:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000015*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000022*/ 
          GetConstructor(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                         class [System.Runtime/*23000001*/]System.Type/*0100001A*/[] types) cil managed
  // SIG: 00 02 12 80 89 12 69 1D 12 69
  {
    .custom /*0C000062:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000063:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*08000025*/ 
    .custom /*0C000088:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000026*/ 
    .custom /*0C00008B:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 03 00 00 00 00 00 ) 
    // Method begins at RVA 0x208c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetConstructor

  .method /*06000016*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000022*/[] 
          GetConstructors(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type') cil managed
  // SIG: 00 01 1D 12 80 89 12 69
  {
    .custom /*0C000064:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000065:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*08000028*/ 
    .custom /*0C000090:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 03 00 00 00 00 00 ) 
    // Method begins at RVA 0x208f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetConstructors

  .method /*06000017*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000022*/[] 
          GetConstructors(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                          valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000025*/ bindingAttr) cil managed
  // SIG: 00 02 1D 12 80 89 12 69 11 80 95
  {
    .custom /*0C000066:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000067:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*08000029*/ 
    .custom /*0C000093:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 07 00 00 00 00 00 ) 
    // Method begins at RVA 0x2092
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetConstructors

  .method /*06000018*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*0100001F*/[] 
          GetDefaultMembers(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type') cil managed
  // SIG: 00 01 1D 12 7D 12 69
  {
    .custom /*0C000068:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000069:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*0800002B*/ 
    .custom /*0C000098:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 AB 0A 00 00 00 00 ) 
    // Method begins at RVA 0x2095
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetDefaultMembers

  .method /*06000019*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100001E*/ 
          GetEvent(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                   string name) cil managed
  // SIG: 00 02 12 79 12 69 0E
  {
    .custom /*0C00006A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00006B:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*0800002C*/ 
    .custom /*0C00009B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800002D*/ 
    .custom /*0C00009E:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 00 08 00 00 00 00 ) 
    // Method begins at RVA 0x2098
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetEvent

  .method /*0600001A*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100001E*/ 
          GetEvent(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                   string name,
                   valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000025*/ bindingAttr) cil managed
  // SIG: 00 03 12 79 12 69 0E 11 80 95
  {
    .custom /*0C00006C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00006D:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*0800002F*/ 
    .custom /*0C0000A3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000030*/ 
    .custom /*0C0000A6:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 00 18 00 00 00 00 ) 
    // Method begins at RVA 0x209b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetEvent

  .method /*0600001B*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100001E*/[] 
          GetEvents(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type') cil managed
  // SIG: 00 01 1D 12 79 12 69
  {
    .custom /*0C00006E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00006F:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*08000033*/ 
    .custom /*0C0000AD:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 00 08 00 00 00 00 ) 
    // Method begins at RVA 0x209e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetEvents

  .method /*0600001C*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100001E*/[] 
          GetEvents(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                    valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000025*/ bindingAttr) cil managed
  // SIG: 00 02 1D 12 79 12 69 11 80 95
  {
    .custom /*0C000071:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000072:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*08000034*/ 
    .custom /*0C0000B0:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 00 18 00 00 00 00 ) 
    // Method begins at RVA 0x20a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetEvents

  .method /*0600001D*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*01000026*/ 
          GetField(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                   string name) cil managed
  // SIG: 00 02 12 80 99 12 69 0E
  {
    .custom /*0C000073:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000074:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*08000036*/ 
    .custom /*0C0000B3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000037*/ 
    .custom /*0C0000B4:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 20 00 00 00 00 00 )                         // .. .....
    // Method begins at RVA 0x20a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetField

  .method /*0600001E*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*01000026*/ 
          GetField(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                   string name,
                   valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000025*/ bindingAttr) cil managed
  // SIG: 00 03 12 80 99 12 69 0E 11 80 95
  {
    .custom /*0C000076:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000077:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*08000039*/ 
    .custom /*0C0000B5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800003A*/ 
    .custom /*0C0000B6:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 60 00 00 00 00 00 )                         // ..`.....
    // Method begins at RVA 0x20a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetField

  .method /*0600001F*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*01000026*/[] 
          GetFields(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type') cil managed
  // SIG: 00 01 1D 12 80 99 12 69
  {
    .custom /*0C000078:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000079:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*0800003D*/ 
    .custom /*0C0000B7:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 20 00 00 00 00 00 )                         // .. .....
    // Method begins at RVA 0x20aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetFields

  .method /*06000020*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*01000026*/[] 
          GetFields(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                    valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000025*/ bindingAttr) cil managed
  // SIG: 00 02 1D 12 80 99 12 69 11 80 95
  {
    .custom /*0C00007A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00007B:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*0800003E*/ 
    .custom /*0C0000B8:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 60 00 00 00 00 00 )                         // ..`.....
    // Method begins at RVA 0x20ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetFields

  .method /*06000021*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Type/*0100001A*/[] 
          GetGenericArguments(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type') cil managed
  // SIG: 00 01 1D 12 69 12 69
  {
    .custom /*0C00007D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00007E:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x20b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetGenericArguments

  .method /*06000022*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Type/*0100001A*/[] 
          GetInterfaces(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type') cil managed
  // SIG: 00 01 1D 12 69 12 69
  {
    .custom /*0C00007F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000080:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*08000041*/ 
    .custom /*0C0000B9:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 00 20 00 00 00 00 )                         // ... ....
    // Method begins at RVA 0x20b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetInterfaces

  .method /*06000023*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*0100001F*/[] 
          GetMember(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                    string name) cil managed
  // SIG: 00 02 1D 12 7D 12 69 0E
  {
    .custom /*0C000082:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000083:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*08000042*/ 
    .custom /*0C0000BA:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 AB 0A 00 00 00 00 ) 
    // Method begins at RVA 0x20b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetMember

  .method /*06000024*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*0100001F*/[] 
          GetMember(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                    string name,
                    valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000025*/ bindingAttr) cil managed
  // SIG: 00 03 1D 12 7D 12 69 0E 11 80 95
  {
    .custom /*0C000084:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000085:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*08000044*/ 
    .custom /*0C0000BB:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 FF 1F 00 00 00 00 ) 
    // Method begins at RVA 0x20b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetMember

  .method /*06000025*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*0100001F*/[] 
          GetMembers(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type') cil managed
  // SIG: 00 01 1D 12 7D 12 69
  {
    .custom /*0C000086:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000087:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*08000047*/ 
    .custom /*0C0000BC:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 AB 0A 00 00 00 00 ) 
    // Method begins at RVA 0x20bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetMembers

  .method /*06000026*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*0100001F*/[] 
          GetMembers(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                     valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000025*/ bindingAttr) cil managed
  // SIG: 00 02 1D 12 7D 12 69 11 80 95
  {
    .custom /*0C000089:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00008A:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*08000048*/ 
    .custom /*0C0000BD:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 FF 1F 00 00 00 00 ) 
    // Method begins at RVA 0x20bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetMembers

  .method /*06000027*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001D*/ 
          GetMethod(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                    string name) cil managed
  // SIG: 00 02 12 75 12 69 0E
  {
    .custom /*0C00008C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00008D:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*0800004A*/ 
    .custom /*0C0000BE:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800004B*/ 
    .custom /*0C0000BF:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 08 00 00 00 00 00 ) 
    // Method begins at RVA 0x20c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetMethod

  .method /*06000028*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001D*/ 
          GetMethod(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                    string name,
                    valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000025*/ bindingAttr) cil managed
  // SIG: 00 03 12 75 12 69 0E 11 80 95
  {
    .custom /*0C00008E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00008F:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*0800004D*/ 
    .custom /*0C0000C0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800004E*/ 
    .custom /*0C0000C1:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 18 00 00 00 00 00 ) 
    // Method begins at RVA 0x20c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetMethod

  .method /*06000029*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001D*/ 
          GetMethod(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                    string name,
                    class [System.Runtime/*23000001*/]System.Type/*0100001A*/[] types) cil managed
  // SIG: 00 03 12 75 12 69 0E 1D 12 69
  {
    .custom /*0C000091:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000092:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*08000051*/ 
    .custom /*0C0000C2:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000052*/ 
    .custom /*0C0000C3:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 08 00 00 00 00 00 ) 
    // Method begins at RVA 0x20c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetMethod

  .method /*0600002A*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001D*/[] 
          GetMethods(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type') cil managed
  // SIG: 00 01 1D 12 75 12 69
  {
    .custom /*0C000094:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000095:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*08000055*/ 
    .custom /*0C0000C4:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 08 00 00 00 00 00 ) 
    // Method begins at RVA 0x20cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetMethods

  .method /*0600002B*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001D*/[] 
          GetMethods(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                     valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000025*/ bindingAttr) cil managed
  // SIG: 00 02 1D 12 75 12 69 11 80 95
  {
    .custom /*0C000096:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000097:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*08000056*/ 
    .custom /*0C0000C5:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 18 00 00 00 00 00 ) 
    // Method begins at RVA 0x20ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetMethods

  .method /*0600002C*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 
          GetNestedType(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                        string name,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000025*/ bindingAttr) cil managed
  // SIG: 00 03 12 69 12 69 0E 11 80 95
  {
    .custom /*0C000099:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00009A:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*08000058*/ 
    .custom /*0C0000C6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000059*/ 
    .custom /*0C0000C7:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 80 01 00 00 00 00 ) 
    // Method begins at RVA 0x20d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetNestedType

  .method /*0600002D*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Type/*0100001A*/[] 
          GetNestedTypes(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                         valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000025*/ bindingAttr) cil managed
  // SIG: 00 02 1D 12 69 12 69 11 80 95
  {
    .custom /*0C00009C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00009D:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*0800005C*/ 
    .custom /*0C0000C8:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 80 01 00 00 00 00 ) 
    // Method begins at RVA 0x20d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetNestedTypes

  .method /*0600002E*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000021*/[] 
          GetProperties(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type') cil managed
  // SIG: 00 01 1D 12 80 85 12 69
  {
    .custom /*0C00009F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000A0:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*0800005E*/ 
    .custom /*0C0000C9:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 00 02 00 00 00 00 ) 
    // Method begins at RVA 0x20d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetProperties

  .method /*0600002F*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000021*/[] 
          GetProperties(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000025*/ bindingAttr) cil managed
  // SIG: 00 02 1D 12 80 85 12 69 11 80 95
  {
    .custom /*0C0000A1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000A2:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [1]/*0800005F*/ 
    .custom /*0C0000CA:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 00 06 00 00 00 00 ) 
    // Method begins at RVA 0x20da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetProperties

  .method /*06000030*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000021*/ 
          GetProperty(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                      string name) cil managed
  // SIG: 00 02 12 80 85 12 69 0E
  {
    .custom /*0C0000A4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000A5:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*08000061*/ 
    .custom /*0C0000CB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000062*/ 
    .custom /*0C0000CC:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 00 02 00 00 00 00 ) 
    // Method begins at RVA 0x20dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetProperty

  .method /*06000031*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000021*/ 
          GetProperty(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                      string name,
                      valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000025*/ bindingAttr) cil managed
  // SIG: 00 03 12 80 85 12 69 0E 11 80 95
  {
    .custom /*0C0000A7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000A8:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*08000064*/ 
    .custom /*0C0000CD:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000065*/ 
    .custom /*0C0000CE:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 00 06 00 00 00 00 ) 
    // Method begins at RVA 0x20e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetProperty

  .method /*06000032*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000021*/ 
          GetProperty(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                      string name,
                      class [System.Runtime/*23000001*/]System.Type/*0100001A*/ returnType) cil managed
  // SIG: 00 03 12 80 85 12 69 0E 12 69
  {
    .custom /*0C0000A9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000AA:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*08000068*/ 
    .custom /*0C0000CF:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000069*/ 
    .custom /*0C0000D0:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 00 02 00 00 00 00 ) 
    .param [3]/*0800006B*/ 
    .custom /*0C0000D1:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetProperty

  .method /*06000033*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000021*/ 
          GetProperty(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                      string name,
                      class [System.Runtime/*23000001*/]System.Type/*0100001A*/ returnType,
                      class [System.Runtime/*23000001*/]System.Type/*0100001A*/[] types) cil managed
  // SIG: 00 04 12 80 85 12 69 0E 12 69 1D 12 69
  {
    .custom /*0C0000AB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000AC:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [0]/*0800006C*/ 
    .custom /*0C0000D2:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800006D*/ 
    .custom /*0C0000D3:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000024*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000023*/) /* 0A000017 */ = ( 01 00 00 02 00 00 00 00 ) 
    .param [3]/*0800006F*/ 
    .custom /*0C0000D4:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::GetProperty

  .method /*06000034*/ public hidebysig static 
          bool  IsAssignableFrom(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                                 class [System.Runtime/*23000001*/]System.Type/*0100001A*/ c) cil managed
  // SIG: 00 02 02 12 69 12 69
  {
    .custom /*0C0000AE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000AF:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [2]/*08000072*/ 
    .custom /*0C0000D5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000D6:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000027*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::IsAssignableFrom

  .method /*06000035*/ public hidebysig static 
          bool  IsInstanceOfType(class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                                 object o) cil managed
  // SIG: 00 02 02 12 69 1C
  {
    .custom /*0C0000B1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000B2:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000017*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000016*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [2]/*08000074*/ 
    .custom /*0C0000D7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000D8:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000027*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeExtensions::IsInstanceOfType

} // end of class System.Reflection.TypeExtensions


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
