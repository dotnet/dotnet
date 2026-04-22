


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
// Time-date stamp:            0xaf7f5a83
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00002400
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x000042a6
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
// Checksum:                       0x0000e885
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00004254 [0x0000004f] address [size] of Import Directory:          
// 0x00006000 [0x000004c4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00002e00 [0x00002938] address [size] of Security Directory:        
// 0x00008000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00004238 [0x0000001c] address [size] of Debug Directory:           
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
//              0x000022ac Virtual Size
//              0x00002000 Virtual Address
//              0x00002400 Size of Raw Data
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
//              0x00002600 Pointer to Raw Data
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
//              0x00002c00 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x000002a8
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00004296 Import Name Table
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
// 0x00002088 [0x00002130] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x000041b8 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000da0 Size
//              '#~' Name
//    Stream 2:
//              0x00000e0c Offset
//              0x00000ea4 Size
//              '#Strings' Name
//    Stream 3:
//              0x00001cb0 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00001cb4 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001cc4 Offset
//              0x0000046c Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000008909a01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 22328
// PE header size       : 512 (496 used)    ( 2.29%)
// PE additional info   : 11971             (53.61%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.32%)
// CLR meta-data size  : 8496               (38.05%)
// CLR additional info : 128                ( 0.57%)
// CLR method headers  : 17                 ( 0.08%)
// Managed code         : 39                ( 0.17%)
// Data                 : 2048              ( 9.17%)
// Unaccounted          : -955              (-4.28%)

// Num.of PE sections   : 3
//   .text    - 9216
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 8496
//   Module        -    1 (10 bytes)
//   TypeDef       -    8 (112 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   27 (162 bytes)
//   MethodDef     -   17 (238 bytes)     0 abstract, 0 native, 17 bodies
//   FieldDef      -  303 (1818 bytes)    226 constant
//   MemberRef     -   23 (138 bytes)
//   ParamDef      -    7 (42 bytes)
//   Constant      -   71 (426 bytes)
//   CustomAttribute-   27 (162 bytes)
//   InterfaceImpl -    1 (4 bytes)
//   PropertyMap   -    1 (4 bytes)
//   Property      -    9 (54 bytes)
//   MethodSemantic-    9 (54 bytes)
//   TypeSpec      -    1 (2 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    1 (20 bytes)
//   ExportedType  -    9 (126 bytes)
//   Strings       -  3746 bytes
//   Blobs         -  1132 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   204 bytes

// CLR additional info : 128

// CLR method headers : 17
//   Num.of method bodies  - 17
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 17

