


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
// Time-date stamp:            0xea90fc56
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00001e00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00003dea
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
// Checksum:                       0x00011800
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00003d95 [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x00000484] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002800 [0x00002480] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00003cb0 [0x00000054] address [size] of Debug Directory:           
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
//              0x00001df0 Virtual Size
//              0x00002000 Virtual Address
//              0x00001e00 Size of Raw Data
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
//              0x00000484 Virtual Size
//              0x00004000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00002000 Pointer to Raw Data
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
//              0x00002600 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000dec
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00003dd7 Import Name Table
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
// 0x00002110 [0x00001b20] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00003c30 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000e0c Size
//              '#~' Name
//    Stream 2:
//              0x00000e78 Offset
//              0x00000678 Size
//              '#Strings' Name
//    Stream 3:
//              0x000014f0 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x000014f4 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001504 Offset
//              0x0000061c Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000040901a01d57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 19584
// PE header size       : 512 (496 used)    ( 2.61%)
// PE additional info   : 10755             (54.92%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.37%)
// CLR meta-data size  : 6944               (35.46%)
// CLR additional info : 128                ( 0.65%)
// CLR method headers  : 42                 ( 0.21%)
// Managed code         : 150               ( 0.77%)
// Data                 : 2048              (10.46%)
// Unaccounted          : -1067             (-5.45%)

// Num.of PE sections   : 3
//   .text    - 7680
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 6944
//   Module        -    1 (10 bytes)
//   TypeDef       -    8 (112 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   38 (228 bytes)
//   MethodDef     -   42 (588 bytes)     0 abstract, 0 native, 42 bodies
//   FieldDef      -    3 (18 bytes)      0 constant
//   MemberRef     -   21 (126 bytes)
//   ParamDef      -  154 (924 bytes)
//   Constant      -   34 (204 bytes)
//   CustomAttribute-  167 (1002 bytes)
//   PropertyMap   -    1 (4 bytes)
//   Property      -    2 (12 bytes)
//   MethodSemantic-    2 (12 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    4 (80 bytes)
//   GenericParam  -   21 (168 bytes)
//   Strings       -  1654 bytes
//   Blobs         -  1564 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   196 bytes

// CLR additional info : 128

// CLR method headers : 42
//   Num.of method bodies  - 42
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 42

