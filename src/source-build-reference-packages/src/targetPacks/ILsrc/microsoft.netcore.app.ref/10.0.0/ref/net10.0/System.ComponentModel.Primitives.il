
//  .NET IL Disassembler.  Version 11.0.0-alpha.1.26055.101


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
// Time-date stamp:            0xf0952918
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00003400
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000535e
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
// Checksum:                       0x0000a20d
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000530c [0x0000004f] address [size] of Import Directory:          
// 0x00006000 [0x000004e8] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00003e00 [0x00002908] address [size] of Security Directory:        
// 0x00008000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x000052f0 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00003364 Virtual Size
//              0x00002000 Virtual Address
//              0x00003400 Size of Raw Data
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
//              0x000004e8 Virtual Size
//              0x00006000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00003600 Pointer to Raw Data
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
//              0x00003c00 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000360
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000534e Import Name Table
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
// 0x0000231c [0x00002f54] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00005270 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00001918 Size
//              '#~' Name
//    Stream 2:
//              0x00001984 Offset
//              0x00000d7c Size
//              '#Strings' Name
//    Stream 3:
//              0x00002700 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00002704 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00002714 Offset
//              0x00000840 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000000903b41f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 26376
// PE header size       : 512 (496 used)    ( 1.94%)
// PE additional info   : 11959             (45.34%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.27%)
// CLR meta-data size  : 12116              (45.94%)
// CLR additional info : 128                ( 0.49%)
// CLR method headers  : 185                ( 0.70%)
// Managed code         : 527               ( 2.00%)
// Data                 : 2048              ( 7.76%)
// Unaccounted          : -1171             (-4.44%)

// Num.of PE sections   : 3
//   .text    - 13312
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 12116
//   Module        -    1 (10 bytes)
//   TypeDef       -   31 (434 bytes)     5 interfaces, 0 explicit layout
//   TypeRef       -   45 (270 bytes)
//   MethodDef     -  175 (2450 bytes)    19 abstract, 0 native, 156 bodies
//   FieldDef      -   43 (258 bytes)     35 constant
//   MemberRef     -   33 (198 bytes)
//   ParamDef      -  102 (612 bytes)
//   MethodImpl    -    1 (6 bytes)
//   Constant      -    6 (36 bytes)
//   CustomAttribute-  179 (1074 bytes)
//   InterfaceImpl -    6 (24 bytes)
//   PropertyMap   -   25 (100 bytes)
//   Property      -   56 (336 bytes)
//   MethodSemantic-   66 (396 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    4 (80 bytes)
//   EventMap      -    2 (8 bytes)
//   Event         -    2 (12 bytes)
//   Strings       -  3452 bytes
//   Blobs         -  2112 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   206 bytes

// CLR additional info : 128

// CLR method headers : 185
//   Num.of method bodies  - 156
//   Num.of fat headers    - 1
//   Num.of tiny headers   - 155
//   Num.of fat sections   - 0
//   Num.of small sections - 1

// Managed code : 527
//   Ave method size - 3
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class BrowsableAttribute             (public) (auto) (ansi) (sealed) 
// Class CategoryAttribute              (public) (auto) (ansi) 
// Class Component                      (public) (auto) (ansi) 
// Class ComponentCollection            (public) (auto) (ansi) 
// Class DescriptionAttribute           (public) (auto) (ansi) 
// Class DesignerAttribute              (public) (auto) (ansi) (sealed) 
// Class DesignerCategoryAttribute      (public) (auto) (ansi) (sealed) 
// Class DesignerSerializationVisibility (public) (auto) (ansi) (sealed) 
// Class DesignerSerializationVisibilityAttribute (public) (auto) (ansi) (sealed) 
// Class DesignOnlyAttribute            (public) (auto) (ansi) (sealed) 
// Class DisplayNameAttribute           (public) (auto) (ansi) 
// Class EditorAttribute                (public) (auto) (ansi) (sealed) 
// Class EventHandlerList               (public) (auto) (ansi) (sealed) 
// Interface IComponent                     (public) (abstract) (auto) (ansi) 
// Interface IContainer                     (public) (abstract) (auto) (ansi) 
// Class ImmutableObjectAttribute       (public) (auto) (ansi) (sealed) 
// Class InitializationEventAttribute   (public) (auto) (ansi) (sealed) 
// Class InvalidAsynchronousStateException (public) (auto) (ansi) 
// Class InvalidEnumArgumentException   (public) (auto) (ansi) 
// Interface ISite                          (public) (abstract) (auto) (ansi) 
// Interface ISupportInitialize             (public) (abstract) (auto) (ansi) 
// Interface ISynchronizeInvoke             (public) (abstract) (auto) (ansi) 
// Class LocalizableAttribute           (public) (auto) (ansi) (sealed) 
// Class MergablePropertyAttribute      (public) (auto) (ansi) (sealed) 
// Class NotifyParentPropertyAttribute  (public) (auto) (ansi) (sealed) 
// Class ParenthesizePropertyNameAttribute (public) (auto) (ansi) (sealed) 
// Class ReadOnlyAttribute              (public) (auto) (ansi) (sealed) 
// Class RefreshProperties              (public) (auto) (ansi) (sealed) 
// Class RefreshPropertiesAttribute     (public) (auto) (ansi) (sealed) 
// Class DesignerSerializerAttribute    (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly extern /*23000002*/ System.Collections.NonGeneric
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly extern /*23000003*/ System.ObjectModel
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly extern /*23000004*/ System.ComponentModel
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 10:0:0:0
}
.assembly /*20000001*/ noplatform System.ComponentModel.Primitives
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
  .custom /*0C00000D:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 20 53 79 73 74 65 6D 2E 43 6F 6D 70 6F 6E   // .. System.Compon
                                                                                                                                                                        65 6E 74 4D 6F 64 65 6C 2E 50 72 69 6D 69 74 69   // entModel.Primiti
                                                                                                                                                                        76 65 73 00 00 )                                  // ves..
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
  .custom /*0C000013:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 20 53 79 73 74 65 6D 2E 43 6F 6D 70 6F 6E   // .. System.Compon
                                                                                                                                                                       65 6E 74 4D 6F 64 65 6C 2E 50 72 69 6D 69 74 69   // entModel.Primiti
                                                                                                                                                                       76 65 73 00 00 )                                  // ves..
  .custom /*0C000014:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0D 31 30 2E 30 2E 32 35 2E 35 32 34 31 31   // ...10.0.25.52411
                                                                                                                                                                       00 00 ) 
  .custom /*0C000015:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2F 31 30 2E 30 2E 30 2B 62 30 66 33 34 64   // ../10.0.0+b0f34d
                                                                                                                                                                                35 31 66 63 63 63 36 39 66 64 33 33 34 32 35 33   // 51fccc69fd334253
                                                                                                                                                                                39 32 34 61 62 64 38 64 36 38 35 33 66 61 64 37   // 924abd8d6853fad7
                                                                                                                                                                                61 61 00 00 )                                     // aa..
  .custom /*0C000016:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000017:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 20 53 79 73 74 65 6D 2E 43 6F 6D 70 6F 6E   // .. System.Compon
                                                                                                                                                                 65 6E 74 4D 6F 64 65 6C 2E 50 72 69 6D 69 74 69   // entModel.Primiti
                                                                                                                                                                 76 65 73 00 00 )                                  // ves..
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
.module System.ComponentModel.Primitives.dll
// MVID: {605911a8-685b-4afc-99f2-3d8681918df2}
.custom /*0C000002:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000011*/::.ctor(int32) /* 0A000010 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000003:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000012*/::.ctor(bool) /* 0A000011 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY
// Image base: 0x00007F502FF1E000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi sealed beforefieldinit System.ComponentModel.BrowsableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C000019:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00001B:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000014 */ = ( 01 00 FF 7F 00 00 00 00 ) 
  .field /*04000001*/ public static initonly class System.ComponentModel.BrowsableAttribute/*02000002*/ Default
  .field /*04000002*/ public static initonly class System.ComponentModel.BrowsableAttribute/*02000002*/ No
  .field /*04000003*/ public static initonly class System.ComponentModel.BrowsableAttribute/*02000002*/ Yes
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool browsable) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BrowsableAttribute::.ctor

  .method /*06000002*/ public hidebysig specialname 
          instance bool  get_Browsable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2058
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowsableAttribute::get_Browsable

  .method /*06000003*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00001E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000002*/ 
    .custom /*0C00001C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000018*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x205b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowsableAttribute::Equals

  .method /*06000004*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x205e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowsableAttribute::GetHashCode

  .method /*06000005*/ public hidebysig virtual 
          instance bool  IsDefaultAttribute() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2061
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BrowsableAttribute::IsDefaultAttribute

  .property /*17000001*/ instance callconv(8) bool
          Browsable()
  {
    .get instance bool System.ComponentModel.BrowsableAttribute/*02000002*/::get_Browsable() /* 06000002 */
  } // end of property BrowsableAttribute::Browsable
} // end of class System.ComponentModel.BrowsableAttribute

