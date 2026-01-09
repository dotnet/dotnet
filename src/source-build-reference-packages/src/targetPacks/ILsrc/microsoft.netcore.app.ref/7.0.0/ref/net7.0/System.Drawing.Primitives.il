


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
// Time-date stamp:            0xd702aa6b
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00005c00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00007bd6
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
// Checksum:                       0x000165ec
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00007b83 [0x0000004f] address [size] of Import Directory:          
// 0x00008000 [0x000004a4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00006600 [0x00002880] address [size] of Security Directory:        
// 0x0000a000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00007a94 [0x00000054] address [size] of Debug Directory:           
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
//              0x00005bdc Virtual Size
//              0x00002000 Virtual Address
//              0x00005c00 Size of Raw Data
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
//              0x000004a4 Virtual Size
//              0x00008000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00005e00 Pointer to Raw Data
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
//              0x00006400 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000bd8
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00007bc5 Import Name Table
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
// 0x00002530 [0x000054e4] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00007a14 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x000035b0 Size
//              '#~' Name
//    Stream 2:
//              0x0000361c Offset
//              0x0000120c Size
//              '#Strings' Name
//    Stream 3:
//              0x00004828 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x0000482c Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x0000483c Offset
//              0x00000ca8 Size
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

// File size            : 36480
// PE header size       : 512 (496 used)    ( 1.40%)
// PE additional info   : 11811             (32.38%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.20%)
// CLR meta-data size  : 21732              (59.57%)
// CLR additional info : 128                ( 0.35%)
// CLR method headers  : 402                ( 1.10%)
// Managed code         : 846               ( 2.32%)
// Data                 : 2048              ( 5.61%)
// Unaccounted          : -1071             (-2.94%)

// Num.of PE sections   : 3
//   .text    - 23552
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 21732
//   Module        -    1 (10 bytes)
//   TypeDef       -   16 (224 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   32 (192 bytes)
//   MethodDef     -  402 (5628 bytes)    0 abstract, 0 native, 402 bodies
//   FieldDef      -  195 (1170 bytes)    7 constant
//   MemberRef     -   23 (138 bytes)
//   ParamDef      -  236 (1416 bytes)
//   Constant      -  175 (1050 bytes)
//   CustomAttribute-  152 (912 bytes)
//   InterfaceImpl -    7 (28 bytes)
//   PropertyMap   -    8 (32 bytes)
//   Property      -  218 (1308 bytes)
//   MethodSemantic-  238 (1428 bytes)
//   TypeSpec      -    7 (14 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    4 (80 bytes)
//   Strings       -  4620 bytes
//   Blobs         -  3240 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   200 bytes

// CLR additional info : 128

// CLR method headers : 402
//   Num.of method bodies  - 402
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 402

