


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
// Time-date stamp:            0xacacee8b
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00003600
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00005492
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
// Checksum:                       0x000099ca
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000543f [0x0000004f] address [size] of Import Directory:          
// 0x00006000 [0x00000474] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00004000 [0x00002480] address [size] of Security Directory:        
// 0x00008000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000535c [0x00000054] address [size] of Debug Directory:           
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
//              0x00003498 Virtual Size
//              0x00002000 Virtual Address
//              0x00003600 Size of Raw Data
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
//              0x00000474 Virtual Size
//              0x00006000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00003800 Pointer to Raw Data
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
//              0x00003e00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00005000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000494
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00005481 Import Name Table
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
// 0x00002234 [0x000030a8] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x000052dc [0x00000080] address [size] of Strong Name Signature:     
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
//              0x0000199c Size
//              '#~' Name
//    Stream 2:
//              0x00001a08 Offset
//              0x00000d08 Size
//              '#Strings' Name
//    Stream 3:
//              0x00002710 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00002714 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00002724 Offset
//              0x00000984 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000160909a01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 25728
// PE header size       : 512 (496 used)    ( 1.99%)
// PE additional info   : 10739             (41.74%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.28%)
// CLR meta-data size  : 12456              (48.41%)
// CLR additional info : 128                ( 0.50%)
// CLR method headers  : 141                ( 0.55%)
// Managed code         : 342               ( 1.33%)
// Data                 : 2048              ( 7.96%)
// Unaccounted          : -710              (-2.76%)

// Num.of PE sections   : 3
//   .text    - 13824
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 12456
//   Module        -    1 (10 bytes)
//   TypeDef       -   15 (210 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   39 (234 bytes)
//   MethodDef     -  141 (1974 bytes)    0 abstract, 0 native, 141 bodies
//   FieldDef      -   71 (426 bytes)     13 constant
//   MemberRef     -   22 (132 bytes)
//   ParamDef      -  307 (1842 bytes)
//   Constant      -  138 (828 bytes)
//   CustomAttribute-   84 (504 bytes)
//   InterfaceImpl -    2 (8 bytes)
//   PropertyMap   -    4 (16 bytes)
//   Property      -    8 (48 bytes)
//   MethodSemantic-   10 (60 bytes)
//   TypeSpec      -    1 (2 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    3 (60 bytes)
//   NestedClass   -    1 (4 bytes)
//   GenericParam  -    6 (48 bytes)
//   GenericParamConstraint-    6 (24 bytes)
//   Strings       -  3335 bytes
//   Blobs         -  2436 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   213 bytes

// CLR additional info : 128

// CLR method headers : 141
//   Num.of method bodies  - 141
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 141

