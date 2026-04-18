


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
// Time-date stamp:            0xc737a48f
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
// Addr. of entry point:           0x00003be2
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
// Checksum:                       0x0000aa67
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00003b8d [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x000004c4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002600 [0x00002878] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00003a98 [0x00000054] address [size] of Debug Directory:           
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
//              0x00001be8 Virtual Size
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
//              0x000004c4 Virtual Size
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
//              Entry 1: Type 0x3 Offset 0x00000be4
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00003bcf Import Name Table
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
// 0x00002130 [0x000018e8] address [size] of Metadata Directory:        
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
//              0x00000a0c Size
//              '#~' Name
//    Stream 2:
//              0x00000a78 Offset
//              0x0000097c Size
//              '#Strings' Name
//    Stream 3:
//              0x000013f4 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x000013f8 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001408 Offset
//              0x000004e0 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000000901a01557 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 20088
// PE header size       : 512 (496 used)    ( 2.55%)
// PE additional info   : 11835             (58.92%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.36%)
// CLR meta-data size  : 6376               (31.74%)
// CLR additional info : 128                ( 0.64%)
// CLR method headers  : 52                 ( 0.26%)
// Managed code         : 170               ( 0.85%)
// Data                 : 2048              (10.20%)
// Unaccounted          : -1105             (-5.50%)

// Num.of PE sections   : 3
//   .text    - 7168
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 6376
//   Module        -    1 (10 bytes)
//   TypeDef       -    8 (112 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   52 (312 bytes)
//   MethodDef     -   52 (728 bytes)     0 abstract, 0 native, 52 bodies
//   FieldDef      -    4 (24 bytes)      0 constant
//   MemberRef     -   21 (126 bytes)
//   ParamDef      -   46 (276 bytes)
//   CustomAttribute-   78 (468 bytes)
//   PropertyMap   -    2 (8 bytes)
//   Property      -   18 (108 bytes)
//   MethodSemantic-   30 (180 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    6 (120 bytes)
//   Strings       -  2425 bytes
//   Blobs         -  1248 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   189 bytes

// CLR additional info : 128

// CLR method headers : 52
//   Num.of method bodies  - 52
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 52

// Managed code : 170
//   Ave method size - 3
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class RefSafetyRulesAttribute        (auto) (ansi) (sealed) 
// Class ClientWebSocket                (public) (auto) (ansi) (sealed) 
// Class ClientWebSocketOptions         (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000002*/ System.Net.WebSockets
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000003*/ System.Net.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000004*/ System.Net.Http
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000005*/ System.Security.Cryptography
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000006*/ System.Net.Security
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly /*20000001*/ noplatform System.Net.WebSockets.Client
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

  .custom /*0C000007:0A000004*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 37 2E 30 01 00 54 0E 14   // ersion=v7.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 37 2E 30 )          // Name..NET 7.0
  .custom /*0C000008:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 1C 53 79 73 74 65 6D 2E 4E 65 74 2E 57 65   // ...System.Net.We
                                                                                                                                                                        62 53 6F 63 6B 65 74 73 2E 43 6C 69 65 6E 74 00   // bSockets.Client.
                                                                                                                                                                        00 ) 
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000F:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000010:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 1C 53 79 73 74 65 6D 2E 4E 65 74 2E 57 65   // ...System.Net.We
                                                                                                                                                                       62 53 6F 63 6B 65 74 73 2E 43 6C 69 65 6E 74 00   // bSockets.Client.
                                                                                                                                                                       00 ) 
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 37 2E 30 2E 32 32 2E 35 31 38 30 35 00   // ...7.0.22.51805.
                                                                                                                                                                       00 ) 
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 37 2E 30 2E 30 2B 64 30 39 39 66 30 37   // ...7.0.0+d099f07
                                                                                                                                                                                35 65 34 35 64 32 61 61 36 30 30 37 61 32 32 62   // 5e45d2aa6007a22b
                                                                                                                                                                                37 31 62 34 35 61 30 38 37 35 38 35 35 39 66 38   // 71b45a08758559f8
                                                                                                                                                                                30 00 00 )                                        // 0..
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000014:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 1C 53 79 73 74 65 6D 2E 4E 65 74 2E 57 65   // ...System.Net.We
                                                                                                                                                                 62 53 6F 63 6B 65 74 73 2E 43 6C 69 65 6E 74 00   // bSockets.Client.
                                                                                                                                                                 00 ) 
  .custom /*0C000015:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    21 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // !https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 72 75 6E 74 69   // com/dotnet/runti
                                                                                                                                                                    6D 65 00 00 )                                     // me..
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
  .ver 7:0:0:0
}
.module System.Net.WebSockets.Client.dll
// MVID: {2aa5fe7a-a5c5-4d29-b272-2590a0e2c7b4}
.custom /*0C000002:06000006*/ instance void System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::.ctor(int32) /* 06000006 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000003:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000016:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000017:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00001A:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00001B:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001C:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000013 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)000034       */ newarr     [System.Runtime/*23000001*/]System.Byte/*01000034*/
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
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000013 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00001D:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00001E:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000013 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000022:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000023:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000024:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000013 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.RefSafetyRulesAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000025:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000026:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000027:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000013 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000004       */ stfld      int32 System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::Version /* 04000004 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method RefSafetyRulesAttribute::.ctor

} // end of class System.Runtime.CompilerServices.RefSafetyRulesAttribute

.class /*02000007*/ public auto ansi sealed beforefieldinit System.Net.WebSockets.ClientWebSocket
       extends [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocket/*01000015*/
{
  .custom /*0C000028:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000029:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ac
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocket/*01000015*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ClientWebSocket::.ctor

  .method /*06000008*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000016*/<valuetype [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocketCloseStatus/*01000017*/> 
          get_CloseStatus() cil managed
  // SIG: 20 00 15 11 59 01 11 5D
  {
    // Method begins at RVA 0x20b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocket::get_CloseStatus

  .method /*06000009*/ public hidebysig specialname virtual 
          instance string  get_CloseStatusDescription() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000030:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocket::get_CloseStatusDescription

  .method /*0600000A*/ public hidebysig specialname 
          instance valuetype [System.Net.Primitives/*23000003*/]System.Net.HttpStatusCode/*01000018*/ 
          get_HttpStatusCode() cil managed
  // SIG: 20 00 11 61
  {
    // Method begins at RVA 0x20ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocket::get_HttpStatusCode

  .method /*0600000B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000019*/<string,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001A*/<string>> 
          get_HttpResponseHeaders() cil managed
  // SIG: 20 00 15 12 65 02 0E 15 12 69 01 0E
  {
    .param [0]/*08000001*/ 
    .custom /*0C000001:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 01 01 01 00 00 ) 
    // Method begins at RVA 0x20bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocket::get_HttpResponseHeaders

  .method /*0600000C*/ public hidebysig specialname 
          instance void  set_HttpResponseHeaders(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000019*/<string,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001A*/<string>> 'value') cil managed
  // SIG: 20 01 01 15 12 65 02 0E 15 12 69 01 0E
  {
    .param [1]/*08000002*/ 
    .custom /*0C000018:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 01 01 01 00 00 ) 
    // Method begins at RVA 0x20c0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ClientWebSocket::set_HttpResponseHeaders

  .method /*0600000D*/ public hidebysig specialname 
          instance class System.Net.WebSockets.ClientWebSocketOptions/*02000008*/ 
          get_Options() cil managed
  // SIG: 20 00 12 20
  {
    // Method begins at RVA 0x20c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocket::get_Options

  .method /*0600000E*/ public hidebysig specialname virtual 
          instance valuetype [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocketState/*0100001B*/ 
          get_State() cil managed
  // SIG: 20 00 11 6D
  {
    // Method begins at RVA 0x20c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocket::get_State

  .method /*0600000F*/ public hidebysig specialname virtual 
          instance string  get_SubProtocol() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000039:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocket::get_SubProtocol

  .method /*06000010*/ public hidebysig virtual 
          instance void  Abort() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20cb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ClientWebSocket::Abort

  .method /*06000011*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001C*/ 
          CloseAsync(valuetype [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocketCloseStatus/*01000017*/ closeStatus,
                     string statusDescription,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 20 03 12 71 11 5D 0E 11 75
  {
    .param [2]/*08000004*/ 
    .custom /*0C000020:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocket::CloseAsync

  .method /*06000012*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001C*/ 
          CloseOutputAsync(valuetype [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocketCloseStatus/*01000017*/ closeStatus,
                           string statusDescription,
                           valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 20 03 12 71 11 5D 0E 11 75
  {
    .param [2]/*08000007*/ 
    .custom /*0C00002A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocket::CloseOutputAsync

  .method /*06000013*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001C*/ 
          ConnectAsync(class [System.Runtime/*23000001*/]System.Uri/*0100001E*/ uri,
                       valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 20 02 12 71 12 79 11 75
  {
    // Method begins at RVA 0x20d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocket::ConnectAsync

  .method /*06000014*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001C*/ 
          ConnectAsync(class [System.Runtime/*23000001*/]System.Uri/*0100001E*/ uri,
                       class [System.Net.Http/*23000004*/]System.Net.Http.HttpMessageInvoker/*0100001F*/ invoker,
                       valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 20 03 12 71 12 79 12 7D 11 75
  {
    .param [2]/*0800000C*/ 
    .custom /*0C000034:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocket::ConnectAsync

  .method /*06000015*/ public hidebysig virtual 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20d9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ClientWebSocket::Dispose

  .method /*06000016*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000020*/<class [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocketReceiveResult/*01000021*/> 
          ReceiveAsync(valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000022*/<uint8> buffer,
                       valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 80 81 01 12 80 85 15 11 80 89 01 05 11 75
  {
    .custom /*0C00003E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [0]/*0800000E*/ 
    .custom /*0C000037:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocket::ReceiveAsync

  .method /*06000017*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000023*/<valuetype [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.ValueWebSocketReceiveResult/*01000024*/> 
          ReceiveAsync(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000025*/<uint8> buffer,
                       valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 8D 01 11 80 91 15 11 80 95 01 05 11 75
  {
    .custom /*0C00003F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocket::ReceiveAsync

  .method /*06000018*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001C*/ 
          SendAsync(valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000022*/<uint8> buffer,
                    valuetype [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocketMessageType/*01000026*/ messageType,
                    bool endOfMessage,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 20 04 12 71 15 11 80 89 01 05 11 80 99 02 11 75
  {
    .custom /*0C000040:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000013*/ 
    .custom /*0C00003D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocket::SendAsync

  .method /*06000019*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000027*/ 
          SendAsync(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000028*/<uint8> buffer,
                    valuetype [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocketMessageType/*01000026*/ messageType,
                    bool endOfMessage,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001D*/ cancellationToken) cil managed
  // SIG: 20 04 11 80 9D 15 11 80 A1 01 05 11 80 99 02 11 75
  {
    .custom /*0C000041:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocket::SendAsync

  .property /*17000001*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000016*/<valuetype [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocketCloseStatus/*01000017*/>
          CloseStatus()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000016*/<valuetype [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocketCloseStatus/*01000017*/> System.Net.WebSockets.ClientWebSocket/*02000007*/::get_CloseStatus() /* 06000008 */
  } // end of property ClientWebSocket::CloseStatus
  .property /*17000002*/ instance callconv(8) string
          CloseStatusDescription()
  {
    .custom /*0C000019:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.WebSockets.ClientWebSocket/*02000007*/::get_CloseStatusDescription() /* 06000009 */
  } // end of property ClientWebSocket::CloseStatusDescription
  .property /*17000003*/ instance callconv(8) valuetype [System.Net.Primitives/*23000003*/]System.Net.HttpStatusCode/*01000018*/
          HttpStatusCode()
  {
    .get instance valuetype [System.Net.Primitives/*23000003*/]System.Net.HttpStatusCode/*01000018*/ System.Net.WebSockets.ClientWebSocket/*02000007*/::get_HttpStatusCode() /* 0600000A */
  } // end of property ClientWebSocket::HttpStatusCode
  .property /*17000004*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000019*/<string,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001A*/<string>>
          HttpResponseHeaders()
  {
    .custom /*0C000021:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 01 01 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000019*/<string,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001A*/<string>> System.Net.WebSockets.ClientWebSocket/*02000007*/::get_HttpResponseHeaders() /* 0600000B */
    .set instance void System.Net.WebSockets.ClientWebSocket/*02000007*/::set_HttpResponseHeaders(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyDictionary`2/*01000019*/<string,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001A*/<string>>) /* 0600000C */
  } // end of property ClientWebSocket::HttpResponseHeaders
  .property /*17000005*/ instance callconv(8) class System.Net.WebSockets.ClientWebSocketOptions/*02000008*/
          Options()
  {
    .get instance class System.Net.WebSockets.ClientWebSocketOptions/*02000008*/ System.Net.WebSockets.ClientWebSocket/*02000007*/::get_Options() /* 0600000D */
  } // end of property ClientWebSocket::Options
  .property /*17000006*/ instance callconv(8) valuetype [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocketState/*0100001B*/
          State()
  {
    .get instance valuetype [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocketState/*0100001B*/ System.Net.WebSockets.ClientWebSocket/*02000007*/::get_State() /* 0600000E */
  } // end of property ClientWebSocket::State
  .property /*17000007*/ instance callconv(8) string
          SubProtocol()
  {
    .custom /*0C00002B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.WebSockets.ClientWebSocket/*02000007*/::get_SubProtocol() /* 0600000F */
  } // end of property ClientWebSocket::SubProtocol
} // end of class System.Net.WebSockets.ClientWebSocket

.class /*02000008*/ public auto ansi sealed beforefieldinit System.Net.WebSockets.ClientWebSocketOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000029*/
{
  .custom /*0C00002C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00002D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600001A*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20e7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000029*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ClientWebSocketOptions::.ctor

  .method /*0600001B*/ public hidebysig specialname 
          instance class [System.Security.Cryptography/*23000005*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*0100002A*/ 
          get_ClientCertificates() cil managed
  // SIG: 20 00 12 80 A9
  {
    .custom /*0C000042:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocketOptions::get_ClientCertificates

  .method /*0600001C*/ public hidebysig specialname 
          instance void  set_ClientCertificates(class [System.Security.Cryptography/*23000005*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*0100002A*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A9
  {
    .custom /*0C000043:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20f2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ClientWebSocketOptions::set_ClientCertificates

  .method /*0600001D*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000003*/]System.Net.CookieContainer/*0100002B*/ 
          get_Cookies() cil managed
  // SIG: 20 00 12 80 AD
  {
    // Method begins at RVA 0x20f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocketOptions::get_Cookies

  .method /*0600001E*/ public hidebysig specialname 
          instance void  set_Cookies(class [System.Net.Primitives/*23000003*/]System.Net.CookieContainer/*0100002B*/ 'value') cil managed
  // SIG: 20 01 01 12 80 AD
  {
    // Method begins at RVA 0x20f7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ClientWebSocketOptions::set_Cookies

  .method /*0600001F*/ public hidebysig specialname 
          instance bool  get_CollectHttpResponseDetails() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocketOptions::get_CollectHttpResponseDetails

  .method /*06000020*/ public hidebysig specialname 
          instance void  set_CollectHttpResponseDetails(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20fc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ClientWebSocketOptions::set_CollectHttpResponseDetails

  .method /*06000021*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100002C*/ 
          get_Credentials() cil managed
  // SIG: 20 00 12 80 B1
  {
    // Method begins at RVA 0x20fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocketOptions::get_Credentials

  .method /*06000022*/ public hidebysig specialname 
          instance void  set_Credentials(class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100002C*/ 'value') cil managed
  // SIG: 20 01 01 12 80 B1
  {
    // Method begins at RVA 0x2101
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ClientWebSocketOptions::set_Credentials

  .method /*06000023*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002D*/ 
          get_KeepAliveInterval() cil managed
  // SIG: 20 00 11 80 B5
  {
    // Method begins at RVA 0x2103
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocketOptions::get_KeepAliveInterval

  .method /*06000024*/ public hidebysig specialname 
          instance void  set_KeepAliveInterval(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002D*/ 'value') cil managed
  // SIG: 20 01 01 11 80 B5
  {
    // Method begins at RVA 0x2106
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ClientWebSocketOptions::set_KeepAliveInterval

  .method /*06000025*/ public hidebysig specialname 
          instance class [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocketDeflateOptions/*0100002E*/ 
          get_DangerousDeflateOptions() cil managed
  // SIG: 20 00 12 80 B9
  {
    // Method begins at RVA 0x2108
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocketOptions::get_DangerousDeflateOptions

  .method /*06000026*/ public hidebysig specialname 
          instance void  set_DangerousDeflateOptions(class [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocketDeflateOptions/*0100002E*/ 'value') cil managed
  // SIG: 20 01 01 12 80 B9
  {
    // Method begins at RVA 0x210b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ClientWebSocketOptions::set_DangerousDeflateOptions

  .method /*06000027*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100002F*/ 
          get_Proxy() cil managed
  // SIG: 20 00 12 80 BD
  {
    // Method begins at RVA 0x210d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocketOptions::get_Proxy

  .method /*06000028*/ public hidebysig specialname 
          instance void  set_Proxy(class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100002F*/ 'value') cil managed
  // SIG: 20 01 01 12 80 BD
  {
    // Method begins at RVA 0x2110
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ClientWebSocketOptions::set_Proxy

  .method /*06000029*/ public hidebysig specialname 
          instance class [System.Net.Security/*23000006*/]System.Net.Security.RemoteCertificateValidationCallback/*01000030*/ 
          get_RemoteCertificateValidationCallback() cil managed
  // SIG: 20 00 12 80 C1
  {
    // Method begins at RVA 0x2112
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocketOptions::get_RemoteCertificateValidationCallback

  .method /*0600002A*/ public hidebysig specialname 
          instance void  set_RemoteCertificateValidationCallback(class [System.Net.Security/*23000006*/]System.Net.Security.RemoteCertificateValidationCallback/*01000030*/ 'value') cil managed
  // SIG: 20 01 01 12 80 C1
  {
    // Method begins at RVA 0x2115
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ClientWebSocketOptions::set_RemoteCertificateValidationCallback

  .method /*0600002B*/ public hidebysig specialname 
          instance bool  get_UseDefaultCredentials() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2117
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocketOptions::get_UseDefaultCredentials

  .method /*0600002C*/ public hidebysig specialname 
          instance void  set_UseDefaultCredentials(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x211a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ClientWebSocketOptions::set_UseDefaultCredentials

  .method /*0600002D*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Version/*01000031*/ 
          get_HttpVersion() cil managed
  // SIG: 20 00 12 80 C5
  {
    .custom /*0C000044:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x211c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocketOptions::get_HttpVersion

  .method /*0600002E*/ public hidebysig specialname 
          instance void  set_HttpVersion(class [System.Runtime/*23000001*/]System.Version/*01000031*/ 'value') cil managed
  // SIG: 20 01 01 12 80 C5
  {
    .custom /*0C000045:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000046:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000032*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x211f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ClientWebSocketOptions::set_HttpVersion

  .method /*0600002F*/ public hidebysig specialname 
          instance valuetype [System.Net.Http/*23000004*/]System.Net.Http.HttpVersionPolicy/*01000033*/ 
          get_HttpVersionPolicy() cil managed
  // SIG: 20 00 11 80 CD
  {
    // Method begins at RVA 0x2121
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientWebSocketOptions::get_HttpVersionPolicy

  .method /*06000030*/ public hidebysig specialname 
          instance void  set_HttpVersionPolicy(valuetype [System.Net.Http/*23000004*/]System.Net.Http.HttpVersionPolicy/*01000033*/ 'value') cil managed
  // SIG: 20 01 01 11 80 CD
  {
    .custom /*0C000048:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000032*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2124
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ClientWebSocketOptions::set_HttpVersionPolicy

  .method /*06000031*/ public hidebysig instance void 
          AddSubProtocol(string subProtocol) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000049:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2126
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ClientWebSocketOptions::AddSubProtocol

  .method /*06000032*/ public hidebysig instance void 
          SetBuffer(int32 receiveBufferSize,
                    int32 sendBufferSize) cil managed
  // SIG: 20 02 01 08 08
  {
    .custom /*0C00004A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000032*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2128
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ClientWebSocketOptions::SetBuffer

  .method /*06000033*/ public hidebysig instance void 
          SetBuffer(int32 receiveBufferSize,
                    int32 sendBufferSize,
                    valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000022*/<uint8> buffer) cil managed
  // SIG: 20 03 01 08 08 15 11 80 89 01 05
  {
    .custom /*0C00004B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .custom /*0C00004C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000032*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x212a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ClientWebSocketOptions::SetBuffer

  .method /*06000034*/ public hidebysig instance void 
          SetRequestHeader(string headerName,
                           string headerValue) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .custom /*0C00004D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00004E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000032*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .param [2]/*0800002E*/ 
    .custom /*0C000047:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x212c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ClientWebSocketOptions::SetRequestHeader

  .property /*17000008*/ instance callconv(8) class [System.Security.Cryptography/*23000005*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*0100002A*/
          ClientCertificates()
  {
    .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00002F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000032*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance class [System.Security.Cryptography/*23000005*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*0100002A*/ System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::get_ClientCertificates() /* 0600001B */
    .set instance void System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::set_ClientCertificates(class [System.Security.Cryptography/*23000005*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*0100002A*/) /* 0600001C */
  } // end of property ClientWebSocketOptions::ClientCertificates
  .property /*17000009*/ instance callconv(8) class [System.Net.Primitives/*23000003*/]System.Net.CookieContainer/*0100002B*/
          Cookies()
  {
    .custom /*0C000031:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000032*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance class [System.Net.Primitives/*23000003*/]System.Net.CookieContainer/*0100002B*/ System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::get_Cookies() /* 0600001D */
    .set instance void System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::set_Cookies(class [System.Net.Primitives/*23000003*/]System.Net.CookieContainer/*0100002B*/) /* 0600001E */
  } // end of property ClientWebSocketOptions::Cookies
  .property /*1700000A*/ instance callconv(8) bool
          CollectHttpResponseDetails()
  {
    .custom /*0C000032:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000032*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance bool System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::get_CollectHttpResponseDetails() /* 0600001F */
    .set instance void System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::set_CollectHttpResponseDetails(bool) /* 06000020 */
  } // end of property ClientWebSocketOptions::CollectHttpResponseDetails
  .property /*1700000B*/ instance callconv(8) class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100002C*/
          Credentials()
  {
    .custom /*0C000033:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000032*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100002C*/ System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::get_Credentials() /* 06000021 */
    .set instance void System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::set_Credentials(class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100002C*/) /* 06000022 */
  } // end of property ClientWebSocketOptions::Credentials
  .property /*1700000C*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002D*/
          KeepAliveInterval()
  {
    .custom /*0C000035:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000032*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002D*/ System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::get_KeepAliveInterval() /* 06000023 */
    .set instance void System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::set_KeepAliveInterval(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002D*/) /* 06000024 */
  } // end of property ClientWebSocketOptions::KeepAliveInterval
  .property /*1700000D*/ instance callconv(8) class [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocketDeflateOptions/*0100002E*/
          DangerousDeflateOptions()
  {
    .custom /*0C000036:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000032*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance class [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocketDeflateOptions/*0100002E*/ System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::get_DangerousDeflateOptions() /* 06000025 */
    .set instance void System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::set_DangerousDeflateOptions(class [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocketDeflateOptions/*0100002E*/) /* 06000026 */
  } // end of property ClientWebSocketOptions::DangerousDeflateOptions
  .property /*1700000E*/ instance callconv(8) class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100002F*/
          Proxy()
  {
    .custom /*0C000038:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000032*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100002F*/ System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::get_Proxy() /* 06000027 */
    .set instance void System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::set_Proxy(class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100002F*/) /* 06000028 */
  } // end of property ClientWebSocketOptions::Proxy
  .property /*1700000F*/ instance callconv(8) class [System.Net.Security/*23000006*/]System.Net.Security.RemoteCertificateValidationCallback/*01000030*/
          RemoteCertificateValidationCallback()
  {
    .custom /*0C00003A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000032*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance class [System.Net.Security/*23000006*/]System.Net.Security.RemoteCertificateValidationCallback/*01000030*/ System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::get_RemoteCertificateValidationCallback() /* 06000029 */
    .set instance void System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::set_RemoteCertificateValidationCallback(class [System.Net.Security/*23000006*/]System.Net.Security.RemoteCertificateValidationCallback/*01000030*/) /* 0600002A */
  } // end of property ClientWebSocketOptions::RemoteCertificateValidationCallback
  .property /*17000010*/ instance callconv(8) bool
          UseDefaultCredentials()
  {
    .custom /*0C00003B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000032*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get instance bool System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::get_UseDefaultCredentials() /* 0600002B */
    .set instance void System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::set_UseDefaultCredentials(bool) /* 0600002C */
  } // end of property ClientWebSocketOptions::UseDefaultCredentials
  .property /*17000011*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Version/*01000031*/
          HttpVersion()
  {
    .custom /*0C00003C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Version/*01000031*/ System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::get_HttpVersion() /* 0600002D */
    .set instance void System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::set_HttpVersion(class [System.Runtime/*23000001*/]System.Version/*01000031*/) /* 0600002E */
  } // end of property ClientWebSocketOptions::HttpVersion
  .property /*17000012*/ instance callconv(8) valuetype [System.Net.Http/*23000004*/]System.Net.Http.HttpVersionPolicy/*01000033*/
          HttpVersionPolicy()
  {
    .get instance valuetype [System.Net.Http/*23000004*/]System.Net.Http.HttpVersionPolicy/*01000033*/ System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::get_HttpVersionPolicy() /* 0600002F */
    .set instance void System.Net.WebSockets.ClientWebSocketOptions/*02000008*/::set_HttpVersionPolicy(valuetype [System.Net.Http/*23000004*/]System.Net.Http.HttpVersionPolicy/*01000033*/) /* 06000030 */
  } // end of property ClientWebSocketOptions::HttpVersionPolicy
} // end of class System.Net.WebSockets.ClientWebSocketOptions


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
