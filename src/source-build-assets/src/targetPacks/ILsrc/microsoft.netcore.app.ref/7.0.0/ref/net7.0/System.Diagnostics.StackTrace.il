


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
// Time-date stamp:            0xcef7dcd3
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00002a00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000493e
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
// Checksum:                       0x0000f3e3
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x000048eb [0x0000004f] address [size] of Import Directory:          
// 0x00006000 [0x000004c4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00003400 [0x000028a0] address [size] of Security Directory:        
// 0x00008000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x000047f4 [0x00000054] address [size] of Debug Directory:           
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
//              0x00002944 Virtual Size
//              0x00002000 Virtual Address
//              0x00002a00 Size of Raw Data
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
//              0x00002c00 Pointer to Raw Data
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
//              0x00003200 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000940
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000492d Import Name Table
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
// 0x00002184 [0x000025f0] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00004774 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00001150 Size
//              '#~' Name
//    Stream 2:
//              0x000011bc Offset
//              0x00000d54 Size
//              '#Strings' Name
//    Stream 3:
//              0x00001f10 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00001f14 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001f24 Offset
//              0x000006cc Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000000909a01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 23712
// PE header size       : 512 (496 used)    ( 2.16%)
// PE additional info   : 11875             (50.08%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.30%)
// CLR meta-data size  : 9712               (40.96%)
// CLR additional info : 128                ( 0.54%)
// CLR method headers  : 48                 ( 0.20%)
// Managed code         : 260               ( 1.10%)
// Data                 : 2048              ( 8.64%)
// Unaccounted          : -943              (-3.98%)

// Num.of PE sections   : 3
//   .text    - 10752
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 9712
//   Module        -    1 (10 bytes)
//   TypeDef       -   24 (336 bytes)     9 interfaces, 0 explicit layout
//   TypeRef       -   35 (210 bytes)
//   MethodDef     -  121 (1694 bytes)    73 abstract, 0 native, 48 bodies
//   FieldDef      -   31 (186 bytes)     13 constant
//   MemberRef     -   24 (144 bytes)
//   ParamDef      -  145 (870 bytes)
//   Constant      -   12 (72 bytes)
//   CustomAttribute-   74 (444 bytes)
//   InterfaceImpl -    1 (4 bytes)
//   PropertyMap   -    7 (28 bytes)
//   Property      -   25 (150 bytes)
//   MethodSemantic-   25 (150 bytes)
//   TypeSpec      -    1 (2 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    1 (20 bytes)
//   Strings       -  3409 bytes
//   Blobs         -  1740 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   201 bytes

// CLR additional info : 128

// CLR method headers : 48
//   Num.of method bodies  - 48
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 48

