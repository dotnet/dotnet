


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
// Time-date stamp:            0xb7f934f8
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00005600
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x000075ee
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
// Checksum:                       0x00012166
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00007599 [0x0000004f] address [size] of Import Directory:          
// 0x00008000 [0x00000484] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00006000 [0x00002880] address [size] of Security Directory:        
// 0x0000a000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x000074b4 [0x00000054] address [size] of Debug Directory:           
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
//              0x000055f4 Virtual Size
//              0x00002000 Virtual Address
//              0x00005600 Size of Raw Data
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
//              0x00005800 Pointer to Raw Data
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
//              0x00005e00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00007000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x000005f0
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x000075db Import Name Table
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
// 0x00002530 [0x00004f04] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00007434 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00003054 Size
//              '#~' Name
//    Stream 2:
//              0x000030c0 Offset
//              0x000011f4 Size
//              '#Strings' Name
//    Stream 3:
//              0x000042b4 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x000042b8 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x000042c8 Offset
//              0x00000c3c Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000014090bb41f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 34944
// PE header size       : 512 (496 used)    ( 1.47%)
// PE additional info   : 11779             (33.71%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.21%)
// CLR meta-data size  : 20228              (57.89%)
// CLR additional info : 128                ( 0.37%)
// CLR method headers  : 364                ( 1.04%)
// Managed code         : 884               ( 2.53%)
// Data                 : 2048              ( 5.86%)
// Unaccounted          : -1071             (-3.06%)

// Num.of PE sections   : 3
//   .text    - 22016
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 20228
//   Module        -    1 (10 bytes)
//   TypeDef       -   30 (420 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   65 (390 bytes)
//   MethodDef     -  367 (5138 bytes)    3 abstract, 0 native, 364 bodies
//   FieldDef      -   25 (150 bytes)     4 constant
//   MemberRef     -   39 (234 bytes)
//   ParamDef      -  400 (2400 bytes)
//   MethodImpl    -   11 (66 bytes)
//   Constant      -   13 (78 bytes)
//   CustomAttribute-  371 (2226 bytes)
//   InterfaceImpl -    8 (32 bytes)
//   PropertyMap   -   16 (64 bytes)
//   Property      -   57 (342 bytes)
//   MethodSemantic-   77 (462 bytes)
//   TypeSpec      -    4 (8 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    3 (60 bytes)
//   EventMap      -    1 (4 bytes)
//   Event         -    2 (12 bytes)
//   GenericParam  -   13 (104 bytes)
//   GenericParamConstraint-   10 (40 bytes)
//   Strings       -  4593 bytes
//   Blobs         -  3132 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   221 bytes

// CLR additional info : 128

// CLR method headers : 364
//   Num.of method bodies  - 364
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 364