.class /*02000003*/ public auto ansi beforefieldinit System.ComponentModel.CategoryAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C00001F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000020:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000021:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000014 */ = ( 01 00 FF 7F 00 00 00 00 ) 
  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2064
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CategoryAttribute::.ctor

  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string category) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x206c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CategoryAttribute::.ctor

  .method /*06000008*/ public hidebysig specialname static 
          class System.ComponentModel.CategoryAttribute/*02000003*/ 
          get_Action() cil managed
  // SIG: 00 00 12 0C
  {
    // Method begins at RVA 0x2074
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CategoryAttribute::get_Action

  .method /*06000009*/ public hidebysig specialname static 
          class System.ComponentModel.CategoryAttribute/*02000003*/ 
          get_Appearance() cil managed
  // SIG: 00 00 12 0C
  {
    // Method begins at RVA 0x2077
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CategoryAttribute::get_Appearance

  .method /*0600000A*/ public hidebysig specialname static 
          class System.ComponentModel.CategoryAttribute/*02000003*/ 
          get_Asynchronous() cil managed
  // SIG: 00 00 12 0C
  {
    // Method begins at RVA 0x207a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CategoryAttribute::get_Asynchronous

  .method /*0600000B*/ public hidebysig specialname static 
          class System.ComponentModel.CategoryAttribute/*02000003*/ 
          get_Behavior() cil managed
  // SIG: 00 00 12 0C
  {
    // Method begins at RVA 0x207d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CategoryAttribute::get_Behavior

  .method /*0600000C*/ public hidebysig specialname 
          instance string  get_Category() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2080
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CategoryAttribute::get_Category

  .method /*0600000D*/ public hidebysig specialname static 
          class System.ComponentModel.CategoryAttribute/*02000003*/ 
          get_Data() cil managed
  // SIG: 00 00 12 0C
  {
    // Method begins at RVA 0x2083
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CategoryAttribute::get_Data

  .method /*0600000E*/ public hidebysig specialname static 
          class System.ComponentModel.CategoryAttribute/*02000003*/ 
          get_Default() cil managed
  // SIG: 00 00 12 0C
  {
    // Method begins at RVA 0x2086
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CategoryAttribute::get_Default

  .method /*0600000F*/ public hidebysig specialname static 
          class System.ComponentModel.CategoryAttribute/*02000003*/ 
          get_Design() cil managed
  // SIG: 00 00 12 0C
  {
    // Method begins at RVA 0x2089
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CategoryAttribute::get_Design

  .method /*06000010*/ public hidebysig specialname static 
          class System.ComponentModel.CategoryAttribute/*02000003*/ 
          get_DragDrop() cil managed
  // SIG: 00 00 12 0C
  {
    // Method begins at RVA 0x208c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CategoryAttribute::get_DragDrop

  .method /*06000011*/ public hidebysig specialname static 
          class System.ComponentModel.CategoryAttribute/*02000003*/ 
          get_Focus() cil managed
  // SIG: 00 00 12 0C
  {
    // Method begins at RVA 0x208f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CategoryAttribute::get_Focus

  .method /*06000012*/ public hidebysig specialname static 
          class System.ComponentModel.CategoryAttribute/*02000003*/ 
          get_Format() cil managed
  // SIG: 00 00 12 0C
  {
    // Method begins at RVA 0x2092
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CategoryAttribute::get_Format

  .method /*06000013*/ public hidebysig specialname static 
          class System.ComponentModel.CategoryAttribute/*02000003*/ 
          get_Key() cil managed
  // SIG: 00 00 12 0C
  {
    // Method begins at RVA 0x2095
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CategoryAttribute::get_Key

  .method /*06000014*/ public hidebysig specialname static 
          class System.ComponentModel.CategoryAttribute/*02000003*/ 
          get_Layout() cil managed
  // SIG: 00 00 12 0C
  {
    // Method begins at RVA 0x2098
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CategoryAttribute::get_Layout

  .method /*06000015*/ public hidebysig specialname static 
          class System.ComponentModel.CategoryAttribute/*02000003*/ 
          get_Mouse() cil managed
  // SIG: 00 00 12 0C
  {
    // Method begins at RVA 0x209b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CategoryAttribute::get_Mouse

  .method /*06000016*/ public hidebysig specialname static 
          class System.ComponentModel.CategoryAttribute/*02000003*/ 
          get_WindowStyle() cil managed
  // SIG: 00 00 12 0C
  {
    // Method begins at RVA 0x209e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CategoryAttribute::get_WindowStyle

  .method /*06000017*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00005F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000004*/ 
    .custom /*0C000026:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000018*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CategoryAttribute::Equals

  .method /*06000018*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CategoryAttribute::GetHashCode

  .method /*06000019*/ family hidebysig newslot virtual 
          instance string  GetLocalizedString(string 'value') cil managed
  // SIG: 20 01 0E 0E
  {
    .param [0]/*08000005*/ 
    .custom /*0C00002B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CategoryAttribute::GetLocalizedString

  .method /*0600001A*/ public hidebysig virtual 
          instance bool  IsDefaultAttribute() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CategoryAttribute::IsDefaultAttribute

  .property /*17000002*/ callconv(8) class System.ComponentModel.CategoryAttribute/*02000003*/
          Action()
  {
    .get class System.ComponentModel.CategoryAttribute/*02000003*/ System.ComponentModel.CategoryAttribute/*02000003*/::get_Action() /* 06000008 */
  } // end of property CategoryAttribute::Action
  .property /*17000003*/ callconv(8) class System.ComponentModel.CategoryAttribute/*02000003*/
          Appearance()
  {
    .get class System.ComponentModel.CategoryAttribute/*02000003*/ System.ComponentModel.CategoryAttribute/*02000003*/::get_Appearance() /* 06000009 */
  } // end of property CategoryAttribute::Appearance
  .property /*17000004*/ callconv(8) class System.ComponentModel.CategoryAttribute/*02000003*/
          Asynchronous()
  {
    .get class System.ComponentModel.CategoryAttribute/*02000003*/ System.ComponentModel.CategoryAttribute/*02000003*/::get_Asynchronous() /* 0600000A */
  } // end of property CategoryAttribute::Asynchronous
  .property /*17000005*/ callconv(8) class System.ComponentModel.CategoryAttribute/*02000003*/
          Behavior()
  {
    .get class System.ComponentModel.CategoryAttribute/*02000003*/ System.ComponentModel.CategoryAttribute/*02000003*/::get_Behavior() /* 0600000B */
  } // end of property CategoryAttribute::Behavior
  .property /*17000006*/ instance callconv(8) string
          Category()
  {
    .get instance string System.ComponentModel.CategoryAttribute/*02000003*/::get_Category() /* 0600000C */
  } // end of property CategoryAttribute::Category
  .property /*17000007*/ callconv(8) class System.ComponentModel.CategoryAttribute/*02000003*/
          Data()
  {
    .get class System.ComponentModel.CategoryAttribute/*02000003*/ System.ComponentModel.CategoryAttribute/*02000003*/::get_Data() /* 0600000D */
  } // end of property CategoryAttribute::Data
  .property /*17000008*/ callconv(8) class System.ComponentModel.CategoryAttribute/*02000003*/
          Default()
  {
    .get class System.ComponentModel.CategoryAttribute/*02000003*/ System.ComponentModel.CategoryAttribute/*02000003*/::get_Default() /* 0600000E */
  } // end of property CategoryAttribute::Default
  .property /*17000009*/ callconv(8) class System.ComponentModel.CategoryAttribute/*02000003*/
          Design()
  {
    .get class System.ComponentModel.CategoryAttribute/*02000003*/ System.ComponentModel.CategoryAttribute/*02000003*/::get_Design() /* 0600000F */
  } // end of property CategoryAttribute::Design
  .property /*1700000A*/ callconv(8) class System.ComponentModel.CategoryAttribute/*02000003*/
          DragDrop()
  {
    .get class System.ComponentModel.CategoryAttribute/*02000003*/ System.ComponentModel.CategoryAttribute/*02000003*/::get_DragDrop() /* 06000010 */
  } // end of property CategoryAttribute::DragDrop
  .property /*1700000B*/ callconv(8) class System.ComponentModel.CategoryAttribute/*02000003*/
          Focus()
  {
    .get class System.ComponentModel.CategoryAttribute/*02000003*/ System.ComponentModel.CategoryAttribute/*02000003*/::get_Focus() /* 06000011 */
  } // end of property CategoryAttribute::Focus
  .property /*1700000C*/ callconv(8) class System.ComponentModel.CategoryAttribute/*02000003*/
          Format()
  {
    .get class System.ComponentModel.CategoryAttribute/*02000003*/ System.ComponentModel.CategoryAttribute/*02000003*/::get_Format() /* 06000012 */
  } // end of property CategoryAttribute::Format
  .property /*1700000D*/ callconv(8) class System.ComponentModel.CategoryAttribute/*02000003*/
          Key()
  {
    .get class System.ComponentModel.CategoryAttribute/*02000003*/ System.ComponentModel.CategoryAttribute/*02000003*/::get_Key() /* 06000013 */
  } // end of property CategoryAttribute::Key
  .property /*1700000E*/ callconv(8) class System.ComponentModel.CategoryAttribute/*02000003*/
          Layout()
  {
    .get class System.ComponentModel.CategoryAttribute/*02000003*/ System.ComponentModel.CategoryAttribute/*02000003*/::get_Layout() /* 06000014 */
  } // end of property CategoryAttribute::Layout
  .property /*1700000F*/ callconv(8) class System.ComponentModel.CategoryAttribute/*02000003*/
          Mouse()
  {
    .get class System.ComponentModel.CategoryAttribute/*02000003*/ System.ComponentModel.CategoryAttribute/*02000003*/::get_Mouse() /* 06000015 */
  } // end of property CategoryAttribute::Mouse
  .property /*17000010*/ callconv(8) class System.ComponentModel.CategoryAttribute/*02000003*/
          WindowStyle()
  {
    .get class System.ComponentModel.CategoryAttribute/*02000003*/ System.ComponentModel.CategoryAttribute/*02000003*/::get_WindowStyle() /* 06000016 */
  } // end of property CategoryAttribute::WindowStyle
} // end of class System.ComponentModel.CategoryAttribute

.class /*02000004*/ public auto ansi beforefieldinit System.ComponentModel.Component
       extends [System.Runtime/*23000001*/]System.MarshalByRefObject/*0100001A*/
       implements System.ComponentModel.IComponent/*0200000F*/,
                  [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
{
  .custom /*0C000023:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000024:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000025:06000040*/ instance void System.ComponentModel.DesignerCategoryAttribute/*02000008*/::.ctor(string) /* 06000040 */ = ( 01 00 09 43 6F 6D 70 6F 6E 65 6E 74 00 00 )       // ...Component..
  .interfaceimpl type System.ComponentModel.IComponent/*0200000F*/
  .custom /*0C000001:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
  .custom /*0C00001D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*0600001B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ad
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.MarshalByRefObject/*0100001A*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Component::.ctor

  .method /*0600001C*/ family hidebysig newslot specialname virtual 
          instance bool  get_CanRaiseEvents() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Component::get_CanRaiseEvents

  .method /*0600001D*/ public hidebysig specialname 
          instance class System.ComponentModel.IContainer/*02000010*/ 
          get_Container() cil managed
  // SIG: 20 00 12 40
  {
    // Method begins at RVA 0x20b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Component::get_Container

  .method /*0600001E*/ family hidebysig specialname 
          instance bool  get_DesignMode() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Component::get_DesignMode

  .method /*0600001F*/ family hidebysig specialname 
          instance class System.ComponentModel.EventHandlerList/*0200000E*/ 
          get_Events() cil managed
  // SIG: 20 00 12 38
  {
    .custom /*0C000071:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Component::get_Events

  .method /*06000020*/ public hidebysig newslot specialname virtual 
          instance class System.ComponentModel.ISite/*02000015*/ 
          get_Site() cil managed
  // SIG: 20 00 12 54
  {
    // Method begins at RVA 0x20c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Component::get_Site

  .method /*06000021*/ public hidebysig newslot specialname virtual 
          instance void  set_Site(class System.ComponentModel.ISite/*02000015*/ 'value') cil managed
  // SIG: 20 01 01 12 54
  {
    // Method begins at RVA 0x20c4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Component::set_Site

  .method /*06000022*/ public hidebysig newslot specialname virtual final 
          instance void  add_Disposed(class [System.Runtime/*23000001*/]System.EventHandler/*0100001E*/ 'value') cil managed
  // SIG: 20 01 01 12 79
  {
    // Method begins at RVA 0x20c6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Component::add_Disposed

  .method /*06000023*/ public hidebysig newslot specialname virtual final 
          instance void  remove_Disposed(class [System.Runtime/*23000001*/]System.EventHandler/*0100001E*/ 'value') cil managed
  // SIG: 20 01 01 12 79
  {
    // Method begins at RVA 0x20c8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Component::remove_Disposed

  .method /*06000024*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ca
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Component::Dispose

  .method /*06000025*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20cc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Component::Dispose

  .method /*06000026*/ family hidebysig virtual 
          instance void  Finalize() cil managed
  // SIG: 20 00 01
  {
    .override [System.Runtime/*23000001*/]System.Object/*01000019*/::Finalize /*01000019::0A000016*/ 
    // Method begins at RVA 0x20d0
    // Code size       10 (0xa)
    .maxstack  1
    .try
    {
      IL_0000:  /* DE   | 07               */ leave.s    IL_0009

    }  // end .try
    finally
    {
      IL_0002:  /* 02   |                  */ ldarg.0
      IL_0003:  /* 28   | (0A)000016       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::Finalize() /* 0A000016 */
      IL_0008:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 00 00 02 02 00 07 00 00 00 00
    IL_0009:  /* 2A   |                  */ ret
  } // end of method Component::Finalize

  .method /*06000027*/ family hidebysig newslot virtual 
          instance object  GetService(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ service) cil managed
  // SIG: 20 01 1C 12 7D
  {
    .custom /*0C00007C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .param [0]/*0800000B*/ 
    .custom /*0C00003D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Component::GetService

  .method /*06000028*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00007D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Component::ToString

  .event /*14000001*/ [System.Runtime/*23000001*/]System.EventHandler/*0100001E*/ Disposed
  {
    .custom /*0C000004:06000001*/ instance void System.ComponentModel.BrowsableAttribute/*02000002*/::.ctor(bool) /* 06000001 */ = ( 01 00 00 00 00 ) 
    .custom /*0C000005:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001D*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001C*/) /* 0A000017 */ = ( 01 00 02 00 00 00 00 00 ) 
    .addon instance void System.ComponentModel.Component/*02000004*/::add_Disposed(class [System.Runtime/*23000001*/]System.EventHandler/*0100001E*/) /* 06000022 */
    .removeon instance void System.ComponentModel.Component/*02000004*/::remove_Disposed(class [System.Runtime/*23000001*/]System.EventHandler/*0100001E*/) /* 06000023 */
  } // end of event Component::Disposed
  .property /*17000011*/ instance callconv(8) bool
          CanRaiseEvents()
  {
    .get instance bool System.ComponentModel.Component/*02000004*/::get_CanRaiseEvents() /* 0600001C */
  } // end of property Component::CanRaiseEvents
  .property /*17000012*/ instance callconv(8) class System.ComponentModel.IContainer/*02000010*/
          Container()
  {
    .custom /*0C000052:06000001*/ instance void System.ComponentModel.BrowsableAttribute/*02000002*/::.ctor(bool) /* 06000001 */ = ( 01 00 00 00 00 ) 
    .custom /*0C000053:06000046*/ instance void System.ComponentModel.DesignerSerializationVisibilityAttribute/*0200000A*/::.ctor(valuetype System.ComponentModel.DesignerSerializationVisibility/*02000009*/) /* 06000046 */ = ( 01 00 00 00 00 00 00 00 ) 
    .get instance class System.ComponentModel.IContainer/*02000010*/ System.ComponentModel.Component/*02000004*/::get_Container() /* 0600001D */
  } // end of property Component::Container
  .property /*17000013*/ instance callconv(8) bool
          DesignMode()
  {
    .custom /*0C000056:06000001*/ instance void System.ComponentModel.BrowsableAttribute/*02000002*/::.ctor(bool) /* 06000001 */ = ( 01 00 00 00 00 ) 
    .custom /*0C000057:06000046*/ instance void System.ComponentModel.DesignerSerializationVisibilityAttribute/*0200000A*/::.ctor(valuetype System.ComponentModel.DesignerSerializationVisibility/*02000009*/) /* 06000046 */ = ( 01 00 00 00 00 00 00 00 ) 
    .get instance bool System.ComponentModel.Component/*02000004*/::get_DesignMode() /* 0600001E */
  } // end of property Component::DesignMode
  .property /*17000014*/ instance callconv(8) class System.ComponentModel.EventHandlerList/*0200000E*/
          Events()
  {
    .custom /*0C00005B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .get instance class System.ComponentModel.EventHandlerList/*0200000E*/ System.ComponentModel.Component/*02000004*/::get_Events() /* 0600001F */
  } // end of property Component::Events
  .property /*17000015*/ instance callconv(8) class System.ComponentModel.ISite/*02000015*/
          Site()
  {
    .custom /*0C00005D:06000001*/ instance void System.ComponentModel.BrowsableAttribute/*02000002*/::.ctor(bool) /* 06000001 */ = ( 01 00 00 00 00 ) 
    .custom /*0C00005E:06000046*/ instance void System.ComponentModel.DesignerSerializationVisibilityAttribute/*0200000A*/::.ctor(valuetype System.ComponentModel.DesignerSerializationVisibility/*02000009*/) /* 06000046 */ = ( 01 00 00 00 00 00 00 00 ) 
    .get instance class System.ComponentModel.ISite/*02000015*/ System.ComponentModel.Component/*02000004*/::get_Site() /* 06000020 */
    .set instance void System.ComponentModel.Component/*02000004*/::set_Site(class System.ComponentModel.ISite/*02000015*/) /* 06000021 */
  } // end of property Component::Site
} // end of class System.ComponentModel.Component

.class /*02000005*/ public auto ansi beforefieldinit System.ComponentModel.ComponentCollection
       extends [System.Collections.NonGeneric/*23000002*/]System.Collections.ReadOnlyCollectionBase/*01000021*/
{
  .custom /*0C000028:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000029:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00002A:0A000018*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000020*/::.ctor(string) /* 0A000018 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .method /*06000029*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.ComponentModel.IComponent/*0200000F*/[] components) cil managed
  // SIG: 20 01 01 1D 12 3C
  {
    .custom /*0C000080:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20fe
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Collections.NonGeneric/*23000002*/]System.Collections.ReadOnlyCollectionBase/*01000021*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ComponentCollection::.ctor

  .method /*0600002A*/ public hidebysig newslot specialname virtual 
          instance class System.ComponentModel.IComponent/*0200000F*/ 
          get_Item(int32 index) cil managed
  // SIG: 20 01 12 3C 08
  {
    // Method begins at RVA 0x2106
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComponentCollection::get_Item

  .method /*0600002B*/ public hidebysig newslot specialname virtual 
          instance class System.ComponentModel.IComponent/*0200000F*/ 
          get_Item(string name) cil managed
  // SIG: 20 01 12 3C 0E
  {
    // Method begins at RVA 0x2109
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ComponentCollection::get_Item

  .method /*0600002C*/ public hidebysig instance void 
          CopyTo(class System.ComponentModel.IComponent/*0200000F*/[] 'array',
                 int32 index) cil managed
  // SIG: 20 02 01 1D 12 3C 08
  {
    .custom /*0C000082:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x210c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ComponentCollection::CopyTo

  .property /*17000016*/ instance callconv(8) class System.ComponentModel.IComponent/*0200000F*/
          Item(int32)
  {
    .get instance class System.ComponentModel.IComponent/*0200000F*/ System.ComponentModel.ComponentCollection/*02000005*/::get_Item(int32) /* 0600002A */
  } // end of property ComponentCollection::Item
  .property /*17000017*/ instance callconv(8) class System.ComponentModel.IComponent/*0200000F*/
          Item(string)
  {
    .get instance class System.ComponentModel.IComponent/*0200000F*/ System.ComponentModel.ComponentCollection/*02000005*/::get_Item(string) /* 0600002B */
  } // end of property ComponentCollection::Item
} // end of class System.ComponentModel.ComponentCollection

.class /*02000006*/ public auto ansi beforefieldinit System.ComponentModel.DescriptionAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C00002D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00002F:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000014 */ = ( 01 00 FF 7F 00 00 00 00 ) 
  .field /*04000004*/ public static initonly class System.ComponentModel.DescriptionAttribute/*02000006*/ Default
  .method /*0600002D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x210e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DescriptionAttribute::.ctor

  .method /*0600002E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string description) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2116
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DescriptionAttribute::.ctor

  .method /*0600002F*/ public hidebysig newslot specialname virtual 
          instance string  get_Description() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x211e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DescriptionAttribute::get_Description

  .method /*06000030*/ family hidebysig specialname 
          instance string  get_DescriptionValue() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2121
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DescriptionAttribute::get_DescriptionValue

  .method /*06000031*/ family hidebysig specialname 
          instance void  set_DescriptionValue(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2124
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DescriptionAttribute::set_DescriptionValue

  .method /*06000032*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000085:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000014*/ 
    .custom /*0C00005A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000018*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2126
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DescriptionAttribute::Equals

  .method /*06000033*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2129
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DescriptionAttribute::GetHashCode

  .method /*06000034*/ public hidebysig virtual 
          instance bool  IsDefaultAttribute() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x212c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DescriptionAttribute::IsDefaultAttribute

  .property /*17000018*/ instance callconv(8) string
          Description()
  {
    .get instance string System.ComponentModel.DescriptionAttribute/*02000006*/::get_Description() /* 0600002F */
  } // end of property DescriptionAttribute::Description
  .property /*17000019*/ instance callconv(8) string
          DescriptionValue()
  {
    .get instance string System.ComponentModel.DescriptionAttribute/*02000006*/::get_DescriptionValue() /* 06000030 */
    .set instance void System.ComponentModel.DescriptionAttribute/*02000006*/::set_DescriptionValue(string) /* 06000031 */
  } // end of property DescriptionAttribute::DescriptionValue
} // end of class System.ComponentModel.DescriptionAttribute

.class /*02000007*/ public auto ansi sealed beforefieldinit System.ComponentModel.DesignerAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C000031:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000032:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000033:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000014 */ = ( 01 00 04 04 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 01 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 01 )                               // rited.
  .method /*06000035*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string designerTypeName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x212f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DesignerAttribute::.ctor

  .method /*06000036*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string designerTypeName,
                               string designerBaseTypeName) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x2137
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DesignerAttribute::.ctor

  .method /*06000037*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string designerTypeName,
                               class [System.Runtime/*23000001*/]System.Type/*0100001F*/ designerBaseType) cil managed
  // SIG: 20 02 01 0E 12 7D
  {
    // Method begins at RVA 0x213f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DesignerAttribute::.ctor

  .method /*06000038*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ designerType) cil managed
  // SIG: 20 01 01 12 7D
  {
    // Method begins at RVA 0x2147
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DesignerAttribute::.ctor

  .method /*06000039*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ designerType,
                               class [System.Runtime/*23000001*/]System.Type/*0100001F*/ designerBaseType) cil managed
  // SIG: 20 02 01 12 7D 12 7D
  {
    // Method begins at RVA 0x214f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DesignerAttribute::.ctor

  .method /*0600003A*/ public hidebysig specialname 
          instance string  get_DesignerBaseTypeName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2157
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignerAttribute::get_DesignerBaseTypeName

  .method /*0600003B*/ public hidebysig specialname 
          instance string  get_DesignerTypeName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x215a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignerAttribute::get_DesignerTypeName

  .method /*0600003C*/ public hidebysig specialname virtual 
          instance object  get_TypeId() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x215d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignerAttribute::get_TypeId

  .method /*0600003D*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000089:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2160
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignerAttribute::Equals

  .method /*0600003E*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2163
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignerAttribute::GetHashCode

  .property /*1700001A*/ instance callconv(8) string
          DesignerBaseTypeName()
  {
    .get instance string System.ComponentModel.DesignerAttribute/*02000007*/::get_DesignerBaseTypeName() /* 0600003A */
  } // end of property DesignerAttribute::DesignerBaseTypeName
  .property /*1700001B*/ instance callconv(8) string
          DesignerTypeName()
  {
    .get instance string System.ComponentModel.DesignerAttribute/*02000007*/::get_DesignerTypeName() /* 0600003B */
  } // end of property DesignerAttribute::DesignerTypeName
  .property /*1700001C*/ instance callconv(8) object
          TypeId()
  {
    .get instance object System.ComponentModel.DesignerAttribute/*02000007*/::get_TypeId() /* 0600003C */
  } // end of property DesignerAttribute::TypeId
} // end of class System.ComponentModel.DesignerAttribute

.class /*02000008*/ public auto ansi sealed beforefieldinit System.ComponentModel.DesignerCategoryAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C000034:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000035:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000036:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000014 */ = ( 01 00 04 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 01 )                               // rited.
  .field /*04000005*/ public static initonly class System.ComponentModel.DesignerCategoryAttribute/*02000008*/ Component
  .field /*04000006*/ public static initonly class System.ComponentModel.DesignerCategoryAttribute/*02000008*/ Default
  .field /*04000007*/ public static initonly class System.ComponentModel.DesignerCategoryAttribute/*02000008*/ Form
  .field /*04000008*/ public static initonly class System.ComponentModel.DesignerCategoryAttribute/*02000008*/ Generic
  .method /*0600003F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2166
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DesignerCategoryAttribute::.ctor

  .method /*06000040*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string category) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x216e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DesignerCategoryAttribute::.ctor

  .method /*06000041*/ public hidebysig specialname 
          instance string  get_Category() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2176
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignerCategoryAttribute::get_Category

  .method /*06000042*/ public hidebysig specialname virtual 
          instance object  get_TypeId() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x2179
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignerCategoryAttribute::get_TypeId

  .method /*06000043*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00008B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800001F*/ 
    .custom /*0C000075:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000018*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x217c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignerCategoryAttribute::Equals

  .method /*06000044*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x217f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignerCategoryAttribute::GetHashCode

  .method /*06000045*/ public hidebysig virtual 
          instance bool  IsDefaultAttribute() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2182
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignerCategoryAttribute::IsDefaultAttribute

  .property /*1700001D*/ instance callconv(8) string
          Category()
  {
    .get instance string System.ComponentModel.DesignerCategoryAttribute/*02000008*/::get_Category() /* 06000041 */
  } // end of property DesignerCategoryAttribute::Category
  .property /*1700001E*/ instance callconv(8) object
          TypeId()
  {
    .get instance object System.ComponentModel.DesignerCategoryAttribute/*02000008*/::get_TypeId() /* 06000042 */
  } // end of property DesignerCategoryAttribute::TypeId
} // end of class System.ComponentModel.DesignerCategoryAttribute

.class /*02000009*/ public auto ansi sealed System.ComponentModel.DesignerSerializationVisibility
       extends [System.Runtime/*23000001*/]System.Enum/*01000022*/
{
  .field /*04000009*/ public specialname rtspecialname int32 value__
  .field /*0400000A*/ public static literal valuetype System.ComponentModel.DesignerSerializationVisibility/*02000009*/ Hidden = int32(0x00000000)
  .field /*0400000B*/ public static literal valuetype System.ComponentModel.DesignerSerializationVisibility/*02000009*/ Visible = int32(0x00000001)
  .field /*0400000C*/ public static literal valuetype System.ComponentModel.DesignerSerializationVisibility/*02000009*/ Content = int32(0x00000002)
} // end of class System.ComponentModel.DesignerSerializationVisibility

.class /*0200000A*/ public auto ansi sealed beforefieldinit System.ComponentModel.DesignerSerializationVisibilityAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C000037:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000038:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000039:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000014 */ = ( 01 00 C0 03 00 00 00 00 ) 
  .field /*0400000D*/ public static initonly class System.ComponentModel.DesignerSerializationVisibilityAttribute/*0200000A*/ Content
  .field /*0400000E*/ public static initonly class System.ComponentModel.DesignerSerializationVisibilityAttribute/*0200000A*/ Default
  .field /*0400000F*/ public static initonly class System.ComponentModel.DesignerSerializationVisibilityAttribute/*0200000A*/ Hidden
  .field /*04000010*/ public static initonly class System.ComponentModel.DesignerSerializationVisibilityAttribute/*0200000A*/ Visible
  .method /*06000046*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.ComponentModel.DesignerSerializationVisibility/*02000009*/ visibility) cil managed
  // SIG: 20 01 01 11 24
  {
    // Method begins at RVA 0x2185
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DesignerSerializationVisibilityAttribute::.ctor

  .method /*06000047*/ public hidebysig specialname 
          instance valuetype System.ComponentModel.DesignerSerializationVisibility/*02000009*/ 
          get_Visibility() cil managed
  // SIG: 20 00 11 24
  {
    // Method begins at RVA 0x218d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignerSerializationVisibilityAttribute::get_Visibility

  .method /*06000048*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00008C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000021*/ 
    .custom /*0C000077:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000018*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2190
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignerSerializationVisibilityAttribute::Equals

  .method /*06000049*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2193
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignerSerializationVisibilityAttribute::GetHashCode

  .method /*0600004A*/ public hidebysig virtual 
          instance bool  IsDefaultAttribute() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2196
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignerSerializationVisibilityAttribute::IsDefaultAttribute

  .property /*1700001F*/ instance callconv(8) valuetype System.ComponentModel.DesignerSerializationVisibility/*02000009*/
          Visibility()
  {
    .get instance valuetype System.ComponentModel.DesignerSerializationVisibility/*02000009*/ System.ComponentModel.DesignerSerializationVisibilityAttribute/*0200000A*/::get_Visibility() /* 06000047 */
  } // end of property DesignerSerializationVisibilityAttribute::Visibility
} // end of class System.ComponentModel.DesignerSerializationVisibilityAttribute

.class /*0200000B*/ public auto ansi sealed beforefieldinit System.ComponentModel.DesignOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C00003A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00003C:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000014 */ = ( 01 00 FF 7F 00 00 00 00 ) 
  .field /*04000011*/ public static initonly class System.ComponentModel.DesignOnlyAttribute/*0200000B*/ Default
  .field /*04000012*/ public static initonly class System.ComponentModel.DesignOnlyAttribute/*0200000B*/ No
  .field /*04000013*/ public static initonly class System.ComponentModel.DesignOnlyAttribute/*0200000B*/ Yes
  .method /*0600004B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool isDesignOnly) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2199
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DesignOnlyAttribute::.ctor

  .method /*0600004C*/ public hidebysig specialname 
          instance bool  get_IsDesignOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignOnlyAttribute::get_IsDesignOnly

  .method /*0600004D*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00008E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000023*/ 
    .custom /*0C000078:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000018*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignOnlyAttribute::Equals

  .method /*0600004E*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignOnlyAttribute::GetHashCode

  .method /*0600004F*/ public hidebysig virtual 
          instance bool  IsDefaultAttribute() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignOnlyAttribute::IsDefaultAttribute

  .property /*17000020*/ instance callconv(8) bool
          IsDesignOnly()
  {
    .get instance bool System.ComponentModel.DesignOnlyAttribute/*0200000B*/::get_IsDesignOnly() /* 0600004C */
  } // end of property DesignOnlyAttribute::IsDesignOnly
} // end of class System.ComponentModel.DesignOnlyAttribute

.class /*0200000C*/ public auto ansi beforefieldinit System.ComponentModel.DisplayNameAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C00003E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000040:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000014 */ = ( 01 00 C4 02 00 00 00 00 ) 
  .field /*04000014*/ public static initonly class System.ComponentModel.DisplayNameAttribute/*0200000C*/ Default
  .method /*06000050*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21ad
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DisplayNameAttribute::.ctor

  .method /*06000051*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string displayName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21b5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DisplayNameAttribute::.ctor

  .method /*06000052*/ public hidebysig newslot specialname virtual 
          instance string  get_DisplayName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DisplayNameAttribute::get_DisplayName

  .method /*06000053*/ family hidebysig specialname 
          instance string  get_DisplayNameValue() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DisplayNameAttribute::get_DisplayNameValue

  .method /*06000054*/ family hidebysig specialname 
          instance void  set_DisplayNameValue(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21c3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DisplayNameAttribute::set_DisplayNameValue

  .method /*06000055*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000092:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000026*/ 
    .custom /*0C00007A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000018*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DisplayNameAttribute::Equals

  .method /*06000056*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DisplayNameAttribute::GetHashCode

  .method /*06000057*/ public hidebysig virtual 
          instance bool  IsDefaultAttribute() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DisplayNameAttribute::IsDefaultAttribute

  .property /*17000021*/ instance callconv(8) string
          DisplayName()
  {
    .get instance string System.ComponentModel.DisplayNameAttribute/*0200000C*/::get_DisplayName() /* 06000052 */
  } // end of property DisplayNameAttribute::DisplayName
  .property /*17000022*/ instance callconv(8) string
          DisplayNameValue()
  {
    .get instance string System.ComponentModel.DisplayNameAttribute/*0200000C*/::get_DisplayNameValue() /* 06000053 */
    .set instance void System.ComponentModel.DisplayNameAttribute/*0200000C*/::set_DisplayNameValue(string) /* 06000054 */
  } // end of property DisplayNameAttribute::DisplayNameValue
} // end of class System.ComponentModel.DisplayNameAttribute

.class /*0200000D*/ public auto ansi sealed beforefieldinit System.ComponentModel.EditorAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C000041:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000042:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000043:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000014 */ = ( 01 00 FF 7F 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 01 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 01 )                               // rited.
  .method /*06000058*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21ce
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EditorAttribute::.ctor

  .method /*06000059*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string typeName,
                               string baseTypeName) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .param [2]/*08000028*/ 
    .custom /*0C00007E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EditorAttribute::.ctor

  .method /*0600005A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string typeName,
                               class [System.Runtime/*23000001*/]System.Type/*0100001F*/ baseType) cil managed
  // SIG: 20 02 01 0E 12 7D
  {
    // Method begins at RVA 0x21de
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EditorAttribute::.ctor

  .method /*0600005B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ 'type',
                               class [System.Runtime/*23000001*/]System.Type/*0100001F*/ baseType) cil managed
  // SIG: 20 02 01 12 7D 12 7D
  {
    // Method begins at RVA 0x21e6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EditorAttribute::.ctor

  .method /*0600005C*/ public hidebysig specialname 
          instance string  get_EditorBaseTypeName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000096:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditorAttribute::get_EditorBaseTypeName

  .method /*0600005D*/ public hidebysig specialname 
          instance string  get_EditorTypeName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditorAttribute::get_EditorTypeName

  .method /*0600005E*/ public hidebysig specialname virtual 
          instance object  get_TypeId() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x21f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditorAttribute::get_TypeId

  .method /*0600005F*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000099:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditorAttribute::Equals

  .method /*06000060*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EditorAttribute::GetHashCode

  .property /*17000023*/ instance callconv(8) string
          EditorBaseTypeName()
  {
    .custom /*0C000079:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance string System.ComponentModel.EditorAttribute/*0200000D*/::get_EditorBaseTypeName() /* 0600005C */
  } // end of property EditorAttribute::EditorBaseTypeName
  .property /*17000024*/ instance callconv(8) string
          EditorTypeName()
  {
    .get instance string System.ComponentModel.EditorAttribute/*0200000D*/::get_EditorTypeName() /* 0600005D */
  } // end of property EditorAttribute::EditorTypeName
  .property /*17000025*/ instance callconv(8) object
          TypeId()
  {
    .get instance object System.ComponentModel.EditorAttribute/*0200000D*/::get_TypeId() /* 0600005E */
  } // end of property EditorAttribute::TypeId
} // end of class System.ComponentModel.EditorAttribute

.class /*0200000E*/ public auto ansi sealed beforefieldinit System.ComponentModel.EventHandlerList
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
{
  .custom /*0C000044:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000045:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000046:0A000018*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000020*/::.ctor(string) /* 0A000018 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
  .custom /*0C000022:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000061*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21fd
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EventHandlerList::.ctor

  .method /*06000062*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Delegate/*01000023*/ 
          get_Item(object key) cil managed
  // SIG: 20 01 12 80 8D 1C
  {
    .param [0]/*0800002E*/ 
    .custom /*0C000083:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2205
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EventHandlerList::get_Item

  .method /*06000063*/ public hidebysig specialname 
          instance void  set_Item(object key,
                                  class [System.Runtime/*23000001*/]System.Delegate/*01000023*/ 'value') cil managed
  // SIG: 20 02 01 1C 12 80 8D
  {
    .param [2]/*08000031*/ 
    .custom /*0C000084:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2208
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EventHandlerList::set_Item

  .method /*06000064*/ public hidebysig instance void 
          AddHandler(object key,
                     class [System.Runtime/*23000001*/]System.Delegate/*01000023*/ 'value') cil managed
  // SIG: 20 02 01 1C 12 80 8D
  {
    .param [2]/*08000033*/ 
    .custom /*0C000086:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x220a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EventHandlerList::AddHandler

  .method /*06000065*/ public hidebysig instance void 
          AddHandlers(class System.ComponentModel.EventHandlerList/*0200000E*/ listToAddFrom) cil managed
  // SIG: 20 01 01 12 38
  {
    // Method begins at RVA 0x220c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EventHandlerList::AddHandlers

  .method /*06000066*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x220e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EventHandlerList::Dispose

  .method /*06000067*/ public hidebysig instance void 
          RemoveHandler(object key,
                        class [System.Runtime/*23000001*/]System.Delegate/*01000023*/ 'value') cil managed
  // SIG: 20 02 01 1C 12 80 8D
  {
    .param [2]/*08000036*/ 
    .custom /*0C000087:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2210
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method EventHandlerList::RemoveHandler

  .property /*17000026*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Delegate/*01000023*/
          Item(object)
  {
    .custom /*0C00007B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Delegate/*01000023*/ System.ComponentModel.EventHandlerList/*0200000E*/::get_Item(object) /* 06000062 */
    .set instance void System.ComponentModel.EventHandlerList/*0200000E*/::set_Item(object,
                                                                                    class [System.Runtime/*23000001*/]System.Delegate/*01000023*/) /* 06000063 */
  } // end of property EventHandlerList::Item
} // end of class System.ComponentModel.EventHandlerList

.class /*0200000F*/ interface public abstract auto ansi beforefieldinit System.ComponentModel.IComponent
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
{
  .custom /*0C000047:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000048:06000035*/ instance void System.ComponentModel.DesignerAttribute/*02000007*/::.ctor(string) /* 06000035 */ = ( 01 00 80 80 53 79 73 74 65 6D 2E 43 6F 6D 70 6F   // ....System.Compo
                                                                                                                                    6E 65 6E 74 4D 6F 64 65 6C 2E 44 65 73 69 67 6E   // nentModel.Design
                                                                                                                                    2E 43 6F 6D 70 6F 6E 65 6E 74 44 65 73 69 67 6E   // .ComponentDesign
                                                                                                                                    65 72 2C 20 53 79 73 74 65 6D 2E 44 65 73 69 67   // er, System.Desig
                                                                                                                                    6E 2C 20 56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30   // n, Version=4.0.0
                                                                                                                                    2E 30 2C 20 43 75 6C 74 75 72 65 3D 6E 65 75 74   // .0, Culture=neut
                                                                                                                                    72 61 6C 2C 20 50 75 62 6C 69 63 4B 65 79 54 6F   // ral, PublicKeyTo
                                                                                                                                    6B 65 6E 3D 62 30 33 66 35 66 37 66 31 31 64 35   // ken=b03f5f7f11d5
                                                                                                                                    30 61 33 61 00 00 )                               // 0a3a..
  .custom /*0C000049:06000036*/ instance void System.ComponentModel.DesignerAttribute/*02000007*/::.ctor(string,
                                                                                                         string) /* 06000036 */ = ( 01 00 80 87 53 79 73 74 65 6D 2E 57 69 6E 64 6F   // ....System.Windo
                                                                                                                                    77 73 2E 46 6F 72 6D 73 2E 44 65 73 69 67 6E 2E   // ws.Forms.Design.
                                                                                                                                    43 6F 6D 70 6F 6E 65 6E 74 44 6F 63 75 6D 65 6E   // ComponentDocumen
                                                                                                                                    74 44 65 73 69 67 6E 65 72 2C 20 53 79 73 74 65   // tDesigner, Syste
                                                                                                                                    6D 2E 44 65 73 69 67 6E 2C 20 56 65 72 73 69 6F   // m.Design, Versio
                                                                                                                                    6E 3D 34 2E 30 2E 30 2E 30 2C 20 43 75 6C 74 75   // n=4.0.0.0, Cultu
                                                                                                                                    72 65 3D 6E 65 75 74 72 61 6C 2C 20 50 75 62 6C   // re=neutral, Publ
                                                                                                                                    69 63 4B 65 79 54 6F 6B 65 6E 3D 62 30 33 66 35   // icKeyToken=b03f5
                                                                                                                                    66 37 66 31 31 64 35 30 61 33 61 75 53 79 73 74   // f7f11d50a3auSyst
                                                                                                                                    65 6D 2E 43 6F 6D 70 6F 6E 65 6E 74 4D 6F 64 65   // em.ComponentMode
                                                                                                                                    6C 2E 44 65 73 69 67 6E 2E 49 52 6F 6F 74 44 65   // l.Design.IRootDe
                                                                                                                                    73 69 67 6E 65 72 2C 20 53 79 73 74 65 6D 2C 20   // signer, System, 
                                                                                                                                    56 65 72 73 69 6F 6E 3D 34 2E 30 2E 30 2E 30 2C   // Version=4.0.0.0,
                                                                                                                                    20 43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C   //  Culture=neutral
                                                                                                                                    2C 20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E   // , PublicKeyToken
                                                                                                                                    3D 62 37 37 61 35 63 35 36 31 39 33 34 65 30 38   // =b77a5c561934e08
                                                                                                                                    39 00 00 )                                        // 9..
  .custom /*0C00004A:0A000019*/ instance void [System.ObjectModel/*23000003*/]System.ComponentModel.TypeConverterAttribute/*01000024*/::.ctor(string) /* 0A000019 */ = ( 01 00 73 53 79 73 74 65 6D 2E 43 6F 6D 70 6F 6E   // ..sSystem.Compon
                                                                                                                                                                         65 6E 74 4D 6F 64 65 6C 2E 43 6F 6D 70 6F 6E 65   // entModel.Compone
                                                                                                                                                                         6E 74 43 6F 6E 76 65 72 74 65 72 2C 20 53 79 73   // ntConverter, Sys
                                                                                                                                                                         74 65 6D 2C 20 56 65 72 73 69 6F 6E 3D 34 2E 30   // tem, Version=4.0
                                                                                                                                                                         2E 30 2E 30 2C 20 43 75 6C 74 75 72 65 3D 6E 65   // .0.0, Culture=ne
                                                                                                                                                                         75 74 72 61 6C 2C 20 50 75 62 6C 69 63 4B 65 79   // utral, PublicKey
                                                                                                                                                                         54 6F 6B 65 6E 3D 62 37 37 61 35 63 35 36 31 39   // Token=b77a5c5619
                                                                                                                                                                         33 34 65 30 38 39 00 00 )                         // 34e089..
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
  .custom /*0C000027:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000068*/ public hidebysig newslot specialname abstract virtual 
          instance class System.ComponentModel.ISite/*02000015*/ 
          get_Site() cil managed
  // SIG: 20 00 12 54
  {
    // Method begins at RVA 0x0
  } // end of method IComponent::get_Site

  .method /*06000069*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_Site(class System.ComponentModel.ISite/*02000015*/ 'value') cil managed
  // SIG: 20 01 01 12 54
  {
    // Method begins at RVA 0x0
  } // end of method IComponent::set_Site

  .method /*0600006A*/ public hidebysig newslot specialname abstract virtual 
          instance void  add_Disposed(class [System.Runtime/*23000001*/]System.EventHandler/*0100001E*/ 'value') cil managed
  // SIG: 20 01 01 12 79
  {
    .custom /*0C00009C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000025*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IComponent::add_Disposed

  .method /*0600006B*/ public hidebysig newslot specialname abstract virtual 
          instance void  remove_Disposed(class [System.Runtime/*23000001*/]System.EventHandler/*0100001E*/ 'value') cil managed
  // SIG: 20 01 01 12 79
  {
    .custom /*0C00009D:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000025*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IComponent::remove_Disposed

  .event /*14000002*/ [System.Runtime/*23000001*/]System.EventHandler/*0100001E*/ Disposed
  {
    .addon instance void System.ComponentModel.IComponent/*0200000F*/::add_Disposed(class [System.Runtime/*23000001*/]System.EventHandler/*0100001E*/) /* 0600006A */
    .removeon instance void System.ComponentModel.IComponent/*0200000F*/::remove_Disposed(class [System.Runtime/*23000001*/]System.EventHandler/*0100001E*/) /* 0600006B */
  } // end of event IComponent::Disposed
  .property /*17000027*/ instance callconv(8) class System.ComponentModel.ISite/*02000015*/
          Site()
  {
    .get instance class System.ComponentModel.ISite/*02000015*/ System.ComponentModel.IComponent/*0200000F*/::get_Site() /* 06000068 */
    .set instance void System.ComponentModel.IComponent/*0200000F*/::set_Site(class System.ComponentModel.ISite/*02000015*/) /* 06000069 */
  } // end of property IComponent::Site
} // end of class System.ComponentModel.IComponent

.class /*02000010*/ interface public abstract auto ansi beforefieldinit System.ComponentModel.IContainer
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
{
  .custom /*0C00004B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
  .custom /*0C00002C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*0600006C*/ public hidebysig newslot specialname abstract virtual 
          instance class System.ComponentModel.ComponentCollection/*02000005*/ 
          get_Components() cil managed
  // SIG: 20 00 12 14
  {
    .custom /*0C00009E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IContainer::get_Components

  .method /*0600006D*/ public hidebysig newslot abstract virtual 
          instance void  Add(class System.ComponentModel.IComponent/*0200000F*/ component) cil managed
  // SIG: 20 01 01 12 3C
  {
    // Method begins at RVA 0x0
  } // end of method IContainer::Add

  .method /*0600006E*/ public hidebysig newslot abstract virtual 
          instance void  Add(class System.ComponentModel.IComponent/*0200000F*/ component,
                             string name) cil managed
  // SIG: 20 02 01 12 3C 0E
  {
    .custom /*0C00009F:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000026*/::.ctor(string) /* 0A00001B */ = ( 01 00 5D 54 68 65 20 54 79 70 65 20 6F 66 20 63   // ..]The Type of c
                                                                                                                                                                                            6F 6D 70 6F 6E 65 6E 74 73 20 69 6E 20 74 68 65   // omponents in the
                                                                                                                                                                                            20 63 6F 6E 74 61 69 6E 65 72 20 63 61 6E 6E 6F   //  container canno
                                                                                                                                                                                            74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79 20   // t be statically 
                                                                                                                                                                                            64 69 73 63 6F 76 65 72 65 64 20 74 6F 20 76 61   // discovered to va
                                                                                                                                                                                            6C 69 64 61 74 65 20 74 68 65 20 6E 61 6D 65 2E   // lidate the name.
                                                                                                                                                                                            00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IContainer::Add

  .method /*0600006F*/ public hidebysig newslot abstract virtual 
          instance void  Remove(class System.ComponentModel.IComponent/*0200000F*/ component) cil managed
  // SIG: 20 01 01 12 3C
  {
    // Method begins at RVA 0x0
  } // end of method IContainer::Remove

  .property /*17000028*/ instance callconv(8) class System.ComponentModel.ComponentCollection/*02000005*/
          Components()
  {
    .custom /*0C00007F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .get instance class System.ComponentModel.ComponentCollection/*02000005*/ System.ComponentModel.IContainer/*02000010*/::get_Components() /* 0600006C */
  } // end of property IContainer::Components
} // end of class System.ComponentModel.IContainer

.class /*02000011*/ public auto ansi sealed beforefieldinit System.ComponentModel.ImmutableObjectAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C00004C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00004E:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000014 */ = ( 01 00 FF 7F 00 00 00 00 ) 
  .field /*04000015*/ public static initonly class System.ComponentModel.ImmutableObjectAttribute/*02000011*/ Default
  .field /*04000016*/ public static initonly class System.ComponentModel.ImmutableObjectAttribute/*02000011*/ No
  .field /*04000017*/ public static initonly class System.ComponentModel.ImmutableObjectAttribute/*02000011*/ Yes
  .method /*06000070*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool immutable) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2212
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ImmutableObjectAttribute::.ctor

  .method /*06000071*/ public hidebysig specialname 
          instance bool  get_Immutable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x221a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ImmutableObjectAttribute::get_Immutable

  .method /*06000072*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000A0:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800003F*/ 
    .custom /*0C00008A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000018*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x221d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ImmutableObjectAttribute::Equals

  .method /*06000073*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2220
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ImmutableObjectAttribute::GetHashCode

  .method /*06000074*/ public hidebysig virtual 
          instance bool  IsDefaultAttribute() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2223
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ImmutableObjectAttribute::IsDefaultAttribute

  .property /*17000029*/ instance callconv(8) bool
          Immutable()
  {
    .get instance bool System.ComponentModel.ImmutableObjectAttribute/*02000011*/::get_Immutable() /* 06000071 */
  } // end of property ImmutableObjectAttribute::Immutable
} // end of class System.ComponentModel.ImmutableObjectAttribute

.class /*02000012*/ public auto ansi sealed beforefieldinit System.ComponentModel.InitializationEventAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C00004F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000050:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000051:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000014 */ = ( 01 00 04 00 00 00 00 00 ) 
  .method /*06000075*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string eventName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2226
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InitializationEventAttribute::.ctor

  .method /*06000076*/ public hidebysig specialname 
          instance string  get_EventName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x222e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InitializationEventAttribute::get_EventName

  .property /*1700002A*/ instance callconv(8) string
          EventName()
  {
    .get instance string System.ComponentModel.InitializationEventAttribute/*02000012*/::get_EventName() /* 06000076 */
  } // end of property InitializationEventAttribute::EventName
} // end of class System.ComponentModel.InitializationEventAttribute

.class /*02000013*/ public auto ansi beforefieldinit System.ComponentModel.InvalidAsynchronousStateException
       extends [System.Runtime/*23000001*/]System.ArgumentException/*01000027*/
{
  .custom /*0C000054:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000055:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000077*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2231
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.ArgumentException/*01000027*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidAsynchronousStateException::.ctor

  .method /*06000078*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000029*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100002A*/ context) cil managed
  // SIG: 20 02 01 12 80 A5 11 80 A9
  {
    .custom /*0C0000A1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000A2:0A00001C*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000028*/::.ctor(string) /* 0A00001C */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
                                                                                                                                                   6F 72 74 73 20 6F 62 73 6F 6C 65 74 65 20 66 6F   // orts obsolete fo
                                                                                                                                                   72 6D 61 74 74 65 72 2D 62 61 73 65 64 20 73 65   // rmatter-based se
                                                                                                                                                   72 69 61 6C 69 7A 61 74 69 6F 6E 2E 20 49 74 20   // rialization. It 
                                                                                                                                                   73 68 6F 75 6C 64 20 6E 6F 74 20 62 65 20 63 61   // should not be ca
                                                                                                                                                   6C 6C 65 64 20 6F 72 20 65 78 74 65 6E 64 65 64   // lled or extended
                                                                                                                                                   20 62 79 20 61 70 70 6C 69 63 61 74 69 6F 6E 20   //  by application 
                                                                                                                                                   63 6F 64 65 2E 02 00 54 0E 0C 44 69 61 67 6E 6F   // code...T..Diagno
                                                                                                                                                   73 74 69 63 49 64 0A 53 59 53 4C 49 42 30 30 35   // sticId.SYSLIB005
                                                                                                                                                   31 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22 68 74   // 1T..UrlFormat"ht
                                                                                                                                                   74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64 6F 74   // tps://aka.ms/dot
                                                                                                                                                   6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B 30 7D ) // net-warnings/{0}
    .custom /*0C0000A3:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001D*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001C*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2239
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.ArgumentException/*01000027*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidAsynchronousStateException::.ctor

  .method /*06000079*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2241
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.ArgumentException/*01000027*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidAsynchronousStateException::.ctor

  .method /*0600007A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*0100002B*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 80 AD
  {
    // Method begins at RVA 0x2249
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.ArgumentException/*01000027*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidAsynchronousStateException::.ctor

} // end of class System.ComponentModel.InvalidAsynchronousStateException

.class /*02000014*/ public auto ansi beforefieldinit System.ComponentModel.InvalidEnumArgumentException
       extends [System.Runtime/*23000001*/]System.ArgumentException/*01000027*/
{
  .custom /*0C000058:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000059:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*0600007B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2251
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.ArgumentException/*01000027*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidEnumArgumentException::.ctor

  .method /*0600007C*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000029*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100002A*/ context) cil managed
  // SIG: 20 02 01 12 80 A5 11 80 A9
  {
    .custom /*0C0000A4:0A00001C*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000028*/::.ctor(string) /* 0A00001C */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
                                                                                                                                                   6F 72 74 73 20 6F 62 73 6F 6C 65 74 65 20 66 6F   // orts obsolete fo
                                                                                                                                                   72 6D 61 74 74 65 72 2D 62 61 73 65 64 20 73 65   // rmatter-based se
                                                                                                                                                   72 69 61 6C 69 7A 61 74 69 6F 6E 2E 20 49 74 20   // rialization. It 
                                                                                                                                                   73 68 6F 75 6C 64 20 6E 6F 74 20 62 65 20 63 61   // should not be ca
                                                                                                                                                   6C 6C 65 64 20 6F 72 20 65 78 74 65 6E 64 65 64   // lled or extended
                                                                                                                                                   20 62 79 20 61 70 70 6C 69 63 61 74 69 6F 6E 20   //  by application 
                                                                                                                                                   63 6F 64 65 2E 02 00 54 0E 0C 44 69 61 67 6E 6F   // code...T..Diagno
                                                                                                                                                   73 74 69 63 49 64 0A 53 59 53 4C 49 42 30 30 35   // sticId.SYSLIB005
                                                                                                                                                   31 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22 68 74   // 1T..UrlFormat"ht
                                                                                                                                                   74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64 6F 74   // tps://aka.ms/dot
                                                                                                                                                   6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B 30 7D ) // net-warnings/{0}
    .custom /*0C0000A5:0A000017*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001D*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001C*/) /* 0A000017 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2259
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.ArgumentException/*01000027*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidEnumArgumentException::.ctor

  .method /*0600007D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000A6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2261
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.ArgumentException/*01000027*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidEnumArgumentException::.ctor

  .method /*0600007E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*0100002B*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 80 AD
  {
    .custom /*0C0000A7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2269
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.ArgumentException/*01000027*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidEnumArgumentException::.ctor

  .method /*0600007F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string argumentName,
                               int32 invalidValue,
                               class [System.Runtime/*23000001*/]System.Type/*0100001F*/ enumClass) cil managed
  // SIG: 20 03 01 0E 08 12 7D
  {
    .param [1]/*0800004B*/ 
    .custom /*0C00008D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2271
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Runtime/*23000001*/]System.ArgumentException/*01000027*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidEnumArgumentException::.ctor

} // end of class System.ComponentModel.InvalidEnumArgumentException

.class /*02000015*/ interface public abstract auto ansi beforefieldinit System.ComponentModel.ISite
       implements [System.ComponentModel/*23000004*/]System.IServiceProvider/*0100002C*/
{
  .custom /*0C00005C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .interfaceimpl type [System.ComponentModel/*23000004*/]System.IServiceProvider/*0100002C*/
  .custom /*0C000030:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .method /*06000080*/ public hidebysig newslot specialname abstract virtual 
          instance class System.ComponentModel.IComponent/*0200000F*/ 
          get_Component() cil managed
  // SIG: 20 00 12 3C
  {
    .custom /*0C0000A8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ISite::get_Component

  .method /*06000081*/ public hidebysig newslot specialname abstract virtual 
          instance class System.ComponentModel.IContainer/*02000010*/ 
          get_Container() cil managed
  // SIG: 20 00 12 40
  {
    // Method begins at RVA 0x0
  } // end of method ISite::get_Container

  .method /*06000082*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_DesignMode() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method ISite::get_DesignMode

  .method /*06000083*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method ISite::get_Name

  .method /*06000084*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000A9:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000026*/::.ctor(string) /* 0A00001B */ = ( 01 00 5D 54 68 65 20 54 79 70 65 20 6F 66 20 63   // ..]The Type of c
                                                                                                                                                                                            6F 6D 70 6F 6E 65 6E 74 73 20 69 6E 20 74 68 65   // omponents in the
                                                                                                                                                                                            20 63 6F 6E 74 61 69 6E 65 72 20 63 61 6E 6E 6F   //  container canno
                                                                                                                                                                                            74 20 62 65 20 73 74 61 74 69 63 61 6C 6C 79 20   // t be statically 
                                                                                                                                                                                            64 69 73 63 6F 76 65 72 65 64 20 74 6F 20 76 61   // discovered to va
                                                                                                                                                                                            6C 69 64 61 74 65 20 74 68 65 20 6E 61 6D 65 2E   // lidate the name.
                                                                                                                                                                                            00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ISite::set_Name

  .property /*1700002B*/ instance callconv(8) class System.ComponentModel.IComponent/*0200000F*/
          Component()
  {
    .custom /*0C000081:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .get instance class System.ComponentModel.IComponent/*0200000F*/ System.ComponentModel.ISite/*02000015*/::get_Component() /* 06000080 */
  } // end of property ISite::Component
  .property /*1700002C*/ instance callconv(8) class System.ComponentModel.IContainer/*02000010*/
          Container()
  {
    .get instance class System.ComponentModel.IContainer/*02000010*/ System.ComponentModel.ISite/*02000015*/::get_Container() /* 06000081 */
  } // end of property ISite::Container
  .property /*1700002D*/ instance callconv(8) bool
          DesignMode()
  {
    .get instance bool System.ComponentModel.ISite/*02000015*/::get_DesignMode() /* 06000082 */
  } // end of property ISite::DesignMode
  .property /*1700002E*/ instance callconv(8) string
          Name()
  {
    .get instance string System.ComponentModel.ISite/*02000015*/::get_Name() /* 06000083 */
    .set instance void System.ComponentModel.ISite/*02000015*/::set_Name(string) /* 06000084 */
  } // end of property ISite::Name
} // end of class System.ComponentModel.ISite

.class /*02000016*/ interface public abstract auto ansi beforefieldinit System.ComponentModel.ISupportInitialize
{
  .method /*06000085*/ public hidebysig newslot abstract virtual 
          instance void  BeginInit() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method ISupportInitialize::BeginInit

  .method /*06000086*/ public hidebysig newslot abstract virtual 
          instance void  EndInit() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method ISupportInitialize::EndInit

} // end of class System.ComponentModel.ISupportInitialize

.class /*02000017*/ interface public abstract auto ansi beforefieldinit System.ComponentModel.ISynchronizeInvoke
{
  .custom /*0C000060:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .method /*06000087*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_InvokeRequired() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method ISynchronizeInvoke::get_InvokeRequired

  .method /*06000088*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002D*/ 
          BeginInvoke(class [System.Runtime/*23000001*/]System.Delegate/*01000023*/ 'method',
                      object[] args) cil managed
  // SIG: 20 02 12 80 B5 12 80 8D 1D 1C
  {
    .param [2]/*08000050*/ 
    .custom /*0C00008F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ISynchronizeInvoke::BeginInvoke

  .method /*06000089*/ public hidebysig newslot abstract virtual 
          instance object  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002D*/ result) cil managed
  // SIG: 20 01 1C 12 80 B5
  {
    .param [0]/*08000051*/ 
    .custom /*0C000090:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ISynchronizeInvoke::EndInvoke

  .method /*0600008A*/ public hidebysig newslot abstract virtual 
          instance object  Invoke(class [System.Runtime/*23000001*/]System.Delegate/*01000023*/ 'method',
                                  object[] args) cil managed
  // SIG: 20 02 1C 12 80 8D 1D 1C
  {
    .custom /*0C0000AA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000053*/ 
    .custom /*0C000091:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ISynchronizeInvoke::Invoke

  .property /*1700002F*/ instance callconv(8) bool
          InvokeRequired()
  {
    .get instance bool System.ComponentModel.ISynchronizeInvoke/*02000017*/::get_InvokeRequired() /* 06000087 */
  } // end of property ISynchronizeInvoke::InvokeRequired
} // end of class System.ComponentModel.ISynchronizeInvoke

.class /*02000018*/ public auto ansi sealed beforefieldinit System.ComponentModel.LocalizableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C000061:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000062:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000063:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000014 */ = ( 01 00 FF 7F 00 00 00 00 ) 
  .field /*04000018*/ public static initonly class System.ComponentModel.LocalizableAttribute/*02000018*/ Default
  .field /*04000019*/ public static initonly class System.ComponentModel.LocalizableAttribute/*02000018*/ No
  .field /*0400001A*/ public static initonly class System.ComponentModel.LocalizableAttribute/*02000018*/ Yes
  .method /*0600008B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool isLocalizable) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2279
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LocalizableAttribute::.ctor

  .method /*0600008C*/ public hidebysig specialname 
          instance bool  get_IsLocalizable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2281
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalizableAttribute::get_IsLocalizable

  .method /*0600008D*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000AB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000056*/ 
    .custom /*0C000093:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000018*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2284
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalizableAttribute::Equals

  .method /*0600008E*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2287
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalizableAttribute::GetHashCode

  .method /*0600008F*/ public hidebysig virtual 
          instance bool  IsDefaultAttribute() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x228a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalizableAttribute::IsDefaultAttribute

  .property /*17000030*/ instance callconv(8) bool
          IsLocalizable()
  {
    .get instance bool System.ComponentModel.LocalizableAttribute/*02000018*/::get_IsLocalizable() /* 0600008C */
  } // end of property LocalizableAttribute::IsLocalizable
} // end of class System.ComponentModel.LocalizableAttribute

.class /*02000019*/ public auto ansi sealed beforefieldinit System.ComponentModel.MergablePropertyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C000064:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000065:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000066:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000014 */ = ( 01 00 FF 7F 00 00 00 00 ) 
  .field /*0400001B*/ public static initonly class System.ComponentModel.MergablePropertyAttribute/*02000019*/ Default
  .field /*0400001C*/ public static initonly class System.ComponentModel.MergablePropertyAttribute/*02000019*/ No
  .field /*0400001D*/ public static initonly class System.ComponentModel.MergablePropertyAttribute/*02000019*/ Yes
  .method /*06000090*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool allowMerge) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x228d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MergablePropertyAttribute::.ctor

  .method /*06000091*/ public hidebysig specialname 
          instance bool  get_AllowMerge() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2295
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MergablePropertyAttribute::get_AllowMerge

  .method /*06000092*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000AC:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000058*/ 
    .custom /*0C000094:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000018*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2298
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MergablePropertyAttribute::Equals

  .method /*06000093*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x229b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MergablePropertyAttribute::GetHashCode

  .method /*06000094*/ public hidebysig virtual 
          instance bool  IsDefaultAttribute() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x229e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MergablePropertyAttribute::IsDefaultAttribute

  .property /*17000031*/ instance callconv(8) bool
          AllowMerge()
  {
    .get instance bool System.ComponentModel.MergablePropertyAttribute/*02000019*/::get_AllowMerge() /* 06000091 */
  } // end of property MergablePropertyAttribute::AllowMerge
} // end of class System.ComponentModel.MergablePropertyAttribute

.class /*0200001A*/ public auto ansi sealed beforefieldinit System.ComponentModel.NotifyParentPropertyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C000067:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000068:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000069:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000014 */ = ( 01 00 80 00 00 00 00 00 ) 
  .field /*0400001E*/ public static initonly class System.ComponentModel.NotifyParentPropertyAttribute/*0200001A*/ Default
  .field /*0400001F*/ public static initonly class System.ComponentModel.NotifyParentPropertyAttribute/*0200001A*/ No
  .field /*04000020*/ public static initonly class System.ComponentModel.NotifyParentPropertyAttribute/*0200001A*/ Yes
  .method /*06000095*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool notifyParent) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x22a1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method NotifyParentPropertyAttribute::.ctor

  .method /*06000096*/ public hidebysig specialname 
          instance bool  get_NotifyParent() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NotifyParentPropertyAttribute::get_NotifyParent

  .method /*06000097*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000AD:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800005A*/ 
    .custom /*0C000095:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000018*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NotifyParentPropertyAttribute::Equals

  .method /*06000098*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x22af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NotifyParentPropertyAttribute::GetHashCode

  .method /*06000099*/ public hidebysig virtual 
          instance bool  IsDefaultAttribute() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NotifyParentPropertyAttribute::IsDefaultAttribute

  .property /*17000032*/ instance callconv(8) bool
          NotifyParent()
  {
    .get instance bool System.ComponentModel.NotifyParentPropertyAttribute/*0200001A*/::get_NotifyParent() /* 06000096 */
  } // end of property NotifyParentPropertyAttribute::NotifyParent
} // end of class System.ComponentModel.NotifyParentPropertyAttribute

.class /*0200001B*/ public auto ansi sealed beforefieldinit System.ComponentModel.ParenthesizePropertyNameAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C00006A:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000014 */ = ( 01 00 FF 7F 00 00 00 00 ) 
  .field /*04000021*/ public static initonly class System.ComponentModel.ParenthesizePropertyNameAttribute/*0200001B*/ Default
  .custom /*0C000076:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .method /*0600009A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22b5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ParenthesizePropertyNameAttribute::.ctor

  .method /*0600009B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool needParenthesis) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x22bd
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ParenthesizePropertyNameAttribute::.ctor

  .method /*0600009C*/ public hidebysig specialname 
          instance bool  get_NeedParenthesis() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParenthesizePropertyNameAttribute::get_NeedParenthesis

  .method /*0600009D*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000AE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800005C*/ 
    .custom /*0C000097:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000018*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParenthesizePropertyNameAttribute::Equals

  .method /*0600009E*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x22cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParenthesizePropertyNameAttribute::GetHashCode

  .method /*0600009F*/ public hidebysig virtual 
          instance bool  IsDefaultAttribute() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParenthesizePropertyNameAttribute::IsDefaultAttribute

  .property /*17000033*/ instance callconv(8) bool
          NeedParenthesis()
  {
    .get instance bool System.ComponentModel.ParenthesizePropertyNameAttribute/*0200001B*/::get_NeedParenthesis() /* 0600009C */
  } // end of property ParenthesizePropertyNameAttribute::NeedParenthesis
} // end of class System.ComponentModel.ParenthesizePropertyNameAttribute

.class /*0200001C*/ public auto ansi sealed beforefieldinit System.ComponentModel.ReadOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C00006B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00006C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00006D:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000014 */ = ( 01 00 FF 7F 00 00 00 00 ) 
  .field /*04000022*/ public static initonly class System.ComponentModel.ReadOnlyAttribute/*0200001C*/ Default
  .field /*04000023*/ public static initonly class System.ComponentModel.ReadOnlyAttribute/*0200001C*/ No
  .field /*04000024*/ public static initonly class System.ComponentModel.ReadOnlyAttribute/*0200001C*/ Yes
  .method /*060000A0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool isReadOnly) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x22d1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ReadOnlyAttribute::.ctor

  .method /*060000A1*/ public hidebysig specialname 
          instance bool  get_IsReadOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyAttribute::get_IsReadOnly

  .method /*060000A2*/ public hidebysig virtual 
          instance bool  Equals(object 'value') cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000AF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800005E*/ 
    .custom /*0C000098:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000018*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyAttribute::Equals

  .method /*060000A3*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x22df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyAttribute::GetHashCode

  .method /*060000A4*/ public hidebysig virtual 
          instance bool  IsDefaultAttribute() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReadOnlyAttribute::IsDefaultAttribute

  .property /*17000034*/ instance callconv(8) bool
          IsReadOnly()
  {
    .get instance bool System.ComponentModel.ReadOnlyAttribute/*0200001C*/::get_IsReadOnly() /* 060000A1 */
  } // end of property ReadOnlyAttribute::IsReadOnly
} // end of class System.ComponentModel.ReadOnlyAttribute

.class /*0200001D*/ public auto ansi sealed System.ComponentModel.RefreshProperties
       extends [System.Runtime/*23000001*/]System.Enum/*01000022*/
{
  .field /*04000025*/ public specialname rtspecialname int32 value__
  .field /*04000026*/ public static literal valuetype System.ComponentModel.RefreshProperties/*0200001D*/ None = int32(0x00000000)
  .field /*04000027*/ public static literal valuetype System.ComponentModel.RefreshProperties/*0200001D*/ All = int32(0x00000001)
  .field /*04000028*/ public static literal valuetype System.ComponentModel.RefreshProperties/*0200001D*/ Repaint = int32(0x00000002)
} // end of class System.ComponentModel.RefreshProperties

.class /*0200001E*/ public auto ansi sealed beforefieldinit System.ComponentModel.RefreshPropertiesAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C00006E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00006F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000070:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000014 */ = ( 01 00 FF 7F 00 00 00 00 ) 
  .field /*04000029*/ public static initonly class System.ComponentModel.RefreshPropertiesAttribute/*0200001E*/ All
  .field /*0400002A*/ public static initonly class System.ComponentModel.RefreshPropertiesAttribute/*0200001E*/ Default
  .field /*0400002B*/ public static initonly class System.ComponentModel.RefreshPropertiesAttribute/*0200001E*/ Repaint
  .method /*060000A5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.ComponentModel.RefreshProperties/*0200001D*/ refresh) cil managed
  // SIG: 20 01 01 11 74
  {
    // Method begins at RVA 0x22e5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RefreshPropertiesAttribute::.ctor

  .method /*060000A6*/ public hidebysig specialname 
          instance valuetype System.ComponentModel.RefreshProperties/*0200001D*/ 
          get_RefreshProperties() cil managed
  // SIG: 20 00 11 74
  {
    // Method begins at RVA 0x22ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RefreshPropertiesAttribute::get_RefreshProperties

  .method /*060000A7*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000B0:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000060*/ 
    .custom /*0C00009A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000018*/::.ctor(bool) /* 0A000015 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RefreshPropertiesAttribute::Equals

  .method /*060000A8*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x22f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RefreshPropertiesAttribute::GetHashCode

  .method /*060000A9*/ public hidebysig virtual 
          instance bool  IsDefaultAttribute() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RefreshPropertiesAttribute::IsDefaultAttribute

  .property /*17000035*/ instance callconv(8) valuetype System.ComponentModel.RefreshProperties/*0200001D*/
          RefreshProperties()
  {
    .get instance valuetype System.ComponentModel.RefreshProperties/*0200001D*/ System.ComponentModel.RefreshPropertiesAttribute/*0200001E*/::get_RefreshProperties() /* 060000A6 */
  } // end of property RefreshPropertiesAttribute::RefreshProperties
} // end of class System.ComponentModel.RefreshPropertiesAttribute

.class /*0200001F*/ public auto ansi sealed beforefieldinit System.ComponentModel.Design.Serialization.DesignerSerializerAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C000072:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000073:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000074:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000014 */ = ( 01 00 04 04 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 01 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 01 )                               // rited.
  .method /*060000AA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string serializerTypeName,
                               string baseSerializerTypeName) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x22f9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DesignerSerializerAttribute::.ctor

  .method /*060000AB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string serializerTypeName,
                               class [System.Runtime/*23000001*/]System.Type/*0100001F*/ baseSerializerType) cil managed
  // SIG: 20 02 01 0E 12 7D
  {
    .custom /*0C0000B1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000063*/ 
    .custom /*0C00009B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2301
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DesignerSerializerAttribute::.ctor

  .method /*060000AC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*0100001F*/ serializerType,
                               class [System.Runtime/*23000001*/]System.Type/*0100001F*/ baseSerializerType) cil managed
  // SIG: 20 02 01 12 7D 12 7D
  {
    .custom /*0C0000B2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2309
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DesignerSerializerAttribute::.ctor

  .method /*060000AD*/ public hidebysig specialname 
          instance string  get_SerializerBaseTypeName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2311
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignerSerializerAttribute::get_SerializerBaseTypeName

  .method /*060000AE*/ public hidebysig specialname 
          instance string  get_SerializerTypeName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2314
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignerSerializerAttribute::get_SerializerTypeName

  .method /*060000AF*/ public hidebysig specialname virtual 
          instance object  get_TypeId() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C0000B3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000013*/::.ctor(uint8) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2317
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DesignerSerializerAttribute::get_TypeId

  .property /*17000036*/ instance callconv(8) string
          SerializerBaseTypeName()
  {
    .get instance string System.ComponentModel.Design.Serialization.DesignerSerializerAttribute/*0200001F*/::get_SerializerBaseTypeName() /* 060000AD */
  } // end of property DesignerSerializerAttribute::SerializerBaseTypeName
  .property /*17000037*/ instance callconv(8) string
          SerializerTypeName()
  {
    .get instance string System.ComponentModel.Design.Serialization.DesignerSerializerAttribute/*0200001F*/::get_SerializerTypeName() /* 060000AE */
  } // end of property DesignerSerializerAttribute::SerializerTypeName
  .property /*17000038*/ instance callconv(8) object
          TypeId()
  {
    .custom /*0C000088:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .get instance object System.ComponentModel.Design.Serialization.DesignerSerializerAttribute/*0200001F*/::get_TypeId() /* 060000AF */
  } // end of property DesignerSerializerAttribute::TypeId
} // end of class System.ComponentModel.Design.Serialization.DesignerSerializerAttribute


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
