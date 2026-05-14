


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
// Time-date stamp:            0xd9787068
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00004c00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00006ada
// Base of code:                   0x00002000
// Base of data:                   0x00008000
// Image base:                     0x10000000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0004
// Minor subsystem version:        0x0000
// Size of image:                  0x0000c000
// Size of headers:                0x00000200
// Checksum:                       0x0000da33
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00006a87 [0x0000004f] address [size] of Import Directory:          
// 0x00008000 [0x00000484] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00005600 [0x00002870] address [size] of Security Directory:        
// 0x0000a000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x000069a0 [0x00000054] address [size] of Debug Directory:           
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
//              0x00004ae0 Virtual Size
//              0x00002000 Virtual Address
//              0x00004c00 Size of Raw Data
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
//              0x00008000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00004e00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x0000a000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x00005400 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00006000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000adc
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00006ac9 Import Name Table
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
// 0x000022cc [0x00004654] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00006920 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00002a0c Size
//              '#~' Name
//    Stream 2:
//              0x00002a78 Offset
//              0x00000a4c Size
//              '#Strings' Name
//    Stream 3:
//              0x000034c4 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x000034c8 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x000034d8 Offset
//              0x0000117c Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000004090ba01757 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 32368
// PE header size       : 512 (496 used)    ( 1.58%)
// PE additional info   : 11763             (36.34%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.22%)
// CLR meta-data size  : 18004              (55.62%)
// CLR additional info : 128                ( 0.40%)
// CLR method headers  : 178                ( 0.55%)
// Managed code         : 455               ( 1.41%)
// Data                 : 2048              ( 6.33%)
// Unaccounted          : -792              (-2.45%)

// Num.of PE sections   : 3
//   .text    - 19456
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 18004
//   Module        -    1 (10 bytes)
//   TypeDef       -   11 (154 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   49 (294 bytes)
//   MethodDef     -  178 (2492 bytes)    0 abstract, 0 native, 178 bodies
//   FieldDef      -    4 (24 bytes)      0 constant
//   MemberRef     -   32 (192 bytes)
//   ParamDef      -  344 (2064 bytes)
//   MethodImpl    -    9 (54 bytes)
//   CustomAttribute-  597 (3582 bytes)
//   InterfaceImpl -    7 (28 bytes)
//   PropertyMap   -    1 (4 bytes)
//   Property      -    3 (18 bytes)
//   MethodSemantic-    3 (18 bytes)
//   TypeSpec      -    3 (6 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    3 (60 bytes)
//   GenericParam  -  206 (1648 bytes)
//   Strings       -  2634 bytes
//   Blobs         -  4476 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   204 bytes

// CLR additional info : 128

// CLR method headers : 178
//   Num.of method bodies  - 178
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 178

// Managed code : 455
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class RefSafetyRulesAttribute        (auto) (ansi) (sealed) 
// Class EnumerableExecutor             (public) (abstract) (auto) (ansi) 
// Class EnumerableExecutor`1           (public) (auto) (ansi) 
// Class EnumerableQuery                (public) (abstract) (auto) (ansi) 
// Class EnumerableQuery`1              (public) (auto) (ansi) 
// Class Queryable                      (public) (abstract) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000002*/ System.Linq.Expressions
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000003*/ System.Linq
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly /*20000001*/ noplatform System.Linq.Queryable
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

  .custom /*0C000009:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000006*/::.ctor(string) /* 0A000005 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 37 2E 30 01 00 54 0E 14   // ersion=v7.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 37 2E 30 )          // Name..NET 7.0
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000007*/::.ctor() /* 0A000006 */ = ( 01 00 00 00 ) 
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000E:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 15 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 51   // ...System.Linq.Q
                                                                                                                                                                        75 65 72 79 61 62 6C 65 00 00 )                   // ueryable..
  .custom /*0C00000F:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000010:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000011:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000012:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 15 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 51   // ...System.Linq.Q
                                                                                                                                                                       75 65 72 79 61 62 6C 65 00 00 )                   // ueryable..
  .custom /*0C000013:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 37 2E 30 2E 32 32 2E 35 31 38 30 35 00   // ...7.0.22.51805.
                                                                                                                                                                       00 ) 
  .custom /*0C000014:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 37 2E 30 2E 30 2B 64 30 39 39 66 30 37   // ...7.0.0+d099f07
                                                                                                                                                                                35 65 34 35 64 32 61 61 36 30 30 37 61 32 32 62   // 5e45d2aa6007a22b
                                                                                                                                                                                37 31 62 34 35 61 30 38 37 35 38 35 35 39 66 38   // 71b45a08758559f8
                                                                                                                                                                                30 00 00 )                                        // 0..
  .custom /*0C000015:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000016:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 15 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 51   // ...System.Linq.Q
                                                                                                                                                                 75 65 72 79 61 62 6C 65 00 00 )                   // ueryable..
  .custom /*0C000017:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
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
.module System.Linq.Queryable.dll
// MVID: {ac27a432-ead1-4ad2-9b4c-fafbf548bb5e}
.custom /*0C000002:06000006*/ instance void System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::.ctor(int32) /* 06000006 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000003:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000019:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C00001A:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C00001D:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C00001E:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A00001F */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)000031       */ newarr     [System.Runtime/*23000001*/]System.Byte/*01000031*/
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
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A00001F */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000021:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000022:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000023:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A00001F */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000024:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000025:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000026:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A00001F */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.RefSafetyRulesAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000029:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C00002A:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00002B:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A00001F */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000004       */ stfld      int32 System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::Version /* 04000004 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method RefSafetyRulesAttribute::.ctor

} // end of class System.Runtime.CompilerServices.RefSafetyRulesAttribute

.class /*02000007*/ public abstract auto ansi beforefieldinit System.Linq.EnumerableExecutor
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .method /*06000007*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ac
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EnumerableExecutor::.ctor

} // end of class System.Linq.EnumerableExecutor

.class /*02000008*/ public auto ansi beforefieldinit System.Linq.EnumerableExecutor`1<T>
       extends System.Linq.EnumerableExecutor/*02000007*/
{
  .param type T /*2A000001*/ 
    .custom /*0C000018:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*06000008*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression/*01000017*/ expression) cil managed
  // SIG: 20 01 01 12 5D
  {
    .custom /*0C00002F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20b4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000007       */ call       instance void System.Linq.EnumerableExecutor/*02000007*/::.ctor() /* 06000007 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EnumerableExecutor`1::.ctor

} // end of class System.Linq.EnumerableExecutor`1

