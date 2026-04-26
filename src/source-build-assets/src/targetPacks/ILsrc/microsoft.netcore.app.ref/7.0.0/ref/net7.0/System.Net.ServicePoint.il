


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
// Time-date stamp:            0xce3e1948
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
// Addr. of entry point:           0x00003d76
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
// Checksum:                       0x00007924
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00003d23 [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x00000494] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002800 [0x000028a0] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00003c38 [0x00000054] address [size] of Debug Directory:           
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
//              0x00001d7c Virtual Size
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
//              0x00000494 Virtual Size
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
//              Entry 1: Type 0x3 Offset 0x00000d78
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00003d65 Import Name Table
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
// 0x00002144 [0x00001a74] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00003bb8 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000abc Size
//              '#~' Name
//    Stream 2:
//              0x00000b28 Offset
//              0x000009e8 Size
//              '#Strings' Name
//    Stream 3:
//              0x00001510 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00001514 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001524 Offset
//              0x00000550 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000000901a01d57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 20640
// PE header size       : 512 (496 used)    ( 2.48%)
// PE additional info   : 11827             (57.30%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.35%)
// CLR meta-data size  : 6772               (32.81%)
// CLR additional info : 128                ( 0.62%)
// CLR method headers  : 59                 ( 0.29%)
// Managed code         : 182               ( 0.88%)
// Data                 : 2048              ( 9.92%)
// Unaccounted          : -960              (-4.65%)

// Num.of PE sections   : 3
//   .text    - 7680
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 6772
//   Module        -    1 (10 bytes)
//   TypeDef       -   10 (140 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   37 (222 bytes)
//   MethodDef     -   63 (882 bytes)     0 abstract, 0 native, 59 bodies
//   FieldDef      -   13 (78 bytes)      0 constant
//   MemberRef     -   22 (132 bytes)
//   ParamDef      -   42 (252 bytes)
//   Constant      -    8 (48 bytes)
//   CustomAttribute-   60 (360 bytes)
//   PropertyMap   -    2 (8 bytes)
//   Property      -   27 (162 bytes)
//   MethodSemantic-   45 (270 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    4 (80 bytes)
//   Strings       -  2533 bytes
//   Blobs         -  1360 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   193 bytes

// CLR additional info : 128

// CLR method headers : 59
//   Num.of method bodies  - 59
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 59

