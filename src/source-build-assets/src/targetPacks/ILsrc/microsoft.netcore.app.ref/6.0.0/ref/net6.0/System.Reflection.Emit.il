


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
// Time-date stamp:            0xe2c96dfd
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00006800
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x000087e6
// Base of code:                   0x00002000
// Base of data:                   0x0000a000
// Image base:                     0x10000000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0004
// Minor subsystem version:        0x0000
// Size of image:                  0x0000e000
// Size of headers:                0x00000200
// Checksum:                       0x0000c3d8
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00008791 [0x0000004f] address [size] of Import Directory:          
// 0x0000a000 [0x00000494] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00007200 [0x00002480] address [size] of Security Directory:        
// 0x0000c000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x000086a8 [0x00000054] address [size] of Debug Directory:           
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
//              0x000067ec Virtual Size
//              0x00002000 Virtual Address
//              0x00006800 Size of Raw Data
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
//              0x0000a000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00006a00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x0000c000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x00007000 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00008000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x000007e8
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x000087d3 Import Name Table
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
// 0x000025e4 [0x00006044] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00008628 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00003b30 Size
//              '#~' Name
//    Stream 2:
//              0x00003b9c Offset
//              0x00001818 Size
//              '#Strings' Name
//    Stream 3:
//              0x000053b4 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x000053b8 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x000053c8 Offset
//              0x00000c7c Size
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

// File size            : 38528
// PE header size       : 512 (496 used)    ( 1.33%)
// PE additional info   : 10771             (27.96%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.19%)
// CLR meta-data size  : 24644              (63.96%)
// CLR additional info : 128                ( 0.33%)
// CLR method headers  : 454                ( 1.18%)
// Managed code         : 973               ( 2.53%)
// Data                 : 2048              ( 5.32%)
// Unaccounted          : -1074             (-2.79%)

// Num.of PE sections   : 3
//   .text    - 26624
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 24644
//   Module        -    1 (10 bytes)
//   TypeDef       -   16 (224 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   79 (474 bytes)
//   MethodDef     -  454 (6356 bytes)    0 abstract, 0 native, 454 bodies
//   FieldDef      -    7 (42 bytes)      0 constant
//   MemberRef     -   33 (198 bytes)
//   ParamDef      -  597 (3582 bytes)
//   Constant      -    3 (18 bytes)
//   CustomAttribute-  413 (2478 bytes)
//   PropertyMap   -    9 (36 bytes)
//   Property      -  128 (768 bytes)
//   MethodSemantic-  130 (780 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    4 (80 bytes)
//   Strings       -  6167 bytes
//   Blobs         -  3196 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   193 bytes

// CLR additional info : 128

// CLR method headers : 454
//   Num.of method bodies  - 454
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 454

