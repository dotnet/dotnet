


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
// Time-date stamp:            0xa6bc8212
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00004e00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00006c9a
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
// Checksum:                       0x0000b33e
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00006c47 [0x0000004f] address [size] of Import Directory:          
// 0x00008000 [0x000004d4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00005800 [0x00002470] address [size] of Security Directory:        
// 0x0000a000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00006b4c [0x00000054] address [size] of Debug Directory:           
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
//              0x00004ca0 Virtual Size
//              0x00002000 Virtual Address
//              0x00004e00 Size of Raw Data
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
//              0x000004d4 Virtual Size
//              0x00008000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00005000 Pointer to Raw Data
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
//              0x00005600 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000c9c
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00006c89 Import Name Table
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
// 0x000023c0 [0x0000470c] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00006acc [0x00000080] address [size] of Strong Name Signature:     
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
//              0x000025ac Size
//              '#~' Name
//    Stream 2:
//              0x00002618 Offset
//              0x00001458 Size
//              '#Strings' Name
//    Stream 3:
//              0x00003a70 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00003a74 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00003a84 Offset
//              0x00000c88 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000004090ba01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 31856
// PE header size       : 512 (496 used)    ( 1.61%)
// PE additional info   : 10819             (33.96%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.23%)
// CLR meta-data size  : 18188              (57.09%)
// CLR additional info : 128                ( 0.40%)
// CLR method headers  : 234                ( 0.73%)
// Managed code         : 644               ( 2.02%)
// Data                 : 2048              ( 6.43%)
// Unaccounted          : -789              (-2.48%)

// Num.of PE sections   : 3
//   .text    - 19968
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 18188
//   Module        -    1 (10 bytes)
//   TypeDef       -   28 (392 bytes)     5 interfaces, 0 explicit layout
//   TypeRef       -   46 (276 bytes)
//   MethodDef     -  244 (3416 bytes)    10 abstract, 0 native, 234 bodies
//   FieldDef      -   11 (66 bytes)      0 constant
//   MemberRef     -   46 (276 bytes)
//   ParamDef      -  299 (1794 bytes)
//   MethodImpl    -   50 (300 bytes)
//   Constant      -    7 (42 bytes)
//   CustomAttribute-  245 (1470 bytes)
//   InterfaceImpl -   52 (208 bytes)
//   PropertyMap   -   17 (68 bytes)
//   Property      -   51 (306 bytes)
//   MethodSemantic-   64 (384 bytes)
//   TypeSpec      -   19 (38 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    1 (20 bytes)
//   GenericParam  -   57 (456 bytes)
//   Strings       -  5208 bytes
//   Blobs         -  3208 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   208 bytes

// CLR additional info : 128

// CLR method headers : 234
//   Num.of method bodies  - 234
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 234

