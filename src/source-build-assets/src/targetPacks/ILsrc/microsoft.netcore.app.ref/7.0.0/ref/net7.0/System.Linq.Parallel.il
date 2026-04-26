


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
// Time-date stamp:            0xddb3627e
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
// Addr. of entry point:           0x00006b0e
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
// Checksum:                       0x00009d74
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00006ab9 [0x0000004f] address [size] of Import Directory:          
// 0x00008000 [0x00000484] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00005600 [0x000028a0] address [size] of Security Directory:        
// 0x0000a000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x000069d4 [0x00000054] address [size] of Debug Directory:           
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
//              0x00004b14 Virtual Size
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
//              Entry 1: Type 0x3 Offset 0x00000b10
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00006afb Import Name Table
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
// 0x00002330 [0x00004624] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00006954 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00002cf0 Size
//              '#~' Name
//    Stream 2:
//              0x00002d5c Offset
//              0x0000095c Size
//              '#Strings' Name
//    Stream 3:
//              0x000036b8 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x000036bc Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x000036cc Offset
//              0x00000f58 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000004090a001f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 32416
// PE header size       : 512 (496 used)    ( 1.58%)
// PE additional info   : 11811             (36.44%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.22%)
// CLR meta-data size  : 17956              (55.39%)
// CLR additional info : 128                ( 0.39%)
// CLR method headers  : 215                ( 0.66%)
// Managed code         : 518               ( 1.60%)
// Data                 : 2048              ( 6.32%)
// Unaccounted          : -844              (-2.60%)

// Num.of PE sections   : 3
//   .text    - 19456
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 17956
//   Module        -    1 (10 bytes)
//   TypeDef       -   12 (168 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   43 (258 bytes)
//   MethodDef     -  215 (3010 bytes)    0 abstract, 0 native, 215 bodies
//   FieldDef      -   12 (72 bytes)      0 constant
//   MemberRef     -   23 (138 bytes)
//   ParamDef      -  415 (2490 bytes)
//   MethodImpl    -    1 (6 bytes)
//   Constant      -    6 (36 bytes)
//   CustomAttribute-  528 (3168 bytes)
//   InterfaceImpl -    3 (12 bytes)
//   TypeSpec      -    2 (4 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    4 (80 bytes)
//   GenericParam  -  243 (1944 bytes)
//   Strings       -  2394 bytes
//   Blobs         -  3928 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   196 bytes

// CLR additional info : 128

// CLR method headers : 215
//   Num.of method bodies  - 215
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 215