// Managed code : 260
//   Ave method size - 5
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class RefSafetyRulesAttribute        (auto) (ansi) (sealed) 
// Class StackFrame                     (public) (auto) (ansi) 
// Class StackFrameExtensions           (public) (abstract) (auto) (ansi) (sealed) 
// Class StackTrace                     (public) (auto) (ansi) 
// Interface ISymbolBinder                  (public) (abstract) (auto) (ansi) 
// Interface ISymbolBinder1                 (public) (abstract) (auto) (ansi) 
// Interface ISymbolDocument                (public) (abstract) (auto) (ansi) 
// Interface ISymbolMethod                  (public) (abstract) (auto) (ansi) 
// Interface ISymbolNamespace               (public) (abstract) (auto) (ansi) 
// Interface ISymbolReader                  (public) (abstract) (auto) (ansi) 
// Interface ISymbolScope                   (public) (abstract) (auto) (ansi) 
// Interface ISymbolVariable                (public) (abstract) (auto) (ansi) 
// Interface ISymbolWriter                  (public) (abstract) (auto) (ansi) 
// Class SymAddressKind                 (public) (auto) (ansi) (sealed) 
// Class SymbolToken                    (public) (sequential) (ansi) (sealed) 
// Class SymDocumentType                (public) (auto) (ansi) 
// Class SymLanguageType                (public) (auto) (ansi) 
// Class SymLanguageVendor              (public) (auto) (ansi) 
// Interface ISymbolDocumentWriter          (public) (abstract) (auto) (ansi) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly /*20000001*/ noplatform System.Diagnostics.StackTrace
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
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 37 2E 30 01 00 54 0E 14   // ersion=v7.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 37 2E 30 )          // Name..NET 7.0
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000007*/::.ctor() /* 0A000006 */ = ( 01 00 00 00 ) 
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 1D 53 79 73 74 65 6D 2E 44 69 61 67 6E 6F   // ...System.Diagno
                                                                                                                                                                        73 74 69 63 73 2E 53 74 61 63 6B 54 72 61 63 65   // stics.StackTrace
                                                                                                                                                                        00 00 ) 
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000E:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000F:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000010:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 1D 53 79 73 74 65 6D 2E 44 69 61 67 6E 6F   // ...System.Diagno
                                                                                                                                                                       73 74 69 63 73 2E 53 74 61 63 6B 54 72 61 63 65   // stics.StackTrace
                                                                                                                                                                       00 00 ) 
  .custom /*0C000011:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 37 2E 30 2E 32 32 2E 35 31 38 30 35 00   // ...7.0.22.51805.
                                                                                                                                                                       00 ) 
  .custom /*0C000012:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 37 2E 30 2E 30 2B 64 30 39 39 66 30 37   // ...7.0.0+d099f07
                                                                                                                                                                                35 65 34 35 64 32 61 61 36 30 30 37 61 32 32 62   // 5e45d2aa6007a22b
                                                                                                                                                                                37 31 62 34 35 61 30 38 37 35 38 35 35 39 66 38   // 71b45a08758559f8
                                                                                                                                                                                30 00 00 )                                        // 0..
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000014:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 1D 53 79 73 74 65 6D 2E 44 69 61 67 6E 6F   // ...System.Diagno
                                                                                                                                                                 73 74 69 63 73 2E 53 74 61 63 6B 54 72 61 63 65   // stics.StackTrace
                                                                                                                                                                 00 00 ) 
  .custom /*0C000015:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
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
.module System.Diagnostics.StackTrace.dll
// MVID: {0a3025c3-64f7-4892-9396-0668fe6b3430}
.custom /*0C000001:06000006*/ instance void System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::.ctor(int32) /* 06000006 */ = ( 01 00 0B 00 00 00 00 00 ) 
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
  .custom /*0C000016:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000017:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000018:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000019:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001A:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000017 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)000023       */ newarr     [System.Runtime/*23000001*/]System.Byte/*01000023*/
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
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000017 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C00001B:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C00001C:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001D:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000017 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C00001E:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000020:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000017 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.RefSafetyRulesAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000021:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000022:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000023:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000017 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000004       */ stfld      int32 System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::Version /* 04000004 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method RefSafetyRulesAttribute::.ctor

} // end of class System.Runtime.CompilerServices.RefSafetyRulesAttribute

.class /*02000007*/ public auto ansi beforefieldinit System.Diagnostics.StackFrame
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000024:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000025:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000005*/ public static literal int32 OFFSET_UNKNOWN = int32(0xFFFFFFFF)
  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ac
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StackFrame::.ctor

  .method /*06000008*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool needFileInfo) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20b4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StackFrame::.ctor

  .method /*06000009*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 skipFrames) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20bc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StackFrame::.ctor

  .method /*0600000A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 skipFrames,
                               bool needFileInfo) cil managed
  // SIG: 20 02 01 08 02
  {
    // Method begins at RVA 0x20c4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StackFrame::.ctor

  .method /*0600000B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName,
                               int32 lineNumber) cil managed
  // SIG: 20 02 01 0E 08
  {
    // Method begins at RVA 0x20cc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StackFrame::.ctor

  .method /*0600000C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName,
                               int32 lineNumber,
                               int32 colNumber) cil managed
  // SIG: 20 03 01 0E 08 08
  {
    // Method begins at RVA 0x20d4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StackFrame::.ctor

  .method /*0600000D*/ public hidebysig newslot virtual 
          instance int32  GetFileColumnNumber() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrame::GetFileColumnNumber

  .method /*0600000E*/ public hidebysig newslot virtual 
          instance int32  GetFileLineNumber() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrame::GetFileLineNumber

  .method /*0600000F*/ public hidebysig newslot virtual 
          instance string  GetFileName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrame::GetFileName

  .method /*06000010*/ public hidebysig newslot virtual 
          instance int32  GetILOffset() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrame::GetILOffset

  .method /*06000011*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodBase/*01000018*/ 
          GetMethod() cil managed
  // SIG: 20 00 12 61
  {
    .custom /*0C000032:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 36 4D 65 74 61 64 61 74 61 20 66 6F 72 20   // ..6Metadata for 
                                                                                                                                                                                            74 68 65 20 6D 65 74 68 6F 64 20 6D 69 67 68 74   // the method might
                                                                                                                                                                                            20 62 65 20 69 6E 63 6F 6D 70 6C 65 74 65 20 6F   //  be incomplete o
                                                                                                                                                                                            72 20 72 65 6D 6F 76 65 64 00 00 )                // r removed..
    // Method begins at RVA 0x20e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrame::GetMethod

  .method /*06000012*/ public hidebysig newslot virtual 
          instance int32  GetNativeOffset() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrame::GetNativeOffset

  .method /*06000013*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000035:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrame::ToString

} // end of class System.Diagnostics.StackFrame

.class /*02000008*/ public abstract auto ansi sealed beforefieldinit System.Diagnostics.StackFrameExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000026:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000027:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000028:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000014*/ public hidebysig static 
          native int  GetNativeImageBase(class System.Diagnostics.StackFrame/*02000007*/ stackFrame) cil managed
  // SIG: 00 01 18 12 1C
  {
    .custom /*0C000036:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameExtensions::GetNativeImageBase

  .method /*06000015*/ public hidebysig static 
          native int  GetNativeIP(class System.Diagnostics.StackFrame/*02000007*/ stackFrame) cil managed
  // SIG: 00 01 18 12 1C
  {
    .custom /*0C000038:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameExtensions::GetNativeIP

  .method /*06000016*/ public hidebysig static 
          bool  HasILOffset(class System.Diagnostics.StackFrame/*02000007*/ stackFrame) cil managed
  // SIG: 00 01 02 12 1C
  {
    .custom /*0C000039:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameExtensions::HasILOffset

  .method /*06000017*/ public hidebysig static 
          bool  HasMethod(class System.Diagnostics.StackFrame/*02000007*/ stackFrame) cil managed
  // SIG: 00 01 02 12 1C
  {
    .custom /*0C00003A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameExtensions::HasMethod

  .method /*06000018*/ public hidebysig static 
          bool  HasNativeImage(class System.Diagnostics.StackFrame/*02000007*/ stackFrame) cil managed
  // SIG: 00 01 02 12 1C
  {
    .custom /*0C00003B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameExtensions::HasNativeImage

  .method /*06000019*/ public hidebysig static 
          bool  HasSource(class System.Diagnostics.StackFrame/*02000007*/ stackFrame) cil managed
  // SIG: 00 01 02 12 1C
  {
    .custom /*0C00003D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2100
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackFrameExtensions::HasSource

} // end of class System.Diagnostics.StackFrameExtensions

.class /*02000009*/ public auto ansi beforefieldinit System.Diagnostics.StackTrace
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000029:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000006*/ public static literal int32 METHODS_TO_SKIP = int32(0x00000000)
  .method /*0600001A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2103
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StackTrace::.ctor

  .method /*0600001B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool fNeedFileInfo) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x210b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StackTrace::.ctor

  .method /*0600001C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.StackFrame/*02000007*/ frame) cil managed
  // SIG: 20 01 01 12 1C
  {
    // Method begins at RVA 0x2113
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StackTrace::.ctor

  .method /*0600001D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Exception/*01000019*/ e) cil managed
  // SIG: 20 01 01 12 65
  {
    // Method begins at RVA 0x211b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StackTrace::.ctor

  .method /*0600001E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Exception/*01000019*/ e,
                               bool fNeedFileInfo) cil managed
  // SIG: 20 02 01 12 65 02
  {
    // Method begins at RVA 0x2123
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StackTrace::.ctor

  .method /*0600001F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Exception/*01000019*/ e,
                               int32 skipFrames) cil managed
  // SIG: 20 02 01 12 65 08
  {
    // Method begins at RVA 0x212b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StackTrace::.ctor

  .method /*06000020*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Exception/*01000019*/ e,
                               int32 skipFrames,
                               bool fNeedFileInfo) cil managed
  // SIG: 20 03 01 12 65 08 02
  {
    // Method begins at RVA 0x2133
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StackTrace::.ctor

  .method /*06000021*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 skipFrames) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x213b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StackTrace::.ctor

  .method /*06000022*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 skipFrames,
                               bool fNeedFileInfo) cil managed
  // SIG: 20 02 01 08 02
  {
    // Method begins at RVA 0x2143
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method StackTrace::.ctor

  .method /*06000023*/ public hidebysig newslot specialname virtual 
          instance int32  get_FrameCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x214b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackTrace::get_FrameCount

  .method /*06000024*/ public hidebysig newslot virtual 
          instance class System.Diagnostics.StackFrame/*02000007*/ 
          GetFrame(int32 index) cil managed
  // SIG: 20 01 12 1C 08
  {
    .custom /*0C000040:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x214e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackTrace::GetFrame

  .method /*06000025*/ public hidebysig newslot virtual 
          instance class System.Diagnostics.StackFrame/*02000007*/[] 
          GetFrames() cil managed
  // SIG: 20 00 1D 12 1C
  {
    // Method begins at RVA 0x2151
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackTrace::GetFrames

  .method /*06000026*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2154
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StackTrace::ToString

  .property /*17000001*/ instance callconv(8) int32
          FrameCount()
  {
    .get instance int32 System.Diagnostics.StackTrace/*02000009*/::get_FrameCount() /* 06000023 */
  } // end of property StackTrace::FrameCount
} // end of class System.Diagnostics.StackTrace

.class /*0200000A*/ interface public abstract auto ansi System.Diagnostics.SymbolStore.ISymbolBinder
{
  .custom /*0C00002B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000027*/ public hidebysig newslot abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolReader/*0200000F*/ 
          GetReader(int32 importer,
                    string filename,
                    string searchPath) cil managed
  // SIG: 20 03 12 3C 08 0E 0E
  {
    .custom /*0C000041:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 81 10 49 53 79 6D 62 6F 6C 42 69 6E 64 65   // ....ISymbolBinde
                                                                                                                                                   72 2E 47 65 74 52 65 61 64 65 72 20 68 61 73 20   // r.GetReader has 
                                                                                                                                                   62 65 65 6E 20 64 65 70 72 65 63 61 74 65 64 20   // been deprecated 
                                                                                                                                                   62 65 63 61 75 73 65 20 69 74 20 69 73 20 6E 6F   // because it is no
                                                                                                                                                   74 20 36 34 2D 62 69 74 20 63 6F 6D 70 61 74 69   // t 64-bit compati
                                                                                                                                                   62 6C 65 2E 20 55 73 65 20 49 53 79 6D 62 6F 6C   // ble. Use ISymbol
                                                                                                                                                   42 69 6E 64 65 72 31 2E 47 65 74 52 65 61 64 65   // Binder1.GetReade
                                                                                                                                                   72 20 69 6E 73 74 65 61 64 2E 20 49 53 79 6D 62   // r instead. ISymb
                                                                                                                                                   6F 6C 42 69 6E 64 65 72 31 2E 47 65 74 52 65 61   // olBinder1.GetRea
                                                                                                                                                   64 65 72 20 61 63 63 65 70 74 73 20 74 68 65 20   // der accepts the 
                                                                                                                                                   69 6D 70 6F 72 74 65 72 20 69 6E 74 65 72 66 61   // importer interfa
                                                                                                                                                   63 65 20 70 6F 69 6E 74 65 72 20 61 73 20 61 6E   // ce pointer as an
                                                                                                                                                   20 49 6E 74 50 74 72 20 69 6E 73 74 65 61 64 20   //  IntPtr instead 
                                                                                                                                                   6F 66 20 61 6E 20 49 6E 74 33 32 2C 20 61 6E 64   // of an Int32, and
                                                                                                                                                   20 74 68 75 73 20 77 6F 72 6B 73 20 6F 6E 20 62   //  thus works on b
                                                                                                                                                   6F 74 68 20 33 32 2D 62 69 74 20 61 6E 64 20 36   // oth 32-bit and 6
                                                                                                                                                   34 2D 62 69 74 20 61 72 63 68 69 74 65 63 74 75   // 4-bit architectu
                                                                                                                                                   72 65 73 2E 00 00 )                               // res...
    .param [0]/*0800001E*/ 
    .custom /*0C00003E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ISymbolBinder::GetReader

} // end of class System.Diagnostics.SymbolStore.ISymbolBinder

.class /*0200000B*/ interface public abstract auto ansi System.Diagnostics.SymbolStore.ISymbolBinder1
{
  .custom /*0C00002C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000028*/ public hidebysig newslot abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolReader/*0200000F*/ 
          GetReader(native int importer,
                    string filename,
                    string searchPath) cil managed
  // SIG: 20 03 12 3C 18 0E 0E
  {
    .param [0]/*08000022*/ 
    .custom /*0C00003F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ISymbolBinder1::GetReader

} // end of class System.Diagnostics.SymbolStore.ISymbolBinder1

.class /*0200000C*/ interface public abstract auto ansi System.Diagnostics.SymbolStore.ISymbolDocument
{
  .custom /*0C00002D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000029*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ 
          get_CheckSumAlgorithmId() cil managed
  // SIG: 20 00 11 6D
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolDocument::get_CheckSumAlgorithmId

  .method /*0600002A*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ 
          get_DocumentType() cil managed
  // SIG: 20 00 11 6D
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolDocument::get_DocumentType

  .method /*0600002B*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_HasEmbeddedSource() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolDocument::get_HasEmbeddedSource

  .method /*0600002C*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ 
          get_Language() cil managed
  // SIG: 20 00 11 6D
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolDocument::get_Language

  .method /*0600002D*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ 
          get_LanguageVendor() cil managed
  // SIG: 20 00 11 6D
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolDocument::get_LanguageVendor

  .method /*0600002E*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_SourceLength() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolDocument::get_SourceLength

  .method /*0600002F*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_URL() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolDocument::get_URL

  .method /*06000030*/ public hidebysig newslot abstract virtual 
          instance int32  FindClosestLine(int32 line) cil managed
  // SIG: 20 01 08 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolDocument::FindClosestLine

  .method /*06000031*/ public hidebysig newslot abstract virtual 
          instance uint8[]  GetCheckSum() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolDocument::GetCheckSum

  .method /*06000032*/ public hidebysig newslot abstract virtual 
          instance uint8[]  GetSourceRange(int32 startLine,
                                           int32 startColumn,
                                           int32 endLine,
                                           int32 endColumn) cil managed
  // SIG: 20 04 1D 05 08 08 08 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolDocument::GetSourceRange

  .property /*17000002*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/
          CheckSumAlgorithmId()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ System.Diagnostics.SymbolStore.ISymbolDocument/*0200000C*/::get_CheckSumAlgorithmId() /* 06000029 */
  } // end of property ISymbolDocument::CheckSumAlgorithmId
  .property /*17000003*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/
          DocumentType()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ System.Diagnostics.SymbolStore.ISymbolDocument/*0200000C*/::get_DocumentType() /* 0600002A */
  } // end of property ISymbolDocument::DocumentType
  .property /*17000004*/ instance callconv(8) bool
          HasEmbeddedSource()
  {
    .get instance bool System.Diagnostics.SymbolStore.ISymbolDocument/*0200000C*/::get_HasEmbeddedSource() /* 0600002B */
  } // end of property ISymbolDocument::HasEmbeddedSource
  .property /*17000005*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/
          Language()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ System.Diagnostics.SymbolStore.ISymbolDocument/*0200000C*/::get_Language() /* 0600002C */
  } // end of property ISymbolDocument::Language
  .property /*17000006*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/
          LanguageVendor()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ System.Diagnostics.SymbolStore.ISymbolDocument/*0200000C*/::get_LanguageVendor() /* 0600002D */
  } // end of property ISymbolDocument::LanguageVendor
  .property /*17000007*/ instance callconv(8) int32
          SourceLength()
  {
    .get instance int32 System.Diagnostics.SymbolStore.ISymbolDocument/*0200000C*/::get_SourceLength() /* 0600002E */
  } // end of property ISymbolDocument::SourceLength
  .property /*17000008*/ instance callconv(8) string
          URL()
  {
    .get instance string System.Diagnostics.SymbolStore.ISymbolDocument/*0200000C*/::get_URL() /* 0600002F */
  } // end of property ISymbolDocument::URL
} // end of class System.Diagnostics.SymbolStore.ISymbolDocument

.class /*0200000D*/ interface public abstract auto ansi System.Diagnostics.SymbolStore.ISymbolMethod
{
  .custom /*0C00002E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000033*/ public hidebysig newslot specialname abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolScope/*02000010*/ 
          get_RootScope() cil managed
  // SIG: 20 00 12 40
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolMethod::get_RootScope

  .method /*06000034*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_SequencePointCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolMethod::get_SequencePointCount

  .method /*06000035*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype System.Diagnostics.SymbolStore.SymbolToken/*02000014*/ 
          get_Token() cil managed
  // SIG: 20 00 11 50
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolMethod::get_Token

  .method /*06000036*/ public hidebysig newslot abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolNamespace/*0200000E*/ 
          GetNamespace() cil managed
  // SIG: 20 00 12 38
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolMethod::GetNamespace

  .method /*06000037*/ public hidebysig newslot abstract virtual 
          instance int32  GetOffset(class System.Diagnostics.SymbolStore.ISymbolDocument/*0200000C*/ document,
                                    int32 line,
                                    int32 column) cil managed
  // SIG: 20 03 08 12 30 08 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolMethod::GetOffset

  .method /*06000038*/ public hidebysig newslot abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolVariable/*02000011*/[] 
          GetParameters() cil managed
  // SIG: 20 00 1D 12 44
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolMethod::GetParameters

  .method /*06000039*/ public hidebysig newslot abstract virtual 
          instance int32[]  GetRanges(class System.Diagnostics.SymbolStore.ISymbolDocument/*0200000C*/ document,
                                      int32 line,
                                      int32 column) cil managed
  // SIG: 20 03 1D 08 12 30 08 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolMethod::GetRanges

  .method /*0600003A*/ public hidebysig newslot abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolScope/*02000010*/ 
          GetScope(int32 offset) cil managed
  // SIG: 20 01 12 40 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolMethod::GetScope

  .method /*0600003B*/ public hidebysig newslot abstract virtual 
          instance void  GetSequencePoints(int32[] offsets,
                                           class System.Diagnostics.SymbolStore.ISymbolDocument/*0200000C*/[] documents,
                                           int32[] lines,
                                           int32[] columns,
                                           int32[] endLines,
                                           int32[] endColumns) cil managed
  // SIG: 20 06 01 1D 08 1D 12 30 1D 08 1D 08 1D 08 1D 08
  {
    .custom /*0C000044:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000033*/ 
    .custom /*0C000042:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ISymbolMethod::GetSequencePoints

  .method /*0600003C*/ public hidebysig newslot abstract virtual 
          instance bool  GetSourceStartEnd(class System.Diagnostics.SymbolStore.ISymbolDocument/*0200000C*/[] docs,
                                           int32[] lines,
                                           int32[] columns) cil managed
  // SIG: 20 03 02 1D 12 30 1D 08 1D 08
  {
    .custom /*0C000046:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000038*/ 
    .custom /*0C000043:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ISymbolMethod::GetSourceStartEnd

  .property /*17000009*/ instance callconv(8) class System.Diagnostics.SymbolStore.ISymbolScope/*02000010*/
          RootScope()
  {
    .get instance class System.Diagnostics.SymbolStore.ISymbolScope/*02000010*/ System.Diagnostics.SymbolStore.ISymbolMethod/*0200000D*/::get_RootScope() /* 06000033 */
  } // end of property ISymbolMethod::RootScope
  .property /*1700000A*/ instance callconv(8) int32
          SequencePointCount()
  {
    .get instance int32 System.Diagnostics.SymbolStore.ISymbolMethod/*0200000D*/::get_SequencePointCount() /* 06000034 */
  } // end of property ISymbolMethod::SequencePointCount
  .property /*1700000B*/ instance callconv(8) valuetype System.Diagnostics.SymbolStore.SymbolToken/*02000014*/
          Token()
  {
    .get instance valuetype System.Diagnostics.SymbolStore.SymbolToken/*02000014*/ System.Diagnostics.SymbolStore.ISymbolMethod/*0200000D*/::get_Token() /* 06000035 */
  } // end of property ISymbolMethod::Token
} // end of class System.Diagnostics.SymbolStore.ISymbolMethod

.class /*0200000E*/ interface public abstract auto ansi System.Diagnostics.SymbolStore.ISymbolNamespace
{
  .custom /*0C00002F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600003D*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolNamespace::get_Name

  .method /*0600003E*/ public hidebysig newslot abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolNamespace/*0200000E*/[] 
          GetNamespaces() cil managed
  // SIG: 20 00 1D 12 38
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolNamespace::GetNamespaces

  .method /*0600003F*/ public hidebysig newslot abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolVariable/*02000011*/[] 
          GetVariables() cil managed
  // SIG: 20 00 1D 12 44
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolNamespace::GetVariables

  .property /*1700000C*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Diagnostics.SymbolStore.ISymbolNamespace/*0200000E*/::get_Name() /* 0600003D */
  } // end of property ISymbolNamespace::Name
} // end of class System.Diagnostics.SymbolStore.ISymbolNamespace

.class /*0200000F*/ interface public abstract auto ansi System.Diagnostics.SymbolStore.ISymbolReader
{
  .custom /*0C000030:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000040*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype System.Diagnostics.SymbolStore.SymbolToken/*02000014*/ 
          get_UserEntryPoint() cil managed
  // SIG: 20 00 11 50
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolReader::get_UserEntryPoint

  .method /*06000041*/ public hidebysig newslot abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolDocument/*0200000C*/ 
          GetDocument(string url,
                      valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ language,
                      valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ languageVendor,
                      valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ documentType) cil managed
  // SIG: 20 04 12 30 0E 11 6D 11 6D 11 6D
  {
    .param [0]/*0800003B*/ 
    .custom /*0C000045:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ISymbolReader::GetDocument

  .method /*06000042*/ public hidebysig newslot abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolDocument/*0200000C*/[] 
          GetDocuments() cil managed
  // SIG: 20 00 1D 12 30
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolReader::GetDocuments

  .method /*06000043*/ public hidebysig newslot abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolVariable/*02000011*/[] 
          GetGlobalVariables() cil managed
  // SIG: 20 00 1D 12 44
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolReader::GetGlobalVariables

  .method /*06000044*/ public hidebysig newslot abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolMethod/*0200000D*/ 
          GetMethod(valuetype System.Diagnostics.SymbolStore.SymbolToken/*02000014*/ 'method') cil managed
  // SIG: 20 01 12 34 11 50
  {
    .custom /*0C000047:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ISymbolReader::GetMethod

  .method /*06000045*/ public hidebysig newslot abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolMethod/*0200000D*/ 
          GetMethod(valuetype System.Diagnostics.SymbolStore.SymbolToken/*02000014*/ 'method',
                    int32 version) cil managed
  // SIG: 20 02 12 34 11 50 08
  {
    .custom /*0C000048:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ISymbolReader::GetMethod

  .method /*06000046*/ public hidebysig newslot abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolMethod/*0200000D*/ 
          GetMethodFromDocumentPosition(class System.Diagnostics.SymbolStore.ISymbolDocument/*0200000C*/ document,
                                        int32 line,
                                        int32 column) cil managed
  // SIG: 20 03 12 34 12 30 08 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolReader::GetMethodFromDocumentPosition

  .method /*06000047*/ public hidebysig newslot abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolNamespace/*0200000E*/[] 
          GetNamespaces() cil managed
  // SIG: 20 00 1D 12 38
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolReader::GetNamespaces

  .method /*06000048*/ public hidebysig newslot abstract virtual 
          instance uint8[]  GetSymAttribute(valuetype System.Diagnostics.SymbolStore.SymbolToken/*02000014*/ parent,
                                            string name) cil managed
  // SIG: 20 02 1D 05 11 50 0E
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolReader::GetSymAttribute

  .method /*06000049*/ public hidebysig newslot abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolVariable/*02000011*/[] 
          GetVariables(valuetype System.Diagnostics.SymbolStore.SymbolToken/*02000014*/ parent) cil managed
  // SIG: 20 01 1D 12 44 11 50
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolReader::GetVariables

  .property /*1700000D*/ instance callconv(8) valuetype System.Diagnostics.SymbolStore.SymbolToken/*02000014*/
          UserEntryPoint()
  {
    .get instance valuetype System.Diagnostics.SymbolStore.SymbolToken/*02000014*/ System.Diagnostics.SymbolStore.ISymbolReader/*0200000F*/::get_UserEntryPoint() /* 06000040 */
  } // end of property ISymbolReader::UserEntryPoint
} // end of class System.Diagnostics.SymbolStore.ISymbolReader

.class /*02000010*/ interface public abstract auto ansi System.Diagnostics.SymbolStore.ISymbolScope
{
  .custom /*0C000031:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600004A*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_EndOffset() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolScope::get_EndOffset

  .method /*0600004B*/ public hidebysig newslot specialname abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolMethod/*0200000D*/ 
          get_Method() cil managed
  // SIG: 20 00 12 34
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolScope::get_Method

  .method /*0600004C*/ public hidebysig newslot specialname abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolScope/*02000010*/ 
          get_Parent() cil managed
  // SIG: 20 00 12 40
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolScope::get_Parent

  .method /*0600004D*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_StartOffset() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolScope::get_StartOffset

  .method /*0600004E*/ public hidebysig newslot abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolScope/*02000010*/[] 
          GetChildren() cil managed
  // SIG: 20 00 1D 12 40
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolScope::GetChildren

  .method /*0600004F*/ public hidebysig newslot abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolVariable/*02000011*/[] 
          GetLocals() cil managed
  // SIG: 20 00 1D 12 44
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolScope::GetLocals

  .method /*06000050*/ public hidebysig newslot abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolNamespace/*0200000E*/[] 
          GetNamespaces() cil managed
  // SIG: 20 00 1D 12 38
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolScope::GetNamespaces

  .property /*1700000E*/ instance callconv(8) int32
          EndOffset()
  {
    .get instance int32 System.Diagnostics.SymbolStore.ISymbolScope/*02000010*/::get_EndOffset() /* 0600004A */
  } // end of property ISymbolScope::EndOffset
  .property /*1700000F*/ instance callconv(8) class System.Diagnostics.SymbolStore.ISymbolMethod/*0200000D*/
          Method()
  {
    .get instance class System.Diagnostics.SymbolStore.ISymbolMethod/*0200000D*/ System.Diagnostics.SymbolStore.ISymbolScope/*02000010*/::get_Method() /* 0600004B */
  } // end of property ISymbolScope::Method
  .property /*17000010*/ instance callconv(8) class System.Diagnostics.SymbolStore.ISymbolScope/*02000010*/
          Parent()
  {
    .get instance class System.Diagnostics.SymbolStore.ISymbolScope/*02000010*/ System.Diagnostics.SymbolStore.ISymbolScope/*02000010*/::get_Parent() /* 0600004C */
  } // end of property ISymbolScope::Parent
  .property /*17000011*/ instance callconv(8) int32
          StartOffset()
  {
    .get instance int32 System.Diagnostics.SymbolStore.ISymbolScope/*02000010*/::get_StartOffset() /* 0600004D */
  } // end of property ISymbolScope::StartOffset
} // end of class System.Diagnostics.SymbolStore.ISymbolScope

.class /*02000011*/ interface public abstract auto ansi System.Diagnostics.SymbolStore.ISymbolVariable
{
  .custom /*0C000033:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000051*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_AddressField1() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolVariable::get_AddressField1

  .method /*06000052*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_AddressField2() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolVariable::get_AddressField2

  .method /*06000053*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_AddressField3() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolVariable::get_AddressField3

  .method /*06000054*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype System.Diagnostics.SymbolStore.SymAddressKind/*02000013*/ 
          get_AddressKind() cil managed
  // SIG: 20 00 11 4C
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolVariable::get_AddressKind

  .method /*06000055*/ public hidebysig newslot specialname abstract virtual 
          instance object  get_Attributes() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolVariable::get_Attributes

  .method /*06000056*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_EndOffset() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolVariable::get_EndOffset

  .method /*06000057*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolVariable::get_Name

  .method /*06000058*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_StartOffset() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolVariable::get_StartOffset

  .method /*06000059*/ public hidebysig newslot abstract virtual 
          instance uint8[]  GetSignature() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolVariable::GetSignature

  .property /*17000012*/ instance callconv(8) int32
          AddressField1()
  {
    .get instance int32 System.Diagnostics.SymbolStore.ISymbolVariable/*02000011*/::get_AddressField1() /* 06000051 */
  } // end of property ISymbolVariable::AddressField1
  .property /*17000013*/ instance callconv(8) int32
          AddressField2()
  {
    .get instance int32 System.Diagnostics.SymbolStore.ISymbolVariable/*02000011*/::get_AddressField2() /* 06000052 */
  } // end of property ISymbolVariable::AddressField2
  .property /*17000014*/ instance callconv(8) int32
          AddressField3()
  {
    .get instance int32 System.Diagnostics.SymbolStore.ISymbolVariable/*02000011*/::get_AddressField3() /* 06000053 */
  } // end of property ISymbolVariable::AddressField3
  .property /*17000015*/ instance callconv(8) valuetype System.Diagnostics.SymbolStore.SymAddressKind/*02000013*/
          AddressKind()
  {
    .get instance valuetype System.Diagnostics.SymbolStore.SymAddressKind/*02000013*/ System.Diagnostics.SymbolStore.ISymbolVariable/*02000011*/::get_AddressKind() /* 06000054 */
  } // end of property ISymbolVariable::AddressKind
  .property /*17000016*/ instance callconv(8) object
          Attributes()
  {
    .get instance object System.Diagnostics.SymbolStore.ISymbolVariable/*02000011*/::get_Attributes() /* 06000055 */
  } // end of property ISymbolVariable::Attributes
  .property /*17000017*/ instance callconv(8) int32
          EndOffset()
  {
    .get instance int32 System.Diagnostics.SymbolStore.ISymbolVariable/*02000011*/::get_EndOffset() /* 06000056 */
  } // end of property ISymbolVariable::EndOffset
  .property /*17000018*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Diagnostics.SymbolStore.ISymbolVariable/*02000011*/::get_Name() /* 06000057 */
  } // end of property ISymbolVariable::Name
  .property /*17000019*/ instance callconv(8) int32
          StartOffset()
  {
    .get instance int32 System.Diagnostics.SymbolStore.ISymbolVariable/*02000011*/::get_StartOffset() /* 06000058 */
  } // end of property ISymbolVariable::StartOffset
} // end of class System.Diagnostics.SymbolStore.ISymbolVariable

.class /*02000012*/ interface public abstract auto ansi System.Diagnostics.SymbolStore.ISymbolWriter
{
  .custom /*0C000034:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600005A*/ public hidebysig newslot abstract virtual 
          instance void  Close() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolWriter::Close

  .method /*0600005B*/ public hidebysig newslot abstract virtual 
          instance void  CloseMethod() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolWriter::CloseMethod

  .method /*0600005C*/ public hidebysig newslot abstract virtual 
          instance void  CloseNamespace() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolWriter::CloseNamespace

  .method /*0600005D*/ public hidebysig newslot abstract virtual 
          instance void  CloseScope(int32 endOffset) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolWriter::CloseScope

  .method /*0600005E*/ public hidebysig newslot abstract virtual 
          instance class System.Diagnostics.SymbolStore.ISymbolDocumentWriter/*02000018*/ 
          DefineDocument(string url,
                         valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ language,
                         valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ languageVendor,
                         valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ documentType) cil managed
  // SIG: 20 04 12 60 0E 11 6D 11 6D 11 6D
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolWriter::DefineDocument

  .method /*0600005F*/ public hidebysig newslot abstract virtual 
          instance void  DefineField(valuetype System.Diagnostics.SymbolStore.SymbolToken/*02000014*/ parent,
                                     string name,
                                     valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*0100001C*/ attributes,
                                     uint8[] signature,
                                     valuetype System.Diagnostics.SymbolStore.SymAddressKind/*02000013*/ addrKind,
                                     int32 addr1,
                                     int32 addr2,
                                     int32 addr3) cil managed
  // SIG: 20 08 01 11 50 0E 11 71 1D 05 11 4C 08 08 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolWriter::DefineField

  .method /*06000060*/ public hidebysig newslot abstract virtual 
          instance void  DefineGlobalVariable(string name,
                                              valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*0100001C*/ attributes,
                                              uint8[] signature,
                                              valuetype System.Diagnostics.SymbolStore.SymAddressKind/*02000013*/ addrKind,
                                              int32 addr1,
                                              int32 addr2,
                                              int32 addr3) cil managed
  // SIG: 20 07 01 0E 11 71 1D 05 11 4C 08 08 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolWriter::DefineGlobalVariable

  .method /*06000061*/ public hidebysig newslot abstract virtual 
          instance void  DefineLocalVariable(string name,
                                             valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*0100001C*/ attributes,
                                             uint8[] signature,
                                             valuetype System.Diagnostics.SymbolStore.SymAddressKind/*02000013*/ addrKind,
                                             int32 addr1,
                                             int32 addr2,
                                             int32 addr3,
                                             int32 startOffset,
                                             int32 endOffset) cil managed
  // SIG: 20 09 01 0E 11 71 1D 05 11 4C 08 08 08 08 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolWriter::DefineLocalVariable

  .method /*06000062*/ public hidebysig newslot abstract virtual 
          instance void  DefineParameter(string name,
                                         valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterAttributes/*0100001D*/ attributes,
                                         int32 sequence,
                                         valuetype System.Diagnostics.SymbolStore.SymAddressKind/*02000013*/ addrKind,
                                         int32 addr1,
                                         int32 addr2,
                                         int32 addr3) cil managed
  // SIG: 20 07 01 0E 11 75 08 11 4C 08 08 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolWriter::DefineParameter

  .method /*06000063*/ public hidebysig newslot abstract virtual 
          instance void  DefineSequencePoints(class System.Diagnostics.SymbolStore.ISymbolDocumentWriter/*02000018*/ document,
                                              int32[] offsets,
                                              int32[] lines,
                                              int32[] columns,
                                              int32[] endLines,
                                              int32[] endColumns) cil managed
  // SIG: 20 06 01 12 60 1D 08 1D 08 1D 08 1D 08 1D 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolWriter::DefineSequencePoints

  .method /*06000064*/ public hidebysig newslot abstract virtual 
          instance void  Initialize(native int emitter,
                                    string filename,
                                    bool fFullBuild) cil managed
  // SIG: 20 03 01 18 0E 02
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolWriter::Initialize

  .method /*06000065*/ public hidebysig newslot abstract virtual 
          instance void  OpenMethod(valuetype System.Diagnostics.SymbolStore.SymbolToken/*02000014*/ 'method') cil managed
  // SIG: 20 01 01 11 50
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolWriter::OpenMethod

  .method /*06000066*/ public hidebysig newslot abstract virtual 
          instance void  OpenNamespace(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolWriter::OpenNamespace

  .method /*06000067*/ public hidebysig newslot abstract virtual 
          instance int32  OpenScope(int32 startOffset) cil managed
  // SIG: 20 01 08 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolWriter::OpenScope

  .method /*06000068*/ public hidebysig newslot abstract virtual 
          instance void  SetMethodSourceRange(class System.Diagnostics.SymbolStore.ISymbolDocumentWriter/*02000018*/ startDoc,
                                              int32 startLine,
                                              int32 startColumn,
                                              class System.Diagnostics.SymbolStore.ISymbolDocumentWriter/*02000018*/ endDoc,
                                              int32 endLine,
                                              int32 endColumn) cil managed
  // SIG: 20 06 01 12 60 08 08 12 60 08 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolWriter::SetMethodSourceRange

  .method /*06000069*/ public hidebysig newslot abstract virtual 
          instance void  SetScopeRange(int32 scopeID,
                                       int32 startOffset,
                                       int32 endOffset) cil managed
  // SIG: 20 03 01 08 08 08
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolWriter::SetScopeRange

  .method /*0600006A*/ public hidebysig newslot abstract virtual 
          instance void  SetSymAttribute(valuetype System.Diagnostics.SymbolStore.SymbolToken/*02000014*/ parent,
                                         string name,
                                         uint8[] data) cil managed
  // SIG: 20 03 01 11 50 0E 1D 05
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolWriter::SetSymAttribute

  .method /*0600006B*/ public hidebysig newslot abstract virtual 
          instance void  SetUnderlyingWriter(native int underlyingWriter) cil managed
  // SIG: 20 01 01 18
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolWriter::SetUnderlyingWriter

  .method /*0600006C*/ public hidebysig newslot abstract virtual 
          instance void  SetUserEntryPoint(valuetype System.Diagnostics.SymbolStore.SymbolToken/*02000014*/ entryMethod) cil managed
  // SIG: 20 01 01 11 50
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolWriter::SetUserEntryPoint

  .method /*0600006D*/ public hidebysig newslot abstract virtual 
          instance void  UsingNamespace(string fullName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolWriter::UsingNamespace

} // end of class System.Diagnostics.SymbolStore.ISymbolWriter

.class /*02000013*/ public auto ansi sealed System.Diagnostics.SymbolStore.SymAddressKind
       extends [System.Runtime/*23000001*/]System.Enum/*0100001E*/
{
  .field /*04000007*/ public specialname rtspecialname int32 value__
  .field /*04000008*/ public static literal valuetype System.Diagnostics.SymbolStore.SymAddressKind/*02000013*/ ILOffset = int32(0x00000001)
  .field /*04000009*/ public static literal valuetype System.Diagnostics.SymbolStore.SymAddressKind/*02000013*/ NativeRVA = int32(0x00000002)
  .field /*0400000A*/ public static literal valuetype System.Diagnostics.SymbolStore.SymAddressKind/*02000013*/ NativeRegister = int32(0x00000003)
  .field /*0400000B*/ public static literal valuetype System.Diagnostics.SymbolStore.SymAddressKind/*02000013*/ NativeRegisterRelative = int32(0x00000004)
  .field /*0400000C*/ public static literal valuetype System.Diagnostics.SymbolStore.SymAddressKind/*02000013*/ NativeOffset = int32(0x00000005)
  .field /*0400000D*/ public static literal valuetype System.Diagnostics.SymbolStore.SymAddressKind/*02000013*/ NativeRegisterRegister = int32(0x00000006)
  .field /*0400000E*/ public static literal valuetype System.Diagnostics.SymbolStore.SymAddressKind/*02000013*/ NativeRegisterStack = int32(0x00000007)
  .field /*0400000F*/ public static literal valuetype System.Diagnostics.SymbolStore.SymAddressKind/*02000013*/ NativeStackRegister = int32(0x00000008)
  .field /*04000010*/ public static literal valuetype System.Diagnostics.SymbolStore.SymAddressKind/*02000013*/ BitField = int32(0x00000009)
  .field /*04000011*/ public static literal valuetype System.Diagnostics.SymbolStore.SymAddressKind/*02000013*/ NativeSectionOffset = int32(0x0000000A)
} // end of class System.Diagnostics.SymbolStore.SymAddressKind

.class /*02000014*/ public sequential ansi sealed beforefieldinit System.Diagnostics.SymbolStore.SymbolToken
       extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000021*/<valuetype System.Diagnostics.SymbolStore.SymbolToken/*02000014*/>/*1B000001*/
{
  .custom /*0C000037:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100001F*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .field /*04000012*/ private initonly int32 _dummyPrimitive
  .method /*0600006E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 val) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2157
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SymbolToken::.ctor

  .method /*0600006F*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Diagnostics.SymbolStore.SymbolToken/*02000014*/ obj) cil managed
  // SIG: 20 01 02 11 50
  {
    // Method begins at RVA 0x215a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SymbolToken::Equals

  .method /*06000070*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000049:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800008A*/ 
    .custom /*0C00004A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000022*/::.ctor(bool) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x215d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SymbolToken::Equals

  .method /*06000071*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2160
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SymbolToken::GetHashCode

  .method /*06000072*/ public hidebysig instance int32 
          GetToken() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2163
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SymbolToken::GetToken

  .method /*06000073*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Diagnostics.SymbolStore.SymbolToken/*02000014*/ a,
                            valuetype System.Diagnostics.SymbolStore.SymbolToken/*02000014*/ b) cil managed
  // SIG: 00 02 02 11 50 11 50
  {
    // Method begins at RVA 0x2166
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SymbolToken::op_Equality

  .method /*06000074*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Diagnostics.SymbolStore.SymbolToken/*02000014*/ a,
                              valuetype System.Diagnostics.SymbolStore.SymbolToken/*02000014*/ b) cil managed
  // SIG: 00 02 02 11 50 11 50
  {
    // Method begins at RVA 0x2169
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SymbolToken::op_Inequality

} // end of class System.Diagnostics.SymbolStore.SymbolToken

.class /*02000015*/ public auto ansi beforefieldinit System.Diagnostics.SymbolStore.SymDocumentType
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .field /*04000013*/ public static initonly valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ Text
  .method /*06000075*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x216c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SymDocumentType::.ctor

} // end of class System.Diagnostics.SymbolStore.SymDocumentType

.class /*02000016*/ public auto ansi beforefieldinit System.Diagnostics.SymbolStore.SymLanguageType
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .field /*04000014*/ public static initonly valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ Basic
  .field /*04000015*/ public static initonly valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ C
  .field /*04000016*/ public static initonly valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ Cobol
  .field /*04000017*/ public static initonly valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ CPlusPlus
  .field /*04000018*/ public static initonly valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ CSharp
  .field /*04000019*/ public static initonly valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ ILAssembly
  .field /*0400001A*/ public static initonly valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ Java
  .field /*0400001B*/ public static initonly valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ JScript
  .field /*0400001C*/ public static initonly valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ MCPlusPlus
  .field /*0400001D*/ public static initonly valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ Pascal
  .field /*0400001E*/ public static initonly valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ SMC
  .method /*06000076*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2174
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SymLanguageType::.ctor

} // end of class System.Diagnostics.SymbolStore.SymLanguageType

.class /*02000017*/ public auto ansi beforefieldinit System.Diagnostics.SymbolStore.SymLanguageVendor
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .field /*0400001F*/ public static initonly valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ Microsoft
  .method /*06000077*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x217c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SymLanguageVendor::.ctor

} // end of class System.Diagnostics.SymbolStore.SymLanguageVendor

.class /*02000018*/ interface public abstract auto ansi System.Diagnostics.SymbolStore.ISymbolDocumentWriter
{
  .custom /*0C00003C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000078*/ public hidebysig newslot abstract virtual 
          instance void  SetCheckSum(valuetype [System.Runtime/*23000001*/]System.Guid/*0100001B*/ algorithmId,
                                     uint8[] checkSum) cil managed
  // SIG: 20 02 01 11 6D 1D 05
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolDocumentWriter::SetCheckSum

  .method /*06000079*/ public hidebysig newslot abstract virtual 
          instance void  SetSource(uint8[] source) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x0
  } // end of method ISymbolDocumentWriter::SetSource

} // end of class System.Diagnostics.SymbolStore.ISymbolDocumentWriter


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