// Managed code : 182
//   Ave method size - 3
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class RefSafetyRulesAttribute        (auto) (ansi) (sealed) 
// Class BindIPEndPoint                 (public) (auto) (ansi) (sealed) 
// Class SecurityProtocolType           (public) (auto) (ansi) (sealed) 
// Class ServicePoint                   (public) (auto) (ansi) 
// Class ServicePointManager            (public) (auto) (ansi) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000002*/ System.Net.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000003*/ System.Security.Cryptography
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000004*/ System.Net.Security
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly /*20000001*/ noplatform System.Net.ServicePoint
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000003:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000001*/::.ctor(int32) /* 0A000001 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000004:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000002*/::.ctor() /* 0A000002 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000005:0A000003*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*//DebuggingModes/*01000004*/) /* 0A000003 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000006:0A000004*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 37 2E 30 01 00 54 0E 14   // ersion=v7.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 37 2E 30 )          // Name..NET 7.0
  .custom /*0C000007:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 17 53 79 73 74 65 6D 2E 4E 65 74 2E 53 65   // ...System.Net.Se
                                                                                                                                                                        72 76 69 63 65 50 6F 69 6E 74 00 00 )             // rvicePoint..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000E:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000F:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 17 53 79 73 74 65 6D 2E 4E 65 74 2E 53 65   // ...System.Net.Se
                                                                                                                                                                       72 76 69 63 65 50 6F 69 6E 74 00 00 )             // rvicePoint..
  .custom /*0C000010:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 37 2E 30 2E 32 32 2E 35 31 38 30 35 00   // ...7.0.22.51805.
                                                                                                                                                                       00 ) 
  .custom /*0C000011:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 37 2E 30 2E 30 2B 64 30 39 39 66 30 37   // ...7.0.0+d099f07
                                                                                                                                                                                35 65 34 35 64 32 61 61 36 30 30 37 61 32 32 62   // 5e45d2aa6007a22b
                                                                                                                                                                                37 31 62 34 35 61 30 38 37 35 38 35 35 39 66 38   // 71b45a08758559f8
                                                                                                                                                                                30 00 00 )                                        // 0..
  .custom /*0C000012:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 17 53 79 73 74 65 6D 2E 4E 65 74 2E 53 65   // ...System.Net.Se
                                                                                                                                                                 72 76 69 63 65 50 6F 69 6E 74 00 00 )             // rvicePoint..
  .custom /*0C000014:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
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
  .ver 7:0:0:0
}
.module System.Net.ServicePoint.dll
// MVID: {db846c05-c99b-4de8-b9c6-5eb08091b5b2}
.custom /*0C000001:06000006*/ instance void System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::.ctor(int32) /* 06000006 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000002:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000015:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000016:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000018:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000019:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001A:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000015 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)000025       */ newarr     [System.Runtime/*23000001*/]System.Byte/*01000025*/
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
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000015 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00001C:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00001D:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001E:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000015 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000020:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000021:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000022:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000015 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.RefSafetyRulesAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000023:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000024:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000025:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000015 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000004       */ stfld      int32 System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::Version /* 04000004 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method RefSafetyRulesAttribute::.ctor

} // end of class System.Runtime.CompilerServices.RefSafetyRulesAttribute

.class /*02000007*/ public auto ansi sealed System.Net.BindIPEndPoint
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000015*/
{
  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method BindIPEndPoint::.ctor

  .method /*06000008*/ public hidebysig newslot virtual 
          instance class [System.Net.Primitives/*23000002*/]System.Net.IPEndPoint/*01000017*/ 
          Invoke(class System.Net.ServicePoint/*02000009*/ servicePoint,
                 class [System.Net.Primitives/*23000002*/]System.Net.IPEndPoint/*01000017*/ remoteEndPoint,
                 int32 retryCount) runtime managed
  // SIG: 20 03 12 5D 12 24 12 5D 08
  {
    .custom /*0C000029:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  } // end of method BindIPEndPoint::Invoke

  .method /*06000009*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000018*/ 
          BeginInvoke(class System.Net.ServicePoint/*02000009*/ servicePoint,
                      class [System.Net.Primitives/*23000002*/]System.Net.IPEndPoint/*01000017*/ remoteEndPoint,
                      int32 retryCount,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*01000019*/ callback,
                      object 'object') runtime managed
  // SIG: 20 05 12 61 12 24 12 5D 08 12 65 1C
  {
    .param [1]/*08000006*/ 
    .custom /*0C000026:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000007*/ 
    .custom /*0C000028:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  } // end of method BindIPEndPoint::BeginInvoke

  .method /*0600000A*/ public hidebysig newslot virtual 
          instance class [System.Net.Primitives/*23000002*/]System.Net.IPEndPoint/*01000017*/ 
          EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000018*/ result) runtime managed
  // SIG: 20 01 12 5D 12 61
  {
    .param [0]/*0800000B*/ 
    .custom /*0C00002F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  } // end of method BindIPEndPoint::EndInvoke

} // end of class System.Net.BindIPEndPoint

.class /*02000008*/ public auto ansi sealed System.Net.SecurityProtocolType
       extends [System.Runtime/*23000001*/]System.Enum/*0100001B*/
{
  .custom /*0C00002A:0A000012*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100001A*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000005*/ public specialname rtspecialname int32 value__
  .field /*04000006*/ public static literal valuetype System.Net.SecurityProtocolType/*02000008*/ SystemDefault = int32(0x00000000)
  .field /*04000007*/ public static literal valuetype System.Net.SecurityProtocolType/*02000008*/ Ssl3 = int32(0x00000030)
  .custom /*0C000027:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001C*/::.ctor(string) /* 0A000013 */ = ( 01 00 43 53 65 63 75 72 69 74 79 50 72 6F 74 6F   // ..CSecurityProto
                                                                                                                                                 63 6F 6C 54 79 70 65 2E 53 73 6C 33 20 68 61 73   // colType.Ssl3 has
                                                                                                                                                 20 62 65 65 6E 20 64 65 70 72 65 63 61 74 65 64   //  been deprecated
                                                                                                                                                 20 61 6E 64 20 69 73 20 6E 6F 74 20 73 75 70 70   //  and is not supp
                                                                                                                                                 6F 72 74 65 64 2E 00 00 )                         // orted...
  .field /*04000008*/ public static literal valuetype System.Net.SecurityProtocolType/*02000008*/ Tls = int32(0x000000C0)
  .field /*04000009*/ public static literal valuetype System.Net.SecurityProtocolType/*02000008*/ Tls11 = int32(0x00000300)
  .field /*0400000A*/ public static literal valuetype System.Net.SecurityProtocolType/*02000008*/ Tls12 = int32(0x00000C00)
  .field /*0400000B*/ public static literal valuetype System.Net.SecurityProtocolType/*02000008*/ Tls13 = int32(0x00003000)
} // end of class System.Net.SecurityProtocolType

.class /*02000009*/ public auto ansi beforefieldinit System.Net.ServicePoint
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C00002B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600000B*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ac
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ServicePoint::.ctor

  .method /*0600000C*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Uri/*0100001D*/ 
          get_Address() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x20b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePoint::get_Address

  .method /*0600000D*/ public hidebysig specialname 
          instance class System.Net.BindIPEndPoint/*02000007*/ 
          get_BindIPEndPointDelegate() cil managed
  // SIG: 20 00 12 1C
  {
    .custom /*0C000030:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePoint::get_BindIPEndPointDelegate

  .method /*0600000E*/ public hidebysig specialname 
          instance void  set_BindIPEndPointDelegate(class System.Net.BindIPEndPoint/*02000007*/ 'value') cil managed
  // SIG: 20 01 01 12 1C
  {
    .custom /*0C000031:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ba
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ServicePoint::set_BindIPEndPointDelegate

  .method /*0600000F*/ public hidebysig specialname 
          instance class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Certificate/*0100001E*/ 
          get_Certificate() cil managed
  // SIG: 20 00 12 79
  {
    .custom /*0C000032:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePoint::get_Certificate

  .method /*06000010*/ public hidebysig specialname 
          instance class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Certificate/*0100001E*/ 
          get_ClientCertificate() cil managed
  // SIG: 20 00 12 79
  {
    .custom /*0C000033:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePoint::get_ClientCertificate

  .method /*06000011*/ public hidebysig specialname 
          instance int32  get_ConnectionLeaseTimeout() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePoint::get_ConnectionLeaseTimeout

  .method /*06000012*/ public hidebysig specialname 
          instance void  set_ConnectionLeaseTimeout(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20c5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ServicePoint::set_ConnectionLeaseTimeout

  .method /*06000013*/ public hidebysig specialname 
          instance int32  get_ConnectionLimit() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePoint::get_ConnectionLimit

  .method /*06000014*/ public hidebysig specialname 
          instance void  set_ConnectionLimit(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20ca
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ServicePoint::set_ConnectionLimit

  .method /*06000015*/ public hidebysig specialname 
          instance string  get_ConnectionName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePoint::get_ConnectionName

  .method /*06000016*/ public hidebysig specialname 
          instance int32  get_CurrentConnections() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePoint::get_CurrentConnections

  .method /*06000017*/ public hidebysig specialname 
          instance bool  get_Expect100Continue() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePoint::get_Expect100Continue

  .method /*06000018*/ public hidebysig specialname 
          instance void  set_Expect100Continue(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20d5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ServicePoint::set_Expect100Continue

  .method /*06000019*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTime/*0100001F*/ 
          get_IdleSince() cil managed
  // SIG: 20 00 11 7D
  {
    // Method begins at RVA 0x20d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePoint::get_IdleSince

  .method /*0600001A*/ public hidebysig specialname 
          instance int32  get_MaxIdleTime() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePoint::get_MaxIdleTime

  .method /*0600001B*/ public hidebysig specialname 
          instance void  set_MaxIdleTime(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20dd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ServicePoint::set_MaxIdleTime

  .method /*0600001C*/ public hidebysig newslot specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Version/*01000020*/ 
          get_ProtocolVersion() cil managed
  // SIG: 20 00 12 80 81
  {
    // Method begins at RVA 0x20df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePoint::get_ProtocolVersion

  .method /*0600001D*/ public hidebysig specialname 
          instance int32  get_ReceiveBufferSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePoint::get_ReceiveBufferSize

  .method /*0600001E*/ public hidebysig specialname 
          instance void  set_ReceiveBufferSize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20e5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ServicePoint::set_ReceiveBufferSize

  .method /*0600001F*/ public hidebysig specialname 
          instance bool  get_SupportsPipelining() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePoint::get_SupportsPipelining

  .method /*06000020*/ public hidebysig specialname 
          instance bool  get_UseNagleAlgorithm() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePoint::get_UseNagleAlgorithm

  .method /*06000021*/ public hidebysig specialname 
          instance void  set_UseNagleAlgorithm(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20ed
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ServicePoint::set_UseNagleAlgorithm

  .method /*06000022*/ public hidebysig instance bool 
          CloseConnectionGroup(string connectionGroupName) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x20ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePoint::CloseConnectionGroup

  .method /*06000023*/ public hidebysig instance void 
          SetTcpKeepAlive(bool enabled,
                          int32 keepAliveTime,
                          int32 keepAliveInterval) cil managed
  // SIG: 20 03 01 02 08 08
  {
    // Method begins at RVA 0x20f2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ServicePoint::SetTcpKeepAlive

  .property /*17000001*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Uri/*0100001D*/
          Address()
  {
    .get instance class [System.Runtime/*23000001*/]System.Uri/*0100001D*/ System.Net.ServicePoint/*02000009*/::get_Address() /* 0600000C */
  } // end of property ServicePoint::Address
  .property /*17000002*/ instance callconv(8) class System.Net.BindIPEndPoint/*02000007*/
          BindIPEndPointDelegate()
  {
    .custom /*0C000017:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Net.BindIPEndPoint/*02000007*/ System.Net.ServicePoint/*02000009*/::get_BindIPEndPointDelegate() /* 0600000D */
    .set instance void System.Net.ServicePoint/*02000009*/::set_BindIPEndPointDelegate(class System.Net.BindIPEndPoint/*02000007*/) /* 0600000E */
  } // end of property ServicePoint::BindIPEndPointDelegate
  .property /*17000003*/ instance callconv(8) class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Certificate/*0100001E*/
          Certificate()
  {
    .custom /*0C00001B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Certificate/*0100001E*/ System.Net.ServicePoint/*02000009*/::get_Certificate() /* 0600000F */
  } // end of property ServicePoint::Certificate
  .property /*17000004*/ instance callconv(8) class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Certificate/*0100001E*/
          ClientCertificate()
  {
    .custom /*0C00001F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Security.Cryptography/*23000003*/]System.Security.Cryptography.X509Certificates.X509Certificate/*0100001E*/ System.Net.ServicePoint/*02000009*/::get_ClientCertificate() /* 06000010 */
  } // end of property ServicePoint::ClientCertificate
  .property /*17000005*/ instance callconv(8) int32
          ConnectionLeaseTimeout()
  {
    .get instance int32 System.Net.ServicePoint/*02000009*/::get_ConnectionLeaseTimeout() /* 06000011 */
    .set instance void System.Net.ServicePoint/*02000009*/::set_ConnectionLeaseTimeout(int32) /* 06000012 */
  } // end of property ServicePoint::ConnectionLeaseTimeout
  .property /*17000006*/ instance callconv(8) int32
          ConnectionLimit()
  {
    .get instance int32 System.Net.ServicePoint/*02000009*/::get_ConnectionLimit() /* 06000013 */
    .set instance void System.Net.ServicePoint/*02000009*/::set_ConnectionLimit(int32) /* 06000014 */
  } // end of property ServicePoint::ConnectionLimit
  .property /*17000007*/ instance callconv(8) string
          ConnectionName()
  {
    .get instance string System.Net.ServicePoint/*02000009*/::get_ConnectionName() /* 06000015 */
  } // end of property ServicePoint::ConnectionName
  .property /*17000008*/ instance callconv(8) int32
          CurrentConnections()
  {
    .get instance int32 System.Net.ServicePoint/*02000009*/::get_CurrentConnections() /* 06000016 */
  } // end of property ServicePoint::CurrentConnections
  .property /*17000009*/ instance callconv(8) bool
          Expect100Continue()
  {
    .get instance bool System.Net.ServicePoint/*02000009*/::get_Expect100Continue() /* 06000017 */
    .set instance void System.Net.ServicePoint/*02000009*/::set_Expect100Continue(bool) /* 06000018 */
  } // end of property ServicePoint::Expect100Continue
  .property /*1700000A*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTime/*0100001F*/
          IdleSince()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTime/*0100001F*/ System.Net.ServicePoint/*02000009*/::get_IdleSince() /* 06000019 */
  } // end of property ServicePoint::IdleSince
  .property /*1700000B*/ instance callconv(8) int32
          MaxIdleTime()
  {
    .get instance int32 System.Net.ServicePoint/*02000009*/::get_MaxIdleTime() /* 0600001A */
    .set instance void System.Net.ServicePoint/*02000009*/::set_MaxIdleTime(int32) /* 0600001B */
  } // end of property ServicePoint::MaxIdleTime
  .property /*1700000C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Version/*01000020*/
          ProtocolVersion()
  {
    .get instance class [System.Runtime/*23000001*/]System.Version/*01000020*/ System.Net.ServicePoint/*02000009*/::get_ProtocolVersion() /* 0600001C */
  } // end of property ServicePoint::ProtocolVersion
  .property /*1700000D*/ instance callconv(8) int32
          ReceiveBufferSize()
  {
    .get instance int32 System.Net.ServicePoint/*02000009*/::get_ReceiveBufferSize() /* 0600001D */
    .set instance void System.Net.ServicePoint/*02000009*/::set_ReceiveBufferSize(int32) /* 0600001E */
  } // end of property ServicePoint::ReceiveBufferSize
  .property /*1700000E*/ instance callconv(8) bool
          SupportsPipelining()
  {
    .get instance bool System.Net.ServicePoint/*02000009*/::get_SupportsPipelining() /* 0600001F */
  } // end of property ServicePoint::SupportsPipelining
  .property /*1700000F*/ instance callconv(8) bool
          UseNagleAlgorithm()
  {
    .get instance bool System.Net.ServicePoint/*02000009*/::get_UseNagleAlgorithm() /* 06000020 */
    .set instance void System.Net.ServicePoint/*02000009*/::set_UseNagleAlgorithm(bool) /* 06000021 */
  } // end of property ServicePoint::UseNagleAlgorithm
} // end of class System.Net.ServicePoint

.class /*0200000A*/ public auto ansi beforefieldinit System.Net.ServicePointManager
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C00002D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*0400000C*/ public static literal int32 DefaultNonPersistentConnectionLimit = int32(0x00000004)
  .field /*0400000D*/ public static literal int32 DefaultPersistentConnectionLimit = int32(0x00000002)
  .method /*06000024*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20f4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ServicePointManager::.ctor

  .method /*06000025*/ public hidebysig specialname static 
          bool  get_CheckCertificateRevocationList() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x20fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePointManager::get_CheckCertificateRevocationList

  .method /*06000026*/ public hidebysig specialname static 
          void  set_CheckCertificateRevocationList(bool 'value') cil managed
  // SIG: 00 01 01 02
  {
    // Method begins at RVA 0x20ff
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ServicePointManager::set_CheckCertificateRevocationList

  .method /*06000027*/ public hidebysig specialname static 
          int32  get_DefaultConnectionLimit() cil managed
  // SIG: 00 00 08
  {
    // Method begins at RVA 0x2101
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePointManager::get_DefaultConnectionLimit

  .method /*06000028*/ public hidebysig specialname static 
          void  set_DefaultConnectionLimit(int32 'value') cil managed
  // SIG: 00 01 01 08
  {
    // Method begins at RVA 0x2104
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ServicePointManager::set_DefaultConnectionLimit

  .method /*06000029*/ public hidebysig specialname static 
          int32  get_DnsRefreshTimeout() cil managed
  // SIG: 00 00 08
  {
    // Method begins at RVA 0x2106
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePointManager::get_DnsRefreshTimeout

  .method /*0600002A*/ public hidebysig specialname static 
          void  set_DnsRefreshTimeout(int32 'value') cil managed
  // SIG: 00 01 01 08
  {
    // Method begins at RVA 0x2109
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ServicePointManager::set_DnsRefreshTimeout

  .method /*0600002B*/ public hidebysig specialname static 
          bool  get_EnableDnsRoundRobin() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x210b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePointManager::get_EnableDnsRoundRobin

  .method /*0600002C*/ public hidebysig specialname static 
          void  set_EnableDnsRoundRobin(bool 'value') cil managed
  // SIG: 00 01 01 02
  {
    // Method begins at RVA 0x210e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ServicePointManager::set_EnableDnsRoundRobin

  .method /*0600002D*/ public hidebysig specialname static 
          valuetype [System.Net.Security/*23000004*/]System.Net.Security.EncryptionPolicy/*01000021*/ 
          get_EncryptionPolicy() cil managed
  // SIG: 00 00 11 80 85
  {
    // Method begins at RVA 0x2110
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePointManager::get_EncryptionPolicy

  .method /*0600002E*/ public hidebysig specialname static 
          bool  get_Expect100Continue() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x2113
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePointManager::get_Expect100Continue

  .method /*0600002F*/ public hidebysig specialname static 
          void  set_Expect100Continue(bool 'value') cil managed
  // SIG: 00 01 01 02
  {
    // Method begins at RVA 0x2116
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ServicePointManager::set_Expect100Continue

  .method /*06000030*/ public hidebysig specialname static 
          int32  get_MaxServicePointIdleTime() cil managed
  // SIG: 00 00 08
  {
    // Method begins at RVA 0x2118
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePointManager::get_MaxServicePointIdleTime

  .method /*06000031*/ public hidebysig specialname static 
          void  set_MaxServicePointIdleTime(int32 'value') cil managed
  // SIG: 00 01 01 08
  {
    // Method begins at RVA 0x211b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ServicePointManager::set_MaxServicePointIdleTime

  .method /*06000032*/ public hidebysig specialname static 
          int32  get_MaxServicePoints() cil managed
  // SIG: 00 00 08
  {
    // Method begins at RVA 0x211d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePointManager::get_MaxServicePoints

  .method /*06000033*/ public hidebysig specialname static 
          void  set_MaxServicePoints(int32 'value') cil managed
  // SIG: 00 01 01 08
  {
    // Method begins at RVA 0x2120
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ServicePointManager::set_MaxServicePoints

  .method /*06000034*/ public hidebysig specialname static 
          bool  get_ReusePort() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x2122
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePointManager::get_ReusePort

  .method /*06000035*/ public hidebysig specialname static 
          void  set_ReusePort(bool 'value') cil managed
  // SIG: 00 01 01 02
  {
    // Method begins at RVA 0x2125
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ServicePointManager::set_ReusePort

  .method /*06000036*/ public hidebysig specialname static 
          valuetype System.Net.SecurityProtocolType/*02000008*/ 
          get_SecurityProtocol() cil managed
  // SIG: 00 00 11 20
  {
    // Method begins at RVA 0x2127
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePointManager::get_SecurityProtocol

  .method /*06000037*/ public hidebysig specialname static 
          void  set_SecurityProtocol(valuetype System.Net.SecurityProtocolType/*02000008*/ 'value') cil managed
  // SIG: 00 01 01 11 20
  {
    // Method begins at RVA 0x212a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ServicePointManager::set_SecurityProtocol

  .method /*06000038*/ public hidebysig specialname static 
          class [System.Net.Security/*23000004*/]System.Net.Security.RemoteCertificateValidationCallback/*01000022*/ 
          get_ServerCertificateValidationCallback() cil managed
  // SIG: 00 00 12 80 89
  {
    .custom /*0C000038:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x212c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePointManager::get_ServerCertificateValidationCallback

  .method /*06000039*/ public hidebysig specialname static 
          void  set_ServerCertificateValidationCallback(class [System.Net.Security/*23000004*/]System.Net.Security.RemoteCertificateValidationCallback/*01000022*/ 'value') cil managed
  // SIG: 00 01 01 12 80 89
  {
    .custom /*0C000039:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x212f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ServicePointManager::set_ServerCertificateValidationCallback

  .method /*0600003A*/ public hidebysig specialname static 
          bool  get_UseNagleAlgorithm() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x2131
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePointManager::get_UseNagleAlgorithm

  .method /*0600003B*/ public hidebysig specialname static 
          void  set_UseNagleAlgorithm(bool 'value') cil managed
  // SIG: 00 01 01 02
  {
    // Method begins at RVA 0x2134
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ServicePointManager::set_UseNagleAlgorithm

  .method /*0600003C*/ public hidebysig static 
          class System.Net.ServicePoint/*02000009*/ 
          FindServicePoint(string uriString,
                           class [System.Net.Primitives/*23000002*/]System.Net.IWebProxy/*01000023*/ proxy) cil managed
  // SIG: 00 02 12 24 0E 12 80 8D
  {
    .custom /*0C00003A:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001C*/::.ctor(string) /* 0A000013 */ = ( 01 00 5D 57 65 62 52 65 71 75 65 73 74 2C 20 48   // ..]WebRequest, H
                                                                                                                                                   74 74 70 57 65 62 52 65 71 75 65 73 74 2C 20 53   // ttpWebRequest, S
                                                                                                                                                   65 72 76 69 63 65 50 6F 69 6E 74 2C 20 61 6E 64   // ervicePoint, and
                                                                                                                                                   20 57 65 62 43 6C 69 65 6E 74 20 61 72 65 20 6F   //  WebClient are o
                                                                                                                                                   62 73 6F 6C 65 74 65 2E 20 55 73 65 20 48 74 74   // bsolete. Use Htt
                                                                                                                                                   70 43 6C 69 65 6E 74 20 69 6E 73 74 65 61 64 2E   // pClient instead.
                                                                                                                                                   02 00 54 0E 0C 44 69 61 67 6E 6F 73 74 69 63 49   // ..T..DiagnosticI
                                                                                                                                                   64 0A 53 59 53 4C 49 42 30 30 31 34 54 0E 09 55   // d.SYSLIB0014T..U
                                                                                                                                                   72 6C 46 6F 72 6D 61 74 22 68 74 74 70 73 3A 2F   // rlFormat"https:/
                                                                                                                                                   2F 61 6B 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77   // /aka.ms/dotnet-w
                                                                                                                                                   61 72 6E 69 6E 67 73 2F 7B 30 7D )                // arnings/{0}
    .param [2]/*08000024*/ 
    .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2136
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePointManager::FindServicePoint

  .method /*0600003D*/ public hidebysig static 
          class System.Net.ServicePoint/*02000009*/ 
          FindServicePoint(class [System.Runtime/*23000001*/]System.Uri/*0100001D*/ address) cil managed
  // SIG: 00 01 12 24 12 75
  {
    .custom /*0C00003B:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001C*/::.ctor(string) /* 0A000013 */ = ( 01 00 5D 57 65 62 52 65 71 75 65 73 74 2C 20 48   // ..]WebRequest, H
                                                                                                                                                   74 74 70 57 65 62 52 65 71 75 65 73 74 2C 20 53   // ttpWebRequest, S
                                                                                                                                                   65 72 76 69 63 65 50 6F 69 6E 74 2C 20 61 6E 64   // ervicePoint, and
                                                                                                                                                   20 57 65 62 43 6C 69 65 6E 74 20 61 72 65 20 6F   //  WebClient are o
                                                                                                                                                   62 73 6F 6C 65 74 65 2E 20 55 73 65 20 48 74 74   // bsolete. Use Htt
                                                                                                                                                   70 43 6C 69 65 6E 74 20 69 6E 73 74 65 61 64 2E   // pClient instead.
                                                                                                                                                   02 00 54 0E 0C 44 69 61 67 6E 6F 73 74 69 63 49   // ..T..DiagnosticI
                                                                                                                                                   64 0A 53 59 53 4C 49 42 30 30 31 34 54 0E 09 55   // d.SYSLIB0014T..U
                                                                                                                                                   72 6C 46 6F 72 6D 61 74 22 68 74 74 70 73 3A 2F   // rlFormat"https:/
                                                                                                                                                   2F 61 6B 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77   // /aka.ms/dotnet-w
                                                                                                                                                   61 72 6E 69 6E 67 73 2F 7B 30 7D )                // arnings/{0}
    // Method begins at RVA 0x2139
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePointManager::FindServicePoint

  .method /*0600003E*/ public hidebysig static 
          class System.Net.ServicePoint/*02000009*/ 
          FindServicePoint(class [System.Runtime/*23000001*/]System.Uri/*0100001D*/ address,
                           class [System.Net.Primitives/*23000002*/]System.Net.IWebProxy/*01000023*/ proxy) cil managed
  // SIG: 00 02 12 24 12 75 12 80 8D
  {
    .custom /*0C00003C:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001C*/::.ctor(string) /* 0A000013 */ = ( 01 00 5D 57 65 62 52 65 71 75 65 73 74 2C 20 48   // ..]WebRequest, H
                                                                                                                                                   74 74 70 57 65 62 52 65 71 75 65 73 74 2C 20 53   // ttpWebRequest, S
                                                                                                                                                   65 72 76 69 63 65 50 6F 69 6E 74 2C 20 61 6E 64   // ervicePoint, and
                                                                                                                                                   20 57 65 62 43 6C 69 65 6E 74 20 61 72 65 20 6F   //  WebClient are o
                                                                                                                                                   62 73 6F 6C 65 74 65 2E 20 55 73 65 20 48 74 74   // bsolete. Use Htt
                                                                                                                                                   70 43 6C 69 65 6E 74 20 69 6E 73 74 65 61 64 2E   // pClient instead.
                                                                                                                                                   02 00 54 0E 0C 44 69 61 67 6E 6F 73 74 69 63 49   // ..T..DiagnosticI
                                                                                                                                                   64 0A 53 59 53 4C 49 42 30 30 31 34 54 0E 09 55   // d.SYSLIB0014T..U
                                                                                                                                                   72 6C 46 6F 72 6D 61 74 22 68 74 74 70 73 3A 2F   // rlFormat"https:/
                                                                                                                                                   2F 61 6B 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77   // /aka.ms/dotnet-w
                                                                                                                                                   61 72 6E 69 6E 67 73 2F 7B 30 7D )                // arnings/{0}
    .param [2]/*08000027*/ 
    .custom /*0C000037:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x213c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServicePointManager::FindServicePoint

  .method /*0600003F*/ public hidebysig static 
          void  SetTcpKeepAlive(bool enabled,
                                int32 keepAliveTime,
                                int32 keepAliveInterval) cil managed
  // SIG: 00 03 01 02 08 08
  {
    // Method begins at RVA 0x213f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ServicePointManager::SetTcpKeepAlive

  .property /*17000010*/ callconv(8) bool
          CheckCertificateRevocationList()
  {
    .get bool System.Net.ServicePointManager/*0200000A*/::get_CheckCertificateRevocationList() /* 06000025 */
    .set void System.Net.ServicePointManager/*0200000A*/::set_CheckCertificateRevocationList(bool) /* 06000026 */
  } // end of property ServicePointManager::CheckCertificateRevocationList
  .property /*17000011*/ callconv(8) int32
          DefaultConnectionLimit()
  {
    .get int32 System.Net.ServicePointManager/*0200000A*/::get_DefaultConnectionLimit() /* 06000027 */
    .set void System.Net.ServicePointManager/*0200000A*/::set_DefaultConnectionLimit(int32) /* 06000028 */
  } // end of property ServicePointManager::DefaultConnectionLimit
  .property /*17000012*/ callconv(8) int32
          DnsRefreshTimeout()
  {
    .get int32 System.Net.ServicePointManager/*0200000A*/::get_DnsRefreshTimeout() /* 06000029 */
    .set void System.Net.ServicePointManager/*0200000A*/::set_DnsRefreshTimeout(int32) /* 0600002A */
  } // end of property ServicePointManager::DnsRefreshTimeout
  .property /*17000013*/ callconv(8) bool
          EnableDnsRoundRobin()
  {
    .get bool System.Net.ServicePointManager/*0200000A*/::get_EnableDnsRoundRobin() /* 0600002B */
    .set void System.Net.ServicePointManager/*0200000A*/::set_EnableDnsRoundRobin(bool) /* 0600002C */
  } // end of property ServicePointManager::EnableDnsRoundRobin
  .property /*17000014*/ callconv(8) valuetype [System.Net.Security/*23000004*/]System.Net.Security.EncryptionPolicy/*01000021*/
          EncryptionPolicy()
  {
    .custom /*0C000034:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000024*/::.ctor(string) /* 0A000014 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .get valuetype [System.Net.Security/*23000004*/]System.Net.Security.EncryptionPolicy/*01000021*/ System.Net.ServicePointManager/*0200000A*/::get_EncryptionPolicy() /* 0600002D */
  } // end of property ServicePointManager::EncryptionPolicy
  .property /*17000015*/ callconv(8) bool
          Expect100Continue()
  {
    .get bool System.Net.ServicePointManager/*0200000A*/::get_Expect100Continue() /* 0600002E */
    .set void System.Net.ServicePointManager/*0200000A*/::set_Expect100Continue(bool) /* 0600002F */
  } // end of property ServicePointManager::Expect100Continue
  .property /*17000016*/ callconv(8) int32
          MaxServicePointIdleTime()
  {
    .get int32 System.Net.ServicePointManager/*0200000A*/::get_MaxServicePointIdleTime() /* 06000030 */
    .set void System.Net.ServicePointManager/*0200000A*/::set_MaxServicePointIdleTime(int32) /* 06000031 */
  } // end of property ServicePointManager::MaxServicePointIdleTime
  .property /*17000017*/ callconv(8) int32
          MaxServicePoints()
  {
    .get int32 System.Net.ServicePointManager/*0200000A*/::get_MaxServicePoints() /* 06000032 */
    .set void System.Net.ServicePointManager/*0200000A*/::set_MaxServicePoints(int32) /* 06000033 */
  } // end of property ServicePointManager::MaxServicePoints
  .property /*17000018*/ callconv(8) bool
          ReusePort()
  {
    .get bool System.Net.ServicePointManager/*0200000A*/::get_ReusePort() /* 06000034 */
    .set void System.Net.ServicePointManager/*0200000A*/::set_ReusePort(bool) /* 06000035 */
  } // end of property ServicePointManager::ReusePort
  .property /*17000019*/ callconv(8) valuetype System.Net.SecurityProtocolType/*02000008*/
          SecurityProtocol()
  {
    .get valuetype System.Net.SecurityProtocolType/*02000008*/ System.Net.ServicePointManager/*0200000A*/::get_SecurityProtocol() /* 06000036 */
    .set void System.Net.ServicePointManager/*0200000A*/::set_SecurityProtocol(valuetype System.Net.SecurityProtocolType/*02000008*/) /* 06000037 */
  } // end of property ServicePointManager::SecurityProtocol
  .property /*1700001A*/ callconv(8) class [System.Net.Security/*23000004*/]System.Net.Security.RemoteCertificateValidationCallback/*01000022*/
          ServerCertificateValidationCallback()
  {
    .custom /*0C000035:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get class [System.Net.Security/*23000004*/]System.Net.Security.RemoteCertificateValidationCallback/*01000022*/ System.Net.ServicePointManager/*0200000A*/::get_ServerCertificateValidationCallback() /* 06000038 */
    .set void System.Net.ServicePointManager/*0200000A*/::set_ServerCertificateValidationCallback(class [System.Net.Security/*23000004*/]System.Net.Security.RemoteCertificateValidationCallback/*01000022*/) /* 06000039 */
  } // end of property ServicePointManager::ServerCertificateValidationCallback
  .property /*1700001B*/ callconv(8) bool
          UseNagleAlgorithm()
  {
    .get bool System.Net.ServicePointManager/*0200000A*/::get_UseNagleAlgorithm() /* 0600003A */
    .set void System.Net.ServicePointManager/*0200000A*/::set_UseNagleAlgorithm(bool) /* 0600003B */
  } // end of property ServicePointManager::UseNagleAlgorithm
} // end of class System.Net.ServicePointManager


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