// Managed code : 518
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class RefSafetyRulesAttribute        (auto) (ansi) (sealed) 
// Class OrderedParallelQuery`1         (public) (auto) (ansi) 
// Class ParallelEnumerable             (public) (abstract) (auto) (ansi) (sealed) 
// Class ParallelExecutionMode          (public) (auto) (ansi) (sealed) 
// Class ParallelMergeOptions           (public) (auto) (ansi) (sealed) 
// Class ParallelQuery                  (public) (auto) (ansi) 
// Class ParallelQuery`1                (public) (auto) (ansi) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000002*/ System.Collections.Concurrent
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000003*/ System.Linq
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000004*/ System.Collections
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly /*20000001*/ noplatform System.Linq.Parallel
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
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 14 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50   // ...System.Linq.P
                                                                                                                                                                        61 72 61 6C 6C 65 6C 00 00 )                      // arallel..
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000E:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000F:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000010:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 14 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50   // ...System.Linq.P
                                                                                                                                                                       61 72 61 6C 6C 65 6C 00 00 )                      // arallel..
  .custom /*0C000011:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 37 2E 30 2E 32 32 2E 35 31 38 30 35 00   // ...7.0.22.51805.
                                                                                                                                                                       00 ) 
  .custom /*0C000012:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 37 2E 30 2E 30 2B 64 30 39 39 66 30 37   // ...7.0.0+d099f07
                                                                                                                                                                                35 65 34 35 64 32 61 61 36 30 30 37 61 32 32 62   // 5e45d2aa6007a22b
                                                                                                                                                                                37 31 62 34 35 61 30 38 37 35 38 35 35 39 66 38   // 71b45a08758559f8
                                                                                                                                                                                30 00 00 )                                        // 0..
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000014:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 14 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50   // ...System.Linq.P
                                                                                                                                                                 61 72 61 6C 6C 65 6C 00 00 )                      // arallel..
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
.module System.Linq.Parallel.dll
// MVID: {8fdbd022-adb0-43c8-b6d2-d0f7c80ab3bf}
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
  .custom /*0C000017:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000018:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C00001B:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C00001C:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001D:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000015 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)00002B       */ newarr     [System.Runtime/*23000001*/]System.Byte/*0100002B*/
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
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000015 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C00001F:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000020:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000021:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000015 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000023:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000024:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000025:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000015 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.RefSafetyRulesAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000027:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000028:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000029:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000015       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000015 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000004       */ stfld      int32 System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::Version /* 04000004 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method RefSafetyRulesAttribute::.ctor

} // end of class System.Runtime.CompilerServices.RefSafetyRulesAttribute

.class /*02000007*/ public auto ansi beforefieldinit System.Linq.OrderedParallelQuery`1<TSource>
       extends class System.Linq.ParallelQuery`1/*0200000C*/<!TSource>/*1B000001*/
{
  .custom /*0C00002B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type TSource /*2A000001*/ 
    .custom /*0C000016:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*06000007*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ac
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000016       */ call       instance void class System.Linq.ParallelQuery`1/*0200000C*/<!TSource>/*1B000001*/::.ctor() /* 0A000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method OrderedParallelQuery`1::.ctor

  .method /*06000008*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000016*/<!TSource> 
          GetEnumerator() cil managed
  // SIG: 20 00 15 12 59 01 13 00
  {
    // Method begins at RVA 0x20b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OrderedParallelQuery`1::GetEnumerator

} // end of class System.Linq.OrderedParallelQuery`1

.class /*02000008*/ public abstract auto ansi sealed beforefieldinit System.Linq.ParallelEnumerable
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00002E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000030:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000009*/ public hidebysig static 
          !!TSource  Aggregate<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                        class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TSource,!!TSource,!!TSource> func) cil managed
  // SIG: 10 01 02 1E 00 15 12 30 01 1E 00 15 12 61 03 1E 00 1E 00 1E 00
  {
    .custom /*0C000032:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000002*/ 
      .custom /*0C00001A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Aggregate

  .method /*0600000A*/ public hidebysig static 
          !!TAccumulate  Aggregate<TSource,TAccumulate>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                                        !!TAccumulate seed,
                                                        class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TAccumulate,!!TSource,!!TAccumulate> func) cil managed
  // SIG: 10 02 03 1E 01 15 12 30 01 1E 00 1E 01 15 12 61 03 1E 01 1E 00 1E 01
  {
    .custom /*0C000034:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000003*/ 
      .custom /*0C00001E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TAccumulate /*2A000004*/ 
      .custom /*0C000022:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Aggregate

  .method /*0600000B*/ public hidebysig static 
          !!TResult  Aggregate<TSource,TAccumulate,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                                            class [System.Runtime/*23000001*/]System.Func`1/*01000019*/<!!TAccumulate> seedFactory,
                                                            class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TAccumulate,!!TSource,!!TAccumulate> updateAccumulatorFunc,
                                                            class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TAccumulate,!!TAccumulate,!!TAccumulate> combineAccumulatorsFunc,
                                                            class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TAccumulate,!!TResult> resultSelector) cil managed
  // SIG: 10 03 05 1E 02 15 12 30 01 1E 00 15 12 65 01 1E 01 15 12 61 03 1E 01 1E 00 1E 01 15 12 61 03 1E
  //      01 1E 01 1E 01 15 12 69 02 1E 01 1E 02
  {
    .custom /*0C000036:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000005*/ 
      .custom /*0C000026:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TAccumulate /*2A000006*/ 
      .custom /*0C00002A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000007*/ 
      .custom /*0C00002D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Aggregate

  .method /*0600000C*/ public hidebysig static 
          !!TResult  Aggregate<TSource,TAccumulate,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                                            !!TAccumulate seed,
                                                            class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TAccumulate,!!TSource,!!TAccumulate> updateAccumulatorFunc,
                                                            class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TAccumulate,!!TAccumulate,!!TAccumulate> combineAccumulatorsFunc,
                                                            class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TAccumulate,!!TResult> resultSelector) cil managed
  // SIG: 10 03 05 1E 02 15 12 30 01 1E 00 1E 01 15 12 61 03 1E 01 1E 00 1E 01 15 12 61 03 1E 01 1E 01 1E
  //      01 15 12 69 02 1E 01 1E 02
  {
    .custom /*0C000038:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000009*/ 
      .custom /*0C000033:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TAccumulate /*2A00000A*/ 
      .custom /*0C000035:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A00000B*/ 
      .custom /*0C000037:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Aggregate

  .method /*0600000D*/ public hidebysig static 
          !!TResult  Aggregate<TSource,TAccumulate,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                                            !!TAccumulate seed,
                                                            class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TAccumulate,!!TSource,!!TAccumulate> func,
                                                            class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TAccumulate,!!TResult> resultSelector) cil managed
  // SIG: 10 03 04 1E 02 15 12 30 01 1E 00 1E 01 15 12 61 03 1E 01 1E 00 1E 01 15 12 69 02 1E 01 1E 02
  {
    .custom /*0C00003A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00000C*/ 
      .custom /*0C000039:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TAccumulate /*2A00000D*/ 
      .custom /*0C00003B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A00000E*/ 
      .custom /*0C00003D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Aggregate

  .method /*0600000E*/ public hidebysig static 
          bool  All<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                             class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,bool> predicate) cil managed
  // SIG: 10 01 02 02 15 12 30 01 1E 00 15 12 69 02 1E 00 02
  {
    .custom /*0C00003C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00000F*/ 
      .custom /*0C00003F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::All

  .method /*0600000F*/ public hidebysig static 
          bool  Any<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source) cil managed
  // SIG: 10 01 01 02 15 12 30 01 1E 00
  {
    .custom /*0C00003E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000010*/ 
      .custom /*0C000041:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Any

  .method /*06000010*/ public hidebysig static 
          bool  Any<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                             class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,bool> predicate) cil managed
  // SIG: 10 01 02 02 15 12 30 01 1E 00 15 12 69 02 1E 00 02
  {
    .custom /*0C000040:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000011*/ 
      .custom /*0C000043:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Any

  .method /*06000011*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> 
          AsEnumerable<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source) cil managed
  // SIG: 10 01 01 15 12 6D 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C000042:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000012*/ 
      .custom /*0C000045:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::AsEnumerable

  .method /*06000012*/ public hidebysig static 
          class System.Linq.ParallelQuery/*0200000B*/ 
          AsOrdered(class System.Linq.ParallelQuery/*0200000B*/ source) cil managed
  // SIG: 00 01 12 2C 12 2C
  {
    .custom /*0C000044:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::AsOrdered

  .method /*06000013*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          AsOrdered<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source) cil managed
  // SIG: 10 01 01 15 12 30 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C000046:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000013*/ 
      .custom /*0C000047:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::AsOrdered

  .method /*06000014*/ public hidebysig static 
          class System.Linq.ParallelQuery/*0200000B*/ 
          AsParallel(class [System.Runtime/*23000001*/]System.Collections.IEnumerable/*0100001C*/ source) cil managed
  // SIG: 00 01 12 2C 12 71
  {
    .custom /*0C000048:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::AsParallel

  .method /*06000015*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          AsParallel<TSource>(class [System.Collections.Concurrent/*23000002*/]System.Collections.Concurrent.Partitioner`1/*0100001D*/<!!TSource> source) cil managed
  // SIG: 10 01 01 15 12 30 01 1E 00 15 12 75 01 1E 00
  {
    .custom /*0C00004A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000014*/ 
      .custom /*0C000049:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::AsParallel

  .method /*06000016*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          AsParallel<TSource>(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> source) cil managed
  // SIG: 10 01 01 15 12 30 01 1E 00 15 12 6D 01 1E 00
  {
    .custom /*0C00004C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000015*/ 
      .custom /*0C00004B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::AsParallel

  .method /*06000017*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> 
          AsSequential<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source) cil managed
  // SIG: 10 01 01 15 12 6D 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C00004E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000016*/ 
      .custom /*0C00004D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::AsSequential

  .method /*06000018*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          AsUnordered<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source) cil managed
  // SIG: 10 01 01 15 12 30 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C000050:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000017*/ 
      .custom /*0C00004F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::AsUnordered

  .method /*06000019*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/ 
          Average(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/> source) cil managed
  // SIG: 00 01 11 79 15 12 30 01 11 79
  {
    .custom /*0C000052:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Average

  .method /*0600001A*/ public hidebysig static 
          float64  Average(class System.Linq.ParallelQuery`1/*0200000C*/<float64> source) cil managed
  // SIG: 00 01 0D 15 12 30 01 0D
  {
    .custom /*0C000054:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Average

  .method /*0600001B*/ public hidebysig static 
          float64  Average(class System.Linq.ParallelQuery`1/*0200000C*/<int32> source) cil managed
  // SIG: 00 01 0D 15 12 30 01 08
  {
    .custom /*0C000056:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Average

  .method /*0600001C*/ public hidebysig static 
          float64  Average(class System.Linq.ParallelQuery`1/*0200000C*/<int64> source) cil managed
  // SIG: 00 01 0D 15 12 30 01 0A
  {
    .custom /*0C000058:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Average

  .method /*0600001D*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/> 
          Average(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/>> source) cil managed
  // SIG: 00 01 15 11 7D 01 11 79 15 12 30 01 15 11 7D 01 11 79
  {
    .custom /*0C00005A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Average

  .method /*0600001E*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float64> 
          Average(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float64>> source) cil managed
  // SIG: 00 01 15 11 7D 01 0D 15 12 30 01 15 11 7D 01 0D
  {
    .custom /*0C00005C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Average

  .method /*0600001F*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float64> 
          Average(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int32>> source) cil managed
  // SIG: 00 01 15 11 7D 01 0D 15 12 30 01 15 11 7D 01 08
  {
    .custom /*0C00005E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Average

  .method /*06000020*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float64> 
          Average(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int64>> source) cil managed
  // SIG: 00 01 15 11 7D 01 0D 15 12 30 01 15 11 7D 01 0A
  {
    .custom /*0C000060:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Average

  .method /*06000021*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float32> 
          Average(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float32>> source) cil managed
  // SIG: 00 01 15 11 7D 01 0C 15 12 30 01 15 11 7D 01 0C
  {
    .custom /*0C000062:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Average

  .method /*06000022*/ public hidebysig static 
          float32  Average(class System.Linq.ParallelQuery`1/*0200000C*/<float32> source) cil managed
  // SIG: 00 01 0C 15 12 30 01 0C
  {
    .custom /*0C000064:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2102
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Average

  .method /*06000023*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/ 
          Average<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                           class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/> selector) cil managed
  // SIG: 10 01 02 11 79 15 12 30 01 1E 00 15 12 69 02 1E 00 11 79
  {
    .custom /*0C000066:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000018*/ 
      .custom /*0C000051:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2105
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Average

  .method /*06000024*/ public hidebysig static 
          float64  Average<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                    class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,float64> selector) cil managed
  // SIG: 10 01 02 0D 15 12 30 01 1E 00 15 12 69 02 1E 00 0D
  {
    .custom /*0C000068:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000019*/ 
      .custom /*0C000053:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2108
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Average

  .method /*06000025*/ public hidebysig static 
          float64  Average<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                    class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,int32> selector) cil managed
  // SIG: 10 01 02 0D 15 12 30 01 1E 00 15 12 69 02 1E 00 08
  {
    .custom /*0C00006A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00001A*/ 
      .custom /*0C000055:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x210b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Average

  .method /*06000026*/ public hidebysig static 
          float64  Average<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                    class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,int64> selector) cil managed
  // SIG: 10 01 02 0D 15 12 30 01 1E 00 15 12 69 02 1E 00 0A
  {
    .custom /*0C00006C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00001B*/ 
      .custom /*0C000057:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x210e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Average

  .method /*06000027*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/> 
          Average<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                           class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/>> selector) cil managed
  // SIG: 10 01 02 15 11 7D 01 11 79 15 12 30 01 1E 00 15 12 69 02 1E 00 15 11 7D 01 11 79
  {
    .custom /*0C00006E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00001C*/ 
      .custom /*0C000059:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2111
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Average

  .method /*06000028*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float64> 
          Average<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                           class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float64>> selector) cil managed
  // SIG: 10 01 02 15 11 7D 01 0D 15 12 30 01 1E 00 15 12 69 02 1E 00 15 11 7D 01 0D
  {
    .custom /*0C000070:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00001D*/ 
      .custom /*0C00005B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2114
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Average

  .method /*06000029*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float64> 
          Average<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                           class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int32>> selector) cil managed
  // SIG: 10 01 02 15 11 7D 01 0D 15 12 30 01 1E 00 15 12 69 02 1E 00 15 11 7D 01 08
  {
    .custom /*0C000072:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00001E*/ 
      .custom /*0C00005D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2117
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Average

  .method /*0600002A*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float64> 
          Average<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                           class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int64>> selector) cil managed
  // SIG: 10 01 02 15 11 7D 01 0D 15 12 30 01 1E 00 15 12 69 02 1E 00 15 11 7D 01 0A
  {
    .custom /*0C000074:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00001F*/ 
      .custom /*0C00005F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Average

  .method /*0600002B*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float32> 
          Average<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                           class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float32>> selector) cil managed
  // SIG: 10 01 02 15 11 7D 01 0C 15 12 30 01 1E 00 15 12 69 02 1E 00 15 11 7D 01 0C
  {
    .custom /*0C000076:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000020*/ 
      .custom /*0C000061:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Average

  .method /*0600002C*/ public hidebysig static 
          float32  Average<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                    class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,float32> selector) cil managed
  // SIG: 10 01 02 0C 15 12 30 01 1E 00 15 12 69 02 1E 00 0C
  {
    .custom /*0C000078:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000021*/ 
      .custom /*0C000063:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2120
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Average

  .method /*0600002D*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          Cast<TResult>(class System.Linq.ParallelQuery/*0200000B*/ source) cil managed
  // SIG: 10 01 01 15 12 30 01 1E 00 12 2C
  {
    .custom /*0C00007A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TResult /*2A000022*/ 
      .custom /*0C000065:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2123
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Cast

  .method /*0600002E*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Concat<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> first,
                          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> second) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 6D 01 1E 00
  {
    .custom /*0C00007B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C00007C:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000020*/::.ctor(string) /* 0A000013 */ = ( 01 00 81 0C 54 68 65 20 73 65 63 6F 6E 64 20 64   // ....The second d
                                                                                                                                                   61 74 61 20 73 6F 75 72 63 65 20 6F 66 20 61 20   // ata source of a 
                                                                                                                                                   62 69 6E 61 72 79 20 6F 70 65 72 61 74 6F 72 20   // binary operator 
                                                                                                                                                   6D 75 73 74 20 62 65 20 6F 66 20 74 79 70 65 20   // must be of type 
                                                                                                                                                   53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50 61 72 61   // System.Linq.Para
                                                                                                                                                   6C 6C 65 6C 51 75 65 72 79 3C 54 3E 20 72 61 74   // llelQuery<T> rat
                                                                                                                                                   68 65 72 20 74 68 61 6E 20 53 79 73 74 65 6D 2E   // her than System.
                                                                                                                                                   43 6F 6C 6C 65 63 74 69 6F 6E 73 2E 47 65 6E 65   // Collections.Gene
                                                                                                                                                   72 69 63 2E 49 45 6E 75 6D 65 72 61 62 6C 65 3C   // ric.IEnumerable<
                                                                                                                                                   54 3E 2E 20 54 6F 20 66 69 78 20 74 68 69 73 20   // T>. To fix this 
                                                                                                                                                   70 72 6F 62 6C 65 6D 2C 20 75 73 65 20 74 68 65   // problem, use the
                                                                                                                                                   20 41 73 50 61 72 61 6C 6C 65 6C 28 29 20 65 78   //  AsParallel() ex
                                                                                                                                                   74 65 6E 73 69 6F 6E 20 6D 65 74 68 6F 64 20 74   // tension method t
                                                                                                                                                   6F 20 63 6F 6E 76 65 72 74 20 74 68 65 20 72 69   // o convert the ri
                                                                                                                                                   67 68 74 20 64 61 74 61 20 73 6F 75 72 63 65 20   // ght data source 
                                                                                                                                                   74 6F 20 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50   // to System.Linq.P
                                                                                                                                                   61 72 61 6C 6C 65 6C 51 75 65 72 79 3C 54 3E 2E   // arallelQuery<T>.
                                                                                                                                                   00 00 ) 
    .param type TSource /*2A000023*/ 
      .custom /*0C000067:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2126
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Concat

  .method /*0600002F*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Concat<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> first,
                          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> second) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C00007E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000024*/ 
      .custom /*0C000069:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2129
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Concat

  .method /*06000030*/ public hidebysig static 
          bool  Contains<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                  !!TSource 'value') cil managed
  // SIG: 10 01 02 02 15 12 30 01 1E 00 1E 00
  {
    .custom /*0C000080:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000025*/ 
      .custom /*0C00006B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x212c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Contains

  .method /*06000031*/ public hidebysig static 
          bool  Contains<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                  !!TSource 'value',
                                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TSource> comparer) cil managed
  // SIG: 10 01 03 02 15 12 30 01 1E 00 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C000082:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000026*/ 
      .custom /*0C00006D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000048*/ 
    .custom /*0C0000B3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x212f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Contains

  .method /*06000032*/ public hidebysig static 
          int32  Count<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source) cil managed
  // SIG: 10 01 01 08 15 12 30 01 1E 00
  {
    .custom /*0C000084:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000027*/ 
      .custom /*0C00006F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2132
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Count

  .method /*06000033*/ public hidebysig static 
          int32  Count<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,bool> predicate) cil managed
  // SIG: 10 01 02 08 15 12 30 01 1E 00 15 12 69 02 1E 00 02
  {
    .custom /*0C000086:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000028*/ 
      .custom /*0C000071:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2135
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Count

  .method /*06000034*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          DefaultIfEmpty<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source) cil managed
  // SIG: 10 01 01 15 12 30 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C000088:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000029*/ 
      .custom /*0C000073:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2138
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::DefaultIfEmpty

  .method /*06000035*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          DefaultIfEmpty<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                  !!TSource defaultValue) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 1E 00
  {
    .custom /*0C000089:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00002A*/ 
      .custom /*0C000075:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x213b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::DefaultIfEmpty

  .method /*06000036*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Distinct<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source) cil managed
  // SIG: 10 01 01 15 12 30 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C00008B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00002B*/ 
      .custom /*0C000077:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x213e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Distinct

  .method /*06000037*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Distinct<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                            class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TSource> comparer) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C00008D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00002C*/ 
      .custom /*0C000079:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000051*/ 
    .custom /*0C0000CA:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2141
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Distinct

  .method /*06000038*/ public hidebysig static 
          !!TSource  ElementAtOrDefault<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                                 int32 index) cil managed
  // SIG: 10 01 02 1E 00 15 12 30 01 1E 00 08
  {
    .custom /*0C00008F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000090:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000052*/ 
    .custom /*0C0000CD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2144
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::ElementAtOrDefault

  .method /*06000039*/ public hidebysig static 
          !!TSource  ElementAt<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                        int32 index) cil managed
  // SIG: 10 01 02 1E 00 15 12 30 01 1E 00 08
  {
    .custom /*0C000092:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00002E*/ 
      .custom /*0C00007D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2147
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::ElementAt

  .method /*0600003A*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          Empty<TResult>() cil managed
  // SIG: 10 01 00 15 12 30 01 1E 00
  {
    .param type TResult /*2A00002F*/ 
      .custom /*0C00007F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x214a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Empty

  .method /*0600003B*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Except<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> first,
                          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> second) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 6D 01 1E 00
  {
    .custom /*0C000095:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000096:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000020*/::.ctor(string) /* 0A000013 */ = ( 01 00 81 0C 54 68 65 20 73 65 63 6F 6E 64 20 64   // ....The second d
                                                                                                                                                   61 74 61 20 73 6F 75 72 63 65 20 6F 66 20 61 20   // ata source of a 
                                                                                                                                                   62 69 6E 61 72 79 20 6F 70 65 72 61 74 6F 72 20   // binary operator 
                                                                                                                                                   6D 75 73 74 20 62 65 20 6F 66 20 74 79 70 65 20   // must be of type 
                                                                                                                                                   53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50 61 72 61   // System.Linq.Para
                                                                                                                                                   6C 6C 65 6C 51 75 65 72 79 3C 54 3E 20 72 61 74   // llelQuery<T> rat
                                                                                                                                                   68 65 72 20 74 68 61 6E 20 53 79 73 74 65 6D 2E   // her than System.
                                                                                                                                                   43 6F 6C 6C 65 63 74 69 6F 6E 73 2E 47 65 6E 65   // Collections.Gene
                                                                                                                                                   72 69 63 2E 49 45 6E 75 6D 65 72 61 62 6C 65 3C   // ric.IEnumerable<
                                                                                                                                                   54 3E 2E 20 54 6F 20 66 69 78 20 74 68 69 73 20   // T>. To fix this 
                                                                                                                                                   70 72 6F 62 6C 65 6D 2C 20 75 73 65 20 74 68 65   // problem, use the
                                                                                                                                                   20 41 73 50 61 72 61 6C 6C 65 6C 28 29 20 65 78   //  AsParallel() ex
                                                                                                                                                   74 65 6E 73 69 6F 6E 20 6D 65 74 68 6F 64 20 74   // tension method t
                                                                                                                                                   6F 20 63 6F 6E 76 65 72 74 20 74 68 65 20 72 69   // o convert the ri
                                                                                                                                                   67 68 74 20 64 61 74 61 20 73 6F 75 72 63 65 20   // ght data source 
                                                                                                                                                   74 6F 20 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50   // to System.Linq.P
                                                                                                                                                   61 72 61 6C 6C 65 6C 51 75 65 72 79 3C 54 3E 2E   // arallelQuery<T>.
                                                                                                                                                   00 00 ) 
    .param type TSource /*2A000030*/ 
      .custom /*0C000081:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x214d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Except

  .method /*0600003C*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Except<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> first,
                          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> second,
                          class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TSource> comparer) cil managed
  // SIG: 10 01 03 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 6D 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C000098:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000099:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000020*/::.ctor(string) /* 0A000013 */ = ( 01 00 81 0C 54 68 65 20 73 65 63 6F 6E 64 20 64   // ....The second d
                                                                                                                                                   61 74 61 20 73 6F 75 72 63 65 20 6F 66 20 61 20   // ata source of a 
                                                                                                                                                   62 69 6E 61 72 79 20 6F 70 65 72 61 74 6F 72 20   // binary operator 
                                                                                                                                                   6D 75 73 74 20 62 65 20 6F 66 20 74 79 70 65 20   // must be of type 
                                                                                                                                                   53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50 61 72 61   // System.Linq.Para
                                                                                                                                                   6C 6C 65 6C 51 75 65 72 79 3C 54 3E 20 72 61 74   // llelQuery<T> rat
                                                                                                                                                   68 65 72 20 74 68 61 6E 20 53 79 73 74 65 6D 2E   // her than System.
                                                                                                                                                   43 6F 6C 6C 65 63 74 69 6F 6E 73 2E 47 65 6E 65   // Collections.Gene
                                                                                                                                                   72 69 63 2E 49 45 6E 75 6D 65 72 61 62 6C 65 3C   // ric.IEnumerable<
                                                                                                                                                   54 3E 2E 20 54 6F 20 66 69 78 20 74 68 69 73 20   // T>. To fix this 
                                                                                                                                                   70 72 6F 62 6C 65 6D 2C 20 75 73 65 20 74 68 65   // problem, use the
                                                                                                                                                   20 41 73 50 61 72 61 6C 6C 65 6C 28 29 20 65 78   //  AsParallel() ex
                                                                                                                                                   74 65 6E 73 69 6F 6E 20 6D 65 74 68 6F 64 20 74   // tension method t
                                                                                                                                                   6F 20 63 6F 6E 76 65 72 74 20 74 68 65 20 72 69   // o convert the ri
                                                                                                                                                   67 68 74 20 64 61 74 61 20 73 6F 75 72 63 65 20   // ght data source 
                                                                                                                                                   74 6F 20 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50   // to System.Linq.P
                                                                                                                                                   61 72 61 6C 6C 65 6C 51 75 65 72 79 3C 54 3E 2E   // arallelQuery<T>.
                                                                                                                                                   00 00 ) 
    .param type TSource /*2A000031*/ 
      .custom /*0C000083:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800005A*/ 
    .custom /*0C0000E1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2150
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Except

  .method /*0600003D*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Except<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> first,
                          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> second) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C00009B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000032*/ 
      .custom /*0C000085:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2153
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Except

  .method /*0600003E*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Except<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> first,
                          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> second,
                          class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TSource> comparer) cil managed
  // SIG: 10 01 03 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C00009D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000033*/ 
      .custom /*0C000087:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800005F*/ 
    .custom /*0C0000EC:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2156
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Except

  .method /*0600003F*/ public hidebysig static 
          !!TSource  FirstOrDefault<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source) cil managed
  // SIG: 10 01 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C00009F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000A0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000060*/ 
    .custom /*0C0000EF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2159
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::FirstOrDefault

  .method /*06000040*/ public hidebysig static 
          !!TSource  FirstOrDefault<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                             class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,bool> predicate) cil managed
  // SIG: 10 01 02 1E 00 15 12 30 01 1E 00 15 12 69 02 1E 00 02
  {
    .custom /*0C0000A2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000035*/ 
      .custom /*0C00008A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000061*/ 
    .custom /*0C0000F2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x215c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::FirstOrDefault

  .method /*06000041*/ public hidebysig static 
          !!TSource  First<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source) cil managed
  // SIG: 10 01 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C0000A4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000036*/ 
      .custom /*0C00008C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x215f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::First

  .method /*06000042*/ public hidebysig static 
          !!TSource  First<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                    class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,bool> predicate) cil managed
  // SIG: 10 01 02 1E 00 15 12 30 01 1E 00 15 12 69 02 1E 00 02
  {
    .custom /*0C0000A6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000037*/ 
      .custom /*0C00008E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2162
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::First

  .method /*06000043*/ public hidebysig static 
          void  ForAll<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                class [System.Runtime/*23000001*/]System.Action`1/*01000022*/<!!TSource> action) cil managed
  // SIG: 10 01 02 01 15 12 30 01 1E 00 15 12 80 89 01 1E 00
  {
    .custom /*0C0000A8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000038*/ 
      .custom /*0C000091:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2165
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ParallelEnumerable::ForAll

  .method /*06000044*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<class [System.Linq/*23000003*/]System.Linq.IGrouping`2/*01000023*/<!!TKey,!!TSource>> 
          GroupBy<TSource,TKey>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector) cil managed
  // SIG: 10 02 02 15 12 30 01 15 12 80 8D 02 1E 01 1E 00 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01
  {
    .custom /*0C0000AA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000039*/ 
      .custom /*0C000093:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A00003A*/ 
      .custom /*0C000094:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2167
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::GroupBy

  .method /*06000045*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<class [System.Linq/*23000003*/]System.Linq.IGrouping`2/*01000023*/<!!TKey,!!TSource>> 
          GroupBy<TSource,TKey>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector,
                                class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TKey> comparer) cil managed
  // SIG: 10 02 03 15 12 30 01 15 12 80 8D 02 1E 01 1E 00 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01 15 12
  //      80 85 01 1E 01
  {
    .custom /*0C0000AC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00003B*/ 
      .custom /*0C000097:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A00003C*/ 
      .custom /*0C00009A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800006D*/ 
    .custom /*0C00010C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x216a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::GroupBy

  .method /*06000046*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<class [System.Linq/*23000003*/]System.Linq.IGrouping`2/*01000023*/<!!TKey,!!TElement>> 
          GroupBy<TSource,TKey,TElement>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                         class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector,
                                         class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TElement> elementSelector) cil managed
  // SIG: 10 03 03 15 12 30 01 15 12 80 8D 02 1E 01 1E 02 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01 15 12
  //      69 02 1E 00 1E 02
  {
    .custom /*0C0000AE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00003D*/ 
      .custom /*0C00009C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A00003E*/ 
      .custom /*0C00009E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TElement /*2A00003F*/ 
      .custom /*0C0000A1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x216d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::GroupBy

  .method /*06000047*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<class [System.Linq/*23000003*/]System.Linq.IGrouping`2/*01000023*/<!!TKey,!!TElement>> 
          GroupBy<TSource,TKey,TElement>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                         class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector,
                                         class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TElement> elementSelector,
                                         class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TKey> comparer) cil managed
  // SIG: 10 03 04 15 12 30 01 15 12 80 8D 02 1E 01 1E 02 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01 15 12
  //      69 02 1E 00 1E 02 15 12 80 85 01 1E 01
  {
    .custom /*0C0000B0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000040*/ 
      .custom /*0C0000A3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000041*/ 
      .custom /*0C0000A5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TElement /*2A000042*/ 
      .custom /*0C0000A7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000074*/ 
    .custom /*0C00011C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2170
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::GroupBy

  .method /*06000048*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          GroupBy<TSource,TKey,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                        class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector,
                                        class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TKey,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource>,!!TResult> resultSelector) cil managed
  // SIG: 10 03 03 15 12 30 01 1E 02 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01 15 12 61 03 1E 01 15 12 6D
  //      01 1E 00 1E 02
  {
    .custom /*0C0000B2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000043*/ 
      .custom /*0C0000A9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000044*/ 
      .custom /*0C0000AB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000045*/ 
      .custom /*0C0000AD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2173
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::GroupBy

  .method /*06000049*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          GroupBy<TSource,TKey,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                        class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector,
                                        class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TKey,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource>,!!TResult> resultSelector,
                                        class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TKey> comparer) cil managed
  // SIG: 10 03 04 15 12 30 01 1E 02 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01 15 12 61 03 1E 01 15 12 6D
  //      01 1E 00 1E 02 15 12 80 85 01 1E 01
  {
    .custom /*0C0000B5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000046*/ 
      .custom /*0C0000AF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000047*/ 
      .custom /*0C0000B1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000048*/ 
      .custom /*0C0000B4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800007B*/ 
    .custom /*0C00012A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2176
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::GroupBy

  .method /*0600004A*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          GroupBy<TSource,TKey,TElement,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                                 class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector,
                                                 class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TElement> elementSelector,
                                                 class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TKey,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TElement>,!!TResult> resultSelector) cil managed
  // SIG: 10 04 04 15 12 30 01 1E 03 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01 15 12 69 02 1E 00 1E 02 15
  //      12 61 03 1E 01 15 12 6D 01 1E 02 1E 03
  {
    .custom /*0C0000B7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000049*/ 
      .custom /*0C0000B6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A00004A*/ 
      .custom /*0C0000B8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TElement /*2A00004B*/ 
      .custom /*0C0000BA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A00004C*/ 
      .custom /*0C0000BD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2179
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::GroupBy

  .method /*0600004B*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          GroupBy<TSource,TKey,TElement,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                                 class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector,
                                                 class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TElement> elementSelector,
                                                 class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TKey,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TElement>,!!TResult> resultSelector,
                                                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TKey> comparer) cil managed
  // SIG: 10 04 05 15 12 30 01 1E 03 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01 15 12 69 02 1E 00 1E 02 15
  //      12 61 03 1E 01 15 12 6D 01 1E 02 1E 03 15 12 80 85 01 1E 01
  {
    .custom /*0C0000B9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00004D*/ 
      .custom /*0C0000C0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A00004E*/ 
      .custom /*0C0000C2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TElement /*2A00004F*/ 
      .custom /*0C0000C4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000050*/ 
      .custom /*0C0000C7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000084*/ 
    .custom /*0C00013D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x217c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::GroupBy

  .method /*0600004C*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          GroupJoin<TOuter,TInner,TKey,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TOuter> outer,
                                                class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TInner> inner,
                                                class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TOuter,!!TKey> outerKeySelector,
                                                class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TInner,!!TKey> innerKeySelector,
                                                class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TOuter,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TInner>,!!TResult> resultSelector) cil managed
  // SIG: 10 04 05 15 12 30 01 1E 03 15 12 30 01 1E 00 15 12 6D 01 1E 01 15 12 69 02 1E 00 1E 02 15 12 69
  //      02 1E 01 1E 02 15 12 61 03 1E 00 15 12 6D 01 1E 01 1E 03
  {
    .custom /*0C0000BB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000BC:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000020*/::.ctor(string) /* 0A000013 */ = ( 01 00 81 0C 54 68 65 20 73 65 63 6F 6E 64 20 64   // ....The second d
                                                                                                                                                   61 74 61 20 73 6F 75 72 63 65 20 6F 66 20 61 20   // ata source of a 
                                                                                                                                                   62 69 6E 61 72 79 20 6F 70 65 72 61 74 6F 72 20   // binary operator 
                                                                                                                                                   6D 75 73 74 20 62 65 20 6F 66 20 74 79 70 65 20   // must be of type 
                                                                                                                                                   53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50 61 72 61   // System.Linq.Para
                                                                                                                                                   6C 6C 65 6C 51 75 65 72 79 3C 54 3E 20 72 61 74   // llelQuery<T> rat
                                                                                                                                                   68 65 72 20 74 68 61 6E 20 53 79 73 74 65 6D 2E   // her than System.
                                                                                                                                                   43 6F 6C 6C 65 63 74 69 6F 6E 73 2E 47 65 6E 65   // Collections.Gene
                                                                                                                                                   72 69 63 2E 49 45 6E 75 6D 65 72 61 62 6C 65 3C   // ric.IEnumerable<
                                                                                                                                                   54 3E 2E 20 54 6F 20 66 69 78 20 74 68 69 73 20   // T>. To fix this 
                                                                                                                                                   70 72 6F 62 6C 65 6D 2C 20 75 73 65 20 74 68 65   // problem, use the
                                                                                                                                                   20 41 73 50 61 72 61 6C 6C 65 6C 28 29 20 65 78   //  AsParallel() ex
                                                                                                                                                   74 65 6E 73 69 6F 6E 20 6D 65 74 68 6F 64 20 74   // tension method t
                                                                                                                                                   6F 20 63 6F 6E 76 65 72 74 20 74 68 65 20 72 69   // o convert the ri
                                                                                                                                                   67 68 74 20 64 61 74 61 20 73 6F 75 72 63 65 20   // ght data source 
                                                                                                                                                   74 6F 20 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50   // to System.Linq.P
                                                                                                                                                   61 72 61 6C 6C 65 6C 51 75 65 72 79 3C 54 3E 2E   // arallelQuery<T>.
                                                                                                                                                   00 00 ) 
    .param type TOuter /*2A000051*/ 
      .custom /*0C0000CB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TInner /*2A000052*/ 
      .custom /*0C0000CE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000053*/ 
      .custom /*0C0000D0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000054*/ 
      .custom /*0C0000D3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x217f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::GroupJoin

  .method /*0600004D*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          GroupJoin<TOuter,TInner,TKey,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TOuter> outer,
                                                class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TInner> inner,
                                                class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TOuter,!!TKey> outerKeySelector,
                                                class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TInner,!!TKey> innerKeySelector,
                                                class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TOuter,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TInner>,!!TResult> resultSelector,
                                                class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TKey> comparer) cil managed
  // SIG: 10 04 06 15 12 30 01 1E 03 15 12 30 01 1E 00 15 12 6D 01 1E 01 15 12 69 02 1E 00 1E 02 15 12 69
  //      02 1E 01 1E 02 15 12 61 03 1E 00 15 12 6D 01 1E 01 1E 03 15 12 80 85 01 1E 02
  {
    .custom /*0C0000BE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000BF:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000020*/::.ctor(string) /* 0A000013 */ = ( 01 00 81 0C 54 68 65 20 73 65 63 6F 6E 64 20 64   // ....The second d
                                                                                                                                                   61 74 61 20 73 6F 75 72 63 65 20 6F 66 20 61 20   // ata source of a 
                                                                                                                                                   62 69 6E 61 72 79 20 6F 70 65 72 61 74 6F 72 20   // binary operator 
                                                                                                                                                   6D 75 73 74 20 62 65 20 6F 66 20 74 79 70 65 20   // must be of type 
                                                                                                                                                   53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50 61 72 61   // System.Linq.Para
                                                                                                                                                   6C 6C 65 6C 51 75 65 72 79 3C 54 3E 20 72 61 74   // llelQuery<T> rat
                                                                                                                                                   68 65 72 20 74 68 61 6E 20 53 79 73 74 65 6D 2E   // her than System.
                                                                                                                                                   43 6F 6C 6C 65 63 74 69 6F 6E 73 2E 47 65 6E 65   // Collections.Gene
                                                                                                                                                   72 69 63 2E 49 45 6E 75 6D 65 72 61 62 6C 65 3C   // ric.IEnumerable<
                                                                                                                                                   54 3E 2E 20 54 6F 20 66 69 78 20 74 68 69 73 20   // T>. To fix this 
                                                                                                                                                   70 72 6F 62 6C 65 6D 2C 20 75 73 65 20 74 68 65   // problem, use the
                                                                                                                                                   20 41 73 50 61 72 61 6C 6C 65 6C 28 29 20 65 78   //  AsParallel() ex
                                                                                                                                                   74 65 6E 73 69 6F 6E 20 6D 65 74 68 6F 64 20 74   // tension method t
                                                                                                                                                   6F 20 63 6F 6E 76 65 72 74 20 74 68 65 20 72 69   // o convert the ri
                                                                                                                                                   67 68 74 20 64 61 74 61 20 73 6F 75 72 63 65 20   // ght data source 
                                                                                                                                                   74 6F 20 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50   // to System.Linq.P
                                                                                                                                                   61 72 61 6C 6C 65 6C 51 75 65 72 79 3C 54 3E 2E   // arallelQuery<T>.
                                                                                                                                                   00 00 ) 
    .param type TOuter /*2A000055*/ 
      .custom /*0C0000D6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TInner /*2A000056*/ 
      .custom /*0C0000D8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000057*/ 
      .custom /*0C0000DA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000058*/ 
      .custom /*0C0000DD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [6]/*0800008F*/ 
    .custom /*0C000151:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2182
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::GroupJoin

  .method /*0600004E*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          GroupJoin<TOuter,TInner,TKey,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TOuter> outer,
                                                class System.Linq.ParallelQuery`1/*0200000C*/<!!TInner> inner,
                                                class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TOuter,!!TKey> outerKeySelector,
                                                class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TInner,!!TKey> innerKeySelector,
                                                class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TOuter,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TInner>,!!TResult> resultSelector) cil managed
  // SIG: 10 04 05 15 12 30 01 1E 03 15 12 30 01 1E 00 15 12 30 01 1E 01 15 12 69 02 1E 00 1E 02 15 12 69
  //      02 1E 01 1E 02 15 12 61 03 1E 00 15 12 6D 01 1E 01 1E 03
  {
    .custom /*0C0000C1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOuter /*2A000059*/ 
      .custom /*0C0000DF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TInner /*2A00005A*/ 
      .custom /*0C0000E2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A00005B*/ 
      .custom /*0C0000E4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A00005C*/ 
      .custom /*0C0000E6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2185
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::GroupJoin

  .method /*0600004F*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          GroupJoin<TOuter,TInner,TKey,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TOuter> outer,
                                                class System.Linq.ParallelQuery`1/*0200000C*/<!!TInner> inner,
                                                class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TOuter,!!TKey> outerKeySelector,
                                                class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TInner,!!TKey> innerKeySelector,
                                                class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TOuter,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TInner>,!!TResult> resultSelector,
                                                class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TKey> comparer) cil managed
  // SIG: 10 04 06 15 12 30 01 1E 03 15 12 30 01 1E 00 15 12 30 01 1E 01 15 12 69 02 1E 00 1E 02 15 12 69
  //      02 1E 01 1E 02 15 12 61 03 1E 00 15 12 6D 01 1E 01 1E 03 15 12 80 85 01 1E 02
  {
    .custom /*0C0000C3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOuter /*2A00005D*/ 
      .custom /*0C0000E8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TInner /*2A00005E*/ 
      .custom /*0C0000EA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A00005F*/ 
      .custom /*0C0000ED:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000060*/ 
      .custom /*0C0000F0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [6]/*0800009A*/ 
    .custom /*0C000167:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2188
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::GroupJoin

  .method /*06000050*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Intersect<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> first,
                             class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> second) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 6D 01 1E 00
  {
    .custom /*0C0000C5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000C6:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000020*/::.ctor(string) /* 0A000013 */ = ( 01 00 81 0C 54 68 65 20 73 65 63 6F 6E 64 20 64   // ....The second d
                                                                                                                                                   61 74 61 20 73 6F 75 72 63 65 20 6F 66 20 61 20   // ata source of a 
                                                                                                                                                   62 69 6E 61 72 79 20 6F 70 65 72 61 74 6F 72 20   // binary operator 
                                                                                                                                                   6D 75 73 74 20 62 65 20 6F 66 20 74 79 70 65 20   // must be of type 
                                                                                                                                                   53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50 61 72 61   // System.Linq.Para
                                                                                                                                                   6C 6C 65 6C 51 75 65 72 79 3C 54 3E 20 72 61 74   // llelQuery<T> rat
                                                                                                                                                   68 65 72 20 74 68 61 6E 20 53 79 73 74 65 6D 2E   // her than System.
                                                                                                                                                   43 6F 6C 6C 65 63 74 69 6F 6E 73 2E 47 65 6E 65   // Collections.Gene
                                                                                                                                                   72 69 63 2E 49 45 6E 75 6D 65 72 61 62 6C 65 3C   // ric.IEnumerable<
                                                                                                                                                   54 3E 2E 20 54 6F 20 66 69 78 20 74 68 69 73 20   // T>. To fix this 
                                                                                                                                                   70 72 6F 62 6C 65 6D 2C 20 75 73 65 20 74 68 65   // problem, use the
                                                                                                                                                   20 41 73 50 61 72 61 6C 6C 65 6C 28 29 20 65 78   //  AsParallel() ex
                                                                                                                                                   74 65 6E 73 69 6F 6E 20 6D 65 74 68 6F 64 20 74   // tension method t
                                                                                                                                                   6F 20 63 6F 6E 76 65 72 74 20 74 68 65 20 72 69   // o convert the ri
                                                                                                                                                   67 68 74 20 64 61 74 61 20 73 6F 75 72 63 65 20   // ght data source 
                                                                                                                                                   74 6F 20 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50   // to System.Linq.P
                                                                                                                                                   61 72 61 6C 6C 65 6C 51 75 65 72 79 3C 54 3E 2E   // arallelQuery<T>.
                                                                                                                                                   00 00 ) 
    .param type TSource /*2A000061*/ 
      .custom /*0C0000F3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x218b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Intersect

  .method /*06000051*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Intersect<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> first,
                             class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> second,
                             class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TSource> comparer) cil managed
  // SIG: 10 01 03 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 6D 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C0000C8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000C9:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000020*/::.ctor(string) /* 0A000013 */ = ( 01 00 81 0C 54 68 65 20 73 65 63 6F 6E 64 20 64   // ....The second d
                                                                                                                                                   61 74 61 20 73 6F 75 72 63 65 20 6F 66 20 61 20   // ata source of a 
                                                                                                                                                   62 69 6E 61 72 79 20 6F 70 65 72 61 74 6F 72 20   // binary operator 
                                                                                                                                                   6D 75 73 74 20 62 65 20 6F 66 20 74 79 70 65 20   // must be of type 
                                                                                                                                                   53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50 61 72 61   // System.Linq.Para
                                                                                                                                                   6C 6C 65 6C 51 75 65 72 79 3C 54 3E 20 72 61 74   // llelQuery<T> rat
                                                                                                                                                   68 65 72 20 74 68 61 6E 20 53 79 73 74 65 6D 2E   // her than System.
                                                                                                                                                   43 6F 6C 6C 65 63 74 69 6F 6E 73 2E 47 65 6E 65   // Collections.Gene
                                                                                                                                                   72 69 63 2E 49 45 6E 75 6D 65 72 61 62 6C 65 3C   // ric.IEnumerable<
                                                                                                                                                   54 3E 2E 20 54 6F 20 66 69 78 20 74 68 69 73 20   // T>. To fix this 
                                                                                                                                                   70 72 6F 62 6C 65 6D 2C 20 75 73 65 20 74 68 65   // problem, use the
                                                                                                                                                   20 41 73 50 61 72 61 6C 6C 65 6C 28 29 20 65 78   //  AsParallel() ex
                                                                                                                                                   74 65 6E 73 69 6F 6E 20 6D 65 74 68 6F 64 20 74   // tension method t
                                                                                                                                                   6F 20 63 6F 6E 76 65 72 74 20 74 68 65 20 72 69   // o convert the ri
                                                                                                                                                   67 68 74 20 64 61 74 61 20 73 6F 75 72 63 65 20   // ght data source 
                                                                                                                                                   74 6F 20 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50   // to System.Linq.P
                                                                                                                                                   61 72 61 6C 6C 65 6C 51 75 65 72 79 3C 54 3E 2E   // arallelQuery<T>.
                                                                                                                                                   00 00 ) 
    .param type TSource /*2A000062*/ 
      .custom /*0C0000F5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800009F*/ 
    .custom /*0C000173:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x218e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Intersect

  .method /*06000052*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Intersect<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> first,
                             class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> second) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C0000CC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000063*/ 
      .custom /*0C0000F7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2191
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Intersect

  .method /*06000053*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Intersect<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> first,
                             class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> second,
                             class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TSource> comparer) cil managed
  // SIG: 10 01 03 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C0000CF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000064*/ 
      .custom /*0C0000F9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080000A4*/ 
    .custom /*0C00017E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2194
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Intersect

  .method /*06000054*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          Join<TOuter,TInner,TKey,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TOuter> outer,
                                           class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TInner> inner,
                                           class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TOuter,!!TKey> outerKeySelector,
                                           class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TInner,!!TKey> innerKeySelector,
                                           class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TOuter,!!TInner,!!TResult> resultSelector) cil managed
  // SIG: 10 04 05 15 12 30 01 1E 03 15 12 30 01 1E 00 15 12 6D 01 1E 01 15 12 69 02 1E 00 1E 02 15 12 69
  //      02 1E 01 1E 02 15 12 61 03 1E 00 1E 01 1E 03
  {
    .custom /*0C0000D1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000D2:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000020*/::.ctor(string) /* 0A000013 */ = ( 01 00 81 0C 54 68 65 20 73 65 63 6F 6E 64 20 64   // ....The second d
                                                                                                                                                   61 74 61 20 73 6F 75 72 63 65 20 6F 66 20 61 20   // ata source of a 
                                                                                                                                                   62 69 6E 61 72 79 20 6F 70 65 72 61 74 6F 72 20   // binary operator 
                                                                                                                                                   6D 75 73 74 20 62 65 20 6F 66 20 74 79 70 65 20   // must be of type 
                                                                                                                                                   53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50 61 72 61   // System.Linq.Para
                                                                                                                                                   6C 6C 65 6C 51 75 65 72 79 3C 54 3E 20 72 61 74   // llelQuery<T> rat
                                                                                                                                                   68 65 72 20 74 68 61 6E 20 53 79 73 74 65 6D 2E   // her than System.
                                                                                                                                                   43 6F 6C 6C 65 63 74 69 6F 6E 73 2E 47 65 6E 65   // Collections.Gene
                                                                                                                                                   72 69 63 2E 49 45 6E 75 6D 65 72 61 62 6C 65 3C   // ric.IEnumerable<
                                                                                                                                                   54 3E 2E 20 54 6F 20 66 69 78 20 74 68 69 73 20   // T>. To fix this 
                                                                                                                                                   70 72 6F 62 6C 65 6D 2C 20 75 73 65 20 74 68 65   // problem, use the
                                                                                                                                                   20 41 73 50 61 72 61 6C 6C 65 6C 28 29 20 65 78   //  AsParallel() ex
                                                                                                                                                   74 65 6E 73 69 6F 6E 20 6D 65 74 68 6F 64 20 74   // tension method t
                                                                                                                                                   6F 20 63 6F 6E 76 65 72 74 20 74 68 65 20 72 69   // o convert the ri
                                                                                                                                                   67 68 74 20 64 61 74 61 20 73 6F 75 72 63 65 20   // ght data source 
                                                                                                                                                   74 6F 20 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50   // to System.Linq.P
                                                                                                                                                   61 72 61 6C 6C 65 6C 51 75 65 72 79 3C 54 3E 2E   // arallelQuery<T>.
                                                                                                                                                   00 00 ) 
    .param type TOuter /*2A000065*/ 
      .custom /*0C0000FB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TInner /*2A000066*/ 
      .custom /*0C0000FD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000067*/ 
      .custom /*0C0000FF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000068*/ 
      .custom /*0C000102:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2197
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Join

  .method /*06000055*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          Join<TOuter,TInner,TKey,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TOuter> outer,
                                           class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TInner> inner,
                                           class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TOuter,!!TKey> outerKeySelector,
                                           class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TInner,!!TKey> innerKeySelector,
                                           class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TOuter,!!TInner,!!TResult> resultSelector,
                                           class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TKey> comparer) cil managed
  // SIG: 10 04 06 15 12 30 01 1E 03 15 12 30 01 1E 00 15 12 6D 01 1E 01 15 12 69 02 1E 00 1E 02 15 12 69
  //      02 1E 01 1E 02 15 12 61 03 1E 00 1E 01 1E 03 15 12 80 85 01 1E 02
  {
    .custom /*0C0000D4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000D5:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000020*/::.ctor(string) /* 0A000013 */ = ( 01 00 81 0C 54 68 65 20 73 65 63 6F 6E 64 20 64   // ....The second d
                                                                                                                                                   61 74 61 20 73 6F 75 72 63 65 20 6F 66 20 61 20   // ata source of a 
                                                                                                                                                   62 69 6E 61 72 79 20 6F 70 65 72 61 74 6F 72 20   // binary operator 
                                                                                                                                                   6D 75 73 74 20 62 65 20 6F 66 20 74 79 70 65 20   // must be of type 
                                                                                                                                                   53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50 61 72 61   // System.Linq.Para
                                                                                                                                                   6C 6C 65 6C 51 75 65 72 79 3C 54 3E 20 72 61 74   // llelQuery<T> rat
                                                                                                                                                   68 65 72 20 74 68 61 6E 20 53 79 73 74 65 6D 2E   // her than System.
                                                                                                                                                   43 6F 6C 6C 65 63 74 69 6F 6E 73 2E 47 65 6E 65   // Collections.Gene
                                                                                                                                                   72 69 63 2E 49 45 6E 75 6D 65 72 61 62 6C 65 3C   // ric.IEnumerable<
                                                                                                                                                   54 3E 2E 20 54 6F 20 66 69 78 20 74 68 69 73 20   // T>. To fix this 
                                                                                                                                                   70 72 6F 62 6C 65 6D 2C 20 75 73 65 20 74 68 65   // problem, use the
                                                                                                                                                   20 41 73 50 61 72 61 6C 6C 65 6C 28 29 20 65 78   //  AsParallel() ex
                                                                                                                                                   74 65 6E 73 69 6F 6E 20 6D 65 74 68 6F 64 20 74   // tension method t
                                                                                                                                                   6F 20 63 6F 6E 76 65 72 74 20 74 68 65 20 72 69   // o convert the ri
                                                                                                                                                   67 68 74 20 64 61 74 61 20 73 6F 75 72 63 65 20   // ght data source 
                                                                                                                                                   74 6F 20 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50   // to System.Linq.P
                                                                                                                                                   61 72 61 6C 6C 65 6C 51 75 65 72 79 3C 54 3E 2E   // arallelQuery<T>.
                                                                                                                                                   00 00 ) 
    .param type TOuter /*2A000069*/ 
      .custom /*0C000104:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TInner /*2A00006A*/ 
      .custom /*0C000106:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A00006B*/ 
      .custom /*0C000108:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A00006C*/ 
      .custom /*0C00010A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [6]/*080000AF*/ 
    .custom /*0C000195:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x219a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Join

  .method /*06000056*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          Join<TOuter,TInner,TKey,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TOuter> outer,
                                           class System.Linq.ParallelQuery`1/*0200000C*/<!!TInner> inner,
                                           class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TOuter,!!TKey> outerKeySelector,
                                           class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TInner,!!TKey> innerKeySelector,
                                           class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TOuter,!!TInner,!!TResult> resultSelector) cil managed
  // SIG: 10 04 05 15 12 30 01 1E 03 15 12 30 01 1E 00 15 12 30 01 1E 01 15 12 69 02 1E 00 1E 02 15 12 69
  //      02 1E 01 1E 02 15 12 61 03 1E 00 1E 01 1E 03
  {
    .custom /*0C0000D7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOuter /*2A00006D*/ 
      .custom /*0C00010D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TInner /*2A00006E*/ 
      .custom /*0C00010F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A00006F*/ 
      .custom /*0C000111:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000070*/ 
      .custom /*0C000113:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x219d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Join

  .method /*06000057*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          Join<TOuter,TInner,TKey,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TOuter> outer,
                                           class System.Linq.ParallelQuery`1/*0200000C*/<!!TInner> inner,
                                           class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TOuter,!!TKey> outerKeySelector,
                                           class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TInner,!!TKey> innerKeySelector,
                                           class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TOuter,!!TInner,!!TResult> resultSelector,
                                           class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TKey> comparer) cil managed
  // SIG: 10 04 06 15 12 30 01 1E 03 15 12 30 01 1E 00 15 12 30 01 1E 01 15 12 69 02 1E 00 1E 02 15 12 69
  //      02 1E 01 1E 02 15 12 61 03 1E 00 1E 01 1E 03 15 12 80 85 01 1E 02
  {
    .custom /*0C0000D9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOuter /*2A000071*/ 
      .custom /*0C000115:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TInner /*2A000072*/ 
      .custom /*0C000117:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000073*/ 
      .custom /*0C00011A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000074*/ 
      .custom /*0C00011D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [6]/*080000BA*/ 
    .custom /*0C0001AB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x21a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Join

  .method /*06000058*/ public hidebysig static 
          !!TSource  LastOrDefault<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source) cil managed
  // SIG: 10 01 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C0000DB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000DC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*080000BB*/ 
    .custom /*0C0001AE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::LastOrDefault

  .method /*06000059*/ public hidebysig static 
          !!TSource  LastOrDefault<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                            class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,bool> predicate) cil managed
  // SIG: 10 01 02 1E 00 15 12 30 01 1E 00 15 12 69 02 1E 00 02
  {
    .custom /*0C0000DE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000076*/ 
      .custom /*0C000120:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080000BC*/ 
    .custom /*0C0001B1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::LastOrDefault

  .method /*0600005A*/ public hidebysig static 
          !!TSource  Last<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source) cil managed
  // SIG: 10 01 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C0000E0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000077*/ 
      .custom /*0C000122:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Last

  .method /*0600005B*/ public hidebysig static 
          !!TSource  Last<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                   class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,bool> predicate) cil managed
  // SIG: 10 01 02 1E 00 15 12 30 01 1E 00 15 12 69 02 1E 00 02
  {
    .custom /*0C0000E3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000078*/ 
      .custom /*0C000124:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Last

  .method /*0600005C*/ public hidebysig static 
          int64  LongCount<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source) cil managed
  // SIG: 10 01 01 0A 15 12 30 01 1E 00
  {
    .custom /*0C0000E5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000079*/ 
      .custom /*0C000126:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::LongCount

  .method /*0600005D*/ public hidebysig static 
          int64  LongCount<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                    class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,bool> predicate) cil managed
  // SIG: 10 01 02 0A 15 12 30 01 1E 00 15 12 69 02 1E 00 02
  {
    .custom /*0C0000E7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00007A*/ 
      .custom /*0C000128:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::LongCount

  .method /*0600005E*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/ 
          Max(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/> source) cil managed
  // SIG: 00 01 11 79 15 12 30 01 11 79
  {
    .custom /*0C0000E9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*0600005F*/ public hidebysig static 
          float64  Max(class System.Linq.ParallelQuery`1/*0200000C*/<float64> source) cil managed
  // SIG: 00 01 0D 15 12 30 01 0D
  {
    .custom /*0C0000EB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*06000060*/ public hidebysig static 
          int32  Max(class System.Linq.ParallelQuery`1/*0200000C*/<int32> source) cil managed
  // SIG: 00 01 08 15 12 30 01 08
  {
    .custom /*0C0000EE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*06000061*/ public hidebysig static 
          int64  Max(class System.Linq.ParallelQuery`1/*0200000C*/<int64> source) cil managed
  // SIG: 00 01 0A 15 12 30 01 0A
  {
    .custom /*0C0000F1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*06000062*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/> 
          Max(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/>> source) cil managed
  // SIG: 00 01 15 11 7D 01 11 79 15 12 30 01 15 11 7D 01 11 79
  {
    .custom /*0C0000F4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*06000063*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float64> 
          Max(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float64>> source) cil managed
  // SIG: 00 01 15 11 7D 01 0D 15 12 30 01 15 11 7D 01 0D
  {
    .custom /*0C0000F6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*06000064*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int32> 
          Max(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int32>> source) cil managed
  // SIG: 00 01 15 11 7D 01 08 15 12 30 01 15 11 7D 01 08
  {
    .custom /*0C0000F8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*06000065*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int64> 
          Max(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int64>> source) cil managed
  // SIG: 00 01 15 11 7D 01 0A 15 12 30 01 15 11 7D 01 0A
  {
    .custom /*0C0000FA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*06000066*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float32> 
          Max(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float32>> source) cil managed
  // SIG: 00 01 15 11 7D 01 0C 15 12 30 01 15 11 7D 01 0C
  {
    .custom /*0C0000FC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*06000067*/ public hidebysig static 
          float32  Max(class System.Linq.ParallelQuery`1/*0200000C*/<float32> source) cil managed
  // SIG: 00 01 0C 15 12 30 01 0C
  {
    .custom /*0C0000FE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*06000068*/ public hidebysig static 
          !!TSource  Max<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source) cil managed
  // SIG: 10 01 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C000100:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000101:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*080000CF*/ 
    .custom /*0C0001DA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*06000069*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/ 
          Max<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                       class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/> selector) cil managed
  // SIG: 10 01 02 11 79 15 12 30 01 1E 00 15 12 69 02 1E 00 11 79
  {
    .custom /*0C000103:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00007C*/ 
      .custom /*0C00012C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*0600006A*/ public hidebysig static 
          float64  Max<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,float64> selector) cil managed
  // SIG: 10 01 02 0D 15 12 30 01 1E 00 15 12 69 02 1E 00 0D
  {
    .custom /*0C000105:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00007D*/ 
      .custom /*0C00012E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*0600006B*/ public hidebysig static 
          int32  Max<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                              class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,int32> selector) cil managed
  // SIG: 10 01 02 08 15 12 30 01 1E 00 15 12 69 02 1E 00 08
  {
    .custom /*0C000107:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00007E*/ 
      .custom /*0C000131:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*0600006C*/ public hidebysig static 
          int64  Max<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                              class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,int64> selector) cil managed
  // SIG: 10 01 02 0A 15 12 30 01 1E 00 15 12 69 02 1E 00 0A
  {
    .custom /*0C000109:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00007F*/ 
      .custom /*0C000133:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*0600006D*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/> 
          Max<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                       class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/>> selector) cil managed
  // SIG: 10 01 02 15 11 7D 01 11 79 15 12 30 01 1E 00 15 12 69 02 1E 00 15 11 7D 01 11 79
  {
    .custom /*0C00010B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000080*/ 
      .custom /*0C000135:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*0600006E*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float64> 
          Max<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                       class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float64>> selector) cil managed
  // SIG: 10 01 02 15 11 7D 01 0D 15 12 30 01 1E 00 15 12 69 02 1E 00 15 11 7D 01 0D
  {
    .custom /*0C00010E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000081*/ 
      .custom /*0C000137:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*0600006F*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int32> 
          Max<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                       class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int32>> selector) cil managed
  // SIG: 10 01 02 15 11 7D 01 08 15 12 30 01 1E 00 15 12 69 02 1E 00 15 11 7D 01 08
  {
    .custom /*0C000110:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000082*/ 
      .custom /*0C000139:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*06000070*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int64> 
          Max<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                       class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int64>> selector) cil managed
  // SIG: 10 01 02 15 11 7D 01 0A 15 12 30 01 1E 00 15 12 69 02 1E 00 15 11 7D 01 0A
  {
    .custom /*0C000112:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000083*/ 
      .custom /*0C00013B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*06000071*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float32> 
          Max<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                       class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float32>> selector) cil managed
  // SIG: 10 01 02 15 11 7D 01 0C 15 12 30 01 1E 00 15 12 69 02 1E 00 15 11 7D 01 0C
  {
    .custom /*0C000114:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000084*/ 
      .custom /*0C00013E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*06000072*/ public hidebysig static 
          float32  Max<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,float32> selector) cil managed
  // SIG: 10 01 02 0C 15 12 30 01 1E 00 15 12 69 02 1E 00 0C
  {
    .custom /*0C000116:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000085*/ 
      .custom /*0C000140:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*06000073*/ public hidebysig static 
          !!TResult  Max<TSource,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                          class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TResult> selector) cil managed
  // SIG: 10 02 02 1E 01 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01
  {
    .custom /*0C000118:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000119:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*080000E4*/ 
    .custom /*0C0001EE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000E5*/ 
    .custom /*0C0001F0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Max

  .method /*06000074*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/ 
          Min(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/> source) cil managed
  // SIG: 00 01 11 79 15 12 30 01 11 79
  {
    .custom /*0C00011B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*06000075*/ public hidebysig static 
          float64  Min(class System.Linq.ParallelQuery`1/*0200000C*/<float64> source) cil managed
  // SIG: 00 01 0D 15 12 30 01 0D
  {
    .custom /*0C00011E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*06000076*/ public hidebysig static 
          int32  Min(class System.Linq.ParallelQuery`1/*0200000C*/<int32> source) cil managed
  // SIG: 00 01 08 15 12 30 01 08
  {
    .custom /*0C00011F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*06000077*/ public hidebysig static 
          int64  Min(class System.Linq.ParallelQuery`1/*0200000C*/<int64> source) cil managed
  // SIG: 00 01 0A 15 12 30 01 0A
  {
    .custom /*0C000121:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2200
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*06000078*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/> 
          Min(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/>> source) cil managed
  // SIG: 00 01 15 11 7D 01 11 79 15 12 30 01 15 11 7D 01 11 79
  {
    .custom /*0C000123:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2203
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*06000079*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float64> 
          Min(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float64>> source) cil managed
  // SIG: 00 01 15 11 7D 01 0D 15 12 30 01 15 11 7D 01 0D
  {
    .custom /*0C000125:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2206
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*0600007A*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int32> 
          Min(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int32>> source) cil managed
  // SIG: 00 01 15 11 7D 01 08 15 12 30 01 15 11 7D 01 08
  {
    .custom /*0C000127:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2209
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*0600007B*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int64> 
          Min(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int64>> source) cil managed
  // SIG: 00 01 15 11 7D 01 0A 15 12 30 01 15 11 7D 01 0A
  {
    .custom /*0C000129:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x220c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*0600007C*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float32> 
          Min(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float32>> source) cil managed
  // SIG: 00 01 15 11 7D 01 0C 15 12 30 01 15 11 7D 01 0C
  {
    .custom /*0C00012B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x220f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*0600007D*/ public hidebysig static 
          float32  Min(class System.Linq.ParallelQuery`1/*0200000C*/<float32> source) cil managed
  // SIG: 00 01 0C 15 12 30 01 0C
  {
    .custom /*0C00012D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2212
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*0600007E*/ public hidebysig static 
          !!TSource  Min<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source) cil managed
  // SIG: 10 01 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C00012F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000130:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*080000F0*/ 
    .custom /*0C0001FC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2215
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*0600007F*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/ 
          Min<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                       class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/> selector) cil managed
  // SIG: 10 01 02 11 79 15 12 30 01 1E 00 15 12 69 02 1E 00 11 79
  {
    .custom /*0C000132:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000089*/ 
      .custom /*0C000146:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2218
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*06000080*/ public hidebysig static 
          float64  Min<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,float64> selector) cil managed
  // SIG: 10 01 02 0D 15 12 30 01 1E 00 15 12 69 02 1E 00 0D
  {
    .custom /*0C000134:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00008A*/ 
      .custom /*0C000148:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x221b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*06000081*/ public hidebysig static 
          int32  Min<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                              class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,int32> selector) cil managed
  // SIG: 10 01 02 08 15 12 30 01 1E 00 15 12 69 02 1E 00 08
  {
    .custom /*0C000136:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00008B*/ 
      .custom /*0C00014A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x221e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*06000082*/ public hidebysig static 
          int64  Min<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                              class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,int64> selector) cil managed
  // SIG: 10 01 02 0A 15 12 30 01 1E 00 15 12 69 02 1E 00 0A
  {
    .custom /*0C000138:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00008C*/ 
      .custom /*0C00014C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2221
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*06000083*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/> 
          Min<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                       class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/>> selector) cil managed
  // SIG: 10 01 02 15 11 7D 01 11 79 15 12 30 01 1E 00 15 12 69 02 1E 00 15 11 7D 01 11 79
  {
    .custom /*0C00013A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00008D*/ 
      .custom /*0C00014E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2224
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*06000084*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float64> 
          Min<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                       class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float64>> selector) cil managed
  // SIG: 10 01 02 15 11 7D 01 0D 15 12 30 01 1E 00 15 12 69 02 1E 00 15 11 7D 01 0D
  {
    .custom /*0C00013C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00008E*/ 
      .custom /*0C000150:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2227
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*06000085*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int32> 
          Min<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                       class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int32>> selector) cil managed
  // SIG: 10 01 02 15 11 7D 01 08 15 12 30 01 1E 00 15 12 69 02 1E 00 15 11 7D 01 08
  {
    .custom /*0C00013F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00008F*/ 
      .custom /*0C000152:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x222a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*06000086*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int64> 
          Min<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                       class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int64>> selector) cil managed
  // SIG: 10 01 02 15 11 7D 01 0A 15 12 30 01 1E 00 15 12 69 02 1E 00 15 11 7D 01 0A
  {
    .custom /*0C000141:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000090*/ 
      .custom /*0C000153:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x222d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*06000087*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float32> 
          Min<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                       class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float32>> selector) cil managed
  // SIG: 10 01 02 15 11 7D 01 0C 15 12 30 01 1E 00 15 12 69 02 1E 00 15 11 7D 01 0C
  {
    .custom /*0C000142:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000091*/ 
      .custom /*0C000155:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2230
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*06000088*/ public hidebysig static 
          float32  Min<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,float32> selector) cil managed
  // SIG: 10 01 02 0C 15 12 30 01 1E 00 15 12 69 02 1E 00 0C
  {
    .custom /*0C000143:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000092*/ 
      .custom /*0C000157:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2233
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*06000089*/ public hidebysig static 
          !!TResult  Min<TSource,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                          class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TResult> selector) cil managed
  // SIG: 10 02 02 1E 01 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01
  {
    .custom /*0C000144:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000145:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000105*/ 
    .custom /*0C000201:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000106*/ 
    .custom /*0C000202:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2236
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Min

  .method /*0600008A*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          OfType<TResult>(class System.Linq.ParallelQuery/*0200000B*/ source) cil managed
  // SIG: 10 01 01 15 12 30 01 1E 00 12 2C
  {
    .custom /*0C000147:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TResult /*2A000095*/ 
      .custom /*0C00015B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2239
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::OfType

  .method /*0600008B*/ public hidebysig static 
          class System.Linq.OrderedParallelQuery`1/*02000007*/<!!TSource> 
          OrderByDescending<TSource,TKey>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                          class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector) cil managed
  // SIG: 10 02 02 15 12 1C 01 1E 00 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01
  {
    .custom /*0C000149:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000096*/ 
      .custom /*0C00015D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000097*/ 
      .custom /*0C00015F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x223c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::OrderByDescending

  .method /*0600008C*/ public hidebysig static 
          class System.Linq.OrderedParallelQuery`1/*02000007*/<!!TSource> 
          OrderByDescending<TSource,TKey>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                          class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector,
                                          class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*01000024*/<!!TKey> comparer) cil managed
  // SIG: 10 02 03 15 12 1C 01 1E 00 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01 15 12 80 91 01 1E 01
  {
    .custom /*0C00014B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A000098*/ 
      .custom /*0C000162:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A000099*/ 
      .custom /*0C000165:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800010C*/ 
    .custom /*0C000203:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x223f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::OrderByDescending

  .method /*0600008D*/ public hidebysig static 
          class System.Linq.OrderedParallelQuery`1/*02000007*/<!!TSource> 
          OrderBy<TSource,TKey>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector) cil managed
  // SIG: 10 02 02 15 12 1C 01 1E 00 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01
  {
    .custom /*0C00014D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00009A*/ 
      .custom /*0C000168:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A00009B*/ 
      .custom /*0C00016A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2242
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::OrderBy

  .method /*0600008E*/ public hidebysig static 
          class System.Linq.OrderedParallelQuery`1/*02000007*/<!!TSource> 
          OrderBy<TSource,TKey>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector,
                                class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*01000024*/<!!TKey> comparer) cil managed
  // SIG: 10 02 03 15 12 1C 01 1E 00 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01 15 12 80 91 01 1E 01
  {
    .custom /*0C00014F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00009C*/ 
      .custom /*0C00016D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A00009D*/ 
      .custom /*0C00016F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000111*/ 
    .custom /*0C000204:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2245
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::OrderBy

  .method /*0600008F*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<int32> 
          Range(int32 start,
                int32 count) cil managed
  // SIG: 00 02 15 12 30 01 08 08 08
  {
    // Method begins at RVA 0x2248
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Range

  .method /*06000090*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          Repeat<TResult>(!!TResult element,
                          int32 count) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 1E 00 08
  {
    .param type TResult /*2A00009E*/ 
      .custom /*0C000171:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x224b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Repeat

  .method /*06000091*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Reverse<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source) cil managed
  // SIG: 10 01 01 15 12 30 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C000154:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A00009F*/ 
      .custom /*0C000174:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x224e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Reverse

  .method /*06000092*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          SelectMany<TSource,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                      class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TResult>> selector) cil managed
  // SIG: 10 02 02 15 12 30 01 1E 01 15 12 30 01 1E 00 15 12 69 02 1E 00 15 12 6D 01 1E 01
  {
    .custom /*0C000156:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000A0*/ 
      .custom /*0C000176:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A0000A1*/ 
      .custom /*0C000178:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2251
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::SelectMany

  .method /*06000093*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          SelectMany<TSource,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                      class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TSource,int32,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TResult>> selector) cil managed
  // SIG: 10 02 02 15 12 30 01 1E 01 15 12 30 01 1E 00 15 12 61 03 1E 00 08 15 12 6D 01 1E 01
  {
    .custom /*0C000158:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000A2*/ 
      .custom /*0C00017A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A0000A3*/ 
      .custom /*0C00017C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2254
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::SelectMany

  .method /*06000094*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          SelectMany<TSource,TCollection,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                                  class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TCollection>> collectionSelector,
                                                  class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TSource,!!TCollection,!!TResult> resultSelector) cil managed
  // SIG: 10 03 03 15 12 30 01 1E 02 15 12 30 01 1E 00 15 12 69 02 1E 00 15 12 6D 01 1E 01 15 12 61 03 1E
  //      00 1E 01 1E 02
  {
    .custom /*0C000159:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000A4*/ 
      .custom /*0C00017F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TCollection /*2A0000A5*/ 
      .custom /*0C000181:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A0000A6*/ 
      .custom /*0C000183:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2257
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::SelectMany

  .method /*06000095*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          SelectMany<TSource,TCollection,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                                  class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TSource,int32,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TCollection>> collectionSelector,
                                                  class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TSource,!!TCollection,!!TResult> resultSelector) cil managed
  // SIG: 10 03 03 15 12 30 01 1E 02 15 12 30 01 1E 00 15 12 61 03 1E 00 08 15 12 6D 01 1E 01 15 12 61 03
  //      1E 00 1E 01 1E 02
  {
    .custom /*0C00015A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000A7*/ 
      .custom /*0C000185:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TCollection /*2A0000A8*/ 
      .custom /*0C000187:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A0000A9*/ 
      .custom /*0C000189:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x225a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::SelectMany

  .method /*06000096*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          Select<TSource,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                  class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TSource,int32,!!TResult> selector) cil managed
  // SIG: 10 02 02 15 12 30 01 1E 01 15 12 30 01 1E 00 15 12 61 03 1E 00 08 1E 01
  {
    .custom /*0C00015C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000AA*/ 
      .custom /*0C00018B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A0000AB*/ 
      .custom /*0C00018D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x225d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Select

  .method /*06000097*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          Select<TSource,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                  class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TResult> selector) cil managed
  // SIG: 10 02 02 15 12 30 01 1E 01 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01
  {
    .custom /*0C00015E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000AC*/ 
      .custom /*0C00018F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A0000AD*/ 
      .custom /*0C000191:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2260
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Select

  .method /*06000098*/ public hidebysig static 
          bool  SequenceEqual<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> first,
                                       class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> second) cil managed
  // SIG: 10 01 02 02 15 12 30 01 1E 00 15 12 6D 01 1E 00
  {
    .custom /*0C000160:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000161:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000020*/::.ctor(string) /* 0A000013 */ = ( 01 00 81 0C 54 68 65 20 73 65 63 6F 6E 64 20 64   // ....The second d
                                                                                                                                                   61 74 61 20 73 6F 75 72 63 65 20 6F 66 20 61 20   // ata source of a 
                                                                                                                                                   62 69 6E 61 72 79 20 6F 70 65 72 61 74 6F 72 20   // binary operator 
                                                                                                                                                   6D 75 73 74 20 62 65 20 6F 66 20 74 79 70 65 20   // must be of type 
                                                                                                                                                   53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50 61 72 61   // System.Linq.Para
                                                                                                                                                   6C 6C 65 6C 51 75 65 72 79 3C 54 3E 20 72 61 74   // llelQuery<T> rat
                                                                                                                                                   68 65 72 20 74 68 61 6E 20 53 79 73 74 65 6D 2E   // her than System.
                                                                                                                                                   43 6F 6C 6C 65 63 74 69 6F 6E 73 2E 47 65 6E 65   // Collections.Gene
                                                                                                                                                   72 69 63 2E 49 45 6E 75 6D 65 72 61 62 6C 65 3C   // ric.IEnumerable<
                                                                                                                                                   54 3E 2E 20 54 6F 20 66 69 78 20 74 68 69 73 20   // T>. To fix this 
                                                                                                                                                   70 72 6F 62 6C 65 6D 2C 20 75 73 65 20 74 68 65   // problem, use the
                                                                                                                                                   20 41 73 50 61 72 61 6C 6C 65 6C 28 29 20 65 78   //  AsParallel() ex
                                                                                                                                                   74 65 6E 73 69 6F 6E 20 6D 65 74 68 6F 64 20 74   // tension method t
                                                                                                                                                   6F 20 63 6F 6E 76 65 72 74 20 74 68 65 20 72 69   // o convert the ri
                                                                                                                                                   67 68 74 20 64 61 74 61 20 73 6F 75 72 63 65 20   // ght data source 
                                                                                                                                                   74 6F 20 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50   // to System.Linq.P
                                                                                                                                                   61 72 61 6C 6C 65 6C 51 75 65 72 79 3C 54 3E 2E   // arallelQuery<T>.
                                                                                                                                                   00 00 ) 
    .param type TSource /*2A0000AE*/ 
      .custom /*0C000193:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2263
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::SequenceEqual

  .method /*06000099*/ public hidebysig static 
          bool  SequenceEqual<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> first,
                                       class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> second,
                                       class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TSource> comparer) cil managed
  // SIG: 10 01 03 02 15 12 30 01 1E 00 15 12 6D 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C000163:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C000164:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000020*/::.ctor(string) /* 0A000013 */ = ( 01 00 81 0C 54 68 65 20 73 65 63 6F 6E 64 20 64   // ....The second d
                                                                                                                                                   61 74 61 20 73 6F 75 72 63 65 20 6F 66 20 61 20   // ata source of a 
                                                                                                                                                   62 69 6E 61 72 79 20 6F 70 65 72 61 74 6F 72 20   // binary operator 
                                                                                                                                                   6D 75 73 74 20 62 65 20 6F 66 20 74 79 70 65 20   // must be of type 
                                                                                                                                                   53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50 61 72 61   // System.Linq.Para
                                                                                                                                                   6C 6C 65 6C 51 75 65 72 79 3C 54 3E 20 72 61 74   // llelQuery<T> rat
                                                                                                                                                   68 65 72 20 74 68 61 6E 20 53 79 73 74 65 6D 2E   // her than System.
                                                                                                                                                   43 6F 6C 6C 65 63 74 69 6F 6E 73 2E 47 65 6E 65   // Collections.Gene
                                                                                                                                                   72 69 63 2E 49 45 6E 75 6D 65 72 61 62 6C 65 3C   // ric.IEnumerable<
                                                                                                                                                   54 3E 2E 20 54 6F 20 66 69 78 20 74 68 69 73 20   // T>. To fix this 
                                                                                                                                                   70 72 6F 62 6C 65 6D 2C 20 75 73 65 20 74 68 65   // problem, use the
                                                                                                                                                   20 41 73 50 61 72 61 6C 6C 65 6C 28 29 20 65 78   //  AsParallel() ex
                                                                                                                                                   74 65 6E 73 69 6F 6E 20 6D 65 74 68 6F 64 20 74   // tension method t
                                                                                                                                                   6F 20 63 6F 6E 76 65 72 74 20 74 68 65 20 72 69   // o convert the ri
                                                                                                                                                   67 68 74 20 64 61 74 61 20 73 6F 75 72 63 65 20   // ght data source 
                                                                                                                                                   74 6F 20 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50   // to System.Linq.P
                                                                                                                                                   61 72 61 6C 6C 65 6C 51 75 65 72 79 3C 54 3E 2E   // arallelQuery<T>.
                                                                                                                                                   00 00 ) 
    .param type TSource /*2A0000AF*/ 
      .custom /*0C000196:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000129*/ 
    .custom /*0C000205:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2266
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::SequenceEqual

  .method /*0600009A*/ public hidebysig static 
          bool  SequenceEqual<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> first,
                                       class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> second) cil managed
  // SIG: 10 01 02 02 15 12 30 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C000166:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000B0*/ 
      .custom /*0C000198:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2269
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::SequenceEqual

  .method /*0600009B*/ public hidebysig static 
          bool  SequenceEqual<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> first,
                                       class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> second,
                                       class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TSource> comparer) cil managed
  // SIG: 10 01 03 02 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C000169:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000B1*/ 
      .custom /*0C00019A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800012E*/ 
    .custom /*0C000206:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x226c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::SequenceEqual

  .method /*0600009C*/ public hidebysig static 
          !!TSource  SingleOrDefault<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source) cil managed
  // SIG: 10 01 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C00016B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00016C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*0800012F*/ 
    .custom /*0C000207:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x226f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::SingleOrDefault

  .method /*0600009D*/ public hidebysig static 
          !!TSource  SingleOrDefault<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                              class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,bool> predicate) cil managed
  // SIG: 10 01 02 1E 00 15 12 30 01 1E 00 15 12 69 02 1E 00 02
  {
    .custom /*0C00016E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000B3*/ 
      .custom /*0C00019D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000130*/ 
    .custom /*0C000208:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2272
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::SingleOrDefault

  .method /*0600009E*/ public hidebysig static 
          !!TSource  Single<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source) cil managed
  // SIG: 10 01 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C000170:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000B4*/ 
      .custom /*0C00019F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2275
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Single

  .method /*0600009F*/ public hidebysig static 
          !!TSource  Single<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                     class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,bool> predicate) cil managed
  // SIG: 10 01 02 1E 00 15 12 30 01 1E 00 15 12 69 02 1E 00 02
  {
    .custom /*0C000172:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000B5*/ 
      .custom /*0C0001A1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2278
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Single

  .method /*060000A0*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          SkipWhile<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                             class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,bool> predicate) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 69 02 1E 00 02
  {
    .custom /*0C000175:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000B6*/ 
      .custom /*0C0001A3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x227b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::SkipWhile

  .method /*060000A1*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          SkipWhile<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                             class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TSource,int32,bool> predicate) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 61 03 1E 00 08 02
  {
    .custom /*0C000177:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000B7*/ 
      .custom /*0C0001A5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x227e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::SkipWhile

  .method /*060000A2*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Skip<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                        int32 count) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 08
  {
    .custom /*0C000179:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000B8*/ 
      .custom /*0C0001A7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2281
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Skip

  .method /*060000A3*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/ 
          Sum(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/> source) cil managed
  // SIG: 00 01 11 79 15 12 30 01 11 79
  {
    .custom /*0C00017B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2284
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Sum

  .method /*060000A4*/ public hidebysig static 
          float64  Sum(class System.Linq.ParallelQuery`1/*0200000C*/<float64> source) cil managed
  // SIG: 00 01 0D 15 12 30 01 0D
  {
    .custom /*0C00017D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2287
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Sum

  .method /*060000A5*/ public hidebysig static 
          int32  Sum(class System.Linq.ParallelQuery`1/*0200000C*/<int32> source) cil managed
  // SIG: 00 01 08 15 12 30 01 08
  {
    .custom /*0C000180:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x228a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Sum

  .method /*060000A6*/ public hidebysig static 
          int64  Sum(class System.Linq.ParallelQuery`1/*0200000C*/<int64> source) cil managed
  // SIG: 00 01 0A 15 12 30 01 0A
  {
    .custom /*0C000182:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x228d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Sum

  .method /*060000A7*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/> 
          Sum(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/>> source) cil managed
  // SIG: 00 01 15 11 7D 01 11 79 15 12 30 01 15 11 7D 01 11 79
  {
    .custom /*0C000184:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2290
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Sum

  .method /*060000A8*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float64> 
          Sum(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float64>> source) cil managed
  // SIG: 00 01 15 11 7D 01 0D 15 12 30 01 15 11 7D 01 0D
  {
    .custom /*0C000186:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2293
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Sum

  .method /*060000A9*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int32> 
          Sum(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int32>> source) cil managed
  // SIG: 00 01 15 11 7D 01 08 15 12 30 01 15 11 7D 01 08
  {
    .custom /*0C000188:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2296
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Sum

  .method /*060000AA*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int64> 
          Sum(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int64>> source) cil managed
  // SIG: 00 01 15 11 7D 01 0A 15 12 30 01 15 11 7D 01 0A
  {
    .custom /*0C00018A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2299
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Sum

  .method /*060000AB*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float32> 
          Sum(class System.Linq.ParallelQuery`1/*0200000C*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float32>> source) cil managed
  // SIG: 00 01 15 11 7D 01 0C 15 12 30 01 15 11 7D 01 0C
  {
    .custom /*0C00018C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x229c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Sum

  .method /*060000AC*/ public hidebysig static 
          float32  Sum(class System.Linq.ParallelQuery`1/*0200000C*/<float32> source) cil managed
  // SIG: 00 01 0C 15 12 30 01 0C
  {
    .custom /*0C00018E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x229f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Sum

  .method /*060000AD*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/ 
          Sum<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                       class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/> selector) cil managed
  // SIG: 10 01 02 11 79 15 12 30 01 1E 00 15 12 69 02 1E 00 11 79
  {
    .custom /*0C000190:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000B9*/ 
      .custom /*0C0001A9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Sum

  .method /*060000AE*/ public hidebysig static 
          float64  Sum<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,float64> selector) cil managed
  // SIG: 10 01 02 0D 15 12 30 01 1E 00 15 12 69 02 1E 00 0D
  {
    .custom /*0C000192:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000BA*/ 
      .custom /*0C0001AC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Sum

  .method /*060000AF*/ public hidebysig static 
          int32  Sum<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                              class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,int32> selector) cil managed
  // SIG: 10 01 02 08 15 12 30 01 1E 00 15 12 69 02 1E 00 08
  {
    .custom /*0C000194:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000BB*/ 
      .custom /*0C0001AF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Sum

  .method /*060000B0*/ public hidebysig static 
          int64  Sum<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                              class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,int64> selector) cil managed
  // SIG: 10 01 02 0A 15 12 30 01 1E 00 15 12 69 02 1E 00 0A
  {
    .custom /*0C000197:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000BC*/ 
      .custom /*0C0001B2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Sum

  .method /*060000B1*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/> 
          Sum<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                       class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Decimal/*0100001E*/>> selector) cil managed
  // SIG: 10 01 02 15 11 7D 01 11 79 15 12 30 01 1E 00 15 12 69 02 1E 00 15 11 7D 01 11 79
  {
    .custom /*0C000199:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000BD*/ 
      .custom /*0C0001B4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Sum

  .method /*060000B2*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float64> 
          Sum<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                       class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float64>> selector) cil managed
  // SIG: 10 01 02 15 11 7D 01 0D 15 12 30 01 1E 00 15 12 69 02 1E 00 15 11 7D 01 0D
  {
    .custom /*0C00019B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000BE*/ 
      .custom /*0C0001B6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Sum

  .method /*060000B3*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int32> 
          Sum<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                       class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int32>> selector) cil managed
  // SIG: 10 01 02 15 11 7D 01 08 15 12 30 01 1E 00 15 12 69 02 1E 00 15 11 7D 01 08
  {
    .custom /*0C00019C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000BF*/ 
      .custom /*0C0001B8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Sum

  .method /*060000B4*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int64> 
          Sum<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                       class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<int64>> selector) cil managed
  // SIG: 10 01 02 15 11 7D 01 0A 15 12 30 01 1E 00 15 12 69 02 1E 00 15 11 7D 01 0A
  {
    .custom /*0C00019E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000C0*/ 
      .custom /*0C0001BA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Sum

  .method /*060000B5*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float32> 
          Sum<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                       class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100001F*/<float32>> selector) cil managed
  // SIG: 10 01 02 15 11 7D 01 0C 15 12 30 01 1E 00 15 12 69 02 1E 00 15 11 7D 01 0C
  {
    .custom /*0C0001A0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000C1*/ 
      .custom /*0C0001BC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Sum

  .method /*060000B6*/ public hidebysig static 
          float32  Sum<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,float32> selector) cil managed
  // SIG: 10 01 02 0C 15 12 30 01 1E 00 15 12 69 02 1E 00 0C
  {
    .custom /*0C0001A2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000C2*/ 
      .custom /*0C0001BE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Sum

  .method /*060000B7*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          TakeWhile<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                             class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,bool> predicate) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 69 02 1E 00 02
  {
    .custom /*0C0001A4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000C3*/ 
      .custom /*0C0001C0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::TakeWhile

  .method /*060000B8*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          TakeWhile<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                             class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TSource,int32,bool> predicate) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 61 03 1E 00 08 02
  {
    .custom /*0C0001A6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000C4*/ 
      .custom /*0C0001C2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::TakeWhile

  .method /*060000B9*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Take<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                        int32 count) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 08
  {
    .custom /*0C0001A8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000C5*/ 
      .custom /*0C0001C4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Take

  .method /*060000BA*/ public hidebysig static 
          class System.Linq.OrderedParallelQuery`1/*02000007*/<!!TSource> 
          ThenByDescending<TSource,TKey>(class System.Linq.OrderedParallelQuery`1/*02000007*/<!!TSource> source,
                                         class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector) cil managed
  // SIG: 10 02 02 15 12 1C 01 1E 00 15 12 1C 01 1E 00 15 12 69 02 1E 00 1E 01
  {
    .custom /*0C0001AA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000C6*/ 
      .custom /*0C0001C6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A0000C7*/ 
      .custom /*0C0001C8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::ThenByDescending

  .method /*060000BB*/ public hidebysig static 
          class System.Linq.OrderedParallelQuery`1/*02000007*/<!!TSource> 
          ThenByDescending<TSource,TKey>(class System.Linq.OrderedParallelQuery`1/*02000007*/<!!TSource> source,
                                         class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector,
                                         class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*01000024*/<!!TKey> comparer) cil managed
  // SIG: 10 02 03 15 12 1C 01 1E 00 15 12 1C 01 1E 00 15 12 69 02 1E 00 1E 01 15 12 80 91 01 1E 01
  {
    .custom /*0C0001AD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000C8*/ 
      .custom /*0C0001CB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A0000C9*/ 
      .custom /*0C0001CE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000164*/ 
    .custom /*0C000209:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::ThenByDescending

  .method /*060000BC*/ public hidebysig static 
          class System.Linq.OrderedParallelQuery`1/*02000007*/<!!TSource> 
          ThenBy<TSource,TKey>(class System.Linq.OrderedParallelQuery`1/*02000007*/<!!TSource> source,
                               class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector) cil managed
  // SIG: 10 02 02 15 12 1C 01 1E 00 15 12 1C 01 1E 00 15 12 69 02 1E 00 1E 01
  {
    .custom /*0C0001B0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000CA*/ 
      .custom /*0C0001D0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A0000CB*/ 
      .custom /*0C0001D2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::ThenBy

  .method /*060000BD*/ public hidebysig static 
          class System.Linq.OrderedParallelQuery`1/*02000007*/<!!TSource> 
          ThenBy<TSource,TKey>(class System.Linq.OrderedParallelQuery`1/*02000007*/<!!TSource> source,
                               class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IComparer`1/*01000024*/<!!TKey> comparer) cil managed
  // SIG: 10 02 03 15 12 1C 01 1E 00 15 12 1C 01 1E 00 15 12 69 02 1E 00 1E 01 15 12 80 91 01 1E 01
  {
    .custom /*0C0001B3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000CC*/ 
      .custom /*0C0001D4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TKey /*2A0000CD*/ 
      .custom /*0C0001D6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000169*/ 
    .custom /*0C00020A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::ThenBy

  .method /*060000BE*/ public hidebysig static 
          !!TSource[]  ToArray<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source) cil managed
  // SIG: 10 01 01 1D 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C0001B5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000CE*/ 
      .custom /*0C0001D8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::ToArray

  .method /*060000BF*/ public hidebysig static 
          class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*01000025*/<!!TKey,!!TSource> 
          ToDictionary<TSource,TKey>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                     class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector) cil managed
  // SIG: 10 02 02 15 12 80 95 02 1E 01 1E 00 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01
  {
    .custom /*0C0001B7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000CF*/ 
      .custom /*0C0001DB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::ToDictionary

  .method /*060000C0*/ public hidebysig static 
          class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*01000025*/<!!TKey,!!TSource> 
          ToDictionary<TSource,TKey>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                     class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector,
                                     class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TKey> comparer) cil managed
  // SIG: 10 02 03 15 12 80 95 02 1E 01 1E 00 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01 15 12 80 85 01 1E
  //      01
  {
    .custom /*0C0001B9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000D1*/ 
      .custom /*0C0001DE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800016F*/ 
    .custom /*0C00020B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::ToDictionary

  .method /*060000C1*/ public hidebysig static 
          class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*01000025*/<!!TKey,!!TElement> 
          ToDictionary<TSource,TKey,TElement>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                              class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector,
                                              class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TElement> elementSelector) cil managed
  // SIG: 10 03 03 15 12 80 95 02 1E 01 1E 02 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01 15 12 69 02 1E 00
  //      1E 02
  {
    .custom /*0C0001BB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000D3*/ 
      .custom /*0C0001E2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TElement /*2A0000D5*/ 
      .custom /*0C0001E3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::ToDictionary

  .method /*060000C2*/ public hidebysig static 
          class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*01000025*/<!!TKey,!!TElement> 
          ToDictionary<TSource,TKey,TElement>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                              class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector,
                                              class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TElement> elementSelector,
                                              class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TKey> comparer) cil managed
  // SIG: 10 03 04 15 12 80 95 02 1E 01 1E 02 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01 15 12 69 02 1E 00
  //      1E 02 15 12 80 85 01 1E 01
  {
    .custom /*0C0001BD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000D6*/ 
      .custom /*0C0001E4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TElement /*2A0000D8*/ 
      .custom /*0C0001E6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000176*/ 
    .custom /*0C00020C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::ToDictionary

  .method /*060000C3*/ public hidebysig static 
          class [System.Collections/*23000004*/]System.Collections.Generic.List`1/*01000026*/<!!TSource> 
          ToList<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source) cil managed
  // SIG: 10 01 01 15 12 80 99 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C0001BF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000D9*/ 
      .custom /*0C0001E7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::ToList

  .method /*060000C4*/ public hidebysig static 
          class [System.Linq/*23000003*/]System.Linq.ILookup`2/*01000027*/<!!TKey,!!TSource> 
          ToLookup<TSource,TKey>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                 class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector) cil managed
  // SIG: 10 02 02 15 12 80 9D 02 1E 01 1E 00 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01
  {
    .custom /*0C0001C1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000DA*/ 
      .custom /*0C0001E8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::ToLookup

  .method /*060000C5*/ public hidebysig static 
          class [System.Linq/*23000003*/]System.Linq.ILookup`2/*01000027*/<!!TKey,!!TSource> 
          ToLookup<TSource,TKey>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                 class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector,
                                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TKey> comparer) cil managed
  // SIG: 10 02 03 15 12 80 9D 02 1E 01 1E 00 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01 15 12 80 85 01 1E
  //      01
  {
    .custom /*0C0001C3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000DC*/ 
      .custom /*0C0001E9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800017C*/ 
    .custom /*0C00020D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::ToLookup

  .method /*060000C6*/ public hidebysig static 
          class [System.Linq/*23000003*/]System.Linq.ILookup`2/*01000027*/<!!TKey,!!TElement> 
          ToLookup<TSource,TKey,TElement>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                          class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector,
                                          class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TElement> elementSelector) cil managed
  // SIG: 10 03 03 15 12 80 9D 02 1E 01 1E 02 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01 15 12 69 02 1E 00
  //      1E 02
  {
    .custom /*0C0001C5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000DE*/ 
      .custom /*0C0001EA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TElement /*2A0000E0*/ 
      .custom /*0C0001EB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::ToLookup

  .method /*060000C7*/ public hidebysig static 
          class [System.Linq/*23000003*/]System.Linq.ILookup`2/*01000027*/<!!TKey,!!TElement> 
          ToLookup<TSource,TKey,TElement>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                          class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TKey> keySelector,
                                          class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,!!TElement> elementSelector,
                                          class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TKey> comparer) cil managed
  // SIG: 10 03 04 15 12 80 9D 02 1E 01 1E 02 15 12 30 01 1E 00 15 12 69 02 1E 00 1E 01 15 12 69 02 1E 00
  //      1E 02 15 12 80 85 01 1E 01
  {
    .custom /*0C0001C7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000E1*/ 
      .custom /*0C0001EC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TElement /*2A0000E3*/ 
      .custom /*0C0001ED:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000183*/ 
    .custom /*0C00020E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::ToLookup

  .method /*060000C8*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Union<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> first,
                         class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> second) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 6D 01 1E 00
  {
    .custom /*0C0001C9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001CA:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000020*/::.ctor(string) /* 0A000013 */ = ( 01 00 81 0C 54 68 65 20 73 65 63 6F 6E 64 20 64   // ....The second d
                                                                                                                                                   61 74 61 20 73 6F 75 72 63 65 20 6F 66 20 61 20   // ata source of a 
                                                                                                                                                   62 69 6E 61 72 79 20 6F 70 65 72 61 74 6F 72 20   // binary operator 
                                                                                                                                                   6D 75 73 74 20 62 65 20 6F 66 20 74 79 70 65 20   // must be of type 
                                                                                                                                                   53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50 61 72 61   // System.Linq.Para
                                                                                                                                                   6C 6C 65 6C 51 75 65 72 79 3C 54 3E 20 72 61 74   // llelQuery<T> rat
                                                                                                                                                   68 65 72 20 74 68 61 6E 20 53 79 73 74 65 6D 2E   // her than System.
                                                                                                                                                   43 6F 6C 6C 65 63 74 69 6F 6E 73 2E 47 65 6E 65   // Collections.Gene
                                                                                                                                                   72 69 63 2E 49 45 6E 75 6D 65 72 61 62 6C 65 3C   // ric.IEnumerable<
                                                                                                                                                   54 3E 2E 20 54 6F 20 66 69 78 20 74 68 69 73 20   // T>. To fix this 
                                                                                                                                                   70 72 6F 62 6C 65 6D 2C 20 75 73 65 20 74 68 65   // problem, use the
                                                                                                                                                   20 41 73 50 61 72 61 6C 6C 65 6C 28 29 20 65 78   //  AsParallel() ex
                                                                                                                                                   74 65 6E 73 69 6F 6E 20 6D 65 74 68 6F 64 20 74   // tension method t
                                                                                                                                                   6F 20 63 6F 6E 76 65 72 74 20 74 68 65 20 72 69   // o convert the ri
                                                                                                                                                   67 68 74 20 64 61 74 61 20 73 6F 75 72 63 65 20   // ght data source 
                                                                                                                                                   74 6F 20 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50   // to System.Linq.P
                                                                                                                                                   61 72 61 6C 6C 65 6C 51 75 65 72 79 3C 54 3E 2E   // arallelQuery<T>.
                                                                                                                                                   00 00 ) 
    .param type TSource /*2A0000E4*/ 
      .custom /*0C0001EF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Union

  .method /*060000C9*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Union<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> first,
                         class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSource> second,
                         class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TSource> comparer) cil managed
  // SIG: 10 01 03 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 6D 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C0001CC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001CD:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000020*/::.ctor(string) /* 0A000013 */ = ( 01 00 81 0C 54 68 65 20 73 65 63 6F 6E 64 20 64   // ....The second d
                                                                                                                                                   61 74 61 20 73 6F 75 72 63 65 20 6F 66 20 61 20   // ata source of a 
                                                                                                                                                   62 69 6E 61 72 79 20 6F 70 65 72 61 74 6F 72 20   // binary operator 
                                                                                                                                                   6D 75 73 74 20 62 65 20 6F 66 20 74 79 70 65 20   // must be of type 
                                                                                                                                                   53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50 61 72 61   // System.Linq.Para
                                                                                                                                                   6C 6C 65 6C 51 75 65 72 79 3C 54 3E 20 72 61 74   // llelQuery<T> rat
                                                                                                                                                   68 65 72 20 74 68 61 6E 20 53 79 73 74 65 6D 2E   // her than System.
                                                                                                                                                   43 6F 6C 6C 65 63 74 69 6F 6E 73 2E 47 65 6E 65   // Collections.Gene
                                                                                                                                                   72 69 63 2E 49 45 6E 75 6D 65 72 61 62 6C 65 3C   // ric.IEnumerable<
                                                                                                                                                   54 3E 2E 20 54 6F 20 66 69 78 20 74 68 69 73 20   // T>. To fix this 
                                                                                                                                                   70 72 6F 62 6C 65 6D 2C 20 75 73 65 20 74 68 65   // problem, use the
                                                                                                                                                   20 41 73 50 61 72 61 6C 6C 65 6C 28 29 20 65 78   //  AsParallel() ex
                                                                                                                                                   74 65 6E 73 69 6F 6E 20 6D 65 74 68 6F 64 20 74   // tension method t
                                                                                                                                                   6F 20 63 6F 6E 76 65 72 74 20 74 68 65 20 72 69   // o convert the ri
                                                                                                                                                   67 68 74 20 64 61 74 61 20 73 6F 75 72 63 65 20   // ght data source 
                                                                                                                                                   74 6F 20 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50   // to System.Linq.P
                                                                                                                                                   61 72 61 6C 6C 65 6C 51 75 65 72 79 3C 54 3E 2E   // arallelQuery<T>.
                                                                                                                                                   00 00 ) 
    .param type TSource /*2A0000E5*/ 
      .custom /*0C0001F1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000188*/ 
    .custom /*0C00020F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Union

  .method /*060000CA*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Union<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> first,
                         class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> second) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 30 01 1E 00
  {
    .custom /*0C0001CF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000E6*/ 
      .custom /*0C0001F2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Union

  .method /*060000CB*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Union<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> first,
                         class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> second,
                         class [System.Runtime/*23000001*/]System.Collections.Generic.IEqualityComparer`1/*01000021*/<!!TSource> comparer) cil managed
  // SIG: 10 01 03 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 80 85 01 1E 00
  {
    .custom /*0C0001D1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000E7*/ 
      .custom /*0C0001F3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800018D*/ 
    .custom /*0C000210:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Union

  .method /*060000CC*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Where<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                         class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<!!TSource,bool> predicate) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 69 02 1E 00 02
  {
    .custom /*0C0001D3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000E8*/ 
      .custom /*0C0001F4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Where

  .method /*060000CD*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          Where<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                         class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TSource,int32,bool> predicate) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 15 12 61 03 1E 00 08 02
  {
    .custom /*0C0001D5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000E9*/ 
      .custom /*0C0001F5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2302
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Where

  .method /*060000CE*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          WithCancellation<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000028*/ cancellationToken) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 11 80 A1
  {
    .custom /*0C0001D7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000EA*/ 
      .custom /*0C0001F6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2305
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::WithCancellation

  .method /*060000CF*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          WithDegreeOfParallelism<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                           int32 degreeOfParallelism) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 08
  {
    .custom /*0C0001D9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000EB*/ 
      .custom /*0C0001F7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2308
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::WithDegreeOfParallelism

  .method /*060000D0*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          WithExecutionMode<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                     valuetype System.Linq.ParallelExecutionMode/*02000009*/ executionMode) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 11 24
  {
    .custom /*0C0001DC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000EC*/ 
      .custom /*0C0001F8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x230b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::WithExecutionMode

  .method /*060000D1*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> 
          WithMergeOptions<TSource>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TSource> source,
                                    valuetype System.Linq.ParallelMergeOptions/*0200000A*/ mergeOptions) cil managed
  // SIG: 10 01 02 15 12 30 01 1E 00 15 12 30 01 1E 00 11 28
  {
    .custom /*0C0001DD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TSource /*2A0000ED*/ 
      .custom /*0C0001F9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x230e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::WithMergeOptions

  .method /*060000D2*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          Zip<TFirst,TSecond,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TFirst> first,
                                      class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!!TSecond> second,
                                      class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TFirst,!!TSecond,!!TResult> resultSelector) cil managed
  // SIG: 10 03 03 15 12 30 01 1E 02 15 12 30 01 1E 00 15 12 6D 01 1E 01 15 12 61 03 1E 00 1E 01 1E 02
  {
    .custom /*0C0001DF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0001E0:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000020*/::.ctor(string) /* 0A000013 */ = ( 01 00 81 0C 54 68 65 20 73 65 63 6F 6E 64 20 64   // ....The second d
                                                                                                                                                   61 74 61 20 73 6F 75 72 63 65 20 6F 66 20 61 20   // ata source of a 
                                                                                                                                                   62 69 6E 61 72 79 20 6F 70 65 72 61 74 6F 72 20   // binary operator 
                                                                                                                                                   6D 75 73 74 20 62 65 20 6F 66 20 74 79 70 65 20   // must be of type 
                                                                                                                                                   53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50 61 72 61   // System.Linq.Para
                                                                                                                                                   6C 6C 65 6C 51 75 65 72 79 3C 54 3E 20 72 61 74   // llelQuery<T> rat
                                                                                                                                                   68 65 72 20 74 68 61 6E 20 53 79 73 74 65 6D 2E   // her than System.
                                                                                                                                                   43 6F 6C 6C 65 63 74 69 6F 6E 73 2E 47 65 6E 65   // Collections.Gene
                                                                                                                                                   72 69 63 2E 49 45 6E 75 6D 65 72 61 62 6C 65 3C   // ric.IEnumerable<
                                                                                                                                                   54 3E 2E 20 54 6F 20 66 69 78 20 74 68 69 73 20   // T>. To fix this 
                                                                                                                                                   70 72 6F 62 6C 65 6D 2C 20 75 73 65 20 74 68 65   // problem, use the
                                                                                                                                                   20 41 73 50 61 72 61 6C 6C 65 6C 28 29 20 65 78   //  AsParallel() ex
                                                                                                                                                   74 65 6E 73 69 6F 6E 20 6D 65 74 68 6F 64 20 74   // tension method t
                                                                                                                                                   6F 20 63 6F 6E 76 65 72 74 20 74 68 65 20 72 69   // o convert the ri
                                                                                                                                                   67 68 74 20 64 61 74 61 20 73 6F 75 72 63 65 20   // ght data source 
                                                                                                                                                   74 6F 20 53 79 73 74 65 6D 2E 4C 69 6E 71 2E 50   // to System.Linq.P
                                                                                                                                                   61 72 61 6C 6C 65 6C 51 75 65 72 79 3C 54 3E 2E   // arallelQuery<T>.
                                                                                                                                                   00 00 ) 
    .param type TFirst /*2A0000EE*/ 
      .custom /*0C0001FA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TSecond /*2A0000EF*/ 
      .custom /*0C0001FB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A0000F0*/ 
      .custom /*0C0001FD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2311
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Zip

  .method /*060000D3*/ public hidebysig static 
          class System.Linq.ParallelQuery`1/*0200000C*/<!!TResult> 
          Zip<TFirst,TSecond,TResult>(class System.Linq.ParallelQuery`1/*0200000C*/<!!TFirst> first,
                                      class System.Linq.ParallelQuery`1/*0200000C*/<!!TSecond> second,
                                      class [System.Runtime/*23000001*/]System.Func`3/*01000018*/<!!TFirst,!!TSecond,!!TResult> resultSelector) cil managed
  // SIG: 10 03 03 15 12 30 01 1E 02 15 12 30 01 1E 00 15 12 30 01 1E 01 15 12 61 03 1E 00 1E 01 1E 02
  {
    .custom /*0C0001E1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TFirst /*2A0000F1*/ 
      .custom /*0C0001FE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TSecond /*2A0000F2*/ 
      .custom /*0C0001FF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A0000F3*/ 
      .custom /*0C000200:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2314
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelEnumerable::Zip

} // end of class System.Linq.ParallelEnumerable

.class /*02000009*/ public auto ansi sealed System.Linq.ParallelExecutionMode
       extends [System.Runtime/*23000001*/]System.Enum/*01000029*/
{
  .field /*04000005*/ public specialname rtspecialname int32 value__
  .field /*04000006*/ public static literal valuetype System.Linq.ParallelExecutionMode/*02000009*/ Default = int32(0x00000000)
  .field /*04000007*/ public static literal valuetype System.Linq.ParallelExecutionMode/*02000009*/ ForceParallelism = int32(0x00000001)
} // end of class System.Linq.ParallelExecutionMode

.class /*0200000A*/ public auto ansi sealed System.Linq.ParallelMergeOptions
       extends [System.Runtime/*23000001*/]System.Enum/*01000029*/
{
  .field /*04000008*/ public specialname rtspecialname int32 value__
  .field /*04000009*/ public static literal valuetype System.Linq.ParallelMergeOptions/*0200000A*/ Default = int32(0x00000000)
  .field /*0400000A*/ public static literal valuetype System.Linq.ParallelMergeOptions/*0200000A*/ NotBuffered = int32(0x00000001)
  .field /*0400000B*/ public static literal valuetype System.Linq.ParallelMergeOptions/*0200000A*/ AutoBuffered = int32(0x00000002)
  .field /*0400000C*/ public static literal valuetype System.Linq.ParallelMergeOptions/*0200000A*/ FullyBuffered = int32(0x00000003)
} // end of class System.Linq.ParallelMergeOptions

.class /*0200000B*/ public auto ansi beforefieldinit System.Linq.ParallelQuery
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [System.Runtime/*23000001*/]System.Collections.IEnumerable/*0100001C*/
{
  .method /*060000D4*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2317
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000017       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A000017 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ParallelQuery::.ctor

  .method /*060000D5*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*0100002A*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 80 A9
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*0100001C*/::GetEnumerator /*0100001C::0A000014*/ 
    // Method begins at RVA 0x231f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelQuery::System.Collections.IEnumerable.GetEnumerator

} // end of class System.Linq.ParallelQuery

.class /*0200000C*/ public auto ansi beforefieldinit System.Linq.ParallelQuery`1<TSource>
       extends System.Linq.ParallelQuery/*0200000B*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!TSource>/*1B000002*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*0100001C*/
{
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001B*/<!TSource>/*1B000002*/
  .custom /*0C000019:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type TSource /*2A000008*/ 
    .custom /*0C000031:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*060000D6*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2322
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0000D4       */ call       instance void System.Linq.ParallelQuery/*0200000B*/::.ctor() /* 060000D4 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ParallelQuery`1::.ctor

  .method /*060000D7*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000016*/<!TSource> 
          GetEnumerator() cil managed
  // SIG: 20 00 15 12 59 01 13 00
  {
    .custom /*0C0001E5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x232a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParallelQuery`1::GetEnumerator

} // end of class System.Linq.ParallelQuery`1


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