// Managed code : 150
//   Ave method size - 3
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class HttpClientJsonExtensions       (public) (abstract) (auto) (ansi) (sealed) 
// Class HttpContentJsonExtensions      (public) (abstract) (auto) (ansi) (sealed) 
// Class JsonContent                    (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ System.Net.Http
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ System.Text.Json
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ System.Net.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly /*20000001*/ noplatform System.Net.Http.Json
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000003:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000004:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000002*/::.ctor(int32) /* 0A000002 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000005:0A000003*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000003*/::.ctor() /* 0A000003 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000006:0A000004*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*//DebuggingModes/*01000005*/) /* 0A000004 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000007:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000006*/::.ctor(string) /* 0A000005 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000007*/::.ctor() /* 0A000006 */ = ( 01 00 00 00 ) 
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000008*/::.ctor(bool) /* 0A000007 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000A:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 14 53 79 73 74 65 6D 2E 4E 65 74 2E 48 74   // ...System.Net.Ht
                                                                                                                                                                        74 70 2E 4A 73 6F 6E 00 00 )                      // tp.Json..
  .custom /*0C00000B:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000C:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000F:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000010:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 14 53 79 73 74 65 6D 2E 4E 65 74 2E 48 74   // ...System.Net.Ht
                                                                                                                                                                       74 70 2E 4A 73 6F 6E 00 00 )                      // tp.Json..
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 36 2E 30 2E 32 31 2E 35 32 32 31 30 00   // ...6.0.21.52210.
                                                                                                                                                                       00 ) 
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 36 2E 30 2E 30 2B 34 38 32 32 65 33 63   // ...6.0.0+4822e3c
                                                                                                                                                                                33 61 61 37 37 65 62 38 32 62 32 66 62 33 33 63   // 3aa77eb82b2fb33c
                                                                                                                                                                                39 33 32 31 66 39 32 33 63 66 31 31 64 64 64 65   // 9321f923cf11ddde
                                                                                                                                                                                36 00 00 )                                        // 6..
  .custom /*0C000014:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000015:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 14 53 79 73 74 65 6D 2E 4E 65 74 2E 48 74   // ...System.Net.Ht
                                                                                                                                                                 74 70 2E 4A 73 6F 6E 00 00 )                      // tp.Json..
  .custom /*0C000016:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
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
  .ver 6:0:0:0
}
.module System.Net.Http.Json.dll
// MVID: {58484299-c2c6-4bee-9e81-361e1722148d}
.custom /*0C000002:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


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
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
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
    // Method begins at RVA 0x2058
    // Code size       23 (0x17)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000014 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)000026       */ newarr     [System.Runtime/*23000001*/]System.Byte/*01000026*/
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
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000014 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C00001F:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000020:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000021:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000014 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000022:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000023:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000024:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000014 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ public abstract auto ansi sealed beforefieldinit System.Net.Http.Json.HttpClientJsonExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000029:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00002B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000006*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<object> 
          GetFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                           string requestUri,
                           class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                           class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001B*/ options,
                           [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 00 05 15 12 61 01 1C 12 65 0E 12 69 12 6D 11 71
  {
    .custom /*0C000027:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000028:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*08000001*/ 
    .custom /*0C000001:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000003*/ 
    .custom /*0C00001E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000005*/ 
    .custom /*0C000025:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000006*/  = nullref
    // Method begins at RVA 0x209d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*06000007*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<object> 
          GetFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                           string requestUri,
                           class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                           class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.JsonSerializerContext/*0100001D*/ context,
                           [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 00 05 15 12 61 01 1C 12 65 0E 12 69 12 75 11 71
  {
    .custom /*0C00002C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000007*/ 
    .custom /*0C000030:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000009*/ 
    .custom /*0C000039:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800000C*/  = nullref
    // Method begins at RVA 0x20a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*06000008*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<object> 
          GetFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                           string requestUri,
                           class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                           [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 00 04 15 12 61 01 1C 12 65 0E 12 69 11 71
  {
    .custom /*0C000032:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000033:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*0800000D*/ 
    .custom /*0C000045:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*0800000F*/ 
    .custom /*0C00004E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000011*/  = nullref
    // Method begins at RVA 0x20a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*06000009*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<object> 
          GetFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                           class [System.Runtime/*23000001*/]System.Uri/*0100001E*/ requestUri,
                           class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                           class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001B*/ options,
                           [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 00 05 15 12 61 01 1C 12 65 12 79 12 69 12 6D 11 71
  {
    .custom /*0C000037:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000038:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*08000012*/ 
    .custom /*0C000058:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000014*/ 
    .custom /*0C00005D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000016*/ 
    .custom /*0C000062:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000017*/  = nullref
    // Method begins at RVA 0x20a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*0600000A*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<object> 
          GetFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                           class [System.Runtime/*23000001*/]System.Uri/*0100001E*/ requestUri,
                           class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                           class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.JsonSerializerContext/*0100001D*/ context,
                           [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 00 05 15 12 61 01 1C 12 65 12 79 12 69 12 75 11 71
  {
    .custom /*0C00003B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000018*/ 
    .custom /*0C000067:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*0800001A*/ 
    .custom /*0C00006B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800001D*/  = nullref
    // Method begins at RVA 0x20a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*0600000B*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<object> 
          GetFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                           class [System.Runtime/*23000001*/]System.Uri/*0100001E*/ requestUri,
                           class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                           [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 00 04 15 12 61 01 1C 12 65 12 79 12 69 11 71
  {
    .custom /*0C00003D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00003E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*0800001E*/ 
    .custom /*0C000073:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000020*/ 
    .custom /*0C000078:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000022*/  = nullref
    // Method begins at RVA 0x20ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*0600000C*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<!!TValue> 
          GetFromJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                                   string requestUri,
                                   class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001B*/ options,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 61 01 1E 00 12 65 0E 12 6D 11 71
  {
    .custom /*0C000040:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000041:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000042:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*08000023*/ 
    .custom /*0C00007A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*08000024*/ 
    .custom /*0C00007C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000027*/  = nullref
    // Method begins at RVA 0x20af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*0600000D*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<!!TValue> 
          GetFromJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                                   string requestUri,
                                   class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*0100001F*/<!!TValue> jsonTypeInfo,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 61 01 1E 00 12 65 0E 15 12 7D 01 1E 00 11 71
  {
    .custom /*0C000044:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A000002*/ 
      .custom /*0C00001A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000028*/ 
    .custom /*0C000081:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*0800002A*/ 
    .custom /*0C000082:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800002C*/  = nullref
    // Method begins at RVA 0x20b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*0600000E*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<!!TValue> 
          GetFromJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                                   string requestUri,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 10 01 03 15 12 61 01 1E 00 12 65 0E 11 71
  {
    .custom /*0C000047:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000048:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000049:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*0800002D*/ 
    .custom /*0C000083:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*0800002E*/ 
    .custom /*0C000084:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000030*/  = nullref
    // Method begins at RVA 0x20b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*0600000F*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<!!TValue> 
          GetFromJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                                   class [System.Runtime/*23000001*/]System.Uri/*0100001E*/ requestUri,
                                   class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001B*/ options,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 61 01 1E 00 12 65 12 79 12 6D 11 71
  {
    .custom /*0C00004B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00004C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00004D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*08000031*/ 
    .custom /*0C000085:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*08000032*/ 
    .custom /*0C000086:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000035*/  = nullref
    // Method begins at RVA 0x20b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*06000010*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<!!TValue> 
          GetFromJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                                   class [System.Runtime/*23000001*/]System.Uri/*0100001E*/ requestUri,
                                   class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*0100001F*/<!!TValue> jsonTypeInfo,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 61 01 1E 00 12 65 12 79 15 12 7D 01 1E 00 11 71
  {
    .custom /*0C000050:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A000005*/ 
      .custom /*0C000026:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000036*/ 
    .custom /*0C000087:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000038*/ 
    .custom /*0C000088:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800003A*/  = nullref
    // Method begins at RVA 0x20bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*06000011*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<!!TValue> 
          GetFromJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                                   class [System.Runtime/*23000001*/]System.Uri/*0100001E*/ requestUri,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 10 01 03 15 12 61 01 1E 00 12 65 12 79 11 71
  {
    .custom /*0C000052:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000053:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000054:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*0800003B*/ 
    .custom /*0C000089:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*0800003C*/ 
    .custom /*0C00008A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*0800003E*/  = nullref
    // Method begins at RVA 0x20be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::GetFromJsonAsync

  .method /*06000012*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000020*/> 
          PostAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                                  string requestUri,
                                  !!TValue 'value',
                                  [opt] class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001B*/ options,
                                  [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 10 01 05 15 12 61 01 12 80 81 12 65 0E 1E 00 12 6D 11 71
  {
    .custom /*0C000056:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000057:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A000007*/ 
      .custom /*0C000031:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000040*/ 
    .custom /*0C00008B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000042*/  = nullref
    .custom /*0C00008C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000043*/  = nullref
    // Method begins at RVA 0x20c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PostAsJsonAsync

  .method /*06000013*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000020*/> 
          PostAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                                  string requestUri,
                                  !!TValue 'value',
                                  class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*0100001F*/<!!TValue> jsonTypeInfo,
                                  [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 10 01 05 15 12 61 01 12 80 81 12 65 0E 1E 00 15 12 7D 01 1E 00 11 71
  {
    .custom /*0C00005A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A000008*/ 
      .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000045*/ 
    .custom /*0C00008D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000048*/  = nullref
    // Method begins at RVA 0x20c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PostAsJsonAsync

  .method /*06000014*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000020*/> 
          PostAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                                  string requestUri,
                                  !!TValue 'value',
                                  valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 61 01 12 80 81 12 65 0E 1E 00 11 71
  {
    .custom /*0C00005B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00005C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A000009*/ 
      .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800004A*/ 
    .custom /*0C00008E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PostAsJsonAsync

  .method /*06000015*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000020*/> 
          PostAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                                  class [System.Runtime/*23000001*/]System.Uri/*0100001E*/ requestUri,
                                  !!TValue 'value',
                                  [opt] class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001B*/ options,
                                  [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 10 01 05 15 12 61 01 12 80 81 12 65 12 79 1E 00 12 6D 11 71
  {
    .custom /*0C00005F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000060:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A00000A*/ 
      .custom /*0C00003C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800004E*/ 
    .custom /*0C00008F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000050*/  = nullref
    .custom /*0C000090:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000051*/  = nullref
    // Method begins at RVA 0x20ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PostAsJsonAsync

  .method /*06000016*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000020*/> 
          PostAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                                  class [System.Runtime/*23000001*/]System.Uri/*0100001E*/ requestUri,
                                  !!TValue 'value',
                                  class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*0100001F*/<!!TValue> jsonTypeInfo,
                                  [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 10 01 05 15 12 61 01 12 80 81 12 65 12 79 1E 00 15 12 7D 01 1E 00 11 71
  {
    .custom /*0C000061:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A00000B*/ 
      .custom /*0C00003F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000053*/ 
    .custom /*0C000091:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000056*/  = nullref
    // Method begins at RVA 0x20cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PostAsJsonAsync

  .method /*06000017*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000020*/> 
          PostAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                                  class [System.Runtime/*23000001*/]System.Uri/*0100001E*/ requestUri,
                                  !!TValue 'value',
                                  valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 61 01 12 80 81 12 65 12 79 1E 00 11 71
  {
    .custom /*0C000063:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000064:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A00000C*/ 
      .custom /*0C000043:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000058*/ 
    .custom /*0C000092:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PostAsJsonAsync

  .method /*06000018*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000020*/> 
          PutAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                                 string requestUri,
                                 !!TValue 'value',
                                 [opt] class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001B*/ options,
                                 [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 10 01 05 15 12 61 01 12 80 81 12 65 0E 1E 00 12 6D 11 71
  {
    .custom /*0C000065:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000066:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A00000D*/ 
      .custom /*0C000046:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800005C*/ 
    .custom /*0C000093:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800005E*/  = nullref
    .custom /*0C000094:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800005F*/  = nullref
    // Method begins at RVA 0x20d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PutAsJsonAsync

  .method /*06000019*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000020*/> 
          PutAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                                 string requestUri,
                                 !!TValue 'value',
                                 class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*0100001F*/<!!TValue> jsonTypeInfo,
                                 [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 10 01 05 15 12 61 01 12 80 81 12 65 0E 1E 00 15 12 7D 01 1E 00 11 71
  {
    .custom /*0C000068:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A00000E*/ 
      .custom /*0C00004A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000061*/ 
    .custom /*0C000095:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000064*/  = nullref
    // Method begins at RVA 0x20d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PutAsJsonAsync

  .method /*0600001A*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000020*/> 
          PutAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                                 string requestUri,
                                 !!TValue 'value',
                                 valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 61 01 12 80 81 12 65 0E 1E 00 11 71
  {
    .custom /*0C000069:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00006A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A00000F*/ 
      .custom /*0C00004F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000066*/ 
    .custom /*0C000096:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PutAsJsonAsync

  .method /*0600001B*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000020*/> 
          PutAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                                 class [System.Runtime/*23000001*/]System.Uri/*0100001E*/ requestUri,
                                 !!TValue 'value',
                                 [opt] class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001B*/ options,
                                 [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 10 01 05 15 12 61 01 12 80 81 12 65 12 79 1E 00 12 6D 11 71
  {
    .custom /*0C00006C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00006D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A000010*/ 
      .custom /*0C000051:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800006A*/ 
    .custom /*0C000097:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800006C*/  = nullref
    .custom /*0C000098:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800006D*/  = nullref
    // Method begins at RVA 0x20dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PutAsJsonAsync

  .method /*0600001C*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000020*/> 
          PutAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                                 class [System.Runtime/*23000001*/]System.Uri/*0100001E*/ requestUri,
                                 !!TValue 'value',
                                 class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*0100001F*/<!!TValue> jsonTypeInfo,
                                 [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 10 01 05 15 12 61 01 12 80 81 12 65 12 79 1E 00 15 12 7D 01 1E 00 11 71
  {
    .custom /*0C00006E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A000011*/ 
      .custom /*0C000055:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800006F*/ 
    .custom /*0C000099:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000072*/  = nullref
    // Method begins at RVA 0x20df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PutAsJsonAsync

  .method /*0600001D*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<class [System.Net.Http/*23000002*/]System.Net.Http.HttpResponseMessage/*01000020*/> 
          PutAsJsonAsync<TValue>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpClient/*01000019*/ client,
                                 class [System.Runtime/*23000001*/]System.Uri/*0100001E*/ requestUri,
                                 !!TValue 'value',
                                 valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 10 01 04 15 12 61 01 12 80 81 12 65 12 79 1E 00 11 71
  {
    .custom /*0C00006F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000070:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param type TValue /*2A000012*/ 
      .custom /*0C000059:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000074*/ 
    .custom /*0C00009A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpClientJsonExtensions::PutAsJsonAsync

} // end of class System.Net.Http.Json.HttpClientJsonExtensions

.class /*02000007*/ public abstract auto ansi sealed beforefieldinit System.Net.Http.Json.HttpContentJsonExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C00002D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00002F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*0600001E*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<object> 
          ReadFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpContent/*01000021*/ content,
                            class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                            [opt] class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001B*/ options,
                            [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 00 04 15 12 61 01 1C 12 80 85 12 69 12 6D 11 71
  {
    .custom /*0C000071:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000072:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*08000077*/ 
    .custom /*0C00009B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [3]/*0800007A*/  = nullref
    .custom /*0C00009C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800007B*/  = nullref
    // Method begins at RVA 0x20e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentJsonExtensions::ReadFromJsonAsync

  .method /*0600001F*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<object> 
          ReadFromJsonAsync(class [System.Net.Http/*23000002*/]System.Net.Http.HttpContent/*01000021*/ content,
                            class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 'type',
                            class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.JsonSerializerContext/*0100001D*/ context,
                            [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 00 04 15 12 61 01 1C 12 80 85 12 69 12 75 11 71
  {
    .custom /*0C000074:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*0800007C*/ 
    .custom /*0C00009D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [4]/*08000080*/  = nullref
    // Method begins at RVA 0x20e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentJsonExtensions::ReadFromJsonAsync

  .method /*06000020*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<!!T> 
          ReadFromJsonAsync<T>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpContent/*01000021*/ content,
                               [opt] class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001B*/ options,
                               [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 10 01 03 15 12 61 01 1E 00 12 80 85 12 6D 11 71
  {
    .custom /*0C000075:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000076:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000077:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*08000081*/ 
    .custom /*0C00009E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*08000082*/ 
    .custom /*0C00009F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000083*/  = nullref
    .param [3]/*08000084*/  = nullref
    // Method begins at RVA 0x20eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentJsonExtensions::ReadFromJsonAsync

  .method /*06000021*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000018*/<!!T> 
          ReadFromJsonAsync<T>(class [System.Net.Http/*23000002*/]System.Net.Http.HttpContent/*01000021*/ content,
                               class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.Metadata.JsonTypeInfo`1/*0100001F*/<!!T> jsonTypeInfo,
                               [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 10 01 03 15 12 61 01 1E 00 12 80 85 15 12 7D 01 1E 00 11 71
  {
    .custom /*0C000079:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000014*/ 
      .custom /*0C00005E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000085*/ 
    .custom /*0C0000A0:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [3]/*08000088*/  = nullref
    // Method begins at RVA 0x20ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpContentJsonExtensions::ReadFromJsonAsync

} // end of class System.Net.Http.Json.HttpContentJsonExtensions

.class /*02000008*/ public auto ansi sealed beforefieldinit System.Net.Http.Json.JsonContent
       extends [System.Net.Http/*23000002*/]System.Net.Http.HttpContent/*01000021*/
{
  .custom /*0C000034:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000035:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000022*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20f1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Net.Http/*23000002*/]System.Net.Http.HttpContent/*01000021*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method JsonContent::.ctor

  .method /*06000023*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001A*/ 
          get_ObjectType() cil managed
  // SIG: 20 00 12 69
  {
    // Method begins at RVA 0x20f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonContent::get_ObjectType

  .method /*06000024*/ public hidebysig specialname 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C00007B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonContent::get_Value

  .method /*06000025*/ public hidebysig static 
          class System.Net.Http.Json.JsonContent/*02000008*/ 
          Create(object inputValue,
                 class [System.Runtime/*23000001*/]System.Type/*0100001A*/ inputType,
                 [opt] class [System.Net.Http/*23000002*/]System.Net.Http.Headers.MediaTypeHeaderValue/*01000022*/ mediaType,
                 [opt] class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001B*/ options) cil managed
  // SIG: 00 04 12 20 1C 12 69 12 80 89 12 6D
  {
    .custom /*0C00007D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00007E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*08000089*/ 
    .custom /*0C0000A1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800008B*/ 
    .custom /*0C0000A2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*0800008C*/  = nullref
    .param [4]/*0800008D*/  = nullref
    // Method begins at RVA 0x20ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonContent::Create

  .method /*06000026*/ public hidebysig static 
          class System.Net.Http.Json.JsonContent/*02000008*/ 
          Create<T>(!!T inputValue,
                    [opt] class [System.Net.Http/*23000002*/]System.Net.Http.Headers.MediaTypeHeaderValue/*01000022*/ mediaType,
                    [opt] class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*0100001B*/ options) cil managed
  // SIG: 10 01 03 12 20 1E 00 12 80 89 12 6D
  {
    .custom /*0C00007F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000080:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 D9 4A 53 4F 4E 20 73 65 72 69 61 6C 69   // ....JSON seriali
                                                                                                                                                                                            7A 61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72   // zation and deser
                                                                                                                                                                                            69 61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74   // ialization might
                                                                                                                                                                                            20 72 65 71 75 69 72 65 20 74 79 70 65 73 20 74   //  require types t
                                                                                                                                                                                            68 61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74   // hat cannot be st
                                                                                                                                                                                            61 74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65   // atically analyze
                                                                                                                                                                                            64 2E 20 55 73 65 20 74 68 65 20 6F 76 65 72 6C   // d. Use the overl
                                                                                                                                                                                            6F 61 64 20 74 68 61 74 20 74 61 6B 65 73 20 61   // oad that takes a
                                                                                                                                                                                            20 4A 73 6F 6E 54 79 70 65 49 6E 66 6F 20 6F 72   //  JsonTypeInfo or
                                                                                                                                                                                            20 4A 73 6F 6E 53 65 72 69 61 6C 69 7A 65 72 43   //  JsonSerializerC
                                                                                                                                                                                            6F 6E 74 65 78 74 2C 20 6F 72 20 6D 61 6B 65 20   // ontext, or make 
                                                                                                                                                                                            73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65 20   // sure all of the 
                                                                                                                                                                                            72 65 71 75 69 72 65 64 20 74 79 70 65 73 20 61   // required types a
                                                                                                                                                                                            72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 )    // re preserved...
    .param [0]/*0800008E*/ 
    .custom /*0C0000A3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800008F*/ 
    .custom /*0C0000A4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000090*/  = nullref
    .param [3]/*08000091*/  = nullref
    // Method begins at RVA 0x2102
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonContent::Create

  .method /*06000027*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000023*/ 
          SerializeToStreamAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/ 'stream',
                                 class [System.Net.Primitives/*23000004*/]System.Net.TransportContext/*01000025*/ context) cil managed
  // SIG: 20 02 12 80 8D 12 80 91 12 80 95
  {
    .param [2]/*08000093*/ 
    .custom /*0C0000A5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2105
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonContent::SerializeToStreamAsync

  .method /*06000028*/ family hidebysig virtual 
          instance bool  TryComputeLength([out] int64& length) cil managed
  // SIG: 20 01 02 10 0A
  {
    // Method begins at RVA 0x2108
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonContent::TryComputeLength

  .method /*06000029*/ family hidebysig virtual 
          instance void  SerializeToStream(class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/ 'stream',
                                           class [System.Net.Primitives/*23000004*/]System.Net.TransportContext/*01000025*/ context,
                                           valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 20 03 01 12 80 91 12 80 95 11 71
  {
    .param [2]/*08000096*/ 
    .custom /*0C0000A6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x210b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method JsonContent::SerializeToStream

  .method /*0600002A*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000023*/ 
          SerializeToStreamAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/ 'stream',
                                 class [System.Net.Primitives/*23000004*/]System.Net.TransportContext/*01000025*/ context,
                                 valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001C*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 8D 12 80 91 12 80 95 11 71
  {
    .param [2]/*08000099*/ 
    .custom /*0C0000A7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x210d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonContent::SerializeToStreamAsync

  .property /*17000001*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001A*/
          ObjectType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001A*/ System.Net.Http.Json.JsonContent/*02000008*/::get_ObjectType() /* 06000023 */
  } // end of property JsonContent::ObjectType
  .property /*17000002*/ instance callconv(8) object
          Value()
  {
    .custom /*0C000019:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance object System.Net.Http.Json.JsonContent/*02000008*/::get_Value() /* 06000024 */
  } // end of property JsonContent::Value
} // end of class System.Net.Http.Json.JsonContent


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
