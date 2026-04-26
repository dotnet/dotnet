


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
// Time-date stamp:            0x96aa53d8
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00002000
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00003e2e
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
// Checksum:                       0x00007fef
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00003ddb [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x000004d4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002a00 [0x00002880] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00003ce0 [0x00000054] address [size] of Debug Directory:           
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
//              0x00001e34 Virtual Size
//              0x00002000 Virtual Address
//              0x00002000 Size of Raw Data
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
//              0x000004d4 Virtual Size
//              0x00004000 Virtual Address
//              0x00000600 Size of Raw Data
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
//              0x00006000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x00002800 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000e30
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00003e1d Import Name Table
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
// 0x00002158 [0x00001b08] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00003c60 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000ce0 Size
//              '#~' Name
//    Stream 2:
//              0x00000d4c Offset
//              0x0000065c Size
//              '#Strings' Name
//    Stream 3:
//              0x000013a8 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x000013ac Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x000013bc Offset
//              0x0000074c Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000040901a01557 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 21120
// PE header size       : 512 (496 used)    ( 2.42%)
// PE additional info   : 11859             (56.15%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.34%)
// CLR meta-data size  : 6920               (32.77%)
// CLR additional info : 128                ( 0.61%)
// CLR method headers  : 62                 ( 0.29%)
// Managed code         : 201               ( 0.95%)
// Data                 : 2048              ( 9.70%)
// Unaccounted          : -682              (-3.23%)

// Num.of PE sections   : 3
//   .text    - 8192
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 6920
//   Module        -    1 (10 bytes)
//   TypeDef       -   10 (140 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   41 (246 bytes)
//   MethodDef     -   62 (868 bytes)     0 abstract, 0 native, 62 bodies
//   FieldDef      -    5 (30 bytes)      0 constant
//   MemberRef     -   20 (120 bytes)
//   ParamDef      -  138 (828 bytes)
//   CustomAttribute-   78 (468 bytes)
//   PropertyMap   -    3 (12 bytes)
//   Property      -    9 (54 bytes)
//   MethodSemantic-   12 (72 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    2 (40 bytes)
//   GenericParam  -   38 (304 bytes)
//   Strings       -  1625 bytes
//   Blobs         -  1868 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   193 bytes

// CLR additional info : 128

// CLR method headers : 62
//   Num.of method bodies  - 62
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 62

// Managed code : 201
//   Ave method size - 3
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class RefSafetyRulesAttribute        (auto) (ansi) (sealed) 
// Class Parallel                       (public) (abstract) (auto) (ansi) (sealed) 
// Class ParallelLoopResult             (public) (sequential) (ansi) (sealed) 
// Class ParallelLoopState              (public) (auto) (ansi) 
// Class ParallelOptions                (public) (auto) (ansi) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000002*/ System.Collections.Concurrent
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly /*20000001*/ noplatform System.Threading.Tasks.Parallel
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
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 1F 53 79 73 74 65 6D 2E 54 68 72 65 61 64   // ...System.Thread
                                                                                                                                                                        69 6E 67 2E 54 61 73 6B 73 2E 50 61 72 61 6C 6C   // ing.Tasks.Parall
                                                                                                                                                                        65 6C 00 00 )                                     // el..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000E:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000F:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 1F 53 79 73 74 65 6D 2E 54 68 72 65 61 64   // ...System.Thread
                                                                                                                                                                       69 6E 67 2E 54 61 73 6B 73 2E 50 61 72 61 6C 6C   // ing.Tasks.Parall
                                                                                                                                                                       65 6C 00 00 )                                     // el..
  .custom /*0C000010:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 37 2E 30 2E 32 32 2E 35 31 38 30 35 00   // ...7.0.22.51805.
                                                                                                                                                                       00 ) 
  .custom /*0C000011:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 37 2E 30 2E 30 2B 64 30 39 39 66 30 37   // ...7.0.0+d099f07
                                                                                                                                                                                35 65 34 35 64 32 61 61 36 30 30 37 61 32 32 62   // 5e45d2aa6007a22b
                                                                                                                                                                                37 31 62 34 35 61 30 38 37 35 38 35 35 39 66 38   // 71b45a08758559f8
                                                                                                                                                                                30 00 00 )                                        // 0..
  .custom /*0C000012:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 1F 53 79 73 74 65 6D 2E 54 68 72 65 61 64   // ...System.Thread
                                                                                                                                                                 69 6E 67 2E 54 61 73 6B 73 2E 50 61 72 61 6C 6C   // ing.Tasks.Parall
                                                                                                                                                                 65 6C 00 00 )                                     // el..
  .custom /*0C000014:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
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
.module System.Threading.Tasks.Parallel.dll
// MVID: {9d183488-2bd8-4d50-8f0e-eee6b05d795b}
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
  .custom /*0C000019:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00001A:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001B:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
    IL_0008:  /* 8D   | (01)000029       */ newarr     [System.Runtime/*23000001*/]System.Byte/*01000029*/
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
  .custom /*0C000021:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000022:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000023:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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

.class /*02000007*/ public abstract auto ansi sealed beforefieldinit System.Threading.Tasks.Parallel
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000029:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000007*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          For(int32 fromInclusive,
              int32 toExclusive,
              class [System.Runtime/*23000001*/]System.Action`2/*01000016*/<int32,class System.Threading.Tasks.ParallelLoopState/*02000009*/> body) cil managed
  // SIG: 00 03 11 20 08 08 15 12 59 02 08 12 24
  {
    // Method begins at RVA 0x20ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::For

  .method /*06000008*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          For(int32 fromInclusive,
              int32 toExclusive,
              class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<int32> body) cil managed
  // SIG: 00 03 11 20 08 08 15 12 5D 01 08
  {
    // Method begins at RVA 0x20af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::For

  .method /*06000009*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          For(int32 fromInclusive,
              int32 toExclusive,
              class System.Threading.Tasks.ParallelOptions/*0200000A*/ parallelOptions,
              class [System.Runtime/*23000001*/]System.Action`2/*01000016*/<int32,class System.Threading.Tasks.ParallelLoopState/*02000009*/> body) cil managed
  // SIG: 00 04 11 20 08 08 12 28 15 12 59 02 08 12 24
  {
    // Method begins at RVA 0x20b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::For

  .method /*0600000A*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          For(int32 fromInclusive,
              int32 toExclusive,
              class System.Threading.Tasks.ParallelOptions/*0200000A*/ parallelOptions,
              class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<int32> body) cil managed
  // SIG: 00 04 11 20 08 08 12 28 15 12 5D 01 08
  {
    // Method begins at RVA 0x20b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::For

  .method /*0600000B*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          For(int64 fromInclusive,
              int64 toExclusive,
              class [System.Runtime/*23000001*/]System.Action`2/*01000016*/<int64,class System.Threading.Tasks.ParallelLoopState/*02000009*/> body) cil managed
  // SIG: 00 03 11 20 0A 0A 15 12 59 02 0A 12 24
  {
    // Method begins at RVA 0x20b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::For

  .method /*0600000C*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          For(int64 fromInclusive,
              int64 toExclusive,
              class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<int64> body) cil managed
  // SIG: 00 03 11 20 0A 0A 15 12 5D 01 0A
  {
    // Method begins at RVA 0x20bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::For

  .method /*0600000D*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          For(int64 fromInclusive,
              int64 toExclusive,
              class System.Threading.Tasks.ParallelOptions/*0200000A*/ parallelOptions,
              class [System.Runtime/*23000001*/]System.Action`2/*01000016*/<int64,class System.Threading.Tasks.ParallelLoopState/*02000009*/> body) cil managed
  // SIG: 00 04 11 20 0A 0A 12 28 15 12 59 02 0A 12 24
  {
    // Method begins at RVA 0x20be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::For

  .method /*0600000E*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          For(int64 fromInclusive,
              int64 toExclusive,
              class System.Threading.Tasks.ParallelOptions/*0200000A*/ parallelOptions,
              class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<int64> body) cil managed
  // SIG: 00 04 11 20 0A 0A 12 28 15 12 5D 01 0A
  {
    // Method begins at RVA 0x20c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::For

  .method /*0600000F*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          ForEach<TSource>(class [System.Collections.Concurrent/*23000002*/]System.Collections.Concurrent.OrderablePartitioner`1/*01000018*/<!!TSource> source,
                           class [System.Runtime/*23000001*/]System.Action`3/*01000019*/<!!TSource,class System.Threading.Tasks.ParallelLoopState/*02000009*/,int64> body) cil managed
  // SIG: 10 01 02 11 20 15 12 61 01 1E 00 15 12 65 03 1E 00 12 24 0A
  {
    .param type TSource /*2A000001*/ 
      .custom /*0C000015:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEach

  .method /*06000010*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          ForEach<TSource>(class [System.Collections.Concurrent/*23000002*/]System.Collections.Concurrent.OrderablePartitioner`1/*01000018*/<!!TSource> source,
                           class System.Threading.Tasks.ParallelOptions/*0200000A*/ parallelOptions,
                           class [System.Runtime/*23000001*/]System.Action`3/*01000019*/<!!TSource,class System.Threading.Tasks.ParallelLoopState/*02000009*/,int64> body) cil managed
  // SIG: 10 01 03 11 20 15 12 61 01 1E 00 12 28 15 12 65 03 1E 00 12 24 0A
  {
    .param type TSource /*2A000002*/ 
      .custom /*0C000018:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEach

  .method /*06000011*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          ForEach<TSource>(class [System.Collections.Concurrent/*23000002*/]System.Collections.Concurrent.Partitioner`1/*0100001A*/<!!TSource> source,
                           class [System.Runtime/*23000001*/]System.Action`2/*01000016*/<!!TSource,class System.Threading.Tasks.ParallelLoopState/*02000009*/> body) cil managed
  // SIG: 10 01 02 11 20 15 12 69 01 1E 00 15 12 59 02 1E 00 12 24
  {
    .param type TSource /*2A000003*/ 
      .custom /*0C00001C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEach

  .method /*06000012*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          ForEach<TSource>(class [System.Collections.Concurrent/*23000002*/]System.Collections.Concurrent.Partitioner`1/*0100001A*/<!!TSource> source,
                           class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<!!TSource> body) cil managed
  // SIG: 10 01 02 11 20 15 12 69 01 1E 00 15 12 5D 01 1E 00
  {
    .param type TSource /*2A000004*/ 
      .custom /*0C000020:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEach

  .method /*06000013*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          ForEach<TSource>(class [System.Collections.Concurrent/*23000002*/]System.Collections.Concurrent.Partitioner`1/*0100001A*/<!!TSource> source,
                           class System.Threading.Tasks.ParallelOptions/*0200000A*/ parallelOptions,
                           class [System.Runtime/*23000001*/]System.Action`2/*01000016*/<!!TSource,class System.Threading.Tasks.ParallelLoopState/*02000009*/> body) cil managed
  // SIG: 10 01 03 11 20 15 12 69 01 1E 00 12 28 15 12 59 02 1E 00 12 24
  {
    .param type TSource /*2A000005*/ 
      .custom /*0C000024:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEach

  .method /*06000014*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          ForEach<TSource>(class [System.Collections.Concurrent/*23000002*/]System.Collections.Concurrent.Partitioner`1/*0100001A*/<!!TSource> source,
                           class System.Threading.Tasks.ParallelOptions/*0200000A*/ parallelOptions,
                           class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<!!TSource> body) cil managed
  // SIG: 10 01 03 11 20 15 12 69 01 1E 00 12 28 15 12 5D 01 1E 00
  {
    .param type TSource /*2A000006*/ 
      .custom /*0C000028:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEach

  .method /*06000015*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          ForEach<TSource>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> source,
                           class [System.Runtime/*23000001*/]System.Action`3/*01000019*/<!!TSource,class System.Threading.Tasks.ParallelLoopState/*02000009*/,int64> body) cil managed
  // SIG: 10 01 02 11 20 15 12 6D 01 1E 00 15 12 65 03 1E 00 12 24 0A
  {
    .param type TSource /*2A000007*/ 
      .custom /*0C00002B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEach

  .method /*06000016*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          ForEach<TSource>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> source,
                           class [System.Runtime/*23000001*/]System.Action`2/*01000016*/<!!TSource,class System.Threading.Tasks.ParallelLoopState/*02000009*/> body) cil managed
  // SIG: 10 01 02 11 20 15 12 6D 01 1E 00 15 12 59 02 1E 00 12 24
  {
    .param type TSource /*2A000008*/ 
      .custom /*0C00002C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEach

  .method /*06000017*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          ForEach<TSource>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> source,
                           class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<!!TSource> body) cil managed
  // SIG: 10 01 02 11 20 15 12 6D 01 1E 00 15 12 5D 01 1E 00
  {
    .param type TSource /*2A000009*/ 
      .custom /*0C00002D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEach

  .method /*06000018*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          ForEach<TSource>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> source,
                           class System.Threading.Tasks.ParallelOptions/*0200000A*/ parallelOptions,
                           class [System.Runtime/*23000001*/]System.Action`3/*01000019*/<!!TSource,class System.Threading.Tasks.ParallelLoopState/*02000009*/,int64> body) cil managed
  // SIG: 10 01 03 11 20 15 12 6D 01 1E 00 12 28 15 12 65 03 1E 00 12 24 0A
  {
    .param type TSource /*2A00000A*/ 
      .custom /*0C000030:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEach

  .method /*06000019*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          ForEach<TSource>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> source,
                           class System.Threading.Tasks.ParallelOptions/*0200000A*/ parallelOptions,
                           class [System.Runtime/*23000001*/]System.Action`2/*01000016*/<!!TSource,class System.Threading.Tasks.ParallelLoopState/*02000009*/> body) cil managed
  // SIG: 10 01 03 11 20 15 12 6D 01 1E 00 12 28 15 12 59 02 1E 00 12 24
  {
    .param type TSource /*2A00000B*/ 
      .custom /*0C000031:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEach

  .method /*0600001A*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          ForEach<TSource>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> source,
                           class System.Threading.Tasks.ParallelOptions/*0200000A*/ parallelOptions,
                           class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<!!TSource> body) cil managed
  // SIG: 10 01 03 11 20 15 12 6D 01 1E 00 12 28 15 12 5D 01 1E 00
  {
    .param type TSource /*2A00000C*/ 
      .custom /*0C000032:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEach

  .method /*0600001B*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          ForEach<TSource,TLocal>(class [System.Collections.Concurrent/*23000002*/]System.Collections.Concurrent.OrderablePartitioner`1/*01000018*/<!!TSource> source,
                                  class [System.Runtime/*23000001*/]System.Func`1/*0100001C*/<!!TLocal> localInit,
                                  class [System.Runtime/*23000001*/]System.Func`5/*0100001D*/<!!TSource,class System.Threading.Tasks.ParallelLoopState/*02000009*/,int64,!!TLocal,!!TLocal> body,
                                  class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<!!TLocal> localFinally) cil managed
  // SIG: 10 02 04 11 20 15 12 61 01 1E 00 15 12 71 01 1E 01 15 12 75 05 1E 00 12 24 0A 1E 01 1E 01 15 12
  //      5D 01 1E 01
  {
    .param type TSource /*2A00000D*/ 
      .custom /*0C000033:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TLocal /*2A00000E*/ 
      .custom /*0C000034:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEach

  .method /*0600001C*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          ForEach<TSource,TLocal>(class [System.Collections.Concurrent/*23000002*/]System.Collections.Concurrent.OrderablePartitioner`1/*01000018*/<!!TSource> source,
                                  class System.Threading.Tasks.ParallelOptions/*0200000A*/ parallelOptions,
                                  class [System.Runtime/*23000001*/]System.Func`1/*0100001C*/<!!TLocal> localInit,
                                  class [System.Runtime/*23000001*/]System.Func`5/*0100001D*/<!!TSource,class System.Threading.Tasks.ParallelLoopState/*02000009*/,int64,!!TLocal,!!TLocal> body,
                                  class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<!!TLocal> localFinally) cil managed
  // SIG: 10 02 05 11 20 15 12 61 01 1E 00 12 28 15 12 71 01 1E 01 15 12 75 05 1E 00 12 24 0A 1E 01 1E 01
  //      15 12 5D 01 1E 01
  {
    .param type TSource /*2A00000F*/ 
      .custom /*0C000035:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TLocal /*2A000010*/ 
      .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEach

  .method /*0600001D*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          ForEach<TSource,TLocal>(class [System.Collections.Concurrent/*23000002*/]System.Collections.Concurrent.Partitioner`1/*0100001A*/<!!TSource> source,
                                  class [System.Runtime/*23000001*/]System.Func`1/*0100001C*/<!!TLocal> localInit,
                                  class [System.Runtime/*23000001*/]System.Func`4/*0100001E*/<!!TSource,class System.Threading.Tasks.ParallelLoopState/*02000009*/,!!TLocal,!!TLocal> body,
                                  class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<!!TLocal> localFinally) cil managed
  // SIG: 10 02 04 11 20 15 12 69 01 1E 00 15 12 71 01 1E 01 15 12 79 04 1E 00 12 24 1E 01 1E 01 15 12 5D
  //      01 1E 01
  {
    .param type TSource /*2A000011*/ 
      .custom /*0C000037:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TLocal /*2A000012*/ 
      .custom /*0C000038:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEach

  .method /*0600001E*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          ForEach<TSource,TLocal>(class [System.Collections.Concurrent/*23000002*/]System.Collections.Concurrent.Partitioner`1/*0100001A*/<!!TSource> source,
                                  class System.Threading.Tasks.ParallelOptions/*0200000A*/ parallelOptions,
                                  class [System.Runtime/*23000001*/]System.Func`1/*0100001C*/<!!TLocal> localInit,
                                  class [System.Runtime/*23000001*/]System.Func`4/*0100001E*/<!!TSource,class System.Threading.Tasks.ParallelLoopState/*02000009*/,!!TLocal,!!TLocal> body,
                                  class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<!!TLocal> localFinally) cil managed
  // SIG: 10 02 05 11 20 15 12 69 01 1E 00 12 28 15 12 71 01 1E 01 15 12 79 04 1E 00 12 24 1E 01 1E 01 15
  //      12 5D 01 1E 01
  {
    .param type TSource /*2A000013*/ 
      .custom /*0C000039:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TLocal /*2A000014*/ 
      .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEach

  .method /*0600001F*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          ForEach<TSource,TLocal>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> source,
                                  class [System.Runtime/*23000001*/]System.Func`1/*0100001C*/<!!TLocal> localInit,
                                  class [System.Runtime/*23000001*/]System.Func`5/*0100001D*/<!!TSource,class System.Threading.Tasks.ParallelLoopState/*02000009*/,int64,!!TLocal,!!TLocal> body,
                                  class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<!!TLocal> localFinally) cil managed
  // SIG: 10 02 04 11 20 15 12 6D 01 1E 00 15 12 71 01 1E 01 15 12 75 05 1E 00 12 24 0A 1E 01 1E 01 15 12
  //      5D 01 1E 01
  {
    .param type TSource /*2A000015*/ 
      .custom /*0C00003B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TLocal /*2A000016*/ 
      .custom /*0C00003C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEach

  .method /*06000020*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          ForEach<TSource,TLocal>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> source,
                                  class [System.Runtime/*23000001*/]System.Func`1/*0100001C*/<!!TLocal> localInit,
                                  class [System.Runtime/*23000001*/]System.Func`4/*0100001E*/<!!TSource,class System.Threading.Tasks.ParallelLoopState/*02000009*/,!!TLocal,!!TLocal> body,
                                  class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<!!TLocal> localFinally) cil managed
  // SIG: 10 02 04 11 20 15 12 6D 01 1E 00 15 12 71 01 1E 01 15 12 79 04 1E 00 12 24 1E 01 1E 01 15 12 5D
  //      01 1E 01
  {
    .param type TSource /*2A000017*/ 
      .custom /*0C00003D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TLocal /*2A000018*/ 
      .custom /*0C00003E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEach

  .method /*06000021*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          ForEach<TSource,TLocal>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> source,
                                  class System.Threading.Tasks.ParallelOptions/*0200000A*/ parallelOptions,
                                  class [System.Runtime/*23000001*/]System.Func`1/*0100001C*/<!!TLocal> localInit,
                                  class [System.Runtime/*23000001*/]System.Func`5/*0100001D*/<!!TSource,class System.Threading.Tasks.ParallelLoopState/*02000009*/,int64,!!TLocal,!!TLocal> body,
                                  class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<!!TLocal> localFinally) cil managed
  // SIG: 10 02 05 11 20 15 12 6D 01 1E 00 12 28 15 12 71 01 1E 01 15 12 75 05 1E 00 12 24 0A 1E 01 1E 01
  //      15 12 5D 01 1E 01
  {
    .param type TSource /*2A000019*/ 
      .custom /*0C00003F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TLocal /*2A00001A*/ 
      .custom /*0C000040:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEach

  .method /*06000022*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          ForEach<TSource,TLocal>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> source,
                                  class System.Threading.Tasks.ParallelOptions/*0200000A*/ parallelOptions,
                                  class [System.Runtime/*23000001*/]System.Func`1/*0100001C*/<!!TLocal> localInit,
                                  class [System.Runtime/*23000001*/]System.Func`4/*0100001E*/<!!TSource,class System.Threading.Tasks.ParallelLoopState/*02000009*/,!!TLocal,!!TLocal> body,
                                  class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<!!TLocal> localFinally) cil managed
  // SIG: 10 02 05 11 20 15 12 6D 01 1E 00 12 28 15 12 71 01 1E 01 15 12 79 04 1E 00 12 24 1E 01 1E 01 15
  //      12 5D 01 1E 01
  {
    .param type TSource /*2A00001B*/ 
      .custom /*0C000041:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TLocal /*2A00001C*/ 
      .custom /*0C000042:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEach

  .method /*06000023*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          ForEachAsync<TSource>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> source,
                                class [System.Runtime/*23000001*/]System.Func`3/*01000020*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000021*/,valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000022*/> body) cil managed
  // SIG: 10 01 02 12 7D 15 12 6D 01 1E 00 15 12 80 81 03 1E 00 11 80 85 11 80 89
  {
    .param type TSource /*2A00001D*/ 
      .custom /*0C000043:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2100
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEachAsync

  .method /*06000024*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          ForEachAsync<TSource>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> source,
                                valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000021*/ cancellationToken,
                                class [System.Runtime/*23000001*/]System.Func`3/*01000020*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000021*/,valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000022*/> body) cil managed
  // SIG: 10 01 03 12 7D 15 12 6D 01 1E 00 11 80 85 15 12 80 81 03 1E 00 11 80 85 11 80 89
  {
    .param type TSource /*2A00001E*/ 
      .custom /*0C000044:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2103
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEachAsync

  .method /*06000025*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          ForEachAsync<TSource>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> source,
                                class System.Threading.Tasks.ParallelOptions/*0200000A*/ parallelOptions,
                                class [System.Runtime/*23000001*/]System.Func`3/*01000020*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000021*/,valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000022*/> body) cil managed
  // SIG: 10 01 03 12 7D 15 12 6D 01 1E 00 12 28 15 12 80 81 03 1E 00 11 80 85 11 80 89
  {
    .param type TSource /*2A00001F*/ 
      .custom /*0C000045:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2106
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEachAsync

  .method /*06000026*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          ForEachAsync<TSource>(class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*01000023*/<!!TSource> source,
                                class [System.Runtime/*23000001*/]System.Func`3/*01000020*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000021*/,valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000022*/> body) cil managed
  // SIG: 10 01 02 12 7D 15 12 80 8D 01 1E 00 15 12 80 81 03 1E 00 11 80 85 11 80 89
  {
    .param type TSource /*2A000020*/ 
      .custom /*0C000046:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2109
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEachAsync

  .method /*06000027*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          ForEachAsync<TSource>(class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*01000023*/<!!TSource> source,
                                valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000021*/ cancellationToken,
                                class [System.Runtime/*23000001*/]System.Func`3/*01000020*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000021*/,valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000022*/> body) cil managed
  // SIG: 10 01 03 12 7D 15 12 80 8D 01 1E 00 11 80 85 15 12 80 81 03 1E 00 11 80 85 11 80 89
  {
    .param type TSource /*2A000021*/ 
      .custom /*0C000047:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x210c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEachAsync

  .method /*06000028*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          ForEachAsync<TSource>(class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*01000023*/<!!TSource> source,
                                class System.Threading.Tasks.ParallelOptions/*0200000A*/ parallelOptions,
                                class [System.Runtime/*23000001*/]System.Func`3/*01000020*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000021*/,valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000022*/> body) cil managed
  // SIG: 10 01 03 12 7D 15 12 80 8D 01 1E 00 12 28 15 12 80 81 03 1E 00 11 80 85 11 80 89
  {
    .param type TSource /*2A000022*/ 
      .custom /*0C000048:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x210f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::ForEachAsync

  .method /*06000029*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          For<TLocal>(int32 fromInclusive,
                      int32 toExclusive,
                      class [System.Runtime/*23000001*/]System.Func`1/*0100001C*/<!!TLocal> localInit,
                      class [System.Runtime/*23000001*/]System.Func`4/*0100001E*/<int32,class System.Threading.Tasks.ParallelLoopState/*02000009*/,!!TLocal,!!TLocal> body,
                      class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<!!TLocal> localFinally) cil managed
  // SIG: 10 01 05 11 20 08 08 15 12 71 01 1E 00 15 12 79 04 08 12 24 1E 00 1E 00 15 12 5D 01 1E 00
  {
    .param type TLocal /*2A000023*/ 
      .custom /*0C000049:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2112
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::For

  .method /*0600002A*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          For<TLocal>(int32 fromInclusive,
                      int32 toExclusive,
                      class System.Threading.Tasks.ParallelOptions/*0200000A*/ parallelOptions,
                      class [System.Runtime/*23000001*/]System.Func`1/*0100001C*/<!!TLocal> localInit,
                      class [System.Runtime/*23000001*/]System.Func`4/*0100001E*/<int32,class System.Threading.Tasks.ParallelLoopState/*02000009*/,!!TLocal,!!TLocal> body,
                      class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<!!TLocal> localFinally) cil managed
  // SIG: 10 01 06 11 20 08 08 12 28 15 12 71 01 1E 00 15 12 79 04 08 12 24 1E 00 1E 00 15 12 5D 01 1E 00
  {
    .param type TLocal /*2A000024*/ 
      .custom /*0C00004A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2115
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::For

  .method /*0600002B*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          For<TLocal>(int64 fromInclusive,
                      int64 toExclusive,
                      class [System.Runtime/*23000001*/]System.Func`1/*0100001C*/<!!TLocal> localInit,
                      class [System.Runtime/*23000001*/]System.Func`4/*0100001E*/<int64,class System.Threading.Tasks.ParallelLoopState/*02000009*/,!!TLocal,!!TLocal> body,
                      class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<!!TLocal> localFinally) cil managed
  // SIG: 10 01 05 11 20 0A 0A 15 12 71 01 1E 00 15 12 79 04 0A 12 24 1E 00 1E 00 15 12 5D 01 1E 00
  {
    .param type TLocal /*2A000025*/ 
      .custom /*0C00004B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2118
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::For

  .method /*0600002C*/ public hidebysig static 
          valuetype System.Threading.Tasks.ParallelLoopResult/*02000008*/ 
          For<TLocal>(int64 fromInclusive,
                      int64 toExclusive,
                      class System.Threading.Tasks.ParallelOptions/*0200000A*/ parallelOptions,
                      class [System.Runtime/*23000001*/]System.Func`1/*0100001C*/<!!TLocal> localInit,
                      class [System.Runtime/*23000001*/]System.Func`4/*0100001E*/<int64,class System.Threading.Tasks.ParallelLoopState/*02000009*/,!!TLocal,!!TLocal> body,
                      class [System.Runtime/*23000001*/]System.Action`1/*01000017*/<!!TLocal> localFinally) cil managed
  // SIG: 10 01 06 11 20 0A 0A 12 28 15 12 71 01 1E 00 15 12 79 04 0A 12 24 1E 00 1E 00 15 12 5D 01 1E 00
  {
    .param type TLocal /*2A000026*/ 
      .custom /*0C00004C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Parallel::For

  .method /*0600002D*/ public hidebysig static 
          void  Invoke(class [System.Runtime/*23000001*/]System.Action/*01000025*/[] actions) cil managed
  // SIG: 00 01 01 1D 12 80 95
  {
    .param [1]/*08000085*/ 
    .custom /*0C00004D:0A000012*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000024*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x211e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Parallel::Invoke

  .method /*0600002E*/ public hidebysig static 
          void  Invoke(class System.Threading.Tasks.ParallelOptions/*0200000A*/ parallelOptions,
                       class [System.Runtime/*23000001*/]System.Action/*01000025*/[] actions) cil managed
  // SIG: 00 02 01 12 28 1D 12 80 95
  {
    .param [2]/*08000087*/ 
    .custom /*0C00004E:0A000012*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000024*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2120
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Parallel::Invoke

} // end of class System.Threading.Tasks.Parallel

.class /*02000008*/ public sequential ansi sealed beforefieldinit System.Threading.Tasks.ParallelLoopResult
       extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
{
  .field /*04000005*/ private int32 _dummyPrimitive
  .method /*0600002F*/ public hidebysig specialname 
          instance bool  get_IsCompleted() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2122
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelLoopResult::get_IsCompleted

  .method /*06000030*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000027*/<int64> 
          get_LowestBreakIteration() cil managed
  // SIG: 20 00 15 11 80 9D 01 0A
  {
    // Method begins at RVA 0x2125
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelLoopResult::get_LowestBreakIteration

  .property /*17000001*/ instance callconv(8) bool
          IsCompleted()
  {
    .get instance bool System.Threading.Tasks.ParallelLoopResult/*02000008*/::get_IsCompleted() /* 0600002F */
  } // end of property ParallelLoopResult::IsCompleted
  .property /*17000002*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000027*/<int64>
          LowestBreakIteration()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000027*/<int64> System.Threading.Tasks.ParallelLoopResult/*02000008*/::get_LowestBreakIteration() /* 06000030 */
  } // end of property ParallelLoopResult::LowestBreakIteration
} // end of class System.Threading.Tasks.ParallelLoopResult

.class /*02000009*/ public auto ansi beforefieldinit System.Threading.Tasks.ParallelLoopState
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .method /*06000031*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2128
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ParallelLoopState::.ctor

  .method /*06000032*/ public hidebysig specialname 
          instance bool  get_IsExceptional() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2130
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelLoopState::get_IsExceptional

  .method /*06000033*/ public hidebysig specialname 
          instance bool  get_IsStopped() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2133
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelLoopState::get_IsStopped

  .method /*06000034*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000027*/<int64> 
          get_LowestBreakIteration() cil managed
  // SIG: 20 00 15 11 80 9D 01 0A
  {
    // Method begins at RVA 0x2136
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelLoopState::get_LowestBreakIteration

  .method /*06000035*/ public hidebysig specialname 
          instance bool  get_ShouldExitCurrentIteration() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2139
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelLoopState::get_ShouldExitCurrentIteration

  .method /*06000036*/ public hidebysig instance void 
          Break() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x213c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ParallelLoopState::Break

  .method /*06000037*/ public hidebysig instance void 
          Stop() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x213e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ParallelLoopState::Stop

  .property /*17000003*/ instance callconv(8) bool
          IsExceptional()
  {
    .get instance bool System.Threading.Tasks.ParallelLoopState/*02000009*/::get_IsExceptional() /* 06000032 */
  } // end of property ParallelLoopState::IsExceptional
  .property /*17000004*/ instance callconv(8) bool
          IsStopped()
  {
    .get instance bool System.Threading.Tasks.ParallelLoopState/*02000009*/::get_IsStopped() /* 06000033 */
  } // end of property ParallelLoopState::IsStopped
  .property /*17000005*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000027*/<int64>
          LowestBreakIteration()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000027*/<int64> System.Threading.Tasks.ParallelLoopState/*02000009*/::get_LowestBreakIteration() /* 06000034 */
  } // end of property ParallelLoopState::LowestBreakIteration
  .property /*17000006*/ instance callconv(8) bool
          ShouldExitCurrentIteration()
  {
    .get instance bool System.Threading.Tasks.ParallelLoopState/*02000009*/::get_ShouldExitCurrentIteration() /* 06000035 */
  } // end of property ParallelLoopState::ShouldExitCurrentIteration
} // end of class System.Threading.Tasks.ParallelLoopState

.class /*0200000A*/ public auto ansi beforefieldinit System.Threading.Tasks.ParallelOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C00002E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00002F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000038*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2140
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000014 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ParallelOptions::.ctor

  .method /*06000039*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000021*/ 
          get_CancellationToken() cil managed
  // SIG: 20 00 11 80 85
  {
    // Method begins at RVA 0x2148
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelOptions::get_CancellationToken

  .method /*0600003A*/ public hidebysig specialname 
          instance void  set_CancellationToken(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000021*/ 'value') cil managed
  // SIG: 20 01 01 11 80 85
  {
    // Method begins at RVA 0x214b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ParallelOptions::set_CancellationToken

  .method /*0600003B*/ public hidebysig specialname 
          instance int32  get_MaxDegreeOfParallelism() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x214d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelOptions::get_MaxDegreeOfParallelism

  .method /*0600003C*/ public hidebysig specialname 
          instance void  set_MaxDegreeOfParallelism(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2150
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ParallelOptions::set_MaxDegreeOfParallelism

  .method /*0600003D*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.TaskScheduler/*01000028*/ 
          get_TaskScheduler() cil managed
  // SIG: 20 00 12 80 A1
  {
    // Method begins at RVA 0x2152
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelOptions::get_TaskScheduler

  .method /*0600003E*/ public hidebysig specialname 
          instance void  set_TaskScheduler(class [System.Runtime/*23000001*/]System.Threading.Tasks.TaskScheduler/*01000028*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A1
  {
    // Method begins at RVA 0x2155
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ParallelOptions::set_TaskScheduler

  .property /*17000007*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000021*/
          CancellationToken()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000021*/ System.Threading.Tasks.ParallelOptions/*0200000A*/::get_CancellationToken() /* 06000039 */
    .set instance void System.Threading.Tasks.ParallelOptions/*0200000A*/::set_CancellationToken(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000021*/) /* 0600003A */
  } // end of property ParallelOptions::CancellationToken
  .property /*17000008*/ instance callconv(8) int32
          MaxDegreeOfParallelism()
  {
    .get instance int32 System.Threading.Tasks.ParallelOptions/*0200000A*/::get_MaxDegreeOfParallelism() /* 0600003B */
    .set instance void System.Threading.Tasks.ParallelOptions/*0200000A*/::set_MaxDegreeOfParallelism(int32) /* 0600003C */
  } // end of property ParallelOptions::MaxDegreeOfParallelism
  .property /*17000009*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.Tasks.TaskScheduler/*01000028*/
          TaskScheduler()
  {
    .get instance class [System.Runtime/*23000001*/]System.Threading.Tasks.TaskScheduler/*01000028*/ System.Threading.Tasks.ParallelOptions/*0200000A*/::get_TaskScheduler() /* 0600003D */
    .set instance void System.Threading.Tasks.ParallelOptions/*0200000A*/::set_TaskScheduler(class [System.Runtime/*23000001*/]System.Threading.Tasks.TaskScheduler/*01000028*/) /* 0600003E */
  } // end of property ParallelOptions::TaskScheduler
} // end of class System.Threading.Tasks.ParallelOptions


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