// Managed code : 644
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class ActionBlock`1                  (public) (auto) (ansi) (sealed) 
// Class BatchBlock`1                   (public) (auto) (ansi) (sealed) 
// Class BatchedJoinBlock`2             (public) (auto) (ansi) (sealed) 
// Class BatchedJoinBlock`3             (public) (auto) (ansi) (sealed) 
// Class BroadcastBlock`1               (public) (auto) (ansi) (sealed) 
// Class BufferBlock`1                  (public) (auto) (ansi) (sealed) 
// Class DataflowBlock                  (public) (abstract) (auto) (ansi) (sealed) 
// Class DataflowBlockOptions           (public) (auto) (ansi) 
// Class DataflowLinkOptions            (public) (auto) (ansi) 
// Class DataflowMessageHeader          (public) (sequential) (ansi) (sealed) 
// Class DataflowMessageStatus          (public) (auto) (ansi) (sealed) 
// Class ExecutionDataflowBlockOptions  (public) (auto) (ansi) 
// Class GroupingDataflowBlockOptions   (public) (auto) (ansi) 
// Interface IDataflowBlock                 (public) (abstract) (auto) (ansi) 
// Interface IPropagatorBlock`2             (public) (abstract) (auto) (ansi) 
// Interface IReceivableSourceBlock`1       (public) (abstract) (auto) (ansi) 
// Interface ISourceBlock`1                 (public) (abstract) (auto) (ansi) 
// Interface ITargetBlock`1                 (public) (abstract) (auto) (ansi) 
// Class JoinBlock`2                    (public) (auto) (ansi) (sealed) 
// Class JoinBlock`3                    (public) (auto) (ansi) (sealed) 
// Class TransformBlock`2               (public) (auto) (ansi) (sealed) 
// Class TransformManyBlock`2           (public) (auto) (ansi) (sealed) 
// Class WriteOnceBlock`1               (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly /*20000001*/ noplatform System.Threading.Tasks.Dataflow
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
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000007*/::.ctor() /* 0A000006 */ = ( 01 00 00 00 ) 
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000008*/::.ctor(bool) /* 0A000007 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000A:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 1F 53 79 73 74 65 6D 2E 54 68 72 65 61 64   // ...System.Thread
                                                                                                                                                                        69 6E 67 2E 54 61 73 6B 73 2E 44 61 74 61 66 6C   // ing.Tasks.Datafl
                                                                                                                                                                        6F 77 00 00 )                                     // ow..
  .custom /*0C00000B:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000C:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000F:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000010:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 1F 53 79 73 74 65 6D 2E 54 68 72 65 61 64   // ...System.Thread
                                                                                                                                                                       69 6E 67 2E 54 61 73 6B 73 2E 44 61 74 61 66 6C   // ing.Tasks.Datafl
                                                                                                                                                                       6F 77 00 00 )                                     // ow..
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 36 2E 30 2E 32 31 2E 35 32 32 31 30 00   // ...6.0.21.52210.
                                                                                                                                                                       00 ) 
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 36 2E 30 2E 30 2B 34 38 32 32 65 33 63   // ...6.0.0+4822e3c
                                                                                                                                                                                33 61 61 37 37 65 62 38 32 62 32 66 62 33 33 63   // 3aa77eb82b2fb33c
                                                                                                                                                                                39 33 32 31 66 39 32 33 63 66 31 31 64 64 64 65   // 9321f923cf11ddde
                                                                                                                                                                                36 00 00 )                                        // 6..
  .custom /*0C000014:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000015:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 1F 53 79 73 74 65 6D 2E 54 68 72 65 61 64   // ...System.Thread
                                                                                                                                                                 69 6E 67 2E 54 61 73 6B 73 2E 44 61 74 61 66 6C   // ing.Tasks.Datafl
                                                                                                                                                                 6F 77 00 00 )                                     // ow..
  .custom /*0C000016:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
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
.module System.Threading.Tasks.Dataflow.dll
// MVID: {026e2594-c371-4a4b-89fa-d7c26f11df90}
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
  .custom /*0C000018:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000019:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A00002D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C00001C:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C00001D:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001E:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
    IL_0001:  /* 28   | (0A)00002D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A00002D */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)00002E       */ newarr     [System.Runtime/*23000001*/]System.Byte/*0100002E*/
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
    IL_0001:  /* 28   | (0A)00002D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A00002D */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000021:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000022:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000023:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
    IL_0001:  /* 28   | (0A)00002D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A00002D */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000026:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000027:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000028:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)00002D       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A00002D */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ public auto ansi sealed beforefieldinit System.Threading.Tasks.Dataflow.ActionBlock`1<TInput>
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/,
                  class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TInput>/*1B000001*/
{
  .custom /*0C00002B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/
  .custom /*0C000001:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TInput>/*1B000001*/
  .custom /*0C00001A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type TInput /*2A000001*/ 
    .custom /*0C000017:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Action`1/*01000018*/<!TInput> action) cil managed
  // SIG: 20 01 01 15 12 61 01 13 00
  {
    // Method begins at RVA 0x209d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ActionBlock`1::.ctor

  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Action`1/*01000018*/<!TInput> action,
                               class System.Threading.Tasks.Dataflow.ExecutionDataflowBlockOptions/*02000011*/ dataflowBlockOptions) cil managed
  // SIG: 20 02 01 15 12 61 01 13 00 12 44
  {
    // Method begins at RVA 0x20a5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ActionBlock`1::.ctor

  .method /*06000008*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<!TInput,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/> action) cil managed
  // SIG: 20 01 01 15 12 65 02 13 00 12 69
  {
    // Method begins at RVA 0x20ad
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ActionBlock`1::.ctor

  .method /*06000009*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<!TInput,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/> action,
                               class System.Threading.Tasks.Dataflow.ExecutionDataflowBlockOptions/*02000011*/ dataflowBlockOptions) cil managed
  // SIG: 20 02 01 15 12 65 02 13 00 12 69 12 44
  {
    // Method begins at RVA 0x20b5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ActionBlock`1::.ctor

  .method /*0600000A*/ public hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          get_Completion() cil managed
  // SIG: 20 00 12 69
  {
    // Method begins at RVA 0x20bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActionBlock`1::get_Completion

  .method /*0600000B*/ public hidebysig specialname 
          instance int32  get_InputCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActionBlock`1::get_InputCount

  .method /*0600000C*/ public hidebysig newslot virtual final 
          instance void  Complete() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20c3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ActionBlock`1::Complete

  .method /*0600000D*/ public hidebysig instance bool 
          Post(!TInput item) cil managed
  // SIG: 20 01 02 13 00
  {
    // Method begins at RVA 0x20c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActionBlock`1::Post

  .method /*0600000E*/ private hidebysig newslot virtual final 
          instance void  System.Threading.Tasks.Dataflow.IDataflowBlock.Fault(class [System.Runtime/*23000001*/]System.Exception/*01000017*/ exception) cil managed
  // SIG: 20 01 01 12 5D
  {
    .override System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/::Fault /*02000013::0600009F*/ 
    // Method begins at RVA 0x20c8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ActionBlock`1::System.Threading.Tasks.Dataflow.IDataflowBlock.Fault

  .method /*0600000F*/ private hidebysig newslot virtual final 
          instance valuetype System.Threading.Tasks.Dataflow.DataflowMessageStatus/*02000010*/ 
          'System.Threading.Tasks.Dataflow.ITargetBlock<TInput>.OfferMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                              !TInput messageValue,
                                                                              class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!TInput> source,
                                                                              bool consumeToAccept) cil managed
  // SIG: 20 04 11 40 11 3C 13 00 15 12 58 01 13 00 02
  {
    .override  method instance valuetype System.Threading.Tasks.Dataflow.DataflowMessageStatus/*02000010*/ class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TInput>/*1B000001*/::OfferMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                                                                                               !0,
                                                                                                                                                                                                               class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!0>,
                                                                                                                                                                                                               bool) /* 0A000013 */
    // Method begins at RVA 0x20ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActionBlock`1::'System.Threading.Tasks.Dataflow.ITargetBlock<TInput>.OfferMessage'

  .method /*06000010*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActionBlock`1::ToString

  .property /*17000001*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/
          Completion()
  {
    .get instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ System.Threading.Tasks.Dataflow.ActionBlock`1/*02000006*/::get_Completion() /* 0600000A */
  } // end of property ActionBlock`1::Completion
  .property /*17000002*/ instance callconv(8) int32
          InputCount()
  {
    .get instance int32 System.Threading.Tasks.Dataflow.ActionBlock`1/*02000006*/::get_InputCount() /* 0600000B */
  } // end of property ActionBlock`1::InputCount
} // end of class System.Threading.Tasks.Dataflow.ActionBlock`1

.class /*02000007*/ public auto ansi sealed beforefieldinit System.Threading.Tasks.Dataflow.BatchBlock`1<T>
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/,
                  class System.Threading.Tasks.Dataflow.IPropagatorBlock`2/*02000014*/<!T,!T[]>/*1B000003*/,
                  class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T[]>/*1B000002*/,
                  class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T>/*1B000001*/,
                  class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<!T[]>/*1B000004*/
{
  .custom /*0C00002F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000030:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/
  .custom /*0C00001F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.IPropagatorBlock`2/*02000014*/<!T,!T[]>/*1B000003*/
  .custom /*0C000024:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 01 01 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T[]>/*1B000002*/
  .custom /*0C000029:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T>/*1B000001*/
  .custom /*0C00002D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<!T[]>/*1B000004*/
  .custom /*0C000031:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 01 00 00 ) 
  .param type T /*2A000002*/ 
    .custom /*0C00001B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*06000011*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 batchSize) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20d0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BatchBlock`1::.ctor

  .method /*06000012*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 batchSize,
                               class System.Threading.Tasks.Dataflow.GroupingDataflowBlockOptions/*02000012*/ dataflowBlockOptions) cil managed
  // SIG: 20 02 01 08 12 48
  {
    // Method begins at RVA 0x20d8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BatchBlock`1::.ctor

  .method /*06000013*/ public hidebysig specialname 
          instance int32  get_BatchSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchBlock`1::get_BatchSize

  .method /*06000014*/ public hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          get_Completion() cil managed
  // SIG: 20 00 12 69
  {
    // Method begins at RVA 0x20e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchBlock`1::get_Completion

  .method /*06000015*/ public hidebysig specialname 
          instance int32  get_OutputCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchBlock`1::get_OutputCount

  .method /*06000016*/ public hidebysig newslot virtual final 
          instance void  Complete() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20e9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BatchBlock`1::Complete

  .method /*06000017*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/ 
          LinkTo(class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T[]> target,
                 class System.Threading.Tasks.Dataflow.DataflowLinkOptions/*0200000E*/ linkOptions) cil managed
  // SIG: 20 02 12 6D 15 12 5C 01 1D 13 00 12 38
  {
    // Method begins at RVA 0x20eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchBlock`1::LinkTo

  .method /*06000018*/ private hidebysig newslot virtual final 
          instance void  System.Threading.Tasks.Dataflow.IDataflowBlock.Fault(class [System.Runtime/*23000001*/]System.Exception/*01000017*/ exception) cil managed
  // SIG: 20 01 01 12 5D
  {
    .override System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/::Fault /*02000013::0600009F*/ 
    // Method begins at RVA 0x20ee
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BatchBlock`1::System.Threading.Tasks.Dataflow.IDataflowBlock.Fault

  .method /*06000019*/ private hidebysig newslot virtual final 
          instance !T[]  'System.Threading.Tasks.Dataflow.ISourceBlock<T[]>.ConsumeMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                            class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T[]> target,
                                                                                            [out] bool& messageConsumed) cil managed
  // SIG: 20 03 1D 13 00 11 3C 15 12 5C 01 1D 13 00 10 02
  {
    .override  method instance !0 class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T[]>/*1B000002*/::ConsumeMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                     class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>,
                                                                                                                                     bool&) /* 0A000014 */
    // Method begins at RVA 0x20f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchBlock`1::'System.Threading.Tasks.Dataflow.ISourceBlock<T[]>.ConsumeMessage'

  .method /*0600001A*/ private hidebysig newslot virtual final 
          instance void  'System.Threading.Tasks.Dataflow.ISourceBlock<T[]>.ReleaseReservation'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                                class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T[]> target) cil managed
  // SIG: 20 02 01 11 3C 15 12 5C 01 1D 13 00
  {
    .override  method instance void class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T[]>/*1B000002*/::ReleaseReservation(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                           class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>) /* 0A000015 */
    // Method begins at RVA 0x20f3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BatchBlock`1::'System.Threading.Tasks.Dataflow.ISourceBlock<T[]>.ReleaseReservation'

  .method /*0600001B*/ private hidebysig newslot virtual final 
          instance bool  'System.Threading.Tasks.Dataflow.ISourceBlock<T[]>.ReserveMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                            class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T[]> target) cil managed
  // SIG: 20 02 02 11 3C 15 12 5C 01 1D 13 00
  {
    .override  method instance bool class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T[]>/*1B000002*/::ReserveMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                       class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>) /* 0A000016 */
    // Method begins at RVA 0x20f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchBlock`1::'System.Threading.Tasks.Dataflow.ISourceBlock<T[]>.ReserveMessage'

  .method /*0600001C*/ private hidebysig newslot virtual final 
          instance valuetype System.Threading.Tasks.Dataflow.DataflowMessageStatus/*02000010*/ 
          'System.Threading.Tasks.Dataflow.ITargetBlock<T>.OfferMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                         !T messageValue,
                                                                         class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T> source,
                                                                         bool consumeToAccept) cil managed
  // SIG: 20 04 11 40 11 3C 13 00 15 12 58 01 13 00 02
  {
    .override  method instance valuetype System.Threading.Tasks.Dataflow.DataflowMessageStatus/*02000010*/ class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T>/*1B000001*/::OfferMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                                                                                          !0,
                                                                                                                                                                                                          class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!0>,
                                                                                                                                                                                                          bool) /* 0A000013 */
    // Method begins at RVA 0x20f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchBlock`1::'System.Threading.Tasks.Dataflow.ITargetBlock<T>.OfferMessage'

  .method /*0600001D*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchBlock`1::ToString

  .method /*0600001E*/ public hidebysig instance void 
          TriggerBatch() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20fe
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BatchBlock`1::TriggerBatch

  .method /*0600001F*/ public hidebysig newslot virtual final 
          instance bool  TryReceive(class [System.Runtime/*23000001*/]System.Predicate`1/*0100001C*/<!T[]> 'filter',
                                    [out] !T[]& item) cil managed
  // SIG: 20 02 02 15 12 71 01 1D 13 00 10 1D 13 00
  {
    .param [1]/*0800001E*/ 
    .custom /*0C000076:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .param [2]/*0800001F*/ 
    .custom /*0C000079:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .custom /*0C00007A:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001D*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2100
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchBlock`1::TryReceive

  .method /*06000020*/ public hidebysig newslot virtual final 
          instance bool  TryReceiveAll([out] class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T[]>& items) cil managed
  // SIG: 20 01 02 10 15 12 79 01 1D 13 00
  {
    .param [1]/*08000020*/ 
    .custom /*0C00007C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .custom /*0C00007D:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001D*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2103
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchBlock`1::TryReceiveAll

  .property /*17000003*/ instance callconv(8) int32
          BatchSize()
  {
    .get instance int32 System.Threading.Tasks.Dataflow.BatchBlock`1/*02000007*/::get_BatchSize() /* 06000013 */
  } // end of property BatchBlock`1::BatchSize
  .property /*17000004*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/
          Completion()
  {
    .get instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ System.Threading.Tasks.Dataflow.BatchBlock`1/*02000007*/::get_Completion() /* 06000014 */
  } // end of property BatchBlock`1::Completion
  .property /*17000005*/ instance callconv(8) int32
          OutputCount()
  {
    .get instance int32 System.Threading.Tasks.Dataflow.BatchBlock`1/*02000007*/::get_OutputCount() /* 06000015 */
  } // end of property BatchBlock`1::OutputCount
} // end of class System.Threading.Tasks.Dataflow.BatchBlock`1

.class /*02000008*/ public auto ansi sealed beforefieldinit System.Threading.Tasks.Dataflow.BatchedJoinBlock`2<T1,T2>
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/,
                  class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>>>/*1B000006*/,
                  class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>>>/*1B000005*/
{
  .custom /*0C000033:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000034:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/
  .custom /*0C000035:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>>>/*1B000006*/
  .custom /*0C000039:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 06 00 00 00 00 01 01 01 01 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>>>/*1B000005*/
  .custom /*0C00003D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 06 00 00 00 00 01 01 01 01 01 00 00 ) 
  .param type T1 /*2A000003*/ 
    .custom /*0C000020:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .param type T2 /*2A000004*/ 
    .custom /*0C000025:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*06000021*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 batchSize) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2106
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BatchedJoinBlock`2::.ctor

  .method /*06000022*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 batchSize,
                               class System.Threading.Tasks.Dataflow.GroupingDataflowBlockOptions/*02000012*/ dataflowBlockOptions) cil managed
  // SIG: 20 02 01 08 12 48
  {
    // Method begins at RVA 0x210e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BatchedJoinBlock`2::.ctor

  .method /*06000023*/ public hidebysig specialname 
          instance int32  get_BatchSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2116
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`2::get_BatchSize

  .method /*06000024*/ public hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          get_Completion() cil managed
  // SIG: 20 00 12 69
  {
    // Method begins at RVA 0x2119
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`2::get_Completion

  .method /*06000025*/ public hidebysig specialname 
          instance int32  get_OutputCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x211c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`2::get_OutputCount

  .method /*06000026*/ public hidebysig specialname 
          instance class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T1> 
          get_Target1() cil managed
  // SIG: 20 00 15 12 5C 01 13 00
  {
    // Method begins at RVA 0x211f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`2::get_Target1

  .method /*06000027*/ public hidebysig specialname 
          instance class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T2> 
          get_Target2() cil managed
  // SIG: 20 00 15 12 5C 01 13 01
  {
    // Method begins at RVA 0x2122
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`2::get_Target2

  .method /*06000028*/ public hidebysig newslot virtual final 
          instance void  Complete() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2125
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BatchedJoinBlock`2::Complete

  .method /*06000029*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/ 
          LinkTo(class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>>> target,
                 class System.Threading.Tasks.Dataflow.DataflowLinkOptions/*0200000E*/ linkOptions) cil managed
  // SIG: 20 02 12 6D 15 12 5C 01 15 12 7D 02 15 12 79 01 13 00 15 12 79 01 13 01 12 38
  {
    // Method begins at RVA 0x2127
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`2::LinkTo

  .method /*0600002A*/ private hidebysig newslot virtual final 
          instance void  System.Threading.Tasks.Dataflow.IDataflowBlock.Fault(class [System.Runtime/*23000001*/]System.Exception/*01000017*/ exception) cil managed
  // SIG: 20 01 01 12 5D
  {
    .override System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/::Fault /*02000013::0600009F*/ 
    // Method begins at RVA 0x212a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BatchedJoinBlock`2::System.Threading.Tasks.Dataflow.IDataflowBlock.Fault

  .method /*0600002B*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>> 
          'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<System.Collections.Generic.IList<T1>,System.Collections.Generic.IList<T2>>>.ConsumeMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                                                                                                 class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>>> target,
                                                                                                                                                                 [out] bool& messageConsumed) cil managed
  // SIG: 20 03 15 12 7D 02 15 12 79 01 13 00 15 12 79 01 13 01 11 3C 15 12 5C 01 15 12 7D 02 15 12 79 01
  //      13 00 15 12 79 01 13 01 10 02
  {
    .override  method instance !0 class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>>>/*1B000005*/::ConsumeMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                                                                                                                                                                                                                                                          class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>,
                                                                                                                                                                                                                                                                                                                                                                          bool&) /* 0A000018 */
    // Method begins at RVA 0x212c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`2::'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<System.Collections.Generic.IList<T1>,System.Collections.Generic.IList<T2>>>.ConsumeMessage'

  .method /*0600002C*/ private hidebysig newslot virtual final 
          instance void  'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<System.Collections.Generic.IList<T1>,System.Collections.Generic.IList<T2>>>.ReleaseReservation'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                                                                                                                    class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>>> target) cil managed
  // SIG: 20 02 01 11 3C 15 12 5C 01 15 12 7D 02 15 12 79 01 13 00 15 12 79 01 13 01
  {
    .override  method instance void class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>>>/*1B000005*/::ReleaseReservation(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                                                                                                                                                                                                                                                                class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>) /* 0A000019 */
    // Method begins at RVA 0x212f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BatchedJoinBlock`2::'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<System.Collections.Generic.IList<T1>,System.Collections.Generic.IList<T2>>>.ReleaseReservation'

  .method /*0600002D*/ private hidebysig newslot virtual final 
          instance bool  'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<System.Collections.Generic.IList<T1>,System.Collections.Generic.IList<T2>>>.ReserveMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                                                                                                                class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>>> target) cil managed
  // SIG: 20 02 02 11 3C 15 12 5C 01 15 12 7D 02 15 12 79 01 13 00 15 12 79 01 13 01
  {
    .override  method instance bool class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>>>/*1B000005*/::ReserveMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                                                                                                                                                                                                                                                            class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>) /* 0A00001A */
    // Method begins at RVA 0x2131
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`2::'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<System.Collections.Generic.IList<T1>,System.Collections.Generic.IList<T2>>>.ReserveMessage'

  .method /*0600002E*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2134
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`2::ToString

  .method /*0600002F*/ public hidebysig newslot virtual final 
          instance bool  TryReceive(class [System.Runtime/*23000001*/]System.Predicate`1/*0100001C*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>>> 'filter',
                                    [out] class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>>& item) cil managed
  // SIG: 20 02 02 15 12 71 01 15 12 7D 02 15 12 79 01 13 00 15 12 79 01 13 01 10 15 12 7D 02 15 12 79 01
  //      13 00 15 12 79 01 13 01
  {
    .param [1]/*0800002E*/ 
    .custom /*0C00009A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 06 00 00 00 02 01 01 01 01 01 00 00 ) 
    .param [2]/*0800002F*/ 
    .custom /*0C00009D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 05 00 00 00 02 01 01 01 01 00 00 ) 
    .custom /*0C00009E:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001D*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2137
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`2::TryReceive

  .method /*06000030*/ public hidebysig newslot virtual final 
          instance bool  TryReceiveAll([out] class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>>>& items) cil managed
  // SIG: 20 01 02 10 15 12 79 01 15 12 7D 02 15 12 79 01 13 00 15 12 79 01 13 01
  {
    .param [1]/*08000030*/ 
    .custom /*0C0000A1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 06 00 00 00 02 01 01 01 01 01 00 00 ) 
    .custom /*0C0000A2:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001D*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x213a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`2::TryReceiveAll

  .property /*17000006*/ instance callconv(8) int32
          BatchSize()
  {
    .get instance int32 System.Threading.Tasks.Dataflow.BatchedJoinBlock`2/*02000008*/::get_BatchSize() /* 06000023 */
  } // end of property BatchedJoinBlock`2::BatchSize
  .property /*17000007*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/
          Completion()
  {
    .get instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ System.Threading.Tasks.Dataflow.BatchedJoinBlock`2/*02000008*/::get_Completion() /* 06000024 */
  } // end of property BatchedJoinBlock`2::Completion
  .property /*17000008*/ instance callconv(8) int32
          OutputCount()
  {
    .get instance int32 System.Threading.Tasks.Dataflow.BatchedJoinBlock`2/*02000008*/::get_OutputCount() /* 06000025 */
  } // end of property BatchedJoinBlock`2::OutputCount
  .property /*17000009*/ instance callconv(8) class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T1>
          Target1()
  {
    .get instance class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T1> System.Threading.Tasks.Dataflow.BatchedJoinBlock`2/*02000008*/::get_Target1() /* 06000026 */
  } // end of property BatchedJoinBlock`2::Target1
  .property /*1700000A*/ instance callconv(8) class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T2>
          Target2()
  {
    .get instance class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T2> System.Threading.Tasks.Dataflow.BatchedJoinBlock`2/*02000008*/::get_Target2() /* 06000027 */
  } // end of property BatchedJoinBlock`2::Target2
} // end of class System.Threading.Tasks.Dataflow.BatchedJoinBlock`2

.class /*02000009*/ public auto ansi sealed beforefieldinit System.Threading.Tasks.Dataflow.BatchedJoinBlock`3<T1,T2,T3>
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/,
                  class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T3>>>/*1B000008*/,
                  class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T3>>>/*1B000007*/
{
  .custom /*0C000037:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000038:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/
  .custom /*0C000040:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T3>>>/*1B000008*/
  .custom /*0C000044:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 08 00 00 00 00 01 01 01 01 01 01 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T3>>>/*1B000007*/
  .custom /*0C000048:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 08 00 00 00 00 01 01 01 01 01 01 01 00 00 ) 
  .param type T1 /*2A000005*/ 
    .custom /*0C00002A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .param type T2 /*2A000006*/ 
    .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .param type T3 /*2A000007*/ 
    .custom /*0C000032:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*06000031*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 batchSize) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x213d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BatchedJoinBlock`3::.ctor

  .method /*06000032*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 batchSize,
                               class System.Threading.Tasks.Dataflow.GroupingDataflowBlockOptions/*02000012*/ dataflowBlockOptions) cil managed
  // SIG: 20 02 01 08 12 48
  {
    // Method begins at RVA 0x2145
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BatchedJoinBlock`3::.ctor

  .method /*06000033*/ public hidebysig specialname 
          instance int32  get_BatchSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x214d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`3::get_BatchSize

  .method /*06000034*/ public hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          get_Completion() cil managed
  // SIG: 20 00 12 69
  {
    // Method begins at RVA 0x2150
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`3::get_Completion

  .method /*06000035*/ public hidebysig specialname 
          instance int32  get_OutputCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2153
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`3::get_OutputCount

  .method /*06000036*/ public hidebysig specialname 
          instance class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T1> 
          get_Target1() cil managed
  // SIG: 20 00 15 12 5C 01 13 00
  {
    // Method begins at RVA 0x2156
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`3::get_Target1

  .method /*06000037*/ public hidebysig specialname 
          instance class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T2> 
          get_Target2() cil managed
  // SIG: 20 00 15 12 5C 01 13 01
  {
    // Method begins at RVA 0x2159
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`3::get_Target2

  .method /*06000038*/ public hidebysig specialname 
          instance class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T3> 
          get_Target3() cil managed
  // SIG: 20 00 15 12 5C 01 13 02
  {
    // Method begins at RVA 0x215c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`3::get_Target3

  .method /*06000039*/ public hidebysig newslot virtual final 
          instance void  Complete() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x215f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BatchedJoinBlock`3::Complete

  .method /*0600003A*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/ 
          LinkTo(class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T3>>> target,
                 class System.Threading.Tasks.Dataflow.DataflowLinkOptions/*0200000E*/ linkOptions) cil managed
  // SIG: 20 02 12 6D 15 12 5C 01 15 12 80 81 03 15 12 79 01 13 00 15 12 79 01 13 01 15 12 79 01 13 02 12
  //      38
  {
    // Method begins at RVA 0x2161
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`3::LinkTo

  .method /*0600003B*/ private hidebysig newslot virtual final 
          instance void  System.Threading.Tasks.Dataflow.IDataflowBlock.Fault(class [System.Runtime/*23000001*/]System.Exception/*01000017*/ exception) cil managed
  // SIG: 20 01 01 12 5D
  {
    .override System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/::Fault /*02000013::0600009F*/ 
    // Method begins at RVA 0x2164
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BatchedJoinBlock`3::System.Threading.Tasks.Dataflow.IDataflowBlock.Fault

  .method /*0600003C*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T3>> 
          'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<System.Collections.Generic.IList<T1>,System.Collections.Generic.IList<T2>,System.Collections.Generic.IList<T3>>>.ConsumeMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                                                                                                                                      class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T3>>> target,
                                                                                                                                                                                                      [out] bool& messageConsumed) cil managed
  // SIG: 20 03 15 12 80 81 03 15 12 79 01 13 00 15 12 79 01 13 01 15 12 79 01 13 02 11 3C 15 12 5C 01 15
  //      12 80 81 03 15 12 79 01 13 00 15 12 79 01 13 01 15 12 79 01 13 02 10 02
  {
    .override  method instance !0 class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T3>>>/*1B000007*/::ConsumeMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                bool&) /* 0A00001B */
    // Method begins at RVA 0x2166
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`3::'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<System.Collections.Generic.IList<T1>,System.Collections.Generic.IList<T2>,System.Collections.Generic.IList<T3>>>.ConsumeMessage'

  .method /*0600003D*/ private hidebysig newslot virtual final 
          instance void  'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<System.Collections.Generic.IList<T1>,System.Collections.Generic.IList<T2>,System.Collections.Generic.IList<T3>>>.ReleaseReservation'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                                                                                                                                                         class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T3>>> target) cil managed
  // SIG: 20 02 01 11 3C 15 12 5C 01 15 12 80 81 03 15 12 79 01 13 00 15 12 79 01 13 01 15 12 79 01 13 02
  {
    .override  method instance void class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T3>>>/*1B000007*/::ReleaseReservation(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                      class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>) /* 0A00001C */
    // Method begins at RVA 0x2169
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BatchedJoinBlock`3::'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<System.Collections.Generic.IList<T1>,System.Collections.Generic.IList<T2>,System.Collections.Generic.IList<T3>>>.ReleaseReservation'

  .method /*0600003E*/ private hidebysig newslot virtual final 
          instance bool  'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<System.Collections.Generic.IList<T1>,System.Collections.Generic.IList<T2>,System.Collections.Generic.IList<T3>>>.ReserveMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                                                                                                                                                     class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T3>>> target) cil managed
  // SIG: 20 02 02 11 3C 15 12 5C 01 15 12 80 81 03 15 12 79 01 13 00 15 12 79 01 13 01 15 12 79 01 13 02
  {
    .override  method instance bool class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T3>>>/*1B000007*/::ReserveMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                  class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>) /* 0A00001D */
    // Method begins at RVA 0x216b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`3::'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<System.Collections.Generic.IList<T1>,System.Collections.Generic.IList<T2>,System.Collections.Generic.IList<T3>>>.ReserveMessage'

  .method /*0600003F*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x216e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`3::ToString

  .method /*06000040*/ public hidebysig newslot virtual final 
          instance bool  TryReceive(class [System.Runtime/*23000001*/]System.Predicate`1/*0100001C*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T3>>> 'filter',
                                    [out] class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T3>>& item) cil managed
  // SIG: 20 02 02 15 12 71 01 15 12 80 81 03 15 12 79 01 13 00 15 12 79 01 13 01 15 12 79 01 13 02 10 15
  //      12 80 81 03 15 12 79 01 13 00 15 12 79 01 13 01 15 12 79 01 13 02
  {
    .param [1]/*0800003E*/ 
    .custom /*0C0000B2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 08 00 00 00 02 01 01 01 01 01 01 01 00 00 ) 
    .param [2]/*0800003F*/ 
    .custom /*0C0000B3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 07 00 00 00 02 01 01 01 01 01 01 00 00 ) 
    .custom /*0C0000B4:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001D*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2171
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`3::TryReceive

  .method /*06000041*/ public hidebysig newslot virtual final 
          instance bool  TryReceiveAll([out] class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T1>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T2>,class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T3>>>& items) cil managed
  // SIG: 20 01 02 10 15 12 79 01 15 12 80 81 03 15 12 79 01 13 00 15 12 79 01 13 01 15 12 79 01 13 02
  {
    .param [1]/*08000040*/ 
    .custom /*0C0000B5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 08 00 00 00 02 01 01 01 01 01 01 01 00 00 ) 
    .custom /*0C0000B6:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001D*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2174
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BatchedJoinBlock`3::TryReceiveAll

  .property /*1700000B*/ instance callconv(8) int32
          BatchSize()
  {
    .get instance int32 System.Threading.Tasks.Dataflow.BatchedJoinBlock`3/*02000009*/::get_BatchSize() /* 06000033 */
  } // end of property BatchedJoinBlock`3::BatchSize
  .property /*1700000C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/
          Completion()
  {
    .get instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ System.Threading.Tasks.Dataflow.BatchedJoinBlock`3/*02000009*/::get_Completion() /* 06000034 */
  } // end of property BatchedJoinBlock`3::Completion
  .property /*1700000D*/ instance callconv(8) int32
          OutputCount()
  {
    .get instance int32 System.Threading.Tasks.Dataflow.BatchedJoinBlock`3/*02000009*/::get_OutputCount() /* 06000035 */
  } // end of property BatchedJoinBlock`3::OutputCount
  .property /*1700000E*/ instance callconv(8) class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T1>
          Target1()
  {
    .get instance class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T1> System.Threading.Tasks.Dataflow.BatchedJoinBlock`3/*02000009*/::get_Target1() /* 06000036 */
  } // end of property BatchedJoinBlock`3::Target1
  .property /*1700000F*/ instance callconv(8) class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T2>
          Target2()
  {
    .get instance class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T2> System.Threading.Tasks.Dataflow.BatchedJoinBlock`3/*02000009*/::get_Target2() /* 06000037 */
  } // end of property BatchedJoinBlock`3::Target2
  .property /*17000010*/ instance callconv(8) class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T3>
          Target3()
  {
    .get instance class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T3> System.Threading.Tasks.Dataflow.BatchedJoinBlock`3/*02000009*/::get_Target3() /* 06000038 */
  } // end of property BatchedJoinBlock`3::Target3
} // end of class System.Threading.Tasks.Dataflow.BatchedJoinBlock`3

.class /*0200000A*/ public auto ansi sealed beforefieldinit System.Threading.Tasks.Dataflow.BroadcastBlock`1<T>
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/,
                  class System.Threading.Tasks.Dataflow.IPropagatorBlock`2/*02000014*/<!T,!T>/*1B00000B*/,
                  class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T>/*1B00000A*/,
                  class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T>/*1B000001*/,
                  class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<!T>/*1B000009*/
{
  .custom /*0C00003B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/
  .custom /*0C00004A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.IPropagatorBlock`2/*02000014*/<!T,!T>/*1B00000B*/
  .custom /*0C00004D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T>/*1B00000A*/
  .custom /*0C00004F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T>/*1B000001*/
  .custom /*0C000051:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<!T>/*1B000009*/
  .custom /*0C000053:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type T /*2A000008*/ 
    .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*06000042*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<!T,!T> cloningFunction) cil managed
  // SIG: 20 01 01 15 12 65 02 13 00 13 00
  {
    .param [1]/*08000041*/ 
    .custom /*0C0000B7:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x2177
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BroadcastBlock`1::.ctor

  .method /*06000043*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<!T,!T> cloningFunction,
                               class System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/ dataflowBlockOptions) cil managed
  // SIG: 20 02 01 15 12 65 02 13 00 13 00 12 34
  {
    .param [1]/*08000042*/ 
    .custom /*0C0000B8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x217f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BroadcastBlock`1::.ctor

  .method /*06000044*/ public hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          get_Completion() cil managed
  // SIG: 20 00 12 69
  {
    // Method begins at RVA 0x2187
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BroadcastBlock`1::get_Completion

  .method /*06000045*/ public hidebysig newslot virtual final 
          instance void  Complete() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x218a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BroadcastBlock`1::Complete

  .method /*06000046*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/ 
          LinkTo(class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T> target,
                 class System.Threading.Tasks.Dataflow.DataflowLinkOptions/*0200000E*/ linkOptions) cil managed
  // SIG: 20 02 12 6D 15 12 5C 01 13 00 12 38
  {
    // Method begins at RVA 0x218c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BroadcastBlock`1::LinkTo

  .method /*06000047*/ private hidebysig newslot virtual final 
          instance void  System.Threading.Tasks.Dataflow.IDataflowBlock.Fault(class [System.Runtime/*23000001*/]System.Exception/*01000017*/ exception) cil managed
  // SIG: 20 01 01 12 5D
  {
    .override System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/::Fault /*02000013::0600009F*/ 
    // Method begins at RVA 0x218f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BroadcastBlock`1::System.Threading.Tasks.Dataflow.IDataflowBlock.Fault

  .method /*06000048*/ private hidebysig newslot virtual final 
          instance bool  'System.Threading.Tasks.Dataflow.IReceivableSourceBlock<T>.TryReceiveAll'([out] class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T>& items) cil managed
  // SIG: 20 01 02 10 15 12 79 01 13 00
  {
    .param [1]/*08000047*/ 
    .custom /*0C0000B9:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001D*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .override  method instance bool class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<!T>/*1B000009*/::TryReceiveAll(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!0>&) /* 0A00001E */
    // Method begins at RVA 0x2191
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BroadcastBlock`1::'System.Threading.Tasks.Dataflow.IReceivableSourceBlock<T>.TryReceiveAll'

  .method /*06000049*/ private hidebysig newslot virtual final 
          instance !T  'System.Threading.Tasks.Dataflow.ISourceBlock<T>.ConsumeMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                        class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T> target,
                                                                                        [out] bool& messageConsumed) cil managed
  // SIG: 20 03 13 00 11 3C 15 12 5C 01 13 00 10 02
  {
    .override  method instance !0 class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T>/*1B00000A*/::ConsumeMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                   class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>,
                                                                                                                                   bool&) /* 0A00001F */
    // Method begins at RVA 0x2194
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BroadcastBlock`1::'System.Threading.Tasks.Dataflow.ISourceBlock<T>.ConsumeMessage'

  .method /*0600004A*/ private hidebysig newslot virtual final 
          instance void  'System.Threading.Tasks.Dataflow.ISourceBlock<T>.ReleaseReservation'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                              class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T> target) cil managed
  // SIG: 20 02 01 11 3C 15 12 5C 01 13 00
  {
    .override  method instance void class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T>/*1B00000A*/::ReleaseReservation(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                         class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>) /* 0A000020 */
    // Method begins at RVA 0x2197
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BroadcastBlock`1::'System.Threading.Tasks.Dataflow.ISourceBlock<T>.ReleaseReservation'

  .method /*0600004B*/ private hidebysig newslot virtual final 
          instance bool  'System.Threading.Tasks.Dataflow.ISourceBlock<T>.ReserveMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                          class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T> target) cil managed
  // SIG: 20 02 02 11 3C 15 12 5C 01 13 00
  {
    .override  method instance bool class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T>/*1B00000A*/::ReserveMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                     class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>) /* 0A000021 */
    // Method begins at RVA 0x2199
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BroadcastBlock`1::'System.Threading.Tasks.Dataflow.ISourceBlock<T>.ReserveMessage'

  .method /*0600004C*/ private hidebysig newslot virtual final 
          instance valuetype System.Threading.Tasks.Dataflow.DataflowMessageStatus/*02000010*/ 
          'System.Threading.Tasks.Dataflow.ITargetBlock<T>.OfferMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                         !T messageValue,
                                                                         class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T> source,
                                                                         bool consumeToAccept) cil managed
  // SIG: 20 04 11 40 11 3C 13 00 15 12 58 01 13 00 02
  {
    .override  method instance valuetype System.Threading.Tasks.Dataflow.DataflowMessageStatus/*02000010*/ class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T>/*1B000001*/::OfferMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                                                                                          !0,
                                                                                                                                                                                                          class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!0>,
                                                                                                                                                                                                          bool) /* 0A000013 */
    // Method begins at RVA 0x219c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BroadcastBlock`1::'System.Threading.Tasks.Dataflow.ITargetBlock<T>.OfferMessage'

  .method /*0600004D*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x219f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BroadcastBlock`1::ToString

  .method /*0600004E*/ public hidebysig newslot virtual final 
          instance bool  TryReceive(class [System.Runtime/*23000001*/]System.Predicate`1/*0100001C*/<!T> 'filter',
                                    [out] !T& item) cil managed
  // SIG: 20 02 02 15 12 71 01 13 00 10 13 00
  {
    .param [1]/*08000053*/ 
    .custom /*0C0000BA:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [2]/*08000054*/ 
    .custom /*0C0000BB:0A000022*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.MaybeNullWhenAttribute/*01000021*/::.ctor(bool) /* 0A000022 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x21a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BroadcastBlock`1::TryReceive

  .property /*17000011*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/
          Completion()
  {
    .get instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ System.Threading.Tasks.Dataflow.BroadcastBlock`1/*0200000A*/::get_Completion() /* 06000044 */
  } // end of property BroadcastBlock`1::Completion
} // end of class System.Threading.Tasks.Dataflow.BroadcastBlock`1

.class /*0200000B*/ public auto ansi sealed beforefieldinit System.Threading.Tasks.Dataflow.BufferBlock`1<T>
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/,
                  class System.Threading.Tasks.Dataflow.IPropagatorBlock`2/*02000014*/<!T,!T>/*1B00000B*/,
                  class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T>/*1B00000A*/,
                  class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T>/*1B000001*/,
                  class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<!T>/*1B000009*/
{
  .custom /*0C00003E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/
  .custom /*0C000056:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.IPropagatorBlock`2/*02000014*/<!T,!T>/*1B00000B*/
  .custom /*0C000059:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T>/*1B00000A*/
  .custom /*0C00005B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T>/*1B000001*/
  .custom /*0C00005E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<!T>/*1B000009*/
  .custom /*0C000060:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type T /*2A000009*/ 
    .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*0600004F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21a5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BufferBlock`1::.ctor

  .method /*06000050*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/ dataflowBlockOptions) cil managed
  // SIG: 20 01 01 12 34
  {
    // Method begins at RVA 0x21ad
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method BufferBlock`1::.ctor

  .method /*06000051*/ public hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          get_Completion() cil managed
  // SIG: 20 00 12 69
  {
    // Method begins at RVA 0x21b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferBlock`1::get_Completion

  .method /*06000052*/ public hidebysig specialname 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferBlock`1::get_Count

  .method /*06000053*/ public hidebysig newslot virtual final 
          instance void  Complete() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21bb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BufferBlock`1::Complete

  .method /*06000054*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/ 
          LinkTo(class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T> target,
                 class System.Threading.Tasks.Dataflow.DataflowLinkOptions/*0200000E*/ linkOptions) cil managed
  // SIG: 20 02 12 6D 15 12 5C 01 13 00 12 38
  {
    // Method begins at RVA 0x21bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferBlock`1::LinkTo

  .method /*06000055*/ private hidebysig newslot virtual final 
          instance void  System.Threading.Tasks.Dataflow.IDataflowBlock.Fault(class [System.Runtime/*23000001*/]System.Exception/*01000017*/ exception) cil managed
  // SIG: 20 01 01 12 5D
  {
    .override System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/::Fault /*02000013::0600009F*/ 
    // Method begins at RVA 0x21c0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BufferBlock`1::System.Threading.Tasks.Dataflow.IDataflowBlock.Fault

  .method /*06000056*/ private hidebysig newslot virtual final 
          instance !T  'System.Threading.Tasks.Dataflow.ISourceBlock<T>.ConsumeMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                        class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T> target,
                                                                                        [out] bool& messageConsumed) cil managed
  // SIG: 20 03 13 00 11 3C 15 12 5C 01 13 00 10 02
  {
    .override  method instance !0 class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T>/*1B00000A*/::ConsumeMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                   class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>,
                                                                                                                                   bool&) /* 0A00001F */
    // Method begins at RVA 0x21c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferBlock`1::'System.Threading.Tasks.Dataflow.ISourceBlock<T>.ConsumeMessage'

  .method /*06000057*/ private hidebysig newslot virtual final 
          instance void  'System.Threading.Tasks.Dataflow.ISourceBlock<T>.ReleaseReservation'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                              class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T> target) cil managed
  // SIG: 20 02 01 11 3C 15 12 5C 01 13 00
  {
    .override  method instance void class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T>/*1B00000A*/::ReleaseReservation(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                         class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>) /* 0A000020 */
    // Method begins at RVA 0x21c5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method BufferBlock`1::'System.Threading.Tasks.Dataflow.ISourceBlock<T>.ReleaseReservation'

  .method /*06000058*/ private hidebysig newslot virtual final 
          instance bool  'System.Threading.Tasks.Dataflow.ISourceBlock<T>.ReserveMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                          class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T> target) cil managed
  // SIG: 20 02 02 11 3C 15 12 5C 01 13 00
  {
    .override  method instance bool class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T>/*1B00000A*/::ReserveMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                     class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>) /* 0A000021 */
    // Method begins at RVA 0x21c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferBlock`1::'System.Threading.Tasks.Dataflow.ISourceBlock<T>.ReserveMessage'

  .method /*06000059*/ private hidebysig newslot virtual final 
          instance valuetype System.Threading.Tasks.Dataflow.DataflowMessageStatus/*02000010*/ 
          'System.Threading.Tasks.Dataflow.ITargetBlock<T>.OfferMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                         !T messageValue,
                                                                         class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T> source,
                                                                         bool consumeToAccept) cil managed
  // SIG: 20 04 11 40 11 3C 13 00 15 12 58 01 13 00 02
  {
    .override  method instance valuetype System.Threading.Tasks.Dataflow.DataflowMessageStatus/*02000010*/ class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T>/*1B000001*/::OfferMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                                                                                          !0,
                                                                                                                                                                                                          class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!0>,
                                                                                                                                                                                                          bool) /* 0A000013 */
    // Method begins at RVA 0x21ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferBlock`1::'System.Threading.Tasks.Dataflow.ITargetBlock<T>.OfferMessage'

  .method /*0600005A*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferBlock`1::ToString

  .method /*0600005B*/ public hidebysig newslot virtual final 
          instance bool  TryReceive(class [System.Runtime/*23000001*/]System.Predicate`1/*0100001C*/<!T> 'filter',
                                    [out] !T& item) cil managed
  // SIG: 20 02 02 15 12 71 01 13 00 10 13 00
  {
    .param [1]/*08000064*/ 
    .custom /*0C0000BF:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [2]/*08000065*/ 
    .custom /*0C0000C1:0A000022*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.MaybeNullWhenAttribute/*01000021*/::.ctor(bool) /* 0A000022 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x21d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferBlock`1::TryReceive

  .method /*0600005C*/ public hidebysig newslot virtual final 
          instance bool  TryReceiveAll([out] class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T>& items) cil managed
  // SIG: 20 01 02 10 15 12 79 01 13 00
  {
    .param [1]/*08000066*/ 
    .custom /*0C0000C3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .custom /*0C0000C4:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001D*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferBlock`1::TryReceiveAll

  .property /*17000012*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/
          Completion()
  {
    .get instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ System.Threading.Tasks.Dataflow.BufferBlock`1/*0200000B*/::get_Completion() /* 06000051 */
  } // end of property BufferBlock`1::Completion
  .property /*17000013*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Threading.Tasks.Dataflow.BufferBlock`1/*0200000B*/::get_Count() /* 06000052 */
  } // end of property BufferBlock`1::Count
} // end of class System.Threading.Tasks.Dataflow.BufferBlock`1

.class /*0200000C*/ public abstract auto ansi sealed beforefieldinit System.Threading.Tasks.Dataflow.DataflowBlock
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000041:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000042:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000043:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*0600005D*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.IObservable`1/*01000022*/<!!TOutput> 
          AsObservable<TOutput>(class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!TOutput> source) cil managed
  // SIG: 10 01 01 15 12 80 89 01 1E 00 15 12 58 01 1E 00
  {
    .custom /*0C0000BC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOutput /*2A000019*/ 
      .custom /*0C000068:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::AsObservable

  .method /*0600005E*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.IObserver`1/*01000023*/<!!TInput> 
          AsObserver<TInput>(class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!!TInput> target) cil managed
  // SIG: 10 01 01 15 12 80 8D 01 1E 00 15 12 5C 01 1E 00
  {
    .custom /*0C0000BD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TInput /*2A00001A*/ 
      .custom /*0C00006C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::AsObserver

  .method /*0600005F*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<int32> 
          Choose<T1,T2>(class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!T1> source1,
                        class [System.Runtime/*23000001*/]System.Action`1/*01000018*/<!!T1> action1,
                        class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!T2> source2,
                        class [System.Runtime/*23000001*/]System.Action`1/*01000018*/<!!T2> action2) cil managed
  // SIG: 10 02 04 15 12 80 91 01 08 15 12 58 01 1E 00 15 12 61 01 1E 00 15 12 58 01 1E 01 15 12 61 01 1E
  //      01
  {
    .param type T1 /*2A00001B*/ 
      .custom /*0C000070:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type T2 /*2A00001C*/ 
      .custom /*0C000073:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::Choose

  .method /*06000060*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<int32> 
          Choose<T1,T2>(class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!T1> source1,
                        class [System.Runtime/*23000001*/]System.Action`1/*01000018*/<!!T1> action1,
                        class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!T2> source2,
                        class [System.Runtime/*23000001*/]System.Action`1/*01000018*/<!!T2> action2,
                        class System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/ dataflowBlockOptions) cil managed
  // SIG: 10 02 05 15 12 80 91 01 08 15 12 58 01 1E 00 15 12 61 01 1E 00 15 12 58 01 1E 01 15 12 61 01 1E
  //      01 12 34
  {
    .param type T1 /*2A00001D*/ 
      .custom /*0C000075:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type T2 /*2A00001E*/ 
      .custom /*0C000078:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::Choose

  .method /*06000061*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<int32> 
          Choose<T1,T2,T3>(class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!T1> source1,
                           class [System.Runtime/*23000001*/]System.Action`1/*01000018*/<!!T1> action1,
                           class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!T2> source2,
                           class [System.Runtime/*23000001*/]System.Action`1/*01000018*/<!!T2> action2,
                           class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!T3> source3,
                           class [System.Runtime/*23000001*/]System.Action`1/*01000018*/<!!T3> action3) cil managed
  // SIG: 10 03 06 15 12 80 91 01 08 15 12 58 01 1E 00 15 12 61 01 1E 00 15 12 58 01 1E 01 15 12 61 01 1E
  //      01 15 12 58 01 1E 02 15 12 61 01 1E 02
  {
    .param type T1 /*2A00001F*/ 
      .custom /*0C00007B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type T2 /*2A000020*/ 
      .custom /*0C00007F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type T3 /*2A000021*/ 
      .custom /*0C000081:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::Choose

  .method /*06000062*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<int32> 
          Choose<T1,T2,T3>(class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!T1> source1,
                           class [System.Runtime/*23000001*/]System.Action`1/*01000018*/<!!T1> action1,
                           class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!T2> source2,
                           class [System.Runtime/*23000001*/]System.Action`1/*01000018*/<!!T2> action2,
                           class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!T3> source3,
                           class [System.Runtime/*23000001*/]System.Action`1/*01000018*/<!!T3> action3,
                           class System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/ dataflowBlockOptions) cil managed
  // SIG: 10 03 07 15 12 80 91 01 08 15 12 58 01 1E 00 15 12 61 01 1E 00 15 12 58 01 1E 01 15 12 61 01 1E
  //      01 15 12 58 01 1E 02 15 12 61 01 1E 02 12 34
  {
    .param type T1 /*2A000022*/ 
      .custom /*0C000083:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type T2 /*2A000023*/ 
      .custom /*0C000085:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type T3 /*2A000024*/ 
      .custom /*0C000087:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::Choose

  .method /*06000063*/ public hidebysig static 
          class System.Threading.Tasks.Dataflow.IPropagatorBlock`2/*02000014*/<!!TInput,!!TOutput> 
          Encapsulate<TInput,TOutput>(class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!!TInput> target,
                                      class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!TOutput> source) cil managed
  // SIG: 10 02 02 15 12 50 02 1E 00 1E 01 15 12 5C 01 1E 00 15 12 58 01 1E 01
  {
    .param type TInput /*2A000025*/ 
      .custom /*0C000089:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TOutput /*2A000026*/ 
      .custom /*0C00008B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::Encapsulate

  .method /*06000064*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/ 
          LinkTo<TOutput>(class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!TOutput> source,
                          class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!!TOutput> target) cil managed
  // SIG: 10 01 02 12 6D 15 12 58 01 1E 00 15 12 5C 01 1E 00
  {
    .custom /*0C0000BE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOutput /*2A000027*/ 
      .custom /*0C00008D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::LinkTo

  .method /*06000065*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/ 
          LinkTo<TOutput>(class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!TOutput> source,
                          class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!!TOutput> target,
                          class [System.Runtime/*23000001*/]System.Predicate`1/*0100001C*/<!!TOutput> predicate) cil managed
  // SIG: 10 01 03 12 6D 15 12 58 01 1E 00 15 12 5C 01 1E 00 15 12 71 01 1E 00
  {
    .custom /*0C0000C0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOutput /*2A000028*/ 
      .custom /*0C00008F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::LinkTo

  .method /*06000066*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/ 
          LinkTo<TOutput>(class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!TOutput> source,
                          class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!!TOutput> target,
                          class System.Threading.Tasks.Dataflow.DataflowLinkOptions/*0200000E*/ linkOptions,
                          class [System.Runtime/*23000001*/]System.Predicate`1/*0100001C*/<!!TOutput> predicate) cil managed
  // SIG: 10 01 04 12 6D 15 12 58 01 1E 00 15 12 5C 01 1E 00 12 38 15 12 71 01 1E 00
  {
    .custom /*0C0000C2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOutput /*2A000029*/ 
      .custom /*0C000091:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::LinkTo

  .method /*06000067*/ public hidebysig static 
          class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!!TInput> 
          NullTarget<TInput>() cil managed
  // SIG: 10 01 00 15 12 5C 01 1E 00
  {
    .param type TInput /*2A00002A*/ 
      .custom /*0C000093:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::NullTarget

  .method /*06000068*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<bool> 
          OutputAvailableAsync<TOutput>(class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!TOutput> source) cil managed
  // SIG: 10 01 01 15 12 80 91 01 02 15 12 58 01 1E 00
  {
    .custom /*0C0000C5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOutput /*2A00002B*/ 
      .custom /*0C000095:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::OutputAvailableAsync

  .method /*06000069*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<bool> 
          OutputAvailableAsync<TOutput>(class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!TOutput> source,
                                        valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000025*/ cancellationToken) cil managed
  // SIG: 10 01 02 15 12 80 91 01 02 15 12 58 01 1E 00 11 80 95
  {
    .custom /*0C0000C6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOutput /*2A00002C*/ 
      .custom /*0C000097:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::OutputAvailableAsync

  .method /*0600006A*/ public hidebysig static 
          bool  Post<TInput>(class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!!TInput> target,
                             !!TInput item) cil managed
  // SIG: 10 01 02 02 15 12 5C 01 1E 00 1E 00
  {
    .custom /*0C0000C7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TInput /*2A00002D*/ 
      .custom /*0C000099:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::Post

  .method /*0600006B*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<!!TOutput> 
          ReceiveAsync<TOutput>(class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!TOutput> source) cil managed
  // SIG: 10 01 01 15 12 80 91 01 1E 00 15 12 58 01 1E 00
  {
    .custom /*0C0000C8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOutput /*2A00002E*/ 
      .custom /*0C00009C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2200
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::ReceiveAsync

  .method /*0600006C*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<!!TOutput> 
          ReceiveAsync<TOutput>(class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!TOutput> source,
                                valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000025*/ cancellationToken) cil managed
  // SIG: 10 01 02 15 12 80 91 01 1E 00 15 12 58 01 1E 00 11 80 95
  {
    .custom /*0C0000C9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOutput /*2A00002F*/ 
      .custom /*0C0000A0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2203
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::ReceiveAsync

  .method /*0600006D*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<!!TOutput> 
          ReceiveAsync<TOutput>(class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!TOutput> source,
                                valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ timeout) cil managed
  // SIG: 10 01 02 15 12 80 91 01 1E 00 15 12 58 01 1E 00 11 80 99
  {
    .custom /*0C0000CA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOutput /*2A000030*/ 
      .custom /*0C0000A4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2206
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::ReceiveAsync

  .method /*0600006E*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<!!TOutput> 
          ReceiveAsync<TOutput>(class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!TOutput> source,
                                valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ timeout,
                                valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000025*/ cancellationToken) cil managed
  // SIG: 10 01 03 15 12 80 91 01 1E 00 15 12 58 01 1E 00 11 80 99 11 80 95
  {
    .custom /*0C0000CB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOutput /*2A000031*/ 
      .custom /*0C0000A6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2209
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::ReceiveAsync

  .method /*0600006F*/ public hidebysig static 
          !!TOutput  Receive<TOutput>(class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!TOutput> source) cil managed
  // SIG: 10 01 01 1E 00 15 12 58 01 1E 00
  {
    .custom /*0C0000CC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOutput /*2A000032*/ 
      .custom /*0C0000A8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x220c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::Receive

  .method /*06000070*/ public hidebysig static 
          !!TOutput  Receive<TOutput>(class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!TOutput> source,
                                      valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000025*/ cancellationToken) cil managed
  // SIG: 10 01 02 1E 00 15 12 58 01 1E 00 11 80 95
  {
    .custom /*0C0000CD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOutput /*2A000033*/ 
      .custom /*0C0000AA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x220f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::Receive

  .method /*06000071*/ public hidebysig static 
          !!TOutput  Receive<TOutput>(class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!TOutput> source,
                                      valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ timeout) cil managed
  // SIG: 10 01 02 1E 00 15 12 58 01 1E 00 11 80 99
  {
    .custom /*0C0000CE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOutput /*2A000034*/ 
      .custom /*0C0000AC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2212
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::Receive

  .method /*06000072*/ public hidebysig static 
          !!TOutput  Receive<TOutput>(class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!!TOutput> source,
                                      valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000026*/ timeout,
                                      valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000025*/ cancellationToken) cil managed
  // SIG: 10 01 03 1E 00 15 12 58 01 1E 00 11 80 99 11 80 95
  {
    .custom /*0C0000CF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOutput /*2A000035*/ 
      .custom /*0C0000AD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2215
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::Receive

  .method /*06000073*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<bool> 
          SendAsync<TInput>(class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!!TInput> target,
                            !!TInput item) cil managed
  // SIG: 10 01 02 15 12 80 91 01 02 15 12 5C 01 1E 00 1E 00
  {
    .custom /*0C0000D0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TInput /*2A000036*/ 
      .custom /*0C0000AE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2218
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::SendAsync

  .method /*06000074*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<bool> 
          SendAsync<TInput>(class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!!TInput> target,
                            !!TInput item,
                            valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000025*/ cancellationToken) cil managed
  // SIG: 10 01 03 15 12 80 91 01 02 15 12 5C 01 1E 00 1E 00 11 80 95
  {
    .custom /*0C0000D1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TInput /*2A000037*/ 
      .custom /*0C0000AF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x221b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::SendAsync

  .method /*06000075*/ public hidebysig static 
          bool  TryReceive<TOutput>(class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<!!TOutput> source,
                                    [out] !!TOutput& item) cil managed
  // SIG: 10 01 02 02 15 12 54 01 1E 00 10 1E 00
  {
    .custom /*0C0000D2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOutput /*2A000038*/ 
      .custom /*0C0000B0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000A5*/ 
    .custom /*0C0000D6:0A000022*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.MaybeNullWhenAttribute/*01000021*/::.ctor(bool) /* 0A000022 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x221e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::TryReceive

  .method /*06000076*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*01000027*/<!!TOutput> 
          ReceiveAllAsync<TOutput>(class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<!!TOutput> source,
                                   [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000025*/ cancellationToken) cil managed
  // SIG: 10 01 02 15 12 80 9D 01 1E 00 15 12 54 01 1E 00 11 80 95
  {
    .custom /*0C0000D3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TOutput /*2A000039*/ 
      .custom /*0C0000B1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000A7*/  = nullref
    // Method begins at RVA 0x2221
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlock::ReceiveAllAsync

} // end of class System.Threading.Tasks.Dataflow.DataflowBlock

.class /*0200000D*/ public auto ansi beforefieldinit System.Threading.Tasks.Dataflow.DataflowBlockOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000046:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000047:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000004*/ public static literal int32 Unbounded = int32(0xFFFFFFFF)
  .method /*06000077*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2224
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataflowBlockOptions::.ctor

  .method /*06000078*/ public hidebysig specialname 
          instance int32  get_BoundedCapacity() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x222c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlockOptions::get_BoundedCapacity

  .method /*06000079*/ public hidebysig specialname 
          instance void  set_BoundedCapacity(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x222f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataflowBlockOptions::set_BoundedCapacity

  .method /*0600007A*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000025*/ 
          get_CancellationToken() cil managed
  // SIG: 20 00 11 80 95
  {
    // Method begins at RVA 0x2231
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlockOptions::get_CancellationToken

  .method /*0600007B*/ public hidebysig specialname 
          instance void  set_CancellationToken(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000025*/ 'value') cil managed
  // SIG: 20 01 01 11 80 95
  {
    // Method begins at RVA 0x2234
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataflowBlockOptions::set_CancellationToken

  .method /*0600007C*/ public hidebysig specialname 
          instance bool  get_EnsureOrdered() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2236
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlockOptions::get_EnsureOrdered

  .method /*0600007D*/ public hidebysig specialname 
          instance void  set_EnsureOrdered(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2239
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataflowBlockOptions::set_EnsureOrdered

  .method /*0600007E*/ public hidebysig specialname 
          instance int32  get_MaxMessagesPerTask() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x223b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlockOptions::get_MaxMessagesPerTask

  .method /*0600007F*/ public hidebysig specialname 
          instance void  set_MaxMessagesPerTask(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x223e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataflowBlockOptions::set_MaxMessagesPerTask

  .method /*06000080*/ public hidebysig specialname 
          instance string  get_NameFormat() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2240
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlockOptions::get_NameFormat

  .method /*06000081*/ public hidebysig specialname 
          instance void  set_NameFormat(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2243
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataflowBlockOptions::set_NameFormat

  .method /*06000082*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.TaskScheduler/*01000028*/ 
          get_TaskScheduler() cil managed
  // SIG: 20 00 12 80 A1
  {
    // Method begins at RVA 0x2245
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowBlockOptions::get_TaskScheduler

  .method /*06000083*/ public hidebysig specialname 
          instance void  set_TaskScheduler(class [System.Runtime/*23000001*/]System.Threading.Tasks.TaskScheduler/*01000028*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A1
  {
    // Method begins at RVA 0x2248
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataflowBlockOptions::set_TaskScheduler

  .property /*17000014*/ instance callconv(8) int32
          BoundedCapacity()
  {
    .get instance int32 System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/::get_BoundedCapacity() /* 06000078 */
    .set instance void System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/::set_BoundedCapacity(int32) /* 06000079 */
  } // end of property DataflowBlockOptions::BoundedCapacity
  .property /*17000015*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000025*/
          CancellationToken()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000025*/ System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/::get_CancellationToken() /* 0600007A */
    .set instance void System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/::set_CancellationToken(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000025*/) /* 0600007B */
  } // end of property DataflowBlockOptions::CancellationToken
  .property /*17000016*/ instance callconv(8) bool
          EnsureOrdered()
  {
    .get instance bool System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/::get_EnsureOrdered() /* 0600007C */
    .set instance void System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/::set_EnsureOrdered(bool) /* 0600007D */
  } // end of property DataflowBlockOptions::EnsureOrdered
  .property /*17000017*/ instance callconv(8) int32
          MaxMessagesPerTask()
  {
    .get instance int32 System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/::get_MaxMessagesPerTask() /* 0600007E */
    .set instance void System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/::set_MaxMessagesPerTask(int32) /* 0600007F */
  } // end of property DataflowBlockOptions::MaxMessagesPerTask
  .property /*17000018*/ instance callconv(8) string
          NameFormat()
  {
    .get instance string System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/::get_NameFormat() /* 06000080 */
    .set instance void System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/::set_NameFormat(string) /* 06000081 */
  } // end of property DataflowBlockOptions::NameFormat
  .property /*17000019*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.Tasks.TaskScheduler/*01000028*/
          TaskScheduler()
  {
    .get instance class [System.Runtime/*23000001*/]System.Threading.Tasks.TaskScheduler/*01000028*/ System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/::get_TaskScheduler() /* 06000082 */
    .set instance void System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/::set_TaskScheduler(class [System.Runtime/*23000001*/]System.Threading.Tasks.TaskScheduler/*01000028*/) /* 06000083 */
  } // end of property DataflowBlockOptions::TaskScheduler
} // end of class System.Threading.Tasks.Dataflow.DataflowBlockOptions

.class /*0200000E*/ public auto ansi beforefieldinit System.Threading.Tasks.Dataflow.DataflowLinkOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .method /*06000084*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x224a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DataflowLinkOptions::.ctor

  .method /*06000085*/ public hidebysig specialname 
          instance bool  get_Append() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2252
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowLinkOptions::get_Append

  .method /*06000086*/ public hidebysig specialname 
          instance void  set_Append(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2255
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataflowLinkOptions::set_Append

  .method /*06000087*/ public hidebysig specialname 
          instance int32  get_MaxMessages() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2257
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowLinkOptions::get_MaxMessages

  .method /*06000088*/ public hidebysig specialname 
          instance void  set_MaxMessages(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x225a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataflowLinkOptions::set_MaxMessages

  .method /*06000089*/ public hidebysig specialname 
          instance bool  get_PropagateCompletion() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x225c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowLinkOptions::get_PropagateCompletion

  .method /*0600008A*/ public hidebysig specialname 
          instance void  set_PropagateCompletion(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x225f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DataflowLinkOptions::set_PropagateCompletion

  .property /*1700001A*/ instance callconv(8) bool
          Append()
  {
    .get instance bool System.Threading.Tasks.Dataflow.DataflowLinkOptions/*0200000E*/::get_Append() /* 06000085 */
    .set instance void System.Threading.Tasks.Dataflow.DataflowLinkOptions/*0200000E*/::set_Append(bool) /* 06000086 */
  } // end of property DataflowLinkOptions::Append
  .property /*1700001B*/ instance callconv(8) int32
          MaxMessages()
  {
    .get instance int32 System.Threading.Tasks.Dataflow.DataflowLinkOptions/*0200000E*/::get_MaxMessages() /* 06000087 */
    .set instance void System.Threading.Tasks.Dataflow.DataflowLinkOptions/*0200000E*/::set_MaxMessages(int32) /* 06000088 */
  } // end of property DataflowLinkOptions::MaxMessages
  .property /*1700001C*/ instance callconv(8) bool
          PropagateCompletion()
  {
    .get instance bool System.Threading.Tasks.Dataflow.DataflowLinkOptions/*0200000E*/::get_PropagateCompletion() /* 06000089 */
    .set instance void System.Threading.Tasks.Dataflow.DataflowLinkOptions/*0200000E*/::set_PropagateCompletion(bool) /* 0600008A */
  } // end of property DataflowLinkOptions::PropagateCompletion
} // end of class System.Threading.Tasks.Dataflow.DataflowLinkOptions

.class /*0200000F*/ public sequential ansi sealed beforefieldinit System.Threading.Tasks.Dataflow.DataflowMessageHeader
       extends [System.Runtime/*23000001*/]System.ValueType/*0100002A*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*0100002B*/<valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/>/*1B00000C*/
{
  .custom /*0C00004C:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000029*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
  .field /*04000005*/ private initonly int32 _dummyPrimitive
  .method /*0600008B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int64 id) cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2261
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowMessageHeader::.ctor

  .method /*0600008C*/ public hidebysig specialname 
          instance int64  get_Id() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2264
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowMessageHeader::get_Id

  .method /*0600008D*/ public hidebysig specialname 
          instance bool  get_IsValid() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2267
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowMessageHeader::get_IsValid

  .method /*0600008E*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000D4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000B2*/ 
    .custom /*0C0000D8:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001D*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x226a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowMessageHeader::Equals

  .method /*0600008F*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ other) cil managed
  // SIG: 20 01 02 11 3C
  {
    // Method begins at RVA 0x226d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowMessageHeader::Equals

  .method /*06000090*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2270
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowMessageHeader::GetHashCode

  .method /*06000091*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ left,
                            valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ right) cil managed
  // SIG: 00 02 02 11 3C 11 3C
  {
    // Method begins at RVA 0x2273
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowMessageHeader::op_Equality

  .method /*06000092*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ left,
                              valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ right) cil managed
  // SIG: 00 02 02 11 3C 11 3C
  {
    // Method begins at RVA 0x2276
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DataflowMessageHeader::op_Inequality

  .property /*1700001D*/ instance callconv(8) int64
          Id()
  {
    .get instance int64 System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/::get_Id() /* 0600008C */
  } // end of property DataflowMessageHeader::Id
  .property /*1700001E*/ instance callconv(8) bool
          IsValid()
  {
    .get instance bool System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/::get_IsValid() /* 0600008D */
  } // end of property DataflowMessageHeader::IsValid
} // end of class System.Threading.Tasks.Dataflow.DataflowMessageHeader

.class /*02000010*/ public auto ansi sealed System.Threading.Tasks.Dataflow.DataflowMessageStatus
       extends [System.Runtime/*23000001*/]System.Enum/*0100002C*/
{
  .field /*04000006*/ public specialname rtspecialname int32 value__
  .field /*04000007*/ public static literal valuetype System.Threading.Tasks.Dataflow.DataflowMessageStatus/*02000010*/ Accepted = int32(0x00000000)
  .field /*04000008*/ public static literal valuetype System.Threading.Tasks.Dataflow.DataflowMessageStatus/*02000010*/ Declined = int32(0x00000001)
  .field /*04000009*/ public static literal valuetype System.Threading.Tasks.Dataflow.DataflowMessageStatus/*02000010*/ Postponed = int32(0x00000002)
  .field /*0400000A*/ public static literal valuetype System.Threading.Tasks.Dataflow.DataflowMessageStatus/*02000010*/ NotAvailable = int32(0x00000003)
  .field /*0400000B*/ public static literal valuetype System.Threading.Tasks.Dataflow.DataflowMessageStatus/*02000010*/ DecliningPermanently = int32(0x00000004)
} // end of class System.Threading.Tasks.Dataflow.DataflowMessageStatus

.class /*02000011*/ public auto ansi beforefieldinit System.Threading.Tasks.Dataflow.ExecutionDataflowBlockOptions
       extends System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/
{
  .method /*06000093*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2279
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000077       */ call       instance void System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/::.ctor() /* 06000077 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ExecutionDataflowBlockOptions::.ctor

  .method /*06000094*/ public hidebysig specialname 
          instance int32  get_MaxDegreeOfParallelism() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2281
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExecutionDataflowBlockOptions::get_MaxDegreeOfParallelism

  .method /*06000095*/ public hidebysig specialname 
          instance void  set_MaxDegreeOfParallelism(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2284
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ExecutionDataflowBlockOptions::set_MaxDegreeOfParallelism

  .method /*06000096*/ public hidebysig specialname 
          instance bool  get_SingleProducerConstrained() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2286
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExecutionDataflowBlockOptions::get_SingleProducerConstrained

  .method /*06000097*/ public hidebysig specialname 
          instance void  set_SingleProducerConstrained(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2289
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ExecutionDataflowBlockOptions::set_SingleProducerConstrained

  .property /*1700001F*/ instance callconv(8) int32
          MaxDegreeOfParallelism()
  {
    .get instance int32 System.Threading.Tasks.Dataflow.ExecutionDataflowBlockOptions/*02000011*/::get_MaxDegreeOfParallelism() /* 06000094 */
    .set instance void System.Threading.Tasks.Dataflow.ExecutionDataflowBlockOptions/*02000011*/::set_MaxDegreeOfParallelism(int32) /* 06000095 */
  } // end of property ExecutionDataflowBlockOptions::MaxDegreeOfParallelism
  .property /*17000020*/ instance callconv(8) bool
          SingleProducerConstrained()
  {
    .get instance bool System.Threading.Tasks.Dataflow.ExecutionDataflowBlockOptions/*02000011*/::get_SingleProducerConstrained() /* 06000096 */
    .set instance void System.Threading.Tasks.Dataflow.ExecutionDataflowBlockOptions/*02000011*/::set_SingleProducerConstrained(bool) /* 06000097 */
  } // end of property ExecutionDataflowBlockOptions::SingleProducerConstrained
} // end of class System.Threading.Tasks.Dataflow.ExecutionDataflowBlockOptions

.class /*02000012*/ public auto ansi beforefieldinit System.Threading.Tasks.Dataflow.GroupingDataflowBlockOptions
       extends System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/
{
  .method /*06000098*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x228b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000077       */ call       instance void System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/::.ctor() /* 06000077 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GroupingDataflowBlockOptions::.ctor

  .method /*06000099*/ public hidebysig specialname 
          instance bool  get_Greedy() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2293
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GroupingDataflowBlockOptions::get_Greedy

  .method /*0600009A*/ public hidebysig specialname 
          instance void  set_Greedy(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2296
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GroupingDataflowBlockOptions::set_Greedy

  .method /*0600009B*/ public hidebysig specialname 
          instance int64  get_MaxNumberOfGroups() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2298
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GroupingDataflowBlockOptions::get_MaxNumberOfGroups

  .method /*0600009C*/ public hidebysig specialname 
          instance void  set_MaxNumberOfGroups(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x229b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GroupingDataflowBlockOptions::set_MaxNumberOfGroups

  .property /*17000021*/ instance callconv(8) bool
          Greedy()
  {
    .get instance bool System.Threading.Tasks.Dataflow.GroupingDataflowBlockOptions/*02000012*/::get_Greedy() /* 06000099 */
    .set instance void System.Threading.Tasks.Dataflow.GroupingDataflowBlockOptions/*02000012*/::set_Greedy(bool) /* 0600009A */
  } // end of property GroupingDataflowBlockOptions::Greedy
  .property /*17000022*/ instance callconv(8) int64
          MaxNumberOfGroups()
  {
    .get instance int64 System.Threading.Tasks.Dataflow.GroupingDataflowBlockOptions/*02000012*/::get_MaxNumberOfGroups() /* 0600009B */
    .set instance void System.Threading.Tasks.Dataflow.GroupingDataflowBlockOptions/*02000012*/::set_MaxNumberOfGroups(int64) /* 0600009C */
  } // end of property GroupingDataflowBlockOptions::MaxNumberOfGroups
} // end of class System.Threading.Tasks.Dataflow.GroupingDataflowBlockOptions

.class /*02000013*/ interface public abstract auto ansi System.Threading.Tasks.Dataflow.IDataflowBlock
{
  .custom /*0C000055:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600009D*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          get_Completion() cil managed
  // SIG: 20 00 12 69
  {
    // Method begins at RVA 0x0
  } // end of method IDataflowBlock::get_Completion

  .method /*0600009E*/ public hidebysig newslot abstract virtual 
          instance void  Complete() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method IDataflowBlock::Complete

  .method /*0600009F*/ public hidebysig newslot abstract virtual 
          instance void  Fault(class [System.Runtime/*23000001*/]System.Exception/*01000017*/ exception) cil managed
  // SIG: 20 01 01 12 5D
  {
    // Method begins at RVA 0x0
  } // end of method IDataflowBlock::Fault

  .property /*17000023*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/
          Completion()
  {
    .get instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/::get_Completion() /* 0600009D */
  } // end of property IDataflowBlock::Completion
} // end of class System.Threading.Tasks.Dataflow.IDataflowBlock

.class /*02000014*/ interface public abstract auto ansi System.Threading.Tasks.Dataflow.IPropagatorBlock`2<- TInput,+ TOutput>
       implements System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/,
                  class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!TOutput>/*1B00000D*/,
                  class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TInput>/*1B000001*/
{
  .custom /*0C000058:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .interfaceimpl type System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/
  .custom /*0C000067:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!TOutput>/*1B00000D*/
  .custom /*0C00006B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TInput>/*1B000001*/
  .custom /*0C00006F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
} // end of class System.Threading.Tasks.Dataflow.IPropagatorBlock`2

.class /*02000015*/ interface public abstract auto ansi System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1<TOutput>
       implements System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/,
                  class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!TOutput>/*1B00000A*/
{
  .interfaceimpl type class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!TOutput>/*1B00000A*/
  .custom /*0C000074:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type TOutput /*2A00000C*/ 
    .custom /*0C000045:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*060000A0*/ public hidebysig newslot abstract virtual 
          instance bool  TryReceive(class [System.Runtime/*23000001*/]System.Predicate`1/*0100001C*/<!TOutput> 'filter',
                                    [out] !TOutput& item) cil managed
  // SIG: 20 02 02 15 12 71 01 13 00 10 13 00
  {
    .custom /*0C0000D5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [1]/*080000BD*/ 
    .custom /*0C0000D9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [2]/*080000BE*/ 
    .custom /*0C0000DA:0A000022*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.MaybeNullWhenAttribute/*01000021*/::.ctor(bool) /* 0A000022 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IReceivableSourceBlock`1::TryReceive

  .method /*060000A1*/ public hidebysig newslot abstract virtual 
          instance bool  TryReceiveAll([out] class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!TOutput>& items) cil managed
  // SIG: 20 01 02 10 15 12 79 01 13 00
  {
    .param [1]/*080000BF*/ 
    .custom /*0C0000DB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .custom /*0C0000DC:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001D*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IReceivableSourceBlock`1::TryReceiveAll

} // end of class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1

.class /*02000016*/ interface public abstract auto ansi System.Threading.Tasks.Dataflow.ISourceBlock`1<+ TOutput>
       implements System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/
{
  .custom /*0C00005D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .interfaceimpl type System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/
  .custom /*0C000077:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .param type TOutput /*2A00000D*/ 
    .custom /*0C000049:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*060000A2*/ public hidebysig newslot abstract virtual 
          instance !TOutput  ConsumeMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                            class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TOutput> target,
                                            [out] bool& messageConsumed) cil managed
  // SIG: 20 03 13 00 11 3C 15 12 5C 01 13 00 10 02
  {
    .param [0]/*080000C0*/ 
    .custom /*0C0000DD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ISourceBlock`1::ConsumeMessage

  .method /*060000A3*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/ 
          LinkTo(class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TOutput> target,
                 class System.Threading.Tasks.Dataflow.DataflowLinkOptions/*0200000E*/ linkOptions) cil managed
  // SIG: 20 02 12 6D 15 12 5C 01 13 00 12 38
  {
    // Method begins at RVA 0x0
  } // end of method ISourceBlock`1::LinkTo

  .method /*060000A4*/ public hidebysig newslot abstract virtual 
          instance void  ReleaseReservation(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                            class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TOutput> target) cil managed
  // SIG: 20 02 01 11 3C 15 12 5C 01 13 00
  {
    // Method begins at RVA 0x0
  } // end of method ISourceBlock`1::ReleaseReservation

  .method /*060000A5*/ public hidebysig newslot abstract virtual 
          instance bool  ReserveMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                        class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TOutput> target) cil managed
  // SIG: 20 02 02 11 3C 15 12 5C 01 13 00
  {
    // Method begins at RVA 0x0
  } // end of method ISourceBlock`1::ReserveMessage

} // end of class System.Threading.Tasks.Dataflow.ISourceBlock`1

.class /*02000017*/ interface public abstract auto ansi System.Threading.Tasks.Dataflow.ITargetBlock`1<- TInput>
       implements System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/
{
  .param type TInput /*2A00000E*/ 
    .custom /*0C00004B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*060000A6*/ public hidebysig newslot abstract virtual 
          instance valuetype System.Threading.Tasks.Dataflow.DataflowMessageStatus/*02000010*/ 
          OfferMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                       !TInput messageValue,
                       class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!TInput> source,
                       bool consumeToAccept) cil managed
  // SIG: 20 04 11 40 11 3C 13 00 15 12 58 01 13 00 02
  {
    .custom /*0C0000D7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [3]/*080000CC*/ 
    .custom /*0C0000DE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ITargetBlock`1::OfferMessage

} // end of class System.Threading.Tasks.Dataflow.ITargetBlock`1

.class /*02000018*/ public auto ansi sealed beforefieldinit System.Threading.Tasks.Dataflow.JoinBlock`2<T1,T2>
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/,
                  class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<!T1,!T2>>/*1B00000F*/,
                  class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<!T1,!T2>>/*1B00000E*/
{
  .custom /*0C000062:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000063:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/
  .custom /*0C00007E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<!T1,!T2>>/*1B00000F*/
  .custom /*0C000080:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 01 01 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<!T1,!T2>>/*1B00000E*/
  .custom /*0C000082:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 01 01 01 00 00 ) 
  .param type T1 /*2A00000F*/ 
    .custom /*0C00004E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .param type T2 /*2A000010*/ 
    .custom /*0C000050:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*060000A7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x229d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method JoinBlock`2::.ctor

  .method /*060000A8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Threading.Tasks.Dataflow.GroupingDataflowBlockOptions/*02000012*/ dataflowBlockOptions) cil managed
  // SIG: 20 01 01 12 48
  {
    // Method begins at RVA 0x22a5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method JoinBlock`2::.ctor

  .method /*060000A9*/ public hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          get_Completion() cil managed
  // SIG: 20 00 12 69
  {
    // Method begins at RVA 0x22ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`2::get_Completion

  .method /*060000AA*/ public hidebysig specialname 
          instance int32  get_OutputCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x22b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`2::get_OutputCount

  .method /*060000AB*/ public hidebysig specialname 
          instance class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T1> 
          get_Target1() cil managed
  // SIG: 20 00 15 12 5C 01 13 00
  {
    // Method begins at RVA 0x22b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`2::get_Target1

  .method /*060000AC*/ public hidebysig specialname 
          instance class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T2> 
          get_Target2() cil managed
  // SIG: 20 00 15 12 5C 01 13 01
  {
    // Method begins at RVA 0x22b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`2::get_Target2

  .method /*060000AD*/ public hidebysig newslot virtual final 
          instance void  Complete() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22b9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method JoinBlock`2::Complete

  .method /*060000AE*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/ 
          LinkTo(class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<!T1,!T2>> target,
                 class System.Threading.Tasks.Dataflow.DataflowLinkOptions/*0200000E*/ linkOptions) cil managed
  // SIG: 20 02 12 6D 15 12 5C 01 15 12 7D 02 13 00 13 01 12 38
  {
    // Method begins at RVA 0x22bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`2::LinkTo

  .method /*060000AF*/ private hidebysig newslot virtual final 
          instance void  System.Threading.Tasks.Dataflow.IDataflowBlock.Fault(class [System.Runtime/*23000001*/]System.Exception/*01000017*/ exception) cil managed
  // SIG: 20 01 01 12 5D
  {
    .override System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/::Fault /*02000013::0600009F*/ 
    // Method begins at RVA 0x22be
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method JoinBlock`2::System.Threading.Tasks.Dataflow.IDataflowBlock.Fault

  .method /*060000B0*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<!T1,!T2> 
          'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<T1,T2>>.ConsumeMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                             class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<!T1,!T2>> target,
                                                                                             [out] bool& messageConsumed) cil managed
  // SIG: 20 03 15 12 7D 02 13 00 13 01 11 3C 15 12 5C 01 15 12 7D 02 13 00 13 01 10 02
  {
    .override  method instance !0 class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<!T1,!T2>>/*1B00000E*/::ConsumeMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                                                                                      class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>,
                                                                                                                                                                                                      bool&) /* 0A000024 */
    // Method begins at RVA 0x22c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`2::'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<T1,T2>>.ConsumeMessage'

  .method /*060000B1*/ private hidebysig newslot virtual final 
          instance void  'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<T1,T2>>.ReleaseReservation'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                                                class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<!T1,!T2>> target) cil managed
  // SIG: 20 02 01 11 3C 15 12 5C 01 15 12 7D 02 13 00 13 01
  {
    .override  method instance void class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<!T1,!T2>>/*1B00000E*/::ReleaseReservation(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                                                                                            class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>) /* 0A000025 */
    // Method begins at RVA 0x22c3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method JoinBlock`2::'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<T1,T2>>.ReleaseReservation'

  .method /*060000B2*/ private hidebysig newslot virtual final 
          instance bool  'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<T1,T2>>.ReserveMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                                            class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<!T1,!T2>> target) cil managed
  // SIG: 20 02 02 11 3C 15 12 5C 01 15 12 7D 02 13 00 13 01
  {
    .override  method instance bool class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<!T1,!T2>>/*1B00000E*/::ReserveMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                                                                                        class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>) /* 0A000026 */
    // Method begins at RVA 0x22c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`2::'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<T1,T2>>.ReserveMessage'

  .method /*060000B3*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`2::ToString

  .method /*060000B4*/ public hidebysig newslot virtual final 
          instance bool  TryReceive(class [System.Runtime/*23000001*/]System.Predicate`1/*0100001C*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<!T1,!T2>> 'filter',
                                    [out] class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<!T1,!T2>& item) cil managed
  // SIG: 20 02 02 15 12 71 01 15 12 7D 02 13 00 13 01 10 15 12 7D 02 13 00 13 01
  {
    .param [1]/*080000D9*/ 
    .custom /*0C0000DF:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 01 01 01 00 00 ) 
    .param [2]/*080000DA*/ 
    .custom /*0C0000E0:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .custom /*0C0000E1:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001D*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`2::TryReceive

  .method /*060000B5*/ public hidebysig newslot virtual final 
          instance bool  TryReceiveAll([out] class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<class [System.Runtime/*23000001*/]System.Tuple`2/*0100001F*/<!T1,!T2>>& items) cil managed
  // SIG: 20 01 02 10 15 12 79 01 15 12 7D 02 13 00 13 01
  {
    .param [1]/*080000DB*/ 
    .custom /*0C0000E2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 01 01 01 00 00 ) 
    .custom /*0C0000E3:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001D*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`2::TryReceiveAll

  .property /*17000024*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/
          Completion()
  {
    .get instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ System.Threading.Tasks.Dataflow.JoinBlock`2/*02000018*/::get_Completion() /* 060000A9 */
  } // end of property JoinBlock`2::Completion
  .property /*17000025*/ instance callconv(8) int32
          OutputCount()
  {
    .get instance int32 System.Threading.Tasks.Dataflow.JoinBlock`2/*02000018*/::get_OutputCount() /* 060000AA */
  } // end of property JoinBlock`2::OutputCount
  .property /*17000026*/ instance callconv(8) class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T1>
          Target1()
  {
    .get instance class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T1> System.Threading.Tasks.Dataflow.JoinBlock`2/*02000018*/::get_Target1() /* 060000AB */
  } // end of property JoinBlock`2::Target1
  .property /*17000027*/ instance callconv(8) class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T2>
          Target2()
  {
    .get instance class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T2> System.Threading.Tasks.Dataflow.JoinBlock`2/*02000018*/::get_Target2() /* 060000AC */
  } // end of property JoinBlock`2::Target2
} // end of class System.Threading.Tasks.Dataflow.JoinBlock`2

.class /*02000019*/ public auto ansi sealed beforefieldinit System.Threading.Tasks.Dataflow.JoinBlock`3<T1,T2,T3>
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/,
                  class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<!T1,!T2,!T3>>/*1B000011*/,
                  class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<!T1,!T2,!T3>>/*1B000010*/
{
  .custom /*0C000065:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000066:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/
  .custom /*0C000084:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<!T1,!T2,!T3>>/*1B000011*/
  .custom /*0C000086:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 05 00 00 00 00 01 01 01 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<!T1,!T2,!T3>>/*1B000010*/
  .custom /*0C000088:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 05 00 00 00 00 01 01 01 01 00 00 ) 
  .param type T1 /*2A000011*/ 
    .custom /*0C000052:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .param type T2 /*2A000012*/ 
    .custom /*0C000054:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .param type T3 /*2A000013*/ 
    .custom /*0C000057:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*060000B6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22d1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method JoinBlock`3::.ctor

  .method /*060000B7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Threading.Tasks.Dataflow.GroupingDataflowBlockOptions/*02000012*/ dataflowBlockOptions) cil managed
  // SIG: 20 01 01 12 48
  {
    // Method begins at RVA 0x22d9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method JoinBlock`3::.ctor

  .method /*060000B8*/ public hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          get_Completion() cil managed
  // SIG: 20 00 12 69
  {
    // Method begins at RVA 0x22e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`3::get_Completion

  .method /*060000B9*/ public hidebysig specialname 
          instance int32  get_OutputCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x22e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`3::get_OutputCount

  .method /*060000BA*/ public hidebysig specialname 
          instance class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T1> 
          get_Target1() cil managed
  // SIG: 20 00 15 12 5C 01 13 00
  {
    // Method begins at RVA 0x22e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`3::get_Target1

  .method /*060000BB*/ public hidebysig specialname 
          instance class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T2> 
          get_Target2() cil managed
  // SIG: 20 00 15 12 5C 01 13 01
  {
    // Method begins at RVA 0x22ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`3::get_Target2

  .method /*060000BC*/ public hidebysig specialname 
          instance class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T3> 
          get_Target3() cil managed
  // SIG: 20 00 15 12 5C 01 13 02
  {
    // Method begins at RVA 0x22ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`3::get_Target3

  .method /*060000BD*/ public hidebysig newslot virtual final 
          instance void  Complete() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22f0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method JoinBlock`3::Complete

  .method /*060000BE*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/ 
          LinkTo(class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<!T1,!T2,!T3>> target,
                 class System.Threading.Tasks.Dataflow.DataflowLinkOptions/*0200000E*/ linkOptions) cil managed
  // SIG: 20 02 12 6D 15 12 5C 01 15 12 80 81 03 13 00 13 01 13 02 12 38
  {
    // Method begins at RVA 0x22f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`3::LinkTo

  .method /*060000BF*/ private hidebysig newslot virtual final 
          instance void  System.Threading.Tasks.Dataflow.IDataflowBlock.Fault(class [System.Runtime/*23000001*/]System.Exception/*01000017*/ exception) cil managed
  // SIG: 20 01 01 12 5D
  {
    .override System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/::Fault /*02000013::0600009F*/ 
    // Method begins at RVA 0x22f5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method JoinBlock`3::System.Threading.Tasks.Dataflow.IDataflowBlock.Fault

  .method /*060000C0*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<!T1,!T2,!T3> 
          'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<T1,T2,T3>>.ConsumeMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                                class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<!T1,!T2,!T3>> target,
                                                                                                [out] bool& messageConsumed) cil managed
  // SIG: 20 03 15 12 80 81 03 13 00 13 01 13 02 11 3C 15 12 5C 01 15 12 80 81 03 13 00 13 01 13 02 10 02
  {
    .override  method instance !0 class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<!T1,!T2,!T3>>/*1B000010*/::ConsumeMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                                                                                          class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>,
                                                                                                                                                                                                          bool&) /* 0A000027 */
    // Method begins at RVA 0x22f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`3::'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<T1,T2,T3>>.ConsumeMessage'

  .method /*060000C1*/ private hidebysig newslot virtual final 
          instance void  'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<T1,T2,T3>>.ReleaseReservation'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                                                   class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<!T1,!T2,!T3>> target) cil managed
  // SIG: 20 02 01 11 3C 15 12 5C 01 15 12 80 81 03 13 00 13 01 13 02
  {
    .override  method instance void class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<!T1,!T2,!T3>>/*1B000010*/::ReleaseReservation(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                                                                                                class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>) /* 0A000028 */
    // Method begins at RVA 0x22fa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method JoinBlock`3::'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<T1,T2,T3>>.ReleaseReservation'

  .method /*060000C2*/ private hidebysig newslot virtual final 
          instance bool  'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<T1,T2,T3>>.ReserveMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                                               class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<!T1,!T2,!T3>> target) cil managed
  // SIG: 20 02 02 11 3C 15 12 5C 01 15 12 80 81 03 13 00 13 01 13 02
  {
    .override  method instance bool class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<!T1,!T2,!T3>>/*1B000010*/::ReserveMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                                                                                            class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>) /* 0A000029 */
    // Method begins at RVA 0x22fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`3::'System.Threading.Tasks.Dataflow.ISourceBlock<System.Tuple<T1,T2,T3>>.ReserveMessage'

  .method /*060000C3*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`3::ToString

  .method /*060000C4*/ public hidebysig newslot virtual final 
          instance bool  TryReceive(class [System.Runtime/*23000001*/]System.Predicate`1/*0100001C*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<!T1,!T2,!T3>> 'filter',
                                    [out] class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<!T1,!T2,!T3>& item) cil managed
  // SIG: 20 02 02 15 12 71 01 15 12 80 81 03 13 00 13 01 13 02 10 15 12 80 81 03 13 00 13 01 13 02
  {
    .param [1]/*080000E7*/ 
    .custom /*0C0000E4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 05 00 00 00 02 01 01 01 01 00 00 ) 
    .param [2]/*080000E8*/ 
    .custom /*0C0000E5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 01 01 01 00 00 ) 
    .custom /*0C0000E6:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001D*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2302
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`3::TryReceive

  .method /*060000C5*/ public hidebysig newslot virtual final 
          instance bool  TryReceiveAll([out] class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<class [System.Runtime/*23000001*/]System.Tuple`3/*01000020*/<!T1,!T2,!T3>>& items) cil managed
  // SIG: 20 01 02 10 15 12 79 01 15 12 80 81 03 13 00 13 01 13 02
  {
    .param [1]/*080000E9*/ 
    .custom /*0C0000E7:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 05 00 00 00 02 01 01 01 01 00 00 ) 
    .custom /*0C0000E8:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001D*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2305
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JoinBlock`3::TryReceiveAll

  .property /*17000028*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/
          Completion()
  {
    .get instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ System.Threading.Tasks.Dataflow.JoinBlock`3/*02000019*/::get_Completion() /* 060000B8 */
  } // end of property JoinBlock`3::Completion
  .property /*17000029*/ instance callconv(8) int32
          OutputCount()
  {
    .get instance int32 System.Threading.Tasks.Dataflow.JoinBlock`3/*02000019*/::get_OutputCount() /* 060000B9 */
  } // end of property JoinBlock`3::OutputCount
  .property /*1700002A*/ instance callconv(8) class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T1>
          Target1()
  {
    .get instance class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T1> System.Threading.Tasks.Dataflow.JoinBlock`3/*02000019*/::get_Target1() /* 060000BA */
  } // end of property JoinBlock`3::Target1
  .property /*1700002B*/ instance callconv(8) class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T2>
          Target2()
  {
    .get instance class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T2> System.Threading.Tasks.Dataflow.JoinBlock`3/*02000019*/::get_Target2() /* 060000BB */
  } // end of property JoinBlock`3::Target2
  .property /*1700002C*/ instance callconv(8) class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T3>
          Target3()
  {
    .get instance class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T3> System.Threading.Tasks.Dataflow.JoinBlock`3/*02000019*/::get_Target3() /* 060000BC */
  } // end of property JoinBlock`3::Target3
} // end of class System.Threading.Tasks.Dataflow.JoinBlock`3

.class /*0200001A*/ public auto ansi sealed beforefieldinit System.Threading.Tasks.Dataflow.TransformBlock`2<TInput,TOutput>
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/,
                  class System.Threading.Tasks.Dataflow.IPropagatorBlock`2/*02000014*/<!TInput,!TOutput>/*1B000012*/,
                  class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!TOutput>/*1B00000D*/,
                  class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TInput>/*1B000001*/,
                  class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<!TOutput>/*1B000013*/
{
  .custom /*0C000069:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00006A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/
  .custom /*0C00008A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.IPropagatorBlock`2/*02000014*/<!TInput,!TOutput>/*1B000012*/
  .custom /*0C00008C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!TOutput>/*1B00000D*/
  .custom /*0C00008E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TInput>/*1B000001*/
  .custom /*0C000090:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<!TOutput>/*1B000013*/
  .custom /*0C000092:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type TInput /*2A000014*/ 
    .custom /*0C00005A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .param type TOutput /*2A000015*/ 
    .custom /*0C00005C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*060000C6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<!TInput,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<!TOutput>> transform) cil managed
  // SIG: 20 01 01 15 12 65 02 13 00 15 12 80 91 01 13 01
  {
    // Method begins at RVA 0x2308
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransformBlock`2::.ctor

  .method /*060000C7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<!TInput,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<!TOutput>> transform,
                               class System.Threading.Tasks.Dataflow.ExecutionDataflowBlockOptions/*02000011*/ dataflowBlockOptions) cil managed
  // SIG: 20 02 01 15 12 65 02 13 00 15 12 80 91 01 13 01 12 44
  {
    // Method begins at RVA 0x2310
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransformBlock`2::.ctor

  .method /*060000C8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<!TInput,!TOutput> transform) cil managed
  // SIG: 20 01 01 15 12 65 02 13 00 13 01
  {
    // Method begins at RVA 0x2318
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransformBlock`2::.ctor

  .method /*060000C9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<!TInput,!TOutput> transform,
                               class System.Threading.Tasks.Dataflow.ExecutionDataflowBlockOptions/*02000011*/ dataflowBlockOptions) cil managed
  // SIG: 20 02 01 15 12 65 02 13 00 13 01 12 44
  {
    // Method begins at RVA 0x2320
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransformBlock`2::.ctor

  .method /*060000CA*/ public hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          get_Completion() cil managed
  // SIG: 20 00 12 69
  {
    // Method begins at RVA 0x2328
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransformBlock`2::get_Completion

  .method /*060000CB*/ public hidebysig specialname 
          instance int32  get_InputCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x232b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransformBlock`2::get_InputCount

  .method /*060000CC*/ public hidebysig specialname 
          instance int32  get_OutputCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x232e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransformBlock`2::get_OutputCount

  .method /*060000CD*/ public hidebysig newslot virtual final 
          instance void  Complete() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2331
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TransformBlock`2::Complete

  .method /*060000CE*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/ 
          LinkTo(class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TOutput> target,
                 class System.Threading.Tasks.Dataflow.DataflowLinkOptions/*0200000E*/ linkOptions) cil managed
  // SIG: 20 02 12 6D 15 12 5C 01 13 01 12 38
  {
    // Method begins at RVA 0x2333
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransformBlock`2::LinkTo

  .method /*060000CF*/ private hidebysig newslot virtual final 
          instance void  System.Threading.Tasks.Dataflow.IDataflowBlock.Fault(class [System.Runtime/*23000001*/]System.Exception/*01000017*/ exception) cil managed
  // SIG: 20 01 01 12 5D
  {
    .override System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/::Fault /*02000013::0600009F*/ 
    // Method begins at RVA 0x2336
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TransformBlock`2::System.Threading.Tasks.Dataflow.IDataflowBlock.Fault

  .method /*060000D0*/ private hidebysig newslot virtual final 
          instance !TOutput  'System.Threading.Tasks.Dataflow.ISourceBlock<TOutput>.ConsumeMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                                    class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TOutput> target,
                                                                                                    [out] bool& messageConsumed) cil managed
  // SIG: 20 03 13 01 11 3C 15 12 5C 01 13 01 10 02
  {
    .override  method instance !0 class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!TOutput>/*1B00000D*/::ConsumeMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                         class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>,
                                                                                                                                         bool&) /* 0A00002A */
    // Method begins at RVA 0x2338
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransformBlock`2::'System.Threading.Tasks.Dataflow.ISourceBlock<TOutput>.ConsumeMessage'

  .method /*060000D1*/ private hidebysig newslot virtual final 
          instance void  'System.Threading.Tasks.Dataflow.ISourceBlock<TOutput>.ReleaseReservation'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                                    class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TOutput> target) cil managed
  // SIG: 20 02 01 11 3C 15 12 5C 01 13 01
  {
    .override  method instance void class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!TOutput>/*1B00000D*/::ReleaseReservation(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                               class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>) /* 0A00002B */
    // Method begins at RVA 0x233b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TransformBlock`2::'System.Threading.Tasks.Dataflow.ISourceBlock<TOutput>.ReleaseReservation'

  .method /*060000D2*/ private hidebysig newslot virtual final 
          instance bool  'System.Threading.Tasks.Dataflow.ISourceBlock<TOutput>.ReserveMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                                class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TOutput> target) cil managed
  // SIG: 20 02 02 11 3C 15 12 5C 01 13 01
  {
    .override  method instance bool class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!TOutput>/*1B00000D*/::ReserveMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                           class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>) /* 0A00002C */
    // Method begins at RVA 0x233d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransformBlock`2::'System.Threading.Tasks.Dataflow.ISourceBlock<TOutput>.ReserveMessage'

  .method /*060000D3*/ private hidebysig newslot virtual final 
          instance valuetype System.Threading.Tasks.Dataflow.DataflowMessageStatus/*02000010*/ 
          'System.Threading.Tasks.Dataflow.ITargetBlock<TInput>.OfferMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                              !TInput messageValue,
                                                                              class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!TInput> source,
                                                                              bool consumeToAccept) cil managed
  // SIG: 20 04 11 40 11 3C 13 00 15 12 58 01 13 00 02
  {
    .override  method instance valuetype System.Threading.Tasks.Dataflow.DataflowMessageStatus/*02000010*/ class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TInput>/*1B000001*/::OfferMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                                                                                               !0,
                                                                                                                                                                                                               class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!0>,
                                                                                                                                                                                                               bool) /* 0A000013 */
    // Method begins at RVA 0x2340
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransformBlock`2::'System.Threading.Tasks.Dataflow.ITargetBlock<TInput>.OfferMessage'

  .method /*060000D4*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2343
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransformBlock`2::ToString

  .method /*060000D5*/ public hidebysig newslot virtual final 
          instance bool  TryReceive(class [System.Runtime/*23000001*/]System.Predicate`1/*0100001C*/<!TOutput> 'filter',
                                    [out] !TOutput& item) cil managed
  // SIG: 20 02 02 15 12 71 01 13 01 10 13 01
  {
    .param [1]/*080000FE*/ 
    .custom /*0C0000E9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [2]/*080000FF*/ 
    .custom /*0C0000EA:0A000022*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.MaybeNullWhenAttribute/*01000021*/::.ctor(bool) /* 0A000022 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2346
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransformBlock`2::TryReceive

  .method /*060000D6*/ public hidebysig newslot virtual final 
          instance bool  TryReceiveAll([out] class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!TOutput>& items) cil managed
  // SIG: 20 01 02 10 15 12 79 01 13 01
  {
    .param [1]/*08000100*/ 
    .custom /*0C0000EB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .custom /*0C0000EC:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001D*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2349
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransformBlock`2::TryReceiveAll

  .property /*1700002D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/
          Completion()
  {
    .get instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ System.Threading.Tasks.Dataflow.TransformBlock`2/*0200001A*/::get_Completion() /* 060000CA */
  } // end of property TransformBlock`2::Completion
  .property /*1700002E*/ instance callconv(8) int32
          InputCount()
  {
    .get instance int32 System.Threading.Tasks.Dataflow.TransformBlock`2/*0200001A*/::get_InputCount() /* 060000CB */
  } // end of property TransformBlock`2::InputCount
  .property /*1700002F*/ instance callconv(8) int32
          OutputCount()
  {
    .get instance int32 System.Threading.Tasks.Dataflow.TransformBlock`2/*0200001A*/::get_OutputCount() /* 060000CC */
  } // end of property TransformBlock`2::OutputCount
} // end of class System.Threading.Tasks.Dataflow.TransformBlock`2

.class /*0200001B*/ public auto ansi sealed beforefieldinit System.Threading.Tasks.Dataflow.TransformManyBlock`2<TInput,TOutput>
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/,
                  class System.Threading.Tasks.Dataflow.IPropagatorBlock`2/*02000014*/<!TInput,!TOutput>/*1B000012*/,
                  class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!TOutput>/*1B00000D*/,
                  class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TInput>/*1B000001*/,
                  class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<!TOutput>/*1B000013*/
{
  .custom /*0C00006D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00006E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/
  .custom /*0C000094:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.IPropagatorBlock`2/*02000014*/<!TInput,!TOutput>/*1B000012*/
  .custom /*0C000096:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!TOutput>/*1B00000D*/
  .custom /*0C000098:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TInput>/*1B000001*/
  .custom /*0C00009B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<!TOutput>/*1B000013*/
  .custom /*0C00009F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type TInput /*2A000016*/ 
    .custom /*0C00005F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .param type TOutput /*2A000017*/ 
    .custom /*0C000061:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*060000D7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<!TInput,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100002D*/<!TOutput>> transform) cil managed
  // SIG: 20 01 01 15 12 65 02 13 00 15 12 80 B5 01 13 01
  {
    // Method begins at RVA 0x234c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransformManyBlock`2::.ctor

  .method /*060000D8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<!TInput,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100002D*/<!TOutput>> transform,
                               class System.Threading.Tasks.Dataflow.ExecutionDataflowBlockOptions/*02000011*/ dataflowBlockOptions) cil managed
  // SIG: 20 02 01 15 12 65 02 13 00 15 12 80 B5 01 13 01 12 44
  {
    // Method begins at RVA 0x2354
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransformManyBlock`2::.ctor

  .method /*060000D9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<!TInput,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100002D*/<!TOutput>>> transform) cil managed
  // SIG: 20 01 01 15 12 65 02 13 00 15 12 80 91 01 15 12 80 B5 01 13 01
  {
    // Method begins at RVA 0x235c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransformManyBlock`2::.ctor

  .method /*060000DA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<!TInput,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100002D*/<!TOutput>>> transform,
                               class System.Threading.Tasks.Dataflow.ExecutionDataflowBlockOptions/*02000011*/ dataflowBlockOptions) cil managed
  // SIG: 20 02 01 15 12 65 02 13 00 15 12 80 91 01 15 12 80 B5 01 13 01 12 44
  {
    // Method begins at RVA 0x2364
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TransformManyBlock`2::.ctor

  .method /*060000DB*/ public hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          get_Completion() cil managed
  // SIG: 20 00 12 69
  {
    // Method begins at RVA 0x236c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransformManyBlock`2::get_Completion

  .method /*060000DC*/ public hidebysig specialname 
          instance int32  get_InputCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x236f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransformManyBlock`2::get_InputCount

  .method /*060000DD*/ public hidebysig specialname 
          instance int32  get_OutputCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2372
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransformManyBlock`2::get_OutputCount

  .method /*060000DE*/ public hidebysig newslot virtual final 
          instance void  Complete() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2375
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TransformManyBlock`2::Complete

  .method /*060000DF*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/ 
          LinkTo(class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TOutput> target,
                 class System.Threading.Tasks.Dataflow.DataflowLinkOptions/*0200000E*/ linkOptions) cil managed
  // SIG: 20 02 12 6D 15 12 5C 01 13 01 12 38
  {
    // Method begins at RVA 0x2377
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransformManyBlock`2::LinkTo

  .method /*060000E0*/ private hidebysig newslot virtual final 
          instance void  System.Threading.Tasks.Dataflow.IDataflowBlock.Fault(class [System.Runtime/*23000001*/]System.Exception/*01000017*/ exception) cil managed
  // SIG: 20 01 01 12 5D
  {
    .override System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/::Fault /*02000013::0600009F*/ 
    // Method begins at RVA 0x237a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TransformManyBlock`2::System.Threading.Tasks.Dataflow.IDataflowBlock.Fault

  .method /*060000E1*/ private hidebysig newslot virtual final 
          instance !TOutput  'System.Threading.Tasks.Dataflow.ISourceBlock<TOutput>.ConsumeMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                                    class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TOutput> target,
                                                                                                    [out] bool& messageConsumed) cil managed
  // SIG: 20 03 13 01 11 3C 15 12 5C 01 13 01 10 02
  {
    .override  method instance !0 class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!TOutput>/*1B00000D*/::ConsumeMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                         class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>,
                                                                                                                                         bool&) /* 0A00002A */
    // Method begins at RVA 0x237c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransformManyBlock`2::'System.Threading.Tasks.Dataflow.ISourceBlock<TOutput>.ConsumeMessage'

  .method /*060000E2*/ private hidebysig newslot virtual final 
          instance void  'System.Threading.Tasks.Dataflow.ISourceBlock<TOutput>.ReleaseReservation'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                                    class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TOutput> target) cil managed
  // SIG: 20 02 01 11 3C 15 12 5C 01 13 01
  {
    .override  method instance void class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!TOutput>/*1B00000D*/::ReleaseReservation(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                               class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>) /* 0A00002B */
    // Method begins at RVA 0x237f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TransformManyBlock`2::'System.Threading.Tasks.Dataflow.ISourceBlock<TOutput>.ReleaseReservation'

  .method /*060000E3*/ private hidebysig newslot virtual final 
          instance bool  'System.Threading.Tasks.Dataflow.ISourceBlock<TOutput>.ReserveMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                                class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TOutput> target) cil managed
  // SIG: 20 02 02 11 3C 15 12 5C 01 13 01
  {
    .override  method instance bool class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!TOutput>/*1B00000D*/::ReserveMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                           class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>) /* 0A00002C */
    // Method begins at RVA 0x2381
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransformManyBlock`2::'System.Threading.Tasks.Dataflow.ISourceBlock<TOutput>.ReserveMessage'

  .method /*060000E4*/ private hidebysig newslot virtual final 
          instance valuetype System.Threading.Tasks.Dataflow.DataflowMessageStatus/*02000010*/ 
          'System.Threading.Tasks.Dataflow.ITargetBlock<TInput>.OfferMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                              !TInput messageValue,
                                                                              class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!TInput> source,
                                                                              bool consumeToAccept) cil managed
  // SIG: 20 04 11 40 11 3C 13 00 15 12 58 01 13 00 02
  {
    .override  method instance valuetype System.Threading.Tasks.Dataflow.DataflowMessageStatus/*02000010*/ class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!TInput>/*1B000001*/::OfferMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                                                                                               !0,
                                                                                                                                                                                                               class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!0>,
                                                                                                                                                                                                               bool) /* 0A000013 */
    // Method begins at RVA 0x2384
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransformManyBlock`2::'System.Threading.Tasks.Dataflow.ITargetBlock<TInput>.OfferMessage'

  .method /*060000E5*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2387
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransformManyBlock`2::ToString

  .method /*060000E6*/ public hidebysig newslot virtual final 
          instance bool  TryReceive(class [System.Runtime/*23000001*/]System.Predicate`1/*0100001C*/<!TOutput> 'filter',
                                    [out] !TOutput& item) cil managed
  // SIG: 20 02 02 15 12 71 01 13 01 10 13 01
  {
    .param [1]/*08000115*/ 
    .custom /*0C0000ED:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [2]/*08000116*/ 
    .custom /*0C0000EE:0A000022*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.MaybeNullWhenAttribute/*01000021*/::.ctor(bool) /* 0A000022 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x238a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransformManyBlock`2::TryReceive

  .method /*060000E7*/ public hidebysig newslot virtual final 
          instance bool  TryReceiveAll([out] class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!TOutput>& items) cil managed
  // SIG: 20 01 02 10 15 12 79 01 13 01
  {
    .param [1]/*08000117*/ 
    .custom /*0C0000EF:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .custom /*0C0000F0:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001D*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x238d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransformManyBlock`2::TryReceiveAll

  .property /*17000030*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/
          Completion()
  {
    .get instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ System.Threading.Tasks.Dataflow.TransformManyBlock`2/*0200001B*/::get_Completion() /* 060000DB */
  } // end of property TransformManyBlock`2::Completion
  .property /*17000031*/ instance callconv(8) int32
          InputCount()
  {
    .get instance int32 System.Threading.Tasks.Dataflow.TransformManyBlock`2/*0200001B*/::get_InputCount() /* 060000DC */
  } // end of property TransformManyBlock`2::InputCount
  .property /*17000032*/ instance callconv(8) int32
          OutputCount()
  {
    .get instance int32 System.Threading.Tasks.Dataflow.TransformManyBlock`2/*0200001B*/::get_OutputCount() /* 060000DD */
  } // end of property TransformManyBlock`2::OutputCount
} // end of class System.Threading.Tasks.Dataflow.TransformManyBlock`2

.class /*0200001C*/ public auto ansi sealed beforefieldinit System.Threading.Tasks.Dataflow.WriteOnceBlock`1<T>
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/,
                  class System.Threading.Tasks.Dataflow.IPropagatorBlock`2/*02000014*/<!T,!T>/*1B00000B*/,
                  class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T>/*1B00000A*/,
                  class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T>/*1B000001*/,
                  class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<!T>/*1B000009*/
{
  .custom /*0C000071:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000072:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/
  .custom /*0C0000A3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.IPropagatorBlock`2/*02000014*/<!T,!T>/*1B00000B*/
  .custom /*0C0000A5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T>/*1B00000A*/
  .custom /*0C0000A7:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T>/*1B000001*/
  .custom /*0C0000A9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<!T>/*1B000009*/
  .custom /*0C0000AB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type T /*2A000018*/ 
    .custom /*0C000064:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*060000E8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<!T,!T> cloningFunction) cil managed
  // SIG: 20 01 01 15 12 65 02 13 00 13 00
  {
    .param [1]/*08000118*/ 
    .custom /*0C0000F1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x2390
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WriteOnceBlock`1::.ctor

  .method /*060000E9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Func`2/*01000019*/<!T,!T> cloningFunction,
                               class System.Threading.Tasks.Dataflow.DataflowBlockOptions/*0200000D*/ dataflowBlockOptions) cil managed
  // SIG: 20 02 01 15 12 65 02 13 00 13 00 12 34
  {
    .param [1]/*08000119*/ 
    .custom /*0C0000F2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x2398
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00002E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WriteOnceBlock`1::.ctor

  .method /*060000EA*/ public hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ 
          get_Completion() cil managed
  // SIG: 20 00 12 69
  {
    // Method begins at RVA 0x23a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WriteOnceBlock`1::get_Completion

  .method /*060000EB*/ public hidebysig newslot virtual final 
          instance void  Complete() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23a3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WriteOnceBlock`1::Complete

  .method /*060000EC*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/ 
          LinkTo(class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T> target,
                 class System.Threading.Tasks.Dataflow.DataflowLinkOptions/*0200000E*/ linkOptions) cil managed
  // SIG: 20 02 12 6D 15 12 5C 01 13 00 12 38
  {
    // Method begins at RVA 0x23a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WriteOnceBlock`1::LinkTo

  .method /*060000ED*/ private hidebysig newslot virtual final 
          instance void  System.Threading.Tasks.Dataflow.IDataflowBlock.Fault(class [System.Runtime/*23000001*/]System.Exception/*01000017*/ exception) cil managed
  // SIG: 20 01 01 12 5D
  {
    .override System.Threading.Tasks.Dataflow.IDataflowBlock/*02000013*/::Fault /*02000013::0600009F*/ 
    // Method begins at RVA 0x23a8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WriteOnceBlock`1::System.Threading.Tasks.Dataflow.IDataflowBlock.Fault

  .method /*060000EE*/ private hidebysig newslot virtual final 
          instance bool  'System.Threading.Tasks.Dataflow.IReceivableSourceBlock<T>.TryReceiveAll'([out] class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!T>& items) cil managed
  // SIG: 20 01 02 10 15 12 79 01 13 00
  {
    .param [1]/*0800011E*/ 
    .custom /*0C0000F3:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100001D*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    .override  method instance bool class System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1/*02000015*/<!T>/*1B000009*/::TryReceiveAll(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100001E*/<!0>&) /* 0A00001E */
    // Method begins at RVA 0x23aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WriteOnceBlock`1::'System.Threading.Tasks.Dataflow.IReceivableSourceBlock<T>.TryReceiveAll'

  .method /*060000EF*/ private hidebysig newslot virtual final 
          instance !T  'System.Threading.Tasks.Dataflow.ISourceBlock<T>.ConsumeMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                        class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T> target,
                                                                                        [out] bool& messageConsumed) cil managed
  // SIG: 20 03 13 00 11 3C 15 12 5C 01 13 00 10 02
  {
    .override  method instance !0 class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T>/*1B00000A*/::ConsumeMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                   class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>,
                                                                                                                                   bool&) /* 0A00001F */
    // Method begins at RVA 0x23ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WriteOnceBlock`1::'System.Threading.Tasks.Dataflow.ISourceBlock<T>.ConsumeMessage'

  .method /*060000F0*/ private hidebysig newslot virtual final 
          instance void  'System.Threading.Tasks.Dataflow.ISourceBlock<T>.ReleaseReservation'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                              class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T> target) cil managed
  // SIG: 20 02 01 11 3C 15 12 5C 01 13 00
  {
    .override  method instance void class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T>/*1B00000A*/::ReleaseReservation(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                         class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>) /* 0A000020 */
    // Method begins at RVA 0x23b0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WriteOnceBlock`1::'System.Threading.Tasks.Dataflow.ISourceBlock<T>.ReleaseReservation'

  .method /*060000F1*/ private hidebysig newslot virtual final 
          instance bool  'System.Threading.Tasks.Dataflow.ISourceBlock<T>.ReserveMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                                          class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T> target) cil managed
  // SIG: 20 02 02 11 3C 15 12 5C 01 13 00
  {
    .override  method instance bool class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T>/*1B00000A*/::ReserveMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                     class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!0>) /* 0A000021 */
    // Method begins at RVA 0x23b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WriteOnceBlock`1::'System.Threading.Tasks.Dataflow.ISourceBlock<T>.ReserveMessage'

  .method /*060000F2*/ private hidebysig newslot virtual final 
          instance valuetype System.Threading.Tasks.Dataflow.DataflowMessageStatus/*02000010*/ 
          'System.Threading.Tasks.Dataflow.ITargetBlock<T>.OfferMessage'(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/ messageHeader,
                                                                         !T messageValue,
                                                                         class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!T> source,
                                                                         bool consumeToAccept) cil managed
  // SIG: 20 04 11 40 11 3C 13 00 15 12 58 01 13 00 02
  {
    .override  method instance valuetype System.Threading.Tasks.Dataflow.DataflowMessageStatus/*02000010*/ class System.Threading.Tasks.Dataflow.ITargetBlock`1/*02000017*/<!T>/*1B000001*/::OfferMessage(valuetype System.Threading.Tasks.Dataflow.DataflowMessageHeader/*0200000F*/,
                                                                                                                                                                                                          !0,
                                                                                                                                                                                                          class System.Threading.Tasks.Dataflow.ISourceBlock`1/*02000016*/<!0>,
                                                                                                                                                                                                          bool) /* 0A000013 */
    // Method begins at RVA 0x23b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WriteOnceBlock`1::'System.Threading.Tasks.Dataflow.ITargetBlock<T>.OfferMessage'

  .method /*060000F3*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WriteOnceBlock`1::ToString

  .method /*060000F4*/ public hidebysig newslot virtual final 
          instance bool  TryReceive(class [System.Runtime/*23000001*/]System.Predicate`1/*0100001C*/<!T> 'filter',
                                    [out] !T& item) cil managed
  // SIG: 20 02 02 15 12 71 01 13 00 10 13 00
  {
    .param [1]/*0800012A*/ 
    .custom /*0C0000F4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .param [2]/*0800012B*/ 
    .custom /*0C0000F5:0A000022*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.MaybeNullWhenAttribute/*01000021*/::.ctor(bool) /* 0A000022 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x23bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WriteOnceBlock`1::TryReceive

  .property /*17000033*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/
          Completion()
  {
    .get instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001A*/ System.Threading.Tasks.Dataflow.WriteOnceBlock`1/*0200001C*/::get_Completion() /* 060000EA */
  } // end of property WriteOnceBlock`1::Completion
} // end of class System.Threading.Tasks.Dataflow.WriteOnceBlock`1


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