// Managed code : 973
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class AssemblyBuilder                (public) (auto) (ansi) (sealed) 
// Class AssemblyBuilderAccess          (public) (auto) (ansi) (sealed) 
// Class ConstructorBuilder             (public) (auto) (ansi) (sealed) 
// Class EnumBuilder                    (public) (auto) (ansi) (sealed) 
// Class EventBuilder                   (public) (auto) (ansi) (sealed) 
// Class FieldBuilder                   (public) (auto) (ansi) (sealed) 
// Class GenericTypeParameterBuilder    (public) (auto) (ansi) (sealed) 
// Class MethodBuilder                  (public) (auto) (ansi) (sealed) 
// Class ModuleBuilder                  (public) (auto) (ansi) 
// Class PropertyBuilder                (public) (auto) (ansi) (sealed) 
// Class TypeBuilder                    (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ System.Reflection.Emit.ILGeneration
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ System.Runtime.InteropServices
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ System.Reflection.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly /*20000001*/ noplatform System.Reflection.Emit
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
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C000008:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 16 53 79 73 74 65 6D 2E 52 65 66 6C 65 63   // ...System.Reflec
                                                                                                                                                                        74 69 6F 6E 2E 45 6D 69 74 00 00 )                // tion.Emit..
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000E:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000F:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000010:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000011:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 16 53 79 73 74 65 6D 2E 52 65 66 6C 65 63   // ...System.Reflec
                                                                                                                                                                       74 69 6F 6E 2E 45 6D 69 74 00 00 )                // tion.Emit..
  .custom /*0C000012:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 36 2E 30 2E 32 31 2E 35 32 32 31 30 00   // ...6.0.21.52210.
                                                                                                                                                                       00 ) 
  .custom /*0C000013:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 36 2E 30 2E 30 2B 34 38 32 32 65 33 63   // ...6.0.0+4822e3c
                                                                                                                                                                                33 61 61 37 37 65 62 38 32 62 32 66 62 33 33 63   // 3aa77eb82b2fb33c
                                                                                                                                                                                39 33 32 31 66 39 32 33 63 66 31 31 64 64 64 65   // 9321f923cf11ddde
                                                                                                                                                                                36 00 00 )                                        // 6..
  .custom /*0C000014:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000015:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 16 53 79 73 74 65 6D 2E 52 65 66 6C 65 63   // ...System.Reflec
                                                                                                                                                                 74 69 6F 6E 2E 45 6D 69 74 00 00 )                // tion.Emit..
  .custom /*0C000016:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
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
  .ver 6:0:0:0
}
.module System.Reflection.Emit.dll
// MVID: {7e3339e4-211c-46ae-8752-eeb66833c798}
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
  .custom /*0C000017:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000018:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000019 */
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
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000019 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)00004F       */ newarr     [System.Runtime/*23000001*/]System.Byte/*0100004F*/
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
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000019 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00001E:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000020:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000019 */
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
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000019 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ public auto ansi sealed beforefieldinit System.Reflection.Emit.AssemblyBuilder
       extends [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/
{
  .custom /*0C000025:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000026:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000006*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x209d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AssemblyBuilder::.ctor

  .method /*06000007*/ public hidebysig specialname virtual 
          instance string  get_CodeBase() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000027:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::get_CodeBase

  .method /*06000008*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          get_EntryPoint() cil managed
  // SIG: 20 00 12 59
  {
    .custom /*0C000029:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::get_EntryPoint

  .method /*06000009*/ public hidebysig specialname virtual 
          instance string  get_FullName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00002C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::get_FullName

  .method /*0600000A*/ public hidebysig specialname virtual 
          instance int64  get_HostContext() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x20ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::get_HostContext

  .method /*0600000B*/ public hidebysig specialname virtual 
          instance bool  get_IsCollectible() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::get_IsCollectible

  .method /*0600000C*/ public hidebysig specialname virtual 
          instance bool  get_IsDynamic() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::get_IsDynamic

  .method /*0600000D*/ public hidebysig specialname virtual 
          instance string  get_Location() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::get_Location

  .method /*0600000E*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ 
          get_ManifestModule() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x20ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::get_ManifestModule

  .method /*0600000F*/ public hidebysig specialname virtual 
          instance bool  get_ReflectionOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::get_ReflectionOnly

  .method /*06000010*/ public hidebysig static 
          class System.Reflection.Emit.AssemblyBuilder/*02000006*/ 
          DefineDynamicAssembly(class [System.Runtime/*23000001*/]System.Reflection.AssemblyName/*01000018*/ name,
                                valuetype System.Reflection.Emit.AssemblyBuilderAccess/*02000007*/ access) cil managed
  // SIG: 00 02 12 18 12 61 11 1C
  {
    // Method begins at RVA 0x20c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::DefineDynamicAssembly

  .method /*06000011*/ public hidebysig static 
          class System.Reflection.Emit.AssemblyBuilder/*02000006*/ 
          DefineDynamicAssembly(class [System.Runtime/*23000001*/]System.Reflection.AssemblyName/*01000018*/ name,
                                valuetype System.Reflection.Emit.AssemblyBuilderAccess/*02000007*/ access,
                                class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000019*/<class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001A*/> assemblyAttributes) cil managed
  // SIG: 00 03 12 18 12 61 11 1C 15 12 65 01 12 69
  {
    .param [3]/*08000005*/ 
    .custom /*0C000024:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::DefineDynamicAssembly

  .method /*06000012*/ public hidebysig instance class System.Reflection.Emit.ModuleBuilder/*0200000E*/ 
          DefineDynamicModule(string name) cil managed
  // SIG: 20 01 12 38 0E
  {
    // Method begins at RVA 0x20c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::DefineDynamicModule

  .method /*06000013*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000042:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::Equals

  .method /*06000014*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(bool inherit) cil managed
  // SIG: 20 01 1D 1C 02
  {
    // Method begins at RVA 0x20cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetCustomAttributes

  .method /*06000015*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ attributeType,
                                                 bool inherit) cil managed
  // SIG: 20 02 1D 1C 12 71 02
  {
    // Method begins at RVA 0x20cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetCustomAttributes

  .method /*06000016*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001D*/<class [System.Runtime/*23000001*/]System.Reflection.CustomAttributeData/*0100001E*/> 
          GetCustomAttributesData() cil managed
  // SIG: 20 00 15 12 75 01 12 79
  {
    // Method begins at RVA 0x20d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetCustomAttributesData

  .method /*06000017*/ public hidebysig instance class System.Reflection.Emit.ModuleBuilder/*0200000E*/ 
          GetDynamicModule(string name) cil managed
  // SIG: 20 01 12 38 0E
  {
    .param [0]/*0800000B*/ 
    .custom /*0C000033:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetDynamicModule

  .method /*06000018*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] 
          GetExportedTypes() cil managed
  // SIG: 20 00 1D 12 71
  {
    .custom /*0C000049:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 16 54 79 70 65 73 20 6D 69 67 68 74 20 62   // ...Types might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 00 00 )                // e removed..
    // Method begins at RVA 0x20d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetExportedTypes

  .method /*06000019*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.FileStream/*01000021*/ 
          GetFile(string name) cil managed
  // SIG: 20 01 12 80 85 0E
  {
    .custom /*0C00004A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresAssemblyFilesAttribute/*01000020*/::.ctor(string) /* 0A000013 */ = ( 01 00 4C 54 68 69 73 20 6D 65 6D 62 65 72 20 74   // ..LThis member t
                                                                                                                                                                                         68 72 6F 77 73 20 61 6E 20 65 78 63 65 70 74 69   // hrows an excepti
                                                                                                                                                                                         6F 6E 20 66 6F 72 20 61 73 73 65 6D 62 6C 69 65   // on for assemblie
                                                                                                                                                                                         73 20 65 6D 62 65 64 64 65 64 20 69 6E 20 61 20   // s embedded in a 
                                                                                                                                                                                         73 69 6E 67 6C 65 2D 66 69 6C 65 20 61 70 70 00   // single-file app.
                                                                                                                                                                                         00 ) 
    // Method begins at RVA 0x20db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetFile

  .method /*0600001A*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.FileStream/*01000021*/[] 
          GetFiles(bool getResourceModules) cil managed
  // SIG: 20 01 1D 12 80 85 02
  {
    .custom /*0C00004B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresAssemblyFilesAttribute/*01000020*/::.ctor(string) /* 0A000013 */ = ( 01 00 4C 54 68 69 73 20 6D 65 6D 62 65 72 20 74   // ..LThis member t
                                                                                                                                                                                         68 72 6F 77 73 20 61 6E 20 65 78 63 65 70 74 69   // hrows an excepti
                                                                                                                                                                                         6F 6E 20 66 6F 72 20 61 73 73 65 6D 62 6C 69 65   // on for assemblie
                                                                                                                                                                                         73 20 65 6D 62 65 64 64 65 64 20 69 6E 20 61 20   // s embedded in a 
                                                                                                                                                                                         73 69 6E 67 6C 65 2D 66 69 6C 65 20 61 70 70 00   // single-file app.
                                                                                                                                                                                         00 ) 
    // Method begins at RVA 0x20de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetFiles

  .method /*0600001B*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetHashCode

  .method /*0600001C*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/[] 
          GetLoadedModules(bool getResourceModules) cil managed
  // SIG: 20 01 1D 12 5D 02
  {
    // Method begins at RVA 0x20e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetLoadedModules

  .method /*0600001D*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ManifestResourceInfo/*01000022*/ 
          GetManifestResourceInfo(string resourceName) cil managed
  // SIG: 20 01 12 80 89 0E
  {
    .param [0]/*08000010*/ 
    .custom /*0C00003F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetManifestResourceInfo

  .method /*0600001E*/ public hidebysig virtual 
          instance string[]  GetManifestResourceNames() cil managed
  // SIG: 20 00 1D 0E
  {
    // Method begins at RVA 0x20ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetManifestResourceNames

  .method /*0600001F*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 
          GetManifestResourceStream(string name) cil managed
  // SIG: 20 01 12 80 8D 0E
  {
    .param [0]/*08000012*/ 
    .custom /*0C000040:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetManifestResourceStream

  .method /*06000020*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 
          GetManifestResourceStream(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                                    string name) cil managed
  // SIG: 20 02 12 80 8D 12 71 0E
  {
    .param [0]/*08000014*/ 
    .custom /*0C000043:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetManifestResourceStream

  .method /*06000021*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ 
          GetModule(string name) cil managed
  // SIG: 20 01 12 5D 0E
  {
    .param [0]/*08000017*/ 
    .custom /*0C000047:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetModule

  .method /*06000022*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/[] 
          GetModules(bool getResourceModules) cil managed
  // SIG: 20 01 1D 12 5D 02
  {
    // Method begins at RVA 0x20f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetModules

  .method /*06000023*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.AssemblyName/*01000018*/ 
          GetName(bool copiedName) cil managed
  // SIG: 20 01 12 61 02
  {
    // Method begins at RVA 0x20f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetName

  .method /*06000024*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.AssemblyName/*01000018*/[] 
          GetReferencedAssemblies() cil managed
  // SIG: 20 00 1D 12 61
  {
    .custom /*0C000050:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 24 41 73 73 65 6D 62 6C 79 20 72 65 66 65   // ..$Assembly refe
                                                                                                                                                                                            72 65 6E 63 65 73 20 6D 69 67 68 74 20 62 65 20   // rences might be 
                                                                                                                                                                                            72 65 6D 6F 76 65 64 00 00 )                      // removed..
    // Method begins at RVA 0x20fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetReferencedAssemblies

  .method /*06000025*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ 
          GetSatelliteAssembly(class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000024*/ culture) cil managed
  // SIG: 20 01 12 55 12 80 91
  {
    // Method begins at RVA 0x20ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetSatelliteAssembly

  .method /*06000026*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ 
          GetSatelliteAssembly(class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000024*/ culture,
                               class [System.Runtime/*23000001*/]System.Version/*01000025*/ version) cil managed
  // SIG: 20 02 12 55 12 80 91 12 80 95
  {
    .param [2]/*0800001D*/ 
    .custom /*0C00004C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2102
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetSatelliteAssembly

  .method /*06000027*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          GetType(string name,
                  bool throwOnError,
                  bool ignoreCase) cil managed
  // SIG: 20 03 12 71 0E 02 02
  {
    .custom /*0C000052:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 16 54 79 70 65 73 20 6D 69 67 68 74 20 62   // ...Types might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 00 00 )                // e removed..
    .param [0]/*0800001E*/ 
    .custom /*0C00004D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2105
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetType

  .method /*06000028*/ public hidebysig virtual 
          instance bool  IsDefined(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ attributeType,
                                   bool inherit) cil managed
  // SIG: 20 02 02 12 71 02
  {
    // Method begins at RVA 0x2108
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::IsDefined

  .method /*06000029*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000026*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 99 1D 05
  {
    // Method begins at RVA 0x210b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AssemblyBuilder::SetCustomAttribute

  .method /*0600002A*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001A*/ customBuilder) cil managed
  // SIG: 20 01 01 12 69
  {
    // Method begins at RVA 0x210d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AssemblyBuilder::SetCustomAttribute

  .property /*17000001*/ instance callconv(8) string
          CodeBase()
  {
    .custom /*0C000002:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000003:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000027*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 81 41 73 73 65 6D 62 6C 79 2E 43 6F 64   // ....Assembly.Cod
                                                                                                                                                   65 42 61 73 65 20 61 6E 64 20 41 73 73 65 6D 62   // eBase and Assemb
                                                                                                                                                   6C 79 2E 45 73 63 61 70 65 64 43 6F 64 65 42 61   // ly.EscapedCodeBa
                                                                                                                                                   73 65 20 61 72 65 20 6F 6E 6C 79 20 69 6E 63 6C   // se are only incl
                                                                                                                                                   75 64 65 64 20 66 6F 72 20 2E 4E 45 54 20 46 72   // uded for .NET Fr
                                                                                                                                                   61 6D 65 77 6F 72 6B 20 63 6F 6D 70 61 74 69 62   // amework compatib
                                                                                                                                                   69 6C 69 74 79 2E 20 55 73 65 20 41 73 73 65 6D   // ility. Use Assem
                                                                                                                                                   62 6C 79 2E 4C 6F 63 61 74 69 6F 6E 20 69 6E 73   // bly.Location ins
                                                                                                                                                   74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67 6E 6F   // tead...T..Diagno
                                                                                                                                                   73 74 69 63 49 64 0A 53 59 53 4C 49 42 30 30 31   // sticId.SYSLIB001
                                                                                                                                                   32 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22 68 74   // 2T..UrlFormat"ht
                                                                                                                                                   74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64 6F 74   // tps://aka.ms/dot
                                                                                                                                                   6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B 30 7D ) // net-warnings/{0}
    .get instance string System.Reflection.Emit.AssemblyBuilder/*02000006*/::get_CodeBase() /* 06000007 */
  } // end of property AssemblyBuilder::CodeBase
  .property /*17000002*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/
          EntryPoint()
  {
    .custom /*0C000019:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ System.Reflection.Emit.AssemblyBuilder/*02000006*/::get_EntryPoint() /* 06000008 */
  } // end of property AssemblyBuilder::EntryPoint
  .property /*17000003*/ instance callconv(8) string
          FullName()
  {
    .custom /*0C00001D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.AssemblyBuilder/*02000006*/::get_FullName() /* 06000009 */
  } // end of property AssemblyBuilder::FullName
  .property /*17000004*/ instance callconv(8) int64
          HostContext()
  {
    .get instance int64 System.Reflection.Emit.AssemblyBuilder/*02000006*/::get_HostContext() /* 0600000A */
  } // end of property AssemblyBuilder::HostContext
  .property /*17000005*/ instance callconv(8) bool
          IsCollectible()
  {
    .get instance bool System.Reflection.Emit.AssemblyBuilder/*02000006*/::get_IsCollectible() /* 0600000B */
  } // end of property AssemblyBuilder::IsCollectible
  .property /*17000006*/ instance callconv(8) bool
          IsDynamic()
  {
    .get instance bool System.Reflection.Emit.AssemblyBuilder/*02000006*/::get_IsDynamic() /* 0600000C */
  } // end of property AssemblyBuilder::IsDynamic
  .property /*17000007*/ instance callconv(8) string
          Location()
  {
    .get instance string System.Reflection.Emit.AssemblyBuilder/*02000006*/::get_Location() /* 0600000D */
  } // end of property AssemblyBuilder::Location
  .property /*17000008*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/
          ManifestModule()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ System.Reflection.Emit.AssemblyBuilder/*02000006*/::get_ManifestModule() /* 0600000E */
  } // end of property AssemblyBuilder::ManifestModule
  .property /*17000009*/ instance callconv(8) bool
          ReflectionOnly()
  {
    .get instance bool System.Reflection.Emit.AssemblyBuilder/*02000006*/::get_ReflectionOnly() /* 0600000F */
  } // end of property AssemblyBuilder::ReflectionOnly
} // end of class System.Reflection.Emit.AssemblyBuilder

.class /*02000007*/ public auto ansi sealed System.Reflection.Emit.AssemblyBuilderAccess
       extends [System.Runtime/*23000001*/]System.Enum/*01000029*/
{
  .custom /*0C000028:0A000015*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*01000028*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .field /*04000004*/ public specialname rtspecialname int32 value__
  .field /*04000005*/ public static literal valuetype System.Reflection.Emit.AssemblyBuilderAccess/*02000007*/ Run = int32(0x00000001)
  .field /*04000006*/ public static literal valuetype System.Reflection.Emit.AssemblyBuilderAccess/*02000007*/ RunAndCollect = int32(0x00000009)
} // end of class System.Reflection.Emit.AssemblyBuilderAccess

.class /*02000008*/ public auto ansi sealed beforefieldinit System.Reflection.Emit.ConstructorBuilder
       extends [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000026*/
{
  .custom /*0C00002A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600002B*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x210f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000026*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ConstructorBuilder::.ctor

  .method /*0600002C*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/ 
          get_Attributes() cil managed
  // SIG: 20 00 11 80 A9
  {
    // Method begins at RVA 0x2117
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::get_Attributes

  .method /*0600002D*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ 
          get_CallingConvention() cil managed
  // SIG: 20 00 11 80 AD
  {
    // Method begins at RVA 0x211a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::get_CallingConvention

  .method /*0600002E*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_DeclaringType() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C000056:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::get_DeclaringType

  .method /*0600002F*/ public hidebysig specialname 
          instance bool  get_InitLocals() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2120
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::get_InitLocals

  .method /*06000030*/ public hidebysig specialname 
          instance void  set_InitLocals(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2123
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConstructorBuilder::set_InitLocals

  .method /*06000031*/ public hidebysig specialname virtual 
          instance int32  get_MetadataToken() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2125
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::get_MetadataToken

  .method /*06000032*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.RuntimeMethodHandle/*0100002C*/ 
          get_MethodHandle() cil managed
  // SIG: 20 00 11 80 B1
  {
    // Method begins at RVA 0x2128
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::get_MethodHandle

  .method /*06000033*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ 
          get_Module() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x212b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::get_Module

  .method /*06000034*/ public hidebysig specialname virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x212e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::get_Name

  .method /*06000035*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_ReflectedType() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C00005C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2131
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::get_ReflectedType

  .method /*06000036*/ public hidebysig instance class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.ParameterBuilder/*0100002D*/ 
          DefineParameter(int32 iSequence,
                          valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterAttributes/*0100002E*/ attributes,
                          string strParamName) cil managed
  // SIG: 20 03 12 80 B5 08 11 80 B9 0E
  {
    .param [3]/*0800002A*/ 
    .custom /*0C000053:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2134
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::DefineParameter

  .method /*06000037*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(bool inherit) cil managed
  // SIG: 20 01 1D 1C 02
  {
    // Method begins at RVA 0x2137
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::GetCustomAttributes

  .method /*06000038*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ attributeType,
                                                 bool inherit) cil managed
  // SIG: 20 02 1D 1C 12 71 02
  {
    // Method begins at RVA 0x213a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::GetCustomAttributes

  .method /*06000039*/ public hidebysig instance class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.ILGenerator/*0100002F*/ 
          GetILGenerator() cil managed
  // SIG: 20 00 12 80 BD
  {
    // Method begins at RVA 0x213d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::GetILGenerator

  .method /*0600003A*/ public hidebysig instance class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.ILGenerator/*0100002F*/ 
          GetILGenerator(int32 streamSize) cil managed
  // SIG: 20 01 12 80 BD 08
  {
    // Method begins at RVA 0x2140
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::GetILGenerator

  .method /*0600003B*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.MethodImplAttributes/*01000030*/ 
          GetMethodImplementationFlags() cil managed
  // SIG: 20 00 11 80 C1
  {
    // Method begins at RVA 0x2143
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::GetMethodImplementationFlags

  .method /*0600003C*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*01000031*/[] 
          GetParameters() cil managed
  // SIG: 20 00 1D 12 80 C5
  {
    // Method begins at RVA 0x2146
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::GetParameters

  .method /*0600003D*/ public hidebysig virtual 
          instance object  Invoke(object obj,
                                  valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ invokeAttr,
                                  class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000033*/ binder,
                                  object[] parameters,
                                  class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000024*/ culture) cil managed
  // SIG: 20 05 1C 1C 11 80 C9 12 80 CD 1D 1C 12 80 91
  {
    .custom /*0C00005E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800002F*/ 
    .custom /*0C000058:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2149
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::Invoke

  .method /*0600003E*/ public hidebysig virtual 
          instance object  Invoke(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ invokeAttr,
                                  class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000033*/ binder,
                                  object[] parameters,
                                  class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000024*/ culture) cil managed
  // SIG: 20 04 1C 11 80 C9 12 80 CD 1D 1C 12 80 91
  {
    .custom /*0C00005F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000035*/ 
    .custom /*0C00005D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x214c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::Invoke

  .method /*0600003F*/ public hidebysig virtual 
          instance bool  IsDefined(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ attributeType,
                                   bool inherit) cil managed
  // SIG: 20 02 02 12 71 02
  {
    // Method begins at RVA 0x214f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::IsDefined

  .method /*06000040*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000026*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 99 1D 05
  {
    // Method begins at RVA 0x2152
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConstructorBuilder::SetCustomAttribute

  .method /*06000041*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001A*/ customBuilder) cil managed
  // SIG: 20 01 01 12 69
  {
    // Method begins at RVA 0x2154
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConstructorBuilder::SetCustomAttribute

  .method /*06000042*/ public hidebysig instance void 
          SetImplementationFlags(valuetype [System.Runtime/*23000001*/]System.Reflection.MethodImplAttributes/*01000030*/ attributes) cil managed
  // SIG: 20 01 01 11 80 C1
  {
    // Method begins at RVA 0x2156
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConstructorBuilder::SetImplementationFlags

  .method /*06000043*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2158
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::ToString

  .property /*1700000A*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/
          Attributes()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/ System.Reflection.Emit.ConstructorBuilder/*02000008*/::get_Attributes() /* 0600002C */
  } // end of property ConstructorBuilder::Attributes
  .property /*1700000B*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/
          CallingConvention()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ System.Reflection.Emit.ConstructorBuilder/*02000008*/::get_CallingConvention() /* 0600002D */
  } // end of property ConstructorBuilder::CallingConvention
  .property /*1700000C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          DeclaringType()
  {
    .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.ConstructorBuilder/*02000008*/::get_DeclaringType() /* 0600002E */
  } // end of property ConstructorBuilder::DeclaringType
  .property /*1700000D*/ instance callconv(8) bool
          InitLocals()
  {
    .get instance bool System.Reflection.Emit.ConstructorBuilder/*02000008*/::get_InitLocals() /* 0600002F */
    .set instance void System.Reflection.Emit.ConstructorBuilder/*02000008*/::set_InitLocals(bool) /* 06000030 */
  } // end of property ConstructorBuilder::InitLocals
  .property /*1700000E*/ instance callconv(8) int32
          MetadataToken()
  {
    .get instance int32 System.Reflection.Emit.ConstructorBuilder/*02000008*/::get_MetadataToken() /* 06000031 */
  } // end of property ConstructorBuilder::MetadataToken
  .property /*1700000F*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.RuntimeMethodHandle/*0100002C*/
          MethodHandle()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.RuntimeMethodHandle/*0100002C*/ System.Reflection.Emit.ConstructorBuilder/*02000008*/::get_MethodHandle() /* 06000032 */
  } // end of property ConstructorBuilder::MethodHandle
  .property /*17000010*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/
          Module()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ System.Reflection.Emit.ConstructorBuilder/*02000008*/::get_Module() /* 06000033 */
  } // end of property ConstructorBuilder::Module
  .property /*17000011*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Reflection.Emit.ConstructorBuilder/*02000008*/::get_Name() /* 06000034 */
  } // end of property ConstructorBuilder::Name
  .property /*17000012*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          ReflectedType()
  {
    .custom /*0C000041:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.ConstructorBuilder/*02000008*/::get_ReflectedType() /* 06000035 */
  } // end of property ConstructorBuilder::ReflectedType
} // end of class System.Reflection.Emit.ConstructorBuilder

.class /*02000009*/ public auto ansi sealed beforefieldinit System.Reflection.Emit.EnumBuilder
       extends [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000034*/
{
  .custom /*0C00002D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000044*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x215b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000034*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EnumBuilder::.ctor

  .method /*06000045*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ 
          get_Assembly() cil managed
  // SIG: 20 00 12 55
  {
    // Method begins at RVA 0x2163
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_Assembly

  .method /*06000046*/ public hidebysig specialname virtual 
          instance string  get_AssemblyQualifiedName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000065:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2166
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_AssemblyQualifiedName

  .method /*06000047*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_BaseType() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C000066:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2169
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_BaseType

  .method /*06000048*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_DeclaringType() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C000069:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x216c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_DeclaringType

  .method /*06000049*/ public hidebysig specialname virtual 
          instance string  get_FullName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00006A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x216f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_FullName

  .method /*0600004A*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/ 
          get_GUID() cil managed
  // SIG: 20 00 11 80 D5
  {
    // Method begins at RVA 0x2172
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_GUID

  .method /*0600004B*/ public hidebysig specialname virtual 
          instance bool  get_IsByRefLike() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2175
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_IsByRefLike

  .method /*0600004C*/ public hidebysig specialname virtual 
          instance bool  get_IsConstructedGenericType() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2178
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_IsConstructedGenericType

  .method /*0600004D*/ public hidebysig specialname virtual 
          instance bool  get_IsSZArray() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x217b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_IsSZArray

  .method /*0600004E*/ public hidebysig specialname virtual 
          instance bool  get_IsTypeDefinition() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x217e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_IsTypeDefinition

  .method /*0600004F*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ 
          get_Module() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x2181
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_Module

  .method /*06000050*/ public hidebysig specialname virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2184
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_Name

  .method /*06000051*/ public hidebysig specialname virtual 
          instance string  get_Namespace() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00006C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2187
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_Namespace

  .method /*06000052*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_ReflectedType() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C00006E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x218a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_ReflectedType

  .method /*06000053*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000036*/ 
          get_TypeHandle() cil managed
  // SIG: 20 00 11 80 D9
  {
    // Method begins at RVA 0x218d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_TypeHandle

  .method /*06000054*/ public hidebysig specialname 
          instance class System.Reflection.Emit.FieldBuilder/*0200000B*/ 
          get_UnderlyingField() cil managed
  // SIG: 20 00 12 2C
  {
    // Method begins at RVA 0x2190
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_UnderlyingField

  .method /*06000055*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_UnderlyingSystemType() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x2193
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_UnderlyingSystemType

  .method /*06000056*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          CreateType() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C000072:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000040*/ 
    .custom /*0C000060:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x2196
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::CreateType

  .method /*06000057*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000034*/ 
          CreateTypeInfo() cil managed
  // SIG: 20 00 12 80 D1
  {
    .custom /*0C000073:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000041*/ 
    .custom /*0C000062:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x2199
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::CreateTypeInfo

  .method /*06000058*/ public hidebysig instance class System.Reflection.Emit.FieldBuilder/*0200000B*/ 
          DefineLiteral(string literalName,
                        object literalValue) cil managed
  // SIG: 20 02 12 2C 0E 1C
  {
    .param [2]/*08000043*/ 
    .custom /*0C000064:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x219c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::DefineLiteral

  .method /*06000059*/ family hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*01000039*/ 
          GetAttributeFlagsImpl() cil managed
  // SIG: 20 00 11 80 E5
  {
    // Method begins at RVA 0x219f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetAttributeFlagsImpl

  .method /*0600005A*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000026*/ 
          GetConstructorImpl(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr,
                             class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000033*/ binder,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callConvention,
                             class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] types,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003A*/[] modifiers) cil managed
  // SIG: 20 05 12 80 99 11 80 C9 12 80 CD 11 80 AD 1D 12 71 1D 11 80 E9
  {
    .custom /*0C000075:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000076:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 07 00 00 00 00 00 ) 
    .param [4]/*08000047*/ 
    .custom /*0C000067:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetConstructorImpl

  .method /*0600005B*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000026*/[] 
          GetConstructors(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 99 11 80 C9
  {
    .custom /*0C000077:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 07 00 00 00 00 00 ) 
    // Method begins at RVA 0x21a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetConstructors

  .method /*0600005C*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(bool inherit) cil managed
  // SIG: 20 01 1D 1C 02
  {
    // Method begins at RVA 0x21a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetCustomAttributes

  .method /*0600005D*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ attributeType,
                                                 bool inherit) cil managed
  // SIG: 20 02 1D 1C 12 71 02
  {
    // Method begins at RVA 0x21ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetCustomAttributes

  .method /*0600005E*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          GetElementType() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C000079:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetElementType

  .method /*0600005F*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          GetEnumUnderlyingType() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x21b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetEnumUnderlyingType

  .method /*06000060*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100003B*/ 
          GetEvent(string name,
                   valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 02 12 80 ED 0E 11 80 C9
  {
    .custom /*0C00007A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 18 00 00 00 00 ) 
    .param [0]/*0800004D*/ 
    .custom /*0C00006B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetEvent

  .method /*06000061*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100003B*/[] 
          GetEvents() cil managed
  // SIG: 20 00 1D 12 80 ED
  {
    .custom /*0C00007C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 08 00 00 00 00 ) 
    // Method begins at RVA 0x21b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetEvents

  .method /*06000062*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100003B*/[] 
          GetEvents(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 ED 11 80 C9
  {
    .custom /*0C00007E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 18 00 00 00 00 ) 
    // Method begins at RVA 0x21ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetEvents

  .method /*06000063*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003C*/ 
          GetField(string name,
                   valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 02 12 80 F1 0E 11 80 C9
  {
    .custom /*0C00007F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 60 00 00 00 00 00 )                         // ..`.....
    .param [0]/*08000051*/ 
    .custom /*0C00006D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetField

  .method /*06000064*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003C*/[] 
          GetFields(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 F1 11 80 C9
  {
    .custom /*0C000080:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 60 00 00 00 00 00 )                         // ..`.....
    // Method begins at RVA 0x21c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetFields

  .method /*06000065*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          GetInterface(string name,
                       bool ignoreCase) cil managed
  // SIG: 20 02 12 71 0E 02
  {
    .custom /*0C000083:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 20 00 00 00 00 )                         // ... ....
    .param [0]/*08000055*/ 
    .custom /*0C000070:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000071:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 20 00 00 00 00 )                         // ... ....
    // Method begins at RVA 0x21c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetInterface

  .method /*06000066*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.InterfaceMapping/*0100003D*/ 
          GetInterfaceMap(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ interfaceType) cil managed
  // SIG: 20 01 11 80 F5 12 71
  {
    .param [1]/*08000058*/ 
    .custom /*0C000074:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 18 00 00 00 00 00 ) 
    // Method begins at RVA 0x21c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetInterfaceMap

  .method /*06000067*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] 
          GetInterfaces() cil managed
  // SIG: 20 00 1D 12 71
  {
    .custom /*0C000085:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 20 00 00 00 00 )                         // ... ....
    // Method begins at RVA 0x21c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetInterfaces

  .method /*06000068*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*0100003E*/[] 
          GetMember(string name,
                    valuetype [System.Runtime/*23000001*/]System.Reflection.MemberTypes/*0100003F*/ 'type',
                    valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 03 1D 12 80 F9 0E 11 80 FD 11 80 C9
  {
    .custom /*0C000087:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF 1F 00 00 00 00 ) 
    // Method begins at RVA 0x21cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetMember

  .method /*06000069*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*0100003E*/[] 
          GetMembers(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 F9 11 80 C9
  {
    .custom /*0C000089:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF 1F 00 00 00 00 ) 
    // Method begins at RVA 0x21cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetMembers

  .method /*0600006A*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetMethodImpl(string name,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr,
                        class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000033*/ binder,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callConvention,
                        class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] types,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003A*/[] modifiers) cil managed
  // SIG: 20 06 12 59 0E 11 80 C9 12 80 CD 11 80 AD 1D 12 71 1D 11 80 E9
  {
    .custom /*0C00008C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00008D:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 18 00 00 00 00 00 ) 
    .param [1]/*0800005D*/ 
    .custom /*0C000078:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [5]/*08000061*/ 
    .custom /*0C00007D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x21d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetMethodImpl

  .method /*0600006B*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/[] 
          GetMethods(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 59 11 80 C9
  {
    .custom /*0C000090:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 18 00 00 00 00 00 ) 
    // Method begins at RVA 0x21d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetMethods

  .method /*0600006C*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          GetNestedType(string name,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 02 12 71 0E 11 80 C9
  {
    .custom /*0C000091:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 80 01 00 00 00 00 ) 
    .param [0]/*08000064*/ 
    .custom /*0C000081:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetNestedType

  .method /*0600006D*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] 
          GetNestedTypes(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 71 11 80 C9
  {
    .custom /*0C000092:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 80 01 00 00 00 00 ) 
    // Method begins at RVA 0x21db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetNestedTypes

  .method /*0600006E*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000040*/[] 
          GetProperties(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 81 01 11 80 C9
  {
    .custom /*0C000093:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 06 00 00 00 00 ) 
    // Method begins at RVA 0x21de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetProperties

  .method /*0600006F*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000040*/ 
          GetPropertyImpl(string name,
                          valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr,
                          class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000033*/ binder,
                          class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType,
                          class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] types,
                          valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003A*/[] modifiers) cil managed
  // SIG: 20 06 12 81 01 0E 11 80 C9 12 80 CD 12 71 1D 12 71 1D 11 80 E9
  {
    .custom /*0C000095:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000096:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 06 00 00 00 00 ) 
    .param [0]/*08000069*/ 
    .custom /*0C00008A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800006A*/ 
    .custom /*0C00008E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [5]/*0800006E*/ 
    .custom /*0C000094:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x21e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetPropertyImpl

  .method /*06000070*/ family hidebysig virtual 
          instance bool  HasElementTypeImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::HasElementTypeImpl

  .method /*06000071*/ public hidebysig virtual 
          instance object  InvokeMember(string name,
                                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ invokeAttr,
                                        class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000033*/ binder,
                                        object target,
                                        object[] args,
                                        valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003A*/[] modifiers,
                                        class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000024*/ culture,
                                        string[] namedParameters) cil managed
  // SIG: 20 08 1C 0E 11 80 C9 12 80 CD 1C 1D 1C 1D 11 80 E9 12 80 91 1D 0E
  {
    .custom /*0C000098:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000099:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF FF FF FF 00 00 ) 
    .param [1]/*08000070*/ 
    .custom /*0C000097:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [8]/*08000077*/ 
    .custom /*0C00009B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x21e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::InvokeMember

  .method /*06000072*/ family hidebysig virtual 
          instance bool  IsArrayImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::IsArrayImpl

  .method /*06000073*/ public hidebysig virtual 
          instance bool  IsAssignableFrom(class [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000034*/ typeInfo) cil managed
  // SIG: 20 01 02 12 80 D1
  {
    .custom /*0C00009A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000078*/ 
    .custom /*0C00009C:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000041*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::IsAssignableFrom

  .method /*06000074*/ family hidebysig virtual 
          instance bool  IsByRefImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::IsByRefImpl

  .method /*06000075*/ family hidebysig virtual 
          instance bool  IsCOMObjectImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::IsCOMObjectImpl

  .method /*06000076*/ public hidebysig virtual 
          instance bool  IsDefined(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ attributeType,
                                   bool inherit) cil managed
  // SIG: 20 02 02 12 71 02
  {
    // Method begins at RVA 0x21f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::IsDefined

  .method /*06000077*/ family hidebysig virtual 
          instance bool  IsPointerImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::IsPointerImpl

  .method /*06000078*/ family hidebysig virtual 
          instance bool  IsPrimitiveImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::IsPrimitiveImpl

  .method /*06000079*/ family hidebysig virtual 
          instance bool  IsValueTypeImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::IsValueTypeImpl

  .method /*0600007A*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          MakeArrayType() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x2202
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::MakeArrayType

  .method /*0600007B*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          MakeArrayType(int32 rank) cil managed
  // SIG: 20 01 12 71 08
  {
    // Method begins at RVA 0x2205
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::MakeArrayType

  .method /*0600007C*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          MakeByRefType() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x2208
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::MakeByRefType

  .method /*0600007D*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          MakePointerType() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x220b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::MakePointerType

  .method /*0600007E*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000026*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 99 1D 05
  {
    // Method begins at RVA 0x220e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EnumBuilder::SetCustomAttribute

  .method /*0600007F*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001A*/ customBuilder) cil managed
  // SIG: 20 01 01 12 69
  {
    // Method begins at RVA 0x2210
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EnumBuilder::SetCustomAttribute

  .property /*17000013*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/
          Assembly()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ System.Reflection.Emit.EnumBuilder/*02000009*/::get_Assembly() /* 06000045 */
  } // end of property EnumBuilder::Assembly
  .property /*17000014*/ instance callconv(8) string
          AssemblyQualifiedName()
  {
    .custom /*0C000044:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.EnumBuilder/*02000009*/::get_AssemblyQualifiedName() /* 06000046 */
  } // end of property EnumBuilder::AssemblyQualifiedName
  .property /*17000015*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          BaseType()
  {
    .custom /*0C000045:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.EnumBuilder/*02000009*/::get_BaseType() /* 06000047 */
  } // end of property EnumBuilder::BaseType
  .property /*17000016*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          DeclaringType()
  {
    .custom /*0C000046:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.EnumBuilder/*02000009*/::get_DeclaringType() /* 06000048 */
  } // end of property EnumBuilder::DeclaringType
  .property /*17000017*/ instance callconv(8) string
          FullName()
  {
    .custom /*0C000048:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.EnumBuilder/*02000009*/::get_FullName() /* 06000049 */
  } // end of property EnumBuilder::FullName
  .property /*17000018*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/
          GUID()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/ System.Reflection.Emit.EnumBuilder/*02000009*/::get_GUID() /* 0600004A */
  } // end of property EnumBuilder::GUID
  .property /*17000019*/ instance callconv(8) bool
          IsByRefLike()
  {
    .get instance bool System.Reflection.Emit.EnumBuilder/*02000009*/::get_IsByRefLike() /* 0600004B */
  } // end of property EnumBuilder::IsByRefLike
  .property /*1700001A*/ instance callconv(8) bool
          IsConstructedGenericType()
  {
    .get instance bool System.Reflection.Emit.EnumBuilder/*02000009*/::get_IsConstructedGenericType() /* 0600004C */
  } // end of property EnumBuilder::IsConstructedGenericType
  .property /*1700001B*/ instance callconv(8) bool
          IsSZArray()
  {
    .get instance bool System.Reflection.Emit.EnumBuilder/*02000009*/::get_IsSZArray() /* 0600004D */
  } // end of property EnumBuilder::IsSZArray
  .property /*1700001C*/ instance callconv(8) bool
          IsTypeDefinition()
  {
    .get instance bool System.Reflection.Emit.EnumBuilder/*02000009*/::get_IsTypeDefinition() /* 0600004E */
  } // end of property EnumBuilder::IsTypeDefinition
  .property /*1700001D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/
          Module()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ System.Reflection.Emit.EnumBuilder/*02000009*/::get_Module() /* 0600004F */
  } // end of property EnumBuilder::Module
  .property /*1700001E*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Reflection.Emit.EnumBuilder/*02000009*/::get_Name() /* 06000050 */
  } // end of property EnumBuilder::Name
  .property /*1700001F*/ instance callconv(8) string
          Namespace()
  {
    .custom /*0C00004E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.EnumBuilder/*02000009*/::get_Namespace() /* 06000051 */
  } // end of property EnumBuilder::Namespace
  .property /*17000020*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          ReflectedType()
  {
    .custom /*0C00004F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.EnumBuilder/*02000009*/::get_ReflectedType() /* 06000052 */
  } // end of property EnumBuilder::ReflectedType
  .property /*17000021*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000036*/
          TypeHandle()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000036*/ System.Reflection.Emit.EnumBuilder/*02000009*/::get_TypeHandle() /* 06000053 */
  } // end of property EnumBuilder::TypeHandle
  .property /*17000022*/ instance callconv(8) class System.Reflection.Emit.FieldBuilder/*0200000B*/
          UnderlyingField()
  {
    .get instance class System.Reflection.Emit.FieldBuilder/*0200000B*/ System.Reflection.Emit.EnumBuilder/*02000009*/::get_UnderlyingField() /* 06000054 */
  } // end of property EnumBuilder::UnderlyingField
  .property /*17000023*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          UnderlyingSystemType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.EnumBuilder/*02000009*/::get_UnderlyingSystemType() /* 06000055 */
  } // end of property EnumBuilder::UnderlyingSystemType
} // end of class System.Reflection.Emit.EnumBuilder

.class /*0200000A*/ public auto ansi sealed beforefieldinit System.Reflection.Emit.EventBuilder
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
{
  .custom /*0C00002F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000030:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000080*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2212
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001B*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EventBuilder::.ctor

  .method /*06000081*/ public hidebysig instance void 
          AddOtherMethod(class System.Reflection.Emit.MethodBuilder/*0200000D*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 34
  {
    // Method begins at RVA 0x221a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EventBuilder::AddOtherMethod

  .method /*06000082*/ public hidebysig instance void 
          SetAddOnMethod(class System.Reflection.Emit.MethodBuilder/*0200000D*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 34
  {
    // Method begins at RVA 0x221c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EventBuilder::SetAddOnMethod

  .method /*06000083*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000026*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 99 1D 05
  {
    // Method begins at RVA 0x221e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EventBuilder::SetCustomAttribute

  .method /*06000084*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001A*/ customBuilder) cil managed
  // SIG: 20 01 01 12 69
  {
    // Method begins at RVA 0x2220
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EventBuilder::SetCustomAttribute

  .method /*06000085*/ public hidebysig instance void 
          SetRaiseMethod(class System.Reflection.Emit.MethodBuilder/*0200000D*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 34
  {
    // Method begins at RVA 0x2222
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EventBuilder::SetRaiseMethod

  .method /*06000086*/ public hidebysig instance void 
          SetRemoveOnMethod(class System.Reflection.Emit.MethodBuilder/*0200000D*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 34
  {
    // Method begins at RVA 0x2224
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EventBuilder::SetRemoveOnMethod

} // end of class System.Reflection.Emit.EventBuilder

.class /*0200000B*/ public auto ansi sealed beforefieldinit System.Reflection.Emit.FieldBuilder
       extends [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003C*/
{
  .custom /*0C000031:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000032:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000087*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2226
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003C*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method FieldBuilder::.ctor

  .method /*06000088*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000042*/ 
          get_Attributes() cil managed
  // SIG: 20 00 11 81 09
  {
    // Method begins at RVA 0x222e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::get_Attributes

  .method /*06000089*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_DeclaringType() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C00009F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2231
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::get_DeclaringType

  .method /*0600008A*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.RuntimeFieldHandle/*01000043*/ 
          get_FieldHandle() cil managed
  // SIG: 20 00 11 81 0D
  {
    // Method begins at RVA 0x2234
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::get_FieldHandle

  .method /*0600008B*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_FieldType() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x2237
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::get_FieldType

  .method /*0600008C*/ public hidebysig specialname virtual 
          instance int32  get_MetadataToken() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x223a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::get_MetadataToken

  .method /*0600008D*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ 
          get_Module() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x223d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::get_Module

  .method /*0600008E*/ public hidebysig specialname virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2240
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::get_Name

  .method /*0600008F*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_ReflectedType() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C0000A0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2243
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::get_ReflectedType

  .method /*06000090*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(bool inherit) cil managed
  // SIG: 20 01 1D 1C 02
  {
    // Method begins at RVA 0x2246
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::GetCustomAttributes

  .method /*06000091*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ attributeType,
                                                 bool inherit) cil managed
  // SIG: 20 02 1D 1C 12 71 02
  {
    // Method begins at RVA 0x2249
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::GetCustomAttributes

  .method /*06000092*/ public hidebysig virtual 
          instance object  GetValue(object obj) cil managed
  // SIG: 20 01 1C 1C
  {
    .custom /*0C0000A1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x224c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::GetValue

  .method /*06000093*/ public hidebysig virtual 
          instance bool  IsDefined(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ attributeType,
                                   bool inherit) cil managed
  // SIG: 20 02 02 12 71 02
  {
    // Method begins at RVA 0x224f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::IsDefined

  .method /*06000094*/ public hidebysig instance void 
          SetConstant(object defaultValue) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C0000A2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2252
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FieldBuilder::SetConstant

  .method /*06000095*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000026*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 99 1D 05
  {
    // Method begins at RVA 0x2254
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FieldBuilder::SetCustomAttribute

  .method /*06000096*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001A*/ customBuilder) cil managed
  // SIG: 20 01 01 12 69
  {
    // Method begins at RVA 0x2256
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FieldBuilder::SetCustomAttribute

  .method /*06000097*/ public hidebysig instance void 
          SetOffset(int32 iOffset) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2258
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FieldBuilder::SetOffset

  .method /*06000098*/ public hidebysig virtual 
          instance void  SetValue(object obj,
                                  object val,
                                  valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ invokeAttr,
                                  class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000033*/ binder,
                                  class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000024*/ culture) cil managed
  // SIG: 20 05 01 1C 1C 11 80 C9 12 80 CD 12 80 91
  {
    .custom /*0C0000A3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x225a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FieldBuilder::SetValue

  .property /*17000024*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000042*/
          Attributes()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000042*/ System.Reflection.Emit.FieldBuilder/*0200000B*/::get_Attributes() /* 06000088 */
  } // end of property FieldBuilder::Attributes
  .property /*17000025*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          DeclaringType()
  {
    .custom /*0C000051:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.FieldBuilder/*0200000B*/::get_DeclaringType() /* 06000089 */
  } // end of property FieldBuilder::DeclaringType
  .property /*17000026*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.RuntimeFieldHandle/*01000043*/
          FieldHandle()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.RuntimeFieldHandle/*01000043*/ System.Reflection.Emit.FieldBuilder/*0200000B*/::get_FieldHandle() /* 0600008A */
  } // end of property FieldBuilder::FieldHandle
  .property /*17000027*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          FieldType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.FieldBuilder/*0200000B*/::get_FieldType() /* 0600008B */
  } // end of property FieldBuilder::FieldType
  .property /*17000028*/ instance callconv(8) int32
          MetadataToken()
  {
    .get instance int32 System.Reflection.Emit.FieldBuilder/*0200000B*/::get_MetadataToken() /* 0600008C */
  } // end of property FieldBuilder::MetadataToken
  .property /*17000029*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/
          Module()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ System.Reflection.Emit.FieldBuilder/*0200000B*/::get_Module() /* 0600008D */
  } // end of property FieldBuilder::Module
  .property /*1700002A*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Reflection.Emit.FieldBuilder/*0200000B*/::get_Name() /* 0600008E */
  } // end of property FieldBuilder::Name
  .property /*1700002B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          ReflectedType()
  {
    .custom /*0C000054:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.FieldBuilder/*0200000B*/::get_ReflectedType() /* 0600008F */
  } // end of property FieldBuilder::ReflectedType
} // end of class System.Reflection.Emit.FieldBuilder

.class /*0200000C*/ public auto ansi sealed beforefieldinit System.Reflection.Emit.GenericTypeParameterBuilder
       extends [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000034*/
{
  .custom /*0C000034:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000035:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000099*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x225c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000034*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GenericTypeParameterBuilder::.ctor

  .method /*0600009A*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ 
          get_Assembly() cil managed
  // SIG: 20 00 12 55
  {
    // Method begins at RVA 0x2264
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_Assembly

  .method /*0600009B*/ public hidebysig specialname virtual 
          instance string  get_AssemblyQualifiedName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000A5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2267
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_AssemblyQualifiedName

  .method /*0600009C*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_BaseType() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C0000A7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x226a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_BaseType

  .method /*0600009D*/ public hidebysig specialname virtual 
          instance bool  get_ContainsGenericParameters() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x226d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_ContainsGenericParameters

  .method /*0600009E*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodBase/*01000044*/ 
          get_DeclaringMethod() cil managed
  // SIG: 20 00 12 81 11
  {
    .custom /*0C0000A8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2270
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_DeclaringMethod

  .method /*0600009F*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_DeclaringType() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C0000A9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2273
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_DeclaringType

  .method /*060000A0*/ public hidebysig specialname virtual 
          instance string  get_FullName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000AA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2276
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_FullName

  .method /*060000A1*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.GenericParameterAttributes/*01000045*/ 
          get_GenericParameterAttributes() cil managed
  // SIG: 20 00 11 81 15
  {
    // Method begins at RVA 0x2279
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_GenericParameterAttributes

  .method /*060000A2*/ public hidebysig specialname virtual 
          instance int32  get_GenericParameterPosition() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x227c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_GenericParameterPosition

  .method /*060000A3*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/ 
          get_GUID() cil managed
  // SIG: 20 00 11 80 D5
  {
    // Method begins at RVA 0x227f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_GUID

  .method /*060000A4*/ public hidebysig specialname virtual 
          instance bool  get_IsByRefLike() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2282
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_IsByRefLike

  .method /*060000A5*/ public hidebysig specialname virtual 
          instance bool  get_IsConstructedGenericType() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2285
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_IsConstructedGenericType

  .method /*060000A6*/ public hidebysig specialname virtual 
          instance bool  get_IsGenericParameter() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2288
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_IsGenericParameter

  .method /*060000A7*/ public hidebysig specialname virtual 
          instance bool  get_IsGenericType() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x228b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_IsGenericType

  .method /*060000A8*/ public hidebysig specialname virtual 
          instance bool  get_IsGenericTypeDefinition() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x228e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_IsGenericTypeDefinition

  .method /*060000A9*/ public hidebysig specialname virtual 
          instance bool  get_IsSZArray() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2291
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_IsSZArray

  .method /*060000AA*/ public hidebysig specialname virtual 
          instance bool  get_IsTypeDefinition() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2294
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_IsTypeDefinition

  .method /*060000AB*/ public hidebysig specialname virtual 
          instance int32  get_MetadataToken() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2297
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_MetadataToken

  .method /*060000AC*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ 
          get_Module() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x229a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_Module

  .method /*060000AD*/ public hidebysig specialname virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x229d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_Name

  .method /*060000AE*/ public hidebysig specialname virtual 
          instance string  get_Namespace() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000AD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_Namespace

  .method /*060000AF*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_ReflectedType() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C0000AE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_ReflectedType

  .method /*060000B0*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000036*/ 
          get_TypeHandle() cil managed
  // SIG: 20 00 11 80 D9
  {
    // Method begins at RVA 0x22a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_TypeHandle

  .method /*060000B1*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_UnderlyingSystemType() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x22a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_UnderlyingSystemType

  .method /*060000B2*/ public hidebysig virtual 
          instance bool  Equals(object o) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000B0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::Equals

  .method /*060000B3*/ family hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*01000039*/ 
          GetAttributeFlagsImpl() cil managed
  // SIG: 20 00 11 80 E5
  {
    // Method begins at RVA 0x22af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetAttributeFlagsImpl

  .method /*060000B4*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000026*/ 
          GetConstructorImpl(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr,
                             class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000033*/ binder,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callConvention,
                             class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] types,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003A*/[] modifiers) cil managed
  // SIG: 20 05 12 80 99 11 80 C9 12 80 CD 11 80 AD 1D 12 71 1D 11 80 E9
  {
    .custom /*0C0000B3:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 07 00 00 00 00 00 ) 
    .param [2]/*08000098*/ 
    .custom /*0C0000A4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800009B*/ 
    .custom /*0C0000A6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetConstructorImpl

  .method /*060000B5*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000026*/[] 
          GetConstructors(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 99 11 80 C9
  {
    .custom /*0C0000B4:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 07 00 00 00 00 00 ) 
    // Method begins at RVA 0x22b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetConstructors

  .method /*060000B6*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(bool inherit) cil managed
  // SIG: 20 01 1D 1C 02
  {
    // Method begins at RVA 0x22b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetCustomAttributes

  .method /*060000B7*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ attributeType,
                                                 bool inherit) cil managed
  // SIG: 20 02 1D 1C 12 71 02
  {
    // Method begins at RVA 0x22bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetCustomAttributes

  .method /*060000B8*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          GetElementType() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x22be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetElementType

  .method /*060000B9*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100003B*/ 
          GetEvent(string name,
                   valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 02 12 80 ED 0E 11 80 C9
  {
    .custom /*0C0000B5:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 18 00 00 00 00 ) 
    // Method begins at RVA 0x22c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetEvent

  .method /*060000BA*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100003B*/[] 
          GetEvents() cil managed
  // SIG: 20 00 1D 12 80 ED
  {
    .custom /*0C0000B7:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 08 00 00 00 00 ) 
    // Method begins at RVA 0x22c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetEvents

  .method /*060000BB*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100003B*/[] 
          GetEvents(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 ED 11 80 C9
  {
    .custom /*0C0000B9:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 18 00 00 00 00 ) 
    // Method begins at RVA 0x22c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetEvents

  .method /*060000BC*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003C*/ 
          GetField(string name,
                   valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 02 12 80 F1 0E 11 80 C9
  {
    .custom /*0C0000BA:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 60 00 00 00 00 00 )                         // ..`.....
    // Method begins at RVA 0x22ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetField

  .method /*060000BD*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003C*/[] 
          GetFields(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 F1 11 80 C9
  {
    .custom /*0C0000BB:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 60 00 00 00 00 00 )                         // ..`.....
    // Method begins at RVA 0x22cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetFields

  .method /*060000BE*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] 
          GetGenericArguments() cil managed
  // SIG: 20 00 1D 12 71
  {
    // Method begins at RVA 0x22d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetGenericArguments

  .method /*060000BF*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          GetGenericTypeDefinition() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x22d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetGenericTypeDefinition

  .method /*060000C0*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x22d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetHashCode

  .method /*060000C1*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          GetInterface(string name,
                       bool ignoreCase) cil managed
  // SIG: 20 02 12 71 0E 02
  {
    .custom /*0C0000BE:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 20 00 00 00 00 )                         // ... ....
    .param [0]/*080000A6*/ 
    .custom /*0C0000AB:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 20 00 00 00 00 )                         // ... ....
    // Method begins at RVA 0x22d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetInterface

  .method /*060000C2*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.InterfaceMapping/*0100003D*/ 
          GetInterfaceMap(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ interfaceType) cil managed
  // SIG: 20 01 11 80 F5 12 71
  {
    .param [1]/*080000A9*/ 
    .custom /*0C0000AC:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 18 00 00 00 00 00 ) 
    // Method begins at RVA 0x22dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetInterfaceMap

  .method /*060000C3*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] 
          GetInterfaces() cil managed
  // SIG: 20 00 1D 12 71
  {
    .custom /*0C0000C0:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 20 00 00 00 00 )                         // ... ....
    // Method begins at RVA 0x22df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetInterfaces

  .method /*060000C4*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*0100003E*/[] 
          GetMember(string name,
                    valuetype [System.Runtime/*23000001*/]System.Reflection.MemberTypes/*0100003F*/ 'type',
                    valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 03 1D 12 80 F9 0E 11 80 FD 11 80 C9
  {
    .custom /*0C0000C1:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF 1F 00 00 00 00 ) 
    // Method begins at RVA 0x22e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetMember

  .method /*060000C5*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*0100003E*/[] 
          GetMembers(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 F9 11 80 C9
  {
    .custom /*0C0000C2:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF 1F 00 00 00 00 ) 
    // Method begins at RVA 0x22e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetMembers

  .method /*060000C6*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetMethodImpl(string name,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr,
                        class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000033*/ binder,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callConvention,
                        class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] types,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003A*/[] modifiers) cil managed
  // SIG: 20 06 12 59 0E 11 80 C9 12 80 CD 11 80 AD 1D 12 71 1D 11 80 E9
  {
    .custom /*0C0000C3:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 18 00 00 00 00 00 ) 
    .param [3]/*080000B0*/ 
    .custom /*0C0000AF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080000B2*/ 
    .custom /*0C0000B1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [6]/*080000B3*/ 
    .custom /*0C0000B2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetMethodImpl

  .method /*060000C7*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/[] 
          GetMethods(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 59 11 80 C9
  {
    .custom /*0C0000C4:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 18 00 00 00 00 00 ) 
    // Method begins at RVA 0x22eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetMethods

  .method /*060000C8*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          GetNestedType(string name,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 02 12 71 0E 11 80 C9
  {
    .custom /*0C0000C5:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 80 01 00 00 00 00 ) 
    // Method begins at RVA 0x22ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetNestedType

  .method /*060000C9*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] 
          GetNestedTypes(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 71 11 80 C9
  {
    .custom /*0C0000C7:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 80 01 00 00 00 00 ) 
    // Method begins at RVA 0x22f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetNestedTypes

  .method /*060000CA*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000040*/[] 
          GetProperties(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 81 01 11 80 C9
  {
    .custom /*0C0000C9:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 06 00 00 00 00 ) 
    // Method begins at RVA 0x22f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetProperties

  .method /*060000CB*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000040*/ 
          GetPropertyImpl(string name,
                          valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr,
                          class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000033*/ binder,
                          class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType,
                          class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] types,
                          valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003A*/[] modifiers) cil managed
  // SIG: 20 06 12 81 01 0E 11 80 C9 12 80 CD 12 71 1D 12 71 1D 11 80 E9
  {
    .custom /*0C0000CB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000CC:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 06 00 00 00 00 ) 
    .param [0]/*080000B9*/ 
    .custom /*0C0000B6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*080000BA*/ 
    .custom /*0C0000B8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [5]/*080000BE*/ 
    .custom /*0C0000BC:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetPropertyImpl

  .method /*060000CC*/ family hidebysig virtual 
          instance bool  HasElementTypeImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::HasElementTypeImpl

  .method /*060000CD*/ public hidebysig virtual 
          instance object  InvokeMember(string name,
                                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ invokeAttr,
                                        class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000033*/ binder,
                                        object target,
                                        object[] args,
                                        valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003A*/[] modifiers,
                                        class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000024*/ culture,
                                        string[] namedParameters) cil managed
  // SIG: 20 08 1C 0E 11 80 C9 12 80 CD 1C 1D 1C 1D 11 80 E9 12 80 91 1D 0E
  {
    .custom /*0C0000CD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000CE:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF FF FF FF 00 00 ) 
    .param [0]/*080000C0*/ 
    .custom /*0C0000BD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*080000C1*/ 
    .custom /*0C0000BF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [8]/*080000C8*/ 
    .custom /*0C0000C6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::InvokeMember

  .method /*060000CE*/ family hidebysig virtual 
          instance bool  IsArrayImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2300
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::IsArrayImpl

  .method /*060000CF*/ public hidebysig virtual 
          instance bool  IsAssignableFrom(class [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000034*/ typeInfo) cil managed
  // SIG: 20 01 02 12 80 D1
  {
    .custom /*0C0000CF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000C9*/ 
    .custom /*0C0000C8:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000041*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2303
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::IsAssignableFrom

  .method /*060000D0*/ public hidebysig virtual 
          instance bool  IsAssignableFrom(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ c) cil managed
  // SIG: 20 01 02 12 71
  {
    .custom /*0C0000D1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000CA*/ 
    .custom /*0C0000CA:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000041*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2306
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::IsAssignableFrom

  .method /*060000D1*/ family hidebysig virtual 
          instance bool  IsByRefImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2309
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::IsByRefImpl

  .method /*060000D2*/ family hidebysig virtual 
          instance bool  IsCOMObjectImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x230c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::IsCOMObjectImpl

  .method /*060000D3*/ public hidebysig virtual 
          instance bool  IsDefined(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ attributeType,
                                   bool inherit) cil managed
  // SIG: 20 02 02 12 71 02
  {
    // Method begins at RVA 0x230f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::IsDefined

  .method /*060000D4*/ family hidebysig virtual 
          instance bool  IsPointerImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2312
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::IsPointerImpl

  .method /*060000D5*/ family hidebysig virtual 
          instance bool  IsPrimitiveImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2315
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::IsPrimitiveImpl

  .method /*060000D6*/ public hidebysig virtual 
          instance bool  IsSubclassOf(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ c) cil managed
  // SIG: 20 01 02 12 71
  {
    // Method begins at RVA 0x2318
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::IsSubclassOf

  .method /*060000D7*/ family hidebysig virtual 
          instance bool  IsValueTypeImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x231b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::IsValueTypeImpl

  .method /*060000D8*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          MakeArrayType() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x231e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::MakeArrayType

  .method /*060000D9*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          MakeArrayType(int32 rank) cil managed
  // SIG: 20 01 12 71 08
  {
    // Method begins at RVA 0x2321
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::MakeArrayType

  .method /*060000DA*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          MakeByRefType() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x2324
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::MakeByRefType

  .method /*060000DB*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          MakeGenericType(class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] typeArguments) cil managed
  // SIG: 20 01 12 71 1D 12 71
  {
    .custom /*0C0000D7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 80 C5 49 66 20 73 6F 6D 65 20 6F 66 20 74   // ....If some of t
                                                                                                                                                                                            68 65 20 67 65 6E 65 72 69 63 20 61 72 67 75 6D   // he generic argum
                                                                                                                                                                                            65 6E 74 73 20 61 72 65 20 61 6E 6E 6F 74 61 74   // ents are annotat
                                                                                                                                                                                            65 64 20 28 65 69 74 68 65 72 20 77 69 74 68 20   // ed (either with 
                                                                                                                                                                                            44 79 6E 61 6D 69 63 61 6C 6C 79 41 63 63 65 73   // DynamicallyAcces
                                                                                                                                                                                            73 65 64 4D 65 6D 62 65 72 73 41 74 74 72 69 62   // sedMembersAttrib
                                                                                                                                                                                            75 74 65 2C 20 6F 72 20 67 65 6E 65 72 69 63 20   // ute, or generic 
                                                                                                                                                                                            63 6F 6E 73 74 72 61 69 6E 74 73 29 2C 20 74 72   // constraints), tr
                                                                                                                                                                                            69 6D 6D 69 6E 67 20 63 61 6E 27 74 20 76 61 6C   // imming can't val
                                                                                                                                                                                            69 64 61 74 65 20 74 68 61 74 20 74 68 65 20 72   // idate that the r
                                                                                                                                                                                            65 71 75 69 72 65 6D 65 6E 74 73 20 6F 66 20 74   // equirements of t
                                                                                                                                                                                            68 6F 73 65 20 61 6E 6E 6F 74 61 74 69 6F 6E 73   // hose annotations
                                                                                                                                                                                            20 61 72 65 20 6D 65 74 2E 00 00 )                //  are met...
    .param [1]/*080000CF*/ 
    .custom /*0C0000D0:0A000018*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000046*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2327
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::MakeGenericType

  .method /*060000DC*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          MakePointerType() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x232a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::MakePointerType

  .method /*060000DD*/ public hidebysig instance void 
          SetBaseTypeConstraint(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ baseTypeConstraint) cil managed
  // SIG: 20 01 01 12 71
  {
    .custom /*0C0000D8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000D0*/ 
    .custom /*0C0000D2:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x232d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GenericTypeParameterBuilder::SetBaseTypeConstraint

  .method /*060000DE*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000026*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 99 1D 05
  {
    // Method begins at RVA 0x232f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GenericTypeParameterBuilder::SetCustomAttribute

  .method /*060000DF*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001A*/ customBuilder) cil managed
  // SIG: 20 01 01 12 69
  {
    // Method begins at RVA 0x2331
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GenericTypeParameterBuilder::SetCustomAttribute

  .method /*060000E0*/ public hidebysig instance void 
          SetGenericParameterAttributes(valuetype [System.Runtime/*23000001*/]System.Reflection.GenericParameterAttributes/*01000045*/ genericParameterAttributes) cil managed
  // SIG: 20 01 01 11 81 15
  {
    // Method begins at RVA 0x2333
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GenericTypeParameterBuilder::SetGenericParameterAttributes

  .method /*060000E1*/ public hidebysig instance void 
          SetInterfaceConstraints(class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] interfaceConstraints) cil managed
  // SIG: 20 01 01 1D 12 71
  {
    .param [1]/*080000D5*/ 
    .custom /*0C0000D3:0A000018*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000046*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    .custom /*0C0000D4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2335
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GenericTypeParameterBuilder::SetInterfaceConstraints

  .method /*060000E2*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2337
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::ToString

  .property /*1700002C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/
          Assembly()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_Assembly() /* 0600009A */
  } // end of property GenericTypeParameterBuilder::Assembly
  .property /*1700002D*/ instance callconv(8) string
          AssemblyQualifiedName()
  {
    .custom /*0C000055:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_AssemblyQualifiedName() /* 0600009B */
  } // end of property GenericTypeParameterBuilder::AssemblyQualifiedName
  .property /*1700002E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          BaseType()
  {
    .custom /*0C000057:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_BaseType() /* 0600009C */
  } // end of property GenericTypeParameterBuilder::BaseType
  .property /*1700002F*/ instance callconv(8) bool
          ContainsGenericParameters()
  {
    .get instance bool System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_ContainsGenericParameters() /* 0600009D */
  } // end of property GenericTypeParameterBuilder::ContainsGenericParameters
  .property /*17000030*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodBase/*01000044*/
          DeclaringMethod()
  {
    .custom /*0C000059:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodBase/*01000044*/ System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_DeclaringMethod() /* 0600009E */
  } // end of property GenericTypeParameterBuilder::DeclaringMethod
  .property /*17000031*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          DeclaringType()
  {
    .custom /*0C00005A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_DeclaringType() /* 0600009F */
  } // end of property GenericTypeParameterBuilder::DeclaringType
  .property /*17000032*/ instance callconv(8) string
          FullName()
  {
    .custom /*0C00005B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_FullName() /* 060000A0 */
  } // end of property GenericTypeParameterBuilder::FullName
  .property /*17000033*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Reflection.GenericParameterAttributes/*01000045*/
          GenericParameterAttributes()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Reflection.GenericParameterAttributes/*01000045*/ System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_GenericParameterAttributes() /* 060000A1 */
  } // end of property GenericTypeParameterBuilder::GenericParameterAttributes
  .property /*17000034*/ instance callconv(8) int32
          GenericParameterPosition()
  {
    .get instance int32 System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_GenericParameterPosition() /* 060000A2 */
  } // end of property GenericTypeParameterBuilder::GenericParameterPosition
  .property /*17000035*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/
          GUID()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/ System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_GUID() /* 060000A3 */
  } // end of property GenericTypeParameterBuilder::GUID
  .property /*17000036*/ instance callconv(8) bool
          IsByRefLike()
  {
    .get instance bool System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_IsByRefLike() /* 060000A4 */
  } // end of property GenericTypeParameterBuilder::IsByRefLike
  .property /*17000037*/ instance callconv(8) bool
          IsConstructedGenericType()
  {
    .get instance bool System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_IsConstructedGenericType() /* 060000A5 */
  } // end of property GenericTypeParameterBuilder::IsConstructedGenericType
  .property /*17000038*/ instance callconv(8) bool
          IsGenericParameter()
  {
    .get instance bool System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_IsGenericParameter() /* 060000A6 */
  } // end of property GenericTypeParameterBuilder::IsGenericParameter
  .property /*17000039*/ instance callconv(8) bool
          IsGenericType()
  {
    .get instance bool System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_IsGenericType() /* 060000A7 */
  } // end of property GenericTypeParameterBuilder::IsGenericType
  .property /*1700003A*/ instance callconv(8) bool
          IsGenericTypeDefinition()
  {
    .get instance bool System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_IsGenericTypeDefinition() /* 060000A8 */
  } // end of property GenericTypeParameterBuilder::IsGenericTypeDefinition
  .property /*1700003B*/ instance callconv(8) bool
          IsSZArray()
  {
    .get instance bool System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_IsSZArray() /* 060000A9 */
  } // end of property GenericTypeParameterBuilder::IsSZArray
  .property /*1700003C*/ instance callconv(8) bool
          IsTypeDefinition()
  {
    .get instance bool System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_IsTypeDefinition() /* 060000AA */
  } // end of property GenericTypeParameterBuilder::IsTypeDefinition
  .property /*1700003D*/ instance callconv(8) int32
          MetadataToken()
  {
    .get instance int32 System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_MetadataToken() /* 060000AB */
  } // end of property GenericTypeParameterBuilder::MetadataToken
  .property /*1700003E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/
          Module()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_Module() /* 060000AC */
  } // end of property GenericTypeParameterBuilder::Module
  .property /*1700003F*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_Name() /* 060000AD */
  } // end of property GenericTypeParameterBuilder::Name
  .property /*17000040*/ instance callconv(8) string
          Namespace()
  {
    .custom /*0C000061:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_Namespace() /* 060000AE */
  } // end of property GenericTypeParameterBuilder::Namespace
  .property /*17000041*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          ReflectedType()
  {
    .custom /*0C000063:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_ReflectedType() /* 060000AF */
  } // end of property GenericTypeParameterBuilder::ReflectedType
  .property /*17000042*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000036*/
          TypeHandle()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000036*/ System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_TypeHandle() /* 060000B0 */
  } // end of property GenericTypeParameterBuilder::TypeHandle
  .property /*17000043*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          UnderlyingSystemType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/::get_UnderlyingSystemType() /* 060000B1 */
  } // end of property GenericTypeParameterBuilder::UnderlyingSystemType
} // end of class System.Reflection.Emit.GenericTypeParameterBuilder

.class /*0200000D*/ public auto ansi sealed beforefieldinit System.Reflection.Emit.MethodBuilder
       extends [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/
{
  .custom /*0C000037:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000038:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000E3*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x233a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MethodBuilder::.ctor

  .method /*060000E4*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/ 
          get_Attributes() cil managed
  // SIG: 20 00 11 80 A9
  {
    // Method begins at RVA 0x2342
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_Attributes

  .method /*060000E5*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ 
          get_CallingConvention() cil managed
  // SIG: 20 00 11 80 AD
  {
    // Method begins at RVA 0x2345
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_CallingConvention

  .method /*060000E6*/ public hidebysig specialname virtual 
          instance bool  get_ContainsGenericParameters() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2348
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_ContainsGenericParameters

  .method /*060000E7*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_DeclaringType() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C0000DA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x234b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_DeclaringType

  .method /*060000E8*/ public hidebysig specialname 
          instance bool  get_InitLocals() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x234e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_InitLocals

  .method /*060000E9*/ public hidebysig specialname 
          instance void  set_InitLocals(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2351
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MethodBuilder::set_InitLocals

  .method /*060000EA*/ public hidebysig specialname virtual 
          instance bool  get_IsGenericMethod() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2353
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_IsGenericMethod

  .method /*060000EB*/ public hidebysig specialname virtual 
          instance bool  get_IsGenericMethodDefinition() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2356
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_IsGenericMethodDefinition

  .method /*060000EC*/ public hidebysig specialname virtual 
          instance bool  get_IsSecurityCritical() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2359
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_IsSecurityCritical

  .method /*060000ED*/ public hidebysig specialname virtual 
          instance bool  get_IsSecuritySafeCritical() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x235c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_IsSecuritySafeCritical

  .method /*060000EE*/ public hidebysig specialname virtual 
          instance bool  get_IsSecurityTransparent() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x235f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_IsSecurityTransparent

  .method /*060000EF*/ public hidebysig specialname virtual 
          instance int32  get_MetadataToken() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2362
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_MetadataToken

  .method /*060000F0*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.RuntimeMethodHandle/*0100002C*/ 
          get_MethodHandle() cil managed
  // SIG: 20 00 11 80 B1
  {
    // Method begins at RVA 0x2365
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_MethodHandle

  .method /*060000F1*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ 
          get_Module() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x2368
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_Module

  .method /*060000F2*/ public hidebysig specialname virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x236b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_Name

  .method /*060000F3*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_ReflectedType() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C0000E0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x236e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_ReflectedType

  .method /*060000F4*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*01000031*/ 
          get_ReturnParameter() cil managed
  // SIG: 20 00 12 80 C5
  {
    // Method begins at RVA 0x2371
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_ReturnParameter

  .method /*060000F5*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_ReturnType() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x2374
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_ReturnType

  .method /*060000F6*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ICustomAttributeProvider/*01000047*/ 
          get_ReturnTypeCustomAttributes() cil managed
  // SIG: 20 00 12 81 1D
  {
    // Method begins at RVA 0x2377
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_ReturnTypeCustomAttributes

  .method /*060000F7*/ public hidebysig instance class System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/[] 
          DefineGenericParameters(string[] names) cil managed
  // SIG: 20 01 1D 12 30 1D 0E
  {
    .param [1]/*080000D7*/ 
    .custom /*0C0000D5:0A000018*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000046*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x237a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::DefineGenericParameters

  .method /*060000F8*/ public hidebysig instance class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.ParameterBuilder/*0100002D*/ 
          DefineParameter(int32 position,
                          valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterAttributes/*0100002E*/ attributes,
                          string strParamName) cil managed
  // SIG: 20 03 12 80 B5 08 11 80 B9 0E
  {
    .param [3]/*080000DA*/ 
    .custom /*0C0000D6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x237d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::DefineParameter

  .method /*060000F9*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000E3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2380
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::Equals

  .method /*060000FA*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetBaseDefinition() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2383
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::GetBaseDefinition

  .method /*060000FB*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(bool inherit) cil managed
  // SIG: 20 01 1D 1C 02
  {
    // Method begins at RVA 0x2386
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::GetCustomAttributes

  .method /*060000FC*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ attributeType,
                                                 bool inherit) cil managed
  // SIG: 20 02 1D 1C 12 71 02
  {
    // Method begins at RVA 0x2389
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::GetCustomAttributes

  .method /*060000FD*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] 
          GetGenericArguments() cil managed
  // SIG: 20 00 1D 12 71
  {
    // Method begins at RVA 0x238c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::GetGenericArguments

  .method /*060000FE*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetGenericMethodDefinition() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x238f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::GetGenericMethodDefinition

  .method /*060000FF*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2392
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::GetHashCode

  .method /*06000100*/ public hidebysig instance class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.ILGenerator/*0100002F*/ 
          GetILGenerator() cil managed
  // SIG: 20 00 12 80 BD
  {
    // Method begins at RVA 0x2395
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::GetILGenerator

  .method /*06000101*/ public hidebysig instance class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.ILGenerator/*0100002F*/ 
          GetILGenerator(int32 size) cil managed
  // SIG: 20 01 12 80 BD 08
  {
    // Method begins at RVA 0x2398
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::GetILGenerator

  .method /*06000102*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.MethodImplAttributes/*01000030*/ 
          GetMethodImplementationFlags() cil managed
  // SIG: 20 00 11 80 C1
  {
    // Method begins at RVA 0x239b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::GetMethodImplementationFlags

  .method /*06000103*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*01000031*/[] 
          GetParameters() cil managed
  // SIG: 20 00 1D 12 80 C5
  {
    // Method begins at RVA 0x239e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::GetParameters

  .method /*06000104*/ public hidebysig virtual 
          instance object  Invoke(object obj,
                                  valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ invokeAttr,
                                  class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000033*/ binder,
                                  object[] parameters,
                                  class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000024*/ culture) cil managed
  // SIG: 20 05 1C 1C 11 80 C9 12 80 CD 1D 1C 12 80 91
  {
    .custom /*0C0000EA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080000E0*/ 
    .custom /*0C0000D9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::Invoke

  .method /*06000105*/ public hidebysig virtual 
          instance bool  IsDefined(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ attributeType,
                                   bool inherit) cil managed
  // SIG: 20 02 02 12 71 02
  {
    // Method begins at RVA 0x23a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::IsDefined

  .method /*06000106*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          MakeGenericMethod(class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] typeArguments) cil managed
  // SIG: 20 01 12 59 1D 12 71
  {
    .custom /*0C0000ED:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 80 C5 49 66 20 73 6F 6D 65 20 6F 66 20 74   // ....If some of t
                                                                                                                                                                                            68 65 20 67 65 6E 65 72 69 63 20 61 72 67 75 6D   // he generic argum
                                                                                                                                                                                            65 6E 74 73 20 61 72 65 20 61 6E 6E 6F 74 61 74   // ents are annotat
                                                                                                                                                                                            65 64 20 28 65 69 74 68 65 72 20 77 69 74 68 20   // ed (either with 
                                                                                                                                                                                            44 79 6E 61 6D 69 63 61 6C 6C 79 41 63 63 65 73   // DynamicallyAcces
                                                                                                                                                                                            73 65 64 4D 65 6D 62 65 72 73 41 74 74 72 69 62   // sedMembersAttrib
                                                                                                                                                                                            75 74 65 2C 20 6F 72 20 67 65 6E 65 72 69 63 20   // ute, or generic 
                                                                                                                                                                                            63 6F 6E 73 74 72 61 69 6E 74 73 29 2C 20 74 72   // constraints), tr
                                                                                                                                                                                            69 6D 6D 69 6E 67 20 63 61 6E 27 74 20 76 61 6C   // imming can't val
                                                                                                                                                                                            69 64 61 74 65 20 74 68 61 74 20 74 68 65 20 72   // idate that the r
                                                                                                                                                                                            65 71 75 69 72 65 6D 65 6E 74 73 20 6F 66 20 74   // equirements of t
                                                                                                                                                                                            68 6F 73 65 20 61 6E 6E 6F 74 61 74 69 6F 6E 73   // hose annotations
                                                                                                                                                                                            20 61 72 65 20 6D 65 74 2E 00 00 )                //  are met...
    .param [1]/*080000E8*/ 
    .custom /*0C0000DB:0A000018*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000046*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::MakeGenericMethod

  .method /*06000107*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000026*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 99 1D 05
  {
    // Method begins at RVA 0x23aa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MethodBuilder::SetCustomAttribute

  .method /*06000108*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001A*/ customBuilder) cil managed
  // SIG: 20 01 01 12 69
  {
    // Method begins at RVA 0x23ac
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MethodBuilder::SetCustomAttribute

  .method /*06000109*/ public hidebysig instance void 
          SetImplementationFlags(valuetype [System.Runtime/*23000001*/]System.Reflection.MethodImplAttributes/*01000030*/ attributes) cil managed
  // SIG: 20 01 01 11 80 C1
  {
    // Method begins at RVA 0x23ae
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MethodBuilder::SetImplementationFlags

  .method /*0600010A*/ public hidebysig instance void 
          SetParameters(class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] parameterTypes) cil managed
  // SIG: 20 01 01 1D 12 71
  {
    .param [1]/*080000ED*/ 
    .custom /*0C0000DC:0A000018*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000046*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23b0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MethodBuilder::SetParameters

  .method /*0600010B*/ public hidebysig instance void 
          SetReturnType(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType) cil managed
  // SIG: 20 01 01 12 71
  {
    .custom /*0C0000F0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23b2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MethodBuilder::SetReturnType

  .method /*0600010C*/ public hidebysig instance void 
          SetSignature(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType,
                       class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] returnTypeRequiredCustomModifiers,
                       class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] returnTypeOptionalCustomModifiers,
                       class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] parameterTypes,
                       class [System.Runtime/*23000001*/]System.Type/*0100001C*/[][] parameterTypeRequiredCustomModifiers,
                       class [System.Runtime/*23000001*/]System.Type/*0100001C*/[][] parameterTypeOptionalCustomModifiers) cil managed
  // SIG: 20 06 01 12 71 1D 12 71 1D 12 71 1D 12 71 1D 1D 12 71 1D 1D 12 71
  {
    .custom /*0C0000F1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000F0*/ 
    .custom /*0C0000DD:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [3]/*080000F1*/ 
    .custom /*0C0000DE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [4]/*080000F2*/ 
    .custom /*0C0000DF:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [5]/*080000F3*/ 
    .custom /*0C0000E1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [6]/*080000F4*/ 
    .custom /*0C0000E2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x23b4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MethodBuilder::SetSignature

  .method /*0600010D*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::ToString

  .property /*17000044*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/
          Attributes()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/ System.Reflection.Emit.MethodBuilder/*0200000D*/::get_Attributes() /* 060000E4 */
  } // end of property MethodBuilder::Attributes
  .property /*17000045*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/
          CallingConvention()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ System.Reflection.Emit.MethodBuilder/*0200000D*/::get_CallingConvention() /* 060000E5 */
  } // end of property MethodBuilder::CallingConvention
  .property /*17000046*/ instance callconv(8) bool
          ContainsGenericParameters()
  {
    .get instance bool System.Reflection.Emit.MethodBuilder/*0200000D*/::get_ContainsGenericParameters() /* 060000E6 */
  } // end of property MethodBuilder::ContainsGenericParameters
  .property /*17000047*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          DeclaringType()
  {
    .custom /*0C000068:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.MethodBuilder/*0200000D*/::get_DeclaringType() /* 060000E7 */
  } // end of property MethodBuilder::DeclaringType
  .property /*17000048*/ instance callconv(8) bool
          InitLocals()
  {
    .get instance bool System.Reflection.Emit.MethodBuilder/*0200000D*/::get_InitLocals() /* 060000E8 */
    .set instance void System.Reflection.Emit.MethodBuilder/*0200000D*/::set_InitLocals(bool) /* 060000E9 */
  } // end of property MethodBuilder::InitLocals
  .property /*17000049*/ instance callconv(8) bool
          IsGenericMethod()
  {
    .get instance bool System.Reflection.Emit.MethodBuilder/*0200000D*/::get_IsGenericMethod() /* 060000EA */
  } // end of property MethodBuilder::IsGenericMethod
  .property /*1700004A*/ instance callconv(8) bool
          IsGenericMethodDefinition()
  {
    .get instance bool System.Reflection.Emit.MethodBuilder/*0200000D*/::get_IsGenericMethodDefinition() /* 060000EB */
  } // end of property MethodBuilder::IsGenericMethodDefinition
  .property /*1700004B*/ instance callconv(8) bool
          IsSecurityCritical()
  {
    .get instance bool System.Reflection.Emit.MethodBuilder/*0200000D*/::get_IsSecurityCritical() /* 060000EC */
  } // end of property MethodBuilder::IsSecurityCritical
  .property /*1700004C*/ instance callconv(8) bool
          IsSecuritySafeCritical()
  {
    .get instance bool System.Reflection.Emit.MethodBuilder/*0200000D*/::get_IsSecuritySafeCritical() /* 060000ED */
  } // end of property MethodBuilder::IsSecuritySafeCritical
  .property /*1700004D*/ instance callconv(8) bool
          IsSecurityTransparent()
  {
    .get instance bool System.Reflection.Emit.MethodBuilder/*0200000D*/::get_IsSecurityTransparent() /* 060000EE */
  } // end of property MethodBuilder::IsSecurityTransparent
  .property /*1700004E*/ instance callconv(8) int32
          MetadataToken()
  {
    .get instance int32 System.Reflection.Emit.MethodBuilder/*0200000D*/::get_MetadataToken() /* 060000EF */
  } // end of property MethodBuilder::MetadataToken
  .property /*1700004F*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.RuntimeMethodHandle/*0100002C*/
          MethodHandle()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.RuntimeMethodHandle/*0100002C*/ System.Reflection.Emit.MethodBuilder/*0200000D*/::get_MethodHandle() /* 060000F0 */
  } // end of property MethodBuilder::MethodHandle
  .property /*17000050*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/
          Module()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ System.Reflection.Emit.MethodBuilder/*0200000D*/::get_Module() /* 060000F1 */
  } // end of property MethodBuilder::Module
  .property /*17000051*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Reflection.Emit.MethodBuilder/*0200000D*/::get_Name() /* 060000F2 */
  } // end of property MethodBuilder::Name
  .property /*17000052*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          ReflectedType()
  {
    .custom /*0C00006F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.MethodBuilder/*0200000D*/::get_ReflectedType() /* 060000F3 */
  } // end of property MethodBuilder::ReflectedType
  .property /*17000053*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*01000031*/
          ReturnParameter()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*01000031*/ System.Reflection.Emit.MethodBuilder/*0200000D*/::get_ReturnParameter() /* 060000F4 */
  } // end of property MethodBuilder::ReturnParameter
  .property /*17000054*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          ReturnType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.MethodBuilder/*0200000D*/::get_ReturnType() /* 060000F5 */
  } // end of property MethodBuilder::ReturnType
  .property /*17000055*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.ICustomAttributeProvider/*01000047*/
          ReturnTypeCustomAttributes()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.ICustomAttributeProvider/*01000047*/ System.Reflection.Emit.MethodBuilder/*0200000D*/::get_ReturnTypeCustomAttributes() /* 060000F6 */
  } // end of property MethodBuilder::ReturnTypeCustomAttributes
} // end of class System.Reflection.Emit.MethodBuilder

.class /*0200000E*/ public auto ansi beforefieldinit System.Reflection.Emit.ModuleBuilder
       extends [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/
{
  .custom /*0C000039:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600010E*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23b9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ModuleBuilder::.ctor

  .method /*0600010F*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ 
          get_Assembly() cil managed
  // SIG: 20 00 12 55
  {
    // Method begins at RVA 0x23c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::get_Assembly

  .method /*06000110*/ public hidebysig specialname virtual 
          instance string  get_FullyQualifiedName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::get_FullyQualifiedName

  .method /*06000111*/ public hidebysig specialname virtual 
          instance int32  get_MDStreamVersion() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x23c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::get_MDStreamVersion

  .method /*06000112*/ public hidebysig specialname virtual 
          instance int32  get_MetadataToken() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x23ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::get_MetadataToken

  .method /*06000113*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/ 
          get_ModuleVersionId() cil managed
  // SIG: 20 00 11 80 D5
  {
    // Method begins at RVA 0x23cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::get_ModuleVersionId

  .method /*06000114*/ public hidebysig specialname virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::get_Name

  .method /*06000115*/ public hidebysig specialname virtual 
          instance string  get_ScopeName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::get_ScopeName

  .method /*06000116*/ public hidebysig instance void 
          CreateGlobalFunctions() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23d6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ModuleBuilder::CreateGlobalFunctions

  .method /*06000117*/ public hidebysig instance class System.Reflection.Emit.EnumBuilder/*02000009*/ 
          DefineEnum(string name,
                     valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*01000039*/ visibility,
                     class [System.Runtime/*23000001*/]System.Type/*0100001C*/ underlyingType) cil managed
  // SIG: 20 03 12 24 0E 11 80 E5 12 71
  {
    // Method begins at RVA 0x23d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineEnum

  .method /*06000118*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*0200000D*/ 
          DefineGlobalMethod(string name,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/ attributes,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callingConvention,
                             class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType,
                             class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] parameterTypes) cil managed
  // SIG: 20 05 12 34 0E 11 80 A9 11 80 AD 12 71 1D 12 71
  {
    .param [4]/*080000FB*/ 
    .custom /*0C0000E4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080000FC*/ 
    .custom /*0C0000E5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x23db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineGlobalMethod

  .method /*06000119*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*0200000D*/ 
          DefineGlobalMethod(string name,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/ attributes,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callingConvention,
                             class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType,
                             class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] requiredReturnTypeCustomModifiers,
                             class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] optionalReturnTypeCustomModifiers,
                             class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] parameterTypes,
                             class [System.Runtime/*23000001*/]System.Type/*0100001C*/[][] requiredParameterTypeCustomModifiers,
                             class [System.Runtime/*23000001*/]System.Type/*0100001C*/[][] optionalParameterTypeCustomModifiers) cil managed
  // SIG: 20 09 12 34 0E 11 80 A9 11 80 AD 12 71 1D 12 71 1D 12 71 1D 12 71 1D 1D 12 71 1D 1D 12 71
  {
    .param [4]/*08000100*/ 
    .custom /*0C0000E6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000101*/ 
    .custom /*0C0000E7:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [6]/*08000102*/ 
    .custom /*0C0000E8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [7]/*08000103*/ 
    .custom /*0C0000E9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [8]/*08000104*/ 
    .custom /*0C0000EB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [9]/*08000105*/ 
    .custom /*0C0000EC:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x23de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineGlobalMethod

  .method /*0600011A*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*0200000D*/ 
          DefineGlobalMethod(string name,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/ attributes,
                             class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType,
                             class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] parameterTypes) cil managed
  // SIG: 20 04 12 34 0E 11 80 A9 12 71 1D 12 71
  {
    .param [3]/*08000108*/ 
    .custom /*0C0000EE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000109*/ 
    .custom /*0C0000EF:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x23e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineGlobalMethod

  .method /*0600011B*/ public hidebysig instance class System.Reflection.Emit.FieldBuilder/*0200000B*/ 
          DefineInitializedData(string name,
                                uint8[] data,
                                valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000042*/ attributes) cil managed
  // SIG: 20 03 12 2C 0E 1D 05 11 81 09
  {
    // Method begins at RVA 0x23e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineInitializedData

  .method /*0600011C*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*0200000D*/ 
          DefinePInvokeMethod(string name,
                              string dllName,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/ attributes,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callingConvention,
                              class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType,
                              class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] parameterTypes,
                              valuetype [System.Runtime.InteropServices/*23000003*/]System.Runtime.InteropServices.CallingConvention/*01000048*/ nativeCallConv,
                              valuetype [System.Runtime/*23000001*/]System.Runtime.InteropServices.CharSet/*01000049*/ nativeCharSet) cil managed
  // SIG: 20 08 12 34 0E 0E 11 80 A9 11 80 AD 12 71 1D 12 71 11 81 21 11 81 25
  {
    .custom /*0C0000F6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 4A 50 2F 49 6E 76 6F 6B 65 20 6D 61 72 73   // ..JP/Invoke mars
                                                                                                                                                                                            68 61 6C 6C 69 6E 67 20 6D 61 79 20 64 79 6E 61   // halling may dyna
                                                                                                                                                                                            6D 69 63 61 6C 6C 79 20 61 63 63 65 73 73 20 6D   // mically access m
                                                                                                                                                                                            65 6D 62 65 72 73 20 74 68 61 74 20 63 6F 75 6C   // embers that coul
                                                                                                                                                                                            64 20 62 65 20 74 72 69 6D 6D 65 64 2E 00 00 )    // d be trimmed...
    .param [5]/*08000111*/ 
    .custom /*0C0000F2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [6]/*08000112*/ 
    .custom /*0C0000F3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x23e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefinePInvokeMethod

  .method /*0600011D*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*0200000D*/ 
          DefinePInvokeMethod(string name,
                              string dllName,
                              string entryName,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/ attributes,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callingConvention,
                              class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType,
                              class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] parameterTypes,
                              valuetype [System.Runtime.InteropServices/*23000003*/]System.Runtime.InteropServices.CallingConvention/*01000048*/ nativeCallConv,
                              valuetype [System.Runtime/*23000001*/]System.Runtime.InteropServices.CharSet/*01000049*/ nativeCharSet) cil managed
  // SIG: 20 09 12 34 0E 0E 0E 11 80 A9 11 80 AD 12 71 1D 12 71 11 81 21 11 81 25
  {
    .custom /*0C0000F7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 4A 50 2F 49 6E 76 6F 6B 65 20 6D 61 72 73   // ..JP/Invoke mars
                                                                                                                                                                                            68 61 6C 6C 69 6E 67 20 6D 61 79 20 64 79 6E 61   // halling may dyna
                                                                                                                                                                                            6D 69 63 61 6C 6C 79 20 61 63 63 65 73 73 20 6D   // mically access m
                                                                                                                                                                                            65 6D 62 65 72 73 20 74 68 61 74 20 63 6F 75 6C   // embers that coul
                                                                                                                                                                                            64 20 62 65 20 74 72 69 6D 6D 65 64 2E 00 00 )    // d be trimmed...
    .param [6]/*0800011A*/ 
    .custom /*0C0000F4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [7]/*0800011B*/ 
    .custom /*0C0000F5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x23ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefinePInvokeMethod

  .method /*0600011E*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*02000010*/ 
          DefineType(string name) cil managed
  // SIG: 20 01 12 40 0E
  {
    // Method begins at RVA 0x23ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineType

  .method /*0600011F*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*02000010*/ 
          DefineType(string name,
                     valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*01000039*/ attr) cil managed
  // SIG: 20 02 12 40 0E 11 80 E5
  {
    // Method begins at RVA 0x23f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineType

  .method /*06000120*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*02000010*/ 
          DefineType(string name,
                     valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*01000039*/ attr,
                     class [System.Runtime/*23000001*/]System.Type/*0100001C*/ parent) cil managed
  // SIG: 20 03 12 40 0E 11 80 E5 12 71
  {
    .param [3]/*08000123*/ 
    .custom /*0C0000F8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000F9:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x23f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineType

  .method /*06000121*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*02000010*/ 
          DefineType(string name,
                     valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*01000039*/ attr,
                     class [System.Runtime/*23000001*/]System.Type/*0100001C*/ parent,
                     int32 typesize) cil managed
  // SIG: 20 04 12 40 0E 11 80 E5 12 71 08
  {
    .param [3]/*08000126*/ 
    .custom /*0C0000FB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000FC:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x23f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineType

  .method /*06000122*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*02000010*/ 
          DefineType(string name,
                     valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*01000039*/ attr,
                     class [System.Runtime/*23000001*/]System.Type/*0100001C*/ parent,
                     valuetype [System.Reflection.Primitives/*23000004*/]System.Reflection.Emit.PackingSize/*0100004A*/ packsize) cil managed
  // SIG: 20 04 12 40 0E 11 80 E5 12 71 11 81 29
  {
    .param [3]/*0800012A*/ 
    .custom /*0C0000FD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000FE:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x23f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineType

  .method /*06000123*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*02000010*/ 
          DefineType(string name,
                     valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*01000039*/ attr,
                     class [System.Runtime/*23000001*/]System.Type/*0100001C*/ parent,
                     valuetype [System.Reflection.Primitives/*23000004*/]System.Reflection.Emit.PackingSize/*0100004A*/ packingSize,
                     int32 typesize) cil managed
  // SIG: 20 05 12 40 0E 11 80 E5 12 71 11 81 29 08
  {
    .param [3]/*0800012E*/ 
    .custom /*0C000103:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000104:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x23fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineType

  .method /*06000124*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*02000010*/ 
          DefineType(string name,
                     valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*01000039*/ attr,
                     class [System.Runtime/*23000001*/]System.Type/*0100001C*/ parent,
                     class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] interfaces) cil managed
  // SIG: 20 04 12 40 0E 11 80 E5 12 71 1D 12 71
  {
    .param [3]/*08000133*/ 
    .custom /*0C000109:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00010A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF FF FF FF 00 00 ) 
    .param [4]/*08000134*/ 
    .custom /*0C00010C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x23ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineType

  .method /*06000125*/ public hidebysig instance class System.Reflection.Emit.FieldBuilder/*0200000B*/ 
          DefineUninitializedData(string name,
                                  int32 size,
                                  valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000042*/ attributes) cil managed
  // SIG: 20 03 12 2C 0E 08 11 81 09
  {
    // Method begins at RVA 0x2402
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineUninitializedData

  .method /*06000126*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000FA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2405
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::Equals

  .method /*06000127*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetArrayMethod(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ arrayClass,
                         string methodName,
                         valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callingConvention,
                         class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType,
                         class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] parameterTypes) cil managed
  // SIG: 20 05 12 59 12 71 0E 11 80 AD 12 71 1D 12 71
  {
    .param [4]/*0800013C*/ 
    .custom /*0C000116:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800013D*/ 
    .custom /*0C000117:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2408
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetArrayMethod

  .method /*06000128*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(bool inherit) cil managed
  // SIG: 20 01 1D 1C 02
  {
    // Method begins at RVA 0x240b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetCustomAttributes

  .method /*06000129*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ attributeType,
                                                 bool inherit) cil managed
  // SIG: 20 02 1D 1C 12 71 02
  {
    // Method begins at RVA 0x240e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetCustomAttributes

  .method /*0600012A*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001D*/<class [System.Runtime/*23000001*/]System.Reflection.CustomAttributeData/*0100001E*/> 
          GetCustomAttributesData() cil managed
  // SIG: 20 00 15 12 75 01 12 79
  {
    // Method begins at RVA 0x2411
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetCustomAttributesData

  .method /*0600012B*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003C*/ 
          GetField(string name,
                   valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 02 12 80 F1 0E 11 80 C9
  {
    .custom /*0C0000FF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 17 46 69 65 6C 64 73 20 6D 69 67 68 74 20   // ...Fields might 
                                                                                                                                                                                            62 65 20 72 65 6D 6F 76 65 64 00 00 )             // be removed..
    .param [0]/*08000141*/ 
    .custom /*0C000118:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2414
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetField

  .method /*0600012C*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003C*/[] 
          GetFields(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingFlags) cil managed
  // SIG: 20 01 1D 12 80 F1 11 80 C9
  {
    .custom /*0C000100:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 17 46 69 65 6C 64 73 20 6D 69 67 68 74 20   // ...Fields might 
                                                                                                                                                                                            62 65 20 72 65 6D 6F 76 65 64 00 00 )             // be removed..
    // Method begins at RVA 0x2417
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetFields

  .method /*0600012D*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x241a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetHashCode

  .method /*0600012E*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetMethodImpl(string name,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr,
                        class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000033*/ binder,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callConvention,
                        class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] types,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003A*/[] modifiers) cil managed
  // SIG: 20 06 12 59 0E 11 80 C9 12 80 CD 11 80 AD 1D 12 71 1D 11 80 E9
  {
    .custom /*0C000101:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000102:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 18 4D 65 74 68 6F 64 73 20 6D 69 67 68 74   // ...Methods might
                                                                                                                                                                                            20 62 65 20 72 65 6D 6F 76 65 64 00 00 )          //  be removed..
    .param [1]/*08000145*/ 
    .custom /*0C00011A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [5]/*08000149*/ 
    .custom /*0C00011C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x241d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetMethodImpl

  .method /*0600012F*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/[] 
          GetMethods(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingFlags) cil managed
  // SIG: 20 01 1D 12 59 11 80 C9
  {
    .custom /*0C000105:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 18 4D 65 74 68 6F 64 73 20 6D 69 67 68 74   // ...Methods might
                                                                                                                                                                                            20 62 65 20 72 65 6D 6F 76 65 64 00 00 )          //  be removed..
    // Method begins at RVA 0x2420
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetMethods

  .method /*06000130*/ public hidebysig virtual 
          instance void  GetPEKind([out] valuetype [System.Runtime/*23000001*/]System.Reflection.PortableExecutableKinds/*0100004B*/& peKind,
                                   [out] valuetype [System.Runtime/*23000001*/]System.Reflection.ImageFileMachine/*0100004C*/& machine) cil managed
  // SIG: 20 02 01 10 11 81 2D 10 11 81 31
  {
    // Method begins at RVA 0x2423
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetPEKind

  .method /*06000131*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          GetType(string className) cil managed
  // SIG: 20 01 12 71 0E
  {
    .custom /*0C000106:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 16 54 79 70 65 73 20 6D 69 67 68 74 20 62   // ...Types might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 00 00 )                // e removed..
    .param [0]/*0800014E*/ 
    .custom /*0C00011F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2426
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetType

  .method /*06000132*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          GetType(string className,
                  bool ignoreCase) cil managed
  // SIG: 20 02 12 71 0E 02
  {
    .custom /*0C000107:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 16 54 79 70 65 73 20 6D 69 67 68 74 20 62   // ...Types might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 00 00 )                // e removed..
    .param [0]/*08000150*/ 
    .custom /*0C000122:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2429
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetType

  .method /*06000133*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          GetType(string className,
                  bool throwOnError,
                  bool ignoreCase) cil managed
  // SIG: 20 03 12 71 0E 02 02
  {
    .custom /*0C000108:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 16 54 79 70 65 73 20 6D 69 67 68 74 20 62   // ...Types might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 00 00 )                // e removed..
    .param [0]/*08000153*/ 
    .custom /*0C000124:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x242c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetType

  .method /*06000134*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] 
          GetTypes() cil managed
  // SIG: 20 00 1D 12 71
  {
    .custom /*0C00010B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 16 54 79 70 65 73 20 6D 69 67 68 74 20 62   // ...Types might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 00 00 )                // e removed..
    // Method begins at RVA 0x242f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetTypes

  .method /*06000135*/ public hidebysig virtual 
          instance bool  IsDefined(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ attributeType,
                                   bool inherit) cil managed
  // SIG: 20 02 02 12 71 02
  {
    // Method begins at RVA 0x2432
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::IsDefined

  .method /*06000136*/ public hidebysig virtual 
          instance bool  IsResource() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2435
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::IsResource

  .method /*06000137*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003C*/ 
          ResolveField(int32 metadataToken,
                       class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] genericTypeArguments,
                       class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] genericMethodArguments) cil managed
  // SIG: 20 03 12 80 F1 08 1D 12 71 1D 12 71
  {
    .custom /*0C00010D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00010E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 20 54 72 69 6D 6D 69 6E 67 20 63 68 61 6E   // .. Trimming chan
                                                                                                                                                                                            67 65 73 20 6D 65 74 61 64 61 74 61 20 74 6F 6B   // ges metadata tok
                                                                                                                                                                                            65 6E 73 00 00 )                                  // ens..
    .param [2]/*0800015A*/ 
    .custom /*0C000127:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [3]/*0800015B*/ 
    .custom /*0C000129:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2438
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::ResolveField

  .method /*06000138*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*0100003E*/ 
          ResolveMember(int32 metadataToken,
                        class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] genericTypeArguments,
                        class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] genericMethodArguments) cil managed
  // SIG: 20 03 12 80 F9 08 1D 12 71 1D 12 71
  {
    .custom /*0C00010F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000110:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 20 54 72 69 6D 6D 69 6E 67 20 63 68 61 6E   // .. Trimming chan
                                                                                                                                                                                            67 65 73 20 6D 65 74 61 64 61 74 61 20 74 6F 6B   // ges metadata tok
                                                                                                                                                                                            65 6E 73 00 00 )                                  // ens..
    .param [2]/*0800015D*/ 
    .custom /*0C00012C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [3]/*0800015E*/ 
    .custom /*0C00012E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x243b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::ResolveMember

  .method /*06000139*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodBase/*01000044*/ 
          ResolveMethod(int32 metadataToken,
                        class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] genericTypeArguments,
                        class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] genericMethodArguments) cil managed
  // SIG: 20 03 12 81 11 08 1D 12 71 1D 12 71
  {
    .custom /*0C000111:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000112:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 20 54 72 69 6D 6D 69 6E 67 20 63 68 61 6E   // .. Trimming chan
                                                                                                                                                                                            67 65 73 20 6D 65 74 61 64 61 74 61 20 74 6F 6B   // ges metadata tok
                                                                                                                                                                                            65 6E 73 00 00 )                                  // ens..
    .param [2]/*08000160*/ 
    .custom /*0C000130:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [3]/*08000161*/ 
    .custom /*0C000131:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x243e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::ResolveMethod

  .method /*0600013A*/ public hidebysig virtual 
          instance uint8[]  ResolveSignature(int32 metadataToken) cil managed
  // SIG: 20 01 1D 05 08
  {
    .custom /*0C000113:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 20 54 72 69 6D 6D 69 6E 67 20 63 68 61 6E   // .. Trimming chan
                                                                                                                                                                                            67 65 73 20 6D 65 74 61 64 61 74 61 20 74 6F 6B   // ges metadata tok
                                                                                                                                                                                            65 6E 73 00 00 )                                  // ens..
    // Method begins at RVA 0x2441
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::ResolveSignature

  .method /*0600013B*/ public hidebysig virtual 
          instance string  ResolveString(int32 metadataToken) cil managed
  // SIG: 20 01 0E 08
  {
    .custom /*0C000114:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 20 54 72 69 6D 6D 69 6E 67 20 63 68 61 6E   // .. Trimming chan
                                                                                                                                                                                            67 65 73 20 6D 65 74 61 64 61 74 61 20 74 6F 6B   // ges metadata tok
                                                                                                                                                                                            65 6E 73 00 00 )                                  // ens..
    // Method begins at RVA 0x2444
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::ResolveString

  .method /*0600013C*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          ResolveType(int32 metadataToken,
                      class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] genericTypeArguments,
                      class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] genericMethodArguments) cil managed
  // SIG: 20 03 12 71 08 1D 12 71 1D 12 71
  {
    .custom /*0C000115:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 20 54 72 69 6D 6D 69 6E 67 20 63 68 61 6E   // .. Trimming chan
                                                                                                                                                                                            67 65 73 20 6D 65 74 61 64 61 74 61 20 74 6F 6B   // ges metadata tok
                                                                                                                                                                                            65 6E 73 00 00 )                                  // ens..
    .param [2]/*08000165*/ 
    .custom /*0C000132:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [3]/*08000166*/ 
    .custom /*0C000133:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2447
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::ResolveType

  .method /*0600013D*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000026*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 99 1D 05
  {
    // Method begins at RVA 0x244a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ModuleBuilder::SetCustomAttribute

  .method /*0600013E*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001A*/ customBuilder) cil managed
  // SIG: 20 01 01 12 69
  {
    // Method begins at RVA 0x244c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ModuleBuilder::SetCustomAttribute

  .property /*17000056*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/
          Assembly()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ System.Reflection.Emit.ModuleBuilder/*0200000E*/::get_Assembly() /* 0600010F */
  } // end of property ModuleBuilder::Assembly
  .property /*17000057*/ instance callconv(8) string
          FullyQualifiedName()
  {
    .get instance string System.Reflection.Emit.ModuleBuilder/*0200000E*/::get_FullyQualifiedName() /* 06000110 */
  } // end of property ModuleBuilder::FullyQualifiedName
  .property /*17000058*/ instance callconv(8) int32
          MDStreamVersion()
  {
    .get instance int32 System.Reflection.Emit.ModuleBuilder/*0200000E*/::get_MDStreamVersion() /* 06000111 */
  } // end of property ModuleBuilder::MDStreamVersion
  .property /*17000059*/ instance callconv(8) int32
          MetadataToken()
  {
    .get instance int32 System.Reflection.Emit.ModuleBuilder/*0200000E*/::get_MetadataToken() /* 06000112 */
  } // end of property ModuleBuilder::MetadataToken
  .property /*1700005A*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/
          ModuleVersionId()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/ System.Reflection.Emit.ModuleBuilder/*0200000E*/::get_ModuleVersionId() /* 06000113 */
  } // end of property ModuleBuilder::ModuleVersionId
  .property /*1700005B*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Reflection.Emit.ModuleBuilder/*0200000E*/::get_Name() /* 06000114 */
  } // end of property ModuleBuilder::Name
  .property /*1700005C*/ instance callconv(8) string
          ScopeName()
  {
    .get instance string System.Reflection.Emit.ModuleBuilder/*0200000E*/::get_ScopeName() /* 06000115 */
  } // end of property ModuleBuilder::ScopeName
} // end of class System.Reflection.Emit.ModuleBuilder

.class /*0200000F*/ public auto ansi sealed beforefieldinit System.Reflection.Emit.PropertyBuilder
       extends [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000040*/
{
  .custom /*0C00003B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600013F*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x244e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000040*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PropertyBuilder::.ctor

  .method /*06000140*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.PropertyAttributes/*0100004D*/ 
          get_Attributes() cil managed
  // SIG: 20 00 11 81 35
  {
    // Method begins at RVA 0x2456
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::get_Attributes

  .method /*06000141*/ public hidebysig specialname virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2459
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::get_CanRead

  .method /*06000142*/ public hidebysig specialname virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x245c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::get_CanWrite

  .method /*06000143*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_DeclaringType() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C000119:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x245f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::get_DeclaringType

  .method /*06000144*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ 
          get_Module() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x2462
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::get_Module

  .method /*06000145*/ public hidebysig specialname virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2465
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::get_Name

  .method /*06000146*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_PropertyType() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x2468
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::get_PropertyType

  .method /*06000147*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_ReflectedType() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C00011B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x246b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::get_ReflectedType

  .method /*06000148*/ public hidebysig instance void 
          AddOtherMethod(class System.Reflection.Emit.MethodBuilder/*0200000D*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 34
  {
    // Method begins at RVA 0x246e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PropertyBuilder::AddOtherMethod

  .method /*06000149*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/[] 
          GetAccessors(bool nonPublic) cil managed
  // SIG: 20 01 1D 12 59 02
  {
    // Method begins at RVA 0x2470
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::GetAccessors

  .method /*0600014A*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(bool inherit) cil managed
  // SIG: 20 01 1D 1C 02
  {
    // Method begins at RVA 0x2473
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::GetCustomAttributes

  .method /*0600014B*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ attributeType,
                                                 bool inherit) cil managed
  // SIG: 20 02 1D 1C 12 71 02
  {
    // Method begins at RVA 0x2476
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::GetCustomAttributes

  .method /*0600014C*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetGetMethod(bool nonPublic) cil managed
  // SIG: 20 01 12 59 02
  {
    .custom /*0C00011D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2479
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::GetGetMethod

  .method /*0600014D*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*01000031*/[] 
          GetIndexParameters() cil managed
  // SIG: 20 00 1D 12 80 C5
  {
    // Method begins at RVA 0x247c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::GetIndexParameters

  .method /*0600014E*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetSetMethod(bool nonPublic) cil managed
  // SIG: 20 01 12 59 02
  {
    .custom /*0C00011E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x247f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::GetSetMethod

  .method /*0600014F*/ public hidebysig virtual 
          instance object  GetValue(object obj,
                                    object[] index) cil managed
  // SIG: 20 02 1C 1C 1D 1C
  {
    .custom /*0C000120:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000171*/ 
    .custom /*0C000135:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2482
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::GetValue

  .method /*06000150*/ public hidebysig virtual 
          instance object  GetValue(object obj,
                                    valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ invokeAttr,
                                    class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000033*/ binder,
                                    object[] index,
                                    class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000024*/ culture) cil managed
  // SIG: 20 05 1C 1C 11 80 C9 12 80 CD 1D 1C 12 80 91
  {
    .custom /*0C000121:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000174*/ 
    .custom /*0C000137:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2485
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::GetValue

  .method /*06000151*/ public hidebysig virtual 
          instance bool  IsDefined(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ attributeType,
                                   bool inherit) cil managed
  // SIG: 20 02 02 12 71 02
  {
    // Method begins at RVA 0x2488
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::IsDefined

  .method /*06000152*/ public hidebysig instance void 
          SetConstant(object defaultValue) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C000123:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x248b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PropertyBuilder::SetConstant

  .method /*06000153*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000026*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 99 1D 05
  {
    // Method begins at RVA 0x248d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PropertyBuilder::SetCustomAttribute

  .method /*06000154*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001A*/ customBuilder) cil managed
  // SIG: 20 01 01 12 69
  {
    // Method begins at RVA 0x248f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PropertyBuilder::SetCustomAttribute

  .method /*06000155*/ public hidebysig instance void 
          SetGetMethod(class System.Reflection.Emit.MethodBuilder/*0200000D*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 34
  {
    // Method begins at RVA 0x2491
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PropertyBuilder::SetGetMethod

  .method /*06000156*/ public hidebysig instance void 
          SetSetMethod(class System.Reflection.Emit.MethodBuilder/*0200000D*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 34
  {
    // Method begins at RVA 0x2493
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PropertyBuilder::SetSetMethod

  .method /*06000157*/ public hidebysig virtual 
          instance void  SetValue(object obj,
                                  object 'value',
                                  object[] index) cil managed
  // SIG: 20 03 01 1C 1C 1D 1C
  {
    .custom /*0C000125:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2495
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PropertyBuilder::SetValue

  .method /*06000158*/ public hidebysig virtual 
          instance void  SetValue(object obj,
                                  object 'value',
                                  valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ invokeAttr,
                                  class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000033*/ binder,
                                  object[] index,
                                  class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000024*/ culture) cil managed
  // SIG: 20 06 01 1C 1C 11 80 C9 12 80 CD 1D 1C 12 80 91
  {
    .custom /*0C000126:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2497
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PropertyBuilder::SetValue

  .property /*1700005D*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Reflection.PropertyAttributes/*0100004D*/
          Attributes()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Reflection.PropertyAttributes/*0100004D*/ System.Reflection.Emit.PropertyBuilder/*0200000F*/::get_Attributes() /* 06000140 */
  } // end of property PropertyBuilder::Attributes
  .property /*1700005E*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool System.Reflection.Emit.PropertyBuilder/*0200000F*/::get_CanRead() /* 06000141 */
  } // end of property PropertyBuilder::CanRead
  .property /*1700005F*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool System.Reflection.Emit.PropertyBuilder/*0200000F*/::get_CanWrite() /* 06000142 */
  } // end of property PropertyBuilder::CanWrite
  .property /*17000060*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          DeclaringType()
  {
    .custom /*0C00007B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.PropertyBuilder/*0200000F*/::get_DeclaringType() /* 06000143 */
  } // end of property PropertyBuilder::DeclaringType
  .property /*17000061*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/
          Module()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ System.Reflection.Emit.PropertyBuilder/*0200000F*/::get_Module() /* 06000144 */
  } // end of property PropertyBuilder::Module
  .property /*17000062*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Reflection.Emit.PropertyBuilder/*0200000F*/::get_Name() /* 06000145 */
  } // end of property PropertyBuilder::Name
  .property /*17000063*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          PropertyType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.PropertyBuilder/*0200000F*/::get_PropertyType() /* 06000146 */
  } // end of property PropertyBuilder::PropertyType
  .property /*17000064*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          ReflectedType()
  {
    .custom /*0C000082:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.PropertyBuilder/*0200000F*/::get_ReflectedType() /* 06000147 */
  } // end of property PropertyBuilder::ReflectedType
} // end of class System.Reflection.Emit.PropertyBuilder

.class /*02000010*/ public auto ansi sealed beforefieldinit System.Reflection.Emit.TypeBuilder
       extends [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000034*/
{
  .custom /*0C00003D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000007*/ public static literal int32 UnspecifiedTypeSize = int32(0x00000000)
  .method /*06000159*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2499
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000034*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TypeBuilder::.ctor

  .method /*0600015A*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ 
          get_Assembly() cil managed
  // SIG: 20 00 12 55
  {
    // Method begins at RVA 0x24a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_Assembly

  .method /*0600015B*/ public hidebysig specialname virtual 
          instance string  get_AssemblyQualifiedName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000128:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_AssemblyQualifiedName

  .method /*0600015C*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_BaseType() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C00012A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_BaseType

  .method /*0600015D*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodBase/*01000044*/ 
          get_DeclaringMethod() cil managed
  // SIG: 20 00 12 81 11
  {
    .custom /*0C00012B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_DeclaringMethod

  .method /*0600015E*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_DeclaringType() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C00012D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_DeclaringType

  .method /*0600015F*/ public hidebysig specialname virtual 
          instance string  get_FullName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00012F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_FullName

  .method /*06000160*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.GenericParameterAttributes/*01000045*/ 
          get_GenericParameterAttributes() cil managed
  // SIG: 20 00 11 81 15
  {
    // Method begins at RVA 0x24b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_GenericParameterAttributes

  .method /*06000161*/ public hidebysig specialname virtual 
          instance int32  get_GenericParameterPosition() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x24b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_GenericParameterPosition

  .method /*06000162*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/ 
          get_GUID() cil managed
  // SIG: 20 00 11 80 D5
  {
    // Method begins at RVA 0x24b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_GUID

  .method /*06000163*/ public hidebysig specialname virtual 
          instance bool  get_IsByRefLike() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_IsByRefLike

  .method /*06000164*/ public hidebysig specialname virtual 
          instance bool  get_IsConstructedGenericType() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_IsConstructedGenericType

  .method /*06000165*/ public hidebysig specialname virtual 
          instance bool  get_IsGenericParameter() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_IsGenericParameter

  .method /*06000166*/ public hidebysig specialname virtual 
          instance bool  get_IsGenericType() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_IsGenericType

  .method /*06000167*/ public hidebysig specialname virtual 
          instance bool  get_IsGenericTypeDefinition() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_IsGenericTypeDefinition

  .method /*06000168*/ public hidebysig specialname virtual 
          instance bool  get_IsSecurityCritical() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_IsSecurityCritical

  .method /*06000169*/ public hidebysig specialname virtual 
          instance bool  get_IsSecuritySafeCritical() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_IsSecuritySafeCritical

  .method /*0600016A*/ public hidebysig specialname virtual 
          instance bool  get_IsSecurityTransparent() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_IsSecurityTransparent

  .method /*0600016B*/ public hidebysig specialname virtual 
          instance bool  get_IsSZArray() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_IsSZArray

  .method /*0600016C*/ public hidebysig specialname virtual 
          instance bool  get_IsTypeDefinition() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_IsTypeDefinition

  .method /*0600016D*/ public hidebysig specialname virtual 
          instance int32  get_MetadataToken() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x24da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_MetadataToken

  .method /*0600016E*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ 
          get_Module() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x24dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_Module

  .method /*0600016F*/ public hidebysig specialname virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x24e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_Name

  .method /*06000170*/ public hidebysig specialname virtual 
          instance string  get_Namespace() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000134:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_Namespace

  .method /*06000171*/ public hidebysig specialname 
          instance valuetype [System.Reflection.Primitives/*23000004*/]System.Reflection.Emit.PackingSize/*0100004A*/ 
          get_PackingSize() cil managed
  // SIG: 20 00 11 81 29
  {
    // Method begins at RVA 0x24e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_PackingSize

  .method /*06000172*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_ReflectedType() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C000136:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_ReflectedType

  .method /*06000173*/ public hidebysig specialname 
          instance int32  get_Size() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x24ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_Size

  .method /*06000174*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000036*/ 
          get_TypeHandle() cil managed
  // SIG: 20 00 11 80 D9
  {
    // Method begins at RVA 0x24ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_TypeHandle

  .method /*06000175*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          get_UnderlyingSystemType() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x24f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_UnderlyingSystemType

  .method /*06000176*/ public hidebysig instance void 
          AddInterfaceImplementation(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ interfaceType) cil managed
  // SIG: 20 01 01 12 71
  {
    .param [1]/*0800018B*/ 
    .custom /*0C00013A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x24f5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TypeBuilder::AddInterfaceImplementation

  .method /*06000177*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          CreateType() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C000138:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800018C*/ 
    .custom /*0C00013B:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x24f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::CreateType

  .method /*06000178*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000034*/ 
          CreateTypeInfo() cil managed
  // SIG: 20 00 12 80 D1
  {
    .custom /*0C000139:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800018D*/ 
    .custom /*0C00013C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x24fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::CreateTypeInfo

  .method /*06000179*/ public hidebysig instance class System.Reflection.Emit.ConstructorBuilder/*02000008*/ 
          DefineConstructor(valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/ attributes,
                            valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callingConvention,
                            class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] parameterTypes) cil managed
  // SIG: 20 03 12 20 11 80 A9 11 80 AD 1D 12 71
  {
    .param [3]/*08000190*/ 
    .custom /*0C000140:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x24fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineConstructor

  .method /*0600017A*/ public hidebysig instance class System.Reflection.Emit.ConstructorBuilder/*02000008*/ 
          DefineConstructor(valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/ attributes,
                            valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callingConvention,
                            class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] parameterTypes,
                            class [System.Runtime/*23000001*/]System.Type/*0100001C*/[][] requiredCustomModifiers,
                            class [System.Runtime/*23000001*/]System.Type/*0100001C*/[][] optionalCustomModifiers) cil managed
  // SIG: 20 05 12 20 11 80 A9 11 80 AD 1D 12 71 1D 1D 12 71 1D 1D 12 71
  {
    .param [3]/*08000193*/ 
    .custom /*0C000141:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [4]/*08000194*/ 
    .custom /*0C000142:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [5]/*08000195*/ 
    .custom /*0C000143:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x2500
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineConstructor

  .method /*0600017B*/ public hidebysig instance class System.Reflection.Emit.ConstructorBuilder/*02000008*/ 
          DefineDefaultConstructor(valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/ attributes) cil managed
  // SIG: 20 01 12 20 11 80 A9
  {
    // Method begins at RVA 0x2503
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineDefaultConstructor

  .method /*0600017C*/ public hidebysig instance class System.Reflection.Emit.EventBuilder/*0200000A*/ 
          DefineEvent(string name,
                      valuetype [System.Runtime/*23000001*/]System.Reflection.EventAttributes/*0100004E*/ attributes,
                      class [System.Runtime/*23000001*/]System.Type/*0100001C*/ eventtype) cil managed
  // SIG: 20 03 12 28 0E 11 81 39 12 71
  {
    // Method begins at RVA 0x2506
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineEvent

  .method /*0600017D*/ public hidebysig instance class System.Reflection.Emit.FieldBuilder/*0200000B*/ 
          DefineField(string fieldName,
                      class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                      valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000042*/ attributes) cil managed
  // SIG: 20 03 12 2C 0E 12 71 11 81 09
  {
    // Method begins at RVA 0x2509
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineField

  .method /*0600017E*/ public hidebysig instance class System.Reflection.Emit.FieldBuilder/*0200000B*/ 
          DefineField(string fieldName,
                      class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                      class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] requiredCustomModifiers,
                      class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] optionalCustomModifiers,
                      valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000042*/ attributes) cil managed
  // SIG: 20 05 12 2C 0E 12 71 1D 12 71 1D 12 71 11 81 09
  {
    .param [3]/*0800019F*/ 
    .custom /*0C000149:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [4]/*080001A0*/ 
    .custom /*0C00014B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x250c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineField

  .method /*0600017F*/ public hidebysig instance class System.Reflection.Emit.GenericTypeParameterBuilder/*0200000C*/[] 
          DefineGenericParameters(string[] names) cil managed
  // SIG: 20 01 1D 12 30 1D 0E
  {
    .param [1]/*080001A2*/ 
    .custom /*0C00014D:0A000018*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000046*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x250f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineGenericParameters

  .method /*06000180*/ public hidebysig instance class System.Reflection.Emit.FieldBuilder/*0200000B*/ 
          DefineInitializedData(string name,
                                uint8[] data,
                                valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000042*/ attributes) cil managed
  // SIG: 20 03 12 2C 0E 1D 05 11 81 09
  {
    // Method begins at RVA 0x2512
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineInitializedData

  .method /*06000181*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*0200000D*/ 
          DefineMethod(string name,
                       valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/ attributes) cil managed
  // SIG: 20 02 12 34 0E 11 80 A9
  {
    // Method begins at RVA 0x2515
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineMethod

  .method /*06000182*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*0200000D*/ 
          DefineMethod(string name,
                       valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/ attributes,
                       valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callingConvention) cil managed
  // SIG: 20 03 12 34 0E 11 80 A9 11 80 AD
  {
    // Method begins at RVA 0x2518
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineMethod

  .method /*06000183*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*0200000D*/ 
          DefineMethod(string name,
                       valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/ attributes,
                       valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callingConvention,
                       class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType,
                       class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] parameterTypes) cil managed
  // SIG: 20 05 12 34 0E 11 80 A9 11 80 AD 12 71 1D 12 71
  {
    .param [4]/*080001AE*/ 
    .custom /*0C000157:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080001AF*/ 
    .custom /*0C000159:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x251b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineMethod

  .method /*06000184*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*0200000D*/ 
          DefineMethod(string name,
                       valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/ attributes,
                       valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callingConvention,
                       class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType,
                       class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] returnTypeRequiredCustomModifiers,
                       class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] returnTypeOptionalCustomModifiers,
                       class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] parameterTypes,
                       class [System.Runtime/*23000001*/]System.Type/*0100001C*/[][] parameterTypeRequiredCustomModifiers,
                       class [System.Runtime/*23000001*/]System.Type/*0100001C*/[][] parameterTypeOptionalCustomModifiers) cil managed
  // SIG: 20 09 12 34 0E 11 80 A9 11 80 AD 12 71 1D 12 71 1D 12 71 1D 12 71 1D 1D 12 71 1D 1D 12 71
  {
    .param [4]/*080001B3*/ 
    .custom /*0C00015F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080001B4*/ 
    .custom /*0C000160:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [6]/*080001B5*/ 
    .custom /*0C000162:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [7]/*080001B6*/ 
    .custom /*0C000164:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [8]/*080001B7*/ 
    .custom /*0C000165:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [9]/*080001B8*/ 
    .custom /*0C000166:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x251e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineMethod

  .method /*06000185*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*0200000D*/ 
          DefineMethod(string name,
                       valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/ attributes,
                       class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType,
                       class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] parameterTypes) cil managed
  // SIG: 20 04 12 34 0E 11 80 A9 12 71 1D 12 71
  {
    .param [3]/*080001BB*/ 
    .custom /*0C000167:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080001BC*/ 
    .custom /*0C000168:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2521
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineMethod

  .method /*06000186*/ public hidebysig instance void 
          DefineMethodOverride(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ methodInfoBody,
                               class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ methodInfoDeclaration) cil managed
  // SIG: 20 02 01 12 59 12 59
  {
    // Method begins at RVA 0x2524
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TypeBuilder::DefineMethodOverride

  .method /*06000187*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*02000010*/ 
          DefineNestedType(string name) cil managed
  // SIG: 20 01 12 40 0E
  {
    // Method begins at RVA 0x2526
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineNestedType

  .method /*06000188*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*02000010*/ 
          DefineNestedType(string name,
                           valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*01000039*/ attr) cil managed
  // SIG: 20 02 12 40 0E 11 80 E5
  {
    // Method begins at RVA 0x2529
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineNestedType

  .method /*06000189*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*02000010*/ 
          DefineNestedType(string name,
                           valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*01000039*/ attr,
                           class [System.Runtime/*23000001*/]System.Type/*0100001C*/ parent) cil managed
  // SIG: 20 03 12 40 0E 11 80 E5 12 71
  {
    .param [3]/*080001C4*/ 
    .custom /*0C00016A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00016B:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x252c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineNestedType

  .method /*0600018A*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*02000010*/ 
          DefineNestedType(string name,
                           valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*01000039*/ attr,
                           class [System.Runtime/*23000001*/]System.Type/*0100001C*/ parent,
                           int32 typeSize) cil managed
  // SIG: 20 04 12 40 0E 11 80 E5 12 71 08
  {
    .param [3]/*080001C7*/ 
    .custom /*0C00016D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00016E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x252f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineNestedType

  .method /*0600018B*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*02000010*/ 
          DefineNestedType(string name,
                           valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*01000039*/ attr,
                           class [System.Runtime/*23000001*/]System.Type/*0100001C*/ parent,
                           valuetype [System.Reflection.Primitives/*23000004*/]System.Reflection.Emit.PackingSize/*0100004A*/ packSize) cil managed
  // SIG: 20 04 12 40 0E 11 80 E5 12 71 11 81 29
  {
    .param [3]/*080001CB*/ 
    .custom /*0C00016F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000170:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x2532
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineNestedType

  .method /*0600018C*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*02000010*/ 
          DefineNestedType(string name,
                           valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*01000039*/ attr,
                           class [System.Runtime/*23000001*/]System.Type/*0100001C*/ parent,
                           valuetype [System.Reflection.Primitives/*23000004*/]System.Reflection.Emit.PackingSize/*0100004A*/ packSize,
                           int32 typeSize) cil managed
  // SIG: 20 05 12 40 0E 11 80 E5 12 71 11 81 29 08
  {
    .param [3]/*080001CF*/ 
    .custom /*0C000171:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000172:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x2535
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineNestedType

  .method /*0600018D*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*02000010*/ 
          DefineNestedType(string name,
                           valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*01000039*/ attr,
                           class [System.Runtime/*23000001*/]System.Type/*0100001C*/ parent,
                           class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] interfaces) cil managed
  // SIG: 20 04 12 40 0E 11 80 E5 12 71 1D 12 71
  {
    .param [3]/*080001D4*/ 
    .custom /*0C000173:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000174:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF FF FF FF 00 00 ) 
    .param [4]/*080001D5*/ 
    .custom /*0C000175:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2538
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineNestedType

  .method /*0600018E*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*0200000D*/ 
          DefinePInvokeMethod(string name,
                              string dllName,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/ attributes,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callingConvention,
                              class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType,
                              class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] parameterTypes,
                              valuetype [System.Runtime.InteropServices/*23000003*/]System.Runtime.InteropServices.CallingConvention/*01000048*/ nativeCallConv,
                              valuetype [System.Runtime/*23000001*/]System.Runtime.InteropServices.CharSet/*01000049*/ nativeCharSet) cil managed
  // SIG: 20 08 12 34 0E 0E 11 80 A9 11 80 AD 12 71 1D 12 71 11 81 21 11 81 25
  {
    .custom /*0C00013D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 4A 50 2F 49 6E 76 6F 6B 65 20 6D 61 72 73   // ..JP/Invoke mars
                                                                                                                                                                                            68 61 6C 6C 69 6E 67 20 6D 61 79 20 64 79 6E 61   // halling may dyna
                                                                                                                                                                                            6D 69 63 61 6C 6C 79 20 61 63 63 65 73 73 20 6D   // mically access m
                                                                                                                                                                                            65 6D 62 65 72 73 20 74 68 61 74 20 63 6F 75 6C   // embers that coul
                                                                                                                                                                                            64 20 62 65 20 74 72 69 6D 6D 65 64 2E 00 00 )    // d be trimmed...
    .param [5]/*080001DA*/ 
    .custom /*0C000176:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [6]/*080001DB*/ 
    .custom /*0C000177:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x253b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefinePInvokeMethod

  .method /*0600018F*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*0200000D*/ 
          DefinePInvokeMethod(string name,
                              string dllName,
                              string entryName,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/ attributes,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callingConvention,
                              class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType,
                              class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] parameterTypes,
                              valuetype [System.Runtime.InteropServices/*23000003*/]System.Runtime.InteropServices.CallingConvention/*01000048*/ nativeCallConv,
                              valuetype [System.Runtime/*23000001*/]System.Runtime.InteropServices.CharSet/*01000049*/ nativeCharSet) cil managed
  // SIG: 20 09 12 34 0E 0E 0E 11 80 A9 11 80 AD 12 71 1D 12 71 11 81 21 11 81 25
  {
    .custom /*0C00013E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 4A 50 2F 49 6E 76 6F 6B 65 20 6D 61 72 73   // ..JP/Invoke mars
                                                                                                                                                                                            68 61 6C 6C 69 6E 67 20 6D 61 79 20 64 79 6E 61   // halling may dyna
                                                                                                                                                                                            6D 69 63 61 6C 6C 79 20 61 63 63 65 73 73 20 6D   // mically access m
                                                                                                                                                                                            65 6D 62 65 72 73 20 74 68 61 74 20 63 6F 75 6C   // embers that coul
                                                                                                                                                                                            64 20 62 65 20 74 72 69 6D 6D 65 64 2E 00 00 )    // d be trimmed...
    .param [6]/*080001E3*/ 
    .custom /*0C000178:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [7]/*080001E4*/ 
    .custom /*0C000179:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x253e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefinePInvokeMethod

  .method /*06000190*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*0200000D*/ 
          DefinePInvokeMethod(string name,
                              string dllName,
                              string entryName,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002A*/ attributes,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callingConvention,
                              class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType,
                              class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] returnTypeRequiredCustomModifiers,
                              class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] returnTypeOptionalCustomModifiers,
                              class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] parameterTypes,
                              class [System.Runtime/*23000001*/]System.Type/*0100001C*/[][] parameterTypeRequiredCustomModifiers,
                              class [System.Runtime/*23000001*/]System.Type/*0100001C*/[][] parameterTypeOptionalCustomModifiers,
                              valuetype [System.Runtime.InteropServices/*23000003*/]System.Runtime.InteropServices.CallingConvention/*01000048*/ nativeCallConv,
                              valuetype [System.Runtime/*23000001*/]System.Runtime.InteropServices.CharSet/*01000049*/ nativeCharSet) cil managed
  // SIG: 20 0D 12 34 0E 0E 0E 11 80 A9 11 80 AD 12 71 1D 12 71 1D 12 71 1D 12 71 1D 1D 12 71 1D 1D 12 71
  //      11 81 21 11 81 25
  {
    .custom /*0C00013F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 4A 50 2F 49 6E 76 6F 6B 65 20 6D 61 72 73   // ..JP/Invoke mars
                                                                                                                                                                                            68 61 6C 6C 69 6E 67 20 6D 61 79 20 64 79 6E 61   // halling may dyna
                                                                                                                                                                                            6D 69 63 61 6C 6C 79 20 61 63 63 65 73 73 20 6D   // mically access m
                                                                                                                                                                                            65 6D 62 65 72 73 20 74 68 61 74 20 63 6F 75 6C   // embers that coul
                                                                                                                                                                                            64 20 62 65 20 74 72 69 6D 6D 65 64 2E 00 00 )    // d be trimmed...
    .param [6]/*080001EC*/ 
    .custom /*0C00017A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [7]/*080001ED*/ 
    .custom /*0C00017B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [8]/*080001EE*/ 
    .custom /*0C00017C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [9]/*080001EF*/ 
    .custom /*0C00017D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [10]/*080001F0*/ 
    .custom /*0C00017E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [11]/*080001F1*/ 
    .custom /*0C00017F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x2541
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefinePInvokeMethod

  .method /*06000191*/ public hidebysig instance class System.Reflection.Emit.PropertyBuilder/*0200000F*/ 
          DefineProperty(string name,
                         valuetype [System.Runtime/*23000001*/]System.Reflection.PropertyAttributes/*0100004D*/ attributes,
                         valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callingConvention,
                         class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType,
                         class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] parameterTypes) cil managed
  // SIG: 20 05 12 3C 0E 11 81 35 11 80 AD 12 71 1D 12 71
  {
    .param [5]/*080001F8*/ 
    .custom /*0C000180:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2544
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineProperty

  .method /*06000192*/ public hidebysig instance class System.Reflection.Emit.PropertyBuilder/*0200000F*/ 
          DefineProperty(string name,
                         valuetype [System.Runtime/*23000001*/]System.Reflection.PropertyAttributes/*0100004D*/ attributes,
                         valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callingConvention,
                         class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType,
                         class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] returnTypeRequiredCustomModifiers,
                         class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] returnTypeOptionalCustomModifiers,
                         class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] parameterTypes,
                         class [System.Runtime/*23000001*/]System.Type/*0100001C*/[][] parameterTypeRequiredCustomModifiers,
                         class [System.Runtime/*23000001*/]System.Type/*0100001C*/[][] parameterTypeOptionalCustomModifiers) cil managed
  // SIG: 20 09 12 3C 0E 11 81 35 11 80 AD 12 71 1D 12 71 1D 12 71 1D 12 71 1D 1D 12 71 1D 1D 12 71
  {
    .param [5]/*080001FD*/ 
    .custom /*0C000181:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [6]/*080001FE*/ 
    .custom /*0C000182:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [7]/*080001FF*/ 
    .custom /*0C000183:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [8]/*08000200*/ 
    .custom /*0C000184:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [9]/*08000201*/ 
    .custom /*0C000185:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x2547
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineProperty

  .method /*06000193*/ public hidebysig instance class System.Reflection.Emit.PropertyBuilder/*0200000F*/ 
          DefineProperty(string name,
                         valuetype [System.Runtime/*23000001*/]System.Reflection.PropertyAttributes/*0100004D*/ attributes,
                         class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType,
                         class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] parameterTypes) cil managed
  // SIG: 20 04 12 3C 0E 11 81 35 12 71 1D 12 71
  {
    .param [4]/*08000205*/ 
    .custom /*0C000186:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x254a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineProperty

  .method /*06000194*/ public hidebysig instance class System.Reflection.Emit.PropertyBuilder/*0200000F*/ 
          DefineProperty(string name,
                         valuetype [System.Runtime/*23000001*/]System.Reflection.PropertyAttributes/*0100004D*/ attributes,
                         class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType,
                         class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] returnTypeRequiredCustomModifiers,
                         class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] returnTypeOptionalCustomModifiers,
                         class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] parameterTypes,
                         class [System.Runtime/*23000001*/]System.Type/*0100001C*/[][] parameterTypeRequiredCustomModifiers,
                         class [System.Runtime/*23000001*/]System.Type/*0100001C*/[][] parameterTypeOptionalCustomModifiers) cil managed
  // SIG: 20 08 12 3C 0E 11 81 35 12 71 1D 12 71 1D 12 71 1D 12 71 1D 1D 12 71 1D 1D 12 71
  {
    .param [4]/*08000209*/ 
    .custom /*0C000187:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [5]/*0800020A*/ 
    .custom /*0C000188:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [6]/*0800020B*/ 
    .custom /*0C000189:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [7]/*0800020C*/ 
    .custom /*0C00018A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [8]/*0800020D*/ 
    .custom /*0C00018B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x254d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineProperty

  .method /*06000195*/ public hidebysig instance class System.Reflection.Emit.ConstructorBuilder/*02000008*/ 
          DefineTypeInitializer() cil managed
  // SIG: 20 00 12 20
  {
    // Method begins at RVA 0x2550
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineTypeInitializer

  .method /*06000196*/ public hidebysig instance class System.Reflection.Emit.FieldBuilder/*0200000B*/ 
          DefineUninitializedData(string name,
                                  int32 size,
                                  valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000042*/ attributes) cil managed
  // SIG: 20 03 12 2C 0E 08 11 81 09
  {
    // Method begins at RVA 0x2553
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineUninitializedData

  .method /*06000197*/ family hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*01000039*/ 
          GetAttributeFlagsImpl() cil managed
  // SIG: 20 00 11 80 E5
  {
    // Method begins at RVA 0x2556
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetAttributeFlagsImpl

  .method /*06000198*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000026*/ 
          GetConstructor(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                         class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000026*/ constructor) cil managed
  // SIG: 00 02 12 80 99 12 71 12 80 99
  {
    // Method begins at RVA 0x2559
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetConstructor

  .method /*06000199*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000026*/ 
          GetConstructorImpl(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr,
                             class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000033*/ binder,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callConvention,
                             class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] types,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003A*/[] modifiers) cil managed
  // SIG: 20 05 12 80 99 11 80 C9 12 80 CD 11 80 AD 1D 12 71 1D 11 80 E9
  {
    .custom /*0C000144:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000145:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 07 00 00 00 00 00 ) 
    .param [4]/*08000216*/ 
    .custom /*0C00018C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x255c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetConstructorImpl

  .method /*0600019A*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000026*/[] 
          GetConstructors(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 99 11 80 C9
  {
    .custom /*0C000146:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 07 00 00 00 00 00 ) 
    // Method begins at RVA 0x255f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetConstructors

  .method /*0600019B*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(bool inherit) cil managed
  // SIG: 20 01 1D 1C 02
  {
    // Method begins at RVA 0x2562
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetCustomAttributes

  .method /*0600019C*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ attributeType,
                                                 bool inherit) cil managed
  // SIG: 20 02 1D 1C 12 71 02
  {
    // Method begins at RVA 0x2565
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetCustomAttributes

  .method /*0600019D*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          GetElementType() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x2568
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetElementType

  .method /*0600019E*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100003B*/ 
          GetEvent(string name,
                   valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 02 12 80 ED 0E 11 80 C9
  {
    .custom /*0C000147:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 18 00 00 00 00 ) 
    .param [0]/*0800021C*/ 
    .custom /*0C00018D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x256b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetEvent

  .method /*0600019F*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100003B*/[] 
          GetEvents() cil managed
  // SIG: 20 00 1D 12 80 ED
  {
    .custom /*0C000148:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 08 00 00 00 00 ) 
    // Method begins at RVA 0x256e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetEvents

  .method /*060001A0*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100003B*/[] 
          GetEvents(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 ED 11 80 C9
  {
    .custom /*0C00014A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 18 00 00 00 00 ) 
    // Method begins at RVA 0x2571
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetEvents

  .method /*060001A1*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003C*/ 
          GetField(string name,
                   valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 02 12 80 F1 0E 11 80 C9
  {
    .custom /*0C00014C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 60 00 00 00 00 00 )                         // ..`.....
    .param [0]/*08000220*/ 
    .custom /*0C00018E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2574
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetField

  .method /*060001A2*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003C*/ 
          GetField(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                   class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003C*/ 'field') cil managed
  // SIG: 00 02 12 80 F1 12 71 12 80 F1
  {
    // Method begins at RVA 0x2577
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetField

  .method /*060001A3*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003C*/[] 
          GetFields(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 F1 11 80 C9
  {
    .custom /*0C00014E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 60 00 00 00 00 00 )                         // ..`.....
    // Method begins at RVA 0x257a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetFields

  .method /*060001A4*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] 
          GetGenericArguments() cil managed
  // SIG: 20 00 1D 12 71
  {
    // Method begins at RVA 0x257d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetGenericArguments

  .method /*060001A5*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          GetGenericTypeDefinition() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x2580
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetGenericTypeDefinition

  .method /*060001A6*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          GetInterface(string name,
                       bool ignoreCase) cil managed
  // SIG: 20 02 12 71 0E 02
  {
    .custom /*0C00014F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 20 00 00 00 00 )                         // ... ....
    .param [0]/*08000226*/ 
    .custom /*0C00018F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000190:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 20 00 00 00 00 )                         // ... ....
    // Method begins at RVA 0x2583
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetInterface

  .method /*060001A7*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.InterfaceMapping/*0100003D*/ 
          GetInterfaceMap(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ interfaceType) cil managed
  // SIG: 20 01 11 80 F5 12 71
  {
    .param [1]/*08000229*/ 
    .custom /*0C000191:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 18 00 00 00 00 00 ) 
    // Method begins at RVA 0x2586
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetInterfaceMap

  .method /*060001A8*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] 
          GetInterfaces() cil managed
  // SIG: 20 00 1D 12 71
  {
    .custom /*0C000150:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 20 00 00 00 00 )                         // ... ....
    // Method begins at RVA 0x2589
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetInterfaces

  .method /*060001A9*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*0100003E*/[] 
          GetMember(string name,
                    valuetype [System.Runtime/*23000001*/]System.Reflection.MemberTypes/*0100003F*/ 'type',
                    valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 03 1D 12 80 F9 0E 11 80 FD 11 80 C9
  {
    .custom /*0C000151:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF 1F 00 00 00 00 ) 
    // Method begins at RVA 0x258c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetMember

  .method /*060001AA*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*0100003E*/[] 
          GetMembers(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 F9 11 80 C9
  {
    .custom /*0C000152:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF 1F 00 00 00 00 ) 
    // Method begins at RVA 0x258f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetMembers

  .method /*060001AB*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetMethod(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 'type',
                    class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 'method') cil managed
  // SIG: 00 02 12 59 12 71 12 59
  {
    // Method begins at RVA 0x2592
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetMethod

  .method /*060001AC*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetMethodImpl(string name,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr,
                        class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000033*/ binder,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002B*/ callConvention,
                        class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] types,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003A*/[] modifiers) cil managed
  // SIG: 20 06 12 59 0E 11 80 C9 12 80 CD 11 80 AD 1D 12 71 1D 11 80 E9
  {
    .custom /*0C000153:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000154:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 18 00 00 00 00 00 ) 
    .param [1]/*08000230*/ 
    .custom /*0C000192:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [5]/*08000234*/ 
    .custom /*0C000193:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2595
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetMethodImpl

  .method /*060001AD*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/[] 
          GetMethods(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 59 11 80 C9
  {
    .custom /*0C000155:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 18 00 00 00 00 00 ) 
    // Method begins at RVA 0x2598
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetMethods

  .method /*060001AE*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          GetNestedType(string name,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 02 12 71 0E 11 80 C9
  {
    .custom /*0C000156:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 80 01 00 00 00 00 ) 
    .param [0]/*08000237*/ 
    .custom /*0C000194:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x259b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetNestedType

  .method /*060001AF*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] 
          GetNestedTypes(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 71 11 80 C9
  {
    .custom /*0C000158:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 80 01 00 00 00 00 ) 
    // Method begins at RVA 0x259e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetNestedTypes

  .method /*060001B0*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000040*/[] 
          GetProperties(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 81 01 11 80 C9
  {
    .custom /*0C00015A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 06 00 00 00 00 ) 
    // Method begins at RVA 0x25a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetProperties

  .method /*060001B1*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000040*/ 
          GetPropertyImpl(string name,
                          valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ bindingAttr,
                          class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000033*/ binder,
                          class [System.Runtime/*23000001*/]System.Type/*0100001C*/ returnType,
                          class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] types,
                          valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003A*/[] modifiers) cil managed
  // SIG: 20 06 12 81 01 0E 11 80 C9 12 80 CD 12 71 1D 12 71 1D 11 80 E9
  {
    .custom /*0C00015B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00015C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 00 06 00 00 00 00 ) 
    .param [0]/*0800023C*/ 
    .custom /*0C000195:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800023D*/ 
    .custom /*0C000196:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [5]/*08000241*/ 
    .custom /*0C000197:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x25a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetPropertyImpl

  .method /*060001B2*/ family hidebysig virtual 
          instance bool  HasElementTypeImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x25a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::HasElementTypeImpl

  .method /*060001B3*/ public hidebysig virtual 
          instance object  InvokeMember(string name,
                                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000032*/ invokeAttr,
                                        class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000033*/ binder,
                                        object target,
                                        object[] args,
                                        valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003A*/[] modifiers,
                                        class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000024*/ culture,
                                        string[] namedParameters) cil managed
  // SIG: 20 08 1C 0E 11 80 C9 12 80 CD 1C 1D 1C 1D 11 80 E9 12 80 91 1D 0E
  {
    .custom /*0C00015D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00015E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF FF FF FF 00 00 ) 
    .param [1]/*08000243*/ 
    .custom /*0C000198:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [8]/*0800024A*/ 
    .custom /*0C000199:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x25aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::InvokeMember

  .method /*060001B4*/ family hidebysig virtual 
          instance bool  IsArrayImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x25ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::IsArrayImpl

  .method /*060001B5*/ public hidebysig virtual 
          instance bool  IsAssignableFrom(class [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000034*/ typeInfo) cil managed
  // SIG: 20 01 02 12 80 D1
  {
    .custom /*0C000161:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800024B*/ 
    .custom /*0C00019A:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000041*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x25b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::IsAssignableFrom

  .method /*060001B6*/ public hidebysig virtual 
          instance bool  IsAssignableFrom(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ c) cil managed
  // SIG: 20 01 02 12 71
  {
    .custom /*0C000163:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800024C*/ 
    .custom /*0C00019B:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000041*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x25b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::IsAssignableFrom

  .method /*060001B7*/ family hidebysig virtual 
          instance bool  IsByRefImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x25b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::IsByRefImpl

  .method /*060001B8*/ family hidebysig virtual 
          instance bool  IsCOMObjectImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x25b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::IsCOMObjectImpl

  .method /*060001B9*/ public hidebysig instance bool 
          IsCreated() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x25bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::IsCreated

  .method /*060001BA*/ public hidebysig virtual 
          instance bool  IsDefined(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ attributeType,
                                   bool inherit) cil managed
  // SIG: 20 02 02 12 71 02
  {
    // Method begins at RVA 0x25bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::IsDefined

  .method /*060001BB*/ family hidebysig virtual 
          instance bool  IsPointerImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x25c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::IsPointerImpl

  .method /*060001BC*/ family hidebysig virtual 
          instance bool  IsPrimitiveImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x25c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::IsPrimitiveImpl

  .method /*060001BD*/ public hidebysig virtual 
          instance bool  IsSubclassOf(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ c) cil managed
  // SIG: 20 01 02 12 71
  {
    // Method begins at RVA 0x25c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::IsSubclassOf

  .method /*060001BE*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          MakeArrayType() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x25cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::MakeArrayType

  .method /*060001BF*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          MakeArrayType(int32 rank) cil managed
  // SIG: 20 01 12 71 08
  {
    // Method begins at RVA 0x25ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::MakeArrayType

  .method /*060001C0*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          MakeByRefType() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x25d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::MakeByRefType

  .method /*060001C1*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          MakeGenericType(class [System.Runtime/*23000001*/]System.Type/*0100001C*/[] typeArguments) cil managed
  // SIG: 20 01 12 71 1D 12 71
  {
    .custom /*0C000169:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001F*/::.ctor(string) /* 0A000012 */ = ( 01 00 80 C5 49 66 20 73 6F 6D 65 20 6F 66 20 74   // ....If some of t
                                                                                                                                                                                            68 65 20 67 65 6E 65 72 69 63 20 61 72 67 75 6D   // he generic argum
                                                                                                                                                                                            65 6E 74 73 20 61 72 65 20 61 6E 6E 6F 74 61 74   // ents are annotat
                                                                                                                                                                                            65 64 20 28 65 69 74 68 65 72 20 77 69 74 68 20   // ed (either with 
                                                                                                                                                                                            44 79 6E 61 6D 69 63 61 6C 6C 79 41 63 63 65 73   // DynamicallyAcces
                                                                                                                                                                                            73 65 64 4D 65 6D 62 65 72 73 41 74 74 72 69 62   // sedMembersAttrib
                                                                                                                                                                                            75 74 65 2C 20 6F 72 20 67 65 6E 65 72 69 63 20   // ute, or generic 
                                                                                                                                                                                            63 6F 6E 73 74 72 61 69 6E 74 73 29 2C 20 74 72   // constraints), tr
                                                                                                                                                                                            69 6D 6D 69 6E 67 20 63 61 6E 27 74 20 76 61 6C   // imming can't val
                                                                                                                                                                                            69 64 61 74 65 20 74 68 61 74 20 74 68 65 20 72   // idate that the r
                                                                                                                                                                                            65 71 75 69 72 65 6D 65 6E 74 73 20 6F 66 20 74   // equirements of t
                                                                                                                                                                                            68 6F 73 65 20 61 6E 6E 6F 74 61 74 69 6F 6E 73   // hose annotations
                                                                                                                                                                                            20 61 72 65 20 6D 65 74 2E 00 00 )                //  are met...
    .param [1]/*08000251*/ 
    .custom /*0C00019C:0A000018*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000046*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x25d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::MakeGenericType

  .method /*060001C2*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ 
          MakePointerType() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x25d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::MakePointerType

  .method /*060001C3*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000026*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 99 1D 05
  {
    // Method begins at RVA 0x25da
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TypeBuilder::SetCustomAttribute

  .method /*060001C4*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001A*/ customBuilder) cil managed
  // SIG: 20 01 01 12 69
  {
    // Method begins at RVA 0x25dc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TypeBuilder::SetCustomAttribute

  .method /*060001C5*/ public hidebysig instance void 
          SetParent(class [System.Runtime/*23000001*/]System.Type/*0100001C*/ parent) cil managed
  // SIG: 20 01 01 12 71
  {
    .custom /*0C00016C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000255*/ 
    .custom /*0C00019D:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000038*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000037*/) /* 0A000016 */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x25de
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TypeBuilder::SetParent

  .method /*060001C6*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x25e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::ToString

  .property /*17000065*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/
          Assembly()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ System.Reflection.Emit.TypeBuilder/*02000010*/::get_Assembly() /* 0600015A */
  } // end of property TypeBuilder::Assembly
  .property /*17000066*/ instance callconv(8) string
          AssemblyQualifiedName()
  {
    .custom /*0C000084:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.TypeBuilder/*02000010*/::get_AssemblyQualifiedName() /* 0600015B */
  } // end of property TypeBuilder::AssemblyQualifiedName
  .property /*17000067*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          BaseType()
  {
    .custom /*0C000086:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.TypeBuilder/*02000010*/::get_BaseType() /* 0600015C */
  } // end of property TypeBuilder::BaseType
  .property /*17000068*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodBase/*01000044*/
          DeclaringMethod()
  {
    .custom /*0C000088:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodBase/*01000044*/ System.Reflection.Emit.TypeBuilder/*02000010*/::get_DeclaringMethod() /* 0600015D */
  } // end of property TypeBuilder::DeclaringMethod
  .property /*17000069*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          DeclaringType()
  {
    .custom /*0C00008B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.TypeBuilder/*02000010*/::get_DeclaringType() /* 0600015E */
  } // end of property TypeBuilder::DeclaringType
  .property /*1700006A*/ instance callconv(8) string
          FullName()
  {
    .custom /*0C00008F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.TypeBuilder/*02000010*/::get_FullName() /* 0600015F */
  } // end of property TypeBuilder::FullName
  .property /*1700006B*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Reflection.GenericParameterAttributes/*01000045*/
          GenericParameterAttributes()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Reflection.GenericParameterAttributes/*01000045*/ System.Reflection.Emit.TypeBuilder/*02000010*/::get_GenericParameterAttributes() /* 06000160 */
  } // end of property TypeBuilder::GenericParameterAttributes
  .property /*1700006C*/ instance callconv(8) int32
          GenericParameterPosition()
  {
    .get instance int32 System.Reflection.Emit.TypeBuilder/*02000010*/::get_GenericParameterPosition() /* 06000161 */
  } // end of property TypeBuilder::GenericParameterPosition
  .property /*1700006D*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/
          GUID()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Guid/*01000035*/ System.Reflection.Emit.TypeBuilder/*02000010*/::get_GUID() /* 06000162 */
  } // end of property TypeBuilder::GUID
  .property /*1700006E*/ instance callconv(8) bool
          IsByRefLike()
  {
    .get instance bool System.Reflection.Emit.TypeBuilder/*02000010*/::get_IsByRefLike() /* 06000163 */
  } // end of property TypeBuilder::IsByRefLike
  .property /*1700006F*/ instance callconv(8) bool
          IsConstructedGenericType()
  {
    .get instance bool System.Reflection.Emit.TypeBuilder/*02000010*/::get_IsConstructedGenericType() /* 06000164 */
  } // end of property TypeBuilder::IsConstructedGenericType
  .property /*17000070*/ instance callconv(8) bool
          IsGenericParameter()
  {
    .get instance bool System.Reflection.Emit.TypeBuilder/*02000010*/::get_IsGenericParameter() /* 06000165 */
  } // end of property TypeBuilder::IsGenericParameter
  .property /*17000071*/ instance callconv(8) bool
          IsGenericType()
  {
    .get instance bool System.Reflection.Emit.TypeBuilder/*02000010*/::get_IsGenericType() /* 06000166 */
  } // end of property TypeBuilder::IsGenericType
  .property /*17000072*/ instance callconv(8) bool
          IsGenericTypeDefinition()
  {
    .get instance bool System.Reflection.Emit.TypeBuilder/*02000010*/::get_IsGenericTypeDefinition() /* 06000167 */
  } // end of property TypeBuilder::IsGenericTypeDefinition
  .property /*17000073*/ instance callconv(8) bool
          IsSecurityCritical()
  {
    .get instance bool System.Reflection.Emit.TypeBuilder/*02000010*/::get_IsSecurityCritical() /* 06000168 */
  } // end of property TypeBuilder::IsSecurityCritical
  .property /*17000074*/ instance callconv(8) bool
          IsSecuritySafeCritical()
  {
    .get instance bool System.Reflection.Emit.TypeBuilder/*02000010*/::get_IsSecuritySafeCritical() /* 06000169 */
  } // end of property TypeBuilder::IsSecuritySafeCritical
  .property /*17000075*/ instance callconv(8) bool
          IsSecurityTransparent()
  {
    .get instance bool System.Reflection.Emit.TypeBuilder/*02000010*/::get_IsSecurityTransparent() /* 0600016A */
  } // end of property TypeBuilder::IsSecurityTransparent
  .property /*17000076*/ instance callconv(8) bool
          IsSZArray()
  {
    .get instance bool System.Reflection.Emit.TypeBuilder/*02000010*/::get_IsSZArray() /* 0600016B */
  } // end of property TypeBuilder::IsSZArray
  .property /*17000077*/ instance callconv(8) bool
          IsTypeDefinition()
  {
    .get instance bool System.Reflection.Emit.TypeBuilder/*02000010*/::get_IsTypeDefinition() /* 0600016C */
  } // end of property TypeBuilder::IsTypeDefinition
  .property /*17000078*/ instance callconv(8) int32
          MetadataToken()
  {
    .get instance int32 System.Reflection.Emit.TypeBuilder/*02000010*/::get_MetadataToken() /* 0600016D */
  } // end of property TypeBuilder::MetadataToken
  .property /*17000079*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/
          Module()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ System.Reflection.Emit.TypeBuilder/*02000010*/::get_Module() /* 0600016E */
  } // end of property TypeBuilder::Module
  .property /*1700007A*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Reflection.Emit.TypeBuilder/*02000010*/::get_Name() /* 0600016F */
  } // end of property TypeBuilder::Name
  .property /*1700007B*/ instance callconv(8) string
          Namespace()
  {
    .custom /*0C00009D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.TypeBuilder/*02000010*/::get_Namespace() /* 06000170 */
  } // end of property TypeBuilder::Namespace
  .property /*1700007C*/ instance callconv(8) valuetype [System.Reflection.Primitives/*23000004*/]System.Reflection.Emit.PackingSize/*0100004A*/
          PackingSize()
  {
    .get instance valuetype [System.Reflection.Primitives/*23000004*/]System.Reflection.Emit.PackingSize/*0100004A*/ System.Reflection.Emit.TypeBuilder/*02000010*/::get_PackingSize() /* 06000171 */
  } // end of property TypeBuilder::PackingSize
  .property /*1700007D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          ReflectedType()
  {
    .custom /*0C00009E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.TypeBuilder/*02000010*/::get_ReflectedType() /* 06000172 */
  } // end of property TypeBuilder::ReflectedType
  .property /*1700007E*/ instance callconv(8) int32
          Size()
  {
    .get instance int32 System.Reflection.Emit.TypeBuilder/*02000010*/::get_Size() /* 06000173 */
  } // end of property TypeBuilder::Size
  .property /*1700007F*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000036*/
          TypeHandle()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000036*/ System.Reflection.Emit.TypeBuilder/*02000010*/::get_TypeHandle() /* 06000174 */
  } // end of property TypeBuilder::TypeHandle
  .property /*17000080*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001C*/
          UnderlyingSystemType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001C*/ System.Reflection.Emit.TypeBuilder/*02000010*/::get_UnderlyingSystemType() /* 06000175 */
  } // end of property TypeBuilder::UnderlyingSystemType
} // end of class System.Reflection.Emit.TypeBuilder


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