// Managed code : 39
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class FlowControl                    (public) (auto) (ansi) (sealed) 
// Class OpCode                         (public) (sequential) (ansi) (sealed) 
// Class OpCodes                        (public) (auto) (ansi) 
// Class OpCodeType                     (public) (auto) (ansi) (sealed) 
// Class OperandType                    (public) (auto) (ansi) (sealed) 
// Class PackingSize                    (public) (auto) (ansi) (sealed) 
// Class StackBehaviour                 (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 9:0:0:0
}
.assembly /*20000001*/ noplatform System.Reflection.Primitives
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000005:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000001*/::.ctor(int32) /* 0A000001 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000006:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000002*/::.ctor() /* 0A000002 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000007:0A000003*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*//DebuggingModes/*01000004*/) /* 0A000003 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000008:0A000004*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 39 2E 30 01 00 54 0E 14   // ersion=v9.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 39 2E 30 )          // Name..NET 9.0
  .custom /*0C000009:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 1C 53 79 73 74 65 6D 2E 52 65 66 6C 65 63   // ...System.Reflec
                                                                                                                                                                        74 69 6F 6E 2E 50 72 69 6D 69 74 69 76 65 73 00   // tion.Primitives.
                                                                                                                                                                        00 ) 
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000F:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000010:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 1C 53 79 73 74 65 6D 2E 52 65 66 6C 65 63   // ...System.Reflec
                                                                                                                                                                       74 69 6F 6E 2E 50 72 69 6D 69 74 69 76 65 73 00   // tion.Primitives.
                                                                                                                                                                       00 ) 
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 39 2E 30 2E 32 34 2E 35 32 38 30 39 00   // ...9.0.24.52809.
                                                                                                                                                                       00 ) 
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 39 2E 30 2E 30 2B 39 64 35 61 36 61 39   // ...9.0.0+9d5a6a9
                                                                                                                                                                                61 61 34 36 33 64 36 64 31 30 62 30 62 30 62 61   // aa463d6d10b0b0ba
                                                                                                                                                                                36 64 35 39 38 32 63 63 38 32 66 33 36 33 64 63   // 6d5982cc82f363dc
                                                                                                                                                                                33 00 00 )                                        // 3..
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000014:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 1C 53 79 73 74 65 6D 2E 52 65 66 6C 65 63   // ...System.Reflec
                                                                                                                                                                 74 69 6F 6E 2E 50 72 69 6D 69 74 69 76 65 73 00   // tion.Primitives.
                                                                                                                                                                 00 ) 
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
  .ver 9:0:0:0
}
.class extern /*27000001*/ forwarder System.Reflection.CallingConventions
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000002*/ forwarder System.Reflection.EventAttributes
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000003*/ forwarder System.Reflection.FieldAttributes
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000004*/ forwarder System.Reflection.GenericParameterAttributes
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000005*/ forwarder System.Reflection.MethodAttributes
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000006*/ forwarder System.Reflection.MethodImplAttributes
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000007*/ forwarder System.Reflection.ParameterAttributes
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000008*/ forwarder System.Reflection.PropertyAttributes
{
  .assembly extern System.Runtime /*23000001*/ 
}
.class extern /*27000009*/ forwarder System.Reflection.TypeAttributes
{
  .assembly extern System.Runtime /*23000001*/ 
}
.module System.Reflection.Primitives.dll
// MVID: {49ced5e0-a5bd-4b4e-8cd8-cdb8bd849e55}
.custom /*0C000003:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000011*/::.ctor(int32) /* 0A000010 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000004:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000012*/::.ctor(bool) /* 0A000011 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi sealed System.Reflection.Emit.FlowControl
       extends [System.Runtime/*23000001*/]System.Enum/*01000013*/
{
  .field /*04000001*/ public specialname rtspecialname int32 value__
  .field /*04000002*/ public static literal valuetype System.Reflection.Emit.FlowControl/*02000002*/ Branch = int32(0x00000000)
  .field /*04000003*/ public static literal valuetype System.Reflection.Emit.FlowControl/*02000002*/ Break = int32(0x00000001)
  .field /*04000004*/ public static literal valuetype System.Reflection.Emit.FlowControl/*02000002*/ Call = int32(0x00000002)
  .field /*04000005*/ public static literal valuetype System.Reflection.Emit.FlowControl/*02000002*/ Cond_Branch = int32(0x00000003)
  .field /*04000006*/ public static literal valuetype System.Reflection.Emit.FlowControl/*02000002*/ Meta = int32(0x00000004)
  .field /*04000007*/ public static literal valuetype System.Reflection.Emit.FlowControl/*02000002*/ Next = int32(0x00000005)
  .field /*04000008*/ public static literal valuetype System.Reflection.Emit.FlowControl/*02000002*/ Phi = int32(0x00000006)
  .custom /*0C000019:0A000012*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000014*/::.ctor(string) /* 0A000012 */ = ( 01 00 39 46 6C 6F 77 43 6F 6E 74 72 6F 6C 2E 50   // ..9FlowControl.P
                                                                                                                                                 68 69 20 68 61 73 20 62 65 65 6E 20 64 65 70 72   // hi has been depr
                                                                                                                                                 65 63 61 74 65 64 20 61 6E 64 20 69 73 20 6E 6F   // ecated and is no
                                                                                                                                                 74 20 73 75 70 70 6F 72 74 65 64 2E 00 00 )       // t supported...
  .field /*04000009*/ public static literal valuetype System.Reflection.Emit.FlowControl/*02000002*/ Return = int32(0x00000007)
  .field /*0400000A*/ public static literal valuetype System.Reflection.Emit.FlowControl/*02000002*/ Throw = int32(0x00000008)
} // end of class System.Reflection.Emit.FlowControl

