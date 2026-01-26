


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
// Time-date stamp:            0xbcb1b48b
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
// Addr. of entry point:           0x00003d0e
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
// Checksum:                       0x0000e597
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00003cbc [0x0000004f] address [size] of Import Directory:          
// 0x00004000 [0x000004f4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002800 [0x00002920] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00003ca0 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00001d14 Virtual Size
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
//              0x000004f4 Virtual Size
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
//              Entry 1: Type 0x3 Offset 0x00000d10
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00003cfe Import Name Table
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
// 0x00002100 [0x00001b20] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00003c20 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000c88 Size
//              '#~' Name
//    Stream 2:
//              0x00000cf4 Offset
//              0x00000958 Size
//              '#Strings' Name
//    Stream 3:
//              0x0000164c Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00001650 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001660 Offset
//              0x000004c0 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000000909a01757 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 20768
// PE header size       : 512 (496 used)    ( 2.47%)
// PE additional info   : 11995             (57.76%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.35%)
// CLR meta-data size  : 6944               (33.44%)
// CLR additional info : 128                ( 0.62%)
// CLR method headers  : 49                 ( 0.24%)
// Managed code         : 125               ( 0.60%)
// Data                 : 2048              ( 9.86%)
// Unaccounted          : -1105             (-5.32%)

// Num.of PE sections   : 3
//   .text    - 7680
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 6944
//   Module        -    1 (10 bytes)
//   TypeDef       -    7 (98 bytes)      0 interfaces, 0 explicit layout
//   TypeRef       -   38 (228 bytes)
//   MethodDef     -   82 (1148 bytes)    33 abstract, 0 native, 49 bodies
//   FieldDef      -    1 (6 bytes)       0 constant
//   MemberRef     -   25 (150 bytes)
//   ParamDef      -  126 (756 bytes)
//   CustomAttribute-   84 (504 bytes)
//   InterfaceImpl -    1 (4 bytes)
//   PropertyMap   -    3 (12 bytes)
//   Property      -   10 (60 bytes)
//   MethodSemantic-   10 (60 bytes)
//   TypeSpec      -    1 (2 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    3 (60 bytes)
//   Strings       -  2390 bytes
//   Blobs         -  1216 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   198 bytes

// CLR additional info : 128

// CLR method headers : 49
//   Num.of method bodies  - 49
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 49