// Managed code : 884
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class RefSafetyRulesAttribute        (auto) (ansi) (sealed) 
// Class Extensions                     (public) (abstract) (auto) (ansi) (sealed) 
// Class Extensions                     (public) (abstract) (auto) (ansi) (sealed) 
// Class LoadOptions                    (public) (auto) (ansi) (sealed) 
// Class ReaderOptions                  (public) (auto) (ansi) (sealed) 
// Class SaveOptions                    (public) (auto) (ansi) (sealed) 
// Class XAttribute                     (public) (auto) (ansi) 
// Class XCData                         (public) (auto) (ansi) 
// Class XComment                       (public) (auto) (ansi) 
// Class XContainer                     (public) (abstract) (auto) (ansi) 
// Class XDeclaration                   (public) (auto) (ansi) 
// Class XDocument                      (public) (auto) (ansi) 
// Class XDocumentType                  (public) (auto) (ansi) 
// Class XElement                       (public) (auto) (ansi) 
// Class XName                          (public) (auto) (ansi) (sealed) 
// Class XNamespace                     (public) (auto) (ansi) (sealed) 
// Class XNode                          (public) (abstract) (auto) (ansi) 
// Class XNodeDocumentOrderComparer     (public) (auto) (ansi) (sealed) 
// Class XNodeEqualityComparer          (public) (auto) (ansi) (sealed) 
// Class XObject                        (public) (abstract) (auto) (ansi) 
// Class XObjectChange                  (public) (auto) (ansi) (sealed) 
// Class XObjectChangeEventArgs         (public) (auto) (ansi) 
// Class XProcessingInstruction         (public) (auto) (ansi) 
// Class XStreamingElement              (public) (auto) (ansi) 
// Class XText                          (public) (auto) (ansi) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000002*/ System.Xml.ReaderWriter
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000003*/ System.ObjectModel
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly /*20000001*/ noplatform System.Xml.XDocument
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
  .custom /*0C00000E:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 14 53 79 73 74 65 6D 2E 58 6D 6C 2E 58 44   // ...System.Xml.XD
                                                                                                                                                                        6F 63 75 6D 65 6E 74 00 00 )                      // ocument..
  .custom /*0C00000F:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000010:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000011:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000012:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 14 53 79 73 74 65 6D 2E 58 6D 6C 2E 58 44   // ...System.Xml.XD
                                                                                                                                                                       6F 63 75 6D 65 6E 74 00 00 )                      // ocument..
  .custom /*0C000013:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 37 2E 30 2E 32 32 2E 35 31 38 30 35 00   // ...7.0.22.51805.
                                                                                                                                                                       00 ) 
  .custom /*0C000014:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 37 2E 30 2E 30 2B 64 30 39 39 66 30 37   // ...7.0.0+d099f07
                                                                                                                                                                                35 65 34 35 64 32 61 61 36 30 30 37 61 32 32 62   // 5e45d2aa6007a22b
                                                                                                                                                                                37 31 62 34 35 61 30 38 37 35 38 35 35 39 66 38   // 71b45a08758559f8
                                                                                                                                                                                30 00 00 )                                        // 0..
  .custom /*0C000015:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000016:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 14 53 79 73 74 65 6D 2E 58 6D 6C 2E 58 44   // ...System.Xml.XD
                                                                                                                                                                 6F 63 75 6D 65 6E 74 00 00 )                      // ocument..
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
.module System.Xml.XDocument.dll
// MVID: {fa6e8519-778b-45a7-b876-2721eb2f1577}
.custom /*0C000003:06000006*/ instance void System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::.ctor(int32) /* 06000006 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000004:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 00 00 00 ) 
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
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000025 */
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
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000025 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)000041       */ newarr     [System.Runtime/*23000001*/]System.Byte/*01000041*/
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
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000025 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000023:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000024:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000025:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000025 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000029:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C00002A:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00002B:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000025 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.RefSafetyRulesAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C00002D:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C00002E:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00002F:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000025 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000004       */ stfld      int32 System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::Version /* 04000004 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method RefSafetyRulesAttribute::.ctor

} // end of class System.Runtime.CompilerServices.RefSafetyRulesAttribute

.class /*02000007*/ public abstract auto ansi sealed beforefieldinit System.Xml.Schema.Extensions
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000034:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000035:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000036:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000007*/ public hidebysig static 
          class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.IXmlSchemaInfo/*01000017*/ 
          GetSchemaInfo(class System.Xml.Linq.XAttribute/*0200000C*/ source) cil managed
  // SIG: 00 01 12 5D 12 30
  {
    .custom /*0C000033:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000001*/ 
    .custom /*0C000001:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Extensions::GetSchemaInfo

  .method /*06000008*/ public hidebysig static 
          class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.IXmlSchemaInfo/*01000017*/ 
          GetSchemaInfo(class System.Xml.Linq.XElement/*02000013*/ source) cil managed
  // SIG: 00 01 12 5D 12 4C
  {
    .custom /*0C000038:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000003*/ 
    .custom /*0C000020:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Extensions::GetSchemaInfo

  .method /*06000009*/ public hidebysig static 
          void  Validate(class System.Xml.Linq.XAttribute/*0200000C*/ source,
                         class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaObject/*01000018*/ partialValidationType,
                         class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaSet/*01000019*/ schemas,
                         class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.ValidationEventHandler/*0100001A*/ validationEventHandler) cil managed
  // SIG: 00 04 01 12 30 12 61 12 65 12 69
  {
    .custom /*0C000040:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [4]/*08000008*/ 
    .custom /*0C00003C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20b2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Extensions::Validate

  .method /*0600000A*/ public hidebysig static 
          void  Validate(class System.Xml.Linq.XAttribute/*0200000C*/ source,
                         class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaObject/*01000018*/ partialValidationType,
                         class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaSet/*01000019*/ schemas,
                         class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.ValidationEventHandler/*0100001A*/ validationEventHandler,
                         bool addSchemaInfo) cil managed
  // SIG: 00 05 01 12 30 12 61 12 65 12 69 02
  {
    .custom /*0C000043:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [4]/*0800000C*/ 
    .custom /*0C00004D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20b4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Extensions::Validate

  .method /*0600000B*/ public hidebysig static 
          void  Validate(class System.Xml.Linq.XDocument/*02000011*/ source,
                         class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaSet/*01000019*/ schemas,
                         class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.ValidationEventHandler/*0100001A*/ validationEventHandler) cil managed
  // SIG: 00 03 01 12 44 12 65 12 69
  {
    .custom /*0C000046:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [3]/*08000010*/ 
    .custom /*0C00005B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20b6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Extensions::Validate

  .method /*0600000C*/ public hidebysig static 
          void  Validate(class System.Xml.Linq.XDocument/*02000011*/ source,
                         class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaSet/*01000019*/ schemas,
                         class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.ValidationEventHandler/*0100001A*/ validationEventHandler,
                         bool addSchemaInfo) cil managed
  // SIG: 00 04 01 12 44 12 65 12 69 02
  {
    .custom /*0C000049:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [3]/*08000013*/ 
    .custom /*0C000069:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20b8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Extensions::Validate

  .method /*0600000D*/ public hidebysig static 
          void  Validate(class System.Xml.Linq.XElement/*02000013*/ source,
                         class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaObject/*01000018*/ partialValidationType,
                         class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaSet/*01000019*/ schemas,
                         class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.ValidationEventHandler/*0100001A*/ validationEventHandler) cil managed
  // SIG: 00 04 01 12 4C 12 61 12 65 12 69
  {
    .custom /*0C00004F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [4]/*08000018*/ 
    .custom /*0C000077:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ba
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Extensions::Validate

  .method /*0600000E*/ public hidebysig static 
          void  Validate(class System.Xml.Linq.XElement/*02000013*/ source,
                         class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaObject/*01000018*/ partialValidationType,
                         class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchemaSet/*01000019*/ schemas,
                         class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.ValidationEventHandler/*0100001A*/ validationEventHandler,
                         bool addSchemaInfo) cil managed
  // SIG: 00 05 01 12 4C 12 61 12 65 12 69 02
  {
    .custom /*0C000052:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [4]/*0800001C*/ 
    .custom /*0C000083:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20bc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Extensions::Validate

} // end of class System.Xml.Schema.Extensions

.class /*02000008*/ public abstract auto ansi sealed beforefieldinit System.Xml.Linq.Extensions
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000039:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00003B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*0600000F*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          AncestorsAndSelf(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> source) cil managed
  // SIG: 00 01 15 12 6D 01 12 4C 15 12 6D 01 12 4C
  {
    .custom /*0C000055:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800001E*/ 
    .custom /*0C00008A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x20be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Extensions::AncestorsAndSelf

  .method /*06000010*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          AncestorsAndSelf(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> source,
                           class System.Xml.Linq.XName/*02000014*/ name) cil managed
  // SIG: 00 02 15 12 6D 01 12 4C 15 12 6D 01 12 4C 12 50
  {
    .custom /*0C000058:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800001F*/ 
    .custom /*0C00008D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000020*/ 
    .custom /*0C000090:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Extensions::AncestorsAndSelf

  .method /*06000011*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          Ancestors<(System.Xml.Linq.XNode/*02000016*/) T>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!T> source) cil managed
  // SIG: 10 01 01 15 12 6D 01 12 4C 15 12 6D 01 1E 00
  {
    .custom /*0C00005D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000001*/ 
      .custom /*0C000018:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000021*/ 
    .custom /*0C000091:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x20c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Extensions::Ancestors

  .method /*06000012*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          Ancestors<(System.Xml.Linq.XNode/*02000016*/) T>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!T> source,
                                                           class System.Xml.Linq.XName/*02000014*/ name) cil managed
  // SIG: 10 01 02 15 12 6D 01 12 4C 15 12 6D 01 1E 00 12 50
  {
    .custom /*0C000061:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000002*/ 
      .custom /*0C00001C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000022*/ 
    .custom /*0C000092:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000023*/ 
    .custom /*0C000093:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Extensions::Ancestors

  .method /*06000013*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XAttribute/*0200000C*/> 
          Attributes(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> source) cil managed
  // SIG: 00 01 15 12 6D 01 12 30 15 12 6D 01 12 4C
  {
    .custom /*0C000064:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000024*/ 
    .custom /*0C000094:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x20ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Extensions::Attributes

  .method /*06000014*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XAttribute/*0200000C*/> 
          Attributes(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> source,
                     class System.Xml.Linq.XName/*02000014*/ name) cil managed
  // SIG: 00 02 15 12 6D 01 12 30 15 12 6D 01 12 4C 12 50
  {
    .custom /*0C00006B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000025*/ 
    .custom /*0C000095:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000026*/ 
    .custom /*0C000097:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Extensions::Attributes

  .method /*06000015*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XNode/*02000016*/> 
          DescendantNodesAndSelf(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> source) cil managed
  // SIG: 00 01 15 12 6D 01 12 58 15 12 6D 01 12 4C
  {
    .custom /*0C00006E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000027*/ 
    .custom /*0C000098:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x20d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Extensions::DescendantNodesAndSelf

  .method /*06000016*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XNode/*02000016*/> 
          DescendantNodes<(System.Xml.Linq.XContainer/*0200000F*/) T>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!T> source) cil managed
  // SIG: 10 01 01 15 12 6D 01 12 58 15 12 6D 01 1E 00
  {
    .custom /*0C000072:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000003*/ 
      .custom /*0C000022:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000028*/ 
    .custom /*0C00009A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x20d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Extensions::DescendantNodes

  .method /*06000017*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          DescendantsAndSelf(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> source) cil managed
  // SIG: 00 01 15 12 6D 01 12 4C 15 12 6D 01 12 4C
  {
    .custom /*0C000075:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000029*/ 
    .custom /*0C00009B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x20d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Extensions::DescendantsAndSelf

  .method /*06000018*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          DescendantsAndSelf(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> source,
                             class System.Xml.Linq.XName/*02000014*/ name) cil managed
  // SIG: 00 02 15 12 6D 01 12 4C 15 12 6D 01 12 4C 12 50
  {
    .custom /*0C000076:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800002A*/ 
    .custom /*0C00009C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*0800002B*/ 
    .custom /*0C00009E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Extensions::DescendantsAndSelf

  .method /*06000019*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          Descendants<(System.Xml.Linq.XContainer/*0200000F*/) T>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!T> source) cil managed
  // SIG: 10 01 01 15 12 6D 01 12 4C 15 12 6D 01 1E 00
  {
    .custom /*0C000078:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000004*/ 
      .custom /*0C000028:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800002C*/ 
    .custom /*0C0000A1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x20dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Extensions::Descendants

  .method /*0600001A*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          Descendants<(System.Xml.Linq.XContainer/*0200000F*/) T>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!T> source,
                                                                  class System.Xml.Linq.XName/*02000014*/ name) cil managed
  // SIG: 10 01 02 15 12 6D 01 12 4C 15 12 6D 01 1E 00 12 50
  {
    .custom /*0C00007B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000005*/ 
      .custom /*0C00002C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800002D*/ 
    .custom /*0C0000A4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*0800002E*/ 
    .custom /*0C0000A6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Extensions::Descendants

  .method /*0600001B*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          Elements<(System.Xml.Linq.XContainer/*0200000F*/) T>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!T> source) cil managed
  // SIG: 10 01 01 15 12 6D 01 12 4C 15 12 6D 01 1E 00
  {
    .custom /*0C00007D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000006*/ 
      .custom /*0C000032:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800002F*/ 
    .custom /*0C0000A9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x20e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Extensions::Elements

  .method /*0600001C*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          Elements<(System.Xml.Linq.XContainer/*0200000F*/) T>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!T> source,
                                                               class System.Xml.Linq.XName/*02000014*/ name) cil managed
  // SIG: 10 01 02 15 12 6D 01 12 4C 15 12 6D 01 1E 00 12 50
  {
    .custom /*0C000080:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000007*/ 
      .custom /*0C000037:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000030*/ 
    .custom /*0C0000AB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000031*/ 
    .custom /*0C0000AE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Extensions::Elements

  .method /*0600001D*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!T> 
          InDocumentOrder<(System.Xml.Linq.XNode/*02000016*/) T>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!T> source) cil managed
  // SIG: 10 01 01 15 12 6D 01 1E 00 15 12 6D 01 1E 00
  {
    .custom /*0C000084:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000008*/ 
      .custom /*0C00003E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param constraint   T/*2A000008*/ , System.Xml.Linq.XNode/*02000016*/
      .custom /*0C00003F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Extensions::InDocumentOrder

  .method /*0600001E*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XNode/*02000016*/> 
          Nodes<(System.Xml.Linq.XContainer/*0200000F*/) T>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!T> source) cil managed
  // SIG: 10 01 01 15 12 6D 01 12 58 15 12 6D 01 1E 00
  {
    .custom /*0C000087:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000009*/ 
      .custom /*0C000042:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000033*/ 
    .custom /*0C0000B2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x20eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Extensions::Nodes

  .method /*0600001F*/ public hidebysig static 
          void  Remove(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XAttribute/*0200000C*/> source) cil managed
  // SIG: 00 01 01 15 12 6D 01 12 30
  {
    .custom /*0C00008C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000034*/ 
    .custom /*0C0000B5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x20ee
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Extensions::Remove

  .method /*06000020*/ public hidebysig static 
          void  Remove<(System.Xml.Linq.XNode/*02000016*/) T>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!T> source) cil managed
  // SIG: 10 01 01 01 15 12 6D 01 1E 00
  {
    .custom /*0C00008E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .custom /*0C00008F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param constraint   T/*2A00000A*/ , System.Xml.Linq.XNode/*02000016*/
      .custom /*0C000045:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000035*/ 
    .custom /*0C0000B8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x20f0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Extensions::Remove

} // end of class System.Xml.Linq.Extensions

.class /*02000009*/ public auto ansi sealed System.Xml.Linq.LoadOptions
       extends [System.Runtime/*23000001*/]System.Enum/*0100001D*/
{
  .custom /*0C000041:0A000013*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100001C*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*04000005*/ public specialname rtspecialname int32 value__
  .field /*04000006*/ public static literal valuetype System.Xml.Linq.LoadOptions/*02000009*/ None = int32(0x00000000)
  .field /*04000007*/ public static literal valuetype System.Xml.Linq.LoadOptions/*02000009*/ PreserveWhitespace = int32(0x00000001)
  .field /*04000008*/ public static literal valuetype System.Xml.Linq.LoadOptions/*02000009*/ SetBaseUri = int32(0x00000002)
  .field /*04000009*/ public static literal valuetype System.Xml.Linq.LoadOptions/*02000009*/ SetLineInfo = int32(0x00000004)
} // end of class System.Xml.Linq.LoadOptions

.class /*0200000A*/ public auto ansi sealed System.Xml.Linq.ReaderOptions
       extends [System.Runtime/*23000001*/]System.Enum/*0100001D*/
{
  .custom /*0C000044:0A000013*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100001C*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*0400000A*/ public specialname rtspecialname int32 value__
  .field /*0400000B*/ public static literal valuetype System.Xml.Linq.ReaderOptions/*0200000A*/ None = int32(0x00000000)
  .field /*0400000C*/ public static literal valuetype System.Xml.Linq.ReaderOptions/*0200000A*/ OmitDuplicateNamespaces = int32(0x00000001)
} // end of class System.Xml.Linq.ReaderOptions

.class /*0200000B*/ public auto ansi sealed System.Xml.Linq.SaveOptions
       extends [System.Runtime/*23000001*/]System.Enum/*0100001D*/
{
  .custom /*0C000047:0A000013*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100001C*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*0400000D*/ public specialname rtspecialname int32 value__
  .field /*0400000E*/ public static literal valuetype System.Xml.Linq.SaveOptions/*0200000B*/ None = int32(0x00000000)
  .field /*0400000F*/ public static literal valuetype System.Xml.Linq.SaveOptions/*0200000B*/ DisableFormatting = int32(0x00000001)
  .field /*04000010*/ public static literal valuetype System.Xml.Linq.SaveOptions/*0200000B*/ OmitDuplicateNamespaces = int32(0x00000002)
} // end of class System.Xml.Linq.SaveOptions

.class /*0200000C*/ public auto ansi beforefieldinit System.Xml.Linq.XAttribute
       extends System.Xml.Linq.XObject/*02000019*/
{
  .custom /*0C00004A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00004C:0A000014*/ instance void [System.ObjectModel/*23000003*/]System.ComponentModel.TypeDescriptionProviderAttribute/*0100001E*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 D4 4D 53 2E 49 6E 74 65 72 6E 61 6C 2E   // ....MS.Internal.
                                                                                                                                                                                   58 6D 6C 2E 4C 69 6E 71 2E 43 6F 6D 70 6F 6E 65   // Xml.Linq.Compone
                                                                                                                                                                                   6E 74 4D 6F 64 65 6C 2E 58 54 79 70 65 44 65 73   // ntModel.XTypeDes
                                                                                                                                                                                   63 72 69 70 74 69 6F 6E 50 72 6F 76 69 64 65 72   // criptionProvider
                                                                                                                                                                                   60 31 5B 5B 53 79 73 74 65 6D 2E 58 6D 6C 2E 4C   // `1[[System.Xml.L
                                                                                                                                                                                   69 6E 71 2E 58 41 74 74 72 69 62 75 74 65 2C 20   // inq.XAttribute, 
                                                                                                                                                                                   53 79 73 74 65 6D 2E 58 6D 6C 2E 4C 69 6E 71 2C   // System.Xml.Linq,
                                                                                                                                                                                   20 56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30   //  Version=4.0.0.0
                                                                                                                                                                                   2C 20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61   // , Culture=neutra
                                                                                                                                                                                   6C 2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65   // l, PublicKeyToke
                                                                                                                                                                                   6E 3D 62 37 37 61 35 63 35 36 31 39 33 34 65 30   // n=b77a5c561934e0
                                                                                                                                                                                   38 39 5D 5D 2C 53 79 73 74 65 6D 2E 43 6F 6D 70   // 89]],System.Comp
                                                                                                                                                                                   6F 6E 65 6E 74 4D 6F 64 65 6C 2E 54 79 70 65 43   // onentModel.TypeC
                                                                                                                                                                                   6F 6E 76 65 72 74 65 72 00 00 )                   // onverter..
  .method /*06000021*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XAttribute/*0200000C*/ other) cil managed
  // SIG: 20 01 01 12 30
  {
    // Method begins at RVA 0x20f2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00013A       */ call       instance void System.Xml.Linq.XObject/*02000019*/::.ctor() /* 0600013A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XAttribute::.ctor

  .method /*06000022*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XName/*02000014*/ name,
                               object 'value') cil managed
  // SIG: 20 02 01 12 50 1C
  {
    // Method begins at RVA 0x20fa
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00013A       */ call       instance void System.Xml.Linq.XObject/*02000019*/::.ctor() /* 0600013A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XAttribute::.ctor

  .method /*06000023*/ public hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XAttribute/*0200000C*/> 
          get_EmptySequence() cil managed
  // SIG: 00 00 15 12 6D 01 12 30
  {
    // Method begins at RVA 0x2102
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::get_EmptySequence

  .method /*06000024*/ public hidebysig specialname 
          instance bool  get_IsNamespaceDeclaration() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2105
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::get_IsNamespaceDeclaration

  .method /*06000025*/ public hidebysig specialname 
          instance class System.Xml.Linq.XName/*02000014*/ 
          get_Name() cil managed
  // SIG: 20 00 12 50
  {
    // Method begins at RVA 0x2108
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::get_Name

  .method /*06000026*/ public hidebysig specialname 
          instance class System.Xml.Linq.XAttribute/*0200000C*/ 
          get_NextAttribute() cil managed
  // SIG: 20 00 12 30
  {
    .custom /*0C000096:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x210b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::get_NextAttribute

  .method /*06000027*/ public hidebysig specialname virtual 
          instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 7D
  {
    // Method begins at RVA 0x210e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::get_NodeType

  .method /*06000028*/ public hidebysig specialname 
          instance class System.Xml.Linq.XAttribute/*0200000C*/ 
          get_PreviousAttribute() cil managed
  // SIG: 20 00 12 30
  {
    .custom /*0C000099:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2111
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::get_PreviousAttribute

  .method /*06000029*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2114
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::get_Value

  .method /*0600002A*/ public hidebysig specialname 
          instance void  set_Value(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2117
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XAttribute::set_Value

  .method /*0600002B*/ public hidebysig specialname static 
          bool  op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 02 12 30
  {
    .custom /*0C00009D:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2119
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*0600002C*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.DateTime/*01000020*/ 
          op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 11 80 81 12 30
  {
    .custom /*0C0000A0:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x211c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*0600002D*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000021*/ 
          op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 11 80 85 12 30
  {
    .custom /*0C0000A3:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x211f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*0600002E*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Decimal/*01000022*/ 
          op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 11 80 89 12 30
  {
    .custom /*0C0000A5:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2122
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*0600002F*/ public hidebysig specialname static 
          float64  op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 0D 12 30
  {
    .custom /*0C0000A8:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2125
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*06000030*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Guid/*01000023*/ 
          op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 11 80 8D 12 30
  {
    .custom /*0C0000AA:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2128
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*06000031*/ public hidebysig specialname static 
          int32  op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 08 12 30
  {
    .custom /*0C0000AD:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x212b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*06000032*/ public hidebysig specialname static 
          int64  op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 0A 12 30
  {
    .custom /*0C0000AF:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x212e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*06000033*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<bool> 
          op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 15 11 80 95 01 02 12 30
  {
    .custom /*0C0000B0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000B1:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000042*/ 
    .custom /*0C0000D0:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 09 61 74 74 72 69 62 75 74 65 00 00 )       // ...attribute..
    // Method begins at RVA 0x2131
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*06000034*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000021*/> 
          op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 15 11 80 95 01 11 80 85 12 30
  {
    .custom /*0C0000B3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000B4:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000044*/ 
    .custom /*0C0000D2:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 09 61 74 74 72 69 62 75 74 65 00 00 )       // ...attribute..
    // Method begins at RVA 0x2134
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*06000035*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<valuetype [System.Runtime/*23000001*/]System.DateTime/*01000020*/> 
          op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 15 11 80 95 01 11 80 81 12 30
  {
    .custom /*0C0000B6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000B7:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000046*/ 
    .custom /*0C0000D3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 09 61 74 74 72 69 62 75 74 65 00 00 )       // ...attribute..
    // Method begins at RVA 0x2137
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*06000036*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*01000022*/> 
          op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 15 11 80 95 01 11 80 89 12 30
  {
    .custom /*0C0000B9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000BA:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000048*/ 
    .custom /*0C0000D4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 09 61 74 74 72 69 62 75 74 65 00 00 )       // ...attribute..
    // Method begins at RVA 0x213a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*06000037*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<float64> 
          op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 15 11 80 95 01 0D 12 30
  {
    .custom /*0C0000BB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000BC:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*0800004A*/ 
    .custom /*0C0000D5:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 09 61 74 74 72 69 62 75 74 65 00 00 )       // ...attribute..
    // Method begins at RVA 0x213d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*06000038*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<valuetype [System.Runtime/*23000001*/]System.Guid/*01000023*/> 
          op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 15 11 80 95 01 11 80 8D 12 30
  {
    .custom /*0C0000BD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000BE:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*0800004C*/ 
    .custom /*0C0000D6:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 09 61 74 74 72 69 62 75 74 65 00 00 )       // ...attribute..
    // Method begins at RVA 0x2140
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*06000039*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<int32> 
          op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 15 11 80 95 01 08 12 30
  {
    .custom /*0C0000BF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000C0:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*0800004E*/ 
    .custom /*0C0000D7:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 09 61 74 74 72 69 62 75 74 65 00 00 )       // ...attribute..
    // Method begins at RVA 0x2143
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*0600003A*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<int64> 
          op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 15 11 80 95 01 0A 12 30
  {
    .custom /*0C0000C1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000C2:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000050*/ 
    .custom /*0C0000D8:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 09 61 74 74 72 69 62 75 74 65 00 00 )       // ...attribute..
    // Method begins at RVA 0x2146
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*0600003B*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<float32> 
          op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 15 11 80 95 01 0C 12 30
  {
    .custom /*0C0000C3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000C4:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000052*/ 
    .custom /*0C0000D9:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 09 61 74 74 72 69 62 75 74 65 00 00 )       // ...attribute..
    // Method begins at RVA 0x2149
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*0600003C*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/> 
          op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 15 11 80 95 01 11 80 99 12 30
  {
    .custom /*0C0000C5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000C6:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000054*/ 
    .custom /*0C0000DB:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 09 61 74 74 72 69 62 75 74 65 00 00 )       // ...attribute..
    // Method begins at RVA 0x214c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*0600003D*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<uint32> 
          op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 15 11 80 95 01 09 12 30
  {
    .custom /*0C0000C7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000C8:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000056*/ 
    .custom /*0C0000DE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 09 61 74 74 72 69 62 75 74 65 00 00 )       // ...attribute..
    // Method begins at RVA 0x214f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*0600003E*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<uint64> 
          op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 15 11 80 95 01 0B 12 30
  {
    .custom /*0C0000C9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000CA:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000058*/ 
    .custom /*0C0000E0:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 09 61 74 74 72 69 62 75 74 65 00 00 )       // ...attribute..
    // Method begins at RVA 0x2152
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*0600003F*/ public hidebysig specialname static 
          float32  op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 0C 12 30
  {
    .custom /*0C0000CB:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2155
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*06000040*/ public hidebysig specialname static 
          string  op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 0E 12 30
  {
    .custom /*0C0000CC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000CD:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*0800005B*/ 
    .custom /*0C0000E1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 09 61 74 74 72 69 62 75 74 65 00 00 )       // ...attribute..
    // Method begins at RVA 0x2158
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*06000041*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ 
          op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 11 80 99 12 30
  {
    .custom /*0C0000CE:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x215b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*06000042*/ public hidebysig specialname static 
          uint32  op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 09 12 30
  {
    .custom /*0C0000CF:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x215e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*06000043*/ public hidebysig specialname static 
          uint64  op_Explicit(class System.Xml.Linq.XAttribute/*0200000C*/ attribute) cil managed
  // SIG: 00 01 0B 12 30
  {
    .custom /*0C0000D1:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2161
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::op_Explicit

  .method /*06000044*/ public hidebysig instance void 
          Remove() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2164
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XAttribute::Remove

  .method /*06000045*/ public hidebysig instance void 
          SetValue(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2166
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XAttribute::SetValue

  .method /*06000046*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2168
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XAttribute::ToString

  .property /*17000001*/ callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XAttribute/*0200000C*/>
          EmptySequence()
  {
    .get class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XAttribute/*0200000C*/> System.Xml.Linq.XAttribute/*0200000C*/::get_EmptySequence() /* 06000023 */
  } // end of property XAttribute::EmptySequence
  .property /*17000002*/ instance callconv(8) bool
          IsNamespaceDeclaration()
  {
    .get instance bool System.Xml.Linq.XAttribute/*0200000C*/::get_IsNamespaceDeclaration() /* 06000024 */
  } // end of property XAttribute::IsNamespaceDeclaration
  .property /*17000003*/ instance callconv(8) class System.Xml.Linq.XName/*02000014*/
          Name()
  {
    .get instance class System.Xml.Linq.XName/*02000014*/ System.Xml.Linq.XAttribute/*0200000C*/::get_Name() /* 06000025 */
  } // end of property XAttribute::Name
  .property /*17000004*/ instance callconv(8) class System.Xml.Linq.XAttribute/*0200000C*/
          NextAttribute()
  {
    .custom /*0C000027:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Xml.Linq.XAttribute/*0200000C*/ System.Xml.Linq.XAttribute/*0200000C*/::get_NextAttribute() /* 06000026 */
  } // end of property XAttribute::NextAttribute
  .property /*17000005*/ instance callconv(8) valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/
          NodeType()
  {
    .get instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/ System.Xml.Linq.XAttribute/*0200000C*/::get_NodeType() /* 06000027 */
  } // end of property XAttribute::NodeType
  .property /*17000006*/ instance callconv(8) class System.Xml.Linq.XAttribute/*0200000C*/
          PreviousAttribute()
  {
    .custom /*0C000031:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Xml.Linq.XAttribute/*0200000C*/ System.Xml.Linq.XAttribute/*0200000C*/::get_PreviousAttribute() /* 06000028 */
  } // end of property XAttribute::PreviousAttribute
  .property /*17000007*/ instance callconv(8) string
          Value()
  {
    .get instance string System.Xml.Linq.XAttribute/*0200000C*/::get_Value() /* 06000029 */
    .set instance void System.Xml.Linq.XAttribute/*0200000C*/::set_Value(string) /* 0600002A */
  } // end of property XAttribute::Value
} // end of class System.Xml.Linq.XAttribute

.class /*0200000D*/ public auto ansi beforefieldinit System.Xml.Linq.XCData
       extends System.Xml.Linq.XText/*0200001E*/
{
  .custom /*0C000050:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000051:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000047*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x216b
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000169       */ call       instance void System.Xml.Linq.XText/*0200001E*/::.ctor(string) /* 06000169 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method XCData::.ctor

  .method /*06000048*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XCData/*0200000D*/ other) cil managed
  // SIG: 20 01 01 12 34
  {
    // Method begins at RVA 0x2174
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000169       */ call       instance void System.Xml.Linq.XText/*0200001E*/::.ctor(string) /* 06000169 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method XCData::.ctor

  .method /*06000049*/ public hidebysig specialname virtual 
          instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 7D
  {
    // Method begins at RVA 0x217d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XCData::get_NodeType

  .method /*0600004A*/ public hidebysig virtual 
          instance void  WriteTo(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer) cil managed
  // SIG: 20 01 01 12 80 9D
  {
    // Method begins at RVA 0x2180
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XCData::WriteTo

  .method /*0600004B*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          WriteToAsync(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer,
                       valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 20 02 12 80 A1 12 80 9D 11 80 A5
  {
    // Method begins at RVA 0x2182
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XCData::WriteToAsync

  .property /*17000008*/ instance callconv(8) valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/
          NodeType()
  {
    .get instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/ System.Xml.Linq.XCData/*0200000D*/::get_NodeType() /* 06000049 */
  } // end of property XCData::NodeType
} // end of class System.Xml.Linq.XCData

.class /*0200000E*/ public auto ansi beforefieldinit System.Xml.Linq.XComment
       extends System.Xml.Linq.XNode/*02000016*/
{
  .custom /*0C000053:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000054:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600004C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2185
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000112       */ call       instance void System.Xml.Linq.XNode/*02000016*/::.ctor() /* 06000112 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XComment::.ctor

  .method /*0600004D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XComment/*0200000E*/ other) cil managed
  // SIG: 20 01 01 12 38
  {
    // Method begins at RVA 0x218d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000112       */ call       instance void System.Xml.Linq.XNode/*02000016*/::.ctor() /* 06000112 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XComment::.ctor

  .method /*0600004E*/ public hidebysig specialname virtual 
          instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 7D
  {
    // Method begins at RVA 0x2195
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XComment::get_NodeType

  .method /*0600004F*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2198
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XComment::get_Value

  .method /*06000050*/ public hidebysig specialname 
          instance void  set_Value(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x219b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XComment::set_Value

  .method /*06000051*/ public hidebysig virtual 
          instance void  WriteTo(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer) cil managed
  // SIG: 20 01 01 12 80 9D
  {
    // Method begins at RVA 0x219d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XComment::WriteTo

  .method /*06000052*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          WriteToAsync(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer,
                       valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 20 02 12 80 A1 12 80 9D 11 80 A5
  {
    // Method begins at RVA 0x219f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XComment::WriteToAsync

  .property /*17000009*/ instance callconv(8) valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/
          NodeType()
  {
    .get instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/ System.Xml.Linq.XComment/*0200000E*/::get_NodeType() /* 0600004E */
  } // end of property XComment::NodeType
  .property /*1700000A*/ instance callconv(8) string
          Value()
  {
    .get instance string System.Xml.Linq.XComment/*0200000E*/::get_Value() /* 0600004F */
    .set instance void System.Xml.Linq.XComment/*0200000E*/::set_Value(string) /* 06000050 */
  } // end of property XComment::Value
} // end of class System.Xml.Linq.XComment

.class /*0200000F*/ public abstract auto ansi beforefieldinit System.Xml.Linq.XContainer
       extends System.Xml.Linq.XNode/*02000016*/
{
  .custom /*0C000056:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000057:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000053*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21a2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000112       */ call       instance void System.Xml.Linq.XNode/*02000016*/::.ctor() /* 06000112 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XContainer::.ctor

  .method /*06000054*/ public hidebysig specialname 
          instance class System.Xml.Linq.XNode/*02000016*/ 
          get_FirstNode() cil managed
  // SIG: 20 00 12 58
  {
    .custom /*0C0000DA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XContainer::get_FirstNode

  .method /*06000055*/ public hidebysig specialname 
          instance class System.Xml.Linq.XNode/*02000016*/ 
          get_LastNode() cil managed
  // SIG: 20 00 12 58
  {
    .custom /*0C0000DC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XContainer::get_LastNode

  .method /*06000056*/ public hidebysig instance void 
          Add(object content) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C0000DD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21b0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XContainer::Add

  .method /*06000057*/ public hidebysig instance void 
          Add(object[] content) cil managed
  // SIG: 20 01 01 1D 1C
  {
    .param [1]/*0800006D*/ 
    .custom /*0C0000E5:0A000016*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100002A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C0000E6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x21b2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XContainer::Add

  .method /*06000058*/ public hidebysig instance void 
          AddFirst(object content) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C0000DF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21b4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XContainer::AddFirst

  .method /*06000059*/ public hidebysig instance void 
          AddFirst(object[] content) cil managed
  // SIG: 20 01 01 1D 1C
  {
    .param [1]/*0800006F*/ 
    .custom /*0C0000E7:0A000016*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100002A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C0000E8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x21b6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XContainer::AddFirst

  .method /*0600005A*/ public hidebysig instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ 
          CreateWriter() cil managed
  // SIG: 20 00 12 80 9D
  {
    // Method begins at RVA 0x21b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XContainer::CreateWriter

  .method /*0600005B*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XNode/*02000016*/> 
          DescendantNodes() cil managed
  // SIG: 20 00 15 12 6D 01 12 58
  {
    // Method begins at RVA 0x21bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XContainer::DescendantNodes

  .method /*0600005C*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          Descendants() cil managed
  // SIG: 20 00 15 12 6D 01 12 4C
  {
    // Method begins at RVA 0x21be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XContainer::Descendants

  .method /*0600005D*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          Descendants(class System.Xml.Linq.XName/*02000014*/ name) cil managed
  // SIG: 20 01 15 12 6D 01 12 4C 12 50
  {
    .param [1]/*08000070*/ 
    .custom /*0C0000EA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XContainer::Descendants

  .method /*0600005E*/ public hidebysig instance class System.Xml.Linq.XElement/*02000013*/ 
          Element(class System.Xml.Linq.XName/*02000014*/ name) cil managed
  // SIG: 20 01 12 4C 12 50
  {
    .param [0]/*08000071*/ 
    .custom /*0C0000EB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XContainer::Element

  .method /*0600005F*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          Elements() cil managed
  // SIG: 20 00 15 12 6D 01 12 4C
  {
    // Method begins at RVA 0x21c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XContainer::Elements

  .method /*06000060*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          Elements(class System.Xml.Linq.XName/*02000014*/ name) cil managed
  // SIG: 20 01 15 12 6D 01 12 4C 12 50
  {
    .param [1]/*08000073*/ 
    .custom /*0C0000EE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XContainer::Elements

  .method /*06000061*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XNode/*02000016*/> 
          Nodes() cil managed
  // SIG: 20 00 15 12 6D 01 12 58
  {
    // Method begins at RVA 0x21cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XContainer::Nodes

  .method /*06000062*/ public hidebysig instance void 
          RemoveNodes() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21d0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XContainer::RemoveNodes

  .method /*06000063*/ public hidebysig instance void 
          ReplaceNodes(object content) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C0000E2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XContainer::ReplaceNodes

  .method /*06000064*/ public hidebysig instance void 
          ReplaceNodes(object[] content) cil managed
  // SIG: 20 01 01 1D 1C
  {
    .param [1]/*08000075*/ 
    .custom /*0C0000F0:0A000016*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100002A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C0000F1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x21d4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XContainer::ReplaceNodes

  .property /*1700000B*/ instance callconv(8) class System.Xml.Linq.XNode/*02000016*/
          FirstNode()
  {
    .custom /*0C000048:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Xml.Linq.XNode/*02000016*/ System.Xml.Linq.XContainer/*0200000F*/::get_FirstNode() /* 06000054 */
  } // end of property XContainer::FirstNode
  .property /*1700000C*/ instance callconv(8) class System.Xml.Linq.XNode/*02000016*/
          LastNode()
  {
    .custom /*0C00004E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Xml.Linq.XNode/*02000016*/ System.Xml.Linq.XContainer/*0200000F*/::get_LastNode() /* 06000055 */
  } // end of property XContainer::LastNode
} // end of class System.Xml.Linq.XContainer

.class /*02000010*/ public auto ansi beforefieldinit System.Xml.Linq.XDeclaration
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000059:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00005A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000065*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string version,
                               string encoding,
                               string standalone) cil managed
  // SIG: 20 03 01 0E 0E 0E
  {
    // Method begins at RVA 0x21d6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XDeclaration::.ctor

  .method /*06000066*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XDeclaration/*02000010*/ other) cil managed
  // SIG: 20 01 01 12 40
  {
    .custom /*0C0000E3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21de
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XDeclaration::.ctor

  .method /*06000067*/ public hidebysig specialname 
          instance string  get_Encoding() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDeclaration::get_Encoding

  .method /*06000068*/ public hidebysig specialname 
          instance void  set_Encoding(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21e9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XDeclaration::set_Encoding

  .method /*06000069*/ public hidebysig specialname 
          instance string  get_Standalone() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDeclaration::get_Standalone

  .method /*0600006A*/ public hidebysig specialname 
          instance void  set_Standalone(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21ee
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XDeclaration::set_Standalone

  .method /*0600006B*/ public hidebysig specialname 
          instance string  get_Version() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDeclaration::get_Version

  .method /*0600006C*/ public hidebysig specialname 
          instance void  set_Version(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21f3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XDeclaration::set_Version

  .method /*0600006D*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000E4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDeclaration::ToString

  .property /*1700000D*/ instance callconv(8) string
          Encoding()
  {
    .get instance string System.Xml.Linq.XDeclaration/*02000010*/::get_Encoding() /* 06000067 */
    .set instance void System.Xml.Linq.XDeclaration/*02000010*/::set_Encoding(string) /* 06000068 */
  } // end of property XDeclaration::Encoding
  .property /*1700000E*/ instance callconv(8) string
          Standalone()
  {
    .get instance string System.Xml.Linq.XDeclaration/*02000010*/::get_Standalone() /* 06000069 */
    .set instance void System.Xml.Linq.XDeclaration/*02000010*/::set_Standalone(string) /* 0600006A */
  } // end of property XDeclaration::Standalone
  .property /*1700000F*/ instance callconv(8) string
          Version()
  {
    .get instance string System.Xml.Linq.XDeclaration/*02000010*/::get_Version() /* 0600006B */
    .set instance void System.Xml.Linq.XDeclaration/*02000010*/::set_Version(string) /* 0600006C */
  } // end of property XDeclaration::Version
} // end of class System.Xml.Linq.XDeclaration

.class /*02000011*/ public auto ansi beforefieldinit System.Xml.Linq.XDocument
       extends System.Xml.Linq.XContainer/*0200000F*/
{
  .custom /*0C00005E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600006E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21f8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000053       */ call       instance void System.Xml.Linq.XContainer/*0200000F*/::.ctor() /* 06000053 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XDocument::.ctor

  .method /*0600006F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object[] content) cil managed
  // SIG: 20 01 01 1D 1C
  {
    .param [1]/*0800007D*/ 
    .custom /*0C0000F3:0A000016*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100002A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C0000F4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2200
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000053       */ call       instance void System.Xml.Linq.XContainer/*0200000F*/::.ctor() /* 06000053 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XDocument::.ctor

  .method /*06000070*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XDeclaration/*02000010*/ declaration,
                               object[] content) cil managed
  // SIG: 20 02 01 12 40 1D 1C
  {
    .custom /*0C0000E9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800007F*/ 
    .custom /*0C0000F5:0A000016*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100002A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C0000F6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2208
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000053       */ call       instance void System.Xml.Linq.XContainer/*0200000F*/::.ctor() /* 06000053 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XDocument::.ctor

  .method /*06000071*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XDocument/*02000011*/ other) cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x2210
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000053       */ call       instance void System.Xml.Linq.XContainer/*0200000F*/::.ctor() /* 06000053 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XDocument::.ctor

  .method /*06000072*/ public hidebysig specialname 
          instance class System.Xml.Linq.XDeclaration/*02000010*/ 
          get_Declaration() cil managed
  // SIG: 20 00 12 40
  {
    .custom /*0C0000EC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2218
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::get_Declaration

  .method /*06000073*/ public hidebysig specialname 
          instance void  set_Declaration(class System.Xml.Linq.XDeclaration/*02000010*/ 'value') cil managed
  // SIG: 20 01 01 12 40
  {
    .custom /*0C0000ED:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x221b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XDocument::set_Declaration

  .method /*06000074*/ public hidebysig specialname 
          instance class System.Xml.Linq.XDocumentType/*02000012*/ 
          get_DocumentType() cil managed
  // SIG: 20 00 12 48
  {
    .custom /*0C0000EF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x221d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::get_DocumentType

  .method /*06000075*/ public hidebysig specialname virtual 
          instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 7D
  {
    // Method begins at RVA 0x2220
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::get_NodeType

  .method /*06000076*/ public hidebysig specialname 
          instance class System.Xml.Linq.XElement/*02000013*/ 
          get_Root() cil managed
  // SIG: 20 00 12 4C
  {
    .custom /*0C0000F2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2223
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::get_Root

  .method /*06000077*/ public hidebysig static 
          class System.Xml.Linq.XDocument/*02000011*/ 
          Load(class [System.Runtime/*23000001*/]System.IO.Stream/*0100002B*/ 'stream') cil managed
  // SIG: 00 01 12 44 12 80 AD
  {
    // Method begins at RVA 0x2226
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::Load

  .method /*06000078*/ public hidebysig static 
          class System.Xml.Linq.XDocument/*02000011*/ 
          Load(class [System.Runtime/*23000001*/]System.IO.Stream/*0100002B*/ 'stream',
               valuetype System.Xml.Linq.LoadOptions/*02000009*/ options) cil managed
  // SIG: 00 02 12 44 12 80 AD 11 24
  {
    // Method begins at RVA 0x2229
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::Load

  .method /*06000079*/ public hidebysig static 
          class System.Xml.Linq.XDocument/*02000011*/ 
          Load(class [System.Runtime/*23000001*/]System.IO.TextReader/*0100002C*/ textReader) cil managed
  // SIG: 00 01 12 44 12 80 B1
  {
    // Method begins at RVA 0x222c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::Load

  .method /*0600007A*/ public hidebysig static 
          class System.Xml.Linq.XDocument/*02000011*/ 
          Load(class [System.Runtime/*23000001*/]System.IO.TextReader/*0100002C*/ textReader,
               valuetype System.Xml.Linq.LoadOptions/*02000009*/ options) cil managed
  // SIG: 00 02 12 44 12 80 B1 11 24
  {
    // Method begins at RVA 0x222f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::Load

  .method /*0600007B*/ public hidebysig static 
          class System.Xml.Linq.XDocument/*02000011*/ 
          Load(string uri) cil managed
  // SIG: 00 01 12 44 0E
  {
    .param [1]/*08000088*/ 
    .custom /*0C0000F7:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100002D*/::.ctor(string) /* 0A000017 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x2232
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::Load

  .method /*0600007C*/ public hidebysig static 
          class System.Xml.Linq.XDocument/*02000011*/ 
          Load(string uri,
               valuetype System.Xml.Linq.LoadOptions/*02000009*/ options) cil managed
  // SIG: 00 02 12 44 0E 11 24
  {
    .param [1]/*08000089*/ 
    .custom /*0C0000F8:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100002D*/::.ctor(string) /* 0A000017 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x2235
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::Load

  .method /*0600007D*/ public hidebysig static 
          class System.Xml.Linq.XDocument/*02000011*/ 
          Load(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100002E*/ reader) cil managed
  // SIG: 00 01 12 44 12 80 B9
  {
    // Method begins at RVA 0x2238
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::Load

  .method /*0600007E*/ public hidebysig static 
          class System.Xml.Linq.XDocument/*02000011*/ 
          Load(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100002E*/ reader,
               valuetype System.Xml.Linq.LoadOptions/*02000009*/ options) cil managed
  // SIG: 00 02 12 44 12 80 B9 11 24
  {
    // Method begins at RVA 0x223b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::Load

  .method /*0600007F*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002F*/<class System.Xml.Linq.XDocument/*02000011*/> 
          LoadAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*0100002B*/ 'stream',
                    valuetype System.Xml.Linq.LoadOptions/*02000009*/ options,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 00 03 15 12 80 BD 01 12 44 12 80 AD 11 24 11 80 A5
  {
    // Method begins at RVA 0x223e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::LoadAsync

  .method /*06000080*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002F*/<class System.Xml.Linq.XDocument/*02000011*/> 
          LoadAsync(class [System.Runtime/*23000001*/]System.IO.TextReader/*0100002C*/ textReader,
                    valuetype System.Xml.Linq.LoadOptions/*02000009*/ options,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 00 03 15 12 80 BD 01 12 44 12 80 B1 11 24 11 80 A5
  {
    // Method begins at RVA 0x2241
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::LoadAsync

  .method /*06000081*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002F*/<class System.Xml.Linq.XDocument/*02000011*/> 
          LoadAsync(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100002E*/ reader,
                    valuetype System.Xml.Linq.LoadOptions/*02000009*/ options,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 00 03 15 12 80 BD 01 12 44 12 80 B9 11 24 11 80 A5
  {
    // Method begins at RVA 0x2244
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::LoadAsync

  .method /*06000082*/ public hidebysig static 
          class System.Xml.Linq.XDocument/*02000011*/ 
          Parse(string text) cil managed
  // SIG: 00 01 12 44 0E
  {
    // Method begins at RVA 0x2247
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::Parse

  .method /*06000083*/ public hidebysig static 
          class System.Xml.Linq.XDocument/*02000011*/ 
          Parse(string text,
                valuetype System.Xml.Linq.LoadOptions/*02000009*/ options) cil managed
  // SIG: 00 02 12 44 0E 11 24
  {
    // Method begins at RVA 0x224a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::Parse

  .method /*06000084*/ public hidebysig instance void 
          Save(class [System.Runtime/*23000001*/]System.IO.Stream/*0100002B*/ 'stream') cil managed
  // SIG: 20 01 01 12 80 AD
  {
    // Method begins at RVA 0x224d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XDocument::Save

  .method /*06000085*/ public hidebysig instance void 
          Save(class [System.Runtime/*23000001*/]System.IO.Stream/*0100002B*/ 'stream',
               valuetype System.Xml.Linq.SaveOptions/*0200000B*/ options) cil managed
  // SIG: 20 02 01 12 80 AD 11 2C
  {
    // Method begins at RVA 0x224f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XDocument::Save

  .method /*06000086*/ public hidebysig instance void 
          Save(class [System.Runtime/*23000001*/]System.IO.TextWriter/*01000030*/ textWriter) cil managed
  // SIG: 20 01 01 12 80 C1
  {
    // Method begins at RVA 0x2251
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XDocument::Save

  .method /*06000087*/ public hidebysig instance void 
          Save(class [System.Runtime/*23000001*/]System.IO.TextWriter/*01000030*/ textWriter,
               valuetype System.Xml.Linq.SaveOptions/*0200000B*/ options) cil managed
  // SIG: 20 02 01 12 80 C1 11 2C
  {
    // Method begins at RVA 0x2253
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XDocument::Save

  .method /*06000088*/ public hidebysig instance void 
          Save(string fileName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2255
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XDocument::Save

  .method /*06000089*/ public hidebysig instance void 
          Save(string fileName,
               valuetype System.Xml.Linq.SaveOptions/*0200000B*/ options) cil managed
  // SIG: 20 02 01 0E 11 2C
  {
    // Method begins at RVA 0x2257
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XDocument::Save

  .method /*0600008A*/ public hidebysig instance void 
          Save(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer) cil managed
  // SIG: 20 01 01 12 80 9D
  {
    // Method begins at RVA 0x2259
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XDocument::Save

  .method /*0600008B*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          SaveAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*0100002B*/ 'stream',
                    valuetype System.Xml.Linq.SaveOptions/*0200000B*/ options,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A1 12 80 AD 11 2C 11 80 A5
  {
    // Method begins at RVA 0x225b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::SaveAsync

  .method /*0600008C*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          SaveAsync(class [System.Runtime/*23000001*/]System.IO.TextWriter/*01000030*/ textWriter,
                    valuetype System.Xml.Linq.SaveOptions/*0200000B*/ options,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A1 12 80 C1 11 2C 11 80 A5
  {
    // Method begins at RVA 0x225e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::SaveAsync

  .method /*0600008D*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          SaveAsync(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 20 02 12 80 A1 12 80 9D 11 80 A5
  {
    // Method begins at RVA 0x2261
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::SaveAsync

  .method /*0600008E*/ public hidebysig virtual 
          instance void  WriteTo(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer) cil managed
  // SIG: 20 01 01 12 80 9D
  {
    // Method begins at RVA 0x2264
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XDocument::WriteTo

  .method /*0600008F*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          WriteToAsync(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer,
                       valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 20 02 12 80 A1 12 80 9D 11 80 A5
  {
    // Method begins at RVA 0x2266
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocument::WriteToAsync

  .property /*17000010*/ instance callconv(8) class System.Xml.Linq.XDeclaration/*02000010*/
          Declaration()
  {
    .custom /*0C00005C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Xml.Linq.XDeclaration/*02000010*/ System.Xml.Linq.XDocument/*02000011*/::get_Declaration() /* 06000072 */
    .set instance void System.Xml.Linq.XDocument/*02000011*/::set_Declaration(class System.Xml.Linq.XDeclaration/*02000010*/) /* 06000073 */
  } // end of property XDocument::Declaration
  .property /*17000011*/ instance callconv(8) class System.Xml.Linq.XDocumentType/*02000012*/
          DocumentType()
  {
    .custom /*0C000060:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Xml.Linq.XDocumentType/*02000012*/ System.Xml.Linq.XDocument/*02000011*/::get_DocumentType() /* 06000074 */
  } // end of property XDocument::DocumentType
  .property /*17000012*/ instance callconv(8) valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/
          NodeType()
  {
    .get instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/ System.Xml.Linq.XDocument/*02000011*/::get_NodeType() /* 06000075 */
  } // end of property XDocument::NodeType
  .property /*17000013*/ instance callconv(8) class System.Xml.Linq.XElement/*02000013*/
          Root()
  {
    .custom /*0C00006A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Xml.Linq.XElement/*02000013*/ System.Xml.Linq.XDocument/*02000011*/::get_Root() /* 06000076 */
  } // end of property XDocument::Root
} // end of class System.Xml.Linq.XDocument

.class /*02000012*/ public auto ansi beforefieldinit System.Xml.Linq.XDocumentType
       extends System.Xml.Linq.XNode/*02000016*/
{
  .custom /*0C000062:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000063:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000090*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               string publicId,
                               string systemId,
                               string internalSubset) cil managed
  // SIG: 20 04 01 0E 0E 0E 0E
  {
    .param [1]/*080000AF*/ 
    .custom /*0C000100:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2269
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000112       */ call       instance void System.Xml.Linq.XNode/*02000016*/::.ctor() /* 06000112 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XDocumentType::.ctor

  .method /*06000091*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XDocumentType/*02000012*/ other) cil managed
  // SIG: 20 01 01 12 48
  {
    .custom /*0C0000F9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2271
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000112       */ call       instance void System.Xml.Linq.XNode/*02000016*/::.ctor() /* 06000112 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XDocumentType::.ctor

  .method /*06000092*/ public hidebysig specialname 
          instance string  get_InternalSubset() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2279
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocumentType::get_InternalSubset

  .method /*06000093*/ public hidebysig specialname 
          instance void  set_InternalSubset(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x227c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XDocumentType::set_InternalSubset

  .method /*06000094*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000FA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x227e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocumentType::get_Name

  .method /*06000095*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000FB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2281
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XDocumentType::set_Name

  .method /*06000096*/ public hidebysig specialname virtual 
          instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 7D
  {
    // Method begins at RVA 0x2283
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocumentType::get_NodeType

  .method /*06000097*/ public hidebysig specialname 
          instance string  get_PublicId() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2286
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocumentType::get_PublicId

  .method /*06000098*/ public hidebysig specialname 
          instance void  set_PublicId(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2289
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XDocumentType::set_PublicId

  .method /*06000099*/ public hidebysig specialname 
          instance string  get_SystemId() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x228b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocumentType::get_SystemId

  .method /*0600009A*/ public hidebysig specialname 
          instance void  set_SystemId(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x228e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XDocumentType::set_SystemId

  .method /*0600009B*/ public hidebysig virtual 
          instance void  WriteTo(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer) cil managed
  // SIG: 20 01 01 12 80 9D
  {
    .custom /*0C0000FC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2290
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XDocumentType::WriteTo

  .method /*0600009C*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          WriteToAsync(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer,
                       valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 20 02 12 80 A1 12 80 9D 11 80 A5
  {
    .custom /*0C0000FD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2292
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XDocumentType::WriteToAsync

  .property /*17000014*/ instance callconv(8) string
          InternalSubset()
  {
    .get instance string System.Xml.Linq.XDocumentType/*02000012*/::get_InternalSubset() /* 06000092 */
    .set instance void System.Xml.Linq.XDocumentType/*02000012*/::set_InternalSubset(string) /* 06000093 */
  } // end of property XDocumentType::InternalSubset
  .property /*17000015*/ instance callconv(8) string
          Name()
  {
    .custom /*0C000071:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string System.Xml.Linq.XDocumentType/*02000012*/::get_Name() /* 06000094 */
    .set instance void System.Xml.Linq.XDocumentType/*02000012*/::set_Name(string) /* 06000095 */
  } // end of property XDocumentType::Name
  .property /*17000016*/ instance callconv(8) valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/
          NodeType()
  {
    .get instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/ System.Xml.Linq.XDocumentType/*02000012*/::get_NodeType() /* 06000096 */
  } // end of property XDocumentType::NodeType
  .property /*17000017*/ instance callconv(8) string
          PublicId()
  {
    .get instance string System.Xml.Linq.XDocumentType/*02000012*/::get_PublicId() /* 06000097 */
    .set instance void System.Xml.Linq.XDocumentType/*02000012*/::set_PublicId(string) /* 06000098 */
  } // end of property XDocumentType::PublicId
  .property /*17000018*/ instance callconv(8) string
          SystemId()
  {
    .get instance string System.Xml.Linq.XDocumentType/*02000012*/::get_SystemId() /* 06000099 */
    .set instance void System.Xml.Linq.XDocumentType/*02000012*/::set_SystemId(string) /* 0600009A */
  } // end of property XDocumentType::SystemId
} // end of class System.Xml.Linq.XDocumentType

.class /*02000013*/ public auto ansi beforefieldinit System.Xml.Linq.XElement
       extends System.Xml.Linq.XContainer/*0200000F*/
       implements [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.IXmlSerializable/*01000032*/
{
  .custom /*0C000065:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000066:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000067:0A000014*/ instance void [System.ObjectModel/*23000003*/]System.ComponentModel.TypeDescriptionProviderAttribute/*0100001E*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 D2 4D 53 2E 49 6E 74 65 72 6E 61 6C 2E   // ....MS.Internal.
                                                                                                                                                                                   58 6D 6C 2E 4C 69 6E 71 2E 43 6F 6D 70 6F 6E 65   // Xml.Linq.Compone
                                                                                                                                                                                   6E 74 4D 6F 64 65 6C 2E 58 54 79 70 65 44 65 73   // ntModel.XTypeDes
                                                                                                                                                                                   63 72 69 70 74 69 6F 6E 50 72 6F 76 69 64 65 72   // criptionProvider
                                                                                                                                                                                   60 31 5B 5B 53 79 73 74 65 6D 2E 58 6D 6C 2E 4C   // `1[[System.Xml.L
                                                                                                                                                                                   69 6E 71 2E 58 45 6C 65 6D 65 6E 74 2C 20 53 79   // inq.XElement, Sy
                                                                                                                                                                                   73 74 65 6D 2E 58 6D 6C 2E 4C 69 6E 71 2C 20 56   // stem.Xml.Linq, V
                                                                                                                                                                                   65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C 20   // ersion=4.0.0.0, 
                                                                                                                                                                                   43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C   // Culture=neutral,
                                                                                                                                                                                   20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D   //  PublicKeyToken=
                                                                                                                                                                                   62 37 37 61 35 63 35 36 31 39 33 34 65 30 38 39   // b77a5c561934e089
                                                                                                                                                                                   5D 5D 2C 53 79 73 74 65 6D 2E 43 6F 6D 70 6F 6E   // ]],System.Compon
                                                                                                                                                                                   65 6E 74 4D 6F 64 65 6C 2E 54 79 70 65 43 6F 6E   // entModel.TypeCon
                                                                                                                                                                                   76 65 72 74 65 72 00 00 )                         // verter..
  .custom /*0C000068:0A000018*/ instance void [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.XmlSchemaProviderAttribute/*01000031*/::.ctor(string) /* 0A000018 */ = ( 01 00 FF 01 00 54 02 05 49 73 41 6E 79 01 )       // .....T..IsAny.
  .interfaceimpl type [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.IXmlSerializable/*01000032*/
  .custom /*0C000002:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600009D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XElement/*02000013*/ other) cil managed
  // SIG: 20 01 01 12 4C
  {
    // Method begins at RVA 0x2295
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000053       */ call       instance void System.Xml.Linq.XContainer/*0200000F*/::.ctor() /* 06000053 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XElement::.ctor

  .method /*0600009E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XName/*02000014*/ name) cil managed
  // SIG: 20 01 01 12 50
  {
    // Method begins at RVA 0x229d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000053       */ call       instance void System.Xml.Linq.XContainer/*0200000F*/::.ctor() /* 06000053 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XElement::.ctor

  .method /*0600009F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XName/*02000014*/ name,
                               object content) cil managed
  // SIG: 20 02 01 12 50 1C
  {
    .param [2]/*080000BE*/ 
    .custom /*0C000101:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22a5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000053       */ call       instance void System.Xml.Linq.XContainer/*0200000F*/::.ctor() /* 06000053 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XElement::.ctor

  .method /*060000A0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XName/*02000014*/ name,
                               object[] content) cil managed
  // SIG: 20 02 01 12 50 1D 1C
  {
    .param [2]/*080000C0*/ 
    .custom /*0C000102:0A000016*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100002A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C000103:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x22ad
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000053       */ call       instance void System.Xml.Linq.XContainer/*0200000F*/::.ctor() /* 06000053 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XElement::.ctor

  .method /*060000A1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XStreamingElement/*0200001D*/ other) cil managed
  // SIG: 20 01 01 12 74
  {
    // Method begins at RVA 0x22b5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000053       */ call       instance void System.Xml.Linq.XContainer/*0200000F*/::.ctor() /* 06000053 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XElement::.ctor

  .method /*060000A2*/ public hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          get_EmptySequence() cil managed
  // SIG: 00 00 15 12 6D 01 12 4C
  {
    // Method begins at RVA 0x22bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::get_EmptySequence

  .method /*060000A3*/ public hidebysig specialname 
          instance class System.Xml.Linq.XAttribute/*0200000C*/ 
          get_FirstAttribute() cil managed
  // SIG: 20 00 12 30
  {
    .custom /*0C0000FE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::get_FirstAttribute

  .method /*060000A4*/ public hidebysig specialname 
          instance bool  get_HasAttributes() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::get_HasAttributes

  .method /*060000A5*/ public hidebysig specialname 
          instance bool  get_HasElements() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::get_HasElements

  .method /*060000A6*/ public hidebysig specialname 
          instance bool  get_IsEmpty() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::get_IsEmpty

  .method /*060000A7*/ public hidebysig specialname 
          instance class System.Xml.Linq.XAttribute/*0200000C*/ 
          get_LastAttribute() cil managed
  // SIG: 20 00 12 30
  {
    .custom /*0C0000FF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::get_LastAttribute

  .method /*060000A8*/ public hidebysig specialname 
          instance class System.Xml.Linq.XName/*02000014*/ 
          get_Name() cil managed
  // SIG: 20 00 12 50
  {
    // Method begins at RVA 0x22cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::get_Name

  .method /*060000A9*/ public hidebysig specialname 
          instance void  set_Name(class System.Xml.Linq.XName/*02000014*/ 'value') cil managed
  // SIG: 20 01 01 12 50
  {
    // Method begins at RVA 0x22d2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::set_Name

  .method /*060000AA*/ public hidebysig specialname virtual 
          instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 7D
  {
    // Method begins at RVA 0x22d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::get_NodeType

  .method /*060000AB*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::get_Value

  .method /*060000AC*/ public hidebysig specialname 
          instance void  set_Value(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x22da
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::set_Value

  .method /*060000AD*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          AncestorsAndSelf() cil managed
  // SIG: 20 00 15 12 6D 01 12 4C
  {
    // Method begins at RVA 0x22dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::AncestorsAndSelf

  .method /*060000AE*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          AncestorsAndSelf(class System.Xml.Linq.XName/*02000014*/ name) cil managed
  // SIG: 20 01 15 12 6D 01 12 4C 12 50
  {
    .param [1]/*080000C4*/ 
    .custom /*0C000106:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::AncestorsAndSelf

  .method /*060000AF*/ public hidebysig instance class System.Xml.Linq.XAttribute/*0200000C*/ 
          Attribute(class System.Xml.Linq.XName/*02000014*/ name) cil managed
  // SIG: 20 01 12 30 12 50
  {
    .param [0]/*080000C5*/ 
    .custom /*0C000108:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::Attribute

  .method /*060000B0*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XAttribute/*0200000C*/> 
          Attributes() cil managed
  // SIG: 20 00 15 12 6D 01 12 30
  {
    // Method begins at RVA 0x22e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::Attributes

  .method /*060000B1*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XAttribute/*0200000C*/> 
          Attributes(class System.Xml.Linq.XName/*02000014*/ name) cil managed
  // SIG: 20 01 15 12 6D 01 12 30 12 50
  {
    .param [1]/*080000C7*/ 
    .custom /*0C00010B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::Attributes

  .method /*060000B2*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XNode/*02000016*/> 
          DescendantNodesAndSelf() cil managed
  // SIG: 20 00 15 12 6D 01 12 58
  {
    // Method begins at RVA 0x22eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::DescendantNodesAndSelf

  .method /*060000B3*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          DescendantsAndSelf() cil managed
  // SIG: 20 00 15 12 6D 01 12 4C
  {
    // Method begins at RVA 0x22ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::DescendantsAndSelf

  .method /*060000B4*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          DescendantsAndSelf(class System.Xml.Linq.XName/*02000014*/ name) cil managed
  // SIG: 20 01 15 12 6D 01 12 4C 12 50
  {
    .param [1]/*080000C8*/ 
    .custom /*0C00010D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::DescendantsAndSelf

  .method /*060000B5*/ public hidebysig instance class System.Xml.Linq.XNamespace/*02000015*/ 
          GetDefaultNamespace() cil managed
  // SIG: 20 00 12 54
  {
    // Method begins at RVA 0x22f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::GetDefaultNamespace

  .method /*060000B6*/ public hidebysig instance class System.Xml.Linq.XNamespace/*02000015*/ 
          GetNamespaceOfPrefix(string prefix) cil managed
  // SIG: 20 01 12 54 0E
  {
    .param [0]/*080000C9*/ 
    .custom /*0C00010F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::GetNamespaceOfPrefix

  .method /*060000B7*/ public hidebysig instance string 
          GetPrefixOfNamespace(class System.Xml.Linq.XNamespace/*02000015*/ ns) cil managed
  // SIG: 20 01 0E 12 54
  {
    .param [0]/*080000CB*/ 
    .custom /*0C000113:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::GetPrefixOfNamespace

  .method /*060000B8*/ public hidebysig static 
          class System.Xml.Linq.XElement/*02000013*/ 
          Load(class [System.Runtime/*23000001*/]System.IO.Stream/*0100002B*/ 'stream') cil managed
  // SIG: 00 01 12 4C 12 80 AD
  {
    // Method begins at RVA 0x22fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::Load

  .method /*060000B9*/ public hidebysig static 
          class System.Xml.Linq.XElement/*02000013*/ 
          Load(class [System.Runtime/*23000001*/]System.IO.Stream/*0100002B*/ 'stream',
               valuetype System.Xml.Linq.LoadOptions/*02000009*/ options) cil managed
  // SIG: 00 02 12 4C 12 80 AD 11 24
  {
    // Method begins at RVA 0x2300
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::Load

  .method /*060000BA*/ public hidebysig static 
          class System.Xml.Linq.XElement/*02000013*/ 
          Load(class [System.Runtime/*23000001*/]System.IO.TextReader/*0100002C*/ textReader) cil managed
  // SIG: 00 01 12 4C 12 80 B1
  {
    // Method begins at RVA 0x2303
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::Load

  .method /*060000BB*/ public hidebysig static 
          class System.Xml.Linq.XElement/*02000013*/ 
          Load(class [System.Runtime/*23000001*/]System.IO.TextReader/*0100002C*/ textReader,
               valuetype System.Xml.Linq.LoadOptions/*02000009*/ options) cil managed
  // SIG: 00 02 12 4C 12 80 B1 11 24
  {
    // Method begins at RVA 0x2306
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::Load

  .method /*060000BC*/ public hidebysig static 
          class System.Xml.Linq.XElement/*02000013*/ 
          Load(string uri) cil managed
  // SIG: 00 01 12 4C 0E
  {
    .param [1]/*080000D3*/ 
    .custom /*0C000124:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100002D*/::.ctor(string) /* 0A000017 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x2309
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::Load

  .method /*060000BD*/ public hidebysig static 
          class System.Xml.Linq.XElement/*02000013*/ 
          Load(string uri,
               valuetype System.Xml.Linq.LoadOptions/*02000009*/ options) cil managed
  // SIG: 00 02 12 4C 0E 11 24
  {
    .param [1]/*080000D4*/ 
    .custom /*0C000127:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.StringSyntaxAttribute/*0100002D*/::.ctor(string) /* 0A000017 */ = ( 01 00 03 55 72 69 00 00 )                         // ...Uri..
    // Method begins at RVA 0x230c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::Load

  .method /*060000BE*/ public hidebysig static 
          class System.Xml.Linq.XElement/*02000013*/ 
          Load(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100002E*/ reader) cil managed
  // SIG: 00 01 12 4C 12 80 B9
  {
    // Method begins at RVA 0x230f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::Load

  .method /*060000BF*/ public hidebysig static 
          class System.Xml.Linq.XElement/*02000013*/ 
          Load(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100002E*/ reader,
               valuetype System.Xml.Linq.LoadOptions/*02000009*/ options) cil managed
  // SIG: 00 02 12 4C 12 80 B9 11 24
  {
    // Method begins at RVA 0x2312
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::Load

  .method /*060000C0*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002F*/<class System.Xml.Linq.XElement/*02000013*/> 
          LoadAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*0100002B*/ 'stream',
                    valuetype System.Xml.Linq.LoadOptions/*02000009*/ options,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 00 03 15 12 80 BD 01 12 4C 12 80 AD 11 24 11 80 A5
  {
    // Method begins at RVA 0x2315
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::LoadAsync

  .method /*060000C1*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002F*/<class System.Xml.Linq.XElement/*02000013*/> 
          LoadAsync(class [System.Runtime/*23000001*/]System.IO.TextReader/*0100002C*/ textReader,
                    valuetype System.Xml.Linq.LoadOptions/*02000009*/ options,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 00 03 15 12 80 BD 01 12 4C 12 80 B1 11 24 11 80 A5
  {
    // Method begins at RVA 0x2318
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::LoadAsync

  .method /*060000C2*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002F*/<class System.Xml.Linq.XElement/*02000013*/> 
          LoadAsync(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100002E*/ reader,
                    valuetype System.Xml.Linq.LoadOptions/*02000009*/ options,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 00 03 15 12 80 BD 01 12 4C 12 80 B9 11 24 11 80 A5
  {
    // Method begins at RVA 0x231b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::LoadAsync

  .method /*060000C3*/ public hidebysig specialname static 
          bool  op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 02 12 4C
  {
    .custom /*0C000104:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x231e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000C4*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.DateTime/*01000020*/ 
          op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 11 80 81 12 4C
  {
    .custom /*0C000105:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2321
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000C5*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000021*/ 
          op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 11 80 85 12 4C
  {
    .custom /*0C000107:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2324
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000C6*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Decimal/*01000022*/ 
          op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 11 80 89 12 4C
  {
    .custom /*0C000109:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2327
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000C7*/ public hidebysig specialname static 
          float64  op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 0D 12 4C
  {
    .custom /*0C00010A:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x232a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000C8*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Guid/*01000023*/ 
          op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 11 80 8D 12 4C
  {
    .custom /*0C00010C:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x232d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000C9*/ public hidebysig specialname static 
          int32  op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 08 12 4C
  {
    .custom /*0C00010E:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2330
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000CA*/ public hidebysig specialname static 
          int64  op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 0A 12 4C
  {
    .custom /*0C000110:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2333
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000CB*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<bool> 
          op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 15 11 80 95 01 02 12 4C
  {
    .custom /*0C000111:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000112:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080000EA*/ 
    .custom /*0C000134:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 65 6C 65 6D 65 6E 74 00 00 )             // ...element..
    // Method begins at RVA 0x2336
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000CC*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000021*/> 
          op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 15 11 80 95 01 11 80 85 12 4C
  {
    .custom /*0C000114:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000115:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080000EC*/ 
    .custom /*0C000135:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 65 6C 65 6D 65 6E 74 00 00 )             // ...element..
    // Method begins at RVA 0x2339
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000CD*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<valuetype [System.Runtime/*23000001*/]System.DateTime/*01000020*/> 
          op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 15 11 80 95 01 11 80 81 12 4C
  {
    .custom /*0C000116:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000117:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080000EE*/ 
    .custom /*0C000136:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 65 6C 65 6D 65 6E 74 00 00 )             // ...element..
    // Method begins at RVA 0x233c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000CE*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*01000022*/> 
          op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 15 11 80 95 01 11 80 89 12 4C
  {
    .custom /*0C000118:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000119:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080000F0*/ 
    .custom /*0C000137:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 65 6C 65 6D 65 6E 74 00 00 )             // ...element..
    // Method begins at RVA 0x233f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000CF*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<float64> 
          op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 15 11 80 95 01 0D 12 4C
  {
    .custom /*0C00011A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00011B:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080000F2*/ 
    .custom /*0C000138:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 65 6C 65 6D 65 6E 74 00 00 )             // ...element..
    // Method begins at RVA 0x2342
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000D0*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<valuetype [System.Runtime/*23000001*/]System.Guid/*01000023*/> 
          op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 15 11 80 95 01 11 80 8D 12 4C
  {
    .custom /*0C00011C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00011D:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080000F4*/ 
    .custom /*0C000139:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 65 6C 65 6D 65 6E 74 00 00 )             // ...element..
    // Method begins at RVA 0x2345
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000D1*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<int32> 
          op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 15 11 80 95 01 08 12 4C
  {
    .custom /*0C00011E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00011F:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080000F6*/ 
    .custom /*0C00013A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 65 6C 65 6D 65 6E 74 00 00 )             // ...element..
    // Method begins at RVA 0x2348
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000D2*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<int64> 
          op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 15 11 80 95 01 0A 12 4C
  {
    .custom /*0C000120:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000121:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080000F8*/ 
    .custom /*0C00013B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 65 6C 65 6D 65 6E 74 00 00 )             // ...element..
    // Method begins at RVA 0x234b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000D3*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<float32> 
          op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 15 11 80 95 01 0C 12 4C
  {
    .custom /*0C000122:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000123:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080000FA*/ 
    .custom /*0C00013D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 65 6C 65 6D 65 6E 74 00 00 )             // ...element..
    // Method begins at RVA 0x234e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000D4*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/> 
          op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 15 11 80 95 01 11 80 99 12 4C
  {
    .custom /*0C000125:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000126:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080000FC*/ 
    .custom /*0C00013E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 65 6C 65 6D 65 6E 74 00 00 )             // ...element..
    // Method begins at RVA 0x2351
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000D5*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<uint32> 
          op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 15 11 80 95 01 09 12 4C
  {
    .custom /*0C000128:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000129:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080000FE*/ 
    .custom /*0C000140:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 65 6C 65 6D 65 6E 74 00 00 )             // ...element..
    // Method begins at RVA 0x2354
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000D6*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<uint64> 
          op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 15 11 80 95 01 0B 12 4C
  {
    .custom /*0C00012A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00012B:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000100*/ 
    .custom /*0C000144:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 65 6C 65 6D 65 6E 74 00 00 )             // ...element..
    // Method begins at RVA 0x2357
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000D7*/ public hidebysig specialname static 
          float32  op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 0C 12 4C
  {
    .custom /*0C00012C:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x235a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000D8*/ public hidebysig specialname static 
          string  op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 0E 12 4C
  {
    .custom /*0C00012D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00012E:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000103*/ 
    .custom /*0C000145:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 65 6C 65 6D 65 6E 74 00 00 )             // ...element..
    // Method begins at RVA 0x235d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000D9*/ public hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ 
          op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 11 80 99 12 4C
  {
    .custom /*0C00012F:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2360
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000DA*/ public hidebysig specialname static 
          uint32  op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 09 12 4C
  {
    .custom /*0C000130:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2363
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000DB*/ public hidebysig specialname static 
          uint64  op_Explicit(class System.Xml.Linq.XElement/*02000013*/ element) cil managed
  // SIG: 00 01 0B 12 4C
  {
    .custom /*0C000131:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2366
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::op_Explicit

  .method /*060000DC*/ public hidebysig static 
          class System.Xml.Linq.XElement/*02000013*/ 
          Parse(string text) cil managed
  // SIG: 00 01 12 4C 0E
  {
    // Method begins at RVA 0x2369
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::Parse

  .method /*060000DD*/ public hidebysig static 
          class System.Xml.Linq.XElement/*02000013*/ 
          Parse(string text,
                valuetype System.Xml.Linq.LoadOptions/*02000009*/ options) cil managed
  // SIG: 00 02 12 4C 0E 11 24
  {
    // Method begins at RVA 0x236c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::Parse

  .method /*060000DE*/ public hidebysig instance void 
          RemoveAll() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x236f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::RemoveAll

  .method /*060000DF*/ public hidebysig instance void 
          RemoveAttributes() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2371
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::RemoveAttributes

  .method /*060000E0*/ public hidebysig instance void 
          ReplaceAll(object content) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C000132:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2373
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::ReplaceAll

  .method /*060000E1*/ public hidebysig instance void 
          ReplaceAll(object[] content) cil managed
  // SIG: 20 01 01 1D 1C
  {
    .param [1]/*0800010C*/ 
    .custom /*0C000147:0A000016*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100002A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C000148:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2375
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::ReplaceAll

  .method /*060000E2*/ public hidebysig instance void 
          ReplaceAttributes(object content) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C000133:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2377
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::ReplaceAttributes

  .method /*060000E3*/ public hidebysig instance void 
          ReplaceAttributes(object[] content) cil managed
  // SIG: 20 01 01 1D 1C
  {
    .param [1]/*0800010E*/ 
    .custom /*0C00014A:0A000016*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100002A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C00014B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2379
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::ReplaceAttributes

  .method /*060000E4*/ public hidebysig instance void 
          Save(class [System.Runtime/*23000001*/]System.IO.Stream/*0100002B*/ 'stream') cil managed
  // SIG: 20 01 01 12 80 AD
  {
    // Method begins at RVA 0x237b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::Save

  .method /*060000E5*/ public hidebysig instance void 
          Save(class [System.Runtime/*23000001*/]System.IO.Stream/*0100002B*/ 'stream',
               valuetype System.Xml.Linq.SaveOptions/*0200000B*/ options) cil managed
  // SIG: 20 02 01 12 80 AD 11 2C
  {
    // Method begins at RVA 0x237d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::Save

  .method /*060000E6*/ public hidebysig instance void 
          Save(class [System.Runtime/*23000001*/]System.IO.TextWriter/*01000030*/ textWriter) cil managed
  // SIG: 20 01 01 12 80 C1
  {
    // Method begins at RVA 0x237f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::Save

  .method /*060000E7*/ public hidebysig instance void 
          Save(class [System.Runtime/*23000001*/]System.IO.TextWriter/*01000030*/ textWriter,
               valuetype System.Xml.Linq.SaveOptions/*0200000B*/ options) cil managed
  // SIG: 20 02 01 12 80 C1 11 2C
  {
    // Method begins at RVA 0x2381
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::Save

  .method /*060000E8*/ public hidebysig instance void 
          Save(string fileName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2383
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::Save

  .method /*060000E9*/ public hidebysig instance void 
          Save(string fileName,
               valuetype System.Xml.Linq.SaveOptions/*0200000B*/ options) cil managed
  // SIG: 20 02 01 0E 11 2C
  {
    // Method begins at RVA 0x2385
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::Save

  .method /*060000EA*/ public hidebysig instance void 
          Save(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer) cil managed
  // SIG: 20 01 01 12 80 9D
  {
    // Method begins at RVA 0x2387
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::Save

  .method /*060000EB*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          SaveAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*0100002B*/ 'stream',
                    valuetype System.Xml.Linq.SaveOptions/*0200000B*/ options,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A1 12 80 AD 11 2C 11 80 A5
  {
    // Method begins at RVA 0x2389
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::SaveAsync

  .method /*060000EC*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          SaveAsync(class [System.Runtime/*23000001*/]System.IO.TextWriter/*01000030*/ textWriter,
                    valuetype System.Xml.Linq.SaveOptions/*0200000B*/ options,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A1 12 80 C1 11 2C 11 80 A5
  {
    // Method begins at RVA 0x238c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::SaveAsync

  .method /*060000ED*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          SaveAsync(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 20 02 12 80 A1 12 80 9D 11 80 A5
  {
    // Method begins at RVA 0x238f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::SaveAsync

  .method /*060000EE*/ public hidebysig instance void 
          SetAttributeValue(class System.Xml.Linq.XName/*02000014*/ name,
                            object 'value') cil managed
  // SIG: 20 02 01 12 50 1C
  {
    .param [2]/*08000122*/ 
    .custom /*0C000155:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2392
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::SetAttributeValue

  .method /*060000EF*/ public hidebysig instance void 
          SetElementValue(class System.Xml.Linq.XName/*02000014*/ name,
                          object 'value') cil managed
  // SIG: 20 02 01 12 50 1C
  {
    .param [2]/*08000124*/ 
    .custom /*0C000156:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2394
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::SetElementValue

  .method /*060000F0*/ public hidebysig instance void 
          SetValue(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2396
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::SetValue

  .method /*060000F1*/ private hidebysig newslot virtual final 
          instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.Schema.XmlSchema/*01000033*/ 
          System.Xml.Serialization.IXmlSerializable.GetSchema() cil managed
  // SIG: 20 00 12 80 CD
  {
    .override [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.IXmlSerializable/*01000032*/::GetSchema /*01000032::0A000019*/ 
    // Method begins at RVA 0x2398
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::System.Xml.Serialization.IXmlSerializable.GetSchema

  .method /*060000F2*/ private hidebysig newslot virtual final 
          instance void  System.Xml.Serialization.IXmlSerializable.ReadXml(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100002E*/ reader) cil managed
  // SIG: 20 01 01 12 80 B9
  {
    .override [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.IXmlSerializable/*01000032*/::ReadXml /*01000032::0A00001A*/ 
    // Method begins at RVA 0x239b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::System.Xml.Serialization.IXmlSerializable.ReadXml

  .method /*060000F3*/ private hidebysig newslot virtual final 
          instance void  System.Xml.Serialization.IXmlSerializable.WriteXml(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer) cil managed
  // SIG: 20 01 01 12 80 9D
  {
    .override [System.Xml.ReaderWriter/*23000002*/]System.Xml.Serialization.IXmlSerializable/*01000032*/::WriteXml /*01000032::0A00001B*/ 
    // Method begins at RVA 0x239d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::System.Xml.Serialization.IXmlSerializable.WriteXml

  .method /*060000F4*/ public hidebysig virtual 
          instance void  WriteTo(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer) cil managed
  // SIG: 20 01 01 12 80 9D
  {
    // Method begins at RVA 0x239f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XElement::WriteTo

  .method /*060000F5*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          WriteToAsync(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer,
                       valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 20 02 12 80 A1 12 80 9D 11 80 A5
  {
    // Method begins at RVA 0x23a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XElement::WriteToAsync

  .property /*17000019*/ callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/>
          EmptySequence()
  {
    .get class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> System.Xml.Linq.XElement/*02000013*/::get_EmptySequence() /* 060000A2 */
  } // end of property XElement::EmptySequence
  .property /*1700001A*/ instance callconv(8) class System.Xml.Linq.XAttribute/*0200000C*/
          FirstAttribute()
  {
    .custom /*0C00007C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Xml.Linq.XAttribute/*0200000C*/ System.Xml.Linq.XElement/*02000013*/::get_FirstAttribute() /* 060000A3 */
  } // end of property XElement::FirstAttribute
  .property /*1700001B*/ instance callconv(8) bool
          HasAttributes()
  {
    .get instance bool System.Xml.Linq.XElement/*02000013*/::get_HasAttributes() /* 060000A4 */
  } // end of property XElement::HasAttributes
  .property /*1700001C*/ instance callconv(8) bool
          HasElements()
  {
    .get instance bool System.Xml.Linq.XElement/*02000013*/::get_HasElements() /* 060000A5 */
  } // end of property XElement::HasElements
  .property /*1700001D*/ instance callconv(8) bool
          IsEmpty()
  {
    .get instance bool System.Xml.Linq.XElement/*02000013*/::get_IsEmpty() /* 060000A6 */
  } // end of property XElement::IsEmpty
  .property /*1700001E*/ instance callconv(8) class System.Xml.Linq.XAttribute/*0200000C*/
          LastAttribute()
  {
    .custom /*0C00008B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Xml.Linq.XAttribute/*0200000C*/ System.Xml.Linq.XElement/*02000013*/::get_LastAttribute() /* 060000A7 */
  } // end of property XElement::LastAttribute
  .property /*1700001F*/ instance callconv(8) class System.Xml.Linq.XName/*02000014*/
          Name()
  {
    .get instance class System.Xml.Linq.XName/*02000014*/ System.Xml.Linq.XElement/*02000013*/::get_Name() /* 060000A8 */
    .set instance void System.Xml.Linq.XElement/*02000013*/::set_Name(class System.Xml.Linq.XName/*02000014*/) /* 060000A9 */
  } // end of property XElement::Name
  .property /*17000020*/ instance callconv(8) valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/
          NodeType()
  {
    .get instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/ System.Xml.Linq.XElement/*02000013*/::get_NodeType() /* 060000AA */
  } // end of property XElement::NodeType
  .property /*17000021*/ instance callconv(8) string
          Value()
  {
    .get instance string System.Xml.Linq.XElement/*02000013*/::get_Value() /* 060000AB */
    .set instance void System.Xml.Linq.XElement/*02000013*/::set_Value(string) /* 060000AC */
  } // end of property XElement::Value
} // end of class System.Xml.Linq.XElement

.class /*02000014*/ public auto ansi sealed beforefieldinit System.Xml.Linq.XName
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000034*/<class System.Xml.Linq.XName/*02000014*/>/*1B000001*/,
                  [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*01000035*/
{
  .custom /*0C00006C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00006D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.IEquatable`1/*01000034*/<class System.Xml.Linq.XName/*02000014*/>/*1B000001*/
  .custom /*0C00001B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*01000035*/
  .custom /*0C000021:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000F6*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23a4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XName::.ctor

  .method /*060000F7*/ public hidebysig specialname 
          instance string  get_LocalName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XName::get_LocalName

  .method /*060000F8*/ public hidebysig specialname 
          instance class System.Xml.Linq.XNamespace/*02000015*/ 
          get_Namespace() cil managed
  // SIG: 20 00 12 54
  {
    // Method begins at RVA 0x23af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XName::get_Namespace

  .method /*060000F9*/ public hidebysig specialname 
          instance string  get_NamespaceName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XName::get_NamespaceName

  .method /*060000FA*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00013C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800012B*/ 
    .custom /*0C000159:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A00001E */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XName::Equals

  .method /*060000FB*/ public hidebysig static 
          class System.Xml.Linq.XName/*02000014*/ 
          Get(string expandedName) cil managed
  // SIG: 00 01 12 50 0E
  {
    // Method begins at RVA 0x23b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XName::Get

  .method /*060000FC*/ public hidebysig static 
          class System.Xml.Linq.XName/*02000014*/ 
          Get(string localName,
              string namespaceName) cil managed
  // SIG: 00 02 12 50 0E 0E
  {
    // Method begins at RVA 0x23bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XName::Get

  .method /*060000FD*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x23be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XName::GetHashCode

  .method /*060000FE*/ public hidebysig specialname static 
          bool  op_Equality(class System.Xml.Linq.XName/*02000014*/ left,
                            class System.Xml.Linq.XName/*02000014*/ right) cil managed
  // SIG: 00 02 02 12 50 12 50
  {
    .custom /*0C00013F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XName::op_Equality

  .method /*060000FF*/ public hidebysig specialname static 
          class System.Xml.Linq.XName/*02000014*/ 
          op_Implicit(string expandedName) cil managed
  // SIG: 00 01 12 50 0E
  {
    .custom /*0C000141:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000142:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000131*/ 
    .custom /*0C00015B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 0C 65 78 70 61 6E 64 65 64 4E 61 6D 65 00   // ...expandedName.
                                                                                                                                                                                    00 ) 
    // Method begins at RVA 0x23c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XName::op_Implicit

  .method /*06000100*/ public hidebysig specialname static 
          bool  op_Inequality(class System.Xml.Linq.XName/*02000014*/ left,
                              class System.Xml.Linq.XName/*02000014*/ right) cil managed
  // SIG: 00 02 02 12 50 12 50
  {
    .custom /*0C000143:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XName::op_Inequality

  .method /*06000101*/ private hidebysig newslot virtual final 
          instance bool  'System.IEquatable<System.Xml.Linq.XName>.Equals'(class System.Xml.Linq.XName/*02000014*/ other) cil managed
  // SIG: 20 01 02 12 50
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.IEquatable`1/*01000034*/<class System.Xml.Linq.XName/*02000014*/>/*1B000001*/::Equals(!0) /* 0A00001C */
    // Method begins at RVA 0x23ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XName::'System.IEquatable<System.Xml.Linq.XName>.Equals'

  .method /*06000102*/ private hidebysig newslot virtual final 
          instance void  System.Runtime.Serialization.ISerializable.GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000036*/ info,
                                                                                  valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000037*/ context) cil managed
  // SIG: 20 02 01 12 80 D9 11 80 DD
  {
    .override [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*01000035*/::GetObjectData /*01000035::0A00001D*/ 
    // Method begins at RVA 0x23cd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XName::System.Runtime.Serialization.ISerializable.GetObjectData

  .method /*06000103*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XName::ToString

  .property /*17000022*/ instance callconv(8) string
          LocalName()
  {
    .get instance string System.Xml.Linq.XName/*02000014*/::get_LocalName() /* 060000F7 */
  } // end of property XName::LocalName
  .property /*17000023*/ instance callconv(8) class System.Xml.Linq.XNamespace/*02000015*/
          Namespace()
  {
    .get instance class System.Xml.Linq.XNamespace/*02000015*/ System.Xml.Linq.XName/*02000014*/::get_Namespace() /* 060000F8 */
  } // end of property XName::Namespace
  .property /*17000024*/ instance callconv(8) string
          NamespaceName()
  {
    .get instance string System.Xml.Linq.XName/*02000014*/::get_NamespaceName() /* 060000F9 */
  } // end of property XName::NamespaceName
} // end of class System.Xml.Linq.XName

.class /*02000015*/ public auto ansi sealed beforefieldinit System.Xml.Linq.XNamespace
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C00006F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000070:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000104*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23d2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XNamespace::.ctor

  .method /*06000105*/ public hidebysig specialname 
          instance string  get_NamespaceName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNamespace::get_NamespaceName

  .method /*06000106*/ public hidebysig specialname static 
          class System.Xml.Linq.XNamespace/*02000015*/ 
          get_None() cil managed
  // SIG: 00 00 12 54
  {
    // Method begins at RVA 0x23dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNamespace::get_None

  .method /*06000107*/ public hidebysig specialname static 
          class System.Xml.Linq.XNamespace/*02000015*/ 
          get_Xml() cil managed
  // SIG: 00 00 12 54
  {
    // Method begins at RVA 0x23e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNamespace::get_Xml

  .method /*06000108*/ public hidebysig specialname static 
          class System.Xml.Linq.XNamespace/*02000015*/ 
          get_Xmlns() cil managed
  // SIG: 00 00 12 54
  {
    // Method begins at RVA 0x23e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNamespace::get_Xmlns

  .method /*06000109*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000146:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000138*/ 
    .custom /*0C00015F:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000038*/::.ctor(bool) /* 0A00001E */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNamespace::Equals

  .method /*0600010A*/ public hidebysig static 
          class System.Xml.Linq.XNamespace/*02000015*/ 
          Get(string namespaceName) cil managed
  // SIG: 00 01 12 54 0E
  {
    // Method begins at RVA 0x23e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNamespace::Get

  .method /*0600010B*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x23ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNamespace::GetHashCode

  .method /*0600010C*/ public hidebysig instance class System.Xml.Linq.XName/*02000014*/ 
          GetName(string localName) cil managed
  // SIG: 20 01 12 50 0E
  {
    // Method begins at RVA 0x23ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNamespace::GetName

  .method /*0600010D*/ public hidebysig specialname static 
          class System.Xml.Linq.XName/*02000014*/ 
          op_Addition(class System.Xml.Linq.XNamespace/*02000015*/ ns,
                      string localName) cil managed
  // SIG: 00 02 12 50 12 54 0E
  {
    // Method begins at RVA 0x23f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNamespace::op_Addition

  .method /*0600010E*/ public hidebysig specialname static 
          bool  op_Equality(class System.Xml.Linq.XNamespace/*02000015*/ left,
                            class System.Xml.Linq.XNamespace/*02000015*/ right) cil managed
  // SIG: 00 02 02 12 54 12 54
  {
    .custom /*0C000149:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNamespace::op_Equality

  .method /*0600010F*/ public hidebysig specialname static 
          class System.Xml.Linq.XNamespace/*02000015*/ 
          op_Implicit(string namespaceName) cil managed
  // SIG: 00 01 12 54 0E
  {
    .custom /*0C00014C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00014D:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
    .param [0]/*0800013F*/ 
    .custom /*0C000162:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000024*/::.ctor(string) /* 0A000015 */ = ( 01 00 0D 6E 61 6D 65 73 70 61 63 65 4E 61 6D 65   // ...namespaceName
                                                                                                                                                                                    00 00 ) 
    // Method begins at RVA 0x23f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNamespace::op_Implicit

  .method /*06000110*/ public hidebysig specialname static 
          bool  op_Inequality(class System.Xml.Linq.XNamespace/*02000015*/ left,
                              class System.Xml.Linq.XNamespace/*02000015*/ right) cil managed
  // SIG: 00 02 02 12 54 12 54
  {
    .custom /*0C00014E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNamespace::op_Inequality

  .method /*06000111*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNamespace::ToString

  .property /*17000025*/ instance callconv(8) string
          NamespaceName()
  {
    .get instance string System.Xml.Linq.XNamespace/*02000015*/::get_NamespaceName() /* 06000105 */
  } // end of property XNamespace::NamespaceName
  .property /*17000026*/ callconv(8) class System.Xml.Linq.XNamespace/*02000015*/
          None()
  {
    .get class System.Xml.Linq.XNamespace/*02000015*/ System.Xml.Linq.XNamespace/*02000015*/::get_None() /* 06000106 */
  } // end of property XNamespace::None
  .property /*17000027*/ callconv(8) class System.Xml.Linq.XNamespace/*02000015*/
          Xml()
  {
    .get class System.Xml.Linq.XNamespace/*02000015*/ System.Xml.Linq.XNamespace/*02000015*/::get_Xml() /* 06000107 */
  } // end of property XNamespace::Xml
  .property /*17000028*/ callconv(8) class System.Xml.Linq.XNamespace/*02000015*/
          Xmlns()
  {
    .get class System.Xml.Linq.XNamespace/*02000015*/ System.Xml.Linq.XNamespace/*02000015*/::get_Xmlns() /* 06000108 */
  } // end of property XNamespace::Xmlns
} // end of class System.Xml.Linq.XNamespace

.class /*02000016*/ public abstract auto ansi beforefieldinit System.Xml.Linq.XNode
       extends System.Xml.Linq.XObject/*02000019*/
{
  .custom /*0C000073:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000074:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000112*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2401
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00013A       */ call       instance void System.Xml.Linq.XObject/*02000019*/::.ctor() /* 0600013A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XNode::.ctor

  .method /*06000113*/ public hidebysig specialname static 
          class System.Xml.Linq.XNodeDocumentOrderComparer/*02000017*/ 
          get_DocumentOrderComparer() cil managed
  // SIG: 00 00 12 5C
  {
    // Method begins at RVA 0x2409
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::get_DocumentOrderComparer

  .method /*06000114*/ public hidebysig specialname static 
          class System.Xml.Linq.XNodeEqualityComparer/*02000018*/ 
          get_EqualityComparer() cil managed
  // SIG: 00 00 12 60
  {
    // Method begins at RVA 0x240c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::get_EqualityComparer

  .method /*06000115*/ public hidebysig specialname 
          instance class System.Xml.Linq.XNode/*02000016*/ 
          get_NextNode() cil managed
  // SIG: 20 00 12 58
  {
    .custom /*0C00014F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x240f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::get_NextNode

  .method /*06000116*/ public hidebysig specialname 
          instance class System.Xml.Linq.XNode/*02000016*/ 
          get_PreviousNode() cil managed
  // SIG: 20 00 12 58
  {
    .custom /*0C000150:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2412
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::get_PreviousNode

  .method /*06000117*/ public hidebysig instance void 
          AddAfterSelf(object content) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C000151:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2415
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XNode::AddAfterSelf

  .method /*06000118*/ public hidebysig instance void 
          AddAfterSelf(object[] content) cil managed
  // SIG: 20 01 01 1D 1C
  {
    .param [1]/*08000144*/ 
    .custom /*0C000163:0A000016*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100002A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C000164:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2417
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XNode::AddAfterSelf

  .method /*06000119*/ public hidebysig instance void 
          AddBeforeSelf(object content) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C000152:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2419
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XNode::AddBeforeSelf

  .method /*0600011A*/ public hidebysig instance void 
          AddBeforeSelf(object[] content) cil managed
  // SIG: 20 01 01 1D 1C
  {
    .param [1]/*08000146*/ 
    .custom /*0C000165:0A000016*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100002A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C000166:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x241b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XNode::AddBeforeSelf

  .method /*0600011B*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          Ancestors() cil managed
  // SIG: 20 00 15 12 6D 01 12 4C
  {
    // Method begins at RVA 0x241d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::Ancestors

  .method /*0600011C*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          Ancestors(class System.Xml.Linq.XName/*02000014*/ name) cil managed
  // SIG: 20 01 15 12 6D 01 12 4C 12 50
  {
    .param [1]/*08000147*/ 
    .custom /*0C000167:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2420
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::Ancestors

  .method /*0600011D*/ public hidebysig static 
          int32  CompareDocumentOrder(class System.Xml.Linq.XNode/*02000016*/ n1,
                                      class System.Xml.Linq.XNode/*02000016*/ n2) cil managed
  // SIG: 00 02 08 12 58 12 58
  {
    .custom /*0C000153:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2423
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::CompareDocumentOrder

  .method /*0600011E*/ public hidebysig instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100002E*/ 
          CreateReader() cil managed
  // SIG: 20 00 12 80 B9
  {
    // Method begins at RVA 0x2426
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::CreateReader

  .method /*0600011F*/ public hidebysig instance class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100002E*/ 
          CreateReader(valuetype System.Xml.Linq.ReaderOptions/*0200000A*/ readerOptions) cil managed
  // SIG: 20 01 12 80 B9 11 28
  {
    // Method begins at RVA 0x2429
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::CreateReader

  .method /*06000120*/ public hidebysig static 
          bool  DeepEquals(class System.Xml.Linq.XNode/*02000016*/ n1,
                           class System.Xml.Linq.XNode/*02000016*/ n2) cil managed
  // SIG: 00 02 02 12 58 12 58
  {
    .custom /*0C000154:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x242c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::DeepEquals

  .method /*06000121*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          ElementsAfterSelf() cil managed
  // SIG: 20 00 15 12 6D 01 12 4C
  {
    // Method begins at RVA 0x242f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::ElementsAfterSelf

  .method /*06000122*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          ElementsAfterSelf(class System.Xml.Linq.XName/*02000014*/ name) cil managed
  // SIG: 20 01 15 12 6D 01 12 4C 12 50
  {
    .param [1]/*0800014D*/ 
    .custom /*0C000168:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2432
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::ElementsAfterSelf

  .method /*06000123*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          ElementsBeforeSelf() cil managed
  // SIG: 20 00 15 12 6D 01 12 4C
  {
    // Method begins at RVA 0x2435
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::ElementsBeforeSelf

  .method /*06000124*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XElement/*02000013*/> 
          ElementsBeforeSelf(class System.Xml.Linq.XName/*02000014*/ name) cil managed
  // SIG: 20 01 15 12 6D 01 12 4C 12 50
  {
    .param [1]/*0800014E*/ 
    .custom /*0C000169:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2438
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::ElementsBeforeSelf

  .method /*06000125*/ public hidebysig instance bool 
          IsAfter(class System.Xml.Linq.XNode/*02000016*/ node) cil managed
  // SIG: 20 01 02 12 58
  {
    .custom /*0C000157:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x243b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::IsAfter

  .method /*06000126*/ public hidebysig instance bool 
          IsBefore(class System.Xml.Linq.XNode/*02000016*/ node) cil managed
  // SIG: 20 01 02 12 58
  {
    .custom /*0C000158:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x243e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::IsBefore

  .method /*06000127*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XNode/*02000016*/> 
          NodesAfterSelf() cil managed
  // SIG: 20 00 15 12 6D 01 12 58
  {
    // Method begins at RVA 0x2441
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::NodesAfterSelf

  .method /*06000128*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<class System.Xml.Linq.XNode/*02000016*/> 
          NodesBeforeSelf() cil managed
  // SIG: 20 00 15 12 6D 01 12 58
  {
    // Method begins at RVA 0x2444
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::NodesBeforeSelf

  .method /*06000129*/ public hidebysig static 
          class System.Xml.Linq.XNode/*02000016*/ 
          ReadFrom(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100002E*/ reader) cil managed
  // SIG: 00 01 12 58 12 80 B9
  {
    // Method begins at RVA 0x2447
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::ReadFrom

  .method /*0600012A*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100002F*/<class System.Xml.Linq.XNode/*02000016*/> 
          ReadFromAsync(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlReader/*0100002E*/ reader,
                        valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 00 02 15 12 80 BD 01 12 58 12 80 B9 11 80 A5
  {
    // Method begins at RVA 0x244a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::ReadFromAsync

  .method /*0600012B*/ public hidebysig instance void 
          Remove() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x244d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XNode::Remove

  .method /*0600012C*/ public hidebysig instance void 
          ReplaceWith(object content) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C00015A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x244f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XNode::ReplaceWith

  .method /*0600012D*/ public hidebysig instance void 
          ReplaceWith(object[] content) cil managed
  // SIG: 20 01 01 1D 1C
  {
    .param [1]/*08000155*/ 
    .custom /*0C00016A:0A000016*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100002A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C00016B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2451
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XNode::ReplaceWith

  .method /*0600012E*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2453
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::ToString

  .method /*0600012F*/ public hidebysig instance string 
          ToString(valuetype System.Xml.Linq.SaveOptions/*0200000B*/ options) cil managed
  // SIG: 20 01 0E 11 2C
  {
    // Method begins at RVA 0x2456
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNode::ToString

  .method /*06000130*/ public hidebysig newslot abstract virtual 
          instance void  WriteTo(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer) cil managed
  // SIG: 20 01 01 12 80 9D
  {
    // Method begins at RVA 0x0
  } // end of method XNode::WriteTo

  .method /*06000131*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          WriteToAsync(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer,
                       valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 20 02 12 80 A1 12 80 9D 11 80 A5
  {
    // Method begins at RVA 0x0
  } // end of method XNode::WriteToAsync

  .property /*17000029*/ callconv(8) class System.Xml.Linq.XNodeDocumentOrderComparer/*02000017*/
          DocumentOrderComparer()
  {
    .get class System.Xml.Linq.XNodeDocumentOrderComparer/*02000017*/ System.Xml.Linq.XNode/*02000016*/::get_DocumentOrderComparer() /* 06000113 */
  } // end of property XNode::DocumentOrderComparer
  .property /*1700002A*/ callconv(8) class System.Xml.Linq.XNodeEqualityComparer/*02000018*/
          EqualityComparer()
  {
    .get class System.Xml.Linq.XNodeEqualityComparer/*02000018*/ System.Xml.Linq.XNode/*02000016*/::get_EqualityComparer() /* 06000114 */
  } // end of property XNode::EqualityComparer
  .property /*1700002B*/ instance callconv(8) class System.Xml.Linq.XNode/*02000016*/
          NextNode()
  {
    .custom /*0C00009F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Xml.Linq.XNode/*02000016*/ System.Xml.Linq.XNode/*02000016*/::get_NextNode() /* 06000115 */
  } // end of property XNode::NextNode
  .property /*1700002C*/ instance callconv(8) class System.Xml.Linq.XNode/*02000016*/
          PreviousNode()
  {
    .custom /*0C0000A2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Xml.Linq.XNode/*02000016*/ System.Xml.Linq.XNode/*02000016*/::get_PreviousNode() /* 06000116 */
  } // end of property XNode::PreviousNode
} // end of class System.Xml.Linq.XNode

.class /*02000017*/ public auto ansi sealed beforefieldinit System.Xml.Linq.XNodeDocumentOrderComparer
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*0100003A*/<class System.Xml.Linq.XNode/*02000016*/>/*1B000002*/,
                  [System.Runtime/*23000001*/]System.Collections.IComparer/*01000039*/
{
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*0100003A*/<class System.Xml.Linq.XNode/*02000016*/>/*1B000002*/
  .custom /*0C000026:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 02 00 00 ) 
  .method /*06000132*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2459
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XNodeDocumentOrderComparer::.ctor

  .method /*06000133*/ public hidebysig newslot virtual final 
          instance int32  Compare(class System.Xml.Linq.XNode/*02000016*/ x,
                                  class System.Xml.Linq.XNode/*02000016*/ y) cil managed
  // SIG: 20 02 08 12 58 12 58
  {
    .custom /*0C00015C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2461
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNodeDocumentOrderComparer::Compare

  .method /*06000134*/ private hidebysig newslot virtual final 
          instance int32  System.Collections.IComparer.Compare(object x,
                                                               object y) cil managed
  // SIG: 20 02 08 1C 1C
  {
    .override [System.Runtime/*23000001*/]System.Collections.IComparer/*01000039*/::Compare /*01000039::0A00001F*/ 
    // Method begins at RVA 0x2464
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNodeDocumentOrderComparer::System.Collections.IComparer.Compare

} // end of class System.Xml.Linq.XNodeDocumentOrderComparer

.class /*02000018*/ public auto ansi sealed beforefieldinit System.Xml.Linq.XNodeEqualityComparer
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*0100003C*/<class System.Xml.Linq.XNode/*02000016*/>/*1B000003*/,
                  [System.Runtime/*23000001*/]System.Collections.IEqualityComparer/*0100003B*/
{
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*0100003C*/<class System.Xml.Linq.XNode/*02000016*/>/*1B000003*/
  .custom /*0C000030:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .method /*06000135*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2467
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XNodeEqualityComparer::.ctor

  .method /*06000136*/ public hidebysig newslot virtual final 
          instance bool  Equals(class System.Xml.Linq.XNode/*02000016*/ x,
                                class System.Xml.Linq.XNode/*02000016*/ y) cil managed
  // SIG: 20 02 02 12 58 12 58
  {
    .custom /*0C00015D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x246f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNodeEqualityComparer::Equals

  .method /*06000137*/ public hidebysig newslot virtual final 
          instance int32  GetHashCode(class System.Xml.Linq.XNode/*02000016*/ obj) cil managed
  // SIG: 20 01 08 12 58
  {
    .custom /*0C00015E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2472
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNodeEqualityComparer::GetHashCode

  .method /*06000138*/ private hidebysig newslot virtual final 
          instance bool  System.Collections.IEqualityComparer.Equals(object x,
                                                                     object y) cil managed
  // SIG: 20 02 02 1C 1C
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEqualityComparer/*0100003B*/::Equals /*0100003B::0A000020*/ 
    // Method begins at RVA 0x2475
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNodeEqualityComparer::System.Collections.IEqualityComparer.Equals

  .method /*06000139*/ private hidebysig newslot virtual final 
          instance int32  System.Collections.IEqualityComparer.GetHashCode(object obj) cil managed
  // SIG: 20 01 08 1C
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEqualityComparer/*0100003B*/::GetHashCode /*0100003B::0A000021*/ 
    // Method begins at RVA 0x2478
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XNodeEqualityComparer::System.Collections.IEqualityComparer.GetHashCode

} // end of class System.Xml.Linq.XNodeEqualityComparer

.class /*02000019*/ public abstract auto ansi beforefieldinit System.Xml.Linq.XObject
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements [System.Xml.ReaderWriter/*23000002*/]System.Xml.IXmlLineInfo/*0100003D*/
{
  .custom /*0C000079:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00007A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Xml.ReaderWriter/*23000002*/]System.Xml.IXmlLineInfo/*0100003D*/
  .custom /*0C00003D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600013A*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x247b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XObject::.ctor

  .method /*0600013B*/ public hidebysig specialname 
          instance string  get_BaseUri() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2483
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XObject::get_BaseUri

  .method /*0600013C*/ public hidebysig specialname 
          instance class System.Xml.Linq.XDocument/*02000011*/ 
          get_Document() cil managed
  // SIG: 20 00 12 44
  {
    .custom /*0C000160:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2486
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XObject::get_Document

  .method /*0600013D*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 7D
  {
    // Method begins at RVA 0x0
  } // end of method XObject::get_NodeType

  .method /*0600013E*/ public hidebysig specialname 
          instance class System.Xml.Linq.XElement/*02000013*/ 
          get_Parent() cil managed
  // SIG: 20 00 12 4C
  {
    .custom /*0C000161:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2489
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XObject::get_Parent

  .method /*0600013F*/ private hidebysig newslot specialname virtual final 
          instance int32  System.Xml.IXmlLineInfo.get_LineNumber() cil managed
  // SIG: 20 00 08
  {
    .override [System.Xml.ReaderWriter/*23000002*/]System.Xml.IXmlLineInfo/*0100003D*/::get_LineNumber /*0100003D::0A000022*/ 
    // Method begins at RVA 0x248c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XObject::System.Xml.IXmlLineInfo.get_LineNumber

  .method /*06000140*/ private hidebysig newslot specialname virtual final 
          instance int32  System.Xml.IXmlLineInfo.get_LinePosition() cil managed
  // SIG: 20 00 08
  {
    .override [System.Xml.ReaderWriter/*23000002*/]System.Xml.IXmlLineInfo/*0100003D*/::get_LinePosition /*0100003D::0A000023*/ 
    // Method begins at RVA 0x248f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XObject::System.Xml.IXmlLineInfo.get_LinePosition

  .method /*06000141*/ public hidebysig specialname 
          instance void  add_Changed(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100003E*/<class System.Xml.Linq.XObjectChangeEventArgs/*0200001B*/> 'value') cil managed
  // SIG: 20 01 01 15 12 80 F9 01 12 6C
  {
    // Method begins at RVA 0x2492
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XObject::add_Changed

  .method /*06000142*/ public hidebysig specialname 
          instance void  remove_Changed(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100003E*/<class System.Xml.Linq.XObjectChangeEventArgs/*0200001B*/> 'value') cil managed
  // SIG: 20 01 01 15 12 80 F9 01 12 6C
  {
    // Method begins at RVA 0x2494
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XObject::remove_Changed

  .method /*06000143*/ public hidebysig specialname 
          instance void  add_Changing(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100003E*/<class System.Xml.Linq.XObjectChangeEventArgs/*0200001B*/> 'value') cil managed
  // SIG: 20 01 01 15 12 80 F9 01 12 6C
  {
    // Method begins at RVA 0x2496
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XObject::add_Changing

  .method /*06000144*/ public hidebysig specialname 
          instance void  remove_Changing(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100003E*/<class System.Xml.Linq.XObjectChangeEventArgs/*0200001B*/> 'value') cil managed
  // SIG: 20 01 01 15 12 80 F9 01 12 6C
  {
    // Method begins at RVA 0x2498
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XObject::remove_Changing

  .method /*06000145*/ public hidebysig instance void 
          AddAnnotation(object annotation) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x249a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XObject::AddAnnotation

  .method /*06000146*/ public hidebysig instance object 
          Annotation(class [System.Runtime/*23000001*/]System.Type/*0100003F*/ 'type') cil managed
  // SIG: 20 01 1C 12 80 FD
  {
    .param [0]/*08000169*/ 
    .custom /*0C00016D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x249c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XObject::Annotation

  .method /*06000147*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<object> 
          Annotations(class [System.Runtime/*23000001*/]System.Type/*0100003F*/ 'type') cil managed
  // SIG: 20 01 15 12 6D 01 1C 12 80 FD
  {
    // Method begins at RVA 0x249f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XObject::Annotations

  .method /*06000148*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!T> 
          Annotations<class T>() cil managed
  // SIG: 30 01 00 15 12 6D 01 1E 00
  {
    // Method begins at RVA 0x24a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XObject::Annotations

  .method /*06000149*/ public hidebysig instance !!T 
          Annotation<class T>() cil managed
  // SIG: 30 01 00 1E 00
  {
    .param [0]/*0800016C*/ 
    .custom /*0C00016E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XObject::Annotation

  .method /*0600014A*/ public hidebysig instance void 
          RemoveAnnotations(class [System.Runtime/*23000001*/]System.Type/*0100003F*/ 'type') cil managed
  // SIG: 20 01 01 12 80 FD
  {
    // Method begins at RVA 0x24a8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XObject::RemoveAnnotations

  .method /*0600014B*/ public hidebysig instance void 
          RemoveAnnotations<class T>() cil managed
  // SIG: 30 01 00 01
  {
    // Method begins at RVA 0x24aa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XObject::RemoveAnnotations

  .method /*0600014C*/ private hidebysig newslot virtual final 
          instance bool  System.Xml.IXmlLineInfo.HasLineInfo() cil managed
  // SIG: 20 00 02
  {
    .override [System.Xml.ReaderWriter/*23000002*/]System.Xml.IXmlLineInfo/*0100003D*/::HasLineInfo /*0100003D::0A000024*/ 
    // Method begins at RVA 0x24ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XObject::System.Xml.IXmlLineInfo.HasLineInfo

  .event /*14000001*/ class [System.Runtime/*23000001*/]System.EventHandler`1/*0100003E*/<class System.Xml.Linq.XObjectChangeEventArgs/*0200001B*/>/*1B000004*/ Changed
  {
    .addon instance void System.Xml.Linq.XObject/*02000019*/::add_Changed(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100003E*/<class System.Xml.Linq.XObjectChangeEventArgs/*0200001B*/>) /* 06000141 */
    .removeon instance void System.Xml.Linq.XObject/*02000019*/::remove_Changed(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100003E*/<class System.Xml.Linq.XObjectChangeEventArgs/*0200001B*/>) /* 06000142 */
  } // end of event XObject::Changed
  .event /*14000002*/ class [System.Runtime/*23000001*/]System.EventHandler`1/*0100003E*/<class System.Xml.Linq.XObjectChangeEventArgs/*0200001B*/>/*1B000004*/ Changing
  {
    .addon instance void System.Xml.Linq.XObject/*02000019*/::add_Changing(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100003E*/<class System.Xml.Linq.XObjectChangeEventArgs/*0200001B*/>) /* 06000143 */
    .removeon instance void System.Xml.Linq.XObject/*02000019*/::remove_Changing(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100003E*/<class System.Xml.Linq.XObjectChangeEventArgs/*0200001B*/>) /* 06000144 */
  } // end of event XObject::Changing
  .property /*1700002D*/ instance callconv(8) string
          BaseUri()
  {
    .get instance string System.Xml.Linq.XObject/*02000019*/::get_BaseUri() /* 0600013B */
  } // end of property XObject::BaseUri
  .property /*1700002E*/ instance callconv(8) class System.Xml.Linq.XDocument/*02000011*/
          Document()
  {
    .custom /*0C0000A7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Xml.Linq.XDocument/*02000011*/ System.Xml.Linq.XObject/*02000019*/::get_Document() /* 0600013C */
  } // end of property XObject::Document
  .property /*1700002F*/ instance callconv(8) valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/
          NodeType()
  {
    .get instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/ System.Xml.Linq.XObject/*02000019*/::get_NodeType() /* 0600013D */
  } // end of property XObject::NodeType
  .property /*17000030*/ instance callconv(8) class System.Xml.Linq.XElement/*02000013*/
          Parent()
  {
    .custom /*0C0000AC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Xml.Linq.XElement/*02000013*/ System.Xml.Linq.XObject/*02000019*/::get_Parent() /* 0600013E */
  } // end of property XObject::Parent
  .property /*17000031*/ instance callconv(8) int32
          System.Xml.IXmlLineInfo.LineNumber()
  {
    .get instance int32 System.Xml.Linq.XObject/*02000019*/::System.Xml.IXmlLineInfo.get_LineNumber() /* 0600013F */
  } // end of property XObject::System.Xml.IXmlLineInfo.LineNumber
  .property /*17000032*/ instance callconv(8) int32
          System.Xml.IXmlLineInfo.LinePosition()
  {
    .get instance int32 System.Xml.Linq.XObject/*02000019*/::System.Xml.IXmlLineInfo.get_LinePosition() /* 06000140 */
  } // end of property XObject::System.Xml.IXmlLineInfo.LinePosition
} // end of class System.Xml.Linq.XObject

.class /*0200001A*/ public auto ansi sealed System.Xml.Linq.XObjectChange
       extends [System.Runtime/*23000001*/]System.Enum/*0100001D*/
{
  .field /*04000011*/ public specialname rtspecialname int32 value__
  .field /*04000012*/ public static literal valuetype System.Xml.Linq.XObjectChange/*0200001A*/ Add = int32(0x00000000)
  .field /*04000013*/ public static literal valuetype System.Xml.Linq.XObjectChange/*0200001A*/ Remove = int32(0x00000001)
  .field /*04000014*/ public static literal valuetype System.Xml.Linq.XObjectChange/*0200001A*/ Name = int32(0x00000002)
  .field /*04000015*/ public static literal valuetype System.Xml.Linq.XObjectChange/*0200001A*/ Value = int32(0x00000003)
} // end of class System.Xml.Linq.XObjectChange

.class /*0200001B*/ public auto ansi beforefieldinit System.Xml.Linq.XObjectChangeEventArgs
       extends [System.Runtime/*23000001*/]System.EventArgs/*01000040*/
{
  .custom /*0C00007E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00007F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000016*/ public static initonly class System.Xml.Linq.XObjectChangeEventArgs/*0200001B*/ Add
  .field /*04000017*/ public static initonly class System.Xml.Linq.XObjectChangeEventArgs/*0200001B*/ Name
  .field /*04000018*/ public static initonly class System.Xml.Linq.XObjectChangeEventArgs/*0200001B*/ Remove
  .field /*04000019*/ public static initonly class System.Xml.Linq.XObjectChangeEventArgs/*0200001B*/ Value
  .method /*0600014D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Xml.Linq.XObjectChange/*0200001A*/ objectChange) cil managed
  // SIG: 20 01 01 11 68
  {
    // Method begins at RVA 0x24af
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000027       */ call       instance void [System.Runtime/*23000001*/]System.EventArgs/*01000040*/::.ctor() /* 0A000027 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XObjectChangeEventArgs::.ctor

  .method /*0600014E*/ public hidebysig specialname 
          instance valuetype System.Xml.Linq.XObjectChange/*0200001A*/ 
          get_ObjectChange() cil managed
  // SIG: 20 00 11 68
  {
    // Method begins at RVA 0x24b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XObjectChangeEventArgs::get_ObjectChange

  .property /*17000033*/ instance callconv(8) valuetype System.Xml.Linq.XObjectChange/*0200001A*/
          ObjectChange()
  {
    .get instance valuetype System.Xml.Linq.XObjectChange/*0200001A*/ System.Xml.Linq.XObjectChangeEventArgs/*0200001B*/::get_ObjectChange() /* 0600014E */
  } // end of property XObjectChangeEventArgs::ObjectChange
} // end of class System.Xml.Linq.XObjectChangeEventArgs

.class /*0200001C*/ public auto ansi beforefieldinit System.Xml.Linq.XProcessingInstruction
       extends System.Xml.Linq.XNode/*02000016*/
{
  .custom /*0C000081:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000082:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600014F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string target,
                               string data) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x24ba
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000112       */ call       instance void System.Xml.Linq.XNode/*02000016*/::.ctor() /* 06000112 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XProcessingInstruction::.ctor

  .method /*06000150*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XProcessingInstruction/*0200001C*/ other) cil managed
  // SIG: 20 01 01 12 70
  {
    // Method begins at RVA 0x24c2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000112       */ call       instance void System.Xml.Linq.XNode/*02000016*/::.ctor() /* 06000112 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XProcessingInstruction::.ctor

  .method /*06000151*/ public hidebysig specialname 
          instance string  get_Data() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x24ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XProcessingInstruction::get_Data

  .method /*06000152*/ public hidebysig specialname 
          instance void  set_Data(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x24cd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XProcessingInstruction::set_Data

  .method /*06000153*/ public hidebysig specialname virtual 
          instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 7D
  {
    // Method begins at RVA 0x24cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XProcessingInstruction::get_NodeType

  .method /*06000154*/ public hidebysig specialname 
          instance string  get_Target() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x24d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XProcessingInstruction::get_Target

  .method /*06000155*/ public hidebysig specialname 
          instance void  set_Target(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x24d5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XProcessingInstruction::set_Target

  .method /*06000156*/ public hidebysig virtual 
          instance void  WriteTo(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer) cil managed
  // SIG: 20 01 01 12 80 9D
  {
    // Method begins at RVA 0x24d7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XProcessingInstruction::WriteTo

  .method /*06000157*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          WriteToAsync(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer,
                       valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 20 02 12 80 A1 12 80 9D 11 80 A5
  {
    // Method begins at RVA 0x24d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XProcessingInstruction::WriteToAsync

  .property /*17000034*/ instance callconv(8) string
          Data()
  {
    .get instance string System.Xml.Linq.XProcessingInstruction/*0200001C*/::get_Data() /* 06000151 */
    .set instance void System.Xml.Linq.XProcessingInstruction/*0200001C*/::set_Data(string) /* 06000152 */
  } // end of property XProcessingInstruction::Data
  .property /*17000035*/ instance callconv(8) valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/
          NodeType()
  {
    .get instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/ System.Xml.Linq.XProcessingInstruction/*0200001C*/::get_NodeType() /* 06000153 */
  } // end of property XProcessingInstruction::NodeType
  .property /*17000036*/ instance callconv(8) string
          Target()
  {
    .get instance string System.Xml.Linq.XProcessingInstruction/*0200001C*/::get_Target() /* 06000154 */
    .set instance void System.Xml.Linq.XProcessingInstruction/*0200001C*/::set_Target(string) /* 06000155 */
  } // end of property XProcessingInstruction::Target
} // end of class System.Xml.Linq.XProcessingInstruction

.class /*0200001D*/ public auto ansi beforefieldinit System.Xml.Linq.XStreamingElement
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000085:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000086:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000158*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XName/*02000014*/ name) cil managed
  // SIG: 20 01 01 12 50
  {
    // Method begins at RVA 0x24dc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XStreamingElement::.ctor

  .method /*06000159*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XName/*02000014*/ name,
                               object content) cil managed
  // SIG: 20 02 01 12 50 1C
  {
    .param [2]/*08000179*/ 
    .custom /*0C00016F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24e4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XStreamingElement::.ctor

  .method /*0600015A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XName/*02000014*/ name,
                               object[] content) cil managed
  // SIG: 20 02 01 12 50 1D 1C
  {
    .param [2]/*0800017B*/ 
    .custom /*0C000170:0A000016*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100002A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C000171:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x24ec
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XStreamingElement::.ctor

  .method /*0600015B*/ public hidebysig specialname 
          instance class System.Xml.Linq.XName/*02000014*/ 
          get_Name() cil managed
  // SIG: 20 00 12 50
  {
    // Method begins at RVA 0x24f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XStreamingElement::get_Name

  .method /*0600015C*/ public hidebysig specialname 
          instance void  set_Name(class System.Xml.Linq.XName/*02000014*/ 'value') cil managed
  // SIG: 20 01 01 12 50
  {
    // Method begins at RVA 0x24f7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XStreamingElement::set_Name

  .method /*0600015D*/ public hidebysig instance void 
          Add(object content) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C00016C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24f9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XStreamingElement::Add

  .method /*0600015E*/ public hidebysig instance void 
          Add(object[] content) cil managed
  // SIG: 20 01 01 1D 1C
  {
    .param [1]/*0800017E*/ 
    .custom /*0C000172:0A000016*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100002A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    .custom /*0C000173:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x24fb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XStreamingElement::Add

  .method /*0600015F*/ public hidebysig instance void 
          Save(class [System.Runtime/*23000001*/]System.IO.Stream/*0100002B*/ 'stream') cil managed
  // SIG: 20 01 01 12 80 AD
  {
    // Method begins at RVA 0x24fd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XStreamingElement::Save

  .method /*06000160*/ public hidebysig instance void 
          Save(class [System.Runtime/*23000001*/]System.IO.Stream/*0100002B*/ 'stream',
               valuetype System.Xml.Linq.SaveOptions/*0200000B*/ options) cil managed
  // SIG: 20 02 01 12 80 AD 11 2C
  {
    // Method begins at RVA 0x24ff
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XStreamingElement::Save

  .method /*06000161*/ public hidebysig instance void 
          Save(class [System.Runtime/*23000001*/]System.IO.TextWriter/*01000030*/ textWriter) cil managed
  // SIG: 20 01 01 12 80 C1
  {
    // Method begins at RVA 0x2501
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XStreamingElement::Save

  .method /*06000162*/ public hidebysig instance void 
          Save(class [System.Runtime/*23000001*/]System.IO.TextWriter/*01000030*/ textWriter,
               valuetype System.Xml.Linq.SaveOptions/*0200000B*/ options) cil managed
  // SIG: 20 02 01 12 80 C1 11 2C
  {
    // Method begins at RVA 0x2503
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XStreamingElement::Save

  .method /*06000163*/ public hidebysig instance void 
          Save(string fileName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2505
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XStreamingElement::Save

  .method /*06000164*/ public hidebysig instance void 
          Save(string fileName,
               valuetype System.Xml.Linq.SaveOptions/*0200000B*/ options) cil managed
  // SIG: 20 02 01 0E 11 2C
  {
    // Method begins at RVA 0x2507
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XStreamingElement::Save

  .method /*06000165*/ public hidebysig instance void 
          Save(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer) cil managed
  // SIG: 20 01 01 12 80 9D
  {
    // Method begins at RVA 0x2509
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XStreamingElement::Save

  .method /*06000166*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x250b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XStreamingElement::ToString

  .method /*06000167*/ public hidebysig instance string 
          ToString(valuetype System.Xml.Linq.SaveOptions/*0200000B*/ options) cil managed
  // SIG: 20 01 0E 11 2C
  {
    // Method begins at RVA 0x250e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XStreamingElement::ToString

  .method /*06000168*/ public hidebysig instance void 
          WriteTo(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer) cil managed
  // SIG: 20 01 01 12 80 9D
  {
    // Method begins at RVA 0x2511
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XStreamingElement::WriteTo

  .property /*17000037*/ instance callconv(8) class System.Xml.Linq.XName/*02000014*/
          Name()
  {
    .get instance class System.Xml.Linq.XName/*02000014*/ System.Xml.Linq.XStreamingElement/*0200001D*/::get_Name() /* 0600015B */
    .set instance void System.Xml.Linq.XStreamingElement/*0200001D*/::set_Name(class System.Xml.Linq.XName/*02000014*/) /* 0600015C */
  } // end of property XStreamingElement::Name
} // end of class System.Xml.Linq.XStreamingElement

.class /*0200001E*/ public auto ansi beforefieldinit System.Xml.Linq.XText
       extends System.Xml.Linq.XNode/*02000016*/
{
  .custom /*0C000088:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000089:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000169*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2513
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000112       */ call       instance void System.Xml.Linq.XNode/*02000016*/::.ctor() /* 06000112 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XText::.ctor

  .method /*0600016A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Xml.Linq.XText/*0200001E*/ other) cil managed
  // SIG: 20 01 01 12 78
  {
    // Method begins at RVA 0x251b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000112       */ call       instance void System.Xml.Linq.XNode/*02000016*/::.ctor() /* 06000112 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method XText::.ctor

  .method /*0600016B*/ public hidebysig specialname virtual 
          instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/ 
          get_NodeType() cil managed
  // SIG: 20 00 11 7D
  {
    // Method begins at RVA 0x2523
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XText::get_NodeType

  .method /*0600016C*/ public hidebysig specialname 
          instance string  get_Value() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2526
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XText::get_Value

  .method /*0600016D*/ public hidebysig specialname 
          instance void  set_Value(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2529
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XText::set_Value

  .method /*0600016E*/ public hidebysig virtual 
          instance void  WriteTo(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer) cil managed
  // SIG: 20 01 01 12 80 9D
  {
    // Method begins at RVA 0x252b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method XText::WriteTo

  .method /*0600016F*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000028*/ 
          WriteToAsync(class [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlWriter/*01000027*/ writer,
                       valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000029*/ cancellationToken) cil managed
  // SIG: 20 02 12 80 A1 12 80 9D 11 80 A5
  {
    // Method begins at RVA 0x252d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method XText::WriteToAsync

  .property /*17000038*/ instance callconv(8) valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/
          NodeType()
  {
    .get instance valuetype [System.Xml.ReaderWriter/*23000002*/]System.Xml.XmlNodeType/*0100001F*/ System.Xml.Linq.XText/*0200001E*/::get_NodeType() /* 0600016B */
  } // end of property XText::NodeType
  .property /*17000039*/ instance callconv(8) string
          Value()
  {
    .get instance string System.Xml.Linq.XText/*0200001E*/::get_Value() /* 0600016C */
    .set instance void System.Xml.Linq.XText/*0200001E*/::set_Value(string) /* 0600016D */
  } // end of property XText::Value
} // end of class System.Xml.Linq.XText


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