// Managed code : 846
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class RefSafetyRulesAttribute        (auto) (ansi) (sealed) 
// Class Color                          (public) (sequential) (ansi) (sealed) 
// Class ColorTranslator                (public) (abstract) (auto) (ansi) (sealed) 
// Class KnownColor                     (public) (auto) (ansi) (sealed) 
// Class Point                          (public) (sequential) (ansi) (sealed) 
// Class PointF                         (public) (sequential) (ansi) (sealed) 
// Class Rectangle                      (public) (sequential) (ansi) (sealed) 
// Class RectangleF                     (public) (sequential) (ansi) (sealed) 
// Class Size                           (public) (sequential) (ansi) (sealed) 
// Class SizeF                          (public) (sequential) (ansi) (sealed) 
// Class SystemColors                   (public) (abstract) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000002*/ System.ComponentModel.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000003*/ System.ObjectModel
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000004*/ System.Numerics.Vectors
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly /*20000001*/ noplatform System.Drawing.Primitives
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
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 19 53 79 73 74 65 6D 2E 44 72 61 77 69 6E   // ...System.Drawin
                                                                                                                                                                        67 2E 50 72 69 6D 69 74 69 76 65 73 00 00 )       // g.Primitives..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000F:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000010:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 19 53 79 73 74 65 6D 2E 44 72 61 77 69 6E   // ...System.Drawin
                                                                                                                                                                       67 2E 50 72 69 6D 69 74 69 76 65 73 00 00 )       // g.Primitives..
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 37 2E 30 2E 32 32 2E 35 31 38 30 35 00   // ...7.0.22.51805.
                                                                                                                                                                       00 ) 
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 37 2E 30 2E 30 2B 64 30 39 39 66 30 37   // ...7.0.0+d099f07
                                                                                                                                                                                35 65 34 35 64 32 61 61 36 30 30 37 61 32 32 62   // 5e45d2aa6007a22b
                                                                                                                                                                                37 31 62 34 35 61 30 38 37 35 38 35 35 39 66 38   // 71b45a08758559f8
                                                                                                                                                                                30 00 00 )                                        // 0..
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000014:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 19 53 79 73 74 65 6D 2E 44 72 61 77 69 6E   // ...System.Drawin
                                                                                                                                                                 67 2E 50 72 69 6D 69 74 69 76 65 73 00 00 )       // g.Primitives..
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
.module System.Drawing.Primitives.dll
// MVID: {10f6c986-bb81-47fc-9efa-52bd425d1268}
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
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000017 */
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
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000017 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)000020       */ newarr     [System.Runtime/*23000001*/]System.Byte/*01000020*/
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
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000017 */
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
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000017 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00001F:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000020:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000021:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000017 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.RefSafetyRulesAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000022:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000023:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000024:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000017 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000004       */ stfld      int32 System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::Version /* 04000004 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method RefSafetyRulesAttribute::.ctor

} // end of class System.Runtime.CompilerServices.RefSafetyRulesAttribute

.class /*02000007*/ public sequential ansi sealed beforefieldinit System.Drawing.Color
       extends [System.Runtime/*23000001*/]System.ValueType/*01000018*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000019*/<valuetype System.Drawing.Color/*02000007*/>/*1B000001*/
{
  .custom /*0C000025:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000026:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000027:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000028:0A000013*/ instance void [System.ComponentModel.Primitives/*23000002*/]System.ComponentModel.EditorAttribute/*01000016*/::.ctor(string,
                                                                                                                                                     string) /* 0A000013 */ = ( 01 00 7B 53 79 73 74 65 6D 2E 44 72 61 77 69 6E   // ..{System.Drawin
                                                                                                                                                                                67 2E 44 65 73 69 67 6E 2E 43 6F 6C 6F 72 45 64   // g.Design.ColorEd
                                                                                                                                                                                69 74 6F 72 2C 20 53 79 73 74 65 6D 2E 44 72 61   // itor, System.Dra
                                                                                                                                                                                77 69 6E 67 2E 44 65 73 69 67 6E 2C 20 56 65 72   // wing.Design, Ver
                                                                                                                                                                                73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C 20 43 75   // sion=4.0.0.0, Cu
                                                                                                                                                                                6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C 20 50   // lture=neutral, P
                                                                                                                                                                                75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D 62 30   // ublicKeyToken=b0
                                                                                                                                                                                33 66 35 66 37 66 31 31 64 35 30 61 33 61 75 53   // 3f5f7f11d50a3auS
                                                                                                                                                                                79 73 74 65 6D 2E 44 72 61 77 69 6E 67 2E 44 65   // ystem.Drawing.De
                                                                                                                                                                                73 69 67 6E 2E 55 49 54 79 70 65 45 64 69 74 6F   // sign.UITypeEdito
                                                                                                                                                                                72 2C 20 53 79 73 74 65 6D 2E 44 72 61 77 69 6E   // r, System.Drawin
                                                                                                                                                                                67 2C 20 56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30   // g, Version=4.0.0
                                                                                                                                                                                2E 30 2C 20 43 75 6C 74 75 72 65 3D 6E 65 75 74   // .0, Culture=neut
                                                                                                                                                                                72 61 6C 2C 20 50 75 62 6C 69 63 4B 65 79 54 6F   // ral, PublicKeyTo
                                                                                                                                                                                6B 65 6E 3D 62 30 33 66 35 66 37 66 31 31 64 35   // ken=b03f5f7f11d5
                                                                                                                                                                                30 61 33 61 00 00 )                               // 0a3a..
  .custom /*0C000029:0A000014*/ instance void [System.ObjectModel/*23000003*/]System.ComponentModel.TypeConverterAttribute/*01000017*/::.ctor(string) /* 0A000014 */ = ( 01 00 70 53 79 73 74 65 6D 2E 44 72 61 77 69 6E   // ..pSystem.Drawin
                                                                                                                                                                         67 2E 43 6F 6C 6F 72 43 6F 6E 76 65 72 74 65 72   // g.ColorConverter
                                                                                                                                                                         2C 20 53 79 73 74 65 6D 2E 44 72 61 77 69 6E 67   // , System.Drawing
                                                                                                                                                                         2C 20 56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E   // , Version=4.0.0.
                                                                                                                                                                         30 2C 20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72   // 0, Culture=neutr
                                                                                                                                                                         61 6C 2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B   // al, PublicKeyTok
                                                                                                                                                                         65 6E 3D 62 30 33 66 35 66 37 66 31 31 64 35 30   // en=b03f5f7f11d50
                                                                                                                                                                         61 33 61 00 00 )                                  // a3a..
  .interfaceimpl type class [System.Runtime/*23000001*/]System.IEquatable`1/*01000019*/<valuetype System.Drawing.Color/*02000007*/>/*1B000001*/
  .custom /*0C000001:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000005*/ private initonly object _dummy
  .field /*04000006*/ private initonly int32 _dummyPrimitive
  .field /*04000007*/ public static initonly valuetype System.Drawing.Color/*02000007*/ Empty
  .method /*06000007*/ public hidebysig specialname 
          instance uint8  get_A() cil managed
  // SIG: 20 00 05
  {
    // Method begins at RVA 0x20ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_A

  .method /*06000008*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_AliceBlue() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_AliceBlue

  .method /*06000009*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_AntiqueWhite() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_AntiqueWhite

  .method /*0600000A*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Aqua() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Aqua

  .method /*0600000B*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Aquamarine() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Aquamarine

  .method /*0600000C*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Azure() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Azure

  .method /*0600000D*/ public hidebysig specialname 
          instance uint8  get_B() cil managed
  // SIG: 20 00 05
  {
    // Method begins at RVA 0x20be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_B

  .method /*0600000E*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Beige() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Beige

  .method /*0600000F*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Bisque() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Bisque

  .method /*06000010*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Black() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Black

  .method /*06000011*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_BlanchedAlmond() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_BlanchedAlmond

  .method /*06000012*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Blue() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Blue

  .method /*06000013*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_BlueViolet() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_BlueViolet

  .method /*06000014*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Brown() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Brown

  .method /*06000015*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_BurlyWood() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_BurlyWood

  .method /*06000016*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_CadetBlue() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_CadetBlue

  .method /*06000017*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Chartreuse() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Chartreuse

  .method /*06000018*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Chocolate() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Chocolate

  .method /*06000019*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Coral() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Coral

  .method /*0600001A*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_CornflowerBlue() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_CornflowerBlue

  .method /*0600001B*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Cornsilk() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Cornsilk

  .method /*0600001C*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Crimson() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Crimson

  .method /*0600001D*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Cyan() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Cyan

  .method /*0600001E*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DarkBlue() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DarkBlue

  .method /*0600001F*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DarkCyan() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DarkCyan

  .method /*06000020*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DarkGoldenrod() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DarkGoldenrod

  .method /*06000021*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DarkGray() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DarkGray

  .method /*06000022*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DarkGreen() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x20fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DarkGreen

  .method /*06000023*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DarkKhaki() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2100
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DarkKhaki

  .method /*06000024*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DarkMagenta() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2103
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DarkMagenta

  .method /*06000025*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DarkOliveGreen() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2106
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DarkOliveGreen

  .method /*06000026*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DarkOrange() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2109
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DarkOrange

  .method /*06000027*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DarkOrchid() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x210c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DarkOrchid

  .method /*06000028*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DarkRed() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x210f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DarkRed

  .method /*06000029*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DarkSalmon() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2112
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DarkSalmon

  .method /*0600002A*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DarkSeaGreen() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2115
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DarkSeaGreen

  .method /*0600002B*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DarkSlateBlue() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2118
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DarkSlateBlue

  .method /*0600002C*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DarkSlateGray() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x211b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DarkSlateGray

  .method /*0600002D*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DarkTurquoise() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x211e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DarkTurquoise

  .method /*0600002E*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DarkViolet() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2121
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DarkViolet

  .method /*0600002F*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DeepPink() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2124
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DeepPink

  .method /*06000030*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DeepSkyBlue() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2127
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DeepSkyBlue

  .method /*06000031*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DimGray() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x212a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DimGray

  .method /*06000032*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_DodgerBlue() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x212d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_DodgerBlue

  .method /*06000033*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Firebrick() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2130
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Firebrick

  .method /*06000034*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_FloralWhite() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2133
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_FloralWhite

  .method /*06000035*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_ForestGreen() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2136
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_ForestGreen

  .method /*06000036*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Fuchsia() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2139
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Fuchsia

  .method /*06000037*/ public hidebysig specialname 
          instance uint8  get_G() cil managed
  // SIG: 20 00 05
  {
    // Method begins at RVA 0x213c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_G

  .method /*06000038*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Gainsboro() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x213f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Gainsboro

  .method /*06000039*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_GhostWhite() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2142
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_GhostWhite

  .method /*0600003A*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Gold() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2145
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Gold

  .method /*0600003B*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Goldenrod() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2148
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Goldenrod

  .method /*0600003C*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Gray() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x214b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Gray

  .method /*0600003D*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Green() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x214e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Green

  .method /*0600003E*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_GreenYellow() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2151
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_GreenYellow

  .method /*0600003F*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Honeydew() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2154
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Honeydew

  .method /*06000040*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_HotPink() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2157
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_HotPink

  .method /*06000041*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_IndianRed() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x215a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_IndianRed

  .method /*06000042*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Indigo() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x215d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Indigo

  .method /*06000043*/ public hidebysig specialname 
          instance bool  get_IsEmpty() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2160
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_IsEmpty

  .method /*06000044*/ public hidebysig specialname 
          instance bool  get_IsKnownColor() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2163
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_IsKnownColor

  .method /*06000045*/ public hidebysig specialname 
          instance bool  get_IsNamedColor() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2166
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_IsNamedColor

  .method /*06000046*/ public hidebysig specialname 
          instance bool  get_IsSystemColor() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2169
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_IsSystemColor

  .method /*06000047*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Ivory() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x216c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Ivory

  .method /*06000048*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Khaki() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x216f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Khaki

  .method /*06000049*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Lavender() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2172
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Lavender

  .method /*0600004A*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_LavenderBlush() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2175
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_LavenderBlush

  .method /*0600004B*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_LawnGreen() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2178
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_LawnGreen

  .method /*0600004C*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_LemonChiffon() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x217b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_LemonChiffon

  .method /*0600004D*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_LightBlue() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x217e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_LightBlue

  .method /*0600004E*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_LightCoral() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2181
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_LightCoral

  .method /*0600004F*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_LightCyan() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2184
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_LightCyan

  .method /*06000050*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_LightGoldenrodYellow() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2187
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_LightGoldenrodYellow

  .method /*06000051*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_LightGray() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x218a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_LightGray

  .method /*06000052*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_LightGreen() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x218d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_LightGreen

  .method /*06000053*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_LightPink() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2190
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_LightPink

  .method /*06000054*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_LightSalmon() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2193
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_LightSalmon

  .method /*06000055*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_LightSeaGreen() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2196
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_LightSeaGreen

  .method /*06000056*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_LightSkyBlue() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2199
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_LightSkyBlue

  .method /*06000057*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_LightSlateGray() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x219c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_LightSlateGray

  .method /*06000058*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_LightSteelBlue() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x219f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_LightSteelBlue

  .method /*06000059*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_LightYellow() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_LightYellow

  .method /*0600005A*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Lime() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Lime

  .method /*0600005B*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_LimeGreen() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_LimeGreen

  .method /*0600005C*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Linen() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Linen

  .method /*0600005D*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Magenta() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Magenta

  .method /*0600005E*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Maroon() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Maroon

  .method /*0600005F*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_MediumAquamarine() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_MediumAquamarine

  .method /*06000060*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_MediumBlue() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_MediumBlue

  .method /*06000061*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_MediumOrchid() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_MediumOrchid

  .method /*06000062*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_MediumPurple() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_MediumPurple

  .method /*06000063*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_MediumSeaGreen() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_MediumSeaGreen

  .method /*06000064*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_MediumSlateBlue() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_MediumSlateBlue

  .method /*06000065*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_MediumSpringGreen() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_MediumSpringGreen

  .method /*06000066*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_MediumTurquoise() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_MediumTurquoise

  .method /*06000067*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_MediumVioletRed() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_MediumVioletRed

  .method /*06000068*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_MidnightBlue() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_MidnightBlue

  .method /*06000069*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_MintCream() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_MintCream

  .method /*0600006A*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_MistyRose() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_MistyRose

  .method /*0600006B*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Moccasin() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Moccasin

  .method /*0600006C*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Name

  .method /*0600006D*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_NavajoWhite() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_NavajoWhite

  .method /*0600006E*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Navy() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Navy

  .method /*0600006F*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_OldLace() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_OldLace

  .method /*06000070*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Olive() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Olive

  .method /*06000071*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_OliveDrab() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_OliveDrab

  .method /*06000072*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Orange() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Orange

  .method /*06000073*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_OrangeRed() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_OrangeRed

  .method /*06000074*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Orchid() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Orchid

  .method /*06000075*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_PaleGoldenrod() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_PaleGoldenrod

  .method /*06000076*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_PaleGreen() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_PaleGreen

  .method /*06000077*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_PaleTurquoise() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_PaleTurquoise

  .method /*06000078*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_PaleVioletRed() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x21ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_PaleVioletRed

  .method /*06000079*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_PapayaWhip() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2202
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_PapayaWhip

  .method /*0600007A*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_PeachPuff() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2205
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_PeachPuff

  .method /*0600007B*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Peru() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2208
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Peru

  .method /*0600007C*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Pink() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x220b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Pink

  .method /*0600007D*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Plum() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x220e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Plum

  .method /*0600007E*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_PowderBlue() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2211
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_PowderBlue

  .method /*0600007F*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Purple() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2214
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Purple

  .method /*06000080*/ public hidebysig specialname 
          instance uint8  get_R() cil managed
  // SIG: 20 00 05
  {
    // Method begins at RVA 0x2217
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_R

  .method /*06000081*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_RebeccaPurple() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x221a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_RebeccaPurple

  .method /*06000082*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Red() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x221d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Red

  .method /*06000083*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_RosyBrown() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2220
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_RosyBrown

  .method /*06000084*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_RoyalBlue() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2223
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_RoyalBlue

  .method /*06000085*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_SaddleBrown() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2226
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_SaddleBrown

  .method /*06000086*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Salmon() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2229
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Salmon

  .method /*06000087*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_SandyBrown() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x222c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_SandyBrown

  .method /*06000088*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_SeaGreen() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x222f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_SeaGreen

  .method /*06000089*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_SeaShell() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2232
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_SeaShell

  .method /*0600008A*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Sienna() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2235
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Sienna

  .method /*0600008B*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Silver() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2238
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Silver

  .method /*0600008C*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_SkyBlue() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x223b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_SkyBlue

  .method /*0600008D*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_SlateBlue() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x223e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_SlateBlue

  .method /*0600008E*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_SlateGray() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2241
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_SlateGray

  .method /*0600008F*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Snow() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2244
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Snow

  .method /*06000090*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_SpringGreen() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2247
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_SpringGreen

  .method /*06000091*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_SteelBlue() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x224a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_SteelBlue

  .method /*06000092*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Tan() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x224d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Tan

  .method /*06000093*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Teal() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2250
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Teal

  .method /*06000094*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Thistle() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2253
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Thistle

  .method /*06000095*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Tomato() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2256
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Tomato

  .method /*06000096*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Transparent() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2259
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Transparent

  .method /*06000097*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Turquoise() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x225c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Turquoise

  .method /*06000098*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Violet() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x225f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Violet

  .method /*06000099*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Wheat() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2262
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Wheat

  .method /*0600009A*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_White() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2265
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_White

  .method /*0600009B*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_WhiteSmoke() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2268
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_WhiteSmoke

  .method /*0600009C*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Yellow() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x226b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_Yellow

  .method /*0600009D*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_YellowGreen() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x226e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::get_YellowGreen

  .method /*0600009E*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Drawing.Color/*02000007*/ other) cil managed
  // SIG: 20 01 02 11 1C
  {
    // Method begins at RVA 0x2271
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::Equals

  .method /*0600009F*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000037:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000002*/ 
    .custom /*0C000018:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001B*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2274
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::Equals

  .method /*060000A0*/ public hidebysig static 
          valuetype System.Drawing.Color/*02000007*/ 
          FromArgb(int32 argb) cil managed
  // SIG: 00 01 11 1C 08
  {
    // Method begins at RVA 0x2277
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::FromArgb

  .method /*060000A1*/ public hidebysig static 
          valuetype System.Drawing.Color/*02000007*/ 
          FromArgb(int32 alpha,
                   valuetype System.Drawing.Color/*02000007*/ baseColor) cil managed
  // SIG: 00 02 11 1C 08 11 1C
  {
    // Method begins at RVA 0x227a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::FromArgb

  .method /*060000A2*/ public hidebysig static 
          valuetype System.Drawing.Color/*02000007*/ 
          FromArgb(int32 red,
                   int32 green,
                   int32 blue) cil managed
  // SIG: 00 03 11 1C 08 08 08
  {
    // Method begins at RVA 0x227d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::FromArgb

  .method /*060000A3*/ public hidebysig static 
          valuetype System.Drawing.Color/*02000007*/ 
          FromArgb(int32 alpha,
                   int32 red,
                   int32 green,
                   int32 blue) cil managed
  // SIG: 00 04 11 1C 08 08 08 08
  {
    // Method begins at RVA 0x2280
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::FromArgb

  .method /*060000A4*/ public hidebysig static 
          valuetype System.Drawing.Color/*02000007*/ 
          FromKnownColor(valuetype System.Drawing.KnownColor/*02000009*/ color) cil managed
  // SIG: 00 01 11 1C 11 24
  {
    // Method begins at RVA 0x2283
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::FromKnownColor

  .method /*060000A5*/ public hidebysig static 
          valuetype System.Drawing.Color/*02000007*/ 
          FromName(string name) cil managed
  // SIG: 00 01 11 1C 0E
  {
    // Method begins at RVA 0x2286
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::FromName

  .method /*060000A6*/ public hidebysig instance float32 
          GetBrightness() cil managed
  // SIG: 20 00 0C
  {
    // Method begins at RVA 0x2289
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::GetBrightness

  .method /*060000A7*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x228c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::GetHashCode

  .method /*060000A8*/ public hidebysig instance float32 
          GetHue() cil managed
  // SIG: 20 00 0C
  {
    // Method begins at RVA 0x228f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::GetHue

  .method /*060000A9*/ public hidebysig instance float32 
          GetSaturation() cil managed
  // SIG: 20 00 0C
  {
    // Method begins at RVA 0x2292
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::GetSaturation

  .method /*060000AA*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Drawing.Color/*02000007*/ left,
                            valuetype System.Drawing.Color/*02000007*/ right) cil managed
  // SIG: 00 02 02 11 1C 11 1C
  {
    // Method begins at RVA 0x2295
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::op_Equality

  .method /*060000AB*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Drawing.Color/*02000007*/ left,
                              valuetype System.Drawing.Color/*02000007*/ right) cil managed
  // SIG: 00 02 02 11 1C 11 1C
  {
    // Method begins at RVA 0x2298
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::op_Inequality

  .method /*060000AC*/ public hidebysig instance int32 
          ToArgb() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x229b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::ToArgb

  .method /*060000AD*/ public hidebysig instance valuetype System.Drawing.KnownColor/*02000009*/ 
          ToKnownColor() cil managed
  // SIG: 20 00 11 24
  {
    // Method begins at RVA 0x229e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::ToKnownColor

  .method /*060000AE*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Color::ToString

  .property /*17000001*/ instance callconv(8) uint8
          A()
  {
    .get instance uint8 System.Drawing.Color/*02000007*/::get_A() /* 06000007 */
  } // end of property Color::A
  .property /*17000002*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          AliceBlue()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_AliceBlue() /* 06000008 */
  } // end of property Color::AliceBlue
  .property /*17000003*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          AntiqueWhite()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_AntiqueWhite() /* 06000009 */
  } // end of property Color::AntiqueWhite
  .property /*17000004*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Aqua()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Aqua() /* 0600000A */
  } // end of property Color::Aqua
  .property /*17000005*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Aquamarine()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Aquamarine() /* 0600000B */
  } // end of property Color::Aquamarine
  .property /*17000006*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Azure()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Azure() /* 0600000C */
  } // end of property Color::Azure
  .property /*17000007*/ instance callconv(8) uint8
          B()
  {
    .get instance uint8 System.Drawing.Color/*02000007*/::get_B() /* 0600000D */
  } // end of property Color::B
  .property /*17000008*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Beige()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Beige() /* 0600000E */
  } // end of property Color::Beige
  .property /*17000009*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Bisque()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Bisque() /* 0600000F */
  } // end of property Color::Bisque
  .property /*1700000A*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Black()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Black() /* 06000010 */
  } // end of property Color::Black
  .property /*1700000B*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          BlanchedAlmond()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_BlanchedAlmond() /* 06000011 */
  } // end of property Color::BlanchedAlmond
  .property /*1700000C*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Blue()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Blue() /* 06000012 */
  } // end of property Color::Blue
  .property /*1700000D*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          BlueViolet()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_BlueViolet() /* 06000013 */
  } // end of property Color::BlueViolet
  .property /*1700000E*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Brown()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Brown() /* 06000014 */
  } // end of property Color::Brown
  .property /*1700000F*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          BurlyWood()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_BurlyWood() /* 06000015 */
  } // end of property Color::BurlyWood
  .property /*17000010*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          CadetBlue()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_CadetBlue() /* 06000016 */
  } // end of property Color::CadetBlue
  .property /*17000011*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Chartreuse()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Chartreuse() /* 06000017 */
  } // end of property Color::Chartreuse
  .property /*17000012*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Chocolate()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Chocolate() /* 06000018 */
  } // end of property Color::Chocolate
  .property /*17000013*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Coral()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Coral() /* 06000019 */
  } // end of property Color::Coral
  .property /*17000014*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          CornflowerBlue()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_CornflowerBlue() /* 0600001A */
  } // end of property Color::CornflowerBlue
  .property /*17000015*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Cornsilk()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Cornsilk() /* 0600001B */
  } // end of property Color::Cornsilk
  .property /*17000016*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Crimson()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Crimson() /* 0600001C */
  } // end of property Color::Crimson
  .property /*17000017*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Cyan()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Cyan() /* 0600001D */
  } // end of property Color::Cyan
  .property /*17000018*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DarkBlue()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DarkBlue() /* 0600001E */
  } // end of property Color::DarkBlue
  .property /*17000019*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DarkCyan()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DarkCyan() /* 0600001F */
  } // end of property Color::DarkCyan
  .property /*1700001A*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DarkGoldenrod()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DarkGoldenrod() /* 06000020 */
  } // end of property Color::DarkGoldenrod
  .property /*1700001B*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DarkGray()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DarkGray() /* 06000021 */
  } // end of property Color::DarkGray
  .property /*1700001C*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DarkGreen()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DarkGreen() /* 06000022 */
  } // end of property Color::DarkGreen
  .property /*1700001D*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DarkKhaki()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DarkKhaki() /* 06000023 */
  } // end of property Color::DarkKhaki
  .property /*1700001E*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DarkMagenta()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DarkMagenta() /* 06000024 */
  } // end of property Color::DarkMagenta
  .property /*1700001F*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DarkOliveGreen()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DarkOliveGreen() /* 06000025 */
  } // end of property Color::DarkOliveGreen
  .property /*17000020*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DarkOrange()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DarkOrange() /* 06000026 */
  } // end of property Color::DarkOrange
  .property /*17000021*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DarkOrchid()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DarkOrchid() /* 06000027 */
  } // end of property Color::DarkOrchid
  .property /*17000022*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DarkRed()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DarkRed() /* 06000028 */
  } // end of property Color::DarkRed
  .property /*17000023*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DarkSalmon()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DarkSalmon() /* 06000029 */
  } // end of property Color::DarkSalmon
  .property /*17000024*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DarkSeaGreen()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DarkSeaGreen() /* 0600002A */
  } // end of property Color::DarkSeaGreen
  .property /*17000025*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DarkSlateBlue()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DarkSlateBlue() /* 0600002B */
  } // end of property Color::DarkSlateBlue
  .property /*17000026*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DarkSlateGray()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DarkSlateGray() /* 0600002C */
  } // end of property Color::DarkSlateGray
  .property /*17000027*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DarkTurquoise()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DarkTurquoise() /* 0600002D */
  } // end of property Color::DarkTurquoise
  .property /*17000028*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DarkViolet()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DarkViolet() /* 0600002E */
  } // end of property Color::DarkViolet
  .property /*17000029*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DeepPink()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DeepPink() /* 0600002F */
  } // end of property Color::DeepPink
  .property /*1700002A*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DeepSkyBlue()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DeepSkyBlue() /* 06000030 */
  } // end of property Color::DeepSkyBlue
  .property /*1700002B*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DimGray()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DimGray() /* 06000031 */
  } // end of property Color::DimGray
  .property /*1700002C*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          DodgerBlue()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_DodgerBlue() /* 06000032 */
  } // end of property Color::DodgerBlue
  .property /*1700002D*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Firebrick()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Firebrick() /* 06000033 */
  } // end of property Color::Firebrick
  .property /*1700002E*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          FloralWhite()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_FloralWhite() /* 06000034 */
  } // end of property Color::FloralWhite
  .property /*1700002F*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          ForestGreen()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_ForestGreen() /* 06000035 */
  } // end of property Color::ForestGreen
  .property /*17000030*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Fuchsia()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Fuchsia() /* 06000036 */
  } // end of property Color::Fuchsia
  .property /*17000031*/ instance callconv(8) uint8
          G()
  {
    .get instance uint8 System.Drawing.Color/*02000007*/::get_G() /* 06000037 */
  } // end of property Color::G
  .property /*17000032*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Gainsboro()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Gainsboro() /* 06000038 */
  } // end of property Color::Gainsboro
  .property /*17000033*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          GhostWhite()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_GhostWhite() /* 06000039 */
  } // end of property Color::GhostWhite
  .property /*17000034*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Gold()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Gold() /* 0600003A */
  } // end of property Color::Gold
  .property /*17000035*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Goldenrod()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Goldenrod() /* 0600003B */
  } // end of property Color::Goldenrod
  .property /*17000036*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Gray()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Gray() /* 0600003C */
  } // end of property Color::Gray
  .property /*17000037*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Green()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Green() /* 0600003D */
  } // end of property Color::Green
  .property /*17000038*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          GreenYellow()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_GreenYellow() /* 0600003E */
  } // end of property Color::GreenYellow
  .property /*17000039*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Honeydew()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Honeydew() /* 0600003F */
  } // end of property Color::Honeydew
  .property /*1700003A*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          HotPink()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_HotPink() /* 06000040 */
  } // end of property Color::HotPink
  .property /*1700003B*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          IndianRed()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_IndianRed() /* 06000041 */
  } // end of property Color::IndianRed
  .property /*1700003C*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Indigo()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Indigo() /* 06000042 */
  } // end of property Color::Indigo
  .property /*1700003D*/ instance callconv(8) bool
          IsEmpty()
  {
    .get instance bool System.Drawing.Color/*02000007*/::get_IsEmpty() /* 06000043 */
  } // end of property Color::IsEmpty
  .property /*1700003E*/ instance callconv(8) bool
          IsKnownColor()
  {
    .get instance bool System.Drawing.Color/*02000007*/::get_IsKnownColor() /* 06000044 */
  } // end of property Color::IsKnownColor
  .property /*1700003F*/ instance callconv(8) bool
          IsNamedColor()
  {
    .get instance bool System.Drawing.Color/*02000007*/::get_IsNamedColor() /* 06000045 */
  } // end of property Color::IsNamedColor
  .property /*17000040*/ instance callconv(8) bool
          IsSystemColor()
  {
    .get instance bool System.Drawing.Color/*02000007*/::get_IsSystemColor() /* 06000046 */
  } // end of property Color::IsSystemColor
  .property /*17000041*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Ivory()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Ivory() /* 06000047 */
  } // end of property Color::Ivory
  .property /*17000042*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Khaki()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Khaki() /* 06000048 */
  } // end of property Color::Khaki
  .property /*17000043*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Lavender()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Lavender() /* 06000049 */
  } // end of property Color::Lavender
  .property /*17000044*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          LavenderBlush()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_LavenderBlush() /* 0600004A */
  } // end of property Color::LavenderBlush
  .property /*17000045*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          LawnGreen()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_LawnGreen() /* 0600004B */
  } // end of property Color::LawnGreen
  .property /*17000046*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          LemonChiffon()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_LemonChiffon() /* 0600004C */
  } // end of property Color::LemonChiffon
  .property /*17000047*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          LightBlue()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_LightBlue() /* 0600004D */
  } // end of property Color::LightBlue
  .property /*17000048*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          LightCoral()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_LightCoral() /* 0600004E */
  } // end of property Color::LightCoral
  .property /*17000049*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          LightCyan()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_LightCyan() /* 0600004F */
  } // end of property Color::LightCyan
  .property /*1700004A*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          LightGoldenrodYellow()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_LightGoldenrodYellow() /* 06000050 */
  } // end of property Color::LightGoldenrodYellow
  .property /*1700004B*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          LightGray()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_LightGray() /* 06000051 */
  } // end of property Color::LightGray
  .property /*1700004C*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          LightGreen()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_LightGreen() /* 06000052 */
  } // end of property Color::LightGreen
  .property /*1700004D*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          LightPink()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_LightPink() /* 06000053 */
  } // end of property Color::LightPink
  .property /*1700004E*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          LightSalmon()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_LightSalmon() /* 06000054 */
  } // end of property Color::LightSalmon
  .property /*1700004F*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          LightSeaGreen()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_LightSeaGreen() /* 06000055 */
  } // end of property Color::LightSeaGreen
  .property /*17000050*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          LightSkyBlue()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_LightSkyBlue() /* 06000056 */
  } // end of property Color::LightSkyBlue
  .property /*17000051*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          LightSlateGray()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_LightSlateGray() /* 06000057 */
  } // end of property Color::LightSlateGray
  .property /*17000052*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          LightSteelBlue()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_LightSteelBlue() /* 06000058 */
  } // end of property Color::LightSteelBlue
  .property /*17000053*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          LightYellow()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_LightYellow() /* 06000059 */
  } // end of property Color::LightYellow
  .property /*17000054*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Lime()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Lime() /* 0600005A */
  } // end of property Color::Lime
  .property /*17000055*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          LimeGreen()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_LimeGreen() /* 0600005B */
  } // end of property Color::LimeGreen
  .property /*17000056*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Linen()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Linen() /* 0600005C */
  } // end of property Color::Linen
  .property /*17000057*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Magenta()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Magenta() /* 0600005D */
  } // end of property Color::Magenta
  .property /*17000058*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Maroon()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Maroon() /* 0600005E */
  } // end of property Color::Maroon
  .property /*17000059*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          MediumAquamarine()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_MediumAquamarine() /* 0600005F */
  } // end of property Color::MediumAquamarine
  .property /*1700005A*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          MediumBlue()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_MediumBlue() /* 06000060 */
  } // end of property Color::MediumBlue
  .property /*1700005B*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          MediumOrchid()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_MediumOrchid() /* 06000061 */
  } // end of property Color::MediumOrchid
  .property /*1700005C*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          MediumPurple()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_MediumPurple() /* 06000062 */
  } // end of property Color::MediumPurple
  .property /*1700005D*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          MediumSeaGreen()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_MediumSeaGreen() /* 06000063 */
  } // end of property Color::MediumSeaGreen
  .property /*1700005E*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          MediumSlateBlue()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_MediumSlateBlue() /* 06000064 */
  } // end of property Color::MediumSlateBlue
  .property /*1700005F*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          MediumSpringGreen()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_MediumSpringGreen() /* 06000065 */
  } // end of property Color::MediumSpringGreen
  .property /*17000060*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          MediumTurquoise()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_MediumTurquoise() /* 06000066 */
  } // end of property Color::MediumTurquoise
  .property /*17000061*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          MediumVioletRed()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_MediumVioletRed() /* 06000067 */
  } // end of property Color::MediumVioletRed
  .property /*17000062*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          MidnightBlue()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_MidnightBlue() /* 06000068 */
  } // end of property Color::MidnightBlue
  .property /*17000063*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          MintCream()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_MintCream() /* 06000069 */
  } // end of property Color::MintCream
  .property /*17000064*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          MistyRose()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_MistyRose() /* 0600006A */
  } // end of property Color::MistyRose
  .property /*17000065*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Moccasin()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Moccasin() /* 0600006B */
  } // end of property Color::Moccasin
  .property /*17000066*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Drawing.Color/*02000007*/::get_Name() /* 0600006C */
  } // end of property Color::Name
  .property /*17000067*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          NavajoWhite()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_NavajoWhite() /* 0600006D */
  } // end of property Color::NavajoWhite
  .property /*17000068*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Navy()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Navy() /* 0600006E */
  } // end of property Color::Navy
  .property /*17000069*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          OldLace()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_OldLace() /* 0600006F */
  } // end of property Color::OldLace
  .property /*1700006A*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Olive()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Olive() /* 06000070 */
  } // end of property Color::Olive
  .property /*1700006B*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          OliveDrab()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_OliveDrab() /* 06000071 */
  } // end of property Color::OliveDrab
  .property /*1700006C*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Orange()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Orange() /* 06000072 */
  } // end of property Color::Orange
  .property /*1700006D*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          OrangeRed()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_OrangeRed() /* 06000073 */
  } // end of property Color::OrangeRed
  .property /*1700006E*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Orchid()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Orchid() /* 06000074 */
  } // end of property Color::Orchid
  .property /*1700006F*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          PaleGoldenrod()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_PaleGoldenrod() /* 06000075 */
  } // end of property Color::PaleGoldenrod
  .property /*17000070*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          PaleGreen()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_PaleGreen() /* 06000076 */
  } // end of property Color::PaleGreen
  .property /*17000071*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          PaleTurquoise()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_PaleTurquoise() /* 06000077 */
  } // end of property Color::PaleTurquoise
  .property /*17000072*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          PaleVioletRed()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_PaleVioletRed() /* 06000078 */
  } // end of property Color::PaleVioletRed
  .property /*17000073*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          PapayaWhip()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_PapayaWhip() /* 06000079 */
  } // end of property Color::PapayaWhip
  .property /*17000074*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          PeachPuff()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_PeachPuff() /* 0600007A */
  } // end of property Color::PeachPuff
  .property /*17000075*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Peru()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Peru() /* 0600007B */
  } // end of property Color::Peru
  .property /*17000076*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Pink()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Pink() /* 0600007C */
  } // end of property Color::Pink
  .property /*17000077*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Plum()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Plum() /* 0600007D */
  } // end of property Color::Plum
  .property /*17000078*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          PowderBlue()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_PowderBlue() /* 0600007E */
  } // end of property Color::PowderBlue
  .property /*17000079*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Purple()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Purple() /* 0600007F */
  } // end of property Color::Purple
  .property /*1700007A*/ instance callconv(8) uint8
          R()
  {
    .get instance uint8 System.Drawing.Color/*02000007*/::get_R() /* 06000080 */
  } // end of property Color::R
  .property /*1700007B*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          RebeccaPurple()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_RebeccaPurple() /* 06000081 */
  } // end of property Color::RebeccaPurple
  .property /*1700007C*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Red()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Red() /* 06000082 */
  } // end of property Color::Red
  .property /*1700007D*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          RosyBrown()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_RosyBrown() /* 06000083 */
  } // end of property Color::RosyBrown
  .property /*1700007E*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          RoyalBlue()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_RoyalBlue() /* 06000084 */
  } // end of property Color::RoyalBlue
  .property /*1700007F*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          SaddleBrown()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_SaddleBrown() /* 06000085 */
  } // end of property Color::SaddleBrown
  .property /*17000080*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Salmon()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Salmon() /* 06000086 */
  } // end of property Color::Salmon
  .property /*17000081*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          SandyBrown()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_SandyBrown() /* 06000087 */
  } // end of property Color::SandyBrown
  .property /*17000082*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          SeaGreen()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_SeaGreen() /* 06000088 */
  } // end of property Color::SeaGreen
  .property /*17000083*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          SeaShell()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_SeaShell() /* 06000089 */
  } // end of property Color::SeaShell
  .property /*17000084*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Sienna()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Sienna() /* 0600008A */
  } // end of property Color::Sienna
  .property /*17000085*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Silver()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Silver() /* 0600008B */
  } // end of property Color::Silver
  .property /*17000086*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          SkyBlue()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_SkyBlue() /* 0600008C */
  } // end of property Color::SkyBlue
  .property /*17000087*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          SlateBlue()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_SlateBlue() /* 0600008D */
  } // end of property Color::SlateBlue
  .property /*17000088*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          SlateGray()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_SlateGray() /* 0600008E */
  } // end of property Color::SlateGray
  .property /*17000089*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Snow()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Snow() /* 0600008F */
  } // end of property Color::Snow
  .property /*1700008A*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          SpringGreen()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_SpringGreen() /* 06000090 */
  } // end of property Color::SpringGreen
  .property /*1700008B*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          SteelBlue()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_SteelBlue() /* 06000091 */
  } // end of property Color::SteelBlue
  .property /*1700008C*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Tan()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Tan() /* 06000092 */
  } // end of property Color::Tan
  .property /*1700008D*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Teal()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Teal() /* 06000093 */
  } // end of property Color::Teal
  .property /*1700008E*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Thistle()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Thistle() /* 06000094 */
  } // end of property Color::Thistle
  .property /*1700008F*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Tomato()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Tomato() /* 06000095 */
  } // end of property Color::Tomato
  .property /*17000090*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Transparent()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Transparent() /* 06000096 */
  } // end of property Color::Transparent
  .property /*17000091*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Turquoise()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Turquoise() /* 06000097 */
  } // end of property Color::Turquoise
  .property /*17000092*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Violet()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Violet() /* 06000098 */
  } // end of property Color::Violet
  .property /*17000093*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Wheat()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Wheat() /* 06000099 */
  } // end of property Color::Wheat
  .property /*17000094*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          White()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_White() /* 0600009A */
  } // end of property Color::White
  .property /*17000095*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          WhiteSmoke()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_WhiteSmoke() /* 0600009B */
  } // end of property Color::WhiteSmoke
  .property /*17000096*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Yellow()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_Yellow() /* 0600009C */
  } // end of property Color::Yellow
  .property /*17000097*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          YellowGreen()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.Color/*02000007*/::get_YellowGreen() /* 0600009D */
  } // end of property Color::YellowGreen
} // end of class System.Drawing.Color

.class /*02000008*/ public abstract auto ansi sealed beforefieldinit System.Drawing.ColorTranslator
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C00002A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000AF*/ public hidebysig static 
          valuetype System.Drawing.Color/*02000007*/ 
          FromHtml(string htmlColor) cil managed
  // SIG: 00 01 11 1C 0E
  {
    // Method begins at RVA 0x22a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ColorTranslator::FromHtml

  .method /*060000B0*/ public hidebysig static 
          valuetype System.Drawing.Color/*02000007*/ 
          FromOle(int32 oleColor) cil managed
  // SIG: 00 01 11 1C 08
  {
    // Method begins at RVA 0x22a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ColorTranslator::FromOle

  .method /*060000B1*/ public hidebysig static 
          valuetype System.Drawing.Color/*02000007*/ 
          FromWin32(int32 win32Color) cil managed
  // SIG: 00 01 11 1C 08
  {
    // Method begins at RVA 0x22aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ColorTranslator::FromWin32

  .method /*060000B2*/ public hidebysig static 
          string  ToHtml(valuetype System.Drawing.Color/*02000007*/ c) cil managed
  // SIG: 00 01 0E 11 1C
  {
    // Method begins at RVA 0x22ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ColorTranslator::ToHtml

  .method /*060000B3*/ public hidebysig static 
          int32  ToOle(valuetype System.Drawing.Color/*02000007*/ c) cil managed
  // SIG: 00 01 08 11 1C
  {
    // Method begins at RVA 0x22b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ColorTranslator::ToOle

  .method /*060000B4*/ public hidebysig static 
          int32  ToWin32(valuetype System.Drawing.Color/*02000007*/ c) cil managed
  // SIG: 00 01 08 11 1C
  {
    // Method begins at RVA 0x22b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ColorTranslator::ToWin32

} // end of class System.Drawing.ColorTranslator

.class /*02000009*/ public auto ansi sealed System.Drawing.KnownColor
       extends [System.Runtime/*23000001*/]System.Enum/*0100001C*/
{
  .field /*04000008*/ public specialname rtspecialname int32 value__
  .field /*04000009*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ ActiveBorder = int32(0x00000001)
  .field /*0400000A*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ ActiveCaption = int32(0x00000002)
  .field /*0400000B*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ ActiveCaptionText = int32(0x00000003)
  .field /*0400000C*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ AppWorkspace = int32(0x00000004)
  .field /*0400000D*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Control = int32(0x00000005)
  .field /*0400000E*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ ControlDark = int32(0x00000006)
  .field /*0400000F*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ ControlDarkDark = int32(0x00000007)
  .field /*04000010*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ ControlLight = int32(0x00000008)
  .field /*04000011*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ ControlLightLight = int32(0x00000009)
  .field /*04000012*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ ControlText = int32(0x0000000A)
  .field /*04000013*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Desktop = int32(0x0000000B)
  .field /*04000014*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ GrayText = int32(0x0000000C)
  .field /*04000015*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Highlight = int32(0x0000000D)
  .field /*04000016*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ HighlightText = int32(0x0000000E)
  .field /*04000017*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ HotTrack = int32(0x0000000F)
  .field /*04000018*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ InactiveBorder = int32(0x00000010)
  .field /*04000019*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ InactiveCaption = int32(0x00000011)
  .field /*0400001A*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ InactiveCaptionText = int32(0x00000012)
  .field /*0400001B*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Info = int32(0x00000013)
  .field /*0400001C*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ InfoText = int32(0x00000014)
  .field /*0400001D*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Menu = int32(0x00000015)
  .field /*0400001E*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ MenuText = int32(0x00000016)
  .field /*0400001F*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ ScrollBar = int32(0x00000017)
  .field /*04000020*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Window = int32(0x00000018)
  .field /*04000021*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ WindowFrame = int32(0x00000019)
  .field /*04000022*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ WindowText = int32(0x0000001A)
  .field /*04000023*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Transparent = int32(0x0000001B)
  .field /*04000024*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ AliceBlue = int32(0x0000001C)
  .field /*04000025*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ AntiqueWhite = int32(0x0000001D)
  .field /*04000026*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Aqua = int32(0x0000001E)
  .field /*04000027*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Aquamarine = int32(0x0000001F)
  .field /*04000028*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Azure = int32(0x00000020)
  .field /*04000029*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Beige = int32(0x00000021)
  .field /*0400002A*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Bisque = int32(0x00000022)
  .field /*0400002B*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Black = int32(0x00000023)
  .field /*0400002C*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ BlanchedAlmond = int32(0x00000024)
  .field /*0400002D*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Blue = int32(0x00000025)
  .field /*0400002E*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ BlueViolet = int32(0x00000026)
  .field /*0400002F*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Brown = int32(0x00000027)
  .field /*04000030*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ BurlyWood = int32(0x00000028)
  .field /*04000031*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ CadetBlue = int32(0x00000029)
  .field /*04000032*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Chartreuse = int32(0x0000002A)
  .field /*04000033*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Chocolate = int32(0x0000002B)
  .field /*04000034*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Coral = int32(0x0000002C)
  .field /*04000035*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ CornflowerBlue = int32(0x0000002D)
  .field /*04000036*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Cornsilk = int32(0x0000002E)
  .field /*04000037*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Crimson = int32(0x0000002F)
  .field /*04000038*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Cyan = int32(0x00000030)
  .field /*04000039*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DarkBlue = int32(0x00000031)
  .field /*0400003A*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DarkCyan = int32(0x00000032)
  .field /*0400003B*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DarkGoldenrod = int32(0x00000033)
  .field /*0400003C*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DarkGray = int32(0x00000034)
  .field /*0400003D*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DarkGreen = int32(0x00000035)
  .field /*0400003E*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DarkKhaki = int32(0x00000036)
  .field /*0400003F*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DarkMagenta = int32(0x00000037)
  .field /*04000040*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DarkOliveGreen = int32(0x00000038)
  .field /*04000041*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DarkOrange = int32(0x00000039)
  .field /*04000042*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DarkOrchid = int32(0x0000003A)
  .field /*04000043*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DarkRed = int32(0x0000003B)
  .field /*04000044*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DarkSalmon = int32(0x0000003C)
  .field /*04000045*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DarkSeaGreen = int32(0x0000003D)
  .field /*04000046*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DarkSlateBlue = int32(0x0000003E)
  .field /*04000047*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DarkSlateGray = int32(0x0000003F)
  .field /*04000048*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DarkTurquoise = int32(0x00000040)
  .field /*04000049*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DarkViolet = int32(0x00000041)
  .field /*0400004A*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DeepPink = int32(0x00000042)
  .field /*0400004B*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DeepSkyBlue = int32(0x00000043)
  .field /*0400004C*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DimGray = int32(0x00000044)
  .field /*0400004D*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ DodgerBlue = int32(0x00000045)
  .field /*0400004E*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Firebrick = int32(0x00000046)
  .field /*0400004F*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ FloralWhite = int32(0x00000047)
  .field /*04000050*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ ForestGreen = int32(0x00000048)
  .field /*04000051*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Fuchsia = int32(0x00000049)
  .field /*04000052*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Gainsboro = int32(0x0000004A)
  .field /*04000053*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ GhostWhite = int32(0x0000004B)
  .field /*04000054*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Gold = int32(0x0000004C)
  .field /*04000055*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Goldenrod = int32(0x0000004D)
  .field /*04000056*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Gray = int32(0x0000004E)
  .field /*04000057*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Green = int32(0x0000004F)
  .field /*04000058*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ GreenYellow = int32(0x00000050)
  .field /*04000059*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Honeydew = int32(0x00000051)
  .field /*0400005A*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ HotPink = int32(0x00000052)
  .field /*0400005B*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ IndianRed = int32(0x00000053)
  .field /*0400005C*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Indigo = int32(0x00000054)
  .field /*0400005D*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Ivory = int32(0x00000055)
  .field /*0400005E*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Khaki = int32(0x00000056)
  .field /*0400005F*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Lavender = int32(0x00000057)
  .field /*04000060*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ LavenderBlush = int32(0x00000058)
  .field /*04000061*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ LawnGreen = int32(0x00000059)
  .field /*04000062*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ LemonChiffon = int32(0x0000005A)
  .field /*04000063*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ LightBlue = int32(0x0000005B)
  .field /*04000064*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ LightCoral = int32(0x0000005C)
  .field /*04000065*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ LightCyan = int32(0x0000005D)
  .field /*04000066*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ LightGoldenrodYellow = int32(0x0000005E)
  .field /*04000067*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ LightGray = int32(0x0000005F)
  .field /*04000068*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ LightGreen = int32(0x00000060)
  .field /*04000069*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ LightPink = int32(0x00000061)
  .field /*0400006A*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ LightSalmon = int32(0x00000062)
  .field /*0400006B*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ LightSeaGreen = int32(0x00000063)
  .field /*0400006C*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ LightSkyBlue = int32(0x00000064)
  .field /*0400006D*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ LightSlateGray = int32(0x00000065)
  .field /*0400006E*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ LightSteelBlue = int32(0x00000066)
  .field /*0400006F*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ LightYellow = int32(0x00000067)
  .field /*04000070*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Lime = int32(0x00000068)
  .field /*04000071*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ LimeGreen = int32(0x00000069)
  .field /*04000072*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Linen = int32(0x0000006A)
  .field /*04000073*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Magenta = int32(0x0000006B)
  .field /*04000074*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Maroon = int32(0x0000006C)
  .field /*04000075*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ MediumAquamarine = int32(0x0000006D)
  .field /*04000076*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ MediumBlue = int32(0x0000006E)
  .field /*04000077*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ MediumOrchid = int32(0x0000006F)
  .field /*04000078*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ MediumPurple = int32(0x00000070)
  .field /*04000079*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ MediumSeaGreen = int32(0x00000071)
  .field /*0400007A*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ MediumSlateBlue = int32(0x00000072)
  .field /*0400007B*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ MediumSpringGreen = int32(0x00000073)
  .field /*0400007C*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ MediumTurquoise = int32(0x00000074)
  .field /*0400007D*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ MediumVioletRed = int32(0x00000075)
  .field /*0400007E*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ MidnightBlue = int32(0x00000076)
  .field /*0400007F*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ MintCream = int32(0x00000077)
  .field /*04000080*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ MistyRose = int32(0x00000078)
  .field /*04000081*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Moccasin = int32(0x00000079)
  .field /*04000082*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ NavajoWhite = int32(0x0000007A)
  .field /*04000083*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Navy = int32(0x0000007B)
  .field /*04000084*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ OldLace = int32(0x0000007C)
  .field /*04000085*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Olive = int32(0x0000007D)
  .field /*04000086*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ OliveDrab = int32(0x0000007E)
  .field /*04000087*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Orange = int32(0x0000007F)
  .field /*04000088*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ OrangeRed = int32(0x00000080)
  .field /*04000089*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Orchid = int32(0x00000081)
  .field /*0400008A*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ PaleGoldenrod = int32(0x00000082)
  .field /*0400008B*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ PaleGreen = int32(0x00000083)
  .field /*0400008C*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ PaleTurquoise = int32(0x00000084)
  .field /*0400008D*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ PaleVioletRed = int32(0x00000085)
  .field /*0400008E*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ PapayaWhip = int32(0x00000086)
  .field /*0400008F*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ PeachPuff = int32(0x00000087)
  .field /*04000090*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Peru = int32(0x00000088)
  .field /*04000091*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Pink = int32(0x00000089)
  .field /*04000092*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Plum = int32(0x0000008A)
  .field /*04000093*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ PowderBlue = int32(0x0000008B)
  .field /*04000094*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Purple = int32(0x0000008C)
  .field /*04000095*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Red = int32(0x0000008D)
  .field /*04000096*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ RosyBrown = int32(0x0000008E)
  .field /*04000097*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ RoyalBlue = int32(0x0000008F)
  .field /*04000098*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ SaddleBrown = int32(0x00000090)
  .field /*04000099*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Salmon = int32(0x00000091)
  .field /*0400009A*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ SandyBrown = int32(0x00000092)
  .field /*0400009B*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ SeaGreen = int32(0x00000093)
  .field /*0400009C*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ SeaShell = int32(0x00000094)
  .field /*0400009D*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Sienna = int32(0x00000095)
  .field /*0400009E*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Silver = int32(0x00000096)
  .field /*0400009F*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ SkyBlue = int32(0x00000097)
  .field /*040000A0*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ SlateBlue = int32(0x00000098)
  .field /*040000A1*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ SlateGray = int32(0x00000099)
  .field /*040000A2*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Snow = int32(0x0000009A)
  .field /*040000A3*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ SpringGreen = int32(0x0000009B)
  .field /*040000A4*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ SteelBlue = int32(0x0000009C)
  .field /*040000A5*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Tan = int32(0x0000009D)
  .field /*040000A6*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Teal = int32(0x0000009E)
  .field /*040000A7*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Thistle = int32(0x0000009F)
  .field /*040000A8*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Tomato = int32(0x000000A0)
  .field /*040000A9*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Turquoise = int32(0x000000A1)
  .field /*040000AA*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Violet = int32(0x000000A2)
  .field /*040000AB*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Wheat = int32(0x000000A3)
  .field /*040000AC*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ White = int32(0x000000A4)
  .field /*040000AD*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ WhiteSmoke = int32(0x000000A5)
  .field /*040000AE*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ Yellow = int32(0x000000A6)
  .field /*040000AF*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ YellowGreen = int32(0x000000A7)
  .field /*040000B0*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ ButtonFace = int32(0x000000A8)
  .field /*040000B1*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ ButtonHighlight = int32(0x000000A9)
  .field /*040000B2*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ ButtonShadow = int32(0x000000AA)
  .field /*040000B3*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ GradientActiveCaption = int32(0x000000AB)
  .field /*040000B4*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ GradientInactiveCaption = int32(0x000000AC)
  .field /*040000B5*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ MenuBar = int32(0x000000AD)
  .field /*040000B6*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ MenuHighlight = int32(0x000000AE)
  .field /*040000B7*/ public static literal valuetype System.Drawing.KnownColor/*02000009*/ RebeccaPurple = int32(0x000000AF)
} // end of class System.Drawing.KnownColor

.class /*0200000A*/ public sequential ansi sealed beforefieldinit System.Drawing.Point
       extends [System.Runtime/*23000001*/]System.ValueType/*01000018*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000019*/<valuetype System.Drawing.Point/*0200000A*/>/*1B000002*/
{
  .custom /*0C00002C:0A000014*/ instance void [System.ObjectModel/*23000003*/]System.ComponentModel.TypeConverterAttribute/*01000017*/::.ctor(string) /* 0A000014 */ = ( 01 00 70 53 79 73 74 65 6D 2E 44 72 61 77 69 6E   // ..pSystem.Drawin
                                                                                                                                                                         67 2E 50 6F 69 6E 74 43 6F 6E 76 65 72 74 65 72   // g.PointConverter
                                                                                                                                                                         2C 20 53 79 73 74 65 6D 2E 44 72 61 77 69 6E 67   // , System.Drawing
                                                                                                                                                                         2C 20 56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E   // , Version=4.0.0.
                                                                                                                                                                         30 2C 20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72   // 0, Culture=neutr
                                                                                                                                                                         61 6C 2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B   // al, PublicKeyTok
                                                                                                                                                                         65 6E 3D 62 30 33 66 35 66 37 66 31 31 64 35 30   // en=b03f5f7f11d50
                                                                                                                                                                         61 33 61 00 00 )                                  // a3a..
  .field /*040000B8*/ private int32 _dummyPrimitive
  .field /*040000B9*/ public static initonly valuetype System.Drawing.Point/*0200000A*/ Empty
  .method /*060000B5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Drawing.Size/*0200000E*/ sz) cil managed
  // SIG: 20 01 01 11 38
  {
    // Method begins at RVA 0x22b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::.ctor

  .method /*060000B6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 dw) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x22b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::.ctor

  .method /*060000B7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 x,
                               int32 y) cil managed
  // SIG: 20 02 01 08 08
  {
    // Method begins at RVA 0x22bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::.ctor

  .method /*060000B8*/ public hidebysig specialname 
          instance bool  get_IsEmpty() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000047:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::get_IsEmpty

  .method /*060000B9*/ public hidebysig specialname 
          instance int32  get_X() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000049:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::get_X

  .method /*060000BA*/ public hidebysig specialname 
          instance void  set_X(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x22c5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Point::set_X

  .method /*060000BB*/ public hidebysig specialname 
          instance int32  get_Y() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00004A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::get_Y

  .method /*060000BC*/ public hidebysig specialname 
          instance void  set_Y(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x22ca
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Point::set_Y

  .method /*060000BD*/ public hidebysig static 
          valuetype System.Drawing.Point/*0200000A*/ 
          Add(valuetype System.Drawing.Point/*0200000A*/ pt,
              valuetype System.Drawing.Size/*0200000E*/ sz) cil managed
  // SIG: 00 02 11 28 11 28 11 38
  {
    // Method begins at RVA 0x22cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::Add

  .method /*060000BE*/ public hidebysig static 
          valuetype System.Drawing.Point/*0200000A*/ 
          Ceiling(valuetype System.Drawing.PointF/*0200000B*/ 'value') cil managed
  // SIG: 00 01 11 28 11 2C
  {
    // Method begins at RVA 0x22cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::Ceiling

  .method /*060000BF*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Drawing.Point/*0200000A*/ other) cil managed
  // SIG: 20 01 02 11 28
  {
    .custom /*0C00004B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::Equals

  .method /*060000C0*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00004C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C00004D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000023*/ 
    .custom /*0C000030:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001B*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::Equals

  .method /*060000C1*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00004E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::GetHashCode

  .method /*060000C2*/ public hidebysig instance void 
          Offset(valuetype System.Drawing.Point/*0200000A*/ p) cil managed
  // SIG: 20 01 01 11 28
  {
    // Method begins at RVA 0x22db
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Point::Offset

  .method /*060000C3*/ public hidebysig instance void 
          Offset(int32 dx,
                 int32 dy) cil managed
  // SIG: 20 02 01 08 08
  {
    // Method begins at RVA 0x22dd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Point::Offset

  .method /*060000C4*/ public hidebysig specialname static 
          valuetype System.Drawing.Point/*0200000A*/ 
          op_Addition(valuetype System.Drawing.Point/*0200000A*/ pt,
                      valuetype System.Drawing.Size/*0200000E*/ sz) cil managed
  // SIG: 00 02 11 28 11 28 11 38
  {
    // Method begins at RVA 0x22df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::op_Addition

  .method /*060000C5*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Drawing.Point/*0200000A*/ left,
                            valuetype System.Drawing.Point/*0200000A*/ right) cil managed
  // SIG: 00 02 02 11 28 11 28
  {
    // Method begins at RVA 0x22e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::op_Equality

  .method /*060000C6*/ public hidebysig specialname static 
          valuetype System.Drawing.Size/*0200000E*/ 
          op_Explicit(valuetype System.Drawing.Point/*0200000A*/ p) cil managed
  // SIG: 00 01 11 38 11 28
  {
    // Method begins at RVA 0x22e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::op_Explicit

  .method /*060000C7*/ public hidebysig specialname static 
          valuetype System.Drawing.PointF/*0200000B*/ 
          op_Implicit(valuetype System.Drawing.Point/*0200000A*/ p) cil managed
  // SIG: 00 01 11 2C 11 28
  {
    // Method begins at RVA 0x22e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::op_Implicit

  .method /*060000C8*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Drawing.Point/*0200000A*/ left,
                              valuetype System.Drawing.Point/*0200000A*/ right) cil managed
  // SIG: 00 02 02 11 28 11 28
  {
    // Method begins at RVA 0x22eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::op_Inequality

  .method /*060000C9*/ public hidebysig specialname static 
          valuetype System.Drawing.Point/*0200000A*/ 
          op_Subtraction(valuetype System.Drawing.Point/*0200000A*/ pt,
                         valuetype System.Drawing.Size/*0200000E*/ sz) cil managed
  // SIG: 00 02 11 28 11 28 11 38
  {
    // Method begins at RVA 0x22ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::op_Subtraction

  .method /*060000CA*/ public hidebysig static 
          valuetype System.Drawing.Point/*0200000A*/ 
          Round(valuetype System.Drawing.PointF/*0200000B*/ 'value') cil managed
  // SIG: 00 01 11 28 11 2C
  {
    // Method begins at RVA 0x22f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::Round

  .method /*060000CB*/ public hidebysig static 
          valuetype System.Drawing.Point/*0200000A*/ 
          Subtract(valuetype System.Drawing.Point/*0200000A*/ pt,
                   valuetype System.Drawing.Size/*0200000E*/ sz) cil managed
  // SIG: 00 02 11 28 11 28 11 38
  {
    // Method begins at RVA 0x22f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::Subtract

  .method /*060000CC*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00004F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C000050:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::ToString

  .method /*060000CD*/ public hidebysig static 
          valuetype System.Drawing.Point/*0200000A*/ 
          Truncate(valuetype System.Drawing.PointF/*0200000B*/ 'value') cil managed
  // SIG: 00 01 11 28 11 2C
  {
    // Method begins at RVA 0x22fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Point::Truncate

  .property /*17000098*/ instance callconv(8) bool
          IsEmpty()
  {
    .custom /*0C000034:0A000016*/ instance void [System.ComponentModel.Primitives/*23000002*/]System.ComponentModel.BrowsableAttribute/*0100001D*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance bool System.Drawing.Point/*0200000A*/::get_IsEmpty() /* 060000B8 */
  } // end of property Point::IsEmpty
  .property /*17000099*/ instance callconv(8) int32
          X()
  {
    .get instance int32 System.Drawing.Point/*0200000A*/::get_X() /* 060000B9 */
    .set instance void System.Drawing.Point/*0200000A*/::set_X(int32) /* 060000BA */
  } // end of property Point::X
  .property /*1700009A*/ instance callconv(8) int32
          Y()
  {
    .get instance int32 System.Drawing.Point/*0200000A*/::get_Y() /* 060000BB */
    .set instance void System.Drawing.Point/*0200000A*/::set_Y(int32) /* 060000BC */
  } // end of property Point::Y
} // end of class System.Drawing.Point

.class /*0200000B*/ public sequential ansi sealed beforefieldinit System.Drawing.PointF
       extends [System.Runtime/*23000001*/]System.ValueType/*01000018*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000019*/<valuetype System.Drawing.PointF/*0200000B*/>/*1B000003*/
{
  .field /*040000BA*/ private int32 _dummyPrimitive
  .field /*040000BB*/ public static initonly valuetype System.Drawing.PointF/*0200000B*/ Empty
  .method /*060000CE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(float32 x,
                               float32 y) cil managed
  // SIG: 20 02 01 0C 0C
  {
    // Method begins at RVA 0x22fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::.ctor

  .method /*060000CF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Numerics.Vectors/*23000004*/]System.Numerics.Vector2/*0100001E*/ 'vector') cil managed
  // SIG: 20 01 01 11 79
  {
    // Method begins at RVA 0x2300
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::.ctor

  .method /*060000D0*/ public hidebysig specialname 
          instance bool  get_IsEmpty() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000051:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2303
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::get_IsEmpty

  .method /*060000D1*/ public hidebysig specialname 
          instance float32  get_X() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C000052:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2306
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::get_X

  .method /*060000D2*/ public hidebysig specialname 
          instance void  set_X(float32 'value') cil managed
  // SIG: 20 01 01 0C
  {
    // Method begins at RVA 0x2309
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PointF::set_X

  .method /*060000D3*/ public hidebysig specialname 
          instance float32  get_Y() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C000053:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x230b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::get_Y

  .method /*060000D4*/ public hidebysig specialname 
          instance void  set_Y(float32 'value') cil managed
  // SIG: 20 01 01 0C
  {
    // Method begins at RVA 0x230e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PointF::set_Y

  .method /*060000D5*/ public hidebysig static 
          valuetype System.Drawing.PointF/*0200000B*/ 
          Add(valuetype System.Drawing.PointF/*0200000B*/ pt,
              valuetype System.Drawing.Size/*0200000E*/ sz) cil managed
  // SIG: 00 02 11 2C 11 2C 11 38
  {
    // Method begins at RVA 0x2310
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::Add

  .method /*060000D6*/ public hidebysig static 
          valuetype System.Drawing.PointF/*0200000B*/ 
          Add(valuetype System.Drawing.PointF/*0200000B*/ pt,
              valuetype System.Drawing.SizeF/*0200000F*/ sz) cil managed
  // SIG: 00 02 11 2C 11 2C 11 3C
  {
    // Method begins at RVA 0x2313
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::Add

  .method /*060000D7*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Drawing.PointF/*0200000B*/ other) cil managed
  // SIG: 20 01 02 11 2C
  {
    .custom /*0C000054:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2316
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::Equals

  .method /*060000D8*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000055:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C000056:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800003F*/ 
    .custom /*0C000031:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001B*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2319
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::Equals

  .method /*060000D9*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000057:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x231c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::GetHashCode

  .method /*060000DA*/ public hidebysig specialname static 
          valuetype [System.Numerics.Vectors/*23000004*/]System.Numerics.Vector2/*0100001E*/ 
          op_Explicit(valuetype System.Drawing.PointF/*0200000B*/ point) cil managed
  // SIG: 00 01 11 79 11 2C
  {
    // Method begins at RVA 0x231f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::op_Explicit

  .method /*060000DB*/ public hidebysig specialname static 
          valuetype System.Drawing.PointF/*0200000B*/ 
          op_Explicit(valuetype [System.Numerics.Vectors/*23000004*/]System.Numerics.Vector2/*0100001E*/ 'vector') cil managed
  // SIG: 00 01 11 2C 11 79
  {
    // Method begins at RVA 0x2322
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::op_Explicit

  .method /*060000DC*/ public hidebysig specialname static 
          valuetype System.Drawing.PointF/*0200000B*/ 
          op_Addition(valuetype System.Drawing.PointF/*0200000B*/ pt,
                      valuetype System.Drawing.Size/*0200000E*/ sz) cil managed
  // SIG: 00 02 11 2C 11 2C 11 38
  {
    // Method begins at RVA 0x2325
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::op_Addition

  .method /*060000DD*/ public hidebysig specialname static 
          valuetype System.Drawing.PointF/*0200000B*/ 
          op_Addition(valuetype System.Drawing.PointF/*0200000B*/ pt,
                      valuetype System.Drawing.SizeF/*0200000F*/ sz) cil managed
  // SIG: 00 02 11 2C 11 2C 11 3C
  {
    // Method begins at RVA 0x2328
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::op_Addition

  .method /*060000DE*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Drawing.PointF/*0200000B*/ left,
                            valuetype System.Drawing.PointF/*0200000B*/ right) cil managed
  // SIG: 00 02 02 11 2C 11 2C
  {
    // Method begins at RVA 0x232b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::op_Equality

  .method /*060000DF*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Drawing.PointF/*0200000B*/ left,
                              valuetype System.Drawing.PointF/*0200000B*/ right) cil managed
  // SIG: 00 02 02 11 2C 11 2C
  {
    // Method begins at RVA 0x232e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::op_Inequality

  .method /*060000E0*/ public hidebysig specialname static 
          valuetype System.Drawing.PointF/*0200000B*/ 
          op_Subtraction(valuetype System.Drawing.PointF/*0200000B*/ pt,
                         valuetype System.Drawing.Size/*0200000E*/ sz) cil managed
  // SIG: 00 02 11 2C 11 2C 11 38
  {
    // Method begins at RVA 0x2331
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::op_Subtraction

  .method /*060000E1*/ public hidebysig specialname static 
          valuetype System.Drawing.PointF/*0200000B*/ 
          op_Subtraction(valuetype System.Drawing.PointF/*0200000B*/ pt,
                         valuetype System.Drawing.SizeF/*0200000F*/ sz) cil managed
  // SIG: 00 02 11 2C 11 2C 11 3C
  {
    // Method begins at RVA 0x2334
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::op_Subtraction

  .method /*060000E2*/ public hidebysig static 
          valuetype System.Drawing.PointF/*0200000B*/ 
          Subtract(valuetype System.Drawing.PointF/*0200000B*/ pt,
                   valuetype System.Drawing.Size/*0200000E*/ sz) cil managed
  // SIG: 00 02 11 2C 11 2C 11 38
  {
    // Method begins at RVA 0x2337
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::Subtract

  .method /*060000E3*/ public hidebysig static 
          valuetype System.Drawing.PointF/*0200000B*/ 
          Subtract(valuetype System.Drawing.PointF/*0200000B*/ pt,
                   valuetype System.Drawing.SizeF/*0200000F*/ sz) cil managed
  // SIG: 00 02 11 2C 11 2C 11 3C
  {
    // Method begins at RVA 0x233a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::Subtract

  .method /*060000E4*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000059:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C00005A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x233d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::ToString

  .method /*060000E5*/ public hidebysig instance valuetype [System.Numerics.Vectors/*23000004*/]System.Numerics.Vector2/*0100001E*/ 
          ToVector2() cil managed
  // SIG: 20 00 11 79
  {
    // Method begins at RVA 0x2340
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PointF::ToVector2

  .property /*1700009B*/ instance callconv(8) bool
          IsEmpty()
  {
    .custom /*0C000035:0A000016*/ instance void [System.ComponentModel.Primitives/*23000002*/]System.ComponentModel.BrowsableAttribute/*0100001D*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance bool System.Drawing.PointF/*0200000B*/::get_IsEmpty() /* 060000D0 */
  } // end of property PointF::IsEmpty
  .property /*1700009C*/ instance callconv(8) float32
          X()
  {
    .get instance float32 System.Drawing.PointF/*0200000B*/::get_X() /* 060000D1 */
    .set instance void System.Drawing.PointF/*0200000B*/::set_X(float32) /* 060000D2 */
  } // end of property PointF::X
  .property /*1700009D*/ instance callconv(8) float32
          Y()
  {
    .get instance float32 System.Drawing.PointF/*0200000B*/::get_Y() /* 060000D3 */
    .set instance void System.Drawing.PointF/*0200000B*/::set_Y(float32) /* 060000D4 */
  } // end of property PointF::Y
} // end of class System.Drawing.PointF

.class /*0200000C*/ public sequential ansi sealed beforefieldinit System.Drawing.Rectangle
       extends [System.Runtime/*23000001*/]System.ValueType/*01000018*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000019*/<valuetype System.Drawing.Rectangle/*0200000C*/>/*1B000004*/
{
  .custom /*0C00002D:0A000014*/ instance void [System.ObjectModel/*23000003*/]System.ComponentModel.TypeConverterAttribute/*01000017*/::.ctor(string) /* 0A000014 */ = ( 01 00 74 53 79 73 74 65 6D 2E 44 72 61 77 69 6E   // ..tSystem.Drawin
                                                                                                                                                                         67 2E 52 65 63 74 61 6E 67 6C 65 43 6F 6E 76 65   // g.RectangleConve
                                                                                                                                                                         72 74 65 72 2C 20 53 79 73 74 65 6D 2E 44 72 61   // rter, System.Dra
                                                                                                                                                                         77 69 6E 67 2C 20 56 65 72 73 69 6F 6E 3D 34 2E   // wing, Version=4.
                                                                                                                                                                         30 2E 30 2E 30 2C 20 43 75 6C 74 75 72 65 3D 6E   // 0.0.0, Culture=n
                                                                                                                                                                         65 75 74 72 61 6C 2C 20 50 75 62 6C 69 63 4B 65   // eutral, PublicKe
                                                                                                                                                                         79 54 6F 6B 65 6E 3D 62 30 33 66 35 66 37 66 31   // yToken=b03f5f7f1
                                                                                                                                                                         31 64 35 30 61 33 61 00 00 )                      // 1d50a3a..
  .field /*040000BC*/ private int32 _dummyPrimitive
  .field /*040000BD*/ public static initonly valuetype System.Drawing.Rectangle/*0200000C*/ Empty
  .method /*060000E6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Drawing.Point/*0200000A*/ location,
                               valuetype System.Drawing.Size/*0200000E*/ size) cil managed
  // SIG: 20 02 01 11 28 11 38
  {
    // Method begins at RVA 0x2343
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::.ctor

  .method /*060000E7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 x,
                               int32 y,
                               int32 width,
                               int32 height) cil managed
  // SIG: 20 04 01 08 08 08 08
  {
    // Method begins at RVA 0x2346
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::.ctor

  .method /*060000E8*/ public hidebysig specialname 
          instance int32  get_Bottom() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00005B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2349
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::get_Bottom

  .method /*060000E9*/ public hidebysig specialname 
          instance int32  get_Height() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00005C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x234c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::get_Height

  .method /*060000EA*/ public hidebysig specialname 
          instance void  set_Height(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x234f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Rectangle::set_Height

  .method /*060000EB*/ public hidebysig specialname 
          instance bool  get_IsEmpty() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00005D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2351
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::get_IsEmpty

  .method /*060000EC*/ public hidebysig specialname 
          instance int32  get_Left() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00005E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2354
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::get_Left

  .method /*060000ED*/ public hidebysig specialname 
          instance valuetype System.Drawing.Point/*0200000A*/ 
          get_Location() cil managed
  // SIG: 20 00 11 28
  {
    .custom /*0C00005F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2357
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::get_Location

  .method /*060000EE*/ public hidebysig specialname 
          instance void  set_Location(valuetype System.Drawing.Point/*0200000A*/ 'value') cil managed
  // SIG: 20 01 01 11 28
  {
    // Method begins at RVA 0x235a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Rectangle::set_Location

  .method /*060000EF*/ public hidebysig specialname 
          instance int32  get_Right() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000060:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x235c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::get_Right

  .method /*060000F0*/ public hidebysig specialname 
          instance valuetype System.Drawing.Size/*0200000E*/ 
          get_Size() cil managed
  // SIG: 20 00 11 38
  {
    .custom /*0C000061:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x235f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::get_Size

  .method /*060000F1*/ public hidebysig specialname 
          instance void  set_Size(valuetype System.Drawing.Size/*0200000E*/ 'value') cil managed
  // SIG: 20 01 01 11 38
  {
    // Method begins at RVA 0x2362
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Rectangle::set_Size

  .method /*060000F2*/ public hidebysig specialname 
          instance int32  get_Top() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000062:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2364
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::get_Top

  .method /*060000F3*/ public hidebysig specialname 
          instance int32  get_Width() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000063:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2367
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::get_Width

  .method /*060000F4*/ public hidebysig specialname 
          instance void  set_Width(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x236a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Rectangle::set_Width

  .method /*060000F5*/ public hidebysig specialname 
          instance int32  get_X() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000064:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x236c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::get_X

  .method /*060000F6*/ public hidebysig specialname 
          instance void  set_X(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x236f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Rectangle::set_X

  .method /*060000F7*/ public hidebysig specialname 
          instance int32  get_Y() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000065:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2371
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::get_Y

  .method /*060000F8*/ public hidebysig specialname 
          instance void  set_Y(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2374
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Rectangle::set_Y

  .method /*060000F9*/ public hidebysig static 
          valuetype System.Drawing.Rectangle/*0200000C*/ 
          Ceiling(valuetype System.Drawing.RectangleF/*0200000D*/ 'value') cil managed
  // SIG: 00 01 11 30 11 34
  {
    // Method begins at RVA 0x2376
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::Ceiling

  .method /*060000FA*/ public hidebysig instance bool 
          Contains(valuetype System.Drawing.Point/*0200000A*/ pt) cil managed
  // SIG: 20 01 02 11 28
  {
    .custom /*0C000066:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2379
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::Contains

  .method /*060000FB*/ public hidebysig instance bool 
          Contains(valuetype System.Drawing.Rectangle/*0200000C*/ rect) cil managed
  // SIG: 20 01 02 11 30
  {
    .custom /*0C000067:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x237c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::Contains

  .method /*060000FC*/ public hidebysig instance bool 
          Contains(int32 x,
                   int32 y) cil managed
  // SIG: 20 02 02 08 08
  {
    .custom /*0C000068:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x237f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::Contains

  .method /*060000FD*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Drawing.Rectangle/*0200000C*/ other) cil managed
  // SIG: 20 01 02 11 30
  {
    .custom /*0C000069:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2382
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::Equals

  .method /*060000FE*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00006A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C00006B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000064*/ 
    .custom /*0C000032:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001B*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2385
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::Equals

  .method /*060000FF*/ public hidebysig static 
          valuetype System.Drawing.Rectangle/*0200000C*/ 
          FromLTRB(int32 left,
                   int32 top,
                   int32 right,
                   int32 bottom) cil managed
  // SIG: 00 04 11 30 08 08 08 08
  {
    // Method begins at RVA 0x2388
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::FromLTRB

  .method /*06000100*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00006C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x238b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::GetHashCode

  .method /*06000101*/ public hidebysig static 
          valuetype System.Drawing.Rectangle/*0200000C*/ 
          Inflate(valuetype System.Drawing.Rectangle/*0200000C*/ rect,
                  int32 x,
                  int32 y) cil managed
  // SIG: 00 03 11 30 11 30 08 08
  {
    // Method begins at RVA 0x238e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::Inflate

  .method /*06000102*/ public hidebysig instance void 
          Inflate(valuetype System.Drawing.Size/*0200000E*/ size) cil managed
  // SIG: 20 01 01 11 38
  {
    // Method begins at RVA 0x2391
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Rectangle::Inflate

  .method /*06000103*/ public hidebysig instance void 
          Inflate(int32 width,
                  int32 height) cil managed
  // SIG: 20 02 01 08 08
  {
    // Method begins at RVA 0x2393
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Rectangle::Inflate

  .method /*06000104*/ public hidebysig instance void 
          Intersect(valuetype System.Drawing.Rectangle/*0200000C*/ rect) cil managed
  // SIG: 20 01 01 11 30
  {
    // Method begins at RVA 0x2395
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Rectangle::Intersect

  .method /*06000105*/ public hidebysig static 
          valuetype System.Drawing.Rectangle/*0200000C*/ 
          Intersect(valuetype System.Drawing.Rectangle/*0200000C*/ a,
                    valuetype System.Drawing.Rectangle/*0200000C*/ b) cil managed
  // SIG: 00 02 11 30 11 30 11 30
  {
    // Method begins at RVA 0x2397
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::Intersect

  .method /*06000106*/ public hidebysig instance bool 
          IntersectsWith(valuetype System.Drawing.Rectangle/*0200000C*/ rect) cil managed
  // SIG: 20 01 02 11 30
  {
    .custom /*0C00006D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x239a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::IntersectsWith

  .method /*06000107*/ public hidebysig instance void 
          Offset(valuetype System.Drawing.Point/*0200000A*/ pos) cil managed
  // SIG: 20 01 01 11 28
  {
    // Method begins at RVA 0x239d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Rectangle::Offset

  .method /*06000108*/ public hidebysig instance void 
          Offset(int32 x,
                 int32 y) cil managed
  // SIG: 20 02 01 08 08
  {
    // Method begins at RVA 0x239f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Rectangle::Offset

  .method /*06000109*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Drawing.Rectangle/*0200000C*/ left,
                            valuetype System.Drawing.Rectangle/*0200000C*/ right) cil managed
  // SIG: 00 02 02 11 30 11 30
  {
    // Method begins at RVA 0x23a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::op_Equality

  .method /*0600010A*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Drawing.Rectangle/*0200000C*/ left,
                              valuetype System.Drawing.Rectangle/*0200000C*/ right) cil managed
  // SIG: 00 02 02 11 30 11 30
  {
    // Method begins at RVA 0x23a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::op_Inequality

  .method /*0600010B*/ public hidebysig static 
          valuetype System.Drawing.Rectangle/*0200000C*/ 
          Round(valuetype System.Drawing.RectangleF/*0200000D*/ 'value') cil managed
  // SIG: 00 01 11 30 11 34
  {
    // Method begins at RVA 0x23a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::Round

  .method /*0600010C*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00006E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C00006F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::ToString

  .method /*0600010D*/ public hidebysig static 
          valuetype System.Drawing.Rectangle/*0200000C*/ 
          Truncate(valuetype System.Drawing.RectangleF/*0200000D*/ 'value') cil managed
  // SIG: 00 01 11 30 11 34
  {
    // Method begins at RVA 0x23ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::Truncate

  .method /*0600010E*/ public hidebysig static 
          valuetype System.Drawing.Rectangle/*0200000C*/ 
          Union(valuetype System.Drawing.Rectangle/*0200000C*/ a,
                valuetype System.Drawing.Rectangle/*0200000C*/ b) cil managed
  // SIG: 00 02 11 30 11 30 11 30
  {
    // Method begins at RVA 0x23b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rectangle::Union

  .property /*1700009E*/ instance callconv(8) int32
          Bottom()
  {
    .custom /*0C000036:0A000016*/ instance void [System.ComponentModel.Primitives/*23000002*/]System.ComponentModel.BrowsableAttribute/*0100001D*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance int32 System.Drawing.Rectangle/*0200000C*/::get_Bottom() /* 060000E8 */
  } // end of property Rectangle::Bottom
  .property /*1700009F*/ instance callconv(8) int32
          Height()
  {
    .get instance int32 System.Drawing.Rectangle/*0200000C*/::get_Height() /* 060000E9 */
    .set instance void System.Drawing.Rectangle/*0200000C*/::set_Height(int32) /* 060000EA */
  } // end of property Rectangle::Height
  .property /*170000A0*/ instance callconv(8) bool
          IsEmpty()
  {
    .custom /*0C000038:0A000016*/ instance void [System.ComponentModel.Primitives/*23000002*/]System.ComponentModel.BrowsableAttribute/*0100001D*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance bool System.Drawing.Rectangle/*0200000C*/::get_IsEmpty() /* 060000EB */
  } // end of property Rectangle::IsEmpty
  .property /*170000A1*/ instance callconv(8) int32
          Left()
  {
    .custom /*0C000039:0A000016*/ instance void [System.ComponentModel.Primitives/*23000002*/]System.ComponentModel.BrowsableAttribute/*0100001D*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance int32 System.Drawing.Rectangle/*0200000C*/::get_Left() /* 060000EC */
  } // end of property Rectangle::Left
  .property /*170000A2*/ instance callconv(8) valuetype System.Drawing.Point/*0200000A*/
          Location()
  {
    .custom /*0C00003A:0A000016*/ instance void [System.ComponentModel.Primitives/*23000002*/]System.ComponentModel.BrowsableAttribute/*0100001D*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance valuetype System.Drawing.Point/*0200000A*/ System.Drawing.Rectangle/*0200000C*/::get_Location() /* 060000ED */
    .set instance void System.Drawing.Rectangle/*0200000C*/::set_Location(valuetype System.Drawing.Point/*0200000A*/) /* 060000EE */
  } // end of property Rectangle::Location
  .property /*170000A3*/ instance callconv(8) int32
          Right()
  {
    .custom /*0C00003B:0A000016*/ instance void [System.ComponentModel.Primitives/*23000002*/]System.ComponentModel.BrowsableAttribute/*0100001D*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance int32 System.Drawing.Rectangle/*0200000C*/::get_Right() /* 060000EF */
  } // end of property Rectangle::Right
  .property /*170000A4*/ instance callconv(8) valuetype System.Drawing.Size/*0200000E*/
          Size()
  {
    .custom /*0C00003C:0A000016*/ instance void [System.ComponentModel.Primitives/*23000002*/]System.ComponentModel.BrowsableAttribute/*0100001D*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance valuetype System.Drawing.Size/*0200000E*/ System.Drawing.Rectangle/*0200000C*/::get_Size() /* 060000F0 */
    .set instance void System.Drawing.Rectangle/*0200000C*/::set_Size(valuetype System.Drawing.Size/*0200000E*/) /* 060000F1 */
  } // end of property Rectangle::Size
  .property /*170000A5*/ instance callconv(8) int32
          Top()
  {
    .custom /*0C00003D:0A000016*/ instance void [System.ComponentModel.Primitives/*23000002*/]System.ComponentModel.BrowsableAttribute/*0100001D*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance int32 System.Drawing.Rectangle/*0200000C*/::get_Top() /* 060000F2 */
  } // end of property Rectangle::Top
  .property /*170000A6*/ instance callconv(8) int32
          Width()
  {
    .get instance int32 System.Drawing.Rectangle/*0200000C*/::get_Width() /* 060000F3 */
    .set instance void System.Drawing.Rectangle/*0200000C*/::set_Width(int32) /* 060000F4 */
  } // end of property Rectangle::Width
  .property /*170000A7*/ instance callconv(8) int32
          X()
  {
    .get instance int32 System.Drawing.Rectangle/*0200000C*/::get_X() /* 060000F5 */
    .set instance void System.Drawing.Rectangle/*0200000C*/::set_X(int32) /* 060000F6 */
  } // end of property Rectangle::X
  .property /*170000A8*/ instance callconv(8) int32
          Y()
  {
    .get instance int32 System.Drawing.Rectangle/*0200000C*/::get_Y() /* 060000F7 */
    .set instance void System.Drawing.Rectangle/*0200000C*/::set_Y(int32) /* 060000F8 */
  } // end of property Rectangle::Y
} // end of class System.Drawing.Rectangle

.class /*0200000D*/ public sequential ansi sealed beforefieldinit System.Drawing.RectangleF
       extends [System.Runtime/*23000001*/]System.ValueType/*01000018*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000019*/<valuetype System.Drawing.RectangleF/*0200000D*/>/*1B000005*/
{
  .field /*040000BE*/ private int32 _dummyPrimitive
  .field /*040000BF*/ public static initonly valuetype System.Drawing.RectangleF/*0200000D*/ Empty
  .method /*0600010F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Drawing.PointF/*0200000B*/ location,
                               valuetype System.Drawing.SizeF/*0200000F*/ size) cil managed
  // SIG: 20 02 01 11 2C 11 3C
  {
    // Method begins at RVA 0x23b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::.ctor

  .method /*06000110*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(float32 x,
                               float32 y,
                               float32 width,
                               float32 height) cil managed
  // SIG: 20 04 01 0C 0C 0C 0C
  {
    // Method begins at RVA 0x23b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::.ctor

  .method /*06000111*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Numerics.Vectors/*23000004*/]System.Numerics.Vector4/*0100001F*/ 'vector') cil managed
  // SIG: 20 01 01 11 7D
  {
    // Method begins at RVA 0x23b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::.ctor

  .method /*06000112*/ public hidebysig specialname 
          instance float32  get_Bottom() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C000070:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::get_Bottom

  .method /*06000113*/ public hidebysig specialname 
          instance float32  get_Height() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C000071:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::get_Height

  .method /*06000114*/ public hidebysig specialname 
          instance void  set_Height(float32 'value') cil managed
  // SIG: 20 01 01 0C
  {
    // Method begins at RVA 0x23c2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RectangleF::set_Height

  .method /*06000115*/ public hidebysig specialname 
          instance bool  get_IsEmpty() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000072:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::get_IsEmpty

  .method /*06000116*/ public hidebysig specialname 
          instance float32  get_Left() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C000073:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::get_Left

  .method /*06000117*/ public hidebysig specialname 
          instance valuetype System.Drawing.PointF/*0200000B*/ 
          get_Location() cil managed
  // SIG: 20 00 11 2C
  {
    .custom /*0C000074:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::get_Location

  .method /*06000118*/ public hidebysig specialname 
          instance void  set_Location(valuetype System.Drawing.PointF/*0200000B*/ 'value') cil managed
  // SIG: 20 01 01 11 2C
  {
    // Method begins at RVA 0x23cd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RectangleF::set_Location

  .method /*06000119*/ public hidebysig specialname 
          instance float32  get_Right() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C000075:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::get_Right

  .method /*0600011A*/ public hidebysig specialname 
          instance valuetype System.Drawing.SizeF/*0200000F*/ 
          get_Size() cil managed
  // SIG: 20 00 11 3C
  {
    .custom /*0C000076:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::get_Size

  .method /*0600011B*/ public hidebysig specialname 
          instance void  set_Size(valuetype System.Drawing.SizeF/*0200000F*/ 'value') cil managed
  // SIG: 20 01 01 11 3C
  {
    // Method begins at RVA 0x23d5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RectangleF::set_Size

  .method /*0600011C*/ public hidebysig specialname 
          instance float32  get_Top() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C000077:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::get_Top

  .method /*0600011D*/ public hidebysig specialname 
          instance float32  get_Width() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C000078:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::get_Width

  .method /*0600011E*/ public hidebysig specialname 
          instance void  set_Width(float32 'value') cil managed
  // SIG: 20 01 01 0C
  {
    // Method begins at RVA 0x23dd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RectangleF::set_Width

  .method /*0600011F*/ public hidebysig specialname 
          instance float32  get_X() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C000079:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::get_X

  .method /*06000120*/ public hidebysig specialname 
          instance void  set_X(float32 'value') cil managed
  // SIG: 20 01 01 0C
  {
    // Method begins at RVA 0x23e2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RectangleF::set_X

  .method /*06000121*/ public hidebysig specialname 
          instance float32  get_Y() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C00007A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::get_Y

  .method /*06000122*/ public hidebysig specialname 
          instance void  set_Y(float32 'value') cil managed
  // SIG: 20 01 01 0C
  {
    // Method begins at RVA 0x23e7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RectangleF::set_Y

  .method /*06000123*/ public hidebysig instance bool 
          Contains(valuetype System.Drawing.PointF/*0200000B*/ pt) cil managed
  // SIG: 20 01 02 11 2C
  {
    .custom /*0C00007B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::Contains

  .method /*06000124*/ public hidebysig instance bool 
          Contains(valuetype System.Drawing.RectangleF/*0200000D*/ rect) cil managed
  // SIG: 20 01 02 11 34
  {
    .custom /*0C00007C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::Contains

  .method /*06000125*/ public hidebysig instance bool 
          Contains(float32 x,
                   float32 y) cil managed
  // SIG: 20 02 02 0C 0C
  {
    .custom /*0C00007D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::Contains

  .method /*06000126*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Drawing.RectangleF/*0200000D*/ other) cil managed
  // SIG: 20 01 02 11 34
  {
    .custom /*0C00007E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::Equals

  .method /*06000127*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00007F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C000080:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000090*/ 
    .custom /*0C000033:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001B*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::Equals

  .method /*06000128*/ public hidebysig static 
          valuetype System.Drawing.RectangleF/*0200000D*/ 
          FromLTRB(float32 left,
                   float32 top,
                   float32 right,
                   float32 bottom) cil managed
  // SIG: 00 04 11 34 0C 0C 0C 0C
  {
    // Method begins at RVA 0x23f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::FromLTRB

  .method /*06000129*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000081:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::GetHashCode

  .method /*0600012A*/ public hidebysig static 
          valuetype System.Drawing.RectangleF/*0200000D*/ 
          Inflate(valuetype System.Drawing.RectangleF/*0200000D*/ rect,
                  float32 x,
                  float32 y) cil managed
  // SIG: 00 03 11 34 11 34 0C 0C
  {
    // Method begins at RVA 0x23fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::Inflate

  .method /*0600012B*/ public hidebysig instance void 
          Inflate(valuetype System.Drawing.SizeF/*0200000F*/ size) cil managed
  // SIG: 20 01 01 11 3C
  {
    // Method begins at RVA 0x2401
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RectangleF::Inflate

  .method /*0600012C*/ public hidebysig instance void 
          Inflate(float32 x,
                  float32 y) cil managed
  // SIG: 20 02 01 0C 0C
  {
    // Method begins at RVA 0x2403
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RectangleF::Inflate

  .method /*0600012D*/ public hidebysig instance void 
          Intersect(valuetype System.Drawing.RectangleF/*0200000D*/ rect) cil managed
  // SIG: 20 01 01 11 34
  {
    // Method begins at RVA 0x2405
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RectangleF::Intersect

  .method /*0600012E*/ public hidebysig static 
          valuetype System.Drawing.RectangleF/*0200000D*/ 
          Intersect(valuetype System.Drawing.RectangleF/*0200000D*/ a,
                    valuetype System.Drawing.RectangleF/*0200000D*/ b) cil managed
  // SIG: 00 02 11 34 11 34 11 34
  {
    // Method begins at RVA 0x2407
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::Intersect

  .method /*0600012F*/ public hidebysig instance bool 
          IntersectsWith(valuetype System.Drawing.RectangleF/*0200000D*/ rect) cil managed
  // SIG: 20 01 02 11 34
  {
    .custom /*0C000082:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x240a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::IntersectsWith

  .method /*06000130*/ public hidebysig instance void 
          Offset(valuetype System.Drawing.PointF/*0200000B*/ pos) cil managed
  // SIG: 20 01 01 11 2C
  {
    // Method begins at RVA 0x240d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RectangleF::Offset

  .method /*06000131*/ public hidebysig instance void 
          Offset(float32 x,
                 float32 y) cil managed
  // SIG: 20 02 01 0C 0C
  {
    // Method begins at RVA 0x240f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RectangleF::Offset

  .method /*06000132*/ public hidebysig specialname static 
          valuetype [System.Numerics.Vectors/*23000004*/]System.Numerics.Vector4/*0100001F*/ 
          op_Explicit(valuetype System.Drawing.RectangleF/*0200000D*/ rectangle) cil managed
  // SIG: 00 01 11 7D 11 34
  {
    // Method begins at RVA 0x2411
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::op_Explicit

  .method /*06000133*/ public hidebysig specialname static 
          valuetype System.Drawing.RectangleF/*0200000D*/ 
          op_Explicit(valuetype [System.Numerics.Vectors/*23000004*/]System.Numerics.Vector4/*0100001F*/ 'vector') cil managed
  // SIG: 00 01 11 34 11 7D
  {
    // Method begins at RVA 0x2414
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::op_Explicit

  .method /*06000134*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Drawing.RectangleF/*0200000D*/ left,
                            valuetype System.Drawing.RectangleF/*0200000D*/ right) cil managed
  // SIG: 00 02 02 11 34 11 34
  {
    // Method begins at RVA 0x2417
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::op_Equality

  .method /*06000135*/ public hidebysig specialname static 
          valuetype System.Drawing.RectangleF/*0200000D*/ 
          op_Implicit(valuetype System.Drawing.Rectangle/*0200000C*/ r) cil managed
  // SIG: 00 01 11 34 11 30
  {
    // Method begins at RVA 0x241a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::op_Implicit

  .method /*06000136*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Drawing.RectangleF/*0200000D*/ left,
                              valuetype System.Drawing.RectangleF/*0200000D*/ right) cil managed
  // SIG: 00 02 02 11 34 11 34
  {
    // Method begins at RVA 0x241d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::op_Inequality

  .method /*06000137*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000083:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C000084:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2420
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::ToString

  .method /*06000138*/ public hidebysig instance valuetype [System.Numerics.Vectors/*23000004*/]System.Numerics.Vector4/*0100001F*/ 
          ToVector4() cil managed
  // SIG: 20 00 11 7D
  {
    // Method begins at RVA 0x2423
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::ToVector4

  .method /*06000139*/ public hidebysig static 
          valuetype System.Drawing.RectangleF/*0200000D*/ 
          Union(valuetype System.Drawing.RectangleF/*0200000D*/ a,
                valuetype System.Drawing.RectangleF/*0200000D*/ b) cil managed
  // SIG: 00 02 11 34 11 34 11 34
  {
    // Method begins at RVA 0x2426
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RectangleF::Union

  .property /*170000A9*/ instance callconv(8) float32
          Bottom()
  {
    .custom /*0C00003E:0A000016*/ instance void [System.ComponentModel.Primitives/*23000002*/]System.ComponentModel.BrowsableAttribute/*0100001D*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance float32 System.Drawing.RectangleF/*0200000D*/::get_Bottom() /* 06000112 */
  } // end of property RectangleF::Bottom
  .property /*170000AA*/ instance callconv(8) float32
          Height()
  {
    .get instance float32 System.Drawing.RectangleF/*0200000D*/::get_Height() /* 06000113 */
    .set instance void System.Drawing.RectangleF/*0200000D*/::set_Height(float32) /* 06000114 */
  } // end of property RectangleF::Height
  .property /*170000AB*/ instance callconv(8) bool
          IsEmpty()
  {
    .custom /*0C00003F:0A000016*/ instance void [System.ComponentModel.Primitives/*23000002*/]System.ComponentModel.BrowsableAttribute/*0100001D*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance bool System.Drawing.RectangleF/*0200000D*/::get_IsEmpty() /* 06000115 */
  } // end of property RectangleF::IsEmpty
  .property /*170000AC*/ instance callconv(8) float32
          Left()
  {
    .custom /*0C000040:0A000016*/ instance void [System.ComponentModel.Primitives/*23000002*/]System.ComponentModel.BrowsableAttribute/*0100001D*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance float32 System.Drawing.RectangleF/*0200000D*/::get_Left() /* 06000116 */
  } // end of property RectangleF::Left
  .property /*170000AD*/ instance callconv(8) valuetype System.Drawing.PointF/*0200000B*/
          Location()
  {
    .custom /*0C000041:0A000016*/ instance void [System.ComponentModel.Primitives/*23000002*/]System.ComponentModel.BrowsableAttribute/*0100001D*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance valuetype System.Drawing.PointF/*0200000B*/ System.Drawing.RectangleF/*0200000D*/::get_Location() /* 06000117 */
    .set instance void System.Drawing.RectangleF/*0200000D*/::set_Location(valuetype System.Drawing.PointF/*0200000B*/) /* 06000118 */
  } // end of property RectangleF::Location
  .property /*170000AE*/ instance callconv(8) float32
          Right()
  {
    .custom /*0C000042:0A000016*/ instance void [System.ComponentModel.Primitives/*23000002*/]System.ComponentModel.BrowsableAttribute/*0100001D*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance float32 System.Drawing.RectangleF/*0200000D*/::get_Right() /* 06000119 */
  } // end of property RectangleF::Right
  .property /*170000AF*/ instance callconv(8) valuetype System.Drawing.SizeF/*0200000F*/
          Size()
  {
    .custom /*0C000043:0A000016*/ instance void [System.ComponentModel.Primitives/*23000002*/]System.ComponentModel.BrowsableAttribute/*0100001D*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance valuetype System.Drawing.SizeF/*0200000F*/ System.Drawing.RectangleF/*0200000D*/::get_Size() /* 0600011A */
    .set instance void System.Drawing.RectangleF/*0200000D*/::set_Size(valuetype System.Drawing.SizeF/*0200000F*/) /* 0600011B */
  } // end of property RectangleF::Size
  .property /*170000B0*/ instance callconv(8) float32
          Top()
  {
    .custom /*0C000044:0A000016*/ instance void [System.ComponentModel.Primitives/*23000002*/]System.ComponentModel.BrowsableAttribute/*0100001D*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance float32 System.Drawing.RectangleF/*0200000D*/::get_Top() /* 0600011C */
  } // end of property RectangleF::Top
  .property /*170000B1*/ instance callconv(8) float32
          Width()
  {
    .get instance float32 System.Drawing.RectangleF/*0200000D*/::get_Width() /* 0600011D */
    .set instance void System.Drawing.RectangleF/*0200000D*/::set_Width(float32) /* 0600011E */
  } // end of property RectangleF::Width
  .property /*170000B2*/ instance callconv(8) float32
          X()
  {
    .get instance float32 System.Drawing.RectangleF/*0200000D*/::get_X() /* 0600011F */
    .set instance void System.Drawing.RectangleF/*0200000D*/::set_X(float32) /* 06000120 */
  } // end of property RectangleF::X
  .property /*170000B3*/ instance callconv(8) float32
          Y()
  {
    .get instance float32 System.Drawing.RectangleF/*0200000D*/::get_Y() /* 06000121 */
    .set instance void System.Drawing.RectangleF/*0200000D*/::set_Y(float32) /* 06000122 */
  } // end of property RectangleF::Y
} // end of class System.Drawing.RectangleF

.class /*0200000E*/ public sequential ansi sealed beforefieldinit System.Drawing.Size
       extends [System.Runtime/*23000001*/]System.ValueType/*01000018*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000019*/<valuetype System.Drawing.Size/*0200000E*/>/*1B000006*/
{
  .custom /*0C00002E:0A000014*/ instance void [System.ObjectModel/*23000003*/]System.ComponentModel.TypeConverterAttribute/*01000017*/::.ctor(string) /* 0A000014 */ = ( 01 00 6F 53 79 73 74 65 6D 2E 44 72 61 77 69 6E   // ..oSystem.Drawin
                                                                                                                                                                         67 2E 53 69 7A 65 43 6F 6E 76 65 72 74 65 72 2C   // g.SizeConverter,
                                                                                                                                                                         20 53 79 73 74 65 6D 2E 44 72 61 77 69 6E 67 2C   //  System.Drawing,
                                                                                                                                                                         20 56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30   //  Version=4.0.0.0
                                                                                                                                                                         2C 20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61   // , Culture=neutra
                                                                                                                                                                         6C 2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65   // l, PublicKeyToke
                                                                                                                                                                         6E 3D 62 30 33 66 35 66 37 66 31 31 64 35 30 61   // n=b03f5f7f11d50a
                                                                                                                                                                         33 61 00 00 )                                     // 3a..
  .field /*040000C0*/ private int32 _dummyPrimitive
  .field /*040000C1*/ public static initonly valuetype System.Drawing.Size/*0200000E*/ Empty
  .method /*0600013A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Drawing.Point/*0200000A*/ pt) cil managed
  // SIG: 20 01 01 11 28
  {
    // Method begins at RVA 0x2429
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::.ctor

  .method /*0600013B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 width,
                               int32 height) cil managed
  // SIG: 20 02 01 08 08
  {
    // Method begins at RVA 0x242c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::.ctor

  .method /*0600013C*/ public hidebysig specialname 
          instance int32  get_Height() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000085:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x242f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::get_Height

  .method /*0600013D*/ public hidebysig specialname 
          instance void  set_Height(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2432
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Size::set_Height

  .method /*0600013E*/ public hidebysig specialname 
          instance bool  get_IsEmpty() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000086:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2434
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::get_IsEmpty

  .method /*0600013F*/ public hidebysig specialname 
          instance int32  get_Width() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000087:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2437
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::get_Width

  .method /*06000140*/ public hidebysig specialname 
          instance void  set_Width(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x243a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Size::set_Width

  .method /*06000141*/ public hidebysig static 
          valuetype System.Drawing.Size/*0200000E*/ 
          Add(valuetype System.Drawing.Size/*0200000E*/ sz1,
              valuetype System.Drawing.Size/*0200000E*/ sz2) cil managed
  // SIG: 00 02 11 38 11 38 11 38
  {
    // Method begins at RVA 0x243c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::Add

  .method /*06000142*/ public hidebysig static 
          valuetype System.Drawing.Size/*0200000E*/ 
          Ceiling(valuetype System.Drawing.SizeF/*0200000F*/ 'value') cil managed
  // SIG: 00 01 11 38 11 3C
  {
    // Method begins at RVA 0x243f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::Ceiling

  .method /*06000143*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Drawing.Size/*0200000E*/ other) cil managed
  // SIG: 20 01 02 11 38
  {
    .custom /*0C000088:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2442
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::Equals

  .method /*06000144*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000089:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C00008A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000B4*/ 
    .custom /*0C000045:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001B*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2445
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::Equals

  .method /*06000145*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00008B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2448
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::GetHashCode

  .method /*06000146*/ public hidebysig specialname static 
          valuetype System.Drawing.Size/*0200000E*/ 
          op_Addition(valuetype System.Drawing.Size/*0200000E*/ sz1,
                      valuetype System.Drawing.Size/*0200000E*/ sz2) cil managed
  // SIG: 00 02 11 38 11 38 11 38
  {
    // Method begins at RVA 0x244b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::op_Addition

  .method /*06000147*/ public hidebysig specialname static 
          valuetype System.Drawing.Size/*0200000E*/ 
          op_Division(valuetype System.Drawing.Size/*0200000E*/ left,
                      int32 right) cil managed
  // SIG: 00 02 11 38 11 38 08
  {
    // Method begins at RVA 0x244e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::op_Division

  .method /*06000148*/ public hidebysig specialname static 
          valuetype System.Drawing.SizeF/*0200000F*/ 
          op_Division(valuetype System.Drawing.Size/*0200000E*/ left,
                      float32 right) cil managed
  // SIG: 00 02 11 3C 11 38 0C
  {
    // Method begins at RVA 0x2451
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::op_Division

  .method /*06000149*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Drawing.Size/*0200000E*/ sz1,
                            valuetype System.Drawing.Size/*0200000E*/ sz2) cil managed
  // SIG: 00 02 02 11 38 11 38
  {
    // Method begins at RVA 0x2454
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::op_Equality

  .method /*0600014A*/ public hidebysig specialname static 
          valuetype System.Drawing.Point/*0200000A*/ 
          op_Explicit(valuetype System.Drawing.Size/*0200000E*/ size) cil managed
  // SIG: 00 01 11 28 11 38
  {
    // Method begins at RVA 0x2457
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::op_Explicit

  .method /*0600014B*/ public hidebysig specialname static 
          valuetype System.Drawing.SizeF/*0200000F*/ 
          op_Implicit(valuetype System.Drawing.Size/*0200000E*/ p) cil managed
  // SIG: 00 01 11 3C 11 38
  {
    // Method begins at RVA 0x245a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::op_Implicit

  .method /*0600014C*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Drawing.Size/*0200000E*/ sz1,
                              valuetype System.Drawing.Size/*0200000E*/ sz2) cil managed
  // SIG: 00 02 02 11 38 11 38
  {
    // Method begins at RVA 0x245d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::op_Inequality

  .method /*0600014D*/ public hidebysig specialname static 
          valuetype System.Drawing.Size/*0200000E*/ 
          op_Multiply(valuetype System.Drawing.Size/*0200000E*/ left,
                      int32 right) cil managed
  // SIG: 00 02 11 38 11 38 08
  {
    // Method begins at RVA 0x2460
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::op_Multiply

  .method /*0600014E*/ public hidebysig specialname static 
          valuetype System.Drawing.SizeF/*0200000F*/ 
          op_Multiply(valuetype System.Drawing.Size/*0200000E*/ left,
                      float32 right) cil managed
  // SIG: 00 02 11 3C 11 38 0C
  {
    // Method begins at RVA 0x2463
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::op_Multiply

  .method /*0600014F*/ public hidebysig specialname static 
          valuetype System.Drawing.Size/*0200000E*/ 
          op_Multiply(int32 left,
                      valuetype System.Drawing.Size/*0200000E*/ right) cil managed
  // SIG: 00 02 11 38 08 11 38
  {
    // Method begins at RVA 0x2466
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::op_Multiply

  .method /*06000150*/ public hidebysig specialname static 
          valuetype System.Drawing.SizeF/*0200000F*/ 
          op_Multiply(float32 left,
                      valuetype System.Drawing.Size/*0200000E*/ right) cil managed
  // SIG: 00 02 11 3C 0C 11 38
  {
    // Method begins at RVA 0x2469
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::op_Multiply

  .method /*06000151*/ public hidebysig specialname static 
          valuetype System.Drawing.Size/*0200000E*/ 
          op_Subtraction(valuetype System.Drawing.Size/*0200000E*/ sz1,
                         valuetype System.Drawing.Size/*0200000E*/ sz2) cil managed
  // SIG: 00 02 11 38 11 38 11 38
  {
    // Method begins at RVA 0x246c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::op_Subtraction

  .method /*06000152*/ public hidebysig static 
          valuetype System.Drawing.Size/*0200000E*/ 
          Round(valuetype System.Drawing.SizeF/*0200000F*/ 'value') cil managed
  // SIG: 00 01 11 38 11 3C
  {
    // Method begins at RVA 0x246f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::Round

  .method /*06000153*/ public hidebysig static 
          valuetype System.Drawing.Size/*0200000E*/ 
          Subtract(valuetype System.Drawing.Size/*0200000E*/ sz1,
                   valuetype System.Drawing.Size/*0200000E*/ sz2) cil managed
  // SIG: 00 02 11 38 11 38 11 38
  {
    // Method begins at RVA 0x2472
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::Subtract

  .method /*06000154*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00008C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C00008D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2475
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::ToString

  .method /*06000155*/ public hidebysig static 
          valuetype System.Drawing.Size/*0200000E*/ 
          Truncate(valuetype System.Drawing.SizeF/*0200000F*/ 'value') cil managed
  // SIG: 00 01 11 38 11 3C
  {
    // Method begins at RVA 0x2478
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Size::Truncate

  .property /*170000B4*/ instance callconv(8) int32
          Height()
  {
    .get instance int32 System.Drawing.Size/*0200000E*/::get_Height() /* 0600013C */
    .set instance void System.Drawing.Size/*0200000E*/::set_Height(int32) /* 0600013D */
  } // end of property Size::Height
  .property /*170000B5*/ instance callconv(8) bool
          IsEmpty()
  {
    .custom /*0C000046:0A000016*/ instance void [System.ComponentModel.Primitives/*23000002*/]System.ComponentModel.BrowsableAttribute/*0100001D*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance bool System.Drawing.Size/*0200000E*/::get_IsEmpty() /* 0600013E */
  } // end of property Size::IsEmpty
  .property /*170000B6*/ instance callconv(8) int32
          Width()
  {
    .get instance int32 System.Drawing.Size/*0200000E*/::get_Width() /* 0600013F */
    .set instance void System.Drawing.Size/*0200000E*/::set_Width(int32) /* 06000140 */
  } // end of property Size::Width
} // end of class System.Drawing.Size

.class /*0200000F*/ public sequential ansi sealed beforefieldinit System.Drawing.SizeF
       extends [System.Runtime/*23000001*/]System.ValueType/*01000018*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000019*/<valuetype System.Drawing.SizeF/*0200000F*/>/*1B000007*/
{
  .custom /*0C00002F:0A000014*/ instance void [System.ObjectModel/*23000003*/]System.ComponentModel.TypeConverterAttribute/*01000017*/::.ctor(string) /* 0A000014 */ = ( 01 00 70 53 79 73 74 65 6D 2E 44 72 61 77 69 6E   // ..pSystem.Drawin
                                                                                                                                                                         67 2E 53 69 7A 65 46 43 6F 6E 76 65 72 74 65 72   // g.SizeFConverter
                                                                                                                                                                         2C 20 53 79 73 74 65 6D 2E 44 72 61 77 69 6E 67   // , System.Drawing
                                                                                                                                                                         2C 20 56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E   // , Version=4.0.0.
                                                                                                                                                                         30 2C 20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72   // 0, Culture=neutr
                                                                                                                                                                         61 6C 2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B   // al, PublicKeyTok
                                                                                                                                                                         65 6E 3D 62 30 33 66 35 66 37 66 31 31 64 35 30   // en=b03f5f7f11d50
                                                                                                                                                                         61 33 61 00 00 )                                  // a3a..
  .field /*040000C2*/ private int32 _dummyPrimitive
  .field /*040000C3*/ public static initonly valuetype System.Drawing.SizeF/*0200000F*/ Empty
  .method /*06000156*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Drawing.PointF/*0200000B*/ pt) cil managed
  // SIG: 20 01 01 11 2C
  {
    // Method begins at RVA 0x247b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::.ctor

  .method /*06000157*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Drawing.SizeF/*0200000F*/ size) cil managed
  // SIG: 20 01 01 11 3C
  {
    // Method begins at RVA 0x247e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::.ctor

  .method /*06000158*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(float32 width,
                               float32 height) cil managed
  // SIG: 20 02 01 0C 0C
  {
    // Method begins at RVA 0x2481
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::.ctor

  .method /*06000159*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Numerics.Vectors/*23000004*/]System.Numerics.Vector2/*0100001E*/ 'vector') cil managed
  // SIG: 20 01 01 11 79
  {
    // Method begins at RVA 0x2484
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::.ctor

  .method /*0600015A*/ public hidebysig specialname 
          instance float32  get_Height() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C00008E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2487
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::get_Height

  .method /*0600015B*/ public hidebysig specialname 
          instance void  set_Height(float32 'value') cil managed
  // SIG: 20 01 01 0C
  {
    // Method begins at RVA 0x248a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SizeF::set_Height

  .method /*0600015C*/ public hidebysig specialname 
          instance bool  get_IsEmpty() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00008F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x248c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::get_IsEmpty

  .method /*0600015D*/ public hidebysig specialname 
          instance float32  get_Width() cil managed
  // SIG: 20 00 0C
  {
    .custom /*0C000090:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x248f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::get_Width

  .method /*0600015E*/ public hidebysig specialname 
          instance void  set_Width(float32 'value') cil managed
  // SIG: 20 01 01 0C
  {
    // Method begins at RVA 0x2492
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SizeF::set_Width

  .method /*0600015F*/ public hidebysig static 
          valuetype System.Drawing.SizeF/*0200000F*/ 
          Add(valuetype System.Drawing.SizeF/*0200000F*/ sz1,
              valuetype System.Drawing.SizeF/*0200000F*/ sz2) cil managed
  // SIG: 00 02 11 3C 11 3C 11 3C
  {
    // Method begins at RVA 0x2494
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::Add

  .method /*06000160*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Drawing.SizeF/*0200000F*/ other) cil managed
  // SIG: 20 01 02 11 3C
  {
    .custom /*0C000091:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2497
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::Equals

  .method /*06000161*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000092:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C000093:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000D9*/ 
    .custom /*0C000058:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001B*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x249a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::Equals

  .method /*06000162*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000094:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x249d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::GetHashCode

  .method /*06000163*/ public hidebysig specialname static 
          valuetype [System.Numerics.Vectors/*23000004*/]System.Numerics.Vector2/*0100001E*/ 
          op_Explicit(valuetype System.Drawing.SizeF/*0200000F*/ size) cil managed
  // SIG: 00 01 11 79 11 3C
  {
    // Method begins at RVA 0x24a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::op_Explicit

  .method /*06000164*/ public hidebysig specialname static 
          valuetype System.Drawing.SizeF/*0200000F*/ 
          op_Explicit(valuetype [System.Numerics.Vectors/*23000004*/]System.Numerics.Vector2/*0100001E*/ 'vector') cil managed
  // SIG: 00 01 11 3C 11 79
  {
    // Method begins at RVA 0x24a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::op_Explicit

  .method /*06000165*/ public hidebysig specialname static 
          valuetype System.Drawing.SizeF/*0200000F*/ 
          op_Addition(valuetype System.Drawing.SizeF/*0200000F*/ sz1,
                      valuetype System.Drawing.SizeF/*0200000F*/ sz2) cil managed
  // SIG: 00 02 11 3C 11 3C 11 3C
  {
    // Method begins at RVA 0x24a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::op_Addition

  .method /*06000166*/ public hidebysig specialname static 
          valuetype System.Drawing.SizeF/*0200000F*/ 
          op_Division(valuetype System.Drawing.SizeF/*0200000F*/ left,
                      float32 right) cil managed
  // SIG: 00 02 11 3C 11 3C 0C
  {
    // Method begins at RVA 0x24a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::op_Division

  .method /*06000167*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Drawing.SizeF/*0200000F*/ sz1,
                            valuetype System.Drawing.SizeF/*0200000F*/ sz2) cil managed
  // SIG: 00 02 02 11 3C 11 3C
  {
    // Method begins at RVA 0x24ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::op_Equality

  .method /*06000168*/ public hidebysig specialname static 
          valuetype System.Drawing.PointF/*0200000B*/ 
          op_Explicit(valuetype System.Drawing.SizeF/*0200000F*/ size) cil managed
  // SIG: 00 01 11 2C 11 3C
  {
    // Method begins at RVA 0x24af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::op_Explicit

  .method /*06000169*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Drawing.SizeF/*0200000F*/ sz1,
                              valuetype System.Drawing.SizeF/*0200000F*/ sz2) cil managed
  // SIG: 00 02 02 11 3C 11 3C
  {
    // Method begins at RVA 0x24b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::op_Inequality

  .method /*0600016A*/ public hidebysig specialname static 
          valuetype System.Drawing.SizeF/*0200000F*/ 
          op_Multiply(valuetype System.Drawing.SizeF/*0200000F*/ left,
                      float32 right) cil managed
  // SIG: 00 02 11 3C 11 3C 0C
  {
    // Method begins at RVA 0x24b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::op_Multiply

  .method /*0600016B*/ public hidebysig specialname static 
          valuetype System.Drawing.SizeF/*0200000F*/ 
          op_Multiply(float32 left,
                      valuetype System.Drawing.SizeF/*0200000F*/ right) cil managed
  // SIG: 00 02 11 3C 0C 11 3C
  {
    // Method begins at RVA 0x24b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::op_Multiply

  .method /*0600016C*/ public hidebysig specialname static 
          valuetype System.Drawing.SizeF/*0200000F*/ 
          op_Subtraction(valuetype System.Drawing.SizeF/*0200000F*/ sz1,
                         valuetype System.Drawing.SizeF/*0200000F*/ sz2) cil managed
  // SIG: 00 02 11 3C 11 3C 11 3C
  {
    // Method begins at RVA 0x24bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::op_Subtraction

  .method /*0600016D*/ public hidebysig static 
          valuetype System.Drawing.SizeF/*0200000F*/ 
          Subtract(valuetype System.Drawing.SizeF/*0200000F*/ sz1,
                   valuetype System.Drawing.SizeF/*0200000F*/ sz2) cil managed
  // SIG: 00 02 11 3C 11 3C 11 3C
  {
    // Method begins at RVA 0x24be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::Subtract

  .method /*0600016E*/ public hidebysig instance valuetype System.Drawing.PointF/*0200000B*/ 
          ToPointF() cil managed
  // SIG: 20 00 11 2C
  {
    .custom /*0C000095:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::ToPointF

  .method /*0600016F*/ public hidebysig instance valuetype System.Drawing.Size/*0200000E*/ 
          ToSize() cil managed
  // SIG: 20 00 11 38
  {
    .custom /*0C000096:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::ToSize

  .method /*06000170*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000097:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000015*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .custom /*0C000098:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x24c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::ToString

  .method /*06000171*/ public hidebysig instance valuetype [System.Numerics.Vectors/*23000004*/]System.Numerics.Vector2/*0100001E*/ 
          ToVector2() cil managed
  // SIG: 20 00 11 79
  {
    // Method begins at RVA 0x24ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SizeF::ToVector2

  .property /*170000B7*/ instance callconv(8) float32
          Height()
  {
    .get instance float32 System.Drawing.SizeF/*0200000F*/::get_Height() /* 0600015A */
    .set instance void System.Drawing.SizeF/*0200000F*/::set_Height(float32) /* 0600015B */
  } // end of property SizeF::Height
  .property /*170000B8*/ instance callconv(8) bool
          IsEmpty()
  {
    .custom /*0C000048:0A000016*/ instance void [System.ComponentModel.Primitives/*23000002*/]System.ComponentModel.BrowsableAttribute/*0100001D*/::.ctor(bool) /* 0A000016 */ = ( 01 00 00 00 00 ) 
    .get instance bool System.Drawing.SizeF/*0200000F*/::get_IsEmpty() /* 0600015C */
  } // end of property SizeF::IsEmpty
  .property /*170000B9*/ instance callconv(8) float32
          Width()
  {
    .get instance float32 System.Drawing.SizeF/*0200000F*/::get_Width() /* 0600015D */
    .set instance void System.Drawing.SizeF/*0200000F*/::set_Width(float32) /* 0600015E */
  } // end of property SizeF::Width
} // end of class System.Drawing.SizeF

.class /*02000010*/ public abstract auto ansi sealed beforefieldinit System.Drawing.SystemColors
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .method /*06000172*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_ActiveBorder() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x24cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_ActiveBorder

  .method /*06000173*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_ActiveCaption() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x24d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_ActiveCaption

  .method /*06000174*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_ActiveCaptionText() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x24d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_ActiveCaptionText

  .method /*06000175*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_AppWorkspace() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x24d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_AppWorkspace

  .method /*06000176*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_ButtonFace() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x24d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_ButtonFace

  .method /*06000177*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_ButtonHighlight() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x24dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_ButtonHighlight

  .method /*06000178*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_ButtonShadow() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x24df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_ButtonShadow

  .method /*06000179*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Control() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x24e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_Control

  .method /*0600017A*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_ControlDark() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x24e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_ControlDark

  .method /*0600017B*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_ControlDarkDark() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x24e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_ControlDarkDark

  .method /*0600017C*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_ControlLight() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x24eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_ControlLight

  .method /*0600017D*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_ControlLightLight() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x24ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_ControlLightLight

  .method /*0600017E*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_ControlText() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x24f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_ControlText

  .method /*0600017F*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Desktop() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x24f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_Desktop

  .method /*06000180*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_GradientActiveCaption() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x24f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_GradientActiveCaption

  .method /*06000181*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_GradientInactiveCaption() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x24fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_GradientInactiveCaption

  .method /*06000182*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_GrayText() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x24fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_GrayText

  .method /*06000183*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Highlight() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2500
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_Highlight

  .method /*06000184*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_HighlightText() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2503
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_HighlightText

  .method /*06000185*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_HotTrack() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2506
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_HotTrack

  .method /*06000186*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_InactiveBorder() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2509
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_InactiveBorder

  .method /*06000187*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_InactiveCaption() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x250c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_InactiveCaption

  .method /*06000188*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_InactiveCaptionText() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x250f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_InactiveCaptionText

  .method /*06000189*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Info() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2512
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_Info

  .method /*0600018A*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_InfoText() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2515
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_InfoText

  .method /*0600018B*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Menu() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2518
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_Menu

  .method /*0600018C*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_MenuBar() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x251b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_MenuBar

  .method /*0600018D*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_MenuHighlight() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x251e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_MenuHighlight

  .method /*0600018E*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_MenuText() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2521
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_MenuText

  .method /*0600018F*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_ScrollBar() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2524
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_ScrollBar

  .method /*06000190*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_Window() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x2527
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_Window

  .method /*06000191*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_WindowFrame() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x252a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_WindowFrame

  .method /*06000192*/ public hidebysig specialname static 
          valuetype System.Drawing.Color/*02000007*/ 
          get_WindowText() cil managed
  // SIG: 00 00 11 1C
  {
    // Method begins at RVA 0x252d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemColors::get_WindowText

  .property /*170000BA*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          ActiveBorder()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_ActiveBorder() /* 06000172 */
  } // end of property SystemColors::ActiveBorder
  .property /*170000BB*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          ActiveCaption()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_ActiveCaption() /* 06000173 */
  } // end of property SystemColors::ActiveCaption
  .property /*170000BC*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          ActiveCaptionText()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_ActiveCaptionText() /* 06000174 */
  } // end of property SystemColors::ActiveCaptionText
  .property /*170000BD*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          AppWorkspace()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_AppWorkspace() /* 06000175 */
  } // end of property SystemColors::AppWorkspace
  .property /*170000BE*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          ButtonFace()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_ButtonFace() /* 06000176 */
  } // end of property SystemColors::ButtonFace
  .property /*170000BF*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          ButtonHighlight()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_ButtonHighlight() /* 06000177 */
  } // end of property SystemColors::ButtonHighlight
  .property /*170000C0*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          ButtonShadow()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_ButtonShadow() /* 06000178 */
  } // end of property SystemColors::ButtonShadow
  .property /*170000C1*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Control()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_Control() /* 06000179 */
  } // end of property SystemColors::Control
  .property /*170000C2*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          ControlDark()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_ControlDark() /* 0600017A */
  } // end of property SystemColors::ControlDark
  .property /*170000C3*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          ControlDarkDark()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_ControlDarkDark() /* 0600017B */
  } // end of property SystemColors::ControlDarkDark
  .property /*170000C4*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          ControlLight()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_ControlLight() /* 0600017C */
  } // end of property SystemColors::ControlLight
  .property /*170000C5*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          ControlLightLight()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_ControlLightLight() /* 0600017D */
  } // end of property SystemColors::ControlLightLight
  .property /*170000C6*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          ControlText()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_ControlText() /* 0600017E */
  } // end of property SystemColors::ControlText
  .property /*170000C7*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Desktop()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_Desktop() /* 0600017F */
  } // end of property SystemColors::Desktop
  .property /*170000C8*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          GradientActiveCaption()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_GradientActiveCaption() /* 06000180 */
  } // end of property SystemColors::GradientActiveCaption
  .property /*170000C9*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          GradientInactiveCaption()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_GradientInactiveCaption() /* 06000181 */
  } // end of property SystemColors::GradientInactiveCaption
  .property /*170000CA*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          GrayText()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_GrayText() /* 06000182 */
  } // end of property SystemColors::GrayText
  .property /*170000CB*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Highlight()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_Highlight() /* 06000183 */
  } // end of property SystemColors::Highlight
  .property /*170000CC*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          HighlightText()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_HighlightText() /* 06000184 */
  } // end of property SystemColors::HighlightText
  .property /*170000CD*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          HotTrack()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_HotTrack() /* 06000185 */
  } // end of property SystemColors::HotTrack
  .property /*170000CE*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          InactiveBorder()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_InactiveBorder() /* 06000186 */
  } // end of property SystemColors::InactiveBorder
  .property /*170000CF*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          InactiveCaption()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_InactiveCaption() /* 06000187 */
  } // end of property SystemColors::InactiveCaption
  .property /*170000D0*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          InactiveCaptionText()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_InactiveCaptionText() /* 06000188 */
  } // end of property SystemColors::InactiveCaptionText
  .property /*170000D1*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Info()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_Info() /* 06000189 */
  } // end of property SystemColors::Info
  .property /*170000D2*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          InfoText()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_InfoText() /* 0600018A */
  } // end of property SystemColors::InfoText
  .property /*170000D3*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Menu()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_Menu() /* 0600018B */
  } // end of property SystemColors::Menu
  .property /*170000D4*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          MenuBar()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_MenuBar() /* 0600018C */
  } // end of property SystemColors::MenuBar
  .property /*170000D5*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          MenuHighlight()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_MenuHighlight() /* 0600018D */
  } // end of property SystemColors::MenuHighlight
  .property /*170000D6*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          MenuText()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_MenuText() /* 0600018E */
  } // end of property SystemColors::MenuText
  .property /*170000D7*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          ScrollBar()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_ScrollBar() /* 0600018F */
  } // end of property SystemColors::ScrollBar
  .property /*170000D8*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          Window()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_Window() /* 06000190 */
  } // end of property SystemColors::Window
  .property /*170000D9*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          WindowFrame()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_WindowFrame() /* 06000191 */
  } // end of property SystemColors::WindowFrame
  .property /*170000DA*/ callconv(8) valuetype System.Drawing.Color/*02000007*/
          WindowText()
  {
    .get valuetype System.Drawing.Color/*02000007*/ System.Drawing.SystemColors/*02000010*/::get_WindowText() /* 06000192 */
  } // end of property SystemColors::WindowText
} // end of class System.Drawing.SystemColors


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
