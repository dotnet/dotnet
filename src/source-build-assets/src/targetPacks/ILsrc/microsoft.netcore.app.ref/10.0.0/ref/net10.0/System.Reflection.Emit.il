


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
// Time-date stamp:            0xd6ee0ce0
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00007c00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00009a36
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
// Checksum:                       0x0001aef1
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x000099e4 [0x0000004f] address [size] of Import Directory:          
// 0x0000a000 [0x00000498] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00008600 [0x00002938] address [size] of Security Directory:        
// 0x0000c000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x000099c8 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00007a3c Virtual Size
//              0x00002000 Virtual Address
//              0x00007c00 Size of Raw Data
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
//              0x00000498 Virtual Size
//              0x0000a000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00007e00 Pointer to Raw Data
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
//              0x00008400 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00009000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000a38
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00009a26 Import Name Table
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
// 0x000025c8 [0x00007380] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00009948 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00004708 Size
//              '#~' Name
//    Stream 2:
//              0x00004774 Offset
//              0x00001dc8 Size
//              '#Strings' Name
//    Stream 3:
//              0x0000653c Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00006540 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00006550 Offset
//              0x00000e30 Size
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

// File size            : 44856
// PE header size       : 512 (496 used)    ( 1.14%)
// PE additional info   : 11927             (26.59%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.16%)
// CLR meta-data size  : 29568              (65.92%)
// CLR additional info : 128                ( 0.29%)
// CLR method headers  : 463                ( 1.03%)
// Managed code         : 934               ( 2.08%)
// Data                 : 2048              ( 4.57%)
// Unaccounted          : -796              (-1.77%)

// Num.of PE sections   : 3
//   .text    - 31744
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 29568
//   Module        -    1 (10 bytes)
//   TypeDef       -   13 (182 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   86 (516 bytes)
//   MethodDef     -  536 (7504 bytes)    73 abstract, 0 native, 463 bodies
//   FieldDef      -    4 (24 bytes)      0 constant
//   MemberRef     -   37 (222 bytes)
//   ParamDef      -  785 (4710 bytes)
//   Constant      -    6 (36 bytes)
//   CustomAttribute-  512 (3072 bytes)
//   PropertyMap   -   10 (40 bytes)
//   Property      -  135 (810 bytes)
//   MethodSemantic-  139 (834 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    6 (120 bytes)
//   Strings       -  7623 bytes
//   Blobs         -  3632 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   191 bytes

// CLR additional info : 128

// CLR method headers : 463
//   Num.of method bodies  - 463
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 463