// Managed code : 125
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class CustomAttributeBuilder         (public) (auto) (ansi) 
// Class ILGenerator                    (public) (abstract) (auto) (ansi) 
// Class Label                          (public) (sequential) (ansi) (sealed) 
// Class LocalBuilder                   (public) (auto) (ansi) (sealed) 
// Class ParameterBuilder               (public) (abstract) (auto) (ansi) 
// Class SignatureHelper                (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly extern /*23000002*/ System.Reflection.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly extern /*23000003*/ System.Runtime.InteropServices
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly /*20000001*/ noplatform System.Reflection.Emit.ILGeneration
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
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 23 53 79 73 74 65 6D 2E 52 65 66 6C 65 63   // ..#System.Reflec
                                                                                                                                                                        74 69 6F 6E 2E 45 6D 69 74 2E 49 4C 47 65 6E 65   // tion.Emit.ILGene
                                                                                                                                                                        72 61 74 69 6F 6E 00 00 )                         // ration..
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000C:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000D:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000E:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 23 53 79 73 74 65 6D 2E 52 65 66 6C 65 63   // ..#System.Reflec
                                                                                                                                                                       74 69 6F 6E 2E 45 6D 69 74 2E 49 4C 47 65 6E 65   // tion.Emit.ILGene
                                                                                                                                                                       72 61 74 69 6F 6E 00 00 )                         // ration..
  .custom /*0C00000F:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 38 2E 30 2E 32 33 2E 35 33 31 30 33 00   // ...8.0.23.53103.
                                                                                                                                                                       00 ) 
  .custom /*0C000010:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 38 2E 30 2E 30 2B 35 35 33 35 65 33 31   // ...8.0.0+5535e31
                                                                                                                                                                                61 37 31 32 33 34 33 61 36 33 66 35 64 37 64 37   // a712343a63f5d7d7
                                                                                                                                                                                39 36 63 64 38 37 34 65 35 36 33 65 35 61 63 31   // 96cd874e563e5ac1
                                                                                                                                                                                34 00 00 )                                        // 4..
  .custom /*0C000011:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000012:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 23 53 79 73 74 65 6D 2E 52 65 66 6C 65 63   // ..#System.Reflec
                                                                                                                                                                 74 69 6F 6E 2E 45 6D 69 74 2E 49 4C 47 65 6E 65   // tion.Emit.ILGene
                                                                                                                                                                 72 61 74 69 6F 6E 00 00 )                         // ration..
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
.module System.Reflection.Emit.ILGeneration.dll
// MVID: {9540868a-74da-4705-aa9e-faa27cf0a340}
.custom /*0C000001:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000011*/::.ctor(int32) /* 0A000010 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000002:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000012*/::.ctor(bool) /* 0A000011 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi beforefieldinit System.Reflection.Emit.CustomAttributeBuilder
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000014:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000015:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000016*/ con,
                               object[] constructorArgs) cil managed
  // SIG: 20 02 01 12 59 1D 1C
  {
    .param [2]/*08000002*/ 
    .custom /*0C000016:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CustomAttributeBuilder::.ctor

  .method /*06000002*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000016*/ con,
                               object[] constructorArgs,
                               class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*01000017*/[] namedFields,
                               object[] fieldValues) cil managed
  // SIG: 20 04 01 12 59 1D 1C 1D 12 5D 1D 1C
  {
    .param [2]/*08000004*/ 
    .custom /*0C00001A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [4]/*08000006*/ 
    .custom /*0C00001F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2058
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CustomAttributeBuilder::.ctor

  .method /*06000003*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000016*/ con,
                               object[] constructorArgs,
                               class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000018*/[] namedProperties,
                               object[] propertyValues) cil managed
  // SIG: 20 04 01 12 59 1D 1C 1D 12 61 1D 1C
  {
    .param [2]/*08000008*/ 
    .custom /*0C000023:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [4]/*0800000A*/ 
    .custom /*0C000024:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2060
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CustomAttributeBuilder::.ctor

  .method /*06000004*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000016*/ con,
                               object[] constructorArgs,
                               class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000018*/[] namedProperties,
                               object[] propertyValues,
                               class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*01000017*/[] namedFields,
                               object[] fieldValues) cil managed
  // SIG: 20 06 01 12 59 1D 1C 1D 12 61 1D 1C 1D 12 5D 1D 1C
  {
    .param [2]/*0800000C*/ 
    .custom /*0C000025:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [4]/*0800000E*/ 
    .custom /*0C000026:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [6]/*08000010*/ 
    .custom /*0C000027:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2068
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CustomAttributeBuilder::.ctor

} // end of class System.Reflection.Emit.CustomAttributeBuilder

.class /*02000003*/ public abstract auto ansi beforefieldinit System.Reflection.Emit.ILGenerator
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000017:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000018:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000005*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2070
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ILGenerator::.ctor

  .method /*06000006*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_ILOffset() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::get_ILOffset

  .method /*06000007*/ public hidebysig newslot abstract virtual 
          instance void  BeginCatchBlock(class [System.Runtime/*23000001*/]System.Type/*01000019*/ exceptionType) cil managed
  // SIG: 20 01 01 12 65
  {
    .custom /*0C000020:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ILGenerator::BeginCatchBlock

  .method /*06000008*/ public hidebysig newslot abstract virtual 
          instance void  BeginExceptFilterBlock() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::BeginExceptFilterBlock

  .method /*06000009*/ public hidebysig newslot abstract virtual 
          instance valuetype System.Reflection.Emit.Label/*02000004*/ 
          BeginExceptionBlock() cil managed
  // SIG: 20 00 11 10
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::BeginExceptionBlock

  .method /*0600000A*/ public hidebysig newslot abstract virtual 
          instance void  BeginFaultBlock() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::BeginFaultBlock

  .method /*0600000B*/ public hidebysig newslot abstract virtual 
          instance void  BeginFinallyBlock() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::BeginFinallyBlock

  .method /*0600000C*/ public hidebysig newslot abstract virtual 
          instance void  BeginScope() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::BeginScope

  .method /*0600000D*/ public hidebysig newslot virtual 
          instance class System.Reflection.Emit.LocalBuilder/*02000005*/ 
          DeclareLocal(class [System.Runtime/*23000001*/]System.Type/*01000019*/ localType) cil managed
  // SIG: 20 01 12 14 12 65
  {
    // Method begins at RVA 0x2078
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ILGenerator::DeclareLocal

  .method /*0600000E*/ public hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.LocalBuilder/*02000005*/ 
          DeclareLocal(class [System.Runtime/*23000001*/]System.Type/*01000019*/ localType,
                       bool 'pinned') cil managed
  // SIG: 20 02 12 14 12 65 02
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::DeclareLocal

  .method /*0600000F*/ public hidebysig newslot abstract virtual 
          instance valuetype System.Reflection.Emit.Label/*02000004*/ 
          DefineLabel() cil managed
  // SIG: 20 00 11 10
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::DefineLabel

  .method /*06000010*/ public hidebysig newslot abstract virtual 
          instance void  Emit(valuetype [System.Reflection.Primitives/*23000002*/]System.Reflection.Emit.OpCode/*0100001A*/ opcode) cil managed
  // SIG: 20 01 01 11 69
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::Emit

  .method /*06000011*/ public hidebysig newslot abstract virtual 
          instance void  Emit(valuetype [System.Reflection.Primitives/*23000002*/]System.Reflection.Emit.OpCode/*0100001A*/ opcode,
                              uint8 arg) cil managed
  // SIG: 20 02 01 11 69 05
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::Emit

  .method /*06000012*/ public hidebysig newslot abstract virtual 
          instance void  Emit(valuetype [System.Reflection.Primitives/*23000002*/]System.Reflection.Emit.OpCode/*0100001A*/ opcode,
                              float64 arg) cil managed
  // SIG: 20 02 01 11 69 0D
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::Emit

  .method /*06000013*/ public hidebysig newslot abstract virtual 
          instance void  Emit(valuetype [System.Reflection.Primitives/*23000002*/]System.Reflection.Emit.OpCode/*0100001A*/ opcode,
                              int16 arg) cil managed
  // SIG: 20 02 01 11 69 06
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::Emit

  .method /*06000014*/ public hidebysig newslot abstract virtual 
          instance void  Emit(valuetype [System.Reflection.Primitives/*23000002*/]System.Reflection.Emit.OpCode/*0100001A*/ opcode,
                              int32 arg) cil managed
  // SIG: 20 02 01 11 69 08
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::Emit

  .method /*06000015*/ public hidebysig newslot abstract virtual 
          instance void  Emit(valuetype [System.Reflection.Primitives/*23000002*/]System.Reflection.Emit.OpCode/*0100001A*/ opcode,
                              int64 arg) cil managed
  // SIG: 20 02 01 11 69 0A
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::Emit

  .method /*06000016*/ public hidebysig newslot abstract virtual 
          instance void  Emit(valuetype [System.Reflection.Primitives/*23000002*/]System.Reflection.Emit.OpCode/*0100001A*/ opcode,
                              class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000016*/ con) cil managed
  // SIG: 20 02 01 11 69 12 59
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::Emit

  .method /*06000017*/ public hidebysig newslot abstract virtual 
          instance void  Emit(valuetype [System.Reflection.Primitives/*23000002*/]System.Reflection.Emit.OpCode/*0100001A*/ opcode,
                              valuetype System.Reflection.Emit.Label/*02000004*/ label) cil managed
  // SIG: 20 02 01 11 69 11 10
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::Emit

  .method /*06000018*/ public hidebysig newslot abstract virtual 
          instance void  Emit(valuetype [System.Reflection.Primitives/*23000002*/]System.Reflection.Emit.OpCode/*0100001A*/ opcode,
                              valuetype System.Reflection.Emit.Label/*02000004*/[] labels) cil managed
  // SIG: 20 02 01 11 69 1D 11 10
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::Emit

  .method /*06000019*/ public hidebysig newslot abstract virtual 
          instance void  Emit(valuetype [System.Reflection.Primitives/*23000002*/]System.Reflection.Emit.OpCode/*0100001A*/ opcode,
                              class System.Reflection.Emit.LocalBuilder/*02000005*/ local) cil managed
  // SIG: 20 02 01 11 69 12 14
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::Emit

  .method /*0600001A*/ public hidebysig newslot abstract virtual 
          instance void  Emit(valuetype [System.Reflection.Primitives/*23000002*/]System.Reflection.Emit.OpCode/*0100001A*/ opcode,
                              class System.Reflection.Emit.SignatureHelper/*02000007*/ signature) cil managed
  // SIG: 20 02 01 11 69 12 1C
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::Emit

  .method /*0600001B*/ public hidebysig newslot abstract virtual 
          instance void  Emit(valuetype [System.Reflection.Primitives/*23000002*/]System.Reflection.Emit.OpCode/*0100001A*/ opcode,
                              class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*01000017*/ 'field') cil managed
  // SIG: 20 02 01 11 69 12 5D
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::Emit

  .method /*0600001C*/ public hidebysig newslot abstract virtual 
          instance void  Emit(valuetype [System.Reflection.Primitives/*23000002*/]System.Reflection.Emit.OpCode/*0100001A*/ opcode,
                              class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001B*/ meth) cil managed
  // SIG: 20 02 01 11 69 12 6D
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::Emit

  .method /*0600001D*/ public hidebysig instance void 
          Emit(valuetype [System.Reflection.Primitives/*23000002*/]System.Reflection.Emit.OpCode/*0100001A*/ opcode,
               int8 arg) cil managed
  // SIG: 20 02 01 11 69 04
  {
    .custom /*0C000028:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x207b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ILGenerator::Emit

  .method /*0600001E*/ public hidebysig newslot abstract virtual 
          instance void  Emit(valuetype [System.Reflection.Primitives/*23000002*/]System.Reflection.Emit.OpCode/*0100001A*/ opcode,
                              float32 arg) cil managed
  // SIG: 20 02 01 11 69 0C
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::Emit

  .method /*0600001F*/ public hidebysig newslot abstract virtual 
          instance void  Emit(valuetype [System.Reflection.Primitives/*23000002*/]System.Reflection.Emit.OpCode/*0100001A*/ opcode,
                              string str) cil managed
  // SIG: 20 02 01 11 69 0E
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::Emit

  .method /*06000020*/ public hidebysig newslot abstract virtual 
          instance void  Emit(valuetype [System.Reflection.Primitives/*23000002*/]System.Reflection.Emit.OpCode/*0100001A*/ opcode,
                              class [System.Runtime/*23000001*/]System.Type/*01000019*/ cls) cil managed
  // SIG: 20 02 01 11 69 12 65
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::Emit

  .method /*06000021*/ public hidebysig newslot abstract virtual 
          instance void  EmitCall(valuetype [System.Reflection.Primitives/*23000002*/]System.Reflection.Emit.OpCode/*0100001A*/ opcode,
                                  class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001B*/ methodInfo,
                                  class [System.Runtime/*23000001*/]System.Type/*01000019*/[] optionalParameterTypes) cil managed
  // SIG: 20 03 01 11 69 12 6D 1D 12 65
  {
    .param [3]/*08000038*/ 
    .custom /*0C00002C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ILGenerator::EmitCall

  .method /*06000022*/ public hidebysig newslot abstract virtual 
          instance void  EmitCalli(valuetype [System.Reflection.Primitives/*23000002*/]System.Reflection.Emit.OpCode/*0100001A*/ opcode,
                                   valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100001C*/ callingConvention,
                                   class [System.Runtime/*23000001*/]System.Type/*01000019*/ returnType,
                                   class [System.Runtime/*23000001*/]System.Type/*01000019*/[] parameterTypes,
                                   class [System.Runtime/*23000001*/]System.Type/*01000019*/[] optionalParameterTypes) cil managed
  // SIG: 20 05 01 11 69 11 71 12 65 1D 12 65 1D 12 65
  {
    .custom /*0C000029:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800003C*/ 
    .custom /*0C00002D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [5]/*0800003D*/ 
    .custom /*0C00002F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ILGenerator::EmitCalli

  .method /*06000023*/ public hidebysig newslot abstract virtual 
          instance void  EmitCalli(valuetype [System.Reflection.Primitives/*23000002*/]System.Reflection.Emit.OpCode/*0100001A*/ opcode,
                                   valuetype [System.Runtime.InteropServices/*23000003*/]System.Runtime.InteropServices.CallingConvention/*0100001D*/ unmanagedCallConv,
                                   class [System.Runtime/*23000001*/]System.Type/*01000019*/ returnType,
                                   class [System.Runtime/*23000001*/]System.Type/*01000019*/[] parameterTypes) cil managed
  // SIG: 20 04 01 11 69 11 75 12 65 1D 12 65
  {
    .custom /*0C00002A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000041*/ 
    .custom /*0C000032:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ILGenerator::EmitCalli

  .method /*06000024*/ public hidebysig newslot virtual 
          instance void  EmitWriteLine(class System.Reflection.Emit.LocalBuilder/*02000005*/ localBuilder) cil managed
  // SIG: 20 01 01 12 14
  {
    // Method begins at RVA 0x207d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ILGenerator::EmitWriteLine

  .method /*06000025*/ public hidebysig newslot virtual 
          instance void  EmitWriteLine(class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*01000017*/ fld) cil managed
  // SIG: 20 01 01 12 5D
  {
    // Method begins at RVA 0x207f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ILGenerator::EmitWriteLine

  .method /*06000026*/ public hidebysig newslot virtual 
          instance void  EmitWriteLine(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2081
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ILGenerator::EmitWriteLine

  .method /*06000027*/ public hidebysig newslot abstract virtual 
          instance void  EndExceptionBlock() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::EndExceptionBlock

  .method /*06000028*/ public hidebysig newslot abstract virtual 
          instance void  EndScope() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::EndScope

  .method /*06000029*/ public hidebysig newslot abstract virtual 
          instance void  MarkLabel(valuetype System.Reflection.Emit.Label/*02000004*/ loc) cil managed
  // SIG: 20 01 01 11 10
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::MarkLabel

  .method /*0600002A*/ public hidebysig newslot virtual 
          instance void  ThrowException(class [System.Runtime/*23000001*/]System.Type/*01000019*/ excType) cil managed
  // SIG: 20 01 01 12 65
  {
    .param [1]/*08000046*/ 
    .custom /*0C000034:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001E*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2083
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ILGenerator::ThrowException

  .method /*0600002B*/ public hidebysig newslot abstract virtual 
          instance void  UsingNamespace(string usingNamespace) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method ILGenerator::UsingNamespace

  .property /*17000001*/ instance callconv(8) int32
          ILOffset()
  {
    .get instance int32 System.Reflection.Emit.ILGenerator/*02000003*/::get_ILOffset() /* 06000006 */
  } // end of property ILGenerator::ILOffset
} // end of class System.Reflection.Emit.ILGenerator

.class /*02000004*/ public sequential ansi sealed beforefieldinit System.Reflection.Emit.Label
       extends [System.Runtime/*23000001*/]System.ValueType/*01000021*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000022*/<valuetype System.Reflection.Emit.Label/*02000004*/>/*1B000001*/
{
  .custom /*0C000019:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000020*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
  .field /*04000001*/ private initonly int32 _dummyPrimitive
  .method /*0600002C*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00002B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000048*/ 
    .custom /*0C000035:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000023*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2085
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Label::Equals

  .method /*0600002D*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Reflection.Emit.Label/*02000004*/ obj) cil managed
  // SIG: 20 01 02 11 10
  {
    // Method begins at RVA 0x2088
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Label::Equals

  .method /*0600002E*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x208b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Label::GetHashCode

  .method /*0600002F*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Reflection.Emit.Label/*02000004*/ a,
                            valuetype System.Reflection.Emit.Label/*02000004*/ b) cil managed
  // SIG: 00 02 02 11 10 11 10
  {
    // Method begins at RVA 0x208e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Label::op_Equality

  .method /*06000030*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Reflection.Emit.Label/*02000004*/ a,
                              valuetype System.Reflection.Emit.Label/*02000004*/ b) cil managed
  // SIG: 00 02 02 11 10 11 10
  {
    // Method begins at RVA 0x2091
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Label::op_Inequality

} // end of class System.Reflection.Emit.Label

.class /*02000005*/ public auto ansi sealed beforefieldinit System.Reflection.Emit.LocalBuilder
       extends [System.Runtime/*23000001*/]System.Reflection.LocalVariableInfo/*01000024*/
{
  .custom /*0C00001B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000031*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2094
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Runtime/*23000001*/]System.Reflection.LocalVariableInfo/*01000024*/::.ctor() /* 0A000019 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LocalBuilder::.ctor

  .method /*06000032*/ public hidebysig specialname virtual 
          instance bool  get_IsPinned() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x209c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalBuilder::get_IsPinned

  .method /*06000033*/ public hidebysig specialname virtual 
          instance int32  get_LocalIndex() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x209f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalBuilder::get_LocalIndex

  .method /*06000034*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*01000019*/ 
          get_LocalType() cil managed
  // SIG: 20 00 12 65
  {
    // Method begins at RVA 0x20a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalBuilder::get_LocalType

  .property /*17000002*/ instance callconv(8) bool
          IsPinned()
  {
    .get instance bool System.Reflection.Emit.LocalBuilder/*02000005*/::get_IsPinned() /* 06000032 */
  } // end of property LocalBuilder::IsPinned
  .property /*17000003*/ instance callconv(8) int32
          LocalIndex()
  {
    .get instance int32 System.Reflection.Emit.LocalBuilder/*02000005*/::get_LocalIndex() /* 06000033 */
  } // end of property LocalBuilder::LocalIndex
  .property /*17000004*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000019*/
          LocalType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000019*/ System.Reflection.Emit.LocalBuilder/*02000005*/::get_LocalType() /* 06000034 */
  } // end of property LocalBuilder::LocalType
} // end of class System.Reflection.Emit.LocalBuilder

.class /*02000006*/ public abstract auto ansi beforefieldinit System.Reflection.Emit.ParameterBuilder
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C00001D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00001E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000035*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20a5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ParameterBuilder::.ctor

  .method /*06000036*/ public hidebysig newslot specialname virtual 
          instance int32  get_Attributes() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterBuilder::get_Attributes

  .method /*06000037*/ public hidebysig specialname 
          instance bool  get_IsIn() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterBuilder::get_IsIn

  .method /*06000038*/ public hidebysig specialname 
          instance bool  get_IsOptional() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterBuilder::get_IsOptional

  .method /*06000039*/ public hidebysig specialname 
          instance bool  get_IsOut() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterBuilder::get_IsOut

  .method /*0600003A*/ public hidebysig newslot specialname virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterBuilder::get_Name

  .method /*0600003B*/ public hidebysig newslot specialname virtual 
          instance int32  get_Position() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterBuilder::get_Position

  .method /*0600003C*/ public hidebysig newslot virtual 
          instance void  SetConstant(object defaultValue) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x20bf
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ParameterBuilder::SetConstant

  .method /*0600003D*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000016*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 59 1D 05
  {
    .custom /*0C00002E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20c1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ParameterBuilder::SetCustomAttribute

  .method /*0600003E*/ public hidebysig instance void 
          SetCustomAttribute(class System.Reflection.Emit.CustomAttributeBuilder/*02000002*/ customBuilder) cil managed
  // SIG: 20 01 01 12 08
  {
    .custom /*0C000030:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20c3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ParameterBuilder::SetCustomAttribute

  .method /*0600003F*/ family hidebysig newslot abstract virtual 
          instance void  SetCustomAttributeCore(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000016*/ con,
                                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000025*/<uint8> binaryAttribute) cil managed
  // SIG: 20 02 01 12 59 15 11 80 95 01 05
  {
    .custom /*0C000031:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000052*/ 
    .custom /*0C00003B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ParameterBuilder::SetCustomAttributeCore

  .property /*17000005*/ instance callconv(8) int32
          Attributes()
  {
    .get instance int32 System.Reflection.Emit.ParameterBuilder/*02000006*/::get_Attributes() /* 06000036 */
  } // end of property ParameterBuilder::Attributes
  .property /*17000006*/ instance callconv(8) bool
          IsIn()
  {
    .get instance bool System.Reflection.Emit.ParameterBuilder/*02000006*/::get_IsIn() /* 06000037 */
  } // end of property ParameterBuilder::IsIn
  .property /*17000007*/ instance callconv(8) bool
          IsOptional()
  {
    .get instance bool System.Reflection.Emit.ParameterBuilder/*02000006*/::get_IsOptional() /* 06000038 */
  } // end of property ParameterBuilder::IsOptional
  .property /*17000008*/ instance callconv(8) bool
          IsOut()
  {
    .get instance bool System.Reflection.Emit.ParameterBuilder/*02000006*/::get_IsOut() /* 06000039 */
  } // end of property ParameterBuilder::IsOut
  .property /*17000009*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Reflection.Emit.ParameterBuilder/*02000006*/::get_Name() /* 0600003A */
  } // end of property ParameterBuilder::Name
  .property /*1700000A*/ instance callconv(8) int32
          Position()
  {
    .get instance int32 System.Reflection.Emit.ParameterBuilder/*02000006*/::get_Position() /* 0600003B */
  } // end of property ParameterBuilder::Position
} // end of class System.Reflection.Emit.ParameterBuilder

.class /*02000007*/ public auto ansi sealed beforefieldinit System.Reflection.Emit.SignatureHelper
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000021:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000022:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000040*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20c5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SignatureHelper::.ctor

  .method /*06000041*/ public hidebysig instance void 
          AddArgument(class [System.Runtime/*23000001*/]System.Type/*01000019*/ clsArgument) cil managed
  // SIG: 20 01 01 12 65
  {
    // Method begins at RVA 0x20cd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SignatureHelper::AddArgument

  .method /*06000042*/ public hidebysig instance void 
          AddArgument(class [System.Runtime/*23000001*/]System.Type/*01000019*/ argument,
                      bool 'pinned') cil managed
  // SIG: 20 02 01 12 65 02
  {
    // Method begins at RVA 0x20cf
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SignatureHelper::AddArgument

  .method /*06000043*/ public hidebysig instance void 
          AddArgument(class [System.Runtime/*23000001*/]System.Type/*01000019*/ argument,
                      class [System.Runtime/*23000001*/]System.Type/*01000019*/[] requiredCustomModifiers,
                      class [System.Runtime/*23000001*/]System.Type/*01000019*/[] optionalCustomModifiers) cil managed
  // SIG: 20 03 01 12 65 1D 12 65 1D 12 65
  {
    .param [2]/*08000058*/ 
    .custom /*0C00003C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [3]/*08000059*/ 
    .custom /*0C00003D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20d1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SignatureHelper::AddArgument

  .method /*06000044*/ public hidebysig instance void 
          AddArguments(class [System.Runtime/*23000001*/]System.Type/*01000019*/[] arguments,
                       class [System.Runtime/*23000001*/]System.Type/*01000019*/[][] requiredCustomModifiers,
                       class [System.Runtime/*23000001*/]System.Type/*01000019*/[][] optionalCustomModifiers) cil managed
  // SIG: 20 03 01 1D 12 65 1D 1D 12 65 1D 1D 12 65
  {
    .param [1]/*0800005A*/ 
    .custom /*0C00003E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [2]/*0800005B*/ 
    .custom /*0C00003F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [3]/*0800005C*/ 
    .custom /*0C000040:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x20d3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SignatureHelper::AddArguments

  .method /*06000045*/ public hidebysig instance void 
          AddSentinel() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20d5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SignatureHelper::AddSentinel

  .method /*06000046*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000033:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignatureHelper::Equals

  .method /*06000047*/ public hidebysig static 
          class System.Reflection.Emit.SignatureHelper/*02000007*/ 
          GetFieldSigHelper(class [System.Runtime/*23000001*/]System.Reflection.Module/*01000026*/ mod) cil managed
  // SIG: 00 01 12 1C 12 80 99
  {
    .param [1]/*0800005E*/ 
    .custom /*0C000041:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignatureHelper::GetFieldSigHelper

  .method /*06000048*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignatureHelper::GetHashCode

  .method /*06000049*/ public hidebysig static 
          class System.Reflection.Emit.SignatureHelper/*02000007*/ 
          GetLocalVarSigHelper() cil managed
  // SIG: 00 00 12 1C
  {
    // Method begins at RVA 0x20e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignatureHelper::GetLocalVarSigHelper

  .method /*0600004A*/ public hidebysig static 
          class System.Reflection.Emit.SignatureHelper/*02000007*/ 
          GetLocalVarSigHelper(class [System.Runtime/*23000001*/]System.Reflection.Module/*01000026*/ mod) cil managed
  // SIG: 00 01 12 1C 12 80 99
  {
    .param [1]/*0800005F*/ 
    .custom /*0C000042:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignatureHelper::GetLocalVarSigHelper

  .method /*0600004B*/ public hidebysig static 
          class System.Reflection.Emit.SignatureHelper/*02000007*/ 
          GetMethodSigHelper(valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100001C*/ callingConvention,
                             class [System.Runtime/*23000001*/]System.Type/*01000019*/ returnType) cil managed
  // SIG: 00 02 12 1C 11 71 12 65
  {
    .param [2]/*08000061*/ 
    .custom /*0C000043:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignatureHelper::GetMethodSigHelper

  .method /*0600004C*/ public hidebysig static 
          class System.Reflection.Emit.SignatureHelper/*02000007*/ 
          GetMethodSigHelper(class [System.Runtime/*23000001*/]System.Reflection.Module/*01000026*/ mod,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100001C*/ callingConvention,
                             class [System.Runtime/*23000001*/]System.Type/*01000019*/ returnType) cil managed
  // SIG: 00 03 12 1C 12 80 99 11 71 12 65
  {
    .custom /*0C000036:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000062*/ 
    .custom /*0C000044:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignatureHelper::GetMethodSigHelper

  .method /*0600004D*/ public hidebysig static 
          class System.Reflection.Emit.SignatureHelper/*02000007*/ 
          GetMethodSigHelper(class [System.Runtime/*23000001*/]System.Reflection.Module/*01000026*/ mod,
                             class [System.Runtime/*23000001*/]System.Type/*01000019*/ returnType,
                             class [System.Runtime/*23000001*/]System.Type/*01000019*/[] parameterTypes) cil managed
  // SIG: 00 03 12 1C 12 80 99 12 65 1D 12 65
  {
    .custom /*0C000037:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000066*/ 
    .custom /*0C000045:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000069*/ 
    .custom /*0C000046:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignatureHelper::GetMethodSigHelper

  .method /*0600004E*/ public hidebysig static 
          class System.Reflection.Emit.SignatureHelper/*02000007*/ 
          GetPropertySigHelper(class [System.Runtime/*23000001*/]System.Reflection.Module/*01000026*/ mod,
                               valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100001C*/ callingConvention,
                               class [System.Runtime/*23000001*/]System.Type/*01000019*/ returnType,
                               class [System.Runtime/*23000001*/]System.Type/*01000019*/[] requiredReturnTypeCustomModifiers,
                               class [System.Runtime/*23000001*/]System.Type/*01000019*/[] optionalReturnTypeCustomModifiers,
                               class [System.Runtime/*23000001*/]System.Type/*01000019*/[] parameterTypes,
                               class [System.Runtime/*23000001*/]System.Type/*01000019*/[][] requiredParameterTypeCustomModifiers,
                               class [System.Runtime/*23000001*/]System.Type/*01000019*/[][] optionalParameterTypeCustomModifiers) cil managed
  // SIG: 00 08 12 1C 12 80 99 11 71 12 65 1D 12 65 1D 12 65 1D 12 65 1D 1D 12 65 1D 1D 12 65
  {
    .custom /*0C000038:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800006A*/ 
    .custom /*0C000047:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [4]/*0800006E*/ 
    .custom /*0C000048:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [5]/*0800006F*/ 
    .custom /*0C000049:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [6]/*08000070*/ 
    .custom /*0C00004A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [7]/*08000071*/ 
    .custom /*0C00004B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [8]/*08000072*/ 
    .custom /*0C00004C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x20ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignatureHelper::GetPropertySigHelper

  .method /*0600004F*/ public hidebysig static 
          class System.Reflection.Emit.SignatureHelper/*02000007*/ 
          GetPropertySigHelper(class [System.Runtime/*23000001*/]System.Reflection.Module/*01000026*/ mod,
                               class [System.Runtime/*23000001*/]System.Type/*01000019*/ returnType,
                               class [System.Runtime/*23000001*/]System.Type/*01000019*/[] parameterTypes) cil managed
  // SIG: 00 03 12 1C 12 80 99 12 65 1D 12 65
  {
    .custom /*0C000039:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000073*/ 
    .custom /*0C00004D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000076*/ 
    .custom /*0C00004E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignatureHelper::GetPropertySigHelper

  .method /*06000050*/ public hidebysig static 
          class System.Reflection.Emit.SignatureHelper/*02000007*/ 
          GetPropertySigHelper(class [System.Runtime/*23000001*/]System.Reflection.Module/*01000026*/ mod,
                               class [System.Runtime/*23000001*/]System.Type/*01000019*/ returnType,
                               class [System.Runtime/*23000001*/]System.Type/*01000019*/[] requiredReturnTypeCustomModifiers,
                               class [System.Runtime/*23000001*/]System.Type/*01000019*/[] optionalReturnTypeCustomModifiers,
                               class [System.Runtime/*23000001*/]System.Type/*01000019*/[] parameterTypes,
                               class [System.Runtime/*23000001*/]System.Type/*01000019*/[][] requiredParameterTypeCustomModifiers,
                               class [System.Runtime/*23000001*/]System.Type/*01000019*/[][] optionalParameterTypeCustomModifiers) cil managed
  // SIG: 00 07 12 1C 12 80 99 12 65 1D 12 65 1D 12 65 1D 12 65 1D 1D 12 65 1D 1D 12 65
  {
    .custom /*0C00003A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000077*/ 
    .custom /*0C00004F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [3]/*0800007A*/ 
    .custom /*0C000050:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [4]/*0800007B*/ 
    .custom /*0C000051:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [5]/*0800007C*/ 
    .custom /*0C000052:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [6]/*0800007D*/ 
    .custom /*0C000053:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [7]/*0800007E*/ 
    .custom /*0C000054:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000014 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x20f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignatureHelper::GetPropertySigHelper

  .method /*06000051*/ public hidebysig instance uint8[] 
          GetSignature() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x20f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignatureHelper::GetSignature

  .method /*06000052*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignatureHelper::ToString

} // end of class System.Reflection.Emit.SignatureHelper


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