// Managed code : 342
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class Asn1Tag                        (public) (sequential) (ansi) (sealed) 
// Class AsnContentException            (public) (auto) (ansi) 
// Class AsnDecoder                     (public) (abstract) (auto) (ansi) (sealed) 
// Class AsnEncodingRules               (public) (auto) (ansi) (sealed) 
// Class AsnReader                      (public) (auto) (ansi) 
// Class AsnReaderOptions               (public) (sequential) (ansi) (sealed) 
// Class AsnWriter                      (public) (auto) (ansi) (sealed) 
// Class TagClass                       (public) (auto) (ansi) (sealed) 
// Class UniversalTagNumber             (public) (auto) (ansi) (sealed) 
// Class Scope                          (sequential) (ansi) (sealed) (nested public) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ System.Runtime.Numerics
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ System.Collections
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly /*20000001*/ noplatform System.Formats.Asn1
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000002:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000001*/::.ctor(int32) /* 0A000001 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000003:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000002*/::.ctor() /* 0A000002 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000004:0A000003*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*//DebuggingModes/*01000004*/) /* 0A000003 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000005:0A000004*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C000006:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000007:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000008:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 13 53 79 73 74 65 6D 2E 46 6F 72 6D 61 74   // ...System.Format
                                                                                                                                                                        73 2E 41 73 6E 31 00 00 )                         // s.Asn1..
  .custom /*0C000009:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000A:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000E:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000F:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 13 53 79 73 74 65 6D 2E 46 6F 72 6D 61 74   // ...System.Format
                                                                                                                                                                       73 2E 41 73 6E 31 00 00 )                         // s.Asn1..
  .custom /*0C000010:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 36 2E 30 2E 32 31 2E 35 32 32 31 30 00   // ...6.0.21.52210.
                                                                                                                                                                       00 ) 
  .custom /*0C000011:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 36 2E 30 2E 30 2B 34 38 32 32 65 33 63   // ...6.0.0+4822e3c
                                                                                                                                                                                33 61 61 37 37 65 62 38 32 62 32 66 62 33 33 63   // 3aa77eb82b2fb33c
                                                                                                                                                                                39 33 32 31 66 39 32 33 63 66 31 31 64 64 64 65   // 9321f923cf11ddde
                                                                                                                                                                                36 00 00 )                                        // 6..
  .custom /*0C000012:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 13 53 79 73 74 65 6D 2E 46 6F 72 6D 61 74   // ...System.Format
                                                                                                                                                                 73 2E 41 73 6E 31 00 00 )                         // s.Asn1..
  .custom /*0C000014:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
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
.module System.Formats.Asn1.dll
// MVID: {9745eb49-5328-426b-9a24-6928b914780c}
.custom /*0C000001:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 00 00 00 ) 
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
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000014 */
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
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000014 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)000027       */ newarr     [System.Runtime/*23000001*/]System.Byte/*01000027*/
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
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000014 */
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
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000014 */
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
    IL_0001:  /* 28   | (0A)000014       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000014 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ public sequential ansi sealed beforefieldinit System.Formats.Asn1.Asn1Tag
       extends [System.Runtime/*23000001*/]System.ValueType/*01000016*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000017*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/>/*1B000001*/
{
  .custom /*0C000025:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000004*/ private initonly int32 _dummyPrimitive
  .field /*04000005*/ public static initonly valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ Boolean
  .field /*04000006*/ public static initonly valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ ConstructedBitString
  .field /*04000007*/ public static initonly valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ ConstructedOctetString
  .field /*04000008*/ public static initonly valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ Enumerated
  .field /*04000009*/ public static initonly valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ GeneralizedTime
  .field /*0400000A*/ public static initonly valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ Integer
  .field /*0400000B*/ public static initonly valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ Null
  .field /*0400000C*/ public static initonly valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ ObjectIdentifier
  .field /*0400000D*/ public static initonly valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ PrimitiveBitString
  .field /*0400000E*/ public static initonly valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ PrimitiveOctetString
  .field /*0400000F*/ public static initonly valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ Sequence
  .field /*04000010*/ public static initonly valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ SetOf
  .field /*04000011*/ public static initonly valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ UtcTime
  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Formats.Asn1.TagClass/*0200000D*/ tagClass,
                               int32 tagValue,
                               [opt] bool isConstructed) cil managed
  // SIG: 20 03 01 11 34 08 02
  {
    .param [3]/*08000003*/  = bool(false)
    // Method begins at RVA 0x209d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Asn1Tag::.ctor

  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ universalTagNumber,
                               [opt] bool isConstructed) cil managed
  // SIG: 20 02 01 11 38 02
  {
    .param [2]/*08000005*/  = bool(false)
    // Method begins at RVA 0x20a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Asn1Tag::.ctor

  .method /*06000008*/ public hidebysig specialname 
          instance bool  get_IsConstructed() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Asn1Tag::get_IsConstructed

  .method /*06000009*/ public hidebysig specialname 
          instance valuetype System.Formats.Asn1.TagClass/*0200000D*/ 
          get_TagClass() cil managed
  // SIG: 20 00 11 34
  {
    // Method begins at RVA 0x20a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Asn1Tag::get_TagClass

  .method /*0600000A*/ public hidebysig specialname 
          instance int32  get_TagValue() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Asn1Tag::get_TagValue

  .method /*0600000B*/ public hidebysig instance valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ 
          AsConstructed() cil managed
  // SIG: 20 00 11 18
  {
    // Method begins at RVA 0x20ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Asn1Tag::AsConstructed

  .method /*0600000C*/ public hidebysig instance valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ 
          AsPrimitive() cil managed
  // SIG: 20 00 11 18
  {
    // Method begins at RVA 0x20af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Asn1Tag::AsPrimitive

  .method /*0600000D*/ public hidebysig instance int32 
          CalculateEncodedSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Asn1Tag::CalculateEncodedSize

  .method /*0600000E*/ public hidebysig static 
          valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ 
          Decode(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                 [out] int32& bytesConsumed) cil managed
  // SIG: 00 02 11 18 15 11 61 01 05 10 08
  {
    // Method begins at RVA 0x20b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Asn1Tag::Decode

  .method /*0600000F*/ public hidebysig instance int32 
          Encode(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000019*/<uint8> destination) cil managed
  // SIG: 20 01 08 15 11 65 01 05
  {
    // Method begins at RVA 0x20b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Asn1Tag::Encode

  .method /*06000010*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ other) cil managed
  // SIG: 20 01 02 11 18
  {
    // Method begins at RVA 0x20bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Asn1Tag::Equals

  .method /*06000011*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00002B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800000A*/ 
    .custom /*0C000029:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001A*/::.ctor(bool) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Asn1Tag::Equals

  .method /*06000012*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Asn1Tag::GetHashCode

  .method /*06000013*/ public hidebysig instance bool 
          HasSameClassAndValue(valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ other) cil managed
  // SIG: 20 01 02 11 18
  {
    // Method begins at RVA 0x20c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Asn1Tag::HasSameClassAndValue

  .method /*06000014*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ left,
                            valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ right) cil managed
  // SIG: 00 02 02 11 18 11 18
  {
    // Method begins at RVA 0x20c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Asn1Tag::op_Equality

  .method /*06000015*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ left,
                              valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ right) cil managed
  // SIG: 00 02 02 11 18 11 18
  {
    // Method begins at RVA 0x20ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Asn1Tag::op_Inequality

  .method /*06000016*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00002C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Asn1Tag::ToString

  .method /*06000017*/ public hidebysig static 
          bool  TryDecode(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                          [out] valuetype System.Formats.Asn1.Asn1Tag/*02000006*/& tag,
                          [out] int32& bytesConsumed) cil managed
  // SIG: 00 03 02 15 11 61 01 05 10 11 18 10 08
  {
    // Method begins at RVA 0x20d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Asn1Tag::TryDecode

  .method /*06000018*/ public hidebysig instance bool 
          TryEncode(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000019*/<uint8> destination,
                    [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 65 01 05 10 08
  {
    // Method begins at RVA 0x20d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Asn1Tag::TryEncode

  .property /*17000001*/ instance callconv(8) bool
          IsConstructed()
  {
    .get instance bool System.Formats.Asn1.Asn1Tag/*02000006*/::get_IsConstructed() /* 06000008 */
  } // end of property Asn1Tag::IsConstructed
  .property /*17000002*/ instance callconv(8) valuetype System.Formats.Asn1.TagClass/*0200000D*/
          TagClass()
  {
    .get instance valuetype System.Formats.Asn1.TagClass/*0200000D*/ System.Formats.Asn1.Asn1Tag/*02000006*/::get_TagClass() /* 06000009 */
  } // end of property Asn1Tag::TagClass
  .property /*17000003*/ instance callconv(8) int32
          TagValue()
  {
    .get instance int32 System.Formats.Asn1.Asn1Tag/*02000006*/::get_TagValue() /* 0600000A */
  } // end of property Asn1Tag::TagValue
} // end of class System.Formats.Asn1.Asn1Tag

.class /*02000007*/ public auto ansi beforefieldinit System.Formats.Asn1.AsnContentException
       extends [System.Runtime/*23000001*/]System.Exception/*0100001C*/
{
  .custom /*0C000027:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000028:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000019*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20d6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*0100001C*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AsnContentException::.ctor

  .method /*0600001A*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001D*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001E*/ context) cil managed
  // SIG: 20 02 01 12 75 11 79
  {
    .custom /*0C00002D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20de
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*0100001C*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AsnContentException::.ctor

  .method /*0600001B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20e6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*0100001C*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AsnContentException::.ctor

  .method /*0600001C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*0100001C*/ inner) cil managed
  // SIG: 20 02 01 0E 12 71
  {
    // Method begins at RVA 0x20ee
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*0100001C*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AsnContentException::.ctor

} // end of class System.Formats.Asn1.AsnContentException

.class /*02000008*/ public abstract auto ansi sealed beforefieldinit System.Formats.Asn1.AsnDecoder
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
{
  .method /*0600001D*/ public hidebysig static 
          uint8[]  ReadBitString(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                                 valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                                 [out] int32& unusedBitCount,
                                 [out] int32& bytesConsumed,
                                 [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 05 1D 05 15 11 61 01 05 11 24 10 08 10 08 15 11 7D 01 11 18
  {
    .param [0]/*0800001A*/ 
    .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [5]/*0800001F*/  = nullref
    // Method begins at RVA 0x20f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::ReadBitString

  .method /*0600001E*/ public hidebysig static 
          bool  ReadBoolean(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                            valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                            [out] int32& bytesConsumed,
                            [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 04 02 15 11 61 01 05 11 24 10 08 15 11 7D 01 11 18
  {
    .param [4]/*08000023*/  = nullref
    // Method begins at RVA 0x20f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::ReadBoolean

  .method /*0600001F*/ public hidebysig static 
          string  ReadCharacterString(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                                      valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                                      valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ encodingType,
                                      [out] int32& bytesConsumed,
                                      [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 05 0E 15 11 61 01 05 11 24 11 38 10 08 15 11 7D 01 11 18
  {
    .param [0]/*08000024*/ 
    .custom /*0C000030:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [5]/*08000029*/  = nullref
    // Method begins at RVA 0x20fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::ReadCharacterString

  .method /*06000020*/ public hidebysig static 
          valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ 
          ReadEncodedValue(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                           valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                           [out] int32& contentOffset,
                           [out] int32& contentLength,
                           [out] int32& bytesConsumed) cil managed
  // SIG: 00 05 11 18 15 11 61 01 05 11 24 10 08 10 08 10 08
  {
    // Method begins at RVA 0x20ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::ReadEncodedValue

  .method /*06000021*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> 
          ReadEnumeratedBytes(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                              valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                              [out] int32& bytesConsumed,
                              [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 04 15 11 61 01 05 15 11 61 01 05 11 24 10 08 15 11 7D 01 11 18
  {
    .param [4]/*08000032*/  = nullref
    // Method begins at RVA 0x2102
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::ReadEnumeratedBytes

  .method /*06000022*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Enum/*01000020*/ 
          ReadEnumeratedValue(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                              valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                              class [System.Runtime/*23000001*/]System.Type/*01000021*/ enumType,
                              [out] int32& bytesConsumed,
                              [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 05 12 80 81 15 11 61 01 05 11 24 12 80 85 10 08 15 11 7D 01 11 18
  {
    .custom /*0C00002F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000033*/ 
    .custom /*0C000032:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [5]/*08000037*/  = nullref
    // Method begins at RVA 0x2105
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::ReadEnumeratedValue

  .method /*06000023*/ public hidebysig static 
          !!TEnum  ReadEnumeratedValue<([System.Runtime/*23000001*/]System.Enum/*01000020*/) TEnum>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                                                                                                    valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                                                                                                    [out] int32& bytesConsumed,
                                                                                                    [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 10 01 04 1E 00 15 11 61 01 05 11 24 10 08 15 11 7D 01 11 18
  {
    .param constraint   TEnum/*2A000001*/ , [System.Runtime/*23000001*/]System.Enum/*01000020*/
      .custom /*0C000015:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [0]/*08000038*/ 
    .custom /*0C000033:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [4]/*0800003C*/  = nullref
    // Method begins at RVA 0x2108
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::ReadEnumeratedValue

  .method /*06000024*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000022*/ 
          ReadGeneralizedTime(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                              valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                              [out] int32& bytesConsumed,
                              [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 04 11 80 89 15 11 61 01 05 11 24 10 08 15 11 7D 01 11 18
  {
    .param [4]/*08000040*/  = nullref
    // Method begins at RVA 0x210b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::ReadGeneralizedTime

  .method /*06000025*/ public hidebysig static 
          valuetype [System.Runtime.Numerics/*23000002*/]System.Numerics.BigInteger/*01000023*/ 
          ReadInteger(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                      valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                      [out] int32& bytesConsumed,
                      [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 04 11 80 8D 15 11 61 01 05 11 24 10 08 15 11 7D 01 11 18
  {
    .param [4]/*08000044*/  = nullref
    // Method begins at RVA 0x210e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::ReadInteger

  .method /*06000026*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> 
          ReadIntegerBytes(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                           valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                           [out] int32& bytesConsumed,
                           [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 04 15 11 61 01 05 15 11 61 01 05 11 24 10 08 15 11 7D 01 11 18
  {
    .param [4]/*08000048*/  = nullref
    // Method begins at RVA 0x2111
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::ReadIntegerBytes

  .method /*06000027*/ public hidebysig static 
          class [System.Collections/*23000003*/]System.Collections.BitArray/*01000024*/ 
          ReadNamedBitList(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                           valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                           [out] int32& bytesConsumed,
                           [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 04 12 80 91 15 11 61 01 05 11 24 10 08 15 11 7D 01 11 18
  {
    .param [0]/*08000049*/ 
    .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [4]/*0800004D*/  = nullref
    // Method begins at RVA 0x2114
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::ReadNamedBitList

  .method /*06000028*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Enum/*01000020*/ 
          ReadNamedBitListValue(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                                valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                                class [System.Runtime/*23000001*/]System.Type/*01000021*/ flagsEnumType,
                                [out] int32& bytesConsumed,
                                [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 05 12 80 81 15 11 61 01 05 11 24 12 80 85 10 08 15 11 7D 01 11 18
  {
    .custom /*0C000031:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800004E*/ 
    .custom /*0C00003E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [5]/*08000052*/  = nullref
    // Method begins at RVA 0x2117
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::ReadNamedBitListValue

  .method /*06000029*/ public hidebysig static 
          !!TFlagsEnum  ReadNamedBitListValue<([System.Runtime/*23000001*/]System.Enum/*01000020*/) TFlagsEnum>(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                                                                                                                valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                                                                                                                [out] int32& bytesConsumed,
                                                                                                                [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 10 01 04 1E 00 15 11 61 01 05 11 24 10 08 15 11 7D 01 11 18
  {
    .param constraint   TFlagsEnum/*2A000002*/ , [System.Runtime/*23000001*/]System.Enum/*01000020*/
      .custom /*0C000018:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [0]/*08000053*/ 
    .custom /*0C000043:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000057*/  = nullref
    // Method begins at RVA 0x211a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::ReadNamedBitListValue

  .method /*0600002A*/ public hidebysig static 
          void  ReadNull(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                         valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                         [out] int32& bytesConsumed,
                         [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 04 01 15 11 61 01 05 11 24 10 08 15 11 7D 01 11 18
  {
    .param [4]/*0800005B*/  = nullref
    // Method begins at RVA 0x211d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::ReadNull

  .method /*0600002B*/ public hidebysig static 
          string  ReadObjectIdentifier(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                                       valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                                       [out] int32& bytesConsumed,
                                       [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 04 0E 15 11 61 01 05 11 24 10 08 15 11 7D 01 11 18
  {
    .param [0]/*0800005C*/ 
    .custom /*0C000045:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000060*/  = nullref
    // Method begins at RVA 0x2120
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::ReadObjectIdentifier

  .method /*0600002C*/ public hidebysig static 
          uint8[]  ReadOctetString(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                                   valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                                   [out] int32& bytesConsumed,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 04 1D 05 15 11 61 01 05 11 24 10 08 15 11 7D 01 11 18
  {
    .param [0]/*08000061*/ 
    .custom /*0C000047:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000065*/  = nullref
    // Method begins at RVA 0x2123
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::ReadOctetString

  .method /*0600002D*/ public hidebysig static 
          void  ReadSequence(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                             valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                             [out] int32& contentOffset,
                             [out] int32& contentLength,
                             [out] int32& bytesConsumed,
                             [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 06 01 15 11 61 01 05 11 24 10 08 10 08 10 08 15 11 7D 01 11 18
  {
    .param [6]/*0800006B*/  = nullref
    // Method begins at RVA 0x2126
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::ReadSequence

  .method /*0600002E*/ public hidebysig static 
          void  ReadSetOf(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                          valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                          [out] int32& contentOffset,
                          [out] int32& contentLength,
                          [out] int32& bytesConsumed,
                          [opt] bool skipSortOrderValidation,
                          [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 07 01 15 11 61 01 05 11 24 10 08 10 08 10 08 02 15 11 7D 01 11 18
  {
    .param [6]/*08000071*/  = bool(false)
    .param [7]/*08000072*/  = nullref
    // Method begins at RVA 0x2129
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::ReadSetOf

  .method /*0600002F*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000022*/ 
          ReadUtcTime(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                      valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                      [out] int32& bytesConsumed,
                      [opt] int32 twoDigitYearMax,
                      [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 05 11 80 89 15 11 61 01 05 11 24 10 08 08 15 11 7D 01 11 18
  {
    .param [4]/*08000076*/  = int32(0x00000801)
    .param [5]/*08000077*/  = nullref
    // Method begins at RVA 0x212c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::ReadUtcTime

  .method /*06000030*/ public hidebysig static 
          bool  TryReadBitString(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                                 valuetype [System.Runtime/*23000001*/]System.Span`1/*01000019*/<uint8> destination,
                                 valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                                 [out] int32& unusedBitCount,
                                 [out] int32& bytesConsumed,
                                 [out] int32& bytesWritten,
                                 [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 07 02 15 11 61 01 05 15 11 65 01 05 11 24 10 08 10 08 10 08 15 11 7D 01 11 18
  {
    .param [7]/*0800007E*/  = nullref
    // Method begins at RVA 0x212f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::TryReadBitString

  .method /*06000031*/ public hidebysig static 
          bool  TryReadCharacterString(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                                       valuetype [System.Runtime/*23000001*/]System.Span`1/*01000019*/<char> destination,
                                       valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                                       valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ encodingType,
                                       [out] int32& bytesConsumed,
                                       [out] int32& charsWritten,
                                       [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 07 02 15 11 61 01 05 15 11 65 01 03 11 24 11 38 10 08 10 08 15 11 7D 01 11 18
  {
    .param [7]/*08000085*/  = nullref
    // Method begins at RVA 0x2132
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::TryReadCharacterString

  .method /*06000032*/ public hidebysig static 
          bool  TryReadCharacterStringBytes(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                                            valuetype [System.Runtime/*23000001*/]System.Span`1/*01000019*/<uint8> destination,
                                            valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                                            valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ expectedTag,
                                            [out] int32& bytesConsumed,
                                            [out] int32& bytesWritten) cil managed
  // SIG: 00 06 02 15 11 61 01 05 15 11 65 01 05 11 24 11 18 10 08 10 08
  {
    // Method begins at RVA 0x2135
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::TryReadCharacterStringBytes

  .method /*06000033*/ public hidebysig static 
          bool  TryReadEncodedValue(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                                    valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                                    [out] valuetype System.Formats.Asn1.Asn1Tag/*02000006*/& tag,
                                    [out] int32& contentOffset,
                                    [out] int32& contentLength,
                                    [out] int32& bytesConsumed) cil managed
  // SIG: 00 06 02 15 11 61 01 05 11 24 10 11 18 10 08 10 08 10 08
  {
    // Method begins at RVA 0x2138
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::TryReadEncodedValue

  .method /*06000034*/ public hidebysig static 
          bool  TryReadInt32(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                             valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                             [out] int32& 'value',
                             [out] int32& bytesConsumed,
                             [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 05 02 15 11 61 01 05 11 24 10 08 10 08 15 11 7D 01 11 18
  {
    .param [5]/*08000096*/  = nullref
    // Method begins at RVA 0x213b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::TryReadInt32

  .method /*06000035*/ public hidebysig static 
          bool  TryReadInt64(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                             valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                             [out] int64& 'value',
                             [out] int32& bytesConsumed,
                             [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 05 02 15 11 61 01 05 11 24 10 0A 10 08 15 11 7D 01 11 18
  {
    .param [5]/*0800009B*/  = nullref
    // Method begins at RVA 0x213e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::TryReadInt64

  .method /*06000036*/ public hidebysig static 
          bool  TryReadOctetString(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                                   valuetype [System.Runtime/*23000001*/]System.Span`1/*01000019*/<uint8> destination,
                                   valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                                   [out] int32& bytesConsumed,
                                   [out] int32& bytesWritten,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 06 02 15 11 61 01 05 15 11 65 01 05 11 24 10 08 10 08 15 11 7D 01 11 18
  {
    .param [6]/*080000A1*/  = nullref
    // Method begins at RVA 0x2141
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::TryReadOctetString

  .method /*06000037*/ public hidebysig static 
          bool  TryReadPrimitiveBitString(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                                          valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                                          [out] int32& unusedBitCount,
                                          [out] valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8>& 'value',
                                          [out] int32& bytesConsumed,
                                          [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 06 02 15 11 61 01 05 11 24 10 08 10 15 11 61 01 05 10 08 15 11 7D 01 11 18
  {
    .param [6]/*080000A7*/  = nullref
    // Method begins at RVA 0x2144
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::TryReadPrimitiveBitString

  .method /*06000038*/ public hidebysig static 
          bool  TryReadPrimitiveCharacterStringBytes(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                                                     valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                                                     valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ expectedTag,
                                                     [out] valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8>& 'value',
                                                     [out] int32& bytesConsumed) cil managed
  // SIG: 00 05 02 15 11 61 01 05 11 24 11 18 10 15 11 61 01 05 10 08
  {
    // Method begins at RVA 0x2147
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::TryReadPrimitiveCharacterStringBytes

  .method /*06000039*/ public hidebysig static 
          bool  TryReadPrimitiveOctetString(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                                            valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                                            [out] valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8>& 'value',
                                            [out] int32& bytesConsumed,
                                            [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 05 02 15 11 61 01 05 11 24 10 15 11 61 01 05 10 08 15 11 7D 01 11 18
  {
    .param [5]/*080000B1*/  = nullref
    // Method begins at RVA 0x214a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::TryReadPrimitiveOctetString

  .method /*0600003A*/ public hidebysig static 
          bool  TryReadUInt32(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                              valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                              [out] uint32& 'value',
                              [out] int32& bytesConsumed,
                              [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 05 02 15 11 61 01 05 11 24 10 09 10 08 15 11 7D 01 11 18
  {
    .custom /*0C000034:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    .param [5]/*080000B6*/  = nullref
    // Method begins at RVA 0x214d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::TryReadUInt32

  .method /*0600003B*/ public hidebysig static 
          bool  TryReadUInt64(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> source,
                              valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                              [out] uint64& 'value',
                              [out] int32& bytesConsumed,
                              [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 00 05 02 15 11 61 01 05 11 24 10 0B 10 08 15 11 7D 01 11 18
  {
    .custom /*0C000035:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    .param [5]/*080000BB*/  = nullref
    // Method begins at RVA 0x2150
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnDecoder::TryReadUInt64

} // end of class System.Formats.Asn1.AsnDecoder

.class /*02000009*/ public auto ansi sealed System.Formats.Asn1.AsnEncodingRules
       extends [System.Runtime/*23000001*/]System.Enum/*01000020*/
{
  .field /*04000012*/ public specialname rtspecialname int32 value__
  .field /*04000013*/ public static literal valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ BER = int32(0x00000000)
  .field /*04000014*/ public static literal valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ CER = int32(0x00000001)
  .field /*04000015*/ public static literal valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ DER = int32(0x00000002)
} // end of class System.Formats.Asn1.AsnEncodingRules

.class /*0200000A*/ public auto ansi beforefieldinit System.Formats.Asn1.AsnReader
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
{
  .method /*0600003C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000025*/<uint8> data,
                               valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet,
                               [opt] valuetype System.Formats.Asn1.AsnReaderOptions/*0200000B*/ options) cil managed
  // SIG: 20 03 01 15 11 80 95 01 05 11 24 11 2C
  {
    .param [3]/*080000BE*/  = nullref
    // Method begins at RVA 0x2153
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001B*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AsnReader::.ctor

  .method /*0600003D*/ public hidebysig specialname 
          instance bool  get_HasData() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x215b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::get_HasData

  .method /*0600003E*/ public hidebysig specialname 
          instance valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ 
          get_RuleSet() cil managed
  // SIG: 20 00 11 24
  {
    // Method begins at RVA 0x215e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::get_RuleSet

  .method /*0600003F*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000025*/<uint8> 
          PeekContentBytes() cil managed
  // SIG: 20 00 15 11 80 95 01 05
  {
    // Method begins at RVA 0x2161
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::PeekContentBytes

  .method /*06000040*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000025*/<uint8> 
          PeekEncodedValue() cil managed
  // SIG: 20 00 15 11 80 95 01 05
  {
    // Method begins at RVA 0x2164
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::PeekEncodedValue

  .method /*06000041*/ public hidebysig instance valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ 
          PeekTag() cil managed
  // SIG: 20 00 11 18
  {
    // Method begins at RVA 0x2167
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::PeekTag

  .method /*06000042*/ public hidebysig instance uint8[] 
          ReadBitString([out] int32& unusedBitCount,
                        [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 02 1D 05 10 08 15 11 7D 01 11 18
  {
    .custom /*0C000036:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000C0*/  = nullref
    // Method begins at RVA 0x216a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::ReadBitString

  .method /*06000043*/ public hidebysig instance bool 
          ReadBoolean([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 01 02 15 11 7D 01 11 18
  {
    .param [1]/*080000C1*/  = nullref
    // Method begins at RVA 0x216d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::ReadBoolean

  .method /*06000044*/ public hidebysig instance string 
          ReadCharacterString(valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ encodingType,
                              [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 02 0E 11 38 15 11 7D 01 11 18
  {
    .custom /*0C000037:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000C3*/  = nullref
    // Method begins at RVA 0x2170
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::ReadCharacterString

  .method /*06000045*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000025*/<uint8> 
          ReadEncodedValue() cil managed
  // SIG: 20 00 15 11 80 95 01 05
  {
    // Method begins at RVA 0x2173
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::ReadEncodedValue

  .method /*06000046*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000025*/<uint8> 
          ReadEnumeratedBytes([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 01 15 11 80 95 01 05 15 11 7D 01 11 18
  {
    .param [1]/*080000C4*/  = nullref
    // Method begins at RVA 0x2176
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::ReadEnumeratedBytes

  .method /*06000047*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Enum/*01000020*/ 
          ReadEnumeratedValue(class [System.Runtime/*23000001*/]System.Type/*01000021*/ enumType,
                              [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 02 12 80 81 12 80 85 15 11 7D 01 11 18
  {
    .custom /*0C000038:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000C6*/  = nullref
    // Method begins at RVA 0x2179
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::ReadEnumeratedValue

  .method /*06000048*/ public hidebysig instance !!TEnum 
          ReadEnumeratedValue<([System.Runtime/*23000001*/]System.Enum/*01000020*/) TEnum>([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 30 01 01 1E 00 15 11 7D 01 11 18
  {
    .custom /*0C000039:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param type TEnum /*2A000003*/ 
      .custom /*0C00001C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [1]/*080000C7*/  = nullref
    // Method begins at RVA 0x217c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::ReadEnumeratedValue

  .method /*06000049*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000022*/ 
          ReadGeneralizedTime([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 01 11 80 89 15 11 7D 01 11 18
  {
    .param [1]/*080000C8*/  = nullref
    // Method begins at RVA 0x217f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::ReadGeneralizedTime

  .method /*0600004A*/ public hidebysig instance valuetype [System.Runtime.Numerics/*23000002*/]System.Numerics.BigInteger/*01000023*/ 
          ReadInteger([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 01 11 80 8D 15 11 7D 01 11 18
  {
    .param [1]/*080000C9*/  = nullref
    // Method begins at RVA 0x2182
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::ReadInteger

  .method /*0600004B*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000025*/<uint8> 
          ReadIntegerBytes([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 01 15 11 80 95 01 05 15 11 7D 01 11 18
  {
    .param [1]/*080000CA*/  = nullref
    // Method begins at RVA 0x2185
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::ReadIntegerBytes

  .method /*0600004C*/ public hidebysig instance class [System.Collections/*23000003*/]System.Collections.BitArray/*01000024*/ 
          ReadNamedBitList([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 01 12 80 91 15 11 7D 01 11 18
  {
    .custom /*0C00003B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [1]/*080000CB*/  = nullref
    // Method begins at RVA 0x2188
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::ReadNamedBitList

  .method /*0600004D*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Enum/*01000020*/ 
          ReadNamedBitListValue(class [System.Runtime/*23000001*/]System.Type/*01000021*/ flagsEnumType,
                                [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 02 12 80 81 12 80 85 15 11 7D 01 11 18
  {
    .custom /*0C00003C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000CD*/  = nullref
    // Method begins at RVA 0x218b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::ReadNamedBitListValue

  .method /*0600004E*/ public hidebysig instance !!TFlagsEnum 
          ReadNamedBitListValue<([System.Runtime/*23000001*/]System.Enum/*01000020*/) TFlagsEnum>([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 30 01 01 1E 00 15 11 7D 01 11 18
  {
    .custom /*0C00003D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param type TFlagsEnum /*2A000004*/ 
      .custom /*0C000020:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [1]/*080000CE*/  = nullref
    // Method begins at RVA 0x218e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::ReadNamedBitListValue

  .method /*0600004F*/ public hidebysig instance void 
          ReadNull([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 01 01 15 11 7D 01 11 18
  {
    .param [1]/*080000CF*/  = nullref
    // Method begins at RVA 0x2191
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnReader::ReadNull

  .method /*06000050*/ public hidebysig instance string 
          ReadObjectIdentifier([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 01 0E 15 11 7D 01 11 18
  {
    .custom /*0C00003F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [1]/*080000D0*/  = nullref
    // Method begins at RVA 0x2193
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::ReadObjectIdentifier

  .method /*06000051*/ public hidebysig instance uint8[] 
          ReadOctetString([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 01 1D 05 15 11 7D 01 11 18
  {
    .custom /*0C000040:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [1]/*080000D1*/  = nullref
    // Method begins at RVA 0x2196
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::ReadOctetString

  .method /*06000052*/ public hidebysig instance class System.Formats.Asn1.AsnReader/*0200000A*/ 
          ReadSequence([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 01 12 28 15 11 7D 01 11 18
  {
    .custom /*0C000041:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [1]/*080000D2*/  = nullref
    // Method begins at RVA 0x2199
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::ReadSequence

  .method /*06000053*/ public hidebysig instance class System.Formats.Asn1.AsnReader/*0200000A*/ 
          ReadSetOf(bool skipSortOrderValidation,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 02 12 28 02 15 11 7D 01 11 18
  {
    .custom /*0C000042:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000D4*/  = nullref
    // Method begins at RVA 0x219c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::ReadSetOf

  .method /*06000054*/ public hidebysig instance class System.Formats.Asn1.AsnReader/*0200000A*/ 
          ReadSetOf([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 01 12 28 15 11 7D 01 11 18
  {
    .custom /*0C000044:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [1]/*080000D5*/  = nullref
    // Method begins at RVA 0x219f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::ReadSetOf

  .method /*06000055*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000022*/ 
          ReadUtcTime(int32 twoDigitYearMax,
                      [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 02 11 80 89 08 15 11 7D 01 11 18
  {
    .param [2]/*080000D7*/  = nullref
    // Method begins at RVA 0x21a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::ReadUtcTime

  .method /*06000056*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000022*/ 
          ReadUtcTime([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 01 11 80 89 15 11 7D 01 11 18
  {
    .param [1]/*080000D8*/  = nullref
    // Method begins at RVA 0x21a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::ReadUtcTime

  .method /*06000057*/ public hidebysig instance void 
          ThrowIfNotEmpty() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21a8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnReader::ThrowIfNotEmpty

  .method /*06000058*/ public hidebysig instance bool 
          TryReadBitString(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000019*/<uint8> destination,
                           [out] int32& unusedBitCount,
                           [out] int32& bytesWritten,
                           [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 04 02 15 11 65 01 05 10 08 10 08 15 11 7D 01 11 18
  {
    .param [4]/*080000DC*/  = nullref
    // Method begins at RVA 0x21aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::TryReadBitString

  .method /*06000059*/ public hidebysig instance bool 
          TryReadCharacterString(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000019*/<char> destination,
                                 valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ encodingType,
                                 [out] int32& charsWritten,
                                 [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 04 02 15 11 65 01 03 11 38 10 08 15 11 7D 01 11 18
  {
    .param [4]/*080000E0*/  = nullref
    // Method begins at RVA 0x21ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::TryReadCharacterString

  .method /*0600005A*/ public hidebysig instance bool 
          TryReadCharacterStringBytes(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000019*/<uint8> destination,
                                      valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ expectedTag,
                                      [out] int32& bytesWritten) cil managed
  // SIG: 20 03 02 15 11 65 01 05 11 18 10 08
  {
    // Method begins at RVA 0x21b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::TryReadCharacterStringBytes

  .method /*0600005B*/ public hidebysig instance bool 
          TryReadInt32([out] int32& 'value',
                       [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 02 02 10 08 15 11 7D 01 11 18
  {
    .param [2]/*080000E5*/  = nullref
    // Method begins at RVA 0x21b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::TryReadInt32

  .method /*0600005C*/ public hidebysig instance bool 
          TryReadInt64([out] int64& 'value',
                       [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 02 02 10 0A 15 11 7D 01 11 18
  {
    .param [2]/*080000E7*/  = nullref
    // Method begins at RVA 0x21b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::TryReadInt64

  .method /*0600005D*/ public hidebysig instance bool 
          TryReadOctetString(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000019*/<uint8> destination,
                             [out] int32& bytesWritten,
                             [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 03 02 15 11 65 01 05 10 08 15 11 7D 01 11 18
  {
    .param [3]/*080000EA*/  = nullref
    // Method begins at RVA 0x21b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::TryReadOctetString

  .method /*0600005E*/ public hidebysig instance bool 
          TryReadPrimitiveBitString([out] int32& unusedBitCount,
                                    [out] valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000025*/<uint8>& 'value',
                                    [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 03 02 10 08 10 15 11 80 95 01 05 15 11 7D 01 11 18
  {
    .param [3]/*080000ED*/  = nullref
    // Method begins at RVA 0x21bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::TryReadPrimitiveBitString

  .method /*0600005F*/ public hidebysig instance bool 
          TryReadPrimitiveCharacterStringBytes(valuetype System.Formats.Asn1.Asn1Tag/*02000006*/ expectedTag,
                                               [out] valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000025*/<uint8>& contents) cil managed
  // SIG: 20 02 02 11 18 10 15 11 80 95 01 05
  {
    // Method begins at RVA 0x21bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::TryReadPrimitiveCharacterStringBytes

  .method /*06000060*/ public hidebysig instance bool 
          TryReadPrimitiveOctetString([out] valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000025*/<uint8>& contents,
                                      [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 02 02 10 15 11 80 95 01 05 15 11 7D 01 11 18
  {
    .param [2]/*080000F1*/  = nullref
    // Method begins at RVA 0x21c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::TryReadPrimitiveOctetString

  .method /*06000061*/ public hidebysig instance bool 
          TryReadUInt32([out] uint32& 'value',
                        [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 02 02 10 09 15 11 7D 01 11 18
  {
    .custom /*0C000046:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    .param [2]/*080000F3*/  = nullref
    // Method begins at RVA 0x21c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::TryReadUInt32

  .method /*06000062*/ public hidebysig instance bool 
          TryReadUInt64([out] uint64& 'value',
                        [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> expectedTag) cil managed
  // SIG: 20 02 02 10 0B 15 11 7D 01 11 18
  {
    .custom /*0C000048:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    .param [2]/*080000F5*/  = nullref
    // Method begins at RVA 0x21c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReader::TryReadUInt64

  .property /*17000004*/ instance callconv(8) bool
          HasData()
  {
    .get instance bool System.Formats.Asn1.AsnReader/*0200000A*/::get_HasData() /* 0600003D */
  } // end of property AsnReader::HasData
  .property /*17000005*/ instance callconv(8) valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/
          RuleSet()
  {
    .get instance valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ System.Formats.Asn1.AsnReader/*0200000A*/::get_RuleSet() /* 0600003E */
  } // end of property AsnReader::RuleSet
} // end of class System.Formats.Asn1.AsnReader

.class /*0200000B*/ public sequential ansi sealed beforefieldinit System.Formats.Asn1.AsnReaderOptions
       extends [System.Runtime/*23000001*/]System.ValueType/*01000016*/
{
  .field /*04000016*/ private int32 _dummyPrimitive
  .method /*06000063*/ public hidebysig specialname 
          instance bool  get_SkipSetSortOrderVerification() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000049:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReaderOptions::get_SkipSetSortOrderVerification

  .method /*06000064*/ public hidebysig specialname 
          instance void  set_SkipSetSortOrderVerification(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x21ce
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnReaderOptions::set_SkipSetSortOrderVerification

  .method /*06000065*/ public hidebysig specialname 
          instance int32  get_UtcTimeTwoDigitYearMax() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnReaderOptions::get_UtcTimeTwoDigitYearMax

  .method /*06000066*/ public hidebysig specialname 
          instance void  set_UtcTimeTwoDigitYearMax(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x21d3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnReaderOptions::set_UtcTimeTwoDigitYearMax

  .property /*17000006*/ instance callconv(8) bool
          SkipSetSortOrderVerification()
  {
    .get instance bool System.Formats.Asn1.AsnReaderOptions/*0200000B*/::get_SkipSetSortOrderVerification() /* 06000063 */
    .set instance void System.Formats.Asn1.AsnReaderOptions/*0200000B*/::set_SkipSetSortOrderVerification(bool) /* 06000064 */
  } // end of property AsnReaderOptions::SkipSetSortOrderVerification
  .property /*17000007*/ instance callconv(8) int32
          UtcTimeTwoDigitYearMax()
  {
    .get instance int32 System.Formats.Asn1.AsnReaderOptions/*0200000B*/::get_UtcTimeTwoDigitYearMax() /* 06000065 */
    .set instance void System.Formats.Asn1.AsnReaderOptions/*0200000B*/::set_UtcTimeTwoDigitYearMax(int32) /* 06000066 */
  } // end of property AsnReaderOptions::UtcTimeTwoDigitYearMax
} // end of class System.Formats.Asn1.AsnReaderOptions

.class /*0200000C*/ public auto ansi sealed beforefieldinit System.Formats.Asn1.AsnWriter
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
{
  .class /*0200000F*/ sequential ansi sealed nested public beforefieldinit Scope
         extends [System.Runtime/*23000001*/]System.ValueType/*01000016*/
         implements [System.Runtime/*23000001*/]System.IDisposable/*01000026*/
  {
    .custom /*0C00002A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000046*/ private initonly object _dummy
    .field /*04000047*/ private initonly int32 _dummyPrimitive
    .method /*0600008D*/ public hidebysig newslot virtual final 
            instance void  Dispose() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x2231
      // Code size       1 (0x1)
      .maxstack  8
      IL_0000:  /* 2A   |                  */ ret
    } // end of method Scope::Dispose

  } // end of class Scope

  .method /*06000067*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ ruleSet) cil managed
  // SIG: 20 01 01 11 24
  {
    // Method begins at RVA 0x21d5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001B*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AsnWriter::.ctor

  .method /*06000068*/ public hidebysig specialname 
          instance valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ 
          get_RuleSet() cil managed
  // SIG: 20 00 11 24
  {
    // Method begins at RVA 0x21dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnWriter::get_RuleSet

  .method /*06000069*/ public hidebysig instance void 
          CopyTo(class System.Formats.Asn1.AsnWriter/*0200000C*/ destination) cil managed
  // SIG: 20 01 01 12 30
  {
    .custom /*0C00004A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21e0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::CopyTo

  .method /*0600006A*/ public hidebysig instance uint8[] 
          Encode() cil managed
  // SIG: 20 00 1D 05
  {
    .custom /*0C00004B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnWriter::Encode

  .method /*0600006B*/ public hidebysig instance int32 
          Encode(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000019*/<uint8> destination) cil managed
  // SIG: 20 01 08 15 11 65 01 05
  {
    // Method begins at RVA 0x21e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnWriter::Encode

  .method /*0600006C*/ public hidebysig instance bool 
          EncodedValueEquals(class System.Formats.Asn1.AsnWriter/*0200000C*/ other) cil managed
  // SIG: 20 01 02 12 30
  {
    .custom /*0C00004C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnWriter::EncodedValueEquals

  .method /*0600006D*/ public hidebysig instance bool 
          EncodedValueEquals(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> other) cil managed
  // SIG: 20 01 02 15 11 61 01 05
  {
    // Method begins at RVA 0x21eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnWriter::EncodedValueEquals

  .method /*0600006E*/ public hidebysig instance int32 
          GetEncodedLength() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnWriter::GetEncodedLength

  .method /*0600006F*/ public hidebysig instance void 
          PopOctetString([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 01 01 15 11 7D 01 11 18
  {
    .param [1]/*080000FD*/  = nullref
    // Method begins at RVA 0x21f1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::PopOctetString

  .method /*06000070*/ public hidebysig instance void 
          PopSequence([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 01 01 15 11 7D 01 11 18
  {
    .param [1]/*080000FE*/  = nullref
    // Method begins at RVA 0x21f3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::PopSequence

  .method /*06000071*/ public hidebysig instance void 
          PopSetOf([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 01 01 15 11 7D 01 11 18
  {
    .param [1]/*080000FF*/  = nullref
    // Method begins at RVA 0x21f5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::PopSetOf

  .method /*06000072*/ public hidebysig instance valuetype System.Formats.Asn1.AsnWriter/*0200000C*//Scope/*0200000F*/ 
          PushOctetString([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 01 11 3C 15 11 7D 01 11 18
  {
    .param [1]/*08000100*/  = nullref
    // Method begins at RVA 0x21f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnWriter::PushOctetString

  .method /*06000073*/ public hidebysig instance valuetype System.Formats.Asn1.AsnWriter/*0200000C*//Scope/*0200000F*/ 
          PushSequence([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 01 11 3C 15 11 7D 01 11 18
  {
    .param [1]/*08000101*/  = nullref
    // Method begins at RVA 0x21fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnWriter::PushSequence

  .method /*06000074*/ public hidebysig instance valuetype System.Formats.Asn1.AsnWriter/*0200000C*//Scope/*0200000F*/ 
          PushSetOf([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 01 11 3C 15 11 7D 01 11 18
  {
    .param [1]/*08000102*/  = nullref
    // Method begins at RVA 0x21fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnWriter::PushSetOf

  .method /*06000075*/ public hidebysig instance void 
          Reset() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2200
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::Reset

  .method /*06000076*/ public hidebysig instance bool 
          TryEncode(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000019*/<uint8> destination,
                    [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 65 01 05 10 08
  {
    // Method begins at RVA 0x2202
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsnWriter::TryEncode

  .method /*06000077*/ public hidebysig instance void 
          WriteBitString(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> 'value',
                         [opt] int32 unusedBitCount,
                         [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 03 01 15 11 61 01 05 08 15 11 7D 01 11 18
  {
    .param [2]/*08000106*/  = int32(0x00000000)
    .param [3]/*08000107*/  = nullref
    // Method begins at RVA 0x2205
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteBitString

  .method /*06000078*/ public hidebysig instance void 
          WriteBoolean(bool 'value',
                       [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 02 01 02 15 11 7D 01 11 18
  {
    .param [2]/*08000109*/  = nullref
    // Method begins at RVA 0x2207
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteBoolean

  .method /*06000079*/ public hidebysig instance void 
          WriteCharacterString(valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ encodingType,
                               valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<char> str,
                               [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 03 01 11 38 15 11 61 01 03 15 11 7D 01 11 18
  {
    .param [3]/*0800010C*/  = nullref
    // Method begins at RVA 0x2209
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteCharacterString

  .method /*0600007A*/ public hidebysig instance void 
          WriteCharacterString(valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ encodingType,
                               string 'value',
                               [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 03 01 11 38 0E 15 11 7D 01 11 18
  {
    .custom /*0C00004D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [3]/*0800010F*/  = nullref
    // Method begins at RVA 0x220b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteCharacterString

  .method /*0600007B*/ public hidebysig instance void 
          WriteEncodedValue(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> 'value') cil managed
  // SIG: 20 01 01 15 11 61 01 05
  {
    // Method begins at RVA 0x220d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteEncodedValue

  .method /*0600007C*/ public hidebysig instance void 
          WriteEnumeratedValue(class [System.Runtime/*23000001*/]System.Enum/*01000020*/ 'value',
                               [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 02 01 12 80 81 15 11 7D 01 11 18
  {
    .custom /*0C00004E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000112*/  = nullref
    // Method begins at RVA 0x220f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteEnumeratedValue

  .method /*0600007D*/ public hidebysig instance void 
          WriteEnumeratedValue<([System.Runtime/*23000001*/]System.Enum/*01000020*/) TEnum>(!!TEnum 'value',
                                                                                            [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 30 01 02 01 1E 00 15 11 7D 01 11 18
  {
    .custom /*0C00004F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param type TEnum /*2A000005*/ 
      .custom /*0C000024:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [2]/*08000114*/  = nullref
    // Method begins at RVA 0x2211
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteEnumeratedValue

  .method /*0600007E*/ public hidebysig instance void 
          WriteGeneralizedTime(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000022*/ 'value',
                               [opt] bool omitFractionalSeconds,
                               [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 03 01 11 80 89 02 15 11 7D 01 11 18
  {
    .param [2]/*08000116*/  = bool(false)
    .param [3]/*08000117*/  = nullref
    // Method begins at RVA 0x2213
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteGeneralizedTime

  .method /*0600007F*/ public hidebysig instance void 
          WriteInteger(int64 'value',
                       [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 02 01 0A 15 11 7D 01 11 18
  {
    .param [2]/*08000119*/  = nullref
    // Method begins at RVA 0x2215
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteInteger

  .method /*06000080*/ public hidebysig instance void 
          WriteInteger(valuetype [System.Runtime.Numerics/*23000002*/]System.Numerics.BigInteger/*01000023*/ 'value',
                       [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 02 01 11 80 8D 15 11 7D 01 11 18
  {
    .param [2]/*0800011B*/  = nullref
    // Method begins at RVA 0x2217
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteInteger

  .method /*06000081*/ public hidebysig instance void 
          WriteInteger(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> 'value',
                       [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 02 01 15 11 61 01 05 15 11 7D 01 11 18
  {
    .param [2]/*0800011D*/  = nullref
    // Method begins at RVA 0x2219
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteInteger

  .method /*06000082*/ public hidebysig instance void 
          WriteInteger(uint64 'value',
                       [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 02 01 0B 15 11 7D 01 11 18
  {
    .custom /*0C000050:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    .param [2]/*0800011F*/  = nullref
    // Method begins at RVA 0x221b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteInteger

  .method /*06000083*/ public hidebysig instance void 
          WriteIntegerUnsigned(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> 'value',
                               [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 02 01 15 11 61 01 05 15 11 7D 01 11 18
  {
    .param [2]/*08000121*/  = nullref
    // Method begins at RVA 0x221d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteIntegerUnsigned

  .method /*06000084*/ public hidebysig instance void 
          WriteNamedBitList(class [System.Collections/*23000003*/]System.Collections.BitArray/*01000024*/ 'value',
                            [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 02 01 12 80 91 15 11 7D 01 11 18
  {
    .custom /*0C000051:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000123*/  = nullref
    // Method begins at RVA 0x221f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteNamedBitList

  .method /*06000085*/ public hidebysig instance void 
          WriteNamedBitList(class [System.Runtime/*23000001*/]System.Enum/*01000020*/ 'value',
                            [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 02 01 12 80 81 15 11 7D 01 11 18
  {
    .custom /*0C000052:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000125*/  = nullref
    // Method begins at RVA 0x2221
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteNamedBitList

  .method /*06000086*/ public hidebysig instance void 
          WriteNamedBitList<([System.Runtime/*23000001*/]System.Enum/*01000020*/) TEnum>(!!TEnum 'value',
                                                                                         [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 30 01 02 01 1E 00 15 11 7D 01 11 18
  {
    .custom /*0C000053:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param type TEnum /*2A000006*/ 
      .custom /*0C000026:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [2]/*08000127*/  = nullref
    // Method begins at RVA 0x2223
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteNamedBitList

  .method /*06000087*/ public hidebysig instance void 
          WriteNull([opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 01 01 15 11 7D 01 11 18
  {
    .param [1]/*08000128*/  = nullref
    // Method begins at RVA 0x2225
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteNull

  .method /*06000088*/ public hidebysig instance void 
          WriteObjectIdentifier(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<char> oidValue,
                                [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 02 01 15 11 61 01 03 15 11 7D 01 11 18
  {
    .param [2]/*0800012A*/  = nullref
    // Method begins at RVA 0x2227
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteObjectIdentifier

  .method /*06000089*/ public hidebysig instance void 
          WriteObjectIdentifier(string oidValue,
                                [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 02 01 0E 15 11 7D 01 11 18
  {
    .custom /*0C000054:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800012C*/  = nullref
    // Method begins at RVA 0x2229
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteObjectIdentifier

  .method /*0600008A*/ public hidebysig instance void 
          WriteOctetString(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000018*/<uint8> 'value',
                           [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 02 01 15 11 61 01 05 15 11 7D 01 11 18
  {
    .param [2]/*0800012E*/  = nullref
    // Method begins at RVA 0x222b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteOctetString

  .method /*0600008B*/ public hidebysig instance void 
          WriteUtcTime(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000022*/ 'value',
                       int32 twoDigitYearMax,
                       [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 03 01 11 80 89 08 15 11 7D 01 11 18
  {
    .param [3]/*08000131*/  = nullref
    // Method begins at RVA 0x222d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteUtcTime

  .method /*0600008C*/ public hidebysig instance void 
          WriteUtcTime(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000022*/ 'value',
                       [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype System.Formats.Asn1.Asn1Tag/*02000006*/> tag) cil managed
  // SIG: 20 02 01 11 80 89 15 11 7D 01 11 18
  {
    .param [2]/*08000133*/  = nullref
    // Method begins at RVA 0x222f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AsnWriter::WriteUtcTime

  .property /*17000008*/ instance callconv(8) valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/
          RuleSet()
  {
    .get instance valuetype System.Formats.Asn1.AsnEncodingRules/*02000009*/ System.Formats.Asn1.AsnWriter/*0200000C*/::get_RuleSet() /* 06000068 */
  } // end of property AsnWriter::RuleSet
} // end of class System.Formats.Asn1.AsnWriter

.class /*0200000D*/ public auto ansi sealed System.Formats.Asn1.TagClass
       extends [System.Runtime/*23000001*/]System.Enum/*01000020*/
{
  .field /*04000017*/ public specialname rtspecialname int32 value__
  .field /*04000018*/ public static literal valuetype System.Formats.Asn1.TagClass/*0200000D*/ Universal = int32(0x00000000)
  .field /*04000019*/ public static literal valuetype System.Formats.Asn1.TagClass/*0200000D*/ Application = int32(0x00000040)
  .field /*0400001A*/ public static literal valuetype System.Formats.Asn1.TagClass/*0200000D*/ ContextSpecific = int32(0x00000080)
  .field /*0400001B*/ public static literal valuetype System.Formats.Asn1.TagClass/*0200000D*/ Private = int32(0x000000C0)
} // end of class System.Formats.Asn1.TagClass

.class /*0200000E*/ public auto ansi sealed System.Formats.Asn1.UniversalTagNumber
       extends [System.Runtime/*23000001*/]System.Enum/*01000020*/
{
  .field /*0400001C*/ public specialname rtspecialname int32 value__
  .field /*0400001D*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ EndOfContents = int32(0x00000000)
  .field /*0400001E*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ Boolean = int32(0x00000001)
  .field /*0400001F*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ Integer = int32(0x00000002)
  .field /*04000020*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ BitString = int32(0x00000003)
  .field /*04000021*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ OctetString = int32(0x00000004)
  .field /*04000022*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ Null = int32(0x00000005)
  .field /*04000023*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ ObjectIdentifier = int32(0x00000006)
  .field /*04000024*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ ObjectDescriptor = int32(0x00000007)
  .field /*04000025*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ External = int32(0x00000008)
  .field /*04000026*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ InstanceOf = int32(0x00000008)
  .field /*04000027*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ Real = int32(0x00000009)
  .field /*04000028*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ Enumerated = int32(0x0000000A)
  .field /*04000029*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ Embedded = int32(0x0000000B)
  .field /*0400002A*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ UTF8String = int32(0x0000000C)
  .field /*0400002B*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ RelativeObjectIdentifier = int32(0x0000000D)
  .field /*0400002C*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ Time = int32(0x0000000E)
  .field /*0400002D*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ Sequence = int32(0x00000010)
  .field /*0400002E*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ SequenceOf = int32(0x00000010)
  .field /*0400002F*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ Set = int32(0x00000011)
  .field /*04000030*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ SetOf = int32(0x00000011)
  .field /*04000031*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ NumericString = int32(0x00000012)
  .field /*04000032*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ PrintableString = int32(0x00000013)
  .field /*04000033*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ T61String = int32(0x00000014)
  .field /*04000034*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ TeletexString = int32(0x00000014)
  .field /*04000035*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ VideotexString = int32(0x00000015)
  .field /*04000036*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ IA5String = int32(0x00000016)
  .field /*04000037*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ UtcTime = int32(0x00000017)
  .field /*04000038*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ GeneralizedTime = int32(0x00000018)
  .field /*04000039*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ GraphicString = int32(0x00000019)
  .field /*0400003A*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ ISO646String = int32(0x0000001A)
  .field /*0400003B*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ VisibleString = int32(0x0000001A)
  .field /*0400003C*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ GeneralString = int32(0x0000001B)
  .field /*0400003D*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ UniversalString = int32(0x0000001C)
  .field /*0400003E*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ UnrestrictedCharacterString = int32(0x0000001D)
  .field /*0400003F*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ BMPString = int32(0x0000001E)
  .field /*04000040*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ Date = int32(0x0000001F)
  .field /*04000041*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ TimeOfDay = int32(0x00000020)
  .field /*04000042*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ DateTime = int32(0x00000021)
  .field /*04000043*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ Duration = int32(0x00000022)
  .field /*04000044*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ ObjectIdentifierIRI = int32(0x00000023)
  .field /*04000045*/ public static literal valuetype System.Formats.Asn1.UniversalTagNumber/*0200000E*/ RelativeObjectIdentifierIRI = int32(0x00000024)
} // end of class System.Formats.Asn1.UniversalTagNumber


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