// Managed code : 934
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class AssemblyBuilder                (public) (abstract) (auto) (ansi) 
// Class AssemblyBuilderAccess          (public) (auto) (ansi) (sealed) 
// Class ConstructorBuilder             (public) (abstract) (auto) (ansi) 
// Class EnumBuilder                    (public) (abstract) (auto) (ansi) 
// Class EventBuilder                   (public) (abstract) (auto) (ansi) 
// Class FieldBuilder                   (public) (abstract) (auto) (ansi) 
// Class GenericTypeParameterBuilder    (public) (abstract) (auto) (ansi) 
// Class MethodBuilder                  (public) (abstract) (auto) (ansi) 
// Class ModuleBuilder                  (public) (abstract) (auto) (ansi) 
// Class PersistedAssemblyBuilder       (public) (auto) (ansi) (sealed) 
// Class PropertyBuilder                (public) (abstract) (auto) (ansi) 
// Class TypeBuilder                    (public) (abstract) (auto) (ansi) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly extern /*23000002*/ System.Reflection.Emit.ILGeneration
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly extern /*23000003*/ System.Diagnostics.StackTrace
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly extern /*23000004*/ System.Runtime.InteropServices
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly extern /*23000005*/ System.Reflection.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly extern /*23000006*/ System.Reflection.Metadata
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly /*20000001*/ noplatform System.Reflection.Emit
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000006:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000001*/::.ctor(int32) /* 0A000001 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000007:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000002*/::.ctor() /* 0A000002 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000008:0A000003*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*//DebuggingModes/*01000004*/) /* 0A000003 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000009:0A000004*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 19 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 31 30 2E 30 01 00 54 0E   // ersion=v10.0..T.
                                                                                                                                                                           14 46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61   // .FrameworkDispla
                                                                                                                                                                           79 4E 61 6D 65 09 2E 4E 45 54 20 31 30 2E 30 )    // yName..NET 10.0
  .custom /*0C00000A:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C00000B:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 16 53 79 73 74 65 6D 2E 52 65 66 6C 65 63   // ...System.Reflec
                                                                                                                                                                        74 69 6F 6E 2E 45 6D 69 74 00 00 )                // tion.Emit..
  .custom /*0C00000E:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000F:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C000010:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0F 49 73 41 6F 74 43 6F 6D 70 61 74 69 62   // ...IsAotCompatib
                                                                                                                                                                    6C 65 04 54 72 75 65 00 00 )                      // le.True..
  .custom /*0C000011:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000012:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000013:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 16 53 79 73 74 65 6D 2E 52 65 66 6C 65 63   // ...System.Reflec
                                                                                                                                                                       74 69 6F 6E 2E 45 6D 69 74 00 00 )                // tion.Emit..
  .custom /*0C000014:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0D 31 30 2E 30 2E 32 35 2E 35 32 34 31 31   // ...10.0.25.52411
                                                                                                                                                                       00 00 ) 
  .custom /*0C000015:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2F 31 30 2E 30 2E 30 2B 62 30 66 33 34 64   // ../10.0.0+b0f34d
                                                                                                                                                                                35 31 66 63 63 63 36 39 66 64 33 33 34 32 35 33   // 51fccc69fd334253
                                                                                                                                                                                39 32 34 61 62 64 38 64 36 38 35 33 66 61 64 37   // 924abd8d6853fad7
                                                                                                                                                                                61 61 00 00 )                                     // aa..
  .custom /*0C000016:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000017:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 16 53 79 73 74 65 6D 2E 52 65 66 6C 65 63   // ...System.Reflec
                                                                                                                                                                 74 69 6F 6E 2E 45 6D 69 74 00 00 )                // tion.Emit..
  .custom /*0C000018:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    20 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   //  https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 64 6F 74 6E 65   // com/dotnet/dotne
                                                                                                                                                                    74 00 00 )                                        // t..
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
  .ver 10:0:0:0
}
.module System.Reflection.Emit.dll
// MVID: {c142a2f8-cdd0-41f2-be76-3b3fc75ec2d6}
.custom /*0C000001:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000011*/::.ctor(int32) /* 0A000010 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000002:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000012*/::.ctor(bool) /* 0A000011 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public abstract auto ansi beforefieldinit System.Reflection.Emit.AssemblyBuilder
       extends [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/
{
  .custom /*0C00001A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000001*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AssemblyBuilder::.ctor

  .method /*06000002*/ public hidebysig specialname virtual 
          instance string  get_CodeBase() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000019:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2058
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::get_CodeBase

  .method /*06000003*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          get_EntryPoint() cil managed
  // SIG: 20 00 12 59
  {
    .custom /*0C00001D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x205b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::get_EntryPoint

  .method /*06000004*/ public hidebysig specialname virtual 
          instance string  get_FullName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000020:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x205e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::get_FullName

  .method /*06000005*/ public hidebysig specialname virtual 
          instance int64  get_HostContext() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2061
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::get_HostContext

  .method /*06000006*/ public hidebysig specialname virtual 
          instance bool  get_IsCollectible() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2064
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::get_IsCollectible

  .method /*06000007*/ public hidebysig specialname virtual 
          instance bool  get_IsDynamic() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2067
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::get_IsDynamic

  .method /*06000008*/ public hidebysig specialname virtual 
          instance string  get_Location() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x206a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::get_Location

  .method /*06000009*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ 
          get_ManifestModule() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x206d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::get_ManifestModule

  .method /*0600000A*/ public hidebysig specialname virtual 
          instance bool  get_ReflectionOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2070
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::get_ReflectionOnly

  .method /*0600000B*/ public hidebysig static 
          class System.Reflection.Emit.AssemblyBuilder/*02000002*/ 
          DefineDynamicAssembly(class [System.Runtime/*23000001*/]System.Reflection.AssemblyName/*01000019*/ name,
                                valuetype System.Reflection.Emit.AssemblyBuilderAccess/*02000003*/ access) cil managed
  // SIG: 00 02 12 08 12 65 11 0C
  {
    .custom /*0C000030:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000018*/::.ctor(string) /* 0A000014 */ = ( 01 00 32 44 65 66 69 6E 69 6E 67 20 61 20 64 79   // ..2Defining a dy
                                                                                                                                                                                       6E 61 6D 69 63 20 61 73 73 65 6D 62 6C 79 20 72   // namic assembly r
                                                                                                                                                                                       65 71 75 69 72 65 73 20 64 79 6E 61 6D 69 63 20   // equires dynamic 
                                                                                                                                                                                       63 6F 64 65 2E 00 00 )                            // code...
    // Method begins at RVA 0x2073
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::DefineDynamicAssembly

  .method /*0600000C*/ public hidebysig static 
          class System.Reflection.Emit.AssemblyBuilder/*02000002*/ 
          DefineDynamicAssembly(class [System.Runtime/*23000001*/]System.Reflection.AssemblyName/*01000019*/ name,
                                valuetype System.Reflection.Emit.AssemblyBuilderAccess/*02000003*/ access,
                                class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001A*/<class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001B*/> assemblyAttributes) cil managed
  // SIG: 00 03 12 08 12 65 11 0C 15 12 69 01 12 6D
  {
    .custom /*0C000033:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000018*/::.ctor(string) /* 0A000014 */ = ( 01 00 32 44 65 66 69 6E 69 6E 67 20 61 20 64 79   // ..2Defining a dy
                                                                                                                                                                                       6E 61 6D 69 63 20 61 73 73 65 6D 62 6C 79 20 72   // namic assembly r
                                                                                                                                                                                       65 71 75 69 72 65 73 20 64 79 6E 61 6D 69 63 20   // equires dynamic 
                                                                                                                                                                                       63 6F 64 65 2E 00 00 )                            // code...
    .param [3]/*08000005*/ 
    .custom /*0C000025:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2076
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::DefineDynamicAssembly

  .method /*0600000D*/ public hidebysig instance class System.Reflection.Emit.ModuleBuilder/*0200000A*/ 
          DefineDynamicModule(string name) cil managed
  // SIG: 20 01 12 28 0E
  {
    // Method begins at RVA 0x2079
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::DefineDynamicModule

  .method /*0600000E*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.ModuleBuilder/*0200000A*/ 
          DefineDynamicModuleCore(string name) cil managed
  // SIG: 20 01 12 28 0E
  {
    // Method begins at RVA 0x0
  } // end of method AssemblyBuilder::DefineDynamicModuleCore

  .method /*0600000F*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00003B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x207c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::Equals

  .method /*06000010*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(bool inherit) cil managed
  // SIG: 20 01 1D 1C 02
  {
    // Method begins at RVA 0x207f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetCustomAttributes

  .method /*06000011*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ attributeType,
                                                 bool inherit) cil managed
  // SIG: 20 02 1D 1C 12 75 02
  {
    // Method begins at RVA 0x2082
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetCustomAttributes

  .method /*06000012*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<class [System.Runtime/*23000001*/]System.Reflection.CustomAttributeData/*0100001F*/> 
          GetCustomAttributesData() cil managed
  // SIG: 20 00 15 12 79 01 12 7D
  {
    // Method begins at RVA 0x2085
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetCustomAttributesData

  .method /*06000013*/ public hidebysig instance class System.Reflection.Emit.ModuleBuilder/*0200000A*/ 
          GetDynamicModule(string name) cil managed
  // SIG: 20 01 12 28 0E
  {
    .param [0]/*0800000C*/ 
    .custom /*0C000036:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2088
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetDynamicModule

  .method /*06000014*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.ModuleBuilder/*0200000A*/ 
          GetDynamicModuleCore(string name) cil managed
  // SIG: 20 01 12 28 0E
  {
    .param [0]/*0800000E*/ 
    .custom /*0C00003A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method AssemblyBuilder::GetDynamicModuleCore

  .method /*06000015*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] 
          GetExportedTypes() cil managed
  // SIG: 20 00 1D 12 75
  {
    .custom /*0C00003E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 16 54 79 70 65 73 20 6D 69 67 68 74 20 62   // ...Types might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 00 00 )                // e removed..
    // Method begins at RVA 0x208b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetExportedTypes

  .method /*06000016*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.FileStream/*01000022*/ 
          GetFile(string name) cil managed
  // SIG: 20 01 12 80 89 0E
  {
    .custom /*0C000041:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresAssemblyFilesAttribute/*01000021*/::.ctor(string) /* 0A000017 */ = ( 01 00 4C 54 68 69 73 20 6D 65 6D 62 65 72 20 74   // ..LThis member t
                                                                                                                                                                                         68 72 6F 77 73 20 61 6E 20 65 78 63 65 70 74 69   // hrows an excepti
                                                                                                                                                                                         6F 6E 20 66 6F 72 20 61 73 73 65 6D 62 6C 69 65   // on for assemblie
                                                                                                                                                                                         73 20 65 6D 62 65 64 64 65 64 20 69 6E 20 61 20   // s embedded in a 
                                                                                                                                                                                         73 69 6E 67 6C 65 2D 66 69 6C 65 20 61 70 70 00   // single-file app.
                                                                                                                                                                                         00 ) 
    // Method begins at RVA 0x208e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetFile

  .method /*06000017*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.FileStream/*01000022*/[] 
          GetFiles(bool getResourceModules) cil managed
  // SIG: 20 01 1D 12 80 89 02
  {
    .custom /*0C000043:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresAssemblyFilesAttribute/*01000021*/::.ctor(string) /* 0A000017 */ = ( 01 00 4C 54 68 69 73 20 6D 65 6D 62 65 72 20 74   // ..LThis member t
                                                                                                                                                                                         68 72 6F 77 73 20 61 6E 20 65 78 63 65 70 74 69   // hrows an excepti
                                                                                                                                                                                         6F 6E 20 66 6F 72 20 61 73 73 65 6D 62 6C 69 65   // on for assemblie
                                                                                                                                                                                         73 20 65 6D 62 65 64 64 65 64 20 69 6E 20 61 20   // s embedded in a 
                                                                                                                                                                                         73 69 6E 67 6C 65 2D 66 69 6C 65 20 61 70 70 00   // single-file app.
                                                                                                                                                                                         00 ) 
    // Method begins at RVA 0x2091
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetFiles

  .method /*06000018*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2094
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetHashCode

  .method /*06000019*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/[] 
          GetLoadedModules(bool getResourceModules) cil managed
  // SIG: 20 01 1D 12 5D 02
  {
    // Method begins at RVA 0x2097
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetLoadedModules

  .method /*0600001A*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ManifestResourceInfo/*01000023*/ 
          GetManifestResourceInfo(string resourceName) cil managed
  // SIG: 20 01 12 80 8D 0E
  {
    .param [0]/*08000013*/ 
    .custom /*0C00003C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x209a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetManifestResourceInfo

  .method /*0600001B*/ public hidebysig virtual 
          instance string[]  GetManifestResourceNames() cil managed
  // SIG: 20 00 1D 0E
  {
    // Method begins at RVA 0x209d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetManifestResourceNames

  .method /*0600001C*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/ 
          GetManifestResourceStream(string name) cil managed
  // SIG: 20 01 12 80 91 0E
  {
    .param [0]/*08000015*/ 
    .custom /*0C00003F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetManifestResourceStream

  .method /*0600001D*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/ 
          GetManifestResourceStream(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 'type',
                                    string name) cil managed
  // SIG: 20 02 12 80 91 12 75 0E
  {
    .param [0]/*08000017*/ 
    .custom /*0C000044:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetManifestResourceStream

  .method /*0600001E*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ 
          GetModule(string name) cil managed
  // SIG: 20 01 12 5D 0E
  {
    .param [0]/*0800001A*/ 
    .custom /*0C000047:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetModule

  .method /*0600001F*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/[] 
          GetModules(bool getResourceModules) cil managed
  // SIG: 20 01 1D 12 5D 02
  {
    // Method begins at RVA 0x20a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetModules

  .method /*06000020*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.AssemblyName/*01000019*/ 
          GetName(bool copiedName) cil managed
  // SIG: 20 01 12 65 02
  {
    // Method begins at RVA 0x20ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetName

  .method /*06000021*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.AssemblyName/*01000019*/[] 
          GetReferencedAssemblies() cil managed
  // SIG: 20 00 1D 12 65
  {
    .custom /*0C00004A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 24 41 73 73 65 6D 62 6C 79 20 72 65 66 65   // ..$Assembly refe
                                                                                                                                                                                            72 65 6E 63 65 73 20 6D 69 67 68 74 20 62 65 20   // rences might be 
                                                                                                                                                                                            72 65 6D 6F 76 65 64 00 00 )                      // removed..
    // Method begins at RVA 0x20af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetReferencedAssemblies

  .method /*06000022*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ 
          GetSatelliteAssembly(class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000025*/ culture) cil managed
  // SIG: 20 01 12 55 12 80 95
  {
    // Method begins at RVA 0x20b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetSatelliteAssembly

  .method /*06000023*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ 
          GetSatelliteAssembly(class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000025*/ culture,
                               class [System.Runtime/*23000001*/]System.Version/*01000026*/ version) cil managed
  // SIG: 20 02 12 55 12 80 95 12 80 99
  {
    .param [2]/*08000020*/ 
    .custom /*0C000048:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetSatelliteAssembly

  .method /*06000024*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          GetType(string name,
                  bool throwOnError,
                  bool ignoreCase) cil managed
  // SIG: 20 03 12 75 0E 02 02
  {
    .custom /*0C00004D:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 6E 54 79 70 65 73 20 6D 69 67 68 74 20 62   // ..nTypes might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 20 62 79 20 74 72 69   // e removed by tri
                                                                                                                                                                                            6D 6D 69 6E 67 2E 20 49 66 20 74 68 65 20 74 79   // mming. If the ty
                                                                                                                                                                                            70 65 20 6E 61 6D 65 20 69 73 20 61 20 73 74 72   // pe name is a str
                                                                                                                                                                                            69 6E 67 20 6C 69 74 65 72 61 6C 2C 20 63 6F 6E   // ing literal, con
                                                                                                                                                                                            73 69 64 65 72 20 75 73 69 6E 67 20 54 79 70 65   // sider using Type
                                                                                                                                                                                            2E 47 65 74 54 79 70 65 20 69 6E 73 74 65 61 64   // .GetType instead
                                                                                                                                                                                            2E 00 00 )                                        // ...
    .param [0]/*08000021*/ 
    .custom /*0C00004B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::GetType

  .method /*06000025*/ public hidebysig virtual 
          instance bool  IsDefined(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ attributeType,
                                   bool inherit) cil managed
  // SIG: 20 02 02 12 75 02
  {
    // Method begins at RVA 0x20bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AssemblyBuilder::IsDefined

  .method /*06000026*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 1D 05
  {
    // Method begins at RVA 0x20be
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AssemblyBuilder::SetCustomAttribute

  .method /*06000027*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001B*/ customBuilder) cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x20c0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AssemblyBuilder::SetCustomAttribute

  .method /*06000028*/ family hidebysig newslot abstract virtual 
          instance void  SetCustomAttributeCore(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000028*/<uint8> binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 15 11 80 A1 01 05
  {
    .custom /*0C00004F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800002A*/ 
    .custom /*0C000050:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method AssemblyBuilder::SetCustomAttributeCore

  .property /*17000001*/ instance callconv(8) string
          CodeBase()
  {
    .custom /*0C000003:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000004:0A000018*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000029*/::.ctor(string) /* 0A000018 */ = ( 01 00 80 81 41 73 73 65 6D 62 6C 79 2E 43 6F 64   // ....Assembly.Cod
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
    .custom /*0C000005:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresAssemblyFilesAttribute/*01000021*/::.ctor(string) /* 0A000017 */ = ( 01 00 4C 54 68 69 73 20 6D 65 6D 62 65 72 20 74   // ..LThis member t
                                                                                                                                                                                         68 72 6F 77 73 20 61 6E 20 65 78 63 65 70 74 69   // hrows an excepti
                                                                                                                                                                                         6F 6E 20 66 6F 72 20 61 73 73 65 6D 62 6C 69 65   // on for assemblie
                                                                                                                                                                                         73 20 65 6D 62 65 64 64 65 64 20 69 6E 20 61 20   // s embedded in a 
                                                                                                                                                                                         73 69 6E 67 6C 65 2D 66 69 6C 65 20 61 70 70 00   // single-file app.
                                                                                                                                                                                         00 ) 
    .get instance string System.Reflection.Emit.AssemblyBuilder/*02000002*/::get_CodeBase() /* 06000002 */
  } // end of property AssemblyBuilder::CodeBase
  .property /*17000002*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/
          EntryPoint()
  {
    .custom /*0C00001C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ System.Reflection.Emit.AssemblyBuilder/*02000002*/::get_EntryPoint() /* 06000003 */
  } // end of property AssemblyBuilder::EntryPoint
  .property /*17000003*/ instance callconv(8) string
          FullName()
  {
    .custom /*0C00001F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.AssemblyBuilder/*02000002*/::get_FullName() /* 06000004 */
  } // end of property AssemblyBuilder::FullName
  .property /*17000004*/ instance callconv(8) int64
          HostContext()
  {
    .get instance int64 System.Reflection.Emit.AssemblyBuilder/*02000002*/::get_HostContext() /* 06000005 */
  } // end of property AssemblyBuilder::HostContext
  .property /*17000005*/ instance callconv(8) bool
          IsCollectible()
  {
    .get instance bool System.Reflection.Emit.AssemblyBuilder/*02000002*/::get_IsCollectible() /* 06000006 */
  } // end of property AssemblyBuilder::IsCollectible
  .property /*17000006*/ instance callconv(8) bool
          IsDynamic()
  {
    .get instance bool System.Reflection.Emit.AssemblyBuilder/*02000002*/::get_IsDynamic() /* 06000007 */
  } // end of property AssemblyBuilder::IsDynamic
  .property /*17000007*/ instance callconv(8) string
          Location()
  {
    .get instance string System.Reflection.Emit.AssemblyBuilder/*02000002*/::get_Location() /* 06000008 */
  } // end of property AssemblyBuilder::Location
  .property /*17000008*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/
          ManifestModule()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ System.Reflection.Emit.AssemblyBuilder/*02000002*/::get_ManifestModule() /* 06000009 */
  } // end of property AssemblyBuilder::ManifestModule
  .property /*17000009*/ instance callconv(8) bool
          ReflectionOnly()
  {
    .get instance bool System.Reflection.Emit.AssemblyBuilder/*02000002*/::get_ReflectionOnly() /* 0600000A */
  } // end of property AssemblyBuilder::ReflectionOnly
} // end of class System.Reflection.Emit.AssemblyBuilder

.class /*02000003*/ public auto ansi sealed System.Reflection.Emit.AssemblyBuilderAccess
       extends [System.Runtime/*23000001*/]System.Enum/*0100002B*/
{
  .custom /*0C00001E:0A000019*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
  .field /*04000001*/ public specialname rtspecialname int32 value__
  .field /*04000002*/ public static literal valuetype System.Reflection.Emit.AssemblyBuilderAccess/*02000003*/ Run = int32(0x00000001)
  .field /*04000003*/ public static literal valuetype System.Reflection.Emit.AssemblyBuilderAccess/*02000003*/ RunAndCollect = int32(0x00000009)
} // end of class System.Reflection.Emit.AssemblyBuilderAccess

.class /*02000004*/ public abstract auto ansi beforefieldinit System.Reflection.Emit.ConstructorBuilder
       extends [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/
{
  .custom /*0C000021:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000022:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000029*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20c2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ConstructorBuilder::.ctor

  .method /*0600002A*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ 
          get_Attributes() cil managed
  // SIG: 20 00 11 80 B1
  {
    // Method begins at RVA 0x20ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::get_Attributes

  .method /*0600002B*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ 
          get_CallingConvention() cil managed
  // SIG: 20 00 11 80 B5
  {
    // Method begins at RVA 0x20cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::get_CallingConvention

  .method /*0600002C*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_DeclaringType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C000051:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::get_DeclaringType

  .method /*0600002D*/ public hidebysig specialname 
          instance bool  get_InitLocals() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::get_InitLocals

  .method /*0600002E*/ public hidebysig specialname 
          instance void  set_InitLocals(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20d6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConstructorBuilder::set_InitLocals

  .method /*0600002F*/ family hidebysig newslot specialname abstract virtual 
          instance bool  get_InitLocalsCore() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method ConstructorBuilder::get_InitLocalsCore

  .method /*06000030*/ family hidebysig newslot specialname abstract virtual 
          instance void  set_InitLocalsCore(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x0
  } // end of method ConstructorBuilder::set_InitLocalsCore

  .method /*06000031*/ public hidebysig specialname virtual 
          instance int32  get_MetadataToken() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::get_MetadataToken

  .method /*06000032*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.RuntimeMethodHandle/*0100002E*/ 
          get_MethodHandle() cil managed
  // SIG: 20 00 11 80 B9
  {
    // Method begins at RVA 0x20db
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
    // Method begins at RVA 0x20de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::get_Module

  .method /*06000034*/ public hidebysig specialname virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::get_Name

  .method /*06000035*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_ReflectedType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C000059:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::get_ReflectedType

  .method /*06000036*/ public hidebysig instance class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.ParameterBuilder/*0100002F*/ 
          DefineParameter(int32 iSequence,
                          valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterAttributes/*01000030*/ attributes,
                          string strParamName) cil managed
  // SIG: 20 03 12 80 BD 08 11 80 C1 0E
  {
    .param [3]/*08000030*/ 
    .custom /*0C000054:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::DefineParameter

  .method /*06000037*/ family hidebysig newslot abstract virtual 
          instance class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.ParameterBuilder/*0100002F*/ 
          DefineParameterCore(int32 iSequence,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterAttributes/*01000030*/ attributes,
                              string strParamName) cil managed
  // SIG: 20 03 12 80 BD 08 11 80 C1 0E
  {
    // Method begins at RVA 0x0
  } // end of method ConstructorBuilder::DefineParameterCore

  .method /*06000038*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(bool inherit) cil managed
  // SIG: 20 01 1D 1C 02
  {
    // Method begins at RVA 0x20ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::GetCustomAttributes

  .method /*06000039*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ attributeType,
                                                 bool inherit) cil managed
  // SIG: 20 02 1D 1C 12 75 02
  {
    // Method begins at RVA 0x20ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::GetCustomAttributes

  .method /*0600003A*/ public hidebysig instance class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.ILGenerator/*01000031*/ 
          GetILGenerator() cil managed
  // SIG: 20 00 12 80 C5
  {
    // Method begins at RVA 0x20f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::GetILGenerator

  .method /*0600003B*/ public hidebysig instance class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.ILGenerator/*01000031*/ 
          GetILGenerator(int32 streamSize) cil managed
  // SIG: 20 01 12 80 C5 08
  {
    // Method begins at RVA 0x20f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::GetILGenerator

  .method /*0600003C*/ family hidebysig newslot abstract virtual 
          instance class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.ILGenerator/*01000031*/ 
          GetILGeneratorCore(int32 streamSize) cil managed
  // SIG: 20 01 12 80 C5 08
  {
    // Method begins at RVA 0x0
  } // end of method ConstructorBuilder::GetILGeneratorCore

  .method /*0600003D*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.MethodImplAttributes/*01000032*/ 
          GetMethodImplementationFlags() cil managed
  // SIG: 20 00 11 80 C9
  {
    // Method begins at RVA 0x20f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::GetMethodImplementationFlags

  .method /*0600003E*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*01000033*/[] 
          GetParameters() cil managed
  // SIG: 20 00 1D 12 80 CD
  {
    // Method begins at RVA 0x20f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::GetParameters

  .method /*0600003F*/ public hidebysig virtual 
          instance object  Invoke(object obj,
                                  valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ invokeAttr,
                                  class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000035*/ binder,
                                  object[] parameters,
                                  class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000025*/ culture) cil managed
  // SIG: 20 05 1C 1C 11 80 D1 12 80 D5 1D 1C 12 80 95
  {
    .custom /*0C00005B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000039*/ 
    .custom /*0C00005A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::Invoke

  .method /*06000040*/ public hidebysig virtual 
          instance object  Invoke(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ invokeAttr,
                                  class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000035*/ binder,
                                  object[] parameters,
                                  class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000025*/ culture) cil managed
  // SIG: 20 04 1C 11 80 D1 12 80 D5 1D 1C 12 80 95
  {
    .custom /*0C00005D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800003F*/ 
    .custom /*0C00005C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::Invoke

  .method /*06000041*/ public hidebysig virtual 
          instance bool  IsDefined(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ attributeType,
                                   bool inherit) cil managed
  // SIG: 20 02 02 12 75 02
  {
    // Method begins at RVA 0x2102
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::IsDefined

  .method /*06000042*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 1D 05
  {
    // Method begins at RVA 0x2105
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConstructorBuilder::SetCustomAttribute

  .method /*06000043*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001B*/ customBuilder) cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x2107
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConstructorBuilder::SetCustomAttribute

  .method /*06000044*/ family hidebysig newslot abstract virtual 
          instance void  SetCustomAttributeCore(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000028*/<uint8> binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 15 11 80 A1 01 05
  {
    .custom /*0C000060:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000049*/ 
    .custom /*0C000061:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ConstructorBuilder::SetCustomAttributeCore

  .method /*06000045*/ public hidebysig instance void 
          SetImplementationFlags(valuetype [System.Runtime/*23000001*/]System.Reflection.MethodImplAttributes/*01000032*/ attributes) cil managed
  // SIG: 20 01 01 11 80 C9
  {
    // Method begins at RVA 0x2109
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ConstructorBuilder::SetImplementationFlags

  .method /*06000046*/ family hidebysig newslot abstract virtual 
          instance void  SetImplementationFlagsCore(valuetype [System.Runtime/*23000001*/]System.Reflection.MethodImplAttributes/*01000032*/ attributes) cil managed
  // SIG: 20 01 01 11 80 C9
  {
    // Method begins at RVA 0x0
  } // end of method ConstructorBuilder::SetImplementationFlagsCore

  .method /*06000047*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x210b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConstructorBuilder::ToString

  .property /*1700000A*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/
          Attributes()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ System.Reflection.Emit.ConstructorBuilder/*02000004*/::get_Attributes() /* 0600002A */
  } // end of property ConstructorBuilder::Attributes
  .property /*1700000B*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/
          CallingConvention()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ System.Reflection.Emit.ConstructorBuilder/*02000004*/::get_CallingConvention() /* 0600002B */
  } // end of property ConstructorBuilder::CallingConvention
  .property /*1700000C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          DeclaringType()
  {
    .custom /*0C000037:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.ConstructorBuilder/*02000004*/::get_DeclaringType() /* 0600002C */
  } // end of property ConstructorBuilder::DeclaringType
  .property /*1700000D*/ instance callconv(8) bool
          InitLocals()
  {
    .get instance bool System.Reflection.Emit.ConstructorBuilder/*02000004*/::get_InitLocals() /* 0600002D */
    .set instance void System.Reflection.Emit.ConstructorBuilder/*02000004*/::set_InitLocals(bool) /* 0600002E */
  } // end of property ConstructorBuilder::InitLocals
  .property /*1700000E*/ instance callconv(8) bool
          InitLocalsCore()
  {
    .get instance bool System.Reflection.Emit.ConstructorBuilder/*02000004*/::get_InitLocalsCore() /* 0600002F */
    .set instance void System.Reflection.Emit.ConstructorBuilder/*02000004*/::set_InitLocalsCore(bool) /* 06000030 */
  } // end of property ConstructorBuilder::InitLocalsCore
  .property /*1700000F*/ instance callconv(8) int32
          MetadataToken()
  {
    .get instance int32 System.Reflection.Emit.ConstructorBuilder/*02000004*/::get_MetadataToken() /* 06000031 */
  } // end of property ConstructorBuilder::MetadataToken
  .property /*17000010*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.RuntimeMethodHandle/*0100002E*/
          MethodHandle()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.RuntimeMethodHandle/*0100002E*/ System.Reflection.Emit.ConstructorBuilder/*02000004*/::get_MethodHandle() /* 06000032 */
  } // end of property ConstructorBuilder::MethodHandle
  .property /*17000011*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/
          Module()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ System.Reflection.Emit.ConstructorBuilder/*02000004*/::get_Module() /* 06000033 */
  } // end of property ConstructorBuilder::Module
  .property /*17000012*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Reflection.Emit.ConstructorBuilder/*02000004*/::get_Name() /* 06000034 */
  } // end of property ConstructorBuilder::Name
  .property /*17000013*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          ReflectedType()
  {
    .custom /*0C00003D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.ConstructorBuilder/*02000004*/::get_ReflectedType() /* 06000035 */
  } // end of property ConstructorBuilder::ReflectedType
} // end of class System.Reflection.Emit.ConstructorBuilder

.class /*02000005*/ public abstract auto ansi beforefieldinit System.Reflection.Emit.EnumBuilder
       extends [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000036*/
{
  .custom /*0C000023:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000024:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000048*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x210e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000036*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EnumBuilder::.ctor

  .method /*06000049*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ 
          get_Assembly() cil managed
  // SIG: 20 00 12 55
  {
    // Method begins at RVA 0x2116
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_Assembly

  .method /*0600004A*/ public hidebysig specialname virtual 
          instance string  get_AssemblyQualifiedName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000063:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2119
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_AssemblyQualifiedName

  .method /*0600004B*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_BaseType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C000064:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_BaseType

  .method /*0600004C*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_DeclaringType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C000065:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_DeclaringType

  .method /*0600004D*/ public hidebysig specialname virtual 
          instance string  get_FullName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000066:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2122
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_FullName

  .method /*0600004E*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*01000037*/ 
          get_GUID() cil managed
  // SIG: 20 00 11 80 DD
  {
    // Method begins at RVA 0x2125
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_GUID

  .method /*0600004F*/ public hidebysig specialname virtual 
          instance bool  get_IsByRefLike() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2128
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_IsByRefLike

  .method /*06000050*/ public hidebysig specialname virtual 
          instance bool  get_IsConstructedGenericType() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x212b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_IsConstructedGenericType

  .method /*06000051*/ public hidebysig specialname virtual 
          instance bool  get_IsSZArray() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x212e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_IsSZArray

  .method /*06000052*/ public hidebysig specialname virtual 
          instance bool  get_IsTypeDefinition() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2131
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_IsTypeDefinition

  .method /*06000053*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ 
          get_Module() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x2134
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_Module

  .method /*06000054*/ public hidebysig specialname virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2137
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_Name

  .method /*06000055*/ public hidebysig specialname virtual 
          instance string  get_Namespace() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00006C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x213a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_Namespace

  .method /*06000056*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_ReflectedType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C00006E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x213d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_ReflectedType

  .method /*06000057*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000038*/ 
          get_TypeHandle() cil managed
  // SIG: 20 00 11 80 E1
  {
    // Method begins at RVA 0x2140
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_TypeHandle

  .method /*06000058*/ public hidebysig specialname 
          instance class System.Reflection.Emit.FieldBuilder/*02000007*/ 
          get_UnderlyingField() cil managed
  // SIG: 20 00 12 1C
  {
    // Method begins at RVA 0x2143
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_UnderlyingField

  .method /*06000059*/ family hidebysig newslot specialname abstract virtual 
          instance class System.Reflection.Emit.FieldBuilder/*02000007*/ 
          get_UnderlyingFieldCore() cil managed
  // SIG: 20 00 12 1C
  {
    // Method begins at RVA 0x0
  } // end of method EnumBuilder::get_UnderlyingFieldCore

  .method /*0600005A*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_UnderlyingSystemType() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x2146
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::get_UnderlyingSystemType

  .method /*0600005B*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          CreateType() cil managed
  // SIG: 20 00 12 75
  {
    .param [0]/*0800004D*/ 
    .custom /*0C000067:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x2149
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::CreateType

  .method /*0600005C*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000036*/ 
          CreateTypeInfo() cil managed
  // SIG: 20 00 12 80 D9
  {
    .param [0]/*0800004E*/ 
    .custom /*0C000068:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x214c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::CreateTypeInfo

  .method /*0600005D*/ family hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000036*/ 
          CreateTypeInfoCore() cil managed
  // SIG: 20 00 12 80 D9
  {
    .param [0]/*0800004F*/ 
    .custom /*0C000069:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method EnumBuilder::CreateTypeInfoCore

  .method /*0600005E*/ public hidebysig instance class System.Reflection.Emit.FieldBuilder/*02000007*/ 
          DefineLiteral(string literalName,
                        object literalValue) cil managed
  // SIG: 20 02 12 1C 0E 1C
  {
    .param [2]/*08000051*/ 
    .custom /*0C00006A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x214f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::DefineLiteral

  .method /*0600005F*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.FieldBuilder/*02000007*/ 
          DefineLiteralCore(string literalName,
                            object literalValue) cil managed
  // SIG: 20 02 12 1C 0E 1C
  {
    .param [2]/*08000053*/ 
    .custom /*0C00006B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method EnumBuilder::DefineLiteralCore

  .method /*06000060*/ family hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*0100003B*/ 
          GetAttributeFlagsImpl() cil managed
  // SIG: 20 00 11 80 ED
  {
    // Method begins at RVA 0x2152
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetAttributeFlagsImpl

  .method /*06000061*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ 
          GetConstructorImpl(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr,
                             class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000035*/ binder,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callConvention,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] types,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003C*/[] modifiers) cil managed
  // SIG: 20 05 12 80 9D 11 80 D1 12 80 D5 11 80 B5 1D 12 75 1D 11 80 F1
  {
    .custom /*0C000074:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000075:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 07 00 00 00 00 00 ) 
    .param [4]/*08000057*/ 
    .custom /*0C00006F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2155
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetConstructorImpl

  .method /*06000062*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/[] 
          GetConstructors(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 9D 11 80 D1
  {
    .custom /*0C000077:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 07 00 00 00 00 00 ) 
    // Method begins at RVA 0x2158
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetConstructors

  .method /*06000063*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(bool inherit) cil managed
  // SIG: 20 01 1D 1C 02
  {
    // Method begins at RVA 0x215b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetCustomAttributes

  .method /*06000064*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ attributeType,
                                                 bool inherit) cil managed
  // SIG: 20 02 1D 1C 12 75 02
  {
    // Method begins at RVA 0x215e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetCustomAttributes

  .method /*06000065*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          GetElementType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C000078:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2161
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetElementType

  .method /*06000066*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          GetEnumUnderlyingType() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x2164
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetEnumUnderlyingType

  .method /*06000067*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100003D*/ 
          GetEvent(string name,
                   valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 02 12 80 F5 0E 11 80 D1
  {
    .custom /*0C00007C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 18 00 00 00 00 ) 
    .param [0]/*0800005D*/ 
    .custom /*0C000071:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2167
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetEvent

  .method /*06000068*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100003D*/[] 
          GetEvents() cil managed
  // SIG: 20 00 1D 12 80 F5
  {
    .custom /*0C00007D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 08 00 00 00 00 ) 
    // Method begins at RVA 0x216a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetEvents

  .method /*06000069*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100003D*/[] 
          GetEvents(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 F5 11 80 D1
  {
    .custom /*0C00007F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 18 00 00 00 00 ) 
    // Method begins at RVA 0x216d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetEvents

  .method /*0600006A*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003E*/ 
          GetField(string name,
                   valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 02 12 80 F9 0E 11 80 D1
  {
    .custom /*0C000081:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 60 00 00 00 00 00 )                         // ..`.....
    .param [0]/*08000061*/ 
    .custom /*0C000076:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2170
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetField

  .method /*0600006B*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003E*/[] 
          GetFields(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 F9 11 80 D1
  {
    .custom /*0C000082:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 60 00 00 00 00 00 )                         // ..`.....
    // Method begins at RVA 0x2173
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetFields

  .method /*0600006C*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          GetInterface(string name,
                       bool ignoreCase) cil managed
  // SIG: 20 02 12 75 0E 02
  {
    .custom /*0C000084:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 20 00 00 00 00 )                         // ... ....
    .param [0]/*08000065*/ 
    .custom /*0C000079:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00007A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 20 00 00 00 00 )                         // ... ....
    // Method begins at RVA 0x2176
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetInterface

  .method /*0600006D*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.InterfaceMapping/*0100003F*/ 
          GetInterfaceMap(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ interfaceType) cil managed
  // SIG: 20 01 11 80 FD 12 75
  {
    .param [1]/*08000068*/ 
    .custom /*0C00007E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 18 00 00 00 00 00 ) 
    // Method begins at RVA 0x2179
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetInterfaceMap

  .method /*0600006E*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] 
          GetInterfaces() cil managed
  // SIG: 20 00 1D 12 75
  {
    .custom /*0C000088:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 20 00 00 00 00 )                         // ... ....
    // Method begins at RVA 0x217c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetInterfaces

  .method /*0600006F*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000040*/[] 
          GetMember(string name,
                    valuetype [System.Runtime/*23000001*/]System.Reflection.MemberTypes/*01000041*/ 'type',
                    valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 03 1D 12 81 01 0E 11 81 05 11 80 D1
  {
    .custom /*0C00008A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF 1F 00 00 00 00 ) 
    // Method begins at RVA 0x217f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetMember

  .method /*06000070*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000040*/[] 
          GetMembers(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 81 01 11 80 D1
  {
    .custom /*0C00008C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF 1F 00 00 00 00 ) 
    // Method begins at RVA 0x2182
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetMembers

  .method /*06000071*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetMethodImpl(string name,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr,
                        class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000035*/ binder,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callConvention,
                        class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] types,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003C*/[] modifiers) cil managed
  // SIG: 20 06 12 59 0E 11 80 D1 12 80 D5 11 80 B5 1D 12 75 1D 11 80 F1
  {
    .custom /*0C00008D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00008E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 18 00 00 00 00 00 ) 
    .param [1]/*0800006D*/ 
    .custom /*0C000086:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [5]/*08000071*/ 
    .custom /*0C00008F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2185
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetMethodImpl

  .method /*06000072*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/[] 
          GetMethods(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 59 11 80 D1
  {
    .custom /*0C000090:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 18 00 00 00 00 00 ) 
    // Method begins at RVA 0x2188
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetMethods

  .method /*06000073*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          GetNestedType(string name,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 02 12 75 0E 11 80 D1
  {
    .custom /*0C000091:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 80 01 00 00 00 00 ) 
    .param [0]/*08000074*/ 
    .custom /*0C000093:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x218b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetNestedType

  .method /*06000074*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] 
          GetNestedTypes(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 75 11 80 D1
  {
    .custom /*0C000092:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 80 01 00 00 00 00 ) 
    // Method begins at RVA 0x218e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetNestedTypes

  .method /*06000075*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000042*/[] 
          GetProperties(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 81 09 11 80 D1
  {
    .custom /*0C000094:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 06 00 00 00 00 ) 
    // Method begins at RVA 0x2191
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetProperties

  .method /*06000076*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000042*/ 
          GetPropertyImpl(string name,
                          valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr,
                          class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000035*/ binder,
                          class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                          class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] types,
                          valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003C*/[] modifiers) cil managed
  // SIG: 20 06 12 81 09 0E 11 80 D1 12 80 D5 12 75 1D 12 75 1D 11 80 F1
  {
    .custom /*0C000095:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000096:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 06 00 00 00 00 ) 
    .param [0]/*08000079*/ 
    .custom /*0C000099:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800007A*/ 
    .custom /*0C00009B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [5]/*0800007E*/ 
    .custom /*0C00009C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2194
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::GetPropertyImpl

  .method /*06000077*/ family hidebysig virtual 
          instance bool  HasElementTypeImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2197
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::HasElementTypeImpl

  .method /*06000078*/ public hidebysig virtual 
          instance object  InvokeMember(string name,
                                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ invokeAttr,
                                        class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000035*/ binder,
                                        object target,
                                        object[] args,
                                        valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003C*/[] modifiers,
                                        class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000025*/ culture,
                                        string[] namedParameters) cil managed
  // SIG: 20 08 1C 0E 11 80 D1 12 80 D5 1C 1D 1C 1D 11 80 F1 12 80 95 1D 0E
  {
    .custom /*0C000097:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000098:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 7F 06 00 00 00 00 ) 
    .param [1]/*08000080*/ 
    .custom /*0C00009D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [8]/*08000087*/ 
    .custom /*0C0000A3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x219a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::InvokeMember

  .method /*06000079*/ family hidebysig virtual 
          instance bool  IsArrayImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x219d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::IsArrayImpl

  .method /*0600007A*/ public hidebysig virtual 
          instance bool  IsAssignableFrom(class [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000036*/ typeInfo) cil managed
  // SIG: 20 01 02 12 80 D9
  {
    .custom /*0C00009A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000088*/ 
    .custom /*0C0000A4:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000043*/::.ctor(bool) /* 0A00001B */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::IsAssignableFrom

  .method /*0600007B*/ family hidebysig virtual 
          instance bool  IsByRefImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::IsByRefImpl

  .method /*0600007C*/ family hidebysig virtual 
          instance bool  IsCOMObjectImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::IsCOMObjectImpl

  .method /*0600007D*/ public hidebysig virtual 
          instance bool  IsDefined(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ attributeType,
                                   bool inherit) cil managed
  // SIG: 20 02 02 12 75 02
  {
    // Method begins at RVA 0x21a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::IsDefined

  .method /*0600007E*/ family hidebysig virtual 
          instance bool  IsPointerImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::IsPointerImpl

  .method /*0600007F*/ family hidebysig virtual 
          instance bool  IsPrimitiveImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::IsPrimitiveImpl

  .method /*06000080*/ family hidebysig virtual 
          instance bool  IsValueTypeImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::IsValueTypeImpl

  .method /*06000081*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          MakeArrayType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C00009F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000018*/::.ctor(string) /* 0A000014 */ = ( 01 00 43 54 68 65 20 63 6F 64 65 20 66 6F 72 20   // ..CThe code for 
                                                                                                                                                                                       61 6E 20 61 72 72 61 79 20 6F 66 20 74 68 65 20   // an array of the 
                                                                                                                                                                                       73 70 65 63 69 66 69 65 64 20 74 79 70 65 20 6D   // specified type m
                                                                                                                                                                                       69 67 68 74 20 6E 6F 74 20 62 65 20 61 76 61 69   // ight not be avai
                                                                                                                                                                                       6C 61 62 6C 65 2E 00 00 )                         // lable...
    // Method begins at RVA 0x21b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::MakeArrayType

  .method /*06000082*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          MakeArrayType(int32 rank) cil managed
  // SIG: 20 01 12 75 08
  {
    .custom /*0C0000A0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000018*/::.ctor(string) /* 0A000014 */ = ( 01 00 43 54 68 65 20 63 6F 64 65 20 66 6F 72 20   // ..CThe code for 
                                                                                                                                                                                       61 6E 20 61 72 72 61 79 20 6F 66 20 74 68 65 20   // an array of the 
                                                                                                                                                                                       73 70 65 63 69 66 69 65 64 20 74 79 70 65 20 6D   // specified type m
                                                                                                                                                                                       69 67 68 74 20 6E 6F 74 20 62 65 20 61 76 61 69   // ight not be avai
                                                                                                                                                                                       6C 61 62 6C 65 2E 00 00 )                         // lable...
    // Method begins at RVA 0x21b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::MakeArrayType

  .method /*06000083*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          MakeByRefType() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x21bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::MakeByRefType

  .method /*06000084*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          MakePointerType() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x21be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EnumBuilder::MakePointerType

  .method /*06000085*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 1D 05
  {
    // Method begins at RVA 0x21c1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EnumBuilder::SetCustomAttribute

  .method /*06000086*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001B*/ customBuilder) cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x21c3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EnumBuilder::SetCustomAttribute

  .method /*06000087*/ family hidebysig newslot abstract virtual 
          instance void  SetCustomAttributeCore(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000028*/<uint8> binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 15 11 80 A1 01 05
  {
    .custom /*0C0000A2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800008F*/ 
    .custom /*0C0000A6:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method EnumBuilder::SetCustomAttributeCore

  .property /*17000014*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/
          Assembly()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ System.Reflection.Emit.EnumBuilder/*02000005*/::get_Assembly() /* 06000049 */
  } // end of property EnumBuilder::Assembly
  .property /*17000015*/ instance callconv(8) string
          AssemblyQualifiedName()
  {
    .custom /*0C000040:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.EnumBuilder/*02000005*/::get_AssemblyQualifiedName() /* 0600004A */
  } // end of property EnumBuilder::AssemblyQualifiedName
  .property /*17000016*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          BaseType()
  {
    .custom /*0C000042:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.EnumBuilder/*02000005*/::get_BaseType() /* 0600004B */
  } // end of property EnumBuilder::BaseType
  .property /*17000017*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          DeclaringType()
  {
    .custom /*0C000045:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.EnumBuilder/*02000005*/::get_DeclaringType() /* 0600004C */
  } // end of property EnumBuilder::DeclaringType
  .property /*17000018*/ instance callconv(8) string
          FullName()
  {
    .custom /*0C000046:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.EnumBuilder/*02000005*/::get_FullName() /* 0600004D */
  } // end of property EnumBuilder::FullName
  .property /*17000019*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Guid/*01000037*/
          GUID()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Guid/*01000037*/ System.Reflection.Emit.EnumBuilder/*02000005*/::get_GUID() /* 0600004E */
  } // end of property EnumBuilder::GUID
  .property /*1700001A*/ instance callconv(8) bool
          IsByRefLike()
  {
    .get instance bool System.Reflection.Emit.EnumBuilder/*02000005*/::get_IsByRefLike() /* 0600004F */
  } // end of property EnumBuilder::IsByRefLike
  .property /*1700001B*/ instance callconv(8) bool
          IsConstructedGenericType()
  {
    .get instance bool System.Reflection.Emit.EnumBuilder/*02000005*/::get_IsConstructedGenericType() /* 06000050 */
  } // end of property EnumBuilder::IsConstructedGenericType
  .property /*1700001C*/ instance callconv(8) bool
          IsSZArray()
  {
    .get instance bool System.Reflection.Emit.EnumBuilder/*02000005*/::get_IsSZArray() /* 06000051 */
  } // end of property EnumBuilder::IsSZArray
  .property /*1700001D*/ instance callconv(8) bool
          IsTypeDefinition()
  {
    .get instance bool System.Reflection.Emit.EnumBuilder/*02000005*/::get_IsTypeDefinition() /* 06000052 */
  } // end of property EnumBuilder::IsTypeDefinition
  .property /*1700001E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/
          Module()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ System.Reflection.Emit.EnumBuilder/*02000005*/::get_Module() /* 06000053 */
  } // end of property EnumBuilder::Module
  .property /*1700001F*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Reflection.Emit.EnumBuilder/*02000005*/::get_Name() /* 06000054 */
  } // end of property EnumBuilder::Name
  .property /*17000020*/ instance callconv(8) string
          Namespace()
  {
    .custom /*0C000049:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.EnumBuilder/*02000005*/::get_Namespace() /* 06000055 */
  } // end of property EnumBuilder::Namespace
  .property /*17000021*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          ReflectedType()
  {
    .custom /*0C00004C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.EnumBuilder/*02000005*/::get_ReflectedType() /* 06000056 */
  } // end of property EnumBuilder::ReflectedType
  .property /*17000022*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000038*/
          TypeHandle()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000038*/ System.Reflection.Emit.EnumBuilder/*02000005*/::get_TypeHandle() /* 06000057 */
  } // end of property EnumBuilder::TypeHandle
  .property /*17000023*/ instance callconv(8) class System.Reflection.Emit.FieldBuilder/*02000007*/
          UnderlyingField()
  {
    .get instance class System.Reflection.Emit.FieldBuilder/*02000007*/ System.Reflection.Emit.EnumBuilder/*02000005*/::get_UnderlyingField() /* 06000058 */
  } // end of property EnumBuilder::UnderlyingField
  .property /*17000024*/ instance callconv(8) class System.Reflection.Emit.FieldBuilder/*02000007*/
          UnderlyingFieldCore()
  {
    .get instance class System.Reflection.Emit.FieldBuilder/*02000007*/ System.Reflection.Emit.EnumBuilder/*02000005*/::get_UnderlyingFieldCore() /* 06000059 */
  } // end of property EnumBuilder::UnderlyingFieldCore
  .property /*17000025*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          UnderlyingSystemType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.EnumBuilder/*02000005*/::get_UnderlyingSystemType() /* 0600005A */
  } // end of property EnumBuilder::UnderlyingSystemType
} // end of class System.Reflection.Emit.EnumBuilder

.class /*02000006*/ public abstract auto ansi beforefieldinit System.Reflection.Emit.EventBuilder
       extends [System.Runtime/*23000001*/]System.Object/*0100001C*/
{
  .custom /*0C000026:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000027:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000088*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21c5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001C*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EventBuilder::.ctor

  .method /*06000089*/ public hidebysig instance void 
          AddOtherMethod(class System.Reflection.Emit.MethodBuilder/*02000009*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x21cd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EventBuilder::AddOtherMethod

  .method /*0600008A*/ family hidebysig newslot abstract virtual 
          instance void  AddOtherMethodCore(class System.Reflection.Emit.MethodBuilder/*02000009*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x0
  } // end of method EventBuilder::AddOtherMethodCore

  .method /*0600008B*/ public hidebysig instance void 
          SetAddOnMethod(class System.Reflection.Emit.MethodBuilder/*02000009*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x21cf
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EventBuilder::SetAddOnMethod

  .method /*0600008C*/ family hidebysig newslot abstract virtual 
          instance void  SetAddOnMethodCore(class System.Reflection.Emit.MethodBuilder/*02000009*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x0
  } // end of method EventBuilder::SetAddOnMethodCore

  .method /*0600008D*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 1D 05
  {
    // Method begins at RVA 0x21d1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EventBuilder::SetCustomAttribute

  .method /*0600008E*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001B*/ customBuilder) cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x21d3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EventBuilder::SetCustomAttribute

  .method /*0600008F*/ family hidebysig newslot abstract virtual 
          instance void  SetCustomAttributeCore(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000028*/<uint8> binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 15 11 80 A1 01 05
  {
    .custom /*0C0000A5:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000098*/ 
    .custom /*0C0000A8:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method EventBuilder::SetCustomAttributeCore

  .method /*06000090*/ public hidebysig instance void 
          SetRaiseMethod(class System.Reflection.Emit.MethodBuilder/*02000009*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x21d5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EventBuilder::SetRaiseMethod

  .method /*06000091*/ family hidebysig newslot abstract virtual 
          instance void  SetRaiseMethodCore(class System.Reflection.Emit.MethodBuilder/*02000009*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x0
  } // end of method EventBuilder::SetRaiseMethodCore

  .method /*06000092*/ public hidebysig instance void 
          SetRemoveOnMethod(class System.Reflection.Emit.MethodBuilder/*02000009*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x21d7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EventBuilder::SetRemoveOnMethod

  .method /*06000093*/ family hidebysig newslot abstract virtual 
          instance void  SetRemoveOnMethodCore(class System.Reflection.Emit.MethodBuilder/*02000009*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x0
  } // end of method EventBuilder::SetRemoveOnMethodCore

} // end of class System.Reflection.Emit.EventBuilder

.class /*02000007*/ public abstract auto ansi beforefieldinit System.Reflection.Emit.FieldBuilder
       extends [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003E*/
{
  .custom /*0C000028:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000029:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000094*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21d9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003E*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method FieldBuilder::.ctor

  .method /*06000095*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000044*/ 
          get_Attributes() cil managed
  // SIG: 20 00 11 81 11
  {
    // Method begins at RVA 0x21e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::get_Attributes

  .method /*06000096*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_DeclaringType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C0000A7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::get_DeclaringType

  .method /*06000097*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.RuntimeFieldHandle/*01000045*/ 
          get_FieldHandle() cil managed
  // SIG: 20 00 11 81 15
  {
    // Method begins at RVA 0x21e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::get_FieldHandle

  .method /*06000098*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_FieldType() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x21ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::get_FieldType

  .method /*06000099*/ public hidebysig specialname virtual 
          instance int32  get_MetadataToken() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::get_MetadataToken

  .method /*0600009A*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ 
          get_Module() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x21f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::get_Module

  .method /*0600009B*/ public hidebysig specialname virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::get_Name

  .method /*0600009C*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_ReflectedType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C0000A9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::get_ReflectedType

  .method /*0600009D*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(bool inherit) cil managed
  // SIG: 20 01 1D 1C 02
  {
    // Method begins at RVA 0x21f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::GetCustomAttributes

  .method /*0600009E*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ attributeType,
                                                 bool inherit) cil managed
  // SIG: 20 02 1D 1C 12 75 02
  {
    // Method begins at RVA 0x21fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::GetCustomAttributes

  .method /*0600009F*/ public hidebysig virtual 
          instance object  GetValue(object obj) cil managed
  // SIG: 20 01 1C 1C
  {
    .custom /*0C0000AA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::GetValue

  .method /*060000A0*/ public hidebysig virtual 
          instance bool  IsDefined(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ attributeType,
                                   bool inherit) cil managed
  // SIG: 20 02 02 12 75 02
  {
    // Method begins at RVA 0x2202
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FieldBuilder::IsDefined

  .method /*060000A1*/ public hidebysig instance void 
          SetConstant(object defaultValue) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C0000AB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2205
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FieldBuilder::SetConstant

  .method /*060000A2*/ family hidebysig newslot abstract virtual 
          instance void  SetConstantCore(object defaultValue) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C0000AC:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method FieldBuilder::SetConstantCore

  .method /*060000A3*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 1D 05
  {
    // Method begins at RVA 0x2207
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FieldBuilder::SetCustomAttribute

  .method /*060000A4*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001B*/ customBuilder) cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x2209
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FieldBuilder::SetCustomAttribute

  .method /*060000A5*/ family hidebysig newslot abstract virtual 
          instance void  SetCustomAttributeCore(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000028*/<uint8> binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 15 11 80 A1 01 05
  {
    .custom /*0C0000AD:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    .param [1]/*080000A9*/ 
    .custom /*0C0000AF:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method FieldBuilder::SetCustomAttributeCore

  .method /*060000A6*/ public hidebysig instance void 
          SetOffset(int32 iOffset) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x220b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FieldBuilder::SetOffset

  .method /*060000A7*/ family hidebysig newslot abstract virtual 
          instance void  SetOffsetCore(int32 iOffset) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x0
  } // end of method FieldBuilder::SetOffsetCore

  .method /*060000A8*/ public hidebysig virtual 
          instance void  SetValue(object obj,
                                  object val,
                                  valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ invokeAttr,
                                  class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000035*/ binder,
                                  class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000025*/ culture) cil managed
  // SIG: 20 05 01 1C 1C 11 80 D1 12 80 D5 12 80 95
  {
    .custom /*0C0000AE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x220d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FieldBuilder::SetValue

  .property /*17000026*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000044*/
          Attributes()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000044*/ System.Reflection.Emit.FieldBuilder/*02000007*/::get_Attributes() /* 06000095 */
  } // end of property FieldBuilder::Attributes
  .property /*17000027*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          DeclaringType()
  {
    .custom /*0C00004E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.FieldBuilder/*02000007*/::get_DeclaringType() /* 06000096 */
  } // end of property FieldBuilder::DeclaringType
  .property /*17000028*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.RuntimeFieldHandle/*01000045*/
          FieldHandle()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.RuntimeFieldHandle/*01000045*/ System.Reflection.Emit.FieldBuilder/*02000007*/::get_FieldHandle() /* 06000097 */
  } // end of property FieldBuilder::FieldHandle
  .property /*17000029*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          FieldType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.FieldBuilder/*02000007*/::get_FieldType() /* 06000098 */
  } // end of property FieldBuilder::FieldType
  .property /*1700002A*/ instance callconv(8) int32
          MetadataToken()
  {
    .get instance int32 System.Reflection.Emit.FieldBuilder/*02000007*/::get_MetadataToken() /* 06000099 */
  } // end of property FieldBuilder::MetadataToken
  .property /*1700002B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/
          Module()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ System.Reflection.Emit.FieldBuilder/*02000007*/::get_Module() /* 0600009A */
  } // end of property FieldBuilder::Module
  .property /*1700002C*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Reflection.Emit.FieldBuilder/*02000007*/::get_Name() /* 0600009B */
  } // end of property FieldBuilder::Name
  .property /*1700002D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          ReflectedType()
  {
    .custom /*0C000052:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.FieldBuilder/*02000007*/::get_ReflectedType() /* 0600009C */
  } // end of property FieldBuilder::ReflectedType
} // end of class System.Reflection.Emit.FieldBuilder

.class /*02000008*/ public abstract auto ansi beforefieldinit System.Reflection.Emit.GenericTypeParameterBuilder
       extends [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000036*/
{
  .custom /*0C00002A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*060000A9*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x220f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000036*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GenericTypeParameterBuilder::.ctor

  .method /*060000AA*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ 
          get_Assembly() cil managed
  // SIG: 20 00 12 55
  {
    // Method begins at RVA 0x2217
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_Assembly

  .method /*060000AB*/ public hidebysig specialname virtual 
          instance string  get_AssemblyQualifiedName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000B0:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x221a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_AssemblyQualifiedName

  .method /*060000AC*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_BaseType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C0000B1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x221d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_BaseType

  .method /*060000AD*/ public hidebysig specialname virtual 
          instance bool  get_ContainsGenericParameters() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2220
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_ContainsGenericParameters

  .method /*060000AE*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodBase/*01000046*/ 
          get_DeclaringMethod() cil managed
  // SIG: 20 00 12 81 19
  {
    .custom /*0C0000B2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2223
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_DeclaringMethod

  .method /*060000AF*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_DeclaringType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C0000B3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2226
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_DeclaringType

  .method /*060000B0*/ public hidebysig specialname virtual 
          instance string  get_FullName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000B4:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2229
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_FullName

  .method /*060000B1*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.GenericParameterAttributes/*01000047*/ 
          get_GenericParameterAttributes() cil managed
  // SIG: 20 00 11 81 1D
  {
    // Method begins at RVA 0x222c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_GenericParameterAttributes

  .method /*060000B2*/ public hidebysig specialname virtual 
          instance int32  get_GenericParameterPosition() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x222f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_GenericParameterPosition

  .method /*060000B3*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*01000037*/ 
          get_GUID() cil managed
  // SIG: 20 00 11 80 DD
  {
    // Method begins at RVA 0x2232
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_GUID

  .method /*060000B4*/ public hidebysig specialname virtual 
          instance bool  get_IsByRefLike() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2235
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_IsByRefLike

  .method /*060000B5*/ public hidebysig specialname virtual 
          instance bool  get_IsConstructedGenericType() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2238
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_IsConstructedGenericType

  .method /*060000B6*/ public hidebysig specialname virtual 
          instance bool  get_IsGenericParameter() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x223b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_IsGenericParameter

  .method /*060000B7*/ public hidebysig specialname virtual 
          instance bool  get_IsGenericType() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x223e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_IsGenericType

  .method /*060000B8*/ public hidebysig specialname virtual 
          instance bool  get_IsGenericTypeDefinition() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2241
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_IsGenericTypeDefinition

  .method /*060000B9*/ public hidebysig specialname virtual 
          instance bool  get_IsSZArray() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2244
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_IsSZArray

  .method /*060000BA*/ public hidebysig specialname virtual 
          instance bool  get_IsTypeDefinition() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2247
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_IsTypeDefinition

  .method /*060000BB*/ public hidebysig specialname virtual 
          instance int32  get_MetadataToken() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x224a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_MetadataToken

  .method /*060000BC*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ 
          get_Module() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x224d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_Module

  .method /*060000BD*/ public hidebysig specialname virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2250
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_Name

  .method /*060000BE*/ public hidebysig specialname virtual 
          instance string  get_Namespace() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000B7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2253
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_Namespace

  .method /*060000BF*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_ReflectedType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C0000B8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2256
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_ReflectedType

  .method /*060000C0*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000038*/ 
          get_TypeHandle() cil managed
  // SIG: 20 00 11 80 E1
  {
    // Method begins at RVA 0x2259
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_TypeHandle

  .method /*060000C1*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_UnderlyingSystemType() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x225c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::get_UnderlyingSystemType

  .method /*060000C2*/ public hidebysig virtual 
          instance bool  Equals(object o) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000B9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x225f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::Equals

  .method /*060000C3*/ family hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*0100003B*/ 
          GetAttributeFlagsImpl() cil managed
  // SIG: 20 00 11 80 ED
  {
    // Method begins at RVA 0x2262
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetAttributeFlagsImpl

  .method /*060000C4*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ 
          GetConstructorImpl(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr,
                             class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000035*/ binder,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callConvention,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] types,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003C*/[] modifiers) cil managed
  // SIG: 20 05 12 80 9D 11 80 D1 12 80 D5 11 80 B5 1D 12 75 1D 11 80 F1
  {
    .custom /*0C0000BB:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 07 00 00 00 00 00 ) 
    .param [2]/*080000B4*/ 
    .custom /*0C0000B5:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080000B7*/ 
    .custom /*0C0000B6:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2265
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetConstructorImpl

  .method /*060000C5*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/[] 
          GetConstructors(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 9D 11 80 D1
  {
    .custom /*0C0000BC:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 07 00 00 00 00 00 ) 
    // Method begins at RVA 0x2268
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetConstructors

  .method /*060000C6*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(bool inherit) cil managed
  // SIG: 20 01 1D 1C 02
  {
    // Method begins at RVA 0x226b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetCustomAttributes

  .method /*060000C7*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ attributeType,
                                                 bool inherit) cil managed
  // SIG: 20 02 1D 1C 12 75 02
  {
    // Method begins at RVA 0x226e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetCustomAttributes

  .method /*060000C8*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          GetElementType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C0000BE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2271
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetElementType

  .method /*060000C9*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100003D*/ 
          GetEvent(string name,
                   valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 02 12 80 F5 0E 11 80 D1
  {
    .custom /*0C0000BF:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 18 00 00 00 00 ) 
    // Method begins at RVA 0x2274
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetEvent

  .method /*060000CA*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100003D*/[] 
          GetEvents() cil managed
  // SIG: 20 00 1D 12 80 F5
  {
    .custom /*0C0000C0:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 08 00 00 00 00 ) 
    // Method begins at RVA 0x2277
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetEvents

  .method /*060000CB*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100003D*/[] 
          GetEvents(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 F5 11 80 D1
  {
    .custom /*0C0000C1:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 18 00 00 00 00 ) 
    // Method begins at RVA 0x227a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetEvents

  .method /*060000CC*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003E*/ 
          GetField(string name,
                   valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 02 12 80 F9 0E 11 80 D1
  {
    .custom /*0C0000C2:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 60 00 00 00 00 00 )                         // ..`.....
    // Method begins at RVA 0x227d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetField

  .method /*060000CD*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003E*/[] 
          GetFields(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 F9 11 80 D1
  {
    .custom /*0C0000C4:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 60 00 00 00 00 00 )                         // ..`.....
    // Method begins at RVA 0x2280
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetFields

  .method /*060000CE*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] 
          GetGenericArguments() cil managed
  // SIG: 20 00 1D 12 75
  {
    // Method begins at RVA 0x2283
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetGenericArguments

  .method /*060000CF*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          GetGenericTypeDefinition() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x2286
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetGenericTypeDefinition

  .method /*060000D0*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2289
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetHashCode

  .method /*060000D1*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          GetInterface(string name,
                       bool ignoreCase) cil managed
  // SIG: 20 02 12 75 0E 02
  {
    .custom /*0C0000C7:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 20 00 00 00 00 )                         // ... ....
    .param [0]/*080000C2*/ 
    .custom /*0C0000BA:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 20 00 00 00 00 )                         // ... ....
    // Method begins at RVA 0x228c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetInterface

  .method /*060000D2*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.InterfaceMapping/*0100003F*/ 
          GetInterfaceMap(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ interfaceType) cil managed
  // SIG: 20 01 11 80 FD 12 75
  {
    .param [1]/*080000C5*/ 
    .custom /*0C0000BD:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 18 00 00 00 00 00 ) 
    // Method begins at RVA 0x228f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetInterfaceMap

  .method /*060000D3*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] 
          GetInterfaces() cil managed
  // SIG: 20 00 1D 12 75
  {
    .custom /*0C0000C8:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 20 00 00 00 00 )                         // ... ....
    // Method begins at RVA 0x2292
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetInterfaces

  .method /*060000D4*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000040*/[] 
          GetMember(string name,
                    valuetype [System.Runtime/*23000001*/]System.Reflection.MemberTypes/*01000041*/ 'type',
                    valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 03 1D 12 81 01 0E 11 81 05 11 80 D1
  {
    .custom /*0C0000C9:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF 1F 00 00 00 00 ) 
    // Method begins at RVA 0x2295
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetMember

  .method /*060000D5*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000040*/[] 
          GetMembers(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 81 01 11 80 D1
  {
    .custom /*0C0000CA:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF 1F 00 00 00 00 ) 
    // Method begins at RVA 0x2298
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetMembers

  .method /*060000D6*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetMethodImpl(string name,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr,
                        class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000035*/ binder,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callConvention,
                        class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] types,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003C*/[] modifiers) cil managed
  // SIG: 20 06 12 59 0E 11 80 D1 12 80 D5 11 80 B5 1D 12 75 1D 11 80 F1
  {
    .custom /*0C0000CC:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 18 00 00 00 00 00 ) 
    .param [3]/*080000CC*/ 
    .custom /*0C0000C3:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080000CE*/ 
    .custom /*0C0000C5:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [6]/*080000CF*/ 
    .custom /*0C0000C6:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x229b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetMethodImpl

  .method /*060000D7*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/[] 
          GetMethods(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 59 11 80 D1
  {
    .custom /*0C0000CE:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 18 00 00 00 00 00 ) 
    // Method begins at RVA 0x229e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetMethods

  .method /*060000D8*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          GetNestedType(string name,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 02 12 75 0E 11 80 D1
  {
    .custom /*0C0000CF:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 80 01 00 00 00 00 ) 
    // Method begins at RVA 0x22a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetNestedType

  .method /*060000D9*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] 
          GetNestedTypes(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 75 11 80 D1
  {
    .custom /*0C0000D0:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 80 01 00 00 00 00 ) 
    // Method begins at RVA 0x22a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetNestedTypes

  .method /*060000DA*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000042*/[] 
          GetProperties(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 81 09 11 80 D1
  {
    .custom /*0C0000D1:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 06 00 00 00 00 ) 
    // Method begins at RVA 0x22a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetProperties

  .method /*060000DB*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000042*/ 
          GetPropertyImpl(string name,
                          valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr,
                          class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000035*/ binder,
                          class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                          class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] types,
                          valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003C*/[] modifiers) cil managed
  // SIG: 20 06 12 81 09 0E 11 80 D1 12 80 D5 12 75 1D 12 75 1D 11 80 F1
  {
    .custom /*0C0000D3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000D4:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 06 00 00 00 00 ) 
    .param [0]/*080000D5*/ 
    .custom /*0C0000CB:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [1]/*080000D6*/ 
    .custom /*0C0000CD:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [5]/*080000DA*/ 
    .custom /*0C0000D2:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::GetPropertyImpl

  .method /*060000DC*/ family hidebysig virtual 
          instance bool  HasElementTypeImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::HasElementTypeImpl

  .method /*060000DD*/ public hidebysig virtual 
          instance object  InvokeMember(string name,
                                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ invokeAttr,
                                        class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000035*/ binder,
                                        object target,
                                        object[] args,
                                        valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003C*/[] modifiers,
                                        class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000025*/ culture,
                                        string[] namedParameters) cil managed
  // SIG: 20 08 1C 0E 11 80 D1 12 80 D5 1C 1D 1C 1D 11 80 F1 12 80 95 1D 0E
  {
    .custom /*0C0000D6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000D7:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 7F 06 00 00 00 00 ) 
    .param [0]/*080000DC*/ 
    .custom /*0C0000D5:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [1]/*080000DD*/ 
    .custom /*0C0000D8:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [8]/*080000E4*/ 
    .custom /*0C0000DB:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::InvokeMember

  .method /*060000DE*/ family hidebysig virtual 
          instance bool  IsArrayImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::IsArrayImpl

  .method /*060000DF*/ public hidebysig virtual 
          instance bool  IsAssignableFrom(class [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000036*/ typeInfo) cil managed
  // SIG: 20 01 02 12 80 D9
  {
    .custom /*0C0000D9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000E5*/ 
    .custom /*0C0000DC:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000043*/::.ctor(bool) /* 0A00001B */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::IsAssignableFrom

  .method /*060000E0*/ public hidebysig virtual 
          instance bool  IsAssignableFrom(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ c) cil managed
  // SIG: 20 01 02 12 75
  {
    .custom /*0C0000DA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000E6*/ 
    .custom /*0C0000DD:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000043*/::.ctor(bool) /* 0A00001B */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::IsAssignableFrom

  .method /*060000E1*/ family hidebysig virtual 
          instance bool  IsByRefImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::IsByRefImpl

  .method /*060000E2*/ family hidebysig virtual 
          instance bool  IsCOMObjectImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::IsCOMObjectImpl

  .method /*060000E3*/ public hidebysig virtual 
          instance bool  IsDefined(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ attributeType,
                                   bool inherit) cil managed
  // SIG: 20 02 02 12 75 02
  {
    // Method begins at RVA 0x22c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::IsDefined

  .method /*060000E4*/ family hidebysig virtual 
          instance bool  IsPointerImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::IsPointerImpl

  .method /*060000E5*/ family hidebysig virtual 
          instance bool  IsPrimitiveImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::IsPrimitiveImpl

  .method /*060000E6*/ public hidebysig virtual 
          instance bool  IsSubclassOf(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ c) cil managed
  // SIG: 20 01 02 12 75
  {
    // Method begins at RVA 0x22cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::IsSubclassOf

  .method /*060000E7*/ family hidebysig virtual 
          instance bool  IsValueTypeImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::IsValueTypeImpl

  .method /*060000E8*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          MakeArrayType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C0000DE:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000018*/::.ctor(string) /* 0A000014 */ = ( 01 00 43 54 68 65 20 63 6F 64 65 20 66 6F 72 20   // ..CThe code for 
                                                                                                                                                                                       61 6E 20 61 72 72 61 79 20 6F 66 20 74 68 65 20   // an array of the 
                                                                                                                                                                                       73 70 65 63 69 66 69 65 64 20 74 79 70 65 20 6D   // specified type m
                                                                                                                                                                                       69 67 68 74 20 6E 6F 74 20 62 65 20 61 76 61 69   // ight not be avai
                                                                                                                                                                                       6C 61 62 6C 65 2E 00 00 )                         // lable...
    // Method begins at RVA 0x22d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::MakeArrayType

  .method /*060000E9*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          MakeArrayType(int32 rank) cil managed
  // SIG: 20 01 12 75 08
  {
    .custom /*0C0000DF:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000018*/::.ctor(string) /* 0A000014 */ = ( 01 00 43 54 68 65 20 63 6F 64 65 20 66 6F 72 20   // ..CThe code for 
                                                                                                                                                                                       61 6E 20 61 72 72 61 79 20 6F 66 20 74 68 65 20   // an array of the 
                                                                                                                                                                                       73 70 65 63 69 66 69 65 64 20 74 79 70 65 20 6D   // specified type m
                                                                                                                                                                                       69 67 68 74 20 6E 6F 74 20 62 65 20 61 76 61 69   // ight not be avai
                                                                                                                                                                                       6C 61 62 6C 65 2E 00 00 )                         // lable...
    // Method begins at RVA 0x22d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::MakeArrayType

  .method /*060000EA*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          MakeByRefType() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x22d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::MakeByRefType

  .method /*060000EB*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          MakeGenericType(class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] typeArguments) cil managed
  // SIG: 20 01 12 75 1D 12 75
  {
    .custom /*0C0000E0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000018*/::.ctor(string) /* 0A000014 */ = ( 01 00 49 54 68 65 20 6E 61 74 69 76 65 20 63 6F   // ..IThe native co
                                                                                                                                                                                       64 65 20 66 6F 72 20 74 68 69 73 20 69 6E 73 74   // de for this inst
                                                                                                                                                                                       61 6E 74 69 61 74 69 6F 6E 20 6D 69 67 68 74 20   // antiation might 
                                                                                                                                                                                       6E 6F 74 20 62 65 20 61 76 61 69 6C 61 62 6C 65   // not be available
                                                                                                                                                                                       20 61 74 20 72 75 6E 74 69 6D 65 2E 00 00 )       //  at runtime...
    .custom /*0C0000E1:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 C5 49 66 20 73 6F 6D 65 20 6F 66 20 74   // ....If some of t
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
    .param [1]/*080000EB*/ 
    .custom /*0C0000E2:0A00001C*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000048*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::MakeGenericType

  .method /*060000EC*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          MakePointerType() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x22dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::MakePointerType

  .method /*060000ED*/ public hidebysig instance void 
          SetBaseTypeConstraint(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ baseTypeConstraint) cil managed
  // SIG: 20 01 01 12 75
  {
    .custom /*0C0000E4:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000EC*/ 
    .custom /*0C0000E3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x22e0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GenericTypeParameterBuilder::SetBaseTypeConstraint

  .method /*060000EE*/ family hidebysig newslot abstract virtual 
          instance void  SetBaseTypeConstraintCore(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ baseTypeConstraint) cil managed
  // SIG: 20 01 01 12 75
  {
    .custom /*0C0000E6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000ED*/ 
    .custom /*0C0000E5:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method GenericTypeParameterBuilder::SetBaseTypeConstraintCore

  .method /*060000EF*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 1D 05
  {
    // Method begins at RVA 0x22e2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GenericTypeParameterBuilder::SetCustomAttribute

  .method /*060000F0*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001B*/ customBuilder) cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x22e4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GenericTypeParameterBuilder::SetCustomAttribute

  .method /*060000F1*/ family hidebysig newslot abstract virtual 
          instance void  SetCustomAttributeCore(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000028*/<uint8> binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 15 11 80 A1 01 05
  {
    .custom /*0C0000E7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    .param [1]/*080000F1*/ 
    .custom /*0C0000E8:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method GenericTypeParameterBuilder::SetCustomAttributeCore

  .method /*060000F2*/ public hidebysig instance void 
          SetGenericParameterAttributes(valuetype [System.Runtime/*23000001*/]System.Reflection.GenericParameterAttributes/*01000047*/ genericParameterAttributes) cil managed
  // SIG: 20 01 01 11 81 1D
  {
    // Method begins at RVA 0x22e6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GenericTypeParameterBuilder::SetGenericParameterAttributes

  .method /*060000F3*/ family hidebysig newslot abstract virtual 
          instance void  SetGenericParameterAttributesCore(valuetype [System.Runtime/*23000001*/]System.Reflection.GenericParameterAttributes/*01000047*/ genericParameterAttributes) cil managed
  // SIG: 20 01 01 11 81 1D
  {
    // Method begins at RVA 0x0
  } // end of method GenericTypeParameterBuilder::SetGenericParameterAttributesCore

  .method /*060000F4*/ public hidebysig instance void 
          SetInterfaceConstraints(class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] interfaceConstraints) cil managed
  // SIG: 20 01 01 1D 12 75
  {
    .param [1]/*080000F5*/ 
    .custom /*0C0000E9:0A00001C*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000048*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
    .custom /*0C0000EA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22e8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GenericTypeParameterBuilder::SetInterfaceConstraints

  .method /*060000F5*/ family hidebysig newslot abstract virtual 
          instance void  SetInterfaceConstraintsCore(class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] interfaceConstraints) cil managed
  // SIG: 20 01 01 1D 12 75
  {
    .param [1]/*080000F6*/ 
    .custom /*0C0000EB:0A00001C*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000048*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
    .custom /*0C0000EC:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method GenericTypeParameterBuilder::SetInterfaceConstraintsCore

  .method /*060000F6*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GenericTypeParameterBuilder::ToString

  .property /*1700002E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/
          Assembly()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_Assembly() /* 060000AA */
  } // end of property GenericTypeParameterBuilder::Assembly
  .property /*1700002F*/ instance callconv(8) string
          AssemblyQualifiedName()
  {
    .custom /*0C000053:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_AssemblyQualifiedName() /* 060000AB */
  } // end of property GenericTypeParameterBuilder::AssemblyQualifiedName
  .property /*17000030*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          BaseType()
  {
    .custom /*0C000055:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_BaseType() /* 060000AC */
  } // end of property GenericTypeParameterBuilder::BaseType
  .property /*17000031*/ instance callconv(8) bool
          ContainsGenericParameters()
  {
    .get instance bool System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_ContainsGenericParameters() /* 060000AD */
  } // end of property GenericTypeParameterBuilder::ContainsGenericParameters
  .property /*17000032*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodBase/*01000046*/
          DeclaringMethod()
  {
    .custom /*0C000056:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodBase/*01000046*/ System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_DeclaringMethod() /* 060000AE */
  } // end of property GenericTypeParameterBuilder::DeclaringMethod
  .property /*17000033*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          DeclaringType()
  {
    .custom /*0C000057:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_DeclaringType() /* 060000AF */
  } // end of property GenericTypeParameterBuilder::DeclaringType
  .property /*17000034*/ instance callconv(8) string
          FullName()
  {
    .custom /*0C000058:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_FullName() /* 060000B0 */
  } // end of property GenericTypeParameterBuilder::FullName
  .property /*17000035*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Reflection.GenericParameterAttributes/*01000047*/
          GenericParameterAttributes()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Reflection.GenericParameterAttributes/*01000047*/ System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_GenericParameterAttributes() /* 060000B1 */
  } // end of property GenericTypeParameterBuilder::GenericParameterAttributes
  .property /*17000036*/ instance callconv(8) int32
          GenericParameterPosition()
  {
    .get instance int32 System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_GenericParameterPosition() /* 060000B2 */
  } // end of property GenericTypeParameterBuilder::GenericParameterPosition
  .property /*17000037*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Guid/*01000037*/
          GUID()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Guid/*01000037*/ System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_GUID() /* 060000B3 */
  } // end of property GenericTypeParameterBuilder::GUID
  .property /*17000038*/ instance callconv(8) bool
          IsByRefLike()
  {
    .get instance bool System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_IsByRefLike() /* 060000B4 */
  } // end of property GenericTypeParameterBuilder::IsByRefLike
  .property /*17000039*/ instance callconv(8) bool
          IsConstructedGenericType()
  {
    .get instance bool System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_IsConstructedGenericType() /* 060000B5 */
  } // end of property GenericTypeParameterBuilder::IsConstructedGenericType
  .property /*1700003A*/ instance callconv(8) bool
          IsGenericParameter()
  {
    .get instance bool System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_IsGenericParameter() /* 060000B6 */
  } // end of property GenericTypeParameterBuilder::IsGenericParameter
  .property /*1700003B*/ instance callconv(8) bool
          IsGenericType()
  {
    .get instance bool System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_IsGenericType() /* 060000B7 */
  } // end of property GenericTypeParameterBuilder::IsGenericType
  .property /*1700003C*/ instance callconv(8) bool
          IsGenericTypeDefinition()
  {
    .get instance bool System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_IsGenericTypeDefinition() /* 060000B8 */
  } // end of property GenericTypeParameterBuilder::IsGenericTypeDefinition
  .property /*1700003D*/ instance callconv(8) bool
          IsSZArray()
  {
    .get instance bool System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_IsSZArray() /* 060000B9 */
  } // end of property GenericTypeParameterBuilder::IsSZArray
  .property /*1700003E*/ instance callconv(8) bool
          IsTypeDefinition()
  {
    .get instance bool System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_IsTypeDefinition() /* 060000BA */
  } // end of property GenericTypeParameterBuilder::IsTypeDefinition
  .property /*1700003F*/ instance callconv(8) int32
          MetadataToken()
  {
    .get instance int32 System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_MetadataToken() /* 060000BB */
  } // end of property GenericTypeParameterBuilder::MetadataToken
  .property /*17000040*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/
          Module()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_Module() /* 060000BC */
  } // end of property GenericTypeParameterBuilder::Module
  .property /*17000041*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_Name() /* 060000BD */
  } // end of property GenericTypeParameterBuilder::Name
  .property /*17000042*/ instance callconv(8) string
          Namespace()
  {
    .custom /*0C00005E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_Namespace() /* 060000BE */
  } // end of property GenericTypeParameterBuilder::Namespace
  .property /*17000043*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          ReflectedType()
  {
    .custom /*0C00005F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_ReflectedType() /* 060000BF */
  } // end of property GenericTypeParameterBuilder::ReflectedType
  .property /*17000044*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000038*/
          TypeHandle()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000038*/ System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_TypeHandle() /* 060000C0 */
  } // end of property GenericTypeParameterBuilder::TypeHandle
  .property /*17000045*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          UnderlyingSystemType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/::get_UnderlyingSystemType() /* 060000C1 */
  } // end of property GenericTypeParameterBuilder::UnderlyingSystemType
} // end of class System.Reflection.Emit.GenericTypeParameterBuilder

.class /*02000009*/ public abstract auto ansi beforefieldinit System.Reflection.Emit.MethodBuilder
       extends [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/
{
  .custom /*0C00002C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*060000F7*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22ed
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MethodBuilder::.ctor

  .method /*060000F8*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ 
          get_Attributes() cil managed
  // SIG: 20 00 11 80 B1
  {
    // Method begins at RVA 0x22f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_Attributes

  .method /*060000F9*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ 
          get_CallingConvention() cil managed
  // SIG: 20 00 11 80 B5
  {
    // Method begins at RVA 0x22f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_CallingConvention

  .method /*060000FA*/ public hidebysig specialname virtual 
          instance bool  get_ContainsGenericParameters() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_ContainsGenericParameters

  .method /*060000FB*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_DeclaringType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C0000EF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_DeclaringType

  .method /*060000FC*/ public hidebysig specialname 
          instance bool  get_InitLocals() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2301
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_InitLocals

  .method /*060000FD*/ public hidebysig specialname 
          instance void  set_InitLocals(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2304
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MethodBuilder::set_InitLocals

  .method /*060000FE*/ family hidebysig newslot specialname abstract virtual 
          instance bool  get_InitLocalsCore() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method MethodBuilder::get_InitLocalsCore

  .method /*060000FF*/ family hidebysig newslot specialname abstract virtual 
          instance void  set_InitLocalsCore(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x0
  } // end of method MethodBuilder::set_InitLocalsCore

  .method /*06000100*/ public hidebysig specialname virtual 
          instance bool  get_IsGenericMethod() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2306
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_IsGenericMethod

  .method /*06000101*/ public hidebysig specialname virtual 
          instance bool  get_IsGenericMethodDefinition() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2309
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_IsGenericMethodDefinition

  .method /*06000102*/ public hidebysig specialname virtual 
          instance bool  get_IsSecurityCritical() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x230c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_IsSecurityCritical

  .method /*06000103*/ public hidebysig specialname virtual 
          instance bool  get_IsSecuritySafeCritical() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x230f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_IsSecuritySafeCritical

  .method /*06000104*/ public hidebysig specialname virtual 
          instance bool  get_IsSecurityTransparent() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2312
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_IsSecurityTransparent

  .method /*06000105*/ public hidebysig specialname virtual 
          instance int32  get_MetadataToken() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2315
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_MetadataToken

  .method /*06000106*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.RuntimeMethodHandle/*0100002E*/ 
          get_MethodHandle() cil managed
  // SIG: 20 00 11 80 B9
  {
    // Method begins at RVA 0x2318
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_MethodHandle

  .method /*06000107*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ 
          get_Module() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x231b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_Module

  .method /*06000108*/ public hidebysig specialname virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x231e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_Name

  .method /*06000109*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_ReflectedType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C0000F3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2321
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_ReflectedType

  .method /*0600010A*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*01000033*/ 
          get_ReturnParameter() cil managed
  // SIG: 20 00 12 80 CD
  {
    // Method begins at RVA 0x2324
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_ReturnParameter

  .method /*0600010B*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_ReturnType() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x2327
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_ReturnType

  .method /*0600010C*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ICustomAttributeProvider/*01000049*/ 
          get_ReturnTypeCustomAttributes() cil managed
  // SIG: 20 00 12 81 25
  {
    // Method begins at RVA 0x232a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::get_ReturnTypeCustomAttributes

  .method /*0600010D*/ public hidebysig instance class System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/[] 
          DefineGenericParameters(string[] names) cil managed
  // SIG: 20 01 1D 12 20 1D 0E
  {
    .param [1]/*080000F9*/ 
    .custom /*0C0000ED:0A00001C*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000048*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x232d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::DefineGenericParameters

  .method /*0600010E*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/[] 
          DefineGenericParametersCore(string[] names) cil managed
  // SIG: 20 01 1D 12 20 1D 0E
  {
    .param [1]/*080000FA*/ 
    .custom /*0C0000EE:0A00001C*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000048*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method MethodBuilder::DefineGenericParametersCore

  .method /*0600010F*/ public hidebysig instance class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.ParameterBuilder/*0100002F*/ 
          DefineParameter(int32 position,
                          valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterAttributes/*01000030*/ attributes,
                          string strParamName) cil managed
  // SIG: 20 03 12 80 BD 08 11 80 C1 0E
  {
    .param [3]/*080000FD*/ 
    .custom /*0C0000F0:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2330
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::DefineParameter

  .method /*06000110*/ family hidebysig newslot abstract virtual 
          instance class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.ParameterBuilder/*0100002F*/ 
          DefineParameterCore(int32 position,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterAttributes/*01000030*/ attributes,
                              string strParamName) cil managed
  // SIG: 20 03 12 80 BD 08 11 80 C1 0E
  {
    .param [3]/*08000100*/ 
    .custom /*0C0000F1:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method MethodBuilder::DefineParameterCore

  .method /*06000111*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000F5:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2333
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::Equals

  .method /*06000112*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetBaseDefinition() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2336
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::GetBaseDefinition

  .method /*06000113*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(bool inherit) cil managed
  // SIG: 20 01 1D 1C 02
  {
    // Method begins at RVA 0x2339
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::GetCustomAttributes

  .method /*06000114*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ attributeType,
                                                 bool inherit) cil managed
  // SIG: 20 02 1D 1C 12 75 02
  {
    // Method begins at RVA 0x233c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::GetCustomAttributes

  .method /*06000115*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] 
          GetGenericArguments() cil managed
  // SIG: 20 00 1D 12 75
  {
    // Method begins at RVA 0x233f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::GetGenericArguments

  .method /*06000116*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetGenericMethodDefinition() cil managed
  // SIG: 20 00 12 59
  {
    // Method begins at RVA 0x2342
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::GetGenericMethodDefinition

  .method /*06000117*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2345
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::GetHashCode

  .method /*06000118*/ public hidebysig instance class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.ILGenerator/*01000031*/ 
          GetILGenerator() cil managed
  // SIG: 20 00 12 80 C5
  {
    // Method begins at RVA 0x2348
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::GetILGenerator

  .method /*06000119*/ public hidebysig instance class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.ILGenerator/*01000031*/ 
          GetILGenerator(int32 size) cil managed
  // SIG: 20 01 12 80 C5 08
  {
    // Method begins at RVA 0x234b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::GetILGenerator

  .method /*0600011A*/ family hidebysig newslot abstract virtual 
          instance class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.ILGenerator/*01000031*/ 
          GetILGeneratorCore(int32 size) cil managed
  // SIG: 20 01 12 80 C5 08
  {
    // Method begins at RVA 0x0
  } // end of method MethodBuilder::GetILGeneratorCore

  .method /*0600011B*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.MethodImplAttributes/*01000032*/ 
          GetMethodImplementationFlags() cil managed
  // SIG: 20 00 11 80 C9
  {
    // Method begins at RVA 0x234e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::GetMethodImplementationFlags

  .method /*0600011C*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*01000033*/[] 
          GetParameters() cil managed
  // SIG: 20 00 1D 12 80 CD
  {
    // Method begins at RVA 0x2351
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::GetParameters

  .method /*0600011D*/ public hidebysig virtual 
          instance object  Invoke(object obj,
                                  valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ invokeAttr,
                                  class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000035*/ binder,
                                  object[] parameters,
                                  class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000025*/ culture) cil managed
  // SIG: 20 05 1C 1C 11 80 D1 12 80 D5 1D 1C 12 80 95
  {
    .custom /*0C0000FB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000107*/ 
    .custom /*0C0000F2:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2354
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::Invoke

  .method /*0600011E*/ public hidebysig virtual 
          instance bool  IsDefined(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ attributeType,
                                   bool inherit) cil managed
  // SIG: 20 02 02 12 75 02
  {
    // Method begins at RVA 0x2357
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::IsDefined

  .method /*0600011F*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          MakeGenericMethod(class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] typeArguments) cil managed
  // SIG: 20 01 12 59 1D 12 75
  {
    .custom /*0C0000FE:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000018*/::.ctor(string) /* 0A000014 */ = ( 01 00 49 54 68 65 20 6E 61 74 69 76 65 20 63 6F   // ..IThe native co
                                                                                                                                                                                       64 65 20 66 6F 72 20 74 68 69 73 20 69 6E 73 74   // de for this inst
                                                                                                                                                                                       61 6E 74 69 61 74 69 6F 6E 20 6D 69 67 68 74 20   // antiation might 
                                                                                                                                                                                       6E 6F 74 20 62 65 20 61 76 61 69 6C 61 62 6C 65   // not be available
                                                                                                                                                                                       20 61 74 20 72 75 6E 74 69 6D 65 2E 00 00 )       //  at runtime...
    .custom /*0C0000FF:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 C5 49 66 20 73 6F 6D 65 20 6F 66 20 74   // ....If some of t
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
    .param [1]/*0800010F*/ 
    .custom /*0C0000F4:0A00001C*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000048*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x235a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::MakeGenericMethod

  .method /*06000120*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 1D 05
  {
    // Method begins at RVA 0x235d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MethodBuilder::SetCustomAttribute

  .method /*06000121*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001B*/ customBuilder) cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x235f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MethodBuilder::SetCustomAttribute

  .method /*06000122*/ family hidebysig newslot abstract virtual 
          instance void  SetCustomAttributeCore(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000028*/<uint8> binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 15 11 80 A1 01 05
  {
    .custom /*0C000102:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000113*/ 
    .custom /*0C0000F6:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method MethodBuilder::SetCustomAttributeCore

  .method /*06000123*/ public hidebysig instance void 
          SetImplementationFlags(valuetype [System.Runtime/*23000001*/]System.Reflection.MethodImplAttributes/*01000032*/ attributes) cil managed
  // SIG: 20 01 01 11 80 C9
  {
    // Method begins at RVA 0x2361
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MethodBuilder::SetImplementationFlags

  .method /*06000124*/ family hidebysig newslot abstract virtual 
          instance void  SetImplementationFlagsCore(valuetype [System.Runtime/*23000001*/]System.Reflection.MethodImplAttributes/*01000032*/ attributes) cil managed
  // SIG: 20 01 01 11 80 C9
  {
    // Method begins at RVA 0x0
  } // end of method MethodBuilder::SetImplementationFlagsCore

  .method /*06000125*/ public hidebysig instance void 
          SetParameters(class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes) cil managed
  // SIG: 20 01 01 1D 12 75
  {
    .param [1]/*08000117*/ 
    .custom /*0C0000F7:0A00001C*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000048*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2363
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MethodBuilder::SetParameters

  .method /*06000126*/ public hidebysig instance void 
          SetReturnType(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType) cil managed
  // SIG: 20 01 01 12 75
  {
    .custom /*0C000106:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2365
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MethodBuilder::SetReturnType

  .method /*06000127*/ public hidebysig instance void 
          SetSignature(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                       class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] returnTypeRequiredCustomModifiers,
                       class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] returnTypeOptionalCustomModifiers,
                       class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes,
                       class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] parameterTypeRequiredCustomModifiers,
                       class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] parameterTypeOptionalCustomModifiers) cil managed
  // SIG: 20 06 01 12 75 1D 12 75 1D 12 75 1D 12 75 1D 1D 12 75 1D 1D 12 75
  {
    .custom /*0C000107:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800011A*/ 
    .custom /*0C0000F8:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [3]/*0800011B*/ 
    .custom /*0C0000F9:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [4]/*0800011C*/ 
    .custom /*0C0000FA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [5]/*0800011D*/ 
    .custom /*0C0000FC:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [6]/*0800011E*/ 
    .custom /*0C0000FD:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x2367
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MethodBuilder::SetSignature

  .method /*06000128*/ family hidebysig newslot abstract virtual 
          instance void  SetSignatureCore(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                                          class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] returnTypeRequiredCustomModifiers,
                                          class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] returnTypeOptionalCustomModifiers,
                                          class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes,
                                          class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] parameterTypeRequiredCustomModifiers,
                                          class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] parameterTypeOptionalCustomModifiers) cil managed
  // SIG: 20 06 01 12 75 1D 12 75 1D 12 75 1D 12 75 1D 1D 12 75 1D 1D 12 75
  {
    .custom /*0C000108:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000120*/ 
    .custom /*0C000100:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [3]/*08000121*/ 
    .custom /*0C000101:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [4]/*08000122*/ 
    .custom /*0C000103:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [5]/*08000123*/ 
    .custom /*0C000104:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [6]/*08000124*/ 
    .custom /*0C000105:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method MethodBuilder::SetSignatureCore

  .method /*06000129*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2369
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MethodBuilder::ToString

  .property /*17000046*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/
          Attributes()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ System.Reflection.Emit.MethodBuilder/*02000009*/::get_Attributes() /* 060000F8 */
  } // end of property MethodBuilder::Attributes
  .property /*17000047*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/
          CallingConvention()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ System.Reflection.Emit.MethodBuilder/*02000009*/::get_CallingConvention() /* 060000F9 */
  } // end of property MethodBuilder::CallingConvention
  .property /*17000048*/ instance callconv(8) bool
          ContainsGenericParameters()
  {
    .get instance bool System.Reflection.Emit.MethodBuilder/*02000009*/::get_ContainsGenericParameters() /* 060000FA */
  } // end of property MethodBuilder::ContainsGenericParameters
  .property /*17000049*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          DeclaringType()
  {
    .custom /*0C000062:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.MethodBuilder/*02000009*/::get_DeclaringType() /* 060000FB */
  } // end of property MethodBuilder::DeclaringType
  .property /*1700004A*/ instance callconv(8) bool
          InitLocals()
  {
    .get instance bool System.Reflection.Emit.MethodBuilder/*02000009*/::get_InitLocals() /* 060000FC */
    .set instance void System.Reflection.Emit.MethodBuilder/*02000009*/::set_InitLocals(bool) /* 060000FD */
  } // end of property MethodBuilder::InitLocals
  .property /*1700004B*/ instance callconv(8) bool
          InitLocalsCore()
  {
    .get instance bool System.Reflection.Emit.MethodBuilder/*02000009*/::get_InitLocalsCore() /* 060000FE */
    .set instance void System.Reflection.Emit.MethodBuilder/*02000009*/::set_InitLocalsCore(bool) /* 060000FF */
  } // end of property MethodBuilder::InitLocalsCore
  .property /*1700004C*/ instance callconv(8) bool
          IsGenericMethod()
  {
    .get instance bool System.Reflection.Emit.MethodBuilder/*02000009*/::get_IsGenericMethod() /* 06000100 */
  } // end of property MethodBuilder::IsGenericMethod
  .property /*1700004D*/ instance callconv(8) bool
          IsGenericMethodDefinition()
  {
    .get instance bool System.Reflection.Emit.MethodBuilder/*02000009*/::get_IsGenericMethodDefinition() /* 06000101 */
  } // end of property MethodBuilder::IsGenericMethodDefinition
  .property /*1700004E*/ instance callconv(8) bool
          IsSecurityCritical()
  {
    .get instance bool System.Reflection.Emit.MethodBuilder/*02000009*/::get_IsSecurityCritical() /* 06000102 */
  } // end of property MethodBuilder::IsSecurityCritical
  .property /*1700004F*/ instance callconv(8) bool
          IsSecuritySafeCritical()
  {
    .get instance bool System.Reflection.Emit.MethodBuilder/*02000009*/::get_IsSecuritySafeCritical() /* 06000103 */
  } // end of property MethodBuilder::IsSecuritySafeCritical
  .property /*17000050*/ instance callconv(8) bool
          IsSecurityTransparent()
  {
    .get instance bool System.Reflection.Emit.MethodBuilder/*02000009*/::get_IsSecurityTransparent() /* 06000104 */
  } // end of property MethodBuilder::IsSecurityTransparent
  .property /*17000051*/ instance callconv(8) int32
          MetadataToken()
  {
    .get instance int32 System.Reflection.Emit.MethodBuilder/*02000009*/::get_MetadataToken() /* 06000105 */
  } // end of property MethodBuilder::MetadataToken
  .property /*17000052*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.RuntimeMethodHandle/*0100002E*/
          MethodHandle()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.RuntimeMethodHandle/*0100002E*/ System.Reflection.Emit.MethodBuilder/*02000009*/::get_MethodHandle() /* 06000106 */
  } // end of property MethodBuilder::MethodHandle
  .property /*17000053*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/
          Module()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ System.Reflection.Emit.MethodBuilder/*02000009*/::get_Module() /* 06000107 */
  } // end of property MethodBuilder::Module
  .property /*17000054*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Reflection.Emit.MethodBuilder/*02000009*/::get_Name() /* 06000108 */
  } // end of property MethodBuilder::Name
  .property /*17000055*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          ReflectedType()
  {
    .custom /*0C00006D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.MethodBuilder/*02000009*/::get_ReflectedType() /* 06000109 */
  } // end of property MethodBuilder::ReflectedType
  .property /*17000056*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*01000033*/
          ReturnParameter()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*01000033*/ System.Reflection.Emit.MethodBuilder/*02000009*/::get_ReturnParameter() /* 0600010A */
  } // end of property MethodBuilder::ReturnParameter
  .property /*17000057*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          ReturnType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.MethodBuilder/*02000009*/::get_ReturnType() /* 0600010B */
  } // end of property MethodBuilder::ReturnType
  .property /*17000058*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.ICustomAttributeProvider/*01000049*/
          ReturnTypeCustomAttributes()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.ICustomAttributeProvider/*01000049*/ System.Reflection.Emit.MethodBuilder/*02000009*/::get_ReturnTypeCustomAttributes() /* 0600010C */
  } // end of property MethodBuilder::ReturnTypeCustomAttributes
} // end of class System.Reflection.Emit.MethodBuilder

.class /*0200000A*/ public abstract auto ansi beforefieldinit System.Reflection.Emit.ModuleBuilder
       extends [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/
{
  .custom /*0C00002E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*0600012A*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x236c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000024       */ call       instance void [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/::.ctor() /* 0A000024 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ModuleBuilder::.ctor

  .method /*0600012B*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ 
          get_Assembly() cil managed
  // SIG: 20 00 12 55
  {
    // Method begins at RVA 0x2374
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::get_Assembly

  .method /*0600012C*/ public hidebysig specialname virtual 
          instance string  get_FullyQualifiedName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2377
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::get_FullyQualifiedName

  .method /*0600012D*/ public hidebysig specialname virtual 
          instance int32  get_MDStreamVersion() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x237a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::get_MDStreamVersion

  .method /*0600012E*/ public hidebysig specialname virtual 
          instance int32  get_MetadataToken() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x237d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::get_MetadataToken

  .method /*0600012F*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*01000037*/ 
          get_ModuleVersionId() cil managed
  // SIG: 20 00 11 80 DD
  {
    // Method begins at RVA 0x2380
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::get_ModuleVersionId

  .method /*06000130*/ public hidebysig specialname virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2383
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::get_Name

  .method /*06000131*/ public hidebysig specialname virtual 
          instance string  get_ScopeName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2386
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::get_ScopeName

  .method /*06000132*/ public hidebysig instance void 
          CreateGlobalFunctions() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2389
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ModuleBuilder::CreateGlobalFunctions

  .method /*06000133*/ family hidebysig newslot abstract virtual 
          instance void  CreateGlobalFunctionsCore() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method ModuleBuilder::CreateGlobalFunctionsCore

  .method /*06000134*/ public hidebysig instance class [System.Diagnostics.StackTrace/*23000003*/]System.Diagnostics.SymbolStore.ISymbolDocumentWriter/*0100004C*/ 
          DefineDocument(string url,
                         valuetype [System.Runtime/*23000001*/]System.Guid/*01000037*/ language,
                         valuetype [System.Runtime/*23000001*/]System.Guid/*01000037*/ languageVendor,
                         valuetype [System.Runtime/*23000001*/]System.Guid/*01000037*/ documentType) cil managed
  // SIG: 20 04 12 81 31 0E 11 80 DD 11 80 DD 11 80 DD
  {
    .custom /*0C000109:0A00001D*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100004B*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100004A*/) /* 0A00001D */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x238b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineDocument

  .method /*06000135*/ public hidebysig instance class [System.Diagnostics.StackTrace/*23000003*/]System.Diagnostics.SymbolStore.ISymbolDocumentWriter/*0100004C*/ 
          DefineDocument(string url,
                         [opt] valuetype [System.Runtime/*23000001*/]System.Guid/*01000037*/ language) cil managed
  // SIG: 20 02 12 81 31 0E 11 80 DD
  {
    .param [2]/*0800012A*/  = nullref
    // Method begins at RVA 0x238e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineDocument

  .method /*06000136*/ family hidebysig newslot virtual 
          instance class [System.Diagnostics.StackTrace/*23000003*/]System.Diagnostics.SymbolStore.ISymbolDocumentWriter/*0100004C*/ 
          DefineDocumentCore(string url,
                             [opt] valuetype [System.Runtime/*23000001*/]System.Guid/*01000037*/ language) cil managed
  // SIG: 20 02 12 81 31 0E 11 80 DD
  {
    .param [2]/*0800012C*/  = nullref
    // Method begins at RVA 0x2391
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineDocumentCore

  .method /*06000137*/ public hidebysig instance class System.Reflection.Emit.EnumBuilder/*02000005*/ 
          DefineEnum(string name,
                     valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*0100003B*/ visibility,
                     class [System.Runtime/*23000001*/]System.Type/*0100001D*/ underlyingType) cil managed
  // SIG: 20 03 12 14 0E 11 80 ED 12 75
  {
    // Method begins at RVA 0x2394
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineEnum

  .method /*06000138*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.EnumBuilder/*02000005*/ 
          DefineEnumCore(string name,
                         valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*0100003B*/ visibility,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/ underlyingType) cil managed
  // SIG: 20 03 12 14 0E 11 80 ED 12 75
  {
    // Method begins at RVA 0x0
  } // end of method ModuleBuilder::DefineEnumCore

  .method /*06000139*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*02000009*/ 
          DefineGlobalMethod(string name,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes) cil managed
  // SIG: 20 05 12 24 0E 11 80 B1 11 80 B5 12 75 1D 12 75
  {
    .param [4]/*08000136*/ 
    .custom /*0C00010A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000137*/ 
    .custom /*0C00010B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2397
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineGlobalMethod

  .method /*0600013A*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*02000009*/ 
          DefineGlobalMethod(string name,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] requiredReturnTypeCustomModifiers,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] optionalReturnTypeCustomModifiers,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] requiredParameterTypeCustomModifiers,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] optionalParameterTypeCustomModifiers) cil managed
  // SIG: 20 09 12 24 0E 11 80 B1 11 80 B5 12 75 1D 12 75 1D 12 75 1D 12 75 1D 1D 12 75 1D 1D 12 75
  {
    .param [4]/*0800013B*/ 
    .custom /*0C00010C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800013C*/ 
    .custom /*0C00010D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [6]/*0800013D*/ 
    .custom /*0C00010E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [7]/*0800013E*/ 
    .custom /*0C00010F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [8]/*0800013F*/ 
    .custom /*0C000111:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [9]/*08000140*/ 
    .custom /*0C000113:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x239a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineGlobalMethod

  .method /*0600013B*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.MethodBuilder/*02000009*/ 
          DefineGlobalMethodCore(string name,
                                 valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes,
                                 valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                                 class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                                 class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] requiredReturnTypeCustomModifiers,
                                 class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] optionalReturnTypeCustomModifiers,
                                 class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes,
                                 class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] requiredParameterTypeCustomModifiers,
                                 class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] optionalParameterTypeCustomModifiers) cil managed
  // SIG: 20 09 12 24 0E 11 80 B1 11 80 B5 12 75 1D 12 75 1D 12 75 1D 12 75 1D 1D 12 75 1D 1D 12 75
  {
    .param [4]/*08000144*/ 
    .custom /*0C000115:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000145*/ 
    .custom /*0C000116:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [6]/*08000146*/ 
    .custom /*0C000117:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [7]/*08000147*/ 
    .custom /*0C000118:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [8]/*08000148*/ 
    .custom /*0C000119:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [9]/*08000149*/ 
    .custom /*0C00011A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ModuleBuilder::DefineGlobalMethodCore

  .method /*0600013C*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*02000009*/ 
          DefineGlobalMethod(string name,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes) cil managed
  // SIG: 20 04 12 24 0E 11 80 B1 12 75 1D 12 75
  {
    .param [3]/*0800014C*/ 
    .custom /*0C00011C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800014D*/ 
    .custom /*0C00011D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x239d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineGlobalMethod

  .method /*0600013D*/ public hidebysig instance class System.Reflection.Emit.FieldBuilder/*02000007*/ 
          DefineInitializedData(string name,
                                uint8[] data,
                                valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000044*/ attributes) cil managed
  // SIG: 20 03 12 1C 0E 1D 05 11 81 11
  {
    // Method begins at RVA 0x23a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineInitializedData

  .method /*0600013E*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.FieldBuilder/*02000007*/ 
          DefineInitializedDataCore(string name,
                                    uint8[] data,
                                    valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000044*/ attributes) cil managed
  // SIG: 20 03 12 1C 0E 1D 05 11 81 11
  {
    // Method begins at RVA 0x0
  } // end of method ModuleBuilder::DefineInitializedDataCore

  .method /*0600013F*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*02000009*/ 
          DefinePInvokeMethod(string name,
                              string dllName,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                              class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                              class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes,
                              valuetype [System.Runtime.InteropServices/*23000004*/]System.Runtime.InteropServices.CallingConvention/*0100004D*/ nativeCallConv,
                              valuetype [System.Runtime/*23000001*/]System.Runtime.InteropServices.CharSet/*0100004E*/ nativeCharSet) cil managed
  // SIG: 20 08 12 24 0E 0E 11 80 B1 11 80 B5 12 75 1D 12 75 11 81 35 11 81 39
  {
    .custom /*0C000110:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 4A 50 2F 49 6E 76 6F 6B 65 20 6D 61 72 73   // ..JP/Invoke mars
                                                                                                                                                                                            68 61 6C 6C 69 6E 67 20 6D 61 79 20 64 79 6E 61   // halling may dyna
                                                                                                                                                                                            6D 69 63 61 6C 6C 79 20 61 63 63 65 73 73 20 6D   // mically access m
                                                                                                                                                                                            65 6D 62 65 72 73 20 74 68 61 74 20 63 6F 75 6C   // embers that coul
                                                                                                                                                                                            64 20 62 65 20 74 72 69 6D 6D 65 64 2E 00 00 )    // d be trimmed...
    .param [5]/*08000158*/ 
    .custom /*0C000122:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [6]/*08000159*/ 
    .custom /*0C000124:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x23a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefinePInvokeMethod

  .method /*06000140*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*02000009*/ 
          DefinePInvokeMethod(string name,
                              string dllName,
                              string entryName,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                              class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                              class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes,
                              valuetype [System.Runtime.InteropServices/*23000004*/]System.Runtime.InteropServices.CallingConvention/*0100004D*/ nativeCallConv,
                              valuetype [System.Runtime/*23000001*/]System.Runtime.InteropServices.CharSet/*0100004E*/ nativeCharSet) cil managed
  // SIG: 20 09 12 24 0E 0E 0E 11 80 B1 11 80 B5 12 75 1D 12 75 11 81 35 11 81 39
  {
    .custom /*0C000112:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 4A 50 2F 49 6E 76 6F 6B 65 20 6D 61 72 73   // ..JP/Invoke mars
                                                                                                                                                                                            68 61 6C 6C 69 6E 67 20 6D 61 79 20 64 79 6E 61   // halling may dyna
                                                                                                                                                                                            6D 69 63 61 6C 6C 79 20 61 63 63 65 73 73 20 6D   // mically access m
                                                                                                                                                                                            65 6D 62 65 72 73 20 74 68 61 74 20 63 6F 75 6C   // embers that coul
                                                                                                                                                                                            64 20 62 65 20 74 72 69 6D 6D 65 64 2E 00 00 )    // d be trimmed...
    .param [6]/*08000161*/ 
    .custom /*0C000129:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [7]/*08000162*/ 
    .custom /*0C00012A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x23a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefinePInvokeMethod

  .method /*06000141*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.MethodBuilder/*02000009*/ 
          DefinePInvokeMethodCore(string name,
                                  string dllName,
                                  string entryName,
                                  valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes,
                                  valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                                  class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                                  class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes,
                                  valuetype [System.Runtime.InteropServices/*23000004*/]System.Runtime.InteropServices.CallingConvention/*0100004D*/ nativeCallConv,
                                  valuetype [System.Runtime/*23000001*/]System.Runtime.InteropServices.CharSet/*0100004E*/ nativeCharSet) cil managed
  // SIG: 20 09 12 24 0E 0E 0E 11 80 B1 11 80 B5 12 75 1D 12 75 11 81 35 11 81 39
  {
    .custom /*0C000114:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 4A 50 2F 49 6E 76 6F 6B 65 20 6D 61 72 73   // ..JP/Invoke mars
                                                                                                                                                                                            68 61 6C 6C 69 6E 67 20 6D 61 79 20 64 79 6E 61   // halling may dyna
                                                                                                                                                                                            6D 69 63 61 6C 6C 79 20 61 63 63 65 73 73 20 6D   // mically access m
                                                                                                                                                                                            65 6D 62 65 72 73 20 74 68 61 74 20 63 6F 75 6C   // embers that coul
                                                                                                                                                                                            64 20 62 65 20 74 72 69 6D 6D 65 64 2E 00 00 )    // d be trimmed...
    .param [6]/*0800016A*/ 
    .custom /*0C000134:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [7]/*0800016B*/ 
    .custom /*0C000135:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ModuleBuilder::DefinePInvokeMethodCore

  .method /*06000142*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*0200000D*/ 
          DefineType(string name) cil managed
  // SIG: 20 01 12 34 0E
  {
    // Method begins at RVA 0x23a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineType

  .method /*06000143*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*0200000D*/ 
          DefineType(string name,
                     valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*0100003B*/ attr) cil managed
  // SIG: 20 02 12 34 0E 11 80 ED
  {
    // Method begins at RVA 0x23ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineType

  .method /*06000144*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*0200000D*/ 
          DefineType(string name,
                     valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*0100003B*/ attr,
                     class [System.Runtime/*23000001*/]System.Type/*0100001D*/ parent) cil managed
  // SIG: 20 03 12 34 0E 11 80 ED 12 75
  {
    .param [3]/*08000173*/ 
    .custom /*0C00013B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00013C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x23af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineType

  .method /*06000145*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*0200000D*/ 
          DefineType(string name,
                     valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*0100003B*/ attr,
                     class [System.Runtime/*23000001*/]System.Type/*0100001D*/ parent,
                     int32 typesize) cil managed
  // SIG: 20 04 12 34 0E 11 80 ED 12 75 08
  {
    .param [3]/*08000176*/ 
    .custom /*0C00013D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00013E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x23b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineType

  .method /*06000146*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*0200000D*/ 
          DefineType(string name,
                     valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*0100003B*/ attr,
                     class [System.Runtime/*23000001*/]System.Type/*0100001D*/ parent,
                     valuetype [System.Reflection.Primitives/*23000005*/]System.Reflection.Emit.PackingSize/*0100004F*/ packsize) cil managed
  // SIG: 20 04 12 34 0E 11 80 ED 12 75 11 81 3D
  {
    .param [3]/*0800017A*/ 
    .custom /*0C000140:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000141:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x23b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineType

  .method /*06000147*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*0200000D*/ 
          DefineType(string name,
                     valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*0100003B*/ attr,
                     class [System.Runtime/*23000001*/]System.Type/*0100001D*/ parent,
                     valuetype [System.Reflection.Primitives/*23000005*/]System.Reflection.Emit.PackingSize/*0100004F*/ packingSize,
                     int32 typesize) cil managed
  // SIG: 20 05 12 34 0E 11 80 ED 12 75 11 81 3D 08
  {
    .param [3]/*0800017E*/ 
    .custom /*0C000143:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000144:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x23b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineType

  .method /*06000148*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*0200000D*/ 
          DefineType(string name,
                     valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*0100003B*/ attr,
                     class [System.Runtime/*23000001*/]System.Type/*0100001D*/ parent,
                     class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] interfaces) cil managed
  // SIG: 20 04 12 34 0E 11 80 ED 12 75 1D 12 75
  {
    .param [3]/*08000183*/ 
    .custom /*0C000146:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000147:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    .param [4]/*08000184*/ 
    .custom /*0C000148:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x23bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineType

  .method /*06000149*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.TypeBuilder/*0200000D*/ 
          DefineTypeCore(string name,
                         valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*0100003B*/ attr,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/ parent,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] interfaces,
                         valuetype [System.Reflection.Primitives/*23000005*/]System.Reflection.Emit.PackingSize/*0100004F*/ packingSize,
                         int32 typesize) cil managed
  // SIG: 20 06 12 34 0E 11 80 ED 12 75 1D 12 75 11 81 3D 08
  {
    .param [3]/*08000187*/ 
    .custom /*0C00014A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00014B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    .param [4]/*08000188*/ 
    .custom /*0C00014D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ModuleBuilder::DefineTypeCore

  .method /*0600014A*/ public hidebysig instance class System.Reflection.Emit.FieldBuilder/*02000007*/ 
          DefineUninitializedData(string name,
                                  int32 size,
                                  valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000044*/ attributes) cil managed
  // SIG: 20 03 12 1C 0E 08 11 81 11
  {
    // Method begins at RVA 0x23be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::DefineUninitializedData

  .method /*0600014B*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.FieldBuilder/*02000007*/ 
          DefineUninitializedDataCore(string name,
                                      int32 size,
                                      valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000044*/ attributes) cil managed
  // SIG: 20 03 12 1C 0E 08 11 81 11
  {
    // Method begins at RVA 0x0
  } // end of method ModuleBuilder::DefineUninitializedDataCore

  .method /*0600014C*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00011B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::Equals

  .method /*0600014D*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetArrayMethod(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ arrayClass,
                         string methodName,
                         valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes) cil managed
  // SIG: 20 05 12 59 12 75 0E 11 80 B5 12 75 1D 12 75
  {
    .param [4]/*08000195*/ 
    .custom /*0C000154:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000196*/ 
    .custom /*0C000156:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x23c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetArrayMethod

  .method /*0600014E*/ family hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetArrayMethodCore(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ arrayClass,
                             string methodName,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes) cil managed
  // SIG: 20 05 12 59 12 75 0E 11 80 B5 12 75 1D 12 75
  {
    .param [4]/*0800019A*/ 
    .custom /*0C000159:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800019B*/ 
    .custom /*0C00015B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ModuleBuilder::GetArrayMethodCore

  .method /*0600014F*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(bool inherit) cil managed
  // SIG: 20 01 1D 1C 02
  {
    // Method begins at RVA 0x23c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetCustomAttributes

  .method /*06000150*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ attributeType,
                                                 bool inherit) cil managed
  // SIG: 20 02 1D 1C 12 75 02
  {
    // Method begins at RVA 0x23ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetCustomAttributes

  .method /*06000151*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<class [System.Runtime/*23000001*/]System.Reflection.CustomAttributeData/*0100001F*/> 
          GetCustomAttributesData() cil managed
  // SIG: 20 00 15 12 79 01 12 7D
  {
    // Method begins at RVA 0x23cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetCustomAttributesData

  .method /*06000152*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003E*/ 
          GetField(string name,
                   valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 02 12 80 F9 0E 11 80 D1
  {
    .custom /*0C00011E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 17 46 69 65 6C 64 73 20 6D 69 67 68 74 20   // ...Fields might 
                                                                                                                                                                                            62 65 20 72 65 6D 6F 76 65 64 00 00 )             // be removed..
    .param [0]/*0800019F*/ 
    .custom /*0C00015E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetField

  .method /*06000153*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003E*/[] 
          GetFields(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingFlags) cil managed
  // SIG: 20 01 1D 12 80 F9 11 80 D1
  {
    .custom /*0C00011F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 17 46 69 65 6C 64 73 20 6D 69 67 68 74 20   // ...Fields might 
                                                                                                                                                                                            62 65 20 72 65 6D 6F 76 65 64 00 00 )             // be removed..
    // Method begins at RVA 0x23d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetFields

  .method /*06000154*/ public hidebysig newslot abstract virtual 
          instance int32  GetFieldMetadataToken(class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003E*/ 'field') cil managed
  // SIG: 20 01 08 12 80 F9
  {
    // Method begins at RVA 0x0
  } // end of method ModuleBuilder::GetFieldMetadataToken

  .method /*06000155*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x23d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetHashCode

  .method /*06000156*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetMethodImpl(string name,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr,
                        class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000035*/ binder,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callConvention,
                        class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] types,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003C*/[] modifiers) cil managed
  // SIG: 20 06 12 59 0E 11 80 D1 12 80 D5 11 80 B5 1D 12 75 1D 11 80 F1
  {
    .custom /*0C000120:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000121:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 18 4D 65 74 68 6F 64 73 20 6D 69 67 68 74   // ...Methods might
                                                                                                                                                                                            20 62 65 20 72 65 6D 6F 76 65 64 00 00 )          //  be removed..
    .param [1]/*080001A4*/ 
    .custom /*0C00015F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [5]/*080001A8*/ 
    .custom /*0C000160:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x23d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetMethodImpl

  .method /*06000157*/ public hidebysig newslot abstract virtual 
          instance int32  GetMethodMetadataToken(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ constructor) cil managed
  // SIG: 20 01 08 12 80 9D
  {
    // Method begins at RVA 0x0
  } // end of method ModuleBuilder::GetMethodMetadataToken

  .method /*06000158*/ public hidebysig newslot abstract virtual 
          instance int32  GetMethodMetadataToken(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 'method') cil managed
  // SIG: 20 01 08 12 59
  {
    // Method begins at RVA 0x0
  } // end of method ModuleBuilder::GetMethodMetadataToken

  .method /*06000159*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/[] 
          GetMethods(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingFlags) cil managed
  // SIG: 20 01 1D 12 59 11 80 D1
  {
    .custom /*0C000123:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 18 4D 65 74 68 6F 64 73 20 6D 69 67 68 74   // ...Methods might
                                                                                                                                                                                            20 62 65 20 72 65 6D 6F 76 65 64 00 00 )          //  be removed..
    // Method begins at RVA 0x23dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetMethods

  .method /*0600015A*/ public hidebysig virtual 
          instance void  GetPEKind([out] valuetype [System.Runtime/*23000001*/]System.Reflection.PortableExecutableKinds/*01000050*/& peKind,
                                   [out] valuetype [System.Runtime/*23000001*/]System.Reflection.ImageFileMachine/*01000051*/& machine) cil managed
  // SIG: 20 02 01 10 11 81 41 10 11 81 45
  {
    // Method begins at RVA 0x23df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetPEKind

  .method /*0600015B*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          GetType(string className) cil managed
  // SIG: 20 01 12 75 0E
  {
    .custom /*0C000125:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 6E 54 79 70 65 73 20 6D 69 67 68 74 20 62   // ..nTypes might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 20 62 79 20 74 72 69   // e removed by tri
                                                                                                                                                                                            6D 6D 69 6E 67 2E 20 49 66 20 74 68 65 20 74 79   // mming. If the ty
                                                                                                                                                                                            70 65 20 6E 61 6D 65 20 69 73 20 61 20 73 74 72   // pe name is a str
                                                                                                                                                                                            69 6E 67 20 6C 69 74 65 72 61 6C 2C 20 63 6F 6E   // ing literal, con
                                                                                                                                                                                            73 69 64 65 72 20 75 73 69 6E 67 20 54 79 70 65   // sider using Type
                                                                                                                                                                                            2E 47 65 74 54 79 70 65 20 69 6E 73 74 65 61 64   // .GetType instead
                                                                                                                                                                                            2E 00 00 )                                        // ...
    .param [0]/*080001AF*/ 
    .custom /*0C000162:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetType

  .method /*0600015C*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          GetType(string className,
                  bool ignoreCase) cil managed
  // SIG: 20 02 12 75 0E 02
  {
    .custom /*0C000126:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 6E 54 79 70 65 73 20 6D 69 67 68 74 20 62   // ..nTypes might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 20 62 79 20 74 72 69   // e removed by tri
                                                                                                                                                                                            6D 6D 69 6E 67 2E 20 49 66 20 74 68 65 20 74 79   // mming. If the ty
                                                                                                                                                                                            70 65 20 6E 61 6D 65 20 69 73 20 61 20 73 74 72   // pe name is a str
                                                                                                                                                                                            69 6E 67 20 6C 69 74 65 72 61 6C 2C 20 63 6F 6E   // ing literal, con
                                                                                                                                                                                            73 69 64 65 72 20 75 73 69 6E 67 20 54 79 70 65   // sider using Type
                                                                                                                                                                                            2E 47 65 74 54 79 70 65 20 69 6E 73 74 65 61 64   // .GetType instead
                                                                                                                                                                                            2E 00 00 )                                        // ...
    .param [0]/*080001B1*/ 
    .custom /*0C000164:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetType

  .method /*0600015D*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          GetType(string className,
                  bool throwOnError,
                  bool ignoreCase) cil managed
  // SIG: 20 03 12 75 0E 02 02
  {
    .custom /*0C000127:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 6E 54 79 70 65 73 20 6D 69 67 68 74 20 62   // ..nTypes might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 20 62 79 20 74 72 69   // e removed by tri
                                                                                                                                                                                            6D 6D 69 6E 67 2E 20 49 66 20 74 68 65 20 74 79   // mming. If the ty
                                                                                                                                                                                            70 65 20 6E 61 6D 65 20 69 73 20 61 20 73 74 72   // pe name is a str
                                                                                                                                                                                            69 6E 67 20 6C 69 74 65 72 61 6C 2C 20 63 6F 6E   // ing literal, con
                                                                                                                                                                                            73 69 64 65 72 20 75 73 69 6E 67 20 54 79 70 65   // sider using Type
                                                                                                                                                                                            2E 47 65 74 54 79 70 65 20 69 6E 73 74 65 61 64   // .GetType instead
                                                                                                                                                                                            2E 00 00 )                                        // ...
    .param [0]/*080001B4*/ 
    .custom /*0C000165:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetType

  .method /*0600015E*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] 
          GetTypes() cil managed
  // SIG: 20 00 1D 12 75
  {
    .custom /*0C000128:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 16 54 79 70 65 73 20 6D 69 67 68 74 20 62   // ...Types might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 00 00 )                // e removed..
    // Method begins at RVA 0x23eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::GetTypes

  .method /*0600015F*/ public hidebysig newslot abstract virtual 
          instance int32  GetTypeMetadataToken(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 'type') cil managed
  // SIG: 20 01 08 12 75
  {
    // Method begins at RVA 0x0
  } // end of method ModuleBuilder::GetTypeMetadataToken

  .method /*06000160*/ public hidebysig newslot abstract virtual 
          instance int32  GetSignatureMetadataToken(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.SignatureHelper/*01000052*/ signature) cil managed
  // SIG: 20 01 08 12 81 49
  {
    // Method begins at RVA 0x0
  } // end of method ModuleBuilder::GetSignatureMetadataToken

  .method /*06000161*/ public hidebysig newslot abstract virtual 
          instance int32  GetStringMetadataToken(string stringConstant) cil managed
  // SIG: 20 01 08 0E
  {
    // Method begins at RVA 0x0
  } // end of method ModuleBuilder::GetStringMetadataToken

  .method /*06000162*/ public hidebysig virtual 
          instance bool  IsDefined(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ attributeType,
                                   bool inherit) cil managed
  // SIG: 20 02 02 12 75 02
  {
    // Method begins at RVA 0x23ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::IsDefined

  .method /*06000163*/ public hidebysig virtual 
          instance bool  IsResource() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x23f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::IsResource

  .method /*06000164*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003E*/ 
          ResolveField(int32 metadataToken,
                       class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] genericTypeArguments,
                       class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] genericMethodArguments) cil managed
  // SIG: 20 03 12 80 F9 08 1D 12 75 1D 12 75
  {
    .custom /*0C00012B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00012C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 20 54 72 69 6D 6D 69 6E 67 20 63 68 61 6E   // .. Trimming chan
                                                                                                                                                                                            67 65 73 20 6D 65 74 61 64 61 74 61 20 74 6F 6B   // ges metadata tok
                                                                                                                                                                                            65 6E 73 00 00 )                                  // ens..
    .param [2]/*080001BE*/ 
    .custom /*0C000166:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [3]/*080001BF*/ 
    .custom /*0C000167:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x23f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::ResolveField

  .method /*06000165*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000040*/ 
          ResolveMember(int32 metadataToken,
                        class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] genericTypeArguments,
                        class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] genericMethodArguments) cil managed
  // SIG: 20 03 12 81 01 08 1D 12 75 1D 12 75
  {
    .custom /*0C00012D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00012E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 20 54 72 69 6D 6D 69 6E 67 20 63 68 61 6E   // .. Trimming chan
                                                                                                                                                                                            67 65 73 20 6D 65 74 61 64 61 74 61 20 74 6F 6B   // ges metadata tok
                                                                                                                                                                                            65 6E 73 00 00 )                                  // ens..
    .param [2]/*080001C1*/ 
    .custom /*0C000168:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [3]/*080001C2*/ 
    .custom /*0C000169:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x23f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::ResolveMember

  .method /*06000166*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodBase/*01000046*/ 
          ResolveMethod(int32 metadataToken,
                        class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] genericTypeArguments,
                        class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] genericMethodArguments) cil managed
  // SIG: 20 03 12 81 19 08 1D 12 75 1D 12 75
  {
    .custom /*0C00012F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000130:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 20 54 72 69 6D 6D 69 6E 67 20 63 68 61 6E   // .. Trimming chan
                                                                                                                                                                                            67 65 73 20 6D 65 74 61 64 61 74 61 20 74 6F 6B   // ges metadata tok
                                                                                                                                                                                            65 6E 73 00 00 )                                  // ens..
    .param [2]/*080001C4*/ 
    .custom /*0C00016A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [3]/*080001C5*/ 
    .custom /*0C00016B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x23fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::ResolveMethod

  .method /*06000167*/ public hidebysig virtual 
          instance uint8[]  ResolveSignature(int32 metadataToken) cil managed
  // SIG: 20 01 1D 05 08
  {
    .custom /*0C000131:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 20 54 72 69 6D 6D 69 6E 67 20 63 68 61 6E   // .. Trimming chan
                                                                                                                                                                                            67 65 73 20 6D 65 74 61 64 61 74 61 20 74 6F 6B   // ges metadata tok
                                                                                                                                                                                            65 6E 73 00 00 )                                  // ens..
    // Method begins at RVA 0x23fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::ResolveSignature

  .method /*06000168*/ public hidebysig virtual 
          instance string  ResolveString(int32 metadataToken) cil managed
  // SIG: 20 01 0E 08
  {
    .custom /*0C000132:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 20 54 72 69 6D 6D 69 6E 67 20 63 68 61 6E   // .. Trimming chan
                                                                                                                                                                                            67 65 73 20 6D 65 74 61 64 61 74 61 20 74 6F 6B   // ges metadata tok
                                                                                                                                                                                            65 6E 73 00 00 )                                  // ens..
    // Method begins at RVA 0x2400
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::ResolveString

  .method /*06000169*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          ResolveType(int32 metadataToken,
                      class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] genericTypeArguments,
                      class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] genericMethodArguments) cil managed
  // SIG: 20 03 12 75 08 1D 12 75 1D 12 75
  {
    .custom /*0C000133:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 20 54 72 69 6D 6D 69 6E 67 20 63 68 61 6E   // .. Trimming chan
                                                                                                                                                                                            67 65 73 20 6D 65 74 61 64 61 74 61 20 74 6F 6B   // ges metadata tok
                                                                                                                                                                                            65 6E 73 00 00 )                                  // ens..
    .param [2]/*080001C9*/ 
    .custom /*0C00016C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [3]/*080001CA*/ 
    .custom /*0C00016D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2403
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ModuleBuilder::ResolveType

  .method /*0600016A*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 1D 05
  {
    // Method begins at RVA 0x2406
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ModuleBuilder::SetCustomAttribute

  .method /*0600016B*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001B*/ customBuilder) cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x2408
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ModuleBuilder::SetCustomAttribute

  .method /*0600016C*/ family hidebysig newslot abstract virtual 
          instance void  SetCustomAttributeCore(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000028*/<uint8> binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 15 11 80 A1 01 05
  {
    .custom /*0C000136:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    .param [1]/*080001CE*/ 
    .custom /*0C00016E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ModuleBuilder::SetCustomAttributeCore

  .property /*17000059*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/
          Assembly()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ System.Reflection.Emit.ModuleBuilder/*0200000A*/::get_Assembly() /* 0600012B */
  } // end of property ModuleBuilder::Assembly
  .property /*1700005A*/ instance callconv(8) string
          FullyQualifiedName()
  {
    .custom /*0C000070:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresAssemblyFilesAttribute/*01000021*/::.ctor(string) /* 0A000017 */ = ( 01 00 2F 52 65 74 75 72 6E 73 20 3C 55 6E 6B 6E   // ../Returns <Unkn
                                                                                                                                                                                         6F 77 6E 3E 20 66 6F 72 20 6D 6F 64 75 6C 65 73   // own> for modules
                                                                                                                                                                                         20 77 69 74 68 20 6E 6F 20 66 69 6C 65 20 70 61   //  with no file pa
                                                                                                                                                                                         74 68 00 00 )                                     // th..
    .get instance string System.Reflection.Emit.ModuleBuilder/*0200000A*/::get_FullyQualifiedName() /* 0600012C */
  } // end of property ModuleBuilder::FullyQualifiedName
  .property /*1700005B*/ instance callconv(8) int32
          MDStreamVersion()
  {
    .get instance int32 System.Reflection.Emit.ModuleBuilder/*0200000A*/::get_MDStreamVersion() /* 0600012D */
  } // end of property ModuleBuilder::MDStreamVersion
  .property /*1700005C*/ instance callconv(8) int32
          MetadataToken()
  {
    .get instance int32 System.Reflection.Emit.ModuleBuilder/*0200000A*/::get_MetadataToken() /* 0600012E */
  } // end of property ModuleBuilder::MetadataToken
  .property /*1700005D*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Guid/*01000037*/
          ModuleVersionId()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Guid/*01000037*/ System.Reflection.Emit.ModuleBuilder/*0200000A*/::get_ModuleVersionId() /* 0600012F */
  } // end of property ModuleBuilder::ModuleVersionId
  .property /*1700005E*/ instance callconv(8) string
          Name()
  {
    .custom /*0C000072:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresAssemblyFilesAttribute/*01000021*/::.ctor(string) /* 0A000017 */ = ( 01 00 2F 52 65 74 75 72 6E 73 20 3C 55 6E 6B 6E   // ../Returns <Unkn
                                                                                                                                                                                         6F 77 6E 3E 20 66 6F 72 20 6D 6F 64 75 6C 65 73   // own> for modules
                                                                                                                                                                                         20 77 69 74 68 20 6E 6F 20 66 69 6C 65 20 70 61   //  with no file pa
                                                                                                                                                                                         74 68 00 00 )                                     // th..
    .get instance string System.Reflection.Emit.ModuleBuilder/*0200000A*/::get_Name() /* 06000130 */
  } // end of property ModuleBuilder::Name
  .property /*1700005F*/ instance callconv(8) string
          ScopeName()
  {
    .get instance string System.Reflection.Emit.ModuleBuilder/*0200000A*/::get_ScopeName() /* 06000131 */
  } // end of property ModuleBuilder::ScopeName
} // end of class System.Reflection.Emit.ModuleBuilder

.class /*0200000B*/ public auto ansi sealed beforefieldinit System.Reflection.Emit.PersistedAssemblyBuilder
       extends System.Reflection.Emit.AssemblyBuilder/*02000002*/
{
  .custom /*0C000031:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000032:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*0600016D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Reflection.AssemblyName/*01000019*/ name,
                               class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ coreAssembly,
                               [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001A*/<class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001B*/> assemblyAttributes) cil managed
  // SIG: 20 03 01 12 65 12 55 15 12 69 01 12 6D
  {
    .param [3]/*080001D2*/  = nullref
    .custom /*0C00016F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x240a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000001       */ call       instance void System.Reflection.Emit.AssemblyBuilder/*02000002*/::.ctor() /* 06000001 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PersistedAssemblyBuilder::.ctor

  .method /*0600016E*/ public hidebysig specialname virtual 
          instance string  get_FullName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000137:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2412
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PersistedAssemblyBuilder::get_FullName

  .method /*0600016F*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ 
          get_ManifestModule() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x2415
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PersistedAssemblyBuilder::get_ManifestModule

  .method /*06000170*/ family hidebysig virtual 
          instance class System.Reflection.Emit.ModuleBuilder/*0200000A*/ 
          DefineDynamicModuleCore(string name) cil managed
  // SIG: 20 01 12 28 0E
  {
    .custom /*0C000138:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000018*/::.ctor(string) /* 0A000014 */ = ( 01 00 32 44 65 66 69 6E 69 6E 67 20 61 20 64 79   // ..2Defining a dy
                                                                                                                                                                                       6E 61 6D 69 63 20 61 73 73 65 6D 62 6C 79 20 72   // namic assembly r
                                                                                                                                                                                       65 71 75 69 72 65 73 20 64 79 6E 61 6D 69 63 20   // equires dynamic 
                                                                                                                                                                                       63 6F 64 65 2E 00 00 )                            // code...
    // Method begins at RVA 0x2418
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PersistedAssemblyBuilder::DefineDynamicModuleCore

  .method /*06000171*/ public hidebysig instance class [System.Reflection.Metadata/*23000006*/]System.Reflection.Metadata.Ecma335.MetadataBuilder/*01000053*/ 
          GenerateMetadata([out] class [System.Reflection.Metadata/*23000006*/]System.Reflection.Metadata.BlobBuilder/*01000054*/& ilStream,
                           [out] class [System.Reflection.Metadata/*23000006*/]System.Reflection.Metadata.BlobBuilder/*01000054*/& mappedFieldData) cil managed
  // SIG: 20 02 12 81 4D 10 12 81 51 10 12 81 51
  {
    .custom /*0C000139:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x241b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PersistedAssemblyBuilder::GenerateMetadata

  .method /*06000172*/ public hidebysig instance class [System.Reflection.Metadata/*23000006*/]System.Reflection.Metadata.Ecma335.MetadataBuilder/*01000053*/ 
          GenerateMetadata([out] class [System.Reflection.Metadata/*23000006*/]System.Reflection.Metadata.BlobBuilder/*01000054*/& ilStream,
                           [out] class [System.Reflection.Metadata/*23000006*/]System.Reflection.Metadata.BlobBuilder/*01000054*/& mappedFieldData,
                           [out] class [System.Reflection.Metadata/*23000006*/]System.Reflection.Metadata.Ecma335.MetadataBuilder/*01000053*/& pdbBuilder) cil managed
  // SIG: 20 03 12 81 4D 10 12 81 51 10 12 81 51 10 12 81 4D
  {
    .custom /*0C00013A:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x241e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PersistedAssemblyBuilder::GenerateMetadata

  .method /*06000173*/ family hidebysig virtual 
          instance class System.Reflection.Emit.ModuleBuilder/*0200000A*/ 
          GetDynamicModuleCore(string name) cil managed
  // SIG: 20 01 12 28 0E
  {
    .param [0]/*080001D9*/ 
    .custom /*0C000171:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2421
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PersistedAssemblyBuilder::GetDynamicModuleCore

  .method /*06000174*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.AssemblyName/*01000019*/ 
          GetName(bool copiedName) cil managed
  // SIG: 20 01 12 65 02
  {
    // Method begins at RVA 0x2424
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PersistedAssemblyBuilder::GetName

  .method /*06000175*/ public hidebysig instance void 
          Save(string assemblyFileName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2427
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PersistedAssemblyBuilder::Save

  .method /*06000176*/ public hidebysig instance void 
          Save(class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/ 'stream') cil managed
  // SIG: 20 01 01 12 80 91
  {
    // Method begins at RVA 0x242a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PersistedAssemblyBuilder::Save

  .method /*06000177*/ family hidebysig virtual 
          instance void  SetCustomAttributeCore(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000028*/<uint8> binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 15 11 80 A1 01 05
  {
    .custom /*0C00013F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    .param [1]/*080001DE*/ 
    .custom /*0C000175:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x242d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PersistedAssemblyBuilder::SetCustomAttributeCore

  .property /*17000060*/ instance callconv(8) string
          FullName()
  {
    .custom /*0C000073:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.PersistedAssemblyBuilder/*0200000B*/::get_FullName() /* 0600016E */
  } // end of property PersistedAssemblyBuilder::FullName
  .property /*17000061*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/
          ManifestModule()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ System.Reflection.Emit.PersistedAssemblyBuilder/*0200000B*/::get_ManifestModule() /* 0600016F */
  } // end of property PersistedAssemblyBuilder::ManifestModule
} // end of class System.Reflection.Emit.PersistedAssemblyBuilder

.class /*0200000C*/ public abstract auto ansi beforefieldinit System.Reflection.Emit.PropertyBuilder
       extends [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000042*/
{
  .custom /*0C000034:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000035:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000178*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2430
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000042*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PropertyBuilder::.ctor

  .method /*06000179*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.PropertyAttributes/*01000055*/ 
          get_Attributes() cil managed
  // SIG: 20 00 11 81 55
  {
    // Method begins at RVA 0x2438
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::get_Attributes

  .method /*0600017A*/ public hidebysig specialname virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x243b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::get_CanRead

  .method /*0600017B*/ public hidebysig specialname virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x243e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::get_CanWrite

  .method /*0600017C*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_DeclaringType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C000142:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2441
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::get_DeclaringType

  .method /*0600017D*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ 
          get_Module() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x2444
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::get_Module

  .method /*0600017E*/ public hidebysig specialname virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2447
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::get_Name

  .method /*0600017F*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_PropertyType() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x244a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::get_PropertyType

  .method /*06000180*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_ReflectedType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C000145:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x244d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::get_ReflectedType

  .method /*06000181*/ public hidebysig instance void 
          AddOtherMethod(class System.Reflection.Emit.MethodBuilder/*02000009*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x2450
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PropertyBuilder::AddOtherMethod

  .method /*06000182*/ family hidebysig newslot abstract virtual 
          instance void  AddOtherMethodCore(class System.Reflection.Emit.MethodBuilder/*02000009*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x0
  } // end of method PropertyBuilder::AddOtherMethodCore

  .method /*06000183*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/[] 
          GetAccessors(bool nonPublic) cil managed
  // SIG: 20 01 1D 12 59 02
  {
    // Method begins at RVA 0x2452
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::GetAccessors

  .method /*06000184*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(bool inherit) cil managed
  // SIG: 20 01 1D 1C 02
  {
    // Method begins at RVA 0x2455
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::GetCustomAttributes

  .method /*06000185*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ attributeType,
                                                 bool inherit) cil managed
  // SIG: 20 02 1D 1C 12 75 02
  {
    // Method begins at RVA 0x2458
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::GetCustomAttributes

  .method /*06000186*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetGetMethod(bool nonPublic) cil managed
  // SIG: 20 01 12 59 02
  {
    .custom /*0C000149:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x245b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::GetGetMethod

  .method /*06000187*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*01000033*/[] 
          GetIndexParameters() cil managed
  // SIG: 20 00 1D 12 80 CD
  {
    // Method begins at RVA 0x245e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::GetIndexParameters

  .method /*06000188*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetSetMethod(bool nonPublic) cil managed
  // SIG: 20 01 12 59 02
  {
    .custom /*0C00014C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2461
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::GetSetMethod

  .method /*06000189*/ public hidebysig virtual 
          instance object  GetValue(object obj,
                                    object[] index) cil managed
  // SIG: 20 02 1C 1C 1D 1C
  {
    .custom /*0C00014E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001E8*/ 
    .custom /*0C000178:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2464
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::GetValue

  .method /*0600018A*/ public hidebysig virtual 
          instance object  GetValue(object obj,
                                    valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ invokeAttr,
                                    class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000035*/ binder,
                                    object[] index,
                                    class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000025*/ culture) cil managed
  // SIG: 20 05 1C 1C 11 80 D1 12 80 D5 1D 1C 12 80 95
  {
    .custom /*0C00014F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080001EB*/ 
    .custom /*0C00017A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2467
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::GetValue

  .method /*0600018B*/ public hidebysig virtual 
          instance bool  IsDefined(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ attributeType,
                                   bool inherit) cil managed
  // SIG: 20 02 02 12 75 02
  {
    // Method begins at RVA 0x246a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyBuilder::IsDefined

  .method /*0600018C*/ public hidebysig instance void 
          SetConstant(object defaultValue) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C000150:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x246d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PropertyBuilder::SetConstant

  .method /*0600018D*/ family hidebysig newslot abstract virtual 
          instance void  SetConstantCore(object defaultValue) cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C000151:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method PropertyBuilder::SetConstantCore

  .method /*0600018E*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 1D 05
  {
    // Method begins at RVA 0x246f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PropertyBuilder::SetCustomAttribute

  .method /*0600018F*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001B*/ customBuilder) cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x2471
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PropertyBuilder::SetCustomAttribute

  .method /*06000190*/ family hidebysig newslot abstract virtual 
          instance void  SetCustomAttributeCore(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000028*/<uint8> binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 15 11 80 A1 01 05
  {
    .custom /*0C000152:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    .param [1]/*080001F8*/ 
    .custom /*0C000183:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method PropertyBuilder::SetCustomAttributeCore

  .method /*06000191*/ public hidebysig instance void 
          SetGetMethod(class System.Reflection.Emit.MethodBuilder/*02000009*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x2473
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PropertyBuilder::SetGetMethod

  .method /*06000192*/ family hidebysig newslot abstract virtual 
          instance void  SetGetMethodCore(class System.Reflection.Emit.MethodBuilder/*02000009*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x0
  } // end of method PropertyBuilder::SetGetMethodCore

  .method /*06000193*/ public hidebysig instance void 
          SetSetMethod(class System.Reflection.Emit.MethodBuilder/*02000009*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x2475
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PropertyBuilder::SetSetMethod

  .method /*06000194*/ family hidebysig newslot abstract virtual 
          instance void  SetSetMethodCore(class System.Reflection.Emit.MethodBuilder/*02000009*/ mdBuilder) cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x0
  } // end of method PropertyBuilder::SetSetMethodCore

  .method /*06000195*/ public hidebysig virtual 
          instance void  SetValue(object obj,
                                  object 'value',
                                  object[] index) cil managed
  // SIG: 20 03 01 1C 1C 1D 1C
  {
    .custom /*0C000153:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2477
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PropertyBuilder::SetValue

  .method /*06000196*/ public hidebysig virtual 
          instance void  SetValue(object obj,
                                  object 'value',
                                  valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ invokeAttr,
                                  class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000035*/ binder,
                                  object[] index,
                                  class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000025*/ culture) cil managed
  // SIG: 20 06 01 1C 1C 11 80 D1 12 80 D5 1D 1C 12 80 95
  {
    .custom /*0C000155:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2479
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PropertyBuilder::SetValue

  .property /*17000062*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Reflection.PropertyAttributes/*01000055*/
          Attributes()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Reflection.PropertyAttributes/*01000055*/ System.Reflection.Emit.PropertyBuilder/*0200000C*/::get_Attributes() /* 06000179 */
  } // end of property PropertyBuilder::Attributes
  .property /*17000063*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool System.Reflection.Emit.PropertyBuilder/*0200000C*/::get_CanRead() /* 0600017A */
  } // end of property PropertyBuilder::CanRead
  .property /*17000064*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool System.Reflection.Emit.PropertyBuilder/*0200000C*/::get_CanWrite() /* 0600017B */
  } // end of property PropertyBuilder::CanWrite
  .property /*17000065*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          DeclaringType()
  {
    .custom /*0C00007B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.PropertyBuilder/*0200000C*/::get_DeclaringType() /* 0600017C */
  } // end of property PropertyBuilder::DeclaringType
  .property /*17000066*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/
          Module()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ System.Reflection.Emit.PropertyBuilder/*0200000C*/::get_Module() /* 0600017D */
  } // end of property PropertyBuilder::Module
  .property /*17000067*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Reflection.Emit.PropertyBuilder/*0200000C*/::get_Name() /* 0600017E */
  } // end of property PropertyBuilder::Name
  .property /*17000068*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          PropertyType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.PropertyBuilder/*0200000C*/::get_PropertyType() /* 0600017F */
  } // end of property PropertyBuilder::PropertyType
  .property /*17000069*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          ReflectedType()
  {
    .custom /*0C000080:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.PropertyBuilder/*0200000C*/::get_ReflectedType() /* 06000180 */
  } // end of property PropertyBuilder::ReflectedType
} // end of class System.Reflection.Emit.PropertyBuilder

.class /*0200000D*/ public abstract auto ansi beforefieldinit System.Reflection.Emit.TypeBuilder
       extends [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000036*/
{
  .custom /*0C000038:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000039:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .field /*04000004*/ public static literal int32 UnspecifiedTypeSize = int32(0x00000000)
  .method /*06000197*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x247b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000036*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TypeBuilder::.ctor

  .method /*06000198*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ 
          get_Assembly() cil managed
  // SIG: 20 00 12 55
  {
    // Method begins at RVA 0x2483
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_Assembly

  .method /*06000199*/ public hidebysig specialname virtual 
          instance string  get_AssemblyQualifiedName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000157:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2486
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_AssemblyQualifiedName

  .method /*0600019A*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_BaseType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C000158:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2489
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_BaseType

  .method /*0600019B*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodBase/*01000046*/ 
          get_DeclaringMethod() cil managed
  // SIG: 20 00 12 81 19
  {
    .custom /*0C00015A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x248c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_DeclaringMethod

  .method /*0600019C*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_DeclaringType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C00015C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x248f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_DeclaringType

  .method /*0600019D*/ public hidebysig specialname virtual 
          instance string  get_FullName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00015D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2492
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_FullName

  .method /*0600019E*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.GenericParameterAttributes/*01000047*/ 
          get_GenericParameterAttributes() cil managed
  // SIG: 20 00 11 81 1D
  {
    // Method begins at RVA 0x2495
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_GenericParameterAttributes

  .method /*0600019F*/ public hidebysig specialname virtual 
          instance int32  get_GenericParameterPosition() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2498
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_GenericParameterPosition

  .method /*060001A0*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Guid/*01000037*/ 
          get_GUID() cil managed
  // SIG: 20 00 11 80 DD
  {
    // Method begins at RVA 0x249b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_GUID

  .method /*060001A1*/ public hidebysig specialname virtual 
          instance bool  get_IsByRefLike() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x249e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_IsByRefLike

  .method /*060001A2*/ public hidebysig specialname virtual 
          instance bool  get_IsConstructedGenericType() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_IsConstructedGenericType

  .method /*060001A3*/ public hidebysig specialname virtual 
          instance bool  get_IsGenericParameter() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_IsGenericParameter

  .method /*060001A4*/ public hidebysig specialname virtual 
          instance bool  get_IsGenericType() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_IsGenericType

  .method /*060001A5*/ public hidebysig specialname virtual 
          instance bool  get_IsGenericTypeDefinition() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_IsGenericTypeDefinition

  .method /*060001A6*/ public hidebysig specialname virtual 
          instance bool  get_IsSecurityCritical() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_IsSecurityCritical

  .method /*060001A7*/ public hidebysig specialname virtual 
          instance bool  get_IsSecuritySafeCritical() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_IsSecuritySafeCritical

  .method /*060001A8*/ public hidebysig specialname virtual 
          instance bool  get_IsSecurityTransparent() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_IsSecurityTransparent

  .method /*060001A9*/ public hidebysig specialname virtual 
          instance bool  get_IsSZArray() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_IsSZArray

  .method /*060001AA*/ public hidebysig specialname virtual 
          instance bool  get_IsTypeDefinition() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_IsTypeDefinition

  .method /*060001AB*/ public hidebysig specialname virtual 
          instance int32  get_MetadataToken() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x24bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_MetadataToken

  .method /*060001AC*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ 
          get_Module() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x24bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_Module

  .method /*060001AD*/ public hidebysig specialname virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x24c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_Name

  .method /*060001AE*/ public hidebysig specialname virtual 
          instance string  get_Namespace() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000161:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_Namespace

  .method /*060001AF*/ public hidebysig specialname 
          instance valuetype [System.Reflection.Primitives/*23000005*/]System.Reflection.Emit.PackingSize/*0100004F*/ 
          get_PackingSize() cil managed
  // SIG: 20 00 11 81 3D
  {
    // Method begins at RVA 0x24c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_PackingSize

  .method /*060001B0*/ family hidebysig newslot specialname abstract virtual 
          instance valuetype [System.Reflection.Primitives/*23000005*/]System.Reflection.Emit.PackingSize/*0100004F*/ 
          get_PackingSizeCore() cil managed
  // SIG: 20 00 11 81 3D
  {
    // Method begins at RVA 0x0
  } // end of method TypeBuilder::get_PackingSizeCore

  .method /*060001B1*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_ReflectedType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C000163:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_ReflectedType

  .method /*060001B2*/ public hidebysig specialname 
          instance int32  get_Size() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x24ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_Size

  .method /*060001B3*/ family hidebysig newslot specialname abstract virtual 
          instance int32  get_SizeCore() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method TypeBuilder::get_SizeCore

  .method /*060001B4*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000038*/ 
          get_TypeHandle() cil managed
  // SIG: 20 00 11 80 E1
  {
    // Method begins at RVA 0x24d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_TypeHandle

  .method /*060001B5*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          get_UnderlyingSystemType() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x24d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::get_UnderlyingSystemType

  .method /*060001B6*/ public hidebysig instance void 
          AddInterfaceImplementation(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ interfaceType) cil managed
  // SIG: 20 01 01 12 75
  {
    .param [1]/*08000207*/ 
    .custom /*0C000191:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x24d7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TypeBuilder::AddInterfaceImplementation

  .method /*060001B7*/ family hidebysig newslot abstract virtual 
          instance void  AddInterfaceImplementationCore(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ interfaceType) cil managed
  // SIG: 20 01 01 12 75
  {
    .param [1]/*08000208*/ 
    .custom /*0C000192:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method TypeBuilder::AddInterfaceImplementationCore

  .method /*060001B8*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          CreateType() cil managed
  // SIG: 20 00 12 75
  {
    .param [0]/*08000209*/ 
    .custom /*0C000193:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x24d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::CreateType

  .method /*060001B9*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000036*/ 
          CreateTypeInfo() cil managed
  // SIG: 20 00 12 80 D9
  {
    .param [0]/*0800020A*/ 
    .custom /*0C000194:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x24dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::CreateTypeInfo

  .method /*060001BA*/ family hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000036*/ 
          CreateTypeInfoCore() cil managed
  // SIG: 20 00 12 80 D9
  {
    .param [0]/*0800020B*/ 
    .custom /*0C000195:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method TypeBuilder::CreateTypeInfoCore

  .method /*060001BB*/ public hidebysig instance class System.Reflection.Emit.ConstructorBuilder/*02000004*/ 
          DefineConstructor(valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes,
                            valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                            class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes) cil managed
  // SIG: 20 03 12 10 11 80 B1 11 80 B5 1D 12 75
  {
    .param [3]/*0800020E*/ 
    .custom /*0C000197:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x24df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineConstructor

  .method /*060001BC*/ public hidebysig instance class System.Reflection.Emit.ConstructorBuilder/*02000004*/ 
          DefineConstructor(valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes,
                            valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                            class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes,
                            class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] requiredCustomModifiers,
                            class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] optionalCustomModifiers) cil managed
  // SIG: 20 05 12 10 11 80 B1 11 80 B5 1D 12 75 1D 1D 12 75 1D 1D 12 75
  {
    .param [3]/*08000211*/ 
    .custom /*0C00019B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [4]/*08000212*/ 
    .custom /*0C00019C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [5]/*08000213*/ 
    .custom /*0C00019D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x24e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineConstructor

  .method /*060001BD*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.ConstructorBuilder/*02000004*/ 
          DefineConstructorCore(valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes,
                                valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                                class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes,
                                class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] requiredCustomModifiers,
                                class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] optionalCustomModifiers) cil managed
  // SIG: 20 05 12 10 11 80 B1 11 80 B5 1D 12 75 1D 1D 12 75 1D 1D 12 75
  {
    .param [3]/*08000216*/ 
    .custom /*0C0001A0:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [4]/*08000217*/ 
    .custom /*0C0001A2:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [5]/*08000218*/ 
    .custom /*0C0001A3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method TypeBuilder::DefineConstructorCore

  .method /*060001BE*/ public hidebysig instance class System.Reflection.Emit.ConstructorBuilder/*02000004*/ 
          DefineDefaultConstructor(valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes) cil managed
  // SIG: 20 01 12 10 11 80 B1
  {
    // Method begins at RVA 0x24e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineDefaultConstructor

  .method /*060001BF*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.ConstructorBuilder/*02000004*/ 
          DefineDefaultConstructorCore(valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes) cil managed
  // SIG: 20 01 12 10 11 80 B1
  {
    // Method begins at RVA 0x0
  } // end of method TypeBuilder::DefineDefaultConstructorCore

  .method /*060001C0*/ public hidebysig instance class System.Reflection.Emit.EventBuilder/*02000006*/ 
          DefineEvent(string name,
                      valuetype [System.Runtime/*23000001*/]System.Reflection.EventAttributes/*01000056*/ attributes,
                      class [System.Runtime/*23000001*/]System.Type/*0100001D*/ eventtype) cil managed
  // SIG: 20 03 12 18 0E 11 81 59 12 75
  {
    // Method begins at RVA 0x24e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineEvent

  .method /*060001C1*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.EventBuilder/*02000006*/ 
          DefineEventCore(string name,
                          valuetype [System.Runtime/*23000001*/]System.Reflection.EventAttributes/*01000056*/ attributes,
                          class [System.Runtime/*23000001*/]System.Type/*0100001D*/ eventtype) cil managed
  // SIG: 20 03 12 18 0E 11 81 59 12 75
  {
    // Method begins at RVA 0x0
  } // end of method TypeBuilder::DefineEventCore

  .method /*060001C2*/ public hidebysig instance class System.Reflection.Emit.FieldBuilder/*02000007*/ 
          DefineField(string fieldName,
                      class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 'type',
                      valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000044*/ attributes) cil managed
  // SIG: 20 03 12 1C 0E 12 75 11 81 11
  {
    // Method begins at RVA 0x24eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineField

  .method /*060001C3*/ public hidebysig instance class System.Reflection.Emit.FieldBuilder/*02000007*/ 
          DefineField(string fieldName,
                      class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 'type',
                      class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] requiredCustomModifiers,
                      class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] optionalCustomModifiers,
                      valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000044*/ attributes) cil managed
  // SIG: 20 05 12 1C 0E 12 75 1D 12 75 1D 12 75 11 81 11
  {
    .param [3]/*08000226*/ 
    .custom /*0C0001A4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [4]/*08000227*/ 
    .custom /*0C0001A5:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x24ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineField

  .method /*060001C4*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.FieldBuilder/*02000007*/ 
          DefineFieldCore(string fieldName,
                          class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 'type',
                          class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] requiredCustomModifiers,
                          class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] optionalCustomModifiers,
                          valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000044*/ attributes) cil managed
  // SIG: 20 05 12 1C 0E 12 75 1D 12 75 1D 12 75 11 81 11
  {
    .param [3]/*0800022B*/ 
    .custom /*0C0001A6:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [4]/*0800022C*/ 
    .custom /*0C0001A7:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method TypeBuilder::DefineFieldCore

  .method /*060001C5*/ public hidebysig instance class System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/[] 
          DefineGenericParameters(string[] names) cil managed
  // SIG: 20 01 1D 12 20 1D 0E
  {
    .param [1]/*0800022E*/ 
    .custom /*0C0001A8:0A00001C*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000048*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x24f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineGenericParameters

  .method /*060001C6*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.GenericTypeParameterBuilder/*02000008*/[] 
          DefineGenericParametersCore(string[] names) cil managed
  // SIG: 20 01 1D 12 20 1D 0E
  {
    .param [1]/*0800022F*/ 
    .custom /*0C0001A9:0A00001C*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000048*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method TypeBuilder::DefineGenericParametersCore

  .method /*060001C7*/ public hidebysig instance class System.Reflection.Emit.FieldBuilder/*02000007*/ 
          DefineInitializedData(string name,
                                uint8[] data,
                                valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000044*/ attributes) cil managed
  // SIG: 20 03 12 1C 0E 1D 05 11 81 11
  {
    // Method begins at RVA 0x24f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineInitializedData

  .method /*060001C8*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.FieldBuilder/*02000007*/ 
          DefineInitializedDataCore(string name,
                                    uint8[] data,
                                    valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000044*/ attributes) cil managed
  // SIG: 20 03 12 1C 0E 1D 05 11 81 11
  {
    // Method begins at RVA 0x0
  } // end of method TypeBuilder::DefineInitializedDataCore

  .method /*060001C9*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*02000009*/ 
          DefineMethod(string name,
                       valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes) cil managed
  // SIG: 20 02 12 24 0E 11 80 B1
  {
    // Method begins at RVA 0x24f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineMethod

  .method /*060001CA*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*02000009*/ 
          DefineMethod(string name,
                       valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes,
                       valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention) cil managed
  // SIG: 20 03 12 24 0E 11 80 B1 11 80 B5
  {
    // Method begins at RVA 0x24fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineMethod

  .method /*060001CB*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*02000009*/ 
          DefineMethod(string name,
                       valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes,
                       valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                       class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                       class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes) cil managed
  // SIG: 20 05 12 24 0E 11 80 B1 11 80 B5 12 75 1D 12 75
  {
    .param [4]/*0800023E*/ 
    .custom /*0C0001AA:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800023F*/ 
    .custom /*0C0001AB:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x24fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineMethod

  .method /*060001CC*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*02000009*/ 
          DefineMethod(string name,
                       valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes,
                       valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                       class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                       class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] returnTypeRequiredCustomModifiers,
                       class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] returnTypeOptionalCustomModifiers,
                       class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes,
                       class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] parameterTypeRequiredCustomModifiers,
                       class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] parameterTypeOptionalCustomModifiers) cil managed
  // SIG: 20 09 12 24 0E 11 80 B1 11 80 B5 12 75 1D 12 75 1D 12 75 1D 12 75 1D 1D 12 75 1D 1D 12 75
  {
    .param [4]/*08000243*/ 
    .custom /*0C0001AC:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000244*/ 
    .custom /*0C0001AD:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [6]/*08000245*/ 
    .custom /*0C0001AE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [7]/*08000246*/ 
    .custom /*0C0001AF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [8]/*08000247*/ 
    .custom /*0C0001B0:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [9]/*08000248*/ 
    .custom /*0C0001B1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x2500
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineMethod

  .method /*060001CD*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.MethodBuilder/*02000009*/ 
          DefineMethodCore(string name,
                           valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes,
                           valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                           class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                           class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] returnTypeRequiredCustomModifiers,
                           class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] returnTypeOptionalCustomModifiers,
                           class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes,
                           class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] parameterTypeRequiredCustomModifiers,
                           class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] parameterTypeOptionalCustomModifiers) cil managed
  // SIG: 20 09 12 24 0E 11 80 B1 11 80 B5 12 75 1D 12 75 1D 12 75 1D 12 75 1D 1D 12 75 1D 1D 12 75
  {
    .param [4]/*0800024C*/ 
    .custom /*0C0001B2:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800024D*/ 
    .custom /*0C0001B3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [6]/*0800024E*/ 
    .custom /*0C0001B4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [7]/*0800024F*/ 
    .custom /*0C0001B5:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [8]/*08000250*/ 
    .custom /*0C0001B6:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [9]/*08000251*/ 
    .custom /*0C0001B7:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method TypeBuilder::DefineMethodCore

  .method /*060001CE*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*02000009*/ 
          DefineMethod(string name,
                       valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes,
                       class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                       class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes) cil managed
  // SIG: 20 04 12 24 0E 11 80 B1 12 75 1D 12 75
  {
    .param [3]/*08000254*/ 
    .custom /*0C0001B8:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000255*/ 
    .custom /*0C0001B9:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2503
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineMethod

  .method /*060001CF*/ public hidebysig instance void 
          DefineMethodOverride(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ methodInfoBody,
                               class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ methodInfoDeclaration) cil managed
  // SIG: 20 02 01 12 59 12 59
  {
    // Method begins at RVA 0x2506
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TypeBuilder::DefineMethodOverride

  .method /*060001D0*/ family hidebysig newslot abstract virtual 
          instance void  DefineMethodOverrideCore(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ methodInfoBody,
                                                  class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ methodInfoDeclaration) cil managed
  // SIG: 20 02 01 12 59 12 59
  {
    // Method begins at RVA 0x0
  } // end of method TypeBuilder::DefineMethodOverrideCore

  .method /*060001D1*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*0200000D*/ 
          DefineNestedType(string name) cil managed
  // SIG: 20 01 12 34 0E
  {
    // Method begins at RVA 0x2508
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineNestedType

  .method /*060001D2*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*0200000D*/ 
          DefineNestedType(string name,
                           valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*0100003B*/ attr) cil managed
  // SIG: 20 02 12 34 0E 11 80 ED
  {
    // Method begins at RVA 0x250b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineNestedType

  .method /*060001D3*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*0200000D*/ 
          DefineNestedType(string name,
                           valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*0100003B*/ attr,
                           class [System.Runtime/*23000001*/]System.Type/*0100001D*/ parent) cil managed
  // SIG: 20 03 12 34 0E 11 80 ED 12 75
  {
    .param [3]/*0800025F*/ 
    .custom /*0C0001BA:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001BB:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x250e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineNestedType

  .method /*060001D4*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*0200000D*/ 
          DefineNestedType(string name,
                           valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*0100003B*/ attr,
                           class [System.Runtime/*23000001*/]System.Type/*0100001D*/ parent,
                           int32 typeSize) cil managed
  // SIG: 20 04 12 34 0E 11 80 ED 12 75 08
  {
    .param [3]/*08000262*/ 
    .custom /*0C0001BC:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001BD:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x2511
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineNestedType

  .method /*060001D5*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*0200000D*/ 
          DefineNestedType(string name,
                           valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*0100003B*/ attr,
                           class [System.Runtime/*23000001*/]System.Type/*0100001D*/ parent,
                           valuetype [System.Reflection.Primitives/*23000005*/]System.Reflection.Emit.PackingSize/*0100004F*/ packSize) cil managed
  // SIG: 20 04 12 34 0E 11 80 ED 12 75 11 81 3D
  {
    .param [3]/*08000266*/ 
    .custom /*0C0001BE:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001BF:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x2514
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineNestedType

  .method /*060001D6*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*0200000D*/ 
          DefineNestedType(string name,
                           valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*0100003B*/ attr,
                           class [System.Runtime/*23000001*/]System.Type/*0100001D*/ parent,
                           valuetype [System.Reflection.Primitives/*23000005*/]System.Reflection.Emit.PackingSize/*0100004F*/ packSize,
                           int32 typeSize) cil managed
  // SIG: 20 05 12 34 0E 11 80 ED 12 75 11 81 3D 08
  {
    .param [3]/*0800026A*/ 
    .custom /*0C0001C0:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001C1:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x2517
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineNestedType

  .method /*060001D7*/ public hidebysig instance class System.Reflection.Emit.TypeBuilder/*0200000D*/ 
          DefineNestedType(string name,
                           valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*0100003B*/ attr,
                           class [System.Runtime/*23000001*/]System.Type/*0100001D*/ parent,
                           class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] interfaces) cil managed
  // SIG: 20 04 12 34 0E 11 80 ED 12 75 1D 12 75
  {
    .param [3]/*0800026F*/ 
    .custom /*0C0001C2:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001C3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    .param [4]/*08000270*/ 
    .custom /*0C0001C4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x251a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineNestedType

  .method /*060001D8*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.TypeBuilder/*0200000D*/ 
          DefineNestedTypeCore(string name,
                               valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*0100003B*/ attr,
                               class [System.Runtime/*23000001*/]System.Type/*0100001D*/ parent,
                               class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] interfaces,
                               valuetype [System.Reflection.Primitives/*23000005*/]System.Reflection.Emit.PackingSize/*0100004F*/ packSize,
                               int32 typeSize) cil managed
  // SIG: 20 06 12 34 0E 11 80 ED 12 75 1D 12 75 11 81 3D 08
  {
    .param [3]/*08000273*/ 
    .custom /*0C0001C5:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001C6:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    .param [4]/*08000274*/ 
    .custom /*0C0001C7:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method TypeBuilder::DefineNestedTypeCore

  .method /*060001D9*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*02000009*/ 
          DefinePInvokeMethod(string name,
                              string dllName,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                              class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                              class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes,
                              valuetype [System.Runtime.InteropServices/*23000004*/]System.Runtime.InteropServices.CallingConvention/*0100004D*/ nativeCallConv,
                              valuetype [System.Runtime/*23000001*/]System.Runtime.InteropServices.CharSet/*0100004E*/ nativeCharSet) cil managed
  // SIG: 20 08 12 24 0E 0E 11 80 B1 11 80 B5 12 75 1D 12 75 11 81 35 11 81 39
  {
    .custom /*0C000170:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 4A 50 2F 49 6E 76 6F 6B 65 20 6D 61 72 73   // ..JP/Invoke mars
                                                                                                                                                                                            68 61 6C 6C 69 6E 67 20 6D 61 79 20 64 79 6E 61   // halling may dyna
                                                                                                                                                                                            6D 69 63 61 6C 6C 79 20 61 63 63 65 73 73 20 6D   // mically access m
                                                                                                                                                                                            65 6D 62 65 72 73 20 74 68 61 74 20 63 6F 75 6C   // embers that coul
                                                                                                                                                                                            64 20 62 65 20 74 72 69 6D 6D 65 64 2E 00 00 )    // d be trimmed...
    .param [5]/*0800027B*/ 
    .custom /*0C0001C8:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [6]/*0800027C*/ 
    .custom /*0C0001C9:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x251d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefinePInvokeMethod

  .method /*060001DA*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*02000009*/ 
          DefinePInvokeMethod(string name,
                              string dllName,
                              string entryName,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                              class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                              class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes,
                              valuetype [System.Runtime.InteropServices/*23000004*/]System.Runtime.InteropServices.CallingConvention/*0100004D*/ nativeCallConv,
                              valuetype [System.Runtime/*23000001*/]System.Runtime.InteropServices.CharSet/*0100004E*/ nativeCharSet) cil managed
  // SIG: 20 09 12 24 0E 0E 0E 11 80 B1 11 80 B5 12 75 1D 12 75 11 81 35 11 81 39
  {
    .custom /*0C000172:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 4A 50 2F 49 6E 76 6F 6B 65 20 6D 61 72 73   // ..JP/Invoke mars
                                                                                                                                                                                            68 61 6C 6C 69 6E 67 20 6D 61 79 20 64 79 6E 61   // halling may dyna
                                                                                                                                                                                            6D 69 63 61 6C 6C 79 20 61 63 63 65 73 73 20 6D   // mically access m
                                                                                                                                                                                            65 6D 62 65 72 73 20 74 68 61 74 20 63 6F 75 6C   // embers that coul
                                                                                                                                                                                            64 20 62 65 20 74 72 69 6D 6D 65 64 2E 00 00 )    // d be trimmed...
    .param [6]/*08000284*/ 
    .custom /*0C0001CA:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [7]/*08000285*/ 
    .custom /*0C0001CB:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2520
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefinePInvokeMethod

  .method /*060001DB*/ public hidebysig instance class System.Reflection.Emit.MethodBuilder/*02000009*/ 
          DefinePInvokeMethod(string name,
                              string dllName,
                              string entryName,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes,
                              valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                              class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                              class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] returnTypeRequiredCustomModifiers,
                              class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] returnTypeOptionalCustomModifiers,
                              class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes,
                              class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] parameterTypeRequiredCustomModifiers,
                              class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] parameterTypeOptionalCustomModifiers,
                              valuetype [System.Runtime.InteropServices/*23000004*/]System.Runtime.InteropServices.CallingConvention/*0100004D*/ nativeCallConv,
                              valuetype [System.Runtime/*23000001*/]System.Runtime.InteropServices.CharSet/*0100004E*/ nativeCharSet) cil managed
  // SIG: 20 0D 12 24 0E 0E 0E 11 80 B1 11 80 B5 12 75 1D 12 75 1D 12 75 1D 12 75 1D 1D 12 75 1D 1D 12 75
  //      11 81 35 11 81 39
  {
    .custom /*0C000173:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 4A 50 2F 49 6E 76 6F 6B 65 20 6D 61 72 73   // ..JP/Invoke mars
                                                                                                                                                                                            68 61 6C 6C 69 6E 67 20 6D 61 79 20 64 79 6E 61   // halling may dyna
                                                                                                                                                                                            6D 69 63 61 6C 6C 79 20 61 63 63 65 73 73 20 6D   // mically access m
                                                                                                                                                                                            65 6D 62 65 72 73 20 74 68 61 74 20 63 6F 75 6C   // embers that coul
                                                                                                                                                                                            64 20 62 65 20 74 72 69 6D 6D 65 64 2E 00 00 )    // d be trimmed...
    .param [6]/*0800028D*/ 
    .custom /*0C0001CC:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [7]/*0800028E*/ 
    .custom /*0C0001CD:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [8]/*0800028F*/ 
    .custom /*0C0001CE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [9]/*08000290*/ 
    .custom /*0C0001CF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [10]/*08000291*/ 
    .custom /*0C0001D0:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [11]/*08000292*/ 
    .custom /*0C0001D1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x2523
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefinePInvokeMethod

  .method /*060001DC*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.MethodBuilder/*02000009*/ 
          DefinePInvokeMethodCore(string name,
                                  string dllName,
                                  string entryName,
                                  valuetype [System.Runtime/*23000001*/]System.Reflection.MethodAttributes/*0100002C*/ attributes,
                                  valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                                  class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                                  class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] returnTypeRequiredCustomModifiers,
                                  class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] returnTypeOptionalCustomModifiers,
                                  class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes,
                                  class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] parameterTypeRequiredCustomModifiers,
                                  class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] parameterTypeOptionalCustomModifiers,
                                  valuetype [System.Runtime.InteropServices/*23000004*/]System.Runtime.InteropServices.CallingConvention/*0100004D*/ nativeCallConv,
                                  valuetype [System.Runtime/*23000001*/]System.Runtime.InteropServices.CharSet/*0100004E*/ nativeCharSet) cil managed
  // SIG: 20 0D 12 24 0E 0E 0E 11 80 B1 11 80 B5 12 75 1D 12 75 1D 12 75 1D 12 75 1D 1D 12 75 1D 1D 12 75
  //      11 81 35 11 81 39
  {
    .custom /*0C000174:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 4A 50 2F 49 6E 76 6F 6B 65 20 6D 61 72 73   // ..JP/Invoke mars
                                                                                                                                                                                            68 61 6C 6C 69 6E 67 20 6D 61 79 20 64 79 6E 61   // halling may dyna
                                                                                                                                                                                            6D 69 63 61 6C 6C 79 20 61 63 63 65 73 73 20 6D   // mically access m
                                                                                                                                                                                            65 6D 62 65 72 73 20 74 68 61 74 20 63 6F 75 6C   // embers that coul
                                                                                                                                                                                            64 20 62 65 20 74 72 69 6D 6D 65 64 2E 00 00 )    // d be trimmed...
    .param [6]/*0800029A*/ 
    .custom /*0C0001D2:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [7]/*0800029B*/ 
    .custom /*0C0001D3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [8]/*0800029C*/ 
    .custom /*0C0001D4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [9]/*0800029D*/ 
    .custom /*0C0001D5:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [10]/*0800029E*/ 
    .custom /*0C0001D6:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [11]/*0800029F*/ 
    .custom /*0C0001D7:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method TypeBuilder::DefinePInvokeMethodCore

  .method /*060001DD*/ public hidebysig instance class System.Reflection.Emit.PropertyBuilder/*0200000C*/ 
          DefineProperty(string name,
                         valuetype [System.Runtime/*23000001*/]System.Reflection.PropertyAttributes/*01000055*/ attributes,
                         valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes) cil managed
  // SIG: 20 05 12 30 0E 11 81 55 11 80 B5 12 75 1D 12 75
  {
    .param [4]/*080002A5*/ 
    .custom /*0C0001D8:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080002A6*/ 
    .custom /*0C0001D9:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2526
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineProperty

  .method /*060001DE*/ public hidebysig instance class System.Reflection.Emit.PropertyBuilder/*0200000C*/ 
          DefineProperty(string name,
                         valuetype [System.Runtime/*23000001*/]System.Reflection.PropertyAttributes/*01000055*/ attributes,
                         valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] returnTypeRequiredCustomModifiers,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] returnTypeOptionalCustomModifiers,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] parameterTypeRequiredCustomModifiers,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] parameterTypeOptionalCustomModifiers) cil managed
  // SIG: 20 09 12 30 0E 11 81 55 11 80 B5 12 75 1D 12 75 1D 12 75 1D 12 75 1D 1D 12 75 1D 1D 12 75
  {
    .param [4]/*080002AA*/ 
    .custom /*0C0001DA:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080002AB*/ 
    .custom /*0C0001DB:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [6]/*080002AC*/ 
    .custom /*0C0001DC:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [7]/*080002AD*/ 
    .custom /*0C0001DD:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [8]/*080002AE*/ 
    .custom /*0C0001DE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [9]/*080002AF*/ 
    .custom /*0C0001DF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x2529
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineProperty

  .method /*060001DF*/ public hidebysig instance class System.Reflection.Emit.PropertyBuilder/*0200000C*/ 
          DefineProperty(string name,
                         valuetype [System.Runtime/*23000001*/]System.Reflection.PropertyAttributes/*01000055*/ attributes,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes) cil managed
  // SIG: 20 04 12 30 0E 11 81 55 12 75 1D 12 75
  {
    .param [3]/*080002B2*/ 
    .custom /*0C0001E0:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080002B3*/ 
    .custom /*0C0001E1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x252c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineProperty

  .method /*060001E0*/ public hidebysig instance class System.Reflection.Emit.PropertyBuilder/*0200000C*/ 
          DefineProperty(string name,
                         valuetype [System.Runtime/*23000001*/]System.Reflection.PropertyAttributes/*01000055*/ attributes,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] returnTypeRequiredCustomModifiers,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] returnTypeOptionalCustomModifiers,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] parameterTypeRequiredCustomModifiers,
                         class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] parameterTypeOptionalCustomModifiers) cil managed
  // SIG: 20 08 12 30 0E 11 81 55 12 75 1D 12 75 1D 12 75 1D 12 75 1D 1D 12 75 1D 1D 12 75
  {
    .param [3]/*080002B6*/ 
    .custom /*0C0001E2:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080002B7*/ 
    .custom /*0C0001E3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [5]/*080002B8*/ 
    .custom /*0C0001E4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [6]/*080002B9*/ 
    .custom /*0C0001E5:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [7]/*080002BA*/ 
    .custom /*0C0001E6:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [8]/*080002BB*/ 
    .custom /*0C0001E7:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x252f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineProperty

  .method /*060001E1*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.PropertyBuilder/*0200000C*/ 
          DefinePropertyCore(string name,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.PropertyAttributes/*01000055*/ attributes,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callingConvention,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] returnTypeRequiredCustomModifiers,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] returnTypeOptionalCustomModifiers,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] parameterTypes,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] parameterTypeRequiredCustomModifiers,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/[][] parameterTypeOptionalCustomModifiers) cil managed
  // SIG: 20 09 12 30 0E 11 81 55 11 80 B5 12 75 1D 12 75 1D 12 75 1D 12 75 1D 1D 12 75 1D 1D 12 75
  {
    .param [4]/*080002BF*/ 
    .custom /*0C0001E8:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080002C0*/ 
    .custom /*0C0001E9:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [6]/*080002C1*/ 
    .custom /*0C0001EA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [7]/*080002C2*/ 
    .custom /*0C0001EB:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [8]/*080002C3*/ 
    .custom /*0C0001EC:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [9]/*080002C4*/ 
    .custom /*0C0001ED:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method TypeBuilder::DefinePropertyCore

  .method /*060001E2*/ public hidebysig instance class System.Reflection.Emit.ConstructorBuilder/*02000004*/ 
          DefineTypeInitializer() cil managed
  // SIG: 20 00 12 10
  {
    // Method begins at RVA 0x2532
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineTypeInitializer

  .method /*060001E3*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.ConstructorBuilder/*02000004*/ 
          DefineTypeInitializerCore() cil managed
  // SIG: 20 00 12 10
  {
    // Method begins at RVA 0x0
  } // end of method TypeBuilder::DefineTypeInitializerCore

  .method /*060001E4*/ public hidebysig instance class System.Reflection.Emit.FieldBuilder/*02000007*/ 
          DefineUninitializedData(string name,
                                  int32 size,
                                  valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000044*/ attributes) cil managed
  // SIG: 20 03 12 1C 0E 08 11 81 11
  {
    // Method begins at RVA 0x2535
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::DefineUninitializedData

  .method /*060001E5*/ family hidebysig newslot abstract virtual 
          instance class System.Reflection.Emit.FieldBuilder/*02000007*/ 
          DefineUninitializedDataCore(string name,
                                      int32 size,
                                      valuetype [System.Runtime/*23000001*/]System.Reflection.FieldAttributes/*01000044*/ attributes) cil managed
  // SIG: 20 03 12 1C 0E 08 11 81 11
  {
    // Method begins at RVA 0x0
  } // end of method TypeBuilder::DefineUninitializedDataCore

  .method /*060001E6*/ family hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.TypeAttributes/*0100003B*/ 
          GetAttributeFlagsImpl() cil managed
  // SIG: 20 00 11 80 ED
  {
    // Method begins at RVA 0x2538
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetAttributeFlagsImpl

  .method /*060001E7*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ 
          GetConstructor(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 'type',
                         class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ constructor) cil managed
  // SIG: 00 02 12 80 9D 12 75 12 80 9D
  {
    // Method begins at RVA 0x253b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetConstructor

  .method /*060001E8*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ 
          GetConstructorImpl(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr,
                             class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000035*/ binder,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callConvention,
                             class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] types,
                             valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003C*/[] modifiers) cil managed
  // SIG: 20 05 12 80 9D 11 80 D1 12 80 D5 11 80 B5 1D 12 75 1D 11 80 F1
  {
    .custom /*0C000176:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000177:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 07 00 00 00 00 00 ) 
    .param [4]/*080002D0*/ 
    .custom /*0C0001EE:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x253e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetConstructorImpl

  .method /*060001E9*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/[] 
          GetConstructors(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 9D 11 80 D1
  {
    .custom /*0C000179:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 07 00 00 00 00 00 ) 
    // Method begins at RVA 0x2541
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetConstructors

  .method /*060001EA*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(bool inherit) cil managed
  // SIG: 20 01 1D 1C 02
  {
    // Method begins at RVA 0x2544
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetCustomAttributes

  .method /*060001EB*/ public hidebysig virtual 
          instance object[]  GetCustomAttributes(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ attributeType,
                                                 bool inherit) cil managed
  // SIG: 20 02 1D 1C 12 75 02
  {
    // Method begins at RVA 0x2547
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetCustomAttributes

  .method /*060001EC*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          GetElementType() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x254a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetElementType

  .method /*060001ED*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100003D*/ 
          GetEvent(string name,
                   valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 02 12 80 F5 0E 11 80 D1
  {
    .custom /*0C00017B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 18 00 00 00 00 ) 
    .param [0]/*080002D6*/ 
    .custom /*0C0001EF:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x254d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetEvent

  .method /*060001EE*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100003D*/[] 
          GetEvents() cil managed
  // SIG: 20 00 1D 12 80 F5
  {
    .custom /*0C00017C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 08 00 00 00 00 ) 
    // Method begins at RVA 0x2550
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetEvents

  .method /*060001EF*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.EventInfo/*0100003D*/[] 
          GetEvents(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 F5 11 80 D1
  {
    .custom /*0C00017D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 18 00 00 00 00 ) 
    // Method begins at RVA 0x2553
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetEvents

  .method /*060001F0*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003E*/ 
          GetField(string name,
                   valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 02 12 80 F9 0E 11 80 D1
  {
    .custom /*0C00017E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 60 00 00 00 00 00 )                         // ..`.....
    .param [0]/*080002DA*/ 
    .custom /*0C0001F0:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2556
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetField

  .method /*060001F1*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003E*/ 
          GetField(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 'type',
                   class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003E*/ 'field') cil managed
  // SIG: 00 02 12 80 F9 12 75 12 80 F9
  {
    // Method begins at RVA 0x2559
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetField

  .method /*060001F2*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.FieldInfo/*0100003E*/[] 
          GetFields(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 80 F9 11 80 D1
  {
    .custom /*0C00017F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 60 00 00 00 00 00 )                         // ..`.....
    // Method begins at RVA 0x255c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetFields

  .method /*060001F3*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] 
          GetGenericArguments() cil managed
  // SIG: 20 00 1D 12 75
  {
    // Method begins at RVA 0x255f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetGenericArguments

  .method /*060001F4*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          GetGenericTypeDefinition() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x2562
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetGenericTypeDefinition

  .method /*060001F5*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          GetInterface(string name,
                       bool ignoreCase) cil managed
  // SIG: 20 02 12 75 0E 02
  {
    .custom /*0C000180:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 20 00 00 00 00 )                         // ... ....
    .param [0]/*080002E0*/ 
    .custom /*0C0001F1:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001F2:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 20 00 00 00 00 )                         // ... ....
    // Method begins at RVA 0x2565
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetInterface

  .method /*060001F6*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Reflection.InterfaceMapping/*0100003F*/ 
          GetInterfaceMap(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ interfaceType) cil managed
  // SIG: 20 01 11 80 FD 12 75
  {
    .param [1]/*080002E3*/ 
    .custom /*0C0001F3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 18 00 00 00 00 00 ) 
    // Method begins at RVA 0x2568
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetInterfaceMap

  .method /*060001F7*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] 
          GetInterfaces() cil managed
  // SIG: 20 00 1D 12 75
  {
    .custom /*0C000181:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 20 00 00 00 00 )                         // ... ....
    // Method begins at RVA 0x256b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetInterfaces

  .method /*060001F8*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000040*/[] 
          GetMember(string name,
                    valuetype [System.Runtime/*23000001*/]System.Reflection.MemberTypes/*01000041*/ 'type',
                    valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 03 1D 12 81 01 0E 11 81 05 11 80 D1
  {
    .custom /*0C000182:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF 1F 00 00 00 00 ) 
    // Method begins at RVA 0x256e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetMember

  .method /*060001F9*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MemberInfo/*01000040*/[] 
          GetMembers(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 81 01 11 80 D1
  {
    .custom /*0C000184:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF 1F 00 00 00 00 ) 
    // Method begins at RVA 0x2571
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetMembers

  .method /*060001FA*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetMethod(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 'type',
                    class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 'method') cil managed
  // SIG: 00 02 12 59 12 75 12 59
  {
    // Method begins at RVA 0x2574
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetMethod

  .method /*060001FB*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/ 
          GetMethodImpl(string name,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr,
                        class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000035*/ binder,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.CallingConventions/*0100002D*/ callConvention,
                        class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] types,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003C*/[] modifiers) cil managed
  // SIG: 20 06 12 59 0E 11 80 D1 12 80 D5 11 80 B5 1D 12 75 1D 11 80 F1
  {
    .custom /*0C000185:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000186:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 18 00 00 00 00 00 ) 
    .param [1]/*080002EA*/ 
    .custom /*0C0001F4:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [5]/*080002EE*/ 
    .custom /*0C0001F5:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2577
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetMethodImpl

  .method /*060001FC*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*01000016*/[] 
          GetMethods(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 59 11 80 D1
  {
    .custom /*0C000187:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 18 00 00 00 00 00 ) 
    // Method begins at RVA 0x257a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetMethods

  .method /*060001FD*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          GetNestedType(string name,
                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 02 12 75 0E 11 80 D1
  {
    .custom /*0C000188:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 80 01 00 00 00 00 ) 
    .param [0]/*080002F1*/ 
    .custom /*0C0001F6:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x257d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetNestedType

  .method /*060001FE*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] 
          GetNestedTypes(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 75 11 80 D1
  {
    .custom /*0C000189:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 80 01 00 00 00 00 ) 
    // Method begins at RVA 0x2580
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetNestedTypes

  .method /*060001FF*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000042*/[] 
          GetProperties(valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr) cil managed
  // SIG: 20 01 1D 12 81 09 11 80 D1
  {
    .custom /*0C00018A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 06 00 00 00 00 ) 
    // Method begins at RVA 0x2583
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetProperties

  .method /*06000200*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*01000042*/ 
          GetPropertyImpl(string name,
                          valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ bindingAttr,
                          class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000035*/ binder,
                          class [System.Runtime/*23000001*/]System.Type/*0100001D*/ returnType,
                          class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] types,
                          valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003C*/[] modifiers) cil managed
  // SIG: 20 06 12 81 09 0E 11 80 D1 12 80 D5 12 75 1D 12 75 1D 11 80 F1
  {
    .custom /*0C00018B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00018C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 00 06 00 00 00 00 ) 
    .param [1]/*080002F6*/ 
    .custom /*0C0001F7:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [5]/*080002FA*/ 
    .custom /*0C0001F8:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2586
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::GetPropertyImpl

  .method /*06000201*/ family hidebysig virtual 
          instance bool  HasElementTypeImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2589
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::HasElementTypeImpl

  .method /*06000202*/ public hidebysig virtual 
          instance object  InvokeMember(string name,
                                        valuetype [System.Runtime/*23000001*/]System.Reflection.BindingFlags/*01000034*/ invokeAttr,
                                        class [System.Runtime/*23000001*/]System.Reflection.Binder/*01000035*/ binder,
                                        object target,
                                        object[] args,
                                        valuetype [System.Runtime/*23000001*/]System.Reflection.ParameterModifier/*0100003C*/[] modifiers,
                                        class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000025*/ culture,
                                        string[] namedParameters) cil managed
  // SIG: 20 08 1C 0E 11 80 D1 12 80 D5 1C 1D 1C 1D 11 80 F1 12 80 95 1D 0E
  {
    .custom /*0C00018D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00018E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 7F 06 00 00 00 00 ) 
    .param [1]/*080002FC*/ 
    .custom /*0C0001F9:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [8]/*08000303*/ 
    .custom /*0C0001FA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x258c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::InvokeMember

  .method /*06000203*/ family hidebysig virtual 
          instance bool  IsArrayImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x258f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::IsArrayImpl

  .method /*06000204*/ public hidebysig virtual 
          instance bool  IsAssignableFrom(class [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000036*/ typeInfo) cil managed
  // SIG: 20 01 02 12 80 D9
  {
    .custom /*0C00018F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000304*/ 
    .custom /*0C0001FB:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000043*/::.ctor(bool) /* 0A00001B */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2592
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::IsAssignableFrom

  .method /*06000205*/ public hidebysig virtual 
          instance bool  IsAssignableFrom(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ c) cil managed
  // SIG: 20 01 02 12 75
  {
    .custom /*0C000190:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000305*/ 
    .custom /*0C0001FC:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000043*/::.ctor(bool) /* 0A00001B */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2595
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::IsAssignableFrom

  .method /*06000206*/ family hidebysig virtual 
          instance bool  IsByRefImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2598
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::IsByRefImpl

  .method /*06000207*/ family hidebysig virtual 
          instance bool  IsCOMObjectImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x259b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::IsCOMObjectImpl

  .method /*06000208*/ public hidebysig instance bool 
          IsCreated() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x259e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::IsCreated

  .method /*06000209*/ family hidebysig newslot abstract virtual 
          instance bool  IsCreatedCore() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method TypeBuilder::IsCreatedCore

  .method /*0600020A*/ public hidebysig virtual 
          instance bool  IsDefined(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ attributeType,
                                   bool inherit) cil managed
  // SIG: 20 02 02 12 75 02
  {
    // Method begins at RVA 0x25a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::IsDefined

  .method /*0600020B*/ family hidebysig virtual 
          instance bool  IsPointerImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x25a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::IsPointerImpl

  .method /*0600020C*/ family hidebysig virtual 
          instance bool  IsPrimitiveImpl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x25a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::IsPrimitiveImpl

  .method /*0600020D*/ public hidebysig virtual 
          instance bool  IsSubclassOf(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ c) cil managed
  // SIG: 20 01 02 12 75
  {
    // Method begins at RVA 0x25aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::IsSubclassOf

  .method /*0600020E*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          MakeArrayType() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C000196:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000018*/::.ctor(string) /* 0A000014 */ = ( 01 00 43 54 68 65 20 63 6F 64 65 20 66 6F 72 20   // ..CThe code for 
                                                                                                                                                                                       61 6E 20 61 72 72 61 79 20 6F 66 20 74 68 65 20   // an array of the 
                                                                                                                                                                                       73 70 65 63 69 66 69 65 64 20 74 79 70 65 20 6D   // specified type m
                                                                                                                                                                                       69 67 68 74 20 6E 6F 74 20 62 65 20 61 76 61 69   // ight not be avai
                                                                                                                                                                                       6C 61 62 6C 65 2E 00 00 )                         // lable...
    // Method begins at RVA 0x25ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::MakeArrayType

  .method /*0600020F*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          MakeArrayType(int32 rank) cil managed
  // SIG: 20 01 12 75 08
  {
    .custom /*0C000198:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000018*/::.ctor(string) /* 0A000014 */ = ( 01 00 43 54 68 65 20 63 6F 64 65 20 66 6F 72 20   // ..CThe code for 
                                                                                                                                                                                       61 6E 20 61 72 72 61 79 20 6F 66 20 74 68 65 20   // an array of the 
                                                                                                                                                                                       73 70 65 63 69 66 69 65 64 20 74 79 70 65 20 6D   // specified type m
                                                                                                                                                                                       69 67 68 74 20 6E 6F 74 20 62 65 20 61 76 61 69   // ight not be avai
                                                                                                                                                                                       6C 61 62 6C 65 2E 00 00 )                         // lable...
    // Method begins at RVA 0x25b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::MakeArrayType

  .method /*06000210*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          MakeByRefType() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x25b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::MakeByRefType

  .method /*06000211*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          MakeGenericType(class [System.Runtime/*23000001*/]System.Type/*0100001D*/[] typeArguments) cil managed
  // SIG: 20 01 12 75 1D 12 75
  {
    .custom /*0C000199:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*01000018*/::.ctor(string) /* 0A000014 */ = ( 01 00 49 54 68 65 20 6E 61 74 69 76 65 20 63 6F   // ..IThe native co
                                                                                                                                                                                       64 65 20 66 6F 72 20 74 68 69 73 20 69 6E 73 74   // de for this inst
                                                                                                                                                                                       61 6E 74 69 61 74 69 6F 6E 20 6D 69 67 68 74 20   // antiation might 
                                                                                                                                                                                       6E 6F 74 20 62 65 20 61 76 61 69 6C 61 62 6C 65   // not be available
                                                                                                                                                                                       20 61 74 20 72 75 6E 74 69 6D 65 2E 00 00 )       //  at runtime...
    .custom /*0C00019A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000020*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 C5 49 66 20 73 6F 6D 65 20 6F 66 20 74   // ....If some of t
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
    .param [1]/*0800030A*/ 
    .custom /*0C0001FD:0A00001C*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000048*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x25b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::MakeGenericType

  .method /*06000212*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ 
          MakePointerType() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x25b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::MakePointerType

  .method /*06000213*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                             uint8[] binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 1D 05
  {
    // Method begins at RVA 0x25bc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TypeBuilder::SetCustomAttribute

  .method /*06000214*/ public hidebysig instance void 
          SetCustomAttribute(class [System.Reflection.Emit.ILGeneration/*23000002*/]System.Reflection.Emit.CustomAttributeBuilder/*0100001B*/ customBuilder) cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x25be
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TypeBuilder::SetCustomAttribute

  .method /*06000215*/ family hidebysig newslot abstract virtual 
          instance void  SetCustomAttributeCore(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*01000027*/ con,
                                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000028*/<uint8> binaryAttribute) cil managed
  // SIG: 20 02 01 12 80 9D 15 11 80 A1 01 05
  {
    .custom /*0C00019E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800030E*/ 
    .custom /*0C0001FE:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method TypeBuilder::SetCustomAttributeCore

  .method /*06000216*/ public hidebysig instance void 
          SetParent(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ parent) cil managed
  // SIG: 20 01 01 12 75
  {
    .custom /*0C00019F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000310*/ 
    .custom /*0C0001FF:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x25c0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TypeBuilder::SetParent

  .method /*06000217*/ family hidebysig newslot abstract virtual 
          instance void  SetParentCore(class [System.Runtime/*23000001*/]System.Type/*0100001D*/ parent) cil managed
  // SIG: 20 01 01 12 75
  {
    .custom /*0C0001A1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000311*/ 
    .custom /*0C000200:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100003A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000039*/) /* 0A00001A */ = ( 01 00 FF FF FF FF 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method TypeBuilder::SetParentCore

  .method /*06000218*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x25c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBuilder::ToString

  .property /*1700006A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/
          Assembly()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*01000015*/ System.Reflection.Emit.TypeBuilder/*0200000D*/::get_Assembly() /* 06000198 */
  } // end of property TypeBuilder::Assembly
  .property /*1700006B*/ instance callconv(8) string
          AssemblyQualifiedName()
  {
    .custom /*0C000083:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.TypeBuilder/*0200000D*/::get_AssemblyQualifiedName() /* 06000199 */
  } // end of property TypeBuilder::AssemblyQualifiedName
  .property /*1700006C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          BaseType()
  {
    .custom /*0C000085:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.TypeBuilder/*0200000D*/::get_BaseType() /* 0600019A */
  } // end of property TypeBuilder::BaseType
  .property /*1700006D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodBase/*01000046*/
          DeclaringMethod()
  {
    .custom /*0C000087:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodBase/*01000046*/ System.Reflection.Emit.TypeBuilder/*0200000D*/::get_DeclaringMethod() /* 0600019B */
  } // end of property TypeBuilder::DeclaringMethod
  .property /*1700006E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          DeclaringType()
  {
    .custom /*0C000089:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.TypeBuilder/*0200000D*/::get_DeclaringType() /* 0600019C */
  } // end of property TypeBuilder::DeclaringType
  .property /*1700006F*/ instance callconv(8) string
          FullName()
  {
    .custom /*0C00008B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.TypeBuilder/*0200000D*/::get_FullName() /* 0600019D */
  } // end of property TypeBuilder::FullName
  .property /*17000070*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Reflection.GenericParameterAttributes/*01000047*/
          GenericParameterAttributes()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Reflection.GenericParameterAttributes/*01000047*/ System.Reflection.Emit.TypeBuilder/*0200000D*/::get_GenericParameterAttributes() /* 0600019E */
  } // end of property TypeBuilder::GenericParameterAttributes
  .property /*17000071*/ instance callconv(8) int32
          GenericParameterPosition()
  {
    .get instance int32 System.Reflection.Emit.TypeBuilder/*0200000D*/::get_GenericParameterPosition() /* 0600019F */
  } // end of property TypeBuilder::GenericParameterPosition
  .property /*17000072*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Guid/*01000037*/
          GUID()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Guid/*01000037*/ System.Reflection.Emit.TypeBuilder/*0200000D*/::get_GUID() /* 060001A0 */
  } // end of property TypeBuilder::GUID
  .property /*17000073*/ instance callconv(8) bool
          IsByRefLike()
  {
    .get instance bool System.Reflection.Emit.TypeBuilder/*0200000D*/::get_IsByRefLike() /* 060001A1 */
  } // end of property TypeBuilder::IsByRefLike
  .property /*17000074*/ instance callconv(8) bool
          IsConstructedGenericType()
  {
    .get instance bool System.Reflection.Emit.TypeBuilder/*0200000D*/::get_IsConstructedGenericType() /* 060001A2 */
  } // end of property TypeBuilder::IsConstructedGenericType
  .property /*17000075*/ instance callconv(8) bool
          IsGenericParameter()
  {
    .get instance bool System.Reflection.Emit.TypeBuilder/*0200000D*/::get_IsGenericParameter() /* 060001A3 */
  } // end of property TypeBuilder::IsGenericParameter
  .property /*17000076*/ instance callconv(8) bool
          IsGenericType()
  {
    .get instance bool System.Reflection.Emit.TypeBuilder/*0200000D*/::get_IsGenericType() /* 060001A4 */
  } // end of property TypeBuilder::IsGenericType
  .property /*17000077*/ instance callconv(8) bool
          IsGenericTypeDefinition()
  {
    .get instance bool System.Reflection.Emit.TypeBuilder/*0200000D*/::get_IsGenericTypeDefinition() /* 060001A5 */
  } // end of property TypeBuilder::IsGenericTypeDefinition
  .property /*17000078*/ instance callconv(8) bool
          IsSecurityCritical()
  {
    .get instance bool System.Reflection.Emit.TypeBuilder/*0200000D*/::get_IsSecurityCritical() /* 060001A6 */
  } // end of property TypeBuilder::IsSecurityCritical
  .property /*17000079*/ instance callconv(8) bool
          IsSecuritySafeCritical()
  {
    .get instance bool System.Reflection.Emit.TypeBuilder/*0200000D*/::get_IsSecuritySafeCritical() /* 060001A7 */
  } // end of property TypeBuilder::IsSecuritySafeCritical
  .property /*1700007A*/ instance callconv(8) bool
          IsSecurityTransparent()
  {
    .get instance bool System.Reflection.Emit.TypeBuilder/*0200000D*/::get_IsSecurityTransparent() /* 060001A8 */
  } // end of property TypeBuilder::IsSecurityTransparent
  .property /*1700007B*/ instance callconv(8) bool
          IsSZArray()
  {
    .get instance bool System.Reflection.Emit.TypeBuilder/*0200000D*/::get_IsSZArray() /* 060001A9 */
  } // end of property TypeBuilder::IsSZArray
  .property /*1700007C*/ instance callconv(8) bool
          IsTypeDefinition()
  {
    .get instance bool System.Reflection.Emit.TypeBuilder/*0200000D*/::get_IsTypeDefinition() /* 060001AA */
  } // end of property TypeBuilder::IsTypeDefinition
  .property /*1700007D*/ instance callconv(8) int32
          MetadataToken()
  {
    .get instance int32 System.Reflection.Emit.TypeBuilder/*0200000D*/::get_MetadataToken() /* 060001AB */
  } // end of property TypeBuilder::MetadataToken
  .property /*1700007E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/
          Module()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.Module/*01000017*/ System.Reflection.Emit.TypeBuilder/*0200000D*/::get_Module() /* 060001AC */
  } // end of property TypeBuilder::Module
  .property /*1700007F*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Reflection.Emit.TypeBuilder/*0200000D*/::get_Name() /* 060001AD */
  } // end of property TypeBuilder::Name
  .property /*17000080*/ instance callconv(8) string
          Namespace()
  {
    .custom /*0C00009E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Reflection.Emit.TypeBuilder/*0200000D*/::get_Namespace() /* 060001AE */
  } // end of property TypeBuilder::Namespace
  .property /*17000081*/ instance callconv(8) valuetype [System.Reflection.Primitives/*23000005*/]System.Reflection.Emit.PackingSize/*0100004F*/
          PackingSize()
  {
    .get instance valuetype [System.Reflection.Primitives/*23000005*/]System.Reflection.Emit.PackingSize/*0100004F*/ System.Reflection.Emit.TypeBuilder/*0200000D*/::get_PackingSize() /* 060001AF */
  } // end of property TypeBuilder::PackingSize
  .property /*17000082*/ instance callconv(8) valuetype [System.Reflection.Primitives/*23000005*/]System.Reflection.Emit.PackingSize/*0100004F*/
          PackingSizeCore()
  {
    .get instance valuetype [System.Reflection.Primitives/*23000005*/]System.Reflection.Emit.PackingSize/*0100004F*/ System.Reflection.Emit.TypeBuilder/*0200000D*/::get_PackingSizeCore() /* 060001B0 */
  } // end of property TypeBuilder::PackingSizeCore
  .property /*17000083*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          ReflectedType()
  {
    .custom /*0C0000A1:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.TypeBuilder/*0200000D*/::get_ReflectedType() /* 060001B1 */
  } // end of property TypeBuilder::ReflectedType
  .property /*17000084*/ instance callconv(8) int32
          Size()
  {
    .get instance int32 System.Reflection.Emit.TypeBuilder/*0200000D*/::get_Size() /* 060001B2 */
  } // end of property TypeBuilder::Size
  .property /*17000085*/ instance callconv(8) int32
          SizeCore()
  {
    .get instance int32 System.Reflection.Emit.TypeBuilder/*0200000D*/::get_SizeCore() /* 060001B3 */
  } // end of property TypeBuilder::SizeCore
  .property /*17000086*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000038*/
          TypeHandle()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.RuntimeTypeHandle/*01000038*/ System.Reflection.Emit.TypeBuilder/*0200000D*/::get_TypeHandle() /* 060001B4 */
  } // end of property TypeBuilder::TypeHandle
  .property /*17000087*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*0100001D*/
          UnderlyingSystemType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*0100001D*/ System.Reflection.Emit.TypeBuilder/*0200000D*/::get_UnderlyingSystemType() /* 060001B5 */
  } // end of property TypeBuilder::UnderlyingSystemType
} // end of class System.Reflection.Emit.TypeBuilder


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
