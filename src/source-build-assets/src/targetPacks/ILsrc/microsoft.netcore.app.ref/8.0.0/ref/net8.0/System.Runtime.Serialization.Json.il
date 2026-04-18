


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
// Time-date stamp:            0x90b07268
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
// Addr. of entry point:           0x00003e22
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
// Checksum:                       0x0001047e
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00003dd0 [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x000004e4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002a00 [0x00002910] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00003db4 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00001e28 Virtual Size
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
//              0x000004e4 Virtual Size
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
//              Entry 1: Type 0x3 Offset 0x00000e24
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00003e12 Import Name Table
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
// 0x00002134 [0x00001c00] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00003d34 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000cc0 Size
//              '#~' Name
//    Stream 2:
//              0x00000d2c Offset
//              0x00000918 Size
//              '#Strings' Name
//    Stream 3:
//              0x00001644 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00001648 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001658 Offset
//              0x000005a8 Size
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

// File size            : 21264
// PE header size       : 512 (496 used)    ( 2.41%)
// PE additional info   : 11963             (56.26%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.34%)
// CLR meta-data size  : 7168               (33.71%)
// CLR additional info : 128                ( 0.60%)
// CLR method headers  : 65                 ( 0.31%)
// Managed code         : 161               ( 0.76%)
// Data                 : 2048              ( 9.63%)
// Unaccounted          : -853              (-4.01%)

// Num.of PE sections   : 3
//   .text    - 8192
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 7168
//   Module        -    1 (10 bytes)
//   TypeDef       -    8 (112 bytes)     2 interfaces, 0 explicit layout
//   TypeRef       -   40 (240 bytes)
//   MethodDef     -   68 (952 bytes)     3 abstract, 0 native, 65 bodies
//   FieldDef      -    4 (24 bytes)      0 constant
//   MemberRef     -   24 (144 bytes)
//   ParamDef      -  105 (630 bytes)
//   Constant      -    3 (18 bytes)
//   CustomAttribute-  111 (666 bytes)
//   PropertyMap   -    3 (12 bytes)
//   Property      -   18 (108 bytes)
//   MethodSemantic-   27 (162 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    4 (80 bytes)
//   Strings       -  2325 bytes
//   Blobs         -  1448 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   195 bytes

// CLR additional info : 128

// CLR method headers : 65
//   Num.of method bodies  - 65
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 65

// Managed code : 161
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class DateTimeFormat                 (public) (auto) (ansi) 
// Class EmitTypeInformation            (public) (auto) (ansi) (sealed) 
// Class DataContractJsonSerializer     (public) (auto) (ansi) (sealed) 
// Class DataContractJsonSerializerSettings (public) (auto) (ansi) 
// Interface IXmlJsonReaderInitializer      (public) (abstract) (auto) (ansi) 
// Interface IXmlJsonWriterInitializer      (public) (abstract) (auto) (ansi) 
// Class JsonReaderWriterFactory        (public) (abstract) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly extern /*23000002*/ System.Runtime.Serialization.Xml
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly extern /*23000003*/ System.Runtime.Serialization.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly extern /*23000004*/ System.Xml.ReaderWriter
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly /*20000001*/ noplatform System.Runtime.Serialization.Json
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
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 38 2E 30 01 00 54 0E 14   // ersion=v8.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 38 2E 30 )          // Name..NET 8.0
  .custom /*0C000007:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 21 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D   // ..!System.Runtim
                                                                                                                                                                        65 2E 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 2E   // e.Serialization.
                                                                                                                                                                        4A 73 6F 6E 00 00 )                               // Json..
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000C:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000D:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000E:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 21 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D   // ..!System.Runtim
                                                                                                                                                                       65 2E 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 2E   // e.Serialization.
                                                                                                                                                                       4A 73 6F 6E 00 00 )                               // Json..
  .custom /*0C00000F:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 38 2E 30 2E 32 33 2E 35 33 31 30 33 00   // ...8.0.23.53103.
                                                                                                                                                                       00 ) 
  .custom /*0C000010:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 38 2E 30 2E 30 2B 35 35 33 35 65 33 31   // ...8.0.0+5535e31
                                                                                                                                                                                61 37 31 32 33 34 33 61 36 33 66 35 64 37 64 37   // a712343a63f5d7d7
                                                                                                                                                                                39 36 63 64 38 37 34 65 35 36 33 65 35 61 63 31   // 96cd874e563e5ac1
                                                                                                                                                                                34 00 00 )                                        // 4..
  .custom /*0C000011:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000012:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 21 53 79 73 74 65 6D 2E 52 75 6E 74 69 6D   // ..!System.Runtim
                                                                                                                                                                 65 2E 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 2E   // e.Serialization.
                                                                                                                                                                 4A 73 6F 6E 00 00 )                               // Json..
  .custom /*0C000013:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
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
  .ver 8:0:0:0
}
.module System.Runtime.Serialization.Json.dll
// MVID: {30138bed-74a4-49df-b9b1-b794a34416c8}
.custom /*0C000001:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000011*/::.ctor(int32) /* 0A000010 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000002:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000012*/::.ctor(bool) /* 0A000011 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi beforefieldinit System.Runtime.Serialization.DateTimeFormat
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000014:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000015:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string formatString) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DateTimeFormat::.ctor

  .method /*06000002*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string formatString,
                               class [System.Runtime/*23000001*/]System.IFormatProvider/*01000016*/ formatProvider) cil managed
  // SIG: 20 02 01 0E 12 59
  {
    // Method begins at RVA 0x2058
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DateTimeFormat::.ctor

  .method /*06000003*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Globalization.DateTimeStyles/*01000017*/ 
          get_DateTimeStyles() cil managed
  // SIG: 20 00 11 5D
  {
    // Method begins at RVA 0x2060
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DateTimeFormat::get_DateTimeStyles

  .method /*06000004*/ public hidebysig specialname 
          instance void  set_DateTimeStyles(valuetype [System.Runtime/*23000001*/]System.Globalization.DateTimeStyles/*01000017*/ 'value') cil managed
  // SIG: 20 01 01 11 5D
  {
    // Method begins at RVA 0x2063
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DateTimeFormat::set_DateTimeStyles

  .method /*06000005*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IFormatProvider/*01000016*/ 
          get_FormatProvider() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2065
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DateTimeFormat::get_FormatProvider

  .method /*06000006*/ public hidebysig specialname 
          instance string  get_FormatString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2068
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DateTimeFormat::get_FormatString

  .property /*17000001*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Globalization.DateTimeStyles/*01000017*/
          DateTimeStyles()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Globalization.DateTimeStyles/*01000017*/ System.Runtime.Serialization.DateTimeFormat/*02000002*/::get_DateTimeStyles() /* 06000003 */
    .set instance void System.Runtime.Serialization.DateTimeFormat/*02000002*/::set_DateTimeStyles(valuetype [System.Runtime/*23000001*/]System.Globalization.DateTimeStyles/*01000017*/) /* 06000004 */
  } // end of property DateTimeFormat::DateTimeStyles
  .property /*17000002*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IFormatProvider/*01000016*/
          FormatProvider()
  {
    .get instance class [System.Runtime/*23000001*/]System.IFormatProvider/*01000016*/ System.Runtime.Serialization.DateTimeFormat/*02000002*/::get_FormatProvider() /* 06000005 */
  } // end of property DateTimeFormat::FormatProvider
  .property /*17000003*/ instance callconv(8) string
          FormatString()
  {
    .get instance string System.Runtime.Serialization.DateTimeFormat/*02000002*/::get_FormatString() /* 06000006 */
  } // end of property DateTimeFormat::FormatString
} // end of class System.Runtime.Serialization.DateTimeFormat

.class /*02000003*/ public auto ansi sealed System.Runtime.Serialization.EmitTypeInformation
       extends [System.Runtime/*23000001*/]System.Enum/*01000018*/
{
  .field /*04000001*/ public specialname rtspecialname int32 value__
  .field /*04000002*/ public static literal valuetype System.Runtime.Serialization.EmitTypeInformation/*02000003*/ AsNeeded = int32(0x00000000)
  .field /*04000003*/ public static literal valuetype System.Runtime.Serialization.EmitTypeInformation/*02000003*/ Always = int32(0x00000001)
  .field /*04000004*/ public static literal valuetype System.Runtime.Serialization.EmitTypeInformation/*02000003*/ Never = int32(0x00000002)
} // end of class System.Runtime.Serialization.EmitTypeInformation

.class /*02000004*/ public auto ansi sealed beforefieldinit System.Runtime.Serialization.Json.DataContractJsonSerializer
       extends [System.Runtime.Serialization.Xml/*23000002*/]System.Runtime.Serialization.XmlObjectSerializer/*01000019*/
{
  .custom /*0C000016:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000017:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type') cil managed
  // SIG: 20 01 01 12 71
  {
    .custom /*0C00001C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C00001D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x206b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime.Serialization.Xml/*23000002*/]System.Runtime.Serialization.XmlObjectSerializer/*01000019*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializer::.ctor

  .method /*06000008*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class [System.Runtime/*23000001*/]System.Type/*0100001C*/> knownTypes) cil managed
  // SIG: 20 02 01 12 71 15 12 75 01 12 71
  {
    .custom /*0C000020:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000021:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*08000007*/ 
    .custom /*0C00001F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2073
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime.Serialization.Xml/*23000002*/]System.Runtime.Serialization.XmlObjectSerializer/*01000019*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializer::.ctor

  .method /*06000009*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                               class System.Runtime.Serialization.Json.DataContractJsonSerializerSettings/*02000005*/ settings) cil managed
  // SIG: 20 02 01 12 71 12 14
  {
    .custom /*0C000024:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000025:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*08000009*/ 
    .custom /*0C000026:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x207b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime.Serialization.Xml/*23000002*/]System.Runtime.Serialization.XmlObjectSerializer/*01000019*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializer::.ctor

  .method /*0600000A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                               string rootName) cil managed
  // SIG: 20 02 01 12 71 0E
  {
    .custom /*0C000027:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000028:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*0800000B*/ 
    .custom /*0C00002B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2083
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime.Serialization.Xml/*23000002*/]System.Runtime.Serialization.XmlObjectSerializer/*01000019*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializer::.ctor

  .method /*0600000B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                               string rootName,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class [System.Runtime/*23000001*/]System.Type/*0100001C*/> knownTypes) cil managed
  // SIG: 20 03 01 12 71 0E 15 12 75 01 12 71
  {
    .custom /*0C000029:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C00002A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*0800000D*/ 
    .custom /*0C000030:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800000E*/ 
    .custom /*0C000032:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x208b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime.Serialization.Xml/*23000002*/]System.Runtime.Serialization.XmlObjectSerializer/*01000019*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializer::.ctor

  .method /*0600000C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                               class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryString/*0100001E*/ rootName) cil managed
  // SIG: 20 02 01 12 71 12 79
  {
    .custom /*0C00002C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C00002D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*08000010*/ 
    .custom /*0C000034:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2093
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime.Serialization.Xml/*23000002*/]System.Runtime.Serialization.XmlObjectSerializer/*01000019*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializer::.ctor

  .method /*0600000D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                               class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryString/*0100001E*/ rootName,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class [System.Runtime/*23000001*/]System.Type/*0100001C*/> knownTypes) cil managed
  // SIG: 20 03 01 12 71 12 79 15 12 75 01 12 71
  {
    .custom /*0C00002E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C00002F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*08000012*/ 
    .custom /*0C000035:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000013*/ 
    .custom /*0C000036:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x209b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime.Serialization.Xml/*23000002*/]System.Runtime.Serialization.XmlObjectSerializer/*01000019*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializer::.ctor

  .method /*0600000E*/ public hidebysig specialname 
          instance class System.Runtime.Serialization.DateTimeFormat/*02000002*/ 
          get_DateTimeFormat() cil managed
  // SIG: 20 00 12 08
  {
    .custom /*0C000031:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializer::get_DateTimeFormat

  .method /*0600000F*/ public hidebysig specialname 
          instance valuetype System.Runtime.Serialization.EmitTypeInformation/*02000003*/ 
          get_EmitTypeInformation() cil managed
  // SIG: 20 00 11 0C
  {
    // Method begins at RVA 0x20a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializer::get_EmitTypeInformation

  .method /*06000010*/ public hidebysig specialname 
          instance bool  get_IgnoreExtensionDataObject() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializer::get_IgnoreExtensionDataObject

  .method /*06000011*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*0100001F*/<class [System.Runtime/*23000001*/]System.Type/*0100001C*/> 
          get_KnownTypes() cil managed
  // SIG: 20 00 15 12 7D 01 12 71
  {
    // Method begins at RVA 0x20ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializer::get_KnownTypes

  .method /*06000012*/ public hidebysig specialname 
          instance int32  get_MaxItemsInObjectGraph() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializer::get_MaxItemsInObjectGraph

  .method /*06000013*/ public hidebysig specialname 
          instance bool  get_SerializeReadOnlyTypes() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializer::get_SerializeReadOnlyTypes

  .method /*06000014*/ public hidebysig specialname 
          instance bool  get_UseSimpleDictionaryFormat() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializer::get_UseSimpleDictionaryFormat

  .method /*06000015*/ public hidebysig instance class [System.Runtime.Serialization.Primitives/*23000003*/]System.Runtime.Serialization.ISerializationSurrogateProvider/*01000020*/ 
          GetSerializationSurrogateProvider() cil managed
  // SIG: 20 00 12 80 81
  {
    .custom /*0C000037:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializer::GetSerializationSurrogateProvider

  .method /*06000016*/ public hidebysig instance void 
          SetSerializationSurrogateProvider(class [System.Runtime.Serialization.Primitives/*23000003*/]System.Runtime.Serialization.ISerializationSurrogateProvider/*01000020*/ provider) cil managed
  // SIG: 20 01 01 12 80 81
  {
    .custom /*0C000038:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20bb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializer::SetSerializationSurrogateProvider

  .method /*06000017*/ public hidebysig virtual 
          instance bool  IsStartObject(class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryReader/*01000021*/ reader) cil managed
  // SIG: 20 01 02 12 80 85
  {
    .custom /*0C000039:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C00003A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x20bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializer::IsStartObject

  .method /*06000018*/ public hidebysig virtual 
          instance bool  IsStartObject(class [System.Xml.ReaderWriter/*23000004*/]System.Xml.XmlReader/*01000022*/ reader) cil managed
  // SIG: 20 01 02 12 80 89
  {
    .custom /*0C00003C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C00003D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x20c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializer::IsStartObject

  .method /*06000019*/ public hidebysig virtual 
          instance object  ReadObject(class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 'stream') cil managed
  // SIG: 20 01 1C 12 80 8D
  {
    .custom /*0C00003E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C00003F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [0]/*08000017*/ 
    .custom /*0C00003B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializer::ReadObject

  .method /*0600001A*/ public hidebysig virtual 
          instance object  ReadObject(class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryReader/*01000021*/ reader) cil managed
  // SIG: 20 01 1C 12 80 85
  {
    .custom /*0C000041:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000042:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [0]/*08000019*/ 
    .custom /*0C000040:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializer::ReadObject

  .method /*0600001B*/ public hidebysig virtual 
          instance object  ReadObject(class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryReader/*01000021*/ reader,
                                      bool verifyObjectName) cil managed
  // SIG: 20 02 1C 12 80 85 02
  {
    .custom /*0C000043:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000044:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [0]/*0800001B*/ 
    .custom /*0C000045:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializer::ReadObject

  .method /*0600001C*/ public hidebysig virtual 
          instance object  ReadObject(class [System.Xml.ReaderWriter/*23000004*/]System.Xml.XmlReader/*01000022*/ reader) cil managed
  // SIG: 20 01 1C 12 80 89
  {
    .custom /*0C000046:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000047:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [0]/*0800001E*/ 
    .custom /*0C00004C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializer::ReadObject

  .method /*0600001D*/ public hidebysig virtual 
          instance object  ReadObject(class [System.Xml.ReaderWriter/*23000004*/]System.Xml.XmlReader/*01000022*/ reader,
                                      bool verifyObjectName) cil managed
  // SIG: 20 02 1C 12 80 89 02
  {
    .custom /*0C000048:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000049:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [0]/*08000020*/ 
    .custom /*0C000051:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializer::ReadObject

  .method /*0600001E*/ public hidebysig virtual 
          instance void  WriteEndObject(class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryWriter/*01000024*/ writer) cil managed
  // SIG: 20 01 01 12 80 91
  {
    .custom /*0C00004A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C00004B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x20d2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializer::WriteEndObject

  .method /*0600001F*/ public hidebysig virtual 
          instance void  WriteEndObject(class [System.Xml.ReaderWriter/*23000004*/]System.Xml.XmlWriter/*01000025*/ writer) cil managed
  // SIG: 20 01 01 12 80 95
  {
    .custom /*0C00004D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C00004E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    // Method begins at RVA 0x20d4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializer::WriteEndObject

  .method /*06000020*/ public hidebysig virtual 
          instance void  WriteObject(class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 'stream',
                                     object graph) cil managed
  // SIG: 20 02 01 12 80 8D 1C
  {
    .custom /*0C00004F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000050:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*08000026*/ 
    .custom /*0C00005E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializer::WriteObject

  .method /*06000021*/ public hidebysig virtual 
          instance void  WriteObject(class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryWriter/*01000024*/ writer,
                                     object graph) cil managed
  // SIG: 20 02 01 12 80 91 1C
  {
    .custom /*0C000052:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000053:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*08000028*/ 
    .custom /*0C00005F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializer::WriteObject

  .method /*06000022*/ public hidebysig virtual 
          instance void  WriteObject(class [System.Xml.ReaderWriter/*23000004*/]System.Xml.XmlWriter/*01000025*/ writer,
                                     object graph) cil managed
  // SIG: 20 02 01 12 80 95 1C
  {
    .custom /*0C000054:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000055:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*0800002A*/ 
    .custom /*0C000060:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20da
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializer::WriteObject

  .method /*06000023*/ public hidebysig virtual 
          instance void  WriteObjectContent(class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryWriter/*01000024*/ writer,
                                            object graph) cil managed
  // SIG: 20 02 01 12 80 91 1C
  {
    .custom /*0C000056:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000057:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*0800002C*/ 
    .custom /*0C000061:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20dc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializer::WriteObjectContent

  .method /*06000024*/ public hidebysig virtual 
          instance void  WriteObjectContent(class [System.Xml.ReaderWriter/*23000004*/]System.Xml.XmlWriter/*01000025*/ writer,
                                            object graph) cil managed
  // SIG: 20 02 01 12 80 95 1C
  {
    .custom /*0C000058:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C000059:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*0800002E*/ 
    .custom /*0C000062:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20de
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializer::WriteObjectContent

  .method /*06000025*/ public hidebysig virtual 
          instance void  WriteStartObject(class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryWriter/*01000024*/ writer,
                                          object graph) cil managed
  // SIG: 20 02 01 12 80 91 1C
  {
    .custom /*0C00005A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C00005B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*08000030*/ 
    .custom /*0C000063:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializer::WriteStartObject

  .method /*06000026*/ public hidebysig virtual 
          instance void  WriteStartObject(class [System.Xml.ReaderWriter/*23000004*/]System.Xml.XmlWriter/*01000025*/ writer,
                                          object graph) cil managed
  // SIG: 20 02 01 12 80 95 1C
  {
    .custom /*0C00005C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001A*/::.ctor(string) /* 0A000014 */ = ( 01 00 67 44 61 74 61 20 43 6F 6E 74 72 61 63 74   // ..gData Contract
                                                                                                                                                                                       20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20 61   //  Serialization a
                                                                                                                                                                                       6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74 69   // nd Deserializati
                                                                                                                                                                                       6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72 65   // on might require
                                                                                                                                                                                       20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E 6E   //  types that cann
                                                                                                                                                                                       6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79   // ot be statically
                                                                                                                                                                                       20 61 6E 61 6C 79 7A 65 64 2E 00 00 )             //  analyzed...
    .custom /*0C00005D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 80 9A 44 61 74 61 20 43 6F 6E 74 72 61 63   // ....Data Contrac
                                                                                                                                                                                            74 20 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E 20   // t Serialization 
                                                                                                                                                                                            61 6E 64 20 44 65 73 65 72 69 61 6C 69 7A 61 74   // and Deserializat
                                                                                                                                                                                            69 6F 6E 20 6D 69 67 68 74 20 72 65 71 75 69 72   // ion might requir
                                                                                                                                                                                            65 20 74 79 70 65 73 20 74 68 61 74 20 63 61 6E   // e types that can
                                                                                                                                                                                            6E 6F 74 20 62 65 20 73 74 61 74 69 63 61 6C 6C   // not be staticall
                                                                                                                                                                                            79 20 61 6E 61 6C 79 7A 65 64 2E 20 4D 61 6B 65   // y analyzed. Make
                                                                                                                                                                                            20 73 75 72 65 20 61 6C 6C 20 6F 66 20 74 68 65   //  sure all of the
                                                                                                                                                                                            20 72 65 71 75 69 72 65 64 20 74 79 70 65 73 20   //  required types 
                                                                                                                                                                                            61 72 65 20 70 72 65 73 65 72 76 65 64 2E 00 00 ) // are preserved...
    .param [2]/*08000032*/ 
    .custom /*0C000064:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializer::WriteStartObject

  .property /*17000004*/ instance callconv(8) class System.Runtime.Serialization.DateTimeFormat/*02000002*/
          DateTimeFormat()
  {
    .custom /*0C000018:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Runtime.Serialization.DateTimeFormat/*02000002*/ System.Runtime.Serialization.Json.DataContractJsonSerializer/*02000004*/::get_DateTimeFormat() /* 0600000E */
  } // end of property DataContractJsonSerializer::DateTimeFormat
  .property /*17000005*/ instance callconv(8) valuetype System.Runtime.Serialization.EmitTypeInformation/*02000003*/
          EmitTypeInformation()
  {
    .get instance valuetype System.Runtime.Serialization.EmitTypeInformation/*02000003*/ System.Runtime.Serialization.Json.DataContractJsonSerializer/*02000004*/::get_EmitTypeInformation() /* 0600000F */
  } // end of property DataContractJsonSerializer::EmitTypeInformation
  .property /*17000006*/ instance callconv(8) bool
          IgnoreExtensionDataObject()
  {
    .get instance bool System.Runtime.Serialization.Json.DataContractJsonSerializer/*02000004*/::get_IgnoreExtensionDataObject() /* 06000010 */
  } // end of property DataContractJsonSerializer::IgnoreExtensionDataObject
  .property /*17000007*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*0100001F*/<class [System.Runtime/*23000001*/]System.Type/*0100001C*/>
          KnownTypes()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*0100001F*/<class [System.Runtime/*23000001*/]System.Type/*0100001C*/> System.Runtime.Serialization.Json.DataContractJsonSerializer/*02000004*/::get_KnownTypes() /* 06000011 */
  } // end of property DataContractJsonSerializer::KnownTypes
  .property /*17000008*/ instance callconv(8) int32
          MaxItemsInObjectGraph()
  {
    .get instance int32 System.Runtime.Serialization.Json.DataContractJsonSerializer/*02000004*/::get_MaxItemsInObjectGraph() /* 06000012 */
  } // end of property DataContractJsonSerializer::MaxItemsInObjectGraph
  .property /*17000009*/ instance callconv(8) bool
          SerializeReadOnlyTypes()
  {
    .get instance bool System.Runtime.Serialization.Json.DataContractJsonSerializer/*02000004*/::get_SerializeReadOnlyTypes() /* 06000013 */
  } // end of property DataContractJsonSerializer::SerializeReadOnlyTypes
  .property /*1700000A*/ instance callconv(8) bool
          UseSimpleDictionaryFormat()
  {
    .get instance bool System.Runtime.Serialization.Json.DataContractJsonSerializer/*02000004*/::get_UseSimpleDictionaryFormat() /* 06000014 */
  } // end of property DataContractJsonSerializer::UseSimpleDictionaryFormat
} // end of class System.Runtime.Serialization.Json.DataContractJsonSerializer

.class /*02000005*/ public auto ansi beforefieldinit System.Runtime.Serialization.Json.DataContractJsonSerializerSettings
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000019:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00001A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000027*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20e4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializerSettings::.ctor

  .method /*06000028*/ public hidebysig specialname 
          instance class System.Runtime.Serialization.DateTimeFormat/*02000002*/ 
          get_DateTimeFormat() cil managed
  // SIG: 20 00 12 08
  {
    // Method begins at RVA 0x20ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializerSettings::get_DateTimeFormat

  .method /*06000029*/ public hidebysig specialname 
          instance void  set_DateTimeFormat(class System.Runtime.Serialization.DateTimeFormat/*02000002*/ 'value') cil managed
  // SIG: 20 01 01 12 08
  {
    // Method begins at RVA 0x20ef
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializerSettings::set_DateTimeFormat

  .method /*0600002A*/ public hidebysig specialname 
          instance valuetype System.Runtime.Serialization.EmitTypeInformation/*02000003*/ 
          get_EmitTypeInformation() cil managed
  // SIG: 20 00 11 0C
  {
    // Method begins at RVA 0x20f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializerSettings::get_EmitTypeInformation

  .method /*0600002B*/ public hidebysig specialname 
          instance void  set_EmitTypeInformation(valuetype System.Runtime.Serialization.EmitTypeInformation/*02000003*/ 'value') cil managed
  // SIG: 20 01 01 11 0C
  {
    // Method begins at RVA 0x20f4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializerSettings::set_EmitTypeInformation

  .method /*0600002C*/ public hidebysig specialname 
          instance bool  get_IgnoreExtensionDataObject() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializerSettings::get_IgnoreExtensionDataObject

  .method /*0600002D*/ public hidebysig specialname 
          instance void  set_IgnoreExtensionDataObject(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20f9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializerSettings::set_IgnoreExtensionDataObject

  .method /*0600002E*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class [System.Runtime/*23000001*/]System.Type/*0100001C*/> 
          get_KnownTypes() cil managed
  // SIG: 20 00 15 12 75 01 12 71
  {
    .param [0]/*08000036*/ 
    .custom /*0C000065:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializerSettings::get_KnownTypes

  .method /*0600002F*/ public hidebysig specialname 
          instance void  set_KnownTypes(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class [System.Runtime/*23000001*/]System.Type/*0100001C*/> 'value') cil managed
  // SIG: 20 01 01 15 12 75 01 12 71
  {
    .param [1]/*08000037*/ 
    .custom /*0C000066:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20fe
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializerSettings::set_KnownTypes

  .method /*06000030*/ public hidebysig specialname 
          instance int32  get_MaxItemsInObjectGraph() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2100
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializerSettings::get_MaxItemsInObjectGraph

  .method /*06000031*/ public hidebysig specialname 
          instance void  set_MaxItemsInObjectGraph(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2103
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializerSettings::set_MaxItemsInObjectGraph

  .method /*06000032*/ public hidebysig specialname 
          instance string  get_RootName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2105
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializerSettings::get_RootName

  .method /*06000033*/ public hidebysig specialname 
          instance void  set_RootName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2108
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializerSettings::set_RootName

  .method /*06000034*/ public hidebysig specialname 
          instance bool  get_SerializeReadOnlyTypes() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x210a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializerSettings::get_SerializeReadOnlyTypes

  .method /*06000035*/ public hidebysig specialname 
          instance void  set_SerializeReadOnlyTypes(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x210d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializerSettings::set_SerializeReadOnlyTypes

  .method /*06000036*/ public hidebysig specialname 
          instance bool  get_UseSimpleDictionaryFormat() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x210f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataContractJsonSerializerSettings::get_UseSimpleDictionaryFormat

  .method /*06000037*/ public hidebysig specialname 
          instance void  set_UseSimpleDictionaryFormat(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2112
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataContractJsonSerializerSettings::set_UseSimpleDictionaryFormat

  .property /*1700000B*/ instance callconv(8) class System.Runtime.Serialization.DateTimeFormat/*02000002*/
          DateTimeFormat()
  {
    .get instance class System.Runtime.Serialization.DateTimeFormat/*02000002*/ System.Runtime.Serialization.Json.DataContractJsonSerializerSettings/*02000005*/::get_DateTimeFormat() /* 06000028 */
    .set instance void System.Runtime.Serialization.Json.DataContractJsonSerializerSettings/*02000005*/::set_DateTimeFormat(class System.Runtime.Serialization.DateTimeFormat/*02000002*/) /* 06000029 */
  } // end of property DataContractJsonSerializerSettings::DateTimeFormat
  .property /*1700000C*/ instance callconv(8) valuetype System.Runtime.Serialization.EmitTypeInformation/*02000003*/
          EmitTypeInformation()
  {
    .get instance valuetype System.Runtime.Serialization.EmitTypeInformation/*02000003*/ System.Runtime.Serialization.Json.DataContractJsonSerializerSettings/*02000005*/::get_EmitTypeInformation() /* 0600002A */
    .set instance void System.Runtime.Serialization.Json.DataContractJsonSerializerSettings/*02000005*/::set_EmitTypeInformation(valuetype System.Runtime.Serialization.EmitTypeInformation/*02000003*/) /* 0600002B */
  } // end of property DataContractJsonSerializerSettings::EmitTypeInformation
  .property /*1700000D*/ instance callconv(8) bool
          IgnoreExtensionDataObject()
  {
    .get instance bool System.Runtime.Serialization.Json.DataContractJsonSerializerSettings/*02000005*/::get_IgnoreExtensionDataObject() /* 0600002C */
    .set instance void System.Runtime.Serialization.Json.DataContractJsonSerializerSettings/*02000005*/::set_IgnoreExtensionDataObject(bool) /* 0600002D */
  } // end of property DataContractJsonSerializerSettings::IgnoreExtensionDataObject
  .property /*1700000E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class [System.Runtime/*23000001*/]System.Type/*0100001C*/>
          KnownTypes()
  {
    .custom /*0C000033:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000016 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class [System.Runtime/*23000001*/]System.Type/*0100001C*/> System.Runtime.Serialization.Json.DataContractJsonSerializerSettings/*02000005*/::get_KnownTypes() /* 0600002E */
    .set instance void System.Runtime.Serialization.Json.DataContractJsonSerializerSettings/*02000005*/::set_KnownTypes(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<class [System.Runtime/*23000001*/]System.Type/*0100001C*/>) /* 0600002F */
  } // end of property DataContractJsonSerializerSettings::KnownTypes
  .property /*1700000F*/ instance callconv(8) int32
          MaxItemsInObjectGraph()
  {
    .get instance int32 System.Runtime.Serialization.Json.DataContractJsonSerializerSettings/*02000005*/::get_MaxItemsInObjectGraph() /* 06000030 */
    .set instance void System.Runtime.Serialization.Json.DataContractJsonSerializerSettings/*02000005*/::set_MaxItemsInObjectGraph(int32) /* 06000031 */
  } // end of property DataContractJsonSerializerSettings::MaxItemsInObjectGraph
  .property /*17000010*/ instance callconv(8) string
          RootName()
  {
    .get instance string System.Runtime.Serialization.Json.DataContractJsonSerializerSettings/*02000005*/::get_RootName() /* 06000032 */
    .set instance void System.Runtime.Serialization.Json.DataContractJsonSerializerSettings/*02000005*/::set_RootName(string) /* 06000033 */
  } // end of property DataContractJsonSerializerSettings::RootName
  .property /*17000011*/ instance callconv(8) bool
          SerializeReadOnlyTypes()
  {
    .get instance bool System.Runtime.Serialization.Json.DataContractJsonSerializerSettings/*02000005*/::get_SerializeReadOnlyTypes() /* 06000034 */
    .set instance void System.Runtime.Serialization.Json.DataContractJsonSerializerSettings/*02000005*/::set_SerializeReadOnlyTypes(bool) /* 06000035 */
  } // end of property DataContractJsonSerializerSettings::SerializeReadOnlyTypes
  .property /*17000012*/ instance callconv(8) bool
          UseSimpleDictionaryFormat()
  {
    .get instance bool System.Runtime.Serialization.Json.DataContractJsonSerializerSettings/*02000005*/::get_UseSimpleDictionaryFormat() /* 06000036 */
    .set instance void System.Runtime.Serialization.Json.DataContractJsonSerializerSettings/*02000005*/::set_UseSimpleDictionaryFormat(bool) /* 06000037 */
  } // end of property DataContractJsonSerializerSettings::UseSimpleDictionaryFormat
} // end of class System.Runtime.Serialization.Json.DataContractJsonSerializerSettings

.class /*02000006*/ interface public abstract auto ansi beforefieldinit System.Runtime.Serialization.Json.IXmlJsonReaderInitializer
{
  .custom /*0C00001B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .method /*06000038*/ public hidebysig newslot abstract virtual 
          instance void  SetInput(uint8[] buffer,
                                  int32 offset,
                                  int32 count,
                                  class [System.Runtime/*23000001*/]System.Text.Encoding/*01000026*/ encoding,
                                  class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryReaderQuotas/*01000027*/ quotas,
                                  class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.OnXmlDictionaryReaderClose/*01000028*/ onClose) cil managed
  // SIG: 20 06 01 1D 05 08 08 12 80 99 12 80 9D 12 80 A1
  {
    .param [4]/*0800003F*/ 
    .custom /*0C000067:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [6]/*08000041*/ 
    .custom /*0C000068:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IXmlJsonReaderInitializer::SetInput

  .method /*06000039*/ public hidebysig newslot abstract virtual 
          instance void  SetInput(class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 'stream',
                                  class [System.Runtime/*23000001*/]System.Text.Encoding/*01000026*/ encoding,
                                  class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryReaderQuotas/*01000027*/ quotas,
                                  class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.OnXmlDictionaryReaderClose/*01000028*/ onClose) cil managed
  // SIG: 20 04 01 12 80 8D 12 80 99 12 80 9D 12 80 A1
  {
    .param [2]/*08000043*/ 
    .custom /*0C000069:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000045*/ 
    .custom /*0C00006A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IXmlJsonReaderInitializer::SetInput

} // end of class System.Runtime.Serialization.Json.IXmlJsonReaderInitializer

.class /*02000007*/ interface public abstract auto ansi beforefieldinit System.Runtime.Serialization.Json.IXmlJsonWriterInitializer
{
  .custom /*0C00001E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .method /*0600003A*/ public hidebysig newslot abstract virtual 
          instance void  SetOutput(class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 'stream',
                                   class [System.Runtime/*23000001*/]System.Text.Encoding/*01000026*/ encoding,
                                   bool ownsStream) cil managed
  // SIG: 20 03 01 12 80 8D 12 80 99 02
  {
    // Method begins at RVA 0x0
  } // end of method IXmlJsonWriterInitializer::SetOutput

} // end of class System.Runtime.Serialization.Json.IXmlJsonWriterInitializer

.class /*02000008*/ public abstract auto ansi sealed beforefieldinit System.Runtime.Serialization.Json.JsonReaderWriterFactory
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000022:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000023:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*0600003B*/ public hidebysig static 
          class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryReader/*01000021*/ 
          CreateJsonReader(uint8[] buffer,
                           int32 offset,
                           int32 count,
                           class [System.Runtime/*23000001*/]System.Text.Encoding/*01000026*/ encoding,
                           class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryReaderQuotas/*01000027*/ quotas,
                           class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.OnXmlDictionaryReaderClose/*01000028*/ onClose) cil managed
  // SIG: 00 06 12 80 85 1D 05 08 08 12 80 99 12 80 9D 12 80 A1
  {
    .param [4]/*0800004C*/ 
    .custom /*0C00006B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [6]/*0800004E*/ 
    .custom /*0C00006C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2114
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonReaderWriterFactory::CreateJsonReader

  .method /*0600003C*/ public hidebysig static 
          class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryReader/*01000021*/ 
          CreateJsonReader(uint8[] buffer,
                           int32 offset,
                           int32 count,
                           class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryReaderQuotas/*01000027*/ quotas) cil managed
  // SIG: 00 04 12 80 85 1D 05 08 08 12 80 9D
  {
    // Method begins at RVA 0x2117
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonReaderWriterFactory::CreateJsonReader

  .method /*0600003D*/ public hidebysig static 
          class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryReader/*01000021*/ 
          CreateJsonReader(uint8[] buffer,
                           class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryReaderQuotas/*01000027*/ quotas) cil managed
  // SIG: 00 02 12 80 85 1D 05 12 80 9D
  {
    // Method begins at RVA 0x211a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonReaderWriterFactory::CreateJsonReader

  .method /*0600003E*/ public hidebysig static 
          class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryReader/*01000021*/ 
          CreateJsonReader(class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 'stream',
                           class [System.Runtime/*23000001*/]System.Text.Encoding/*01000026*/ encoding,
                           class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryReaderQuotas/*01000027*/ quotas,
                           class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.OnXmlDictionaryReaderClose/*01000028*/ onClose) cil managed
  // SIG: 00 04 12 80 85 12 80 8D 12 80 99 12 80 9D 12 80 A1
  {
    .param [2]/*08000056*/ 
    .custom /*0C00006D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000058*/ 
    .custom /*0C00006E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonReaderWriterFactory::CreateJsonReader

  .method /*0600003F*/ public hidebysig static 
          class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryReader/*01000021*/ 
          CreateJsonReader(class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 'stream',
                           class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryReaderQuotas/*01000027*/ quotas) cil managed
  // SIG: 00 02 12 80 85 12 80 8D 12 80 9D
  {
    // Method begins at RVA 0x2120
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonReaderWriterFactory::CreateJsonReader

  .method /*06000040*/ public hidebysig static 
          class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryWriter/*01000024*/ 
          CreateJsonWriter(class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 'stream') cil managed
  // SIG: 00 01 12 80 91 12 80 8D
  {
    // Method begins at RVA 0x2123
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonReaderWriterFactory::CreateJsonWriter

  .method /*06000041*/ public hidebysig static 
          class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryWriter/*01000024*/ 
          CreateJsonWriter(class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 'stream',
                           class [System.Runtime/*23000001*/]System.Text.Encoding/*01000026*/ encoding) cil managed
  // SIG: 00 02 12 80 91 12 80 8D 12 80 99
  {
    // Method begins at RVA 0x2126
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonReaderWriterFactory::CreateJsonWriter

  .method /*06000042*/ public hidebysig static 
          class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryWriter/*01000024*/ 
          CreateJsonWriter(class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 'stream',
                           class [System.Runtime/*23000001*/]System.Text.Encoding/*01000026*/ encoding,
                           bool ownsStream) cil managed
  // SIG: 00 03 12 80 91 12 80 8D 12 80 99 02
  {
    // Method begins at RVA 0x2129
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonReaderWriterFactory::CreateJsonWriter

  .method /*06000043*/ public hidebysig static 
          class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryWriter/*01000024*/ 
          CreateJsonWriter(class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 'stream',
                           class [System.Runtime/*23000001*/]System.Text.Encoding/*01000026*/ encoding,
                           bool ownsStream,
                           bool indent) cil managed
  // SIG: 00 04 12 80 91 12 80 8D 12 80 99 02 02
  {
    // Method begins at RVA 0x212c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonReaderWriterFactory::CreateJsonWriter

  .method /*06000044*/ public hidebysig static 
          class [System.Runtime.Serialization.Xml/*23000002*/]System.Xml.XmlDictionaryWriter/*01000024*/ 
          CreateJsonWriter(class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 'stream',
                           class [System.Runtime/*23000001*/]System.Text.Encoding/*01000026*/ encoding,
                           bool ownsStream,
                           bool indent,
                           string indentChars) cil managed
  // SIG: 00 05 12 80 91 12 80 8D 12 80 99 02 02 0E
  {
    .param [5]/*08000069*/ 
    .custom /*0C00006F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x212f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonReaderWriterFactory::CreateJsonWriter

} // end of class System.Runtime.Serialization.Json.JsonReaderWriterFactory


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