.class /*02000009*/ public abstract auto ansi beforefieldinit System.Linq.EnumerableQuery
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .method /*06000009*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20bc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EnumerableQuery::.ctor

} // end of class System.Linq.EnumerableQuery

.class /*0200000A*/ public auto ansi beforefieldinit System.Linq.EnumerableQuery`1<T>
       extends System.Linq.EnumerableQuery/*02000009*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!T>/*1B000001*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*0100001F*/,
                  [System.Linq.Expressions/*23000002*/]System.Linq.IOrderedQueryable/*01000022*/,
                  [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable/*0100001A*/,
                  class [System.Linq.Expressions/*23000002*/]System.Linq.IOrderedQueryable`1/*01000023*/<!T>/*1B000002*/,
                  class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!T>/*1B000003*/,
                  [System.Linq.Expressions/*23000002*/]System.Linq.IQueryProvider/*0100001C*/
{
  .custom /*0C000033:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000013 */ = ( 01 00 81 1B 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                          69 6E 2D 6D 65 6D 6F 72 79 20 63 6F 6C 6C 65 63   // in-memory collec
                                                                                                                                                                                          74 69 6F 6E 73 20 61 73 20 49 51 75 65 72 79 61   // tions as IQuerya
                                                                                                                                                                                          62 6C 65 20 63 61 6E 20 72 65 71 75 69 72 65 20   // ble can require 
                                                                                                                                                                                          75 6E 72 65 66 65 72 65 6E 63 65 64 20 63 6F 64   // unreferenced cod
                                                                                                                                                                                          65 20 62 65 63 61 75 73 65 20 65 78 70 72 65 73   // e because expres
                                                                                                                                                                                          73 69 6F 6E 73 20 72 65 66 65 72 65 6E 63 69 6E   // sions referencin
                                                                                                                                                                                          67 20 49 51 75 65 72 79 61 62 6C 65 20 65 78 74   // g IQueryable ext
                                                                                                                                                                                          65 6E 73 69 6F 6E 20 6D 65 74 68 6F 64 73 20 63   // ension methods c
                                                                                                                                                                                          61 6E 20 67 65 74 20 72 65 62 6F 75 6E 64 20 74   // an get rebound t
                                                                                                                                                                                          6F 20 49 45 6E 75 6D 65 72 61 62 6C 65 20 65 78   // o IEnumerable ex
                                                                                                                                                                                          74 65 6E 73 69 6F 6E 20 6D 65 74 68 6F 64 73 2E   // tension methods.
                                                                                                                                                                                          20 54 68 65 20 49 45 6E 75 6D 65 72 61 62 6C 65   //  The IEnumerable
                                                                                                                                                                                          20 65 78 74 65 6E 73 69 6F 6E 20 6D 65 74 68 6F   //  extension metho
                                                                                                                                                                                          64 73 20 63 6F 75 6C 64 20 62 65 20 74 72 69 6D   // ds could be trim
                                                                                                                                                                                          6D 65 64 20 63 61 75 73 69 6E 67 20 74 68 65 20   // med causing the 
                                                                                                                                                                                          61 70 70 6C 69 63 61 74 69 6F 6E 20 74 6F 20 66   // application to f
                                                                                                                                                                                          61 69 6C 20 61 74 20 72 75 6E 74 69 6D 65 2E 00   // ail at runtime..
                                                                                                                                                                                          00 ) 
  .custom /*0C000034:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                     63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                     51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                     71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                     65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                     20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                     63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                     74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                     74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                     6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                     20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!T>/*1B000001*/
  .custom /*0C000001:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class [System.Linq.Expressions/*23000002*/]System.Linq.IOrderedQueryable`1/*01000023*/<!T>/*1B000002*/
  .custom /*0C000027:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!T>/*1B000003*/
  .custom /*0C00002C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type T /*2A000002*/ 
    .custom /*0C00001C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*0600000A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!T> enumerable) cil managed
  // SIG: 20 01 01 15 12 75 01 13 00
  {
    .custom /*0C000032:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20c4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000009       */ call       instance void System.Linq.EnumerableQuery/*02000009*/::.ctor() /* 06000009 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EnumerableQuery`1::.ctor

  .method /*0600000B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression/*01000017*/ expression) cil managed
  // SIG: 20 01 01 12 5D
  {
    .custom /*0C000036:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20cc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000009       */ call       instance void System.Linq.EnumerableQuery/*02000009*/::.ctor() /* 06000009 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EnumerableQuery`1::.ctor

  .method /*0600000C*/ private hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001B*/ 
          System.Linq.IQueryable.get_ElementType() cil managed
  // SIG: 20 00 12 6D
  {
    .override [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable/*0100001A*/::get_ElementType /*0100001A::0A000015*/ 
    // Method begins at RVA 0x20d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumerableQuery`1::System.Linq.IQueryable.get_ElementType

  .method /*0600000D*/ private hidebysig newslot specialname virtual final 
          instance class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression/*01000017*/ 
          System.Linq.IQueryable.get_Expression() cil managed
  // SIG: 20 00 12 5D
  {
    .override [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable/*0100001A*/::get_Expression /*0100001A::0A000016*/ 
    // Method begins at RVA 0x20d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumerableQuery`1::System.Linq.IQueryable.get_Expression

  .method /*0600000E*/ private hidebysig newslot specialname virtual final 
          instance class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryProvider/*0100001C*/ 
          System.Linq.IQueryable.get_Provider() cil managed
  // SIG: 20 00 12 71
  {
    .override [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable/*0100001A*/::get_Provider /*0100001A::0A000017*/ 
    // Method begins at RVA 0x20da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumerableQuery`1::System.Linq.IQueryable.get_Provider

  .method /*0600000F*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*0100001E*/<!T> 
          'System.Collections.Generic.IEnumerable<T>.GetEnumerator'() cil managed
  // SIG: 20 00 15 12 79 01 13 00
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*0100001E*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!T>/*1B000001*/::GetEnumerator() /* 0A000018 */
    // Method begins at RVA 0x20dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumerableQuery`1::'System.Collections.Generic.IEnumerable<T>.GetEnumerator'

  .method /*06000010*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000020*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 80 81
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*0100001F*/::GetEnumerator /*0100001F::0A000019*/ 
    // Method begins at RVA 0x20e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumerableQuery`1::System.Collections.IEnumerable.GetEnumerator

  .method /*06000011*/ private hidebysig newslot virtual final 
          instance class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable/*0100001A*/ 
          System.Linq.IQueryProvider.CreateQuery(class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression/*01000017*/ expression) cil managed
  // SIG: 20 01 12 69 12 5D
  {
    .override [System.Linq.Expressions/*23000002*/]System.Linq.IQueryProvider/*0100001C*/::CreateQuery /*0100001C::0A00001A*/ 
    // Method begins at RVA 0x20e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumerableQuery`1::System.Linq.IQueryProvider.CreateQuery

  .method /*06000012*/ private hidebysig newslot virtual final 
          instance class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TElement> 
          System.Linq.IQueryProvider.CreateQuery<TElement>(class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression/*01000017*/ expression) cil managed
  // SIG: 30 01 01 15 12 80 85 01 1E 00 12 5D
  {
    .override [System.Linq.Expressions/*23000002*/]System.Linq.IQueryProvider/*0100001C*/::CreateQuery /*0100001C::0A00001B*/ 
    // Method begins at RVA 0x20e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumerableQuery`1::System.Linq.IQueryProvider.CreateQuery

  .method /*06000013*/ private hidebysig newslot virtual final 
          instance object  System.Linq.IQueryProvider.Execute(class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression/*01000017*/ expression) cil managed
  // SIG: 20 01 1C 12 5D
  {
    .override [System.Linq.Expressions/*23000002*/]System.Linq.IQueryProvider/*0100001C*/::Execute /*0100001C::0A00001C*/ 
    // Method begins at RVA 0x20e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumerableQuery`1::System.Linq.IQueryProvider.Execute

  .method /*06000014*/ private hidebysig newslot virtual final 
          instance !!TElement  System.Linq.IQueryProvider.Execute<TElement>(class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression/*01000017*/ expression) cil managed
  // SIG: 30 01 01 1E 00 12 5D
  {
    .override [System.Linq.Expressions/*23000002*/]System.Linq.IQueryProvider/*0100001C*/::Execute /*0100001C::0A00001D*/ 
    // Method begins at RVA 0x20ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumerableQuery`1::System.Linq.IQueryProvider.Execute

  .method /*06000015*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000044:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumerableQuery`1::ToString

  .property /*17000001*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001B*/
          System.Linq.IQueryable.ElementType()
  {
    .custom /*0C000004:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001B*/ System.Linq.EnumerableQuery`1/*0200000A*/::System.Linq.IQueryable.get_ElementType() /* 0600000C */
  } // end of property EnumerableQuery`1::System.Linq.IQueryable.ElementType
  .property /*17000002*/ instance callconv(8) class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression/*01000017*/
          System.Linq.IQueryable.Expression()
  {
    .custom /*0C00001B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression/*01000017*/ System.Linq.EnumerableQuery`1/*0200000A*/::System.Linq.IQueryable.get_Expression() /* 0600000D */
  } // end of property EnumerableQuery`1::System.Linq.IQueryable.Expression
  .property /*17000003*/ instance callconv(8) class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryProvider/*0100001C*/
          System.Linq.IQueryable.Provider()
  {
    .custom /*0C000020:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryProvider/*0100001C*/ System.Linq.EnumerableQuery`1/*0200000A*/::System.Linq.IQueryable.get_Provider() /* 0600000E */
  } // end of property EnumerableQuery`1::System.Linq.IQueryable.Provider
} // end of class System.Linq.EnumerableQuery`1

.class /*0200000B*/ public abstract auto ansi sealed beforefieldinit System.Linq.Queryable
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000037:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000038:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000039:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000016*/ public hidebysig static 
          !!TSource  Aggregate<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                        class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`3/*01000025*/<!!TSource,!!TSource,!!TSource>> func) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 95 03 1E 00 1E 00 1E 00
  {
    .custom /*0C000046:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000047:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000005*/ 
      .custom /*0C000028:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Aggregate

  .method /*06000017*/ public hidebysig static 
          !!TAccumulate  Aggregate<TSource,TAccumulate>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                                        !!TAccumulate seed,
                                                        class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`3/*01000025*/<!!TAccumulate,!!TSource,!!TAccumulate>> func) cil managed
  // SIG: 10 02 03 1E 01 15 12 80 85 01 1E 00 1E 01 15 12 80 91 01 15 12 80 95 03 1E 01 1E 00 1E 01
  {
    .custom /*0C000049:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00004A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000006*/ 
      .custom /*0C00002D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TAccumulate /*2A000007*/ 
      .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Aggregate

  .method /*06000018*/ public hidebysig static 
          !!TResult  Aggregate<TSource,TAccumulate,TResult>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                                            !!TAccumulate seed,
                                                            class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`3/*01000025*/<!!TAccumulate,!!TSource,!!TAccumulate>> func,
                                                            class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TAccumulate,!!TResult>> selector) cil managed
  // SIG: 10 03 04 1E 02 15 12 80 85 01 1E 00 1E 01 15 12 80 91 01 15 12 80 95 03 1E 01 1E 00 1E 01 15 12
  //      80 91 01 15 12 80 99 02 1E 01 1E 02
  {
    .custom /*0C00004C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00004D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000008*/ 
      .custom /*0C000030:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TAccumulate /*2A000009*/ 
      .custom /*0C000031:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A00000A*/ 
      .custom /*0C000035:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Aggregate

  .method /*06000019*/ public hidebysig static 
          bool  All<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                             class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,bool>> predicate) cil managed
  // SIG: 10 01 02 02 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 02
  {
    .custom /*0C00004F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000050:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00000B*/ 
      .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::All

  .method /*0600001A*/ public hidebysig static 
          bool  Any<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source) cil managed
  // SIG: 10 01 01 02 15 12 80 85 01 1E 00
  {
    .custom /*0C000052:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000053:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00000C*/ 
      .custom /*0C00003B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Any

  .method /*0600001B*/ public hidebysig static 
          bool  Any<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                             class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,bool>> predicate) cil managed
  // SIG: 10 01 02 02 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 02
  {
    .custom /*0C000055:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000056:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00000D*/ 
      .custom /*0C00003C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2101
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Any

  .method /*0600001C*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          Append<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                          !!TSource element) cil managed
  // SIG: 10 01 02 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 1E 00
  {
    .custom /*0C000058:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000059:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00000E*/ 
      .custom /*0C00003D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2104
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Append

  .method /*0600001D*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable/*0100001A*/ 
          AsQueryable(class [System.Runtime/*23000001*/]System.Collections.IEnumerable/*0100001F*/ source) cil managed
  // SIG: 00 01 12 69 12 7D
  {
    .custom /*0C00005B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00005C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000013 */ = ( 01 00 81 1B 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                            69 6E 2D 6D 65 6D 6F 72 79 20 63 6F 6C 6C 65 63   // in-memory collec
                                                                                                                                                                                            74 69 6F 6E 73 20 61 73 20 49 51 75 65 72 79 61   // tions as IQuerya
                                                                                                                                                                                            62 6C 65 20 63 61 6E 20 72 65 71 75 69 72 65 20   // ble can require 
                                                                                                                                                                                            75 6E 72 65 66 65 72 65 6E 63 65 64 20 63 6F 64   // unreferenced cod
                                                                                                                                                                                            65 20 62 65 63 61 75 73 65 20 65 78 70 72 65 73   // e because expres
                                                                                                                                                                                            73 69 6F 6E 73 20 72 65 66 65 72 65 6E 63 69 6E   // sions referencin
                                                                                                                                                                                            67 20 49 51 75 65 72 79 61 62 6C 65 20 65 78 74   // g IQueryable ext
                                                                                                                                                                                            65 6E 73 69 6F 6E 20 6D 65 74 68 6F 64 73 20 63   // ension methods c
                                                                                                                                                                                            61 6E 20 67 65 74 20 72 65 62 6F 75 6E 64 20 74   // an get rebound t
                                                                                                                                                                                            6F 20 49 45 6E 75 6D 65 72 61 62 6C 65 20 65 78   // o IEnumerable ex
                                                                                                                                                                                            74 65 6E 73 69 6F 6E 20 6D 65 74 68 6F 64 73 2E   // tension methods.
                                                                                                                                                                                            20 54 68 65 20 49 45 6E 75 6D 65 72 61 62 6C 65   //  The IEnumerable
                                                                                                                                                                                            20 65 78 74 65 6E 73 69 6F 6E 20 6D 65 74 68 6F   //  extension metho
                                                                                                                                                                                            64 73 20 63 6F 75 6C 64 20 62 65 20 74 72 69 6D   // ds could be trim
                                                                                                                                                                                            6D 65 64 20 63 61 75 73 69 6E 67 20 74 68 65 20   // med causing the 
                                                                                                                                                                                            61 70 70 6C 69 63 61 74 69 6F 6E 20 74 6F 20 66   // application to f
                                                                                                                                                                                            61 69 6C 20 61 74 20 72 75 6E 74 69 6D 65 2E 00   // ail at runtime..
                                                                                                                                                                                            00 ) 
    .custom /*0C00005D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    // Method begins at RVA 0x2107
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::AsQueryable

  .method /*0600001E*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TElement> 
          AsQueryable<TElement>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TElement> source) cil managed
  // SIG: 10 01 01 15 12 80 85 01 1E 00 15 12 75 01 1E 00
  {
    .custom /*0C00005F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000060:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000018*/::.ctor(string) /* 0A000013 */ = ( 01 00 81 1B 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                            69 6E 2D 6D 65 6D 6F 72 79 20 63 6F 6C 6C 65 63   // in-memory collec
                                                                                                                                                                                            74 69 6F 6E 73 20 61 73 20 49 51 75 65 72 79 61   // tions as IQuerya
                                                                                                                                                                                            62 6C 65 20 63 61 6E 20 72 65 71 75 69 72 65 20   // ble can require 
                                                                                                                                                                                            75 6E 72 65 66 65 72 65 6E 63 65 64 20 63 6F 64   // unreferenced cod
                                                                                                                                                                                            65 20 62 65 63 61 75 73 65 20 65 78 70 72 65 73   // e because expres
                                                                                                                                                                                            73 69 6F 6E 73 20 72 65 66 65 72 65 6E 63 69 6E   // sions referencin
                                                                                                                                                                                            67 20 49 51 75 65 72 79 61 62 6C 65 20 65 78 74   // g IQueryable ext
                                                                                                                                                                                            65 6E 73 69 6F 6E 20 6D 65 74 68 6F 64 73 20 63   // ension methods c
                                                                                                                                                                                            61 6E 20 67 65 74 20 72 65 62 6F 75 6E 64 20 74   // an get rebound t
                                                                                                                                                                                            6F 20 49 45 6E 75 6D 65 72 61 62 6C 65 20 65 78   // o IEnumerable ex
                                                                                                                                                                                            74 65 6E 73 69 6F 6E 20 6D 65 74 68 6F 64 73 2E   // tension methods.
                                                                                                                                                                                            20 54 68 65 20 49 45 6E 75 6D 65 72 61 62 6C 65   //  The IEnumerable
                                                                                                                                                                                            20 65 78 74 65 6E 73 69 6F 6E 20 6D 65 74 68 6F   //  extension metho
                                                                                                                                                                                            64 73 20 63 6F 75 6C 64 20 62 65 20 74 72 69 6D   // ds could be trim
                                                                                                                                                                                            6D 65 64 20 63 61 75 73 69 6E 67 20 74 68 65 20   // med causing the 
                                                                                                                                                                                            61 70 70 6C 69 63 61 74 69 6F 6E 20 74 6F 20 66   // application to f
                                                                                                                                                                                            61 69 6C 20 61 74 20 72 75 6E 74 69 6D 65 2E 00   // ail at runtime..
                                                                                                                                                                                            00 ) 
    .custom /*0C000061:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TElement /*2A00000F*/ 
      .custom /*0C00003E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x210a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::AsQueryable

  .method /*0600001F*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Decimal/*01000027*/ 
          Average(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*01000027*/> source) cil managed
  // SIG: 00 01 11 80 9D 15 12 80 85 01 11 80 9D
  {
    .custom /*0C000063:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x210d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Average

  .method /*06000020*/ public hidebysig static 
          float64  Average(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<float64> source) cil managed
  // SIG: 00 01 0D 15 12 80 85 01 0D
  {
    .custom /*0C000065:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2110
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Average

  .method /*06000021*/ public hidebysig static 
          float64  Average(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<int32> source) cil managed
  // SIG: 00 01 0D 15 12 80 85 01 08
  {
    .custom /*0C000067:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2113
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Average

  .method /*06000022*/ public hidebysig static 
          float64  Average(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<int64> source) cil managed
  // SIG: 00 01 0D 15 12 80 85 01 0A
  {
    .custom /*0C000069:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2116
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Average

  .method /*06000023*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*01000027*/> 
          Average(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*01000027*/>> source) cil managed
  // SIG: 00 01 15 11 80 A1 01 11 80 9D 15 12 80 85 01 15 11 80 A1 01 11 80 9D
  {
    .custom /*0C00006B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2119
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Average

  .method /*06000024*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<float64> 
          Average(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<float64>> source) cil managed
  // SIG: 00 01 15 11 80 A1 01 0D 15 12 80 85 01 15 11 80 A1 01 0D
  {
    .custom /*0C00006D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x211c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Average

  .method /*06000025*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<float64> 
          Average(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<int32>> source) cil managed
  // SIG: 00 01 15 11 80 A1 01 0D 15 12 80 85 01 15 11 80 A1 01 08
  {
    .custom /*0C00006F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x211f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Average

  .method /*06000026*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<float64> 
          Average(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<int64>> source) cil managed
  // SIG: 00 01 15 11 80 A1 01 0D 15 12 80 85 01 15 11 80 A1 01 0A
  {
    .custom /*0C000071:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2122
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Average

  .method /*06000027*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<float32> 
          Average(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<float32>> source) cil managed
  // SIG: 00 01 15 11 80 A1 01 0C 15 12 80 85 01 15 11 80 A1 01 0C
  {
    .custom /*0C000073:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2125
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Average

  .method /*06000028*/ public hidebysig static 
          float32  Average(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<float32> source) cil managed
  // SIG: 00 01 0C 15 12 80 85 01 0C
  {
    .custom /*0C000075:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2128
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Average

  .method /*06000029*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Decimal/*01000027*/ 
          Average<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                           class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Decimal/*01000027*/>> selector) cil managed
  // SIG: 10 01 02 11 80 9D 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 11 80 9D
  {
    .custom /*0C000077:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000078:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000010*/ 
      .custom /*0C00003F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x212b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Average

  .method /*0600002A*/ public hidebysig static 
          float64  Average<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                    class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,float64>> selector) cil managed
  // SIG: 10 01 02 0D 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 0D
  {
    .custom /*0C000079:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00007A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000011*/ 
      .custom /*0C000040:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x212e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Average

  .method /*0600002B*/ public hidebysig static 
          float64  Average<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                    class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,int32>> selector) cil managed
  // SIG: 10 01 02 0D 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 08
  {
    .custom /*0C00007B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00007C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000012*/ 
      .custom /*0C000041:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2131
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Average

  .method /*0600002C*/ public hidebysig static 
          float64  Average<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                    class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,int64>> selector) cil managed
  // SIG: 10 01 02 0D 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 0A
  {
    .custom /*0C00007E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00007F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000013*/ 
      .custom /*0C000042:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2134
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Average

  .method /*0600002D*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*01000027*/> 
          Average<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                           class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*01000027*/>>> selector) cil managed
  // SIG: 10 01 02 15 11 80 A1 01 11 80 9D 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 15 11
  //      80 A1 01 11 80 9D
  {
    .custom /*0C000081:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000082:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000014*/ 
      .custom /*0C000043:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2137
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Average

  .method /*0600002E*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<float64> 
          Average<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                           class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<float64>>> selector) cil managed
  // SIG: 10 01 02 15 11 80 A1 01 0D 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 15 11 80 A1
  //      01 0D
  {
    .custom /*0C000084:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000085:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000015*/ 
      .custom /*0C000045:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x213a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Average

  .method /*0600002F*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<float64> 
          Average<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                           class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<int32>>> selector) cil managed
  // SIG: 10 01 02 15 11 80 A1 01 0D 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 15 11 80 A1
  //      01 08
  {
    .custom /*0C000087:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000088:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000016*/ 
      .custom /*0C000048:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x213d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Average

  .method /*06000030*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<float64> 
          Average<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                           class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<int64>>> selector) cil managed
  // SIG: 10 01 02 15 11 80 A1 01 0D 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 15 11 80 A1
  //      01 0A
  {
    .custom /*0C00008A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00008B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000017*/ 
      .custom /*0C00004B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2140
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Average

  .method /*06000031*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<float32> 
          Average<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                           class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<float32>>> selector) cil managed
  // SIG: 10 01 02 15 11 80 A1 01 0C 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 15 11 80 A1
  //      01 0C
  {
    .custom /*0C00008D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00008E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000018*/ 
      .custom /*0C00004E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2143
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Average

  .method /*06000032*/ public hidebysig static 
          float32  Average<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                    class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,float32>> selector) cil managed
  // SIG: 10 01 02 0C 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 0C
  {
    .custom /*0C000090:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000091:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000019*/ 
      .custom /*0C000051:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2146
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Average

  .method /*06000033*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TResult> 
          Cast<TResult>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable/*0100001A*/ source) cil managed
  // SIG: 10 01 01 15 12 80 85 01 1E 00 12 69
  {
    .custom /*0C000093:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000094:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TResult /*2A00001A*/ 
      .custom /*0C000054:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2149
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Cast

  .method /*06000034*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource[]> 
          Chunk<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                         int32 size) cil managed
  // SIG: 10 01 02 15 12 80 85 01 1D 1E 00 15 12 80 85 01 1E 00 08
  {
    .custom /*0C000095:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000096:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00001B*/ 
      .custom /*0C000057:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x214c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Chunk

  .method /*06000035*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          Concat<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source1,
                          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TSource> source2) cil managed
  // SIG: 10 01 02 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 75 01 1E 00
  {
    .custom /*0C000098:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000099:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00001C*/ 
      .custom /*0C00005A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x214f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Concat

  .method /*06000036*/ public hidebysig static 
          bool  Contains<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                  !!TSource item) cil managed
  // SIG: 10 01 02 02 15 12 80 85 01 1E 00 1E 00
  {
    .custom /*0C00009B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00009C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00001D*/ 
      .custom /*0C00005E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2152
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Contains

  .method /*06000037*/ public hidebysig static 
          bool  Contains<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                  !!TSource item,
                                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!TSource> comparer) cil managed
  // SIG: 10 01 03 02 15 12 80 85 01 1E 00 1E 00 15 12 80 A5 01 1E 00
  {
    .custom /*0C00009E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00009F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00001E*/ 
      .custom /*0C000062:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000041*/ 
    .custom /*0C0000C0:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2155
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Contains

  .method /*06000038*/ public hidebysig static 
          int32  Count<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source) cil managed
  // SIG: 10 01 01 08 15 12 80 85 01 1E 00
  {
    .custom /*0C0000A1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000A2:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00001F*/ 
      .custom /*0C000064:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2158
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Count

  .method /*06000039*/ public hidebysig static 
          int32  Count<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,bool>> predicate) cil managed
  // SIG: 10 01 02 08 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 02
  {
    .custom /*0C0000A4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000A5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000020*/ 
      .custom /*0C000066:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x215b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Count

  .method /*0600003A*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          DefaultIfEmpty<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source) cil managed
  // SIG: 10 01 01 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C0000A7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000A8:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000021*/ 
      .custom /*0C000068:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000045*/ 
    .custom /*0C0000CD:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x215e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::DefaultIfEmpty

  .method /*0600003B*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          DefaultIfEmpty<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                  !!TSource defaultValue) cil managed
  // SIG: 10 01 02 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 1E 00
  {
    .custom /*0C0000AA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000AB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000022*/ 
      .custom /*0C00006A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2161
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::DefaultIfEmpty

  .method /*0600003C*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          DistinctBy<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                   class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector) cil managed
  // SIG: 10 02 02 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01
  {
    .custom /*0C0000AD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000AE:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000023*/ 
      .custom /*0C00006C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000024*/ 
      .custom /*0C00006E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2164
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::DistinctBy

  .method /*0600003D*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          DistinctBy<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                   class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector,
                                   class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!TKey> comparer) cil managed
  // SIG: 10 02 03 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01 15
  //      12 80 A5 01 1E 01
  {
    .custom /*0C0000B0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000B1:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000025*/ 
      .custom /*0C000070:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000026*/ 
      .custom /*0C000072:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800004D*/ 
    .custom /*0C0000E7:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2167
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::DistinctBy

  .method /*0600003E*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          Distinct<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source) cil managed
  // SIG: 10 01 01 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C0000B3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000B4:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000027*/ 
      .custom /*0C000074:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x216a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Distinct

  .method /*0600003F*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          Distinct<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                            class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!TSource> comparer) cil managed
  // SIG: 10 01 02 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 80 A5 01 1E 00
  {
    .custom /*0C0000B6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000B7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000028*/ 
      .custom /*0C000076:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000050*/ 
    .custom /*0C0000F1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x216d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Distinct

  .method /*06000040*/ public hidebysig static 
          !!TSource  ElementAtOrDefault<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                                 valuetype [System.Runtime/*23000001*/]System.Index/*0100002A*/ index) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 85 01 1E 00 11 80 A9
  {
    .custom /*0C0000B9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000BA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000BB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param [1]/*08000051*/ 
    .custom /*0C0000F5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2170
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::ElementAtOrDefault

  .method /*06000041*/ public hidebysig static 
          !!TSource  ElementAtOrDefault<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                                 int32 index) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 85 01 1E 00 08
  {
    .custom /*0C0000BD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000BE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000BF:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param [1]/*08000053*/ 
    .custom /*0C0000FC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2173
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::ElementAtOrDefault

  .method /*06000042*/ public hidebysig static 
          !!TSource  ElementAt<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                        valuetype [System.Runtime/*23000001*/]System.Index/*0100002A*/ index) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 85 01 1E 00 11 80 A9
  {
    .custom /*0C0000C2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000C3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00002B*/ 
      .custom /*0C00007D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2176
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::ElementAt

  .method /*06000043*/ public hidebysig static 
          !!TSource  ElementAt<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                        int32 index) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 85 01 1E 00 08
  {
    .custom /*0C0000C5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000C6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00002C*/ 
      .custom /*0C000080:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2179
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::ElementAt

  .method /*06000044*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          ExceptBy<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source1,
                                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TKey> source2,
                                 class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector) cil managed
  // SIG: 10 02 03 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 75 01 1E 01 15 12 80 91 01 15 12 80 99
  //      02 1E 00 1E 01
  {
    .custom /*0C0000C8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000C9:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00002D*/ 
      .custom /*0C000083:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A00002E*/ 
      .custom /*0C000086:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x217c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::ExceptBy

  .method /*06000045*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          ExceptBy<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source1,
                                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TKey> source2,
                                 class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector,
                                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!TKey> comparer) cil managed
  // SIG: 10 02 04 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 75 01 1E 01 15 12 80 91 01 15 12 80 99
  //      02 1E 00 1E 01 15 12 80 A5 01 1E 01
  {
    .custom /*0C0000CB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000CC:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00002F*/ 
      .custom /*0C000089:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000030*/ 
      .custom /*0C00008C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800005F*/ 
    .custom /*0C000122:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x217f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::ExceptBy

  .method /*06000046*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          Except<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source1,
                          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TSource> source2) cil managed
  // SIG: 10 01 02 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 75 01 1E 00
  {
    .custom /*0C0000CF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000D0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000031*/ 
      .custom /*0C00008F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2182
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Except

  .method /*06000047*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          Except<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source1,
                          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TSource> source2,
                          class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!TSource> comparer) cil managed
  // SIG: 10 01 03 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 75 01 1E 00 15 12 80 A5 01 1E 00
  {
    .custom /*0C0000D2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000D3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000032*/ 
      .custom /*0C000092:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000064*/ 
    .custom /*0C000132:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2185
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Except

  .method /*06000048*/ public hidebysig static 
          !!TSource  FirstOrDefault<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source) cil managed
  // SIG: 10 01 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C0000D5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000D6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000D7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param [1]/*08000065*/ 
    .custom /*0C000136:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2188
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::FirstOrDefault

  .method /*06000049*/ public hidebysig static 
          !!TSource  FirstOrDefault<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                             class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,bool>> predicate) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 02
  {
    .custom /*0C0000D9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000DA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000034*/ 
      .custom /*0C000097:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000066*/ 
    .custom /*0C00013B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x218b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::FirstOrDefault

  .method /*0600004A*/ public hidebysig static 
          !!TSource  FirstOrDefault<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                             class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,bool>> predicate,
                                             !!TSource defaultValue) cil managed
  // SIG: 10 01 03 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 02 1E 00
  {
    .custom /*0C0000DC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000DD:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000035*/ 
      .custom /*0C00009A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x218e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::FirstOrDefault

  .method /*0600004B*/ public hidebysig static 
          !!TSource  FirstOrDefault<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                             !!TSource defaultValue) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 85 01 1E 00 1E 00
  {
    .custom /*0C0000DF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000E0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000036*/ 
      .custom /*0C00009D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2191
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::FirstOrDefault

  .method /*0600004C*/ public hidebysig static 
          !!TSource  First<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source) cil managed
  // SIG: 10 01 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C0000E2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000E3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000037*/ 
      .custom /*0C0000A0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2194
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::First

  .method /*0600004D*/ public hidebysig static 
          !!TSource  First<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                    class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,bool>> predicate) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 02
  {
    .custom /*0C0000E5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000E6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000038*/ 
      .custom /*0C0000A3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2197
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::First

  .method /*0600004E*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<class [System.Linq/*23000003*/]System.Linq.IGrouping`2/*0100002B*/<!!TKey,!!TSource>> 
          GroupBy<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector) cil managed
  // SIG: 10 02 02 15 12 80 85 01 15 12 80 AD 02 1E 01 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80
  //      99 02 1E 00 1E 01
  {
    .custom /*0C0000E9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000EA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000039*/ 
      .custom /*0C0000A6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A00003A*/ 
      .custom /*0C0000A9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x219a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::GroupBy

  .method /*0600004F*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<class [System.Linq/*23000003*/]System.Linq.IGrouping`2/*0100002B*/<!!TKey,!!TSource>> 
          GroupBy<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector,
                                class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!TKey> comparer) cil managed
  // SIG: 10 02 03 15 12 80 85 01 15 12 80 AD 02 1E 01 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80
  //      99 02 1E 00 1E 01 15 12 80 A5 01 1E 01
  {
    .custom /*0C0000EC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000ED:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00003B*/ 
      .custom /*0C0000AC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A00003C*/ 
      .custom /*0C0000AF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000075*/ 
    .custom /*0C00016B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x219d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::GroupBy

  .method /*06000050*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<class [System.Linq/*23000003*/]System.Linq.IGrouping`2/*0100002B*/<!!TKey,!!TElement>> 
          GroupBy<TSource,TKey,TElement>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                         class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector,
                                         class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TElement>> elementSelector) cil managed
  // SIG: 10 03 03 15 12 80 85 01 15 12 80 AD 02 1E 01 1E 02 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80
  //      99 02 1E 00 1E 01 15 12 80 91 01 15 12 80 99 02 1E 00 1E 02
  {
    .custom /*0C0000EF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000F0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00003D*/ 
      .custom /*0C0000B2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A00003E*/ 
      .custom /*0C0000B5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TElement /*2A00003F*/ 
      .custom /*0C0000B8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::GroupBy

  .method /*06000051*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<class [System.Linq/*23000003*/]System.Linq.IGrouping`2/*0100002B*/<!!TKey,!!TElement>> 
          GroupBy<TSource,TKey,TElement>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                         class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector,
                                         class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TElement>> elementSelector,
                                         class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!TKey> comparer) cil managed
  // SIG: 10 03 04 15 12 80 85 01 15 12 80 AD 02 1E 01 1E 02 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80
  //      99 02 1E 00 1E 01 15 12 80 91 01 15 12 80 99 02 1E 00 1E 02 15 12 80 A5 01 1E 01
  {
    .custom /*0C0000F3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000F4:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000040*/ 
      .custom /*0C0000BC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000041*/ 
      .custom /*0C0000C1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TElement /*2A000042*/ 
      .custom /*0C0000C4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800007C*/ 
    .custom /*0C00017A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x21a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::GroupBy

  .method /*06000052*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TResult> 
          GroupBy<TSource,TKey,TResult>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                        class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector,
                                        class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`3/*01000025*/<!!TKey,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TSource>,!!TResult>> resultSelector) cil managed
  // SIG: 10 03 03 15 12 80 85 01 1E 02 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01 15
  //      12 80 91 01 15 12 80 95 03 1E 01 15 12 75 01 1E 00 1E 02
  {
    .custom /*0C0000F7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000F8:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000043*/ 
      .custom /*0C0000C7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000044*/ 
      .custom /*0C0000CA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000045*/ 
      .custom /*0C0000CE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::GroupBy

  .method /*06000053*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TResult> 
          GroupBy<TSource,TKey,TResult>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                        class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector,
                                        class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`3/*01000025*/<!!TKey,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TSource>,!!TResult>> resultSelector,
                                        class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!TKey> comparer) cil managed
  // SIG: 10 03 04 15 12 80 85 01 1E 02 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01 15
  //      12 80 91 01 15 12 80 95 03 1E 01 15 12 75 01 1E 00 1E 02 15 12 80 A5 01 1E 01
  {
    .custom /*0C0000FA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000FB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000046*/ 
      .custom /*0C0000D1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000047*/ 
      .custom /*0C0000D4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000048*/ 
      .custom /*0C0000D8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000083*/ 
    .custom /*0C00018E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x21a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::GroupBy

  .method /*06000054*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TResult> 
          GroupBy<TSource,TKey,TElement,TResult>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                                 class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector,
                                                 class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TElement>> elementSelector,
                                                 class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`3/*01000025*/<!!TKey,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TElement>,!!TResult>> resultSelector) cil managed
  // SIG: 10 04 04 15 12 80 85 01 1E 03 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01 15
  //      12 80 91 01 15 12 80 99 02 1E 00 1E 02 15 12 80 91 01 15 12 80 95 03 1E 01 15 12 75 01 1E 02 1E
  //      03
  {
    .custom /*0C0000FE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000FF:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000049*/ 
      .custom /*0C0000DB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A00004A*/ 
      .custom /*0C0000DE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TElement /*2A00004B*/ 
      .custom /*0C0000E1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A00004C*/ 
      .custom /*0C0000E4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::GroupBy

  .method /*06000055*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TResult> 
          GroupBy<TSource,TKey,TElement,TResult>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                                 class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector,
                                                 class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TElement>> elementSelector,
                                                 class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`3/*01000025*/<!!TKey,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TElement>,!!TResult>> resultSelector,
                                                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!TKey> comparer) cil managed
  // SIG: 10 04 05 15 12 80 85 01 1E 03 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01 15
  //      12 80 91 01 15 12 80 99 02 1E 00 1E 02 15 12 80 91 01 15 12 80 95 03 1E 01 15 12 75 01 1E 02 1E
  //      03 15 12 80 A5 01 1E 01
  {
    .custom /*0C000101:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000102:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00004D*/ 
      .custom /*0C0000E8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A00004E*/ 
      .custom /*0C0000EB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TElement /*2A00004F*/ 
      .custom /*0C0000EE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000050*/ 
      .custom /*0C0000F2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800008C*/ 
    .custom /*0C0001AA:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x21af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::GroupBy

  .method /*06000056*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TResult> 
          GroupJoin<TOuter,TInner,TKey,TResult>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TOuter> outer,
                                                class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TInner> inner,
                                                class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TOuter,!!TKey>> outerKeySelector,
                                                class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TInner,!!TKey>> innerKeySelector,
                                                class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`3/*01000025*/<!!TOuter,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TInner>,!!TResult>> resultSelector) cil managed
  // SIG: 10 04 05 15 12 80 85 01 1E 03 15 12 80 85 01 1E 00 15 12 75 01 1E 01 15 12 80 91 01 15 12 80 99
  //      02 1E 00 1E 02 15 12 80 91 01 15 12 80 99 02 1E 01 1E 02 15 12 80 91 01 15 12 80 95 03 1E 00 15
  //      12 75 01 1E 01 1E 03
  {
    .custom /*0C000104:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000105:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TOuter /*2A000051*/ 
      .custom /*0C0000F6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TInner /*2A000052*/ 
      .custom /*0C0000F9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000053*/ 
      .custom /*0C0000FD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000054*/ 
      .custom /*0C000100:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::GroupJoin

  .method /*06000057*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TResult> 
          GroupJoin<TOuter,TInner,TKey,TResult>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TOuter> outer,
                                                class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TInner> inner,
                                                class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TOuter,!!TKey>> outerKeySelector,
                                                class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TInner,!!TKey>> innerKeySelector,
                                                class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`3/*01000025*/<!!TOuter,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TInner>,!!TResult>> resultSelector,
                                                class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!TKey> comparer) cil managed
  // SIG: 10 04 06 15 12 80 85 01 1E 03 15 12 80 85 01 1E 00 15 12 75 01 1E 01 15 12 80 91 01 15 12 80 99
  //      02 1E 00 1E 02 15 12 80 91 01 15 12 80 99 02 1E 01 1E 02 15 12 80 91 01 15 12 80 95 03 1E 00 15
  //      12 75 01 1E 01 1E 03 15 12 80 A5 01 1E 02
  {
    .custom /*0C000107:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000108:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TOuter /*2A000055*/ 
      .custom /*0C000103:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TInner /*2A000056*/ 
      .custom /*0C000106:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000057*/ 
      .custom /*0C000109:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000058*/ 
      .custom /*0C00010C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [6]/*08000097*/ 
    .custom /*0C0001C2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x21b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::GroupJoin

  .method /*06000058*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          IntersectBy<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source1,
                                    class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TKey> source2,
                                    class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector) cil managed
  // SIG: 10 02 03 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 75 01 1E 01 15 12 80 91 01 15 12 80 99
  //      02 1E 00 1E 01
  {
    .custom /*0C00010A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00010B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000059*/ 
      .custom /*0C00010F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A00005A*/ 
      .custom /*0C000112:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::IntersectBy

  .method /*06000059*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          IntersectBy<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source1,
                                    class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TKey> source2,
                                    class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector,
                                    class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!TKey> comparer) cil managed
  // SIG: 10 02 04 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 75 01 1E 01 15 12 80 91 01 15 12 80 99
  //      02 1E 00 1E 01 15 12 80 A5 01 1E 01
  {
    .custom /*0C00010D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00010E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00005B*/ 
      .custom /*0C000115:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A00005C*/ 
      .custom /*0C000118:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800009E*/ 
    .custom /*0C0001D8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x21bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::IntersectBy

  .method /*0600005A*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          Intersect<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source1,
                             class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TSource> source2) cil managed
  // SIG: 10 01 02 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 75 01 1E 00
  {
    .custom /*0C000110:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000111:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00005D*/ 
      .custom /*0C00011B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Intersect

  .method /*0600005B*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          Intersect<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source1,
                             class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TSource> source2,
                             class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!TSource> comparer) cil managed
  // SIG: 10 01 03 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 75 01 1E 00 15 12 80 A5 01 1E 00
  {
    .custom /*0C000113:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000114:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00005E*/ 
      .custom /*0C00011F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080000A3*/ 
    .custom /*0C0001E7:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x21c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Intersect

  .method /*0600005C*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TResult> 
          Join<TOuter,TInner,TKey,TResult>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TOuter> outer,
                                           class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TInner> inner,
                                           class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TOuter,!!TKey>> outerKeySelector,
                                           class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TInner,!!TKey>> innerKeySelector,
                                           class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`3/*01000025*/<!!TOuter,!!TInner,!!TResult>> resultSelector) cil managed
  // SIG: 10 04 05 15 12 80 85 01 1E 03 15 12 80 85 01 1E 00 15 12 75 01 1E 01 15 12 80 91 01 15 12 80 99
  //      02 1E 00 1E 02 15 12 80 91 01 15 12 80 99 02 1E 01 1E 02 15 12 80 91 01 15 12 80 95 03 1E 00 1E
  //      01 1E 03
  {
    .custom /*0C000116:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000117:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TOuter /*2A00005F*/ 
      .custom /*0C000123:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TInner /*2A000060*/ 
      .custom /*0C000126:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000061*/ 
      .custom /*0C000129:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000062*/ 
      .custom /*0C00012C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Join

  .method /*0600005D*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TResult> 
          Join<TOuter,TInner,TKey,TResult>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TOuter> outer,
                                           class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TInner> inner,
                                           class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TOuter,!!TKey>> outerKeySelector,
                                           class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TInner,!!TKey>> innerKeySelector,
                                           class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`3/*01000025*/<!!TOuter,!!TInner,!!TResult>> resultSelector,
                                           class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!TKey> comparer) cil managed
  // SIG: 10 04 06 15 12 80 85 01 1E 03 15 12 80 85 01 1E 00 15 12 75 01 1E 01 15 12 80 91 01 15 12 80 99
  //      02 1E 00 1E 02 15 12 80 91 01 15 12 80 99 02 1E 01 1E 02 15 12 80 91 01 15 12 80 95 03 1E 00 1E
  //      01 1E 03 15 12 80 A5 01 1E 02
  {
    .custom /*0C000119:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00011A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TOuter /*2A000063*/ 
      .custom /*0C00012F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TInner /*2A000064*/ 
      .custom /*0C000133:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000065*/ 
      .custom /*0C000137:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000066*/ 
      .custom /*0C00013C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [6]/*080000AE*/ 
    .custom /*0C000209:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x21c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Join

  .method /*0600005E*/ public hidebysig static 
          !!TSource  LastOrDefault<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source) cil managed
  // SIG: 10 01 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C00011C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00011D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00011E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param [1]/*080000AF*/ 
    .custom /*0C00020D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::LastOrDefault

  .method /*0600005F*/ public hidebysig static 
          !!TSource  LastOrDefault<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                            class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,bool>> predicate) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 02
  {
    .custom /*0C000120:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000121:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000068*/ 
      .custom /*0C000143:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080000B0*/ 
    .custom /*0C000211:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::LastOrDefault

  .method /*06000060*/ public hidebysig static 
          !!TSource  LastOrDefault<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                            class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,bool>> predicate,
                                            !!TSource defaultValue) cil managed
  // SIG: 10 01 03 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 02 1E 00
  {
    .custom /*0C000124:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000125:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000069*/ 
      .custom /*0C000147:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::LastOrDefault

  .method /*06000061*/ public hidebysig static 
          !!TSource  LastOrDefault<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                            !!TSource defaultValue) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 85 01 1E 00 1E 00
  {
    .custom /*0C000127:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000128:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00006A*/ 
      .custom /*0C00014B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::LastOrDefault

  .method /*06000062*/ public hidebysig static 
          !!TSource  Last<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source) cil managed
  // SIG: 10 01 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C00012A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00012B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00006B*/ 
      .custom /*0C00014F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Last

  .method /*06000063*/ public hidebysig static 
          !!TSource  Last<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                   class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,bool>> predicate) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 02
  {
    .custom /*0C00012D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00012E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00006C*/ 
      .custom /*0C000153:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Last

  .method /*06000064*/ public hidebysig static 
          int64  LongCount<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source) cil managed
  // SIG: 10 01 01 0A 15 12 80 85 01 1E 00
  {
    .custom /*0C000130:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000131:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00006D*/ 
      .custom /*0C000157:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::LongCount

  .method /*06000065*/ public hidebysig static 
          int64  LongCount<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                    class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,bool>> predicate) cil managed
  // SIG: 10 01 02 0A 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 02
  {
    .custom /*0C000134:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000135:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00006E*/ 
      .custom /*0C00015B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::LongCount

  .method /*06000066*/ public hidebysig static 
          !!TSource  MaxBy<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                         class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector) cil managed
  // SIG: 10 02 02 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01
  {
    .custom /*0C000138:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000139:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00013A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param [1]/*080000BE*/ 
    .custom /*0C000224:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000BF*/ 
    .custom /*0C000226:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::MaxBy

  .method /*06000067*/ public hidebysig static 
          !!TSource  MaxBy<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                         class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector,
                                         class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*0100002C*/<!!TSource> comparer) cil managed
  // SIG: 10 02 03 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01 15 12 80 B1 01 1E
  //      00
  {
    .custom /*0C00013D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00013E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00013F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param [1]/*080000C0*/ 
    .custom /*0C000228:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000C1*/ 
    .custom /*0C00022A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*080000C2*/ 
    .custom /*0C00022C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x21e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::MaxBy

  .method /*06000068*/ public hidebysig static 
          !!TSource  Max<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source) cil managed
  // SIG: 10 01 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C000140:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000141:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000142:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param [1]/*080000C3*/ 
    .custom /*0C00022E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Max

  .method /*06000069*/ public hidebysig static 
          !!TSource  Max<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                  class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*0100002C*/<!!TSource> comparer) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 85 01 1E 00 15 12 80 B1 01 1E 00
  {
    .custom /*0C000144:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000145:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000146:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param [1]/*080000C4*/ 
    .custom /*0C000230:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000C5*/ 
    .custom /*0C000232:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x21eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Max

  .method /*0600006A*/ public hidebysig static 
          !!TResult  Max<TSource,TResult>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                          class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TResult>> selector) cil managed
  // SIG: 10 02 02 1E 01 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01
  {
    .custom /*0C000148:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000149:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00014A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param [1]/*080000C6*/ 
    .custom /*0C000234:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000C7*/ 
    .custom /*0C000236:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Max

  .method /*0600006B*/ public hidebysig static 
          !!TSource  MinBy<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                         class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector) cil managed
  // SIG: 10 02 02 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01
  {
    .custom /*0C00014C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00014D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00014E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param [1]/*080000C8*/ 
    .custom /*0C000238:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000C9*/ 
    .custom /*0C00023A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::MinBy

  .method /*0600006C*/ public hidebysig static 
          !!TSource  MinBy<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                         class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector,
                                         class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*0100002C*/<!!TSource> comparer) cil managed
  // SIG: 10 02 03 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01 15 12 80 B1 01 1E
  //      00
  {
    .custom /*0C000150:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000151:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000152:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param [1]/*080000CA*/ 
    .custom /*0C00023C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000CB*/ 
    .custom /*0C00023E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*080000CC*/ 
    .custom /*0C000240:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x21f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::MinBy

  .method /*0600006D*/ public hidebysig static 
          !!TSource  Min<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source) cil managed
  // SIG: 10 01 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C000154:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000155:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000156:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param [1]/*080000CD*/ 
    .custom /*0C000242:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Min

  .method /*0600006E*/ public hidebysig static 
          !!TSource  Min<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                  class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*0100002C*/<!!TSource> comparer) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 85 01 1E 00 15 12 80 B1 01 1E 00
  {
    .custom /*0C000158:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000159:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00015A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param [1]/*080000CE*/ 
    .custom /*0C000244:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000CF*/ 
    .custom /*0C000246:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x21fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Min

  .method /*0600006F*/ public hidebysig static 
          !!TResult  Min<TSource,TResult>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                          class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TResult>> selector) cil managed
  // SIG: 10 02 02 1E 01 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01
  {
    .custom /*0C00015C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00015D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00015E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param [1]/*080000D0*/ 
    .custom /*0C000247:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000D1*/ 
    .custom /*0C000248:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Min

  .method /*06000070*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TResult> 
          OfType<TResult>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable/*0100001A*/ source) cil managed
  // SIG: 10 01 01 15 12 80 85 01 1E 00 12 69
  {
    .custom /*0C00015F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000160:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TResult /*2A00007F*/ 
      .custom /*0C000181:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2200
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::OfType

  .method /*06000071*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IOrderedQueryable`1/*01000023*/<!!TSource> 
          OrderByDescending<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                          class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector) cil managed
  // SIG: 10 02 02 15 12 80 8D 01 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01
  {
    .custom /*0C000161:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000162:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000080*/ 
      .custom /*0C000184:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000081*/ 
      .custom /*0C000187:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2203
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::OrderByDescending

  .method /*06000072*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IOrderedQueryable`1/*01000023*/<!!TSource> 
          OrderByDescending<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                          class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector,
                                          class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*0100002C*/<!!TKey> comparer) cil managed
  // SIG: 10 02 03 15 12 80 8D 01 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01 15
  //      12 80 B1 01 1E 01
  {
    .custom /*0C000163:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000164:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000082*/ 
      .custom /*0C00018A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000083*/ 
      .custom /*0C00018F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080000D7*/ 
    .custom /*0C000249:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2206
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::OrderByDescending

  .method /*06000073*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IOrderedQueryable`1/*01000023*/<!!TSource> 
          OrderBy<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector) cil managed
  // SIG: 10 02 02 15 12 80 8D 01 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01
  {
    .custom /*0C000165:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000166:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000084*/ 
      .custom /*0C000192:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000085*/ 
      .custom /*0C000195:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2209
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::OrderBy

  .method /*06000074*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IOrderedQueryable`1/*01000023*/<!!TSource> 
          OrderBy<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector,
                                class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*0100002C*/<!!TKey> comparer) cil managed
  // SIG: 10 02 03 15 12 80 8D 01 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01 15
  //      12 80 B1 01 1E 01
  {
    .custom /*0C000167:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000168:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000086*/ 
      .custom /*0C000198:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000087*/ 
      .custom /*0C00019B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080000DC*/ 
    .custom /*0C00024A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x220c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::OrderBy

  .method /*06000075*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IOrderedQueryable`1/*01000023*/<!!T> 
          OrderDescending<T>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!T> source) cil managed
  // SIG: 10 01 01 15 12 80 8D 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C000169:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00016A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type T /*2A000088*/ 
      .custom /*0C00019E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x220f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::OrderDescending

  .method /*06000076*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IOrderedQueryable`1/*01000023*/<!!T> 
          OrderDescending<T>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!T> source,
                             class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*0100002C*/<!!T> comparer) cil managed
  // SIG: 10 01 02 15 12 80 8D 01 1E 00 15 12 80 85 01 1E 00 15 12 80 B1 01 1E 00
  {
    .custom /*0C00016C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00016D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type T /*2A000089*/ 
      .custom /*0C0001A1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2212
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::OrderDescending

  .method /*06000077*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IOrderedQueryable`1/*01000023*/<!!T> 
          Order<T>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!T> source) cil managed
  // SIG: 10 01 01 15 12 80 8D 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C00016E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00016F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type T /*2A00008A*/ 
      .custom /*0C0001A4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2215
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Order

  .method /*06000078*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IOrderedQueryable`1/*01000023*/<!!T> 
          Order<T>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!T> source,
                   class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*0100002C*/<!!T> comparer) cil managed
  // SIG: 10 01 02 15 12 80 8D 01 1E 00 15 12 80 85 01 1E 00 15 12 80 B1 01 1E 00
  {
    .custom /*0C000170:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000171:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type T /*2A00008B*/ 
      .custom /*0C0001A7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2218
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Order

  .method /*06000079*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          Prepend<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                           !!TSource element) cil managed
  // SIG: 10 01 02 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 1E 00
  {
    .custom /*0C000172:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000173:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00008C*/ 
      .custom /*0C0001AB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x221b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Prepend

  .method /*0600007A*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          Reverse<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source) cil managed
  // SIG: 10 01 01 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C000174:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000175:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00008D*/ 
      .custom /*0C0001AD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x221e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Reverse

  .method /*0600007B*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TResult> 
          SelectMany<TSource,TResult>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                      class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TResult>>> selector) cil managed
  // SIG: 10 02 02 15 12 80 85 01 1E 01 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 15 12 75
  //      01 1E 01
  {
    .custom /*0C000176:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000177:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00008E*/ 
      .custom /*0C0001AF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A00008F*/ 
      .custom /*0C0001B1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2221
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::SelectMany

  .method /*0600007C*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TResult> 
          SelectMany<TSource,TResult>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                      class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`3/*01000025*/<!!TSource,int32,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TResult>>> selector) cil managed
  // SIG: 10 02 02 15 12 80 85 01 1E 01 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 95 03 1E 00 08 15 12
  //      75 01 1E 01
  {
    .custom /*0C000178:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000179:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000090*/ 
      .custom /*0C0001B3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000091*/ 
      .custom /*0C0001B5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2224
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::SelectMany

  .method /*0600007D*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TResult> 
          SelectMany<TSource,TCollection,TResult>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                                  class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TCollection>>> collectionSelector,
                                                  class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`3/*01000025*/<!!TSource,!!TCollection,!!TResult>> resultSelector) cil managed
  // SIG: 10 03 03 15 12 80 85 01 1E 02 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 15 12 75
  //      01 1E 01 15 12 80 91 01 15 12 80 95 03 1E 00 1E 01 1E 02
  {
    .custom /*0C00017B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00017C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000092*/ 
      .custom /*0C0001B7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TCollection /*2A000093*/ 
      .custom /*0C0001B9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000094*/ 
      .custom /*0C0001BB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2227
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::SelectMany

  .method /*0600007E*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TResult> 
          SelectMany<TSource,TCollection,TResult>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                                  class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`3/*01000025*/<!!TSource,int32,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TCollection>>> collectionSelector,
                                                  class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`3/*01000025*/<!!TSource,!!TCollection,!!TResult>> resultSelector) cil managed
  // SIG: 10 03 03 15 12 80 85 01 1E 02 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 95 03 1E 00 08 15 12
  //      75 01 1E 01 15 12 80 91 01 15 12 80 95 03 1E 00 1E 01 1E 02
  {
    .custom /*0C00017D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00017E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000095*/ 
      .custom /*0C0001BD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TCollection /*2A000096*/ 
      .custom /*0C0001BF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000097*/ 
      .custom /*0C0001C3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x222a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::SelectMany

  .method /*0600007F*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TResult> 
          Select<TSource,TResult>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                  class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`3/*01000025*/<!!TSource,int32,!!TResult>> selector) cil managed
  // SIG: 10 02 02 15 12 80 85 01 1E 01 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 95 03 1E 00 08 1E 01
  {
    .custom /*0C00017F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000180:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A000098*/ 
      .custom /*0C0001C6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000099*/ 
      .custom /*0C0001C9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x222d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Select

  .method /*06000080*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TResult> 
          Select<TSource,TResult>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                  class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TResult>> selector) cil managed
  // SIG: 10 02 02 15 12 80 85 01 1E 01 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01
  {
    .custom /*0C000182:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000183:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00009A*/ 
      .custom /*0C0001CC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A00009B*/ 
      .custom /*0C0001CF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2230
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Select

  .method /*06000081*/ public hidebysig static 
          bool  SequenceEqual<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source1,
                                       class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TSource> source2) cil managed
  // SIG: 10 01 02 02 15 12 80 85 01 1E 00 15 12 75 01 1E 00
  {
    .custom /*0C000185:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000186:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00009C*/ 
      .custom /*0C0001D2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2233
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::SequenceEqual

  .method /*06000082*/ public hidebysig static 
          bool  SequenceEqual<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source1,
                                       class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TSource> source2,
                                       class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!TSource> comparer) cil managed
  // SIG: 10 01 03 02 15 12 80 85 01 1E 00 15 12 75 01 1E 00 15 12 80 A5 01 1E 00
  {
    .custom /*0C000188:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000189:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00009D*/ 
      .custom /*0C0001D5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080000F8*/ 
    .custom /*0C00024B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2236
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::SequenceEqual

  .method /*06000083*/ public hidebysig static 
          !!TSource  SingleOrDefault<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source) cil managed
  // SIG: 10 01 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C00018B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00018C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00018D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param [1]/*080000F9*/ 
    .custom /*0C00024C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2239
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::SingleOrDefault

  .method /*06000084*/ public hidebysig static 
          !!TSource  SingleOrDefault<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                              class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,bool>> predicate) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 02
  {
    .custom /*0C000190:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000191:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A00009F*/ 
      .custom /*0C0001DB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080000FA*/ 
    .custom /*0C00024D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x223c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::SingleOrDefault

  .method /*06000085*/ public hidebysig static 
          !!TSource  SingleOrDefault<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                              class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,bool>> predicate,
                                              !!TSource defaultValue) cil managed
  // SIG: 10 01 03 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 02 1E 00
  {
    .custom /*0C000193:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000194:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000A0*/ 
      .custom /*0C0001DE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x223f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::SingleOrDefault

  .method /*06000086*/ public hidebysig static 
          !!TSource  SingleOrDefault<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                              !!TSource defaultValue) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 85 01 1E 00 1E 00
  {
    .custom /*0C000196:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000197:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000A1*/ 
      .custom /*0C0001E1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2242
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::SingleOrDefault

  .method /*06000087*/ public hidebysig static 
          !!TSource  Single<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source) cil managed
  // SIG: 10 01 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C000199:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00019A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000A2*/ 
      .custom /*0C0001E4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2245
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Single

  .method /*06000088*/ public hidebysig static 
          !!TSource  Single<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                     class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,bool>> predicate) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 02
  {
    .custom /*0C00019C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00019D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000A3*/ 
      .custom /*0C0001E8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2248
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Single

  .method /*06000089*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          SkipLast<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                            int32 count) cil managed
  // SIG: 10 01 02 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 08
  {
    .custom /*0C00019F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001A0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000A4*/ 
      .custom /*0C0001EB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x224b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::SkipLast

  .method /*0600008A*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          SkipWhile<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                             class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,bool>> predicate) cil managed
  // SIG: 10 01 02 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 02
  {
    .custom /*0C0001A2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001A3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000A5*/ 
      .custom /*0C0001EE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x224e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::SkipWhile

  .method /*0600008B*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          SkipWhile<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                             class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`3/*01000025*/<!!TSource,int32,bool>> predicate) cil managed
  // SIG: 10 01 02 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 95 03 1E 00 08 02
  {
    .custom /*0C0001A5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001A6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000A6*/ 
      .custom /*0C0001F1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2251
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::SkipWhile

  .method /*0600008C*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          Skip<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                        int32 count) cil managed
  // SIG: 10 01 02 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 08
  {
    .custom /*0C0001A8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001A9:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000A7*/ 
      .custom /*0C0001F4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2254
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Skip

  .method /*0600008D*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Decimal/*01000027*/ 
          Sum(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*01000027*/> source) cil managed
  // SIG: 00 01 11 80 9D 15 12 80 85 01 11 80 9D
  {
    .custom /*0C0001AC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2257
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Sum

  .method /*0600008E*/ public hidebysig static 
          float64  Sum(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<float64> source) cil managed
  // SIG: 00 01 0D 15 12 80 85 01 0D
  {
    .custom /*0C0001AE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x225a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Sum

  .method /*0600008F*/ public hidebysig static 
          int32  Sum(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<int32> source) cil managed
  // SIG: 00 01 08 15 12 80 85 01 08
  {
    .custom /*0C0001B0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x225d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Sum

  .method /*06000090*/ public hidebysig static 
          int64  Sum(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<int64> source) cil managed
  // SIG: 00 01 0A 15 12 80 85 01 0A
  {
    .custom /*0C0001B2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2260
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Sum

  .method /*06000091*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*01000027*/> 
          Sum(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*01000027*/>> source) cil managed
  // SIG: 00 01 15 11 80 A1 01 11 80 9D 15 12 80 85 01 15 11 80 A1 01 11 80 9D
  {
    .custom /*0C0001B4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2263
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Sum

  .method /*06000092*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<float64> 
          Sum(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<float64>> source) cil managed
  // SIG: 00 01 15 11 80 A1 01 0D 15 12 80 85 01 15 11 80 A1 01 0D
  {
    .custom /*0C0001B6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2266
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Sum

  .method /*06000093*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<int32> 
          Sum(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<int32>> source) cil managed
  // SIG: 00 01 15 11 80 A1 01 08 15 12 80 85 01 15 11 80 A1 01 08
  {
    .custom /*0C0001B8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2269
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Sum

  .method /*06000094*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<int64> 
          Sum(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<int64>> source) cil managed
  // SIG: 00 01 15 11 80 A1 01 0A 15 12 80 85 01 15 11 80 A1 01 0A
  {
    .custom /*0C0001BA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x226c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Sum

  .method /*06000095*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<float32> 
          Sum(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<float32>> source) cil managed
  // SIG: 00 01 15 11 80 A1 01 0C 15 12 80 85 01 15 11 80 A1 01 0C
  {
    .custom /*0C0001BC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x226f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Sum

  .method /*06000096*/ public hidebysig static 
          float32  Sum(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<float32> source) cil managed
  // SIG: 00 01 0C 15 12 80 85 01 0C
  {
    .custom /*0C0001BE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2272
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Sum

  .method /*06000097*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Decimal/*01000027*/ 
          Sum<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                       class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Decimal/*01000027*/>> selector) cil managed
  // SIG: 10 01 02 11 80 9D 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 11 80 9D
  {
    .custom /*0C0001C0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001C1:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000A8*/ 
      .custom /*0C0001F7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2275
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Sum

  .method /*06000098*/ public hidebysig static 
          float64  Sum<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,float64>> selector) cil managed
  // SIG: 10 01 02 0D 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 0D
  {
    .custom /*0C0001C4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001C5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000A9*/ 
      .custom /*0C0001FA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2278
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Sum

  .method /*06000099*/ public hidebysig static 
          int32  Sum<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                              class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,int32>> selector) cil managed
  // SIG: 10 01 02 08 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 08
  {
    .custom /*0C0001C7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001C8:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000AA*/ 
      .custom /*0C0001FD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x227b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Sum

  .method /*0600009A*/ public hidebysig static 
          int64  Sum<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                              class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,int64>> selector) cil managed
  // SIG: 10 01 02 0A 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 0A
  {
    .custom /*0C0001CA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001CB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000AB*/ 
      .custom /*0C000200:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x227e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Sum

  .method /*0600009B*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*01000027*/> 
          Sum<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                       class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*01000027*/>>> selector) cil managed
  // SIG: 10 01 02 15 11 80 A1 01 11 80 9D 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 15 11
  //      80 A1 01 11 80 9D
  {
    .custom /*0C0001CD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001CE:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000AC*/ 
      .custom /*0C000203:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2281
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Sum

  .method /*0600009C*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<float64> 
          Sum<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                       class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<float64>>> selector) cil managed
  // SIG: 10 01 02 15 11 80 A1 01 0D 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 15 11 80 A1
  //      01 0D
  {
    .custom /*0C0001D0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001D1:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000AD*/ 
      .custom /*0C000206:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2284
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Sum

  .method /*0600009D*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<int32> 
          Sum<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                       class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<int32>>> selector) cil managed
  // SIG: 10 01 02 15 11 80 A1 01 08 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 15 11 80 A1
  //      01 08
  {
    .custom /*0C0001D3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001D4:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000AE*/ 
      .custom /*0C00020A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2287
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Sum

  .method /*0600009E*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<int64> 
          Sum<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                       class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<int64>>> selector) cil managed
  // SIG: 10 01 02 15 11 80 A1 01 0A 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 15 11 80 A1
  //      01 0A
  {
    .custom /*0C0001D6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001D7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000AF*/ 
      .custom /*0C00020E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x228a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Sum

  .method /*0600009F*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<float32> 
          Sum<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                       class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000028*/<float32>>> selector) cil managed
  // SIG: 10 01 02 15 11 80 A1 01 0C 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 15 11 80 A1
  //      01 0C
  {
    .custom /*0C0001D9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001DA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000B0*/ 
      .custom /*0C000212:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x228d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Sum

  .method /*060000A0*/ public hidebysig static 
          float32  Sum<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                                class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,float32>> selector) cil managed
  // SIG: 10 01 02 0C 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 0C
  {
    .custom /*0C0001DC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001DD:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000B1*/ 
      .custom /*0C000215:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2290
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Sum

  .method /*060000A1*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          TakeLast<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                            int32 count) cil managed
  // SIG: 10 01 02 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 08
  {
    .custom /*0C0001DF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001E0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000B2*/ 
      .custom /*0C000218:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2293
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::TakeLast

  .method /*060000A2*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          TakeWhile<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                             class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,bool>> predicate) cil managed
  // SIG: 10 01 02 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 02
  {
    .custom /*0C0001E2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001E3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000B3*/ 
      .custom /*0C000219:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2296
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::TakeWhile

  .method /*060000A3*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          TakeWhile<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                             class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`3/*01000025*/<!!TSource,int32,bool>> predicate) cil managed
  // SIG: 10 01 02 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 95 03 1E 00 08 02
  {
    .custom /*0C0001E5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001E6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000B4*/ 
      .custom /*0C00021A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2299
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::TakeWhile

  .method /*060000A4*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          Take<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                        int32 count) cil managed
  // SIG: 10 01 02 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 08
  {
    .custom /*0C0001E9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001EA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000B5*/ 
      .custom /*0C00021B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x229c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Take

  .method /*060000A5*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          Take<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                        valuetype [System.Runtime/*23000001*/]System.Range/*0100002D*/ range) cil managed
  // SIG: 10 01 02 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 11 80 B5
  {
    .custom /*0C0001EC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001ED:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000B6*/ 
      .custom /*0C00021C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x229f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Take

  .method /*060000A6*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IOrderedQueryable`1/*01000023*/<!!TSource> 
          ThenByDescending<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IOrderedQueryable`1/*01000023*/<!!TSource> source,
                                         class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector) cil managed
  // SIG: 10 02 02 15 12 80 8D 01 1E 00 15 12 80 8D 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01
  {
    .custom /*0C0001EF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001F0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000B7*/ 
      .custom /*0C00021D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A0000B8*/ 
      .custom /*0C00021E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::ThenByDescending

  .method /*060000A7*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IOrderedQueryable`1/*01000023*/<!!TSource> 
          ThenByDescending<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IOrderedQueryable`1/*01000023*/<!!TSource> source,
                                         class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector,
                                         class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*0100002C*/<!!TKey> comparer) cil managed
  // SIG: 10 02 03 15 12 80 8D 01 1E 00 15 12 80 8D 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01 15
  //      12 80 B1 01 1E 01
  {
    .custom /*0C0001F2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001F3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000B9*/ 
      .custom /*0C00021F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A0000BA*/ 
      .custom /*0C000220:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000139*/ 
    .custom /*0C00024E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::ThenByDescending

  .method /*060000A8*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IOrderedQueryable`1/*01000023*/<!!TSource> 
          ThenBy<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IOrderedQueryable`1/*01000023*/<!!TSource> source,
                               class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector) cil managed
  // SIG: 10 02 02 15 12 80 8D 01 1E 00 15 12 80 8D 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01
  {
    .custom /*0C0001F5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001F6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000BB*/ 
      .custom /*0C000221:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A0000BC*/ 
      .custom /*0C000222:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::ThenBy

  .method /*060000A9*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IOrderedQueryable`1/*01000023*/<!!TSource> 
          ThenBy<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IOrderedQueryable`1/*01000023*/<!!TSource> source,
                               class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*0100002C*/<!!TKey> comparer) cil managed
  // SIG: 10 02 03 15 12 80 8D 01 1E 00 15 12 80 8D 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 1E 01 15
  //      12 80 B1 01 1E 01
  {
    .custom /*0C0001F8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001F9:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000BD*/ 
      .custom /*0C000223:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A0000BE*/ 
      .custom /*0C000225:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800013E*/ 
    .custom /*0C00024F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::ThenBy

  .method /*060000AA*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          UnionBy<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source1,
                                class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TSource> source2,
                                class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector) cil managed
  // SIG: 10 02 03 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 75 01 1E 00 15 12 80 91 01 15 12 80 99
  //      02 1E 00 1E 01
  {
    .custom /*0C0001FB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001FC:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000BF*/ 
      .custom /*0C000227:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A0000C0*/ 
      .custom /*0C000229:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::UnionBy

  .method /*060000AB*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          UnionBy<TSource,TKey>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source1,
                                class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TSource> source2,
                                class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,!!TKey>> keySelector,
                                class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!TKey> comparer) cil managed
  // SIG: 10 02 04 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 75 01 1E 00 15 12 80 91 01 15 12 80 99
  //      02 1E 00 1E 01 15 12 80 A5 01 1E 01
  {
    .custom /*0C0001FE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001FF:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000C1*/ 
      .custom /*0C00022B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A0000C2*/ 
      .custom /*0C00022D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000145*/ 
    .custom /*0C000250:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::UnionBy

  .method /*060000AC*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          Union<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source1,
                         class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TSource> source2) cil managed
  // SIG: 10 01 02 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 75 01 1E 00
  {
    .custom /*0C000201:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000202:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000C3*/ 
      .custom /*0C00022F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Union

  .method /*060000AD*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          Union<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source1,
                         class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TSource> source2,
                         class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000029*/<!!TSource> comparer) cil managed
  // SIG: 10 01 03 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 75 01 1E 00 15 12 80 A5 01 1E 00
  {
    .custom /*0C000204:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000205:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000C4*/ 
      .custom /*0C000231:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800014A*/ 
    .custom /*0C000251:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Union

  .method /*060000AE*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          Where<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                         class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<!!TSource,bool>> predicate) cil managed
  // SIG: 10 01 02 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 99 02 1E 00 02
  {
    .custom /*0C000207:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000208:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000C5*/ 
      .custom /*0C000233:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Where

  .method /*060000AF*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> 
          Where<TSource>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TSource> source,
                         class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`3/*01000025*/<!!TSource,int32,bool>> predicate) cil managed
  // SIG: 10 01 02 15 12 80 85 01 1E 00 15 12 80 85 01 1E 00 15 12 80 91 01 15 12 80 95 03 1E 00 08 02
  {
    .custom /*0C00020B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00020C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TSource /*2A0000C6*/ 
      .custom /*0C000235:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Where

  .method /*060000B0*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<valuetype [System.Runtime/*23000001*/]System.ValueTuple`2/*0100002F*/<!!TFirst,!!TSecond>> 
          Zip<TFirst,TSecond>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TFirst> source1,
                              class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TSecond> source2) cil managed
  // SIG: 10 02 02 15 12 80 85 01 15 11 80 BD 02 1E 00 1E 01 15 12 80 85 01 1E 00 15 12 75 01 1E 01
  {
    .custom /*0C00020F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000210:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TFirst /*2A0000C7*/ 
      .custom /*0C000237:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TSecond /*2A0000C8*/ 
      .custom /*0C000239:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800014F*/ 
    .custom /*0C000252:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TupleElementNamesAttribute/*0100002E*/::.ctor(string[]) /* 0A00001E */ = ( 01 00 02 00 00 00 05 46 69 72 73 74 06 53 65 63   // .......First.Sec
                                                                                                                                                                                       6F 6E 64 00 00 )                                  // ond..
    .custom /*0C000253:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 01 00 00 ) 
    // Method begins at RVA 0x22c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Zip

  .method /*060000B1*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<valuetype [System.Runtime/*23000001*/]System.ValueTuple`3/*01000030*/<!!TFirst,!!TSecond,!!TThird>> 
          Zip<TFirst,TSecond,TThird>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TFirst> source1,
                                     class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TSecond> source2,
                                     class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TThird> source3) cil managed
  // SIG: 10 03 03 15 12 80 85 01 15 11 80 C1 03 1E 00 1E 01 1E 02 15 12 80 85 01 1E 00 15 12 75 01 1E 01
  //      15 12 75 01 1E 02
  {
    .custom /*0C000213:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000214:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TFirst /*2A0000C9*/ 
      .custom /*0C00023B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TSecond /*2A0000CA*/ 
      .custom /*0C00023D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TThird /*2A0000CB*/ 
      .custom /*0C00023F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000152*/ 
    .custom /*0C000254:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TupleElementNamesAttribute/*0100002E*/::.ctor(string[]) /* 0A00001E */ = ( 01 00 03 00 00 00 05 46 69 72 73 74 06 53 65 63   // .......First.Sec
                                                                                                                                                                                       6F 6E 64 05 54 68 69 72 64 00 00 )                // ond.Third..
    .custom /*0C000255:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 05 00 00 00 01 00 01 01 01 00 00 ) 
    // Method begins at RVA 0x22c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Zip

  .method /*060000B2*/ public hidebysig static 
          class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TResult> 
          Zip<TFirst,TSecond,TResult>(class [System.Linq.Expressions/*23000002*/]System.Linq.IQueryable`1/*01000021*/<!!TFirst> source1,
                                      class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<!!TSecond> source2,
                                      class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression`1/*01000024*/<class [System.Runtime/*23000001*/]System.Func`3/*01000025*/<!!TFirst,!!TSecond,!!TResult>> resultSelector) cil managed
  // SIG: 10 03 03 15 12 80 85 01 1E 02 15 12 80 85 01 1E 00 15 12 75 01 1E 01 15 12 80 91 01 15 12 80 95
  //      03 1E 00 1E 01 1E 02
  {
    .custom /*0C000216:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000217:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000019*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 A7 45 6E 75 6D 65 72 61 74 69 6E 67 20   // ....Enumerating 
                                                                                                                                                                                       63 6F 6C 6C 65 63 74 69 6F 6E 73 20 61 73 20 49   // collections as I
                                                                                                                                                                                       51 75 65 72 79 61 62 6C 65 20 63 61 6E 20 72 65   // Queryable can re
                                                                                                                                                                                       71 75 69 72 65 20 63 72 65 61 74 69 6E 67 20 6E   // quire creating n
                                                                                                                                                                                       65 77 20 67 65 6E 65 72 69 63 20 74 79 70 65 73   // ew generic types
                                                                                                                                                                                       20 6F 72 20 6D 65 74 68 6F 64 73 2C 20 77 68 69   //  or methods, whi
                                                                                                                                                                                       63 68 20 72 65 71 75 69 72 65 73 20 63 72 65 61   // ch requires crea
                                                                                                                                                                                       74 69 6E 67 20 63 6F 64 65 20 61 74 20 72 75 6E   // ting code at run
                                                                                                                                                                                       74 69 6D 65 2E 20 54 68 69 73 20 6D 61 79 20 6E   // time. This may n
                                                                                                                                                                                       6F 74 20 77 6F 72 6B 20 77 68 65 6E 20 41 4F 54   // ot work when AOT
                                                                                                                                                                                       20 63 6F 6D 70 69 6C 69 6E 67 2E 00 00 )          //  compiling...
    .param type TFirst /*2A0000CC*/ 
      .custom /*0C000241:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TSecond /*2A0000CD*/ 
      .custom /*0C000243:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A0000CE*/ 
      .custom /*0C000245:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Queryable::Zip

} // end of class System.Linq.Queryable


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