.class /*02000003*/ public sequential ansi sealed beforefieldinit System.Reflection.Emit.OpCode
       extends [System.Runtime/*23000001*/]System.ValueType/*01000018*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000019*/<valuetype System.Reflection.Emit.OpCode/*02000003*/>/*1B000001*/
{
  .custom /*0C000016:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000015*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000017:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000018:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.IEquatable`1/*01000019*/<valuetype System.Reflection.Emit.OpCode/*02000003*/>/*1B000001*/
  .custom /*0C000002:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000016*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .field /*0400000B*/ private initonly int32 _dummyPrimitive
  .method /*06000001*/ public hidebysig specialname 
          instance int32  get_EvaluationStackDelta() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OpCode::get_EvaluationStackDelta

  .method /*06000002*/ public hidebysig specialname 
          instance valuetype System.Reflection.Emit.FlowControl/*02000002*/ 
          get_FlowControl() cil managed
  // SIG: 20 00 11 08
  {
    // Method begins at RVA 0x2053
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OpCode::get_FlowControl

  .method /*06000003*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2056
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OpCode::get_Name

  .method /*06000004*/ public hidebysig specialname 
          instance valuetype System.Reflection.Emit.OpCodeType/*02000005*/ 
          get_OpCodeType() cil managed
  // SIG: 20 00 11 14
  {
    // Method begins at RVA 0x2059
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OpCode::get_OpCodeType

  .method /*06000005*/ public hidebysig specialname 
          instance valuetype System.Reflection.Emit.OperandType/*02000006*/ 
          get_OperandType() cil managed
  // SIG: 20 00 11 18
  {
    // Method begins at RVA 0x205c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OpCode::get_OperandType

  .method /*06000006*/ public hidebysig specialname 
          instance int32  get_Size() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x205f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OpCode::get_Size

  .method /*06000007*/ public hidebysig specialname 
          instance valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ 
          get_StackBehaviourPop() cil managed
  // SIG: 20 00 11 20
  {
    // Method begins at RVA 0x2062
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OpCode::get_StackBehaviourPop

  .method /*06000008*/ public hidebysig specialname 
          instance valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ 
          get_StackBehaviourPush() cil managed
  // SIG: 20 00 11 20
  {
    // Method begins at RVA 0x2065
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OpCode::get_StackBehaviourPush

  .method /*06000009*/ public hidebysig specialname 
          instance int16  get_Value() cil managed
  // SIG: 20 00 06
  {
    // Method begins at RVA 0x2068
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OpCode::get_Value

  .method /*0600000A*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .param [1]/*08000001*/ 
    .custom /*0C000001:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001A*/::.ctor(bool) /* 0A000016 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x206b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OpCode::Equals

  .method /*0600000B*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Reflection.Emit.OpCode/*02000003*/ obj) cil managed
  // SIG: 20 01 02 11 0C
  {
    // Method begins at RVA 0x206e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OpCode::Equals

  .method /*0600000C*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2071
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OpCode::GetHashCode

  .method /*0600000D*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Reflection.Emit.OpCode/*02000003*/ a,
                            valuetype System.Reflection.Emit.OpCode/*02000003*/ b) cil managed
  // SIG: 00 02 02 11 0C 11 0C
  {
    // Method begins at RVA 0x2074
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OpCode::op_Equality

  .method /*0600000E*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Reflection.Emit.OpCode/*02000003*/ a,
                              valuetype System.Reflection.Emit.OpCode/*02000003*/ b) cil managed
  // SIG: 00 02 02 11 0C 11 0C
  {
    // Method begins at RVA 0x2077
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OpCode::op_Inequality

  .method /*0600000F*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x207a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OpCode::ToString

  .property /*17000001*/ instance callconv(8) int32
          EvaluationStackDelta()
  {
    .get instance int32 System.Reflection.Emit.OpCode/*02000003*/::get_EvaluationStackDelta() /* 06000001 */
  } // end of property OpCode::EvaluationStackDelta
  .property /*17000002*/ instance callconv(8) valuetype System.Reflection.Emit.FlowControl/*02000002*/
          FlowControl()
  {
    .get instance valuetype System.Reflection.Emit.FlowControl/*02000002*/ System.Reflection.Emit.OpCode/*02000003*/::get_FlowControl() /* 06000002 */
  } // end of property OpCode::FlowControl
  .property /*17000003*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Reflection.Emit.OpCode/*02000003*/::get_Name() /* 06000003 */
  } // end of property OpCode::Name
  .property /*17000004*/ instance callconv(8) valuetype System.Reflection.Emit.OpCodeType/*02000005*/
          OpCodeType()
  {
    .get instance valuetype System.Reflection.Emit.OpCodeType/*02000005*/ System.Reflection.Emit.OpCode/*02000003*/::get_OpCodeType() /* 06000004 */
  } // end of property OpCode::OpCodeType
  .property /*17000005*/ instance callconv(8) valuetype System.Reflection.Emit.OperandType/*02000006*/
          OperandType()
  {
    .get instance valuetype System.Reflection.Emit.OperandType/*02000006*/ System.Reflection.Emit.OpCode/*02000003*/::get_OperandType() /* 06000005 */
  } // end of property OpCode::OperandType
  .property /*17000006*/ instance callconv(8) int32
          Size()
  {
    .get instance int32 System.Reflection.Emit.OpCode/*02000003*/::get_Size() /* 06000006 */
  } // end of property OpCode::Size
  .property /*17000007*/ instance callconv(8) valuetype System.Reflection.Emit.StackBehaviour/*02000008*/
          StackBehaviourPop()
  {
    .get instance valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ System.Reflection.Emit.OpCode/*02000003*/::get_StackBehaviourPop() /* 06000007 */
  } // end of property OpCode::StackBehaviourPop
  .property /*17000008*/ instance callconv(8) valuetype System.Reflection.Emit.StackBehaviour/*02000008*/
          StackBehaviourPush()
  {
    .get instance valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ System.Reflection.Emit.OpCode/*02000003*/::get_StackBehaviourPush() /* 06000008 */
  } // end of property OpCode::StackBehaviourPush
  .property /*17000009*/ instance callconv(8) int16
          Value()
  {
    .get instance int16 System.Reflection.Emit.OpCode/*02000003*/::get_Value() /* 06000009 */
  } // end of property OpCode::Value
} // end of class System.Reflection.Emit.OpCode

.class /*02000004*/ public auto ansi beforefieldinit System.Reflection.Emit.OpCodes
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
{
  .field /*0400000C*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Add
  .field /*0400000D*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Add_Ovf
  .field /*0400000E*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Add_Ovf_Un
  .field /*0400000F*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ And
  .field /*04000010*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Arglist
  .field /*04000011*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Beq
  .field /*04000012*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Beq_S
  .field /*04000013*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Bge
  .field /*04000014*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Bge_S
  .field /*04000015*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Bge_Un
  .field /*04000016*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Bge_Un_S
  .field /*04000017*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Bgt
  .field /*04000018*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Bgt_S
  .field /*04000019*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Bgt_Un
  .field /*0400001A*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Bgt_Un_S
  .field /*0400001B*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ble
  .field /*0400001C*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ble_S
  .field /*0400001D*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ble_Un
  .field /*0400001E*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ble_Un_S
  .field /*0400001F*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Blt
  .field /*04000020*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Blt_S
  .field /*04000021*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Blt_Un
  .field /*04000022*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Blt_Un_S
  .field /*04000023*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Bne_Un
  .field /*04000024*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Bne_Un_S
  .field /*04000025*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Box
  .field /*04000026*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Br
  .field /*04000027*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Break
  .field /*04000028*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Brfalse
  .field /*04000029*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Brfalse_S
  .field /*0400002A*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Brtrue
  .field /*0400002B*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Brtrue_S
  .field /*0400002C*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Br_S
  .field /*0400002D*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Call
  .field /*0400002E*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Calli
  .field /*0400002F*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Callvirt
  .field /*04000030*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Castclass
  .field /*04000031*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ceq
  .field /*04000032*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Cgt
  .field /*04000033*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Cgt_Un
  .field /*04000034*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ckfinite
  .field /*04000035*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Clt
  .field /*04000036*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Clt_Un
  .field /*04000037*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Constrained
  .field /*04000038*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_I
  .field /*04000039*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_I1
  .field /*0400003A*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_I2
  .field /*0400003B*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_I4
  .field /*0400003C*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_I8
  .field /*0400003D*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_Ovf_I
  .field /*0400003E*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_Ovf_I1
  .field /*0400003F*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_Ovf_I1_Un
  .field /*04000040*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_Ovf_I2
  .field /*04000041*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_Ovf_I2_Un
  .field /*04000042*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_Ovf_I4
  .field /*04000043*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_Ovf_I4_Un
  .field /*04000044*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_Ovf_I8
  .field /*04000045*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_Ovf_I8_Un
  .field /*04000046*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_Ovf_I_Un
  .field /*04000047*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_Ovf_U
  .field /*04000048*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_Ovf_U1
  .field /*04000049*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_Ovf_U1_Un
  .field /*0400004A*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_Ovf_U2
  .field /*0400004B*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_Ovf_U2_Un
  .field /*0400004C*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_Ovf_U4
  .field /*0400004D*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_Ovf_U4_Un
  .field /*0400004E*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_Ovf_U8
  .field /*0400004F*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_Ovf_U8_Un
  .field /*04000050*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_Ovf_U_Un
  .field /*04000051*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_R4
  .field /*04000052*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_R8
  .field /*04000053*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_R_Un
  .field /*04000054*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_U
  .field /*04000055*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_U1
  .field /*04000056*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_U2
  .field /*04000057*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_U4
  .field /*04000058*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Conv_U8
  .field /*04000059*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Cpblk
  .field /*0400005A*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Cpobj
  .field /*0400005B*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Div
  .field /*0400005C*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Div_Un
  .field /*0400005D*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Dup
  .field /*0400005E*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Endfilter
  .field /*0400005F*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Endfinally
  .field /*04000060*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Initblk
  .field /*04000061*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Initobj
  .field /*04000062*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Isinst
  .field /*04000063*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Jmp
  .field /*04000064*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldarg
  .field /*04000065*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldarga
  .field /*04000066*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldarga_S
  .field /*04000067*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldarg_0
  .field /*04000068*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldarg_1
  .field /*04000069*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldarg_2
  .field /*0400006A*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldarg_3
  .field /*0400006B*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldarg_S
  .field /*0400006C*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldc_I4
  .field /*0400006D*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldc_I4_0
  .field /*0400006E*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldc_I4_1
  .field /*0400006F*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldc_I4_2
  .field /*04000070*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldc_I4_3
  .field /*04000071*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldc_I4_4
  .field /*04000072*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldc_I4_5
  .field /*04000073*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldc_I4_6
  .field /*04000074*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldc_I4_7
  .field /*04000075*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldc_I4_8
  .field /*04000076*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldc_I4_M1
  .field /*04000077*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldc_I4_S
  .field /*04000078*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldc_I8
  .field /*04000079*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldc_R4
  .field /*0400007A*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldc_R8
  .field /*0400007B*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldelem
  .field /*0400007C*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldelema
  .field /*0400007D*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldelem_I
  .field /*0400007E*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldelem_I1
  .field /*0400007F*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldelem_I2
  .field /*04000080*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldelem_I4
  .field /*04000081*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldelem_I8
  .field /*04000082*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldelem_R4
  .field /*04000083*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldelem_R8
  .field /*04000084*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldelem_Ref
  .field /*04000085*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldelem_U1
  .field /*04000086*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldelem_U2
  .field /*04000087*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldelem_U4
  .field /*04000088*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldfld
  .field /*04000089*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldflda
  .field /*0400008A*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldftn
  .field /*0400008B*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldind_I
  .field /*0400008C*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldind_I1
  .field /*0400008D*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldind_I2
  .field /*0400008E*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldind_I4
  .field /*0400008F*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldind_I8
  .field /*04000090*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldind_R4
  .field /*04000091*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldind_R8
  .field /*04000092*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldind_Ref
  .field /*04000093*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldind_U1
  .field /*04000094*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldind_U2
  .field /*04000095*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldind_U4
  .field /*04000096*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldlen
  .field /*04000097*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldloc
  .field /*04000098*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldloca
  .field /*04000099*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldloca_S
  .field /*0400009A*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldloc_0
  .field /*0400009B*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldloc_1
  .field /*0400009C*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldloc_2
  .field /*0400009D*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldloc_3
  .field /*0400009E*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldloc_S
  .field /*0400009F*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldnull
  .field /*040000A0*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldobj
  .field /*040000A1*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldsfld
  .field /*040000A2*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldsflda
  .field /*040000A3*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldstr
  .field /*040000A4*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldtoken
  .field /*040000A5*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ldvirtftn
  .field /*040000A6*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Leave
  .field /*040000A7*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Leave_S
  .field /*040000A8*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Localloc
  .field /*040000A9*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Mkrefany
  .field /*040000AA*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Mul
  .field /*040000AB*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Mul_Ovf
  .field /*040000AC*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Mul_Ovf_Un
  .field /*040000AD*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Neg
  .field /*040000AE*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Newarr
  .field /*040000AF*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Newobj
  .field /*040000B0*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Nop
  .field /*040000B1*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Not
  .field /*040000B2*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Or
  .field /*040000B3*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Pop
  .field /*040000B4*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Prefix1
  .field /*040000B5*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Prefix2
  .field /*040000B6*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Prefix3
  .field /*040000B7*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Prefix4
  .field /*040000B8*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Prefix5
  .field /*040000B9*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Prefix6
  .field /*040000BA*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Prefix7
  .field /*040000BB*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Prefixref
  .field /*040000BC*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Readonly
  .field /*040000BD*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Refanytype
  .field /*040000BE*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Refanyval
  .field /*040000BF*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Rem
  .field /*040000C0*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Rem_Un
  .field /*040000C1*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Ret
  .field /*040000C2*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Rethrow
  .field /*040000C3*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Shl
  .field /*040000C4*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Shr
  .field /*040000C5*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Shr_Un
  .field /*040000C6*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Sizeof
  .field /*040000C7*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Starg
  .field /*040000C8*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Starg_S
  .field /*040000C9*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stelem
  .field /*040000CA*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stelem_I
  .field /*040000CB*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stelem_I1
  .field /*040000CC*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stelem_I2
  .field /*040000CD*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stelem_I4
  .field /*040000CE*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stelem_I8
  .field /*040000CF*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stelem_R4
  .field /*040000D0*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stelem_R8
  .field /*040000D1*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stelem_Ref
  .field /*040000D2*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stfld
  .field /*040000D3*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stind_I
  .field /*040000D4*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stind_I1
  .field /*040000D5*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stind_I2
  .field /*040000D6*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stind_I4
  .field /*040000D7*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stind_I8
  .field /*040000D8*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stind_R4
  .field /*040000D9*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stind_R8
  .field /*040000DA*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stind_Ref
  .field /*040000DB*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stloc
  .field /*040000DC*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stloc_0
  .field /*040000DD*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stloc_1
  .field /*040000DE*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stloc_2
  .field /*040000DF*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stloc_3
  .field /*040000E0*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stloc_S
  .field /*040000E1*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stobj
  .field /*040000E2*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Stsfld
  .field /*040000E3*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Sub
  .field /*040000E4*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Sub_Ovf
  .field /*040000E5*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Sub_Ovf_Un
  .field /*040000E6*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Switch
  .field /*040000E7*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Tailcall
  .field /*040000E8*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Throw
  .field /*040000E9*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Unaligned
  .field /*040000EA*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Unbox
  .field /*040000EB*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Unbox_Any
  .field /*040000EC*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Volatile
  .field /*040000ED*/ public static initonly valuetype System.Reflection.Emit.OpCode/*02000003*/ Xor
  .method /*06000010*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x207d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001B*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method OpCodes::.ctor

  .method /*06000011*/ public hidebysig static 
          bool  TakesSingleByteArgument(valuetype System.Reflection.Emit.OpCode/*02000003*/ inst) cil managed
  // SIG: 00 01 02 11 0C
  {
    // Method begins at RVA 0x2085
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OpCodes::TakesSingleByteArgument

} // end of class System.Reflection.Emit.OpCodes

.class /*02000005*/ public auto ansi sealed System.Reflection.Emit.OpCodeType
       extends [System.Runtime/*23000001*/]System.Enum/*01000013*/
{
  .field /*040000EE*/ public specialname rtspecialname int32 value__
  .field /*040000EF*/ public static literal valuetype System.Reflection.Emit.OpCodeType/*02000005*/ Annotation = int32(0x00000000)
  .custom /*0C00001A:0A000012*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000014*/::.ctor(string) /* 0A000012 */ = ( 01 00 3F 4F 70 43 6F 64 65 54 79 70 65 2E 41 6E   // ..?OpCodeType.An
                                                                                                                                                 6E 6F 74 61 74 69 6F 6E 20 68 61 73 20 62 65 65   // notation has bee
                                                                                                                                                 6E 20 64 65 70 72 65 63 61 74 65 64 20 61 6E 64   // n deprecated and
                                                                                                                                                 20 69 73 20 6E 6F 74 20 73 75 70 70 6F 72 74 65   //  is not supporte
                                                                                                                                                 64 2E 00 00 )                                     // d...
  .field /*040000F0*/ public static literal valuetype System.Reflection.Emit.OpCodeType/*02000005*/ Macro = int32(0x00000001)
  .field /*040000F1*/ public static literal valuetype System.Reflection.Emit.OpCodeType/*02000005*/ Nternal = int32(0x00000002)
  .field /*040000F2*/ public static literal valuetype System.Reflection.Emit.OpCodeType/*02000005*/ Objmodel = int32(0x00000003)
  .field /*040000F3*/ public static literal valuetype System.Reflection.Emit.OpCodeType/*02000005*/ Prefix = int32(0x00000004)
  .field /*040000F4*/ public static literal valuetype System.Reflection.Emit.OpCodeType/*02000005*/ Primitive = int32(0x00000005)
} // end of class System.Reflection.Emit.OpCodeType

.class /*02000006*/ public auto ansi sealed System.Reflection.Emit.OperandType
       extends [System.Runtime/*23000001*/]System.Enum/*01000013*/
{
  .field /*040000F5*/ public specialname rtspecialname int32 value__
  .field /*040000F6*/ public static literal valuetype System.Reflection.Emit.OperandType/*02000006*/ InlineBrTarget = int32(0x00000000)
  .field /*040000F7*/ public static literal valuetype System.Reflection.Emit.OperandType/*02000006*/ InlineField = int32(0x00000001)
  .field /*040000F8*/ public static literal valuetype System.Reflection.Emit.OperandType/*02000006*/ InlineI = int32(0x00000002)
  .field /*040000F9*/ public static literal valuetype System.Reflection.Emit.OperandType/*02000006*/ InlineI8 = int32(0x00000003)
  .field /*040000FA*/ public static literal valuetype System.Reflection.Emit.OperandType/*02000006*/ InlineMethod = int32(0x00000004)
  .field /*040000FB*/ public static literal valuetype System.Reflection.Emit.OperandType/*02000006*/ InlineNone = int32(0x00000005)
  .field /*040000FC*/ public static literal valuetype System.Reflection.Emit.OperandType/*02000006*/ InlinePhi = int32(0x00000006)
  .custom /*0C00001B:0A000012*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000014*/::.ctor(string) /* 0A000012 */ = ( 01 00 3F 4F 70 65 72 61 6E 64 54 79 70 65 2E 49   // ..?OperandType.I
                                                                                                                                                 6E 6C 69 6E 65 50 68 69 20 68 61 73 20 62 65 65   // nlinePhi has bee
                                                                                                                                                 6E 20 64 65 70 72 65 63 61 74 65 64 20 61 6E 64   // n deprecated and
                                                                                                                                                 20 69 73 20 6E 6F 74 20 73 75 70 70 6F 72 74 65   //  is not supporte
                                                                                                                                                 64 2E 00 00 )                                     // d...
  .field /*040000FD*/ public static literal valuetype System.Reflection.Emit.OperandType/*02000006*/ InlineR = int32(0x00000007)
  .field /*040000FE*/ public static literal valuetype System.Reflection.Emit.OperandType/*02000006*/ InlineSig = int32(0x00000009)
  .field /*040000FF*/ public static literal valuetype System.Reflection.Emit.OperandType/*02000006*/ InlineString = int32(0x0000000A)
  .field /*04000100*/ public static literal valuetype System.Reflection.Emit.OperandType/*02000006*/ InlineSwitch = int32(0x0000000B)
  .field /*04000101*/ public static literal valuetype System.Reflection.Emit.OperandType/*02000006*/ InlineTok = int32(0x0000000C)
  .field /*04000102*/ public static literal valuetype System.Reflection.Emit.OperandType/*02000006*/ InlineType = int32(0x0000000D)
  .field /*04000103*/ public static literal valuetype System.Reflection.Emit.OperandType/*02000006*/ InlineVar = int32(0x0000000E)
  .field /*04000104*/ public static literal valuetype System.Reflection.Emit.OperandType/*02000006*/ ShortInlineBrTarget = int32(0x0000000F)
  .field /*04000105*/ public static literal valuetype System.Reflection.Emit.OperandType/*02000006*/ ShortInlineI = int32(0x00000010)
  .field /*04000106*/ public static literal valuetype System.Reflection.Emit.OperandType/*02000006*/ ShortInlineR = int32(0x00000011)
  .field /*04000107*/ public static literal valuetype System.Reflection.Emit.OperandType/*02000006*/ ShortInlineVar = int32(0x00000012)
} // end of class System.Reflection.Emit.OperandType

.class /*02000007*/ public auto ansi sealed System.Reflection.Emit.PackingSize
       extends [System.Runtime/*23000001*/]System.Enum/*01000013*/
{
  .field /*04000108*/ public specialname rtspecialname int32 value__
  .field /*04000109*/ public static literal valuetype System.Reflection.Emit.PackingSize/*02000007*/ Unspecified = int32(0x00000000)
  .field /*0400010A*/ public static literal valuetype System.Reflection.Emit.PackingSize/*02000007*/ Size1 = int32(0x00000001)
  .field /*0400010B*/ public static literal valuetype System.Reflection.Emit.PackingSize/*02000007*/ Size2 = int32(0x00000002)
  .field /*0400010C*/ public static literal valuetype System.Reflection.Emit.PackingSize/*02000007*/ Size4 = int32(0x00000004)
  .field /*0400010D*/ public static literal valuetype System.Reflection.Emit.PackingSize/*02000007*/ Size8 = int32(0x00000008)
  .field /*0400010E*/ public static literal valuetype System.Reflection.Emit.PackingSize/*02000007*/ Size16 = int32(0x00000010)
  .field /*0400010F*/ public static literal valuetype System.Reflection.Emit.PackingSize/*02000007*/ Size32 = int32(0x00000020)
  .field /*04000110*/ public static literal valuetype System.Reflection.Emit.PackingSize/*02000007*/ Size64 = int32(0x00000040)
  .field /*04000111*/ public static literal valuetype System.Reflection.Emit.PackingSize/*02000007*/ Size128 = int32(0x00000080)
} // end of class System.Reflection.Emit.PackingSize

.class /*02000008*/ public auto ansi sealed System.Reflection.Emit.StackBehaviour
       extends [System.Runtime/*23000001*/]System.Enum/*01000013*/
{
  .field /*04000112*/ public specialname rtspecialname int32 value__
  .field /*04000113*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Pop0 = int32(0x00000000)
  .field /*04000114*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Pop1 = int32(0x00000001)
  .field /*04000115*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Pop1_pop1 = int32(0x00000002)
  .field /*04000116*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Popi = int32(0x00000003)
  .field /*04000117*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Popi_pop1 = int32(0x00000004)
  .field /*04000118*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Popi_popi = int32(0x00000005)
  .field /*04000119*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Popi_popi8 = int32(0x00000006)
  .field /*0400011A*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Popi_popi_popi = int32(0x00000007)
  .field /*0400011B*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Popi_popr4 = int32(0x00000008)
  .field /*0400011C*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Popi_popr8 = int32(0x00000009)
  .field /*0400011D*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Popref = int32(0x0000000A)
  .field /*0400011E*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Popref_pop1 = int32(0x0000000B)
  .field /*0400011F*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Popref_popi = int32(0x0000000C)
  .field /*04000120*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Popref_popi_popi = int32(0x0000000D)
  .field /*04000121*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Popref_popi_popi8 = int32(0x0000000E)
  .field /*04000122*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Popref_popi_popr4 = int32(0x0000000F)
  .field /*04000123*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Popref_popi_popr8 = int32(0x00000010)
  .field /*04000124*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Popref_popi_popref = int32(0x00000011)
  .field /*04000125*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Push0 = int32(0x00000012)
  .field /*04000126*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Push1 = int32(0x00000013)
  .field /*04000127*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Push1_push1 = int32(0x00000014)
  .field /*04000128*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Pushi = int32(0x00000015)
  .field /*04000129*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Pushi8 = int32(0x00000016)
  .field /*0400012A*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Pushr4 = int32(0x00000017)
  .field /*0400012B*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Pushr8 = int32(0x00000018)
  .field /*0400012C*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Pushref = int32(0x00000019)
  .field /*0400012D*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Varpop = int32(0x0000001A)
  .field /*0400012E*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Varpush = int32(0x0000001B)
  .field /*0400012F*/ public static literal valuetype System.Reflection.Emit.StackBehaviour/*02000008*/ Popref_popi_pop1 = int32(0x0000001C)
} // end of class System.Reflection.Emit.StackBehaviour


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
